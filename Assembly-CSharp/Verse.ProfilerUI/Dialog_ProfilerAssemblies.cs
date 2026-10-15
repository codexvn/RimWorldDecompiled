using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Verse.ProfilerUI;

public sealed class Dialog_ProfilerAssemblies : Window
{
	private sealed class Row
	{
		public string group;

		public string assembly;

		public List<string> patchLines;
	}

	private const float RowHeight = 24f;

	private const float PatchesWidth = 110f;

	private static readonly char[] LineSeparators = new char[2] { '\n', '\r' };

	private readonly Func<string> getDraft;

	private readonly Action<string> setDraft;

	private readonly List<Row> rows = new List<Row>();

	private readonly HashSet<string> lines = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

	private string linesSource;

	private string search = "";

	private Vector2 scroll;

	public override Vector2 InitialSize => new Vector2(560f, 640f);

	public Dialog_ProfilerAssemblies(Func<string> getDraft, Action<string> setDraft)
	{
		this.getDraft = getDraft;
		this.setDraft = setDraft;
		layer = WindowLayer.Super;
		doCloseX = true;
		draggable = true;
		closeOnClickedOutside = false;
		absorbInputAroundWindow = false;
		preventCameraMotion = false;
		optionalTitle = "Instrumented assemblies";
		BuildRows();
	}

	private void BuildRows()
	{
		Dictionary<Assembly, List<string>> patches = HarmonyPatchLines();
		HashSet<Assembly> seen = new HashSet<Assembly>();
		AddRow("Game", typeof(Root).Assembly, patches, seen);
		foreach (ModContentPack item in LoadedModManager.RunningModsListForReading)
		{
			foreach (Assembly loadedAssembly in item.assemblies.loadedAssemblies)
			{
				AddRow(item.Name, loadedAssembly, patches, seen);
			}
		}
	}

	private void AddRow(string group, Assembly assembly, Dictionary<Assembly, List<string>> patches, HashSet<Assembly> seen)
	{
		if (!assembly.IsDynamic && !assembly.IsCollectible && seen.Add(assembly))
		{
			string name = assembly.GetName().Name;
			if (name != null)
			{
				patches.TryGetValue(assembly, out var value);
				rows.Add(new Row
				{
					group = group,
					assembly = name,
					patchLines = value
				});
			}
		}
	}

