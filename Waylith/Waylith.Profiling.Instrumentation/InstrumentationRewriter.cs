using System;
using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using WaylithClr;

namespace Waylith.Profiling.Instrumentation;

internal static class InstrumentationRewriter
{
	private sealed class SignatureBytesComparer : IEqualityComparer<byte[]>
	{
		public bool Equals(byte[]? x, byte[]? y)
		{
			return ((ReadOnlySpan<byte>)x.AsSpan()).SequenceEqual((ReadOnlySpan<byte>)y);
		}

		public int GetHashCode(byte[] bytes)
		{
			HashCode hashCode = default(HashCode);
			hashCode.AddBytes(bytes);
			return hashCode.ToHashCode();
		}
	}

	internal readonly struct Instruction
	{
		public readonly int Offset;

		public readonly ILOpcode Opcode;

		public readonly int Size;

		public readonly int BranchTarget;

		public readonly int[]? SwitchTargets;

		public Instruction(int offset, ILOpcode opcode, int size, int branchTarget, int[]? switchTargets)
		{
			Offset = offset;
			Opcode = opcode;
			Size = size;
			BranchTarget = branchTarget;
			SwitchTargets = switchTargets;
		}
	}

	public readonly struct Analysis
	{
		internal readonly Instruction[] Instructions;

		internal readonly Type? ReturnValueType;

		internal readonly bool HasDynamicStackAllocation;

		public Analysis(Instruction[] instructions, Type? returnValueType, bool hasDynamicStackAllocation)
		{
			Instructions = instructions;
			ReturnValueType = returnValueType;
			HasDynamicStackAllocation = hasDynamicStackAllocation;
		}
	}

	public readonly struct EmittedMethod
	{
		internal readonly int BodyOffset;

		internal readonly int BodyLength;

		internal readonly int MapOffset;

		internal readonly int MapCount;

		internal readonly int LabelId;

		public EmittedMethod(int bodyOffset, int bodyLength, int mapOffset, int mapCount, int labelId)
		{
			BodyOffset = bodyOffset;
			BodyLength = bodyLength;
			MapOffset = mapOffset;
			MapCount = mapCount;
			LabelId = labelId;
		}
	}

	private enum Triviality
	{
		Trivial,
		NonTrivial,
		Recursive
	}

	private const int MaxTrivialBodyBytes = 256;

	private static readonly BlobBuilder sharedCodeBuilder = new BlobBuilder(65536);

	private static readonly BlobBuilder sharedBodyBuilder = new BlobBuilder(65536);

	private static readonly BlobBuilder sharedSignatureBuilder = new BlobBuilder(1024);

	private static readonly ControlFlowBuilder sharedFlowBuilder = new ControlFlowBuilder();

	[ThreadStatic]
	private static Stack<List<Instruction>>? instructionListPool;

	private static readonly Dictionary<int, LabelHandle> sharedLabels = new Dictionary<int, LabelHandle>();

	private static readonly Dictionary<(Module Module, Type Type), int> typeTokenCache = new Dictionary<(Module, Type), int>();

	private static readonly Dictionary<Module, Dictionary<byte[], int>> localsSignatureTokenCache = new Dictionary<Module, Dictionary<byte[], int>>();

	private static readonly SignatureBytesComparer signatureBytesComparer = new SignatureBytesComparer();

	private static byte[] emittedBodies = Array.Empty<byte>();

	private static int emittedBodyBytes;

	private static ILOffsetMapEntry[] emittedOffsetMaps = Array.Empty<ILOffsetMapEntry>();

	private static int emittedOffsetMapEntries;

	private const int MaxTrivialCalleeChain = 4;

	private static readonly ConcurrentDictionary<(Module Module, int Token), MethodBase?> resolvedCalleeCache = new ConcurrentDictionary<(Module, int), MethodBase>();

	private static readonly ConcurrentDictionary<MethodBase, bool> doesNotReturnCache = new ConcurrentDictionary<MethodBase, bool>();

