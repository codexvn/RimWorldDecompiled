using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C07 RID: 7175
	public class QuestPart_AddMemoryThought : QuestPart
	{
		// Token: 0x1700188D RID: 6285
		// (get) Token: 0x06009FCA RID: 40906 RVA: 0x002E1943 File Offset: 0x002DFB43
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.pawn != null && this.addToLookTargets)
				{
					yield return this.pawn;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06009FCB RID: 40907 RVA: 0x002E1954 File Offset: 0x002DFB54
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.pawn != null && this.pawn.needs != null)
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(this.def, this.otherPawn, null);
			}
		}

		// Token: 0x06009FCC RID: 40908 RVA: 0x002E19BC File Offset: 0x002DFBBC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.addToLookTargets, "addToLookTargets", false, false);
			Scribe_Defs.Look<ThoughtDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", false);
		}

		// Token: 0x06009FCD RID: 40909 RVA: 0x002E1A25 File Offset: 0x002DFC25
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.def = (ThoughtDefOf.DecreeMet ?? ThoughtDefOf.DebugGood);
			this.pawn = PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>();
		}

		// Token: 0x06009FCE RID: 40910 RVA: 0x002E1A51 File Offset: 0x002DFC51
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
			if (this.otherPawn == replace)
			{
				this.otherPawn = with;
			}
		}

		// Token: 0x04006E83 RID: 28291
		public string inSignal;

		// Token: 0x04006E84 RID: 28292
		public ThoughtDef def;

		// Token: 0x04006E85 RID: 28293
		public Pawn pawn;

		// Token: 0x04006E86 RID: 28294
		public Pawn otherPawn;

		// Token: 0x04006E87 RID: 28295
		public bool addToLookTargets = true;
	}
}
