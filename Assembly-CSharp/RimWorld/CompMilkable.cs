using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D65 RID: 11621
	public class CompMilkable : CompHasGatherableBodyResource
	{
		// Token: 0x17002984 RID: 10628
		// (get) Token: 0x0601053C RID: 66876 RVA: 0x004B6B07 File Offset: 0x004B4D07
		protected override int GatherResourcesIntervalDays
		{
			get
			{
				return this.Props.milkIntervalDays;
			}
		}

		// Token: 0x17002985 RID: 10629
		// (get) Token: 0x0601053D RID: 66877 RVA: 0x004B6B14 File Offset: 0x004B4D14
		protected override int ResourceAmount
		{
			get
			{
				return this.Props.milkAmount;
			}
		}

		// Token: 0x17002986 RID: 10630
		// (get) Token: 0x0601053E RID: 66878 RVA: 0x004B6B21 File Offset: 0x004B4D21
		protected override ThingDef ResourceDef
		{
			get
			{
				return this.Props.milkDef;
			}
		}

		// Token: 0x17002987 RID: 10631
		// (get) Token: 0x0601053F RID: 66879 RVA: 0x004B6B2E File Offset: 0x004B4D2E
		protected override string SaveKey
		{
			get
			{
				return "milkFullness";
			}
		}

		// Token: 0x17002988 RID: 10632
		// (get) Token: 0x06010540 RID: 66880 RVA: 0x004B6B35 File Offset: 0x004B4D35
		public CompProperties_Milkable Props
		{
			get
			{
				return (CompProperties_Milkable)this.props;
			}
		}

		// Token: 0x17002989 RID: 10633
		// (get) Token: 0x06010541 RID: 66881 RVA: 0x004B6B44 File Offset: 0x004B4D44
		protected override bool Active
		{
			get
			{
				if (!base.Active)
				{
					return false;
				}
				Pawn pawn = this.parent as Pawn;
				return (!this.Props.milkFemaleOnly || pawn == null || pawn.gender == Gender.Female) && (pawn == null || pawn.ageTracker.CurLifeStage.milkable) && (!ModsConfig.AnomalyActive || !pawn.IsShambler);
			}
		}

		// Token: 0x06010542 RID: 66882 RVA: 0x004B6BAB File Offset: 0x004B4DAB
		public override string CompInspectStringExtra()
		{
			if (!this.Active)
			{
				return null;
			}
			return "MilkFullness".Translate() + ": " + base.Fullness.ToStringPercent();
		}
	}
}
