using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010BC RID: 4284
	public class TransitionAction_GiveGift : TransitionAction
	{
		// Token: 0x060068F3 RID: 26867 RVA: 0x002120B4 File Offset: 0x002102B4
		public override void DoAction(Transition trans)
		{
			if (this.gifts.NullOrEmpty<Thing>())
			{
				return;
			}
			VisitorGiftForPlayerUtility.GiveGift(trans.target.lord.ownedPawns, trans.target.lord.faction, this.gifts);
			this.gifts.Clear();
		}

		// Token: 0x04004A59 RID: 19033
		public List<Thing> gifts;
	}
}
