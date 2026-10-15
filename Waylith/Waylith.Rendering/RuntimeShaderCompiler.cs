using System;
using System.Buffers.Binary;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;
using System.Threading;
using System.Threading.Tasks;
using Brutal;
using Brutal.SlangApi;
using Brutal.Strings;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Waylith.Assets;
using Waylith.Diagnostics;
using Waylith.Graphics;
using Waylith.Logging;
using Waylith.Utils;

namespace Waylith.Rendering;

public static class RuntimeShaderCompiler
{
	private const uint CacheMagic = 1129531470u;

	private const int CacheFormatVersion = 4;

	private const string CacheExtension = ".spv-cache";

	private static readonly TimeSpan CacheEntryRenewAfter = TimeSpan.FromDays(1);

	private static readonly TimeSpan CacheEntryUnusedFor = TimeSpan.FromDays(30);

	private const uint SpirvOpName = 5u;

	private const uint SpirvOpDecorate = 71u;

	private const uint SpirvDecorationIndex = 32u;

	private const int NvidiaVendorId = 4318;

	private static readonly ConcurrentBag<IGlobalSession> AvailableSessions = new ConcurrentBag<IGlobalSession>();

	private static readonly StrategyBasedComWrappers SessionWrappers = new StrategyBasedComWrappers();

	private static readonly Lazy<string> CompilerVersion = new Lazy<string>(() => (typeof(Slang).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "unknown") + " " + Slang.GetBuildTagString());

	private static readonly int DefaultConcurrentSessions = System.Math.Clamp(Environment.ProcessorCount / 2, 2, 8);

	private static SemaphoreSlim SessionSlots = new SemaphoreSlim(DefaultConcurrentSessions, DefaultConcurrentSessions);

	private static string? CacheDirectory;

	private static AssetPack? Pack;

	public const string DefaultTargetProfile = "spirv_1_3";

	private static string TargetProfile = "spirv_1_3";

	private static readonly (string Name, string Value)[] HlslCompatibilityDefines = new(string, string)[12]
	{
		("min16float", "float"),
		("min16float2", "float2"),
		("min16float3", "float3"),
		("min16float4", "float4"),
		("min16int", "int"),
		("min16int2", "int2"),
		("min16int3", "int3"),
		("min16int4", "int4"),
		("min16uint", "uint"),
		("min16uint2", "uint2"),
		("min16uint3", "uint3"),
		("min16uint4", "uint4")
	};

	internal static string? CacheDirectoryPath => CacheDirectory;

	internal static EmitSpirvMethod HlslComputeEmitMethod => SelectEmitMethod(RuntimeShaderLanguage.Hlsl);

	public static void ConfigureCacheDirectory(string directory)
	{
		CacheDirectory = (string.IsNullOrWhiteSpace(directory) ? null : Path.GetFullPath(directory));
		string cacheDirectory = CacheDirectory;
		if (cacheDirectory != null)
		{
			Task.Run(delegate
			{
				CacheFileWriter.DeleteAbandonedTemporaries(cacheDirectory);
				CacheFileWriter.DeleteUnwrittenFor(cacheDirectory, "*.spv-cache", CacheEntryUnusedFor);
			});
		}
	}

	public static void UseAssetPack(AssetPack? pack)
	{
		Pack = pack;
	}

	public static void ConfigureTargetProfile(string profile)
	{
		TargetProfile = profile;
	}

	public static string PackEntryName(byte[] keyHash)
	{
		return "shaders/" + Convert.ToHexStringLower(keyHash) + ".spv-cache";
	}

	public static byte[] Compile(string source, string sourceName, string entryPoint, RuntimeShaderLanguage language, RuntimeShaderStage stage)
	{
		bool cacheHit;
		return Compile(source, sourceName, entryPoint, language, stage, out cacheHit);
	}

