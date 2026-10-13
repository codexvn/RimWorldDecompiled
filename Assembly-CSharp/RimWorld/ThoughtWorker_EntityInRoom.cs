using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018F4 RID: 6388
	public class ThoughtWorker_EntityInRoom : ThoughtWorker_EntityInRoomBase
	{
		// Token: 0x06009138 RID: 37176 RVA: 0x002AB6EC File Offset: 0x002A98EC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.IsPrisoner)
			{
				return ThoughtState.Inactive;
			}
			if (!p.IsColonist)
			{
				return ThoughtState.Inactive;
			}
			return base.CurrentStateInternal(p);
		}
	}
}
