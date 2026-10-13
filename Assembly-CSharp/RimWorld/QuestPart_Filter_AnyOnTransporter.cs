using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BA5 RID: 7077
	public class QuestPart_Filter_AnyOnTransporter : QuestPart_Filter
	{
		// Token: 0x06009DC8 RID: 40392 RVA: 0x002DB340 File Offset: 0x002D9540
		protected override bool Pass(SignalArgs args)
		{
			if (this.pawns.NullOrEmpty<Pawn>())
			{
				return false;
			}
			ThingOwner directlyHeldThings = this.transporter.TryGetComp<CompTransporter>().GetDirectlyHeldThings();
			foreach (Pawn item in this.pawns)
			{
				if (directlyHeldThings.Contains(item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009DC9 RID: 40393 RVA: 0x002DB3BC File Offset: 0x002D95BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Thing>(ref this.transporter, "transporter", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04006D4B RID: 27979
		public Thing transporter;

		// Token: 0x04006D4C RID: 27980
		public List<Pawn> pawns;
	}
}
