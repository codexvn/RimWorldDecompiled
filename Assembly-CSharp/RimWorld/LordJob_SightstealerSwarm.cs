using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001675 RID: 5749
	public class LordJob_SightstealerSwarm : LordJob_EntitySwarm
	{
		// Token: 0x06008779 RID: 34681 RVA: 0x002871DB File Offset: 0x002853DB
		public LordJob_SightstealerSwarm()
		{
		}

		// Token: 0x0600877A RID: 34682 RVA: 0x002871E3 File Offset: 0x002853E3
		public LordJob_SightstealerSwarm(IntVec3 startPos, IntVec3 destPos) : base(startPos, destPos)
		{
		}

		// Token: 0x0600877B RID: 34683 RVA: 0x002874C7 File Offset: 0x002856C7
		protected override LordToil CreateTravelingToil(IntVec3 start, IntVec3 dest)
		{
			return new LordToil_SightstealerSwarm(start, dest);
		}
	}
}
