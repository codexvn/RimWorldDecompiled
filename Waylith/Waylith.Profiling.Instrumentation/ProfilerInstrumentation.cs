using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO.Enumeration;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Brutal.Logging;
using Brutal.Logging.Internal;
using WaylithClr;

namespace Waylith.Profiling.Instrumentation;

public static class ProfilerInstrumentation
{
	private sealed record TypeEntry(Type Type, string FullName, MethodInfo[] Methods);

	public static readonly bool IsSupported = MethodBodyUpdater.IsSupported;

	private static readonly LogCategory Log = LogCategory.Make("Instrumentation");

	private static readonly BindingFlags DeclaredMethods = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

	private static string[] targets = Array.Empty<string>();

	private static ProfilerTargetPattern[] targetPatterns = Array.Empty<ProfilerTargetPattern>();

	private static readonly List<(MethodBase Method, ProfilerLabelId Label)> replacedMethods = new List<(MethodBase, ProfilerLabelId)>();

	private static readonly Dictionary<MethodBase, ProfilerLabelId> mutedMethods = new Dictionary<MethodBase, ProfilerLabelId>();

	private static readonly HashSet<MethodBase> replacedMethodSet = new HashSet<MethodBase>();

	private static readonly HashSet<MethodBase> rejectedMethods = new HashSet<MethodBase>();

	private static HashSet<(Type Type, string Name)> overloadedMethods = new HashSet<(Type, string)>();

	private static readonly Dictionary<Assembly, TypeEntry[]> assemblyIndex = new Dictionary<Assembly, TypeEntry[]>();

	private static readonly string ProfilerNamespace = typeof(FrameProfiler).Namespace;

	private static readonly Func<MethodBase, int> LabelFactory = delegate(MethodBase method)
	{
		ProfilerLabelId orCreateLabelId = FrameProfiler.GetOrCreateLabelId(BuildLabel(method));
		FrameProfiler.MarkLabelDiscardable(orCreateLabelId);
		return orCreateLabelId.Value;
	};

	private const long NoGCBudgetPerCandidateBytes = 2560L;

	private const long MaxNoGCBudgetBytes = 536870912L;

	private const int MuteRevertBatchSize = 4096;

	private static readonly TimeSpan MuteRevertMaxDelay = TimeSpan.FromSeconds(5L);

	private static readonly HashSet<MethodBase> pendingMuteReverts = new HashSet<MethodBase>();

	private static readonly Stopwatch pendingMuteRevertAge = new Stopwatch();

	private static int metadataUpdateVersion = MethodBodyUpdater.MetadataUpdateVersion;

	private static readonly StringBuilder labelBuilder = new StringBuilder();

	public static bool IsEnabled { get; private set; }

	public static int InstrumentedMethodCount => replacedMethods.Count;

	public static int MutedMethodCount => mutedMethods.Count;

	public static int MatchedMethodCount { get; private set; }

	public static IReadOnlyList<string> Targets => targets;

	public static string[] ExcludedAssemblies { get; set; } = Array.Empty<string>();

	public static event Action? InstrumentedMethodsChanged;

	private static bool IsExcluded(string assemblyName)
	{
		string[] excludedAssemblies = ExcludedAssemblies;
		for (int i = 0; i < excludedAssemblies.Length; i++)
		{
			if (FileSystemName.MatchesSimpleExpression(excludedAssemblies[i].AsSpan(), assemblyName.AsSpan()))
			{
				return true;
			}
		}
		return false;
	}