	public static byte[] Compile(string source, string sourceName, string entryPoint, RuntimeShaderLanguage language, RuntimeShaderStage stage, out bool cacheHit)
	{
		cacheHit = false;
		if (TryLoadOverride(sourceName, entryPoint, out byte[] spirv))
		{
			return spirv;
		}
		EmitSpirvMethod emitMethod = SelectEmitMethod(language);
		(OptimizationLevel, DebugInfoLevel) tuple = SelectCompileLevels(emitMethod);
		OptimizationLevel item = tuple.Item1;
		DebugInfoLevel item2 = tuple.Item2;
		byte[] keyHash = ComputeKeyHash(source, sourceName, entryPoint, language, stage, emitMethod, item, item2);
		if (TryReadPack(keyHash, out byte[] spirv2))
		{
			cacheHit = true;
			return spirv2;
		}
		string userCachePath = GetUserCachePath(keyHash);
		if (userCachePath != null && !ColdBoot.Enabled && TryReadCache(userCachePath, out byte[] spirv3))
		{
			cacheHit = true;
			return spirv3;
		}
		return CompileEntryPoints(source, sourceName, new string[1] { entryPoint }, language, stage, emitMethod, item, item2, new string[1] { userCachePath })[0];
	}

	public static byte[][] CompileBatch(string source, string sourceName, string[] entryPoints, RuntimeShaderLanguage language, RuntimeShaderStage stage)
	{
		EmitSpirvMethod emitMethod = SelectEmitMethod(language);
		(OptimizationLevel, DebugInfoLevel) tuple = SelectCompileLevels(emitMethod);
		OptimizationLevel item = tuple.Item1;
		DebugInfoLevel item2 = tuple.Item2;
		byte[][] array = new byte[entryPoints.Length][];
		string[] array2 = new string[entryPoints.Length];
		for (int i = 0; i < entryPoints.Length; i++)
		{
			if (TryLoadOverride(sourceName, entryPoints[i], out byte[] spirv))
			{
				array[i] = spirv;
				continue;
			}
			byte[] keyHash = ComputeKeyHash(source, sourceName, entryPoints[i], language, stage, emitMethod, item, item2);
			if (TryReadPack(keyHash, out byte[] spirv2))
			{
				array[i] = spirv2;
				continue;
			}
			array2[i] = GetUserCachePath(keyHash);
			if (array2[i] != null && !ColdBoot.Enabled && TryReadCache(array2[i], out byte[] spirv3))
			{
				array[i] = spirv3;
			}
		}
		if (Array.TrueForAll(array, (byte[] entry) => entry != null))
		{
			return array;
		}
		List<int> list = new List<int>(entryPoints.Length);
		for (int j = 0; j < entryPoints.Length; j++)
		{
			if (array[j] == null)
			{
				list.Add(j);
			}
		}
		string[] array3 = new string[list.Count];
		string[] array4 = new string[list.Count];
		for (int k = 0; k < list.Count; k++)
		{
			array3[k] = entryPoints[list[k]];
			array4[k] = array2[list[k]];
		}
		byte[][] array5 = CompileEntryPoints(source, sourceName, array3, language, stage, emitMethod, item, item2, array4);
		for (int l = 0; l < list.Count; l++)
		{
			array[list[l]] = array5[l];
		}
		return array;
	}

	public static void SetMaxConcurrentSessions(int count)
	{
		SessionSlots = new SemaphoreSlim(count, count);
	}

	public static void ReleaseSessions()
	{
		AvailableSessions.Clear();
	}

	public static List<(string Name, byte[] Record, bool Reused)> CompileForPack(string source, string sourceName, string[] entryPoints, RuntimeShaderLanguage language, RuntimeShaderStage stage, AssetPack? previous)
	{
		EmitSpirvMethod[] source2 = ((language != RuntimeShaderLanguage.Hlsl) ? new EmitSpirvMethod[1] { EmitSpirvMethod.Directly } : new EmitSpirvMethod[2]
		{
			EmitSpirvMethod.Directly,
			EmitSpirvMethod.ViaGlsl
		});
		ConcurrentBag<(string Name, byte[] Record, bool Reused)> baked = new ConcurrentBag<(string, byte[], bool)>();
		Parallel.ForEach(source2, delegate(EmitSpirvMethod emitMethod)
		{
			(OptimizationLevel, DebugInfoLevel) tuple = SelectCompileLevels(emitMethod);
			OptimizationLevel item = tuple.Item1;
			DebugInfoLevel item2 = tuple.Item2;
			List<string> list = new List<string>(entryPoints.Length);
			List<string> list2 = new List<string>(entryPoints.Length);
			string[] array = entryPoints;
			foreach (string text in array)
			{
				string text2 = PackEntryName(ComputeKeyHash(source, sourceName, text, language, stage, emitMethod, item, item2));
				if (previous != null && previous.TryRead(text2, out byte[] data) && TryParseRecord(data, out byte[] _))
				{
					baked.Add((text2, data, true));
				}
				else
				{
					list.Add(text);
					list2.Add(text2);
				}
			}
			if (list.Count != 0)
			{
				byte[][] array2 = new byte[list.Count][];
				CompileEntryPoints(source, sourceName, list.ToArray(), language, stage, emitMethod, item, item2, new string[list.Count], array2);
				for (int j = 0; j < array2.Length; j++)
				{
					baked.Add((list2[j], array2[j], false));
				}
			}
		});
		return baked.ToList();
	}

