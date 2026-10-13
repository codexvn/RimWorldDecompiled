using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C8A RID: 7306
	public class QuestPart_RequirementsToAcceptColonistWithTitle : QuestPart_RequirementsToAccept
	{
		// Token: 0x17001902 RID: 6402
		// (get) Token: 0x0600A2CB RID: 41675 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool RequiresAccepter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600A2CC RID: 41676 RVA: 0x002ECBF8 File Offset: 0x002EADF8
		public override AcceptanceReport CanAccept()
		{
			foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
			{
				if (this.CanPawnAccept(p))
				{
					return true;
				}
			}
			return new AcceptanceReport("QuestNoColonistWithTitle".Translate(this.minimumTitle.GetLabelCapForBothGenders()));
		}

		// Token: 0x0600A2CD RID: 41677 RVA: 0x002ECC7C File Offset: 0x002EAE7C
		public override bool CanPawnAccept(Pawn p)
		{
			if (p.royalty == null)
			{
				return false;
			}
			RoyalTitleDef currentTitle = p.royalty.GetCurrentTitle(this.faction);
			return currentTitle != null && this.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) >= this.faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(this.minimumTitle);
		}

		// Token: 0x0600A2CE RID: 41678 RVA: 0x002ECCE0 File Offset: 0x002EAEE0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RoyalTitleDef>(ref this.minimumTitle, "minimumTitle");
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x17001903 RID: 6403
		// (get) Token: 0x0600A2CF RID: 41679 RVA: 0x002ECD09 File Offset: 0x002EAF09
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				yield return new Dialog_InfoCard.Hyperlink(this.minimumTitle, this.faction, -1);
				yield break;
			}
		}

		// Token: 0x04007097 RID: 28823
		public RoyalTitleDef minimumTitle;

		// Token: 0x04007098 RID: 28824
		public Faction faction;
	}
}
