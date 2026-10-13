using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse.AI
{
	// Token: 0x02000F9A RID: 3994
	public class MentalStateWorker
	{
		// Token: 0x06006278 RID: 25208 RVA: 0x001F1100 File Offset: 0x001EF300
		public virtual bool StateCanOccur(Pawn pawn)
		{
			if (!this.def.inCaravanCanDo && !pawn.Spawned && pawn.IsCaravanMember())
			{
				return false;
			}
			if (!this.def.unspawnedNotInCaravanCanDo && !pawn.Spawned && !pawn.IsCaravanMember())
			{
				return false;
			}
			if (!this.def.prisonersCanDo && pawn.IsPrisoner)
			{
				return false;
			}
			if (!this.def.slavesCanDo && pawn.IsSlave)
			{
				return false;
			}
			if (this.def.colonistsOnly && pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			if (this.def.slavesOnly && !pawn.IsSlave)
			{
				return false;
			}
			if (!this.def.downedCanDo && pawn.Downed)
			{
				return false;
			}
			if (pawn.IsWorldPawn())
			{
				return false;
			}
			for (int i = 0; i < this.def.requiredCapacities.Count; i++)
			{
				if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400467D RID: 18045
		public MentalStateDef def;
	}
}
