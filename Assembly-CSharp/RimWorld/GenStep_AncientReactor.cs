using System;
using RimWorld.BaseGen;
using Verse;

namespace RimWorld
{
	// Token: 0x0200204F RID: 8271
	public class GenStep_AncientReactor : GenStep_LargeRuins
	{
		// Token: 0x17001BB4 RID: 7092
		// (get) Token: 0x0600B5AC RID: 46508 RVA: 0x00343676 File Offset: 0x00341876
		public override int SeedPart
		{
			get
			{
				return 79884699;
			}
		}

		// Token: 0x17001BB5 RID: 7093
		// (get) Token: 0x0600B5AD RID: 46509 RVA: 0x003331E9 File Offset: 0x003313E9
		protected override int RegionSize
		{
			get
			{
				return 45;
			}
		}

		// Token: 0x17001BB6 RID: 7094
		// (get) Token: 0x0600B5AE RID: 46510 RVA: 0x0034367D File Offset: 0x0034187D
		protected override FloatRange DefaultMapFillPercentRange
		{
			get
			{
				return new FloatRange(0.6f, 0.75f);
			}
		}

		// Token: 0x17001BB7 RID: 7095
		// (get) Token: 0x0600B5AF RID: 46511 RVA: 0x0001B99B File Offset: 0x00019B9B
		protected override FloatRange MergeRange
		{
			get
			{
				return new FloatRange(1f, 1f);
			}
		}

		// Token: 0x17001BB8 RID: 7096
		// (get) Token: 0x0600B5B0 RID: 46512 RVA: 0x00123991 File Offset: 0x00121B91
		protected override int MoveRangeLimit
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17001BB9 RID: 7097
		// (get) Token: 0x0600B5B1 RID: 46513 RVA: 0x00123991 File Offset: 0x00121B91
		protected override int ContractLimit
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17001BBA RID: 7098
		// (get) Token: 0x0600B5B2 RID: 46514 RVA: 0x001304E5 File Offset: 0x0012E6E5
		protected override int MinRegionSize
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x17001BBB RID: 7099
		// (get) Token: 0x0600B5B3 RID: 46515 RVA: 0x0034368E File Offset: 0x0034188E
		protected override IntRange RuinsMinMaxRange
		{
			get
			{
				return new IntRange(2, 6);
			}
		}

		// Token: 0x17001BBC RID: 7100
		// (get) Token: 0x0600B5B4 RID: 46516 RVA: 0x00343697 File Offset: 0x00341897
		protected override LayoutDef LayoutDef
		{
			get
			{
				return LayoutDefOf.AncientRuinsReactor_Standard;
			}
		}

		// Token: 0x17001BBD RID: 7101
		// (get) Token: 0x0600B5B5 RID: 46517 RVA: 0x00342320 File Offset: 0x00340520
		protected override Faction Faction
		{
			get
			{
				return Faction.OfAncientsHostile;
			}
		}

		// Token: 0x0600B5B6 RID: 46518 RVA: 0x0034369E File Offset: 0x0034189E
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckOdyssey("Ancient Reactor"))
			{
				return;
			}
			this.placedReactorLayout = false;
			base.Generate(map, parms);
		}

		// Token: 0x0600B5B7 RID: 46519 RVA: 0x003436BC File Offset: 0x003418BC
		protected override LayoutStructureSketch GenerateAndSpawn(CellRect rect, Map map, GenStepParams parms, LayoutDef layoutDef)
		{
			if (!this.placedReactorLayout)
			{
				this.placedReactorLayout = true;
				layoutDef = LayoutDefOf.AncientRuinsReactor_Reactor;
				MapGenerator.SetVar<CellRect>("SpawnRect", rect.ExpandedBy(1));
			}
			return base.GenerateAndSpawn(rect, map, parms, layoutDef);
		}

		// Token: 0x0600B5B8 RID: 46520 RVA: 0x003436F1 File Offset: 0x003418F1
		public override void PostMapInitialized(Map map, GenStepParams parms)
		{
			BaseGenUtility.ScatterSentryDronesInMap(GenStep_AncientReactor.SentryCountFromPointsCurve, map, Faction.OfAncientsHostile, parms);
		}

		// Token: 0x04007CAC RID: 31916
		private bool placedReactorLayout;

		// Token: 0x04007CAD RID: 31917
		private static readonly SimpleCurve SentryCountFromPointsCurve = new SimpleCurve(new CurvePoint[]
		{
			new CurvePoint(0f, 0f),
			new CurvePoint(100f, 2f),
			new CurvePoint(1000f, 10f),
			new CurvePoint(5000f, 20f)
		});
	}
}
