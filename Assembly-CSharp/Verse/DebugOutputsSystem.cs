using System;
using System.Linq;
using System.Text;
using LudeonTK;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse;

public static class DebugOutputsSystem
{
	[DebugOutput("System", false)]
	public static void LoadedAssets()
	{
		StringBuilder stringBuilder = new StringBuilder();
		Mesh[] array = Resources.FindObjectsOfTypeAll<Mesh>();
		string[] obj = new string[5]
		{
			"Meshes: ",
			array.Length.ToString(),
			" (",
			null,
			null
		};
		UnityEngine.Object[] arr = array;
		obj[3] = TotalBytes(arr).ToStringBytes();
		obj[4] = ")";
		stringBuilder.AppendLine(string.Concat(obj));
		Material[] array2 = Resources.FindObjectsOfTypeAll<Material>();
		string[] obj2 = new string[5]
		{
			"Materials: ",
			array2.Length.ToString(),
			" (",
			null,
			null
		};
		arr = array2;
		obj2[3] = TotalBytes(arr).ToStringBytes();
		obj2[4] = ")";
		stringBuilder.AppendLine(string.Concat(obj2));
		stringBuilder.AppendLine("   Damaged: " + DamagedMatPool.MatCount);
		stringBuilder.AppendLine("   Faded: " + FadedMaterialPool.TotalMaterialCount + " (" + FadedMaterialPool.TotalMaterialBytes.ToStringBytes() + ")");
		stringBuilder.AppendLine("   SolidColorsSimple: " + SolidColorMaterials.SimpleColorMatCount);
		Texture[] array3 = Resources.FindObjectsOfTypeAll<Texture>();
		string[] obj3 = new string[5]
		{
			"Textures: ",
			array3.Length.ToString(),
			" (",
			null,
			null
		};
		arr = array3;
		obj3[3] = TotalBytes(arr).ToStringBytes();
		obj3[4] = ")";
		stringBuilder.AppendLine(string.Concat(obj3));
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("Texture list:");
		Texture[] array4 = array3;
		for (int i = 0; i < array4.Length; i++)
		{
			string text = array4[i].name;
			if (text.NullOrEmpty())
			{
				text = "-";
			}
			stringBuilder.AppendLine(text);
		}
		Log.Message(stringBuilder.ToString());
	}

	private static long TotalBytes(UnityEngine.Object[] arr)
	{
		long num = 0L;
		foreach (UnityEngine.Object o in arr)
		{
			num += Profiler.GetRuntimeMemorySizeLong(o);
		}
		return num;
	}

	[DebugOutput("System", true)]
	public static void DynamicDrawThingsList()
	{
		Find.CurrentMap.dynamicDrawManager.LogDynamicDrawThings();
	}

	[DebugOutput("System", false)]
	public static void RandByCurveTests()
	{
		DebugHistogram debugHistogram = new DebugHistogram(Enumerable.Range(0, 30).Select((Func<int, float>)((int x) => x)).ToArray());
		SimpleCurve curve = new SimpleCurve
		{
			new CurvePoint(0f, 0f),
			new CurvePoint(10f, 1f),
			new CurvePoint(15f, 2f),
			new CurvePoint(20f, 2f),
			new CurvePoint(21f, 0.5f),
			new CurvePoint(30f, 0f)
		};
		float num = 0f;
		for (int i = 0; i < 1000000; i++)
		{
			float num2 = Rand.ByCurve(curve);
			num += num2;
			debugHistogram.Add(num2);
		}
		debugHistogram.Display();
		Log.Message($"Average {num / 1000000f}, calculated as {Rand.ByCurveAverage(curve)}");
	}
}
