using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200203D RID: 8253
	public class GenStep_AncientRuins : GenStep_BaseRuins
	{
		// Token: 0x17001B92 RID: 7058
		// (get) Token: 0x0600B551 RID: 46417 RVA: 0x00342311 File Offset: 0x00340511
		public override int SeedPart
		{
			get
			{
				return 964521;
			}
		}

		// Token: 0x17001B93 RID: 7059
		// (get) Token: 0x0600B552 RID: 46418 RVA: 0x00342318 File Offset: 0x00340518
		protected override LayoutDef LayoutDef
		{
			get
			{
				return this.layoutDef;
			}
		}

		// Token: 0x17001B94 RID: 7060
		// (get) Token: 0x0600B553 RID: 46419 RVA: 0x003331E9 File Offset: 0x003313E9
		protected override int RegionSize
		{
			get
			{
				return 45;
			}
		}

		// Token: 0x17001B95 RID: 7061
		// (get) Token: 0x0600B554 RID: 46420 RVA: 0x003331ED File Offset: 0x003313ED
		protected override FloatRange DefaultMapFillPercentRange
		{
			get
			{
				return new FloatRange(0.15f, 0.3f);
			}
		}

		// Token: 0x17001B96 RID: 7062
		// (get) Token: 0x0600B555 RID: 46421 RVA: 0x003331FE File Offset: 0x003313FE
		protected override FloatRange MergeRange
		{
			get
			{
				return new FloatRange(0.1f, 0.35f);
			}
		}

		// Token: 0x17001B97 RID: 7063
		// (get) Token: 0x0600B556 RID: 46422 RVA: 0x00075162 File Offset: 0x00073362
		protected override int MoveRangeLimit
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001B98 RID: 7064
		// (get) Token: 0x0600B557 RID: 46423 RVA: 0x00075162 File Offset: 0x00073362
		protected override int ContractLimit
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17001B99 RID: 7065
		// (get) Token: 0x0600B558 RID: 46424 RVA: 0x002A9950 File Offset: 0x002A7B50
		protected override int MinRegionSize
		{
			get
			{
				return 14;
			}
		}

		// Token: 0x17001B9A RID: 7066
		// (get) Token: 0x0600B559 RID: 46425 RVA: 0x00342320 File Offset: 0x00340520
		protected override Faction Faction
		{
			get
			{
				return Faction.OfAncientsHostile;
			}
		}

		// Token: 0x0600B55A RID: 46426 RVA: 0x00342328 File Offset: 0x00340528
		public override void GenerateRuins(Map map, GenStepParams parms, FloatRange mapFillPercentRange)
		{
			if (!ModLister.CheckOdyssey("Ancient Ruins"))
			{
				return;
			}
			base.GenerateRuins(map, parms, mapFillPercentRange);
			MapGenUtility.SpawnExteriorLumps(map, ThingDefOf.RubblePile, GenStep_AncientRuins.RubblePilesPer10K, GenStep_AncientRuins.RubblePileCountRange, GenStep_AncientRuins.RubblePileDistanceRange, 6);
			MapGenUtility.SpawnScatter(map, ThingDefOf.Filth_BlastMark, GenStep_AncientRuins.BlastMarksPer10K);
		}

		// Token: 0x04007C85 RID: 31877
		private LayoutDef layoutDef;

		// Token: 0x04007C86 RID: 31878
		private static readonly FloatRange BlastMarksPer10K = new FloatRange(2f, 6f);

		// Token: 0x04007C87 RID: 31879
		private static readonly FloatRange RubblePilesPer10K = new FloatRange(4f, 10f);

		// Token: 0x04007C88 RID: 31880
		private static readonly IntRange RubblePileCountRange = new IntRange(3, 8);

		// Token: 0x04007C89 RID: 31881
		private static readonly IntRange RubblePileDistanceRange = new IntRange(3, 8);
	}
}