	private static readonly ConcurrentDictionary<MethodBase, (MethodBody Body, byte[] Il)?> calleeBodyCache = new ConcurrentDictionary<MethodBase, (MethodBody, byte[])?>();

	private static readonly ConcurrentDictionary<(MethodBase Caller, int Token), MethodBase?> resolvedGenericCalleeCache = new ConcurrentDictionary<(MethodBase, int), MethodBase>();

	private static readonly ConcurrentDictionary<Type, Type[]> typeGenericArgumentsCache = new ConcurrentDictionary<Type, Type[]>();

	private static readonly ConcurrentDictionary<MethodInfo, Type[]> methodGenericArgumentsCache = new ConcurrentDictionary<MethodInfo, Type[]>();

	private static readonly ConcurrentDictionary<MethodBase, bool> trivialCalleeCache = new ConcurrentDictionary<MethodBase, bool>();

	[ThreadStatic]
	private static List<MethodBase>? sharedChain;

	internal static ReadOnlySpan<byte> EmittedBodies => emittedBodies.AsSpan(0, emittedBodyBytes);

	internal static ReadOnlySpan<ILOffsetMapEntry> EmittedOffsetMaps => emittedOffsetMaps.AsSpan(0, emittedOffsetMapEntries);

	private static List<Instruction> RentInstructionList()
	{
		Stack<List<Instruction>> stack = instructionListPool;
		if (stack == null || !stack.TryPop(out var result))
		{
			return new List<Instruction>(256);
		}
		return result;
	}

	private static void ReturnInstructionList(List<Instruction> list)
	{
		list.Clear();
		(instructionListPool ?? (instructionListPool = new Stack<List<Instruction>>())).Push(list);
	}

	public static bool TryAnalyze(MethodBase method, MethodBody body, byte[] ilBytes, out Analysis analysis)
	{
		analysis = default(Analysis);
		bool hasDynamicStackAllocation;
		List<Instruction> list = DecodeInstructions(ilBytes, out hasDynamicStackAllocation);
		if (list == null)
		{
			return false;
		}
		if (OpensWithManualScope(method, ilBytes, list))
		{
			ReturnInstructionList(list);
			return false;
		}
		List<MethodBase> list2 = sharedChain ?? (sharedChain = new List<MethodBase>(4));
		list2.Clear();
		list2.Add(method);
		if (ComputeTriviality(method, body, list, ilBytes, list2) == Triviality.Trivial)
		{
			ReturnInstructionList(list);
			return false;
		}
		object obj;
		if (method is MethodInfo methodInfo)
		{
			Type returnType = methodInfo.ReturnType;
			if (returnType != typeof(void))
			{
				obj = returnType;
				goto IL_008b;
			}
		}
		obj = null;
		goto IL_008b;
		IL_008b:
		Type returnValueType = (Type)obj;
		analysis = new Analysis(list.ToArray(), returnValueType, hasDynamicStackAllocation);
		ReturnInstructionList(list);
		return true;
	}

	internal static void ResetEmitOutput()
	{
		emittedBodyBytes = 0;
		emittedOffsetMapEntries = 0;
	}

	private static void Grow<T>(ref T[] buffer, int used, int extra)
	{
		if (used + extra > buffer.Length)
		{
			int num;
			for (num = ((buffer.Length == 0) ? 65536 : buffer.Length); num < used + extra; num *= 2)
			{
			}
			Array.Resize(ref buffer, num);
		}
	}

	private static void AddLabelOffset(in InstructionEncoder il, int offset)
	{
		if (!sharedLabels.ContainsKey(offset))
		{
			sharedLabels.Add(offset, il.DefineLabel());
		}
	}

	private static void CopyBlobRange(BlobBuilder builder, int start, int length, Span<byte> destination)
	{
		int num = start;
		int num2 = 0;
		foreach (Blob blob in builder.GetBlobs())
		{
			if (num2 == length)
			{
				break;
			}
			ArraySegment<byte> bytes = blob.GetBytes();
			if (num >= bytes.Count)
			{
				num -= bytes.Count;
				continue;
			}
			int num3 = bytes.Count - num;
			int num4 = length - num2;
			int num5 = ((num3 < num4) ? num3 : num4);
			bytes.AsSpan(num, num5).CopyTo(destination.Slice(num2));
			num2 += num5;
			num = 0;
		}
	}

