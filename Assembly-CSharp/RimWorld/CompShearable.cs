using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E06 RID: 11782
	public class CompShearable : CompHasGatherableBodyResource
	{
		// Token: 0x17002A61 RID: 10849
		// (get) Token: 0x06010947 RID: 67911 RVA: 0x004C75CB File Offset: 0x004C57CB
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.shearIntervalDays;
			}
		}

		// Token: 0x17002A62 RID: 10850
		// (get) Token: 0x06010948 RID: 67912 RVA: 0x004C75D8 File Offset: 0x004C57D8
		protected override int ResourceAmount
		{
			get
			{
				return this.Props.woolAmount;
			}
		}

		// Token: 0x17002A63 RID: 10851
		// (get) Token: 0x06010949 RID: 67913 RVA: 0x004C75E5 File Offset: 0x004C57E5
		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.woolDef;
			}
		}

		// Token: 0x17002A64 RID: 10852
		// (get) Token: 0x0601094A RID: 67914 RVA: 0x004C75F2 File Offset: 0x004C57F2
		protected override string SaveKey
		{
			get
			{
				return "woolGrowth";
			}
		}

		// Token: 0x17002A65 RID: 10853
		// (get) Token: 0x0601094B RID: 67915 RVA: 0x004C75F9 File Offset: 0x004C57F9
		public CompProperties_Shearable Props
		{
			get
			{
				return (CompProperties_Shearable)this.props;
			}
		}

		// Token: 0x17002A66 RID: 10854
		// (get) Token: 0x0601094C RID: 67916 RVA: 0x004C7608 File Offset: 0x004C5808
		protected override bool Active
		{
			get
			{
				if (!base.Active)
				{
					return false;
				}
				Pawn pawn = this.parent as Pawn;
				return (pawn == null || pawn.ageTracker.CurLifeStage.shearable) && (!ModsConfig.AnomalyActive || !pawn.IsShambler);
			}
		}

		// Token: 0x0601094D RID: 67917 RVA: 0x004C7654 File Offset: 0x004C5854
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			return "WoolGrowth".Translate() + ": " + base.Fullness.ToStringPercent();
		}
	}
}
