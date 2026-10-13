using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BCF RID: 7119
	public class QuestPart_IsPrisoner : QuestPartActivable
	{
		// Token: 0x17001856 RID: 6230
		// (get) Token: 0x06009E80 RID: 40576 RVA: 0x002DD81F File Offset: 0x002DBA1F
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

		// Token: 0x06009E81 RID: 40577 RVA: 0x002DD82F File Offset: 0x002DBA2F
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.pawn != null && this.pawn.IsPrisoner)
			{
				base.Complete(this.pawn.Named("SUBJECT"));
			}
		}

		// Token: 0x06009E82 RID: 40578 RVA: 0x002DD862 File Offset: 0x002DBA62
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
		}

		// Token: 0x06009E83 RID: 40579 RVA: 0x002DD87B File Offset: 0x002DBA7B
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.pawn = Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>();
			}
		}

		// Token: 0x06009E84 RID: 40580 RVA: 0x002DD8A4 File Offset: 0x002DBAA4
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x04006DB9 RID: 28089
		public Pawn pawn;
	}
}
