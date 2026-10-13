using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D87 RID: 11655
	public class CompObelisk_Duplicator : CompObelisk_ExplodingSpawner
	{
		// Token: 0x0601062F RID: 67119 RVA: 0x004BA534 File Offset: 0x004B8734
		public override void TriggerInteractionEffect(Pawn interactor, bool triggeredByPlayer = false)
		{
			IntVec3 targetCell;
			if (!CellFinder.TryFindRandomCellNear(this.parent.Position, this.parent.Map, 16, new Predicate<IntVec3>(this.IsValidSpawnCell), out targetCell, 100))
			{
				return;
			}
			Pawn pawn;
			if (!AnomalyUtility.TryDuplicatePawn(interactor, targetCell, this.parent.Map, out pawn, null, false, true, true))
			{
				if (triggeredByPlayer)
				{
					this.parent.GetComp<CompObeliskTriggerInteractor>().ResetCooldown(false);
					Find.LetterStack.ReceiveLetter("ObeliskDuplicationFailedLetterLabel".Translate(), "ObeliskDuplicationFailedLetter".Translate(interactor.Named("PAWN")), LetterDefOf.NeutralEvent, pawn, null, null, null, null, 0, true);
				}
				return;
			}
			this.lastInteractionEffectTick = Find.TickManager.TicksGame;
			if (pawn.Faction == Faction.OfEntities)
			{
				base.Lord.AddPawn(pawn);
				return;
			}
			Find.LetterStack.ReceiveLetter("ObeliskDuplicationLetterLabel".Translate(), "ObeliskDuplicationLetter".Translate(interactor.Named("PAWN")), LetterDefOf.NeutralEvent, pawn, null, null, null, null, 0, true);
			Messages.Message("ObeliskDuplicationSuccessMessage".Translate(interactor.Named("PAWN")), interactor, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x06010630 RID: 67120 RVA: 0x004BA66C File Offset: 0x004B886C
		public override void OnActivityActivated()
		{
			base.OnActivityActivated();
			this.pointsRemaining *= 0.6f;
			int num = Mathf.CeilToInt(this.pointsRemaining / 35f);
			this.outsideBlastCount = Mathf.CeilToInt((float)num * 0.7f);
			Find.LetterStack.ReceiveLetter("DuplicatorObeliskLetterLabel".Translate(), "DuplicatorObeliskLetter".Translate(), LetterDefOf.ThreatBig, this.parent, null, null, null, null, 0, true);
		}

		// Token: 0x06010631 RID: 67121 RVA: 0x004BA6EC File Offset: 0x004B88EC
		public override void CompTick()
		{
			base.CompTick();
			if (!this.activated)
			{
				return;
			}
			if (base.ActivityComp.Deactivated)
			{
				return;
			}
			if (this.explodeTick > 0)
			{
				return;
			}
			if (Find.TickManager.TicksGame < this.nextSpawnTick)
			{
				return;
			}
			if (!this.warmupComplete)
			{
				return;
			}
			this.nextSpawnTick = Find.TickManager.TicksGame + this.SpawnIntervalTicks.RandomInRange;
			CompObelisk_Duplicator.tmpDuplicateCandidates.Clear();
			bool flag = false;
			if (Find.Storyteller.difficulty.childRaidersAllowed)
			{
				CompObelisk_Duplicator.tmpDuplicateCandidates = (from p in this.parent.Map.mapPawns.FreeColonistsSpawned
				where p.ageTracker.AgeBiologicalYears >= 10
				select p).ToList<Pawn>();
				if (CompObelisk_Duplicator.tmpDuplicateCandidates.Count == 0)
				{
					CompObelisk_Duplicator.tmpDuplicateCandidates = (from p in Find.WorldPawns.AllPawnsAlive
					where p.ageTracker.AgeBiologicalYears >= 10
					select p).ToList<Pawn>();
					flag = true;
				}
			}
			else
			{
				CompObelisk_Duplicator.tmpDuplicateCandidates = this.parent.Map.mapPawns.FreeAdultColonistsSpawned;
				if (CompObelisk_Duplicator.tmpDuplicateCandidates.Count == 0)
				{
					CompObelisk_Duplicator.tmpDuplicateCandidates = (from p in Find.WorldPawns.AllPawnsAlive
					where p.DevelopmentalStage.Adult()
					select p).ToList<Pawn>();
					flag = true;
				}
			}
			if (CompObelisk_Duplicator.tmpDuplicateCandidates.Count == 0)
			{
				return;
			}
			Pawn pawn = CompObelisk_Duplicator.tmpDuplicateCandidates.RandomElement<Pawn>();
			IntVec3 root;
			int squareRadius;
			if (this.spawnedCount < this.outsideBlastCount)
			{
				root = (flag ? this.parent.Position : pawn.Position);
				squareRadius = 20;
			}
			else
			{
				root = this.parent.Position;
				squareRadius = Mathf.FloorToInt(4.9f);
			}
			IntVec3 targetCell;
			Pawn pawn2;
			if (CellFinder.TryFindRandomCellNear(root, this.parent.Map, squareRadius, new Predicate<IntVec3>(this.IsValidSpawnCell), out targetCell, 100) && AnomalyUtility.TryDuplicatePawn(pawn, targetCell, this.parent.Map, out pawn2, Faction.OfEntities, false, false, false))
			{
				base.Lord.AddPawn(pawn2);
				EffecterDefOf.ObeliskSpark.Spawn(this.parent.Position, this.parent.Map, 1f).Cleanup();
				this.pointsRemaining -= CompObelisk_Duplicator.CombatPointsSpawnPointsCurve.Evaluate(this.totalCombatPoints);
				this.spawnedCount++;
				Messages.Message("MessageHostileDuplicate".Translate(pawn), pawn2, MessageTypeDefOf.ThreatBig, true);
			}
			if (this.pointsRemaining <= 0f)
			{
				base.PrepareExplosion();
			}
		}

		// Token: 0x06010632 RID: 67122 RVA: 0x004BA9A8 File Offset: 0x004B8BA8
		private bool IsValidSpawnCell(IntVec3 pos)
		{
			return pos.Standable(this.parent.Map) && pos.Walkable(this.parent.Map) && !pos.Fogged(this.parent.Map);
		}

		// Token: 0x06010633 RID: 67123 RVA: 0x004BA9E6 File Offset: 0x004B8BE6
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.outsideBlastCount, "outsideBlastCount", 0, false);
			Scribe_Values.Look<int>(ref this.spawnedCount, "spawnedCount", 0, false);
		}

		// Token: 0x0400AAF9 RID: 43769
		private int outsideBlastCount;

		// Token: 0x0400AAFA RID: 43770
		private int spawnedCount;

		// Token: 0x0400AAFB RID: 43771
		private static List<Pawn> tmpDuplicateCandidates = new List<Pawn>();

		// Token: 0x0400AAFC RID: 43772
		private const float DuplicateCombatPoints = 35f;

		// Token: 0x0400AAFD RID: 43773
		private const float BlastRadius = 4.9f;

		// Token: 0x0400AAFE RID: 43774
		private const float PercentSpawnedOutsideBlast = 0.7f;

		// Token: 0x0400AAFF RID: 43775
		private const float CombatPointsFactor = 0.6f;

		// Token: 0x0400AB00 RID: 43776
		private const int MinAgeYears = 10;

		// Token: 0x0400AB01 RID: 43777
		private static readonly SimpleCurve CombatPointsSpawnPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(500f, 50f),
				true
			},
			{
				new CurvePoint(1000f, 100f),
				true
			},
			{
				new CurvePoint(5000f, 200f),
				true
			}
		};
	}
}
