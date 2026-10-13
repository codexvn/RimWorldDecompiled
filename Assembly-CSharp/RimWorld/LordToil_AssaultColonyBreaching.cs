using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016E3 RID: 5859
	public class LordToil_AssaultColonyBreaching : LordToil
	{
		// Token: 0x17001536 RID: 5430
		// (get) Token: 0x06008A45 RID: 35397 RVA: 0x00293D8C File Offset: 0x00291F8C
		public LordToilData_AssaultColonyBreaching Data
		{
			get
			{
				if (this.data == null)
				{
					this.data = new LordToilData_AssaultColonyBreaching(this.lord);
				}
				return (LordToilData_AssaultColonyBreaching)this.data;
			}
		}

		// Token: 0x17001537 RID: 5431
		// (get) Token: 0x06008A46 RID: 35398 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001538 RID: 5432
		// (get) Token: 0x06008A47 RID: 35399 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008A49 RID: 35401 RVA: 0x00293AB7 File Offset: 0x00291CB7
		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		// Token: 0x06008A4A RID: 35402 RVA: 0x00293E09 File Offset: 0x00292009
		public override void Notify_ReachedDutyLocation(Pawn pawn)
		{
			this.Data.Reset();
			this.UpdateAllDuties();
		}

		// Token: 0x06008A4B RID: 35403 RVA: 0x00293E1C File Offset: 0x0029201C
		public override void Notify_BuildingSpawnedOnMap(Building b)
		{
			this.Data.breachingGrid.Notify_BuildingStateChanged(b);
		}

		// Token: 0x06008A4C RID: 35404 RVA: 0x00293E1C File Offset: 0x0029201C
		public override void Notify_BuildingDespawnedOnMap(Building b)
		{
			this.Data.breachingGrid.Notify_BuildingStateChanged(b);
		}

		// Token: 0x06008A4D RID: 35405 RVA: 0x00293E2F File Offset: 0x0029202F
		public override void LordToilTick()
		{
			if (this.lord.ticksInToil % 300 == 0)
			{
				this.UpdateAllDuties();
			}
		}

		// Token: 0x06008A4E RID: 35406 RVA: 0x00293E4C File Offset: 0x0029204C
		public override void UpdateAllDuties()
		{
			if (!this.lord.ownedPawns.Any<Pawn>())
			{
				return;
			}
			if (!this.Data.breachDest.IsValid)
			{
				this.Data.Reset();
				this.Data.preferMelee = Rand.Chance(0.5f);
				this.Data.breachStart = this.lord.ownedPawns[0].PositionHeld;
				this.Data.breachDest = GenAI.RandomRaidDest(this.Data.breachStart, base.Map);
				int breachRadius = Mathf.RoundToInt(LordToil_AssaultColonyBreaching.BreachRadiusFromNumRaiders.Evaluate((float)this.lord.ownedPawns.Count));
				int walkMargin = Mathf.RoundToInt(LordToil_AssaultColonyBreaching.WalkMarginFromNumRaiders.Evaluate((float)this.lord.ownedPawns.Count));
				this.Data.breachingGrid.CreateBreachPath(this.Data.breachStart, this.Data.breachDest, breachRadius, walkMargin, this.useAvoidGrid);
			}
			this.pawnsRangedDestructive.Clear();
			this.pawnsMeleeDestructive.Clear();
			this.pawnsRangedGeneral.Clear();
			this.pawnSoloAttackers.Clear();
			this.pawnsEscort.Clear();
			this.pawnsLost.Clear();
			this.Data.maxRange = 12f;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (!pawn.CanReach(this.Data.breachStart, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					this.pawnsLost.Add(pawn);
				}
				else
				{
					Verb verb = BreachingUtility.FindVerbToUseForBreaching(pawn);
					if (verb == null)
					{
						this.pawnsEscort.Add(pawn);
					}
					else if (!pawn.RaceProps.IsMechanoid && BreachingUtility.IsSoloAttackVerb(verb))
					{
						this.pawnSoloAttackers.Add(pawn);
					}
					else if (verb.verbProps.ai_IsBuildingDestroyer)
					{
						if (verb.IsMeleeAttack)
						{
							this.pawnsMeleeDestructive.Add(pawn);
						}
						else
						{
							this.pawnsRangedDestructive.Add(pawn);
							this.Data.maxRange = Math.Min(this.Data.maxRange, verb.EffectiveRange);
						}
					}
					else if (verb.IsMeleeAttack)
					{
						this.pawnsEscort.Add(pawn);
					}
					else
					{
						this.pawnsRangedGeneral.Add(pawn);
					}
				}
			}
			bool flag = this.pawnsMeleeDestructive.Any<Pawn>();
			bool flag2 = this.pawnsRangedDestructive.Any<Pawn>();
			if (flag && (!flag2 || this.Data.preferMelee))
			{
				LordToil_AssaultColonyBreaching.BalanceAndSetDuties(this.Data.breachDest, this.pawnsMeleeDestructive, this.pawnSoloAttackers, this.pawnsRangedDestructive, this.pawnsRangedGeneral, this.pawnsEscort);
				LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsLost);
				return;
			}
			if (flag2)
			{
				LordToil_AssaultColonyBreaching.BalanceAndSetDuties(this.Data.breachDest, this.pawnsRangedDestructive, this.pawnSoloAttackers, this.pawnsRangedGeneral, this.pawnsMeleeDestructive, this.pawnsEscort);
				LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsLost);
				return;
			}
			if (this.pawnsRangedGeneral.Any<Pawn>())
			{
				LordToil_AssaultColonyBreaching.BalanceAndSetDuties(this.Data.breachDest, this.pawnsRangedGeneral, this.pawnSoloAttackers, this.pawnsMeleeDestructive, this.pawnsRangedDestructive, this.pawnsEscort);
				LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsLost);
				return;
			}
			LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsMeleeDestructive);
			LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsRangedDestructive);
			LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsRangedGeneral);
			LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnSoloAttackers);
			LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsEscort);
			LordToil_AssaultColonyBreaching.SetBackupDuty(this.pawnsLost);
		}

		// Token: 0x06008A4F RID: 35407 RVA: 0x002941F4 File Offset: 0x002923F4
		private static void BalanceAndSetDuties(IntVec3 breachDest, List<Pawn> breachers, List<Pawn> soloAttackers, List<Pawn> escorts1, List<Pawn> escorts2, List<Pawn> escorts3)
		{
			if (!escorts1.Any<Pawn>() && !escorts2.Any<Pawn>() && !escorts3.Any<Pawn>())
			{
				if (soloAttackers.Any<Pawn>())
				{
					escorts3.AddRange(soloAttackers);
					soloAttackers.Clear();
				}
				else if (breachers.Count > 1)
				{
					Pawn item = breachers.First<Pawn>();
					breachers.Remove(item);
					escorts3.Add(item);
				}
			}
			LordToil_AssaultColonyBreaching.SetBreachDuty(breachers, breachDest);
			LordToil_AssaultColonyBreaching.SetSoloAttackDuty(soloAttackers, breachDest);
			LordToil_AssaultColonyBreaching.SetEscortDuty(escorts1, breachers);
			LordToil_AssaultColonyBreaching.SetEscortDuty(escorts2, breachers);
			LordToil_AssaultColonyBreaching.SetEscortDuty(escorts3, breachers);
		}

		// Token: 0x06008A50 RID: 35408 RVA: 0x00294278 File Offset: 0x00292478
		private static void SetBackupDuty(List<Pawn> pawns)
		{
			for (int i = 0; i < pawns.Count; i++)
			{
				pawns[i].mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
			}
		}

		// Token: 0x06008A51 RID: 35409 RVA: 0x002942B4 File Offset: 0x002924B4
		private static void SetEscortDuty(List<Pawn> escorts, List<Pawn> targets)
		{
			for (int i = 0; i < escorts.Count; i++)
			{
				Pawn pawn = escorts[i];
				pawn.mindState.duty = new PawnDuty(DutyDefOf.Escort, targets.RandomElement<Pawn>(), BreachingUtility.EscortRadius(pawn));
			}
		}

		// Token: 0x06008A52 RID: 35410 RVA: 0x00294300 File Offset: 0x00292500
		private static void SetBreachDuty(List<Pawn> breachers, IntVec3 breachDest)
		{
			for (int i = 0; i < breachers.Count; i++)
			{
				breachers[i].mindState.duty = new PawnDuty(DutyDefOf.Breaching, breachDest, -1f);
			}
		}

		// Token: 0x06008A53 RID: 35411 RVA: 0x00294344 File Offset: 0x00292544
		private static void SetSoloAttackDuty(List<Pawn> breachers, IntVec3 breachDest)
		{
			for (int i = 0; i < breachers.Count; i++)
			{
				breachers[i].mindState.duty = new PawnDuty(DutyDefOf.Breaching, breachDest, -1f);
			}
		}

		// Token: 0x06008A54 RID: 35412 RVA: 0x00294388 File Offset: 0x00292588
		public void UpdateCurrentBreachTarget()
		{
			if (this.Data.currentTarget != null && this.Data.currentTarget.Destroyed)
			{
				this.Data.currentTarget = null;
			}
			if (this.Data.soloAttacker != null && BreachingUtility.FindVerbToUseForBreaching(this.Data.soloAttacker) == null)
			{
				this.Data.currentTarget = null;
			}
			if (this.Data.currentTarget == null)
			{
				this.Data.currentTarget = this.Data.breachingGrid.FindBuildingToBreach();
				this.Data.soloAttacker = null;
				if (this.pawnSoloAttackers.Any<Pawn>() && BreachingUtility.CanSoloAttackTargetBuilding(this.Data.currentTarget) && Rand.Chance(0.2f))
				{
					Pawn pawn = this.pawnSoloAttackers.RandomElement<Pawn>();
					if (BreachingUtility.FindVerbToUseForBreaching(pawn) != null)
					{
						this.Data.soloAttacker = pawn;
					}
				}
			}
		}

		// Token: 0x04005A22 RID: 23074
		private const int UpdateIntervalTicks = 300;

		// Token: 0x04005A23 RID: 23075
		private const float PreferMeleeChance = 0.5f;

		// Token: 0x04005A24 RID: 23076
		private const float UseSoloAttackOnTargetChance = 0.2f;

		// Token: 0x04005A25 RID: 23077
		public const float MaxRangeForShooters = 12f;

		// Token: 0x04005A26 RID: 23078
		private static readonly SimpleCurve BreachRadiusFromNumRaiders = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(120f, 2f),
				true
			}
		};

		// Token: 0x04005A27 RID: 23079
		private static readonly SimpleCurve WalkMarginFromNumRaiders = new SimpleCurve
		{
			{
				new CurvePoint(0f, 2f),
				true
			},
			{
				new CurvePoint(60f, 4f),
				true
			}
		};

		// Token: 0x04005A28 RID: 23080
		private List<Pawn> pawnsRangedDestructive = new List<Pawn>();

		// Token: 0x04005A29 RID: 23081
		private List<Pawn> pawnsMeleeDestructive = new List<Pawn>();

		// Token: 0x04005A2A RID: 23082
		private List<Pawn> pawnsRangedGeneral = new List<Pawn>();

		// Token: 0x04005A2B RID: 23083
		private List<Pawn> pawnSoloAttackers = new List<Pawn>();

		// Token: 0x04005A2C RID: 23084
		private List<Pawn> pawnsEscort = new List<Pawn>();

		// Token: 0x04005A2D RID: 23085
		private List<Pawn> pawnsLost = new List<Pawn>();
	}
}
