using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018F5 RID: 6389
	public class ThoughtWorker_ImprisonedWithEntity : ThoughtWorker_EntityInRoomBase
	{
		// Token: 0x0600913A RID: 37178 RVA: 0x002AB719 File Offset: 0x002A9919
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsPrisoner)
			{
				return ThoughtState.Inactive;
			}
			if (!p.guest.PrisonerIsSecure)
			{
				return ThoughtState.Inactive;
			}
			return base.CurrentStateInternal(p);
		}
	}
}
