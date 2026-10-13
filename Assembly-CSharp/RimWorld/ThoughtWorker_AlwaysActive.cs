using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200189C RID: 6300
	public class ThoughtWorker_AlwaysActive : ThoughtWorker
	{
		// Token: 0x06009029 RID: 36905 RVA: 0x002A44A2 File Offset: 0x002A26A2
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return true;
		}

		// Token: 0x0600902A RID: 36906 RVA: 0x002A44A2 File Offset: 0x002A26A2
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn otherPawn)
		{
			return true;
		}
	}
}
