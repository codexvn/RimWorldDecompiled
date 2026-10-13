using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200042F RID: 1071
	public static class GasUtility
	{
		// Token: 0x06001B38 RID: 6968 RVA: 0x000897C8 File Offset: 0x000879C8
		public static string GetLabel(this GasType gasType)
		{
			if (gasType <= GasType.ToxGas)
			{
				if (gasType == GasType.BlindSmoke)
				{
					return "BlindSmoke".Translate();
				}
				if (gasType == GasType.ToxGas)
				{
					return "ToxGas".Translate();
				}
			}
			else
			{
				if (gasType == GasType.RotStink)
				{
					return "RotStink".Translate();
				}
				if (gasType == GasType.DeadlifeDust)
				{
					return "DeadlifeDust".Translate();
				}
			}
			Log.ErrorOnce("Trying to get unknown gas type label.", 172091);
			return string.Empty;
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x00089844 File Offset: 0x00087A44
		public static void AddGas(IntVec3 cell, Map map, GasType gasType, float radius)
		{
			int num = GenRadial.NumCellsInRadius(radius);
			map.gasGrid.AddGas(cell, gasType, 255 * num, true);
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x0008986D File Offset: 0x00087A6D
		public static void AddGas(IntVec3 cell, Map map, GasType gasType, int amount)
		{
			map.gasGrid.AddGas(cell, gasType, amount, true);
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x0008987E File Offset: 0x00087A7E
		public static void AddDeadifeGas(IntVec3 cell, Map map, Faction faction, int amount)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			GasUtility.MarkDeadlifeCorpsesForFaction(cell, map, faction, amount);
			map.gasGrid.AddGas(cell, GasType.DeadlifeDust, amount, true);
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x000898A4 File Offset: 0x00087AA4
		public static void MarkDeadlifeCorpsesForFaction(IntVec3 cell, Map map, Faction faction, int amount)
		{
			map.gasGrid.EstimateGasDiffusion(cell, GasType.DeadlifeDust, amount, delegate(IntVec3 c)
			{
				foreach (Thing thing in c.GetThingList(map))
				{
					Corpse corpse = thing as Corpse;
					if (corpse != null)
					{
						corpse.InnerPawn.MarkDeadlifeDustForFaction(faction);
					}
				}
			});
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x000898E5 File Offset: 0x00087AE5
		public static byte GasDensity(this IntVec3 cell, Map map, GasType gasType)
		{
			return map.gasGrid.DensityAt(cell, gasType);
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x000898F4 File Offset: 0x00087AF4
		public static bool AnyGas(this IntVec3 cell, Map map, GasType gasType)
		{
			return map.gasGrid.DensityAt(cell, gasType) > 0;
		}

		// Token: 0x06001B3F RID: 6975 RVA: 0x00089908 File Offset: 0x00087B08
		public static int RotStinkToSpawnForCorpse(Corpse corpse)
		{
			if (GenTemperature.RotRateAtTemperature((float)Mathf.RoundToInt(corpse.AmbientTemperature)) <= 0f)
			{
				return 0;
			}
			if (corpse.GetRotStage() == RotStage.Rotting)
			{
				float num = corpse.InnerPawn.BodySize;
				if (corpse.InnerPawn.RaceProps.Humanlike)
				{
					num *= 1.15f;
				}
				return Mathf.CeilToInt(num * 52f);
			}
			return 0;
		}

		// Token: 0x06001B40 RID: 6976 RVA: 0x0008996C File Offset: 0x00087B6C
		public static void PawnGasEffectsTickInterval(Pawn pawn, int delta)
		{
			if (!pawn.Spawned || !pawn.IsHashIntervalTick(50, delta))
			{
				return;
			}
			if (pawn.Position.GasDensity(pawn.Map, GasType.RotStink) > 0 && (pawn.RaceProps.Animal || pawn.RaceProps.Humanlike) && (!pawn.IsMutant || !pawn.mutant.Def.isImmuneToInfections) && !pawn.RaceProps.isImmuneToInfections && !pawn.health.hediffSet.HasHediff(HediffDefOf.LungRotExposure, false) && GasUtility.GetLungRotAffectedBodyParts(pawn).Any<BodyPartRecord>())
			{
				pawn.health.AddHediff(HediffDefOf.LungRotExposure, null, null, null);
			}
			if (ModsConfig.BiotechActive && pawn.Spawned)
			{
				byte b = pawn.Position.GasDensity(pawn.Map, GasType.ToxGas);
				if (b > 0)
				{
					float num = (float)b / 255f;
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup, false);
					if (firstHediffOfDef != null && firstHediffOfDef.CurStageIndex == firstHediffOfDef.def.stages.Count - 1)
					{
						num *= 0.25f;
					}
					if (GasUtility.ShouldGetGasExposureHediff(pawn))
					{
						pawn.health.AddHediff(HediffDefOf.ToxGasExposure, null, null, null);
					}
					ToxicUtility.DoPawnToxicDamage(pawn, num);
				}
			}
			if (ModsConfig.AnomalyActive && pawn.Spawned && pawn.Position.GasDensity(pawn.Map, GasType.DeadlifeDust) > 0 && pawn.IsShambler)
			{
				Hediff firstHediffOfDef2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Shambler, false);
				HediffComp_DisappearsAndKills hediffComp_DisappearsAndKills = (firstHediffOfDef2 != null) ? firstHediffOfDef2.TryGetComp<HediffComp_DisappearsAndKills>() : null;
				if (hediffComp_DisappearsAndKills != null)
				{
					hediffComp_DisappearsAndKills.ticksToDisappear = Mathf.Max(hediffComp_DisappearsAndKills.ticksToDisappear, 15000);
				}
			}
		}

		// Token: 0x06001B41 RID: 6977 RVA: 0x00089B34 File Offset: 0x00087D34
		public static void CorpseGasEffectsTickRare(Corpse corpse)
		{
			if (!corpse.Spawned)
			{
				return;
			}
			if (corpse.GetRotStage() == RotStage.Rotting)
			{
				int num = GasUtility.RotStinkToSpawnForCorpse(corpse);
				if (num > 0)
				{
					GasUtility.AddGas(corpse.Position, corpse.Map, GasType.RotStink, num);
				}
			}
			if (ModsConfig.AnomalyActive && corpse.Position.GasDensity(corpse.Map, GasType.DeadlifeDust) > 0 && !corpse.InnerPawn.health.hediffSet.HasHediff(HediffDefOf.Shambler, false) && MutantUtility.CanResurrectAsShambler(corpse, true))
			{
				MutantUtility.ResurrectAsShambler(corpse.InnerPawn, 15000, corpse.InnerPawn.DeadlifeDustFaction);
			}
		}

		// Token: 0x06001B42 RID: 6978 RVA: 0x00089BD0 File Offset: 0x00087DD0
		public static void DoSteadyEffects(IntVec3 cell, Map map)
		{
			if (ModsConfig.AnomalyActive && cell.GasDensity(map, GasType.DeadlifeDust) > 0)
			{
				GasUtility.tempThingList.Clear();
				GasUtility.tempThingList.AddRange(cell.GetThingList(map));
				foreach (Thing thing in GasUtility.tempThingList)
				{
					Building_Grave building_Grave = thing as Building_Grave;
					if (building_Grave != null && building_Grave.Corpse != null && MutantUtility.CanResurrectAsShambler(building_Grave.Corpse, true))
					{
						Corpse corpse = building_Grave.Corpse;
						building_Grave.EjectContents();
						MutantUtility.ResurrectAsShambler(corpse.InnerPawn, 15000, corpse.InnerPawn.DeadlifeDustFaction);
					}
				}
			}
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x00089C98 File Offset: 0x00087E98
		public static IEnumerable<BodyPartRecord> GetLungRotAffectedBodyParts(Pawn pawn)
		{
			return from p in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where p.def == BodyPartDefOf.Lung && !pawn.health.hediffSet.hediffs.Any((Hediff x) => x.Part == p && x.def.preventsLungRot)
			select p;
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x00089CDC File Offset: 0x00087EDC
		private static bool ShouldGetGasExposureHediff(Pawn pawn)
		{
			return GasUtility.IsAffectedByExposure(pawn) && !pawn.health.hediffSet.HasHediff(HediffDefOf.ToxGasExposure, false);
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x00089D04 File Offset: 0x00087F04
		public static bool IsAffectedByExposure(Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike || pawn.RaceProps.Humanlike)
			{
				if (pawn.apparel != null)
				{
					using (List<Apparel>.Enumerator enumerator = pawn.apparel.WornApparel.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.def.apparel.immuneToToxGasExposure)
							{
								return false;
							}
						}
					}
				}
				if (pawn.genes != null)
				{
					using (List<Gene>.Enumerator enumerator2 = pawn.genes.GenesListForReading.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.def.immuneToToxGasExposure)
							{
								return false;
							}
						}
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x040017DC RID: 6108
		public const float BlindingGasAccuracyPenalty = 0.7f;

		// Token: 0x040017DD RID: 6109
		public const int RotStinkPerRawMeatRotting = 2;

		// Token: 0x040017DE RID: 6110
		private const float RotStinkPerBodySize = 52f;

		// Token: 0x040017DF RID: 6111
		private const float RotStinkHumanlikeFactor = 1.15f;

		// Token: 0x040017E0 RID: 6112
		private const int GasCheckInterval = 50;

		// Token: 0x040017E1 RID: 6113
		private const float ToxGasEffectOnExtremeBuildupFactor = 0.25f;

		// Token: 0x040017E2 RID: 6114
		private const int DeadlifeShamblerLifespanTicks = 15000;

		// Token: 0x040017E3 RID: 6115
		private static List<Thing> tempThingList = new List<Thing>();
	}
}
