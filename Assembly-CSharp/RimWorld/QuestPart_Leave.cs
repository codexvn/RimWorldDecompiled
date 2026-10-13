using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C55 RID: 7253
	public class QuestPart_Leave : QuestPart
	{
		// Token: 0x170018D9 RID: 6361
		// (get) Token: 0x0600A18C RID: 41356 RVA: 0x002E6DDF File Offset: 0x002E4FDF
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				foreach (Pawn t in PawnsArriveQuestPartUtility.GetQuestLookTargets(this.pawns))
				{
					yield return t;
				}
				IEnumerator<Pawn> enumerator2 = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A18D RID: 41357 RVA: 0x002E6DF0 File Offset: 0x002E4FF0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			Pawn item;
			if (signal.tag == this.inSignalRemovePawn && signal.args.TryGetArg<Pawn>("SUBJECT", out item) && this.pawns.Contains(item))
			{
				this.pawns.Remove(item);
			}
			if (signal.tag == this.inSignal)
			{
				LeaveQuestPartUtility.MakePawnsLeave(this.pawns, this.sendStandardLetter, this.quest, this.wakeUp);
			}
		}

		// Token: 0x0600A18E RID: 41358 RVA: 0x002E6E77 File Offset: 0x002E5077
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.leaveOnCleanup)
			{
				LeaveQuestPartUtility.MakePawnsLeave(this.pawns, this.sendStandardLetter, this.quest, this.wakeUp);
			}
		}

		// Token: 0x0600A18F RID: 41359 RVA: 0x002E6EA4 File Offset: 0x002E50A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.sendStandardLetter, "sendStandardLetter", true, false);
			Scribe_Values.Look<bool>(ref this.leaveOnCleanup, "leaveOnCleanup", false, false);
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			Scribe_Values.Look<bool>(ref this.wakeUp, "wakeUp", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600A190 RID: 41360 RVA: 0x002E6F5C File Offset: 0x002E515C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				Map randomPlayerHomeMap = Find.RandomPlayerHomeMap;
				if (randomPlayerHomeMap.mapPawns.FreeColonistsCount != 0)
				{
					this.pawns.Add(randomPlayerHomeMap.mapPawns.FreeColonists.First<Pawn>());
				}
			}
		}

		// Token: 0x0600A191 RID: 41361 RVA: 0x002E6FC1 File Offset: 0x002E51C1
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04006F9E RID: 28574
		public string inSignal;

		// Token: 0x04006F9F RID: 28575
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04006FA0 RID: 28576
		public bool sendStandardLetter = true;

		// Token: 0x04006FA1 RID: 28577
		public bool leaveOnCleanup = true;

		// Token: 0x04006FA2 RID: 28578
		public string inSignalRemovePawn;

		// Token: 0x04006FA3 RID: 28579
		public bool wakeUp;
	}
}