	public static EmittedMethod Emit(MethodBase method, MethodBody body, byte[] ilBytes, in Analysis analysis, Func<MethodBase, int> labelFactory, int labelIdCtorToken, int tryEnterScopeToken, int leaveScopeToken)
	{
		BlobBuilder blobBuilder = sharedCodeBuilder;
		blobBuilder.Clear();
		ControlFlowBuilder controlFlowBuilder = sharedFlowBuilder;
		controlFlowBuilder.Clear();
		InstructionEncoder il = new InstructionEncoder(blobBuilder, controlFlowBuilder);
		Dictionary<int, LabelHandle> dictionary = sharedLabels;
		dictionary.Clear();
		Instruction[] instructions = analysis.Instructions;
		Span<Instruction> span = instructions.AsSpan();
		for (int i = 0; i < span.Length; i++)
		{
			ref Instruction reference = ref span[i];
			if (reference.BranchTarget >= 0)
			{
				AddLabelOffset(in il, reference.BranchTarget);
			}
			int[] switchTargets = reference.SwitchTargets;
			if (switchTargets != null)
			{
				int[] array = switchTargets;
				foreach (int offset in array)
				{
					AddLabelOffset(in il, offset);
				}
			}
		}
		IList<ExceptionHandlingClause> exceptionHandlingClauses = body.ExceptionHandlingClauses;
		for (int k = 0; k < exceptionHandlingClauses.Count; k++)
		{
			ExceptionHandlingClause exceptionHandlingClause = exceptionHandlingClauses[k];
			AddLabelOffset(in il, exceptionHandlingClause.TryOffset);
			AddLabelOffset(in il, exceptionHandlingClause.TryOffset + exceptionHandlingClause.TryLength);
			AddLabelOffset(in il, exceptionHandlingClause.HandlerOffset);
			AddLabelOffset(in il, exceptionHandlingClause.HandlerOffset + exceptionHandlingClause.HandlerLength);
			if (exceptionHandlingClause.Flags == ExceptionHandlingClauseOptions.Filter)
			{
				AddLabelOffset(in il, exceptionHandlingClause.FilterOffset);
			}
		}
		int activeScopeIndex;
		StandaloneSignatureHandle localVariablesSignature = BuildLocalsSignature(method, body, analysis.ReturnValueType, out activeScopeIndex);
		int num = (((object)analysis.ReturnValueType != null) ? (activeScopeIndex + 1) : (-1));
		LabelHandle labelHandle = il.DefineLabel();
		LabelHandle labelHandle2 = il.DefineLabel();
		LabelHandle label = il.DefineLabel();
		LabelHandle labelHandle3 = il.DefineLabel();
		int num2 = labelFactory(method);
		il.LoadConstantI4(num2);
		il.OpCode(ILOpCode.Newobj);
		il.Token(labelIdCtorToken);
		il.OpCode(ILOpCode.Call);
		il.Token(tryEnterScopeToken);
		il.StoreLocal(activeScopeIndex);
		il.MarkLabel(labelHandle);
		int mapOffset = emittedOffsetMapEntries;
		Grow(ref emittedOffsetMaps, emittedOffsetMapEntries, instructions.Length);
		Span<ILOffsetMapEntry> span2 = emittedOffsetMaps.AsSpan(emittedOffsetMapEntries);
		int num3 = 0;
		for (int l = 0; l < span.Length; l++)
		{
			ref Instruction reference2 = ref span[l];
			if (dictionary.TryGetValue(reference2.Offset, out var value))
			{
				il.MarkLabel(value);
			}
			if (reference2.Opcode != ILOpcode.tail)
			{
				span2[num3++] = new ILOffsetMapEntry(reference2.Offset, il.Offset);
			}
			switch (reference2.Opcode)
			{
			case ILOpcode.ret:
				if (num >= 0)
				{
					il.StoreLocal(num);
				}
				il.Branch(ILOpCode.Leave, labelHandle3);
				break;
			case ILOpcode.switch_:
			{
				SwitchInstructionEncoder switchInstructionEncoder = il.Switch(reference2.SwitchTargets.Length);
				int[] array = reference2.SwitchTargets;
				foreach (int key in array)
				{
					switchInstructionEncoder.Branch(dictionary[key]);
				}
				break;
			}
			default:
				if (reference2.Opcode.IsBranch())
				{
					il.Branch(((ILOpCode)reference2.Opcode).GetLongBranch(), dictionary[reference2.BranchTarget]);
				}
				else
				{
					blobBuilder.WriteBytes(ilBytes, reference2.Offset, reference2.Size);
				}
				break;
			case ILOpcode.tail:
				break;
			}
		}
		if (dictionary.TryGetValue(ilBytes.Length, out var value2))
		{
			il.MarkLabel(value2);
		}
		il.MarkLabel(labelHandle2);
		il.LoadLocal(activeScopeIndex);
		il.Branch(ILOpCode.Brfalse, label);
		il.OpCode(ILOpCode.Call);
		il.Token(leaveScopeToken);
		il.MarkLabel(label);
		il.OpCode(ILOpCode.Endfinally);
		il.MarkLabel(labelHandle3);
		if (num >= 0)
		{
			il.LoadLocal(num);
		}
		il.OpCode(ILOpCode.Ret);
		for (int m = 0; m < exceptionHandlingClauses.Count; m++)
		{
			ExceptionHandlingClause exceptionHandlingClause2 = exceptionHandlingClauses[m];
			LabelHandle tryStart = dictionary[exceptionHandlingClause2.TryOffset];
			LabelHandle tryEnd = dictionary[exceptionHandlingClause2.TryOffset + exceptionHandlingClause2.TryLength];
			LabelHandle handlerStart = dictionary[exceptionHandlingClause2.HandlerOffset];
			LabelHandle handlerEnd = dictionary[exceptionHandlingClause2.HandlerOffset + exceptionHandlingClause2.HandlerLength];
			switch (exceptionHandlingClause2.Flags)
			{
			case ExceptionHandlingClauseOptions.Clause:
				controlFlowBuilder.AddCatchRegion(tryStart, tryEnd, handlerStart, handlerEnd, TypeHandle(exceptionHandlingClause2.CatchType, method));
				break;
			case ExceptionHandlingClauseOptions.Filter:
				controlFlowBuilder.AddFilterRegion(tryStart, tryEnd, handlerStart, handlerEnd, dictionary[exceptionHandlingClause2.FilterOffset]);
				break;
			case ExceptionHandlingClauseOptions.Finally:
				controlFlowBuilder.AddFinallyRegion(tryStart, tryEnd, handlerStart, handlerEnd);
				break;
			case ExceptionHandlingClauseOptions.Fault:
				controlFlowBuilder.AddFaultRegion(tryStart, tryEnd, handlerStart, handlerEnd);
				break;
			}
		}
		controlFlowBuilder.AddFinallyRegion(labelHandle, labelHandle2, labelHandle2, labelHandle3);
		BlobBuilder blobBuilder2 = sharedBodyBuilder;
		blobBuilder2.Clear();
		int num4 = new MethodBodyStreamEncoder(blobBuilder2).AddMethodBody(il, System.Math.Max(body.MaxStackSize, 1), localVariablesSignature, body.InitLocals ? MethodBodyAttributes.InitLocals : MethodBodyAttributes.None, analysis.HasDynamicStackAllocation);
		emittedOffsetMapEntries += num3;
		int num5 = blobBuilder2.Count - num4;
		int bodyOffset = emittedBodyBytes;
		Grow(ref emittedBodies, emittedBodyBytes, num5);
		CopyBlobRange(blobBuilder2, num4, num5, emittedBodies.AsSpan(emittedBodyBytes, num5));
		emittedBodyBytes += num5;
		return new EmittedMethod(bodyOffset, num5, mapOffset, num3, num2);
	}

