using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020022C9 RID: 8905
	public static class CreepJoinerUtility
	{
		// Token: 0x0600C1C6 RID: 49606 RVA: 0x003823A0 File Offset: 0x003805A0
		public static void GetCreepjoinerSpecifics(Map map, ref CreepJoinerFormKindDef form, ref CreepJoinerBenefitDef benefit, ref CreepJoinerDownsideDef downside, ref CreepJoinerAggressiveDef aggressive, ref CreepJoinerRejectionDef rejection)
		{
			float combatPoints = StorytellerUtility.DefaultThreatPointsNow(map);
			if (form == null)
			{
				form = DefDatabase<CreepJoinerFormKindDef>.AllDefsListForReading.RandomElementByWeight((CreepJoinerFormKindDef x) => x.Weight);
			}
			CreepJoinerUtility.requires.AddRange(form.Requires);
			CreepJoinerUtility.exclude.AddRange(form.Excludes);
			if (benefit == null)
			{
				benefit = CreepJoinerUtility.GetRandom<CreepJoinerBenefitDef>(DefDatabase<CreepJoinerBenefitDef>.AllDefsListForReading, combatPoints, CreepJoinerUtility.requires, CreepJoinerUtility.exclude);
			}
			if (downside == null)
			{
				downside = CreepJoinerUtility.GetRandom<CreepJoinerDownsideDef>(DefDatabase<CreepJoinerDownsideDef>.AllDefsListForReading, combatPoints, CreepJoinerUtility.requires, CreepJoinerUtility.exclude);
			}
			if (aggressive == null)
			{
				aggressive = CreepJoinerUtility.GetRandom<CreepJoinerAggressiveDef>(DefDatabase<CreepJoinerAggressiveDef>.AllDefsListForReading, combatPoints, CreepJoinerUtility.requires, CreepJoinerUtility.exclude);
			}
			if (rejection == null)
			{
				rejection = CreepJoinerUtility.GetRandom<CreepJoinerRejectionDef>(DefDatabase<CreepJoinerRejectionDef>.AllDefsListForReading, combatPoints, CreepJoinerUtility.requires, CreepJoinerUtility.exclude);
			}
			CreepJoinerUtility.exclude.Clear();
			CreepJoinerUtility.requires.Clear();
		}

		// Token: 0x0600C1C7 RID: 49607 RVA: 0x0038248C File Offset: 0x0038068C
		public static Pawn GenerateAndSpawn(Map map, float combatPoints)
		{
			CreepJoinerFormKindDef creepJoinerFormKindDef = DefDatabase<CreepJoinerFormKindDef>.AllDefsListForReading.RandomElementByWeight((CreepJoinerFormKindDef x) => x.Weight);
			CreepJoinerUtility.requires.AddRange(creepJoinerFormKindDef.Requires);
			CreepJoinerUtility.exclude.AddRange(creepJoinerFormKindDef.Excludes);
			CreepJoinerBenefitDef random = CreepJoinerUtility.GetRandom<CreepJoinerBenefitDef>(DefDatabase<CreepJoinerBenefitDef>.AllDefsListForReading, combatPoints, CreepJoinerUtility.requires, CreepJoinerUtility.exclude);
			CreepJoinerDownsideDef random2 = CreepJoinerUtility.GetRandom<CreepJoinerDownsideDef>(DefDatabase<CreepJoinerDownsideDef>.AllDefsListForReading, combatPoints, CreepJoinerUtility.requires, CreepJoinerUtility.exclude);
			CreepJoinerAggressiveDef random3 = CreepJoinerUtility.GetRandom<CreepJoinerAggressiveDef>(DefDatabase<CreepJoinerAggressiveDef>.AllDefsListForReading, combatPoints, CreepJoinerUtility.requires, CreepJoinerUtility.exclude);
			CreepJoinerRejectionDef random4 = CreepJoinerUtility.GetRandom<CreepJoinerRejectionDef>(DefDatabase<CreepJoinerRejectionDef>.AllDefsListForReading, combatPoints, CreepJoinerUtility.requires, CreepJoinerUtility.exclude);
			CreepJoinerUtility.exclude.Clear();
			CreepJoinerUtility.requires.Clear();
			return CreepJoinerUtility.GenerateAndSpawn(creepJoinerFormKindDef, random, random2, random3, random4, map);
		}

		// Token: 0x0600C1C8 RID: 49608 RVA: 0x0038255C File Offset: 0x0038075C
		public static Pawn GenerateAndSpawn(CreepJoinerFormKindDef form, CreepJoinerBenefitDef benefit, CreepJoinerDownsideDef downside, CreepJoinerAggressiveDef aggressive, CreepJoinerRejectionDef rejection, Map map)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(form, null, PawnGenerationContext.NonPlayer, null, true, false, false, true, false, 1f, false, true, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, true, false, false, -1, 0, false)
			{
				AllowedDevelopmentalStages = DevelopmentalStage.Adult,
				ForceGenerateNewPawn = true,
				AllowFood = true,
				DontGiveWeapon = true,
				OnlyUseForcedBackstories = form.fixedAdultBackstories.Any<BackstoryDef>(),
				MaximumAgeTraits = 1,
				MinimumAgeTraits = 1,
				IsCreepJoiner = true,
				ForceNoIdeoGear = true,
				MustBeCapableOfViolence = true
			});
			Pawn_CreepJoinerTracker creepjoiner = pawn.creepjoiner;
			creepjoiner.form = form;
			creepjoiner.benefit = benefit;
			creepjoiner.downside = downside;
			creepjoiner.aggressive = aggressive;
			creepjoiner.rejection = rejection;
			CreepJoinerUtility.ApplyExtraTraits(pawn, benefit.traits);
			CreepJoinerUtility.ApplyExtraTraits(pawn, downside.traits);
			CreepJoinerUtility.ApplyExtraHediffs(pawn, benefit.hediffs);
			CreepJoinerUtility.ApplyExtraHediffs(pawn, downside.hediffs);
			CreepJoinerUtility.ApplySkillOverrides(pawn, benefit.skills);
			CreepJoinerUtility.ApplyExtraAbilities(pawn, benefit.abilities);
			CreepJoinerUtility.ApplyExtraAbilities(pawn, downside.abilities);
			pawn.guest.Recruitable = false;
			IntVec3 loc;
			if (!RCellFinder.TryFindRandomPawnEntryCell(out loc, map, CellFinder.EdgeRoadChance_Friendly, false, (IntVec3 cell) => map.reachability.CanReachMapEdge(cell, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false))))
			{
				return null;
			}
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
			IntVec3 idleSpot;
			if (!RCellFinder.TryFindRandomSpotJustOutsideColony(pawn, out idleSpot))
			{
				return null;
			}
			LordMaker.MakeNewLord(pawn.Faction, new LordJob_CreepJoiner(idleSpot, pawn), map, null).AddPawn(pawn);
			creepjoiner.Notify_Created();
			return pawn;
		}

		// Token: 0x0600C1C9 RID: 49609 RVA: 0x0038275C File Offset: 0x0038095C
		private static void ApplySkillOverrides(Pawn pawn, List<CreepJoinerBenefitDef.SkillValue> skills)
		{
			foreach (CreepJoinerBenefitDef.SkillValue skillValue in skills)
			{
				SkillRecord skill = pawn.skills.GetSkill(skillValue.skill);
				skill.Level = skillValue.range.RandomInRange;
				skill.xpSinceMidnight = 0f;
				skill.xpSinceLastLevel = 0f;
			}
		}

		// Token: 0x0600C1CA RID: 49610 RVA: 0x003827DC File Offset: 0x003809DC
		private static void ApplyExtraTraits(Pawn pawn, List<BackstoryTrait> traits)
		{
			foreach (BackstoryTrait backstoryTrait in traits)
			{
				if (!pawn.story.traits.HasTrait(backstoryTrait.def))
				{
					pawn.story.traits.GainTrait(new Trait(backstoryTrait.def, backstoryTrait.degree, true), false);
				}
			}
		}

		// Token: 0x0600C1CB RID: 49611 RVA: 0x00382860 File Offset: 0x00380A60
		private static void ApplyExtraHediffs(Pawn pawn, List<HediffDef> hediffs)
		{
			foreach (HediffDef def in hediffs)
			{
				pawn.health.AddHediff(def, null, null, null);
			}
		}

		// Token: 0x0600C1CC RID: 49612 RVA: 0x003828C0 File Offset: 0x00380AC0
		private static void ApplyExtraAbilities(Pawn pawn, List<AbilityDef> abilities)
		{
			foreach (AbilityDef def in abilities)
			{
				pawn.abilities.GainAbility(def);
			}
		}

		// Token: 0x0600C1CD RID: 49613 RVA: 0x00382914 File Offset: 0x00380B14
		public static T GetRandom<T>(List<T> defs, float combatPoints, List<CreepJoinerBaseDef> requires, List<CreepJoinerBaseDef> exclude) where T : CreepJoinerBaseDef
		{
			T t;
			if (requires.Empty<CreepJoinerBaseDef>() && exclude.Empty<CreepJoinerBaseDef>())
			{
				t = (from x in defs
				where combatPoints >= x.MinCombatPoints && x.CanOccurRandomly
				select x).RandomElementByWeight((T x) => x.Weight);
			}
			else
			{
				bool flag = false;
				foreach (T t2 in defs)
				{
					if (combatPoints >= t2.MinCombatPoints && t2.CanOccurRandomly && requires.Contains(t2))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					using (List<T>.Enumerator enumerator = defs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							T t3 = enumerator.Current;
							if (combatPoints >= t3.MinCombatPoints && t3.CanOccurRandomly && requires.Contains(t3))
							{
								CreepJoinerUtility.temp.Add(t3);
							}
						}
						goto IL_189;
					}
				}
				foreach (T t4 in defs)
				{
					if (combatPoints >= t4.MinCombatPoints && t4.CanOccurRandomly)
					{
						CreepJoinerUtility.temp.Add(t4);
					}
				}
				IL_189:
				for (int i = CreepJoinerUtility.temp.Count - 1; i >= 0; i--)
				{
					if (exclude.Contains(CreepJoinerUtility.temp[i]))
					{
						defs.RemoveAt(i);
					}
				}
				if (CreepJoinerUtility.temp.Empty<ICreepJoinerDef>())
				{
					string text = (from x in defs
					select x.label).ToCommaList(false, false);
					string text2 = (from x in requires
					select x.label).ToCommaList(false, false);
					string text3 = (from x in exclude
					select x.label).ToCommaList(false, false);
					Log.Error(string.Format("Attempted to create creepjoiner but blacklist removed all possible whitelist; combatPoints = {0}, defs = ({1}), whitelist = ({2}), blacklist = ({3})", new object[]
					{
						combatPoints,
						text,
						text2,
						text3
					}));
					t = defs.RandomElementByWeight((T x) => x.Weight);
				}
				else
				{
					t = (CreepJoinerUtility.temp.RandomElementByWeight((ICreepJoinerDef x) => x.Weight) as T);
				}
				CreepJoinerUtility.temp.Clear();
			}
			exclude.AddRange(t.Excludes);
			requires.AddRange(t.Requires);
			return t;
		}

		// Token: 0x04008457 RID: 33879
		private static readonly List<CreepJoinerBaseDef> requires = new List<CreepJoinerBaseDef>();

		// Token: 0x04008458 RID: 33880
		private static readonly List<CreepJoinerBaseDef> exclude = new List<CreepJoinerBaseDef>();

		// Token: 0x04008459 RID: 33881
		private static readonly List<ICreepJoinerDef> temp = new List<ICreepJoinerDef>();
	}
}
