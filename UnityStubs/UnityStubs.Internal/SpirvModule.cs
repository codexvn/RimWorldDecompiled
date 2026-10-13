using System;
using System.Collections.Generic;

namespace UnityStubs.Internal;

internal sealed class SpirvModule
{
	private const int OpEntryPoint = 15;

	private const int OpTypeInt = 21;

	private const int OpTypeFloat = 22;

	private const int OpTypeVector = 23;

	private const int OpTypeImage = 25;

	private const int OpTypeSampledImage = 27;

	private const int OpTypeMatrix = 24;

	private const int OpTypeArray = 28;

	private const int OpTypeRuntimeArray = 29;

	private const int OpTypeStruct = 30;

	private const int OpTypePointer = 32;

	private const int OpConstant = 43;

	private const int OpFunction = 54;

	private const int OpVariable = 59;

	private const int OpLoad = 61;

	private const int OpCopyObject = 83;

	private const int OpSampledImage = 86;

	private const int OpImageSampleImplicitLod = 87;

	private const int OpImageSampleProjImplicitLod = 91;

	private const int OpImageSampleProjDrefExplicitLod = 94;

	private const int OpImageFetch = 95;

	private const int OpImageDrefGather = 97;

	private const int OpCompositeExtract = 81;

	private const int OpCompositeInsert = 82;

	private const int OpFSub = 131;

	private const int OpFNegate = 127;

	private const int OpAccessChain = 65;

	private const int OpInBoundsAccessChain = 66;

	private const int OpDecorate = 71;

	private const int OpMemberDecorate = 72;

	private const uint DecorationBlock = 2u;

	private const uint DecorationBufferBlock = 3u;

	private const uint DecorationArrayStride = 6u;

	private const uint DecorationRowMajor = 4u;

	private const uint DecorationColMajor = 5u;

	private const uint DecorationMatrixStride = 7u;

	private const uint DecorationBuiltIn = 11u;

	private const uint DecorationLocation = 30u;

	private const uint DecorationBinding = 33u;

	private const uint DecorationDescriptorSet = 34u;

	private const uint DecorationOffset = 35u;

	private const uint StorageUniformConstant = 0u;

	private const uint StorageInput = 1u;

	private const uint StorageOutput = 3u;

	private const uint StorageUniform = 2u;

	private const uint StoragePushConstant = 9u;

	private const uint StorageStorageBuffer = 12u;

	private readonly uint[] words;

	private readonly List<int> instructions = new List<int>();

	private readonly Dictionary<uint, (uint storage, uint pointee)> pointers = new Dictionary<uint, (uint, uint)>();

	private readonly Dictionary<uint, int> typeOps = new Dictionary<uint, int>();

	private readonly Dictionary<uint, uint[]> structMembers = new Dictionary<uint, uint[]>();

	private readonly Dictionary<uint, (uint type, uint storage)> variables = new Dictionary<uint, (uint, uint)>();

	private readonly Dictionary<uint, (uint type, uint value)> intConstants = new Dictionary<uint, (uint, uint)>();

	private readonly Dictionary<uint, int> setDecorations = new Dictionary<uint, int>();

	private readonly Dictionary<uint, int> bindingDecorations = new Dictionary<uint, int>();

	private readonly Dictionary<uint, int> locationDecorations = new Dictionary<uint, int>();

	private readonly Dictionary<(uint type, uint member), int> offsetDecorations = new Dictionary<(uint, uint), int>();

	private readonly Dictionary<(uint type, uint member), int> majorDecorations = new Dictionary<(uint, uint), int>();

	private readonly Dictionary<uint, int> structInstructions = new Dictionary<uint, int>();

	private readonly Dictionary<uint, uint> createdMatrices = new Dictionary<uint, uint>();

	private readonly List<uint> newAnnotations = new List<uint>();

	private readonly List<uint> newTypes = new List<uint>();

	private readonly Dictionary<int, List<uint>> insertBefore = new Dictionary<int, List<uint>>();

	private int firstTypeInstruction = -1;

	private int firstFunctionInstruction = -1;

	private uint bound;

	private SpirvModule(uint[] words)
	{
		this.words = words;
		bound = words[3];
	}