	private unsafe static StandaloneSignatureHandle BuildLocalsSignature(MethodBase method, MethodBody body, Type? returnValueType, out int activeScopeIndex)
	{
		BlobBuilder blobBuilder = sharedSignatureBuilder;
		blobBuilder.Clear();
		int num = 0;
		if (body.LocalSignatureMetadataToken != 0)
		{
			byte[] array = method.Module.ResolveSignature(body.LocalSignatureMetadataToken);
			fixed (byte* buffer = array)
			{
				BlobReader blobReader = new BlobReader(buffer, array.Length);
				blobReader.ReadSignatureHeader();
				num = blobReader.ReadCompressedInteger();
				new BlobEncoder(blobBuilder).LocalVariableSignature(num + (((object)returnValueType == null) ? 1 : 2));
				blobBuilder.WriteBytes(blobReader.CurrentPointer, blobReader.RemainingBytes);
			}
		}
		else
		{
			new BlobEncoder(blobBuilder).LocalVariableSignature(((object)returnValueType == null) ? 1 : 2);
		}
		activeScopeIndex = num;
		blobBuilder.WriteByte(2);
		if ((object)returnValueType != null)
		{
			if (returnValueType.IsByRef)
			{
				blobBuilder.WriteByte(16);
				returnValueType = returnValueType.GetElementType();
			}
			EncodeType(new SignatureTypeEncoder(blobBuilder), returnValueType, method);
		}
		if (!localsSignatureTokenCache.TryGetValue(method.Module, out Dictionary<byte[], int> value))
		{
			localsSignatureTokenCache.Add(method.Module, value = new Dictionary<byte[], int>(signatureBytesComparer));
		}
		byte[] array2 = blobBuilder.ToArray();
		if (!value.TryGetValue(array2, out var value2))
		{
			value.Add(array2, value2 = MethodBodyUpdater.GetTokenForLocals(method, array2));
		}
		return (StandaloneSignatureHandle)MetadataTokens.EntityHandle(value2);
	}

