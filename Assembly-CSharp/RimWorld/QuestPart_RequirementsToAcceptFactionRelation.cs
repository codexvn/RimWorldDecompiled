using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C8C RID: 7308
	public class QuestPart_RequirementsToAcceptFactionRelation : QuestPart_RequirementsToAccept
	{
		// Token: 0x17001906 RID: 6406
		// (get) Token: 0x0600A2D9 RID: 41689 RVA: 0x002ECDF4 File Offset: 0x002EAFF4
		public string ReasonText
		{
			get
			{
				switch (this.relationKind)
				{
				case FactionRelationKind.Hostile:
					return "QuestHostileTo".Translate(this.otherFaction);
				case FactionRelationKind.Neutral:
					return "QuestNeutralTo".Translate(this.otherFaction);
				case FactionRelationKind.Ally:
					return "QuestAlliedTo".Translate(this.otherFaction);
				default:
					throw new Exception(string.Format("Unknown faction relation kind: {0}", this.relationKind));
				}
			}
		}

		// Token: 0x0600A2DA RID: 41690 RVA: 0x002ECE88 File Offset: 0x002EB088
		public override AcceptanceReport CanAccept()
		{
			if (this.otherFaction != null && ((this.acceptIfDefeated && this.otherFaction.defeated) || Faction.OfPlayer.RelationKindWith(this.otherFaction) == this.relationKind))
			{
				return true;
			}
			return new AcceptanceReport(this.ReasonText);
		}

		// Token: 0x0600A2DB RID: 41691 RVA: 0x002ECEDC File Offset: 0x002EB0DC
		public override void Notify_FactionRemoved(Faction faction)
		{
			base.Notify_FactionRemoved(faction);
			if (this.otherFaction == faction)
			{
				this.otherFaction = null;
			}
		}

		// Token: 0x0600A2DC RID: 41692 RVA: 0x002ECEF5 File Offset: 0x002EB0F5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<FactionRelationKind>(ref this.relationKind, "relationKind", FactionRelationKind.Hostile, false);
			Scribe_References.Look<Faction>(ref this.otherFaction, "otherFaction", false);
			Scribe_Values.Look<bool>(ref this.acceptIfDefeated, "acceptIfDefeated", false, false);
		}

		// Token: 0x0400709D RID: 28829
		public Faction otherFaction;

		// Token: 0x0400709E RID: 28830
		public FactionRelationKind relationKind;

		// Token: 0x0400709F RID: 28831
		public bool acceptIfDefeated;
	}
}
