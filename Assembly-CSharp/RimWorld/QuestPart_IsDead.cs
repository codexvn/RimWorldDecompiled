using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BCD RID: 7117
	public class QuestPart_IsDead : QuestPartActivable
	{
		// Token: 0x17001853 RID: 6227
		// (get) Token: 0x06009E70 RID: 40560 RVA: 0x002DD5D7 File Offset: 0x002DB7D7
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.pawn != null)
				{
					yield return this.pawn;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06009E71 RID: 40561 RVA: 0x002DD5E7 File Offset: 0x002DB7E7
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.pawn != null && this.pawn.Destroyed)
			{
				base.Complete(this.pawn.Named("SUBJECT"));
			}
		}

		// Token: 0x06009E72 RID: 40562 RVA: 0x002DD61A File Offset: 0x002DB81A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
		}

		// Token: 0x06009E73 RID: 40563 RVA: 0x002DD633 File Offset: 0x002DB833
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.pawn = Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>();
			}
		}

		// Token: 0x06009E74 RID: 40564 RVA: 0x002DD65C File Offset: 0x002DB85C
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x04006DB3 RID: 28083
		public Pawn pawn;
	}
}