	private static (OptimizationLevel, DebugInfoLevel) SelectCompileLevels(EmitSpirvMethod emitMethod)
	{
		_ = 1;
		return (OptimizationLevel.Maximal, DebugInfoLevel.None);
	}

	private static byte[][] CompileEntryPoints(string source, string sourceName, string[] entryPoints, RuntimeShaderLanguage language, RuntimeShaderStage stage, EmitSpirvMethod emitMethod, OptimizationLevel optimizationLevel, DebugInfoLevel debugInfoLevel, string?[] writePaths, byte[][]? records = null)
	{
		byte[][] array = new byte[entryPoints.Length][];
		Stopwatch stopwatch = Stopwatch.StartNew();
		SemaphoreSlim sessionSlots = SessionSlots;
		sessionSlots.Wait();
		IGlobalSession result = null;
		try
		{
			if (!AvailableSessions.TryTake(out result))
			{
				result = CreateGlobalSession();
			}
			using ICompileRequest request = result.CreateCompileRequest();
			int translationUnitIndex = ConfigureRequest(request, result, source, sourceName, language, emitMethod, optimizationLevel, debugInfoLevel);
			foreach (string value in entryPoints)
			{
				request.AddEntryPoint(translationUnitIndex, ToRefString(value), ToSlangStage(stage));
			}
			if (request.Compile().IsFailure)
			{
				throw new InvalidOperationException("Failed to compile shader '" + sourceName + "': " + request.GetDiagnosticOutput());
			}
			for (int j = 0; j < entryPoints.Length; j++)
			{
				byte[] array2 = ExtractEntryPointCode(request, j, sourceName);
				if (language == RuntimeShaderLanguage.Glsl)
				{
					array2 = ApplyIndexedOutputDecorations(array2, source);
				}
				array[j] = array2;
				if (writePaths[j] != null)
				{
					TryWriteCache(writePaths[j], array2, request);
				}
				if (records != null)
				{
					records[j] = BuildRecord(array2, request);
				}
			}
			VerboseLog.Log($"ShaderCompiler: compiled '{sourceName}' ({entryPoints.Length} entry points) in {stopwatch.Elapsed.TotalMilliseconds:0}ms on thread {Environment.CurrentManagedThreadId}.");
			return array;
		}
		finally
		{
			if (result != null)
			{
				AvailableSessions.Add(result);
			}
			sessionSlots.Release();
		}
	}

	private unsafe static byte[] ExtractEntryPointCode(ICompileRequest request, int entryPointIndex, string sourceName)
	{
		ByteSize64 outSize;
		void* entryPointCode = request.GetEntryPointCode(entryPointIndex, out outSize);
		int num = outSize;
		if (entryPointCode == null || num == 0)
		{
			throw new InvalidOperationException("Slang produced no SPIR-V for shader '" + sourceName + "'.");
		}
		return new ReadOnlySpan<byte>(entryPointCode, num).ToArray();
	}