	public static int ExecutionModelOf(byte[] spirv)
	{
		int num2;
		for (int i = 20; i + 8 <= spirv.Length; i += num2 * 4)
		{
			uint num = BitConverter.ToUInt32(spirv, i);
			num2 = (int)(num >> 16);
			if (num2 == 0)
			{
				return -1;
			}
			if ((num & 0xFFFF) == 15)
			{
				return BitConverter.ToInt32(spirv, i + 4);
			}
		}
		return -1;
	}

	private static SpirvModule Parse(byte[] spirv)
	{
		if (spirv.Length < 20 || spirv.Length % 4 != 0 || BitConverter.ToUInt32(spirv, 0) != 119734787)
		{
			return null;
		}
		uint[] array = new uint[spirv.Length / 4];
		Buffer.BlockCopy(spirv, 0, array, 0, spirv.Length);
		SpirvModule spirvModule = new SpirvModule(array);
		int num;
		for (int i = 5; i < array.Length; i += num)
		{
			num = (int)(array[i] >> 16);
			int num2 = (int)(array[i] & 0xFFFF);
			if (num == 0 || i + num > array.Length)
			{
				return null;
			}
			int count = spirvModule.instructions.Count;
			spirvModule.instructions.Add(i);
			if (spirvModule.firstTypeInstruction < 0 && num2 >= 19 && num2 <= 52)
			{
				spirvModule.firstTypeInstruction = count;
			}
			if (spirvModule.firstFunctionInstruction < 0 && num2 == 54)
			{
				spirvModule.firstFunctionInstruction = count;
			}
			switch (num2)
			{
			case 32:
				spirvModule.pointers[array[i + 1]] = (array[i + 2], array[i + 3]);
				break;
			case 30:
				spirvModule.structMembers[array[i + 1]] = array.AsSpan(i + 2, num - 2).ToArray();
				spirvModule.structInstructions[array[i + 1]] = i;
				break;
			case 43:
				if (num == 4)
				{
					spirvModule.intConstants[array[i + 2]] = (array[i + 1], array[i + 3]);
				}
				break;
			case 59:
				spirvModule.variables[array[i + 2]] = (array[i + 1], array[i + 3]);
				break;
			case 71:
				if (num == 4)
				{
					Dictionary<uint, int> dictionary = array[i + 2] switch
					{
						34u => spirvModule.setDecorations, 
						33u => spirvModule.bindingDecorations, 
						30u => spirvModule.locationDecorations, 
						_ => null, 
					};
					if (dictionary != null)
					{
						dictionary[array[i + 1]] = i + 3;
					}
				}
				break;
			case 72:
			{
				if (num == 5 && array[i + 3] == 35)
				{
					spirvModule.offsetDecorations[(array[i + 1], array[i + 2])] = i + 4;
					break;
				}
				bool flag = num == 4;
				if (flag)
				{
					uint num3 = array[i + 3];
					bool flag2 = num3 - 4 <= 1;
					flag = flag2;
				}
				if (flag)
				{
					spirvModule.majorDecorations[(array[i + 1], array[i + 2])] = i + 3;
				}
				break;
			}
			}
			if (num2 >= 19 && num2 <= 39)
			{
				spirvModule.typeOps[array[i + 1]] = num2;
			}
		}
		HashSet<uint> hashSet = new HashSet<uint>();
		foreach (int instruction in spirvModule.instructions)
		{
			if ((array[instruction] & 0xFFFF) == 21 && array[instruction + 2] == 32)
			{
				hashSet.Add(array[instruction + 1]);
			}
		}
		foreach (uint item in new List<uint>(spirvModule.intConstants.Keys))
		{
			if (!hashSet.Contains(spirvModule.intConstants[item].type))
			{
				spirvModule.intConstants.Remove(item);
			}
		}
		if (spirvModule.firstTypeInstruction >= 0 && spirvModule.firstFunctionInstruction >= 0)
		{
			return spirvModule;
		}
		return null;
	}

	public static byte[] Rewrite(byte[] spirv, int stage, UnityShaderPass pass, UnityShaderBindingPlan plan, out string failure)
	{
		SpirvModule spirvModule = Parse(spirv);
		if (spirvModule == null)
		{
			failure = "malformed SPIR-V";
			return null;
		}
		if ((spirvModule.words[1] & 0xFFFFFF) >= 66560)
		{
			failure = "SPIR-V 1.4+ interface rules are not handled";
			return null;
		}
		if (!spirvModule.Rewrite(stage, pass, plan, out failure))
		{
			return null;
		}
		return spirvModule.Emit();
	}

