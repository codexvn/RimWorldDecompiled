using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001742 RID: 5954
	public class LordToil_Concert : LordToil_Party
	{
		// Token: 0x1700158A RID: 5514
		// (get) Token: 0x06008BE9 RID: 35817 RVA: 0x0029AA87 File Offset: 0x00298C87
		public Pawn Organizer
		{
			get
			{
				return this.organizer;
			}
		}

		// Token: 0x06008BEA RID: 35818 RVA: 0x0029AA8F File Offset: 0x00298C8F
		public LordToil_Concert(IntVec3 spot, Pawn organizer, GatheringDef gatheringDef, float joyPerTick = 3.5E-05f) : base(spot, gatheringDef, joyPerTick)
		{
			this.organizer = organizer;
		}

		// Token: 0x04005AF2 RID: 23282
		private Pawn organizer;
	}
}
