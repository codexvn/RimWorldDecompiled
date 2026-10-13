using System;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003935 RID: 14645
	public class SymbolResolver_SinglePawn : SymbolResolver
	{
		// Token: 0x060142B1 RID: 82609 RVA: 0x00602474 File Offset: 0x00600674
		public override bool CanResolve(ResolveParams rp)
		{
			IntVec3 intVec;
			return base.CanResolve(rp) && ((rp.singlePawnToSpawn != null && rp.singlePawnToSpawn.Spawned) || SymbolResolver_SinglePawn.TryFindSpawnCell(rp, out intVec));
		}

		// Token: 0x060142B2 RID: 82610 RVA: 0x006024B0 File Offset: 0x006006B0
		public override void Resolve(ResolveParams rp)
		{
			if (rp.singlePawnToSpawn != null && rp.singlePawnToSpawn.Spawned)
			{
				return;
			}
			Map map = BaseGen.globalSettings.map;
			IntVec3 loc;
			if (!SymbolResolver_SinglePawn.TryFindSpawnCell(rp, out loc))
			{
				if (rp.singlePawnToSpawn != null)
				{
					Find.WorldPawns.PassToWorld(rp.singlePawnToSpawn, PawnDiscardDecideMode.Decide);
				}
				return;
			}
			Pawn pawn;
			if (rp.singlePawnToSpawn == null)
			{
				PawnGenerationRequest value;
				if (rp.singlePawnGenerationRequest != null)
				{
					value = rp.singlePawnGenerationRequest.Value;
				}
				else
				{
					PawnKindDef pawnKindDef;
					if ((pawnKindDef = rp.singlePawnKindDef) == null)
					{
						pawnKindDef = (from x in DefDatabase<PawnKindDef>.AllDefsListForReading
						where x.defaultFactionDef == null || !x.defaultFactionDef.isPlayer
						select x).RandomElement<PawnKindDef>();
					}
					PawnKindDef pawnKindDef2 = pawnKindDef;
					Faction faction = rp.faction;
					if (faction == null && pawnKindDef2.RaceProps.Humanlike)
					{
						if (pawnKindDef2.defaultFactionDef != null)
						{
							faction = FactionUtility.DefaultFactionFrom(pawnKindDef2.defaultFactionDef);
							if (faction == null)
							{
								return;
							}
						}
						else if (!(from x in Find.FactionManager.AllFactions
						where !x.IsPlayer
						select x).TryRandomElement(out faction))
						{
							return;
						}
					}
					value = new PawnGenerationRequest(pawnKindDef2, faction, PawnGenerationContext.NonPlayer, new PlanetTile?(map.Tile), false, false, faction.deactivated, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
				}
				pawn = PawnGenerator.GeneratePawn(value);
				if (rp.postThingGenerate != null)
				{
					rp.postThingGenerate(pawn);
				}
			}
			else
			{
				pawn = rp.singlePawnToSpawn;
			}
			if (!pawn.Dead && rp.disableSinglePawn != null && rp.disableSinglePawn.Value)
			{
				pawn.mindState.Active = false;
			}
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
			if (rp.singlePawnLord != null)
			{
				rp.singlePawnLord.AddPawn(pawn);
			}
			if (rp.postThingSpawn != null)
			{
				rp.postThingSpawn(pawn);
			}
		}

		// Token: 0x060142B3 RID: 82611 RVA: 0x006026F8 File Offset: 0x006008F8
		public static bool TryFindSpawnCell(ResolveParams rp, out IntVec3 cell)
		{
			Map map = BaseGen.globalSettings.map;
			return CellFinder.TryFindRandomCellInsideWith(rp.rect, (IntVec3 x) => x.Standable(map) && (rp.singlePawnSpawnCellExtraPredicate == null || rp.singlePawnSpawnCellExtraPredicate(x)), out cell);
		}
	}
}
