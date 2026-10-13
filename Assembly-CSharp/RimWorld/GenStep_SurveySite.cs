using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02002082 RID: 8322
	public class GenStep_SurveySite : GenStep
	{
		// Token: 0x17001BDB RID: 7131
		// (get) Token: 0x0600B666 RID: 46694 RVA: 0x003468C1 File Offset: 0x00344AC1
		public override int SeedPart
		{
			get
			{
				return 3615199;
			}
		}

		// Token: 0x0600B667 RID: 46695 RVA: 0x003468C8 File Offset: 0x00344AC8
		public override void Generate(Map map, GenStepParams parms)
		{
			GenStep_SurveySite.<>c__DisplayClass2_0 CS$<>8__locals1 = new GenStep_SurveySite.<>c__DisplayClass2_0();
			CS$<>8__locals1.map = map;
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			CellRect cellRect;
			IntVec3 intVec = MapGenUtility.TryGetRandomClearRect(20, 20, out cellRect, -1, -1, new Predicate<CellRect>(CS$<>8__locals1.<Generate>g__RectValidator|0), -1f, 0.7f, MapGenUtility.SearchWeightMode.Center, 1) ? (cellRect.CenterCell + IntVec3.SouthEast) : CS$<>8__locals1.map.Center;
			PrefabDef surveyScanner = PrefabDefOf.SurveyScanner;
			CellRect cellRect2 = intVec.RectAbout(surveyScanner.size);
			MapGenerator.UsedRects.Add(cellRect2.ExpandedBy(8));
			foreach (IntVec3 c in cellRect2.Cells)
			{
				Building edifice = c.GetEdifice(CS$<>8__locals1.map);
				if (edifice != null)
				{
					edifice.Destroy(DestroyMode.Vanish);
				}
			}
			MapGenUtility.DoRectEdgeLumps(CS$<>8__locals1.map, null, ref cellRect2, IntRange.Between(2, 4), IntRange.Between(2, 1), null);
			PrefabUtility.SpawnPrefab(PrefabDefOf.SurveyScanner, CS$<>8__locals1.map, intVec, Rot4.North, null, null, null, new Action<Thing>(GenStep_SurveySite.<Generate>g__OnSpawned|2_1), false);
		}

		// Token: 0x0600B669 RID: 46697 RVA: 0x003469F4 File Offset: 0x00344BF4
		[CompilerGenerated]
		internal static void <Generate>g__OnSpawned|2_1(Thing thing)
		{
			if (thing.def == ThingDefOf.SurveyScanner)
			{
				thing.SetFaction(Find.FactionManager.OfPlayer, null);
			}
		}
	}
}
