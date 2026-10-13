using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020022D0 RID: 8912
	public class CreepJoinerWorker_SightstealerAttack : BaseCreepJoinerWorker
	{
		// Token: 0x17001DAA RID: 7594
		// (get) Token: 0x0600C1E4 RID: 49636 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CanOccurOnDeath
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C1E5 RID: 49637 RVA: 0x00382DD4 File Offset: 0x00380FD4
		public override void DoResponse(List<TargetInfo> looktargets, List<NamedArgument> namedArgs)
		{
			if (!base.Pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			if (!base.Pawn.Dead)
			{
				base.Pawn.SetFaction(Find.FactionManager.OfEntities, null);
				base.Pawn.guest.Recruitable = false;
				if (base.Pawn.GetLord() != null)
				{
					base.Pawn.GetLord().Notify_PawnLost(base.Pawn, PawnLostCondition.Undefined, null);
				}
				LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_AssaultColony(Faction.OfEntities, false, false, false, false, false, false, false), base.Pawn.MapHeld, null).AddPawn(base.Pawn);
			}
			Lord lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_SightstealerAssault(), base.Pawn.MapHeld, null);
			float num = StorytellerUtility.DefaultThreatPointsNow(base.Pawn.MapHeld) * CreepJoinerWorker_SightstealerAttack.SpawnPointScale.RandomInRange;
			num = Mathf.Max(Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Sightstealers, null), num);
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				faction = Faction.OfEntities,
				groupKind = PawnGroupKindDefOf.Sightstealers,
				points = num,
				tile = base.Pawn.MapHeld.Tile
			}, true).ToList<Pawn>();
			foreach (Pair<List<Pawn>, IntVec3> pair in PawnsArrivalModeWorkerUtility.SplitIntoRandomGroupsNearMapEdge(list, base.Pawn.MapHeld, false))
			{
				foreach (Thing newThing in pair.First)
				{
					IntVec3 loc = CellFinder.RandomClosewalkCellNear(pair.Second, base.Pawn.MapHeld, 8, null);
					GenSpawn.Spawn(newThing, loc, base.Pawn.MapHeld, WipeMode.Vanish);
				}
			}
			foreach (Pawn p in list)
			{
				lord.AddPawn(p);
			}
			SoundDefOf.Sightstealer_SummonedHowl.PlayOneShot(base.Pawn);
		}

		// Token: 0x04008466 RID: 33894
		private static readonly FloatRange SpawnPointScale = new FloatRange(0.25f, 0.65f);
	}
}
