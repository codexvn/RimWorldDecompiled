using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Waylith.Rendering.Culling;

namespace Waylith.Rendering;

public static class MeshDrawStats
{
	private struct ProgramStat
	{
		public int Draws;

		public long Tris;
	}

	private struct PassStat
	{
		public int Draws;

		public int Binds;

		public long Tris;

		public int IdentityDraws;
	}

	private static byte currentPass;

	private static readonly Dictionary<byte, PassStat> byPass = new Dictionary<byte, PassStat>();

	private static readonly Dictionary<(Shader Shader, byte Pass), ProgramStat> byProgramPass = new Dictionary<(Shader, byte), ProgramStat>();

	private static long lastLogMs;

	private static readonly List<KeyValuePair<byte, PassStat>> logPassRows = new List<KeyValuePair<byte, PassStat>>();

	private static readonly List<KeyValuePair<(Shader Shader, byte Pass), ProgramStat>> logRows = new List<KeyValuePair<(Shader, byte), ProgramStat>>();

	private static readonly HashSet<Shader> logPrograms = new HashSet<Shader>();

	[Conditional("MESH_DRAW_STATS")]
	public static void BeginPass(byte pass)
	{
		currentPass = pass;
	}

	[Conditional("MESH_DRAW_STATS")]
	public static void RecordBindPipeline()
	{
		byPass.TryGetValue(currentPass, out var value);
		value.Binds++;
		byPass[currentPass] = value;
	}

	[Conditional("MESH_DRAW_STATS")]
	public static void RecordDraw(FinalDrawItem draw, int instances)
	{
		long num = (long)(draw.Mesh.IndexCount / 3) * (long)instances;
		byPass.TryGetValue(currentPass, out var value);
		value.Draws++;
		value.Tris += num;
		if (draw.Matrix.IsIdentity)
		{
			value.IdentityDraws++;
		}
		byPass[currentPass] = value;
		(Shader, byte) key = (draw.Binding.Shader, currentPass);
		byProgramPass.TryGetValue(key, out var value2);
		value2.Draws++;
		value2.Tris += num;
		byProgramPass[key] = value2;
	}

	[Conditional("MESH_DRAW_STATS")]
	public static void EndFrame()
	{
		bool flag = false;
		foreach (PassStat value in byPass.Values)
		{
			if (value.Draws > 0)
			{
				flag = true;
				break;
			}
		}
		if (flag && Environment.TickCount64 - lastLogMs >= 1000)
		{
			lastLogMs = Environment.TickCount64;
			Log();
		}
		byPass.Clear();
		byProgramPass.Clear();
	}

	private static int PassRank(byte pass)
	{
		string name = RenderPassRegistry.GetName(pass);
		if (name == null || !name.Contains("Shadow"))
		{
			return 0;
		}
		return 1;
	}

	private static void Log()
	{
		StringBuilder stringBuilder = new StringBuilder();
		logPassRows.Clear();
		foreach (KeyValuePair<byte, PassStat> item in byPass)
		{
			logPassRows.Add(item);
		}
		logPassRows.Sort((KeyValuePair<byte, PassStat> a, KeyValuePair<byte, PassStat> b) => PassRank(a.Key).CompareTo(PassRank(b.Key)));
		foreach (KeyValuePair<byte, PassStat> logPassRow in logPassRows)
		{
			PassStat value = logPassRow.Value;
			if (value.Draws == 0)
			{
				continue;
			}
			logPrograms.Clear();
			foreach (var key in byProgramPass.Keys)
			{
				if (key.Pass == logPassRow.Key)
				{
					logPrograms.Add(key.Shader);
				}
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append('\n');
			}
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder3 = stringBuilder2;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(59, 6, stringBuilder2);
			handler.AppendLiteral("[meshdraws] ");
			handler.AppendFormatted(RenderPassRegistry.GetName(logPassRow.Key));
			handler.AppendLiteral(": ");
			handler.AppendFormatted(value.Draws);
			handler.AppendLiteral(" draws, ");
			handler.AppendFormatted(value.Binds);
			handler.AppendLiteral(" binds, ");
			handler.AppendFormatted(value.Tris / 1000);
			handler.AppendLiteral("k tris, ");
			handler.AppendFormatted(logPrograms.Count);
			handler.AppendLiteral(" programs, ");
			handler.AppendFormatted(value.IdentityDraws * 100 / value.Draws);
			handler.AppendLiteral("% identity");
			stringBuilder3.Append(ref handler);
		}
		if (OcclusionQuery.RegisteredCount > 0)
		{
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder4 = stringBuilder2;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(21, 2, stringBuilder2);
			handler.AppendLiteral("\n[occlusion] ");
			handler.AppendFormatted(OcclusionQuery.CulledCount);
			handler.AppendLiteral("/");
			handler.AppendFormatted(OcclusionQuery.RegisteredCount);
			handler.AppendLiteral(" culled");
			stringBuilder4.Append(ref handler);
		}
		logRows.Clear();
		foreach (KeyValuePair<(Shader, byte), ProgramStat> item2 in byProgramPass)
		{
			logRows.Add(item2);
		}
		logRows.Sort(delegate(KeyValuePair<(Shader Shader, byte Pass), ProgramStat> a, KeyValuePair<(Shader Shader, byte Pass), ProgramStat> b)
		{
			int num = PassRank(a.Key.Pass).CompareTo(PassRank(b.Key.Pass));
			return (num == 0) ? b.Value.Draws.CompareTo(a.Value.Draws) : num;
		});
		foreach (KeyValuePair<(Shader, byte), ProgramStat> logRow in logRows)
		{
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder stringBuilder5 = stringBuilder2;
			StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(24, 4, stringBuilder2);
			handler.AppendLiteral("\n    ");
			handler.AppendFormatted(logRow.Key.Item1.Name);
			handler.AppendLiteral(" [");
			handler.AppendFormatted(RenderPassRegistry.GetName(logRow.Key.Item2));
			handler.AppendLiteral("]: ");
			handler.AppendFormatted(logRow.Value.Draws);
			handler.AppendLiteral(" draws, ");
			handler.AppendFormatted(logRow.Value.Tris / 1000);
			handler.AppendLiteral("k tris");
			stringBuilder5.Append(ref handler);
		}
		RenderLog.Log.Info(stringBuilder.ToString(), "Log", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\DrawCallCore\\MeshDrawStats.cs", 149);
	}
}