	private static int ConfigureRequest(ICompileRequest request, IGlobalSession session, string source, string sourceName, RuntimeShaderLanguage language, EmitSpirvMethod emitMethod, OptimizationLevel optimizationLevel, DebugInfoLevel debugInfoLevel)
	{
		request.SetCodeGenTarget(CompileTarget.Spirv);
		request.SetTargetProfile(0, FindProfile(session, TargetProfile));
		request.SetMatrixLayoutMode(MatrixLayoutMode.ColumnMajor);
		request.SetOptimizationLevel(optimizationLevel);
		request.SetDebugInfoLevel(debugInfoLevel);
		if (emitMethod == EmitSpirvMethod.ViaGlsl)
		{
			ConfigureSpirvEmission(request);
		}
		AddDefine(request, "WAYLITH_SLANG_COMPILER", "1");
		if (language == RuntimeShaderLanguage.Glsl && source.Contains("uniform sampler ", StringComparison.Ordinal))
		{
			AddDefine(request, "sampler", "SamplerState");
		}
		else if (language == RuntimeShaderLanguage.Hlsl && source.Contains("#define ASPM_HLSL", StringComparison.Ordinal))
		{
			(string, string)[] hlslCompatibilityDefines = HlslCompatibilityDefines;
			for (int i = 0; i < hlslCompatibilityDefines.Length; i++)
			{
				var (name, value) = hlslCompatibilityDefines[i];
				AddDefine(request, name, value);
			}
		}
		string[] compilerSearchDirectories = ShaderSourceLoader.GetCompilerSearchDirectories(sourceName);
		foreach (string value2 in compilerSearchDirectories)
		{
			request.AddSearchPath(ToRefString(value2));
		}
		int num = request.AddTranslationUnit(ToSlangLanguage(language), ToRefString(sourceName));
		request.AddTranslationUnitSourceString(num, ToRefString(sourceName), ToRefString(source));
		return num;
	}

	private static EmitSpirvMethod SelectEmitMethod(RuntimeShaderLanguage language)
	{
		if (language != RuntimeShaderLanguage.Hlsl || GpuInfo.VendorId != 4318)
		{
			return EmitSpirvMethod.Directly;
		}
		return EmitSpirvMethod.ViaGlsl;
	}

	private unsafe static void ConfigureSpirvEmission(ICompileRequest request)
	{
		fixed (byte* ptr2 = ToNullTerminatedUtf8("-emit-spirv-via-glsl"))
		{
			byte** ptr = stackalloc byte*[1];
			*ptr = ptr2;
			if (request.ProcessCommandLineArguments(ptr, 1).IsFailure)
			{
				throw new InvalidOperationException("Failed to select via-GLSL SPIR-V emission.");
			}
		}
	}

	private static bool TryLoadOverride(string sourceName, string entryPoint, out byte[] spirv)
	{
		spirv = Array.Empty<byte>();
		return false;
	}

	public static VkShaderModule CompileModule(DeviceEx device, string source, string sourceName, string entryPoint, RuntimeShaderLanguage language, RuntimeShaderStage stage)
	{
		bool cacheHit;
		byte[] spirv = Compile(source, sourceName, entryPoint, language, stage, out cacheHit);
		return CreateShaderModule(device, spirv, sourceName);
	}

	public unsafe static VkShaderModule CreateShaderModule(DeviceEx device, byte[] spirv, string name)
	{
		fixed (byte* code = spirv)
		{
			VkShaderModuleCreateInfo pCreateInfo = new VkShaderModuleCreateInfo
			{
				CodeSize = ByteSize.Of(spirv.Length),
				Code = (int*)code
			};
			return device.CreateShaderModule(in pCreateInfo, device.VkAllocator);
		}
	}

	private unsafe static IGlobalSession CreateGlobalSession()
	{
		GlobalSessionDesc globalSessionDesc = default(GlobalSessionDesc);
		globalSessionDesc.StructureSize = (uint)sizeof(GlobalSessionDesc);
		globalSessionDesc.ApiVersion = 0u;
		globalSessionDesc.EnableGlsl = true;
		GlobalSessionDesc desc = globalSessionDesc;
		Slang.PInvoke.slang_createGlobalSession2(in desc, out var outGlobalSession).ThrowIfFailure();
		try
		{
			return (IGlobalSession)SessionWrappers.GetOrCreateObjectForComInstance(outGlobalSession, CreateObjectFlags.None);
		}
		finally
		{
			Marshal.Release(outGlobalSession);
		}
	}

	private unsafe static ProfileID FindProfile(IGlobalSession session, string name)
	{
		fixed (byte* name2 = ToNullTerminatedUtf8(name))
		{
			return session.FindProfile(name2);
		}
	}

	private static void AddDefine(ICompileRequest request, string name, string value)
	{
		request.AddPreprocessorDefine(ToRefString(name), ToRefString(value));
	}

	private static RefString8 ToRefString(string value)
	{
		return new RefString8(ToNullTerminatedUtf8(value));
	}

	private static byte[] ToNullTerminatedUtf8(string value)
	{
		return Encoding.UTF8.GetBytes(value + "\0");
	}

