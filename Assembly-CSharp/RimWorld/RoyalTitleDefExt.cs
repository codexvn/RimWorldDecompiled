using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020037B9 RID: 14265
	public static class RoyalTitleDefExt
	{
		// Token: 0x06013C4C RID: 80972 RVA: 0x005DE000 File Offset: 0x005DC200
		public static RoyalTitleDef GetNextTitle(this RoyalTitleDef currentTitle, Faction faction)
		{
			int num = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle);
			if (num == -1 && currentTitle != null)
			{
				return null;
			}
			int num2 = (currentTitle == null) ? 0 : (num + 1);
			if (faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.Count <= num2)
			{
				return null;
			}
			return faction.def.RoyalTitlesAwardableInSeniorityOrderForReading[num2];
		}

		// Token: 0x06013C4D RID: 80973 RVA: 0x005DE058 File Offset: 0x005DC258
		public static RoyalTitleDef GetPreviousTitle(this RoyalTitleDef currentTitle, Faction faction)
		{
			if (currentTitle == null)
			{
				return null;
			}
			int num = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle) - 1;
			if (num >= faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.Count || num < 0)
			{
				return null;
			}
			return faction.def.RoyalTitlesAwardableInSeniorityOrderForReading[num];
		}

		// Token: 0x06013C4E RID: 80974 RVA: 0x005DE0A8 File Offset: 0x005DC2A8
		public static RoyalTitleDef GetPreviousTitle_IncludeNonRewardable(this RoyalTitleDef currentTitle, Faction faction)
		{
			if (currentTitle == null)
			{
				return null;
			}
			int num = faction.def.RoyalTitlesAllInSeniorityOrderForReading.IndexOf(currentTitle) - 1;
			if (num >= faction.def.RoyalTitlesAllInSeniorityOrderForReading.Count || num < 0)
			{
				return null;
			}
			return faction.def.RoyalTitlesAllInSeniorityOrderForReading[num];
		}

		// Token: 0x06013C4F RID: 80975 RVA: 0x005DE0F8 File Offset: 0x005DC2F8
		public static bool TryInherit(this RoyalTitleDef title, Pawn from, Faction faction, out RoyalTitleInheritanceOutcome outcome)
		{
			outcome = default(RoyalTitleInheritanceOutcome);
			if (title.GetInheritanceWorker(faction) == null)
			{
				return false;
			}
			Pawn heir = from.royalty.GetHeir(faction);
			if (heir == null || heir.Destroyed)
			{
				return false;
			}
			RoyalTitleDef currentTitle = heir.royalty.GetCurrentTitle(faction);
			bool heirTitleHigher = currentTitle != null && currentTitle.seniority >= title.seniority;
			outcome = new RoyalTitleInheritanceOutcome
			{
				heir = heir,
				heirCurrentTitle = currentTitle,
				heirTitleHigher = heirTitleHigher
			};
			return true;
		}
	}
}