	private static Dictionary<Assembly, List<string>> HarmonyPatchLines()
	{
		Dictionary<Assembly, List<string>> dictionary = new Dictionary<Assembly, List<string>>();
		HashSet<MethodBase> hashSet = new HashSet<MethodBase>();
		try
		{
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly("HarmonyLib.Harmony");
			MethodInfo methodInfo = typeInAnyAssembly?.GetMethod("GetAllPatchedMethods", BindingFlags.Static | BindingFlags.Public);
			MethodInfo methodInfo2 = typeInAnyAssembly?.GetMethod("GetPatchInfo", BindingFlags.Static | BindingFlags.Public);
			if (methodInfo == null || methodInfo2 == null)
			{
				return dictionary;
			}
			foreach (MethodBase item2 in (IEnumerable)methodInfo.Invoke(null, null))
			{
				object obj = methodInfo2.Invoke(null, new object[1] { item2 });
				if (obj == null)
				{
					continue;
				}
				string[] array = new string[3] { "Prefixes", "Postfixes", "Finalizers" };
				foreach (string name in array)
				{
					if (!(Member(obj, name) is IEnumerable enumerable))
					{
						continue;
					}
					foreach (object item3 in enumerable)
					{
						if (!(Member(item3, "PatchMethod") is MethodInfo methodInfo3))
						{
							continue;
						}
						string text = methodInfo3.DeclaringType?.FullName;
						if (text != null && hashSet.Add(methodInfo3))
						{
							Assembly assembly = methodInfo3.DeclaringType.Assembly;
							if (!dictionary.TryGetValue(assembly, out var value))
							{
								value = (dictionary[assembly] = new List<string>());
							}
							string item = assembly.GetName().Name + "!" + text + "::" + methodInfo3.Name;
							if (!value.Contains(item))
							{
								value.Add(item);
							}
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			Log.Warning("Could not list harmony patches for the profiler: " + ex.Message);
		}
		return dictionary;
	}

	private static object Member(object target, string name)
	{
		Type type = target.GetType();
		object obj = type.GetField(name, BindingFlags.Instance | BindingFlags.Public)?.GetValue(target);
		if (obj == null)
		{
			PropertyInfo? property = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
			if ((object)property == null)
			{
				return null;
			}
			obj = property.GetValue(target);
		}
		return obj;
	}

	private void SyncLines()
	{
		string text = getDraft();
		if (!(text == linesSource))
		{
			linesSource = text;
			lines.Clear();
			string[] array = text.Split(LineSeparators, StringSplitOptions.RemoveEmptyEntries);
			foreach (string text2 in array)
			{
				lines.Add(text2.Trim());
			}
		}
	}

	private void SetLines(IEnumerable<string> toggled, bool on)
	{
		List<string> list = new List<string>();
		string[] array = getDraft().Split(LineSeparators, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			string text = array[i].Trim();
			if (text.Length > 0)
			{
				list.Add(text);
			}
		}
		foreach (string line in toggled)
		{
			list.RemoveAll((string existing) => string.Equals(existing, line, StringComparison.OrdinalIgnoreCase));
			if (on)
			{
				list.Add(line);
			}
		}
		setDraft(string.Join("\n", list));
	}

	private bool AllPresent(List<string> wanted)
	{
		foreach (string item in wanted)
		{
			if (!lines.Contains(item))
			{
				return false;
			}
		}
		return true;
	}

	public override void DoWindowContents(Rect inRect)
	{
		SyncLines();
		Text.Font = GameFont.Small;
		search = Widgets.TextField(new Rect(inRect.x, inRect.y, inRect.width, 28f), search);
		List<Row> list = new List<Row>();
		foreach (Row row in rows)
		{
			if (search.Length == 0 || row.assembly.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 || row.group.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				list.Add(row);
			}
		}
		float num = 0f;
		string text = null;
		foreach (Row item in list)
		{
			if (item.group != text)
			{
				num += 24f;
			}
			text = item.group;
			num += 24f;
		}
		Rect outRect = new Rect(inRect.x, inRect.y + 34f, inRect.width, inRect.height - 34f);
		Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, num);
		Widgets.BeginScrollView(outRect, ref scroll, viewRect);
		float num2 = 0f;
		text = null;
		foreach (Row item2 in list)
		{
			if (item2.group != text)
			{
				GUI.color = Color.gray;
				Widgets.Label(new Rect(0f, num2 + 2f, viewRect.width, 24f), item2.group);
				GUI.color = Color.white;
				num2 += 24f;
				text = item2.group;
			}
			if (num2 + 24f >= scroll.y && num2 <= scroll.y + outRect.height)
			{
				DrawRow(new Rect(12f, num2, viewRect.width - 12f, 24f), item2);
			}
			num2 += 24f;
		}
		Widgets.EndScrollView();
	}

	private void DrawRow(Rect rect, Row row)
	{
		string text = row.assembly + "!*";
		bool checkOn = lines.Contains(text);
		bool flag = checkOn;
		bool flag2 = row.patchLines != null;
		Widgets.CheckboxLabeled(new Rect(rect.x, rect.y, rect.width - (flag2 ? 118f : 0f), rect.height), row.assembly, ref checkOn);
		if (checkOn != flag)
		{
			SetLines(new string[1] { text }, checkOn);
		}
		if (flag2)
		{
			bool checkOn2 = AllPresent(row.patchLines);
			bool flag3 = checkOn2;
			Widgets.CheckboxLabeled(new Rect(rect.xMax - 110f, rect.y, 110f, rect.height), row.patchLines.Count + " patches", ref checkOn2);
			if (checkOn2 != flag3)
			{
				SetLines(row.patchLines, checkOn2);
			}
		}
	}
}