	private static byte[] ComputeKeyHash(string source, string sourceName, string entryPoint, RuntimeShaderLanguage language, RuntimeShaderStage stage, EmitSpirvMethod emitMethod, OptimizationLevel optimizationLevel, DebugInfoLevel debugInfoLevel)
	{
		InlineArray12<object> buffer = default(InlineArray12<object>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray12<object>, object>(ref buffer, 0) = 4;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray12<object>, object>(ref buffer, 1) = CompilerVersion.Value;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray12<object>, object>(ref buffer, 2) = TargetProfile;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray12<object>, object>(ref buffer, 3) = emitMethod;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray12<object>, object>(ref buffer, 4) = MatrixLayoutMode.ColumnMajor;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray12<object>, object>(ref buffer, 5) = optimizationLevel;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray12<object>, object>(ref buffer, 6) = debugInfoLevel;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray12<object>, object>(ref buffer, 7) = language;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray12<object>, object>(ref buffer, 8) = stage;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray12<object>, object>(ref buffer, 9) = entryPoint;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray12<object>, object>(ref buffer, 10) = GetKeyDirectory(sourceName);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray12<object>, object>(ref buffer, 11) = source.Replace("\r\n", "\n").Replace("\r", "\n");
		string s = string.Join('\n', global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray12<object>, object>(in buffer, 12));
		return SHA256.HashData(Encoding.UTF8.GetBytes(s));
	}

	private static string? GetUserCachePath(byte[] keyHash)
	{
		string cacheDirectory = CacheDirectory;
		if (cacheDirectory != null)
		{
			return Path.Combine(cacheDirectory, Convert.ToHexStringLower(keyHash) + ".spv-cache");
		}
		return null;
	}

	private static bool TryReadPack(byte[] keyHash, out byte[] spirv)
	{
		spirv = Array.Empty<byte>();
		if (Pack != null && Pack.TryRead(PackEntryName(keyHash), out byte[] data))
		{
			return TryParseRecord(data, out spirv);
		}
		return false;
	}

	private static string GetKeyDirectory(string sourceName)
	{
		string directoryName = Path.GetDirectoryName(sourceName);
		if (string.IsNullOrWhiteSpace(directoryName))
		{
			return string.Empty;
		}
		if (!Path.IsPathRooted(directoryName))
		{
			return directoryName.Replace('\\', '/');
		}
		if (!ShaderSourceLoader.TryRelativizeToShaderRoot(directoryName, out string relative))
		{
			return Path.GetFullPath(directoryName);
		}
		return relative;
	}

	private static bool TryReadCache(string path, out byte[] spirv)
	{
		spirv = Array.Empty<byte>();
		try
		{
			FileInfo fileInfo = new FileInfo(path);
			if (!fileInfo.Exists || !TryParseRecord(File.ReadAllBytes(path), out spirv))
			{
				return false;
			}
			if (fileInfo.LastWriteTimeUtc < DateTime.UtcNow - CacheEntryRenewAfter)
			{
				RenewCacheEntry(fileInfo);
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	private static void RenewCacheEntry(FileInfo file)
	{
		try
		{
			file.LastWriteTimeUtc = DateTime.UtcNow;
		}
		catch (Exception ex) when (((ex is IOException || ex is UnauthorizedAccessException) ? 1 : 0) != 0)
		{
		}
	}

	private static bool TryParseRecord(byte[] record, out byte[] spirv)
	{
		spirv = Array.Empty<byte>();
		try
		{
			using MemoryStream memoryStream = new MemoryStream(record, writable: false);
			using BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8, leaveOpen: false);
			if (binaryReader.ReadUInt32() != 1129531470 || binaryReader.ReadInt32() != 4)
			{
				return false;
			}
			int num = binaryReader.ReadInt32();
			if ((num < 0 || num > 1024) ? true : false)
			{
				return false;
			}
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				string storedPath = binaryReader.ReadString();
				byte[] array2 = binaryReader.ReadBytes(32);
				if (array2.Length != 32 || !TryResolveDependency(storedPath, out array[i]))
				{
					return false;
				}
				byte[] array3 = SHA256.HashData(File.ReadAllBytes(array[i]));
				if (!CryptographicOperations.FixedTimeEquals(array2, array3))
				{
					return false;
				}
			}
			int num2 = binaryReader.ReadInt32();
			if ((num2 < 0 || num2 > 4096) ? true : false)
			{
				return false;
			}
			for (int j = 0; j < num2; j++)
			{
				int num3 = binaryReader.ReadInt32();
				string name = binaryReader.ReadString();
				int num4 = binaryReader.ReadInt32();
				if ((uint)num3 >= (uint)num || (uint)num4 >= (uint)num || !IsSameFile(ResolveInclude(array[num3], name), array[num4]))
				{
					return false;
				}
			}
			int num5 = binaryReader.ReadInt32();
			if (num5 <= 0 || num5 > 67108864 || num5 % 4 != 0 || memoryStream.Length - memoryStream.Position != num5)
			{
				return false;
			}
			spirv = binaryReader.ReadBytes(num5);
			return spirv.Length == num5 && BinaryPrimitives.ReadUInt32LittleEndian(spirv) == 119734787;
		}
		catch
		{
			spirv = Array.Empty<byte>();
			return false;
		}
	}

	private static void TryWriteCache(string path, byte[] spirv, ICompileRequest request)
	{
		try
		{
			CacheFileWriter.Write(path, BuildRecord(spirv, request));
		}
		catch
		{
		}
	}

	private static byte[] BuildRecord(byte[] spirv, ICompileRequest request)
	{
		var (list, list2) = GetDependencies(request);
		using MemoryStream memoryStream = new MemoryStream();
		using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8, leaveOpen: true))
		{
			binaryWriter.Write(1129531470u);
			binaryWriter.Write(4);
			binaryWriter.Write(list.Count);
			foreach (var (value, buffer) in list)
			{
				binaryWriter.Write(value);
				binaryWriter.Write(buffer);
			}
			binaryWriter.Write(list2.Count);
			foreach (var (value2, value3, value4) in list2)
			{
				binaryWriter.Write(value2);
				binaryWriter.Write(value3);
				binaryWriter.Write(value4);
			}
			binaryWriter.Write(spirv.Length);
			binaryWriter.Write(spirv);
		}
		return memoryStream.ToArray();
	}

