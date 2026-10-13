using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014E3 RID: 5347
	public abstract class JoyGiver
	{
		// Token: 0x06007FB6 RID: 32694 RVA: 0x00266F4A File Offset: 0x0026514A
		public virtual float GetChance(Pawn pawn)
		{
			return this.def.baseChance;
		}

		// Token: 0x06007FB7 RID: 32695 RVA: 0x00266F58 File Offset: 0x00265158
		protected virtual void GetSearchSet(Pawn pawn, List<Thing> outCandidates)
		{
			outCandidates.Clear();
			if (this.def.thingDefs == null)
			{
				return;
			}
			if (this.def.thingDefs.Count == 1)
			{
				outCandidates.AddRange(pawn.Map.listerThings.ThingsOfDef(this.def.thingDefs[0]));
			}
			else
			{
				for (int i = 0; i < this.def.thingDefs.Count; i++)
				{
					outCandidates.AddRange(pawn.Map.listerThings.ThingsOfDef(this.def.thingDefs[i]));
				}
			}
			if (pawn.ConcernedByVacuum)
			{
				outCandidates.RemoveWhere((Thing thing) => thing.Position.VacuumConcernTo(pawn));
			}
		}

		// Token: 0x06007FB8 RID: 32696
		public abstract Job TryGiveJob(Pawn pawn);

		// Token: 0x06007FB9 RID: 32697 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual Job TryGiveJobWhileInBed(Pawn pawn)
		{
			return null;
		}

		// Token: 0x06007FBA RID: 32698 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual Job TryGiveJobInGatheringArea(Pawn pawn, IntVec3 gatherSpot, float maxRadius = -1f)
		{
			return null;
		}

		// Token: 0x06007FBB RID: 32699 RVA: 0x00267030 File Offset: 0x00265230
		public virtual bool CanBeGivenTo(Pawn pawn)
		{
			return this.MissingRequiredCapacity(pawn) == null && (!this.def.requiresEnjoyOutdoors || (pawn.needs.PrefersOutdoors && (pawn.genes == null || pawn.genes.EnjoysSunlight))) && this.def.joyKind.PawnCanDo(pawn);
		}

		// Token: 0x06007FBC RID: 32700 RVA: 0x0026708C File Offset: 0x0026528C
		public PawnCapacityDef MissingRequiredCapacity(Pawn pawn)
		{
			for (int i = 0; i < this.def.requiredCapacities.Count; i++)
			{
				if (!pawn.health.capacities.CapableOf(this.def.requiredCapacities[i]))
				{
					return this.def.requiredCapacities[i];
				}
			}
			return null;
		}

		// Token: 0x0400556C RID: 21868
		public JoyGiverDef def;
	}
}