	private bool Rewrite(int stage, UnityShaderPass pass, UnityShaderBindingPlan plan, out string failure)
	{
		failure = null;
		uint key;
		foreach (KeyValuePair<uint, (uint, uint)> variable in variables)
		{
			variable.Deconstruct(out key, out var value);
			if (value.Item2 == 9)
			{
				failure = "already uses push constants";
				return false;
			}
		}
		foreach (KeyValuePair<uint, int> item2 in new Dictionary<uint, int>(setDecorations))
		{
			item2.Deconstruct(out key, out var value2);
			uint num = key;
			int num2 = value2;
			if (!variables.TryGetValue(num, out (uint, uint) value3) || !bindingDecorations.TryGetValue(num, out var value4))
			{
				continue;
			}
			int num3 = (int)((words[num2] << 16) | words[value4]);
			if (value3.Item2 == 0)
			{
				if (!plan.ImageBinding.TryGetValue(num3, out var value5) && !plan.SamplerBinding.TryGetValue(num3, out value5))
				{
					failure = $"resource at set {words[num2]} binding {words[value4]} is not in the parameter table";
					return false;
				}
				words[num2] = 2u;
				words[value4] = (uint)value5;
				continue;
			}
			UnityShaderBinding source = null;
			bool flag;
			foreach (UnityShaderBinding binding in pass.Bindings)
			{
				UnityShaderBindingKind kind = binding.Kind;
				flag = (uint)(kind - 1) <= 1u;
				if (flag && (binding.PackedIndex & 0xFFFFFF) == num3 && (binding.Stages & stage) != 0)
				{
					source = binding;
				}
			}
			UnityShaderBinding unityShaderBinding = source;
			flag = unityShaderBinding != null && unityShaderBinding.Kind == UnityShaderBindingKind.Buffer;
			if (flag)
			{
				key = value3.Item2;
				bool flag2 = ((key == 2 || key == 12) ? true : false);
				flag = flag2;
			}
			if (flag && plan.StorageBinding.TryGetValue(source.Name, out var value6))
			{
				words[num2] = 2u;
				words[value4] = (uint)value6;
				continue;
			}
			UnityShaderCBuffer unityShaderCBuffer = ((source == null) ? null : pass.CBuffers.Find((UnityShaderCBuffer c) => c.Name == source.Name));
			if (value3.Item2 == 2 && unityShaderCBuffer != null && plan.MovedBlock.TryGetValue(unityShaderCBuffer.Name, out var value7))
			{
				uint movedType = pointers[value3.Item1].pointee;
				if (!RedirectBuiltins(num, movedType, unityShaderCBuffer, stage, ref failure))
				{
					return false;
				}
				RowMajorMatrices(num, movedType, unityShaderCBuffer);
				int num4 = instructions.Find((int at) => (words[at] & 0xFFFF) == 71 && words[at + 1] == movedType && words[at + 2] == 2);
				if (num4 == 0)
				{
					failure = unityShaderCBuffer.Name + " is not decorated as a block";
					return false;
				}
				words[num4 + 2] = 3u;
				words[num2] = 2u;
				words[value4] = (uint)value7;
				continue;
			}
			if (value3.Item2 != 2 || unityShaderCBuffer == null || !plan.CBufferBase.TryGetValue(unityShaderCBuffer.Name, out var value8))
			{
				failure = $"buffer at set {words[num2]} binding {words[value4]} is not a known constant buffer";
				return false;
			}
			uint item = pointers[value3.Item1].pointee;
			if (!RedirectBuiltins(num, item, unityShaderCBuffer, stage, ref failure))
			{
				return false;
			}
			RowMajorMatrices(num, item, unityShaderCBuffer);
			if (!LiftArrays(num, item, unityShaderCBuffer, plan, ref failure))
			{
				return false;
			}
			words[num2] = 2u;
			words[value4] = 0u;
			for (uint num5 = 0u; num5 < structMembers[item].Length; num5++)
			{
				if (offsetDecorations.TryGetValue((item, num5), out var value9))
				{
					words[value9] += (uint)value8;
				}
			}
		}
		if (stage == 4)
		{
			KeepClipSpaceY();
		}
		FlipSampledV();
		return true;
	}

