using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C13 RID: 7187
	public class QuestPart_AssignMechToMechanitor : QuestPart
	{
		// Token: 0x0600A012 RID: 40978 RVA: 0x002E25B4 File Offset: 0x002E07B4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && MechanitorUtility.IsMechanitor(this.mechanitor) && this.mechanitor.mechanitor.CanOverseeSubject(this.mech))
			{
				this.mechanitor.relations.AddDirectRelation(PawnRelationDefOf.Overseer, this.mech);
			}
		}

		// Token: 0x0600A013 RID: 40979 RVA: 0x002E261B File Offset: 0x002E081B
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			if (this.mech == pawn)
			{
				this.mech = null;
			}
			if (this.mechanitor == pawn)
			{
				this.mechanitor = null;
			}
		}

		// Token: 0x0600A014 RID: 40980 RVA: 0x002E263D File Offset: 0x002E083D
		public override bool QuestPartReserves(Pawn p)
		{
			return p == this.mech || p == this.mechanitor;
		}

		// Token: 0x0600A015 RID: 40981 RVA: 0x002E2653 File Offset: 0x002E0853
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Pawn>(ref this.mech, "mech", false);
			Scribe_References.Look<Pawn>(ref this.mechanitor, "mechanitor", false);
		}

		// Token: 0x04006EB3 RID: 28339
		public Pawn mech;

		// Token: 0x04006EB4 RID: 28340
		public Pawn mechanitor;

		// Token: 0x04006EB5 RID: 28341
		public string inSignal;
	}
}
