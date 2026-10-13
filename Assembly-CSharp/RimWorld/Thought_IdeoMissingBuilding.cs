using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200253A RID: 9530
	public class Thought_IdeoMissingBuilding : Thought_Situational
	{
		// Token: 0x17002005 RID: 8197
		// (get) Token: 0x0600D080 RID: 53376 RVA: 0x003D5381 File Offset: 0x003D3581
		public override string LabelCap
		{
			get
			{
				return base.CurStage.LabelCap.Formatted(this.demand.parent.Named("BUILDING"));
			}
		}

		// Token: 0x17002006 RID: 8198
		// (get) Token: 0x0600D081 RID: 53377 RVA: 0x003D53AD File Offset: 0x003D35AD
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.demand.parent.Named("BUILDING"));
			}
		}

		// Token: 0x0600D082 RID: 53378 RVA: 0x003D53DC File Offset: 0x003D35DC
		protected override ThoughtState CurrentStateInternal()
		{
			if (this.pawn.IsSlave)
			{
				return ThoughtState.Inactive;
			}
			if (!Faction.OfPlayer.ideos.Has(this.demand.parent.ideo))
			{
				return ThoughtState.Inactive;
			}
			if (!Faction.OfPlayer.ideos.IsPrimary(this.demand.parent.ideo) && this.demand.parent.ideo.ColonistBelieverCountCached < 3)
			{
				return ThoughtState.Inactive;
			}
			Map mapHeld = this.pawn.MapHeld;
			return mapHeld != null && this.demand.AppliesTo(mapHeld) && !this.demand.BuildingPresent(mapHeld) && !this.pawn.IsQuestLodger();
		}

		// Token: 0x0600D083 RID: 53379 RVA: 0x003D54A4 File Offset: 0x003D36A4
		public override bool GroupsWith(Thought other)
		{
			Thought_IdeoMissingBuilding thought_IdeoMissingBuilding = other as Thought_IdeoMissingBuilding;
			return thought_IdeoMissingBuilding != null && thought_IdeoMissingBuilding.demand == this.demand;
		}

		// Token: 0x0600D084 RID: 53380 RVA: 0x003D54CB File Offset: 0x003D36CB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<IdeoBuildingPresenceDemand>(ref this.demand, "demand", false);
		}

		// Token: 0x04008DD1 RID: 36305
		public IdeoBuildingPresenceDemand demand;

		// Token: 0x04008DD2 RID: 36306
		public const int MinBelieversPresent = 3;
	}
}
