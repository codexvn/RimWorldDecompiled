using System;
using System.Linq;
using System.Text;
using LudeonTK;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000B0E RID: 2830
	public static class DebugOutputsSystem
	{
		// Token: 0x0600499A RID: 18842 RVA: 0x00176B50 File Offset: 0x00174D50
		[DebugOutput("System", false)]
		public static void LoadedAssets()
		{
			StringBuilder stringBuilder = new StringBuilder();
			UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(Mesh));
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"Meshes: ",
				array.Length.ToString(),
				" (",
				DebugOutputsSystem.TotalBytes(array).ToStringBytes("F2"),
				")"
			}));
			UnityEngine.Object[] array2 = Resources.FindObjectsOfTypeAll(typeof(Material));
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"Materials: ",
				array2.Length.ToString(),
				" (",
				DebugOutputsSystem.TotalBytes(array2).ToStringBytes("F2"),
				")"
			}));
			stringBuilder.AppendLine("   Damaged: " + DamagedMatPool.MatCount.ToString());
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"   Faded: ",
				FadedMaterialPool.TotalMaterialCount.ToString(),
				" (",
				FadedMaterialPool.TotalMaterialBytes.ToStringBytes("F2"),
				")"
			}));
			stringBuilder.AppendLine("   SolidColorsSimple: " + SolidColorMaterials.SimpleColorMatCount.ToString());
			UnityEngine.Object[] array3 = Resources.FindObjectsOfTypeAll(typeof(Texture));
			stringBuilder.AppendLine(string.Concat(new string[]
			{
				"Textures: ",
				array3.Length.ToString(),
				" (",
				DebugOutputsSystem.TotalBytes(array3).ToStringBytes("F2"),
				")"
			}));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Texture list:");
			UnityEngine.Object[] array4 = array3;
			for (int i = 0; i < array4.Length; i++)
			{
				string text = ((Texture)array4[i]).name;
				if (text.NullOrEmpty())
				{
					text = "-";
				}
				stringBuilder.AppendLine(text);
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600499B RID: 18843 RVA: 0x00176D5C File Offset: 0x00174F5C
		private static long TotalBytes(UnityEngine.Object[] arr)
		{
			long num = 0L;
			foreach (UnityEngine.Object o in arr)
			{
				num += Profiler.GetRuntimeMemorySizeLong(o);
			}
			return num;
		}

		// Token: 0x0600499C RID: 18844 RVA: 0x00176D8A File Offset: 0x00174F8A
		[DebugOutput("System", true)]
		public static void DynamicDrawThingsList()
		{
			Find.CurrentMap.dynamicDrawManager.LogDynamicDrawThings();
		}

		// Token: 0x0600499D RID: 18845 RVA: 0x00176D9C File Offset: 0x00174F9C
		[DebugOutput("System", false)]
		public static void RandByCurveTests()
		{
			DebugHistogram debugHistogram = new DebugHistogram((from x in Enumerable.Range(0, 30)
			select (float)x).ToArray<float>());
			SimpleCurve curve = new SimpleCurve
			{
				{
					new CurvePoint(0f, 0f),
					true
				},
				{
					new CurvePoint(10f, 1f),
					true
				},
				{
					new CurvePoint(15f, 2f),
					true
				},
				{
					new CurvePoint(20f, 2f),
					true
				},
				{
					new CurvePoint(21f, 0.5f),
					true
				},
				{
					new CurvePoint(30f, 0f),
					true
				}
			};
			float num = 0f;
			for (int i = 0; i < 1000000; i++)
			{
				float num2 = Rand.ByCurve(curve);
				num += num2;
				debugHistogram.Add(num2);
			}
			debugHistogram.Display();
			Log.Message(string.Format("Average {0}, calculated as {1}", num / 1000000f, Rand.ByCurveAverage(curve)));
		}
	}
}
