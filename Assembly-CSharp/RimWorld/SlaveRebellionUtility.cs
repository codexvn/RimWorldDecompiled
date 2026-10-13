using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200244F RID: 9295
	public static class SlaveRebellionUtility
	{
		// Token: 0x0600C949 RID: 51529 RVA: 0x003ADEBC File Offset: 0x003AC0BC
		public static float InitiateSlaveRebellionMtbDays(Pawn pawn)
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (ticksGame >= SlaveRebellionUtility.pawnMtbCacheNextWipeTick)
			{
				SlaveRebellionUtility.cachedSlaveRebellionMTB.Clear();
				SlaveRebellionUtility.pawnMtbCacheNextWipeTick = ticksGame + 60000;
			}
			if (!pawn.IsSlave)
			{
				return -1f;
			}
			CachedRebellionMtb cachedRebellionMtb = SlaveRebellionUtility.cachedSlaveRebellionMTB.TryGetValue(pawn, null);
			if (cachedRebellionMtb != null && cachedRebellionMtb.gameTick == ticksGame)
			{
				return cachedRebellionMtb.mtb;
			}
			if (cachedRebellionMtb == null)
			{
				cachedRebellionMtb = new CachedRebellionMtb();
				SlaveRebellionUtility.cachedSlaveRebellionMTB[pawn] = cachedRebellionMtb;
			}
			cachedRebellionMtb.gameTick = ticksGame;
			cachedRebellionMtb.mtb = SlaveRebellionUtility.InitiateSlaveRebellionMtbDaysHelper(pawn);
			return cachedRebellionMtb.mtb;
		}

		// Token: 0x0600C94A RID: 51530 RVA: 0x003ADF50 File Offset: 0x003AC150
		private static float InitiateSlaveRebellionMtbDaysHelper(Pawn pawn)
		{
			if (!SlaveRebellionUtility.CanParticipateInSlaveRebellion(pawn))
			{
				return -1f;
			}
			Need_Suppression need_Suppression;
			if (!pawn.needs.TryGetNeed<Need_Suppression>(out need_Suppression))
			{
				return -1f;
			}
			float num = 45f;
			num /= SlaveRebellionUtility.MovingCapacityFactorCurve.Evaluate(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving));
			num /= SlaveRebellionUtility.SuppresionRebellionFactorCurve.Evaluate(need_Suppression.CurLevelPercentage);
			num /= SlaveRebellionUtility.SlaveCountFactorCurve.Evaluate((float)pawn.Map.mapPawns.SlavesOfColonySpawned.Count);
			if (pawn.needs.mood != null)
			{
				num /= SlaveRebellionUtility.MoodRebellionFactorCurve.Evaluate(pawn.needs.mood.CurLevelPercentage);
			}
			if (SlaveRebellionUtility.InRoomTouchingMapEdge(pawn))
			{
				num /= 1.7f;
			}
			if (SlaveRebellionUtility.CanApplyWeaponFactor(pawn))
			{
				num /= 4f;
			}
			if (SlaveRebellionUtility.IsUnattendedByColonists(pawn.Map))
			{
				num /= 20f;
			}
			if (SlaveRebellionUtility.BlissLobotomized(pawn))
			{
				num *= 10f;
			}
			return num;
		}

		// Token: 0x0600C94B RID: 51531 RVA: 0x003AE050 File Offset: 0x003AC250
		public static void IncrementInteractionSuppression(Pawn initiator, Pawn recipient)
		{
			if (initiator == null || recipient == null)
			{
				return;
			}
			Need_Suppression need_Suppression;
			if (recipient.needs == null || !recipient.needs.TryGetNeed<Need_Suppression>(out need_Suppression))
			{
				return;
			}
			float statValue = initiator.GetStatValue(StatDefOf.SuppressionPower, true, -1);
			float num = SlaveRebellionUtility.CurrentSuppressionFactorCurve.Evaluate(need_Suppression.CurLevel);
			SlaveRebellionUtility.IncrementSuppression(need_Suppression, initiator, recipient, statValue * num * need_Suppression.MaxLevel);
		}

		// Token: 0x0600C94C RID: 51532 RVA: 0x003AE0B0 File Offset: 0x003AC2B0
		public static void IncrementMeleeSuppression(Pawn initiator, Pawn recipient, float damageDealt)
		{
			if (initiator == null || recipient == null)
			{
				return;
			}
			Need_Suppression need_Suppression;
			if (recipient.needs == null || !recipient.needs.TryGetNeed<Need_Suppression>(out need_Suppression))
			{
				return;
			}
			float num = SlaveRebellionUtility.CurrentSuppressionFactorCurve.Evaluate(need_Suppression.CurLevel);
			float num2 = 0.2f * damageDealt;
			SlaveRebellionUtility.IncrementSuppression(need_Suppression, initiator, recipient, 0.02f * num * need_Suppression.MaxLevel * num2);
		}

		// Token: 0x0600C94D RID: 51533 RVA: 0x003AE110 File Offset: 0x003AC310
		public static void IncrementSuppression(Need_Suppression suppressionNeed, Pawn initiator, Pawn recipient, float suppressionIncrementPct)
		{
			if (suppressionNeed == null || initiator == null || recipient == null)
			{
				return;
			}
			suppressionNeed.CurLevelPercentage += suppressionIncrementPct;
			TaggedString taggedString = "TextMote_SuppressionIncreased".Translate(suppressionNeed.CurLevel.ToStringPercent());
			MoteMaker.ThrowText((initiator.DrawPos + recipient.DrawPos) / 2f, initiator.Map, taggedString, 4f);
		}

		// Token: 0x0600C94E RID: 51534 RVA: 0x003AE184 File Offset: 0x003AC384
		private static bool InRoomTouchingMapEdge(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_All);
			return room != null && room.TouchesMapEdge;
		}

		// Token: 0x0600C94F RID: 51535 RVA: 0x003AE1A8 File Offset: 0x003AC3A8
		private static bool CanApplyWeaponFactor(Pawn pawn)
		{
			if (pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return false;
			}
			ThingWithComps primary = pawn.equipment.Primary;
			return (primary != null && primary.def.IsWeapon && SlaveRebellionUtility.WeaponUsableInRebellion(primary)) || SlaveRebellionUtility.GoodWeaponInSameRoom(pawn);
		}

		// Token: 0x0600C950 RID: 51536 RVA: 0x003AE1EC File Offset: 0x003AC3EC
		public static bool IsUnattendedByColonists(Map map)
		{
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (!pawn.IsSlave && !pawn.Downed && !pawn.Dead)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C951 RID: 51537 RVA: 0x003AE25C File Offset: 0x003AC45C
		public static bool BlissLobotomized(Pawn p)
		{
			return ModsConfig.AnomalyActive && p.health.hediffSet.HasHediff(HediffDefOf.BlissLobotomy, false);
		}

		// Token: 0x0600C952 RID: 51538 RVA: 0x003AE280 File Offset: 0x003AC480
		public static string GetAnySlaveRebellionExplanation(Pawn pawn)
		{
			int count = pawn.Map.mapPawns.SlavesOfColonySpawned.Count;
			if (count > 1 && SlaveRebellionUtility.RebellionForAnySlaveInMapMtbDays(pawn.Map) != -1f)
			{
				int numTicks = (int)(SlaveRebellionUtility.RebellionForAnySlaveInMapMtbDays(pawn.Map) * 60000f);
				return string.Format("\n{0}", "SuppressionRebellionAnySlave".Translate(count, numTicks.ToStringTicksToPeriod(true, false, true, true, false)));
			}
			return "";
		}

		// Token: 0x0600C953 RID: 51539 RVA: 0x003AE304 File Offset: 0x003AC504
		public static string GetSlaveRebellionMtbCalculationExplanation(Pawn pawn)
		{
			Need_Suppression need_Suppression;
			if (!pawn.needs.TryGetNeed<Need_Suppression>(out need_Suppression) || !SlaveRebellionUtility.CanParticipateInSlaveRebellion(pawn))
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(string.Format("{0}: {1}", "SuppressionBaseInterval".Translate(), 2700000.ToStringTicksToPeriodVague(true, true)));
			float f = 1f / SlaveRebellionUtility.MovingCapacityFactorCurve.Evaluate(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving));
			stringBuilder.AppendLine(string.Format("{0}: x{1}", "SuppressionMovingCapacityFactor".Translate(), f.ToStringPercent()));
			float f2 = 1f / SlaveRebellionUtility.SuppresionRebellionFactorCurve.Evaluate(need_Suppression.CurLevelPercentage);
			stringBuilder.AppendLine(string.Format("{0}: x{1}", "SuppressionFactor".Translate(), f2.ToStringPercent()));
			float f3 = 1f / SlaveRebellionUtility.MoodRebellionFactorCurve.Evaluate(pawn.needs.mood.CurLevelPercentage);
			stringBuilder.AppendLine(string.Format("{0}: x{1}", "SuppressionMoodFactor".Translate(), f3.ToStringPercent()));
			int count = pawn.Map.mapPawns.SlavesOfColonySpawned.Count;
			float f4 = 1f / SlaveRebellionUtility.SlaveCountFactorCurve.Evaluate((float)count);
			stringBuilder.AppendLine(string.Format("{0}: x{1}", "SuppressionSlaveCountFactor".Translate(), f4.ToStringPercent()));
			if (SlaveRebellionUtility.InRoomTouchingMapEdge(pawn))
			{
				float f5 = 0.58823526f;
				stringBuilder.AppendLine(string.Format("{0}: x{1}", "SuppressionEscapeFactor".Translate(), f5.ToStringPercent()));
			}
			if (SlaveRebellionUtility.CanApplyWeaponFactor(pawn))
			{
				float f6 = 0.25f;
				stringBuilder.AppendLine(string.Format("{0}: x{1}", "SuppressionWeaponProximityFactor".Translate(), f6.ToStringPercent()));
			}
			if (SlaveRebellionUtility.IsUnattendedByColonists(pawn.Map))
			{
				float f7 = 0.05f;
				stringBuilder.AppendLine(string.Format("{0}: x{1}", "SuppressionUnattendedByColonists".Translate(), f7.ToStringPercent()));
			}
			if (SlaveRebellionUtility.BlissLobotomized(pawn))
			{
				stringBuilder.AppendLine(string.Format("{0}: x{1}", "BlissLobotomy".Translate(), 10f.ToStringPercent()));
			}
			stringBuilder.Append(string.Format("{0}: {1}", "SuppressionFinalInterval".Translate(), ((int)(SlaveRebellionUtility.InitiateSlaveRebellionMtbDays(pawn) * 60000f)).ToStringTicksToPeriod(true, false, true, true, false)));
			return stringBuilder.ToString();
		}

		// Token: 0x0600C954 RID: 51540 RVA: 0x003AE59C File Offset: 0x003AC79C
		private static bool IsDamagelessWeapon(Thing t)
		{
			ProjectileProperties projectileProperties;
			if (t == null)
			{
				projectileProperties = null;
			}
			else
			{
				ThingDef def = t.def;
				if (def == null)
				{
					projectileProperties = null;
				}
				else
				{
					List<VerbProperties> verbs = def.Verbs;
					if (verbs == null)
					{
						projectileProperties = null;
					}
					else
					{
						VerbProperties verbProperties = verbs.FirstOrDefault((VerbProperties v) => v.isPrimary);
						if (verbProperties == null)
						{
							projectileProperties = null;
						}
						else
						{
							ThingDef defaultProjectile = verbProperties.defaultProjectile;
							projectileProperties = ((defaultProjectile != null) ? defaultProjectile.projectile : null);
						}
					}
				}
			}
			ProjectileProperties projectileProperties2 = projectileProperties;
			if (projectileProperties2 != null && projectileProperties2.GetDamageAmount(null, null) == 0)
			{
				return true;
			}
			if (projectileProperties2 == null)
			{
				return false;
			}
			DamageDef damageDef = projectileProperties2.damageDef;
			bool? flag = (damageDef != null) ? new bool?(damageDef.harmsHealth) : null;
			bool flag2 = false;
			return flag.GetValueOrDefault() == flag2 & flag != null;
		}

		// Token: 0x0600C955 RID: 51541 RVA: 0x003AE64D File Offset: 0x003AC84D
		public static bool WeaponUsableInRebellion(Thing weapon)
		{
			return weapon != null && !SlaveRebellionUtility.IsDamagelessWeapon(weapon) && weapon.def != ThingDefOf.WoodLog && !PawnWeaponGenerator.IsDerpWeapon(weapon.def, weapon.Stuff);
		}

		// Token: 0x0600C956 RID: 51542 RVA: 0x003AE680 File Offset: 0x003AC880
		public static float RebellionForAnySlaveInMapMtbDays(Map m)
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (ticksGame >= SlaveRebellionUtility.mapMtbCacheNextWipeTick)
			{
				SlaveRebellionUtility.cachedCombinedSlaveRebellionMTB.Clear();
				SlaveRebellionUtility.mapMtbCacheNextWipeTick = ticksGame + 60000;
			}
			CachedRebellionMtb cachedRebellionMtb = SlaveRebellionUtility.cachedCombinedSlaveRebellionMTB.TryGetValue(m, null);
			if (cachedRebellionMtb != null && cachedRebellionMtb.gameTick == ticksGame)
			{
				return cachedRebellionMtb.mtb;
			}
			if (cachedRebellionMtb == null)
			{
				cachedRebellionMtb = new CachedRebellionMtb();
				SlaveRebellionUtility.cachedCombinedSlaveRebellionMTB[m] = cachedRebellionMtb;
			}
			cachedRebellionMtb.gameTick = ticksGame;
			cachedRebellionMtb.mtb = SlaveRebellionUtility.RebellionForAnySlaveInMapMtbDaysHelper(m);
			return cachedRebellionMtb.mtb;
		}

		// Token: 0x0600C957 RID: 51543 RVA: 0x003AE704 File Offset: 0x003AC904
		private static float RebellionForAnySlaveInMapMtbDaysHelper(Map m)
		{
			SlaveRebellionUtility.computeCombinedMTBs.Clear();
			foreach (Pawn pawn in m.mapPawns.SlavesOfColonySpawned)
			{
				float num = SlaveRebellionUtility.InitiateSlaveRebellionMtbDays(pawn);
				if (num != -1f)
				{
					SlaveRebellionUtility.computeCombinedMTBs.Add(num);
				}
			}
			if (SlaveRebellionUtility.computeCombinedMTBs.Count <= 0)
			{
				return -1f;
			}
			return Rand.CombineMTBs(SlaveRebellionUtility.computeCombinedMTBs);
		}

		// Token: 0x0600C958 RID: 51544 RVA: 0x003AE794 File Offset: 0x003AC994
		private static bool GoodWeaponInSameRoom(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_All);
			if (room == null || room.PsychologicallyOutdoors)
			{
				return false;
			}
			ThingRequest thingReq = ThingRequest.ForGroup(ThingRequestGroup.Weapon);
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, thingReq, PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 6.9f, (Thing t) => EquipmentUtility.CanEquip(t, pawn) && SlaveRebellionUtility.WeaponUsableInRebellion(t) && t.GetRoom(RegionType.Set_All) == room, null, 0, -1, false, RegionType.Set_Passable, false, false) != null;
		}

		// Token: 0x0600C959 RID: 51545 RVA: 0x003AE82C File Offset: 0x003ACA2C
		public static bool CanParticipateInSlaveRebellion(Pawn pawn)
		{
			return pawn.Spawned && (!pawn.Map.IsPocketMap || !pawn.Map.generatorDef.pocketMapProperties.preventPrisonerEscape) && !pawn.Map.Biome.inVacuum && (!pawn.Downed && pawn.Spawned && pawn.IsSlave && !pawn.InMentalState && pawn.Awake()) && !SlaveRebellionUtility.IsRebelling(pawn);
		}

		// Token: 0x0600C95A RID: 51546 RVA: 0x003AE8B0 File Offset: 0x003ACAB0
		public static bool IsRebelling(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && lord.LordJob is LordJob_SlaveRebellion;
		}

		// Token: 0x0600C95B RID: 51547 RVA: 0x003AE8D8 File Offset: 0x003ACAD8
		public static bool StartSlaveRebellion(Pawn initiator, bool forceAggressive = false)
		{
			string str;
			string str2;
			LetterDef textLetterDef;
			LookTargets lookTargets;
			if (SlaveRebellionUtility.StartSlaveRebellion(initiator, out str, out str2, out textLetterDef, out lookTargets, forceAggressive))
			{
				Find.LetterStack.ReceiveLetter(str2, str, textLetterDef, lookTargets, null, null, null, null, 0, true);
				return true;
			}
			return false;
		}

		// Token: 0x0600C95C RID: 51548 RVA: 0x003AE918 File Offset: 0x003ACB18
		public static bool StartSlaveRebellion(Pawn initiator, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets, bool forceAggressive = false)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
			if (!ModLister.CheckIdeology("Slave rebellion"))
			{
				return false;
			}
			SlaveRebellionUtility.rebellingSlaves.Clear();
			SlaveRebellionUtility.rebellingSlaves.Add(initiator);
			SlaveRebellionUtility.allPossibleRebellingSlaves.Clear();
			List<Pawn> slavesOfColonySpawned = initiator.Map.mapPawns.SlavesOfColonySpawned;
			for (int i = 0; i < slavesOfColonySpawned.Count; i++)
			{
				Pawn pawn = slavesOfColonySpawned[i];
				if (pawn != initiator && SlaveRebellionUtility.CanParticipateInSlaveRebellion(pawn))
				{
					SlaveRebellionUtility.allPossibleRebellingSlaves.Add(pawn);
				}
			}
			SlaveRebellionUtility.SlaveRebellionType slaveRebellionType = SlaveRebellionUtility.DecideSlaveRebellionType();
			if (slaveRebellionType != SlaveRebellionUtility.SlaveRebellionType.GrandRebellion)
			{
				if (slaveRebellionType == SlaveRebellionUtility.SlaveRebellionType.LocalRebellion)
				{
					for (int j = 0; j < SlaveRebellionUtility.allPossibleRebellingSlaves.Count; j++)
					{
						Pawn pawn2 = SlaveRebellionUtility.allPossibleRebellingSlaves[j];
						if (initiator.Position.DistanceTo(pawn2.Position) <= 35f)
						{
							SlaveRebellionUtility.rebellingSlaves.Add(pawn2);
						}
					}
				}
			}
			else
			{
				for (int k = 0; k < SlaveRebellionUtility.allPossibleRebellingSlaves.Count; k++)
				{
					SlaveRebellionUtility.rebellingSlaves.Add(SlaveRebellionUtility.allPossibleRebellingSlaves[k]);
				}
			}
			if (SlaveRebellionUtility.rebellingSlaves.Count == 1)
			{
				slaveRebellionType = SlaveRebellionUtility.SlaveRebellionType.SingleRebellion;
			}
			else if (SlaveRebellionUtility.rebellingSlaves.Count == SlaveRebellionUtility.allPossibleRebellingSlaves.Count)
			{
				slaveRebellionType = SlaveRebellionUtility.SlaveRebellionType.GrandRebellion;
			}
			IntVec3 exitPoint;
			if (!RCellFinder.TryFindRandomExitSpot(initiator, out exitPoint, TraverseMode.PassDoors))
			{
				return false;
			}
			IntVec3 groupUpLoc;
			if (!PrisonBreakUtility.TryFindGroupUpLoc(SlaveRebellionUtility.rebellingSlaves, exitPoint, out groupUpLoc))
			{
				return false;
			}
			bool flag = forceAggressive || Rand.Chance(0.5f);
			switch (slaveRebellionType)
			{
			case SlaveRebellionUtility.SlaveRebellionType.GrandRebellion:
				if (flag)
				{
					letterLabel = "LetterLabelGrandSlaveRebellion".Translate();
					letterText = "LetterGrandSlaveRebellion".Translate(GenLabel.ThingsLabel(SlaveRebellionUtility.rebellingSlaves, "  - "));
				}
				else
				{
					letterLabel = "LetterLabelGrandSlaveEscape".Translate();
					letterText = "LetterGrandSlaveEscape".Translate(GenLabel.ThingsLabel(SlaveRebellionUtility.rebellingSlaves, "  - "));
				}
				break;
			case SlaveRebellionUtility.SlaveRebellionType.LocalRebellion:
				if (flag)
				{
					letterLabel = "LetterLabelLocalSlaveRebellion".Translate();
					letterText = "LetterLocalSlaveRebellion".Translate(initiator, GenLabel.ThingsLabel(SlaveRebellionUtility.rebellingSlaves, "  - "));
				}
				else
				{
					letterLabel = "LetterLabelLocalSlaveEscape".Translate();
					letterText = "LetterLocalSlaveEscape".Translate(initiator, GenLabel.ThingsLabel(SlaveRebellionUtility.rebellingSlaves, "  - "));
				}
				break;
			case SlaveRebellionUtility.SlaveRebellionType.SingleRebellion:
				if (flag)
				{
					letterLabel = "LetterLabelSingleSlaveRebellion".Translate() + (": " + initiator.LabelShort);
					letterText = "LetterSingleSlaveRebellion".Translate(initiator);
				}
				else
				{
					letterLabel = "LetterLabelSingleSlaveEscape".Translate() + (": " + initiator.LabelShort);
					letterText = "LetterSingleSlaveEscape".Translate(initiator);
				}
				break;
			default:
				Log.Error(string.Format("Unkown slave rebellion type {0}", slaveRebellionType));
				break;
			}
			letterText += "\n\n" + "SlaveRebellionSuppressionExplanation".Translate();
			lookTargets = new LookTargets(SlaveRebellionUtility.rebellingSlaves);
			letterDef = LetterDefOf.ThreatBig;
			int sapperThingID = -1;
			if (Rand.Value < 0.5f)
			{
				sapperThingID = initiator.thingIDNumber;
			}
			for (int l = 0; l < SlaveRebellionUtility.rebellingSlaves.Count; l++)
			{
				Lord lord = SlaveRebellionUtility.rebellingSlaves[l].GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(SlaveRebellionUtility.rebellingSlaves[l], PawnLostCondition.ForcedToJoinOtherLord, null);
				}
			}
			LordMaker.MakeNewLord(SlaveRebellionUtility.rebellingSlaves[0].Faction, new LordJob_SlaveRebellion(groupUpLoc, exitPoint, sapperThingID, !flag), initiator.Map, SlaveRebellionUtility.rebellingSlaves);
			for (int m = 0; m < SlaveRebellionUtility.rebellingSlaves.Count; m++)
			{
				if (!SlaveRebellionUtility.rebellingSlaves[m].Awake())
				{
					RestUtility.WakeUp(SlaveRebellionUtility.rebellingSlaves[m], true);
				}
				SlaveRebellionUtility.rebellingSlaves[m].drafter.Drafted = false;
				if (SlaveRebellionUtility.rebellingSlaves[m].CurJob != null)
				{
					SlaveRebellionUtility.rebellingSlaves[m].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				SlaveRebellionUtility.rebellingSlaves[m].Map.attackTargetsCache.UpdateTarget(SlaveRebellionUtility.rebellingSlaves[m]);
				if (SlaveRebellionUtility.rebellingSlaves[m].carryTracker.CarriedThing != null)
				{
					Thing thing;
					SlaveRebellionUtility.rebellingSlaves[m].carryTracker.TryDropCarriedThing(SlaveRebellionUtility.rebellingSlaves[m].Position, ThingPlaceMode.Near, out thing, null);
				}
			}
			SlaveRebellionUtility.rebellingSlaves.Clear();
			return true;
		}

		// Token: 0x0600C95D RID: 51549 RVA: 0x003AEE06 File Offset: 0x003AD006
		private static SlaveRebellionUtility.SlaveRebellionType DecideSlaveRebellionType()
		{
			return Enum.GetValues(typeof(SlaveRebellionUtility.SlaveRebellionType)).Cast<SlaveRebellionUtility.SlaveRebellionType>().RandomElement<SlaveRebellionUtility.SlaveRebellionType>();
		}

		// Token: 0x0600C95E RID: 51550 RVA: 0x003AEE24 File Offset: 0x003AD024
		public static Pawn FindSlaveForRebellion(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return null;
			}
			SlaveRebellionUtility.tmpSlaves.Clear();
			IReadOnlyList<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn2 = allPawnsSpawned[i];
				if (pawn2.IsSlave && pawn2.SlaveFaction == pawn.SlaveFaction && pawn2 != pawn && !pawn2.Downed && !pawn2.InMentalState && !pawn2.IsBurning() && pawn2.Awake() && SlaveRebellionUtility.CanParticipateInSlaveRebellion(pawn2) && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					SlaveRebellionUtility.tmpSlaves.Add(pawn2);
				}
			}
			if (!SlaveRebellionUtility.tmpSlaves.Any<Pawn>())
			{
				return null;
			}
			Pawn result = SlaveRebellionUtility.tmpSlaves.RandomElement<Pawn>();
			SlaveRebellionUtility.tmpSlaves.Clear();
			return result;
		}

		// Token: 0x0600C95F RID: 51551 RVA: 0x003AEEF1 File Offset: 0x003AD0F1
		public static void ClearCache()
		{
			SlaveRebellionUtility.cachedSlaveRebellionMTB.Clear();
			SlaveRebellionUtility.cachedCombinedSlaveRebellionMTB.Clear();
			SlaveRebellionUtility.pawnMtbCacheNextWipeTick = 0;
			SlaveRebellionUtility.mapMtbCacheNextWipeTick = 0;
		}

		// Token: 0x04008902 RID: 35074
		private static List<float> computeCombinedMTBs = new List<float>();

		// Token: 0x04008903 RID: 35075
		private static Dictionary<Pawn, CachedRebellionMtb> cachedSlaveRebellionMTB = new Dictionary<Pawn, CachedRebellionMtb>();

		// Token: 0x04008904 RID: 35076
		private static Dictionary<Map, CachedRebellionMtb> cachedCombinedSlaveRebellionMTB = new Dictionary<Map, CachedRebellionMtb>();

		// Token: 0x04008905 RID: 35077
		private static int pawnMtbCacheNextWipeTick = 0;

		// Token: 0x04008906 RID: 35078
		private static int mapMtbCacheNextWipeTick = 0;

		// Token: 0x04008907 RID: 35079
		private const int cacheWipeIntervalTicks = 60000;

		// Token: 0x04008908 RID: 35080
		private const int MaxRegionsForDoorCount = 25;

		// Token: 0x04008909 RID: 35081
		private const float WontRebelMTB = -1f;

		// Token: 0x0400890A RID: 35082
		private const float BaseInitiateSlaveRebellionMtbDays = 45f;

		// Token: 0x0400890B RID: 35083
		private const float WeaponProximityMultiplier = 4f;

		// Token: 0x0400890C RID: 35084
		private const float WeaponMaxDistanceForProximityMultiplier = 6.9f;

		// Token: 0x0400890D RID: 35085
		private const float RoomTouchesMapEdge = 1.7f;

		// Token: 0x0400890E RID: 35086
		private const float UnattendedByColonistsMultiplier = 20f;

		// Token: 0x0400890F RID: 35087
		private const float BlissLobotomyMultiplier = 10f;

		// Token: 0x04008910 RID: 35088
		private static readonly SimpleCurve MovingCapacityFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.01f),
				true
			},
			{
				new CurvePoint(0.5f, 0.5f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			}
		};

		// Token: 0x04008911 RID: 35089
		private static readonly SimpleCurve SuppresionRebellionFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 5f),
				true
			},
			{
				new CurvePoint(0.333f, 1.5f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.25f),
				true
			}
		};

		// Token: 0x04008912 RID: 35090
		private static readonly SimpleCurve MoodRebellionFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1.5f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.8f),
				true
			}
		};

		// Token: 0x04008913 RID: 35091
		private static readonly SimpleCurve SlaveCountFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(2f, 0.75f),
				true
			},
			{
				new CurvePoint(5f, 0.5f),
				true
			},
			{
				new CurvePoint(10f, 0.3f),
				true
			},
			{
				new CurvePoint(20f, 0.2f),
				true
			}
		};

		// Token: 0x04008914 RID: 35092
		private const float SapperChance = 0.5f;

		// Token: 0x04008915 RID: 35093
		private const float LocalRebellionSearchDistance = 35f;

		// Token: 0x04008916 RID: 35094
		private const float AggressiveRebellionChance = 0.5f;

		// Token: 0x04008917 RID: 35095
		private const float BaseMeleeSlaveSuppressionPct = 0.02f;

		// Token: 0x04008918 RID: 35096
		private const float DamageSuppressionMultiplier = 0.2f;

		// Token: 0x04008919 RID: 35097
		private static readonly SimpleCurve CurrentSuppressionFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 2f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.5f),
				true
			}
		};

		// Token: 0x0400891A RID: 35098
		private static List<Pawn> rebellingSlaves = new List<Pawn>();

		// Token: 0x0400891B RID: 35099
		private static List<Pawn> allPossibleRebellingSlaves = new List<Pawn>();

		// Token: 0x0400891C RID: 35100
		private static List<Pawn> tmpSlaves = new List<Pawn>();

		// Token: 0x02002450 RID: 9296
		private enum SlaveRebellionType
		{
			// Token: 0x0400891E RID: 35102
			GrandRebellion,
			// Token: 0x0400891F RID: 35103
			LocalRebellion,
			// Token: 0x04008920 RID: 35104
			SingleRebellion
		}
	}
}
