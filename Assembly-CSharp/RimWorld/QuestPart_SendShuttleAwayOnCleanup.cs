using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CA3 RID: 7331
	public class QuestPart_SendShuttleAwayOnCleanup : QuestPart
	{
		// Token: 0x1700191C RID: 6428
		// (get) Token: 0x0600A34C RID: 41804 RVA: 0x002EE0E6 File Offset: 0x002EC2E6
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				yield return this.shuttle;
				yield break;
			}
		}

		// Token: 0x0600A34D RID: 41805 RVA: 0x002EE0F6 File Offset: 0x002EC2F6
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.shuttle != null)
			{
				SendShuttleAwayQuestPartUtility.SendAway(this.shuttle, this.dropEverything);
			}
		}

		// Token: 0x0600A34E RID: 41806 RVA: 0x002EE117 File Offset: 0x002EC317
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<bool>(ref this.dropEverything, "dropEverything", false, false);
		}

		// Token: 0x0600A34F RID: 41807 RVA: 0x002EE142 File Offset: 0x002EC342
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.shuttle != null)
			{
				this.shuttle.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
			}
		}

		// Token: 0x040070D6 RID: 28886
		public Thing shuttle;

		// Token: 0x040070D7 RID: 28887
		public bool dropEverything;
	}
}
