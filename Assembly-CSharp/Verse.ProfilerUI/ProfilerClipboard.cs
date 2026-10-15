using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse.ProfilerUI;

internal static class ProfilerClipboard
{
	public const string CallTreeHeader = "%\ttotal\tself\tcalls\talloc\tscope";

	public const string PieHeader = "%\ttotal\tself\tcalls\talloc\tzone";

	public static void Put(StringBuilder builder)
	{
		GUIUtility.systemCopyBuffer = AlignColumns(builder.ToString());
	}

	public static string AlignColumns(string text)
	{
		string[] array = text.Replace("\r\n", "\n").Split('\n');
		List<int> list = new List<int>();
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			ReadOnlySpan<char> span = array2[i].AsSpan();
			int num = 0;
			int num2 = span.IndexOf('\t');
			while (num2 >= 0)
			{
				if (num >= list.Count)
				{
					list.Add(0);
				}
				list[num] = Math.Max(list[num], num2);
				span = span.Slice(num2 + 1);
				num2 = span.IndexOf('\t');
				num++;
			}
		}
		if (list.Count == 0)
		{
			return text;
		}
		StringBuilder stringBuilder = new StringBuilder(text.Length + text.Length / 2);
		bool flag = true;
		array2 = array;
		foreach (string text2 in array2)
		{
			if (!flag)
			{
				stringBuilder.Append('\n');
			}
			flag = false;
			ReadOnlySpan<char> readOnlySpan = text2.AsSpan();
			int num3 = 0;
			int num4 = readOnlySpan.IndexOf('\t');
			while (num4 >= 0)
			{
				if (num3 > 0)
				{
					stringBuilder.Append("  ");
				}
				stringBuilder.Append(' ', list[num3] - num4);
				stringBuilder.Append(readOnlySpan.Slice(0, num4));
				readOnlySpan = readOnlySpan.Slice(num4 + 1);
				num4 = readOnlySpan.IndexOf('\t');
				num3++;
			}
			if (num3 > 0)
			{
				stringBuilder.Append("  ");
			}
			stringBuilder.Append(readOnlySpan);
		}
		return stringBuilder.ToString();
	}

	public static bool AppendRow(StringBuilder builder, IList<ProfilerRowData> rows, int index)
	{
		if (index < 0 || index >= rows.Count)
		{
			return false;
		}
		rows[index].AppendCopyLine(builder);
		return true;
	}

	public static void AppendSubtree(StringBuilder builder, IList<ProfilerRowData> rows, int index)
	{
		int depth = rows[index].Depth;
		for (int i = index; i < rows.Count && (i == index || rows[i].Depth > depth); i++)
		{
			rows[i].AppendCopyLine(builder, rows[i].Depth - depth);
			builder.AppendLine();
		}
	}

	public static void AppendRows(StringBuilder builder, IList<ProfilerRowData> rows)
	{
		for (int i = 0; i < rows.Count; i++)
		{
			rows[i].AppendCopyLine(builder);
			builder.AppendLine();
		}
	}
}
