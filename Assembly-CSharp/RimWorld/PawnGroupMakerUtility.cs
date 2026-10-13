using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002381 RID: 9089
	public class PawnGroupMakerUtility
	{
		// Token: 0x0600C4FF RID: 50431 RVA: 0x00394F8C File Offset: 0x0039318C
		public static IEnumerable<Pawn> GeneratePawns(PawnGroupMakerParms parms, bool warnOnZeroResults = true)
		{
			if (parms.groupKind == null)
			{
				Log.Error(string.Format("Tried to generate pawns with null pawn group kind def. parms={0}", parms));
				yield break;
			}
			if (parms.faction == null)
			{
				Log.Error(string.Format("Tried to generate pawn kinds with null faction. parms={0}", parms));
				yield break;
			}
			if (parms.faction.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>())
			{
				Log.Error(string.Format("Faction {0} of def {1} has no PawnGroupMakers.", parms.faction, parms.faction.def));
				yield break;
			}
			PawnGroupMaker pawnGroupMaker;
			if (!PawnGroupMakerUtility.TryGetRandomPawnGroupMaker(parms, out pawnGroupMaker, parms.ignoreGroupCommonality))
			{
				Log.Error(string.Format("Faction {0} of def {1} has no usable PawnGroupMakers for parms {2}", parms.faction, parms.faction.def, parms));
				yield break;
			}
			foreach (Pawn pawn in pawnGroupMaker.GeneratePawns(parms, warnOnZeroResults))
			{
				yield return pawn;
			}
			IEnumerator<Pawn> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600C500 RID: 50432 RVA: 0x00394FA3 File Offset: 0x003931A3
		public static IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms)
		{
			if (parms.groupKind == null)
			{
				Log.Error(string.Format("Tried to generate pawn kinds with null pawn group kind def. parms={0}", parms));
				yield break;
			}
			if (parms.faction == null)
			{
				Log.Error(string.Format("Tried to generate pawn kinds with null faction. parms={0}", parms));
				yield break;
			}
			if (parms.faction.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>())
			{
				Log.Error(string.Format("Faction {0} of def {1} has no PawnGroupMakers.", parms.faction, parms.faction.def));
				yield break;
			}
			PawnGroupMaker pawnGroupMaker;
			if (!PawnGroupMakerUtility.TryGetRandomPawnGroupMaker(parms, out pawnGroupMaker, false))
			{
				Log.Error(string.Format("Faction {0} of def {1} has no usable PawnGroupMakers for parms {2}", parms.faction, parms.faction.def, parms));
				yield break;
			}
			foreach (PawnKindDef pawnKindDef in pawnGroupMaker.GeneratePawnKindsExample(parms))
			{
				yield return pawnKindDef;
			}
			IEnumerator<PawnKindDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600C501 RID: 50433 RVA: 0x00394FB4 File Offset: 0x003931B4
		public static bool TryGetRandomPawnGroupMaker(PawnGroupMakerParms parms, out PawnGroupMaker pawnGroupMaker, bool ignoreCommonality = false)
		{
			if (parms.seed != null)
			{
				Rand.PushState(parms.seed.Value);
			}
			IEnumerable<PawnGroupMaker> source = from gm in parms.faction.def.pawnGroupMakers
			where gm.kindDef == parms.groupKind && gm.CanGenerateFrom(parms)
			select gm;
			bool result;
			if (ignoreCommonality)
			{
				result = source.TryRandomElement(out pawnGroupMaker);
			}
			else
			{
				result = source.TryRandomElementByWeight((PawnGroupMaker gm) => gm.commonality, out pawnGroupMaker);
			}
			if (parms.seed != null)
			{
				Rand.PopState();
			}
			return result;
		}

		// Token: 0x0600C502 RID: 50434 RVA: 0x00395068 File Offset: 0x00393268
		public static bool PawnGenOptionValid(PawnGenOption o, PawnGroupMakerParms groupParms, List<PawnGenOptionWithXenotype> chosenOptions = null)
		{
			PawnGroupMakerUtility.<>c__DisplayClass5_0 CS$<>8__locals1;
			CS$<>8__locals1.chosenOptions = chosenOptions;
			if (groupParms != null)
			{
				if (groupParms.generateFightersOnly && !o.kind.isFighter)
				{
					return false;
				}
				if (groupParms.dontUseSingleUseRocketLaunchers && o.kind.weaponTags != null && o.kind.weaponTags.Contains("GunSingleUse"))
				{
					return false;
				}
				if (groupParms.raidStrategy != null && !groupParms.raidStrategy.Worker.CanUsePawnGenOption(groupParms.points, o, CS$<>8__locals1.chosenOptions, groupParms.faction))
				{
					return false;
				}
				if (groupParms.raidAgeRestriction != null && groupParms.raidAgeRestriction.Worker.ShouldApplyToKind(o.kind) && !groupParms.raidAgeRestriction.Worker.CanUseKind(o.kind))
				{
					return false;
				}
			}
			return (!ModsConfig.BiotechActive || !Find.BossgroupManager.ReservedByBossgroup(o.kind)) && (!ModsConfig.AnomalyActive || !(o.kind is CreepJoinerFormKindDef)) && (o.kind.maxPerGroup >= int.MaxValue || PawnGroupMakerUtility.<PawnGenOptionValid>g__ChosenKindCount|5_0(o.kind, ref CS$<>8__locals1) < o.kind.maxPerGroup);
		}

		// Token: 0x0600C503 RID: 50435 RVA: 0x00395194 File Offset: 0x00393394
		public static List<PawnGenOptionWithXenotype> GetOptions(PawnGroupMakerParms groupParms, FactionDef faction, List<PawnGenOption> options, float pointsTotal, float pointsLeft, float? maxCost, List<PawnGenOptionWithXenotype> chosenOptions = null, bool leaderChosen = false)
		{
			List<PawnGenOptionWithXenotype> list = new List<PawnGenOptionWithXenotype>();
			bool flag = ModsConfig.BiotechActive && (faction == null || faction.humanlikeFaction);
			float num = maxCost ?? PawnGroupMakerUtility.MaxPawnCost((groupParms != null) ? groupParms.faction : null, pointsTotal, (groupParms != null) ? groupParms.raidStrategy : null, (groupParms != null) ? groupParms.groupKind : null);
			int i = 0;
			while (i < options.Count)
			{
				PawnGenOption pawnGenOption = options[i];
				if (flag)
				{
					using (Dictionary<XenotypeDef, float>.Enumerator enumerator = PawnGenerator.XenotypesAvailableFor(pawnGenOption.kind, faction, (groupParms != null) ? groupParms.faction : null).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<XenotypeDef, float> keyValuePair = enumerator.Current;
							if (PawnGroupMakerUtility.CanUseOption(pawnGenOption, keyValuePair.Key, groupParms, chosenOptions, pointsLeft, num * keyValuePair.Key.combatPowerFactor, leaderChosen))
							{
								list.Add(new PawnGenOptionWithXenotype(pawnGenOption, keyValuePair.Key, pawnGenOption.selectionWeight * keyValuePair.Value));
							}
						}
						goto IL_128;
					}
					goto IL_101;
				}
				goto IL_101;
				IL_128:
				i++;
				continue;
				IL_101:
				if (PawnGroupMakerUtility.CanUseOption(pawnGenOption, null, groupParms, chosenOptions, pointsLeft, num, leaderChosen))
				{
					list.Add(new PawnGenOptionWithXenotype(pawnGenOption, null, pawnGenOption.selectionWeight));
					goto IL_128;
				}
				goto IL_128;
			}
			return list;
		}

		// Token: 0x0600C504 RID: 50436 RVA: 0x003952F0 File Offset: 0x003934F0
		public static bool AnyOptions(PawnGroupMakerParms groupParms, FactionDef faction, List<PawnGenOption> options, float points)
		{
			bool flag = ModsConfig.BiotechActive && (faction == null || faction.humanlikeFaction);
			int i = 0;
			while (i < options.Count)
			{
				PawnGenOption pawnGenOption = options[i];
				if (flag)
				{
					using (Dictionary<XenotypeDef, float>.Enumerator enumerator = PawnGenerator.XenotypesAvailableFor(pawnGenOption.kind, faction, (groupParms != null) ? groupParms.faction : null).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<XenotypeDef, float> keyValuePair = enumerator.Current;
							if (PawnGroupMakerUtility.CanUseOption(pawnGenOption, keyValuePair.Key, groupParms, null, points, points, false))
							{
								return true;
							}
						}
						goto IL_91;
					}
					goto IL_81;
				}
				goto IL_81;
				IL_91:
				i++;
				continue;
				IL_81:
				if (PawnGroupMakerUtility.CanUseOption(pawnGenOption, null, groupParms, null, points, points, false))
				{
					return true;
				}
				goto IL_91;
			}
			return false;
		}

		// Token: 0x0600C505 RID: 50437 RVA: 0x003953B4 File Offset: 0x003935B4
		private static bool CanUseOption(PawnGenOption o, XenotypeDef xenotype, PawnGroupMakerParms groupParms, List<PawnGenOptionWithXenotype> chosenOptions, float pointsLeft, float maxOptionCost, bool leaderChosen)
		{
			float num = o.Cost;
			if (xenotype != null)
			{
				num *= xenotype.combatPowerFactor;
			}
			return num <= pointsLeft && num <= maxOptionCost && (!leaderChosen || !o.kind.factionLeader) && PawnGroupMakerUtility.PawnGenOptionValid(o, groupParms, chosenOptions) && (Find.Storyteller.difficulty.ChildRaidersAllowed || o.kind.pawnGroupDevelopmentStage == null || o.kind.pawnGroupDevelopmentStage.Value == DevelopmentalStage.Adult);
		}

		// Token: 0x0600C506 RID: 50438 RVA: 0x0039543C File Offset: 0x0039363C
		public static IEnumerable<PawnGenOptionWithXenotype> ChoosePawnGenOptionsByPoints(float pointsTotal, List<PawnGenOption> options, PawnGroupMakerParms groupParms)
		{
			if (groupParms.seed != null)
			{
				Rand.PushState(groupParms.seed.Value);
			}
			List<PawnGenOptionWithXenotype> list = new List<PawnGenOptionWithXenotype>();
			List<PawnGenOptionWithXenotype> chosenOptions = new List<PawnGenOptionWithXenotype>();
			float num = pointsTotal;
			bool leaderChosen = false;
			float highestCost = -1f;
			Func<PawnGenOptionWithXenotype, float> <>9__0;
			for (;;)
			{
				list.Clear();
				foreach (PawnGenOptionWithXenotype item in PawnGroupMakerUtility.GetOptions(groupParms, groupParms.faction.def, options, pointsTotal, num, null, chosenOptions, leaderChosen))
				{
					if (item.Cost <= num)
					{
						if (item.Cost > highestCost)
						{
							highestCost = item.Cost;
						}
						list.Add(item);
					}
				}
				Func<PawnGenOptionWithXenotype, float> func;
				if ((func = <>9__0) == null)
				{
					func = (<>9__0 = delegate(PawnGenOptionWithXenotype gr)
					{
						if (!PawnGroupMakerUtility.PawnGenOptionValid(gr.Option, groupParms, chosenOptions))
						{
							return 0f;
						}
						return gr.SelectionWeight * PawnGroupMakerUtility.PawnWeightFactorByMostExpensivePawnCostFractionCurve.Evaluate(gr.Cost / highestCost);
					});
				}
				Func<PawnGenOptionWithXenotype, float> weightSelector = func;
				PawnGenOptionWithXenotype item2;
				if (!list.TryRandomElementByWeight(weightSelector, out item2))
				{
					break;
				}
				chosenOptions.Add(item2);
				num -= item2.Cost;
				if (item2.Option.kind.factionLeader)
				{
					leaderChosen = true;
				}
			}
			list.Clear();
			if (chosenOptions.Count == 1 && num > pointsTotal / 2f)
			{
				Log.Warning(string.Format("Used only {0} / {1} points generating for {2}", pointsTotal - num, pointsTotal, groupParms.faction));
			}
			if (groupParms.seed != null)
			{
				Rand.PopState();
			}
			return chosenOptions;
		}

		// Token: 0x0600C507 RID: 50439 RVA: 0x00395604 File Offset: 0x00393804
		public static float MaxPawnCost(Faction faction, float totalPoints, RaidStrategyDef raidStrategy, PawnGroupKindDef groupKind)
		{
			if (faction == null)
			{
				return totalPoints;
			}
			float num = faction.def.maxPawnCostPerTotalPointsCurve.Evaluate(totalPoints);
			if (raidStrategy != null)
			{
				num = Mathf.Min(num, totalPoints / raidStrategy.minPawns);
			}
			num = Mathf.Max(num, faction.def.MinPointsToGeneratePawnGroup(groupKind, null) * 1.2f);
			if (raidStrategy != null)
			{
				num = Mathf.Max(num, raidStrategy.Worker.MinMaxAllowedPawnGenOptionCost(faction, groupKind) * 1.2f);
			}
			return num;
		}

		// Token: 0x0600C508 RID: 50440 RVA: 0x00395674 File Offset: 0x00393874
		public static bool CanGenerateAnyNormalGroup(Faction faction, float points)
		{
			if (faction.def.pawnGroupMakers == null)
			{
				return false;
			}
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
			pawnGroupMakerParms.faction = faction;
			pawnGroupMakerParms.points = points;
			for (int i = 0; i < faction.def.pawnGroupMakers.Count; i++)
			{
				PawnGroupMaker pawnGroupMaker = faction.def.pawnGroupMakers[i];
				if (pawnGroupMaker.kindDef == PawnGroupKindDefOf.Combat && pawnGroupMaker.CanGenerateFrom(pawnGroupMakerParms))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600C509 RID: 50441 RVA: 0x003956EC File Offset: 0x003938EC
		[DebugOutput]
		public static void PawnGroupsMade()
		{
			Dialog_DebugOptionListLister.ShowSimpleDebugMenu<Faction>(from fac in Find.FactionManager.AllFactions
			where !fac.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>()
			select fac, (Faction fac) => fac.Name + " (" + fac.def.defName + ")", delegate(Faction fac)
			{
				StringBuilder sb = new StringBuilder();
				sb.AppendLine(string.Format("FACTION: {0} ({1}) min={2}", fac.Name, fac.def.defName, fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null)));
				Action<float> action = delegate(float points)
				{
					if (points < fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null))
					{
						return;
					}
					PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms();
					pawnGroupMakerParms.groupKind = PawnGroupKindDefOf.Combat;
					pawnGroupMakerParms.tile = Find.CurrentMap.Tile;
					pawnGroupMakerParms.points = points;
					pawnGroupMakerParms.faction = fac;
					sb.AppendLine(string.Format("Group with {0} points (max option cost: {1})", pawnGroupMakerParms.points, PawnGroupMakerUtility.MaxPawnCost(fac, points, RaidStrategyDefOf.ImmediateAttack, PawnGroupKindDefOf.Combat)));
					float num = 0f;
					foreach (Pawn pawn in PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms, false).OrderBy((Pawn pa) => pa.kindDef.combatPower))
					{
						string text;
						if (pawn.equipment.Primary != null)
						{
							text = pawn.equipment.Primary.Label;
						}
						else
						{
							text = "no-equipment";
						}
						Apparel apparel = pawn.apparel.FirstApparelOnBodyPartGroup(BodyPartGroupDefOf.Torso);
						string text2;
						if (apparel != null)
						{
							text2 = apparel.LabelCap;
						}
						else
						{
							text2 = "shirtless";
						}
						sb.AppendLine(string.Concat(new string[]
						{
							"  ",
							pawn.kindDef.combatPower.ToString("F0").PadRight(6),
							pawn.kindDef.defName,
							", ",
							text,
							", ",
							text2
						}));
						num += pawn.kindDef.combatPower;
					}
					sb.AppendLine(string.Format("         totalCost {0}", num));
					sb.AppendLine();
				};
				foreach (float obj in DebugActionsUtility.PointsOptions(false))
				{
					action(obj);
				}
				Log.Message(sb.ToString());
			});
		}

		// Token: 0x0600C50A RID: 50442 RVA: 0x0039576C File Offset: 0x0039396C
		private static List<Faction> UsableFactions(float points, Predicate<Faction> validator = null, bool allowNonHostileToPlayer = false, bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true)
		{
			return Find.FactionManager.AllFactions.Where(delegate(Faction f)
			{
				if ((allowHidden || !f.Hidden) && !f.temporary && (allowDefeated || !f.defeated) && (allowNonHumanlike || f.def.humanlikeFaction) && (allowNonHostileToPlayer || f.HostileTo(Faction.OfPlayer)) && f.def.pawnGroupMakers != null)
				{
					if (f.def.pawnGroupMakers.Any((PawnGroupMaker x) => x.kindDef == PawnGroupKindDefOf.Combat) && !f.def.raidsForbidden && (validator == null || validator(f)))
					{
						return points >= f.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null);
					}
				}
				return false;
			}).ToList<Faction>();
		}

		// Token: 0x0600C50B RID: 50443 RVA: 0x003957CC File Offset: 0x003939CC
		public static bool TryGetRandomFactionForCombatPawnGroup(float points, out Faction faction, Predicate<Faction> validator = null, bool allowNonHostileToPlayer = false, bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true)
		{
			return PawnGroupMakerUtility.UsableFactions(points, validator, allowNonHostileToPlayer, allowHidden, allowDefeated, allowNonHumanlike).TryRandomElementByWeight((Faction f) => f.def.RaidCommonalityFromPoints(points), out faction);
		}

		// Token: 0x0600C50C RID: 50444 RVA: 0x0039580C File Offset: 0x00393A0C
		public static bool TryGetRandomFactionForCombatPawnGroupWeighted(IncidentParms parms, out Faction faction, Predicate<Faction> validator = null, bool allowNonHostileToPlayer = false, bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true)
		{
			float maxPoints = parms.points;
			if (maxPoints <= 0f)
			{
				maxPoints = 999999f;
			}
			return PawnGroupMakerUtility.UsableFactions(maxPoints, validator, allowNonHostileToPlayer, allowHidden, allowDefeated, allowNonHumanlike).TryRandomElementByWeight(delegate(Faction f)
			{
				float num = 1f;
				if (parms.target != null && f == parms.target.StoryState.lastRaidFaction)
				{
					num = 0.4f;
				}
				return f.def.RaidCommonalityFromPoints(maxPoints) * num;
			}, out faction);
		}

		// Token: 0x0600C50F RID: 50447 RVA: 0x003958D0 File Offset: 0x00393AD0
		[CompilerGenerated]
		internal static int <PawnGenOptionValid>g__ChosenKindCount|5_0(PawnKindDef d, ref PawnGroupMakerUtility.<>c__DisplayClass5_0 A_1)
		{
			int num = 0;
			if (A_1.chosenOptions.NullOrEmpty<PawnGenOptionWithXenotype>())
			{
				return num;
			}
			for (int i = 0; i < A_1.chosenOptions.Count; i++)
			{
				if (A_1.chosenOptions[i].Option.kind == d)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0400866E RID: 34414
		private static readonly SimpleCurve PawnWeightFactorByMostExpensivePawnCostFractionCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.2f, 0.01f),
				true
			},
			{
				new CurvePoint(0.3f, 0.3f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			}
		};

		// Token: 0x0400866F RID: 34415
		private const float FactionSelectionWeightFactor_RecentlyRaided = 0.4f;
	}
}
