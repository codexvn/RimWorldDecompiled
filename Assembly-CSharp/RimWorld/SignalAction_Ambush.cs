using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002ABA RID: 10938
	public class SignalAction_Ambush : SignalAction
	{
		// Token: 0x0600F3B8 RID: 62392 RVA: 0x0046CDD4 File Offset: 0x0046AFD4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.points, "points", 0f, false);
			Scribe_Values.Look<SignalActionAmbushType>(ref this.ambushType, "ambushType", SignalActionAmbushType.Normal, false);
			Scribe_Values.Look<IntVec3>(ref this.spawnNear, "spawnNear", default(IntVec3), false);
			Scribe_Values.Look<CellRect>(ref this.spawnAround, "spawnAround", default(CellRect), false);
			Scribe_Values.Look<bool>(ref this.spawnPawnsOnEdge, "spawnPawnsOnEdge", false, false);
			Scribe_Values.Look<bool>(ref this.useDropPods, "useDropPods", false, false);
		}

		// Token: 0x0600F3B9 RID: 62393 RVA: 0x0046CE68 File Offset: 0x0046B068
		protected override void DoAction(SignalArgs args)
		{
			if (this.points <= 0f)
			{
				return;
			}
			List<Pawn> list = new List<Pawn>();
			foreach (Pawn pawn in this.GenerateAmbushPawns())
			{
				IntVec3 intVec;
				if (this.spawnPawnsOnEdge)
				{
					if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => x.Standable(base.Map) && !x.Fogged(base.Map) && base.Map.reachability.CanReachColony(x), base.Map, CellFinder.EdgeRoadChance_Ignore, out intVec))
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
						break;
					}
				}
				else if (!SiteGenStepUtility.TryFindSpawnCellAroundOrNear(this.spawnAround, this.spawnNear, base.Map, out intVec))
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
					break;
				}
				if (this.useDropPods)
				{
					DropPodUtility.DropThingsNear(intVec, base.Map, Gen.YieldSingle<Pawn>(pawn), 110, false, false, true, true, true, null);
				}
				else
				{
					GenSpawn.Spawn(pawn, intVec, base.Map, WipeMode.Vanish);
					if (!this.spawnPawnsOnEdge)
					{
						for (int i = 0; i < 10; i++)
						{
							FleckMaker.ThrowAirPuffUp(pawn.DrawPos, base.Map);
						}
					}
				}
				list.Add(pawn);
			}
			if (!list.Any<Pawn>())
			{
				return;
			}
			if (this.ambushType == SignalActionAmbushType.Manhunters)
			{
				for (int j = 0; j < list.Count; j++)
				{
					list[j].health.AddHediff(HediffDefOf.Scaria, null, null, null);
					list[j].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, false, null, false, false, false);
				}
			}
			else
			{
				Faction faction = list[0].Faction;
				LordMaker.MakeNewLord(faction, new LordJob_AssaultColony(faction, true, true, false, false, true, false, false), base.Map, list);
			}
			if (!this.spawnPawnsOnEdge && !this.useDropPods)
			{
				for (int k = 0; k < list.Count; k++)
				{
					list[k].jobs.StartJob(JobMaker.MakeJob(JobDefOf.Wait, 120, false), JobCondition.None, null, false, true, null, null, false, false, null, false, true, false);
					list[k].Rotation = Rot4.Random;
				}
			}
			Find.LetterStack.ReceiveLetter("LetterLabelAmbushInExistingMap".Translate(), "LetterAmbushInExistingMap".Translate(Faction.OfPlayer.def.pawnsPlural).CapitalizeFirst(), LetterDefOf.ThreatBig, list, null, null, null, null, 0, true);
		}

		// Token: 0x0600F3BA RID: 62394 RVA: 0x0046D0E8 File Offset: 0x0046B2E8
		private IEnumerable<Pawn> GenerateAmbushPawns()
		{
			if (this.ambushType == SignalActionAmbushType.Manhunters)
			{
				PawnKindDef animalKind;
				if (!AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(this.points, base.Map.Tile, out animalKind) && !AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(this.points, PlanetTile.Invalid, out animalKind))
				{
					return Enumerable.Empty<Pawn>();
				}
				return AggressiveAnimalIncidentUtility.GenerateAnimals(animalKind, base.Map.Tile, this.points, 0);
			}
			else
			{
				Faction faction;
				if (this.ambushType == SignalActionAmbushType.Mechanoids)
				{
					faction = Faction.OfMechanoids;
				}
				else
				{
					faction = ((base.Map.ParentFaction != null && base.Map.ParentFaction.HostileTo(Faction.OfPlayer)) ? base.Map.ParentFaction : Find.FactionManager.RandomEnemyFaction(false, false, false, TechLevel.Undefined));
				}
				if (faction == null)
				{
					return Enumerable.Empty<Pawn>();
				}
				return PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
				{
					groupKind = PawnGroupKindDefOf.Combat,
					tile = base.Map.Tile,
					faction = faction,
					points = Mathf.Max(this.points, faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null))
				}, true);
			}
		}

		// Token: 0x0400A08C RID: 41100
		public float points;

		// Token: 0x0400A08D RID: 41101
		public SignalActionAmbushType ambushType;

		// Token: 0x0400A08E RID: 41102
		public IntVec3 spawnNear = IntVec3.Invalid;

		// Token: 0x0400A08F RID: 41103
		public CellRect spawnAround;

		// Token: 0x0400A090 RID: 41104
		public bool spawnPawnsOnEdge;

		// Token: 0x0400A091 RID: 41105
		public bool useDropPods;

		// Token: 0x0400A092 RID: 41106
		private const int PawnsDelayAfterSpawnTicks = 120;
	}
}
