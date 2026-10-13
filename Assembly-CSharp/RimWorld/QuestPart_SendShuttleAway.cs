using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CA0 RID: 7328
	public class QuestPart_SendShuttleAway : QuestPart
	{
		// Token: 0x17001919 RID: 6425
		// (get) Token: 0x0600A33D RID: 41789 RVA: 0x002EDE97 File Offset: 0x002EC097
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				yield return this.shuttle;
				yield break;
			}
		}

		// Token: 0x0600A33E RID: 41790 RVA: 0x002EDEA7 File Offset: 0x002EC0A7
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.shuttle != null)
			{
				SendShuttleAwayQuestPartUtility.SendAway(this.shuttle, this.dropEverything);
			}
		}

		// Token: 0x0600A33F RID: 41791 RVA: 0x002EDEDC File Offset: 0x002EC0DC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_Values.Look<bool>(ref this.dropEverything, "dropEverything", false, false);
		}

		// Token: 0x0600A340 RID: 41792 RVA: 0x002EDF1C File Offset: 0x002EC11C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				Map randomPlayerHomeMap = Find.RandomPlayerHomeMap;
				IntVec3 center = DropCellFinder.RandomDropSpot(randomPlayerHomeMap, true);
				this.shuttle = ThingMaker.MakeThing(ThingDefOf.Shuttle, null);
				GenPlace.TryPlaceThing(SkyfallerMaker.MakeSkyfaller(ThingDefOf.ShuttleIncoming, this.shuttle), center, randomPlayerHomeMap, ThingPlaceMode.Near, null, null, null, 1);
			}
		}

		// Token: 0x0600A341 RID: 41793 RVA: 0x002EDF97 File Offset: 0x002EC197
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.shuttle != null)
			{
				this.shuttle.TryGetComp<CompShuttle>().requiredPawns.Replace(replace, with);
			}
		}

		// Token: 0x040070CF RID: 28879
		public string inSignal;

		// Token: 0x040070D0 RID: 28880
		public Thing shuttle;

		// Token: 0x040070D1 RID: 28881
		public bool dropEverything;
	}
}
