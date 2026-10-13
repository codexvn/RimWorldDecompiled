using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200253B RID: 9531
	public class Thought_IdeoDisrespectedBuilding : Thought_Situational
	{
		// Token: 0x17002007 RID: 8199
		// (get) Token: 0x0600D086 RID: 53382 RVA: 0x003D54E4 File Offset: 0x003D36E4
		public override string LabelCap
		{
			get
			{
				return base.CurStage.LabelCap.Formatted(this.demand.parent.Named("BUILDING"));
			}
		}

		// Token: 0x17002008 RID: 8200
		// (get) Token: 0x0600D087 RID: 53383 RVA: 0x003D5510 File Offset: 0x003D3710
		public override string Description
		{
			get
			{
				return base.CurStage.description.Formatted(this.demand.parent.Named("BUILDING"));
			}
		}

		// Token: 0x0600D088 RID: 53384 RVA: 0x003D553C File Offset: 0x003D373C
		protected override ThoughtState CurrentStateInternal()
		{
			if (!Faction.OfPlayer.ideos.Has(this.demand.parent.ideo))
			{
				return ThoughtState.Inactive;
			}
			Map mapHeld = this.pawn.MapHeld;
			return mapHeld != null && this.demand.AppliesTo(mapHeld) && this.demand.BuildingPresent(mapHeld) && !this.demand.RequirementsSatisfied(mapHeld);
		}

		// Token: 0x0600D089 RID: 53385 RVA: 0x003D55B4 File Offset: 0x003D37B4
		public override bool GroupsWith(Thought other)
		{
			Thought_IdeoDisrespectedBuilding thought_IdeoDisrespectedBuilding = other as Thought_IdeoDisrespectedBuilding;
			return thought_IdeoDisrespectedBuilding != null && thought_IdeoDisrespectedBuilding.demand == this.demand && !this.pawn.IsQuestLodger();
		}

		// Token: 0x0600D08A RID: 53386 RVA: 0x003D55E9 File Offset: 0x003D37E9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<IdeoBuildingPresenceDemand>(ref this.demand, "demand", false);
		}

		// Token: 0x04008DD3 RID: 36307
		public IdeoBuildingPresenceDemand demand;
	}
}
