using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C1A RID: 7194
	public class QuestPart_ChangeNeed : QuestPart
	{
		// Token: 0x1700189F RID: 6303
		// (get) Token: 0x0600A033 RID: 41011 RVA: 0x002E2CB7 File Offset: 0x002E0EB7
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

		// Token: 0x0600A034 RID: 41012 RVA: 0x002E2CC8 File Offset: 0x002E0EC8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			Need need;
			if (signal.tag == this.inSignal && this.pawn != null && this.pawn.needs != null && this.pawn.needs.TryGetNeed(this.need, out need))
			{
				need.CurLevel += this.offset;
			}
		}

		// Token: 0x0600A035 RID: 41013 RVA: 0x002E2D34 File Offset: 0x002E0F34
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Defs.Look<NeedDef>(ref this.need, "need");
			Scribe_Values.Look<float>(ref this.offset, "offset", 0f, false);
		}

		// Token: 0x0600A036 RID: 41014 RVA: 0x002E2D90 File Offset: 0x002E0F90
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.need = NeedDefOf.Food;
			this.offset = 0.5f;
			if (Find.AnyPlayerHomeMap != null)
			{
				Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault<Pawn>();
			}
		}

		// Token: 0x0600A037 RID: 41015 RVA: 0x002E2DCA File Offset: 0x002E0FCA
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			if (this.pawn == replace)
			{
				this.pawn = with;
			}
		}

		// Token: 0x04006EC9 RID: 28361
		public string inSignal;

		// Token: 0x04006ECA RID: 28362
		public Pawn pawn;

		// Token: 0x04006ECB RID: 28363
		public NeedDef need;

		// Token: 0x04006ECC RID: 28364
		public float offset;
	}
}