	private static (List<(string Path, byte[] Hash)> Dependencies, List<(int Includer, string Name, int Target)> Includes) GetDependencies(ICompileRequest request)
	{
		List<(string, byte[])> list = new List<(string, byte[])>();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		int dependencyFileCount = request.GetDependencyFileCount();
		for (int i = 0; i < dependencyFileCount; i++)
		{
			string path2 = request.GetDependencyFilePath(i);
			if (!string.IsNullOrWhiteSpace(path2) && File.Exists(path2))
			{
				path2 = Path.GetFullPath(path2);
				if (!list2.Exists((string seen) => IsSameFile(seen, path2)))
				{
					byte[] array = File.ReadAllBytes(path2);
					string relative;
					string item = (ShaderSourceLoader.TryRelativizeToShaderRoot(path2, out relative) ? relative : path2);
					list.Add((item, SHA256.HashData(array)));
					list2.Add(path2);
					list3.Add(Encoding.UTF8.GetString(array));
				}
			}
		}
		List<(int, string, int)> list4 = new List<(int, string, int)>();
		for (int j = 0; j < list3.Count; j++)
		{
			string[] array2 = list3[j].Split('\n');
			foreach (string line in array2)
			{
				if (ShaderSourceLoader.TryParseQuotedInclude(line, out string includePath))
				{
					string resolved = ResolveInclude(list2[j], includePath);
					int num = list2.FindIndex((string path) => IsSameFile(path, resolved));
					if (num >= 0)
					{
						list4.Add((j, includePath, num));
					}
				}
			}
		}
		return (Dependencies: list, Includes: list4);
	}

	private static string ResolveInclude(string includerPath, string name)
	{
		return ShaderSourceLoader.ResolveIncludePath(Path.GetDirectoryName(includerPath), name);
	}

	private static bool IsSameFile(string a, string b)
	{
		return string.Equals(Path.GetFullPath(a), Path.GetFullPath(b), StringComparison.OrdinalIgnoreCase);
	}

	private static bool TryResolveDependency(string storedPath, out string resolvedPath)
	{
		if (Path.IsPathRooted(storedPath))
		{
			resolvedPath = storedPath;
			return File.Exists(storedPath);
		}
		return ShaderSourceLoader.TryResolveShaderPath(storedPath, out resolvedPath);
	}

	private static byte[] ApplyIndexedOutputDecorations(byte[] spirv, string source)
	{
		foreach (Match item in IndexedOutputPattern().Matches(source))
		{
			if (uint.TryParse(item.Groups["index"].Value, out var result))
			{
				uint num = FindNamedId(spirv, item.Groups["name"].Value);
				if (num != 0)
				{
					spirv = AddDecoration(spirv, num, 32u, result);
				}
			}
		}
		return spirv;
	}