	private static void EncodeType(SignatureTypeEncoder encoder, Type type, MethodBase anchor)
	{
		if (type.IsSZArray)
		{
			EncodeType(encoder.SZArray(), type.GetElementType(), anchor);
			return;
		}
		if (type.IsArray)
		{
			encoder.Array(out var elementType, out var arrayShape);
			EncodeType(elementType, type.GetElementType(), anchor);
			arrayShape.Shape(type.GetArrayRank(), ImmutableArray<int>.Empty, ImmutableArray<int>.Empty);
			return;
		}
		if (type.IsPointer)
		{
			EncodeType(encoder.Pointer(), type.GetElementType(), anchor);
			return;
		}
		if (type.IsGenericParameter)
		{
			if ((object)type.DeclaringMethod != null)
			{
				encoder.GenericMethodTypeParameter(type.GenericParameterPosition);
			}
			else
			{
				encoder.GenericTypeParameter(type.GenericParameterPosition);
			}
			return;
		}
		if (type.IsGenericType)
		{
			Type genericTypeDefinition = type.GetGenericTypeDefinition();
			Type[] genericArguments = type.GetGenericArguments();
			GenericTypeArgumentsEncoder genericTypeArgumentsEncoder = encoder.GenericInstantiation(TypeHandle(genericTypeDefinition, anchor), genericArguments.Length, genericTypeDefinition.IsValueType);
			Type[] array = genericArguments;
			foreach (Type type2 in array)
			{
				EncodeType(genericTypeArgumentsEncoder.AddArgument(), type2, anchor);
			}
			return;
		}
		PrimitiveTypeCode? primitiveTypeCode = GetPrimitiveTypeCode(type);
		if (primitiveTypeCode.HasValue)
		{
			PrimitiveTypeCode valueOrDefault = primitiveTypeCode.GetValueOrDefault();
			encoder.PrimitiveType(valueOrDefault);
			return;
		}
		if (type.IsFunctionPointer)
		{
			encoder.PrimitiveType(PrimitiveTypeCode.IntPtr);
			return;
		}
		if (type.IsByRef)
		{
			throw new NotSupportedException($"Cannot encode {type} into a local signature.");
		}
		encoder.Type(TypeHandle(type, anchor), type.IsValueType);
	}

