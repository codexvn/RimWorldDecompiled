using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CCE RID: 11470
	public interface IHackable
	{
		// Token: 0x060101A6 RID: 65958
		void OnLockedOut(Pawn pawn = null);

		// Token: 0x060101A7 RID: 65959
		void OnHacked(Pawn pawn = null);
	}
}
