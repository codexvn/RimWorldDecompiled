using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002464 RID: 9316
	public static class MutantUtility
	{
		// Token: 0x0600C9F2 RID: 51698 RVA: 0x003B1B78 File Offset: 0x003AFD78
		public static bool CanResurrectAsShambler(Corpse corpse, bool ignoreIndoors = false)
		{
			if (((corpse != null) ? corpse.InnerPawn : null) == null)
			{
				return false;
			}
			if (!corpse.InnerPawn.RaceProps.IsFlesh)
			{
				return false;
			}
			if (!corpse.InnerPawn.RaceProps.canBecomeShambler)
			{
				return false;
			}
			if (corpse.InnerPawn.IsMutant)
			{
				return false;
			}
			if (corpse is UnnaturalCorpse)
			{
				return false;
			}
			Room room = corpse.PositionHeld.GetRoom(corpse.MapHeld);
			if (room != null && !ignoreIndoors && corpse.PositionHeld.Roofed(corpse.MapHeld) && (room.ProperRoom || room.IsDoorway))
			{
				return false;
			}
			if (!Find.Storyteller.difficulty.childShamblersAllowed && !corpse.InnerPawn.ageTracker.Adult)
			{
				return false;
			}
			Hediff_DeathRefusal firstHediff = corpse.InnerPawn.health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
			return firstHediff == null || (!firstHediff.InProgress && firstHediff.UsesLeft <= 0);
		}

		// Token: 0x0600C9F3 RID: 51699 RVA: 0x003B1C68 File Offset: 0x003AFE68
		public static void ResurrectAsShambler(Pawn pawn, int lifespanTicks = -1, Faction faction = null)
		{
			RotStage rotStage = pawn.Corpse.GetRotStage();
			pawn.mutant = new Pawn_MutantTracker(pawn, MutantDefOf.Shambler, rotStage);
			Hediff_Shambler hediff_Shambler = pawn.health.AddHediff(MutantDefOf.Shambler.hediff, null, null, null) as Hediff_Shambler;
			HediffComp_DisappearsAndKills hediffComp_DisappearsAndKills = hediff_Shambler.TryGetComp<HediffComp_DisappearsAndKills>();
			if (hediffComp_DisappearsAndKills != null)
			{
				if (lifespanTicks > 0)
				{
					hediffComp_DisappearsAndKills.disappearsAfterTicks = lifespanTicks;
					hediffComp_DisappearsAndKills.ticksToDisappear = lifespanTicks;
				}
				else
				{
					hediffComp_DisappearsAndKills.disabled = true;
				}
			}
			if (hediff_Shambler != null)
			{
				hediff_Shambler.StartRising(lifespanTicks);
			}
			if (faction == null && MutantDefOf.Shambler.defaultFaction != null)
			{
				faction = Find.FactionManager.FirstFactionOfDef(MutantDefOf.Shambler.defaultFaction);
			}
			if (faction != null && pawn.Faction != faction)
			{
				pawn.SetFaction(faction, null);
			}
		}

		// Token: 0x0600C9F4 RID: 51700 RVA: 0x003B1D24 File Offset: 0x003AFF24
		public static void SetFreshPawnAsMutant(Pawn pawn, MutantDef mutant)
		{
			RotStage rotStage = mutant.useCorpseGraphics ? Gen.RandomEnumValue<RotStage>(false) : RotStage.Fresh;
			Pawn_EquipmentTracker equipment = pawn.equipment;
			if (((equipment != null) ? equipment.Primary : null) != null)
			{
				pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
			}
			if (ModsConfig.BiotechActive && pawn.genes != null && pawn.genes.Xenotype != XenotypeDefOf.Baseliner && rotStage == RotStage.Dessicated)
			{
				rotStage = RotStage.Fresh;
			}
			if (rotStage == RotStage.Dessicated)
			{
				Pawn_ApparelTracker apparel = pawn.apparel;
				if (apparel != null)
				{
					apparel.DestroyAll(DestroyMode.Vanish);
				}
			}
			MutantUtility.SetPawnAsMutantInstantly(pawn, mutant, rotStage);
		}

		// Token: 0x0600C9F5 RID: 51701 RVA: 0x003B1DB4 File Offset: 0x003AFFB4
		public static void SetPawnAsMutantInstantly(Pawn pawn, MutantDef mutant, RotStage rotStage = RotStage.Fresh)
		{
			pawn.mutant = new Pawn_MutantTracker(pawn, mutant, rotStage);
			pawn.mutant.Turn(true);
			if (mutant.defaultFaction != null)
			{
				Faction faction = Find.FactionManager.FirstFactionOfDef(mutant.defaultFaction);
				if (faction != null && pawn.Faction != faction)
				{
					pawn.SetFaction(faction, null);
				}
			}
			MutantUtility.RegenerateHealth(pawn);
		}

		// Token: 0x0600C9F6 RID: 51702 RVA: 0x003B1E10 File Offset: 0x003B0010
		public static Thing FindShamblerTarget(Pawn pawn)
		{
			TargetScanFlags flags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedReachable | TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable;
			Thing thing = null;
			if (thing == null)
			{
				thing = (Thing)AttackTargetFinder.BestAttackTarget(pawn, flags, null, 0f, 20f, default(IntVec3), float.MaxValue, true, true, true, false);
			}
			return thing;
		}

		// Token: 0x0600C9F7 RID: 51703 RVA: 0x003B1E54 File Offset: 0x003B0054
		public static void ActivateNearbyShamblers(Pawn pawn, Thing target)
		{
			if (!pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			foreach (Pawn pawn2 in pawn.MapHeld.mapPawns.SpawnedShamblers)
			{
				if (pawn.Faction == pawn2.Faction && pawn.Position.InHorDistOf(pawn2.Position, 10f))
				{
					Hediff_Shambler hediff_Shambler = pawn2.mutant.Hediff as Hediff_Shambler;
					if (hediff_Shambler != null)
					{
						hediff_Shambler.Notify_DelayedAlert(target);
					}
				}
			}
		}

		// Token: 0x0600C9F8 RID: 51704 RVA: 0x003B1EF8 File Offset: 0x003B00F8
		public static void RegenerateHealth(Pawn pawn)
		{
			if (!pawn.IsMutant || pawn.Dead)
			{
				return;
			}
			MutantDef def = pawn.mutant.Def;
			MutantUtility.tmpHediffs.Clear();
			MutantUtility.tmpHediffs.AddRange(pawn.health.hediffSet.hediffs);
			foreach (Hediff hediff in MutantUtility.tmpHediffs)
			{
				Hediff_Injury hediff_Injury = hediff as Hediff_Injury;
				Hediff_MissingPart hediff_MissingPart = hediff as Hediff_MissingPart;
				Hediff_Addiction hediff_Addiction = hediff as Hediff_Addiction;
				Hediff_ChemicalDependency hediff_ChemicalDependency = hediff as Hediff_ChemicalDependency;
				if (def.isImmuneToInfections && hediff.def.isInfection)
				{
					pawn.health.RemoveHediff(hediff);
				}
				if (def.removeChronicIllnesses && hediff.def.chronic)
				{
					pawn.health.RemoveHediff(hediff);
				}
				else if (def.removeAddictions && (hediff_Addiction != null || hediff_ChemicalDependency != null))
				{
					pawn.health.RemoveHediff(hediff);
				}
				else if (def.removeAllInjuries && hediff_Injury != null)
				{
					pawn.health.RemoveHediff(hediff);
				}
				else if (def.removePermanentInjuries && hediff_Injury != null && !hediff_Injury.IsPermanent())
				{
					pawn.health.RemoveHediff(hediff);
				}
				else if (def.removesHediffs.Contains(hediff.def))
				{
					pawn.health.RemoveHediff(hediff);
				}
				else if (def.restoreLegs && hediff_MissingPart != null && (hediff_MissingPart.Part.def.tags.Contains(BodyPartTagDefOf.MovingLimbCore) || hediff_MissingPart.Part.def.tags.Contains(BodyPartTagDefOf.MovingLimbSegment)) && (hediff_MissingPart.Part.parent == null || pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(hediff_MissingPart.Part.parent)))
				{
					pawn.health.RestorePart(hediff_MissingPart.Part, null, true);
				}
			}
			MutantUtility.tmpHediffs.Clear();
		}

		// Token: 0x0600C9F9 RID: 51705 RVA: 0x003B2120 File Offset: 0x003B0320
		public static void RestoreBodyParts(Pawn pawn)
		{
			foreach (Hediff hediff2 in (from hediff in pawn.health.hediffSet.hediffs
			where hediff is Hediff_MissingPart
			select hediff).ToList<Hediff>())
			{
				if (hediff2.Part.def.tags.Contains(BodyPartTagDefOf.MovingLimbCore) || !hediff2.Part.def.canSuggestAmputation)
				{
					pawn.health.RestorePart(hediff2.Part, null, true);
				}
				if (hediff2.Part.def == BodyPartDefOf.Arm && Rand.Bool)
				{
					pawn.health.RestorePart(hediff2.Part, null, true);
				}
			}
			pawn.health.RestorePart(pawn.health.hediffSet.GetBrain(), null, true);
		}

		// Token: 0x0600C9FA RID: 51706 RVA: 0x003B222C File Offset: 0x003B042C
		public static void RestoreUntilNotDowned(Pawn pawn)
		{
			MutantUtility.tmpHediffs.Clear();
			MutantUtility.tmpHediffs.AddRange(pawn.health.hediffSet.hediffs);
			int num = 0;
			while (num < 300 && pawn.Downed)
			{
				num++;
				Hediff hediff = MutantUtility.tmpHediffs.RandomElement<Hediff>();
				if (hediff is Hediff_Injury)
				{
					hediff.Severity -= 1f;
					pawn.health.Notify_HediffChanged(hediff);
				}
			}
			MutantUtility.tmpHediffs.Clear();
			if (pawn.Downed)
			{
				MutantUtility.RestoreBodyParts(pawn);
			}
			if (pawn.Downed)
			{
				MutantUtility.RegenerateHealth(pawn);
			}
		}

		// Token: 0x0600C9FB RID: 51707 RVA: 0x003B22CC File Offset: 0x003B04CC
		public static bool CheckShamblerHostility(Pawn a, Pawn b)
		{
			if (a.Faction == b.Faction)
			{
				return false;
			}
			if ((!a.IsShambler || !a.Faction.HostileTo(b.Faction)) && (!b.IsShambler || !b.Faction.HostileTo(a.Faction)))
			{
				return (a.IsShambler && a.Faction == null && b.Faction != null) || (b.IsShambler && b.Faction == null && a.Faction != null);
			}
			if (a.IsAnimal)
			{
				return a.Faction != null && (!a.RaceProps.herdAnimal || a.IsReleasedToAttack());
			}
			return !b.IsAnimal || (b.Faction != null && (!b.RaceProps.herdAnimal || b.IsReleasedToAttack()));
		}

		// Token: 0x0600C9FC RID: 51708 RVA: 0x003B23A4 File Offset: 0x003B05A4
		public static bool ShamblerShouldCollideWith(Pawn me, Pawn other)
		{
			return me.mindState.anyCloseHostilesRecently || other.mindState.anyCloseHostilesRecently || (other.mindState.lastAttackedTarget.Thing != null && (other.mindState.lastAttackTargetTick > Find.TickManager.TicksGame - 300 && other.mindState.lastAttackedTarget.Thing.Spawned));
		}

		// Token: 0x0600C9FD RID: 51709 RVA: 0x003B2418 File Offset: 0x003B0618
		public static bool CanUseDrug(Pawn pawn, ThingDef drug)
		{
			return !pawn.IsMutant || !pawn.mutant.HasTurned || (pawn.mutant.Def.canUseDrugs && pawn.mutant.Def.drugWhitelist.Contains(drug));
		}

		// Token: 0x0600C9FE RID: 51710 RVA: 0x003B246C File Offset: 0x003B066C
		public static Color GetMutantSkinColor(Pawn pawn, Color baseColor)
		{
			if (!pawn.IsMutant || !pawn.mutant.HasTurned)
			{
				return baseColor;
			}
			if (pawn.IsShambler)
			{
				return MutantUtility.GetShamblerColor(baseColor);
			}
			if (pawn.mutant.Def.skinColorTint != null)
			{
				return baseColor * (1f - pawn.mutant.Def.skinColorTintStrength) + pawn.mutant.Def.skinColorTint.Value * pawn.mutant.Def.skinColorTintStrength;
			}
			if (pawn.mutant.Def.skinColorOverride != null)
			{
				return pawn.mutant.Def.skinColorOverride.Value;
			}
			return baseColor;
		}

		// Token: 0x0600C9FF RID: 51711 RVA: 0x003B2530 File Offset: 0x003B0730
		public static Color GetShamblerColor(Color color)
		{
			float num;
			float num2;
			float num3;
			Color.RGBToHSV(color, out num, out num2, out num3);
			return Color.HSVToRGB((num + 0.05f) % 1f, Mathf.Clamp(num2 * 0.5f, 0.2f, 0.5f), Mathf.Max(num3 * 0.5f, 0.3f));
		}

		// Token: 0x0400897D RID: 35197
		public const float ShamblerLOS = 20f;

		// Token: 0x0400897E RID: 35198
		public const float ShamblerBuildingDamageFactor = 1.5f;

		// Token: 0x0400897F RID: 35199
		private const float ActivateRadius = 10f;

		// Token: 0x04008980 RID: 35200
		public static IntRange CallIntervalRange = new IntRange(500, 1000);

		// Token: 0x04008981 RID: 35201
		private static List<Hediff> tmpHediffs = new List<Hediff>();
	}
}
