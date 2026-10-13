using System;
using System.Collections.Generic;
using RimWorld.SketchGen;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A58 RID: 14936
	public class QuestNode_GetMonumentSketch : QuestNode
	{
		// Token: 0x060146D0 RID: 83664 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x060146D1 RID: 83665 RVA: 0x00619CCA File Offset: 0x00617ECA
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x060146D2 RID: 83666 RVA: 0x00619CD8 File Offset: 0x00617ED8
		private bool DoWork(Slate slate)
		{
			float num = slate.Get<float>("points", 0f, false);
			float value = this.pointsPerArea.GetValue(slate);
			float num2 = Mathf.Min(num / value, 2500f);
			float randomInRange = QuestNode_GetMonumentSketch.RandomAspectRatioRange.RandomInRange;
			float f = Mathf.Sqrt(randomInRange * num2);
			float f2 = Mathf.Sqrt(num2 / randomInRange);
			int num3 = GenMath.RoundRandom(f);
			int num4 = GenMath.RoundRandom(f2);
			if (Rand.Bool)
			{
				int num5 = num3;
				num3 = num4;
				num4 = num5;
			}
			int? value2 = this.maxSize.GetValue(slate);
			if (value2 != null)
			{
				num3 = Mathf.Min(num3, value2.Value);
				num4 = Mathf.Min(num4, value2.Value);
			}
			num3 = Mathf.Max(num3, 3);
			num4 = Mathf.Max(num4, 3);
			IntVec2 value3 = new IntVec2(num3, num4);
			SketchResolveParams sketchResolveParams = default(SketchResolveParams);
			sketchResolveParams.sketch = new Sketch();
			sketchResolveParams.monumentSize = new IntVec2?(value3);
			sketchResolveParams.useOnlyStonesAvailableOnMap = this.useOnlyResourcesAvailableOnMap.GetValue(slate);
			sketchResolveParams.onlyBuildableByPlayer = new bool?(true);
			if (this.useOnlyResourcesAvailableOnMap.GetValue(slate) != null)
			{
				sketchResolveParams.allowWood = new bool?(this.useOnlyResourcesAvailableOnMap.GetValue(slate).Biome.TreeDensity >= BiomeDefOf.BorealForest.TreeDensity);
			}
			sketchResolveParams.allowedMonumentThings = new ThingFilter();
			sketchResolveParams.allowedMonumentThings.SetAllowAll(null, true);
			sketchResolveParams.allowedMonumentThings.SetAllow(ThingDefOf.Urn, false);
			Sketch sketch = SketchGen.Generate(SketchResolverDefOf.Monument, sketchResolveParams);
			if (this.clearStuff.GetValue(slate) ?? true)
			{
				List<SketchThing> things = sketch.Things;
				for (int i = 0; i < things.Count; i++)
				{
					things[i].stuff = null;
				}
				List<SketchTerrain> terrain = sketch.Terrain;
				for (int j = 0; j < terrain.Count; j++)
				{
					terrain[j].treatSimilarAsSame = true;
				}
			}
			slate.Set<Sketch>(this.storeAs.GetValue(slate), sketch, false);
			return true;
		}

		// Token: 0x0400E1D4 RID: 57812
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E1D5 RID: 57813
		public SlateRef<Map> useOnlyResourcesAvailableOnMap;

		// Token: 0x0400E1D6 RID: 57814
		public SlateRef<int?> maxSize;

		// Token: 0x0400E1D7 RID: 57815
		public SlateRef<float> pointsPerArea;

		// Token: 0x0400E1D8 RID: 57816
		public SlateRef<bool?> clearStuff;

		// Token: 0x0400E1D9 RID: 57817
		private static readonly FloatRange RandomAspectRatioRange = new FloatRange(1f, 3f);

		// Token: 0x0400E1DA RID: 57818
		private const int MinEdgeLength = 3;

		// Token: 0x0400E1DB RID: 57819
		private const int MaxArea = 2500;
	}
}
