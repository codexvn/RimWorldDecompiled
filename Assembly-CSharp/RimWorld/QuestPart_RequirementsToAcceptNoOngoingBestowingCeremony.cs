using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C8F RID: 7311
	public class QuestPart_RequirementsToAcceptNoOngoingBestowingCeremony : QuestPart_RequirementsToAccept
	{
		// Token: 0x0600A2EB RID: 41707 RVA: 0x002ED0DC File Offset: 0x002EB2DC
		public override AcceptanceReport CanAccept()
		{
			if (Find.QuestManager.QuestsListForReading.Any((Quest q) => q.State == QuestState.Ongoing && q.root == QuestScriptDefOf.BestowingCeremony))
			{
				return new AcceptanceReport("QuestCanNotStartUntilBestowingCeremonyFinished".Translate());
			}
			return true;
		}
	}
}