	private static EntityHandle TypeHandle(Type type, MethodBase anchor)
	{
		(Module, Type) key = (anchor.Module, type);
		if (!typeTokenCache.TryGetValue(key, out var value))
		{
			typeTokenCache.Add(key, value = MethodBodyUpdater.GetTokenFor(anchor, type));
		}
		return MetadataTokens.EntityHandle(value);
	}

	private static PrimitiveTypeCode? GetPrimitiveTypeCode(Type type)
	{
		if (type == typeof(string))
		{
			return PrimitiveTypeCode.String;
		}
		if (type == typeof(object))
		{
			return PrimitiveTypeCode.Object;
		}
		if (!type.IsPrimitive)
		{
			return null;
		}
		if (type == typeof(bool))
		{
			return PrimitiveTypeCode.Boolean;
		}
		if (type == typeof(char))
		{
			return PrimitiveTypeCode.Char;
		}
		if (type == typeof(sbyte))
		{
			return PrimitiveTypeCode.SByte;
		}
		if (type == typeof(byte))
		{
			return PrimitiveTypeCode.Byte;
		}
		if (type == typeof(short))
		{
			return PrimitiveTypeCode.Int16;
		}
		if (type == typeof(ushort))
		{
			return PrimitiveTypeCode.UInt16;
		}
		if (type == typeof(int))
		{
			return PrimitiveTypeCode.Int32;
		}
		if (type == typeof(uint))
		{
			return PrimitiveTypeCode.UInt32;
		}
		if (type == typeof(long))
		{
			return PrimitiveTypeCode.Int64;
		}
		if (type == typeof(ulong))
		{
			return PrimitiveTypeCode.UInt64;
		}
		if (type == typeof(float))
		{
			return PrimitiveTypeCode.Single;
		}
		if (type == typeof(double))
		{
			return PrimitiveTypeCode.Double;
		}
		if (type == typeof(nint))
		{
			return PrimitiveTypeCode.IntPtr;
		}
		if (type == typeof(nuint))
		{
			return PrimitiveTypeCode.UIntPtr;
		}
		return null;
	}