	private void FlipSampledV()
	{
		Dictionary<uint, uint> dictionary = new Dictionary<uint, uint>();
		Dictionary<uint, (bool, bool)> dictionary2 = new Dictionary<uint, (bool, bool)>();
		Dictionary<uint, uint> dictionary3 = new Dictionary<uint, uint>();
		uint num = 0u;
		uint num2 = 0u;
		Dictionary<uint, uint> dictionary4 = new Dictionary<uint, uint>();
		foreach (int instruction in instructions)
		{
			switch ((int)(words[instruction] & 0xFFFF))
			{
			case 22:
				if (words[instruction + 2] == 32)
				{
					num = words[instruction + 1];
				}
				break;
			case 23:
				if (words[instruction + 2] == num)
				{
					dictionary4[words[instruction + 3]] = words[instruction + 1];
				}
				break;
			case 25:
				dictionary2[words[instruction + 1]] = (words[instruction + 3] == 1, words[instruction + 5] == 1);
				break;
			case 27:
				dictionary3[words[instruction + 1]] = words[instruction + 2];
				break;
			case 43:
				if (words[instruction] >> 16 == 4 && words[instruction + 1] == num && words[instruction + 3] == 1065353216)
				{
					num2 = words[instruction + 2];
				}
				break;
			case 61:
			case 86:
				dictionary[words[instruction + 2]] = words[instruction + 1];
				break;
			}
		}
		for (int i = 0; i < instructions.Count; i++)
		{
			int num3 = instructions[i];
			int num4 = (int)(words[num3] & 0xFFFF);
			bool flag = ((num4 < 87 || num4 > 97) ? true : false);
			if (flag || num4 == 95 || !dictionary.TryGetValue(words[num3 + 3], out var value) || !dictionary3.TryGetValue(value, out var value2) || !dictionary2.TryGetValue(value2, out var value3) || !value3.Item1)
			{
				continue;
			}
			bool flag2 = num4 >= 91 && num4 <= 94;
			uint key = ((flag2 || value3.Item2) ? 3u : 2u);
			if (dictionary4.TryGetValue(key, out var value4))
			{
				if (num2 == 0)
				{
					num2 = bound++;
					newTypes.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[4] { 262187u, num, num2, 1065353216u }));
				}
				uint num5 = words[num3 + 4];
				uint num6 = bound++;
				uint num7 = bound++;
				uint num8 = bound++;
				List<uint> list = new List<uint> { 327761u, num, num6, num5, 1u };
				uint num9 = num2;
				if (flag2)
				{
					num9 = bound++;
					list.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[5] { 327761u, num, num9, num5, 2u }));
				}
				list.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[5] { 327811u, num, num7, num9, num6 }));
				list.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[6] { 393298u, value4, num8, num7, num5, 1u }));
				insertBefore[i] = list;
				words[num3 + 4] = num8;
			}
		}
	}

	private void KeepClipSpaceY()
	{
		uint num = 0u;
		uint value = 0u;
		uint num2 = 0u;
		foreach (int instruction in instructions)
		{
			int num3 = (int)(words[instruction] & 0xFFFF);
			if (num3 == 72 && words[instruction] >> 16 == 5 && words[instruction + 3] == 11 && words[instruction + 4] == 0)
			{
				uint num4 = words[instruction + 1];
				value = words[instruction + 2];
				num = num4;
			}
			else if (num3 == 71 && words[instruction] >> 16 == 4 && words[instruction + 2] == 11 && words[instruction + 3] == 0)
			{
				num2 = words[instruction + 1];
			}
		}
		HashSet<uint> hashSet = new HashSet<uint>();
		foreach (int instruction2 in instructions)
		{
			int num5 = (int)(words[instruction2] & 0xFFFF);
			if ((num5 == 65 || num5 == 66) && variables.TryGetValue(words[instruction2 + 3], out (uint, uint) value2) && value2.Item2 == 3)
			{
				int num6 = (int)(words[instruction2] >> 16);
				bool num7 = num != 0 && pointers[value2.Item1].pointee == num && num6 == 6 && IsConstant(words[instruction2 + 4], value) && IsConstant(words[instruction2 + 5], 1u);
				bool flag = words[instruction2 + 3] == num2 && num6 == 5 && IsConstant(words[instruction2 + 4], 1u);
				if (num7 || flag)
				{
					hashSet.Add(words[instruction2 + 2]);
				}
			}
		}
		HashSet<uint> hashSet2 = new HashSet<uint>();
		foreach (int instruction3 in instructions)
		{
			if ((words[instruction3] & 0xFFFF) == 61 && hashSet.Contains(words[instruction3 + 3]))
			{
				hashSet2.Add(words[instruction3 + 2]);
			}
		}
		foreach (int instruction4 in instructions)
		{
			if ((words[instruction4] & 0xFFFF) == 127 && hashSet2.Contains(words[instruction4 + 3]))
			{
				words[instruction4] = (words[instruction4] & 0xFFFF0000u) | 0x53;
			}
		}
	}

	private bool IsConstant(uint id, uint value)
	{
		if (intConstants.TryGetValue(id, out (uint, uint) value2))
		{
			return value2.Item2 == value;
		}
		return false;
	}

	private bool RedirectBuiltins(uint variable, uint structType, UnityShaderCBuffer buffer, int stage, ref string failure)
	{
		foreach (UnityShaderParam param in buffer.Params)
		{
			if (!param.IsMatrix || param.Rows != 4 || param.Columns != 4 || param.ArraySize != 0)
			{
				continue;
			}
			uint storage;
			int offset;
			switch (param.Name)
			{
			case "unity_ObjectToWorld":
				if (stage != 4)
				{
					continue;
				}
				storage = 9u;
				offset = 0;
				break;
			case "unity_MatrixVP":
				storage = 2u;
				offset = 80;
				break;
			case "unity_MatrixV":
				storage = 2u;
				offset = 16;
				break;
			default:
				continue;
			}
			int num = MemberAt(structType, param.Offset);
			if (num >= 0 && !Redirect(variable, structMembers[structType][num], (uint)num, storage, (uint)offset))
			{
				failure = param.Name + " is accessed in a way that can't be redirected";
				return false;
			}
		}
		return true;
	}

	private void RowMajorMatrices(uint variable, uint structType, UnityShaderCBuffer buffer)
	{
		foreach (UnityShaderParam param in buffer.Params)
		{
			if (!param.IsMatrix || param.Rows != 4 || param.Columns != 4 || param.ArraySize != 0)
			{
				continue;
			}
			int num = MemberAt(structType, param.Offset);
			if (num < 0)
			{
				continue;
			}
			if (majorDecorations.TryGetValue((structType, (uint)num), out var value))
			{
				words[value] = ((words[value] == 4) ? 5u : 4u);
				continue;
			}
			uint num2 = structMembers[structType][num];
			if (typeOps.GetValueOrDefault(num2) == 28 && !ReadsWholeMember(variable, (uint)num) && TryMatrixFor(num2, out var matrix))
			{
				words[structInstructions[structType] + 2 + num] = matrix;
				newAnnotations.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[4]
				{
					262216u,
					structType,
					(uint)num,
					4u
				}));
				List<uint> list = newAnnotations;
				uint[] obj = new uint[5] { 327752u, 0u, 0u, 7u, 16u };
				obj[1] = structType;
				obj[2] = (uint)num;
				list.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(obj));
			}
		}
	}

	private bool LiftArrays(uint variable, uint structType, UnityShaderCBuffer buffer, UnityShaderBindingPlan plan, ref string failure)
	{
		foreach (UnityShaderParam param in buffer.Params)
		{
			if (param.ArraySize == 0 || param.IsMatrix || !plan.StorageBinding.TryGetValue(param.Name, out var value))
			{
				continue;
			}
			int num = MemberAt(structType, param.Offset);
			if (num < 0)
			{
				continue;
			}
			uint memberType = structMembers[structType][num];
			int num2 = instructions.Find((int at) => (words[at] & 0xFFFF) == 28 && words[at + 1] == memberType);
			if (num2 == 0)
			{
				failure = param.Name + " is not declared as an array";
				return false;
			}
			uint num3 = words[num2 + 2];
			uint num4;
			switch (typeOps.GetValueOrDefault(num3))
			{
			case 21:
			case 22:
				num4 = 4u;
				break;
			case 23:
				num4 = 16u;
				break;
			default:
				failure = param.Name + " is an array of something other than scalars or vectors";
				return false;
			}
			List<int> list = new List<int>();
			uint num5 = 0u;
			foreach (int instruction in instructions)
			{
				int num6 = (int)(words[instruction] & 0xFFFF);
				if ((num6 != 65 && num6 != 66) || words[instruction + 3] != variable || words[instruction] >> 16 < 5)
				{
					continue;
				}
				if (!intConstants.TryGetValue(words[instruction + 4], out (uint, uint) value2))
				{
					failure = buffer.Name + " is indexed dynamically";
					return false;
				}
				if (value2.Item2 == num)
				{
					if (words[instruction] >> 16 < 6)
					{
						failure = param.Name + " is read as a whole array";
						return false;
					}
					(num5, _) = value2;
					list.Add(instruction);
				}
			}
			if (list.Count == 0)
			{
				continue;
			}
			uint num7 = 0u;
			uint key;
			foreach (KeyValuePair<uint, (uint, uint)> intConstant in intConstants)
			{
				intConstant.Deconstruct(out key, out var value3);
				uint num8 = key;
				(uint, uint) tuple2 = value3;
				if (tuple2.Item2 == 0 && tuple2.Item1 == num5)
				{
					num7 = num8;
				}
			}
			if (num7 == 0)
			{
				num7 = bound++;
				newTypes.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[4] { 262187u, num5, num7, 0u }));
			}
			uint num9 = bound++;
			uint num10 = bound++;
			uint num11 = bound++;
			key = bound++;
			uint num12 = key;
			newAnnotations.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[4] { 262215u, num9, 6u, num4 }));
			newAnnotations.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[3] { 196679u, num10, 3u }));
			newAnnotations.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[5] { 327752u, num10, 0u, 35u, 0u }));
			List<uint> list2 = newAnnotations;
			uint[] obj = new uint[4] { 262215u, 0u, 34u, 2u };
			obj[1] = num12;
			list2.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(obj));
			newAnnotations.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[4]
			{
				262215u,
				num12,
				33u,
				(uint)value
			}));
			newTypes.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[3] { 196637u, num9, num3 }));
			newTypes.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[3] { 196638u, num10, num9 }));
			newTypes.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[4] { 262176u, num11, 2u, num10 }));
			newTypes.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[4] { 262203u, num11, num12, 2u }));
			foreach (int item in list)
			{
				words[item + 3] = num12;
				words[item + 4] = num7;
			}
		}
		return true;
	}

	private bool ReadsWholeMember(uint variable, uint member)
	{
		foreach (int instruction in instructions)
		{
			int num = (int)(words[instruction] & 0xFFFF);
			if ((num == 65 || num == 66) && words[instruction + 3] == variable && words[instruction] >> 16 == 5 && (!intConstants.TryGetValue(words[instruction + 4], out (uint, uint) value) || value.Item2 == member))
			{
				return true;
			}
		}
		return false;
	}

	private int MemberAt(uint structType, int offset)
	{
		for (uint num = 0u; num < structMembers[structType].Length; num++)
		{
			if (offsetDecorations.TryGetValue((structType, num), out var value) && words[value] == offset)
			{
				return (int)num;
			}
		}
		return -1;
	}

	private bool Redirect(uint variable, uint memberType, uint member, uint storage, uint offset)
	{
		List<int> list = new List<int>();
		HashSet<uint> hashSet = new HashSet<uint>();
		uint num = 0u;
		foreach (int instruction in instructions)
		{
			int num2 = (int)(words[instruction] & 0xFFFF);
			if ((num2 == 65 || num2 == 66) && words[instruction + 3] == variable && words[instruction] >> 16 >= 5)
			{
				if (!intConstants.TryGetValue(words[instruction + 4], out (uint, uint) value))
				{
					return false;
				}
				if (value.Item2 == member)
				{
					(num, _) = value;
					list.Add(instruction);
					hashSet.Add(words[instruction + 2]);
				}
			}
		}
		if (list.Count == 0)
		{
			return true;
		}
		foreach (int instruction2 in instructions)
		{
			int num3 = (int)(words[instruction2] & 0xFFFF);
			if ((num3 == 65 || num3 == 66) && hashSet.Contains(words[instruction2 + 3]))
			{
				return false;
			}
		}
		uint num4 = 0u;
		foreach (var (num6, tuple3) in intConstants)
		{
			if (tuple3.Item2 == 0 && tuple3.Item1 == num)
			{
				num4 = num6;
			}
		}
		if (num4 == 0)
		{
			num4 = bound++;
			newTypes.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[4] { 262187u, num, num4, 0u }));
		}
		if (!TryMatrixFor(memberType, out memberType))
		{
			return false;
		}
		uint num7 = bound++;
		uint num8 = bound++;
		uint num9 = bound++;
		newAnnotations.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[3] { 196679u, num7, 2u }));
		newAnnotations.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[5] { 327752u, num7, 0u, 35u, offset }));
		newAnnotations.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[4] { 262216u, num7, 0u, 4u }));
		List<uint> list2 = newAnnotations;
		uint[] obj = new uint[5] { 327752u, 0u, 0u, 7u, 16u };
		obj[1] = num7;
		list2.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(obj));
		if (storage == 2)
		{
			newAnnotations.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[4] { 262215u, num9, 34u, 0u }));
			List<uint> list3 = newAnnotations;
			uint[] obj2 = new uint[4] { 262215u, 0u, 33u, 1u };
			obj2[1] = num9;
			list3.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(obj2));
		}
		newTypes.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[3] { 196638u, num7, memberType }));
		newTypes.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[4] { 262176u, num8, storage, num7 }));
		newTypes.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[4] { 262203u, num8, num9, storage }));
		Dictionary<uint, uint> dictionary = new Dictionary<uint, uint>();
		foreach (int item2 in list)
		{
			words[item2 + 3] = num9;
			words[item2 + 4] = num4;
			if (storage != 2)
			{
				uint item = pointers[words[item2 + 1]].pointee;
				if (!dictionary.TryGetValue(item, out var value2))
				{
					value2 = (dictionary[item] = bound++);
					newTypes.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[4] { 262176u, value2, storage, item }));
				}
				words[item2 + 1] = value2;
			}
		}
		return true;
	}

	private bool TryMatrixFor(uint memberType, out uint matrix)
	{
		matrix = memberType;
		typeOps.TryGetValue(memberType, out var value);
		switch (value)
		{
		case 24:
			return true;
		default:
			return false;
		case 28:
		{
			uint column = 0u;
			foreach (int instruction in instructions)
			{
				if ((words[instruction] & 0xFFFF) == 28 && words[instruction + 1] == memberType)
				{
					column = ((intConstants.TryGetValue(words[instruction + 3], out (uint, uint) value2) && value2.Item2 == 4) ? words[instruction + 2] : 0u);
				}
			}
			if (column == 0)
			{
				return false;
			}
			foreach (int instruction2 in instructions)
			{
				if ((words[instruction2] & 0xFFFF) == 24 && words[instruction2 + 2] == column && words[instruction2 + 3] == 4)
				{
					matrix = words[instruction2 + 1];
					return true;
				}
			}
			if (createdMatrices.TryGetValue(column, out matrix))
			{
				return true;
			}
			int num = instructions.FindIndex((int at) => (words[at] & 0xFFFF) == 23 && words[at + 1] == column);
			if (num < 0)
			{
				return false;
			}
			matrix = bound++;
			createdMatrices[column] = matrix;
			if (!insertBefore.TryGetValue(num + 1, out var value3))
			{
				value3 = (insertBefore[num + 1] = new List<uint>());
			}
			value3.AddRange(new global::_003C_003Ez__ReadOnlyArray<uint>(new uint[4] { 262168u, matrix, column, 4u }));
			return true;
		}
		}
	}

	private byte[] Emit()
	{
		List<uint> list = new List<uint>(words.Length + newAnnotations.Count + newTypes.Count);
		list.AddRange(words.AsSpan(0, 5).ToArray());
		list[3] = bound;
		for (int i = 0; i < instructions.Count; i++)
		{
			if (i == firstTypeInstruction)
			{
				list.AddRange(newAnnotations);
			}
			if (insertBefore.TryGetValue(i, out var value))
			{
				list.AddRange(value);
			}
			if (i == firstFunctionInstruction)
			{
				list.AddRange(newTypes);
			}
			int num = instructions[i];
			list.AddRange(words.AsSpan(num, (int)(words[num] >> 16)).ToArray());
		}
		byte[] array = new byte[list.Count * 4];
		Buffer.BlockCopy(list.ToArray(), 0, array, 0, array.Length);
		return array;
	}
}