	public static void SetTargets(IEnumerable<string> patterns)
	{
		string[] array = (from pattern in patterns
			select pattern.Trim() into trimmed
			where trimmed.Length != 0
			select trimmed).ToArray();
		if (((ReadOnlySpan<string>)array.AsSpan()).SequenceEqual((ReadOnlySpan<string>)targets))
		{
			return;
		}
		targets = array;
		targetPatterns = ParsePatterns(targets);
		if (!IsEnabled)
		{
			return;
		}
		try
		{
			ApplyTargets();
		}
		catch (Exception value)
		{
			LogCategory log = Log;
			bool shouldAppend;
			LogString<Error> message = new LogString<Error>(49, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Re-instrumentation after a target change failed: ");
				message.AppendFormatted(value);
			}
			log.Error(message, "SetTargets", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Profiling\\Instrumentation\\ProfilerInstrumentation.cs", 106);
		}
	}

	public static int[] CountMatches(IReadOnlyList<string> patterns, out int total)
	{
		ProfilerTargetPattern[] patterns2 = ParsePatterns(patterns);
		int[] array = new int[patterns.Count];
		total = CollectMatches(patterns2, null, null, array);
		return array;
	}

	public static void SetEnabled(bool value)
	{
		if (!IsSupported || IsEnabled == value)
		{
			return;
		}
		try
		{
			if (value)
			{
				ApplyTargets();
				FrameProfiler.MutedLabelsChanged += ReconcileMutedLabels;
			}
			else
			{
				FrameProfiler.MutedLabelsChanged -= ReconcileMutedLabels;
				RevertAll();
			}
			IsEnabled = value;
		}
		catch (Exception value2)
		{
			LogCategory log = Log;
			bool shouldAppend;
			LogString<Error> message = new LogString<Error>(25, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Instrumentation ");
				message.AppendFormatted(value ? "setup" : "revert");
				message.AppendLiteral(" failed: ");
				message.AppendFormatted(value2);
			}
			log.Error(message, "SetEnabled", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Profiling\\Instrumentation\\ProfilerInstrumentation.cs", 142);
		}
	}

	private static ProfilerTargetPattern[] ParsePatterns(IReadOnlyList<string> patterns)
	{
		ProfilerTargetPattern[] array = new ProfilerTargetPattern[patterns.Count];
		for (int i = 0; i < patterns.Count; i++)
		{
			array[i] = ProfilerTargetPattern.Parse(patterns[i]);
		}
		return array;
	}

	private static int CollectMatches(ProfilerTargetPattern[] patterns, List<MethodInfo>? matched, HashSet<(Type, string)>? overloads, int[]? perPatternCounts)
	{
		using (FrameProfiler.Scope("ProfilerInstrumentation.CollectMatches"))
		{
			if (patterns.Length == 0)
			{
				return 0;
			}
			int num = 0;
			HashSet<string> hashSet = new HashSet<string>();
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				if (assembly.IsDynamic || assembly.IsCollectible)
				{
					continue;
				}
				string name = assembly.GetName().Name;
				if (name == null || IsExcluded(name))
				{
					continue;
				}
				bool flag = assembly == typeof(object).Assembly;
				list.Clear();
				for (int j = 0; j < patterns.Length; j++)
				{
					if ((!flag || !patterns[j].HasAssemblyWildcard) && patterns[j].MatchesAssembly(name))
					{
						list.Add(j);
					}
				}
				if (list.Count == 0)
				{
					continue;
				}
				TypeEntry[] typeIndex = GetTypeIndex(assembly);
				foreach (TypeEntry typeEntry in typeIndex)
				{
					list2.Clear();
					foreach (int item in list)
					{
						if (patterns[item].MatchesType(typeEntry.FullName))
						{
							list2.Add(item);
						}
					}
					if (list2.Count == 0)
					{
						continue;
					}
					hashSet.Clear();
					MethodInfo[] methods = typeEntry.Methods;
					foreach (MethodInfo methodInfo in methods)
					{
						bool flag2 = false;
						foreach (int item2 in list2)
						{
							if (patterns[item2].MatchesMethod(methodInfo.Name))
							{
								flag2 = true;
								if (perPatternCounts == null)
								{
									break;
								}
								perPatternCounts[item2]++;
							}
						}
						if (flag2)
						{
							num++;
							matched?.Add(methodInfo);
							if (overloads != null && !hashSet.Add(methodInfo.Name))
							{
								overloads.Add((typeEntry.Type, methodInfo.Name));
							}
						}
					}
				}
			}
			return num;
		}
	}

	private static TypeEntry[] GetTypeIndex(Assembly assembly)
	{
		if (assemblyIndex.TryGetValue(assembly, out TypeEntry[] value))
		{
			return value;
		}
		bool flag = assembly == typeof(object).Assembly;
		bool flag2 = assembly == typeof(FrameProfiler).Assembly;
		List<TypeEntry> list = new List<TypeEntry>();
		foreach (Type loadableType in GetLoadableTypes(assembly))
		{
			string fullName = loadableType.FullName;
			if (fullName == null || IsCompilerGenerated(loadableType) || (flag && loadableType.Namespace == "System.Runtime.CompilerServices") || loadableType == typeof(SelfProfiler))
			{
				continue;
			}
			if (flag2)
			{
				string @namespace = loadableType.Namespace;
				if (@namespace != null && (@namespace == ProfilerNamespace || @namespace.StartsWith(ProfilerNamespace + ".", StringComparison.Ordinal)))
				{
					continue;
				}
			}
			list.Add(new TypeEntry(loadableType, fullName, loadableType.GetMethods(DeclaredMethods)));
		}
		value = list.ToArray();
		assemblyIndex[assembly] = value;
		return value;
	}

	private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
	{
		try
		{
			return assembly.GetTypes();
		}
		catch (ReflectionTypeLoadException ex)
		{
			List<Type> list = new List<Type>(ex.Types.Length);
			Type[] types = ex.Types;
			foreach (Type type in types)
			{
				if ((object)type != null)
				{
					list.Add(type);
				}
			}
			LogCategory log = Log;
			bool shouldAppend;
			LogString<Warning> message = new LogString<Warning>(47, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendFormatted(assembly.GetName().Name);
				message.AppendLiteral(" has ");
				message.AppendFormatted(ex.Types.Length - list.Count);
				message.AppendLiteral(" unloadable types; instrumenting the rest.");
			}
			log.Warning(message, "GetLoadableTypes", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Profiling\\Instrumentation\\ProfilerInstrumentation.cs", 292);
			return list;
		}
	}

	private static bool IsCompilerGenerated(MemberInfo member)
	{
		return member.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false);
	}

	private static void ApplyTargets()
	{
		using (FrameProfiler.Scope("ProfilerInstrumentation.ApplyTargets"))
		{
			Stopwatch stopwatch = Stopwatch.StartNew();
			HashSet<(Type, string)> overloads = new HashSet<(Type, string)>();
			List<MethodInfo> list = new List<MethodInfo>();
			MatchedMethodCount = CollectMatches(targetPatterns, list, overloads, null);
			HashSet<MethodBase> wanted = new HashSet<MethodBase>(list);
			int value = RevertWhere(delegate(MethodBase method, ProfilerLabelId label)
			{
				if (!wanted.Contains(method))
				{
					return true;
				}
				if (FrameProfiler.IsLabelMuted(label))
				{
					mutedMethods[method] = label;
					return true;
				}
				(Type, string) item = (method.DeclaringType, method.Name);
				return overloadedMethods.Contains(item) != overloads.Contains(item);
			});
			overloadedMethods = overloads;
			pendingMuteReverts.Clear();
			pendingMuteRevertAge.Reset();
			rejectedMethods.RemoveWhere((MethodBase method) => !wanted.Contains(method));
			foreach (var (methodBase2, id) in mutedMethods)
			{
				if (!wanted.Contains(methodBase2) || !FrameProfiler.IsLabelMuted(id))
				{
					mutedMethods.Remove(methodBase2);
				}
			}
			List<MethodInfo> list2 = new List<MethodInfo>();
			foreach (MethodInfo item2 in list)
			{
				if (!replacedMethodSet.Contains(item2) && !rejectedMethods.Contains(item2) && !mutedMethods.ContainsKey(item2))
				{
					list2.Add(item2);
				}
			}
			bool flag = TryStartNoGCRegion(list2.Count);
			int value2;
			try
			{
				value2 = Instrument(list2);
			}
			finally
			{
				if (flag)
				{
					EndNoGCRegion();
				}
			}
			LogCategory log = Log;
			bool shouldAppend;
			LogString<Information> message = new LogString<Information>(53, 6, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Instrumented +");
				message.AppendFormatted(value2);
				message.AppendLiteral(" / reverted -");
				message.AppendFormatted(value);
				message.AppendLiteral(" in ");
				message.AppendFormatted(stopwatch.ElapsedMilliseconds);
				message.AppendLiteral("ms; ");
				message.AppendFormatted(replacedMethods.Count);
				message.AppendLiteral(" live of ");
				message.AppendFormatted(MatchedMethodCount);
				message.AppendLiteral(" matched");
				message.AppendFormatted((mutedMethods.Count == 0) ? "" : $", {mutedMethods.Count} held back as muted");
				message.AppendLiteral(".");
			}
			log.Info(message, "ApplyTargets", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Profiling\\Instrumentation\\ProfilerInstrumentation.cs", 366);
			ProfilerInstrumentation.InstrumentedMethodsChanged?.Invoke();
		}
	}

	private static bool TryStartNoGCRegion(int candidateCount)
	{
		long num = (long)candidateCount * 2560L;
		long num2 = ((num < 536870912) ? num : 536870912);
		if (num2 == 0L)
		{
			return false;
		}
		try
		{
			return GC.TryStartNoGCRegion(num2);
		}
		catch (Exception ex) when (((ex is ArgumentOutOfRangeException || ex is InvalidOperationException) ? 1 : 0) != 0)
		{
			return false;
		}
	}

	private static void EndNoGCRegion()
	{
		if (GCSettings.LatencyMode != GCLatencyMode.NoGCRegion)
		{
			return;
		}
		try
		{
			GC.EndNoGCRegion();
		}
		catch (InvalidOperationException)
		{
		}
	}

	private static void ReconcileMutedLabels()
	{
		if (!IsEnabled)
		{
			return;
		}
		try
		{
			int num = 0;
			foreach (var replacedMethod in replacedMethods)
			{
				var (item, _) = replacedMethod;
				if (FrameProfiler.IsLabelMuted(replacedMethod.Label) && pendingMuteReverts.Add(item))
				{
					num++;
				}
			}
			if (num != 0 && !pendingMuteRevertAge.IsRunning)
			{
				pendingMuteRevertAge.Restart();
			}
			if (pendingMuteReverts.Count >= 4096 || !FrameProfiler.Enabled || FrameProfiler.Paused)
			{
				FlushMuteReverts();
			}
			foreach (KeyValuePair<MethodBase, ProfilerLabelId> mutedMethod in mutedMethods)
			{
				mutedMethod.Deconstruct(out var _, out var value);
				if (!FrameProfiler.IsLabelMuted(value))
				{
					ApplyTargets();
					break;
				}
			}
		}
		catch (Exception value2)
		{
			LogCategory log = Log;
			bool shouldAppend;
			LogString<Error> message = new LogString<Error>(54, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Reconciling instrumentation with muted labels failed: ");
				message.AppendFormatted(value2);
			}
			log.Error(message, "ReconcileMutedLabels", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Profiling\\Instrumentation\\ProfilerInstrumentation.cs", 443);
		}
	}

	internal static void OnProfilerFrameStarted()
	{
		ReconcileMetadataUpdates();
		if (pendingMuteReverts.Count != 0 && pendingMuteRevertAge.Elapsed >= MuteRevertMaxDelay)
		{
			FlushMuteReverts();
		}
	}

	private static void ReconcileMetadataUpdates()
	{
		int num = MethodBodyUpdater.MetadataUpdateVersion;
		if (num == metadataUpdateVersion)
		{
			return;
		}
		metadataUpdateVersion = num;
		if (!IsEnabled)
		{
			return;
		}
		bool shouldAppend;
		try
		{
			HashSet<MethodBase> updated = new HashSet<MethodBase>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				if (assembly.IsDynamic)
				{
					continue;
				}
				Module[] modules = assembly.GetModules();
				foreach (Module module in modules)
				{
					int[] updatedMethodTokens = MethodBodyUpdater.GetUpdatedMethodTokens(module);
					if (updatedMethodTokens.Length == 0)
					{
						continue;
					}
					assemblyIndex.Remove(assembly);
					int[] array = updatedMethodTokens;
					foreach (int metadataToken in array)
					{
						MethodBase methodBase = module.ResolveMethod(metadataToken);
						if ((object)methodBase != null)
						{
							updated.Add(methodBase);
						}
					}
				}
			}
			if (updated.Count != 0)
			{
				replacedMethodSet.ExceptWith(updated);
				replacedMethods.RemoveAll(((MethodBase Method, ProfilerLabelId Label) entry) => updated.Contains(entry.Method));
				rejectedMethods.ExceptWith(updated);
				LogCategory log = Log;
				LogCategory logCategory = log;
				LogString<Information> message = new LogString<Information>(75, 1, log, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendLiteral("Hot reload changed ");
					message.AppendFormatted(updated.Count);
					message.AppendLiteral(" instrumented methods; re-instrumenting from the new IL.");
				}
				logCategory.Info(message, "ReconcileMetadataUpdates", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Profiling\\Instrumentation\\ProfilerInstrumentation.cs", 502);
				ApplyTargets();
			}
		}
		catch (Exception value)
		{
			LogCategory log = Log;
			LogCategory logCategory2 = log;
			LogString<Error> message2 = new LogString<Error>(46, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message2.AppendLiteral("Re-instrumentation after a hot reload failed: ");
				message2.AppendFormatted(value);
			}
			logCategory2.Error(message2, "ReconcileMetadataUpdates", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Profiling\\Instrumentation\\ProfilerInstrumentation.cs", 507);
		}
	}

	private static void FlushMuteReverts()
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		int num = RevertWhere(delegate(MethodBase method, ProfilerLabelId label)
		{
			if (!pendingMuteReverts.Contains(method) || !FrameProfiler.IsLabelMuted(label))
			{
				return false;
			}
			mutedMethods[method] = label;
			return true;
		});
		pendingMuteReverts.Clear();
		pendingMuteRevertAge.Reset();
		if (num != 0)
		{
			LogCategory log = Log;
			bool shouldAppend;
			LogString<Trace> message = new LogString<Trace>(48, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Reverted ");
				message.AppendFormatted(num);
				message.AppendLiteral(" methods whose labels were muted in ");
				message.AppendFormatted(stopwatch.ElapsedMilliseconds);
				message.AppendLiteral("ms.");
			}
			log.Trace(message, "FlushMuteReverts", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Profiling\\Instrumentation\\ProfilerInstrumentation.cs", 529);
			ProfilerInstrumentation.InstrumentedMethodsChanged?.Invoke();
		}
	}

	private static int Instrument(List<MethodInfo> candidates)
	{
		if (candidates.Count == 0)
		{
			return 0;
		}
		InstrumentationRewriter.ClearAnalysisCaches();
		List<MethodBase> list = new List<MethodBase>();
		List<ProfilerLabelId> list2 = new List<ProfilerLabelId>();
		List<InstrumentationRewriter.EmittedMethod> list3 = new List<InstrumentationRewriter.EmittedMethod>();
		int num = 0;
		Dictionary<Module, (int, int, int)> dictionary = new Dictionary<Module, (int, int, int)>();
		ConstructorInfo constructor = typeof(ProfilerLabelId).GetConstructor(new Type[1] { typeof(ushort) });
		MethodInfo method2 = typeof(FrameProfiler).GetMethod("TryEnterScope", new Type[1] { typeof(ProfilerLabelId) });
		MethodInfo method3 = typeof(FrameProfiler).GetMethod("LeaveScope", Type.EmptyTypes);
		(MethodBody Body, byte[] Il, InstrumentationRewriter.Analysis Analysis)?[] accepted = new(MethodBody, byte[], InstrumentationRewriter.Analysis)?[candidates.Count];
		ConcurrentQueue<string> analysisFailures = new ConcurrentQueue<string>();
		using (FrameProfiler.Scope("ProfilerInstrumentation.Instrument/Analyze"))
		{
			Parallel.For(0, candidates.Count, delegate(int index)
			{
				MethodInfo methodInfo = candidates[index];
				try
				{
					if (ShouldInstrument(methodInfo, out MethodBody body, out byte[] ilBytes) && InstrumentationRewriter.TryAnalyze(methodInfo, body, ilBytes, out var analysis))
					{
						accepted[index] = (body, ilBytes, analysis);
					}
				}
				catch (Exception value)
				{
					analysisFailures.Enqueue($"Failed to analyze {methodInfo.DeclaringType?.FullName}.{methodInfo.Name}: {value}");
				}
			});
		}
		num += analysisFailures.Count;
		foreach (string item2 in analysisFailures)
		{
			Log.Warning(item2, "Instrument", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Profiling\\Instrumentation\\ProfilerInstrumentation.cs", 573);
		}
		InstrumentationRewriter.ResetEmitOutput();
		bool shouldAppend;
		using (FrameProfiler.Scope("ProfilerInstrumentation.Instrument/Emit"))
		{
			for (int i = 0; i < candidates.Count; i++)
			{
				MethodInfo methodInfo2 = candidates[i];
				(MethodBody, byte[], InstrumentationRewriter.Analysis)? tuple = accepted[i];
				if (tuple.HasValue)
				{
					(MethodBody, byte[], InstrumentationRewriter.Analysis) valueOrDefault = tuple.GetValueOrDefault();
					try
					{
						if (!dictionary.TryGetValue(methodInfo2.Module, out var value2))
						{
							value2 = (MethodBodyUpdater.GetTokenFor(methodInfo2, constructor), MethodBodyUpdater.GetTokenFor(methodInfo2, method2), MethodBodyUpdater.GetTokenFor(methodInfo2, method3));
							dictionary.Add(methodInfo2.Module, value2);
						}
						InstrumentationRewriter.EmittedMethod item = InstrumentationRewriter.Emit(methodInfo2, valueOrDefault.Item1, valueOrDefault.Item2, in valueOrDefault.Item3, LabelFactory, value2.Item1, value2.Item2, value2.Item3);
						list.Add(methodInfo2);
						list2.Add(new ProfilerLabelId((ushort)item.LabelId));
						list3.Add(item);
					}
					catch (Exception value3)
					{
						num++;
						rejectedMethods.Add(methodInfo2);
						LogCategory log = Log;
						LogCategory logCategory = log;
						LogString<Warning> message = new LogString<Warning>(21, 3, log, out shouldAppend);
						if (shouldAppend)
						{
							message.AppendLiteral("Failed to rewrite ");
							message.AppendFormatted(methodInfo2.DeclaringType?.FullName);
							message.AppendLiteral(".");
							message.AppendFormatted(methodInfo2.Name);
							message.AppendLiteral(": ");
							message.AppendFormatted(value3);
						}
						logCategory.Warning(message, "Instrument", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Profiling\\Instrumentation\\ProfilerInstrumentation.cs", 607);
					}
				}
				else
				{
					rejectedMethods.Add(methodInfo2);
				}
			}
		}
		int num2 = 0;
		using (FrameProfiler.Scope("ProfilerInstrumentation.Instrument/ReplaceMethodBodies"))
		{
			int[] array = new int[list.Count];
			int[] array2 = new int[list.Count];
			for (int j = 0; j < list.Count; j++)
			{
				array[j] = list3[j].BodyLength;
				array2[j] = list3[j].MapCount;
			}
			try
			{
				MethodBodyUpdater.ReplaceMethodBodies(CollectionsMarshal.AsSpan(list), InstrumentationRewriter.EmittedBodies, array, InstrumentationRewriter.EmittedOffsetMaps, array2);
				for (int k = 0; k < list.Count; k++)
				{
					num2 += Track(list[k], list2[k]);
				}
			}
			catch (Exception ex)
			{
				LogCategory log = Log;
				LogCategory logCategory2 = log;
				LogString<Warning> message = new LogString<Warning>(49, 1, log, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendLiteral("Batch replacement failed (");
					message.AppendFormatted(ex.Message);
					message.AppendLiteral("); retrying per method.");
				}
				logCategory2.Warning(message, "Instrument", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Profiling\\Instrumentation\\ProfilerInstrumentation.cs", 632);
				for (int l = 0; l < list.Count; l++)
				{
					ReadOnlySpan<byte> methodBody = InstrumentationRewriter.EmittedBodies.Slice(list3[l].BodyOffset, list3[l].BodyLength);
					ReadOnlySpan<ILOffsetMapEntry> ilOffsetMap = InstrumentationRewriter.EmittedOffsetMaps.Slice(list3[l].MapOffset, list3[l].MapCount);
					try
					{
						MethodBodyUpdater.ReplaceMethodBody(list[l], methodBody, ilOffsetMap);
						num2 += Track(list[l], list2[l]);
					}
					catch (Exception value4)
					{
						num++;
						rejectedMethods.Add(list[l]);
						log = Log;
						LogCategory logCategory3 = log;
						message = new LogString<Warning>(21, 3, log, out shouldAppend);
						if (shouldAppend)
						{
							message.AppendLiteral("Failed to replace ");
							message.AppendFormatted(list[l].DeclaringType?.FullName);
							message.AppendLiteral(".");
							message.AppendFormatted(list[l].Name);
							message.AppendLiteral(": ");
							message.AppendFormatted(value4);
						}
						logCategory3.Warning(message, "Instrument", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Profiling\\Instrumentation\\ProfilerInstrumentation.cs", 647);
					}
				}
			}
		}
		if (num != 0)
		{
			LogCategory log = Log;
			LogCategory logCategory4 = log;
			LogString<Warning> message = new LogString<Warning>(39, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendFormatted(num);
				message.AppendLiteral(" of ");
				message.AppendFormatted(candidates.Count);
				message.AppendLiteral(" methods could not be instrumented.");
			}
			logCategory4.Warning(message, "Instrument", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Profiling\\Instrumentation\\ProfilerInstrumentation.cs", 654);
		}
		return num2;
		static int Track(MethodBase method, ProfilerLabelId label)
		{
			if (!replacedMethodSet.Add(method))
			{
				return 0;
			}
			replacedMethods.Add((method, label));
			return 1;
		}
	}

	private static int RevertWhere(Func<MethodBase, ProfilerLabelId, bool> stale)
	{
		List<MethodBase> list = new List<MethodBase>();
		foreach (var (methodBase, arg) in replacedMethods)
		{
			if (stale(methodBase, arg))
			{
				list.Add(methodBase);
			}
		}
		if (list.Count == 0)
		{
			return 0;
		}
		using (FrameProfiler.Scope("ProfilerInstrumentation.Revert"))
		{
			MethodBodyUpdater.Revert(CollectionsMarshal.AsSpan(list));
			foreach (MethodBase item in list)
			{
				replacedMethodSet.Remove(item);
			}
			replacedMethods.RemoveAll(((MethodBase Method, ProfilerLabelId Label) entry) => !replacedMethodSet.Contains(entry.Method));
			return list.Count;
		}
	}

	private static void RevertAll()
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		int value = RevertWhere((MethodBase _, ProfilerLabelId _) => true);
		rejectedMethods.Clear();
		mutedMethods.Clear();
		assemblyIndex.Clear();
		pendingMuteReverts.Clear();
		pendingMuteRevertAge.Reset();
		MatchedMethodCount = 0;
		LogCategory log = Log;
		bool shouldAppend;
		LogString<Information> message = new LogString<Information>(24, 2, log, out shouldAppend);
		if (shouldAppend)
		{
			message.AppendLiteral("Reverted ");
			message.AppendFormatted(value);
			message.AppendLiteral(" methods in ");
			message.AppendFormatted(stopwatch.ElapsedMilliseconds);
			message.AppendLiteral("ms.");
		}
		log.Info(message, "RevertAll", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Profiling\\Instrumentation\\ProfilerInstrumentation.cs", 699);
		ProfilerInstrumentation.InstrumentedMethodsChanged?.Invoke();
	}

	private static bool ShouldInstrument(MethodInfo method, [NotNullWhen(true)] out MethodBody? body, [NotNullWhen(true)] out byte[]? ilBytes)
	{
		body = null;
		ilBytes = null;
		if (method.IsAbstract || (method.Attributes & MethodAttributes.PinvokeImpl) != 0)
		{
			return false;
		}
		if ((method.MethodImplementationFlags & MethodImplAttributes.AggressiveInlining) != 0)
		{
			return false;
		}
		if (IsCompilerGenerated(method) || method.IsDefined(typeof(UnmanagedCallersOnlyAttribute), inherit: false))
		{
			return false;
		}
		body = method.GetMethodBody();
		if (body == null)
		{
			return false;
		}
		ilBytes = body.GetILAsByteArray();
		byte[] array = ilBytes;
		if (array != null)
		{
			return array.Length > 0;
		}
		return false;
	}

	private static string BuildLabel(MethodBase method)
	{
		labelBuilder.Clear();
		labelBuilder.Append("Auto:");
		AppendTypeDisplayName(labelBuilder, method.DeclaringType);
		labelBuilder.Append('.').Append(method.Name);
		if (!overloadedMethods.Contains((method.DeclaringType, method.Name)))
		{
			return labelBuilder.ToString();
		}
		labelBuilder.Append('(');
		ParameterInfo[] parameters = method.GetParameters();
		for (int i = 0; i < parameters.Length; i++)
		{
			if (i != 0)
			{
				labelBuilder.Append(", ");
			}
			AppendTypeDisplayName(labelBuilder, parameters[i].ParameterType);
		}
		return labelBuilder.Append(')').ToString();
	}

	private static void AppendTypeDisplayName(StringBuilder builder, Type type)
	{
		Type declaringType = type.DeclaringType;
		if ((object)declaringType != null)
		{
			AppendTypeDisplayName(builder, declaringType);
			builder.Append('.');
		}
		string name = type.Name;
		int num = name.IndexOf('`', StringComparison.Ordinal);
		builder.Append(name, 0, (num < 0) ? name.Length : num);
	}
}
