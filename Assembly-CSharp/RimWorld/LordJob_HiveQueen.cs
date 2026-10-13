using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200167E RID: 5758
	public class LordJob_HiveQueen : LordJob_StructureThreatCluster
	{
		// Token: 0x060087B4 RID: 34740 RVA: 0x0028862B File Offset: 0x0028682B
		public LordJob_HiveQueen()
		{
		}

		// Token: 0x060087B5 RID: 34741 RVA: 0x00288633 File Offset: 0x00286833
		public LordJob_HiveQueen(Faction faction, IntVec3 position, float wanderRadius, bool sendWokenUpMessage = true, bool awakeOnClamor = false) : base(faction, position, wanderRadius, sendWokenUpMessage, awakeOnClamor)
		{
		}

		// Token: 0x060087B6 RID: 34742 RVA: 0x00288642 File Offset: 0x00286842
		protected override LordToil GetIdleToil()
		{
			return new LordToil_WanderNest();
		}
	}
}