	private static List<Instruction>? DecodeInstructions(byte[] ilBytes, out bool hasDynamicStackAllocation)
	{
		List<Instruction> list = RentInstructionList();
		hasDynamicStackAllocation = false;
		int offset;
		ILOpcode iLOpcode;
		int branchTarget;
		int[] array;
		for (ILReader iLReader = new ILReader(ilBytes); iLReader.HasNext; list.Add(new Instruction(offset, iLOpcode, iLReader.Offset - offset, branchTarget, array)))
		{
			offset = iLReader.Offset;
			iLOpcode = iLReader.ReadILOpcode();
			branchTarget = -1;
			array = null;
			if (iLOpcode.IsBranch())
			{
				branchTarget = iLReader.ReadBranchDestination(iLOpcode);
				continue;
			}
			switch (iLOpcode)
			{
			case ILOpcode.switch_:
			{
				array = new int[checked((int)iLReader.ReadILUInt32())];
				int num = iLReader.Offset + array.Length * 4;
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = num + (int)iLReader.ReadILUInt32();
				}
				continue;
			}
			case ILOpcode.jmp:
				ReturnInstructionList(list);
				return null;
			case ILOpcode.localloc:
				hasDynamicStackAllocation = true;
				break;
			}
			iLReader.Skip(iLOpcode);
		}
		return list;
	}

	internal static void ClearAnalysisCaches()
	{
		resolvedCalleeCache.Clear();
		doesNotReturnCache.Clear();
		calleeBodyCache.Clear();
		resolvedGenericCalleeCache.Clear();
		typeGenericArgumentsCache.Clear();
		methodGenericArgumentsCache.Clear();
		trivialCalleeCache.Clear();
	}

	private static (Type[]? TypeArguments, Type[]? MethodArguments) GetGenericContext(MethodBase method)
	{
		Type declaringType = method.DeclaringType;
		Type[] item = (((object)declaringType != null && declaringType.IsGenericType) ? typeGenericArgumentsCache.GetOrAdd(declaringType, (Type type) => type.GetGenericArguments()) : null);
		Type[] item2 = ((method is MethodInfo key && method.IsGenericMethod) ? methodGenericArgumentsCache.GetOrAdd(key, (MethodInfo generic) => generic.GetGenericArguments()) : null);
		return (TypeArguments: item, MethodArguments: item2);
	}

	private static Triviality ComputeTriviality(MethodBase method, MethodBody body, List<Instruction> decoded, byte[] il, List<MethodBase> chain)
	{
		if (il.Length > 256)
		{
			return Triviality.NonTrivial;
		}
		if (body.ExceptionHandlingClauses.Count > 0)
		{
			return Triviality.NonTrivial;
		}
		(Type[]? TypeArguments, Type[]? MethodArguments) genericContext = GetGenericContext(method);
		Type[] item = genericContext.TypeArguments;
		Type[] item2 = genericContext.MethodArguments;
		int num = 0;
		foreach (Instruction item3 in decoded)
		{
			Instruction callInstruction = item3;
			ILOpcode opcode = callInstruction.Opcode;
			if ((uint)(opcode - 140) <= 1u)
			{
				return Triviality.NonTrivial;
			}
			if (callInstruction.Opcode == ILOpcode.newobj)
			{
				Type type = ResolveCallee(method, il, in callInstruction, item, item2)?.DeclaringType;
				if ((object)type == null || !type.IsValueType)
				{
					return Triviality.NonTrivial;
				}
			}
			if (callInstruction.Opcode == ILOpcode.calli)
			{
				if (++num > 1)
				{
					return Triviality.NonTrivial;
				}
			}
			else
			{
				opcode = callInstruction.Opcode;
				if ((opcode == ILOpcode.call || opcode == ILOpcode.callvirt || opcode == ILOpcode.newobj) ? true : false)
				{
					switch (EvaluateCallee(method, il, in callInstruction, item, item2, chain))
					{
					case Triviality.Recursive:
						return Triviality.Recursive;
					case Triviality.NonTrivial:
						if (++num > 1)
						{
							return Triviality.NonTrivial;
						}
						break;
					}
				}
			}
			if (callInstruction.BranchTarget >= 0 && callInstruction.BranchTarget <= callInstruction.Offset)
			{
				return Triviality.NonTrivial;
			}
			int[] switchTargets = callInstruction.SwitchTargets;
			if (switchTargets == null)
			{
				continue;
			}
			int[] array = switchTargets;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] <= callInstruction.Offset)
				{
					return Triviality.NonTrivial;
				}
			}
		}
		return Triviality.Trivial;
	}

	private static bool OpensWithManualScope(MethodBase method, byte[] il, List<Instruction> decoded)
	{
		var (callerTypeArguments, callerMethodArguments) = GetGenericContext(method);
		foreach (Instruction item in decoded)
		{
			Instruction callInstruction = item;
			if (callInstruction.Opcode == ILOpcode.calli)
			{
				return false;
			}
			ILOpcode opcode = callInstruction.Opcode;
			if ((opcode != ILOpcode.call && opcode != ILOpcode.callvirt && opcode != ILOpcode.newobj) || 1 == 0)
			{
				continue;
			}
			MethodBase methodBase = ResolveCallee(method, il, in callInstruction, callerTypeArguments, callerMethodArguments);
			bool flag;
			if ((object)methodBase != null)
			{
				string name = methodBase.Name;
				if (name == "Scope" || name == "TryEnterScope")
				{
					flag = true;
					goto IL_009d;
				}
			}
			flag = false;
			goto IL_009d;
			IL_009d:
			return flag && methodBase.DeclaringType == typeof(FrameProfiler);
		}
		return false;
	}

	private static MethodBase? ResolveCallee(MethodBase caller, byte[] callerIl, in Instruction callInstruction, Type[]? callerTypeArguments, Type[]? callerMethodArguments)
	{
		int num = BinaryPrimitives.ReadInt32LittleEndian(callerIl.AsSpan(callInstruction.Offset + 1));
		if (callerTypeArguments == null && callerMethodArguments == null)
		{
			(Module, int) key = (caller.Module, num);
			if (resolvedCalleeCache.TryGetValue(key, out MethodBase value))
			{
				return value;
			}
			return resolvedCalleeCache[key] = Resolve(caller, num, null, null);
		}
		(MethodBase, int) key2 = (caller, num);
		if (resolvedGenericCalleeCache.TryGetValue(key2, out MethodBase value2))
		{
			return value2;
		}
		return resolvedGenericCalleeCache[key2] = Resolve(caller, num, callerTypeArguments, callerMethodArguments);
	}

	private static MethodBase? Resolve(MethodBase caller, int token, Type[]? typeArguments, Type[]? methodArguments)
	{
		try
		{
			return caller.Module.ResolveMethod(token, typeArguments, methodArguments);
		}
		catch
		{
			return null;
		}
	}

	private static (MethodBody Body, byte[] Il)? LoadCalleeBody(MethodBase callee)
	{
		MethodBody methodBody;
		try
		{
			methodBody = callee.GetMethodBody();
		}
		catch
		{
			return null;
		}
		byte[] array = methodBody?.GetILAsByteArray();
		if (array == null || array.Length <= 0)
		{
			return null;
		}
		return (methodBody, array);
	}

	private static Triviality EvaluateCallee(MethodBase caller, byte[] callerIl, in Instruction callInstruction, Type[]? callerTypeArguments, Type[]? callerMethodArguments, List<MethodBase> chain)
	{
		if (chain.Count >= 4)
		{
			return Triviality.NonTrivial;
		}
		MethodBase methodBase = ResolveCallee(caller, callerIl, in callInstruction, callerTypeArguments, callerMethodArguments);
		if ((object)methodBase == null)
		{
			return Triviality.NonTrivial;
		}
		if (callInstruction.Opcode == ILOpcode.callvirt && methodBase.IsVirtual && !methodBase.IsFinal)
		{
			Type declaringType = methodBase.DeclaringType;
			if ((object)declaringType == null || !declaringType.IsSealed)
			{
				return Triviality.NonTrivial;
			}
		}
		if (!doesNotReturnCache.TryGetValue(methodBase, out var value))
		{
			value = (doesNotReturnCache[methodBase] = methodBase.IsDefined(typeof(DoesNotReturnAttribute), inherit: false));
		}
		if (value)
		{
			return Triviality.Trivial;
		}
		if (chain.Contains(methodBase))
		{
			return Triviality.Recursive;
		}
		if (trivialCalleeCache.ContainsKey(methodBase))
		{
			return Triviality.Trivial;
		}
		if (!calleeBodyCache.TryGetValue(methodBase, out (MethodBody, byte[])? value2))
		{
			value2 = (calleeBodyCache[methodBase] = LoadCalleeBody(methodBase));
		}
		if (!value2.HasValue)
		{
			return Triviality.NonTrivial;
		}
		var (body, array) = value2.GetValueOrDefault();
		if (array.Length > 256)
		{
			return Triviality.NonTrivial;
		}
		bool hasDynamicStackAllocation;
		List<Instruction> list = DecodeInstructions(array, out hasDynamicStackAllocation);
		if (list == null)
		{
			return Triviality.NonTrivial;
		}
		chain.Add(methodBase);
		Triviality num = ComputeTriviality(methodBase, body, list, array, chain);
		chain.RemoveAt(chain.Count - 1);
		ReturnInstructionList(list);
		if (num == Triviality.Trivial)
		{
			trivialCalleeCache.TryAdd(methodBase, value: true);
		}
		return num;
	}
}
