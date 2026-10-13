using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200154A RID: 5450
	public abstract class WorkGiver_GatherAnimalBodyResources : WorkGiver_Scanner
	{
		// Token: 0x170013C2 RID: 5058
		// (get) Token: 0x06008162 RID: 33122
		protected abstract JobDef JobDef { get; }

		// Token: 0x06008163 RID: 33123
		protected abstract CompHasGatherableBodyResource GetComp(Pawn animal);

		// Token: 0x06008164 RID: 33124 RVA: 0x0026F7EF File Offset: 0x0026D9EF
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
		}

		// Token: 0x06008165 RID: 33125 RVA: 0x0026F808 File Offset: 0x0026DA08
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			List<Pawn> list = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].IsAnimal)
				{
					CompHasGatherableBodyResource comp = this.GetComp(list[i]);
					if (comp != null && comp.ActiveAndFull)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x170013C3 RID: 5059
		// (get) Token: 0x06008166 RID: 33126 RVA: 0x000E96BB File Offset: 0x000E78BB
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		// Token: 0x06008167 RID: 33127 RVA: 0x0026F868 File Offset: 0x0026DA68
		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.IsAnimal)
			{
				return false;
			}
			CompHasGatherableBodyResource comp = this.GetComp(pawn2);
			return comp != null && comp.ActiveAndFull && !pawn2.Downed && (pawn2.roping == null || !pawn2.roping.IsRopedByPawn) && pawn2.CanCasuallyInteractNow(false, false, false, false) && pawn.CanReserve(pawn2, 1, -1, null, forced);
		}

		// Token: 0x06008168 RID: 33128 RVA: 0x0026F8DA File Offset: 0x0026DADA
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			return JobMaker.MakeJob(this.JobDef, t);
		}
	}
}
