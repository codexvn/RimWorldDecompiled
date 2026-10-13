using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BBA RID: 7098
	public class QuestPart_Filter_PawnDestroyed : QuestPart_Filter
	{
		// Token: 0x06009E09 RID: 40457 RVA: 0x002DBD27 File Offset: 0x002D9F27
		protected override bool Pass(SignalArgs args)
		{
			return this.pawn.DestroyedOrNull();
		}

		// Token: 0x06009E0A RID: 40458 RVA: 0x002DBD34 File Offset: 0x002D9F34
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
		}

		// Token: 0x04006D6D RID: 28013
		public Pawn pawn;
	}
}