	private static uint FindNamedId(byte[] spirv, string sourceName)
	{
		int num2;
		for (int i = 5; i < spirv.Length / 4; i += num2)
		{
			uint num = BinaryPrimitives.ReadUInt32LittleEndian(spirv.AsSpan(i * 4));
			num2 = (int)(num >> 16);
			uint num3 = num & 0xFFFF;
			if (num2 <= 0)
			{
				return 0u;
			}
			if (num3 != 5 || num2 < 3)
			{
				continue;
			}
			Span<byte> span = spirv.AsSpan((i + 2) * 4, (num2 - 2) * 4);
			int num4 = MemoryExtensions.IndexOf((ReadOnlySpan<byte>)span, (byte)0);
			string @string = Encoding.UTF8.GetString(span.Slice(0, (num4 >= 0) ? num4 : span.Length));
			int num5 = @string.Length - sourceName.Length;
			bool flag = @string.Equals(sourceName, StringComparison.Ordinal);
			if (!flag)
			{
				bool flag2 = num5 > 0;
				if (flag2)
				{
					char c = @string[num5 - 1];
					bool flag3 = ((c == '.' || c == '_') ? true : false);
					flag2 = flag3;
				}
				flag = flag2 && MemoryExtensions.Equals(@string.AsSpan(num5), sourceName.AsSpan(), StringComparison.Ordinal);
			}
			if (flag)
			{
				return BinaryPrimitives.ReadUInt32LittleEndian(spirv.AsSpan((i + 1) * 4));
			}
		}
		return 0u;
	}

	private static byte[] AddDecoration(byte[] spirv, uint targetId, uint decoration, uint value)
	{
		int num = -1;
		int num3;
		for (int i = 5; i < spirv.Length / 4; i += num3)
		{
			uint num2 = BinaryPrimitives.ReadUInt32LittleEndian(spirv.AsSpan(i * 4));
			num3 = (int)(num2 >> 16);
			uint num4 = num2 & 0xFFFF;
			if (num3 <= 0)
			{
				return spirv;
			}
			if (num4 == 71 && num3 >= 3 && BinaryPrimitives.ReadUInt32LittleEndian(spirv.AsSpan((i + 1) * 4)) == targetId)
			{
				if (num3 >= 4 && BinaryPrimitives.ReadUInt32LittleEndian(spirv.AsSpan((i + 2) * 4)) == decoration)
				{
					return spirv;
				}
				num = (i + num3) * 4;
			}
		}
		if (num < 0)
		{
			return spirv;
		}
		byte[] array = new byte[spirv.Length + 16];
		spirv.AsSpan(0, num).CopyTo(array);
		Span<byte> destination = array.AsSpan(num, 16);
		BinaryPrimitives.WriteUInt32LittleEndian(destination, 262215u);
		BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice(4), targetId);
		BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice(8), decoration);
		BinaryPrimitives.WriteUInt32LittleEndian(destination.Slice(12), value);
		spirv.AsSpan(num).CopyTo(array.AsSpan(num + 16));
		return array;
	}

	[GeneratedRegex("layout\\s*\\([^)]*\\bindex\\s*=\\s*(?<index>\\d+)[^)]*\\)\\s*(?:\\w+\\s+)*out\\s+\\w+\\s+(?<name>\\w+)\\s*;", RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex IndexedOutputPattern()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__IndexedOutputPattern_1.Instance;
	}

	private static SourceLanguage ToSlangLanguage(RuntimeShaderLanguage language)
	{
		return language switch
		{
			RuntimeShaderLanguage.Glsl => SourceLanguage.Glsl, 
			RuntimeShaderLanguage.Hlsl => SourceLanguage.Hlsl, 
			_ => throw new ArgumentOutOfRangeException("language"), 
		};
	}

	private static Stage ToSlangStage(RuntimeShaderStage stage)
	{
		return stage switch
		{
			RuntimeShaderStage.Vertex => Stage.Vertex, 
			RuntimeShaderStage.Fragment => Stage.Fragment, 
			RuntimeShaderStage.Compute => Stage.Compute, 
			_ => throw new ArgumentOutOfRangeException("stage"), 
		};
	}
}
