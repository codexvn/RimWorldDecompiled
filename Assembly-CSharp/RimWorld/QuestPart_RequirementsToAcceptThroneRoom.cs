using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C9A RID: 7322
	public class QuestPart_RequirementsToAcceptThroneRoom : QuestPart_RequirementsToAccept
	{
		// Token: 0x17001916 RID: 6422
		// (get) Token: 0x0600A321 RID: 41761 RVA: 0x002ED9F3 File Offset: 0x002EBBF3
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				yield return new Dialog_InfoCard.Hyperlink(this.forTitle, this.faction, -1);
				yield break;
			}
		}

		// Token: 0x0600A322 RID: 41762 RVA: 0x002EDA04 File Offset: 0x002EBC04
		public override AcceptanceReport CanAccept()
		{
			this.acceptanceReportUnmetRequirements.Clear();
			if (this.forTitle.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
			{
				return true;
			}
			Building_Throne assignedThrone = this.forPawn.ownership.AssignedThrone;
			if (assignedThrone == null)
			{
				return "QuestNoThroneRoom".Translate(this.forPawn.Named("PAWN"));
			}
			foreach (RoomRequirement roomRequirement in this.forTitle.throneRoomRequirements)
			{
				if (!roomRequirement.MetOrDisabled(assignedThrone.GetRoom(RegionType.Set_All), this.forPawn))
				{
					this.acceptanceReportUnmetRequirements.Add(roomRequirement.LabelCap(assignedThrone.GetRoom(RegionType.Set_All)));
				}
			}
			if (this.acceptanceReportUnmetRequirements.Count != 0)
			{
				return new AcceptanceReport("QuestThroneRoomRequirementsUnsatisfied".Translate(this.forPawn.Named("PAWN"), this.forTitle.GetLabelFor(this.forPawn).Named("TITLE")) + ":\n\n" + this.acceptanceReportUnmetRequirements.ToLineList("- "));
			}
			return true;
		}

		// Token: 0x0600A323 RID: 41763 RVA: 0x002EDB50 File Offset: 0x002EBD50
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RoyalTitleDef>(ref this.forTitle, "forTitle");
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.forPawn, "forPawn", false);
		}

		// Token: 0x0600A324 RID: 41764 RVA: 0x002EDB8A File Offset: 0x002EBD8A
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.forPawn == replace)
			{
				this.forPawn = with;
			}
		}

		// Token: 0x040070C0 RID: 28864
		public RoyalTitleDef forTitle;

		// Token: 0x040070C1 RID: 28865
		public Faction faction;

		// Token: 0x040070C2 RID: 28866
		public Pawn forPawn;

		// Token: 0x040070C3 RID: 28867
		private List<string> acceptanceReportUnmetRequirements = new List<string>();
	}
}
