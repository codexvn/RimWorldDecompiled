using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D9D RID: 7581
	public class IncidentWorker_GameEndedWanderersJoin : IncidentWorker
	{
		// Token: 0x0600A993 RID: 43411 RVA: 0x0030889C File Offset: 0x00306A9C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			return this.CanSpawnJoiner(map);
		}

		// Token: 0x0600A994 RID: 43412 RVA: 0x003088C8 File Offset: 0x00306AC8
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!this.CanSpawnJoiner(map))
			{
				return false;
			}
			IntVec3 loc;
			this.TryFindEntryCell(map, out loc);
			List<Pawn> startingAndOptionalPawns = Find.GameInitData.startingAndOptionalPawns;
			foreach (Pawn pawn in startingAndOptionalPawns)
			{
				GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
				foreach (ThingDefCount t in Find.GameInitData.startingPossessions[pawn])
				{
					ThingWithComps thingWithComps = StartingPawnUtility.GenerateStartingPossession(t) as ThingWithComps;
					if (thingWithComps.HasComp<CompEquippable>())
					{
						pawn.equipment.AddEquipment(thingWithComps);
					}
					else
					{
						Apparel apparel = thingWithComps as Apparel;
						if (apparel != null)
						{
							pawn.apparel.Wear(apparel, true, false);
						}
						else
						{
							pawn.inventory.innerContainer.TryAdd(thingWithComps, true);
						}
					}
				}
			}
			base.SendStandardLetter(this.def.letterLabel, this.def.letterText, LetterDefOf.PositiveEvent, parms, startingAndOptionalPawns, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x0600A995 RID: 43413 RVA: 0x00308A24 File Offset: 0x00306C24
		public virtual bool CanSpawnJoiner(Map map)
		{
			IntVec3 intVec;
			return this.TryFindEntryCell(map, out intVec);
		}

		// Token: 0x0600A996 RID: 43414 RVA: 0x00308A3C File Offset: 0x00306C3C
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c) && !c.Fogged(map), map, CellFinder.EdgeRoadChance_Neutral, out cell);
		}
	}
}
