using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020027E5 RID: 10213
	public static class RitualUtility
	{
		// Token: 0x0600DF92 RID: 57234 RVA: 0x0041A4BB File Offset: 0x004186BB
		public static bool IsRitualTarget(this Thing thing)
		{
			return thing.TargetOfRitual() != null;
		}

		// Token: 0x0600DF93 RID: 57235 RVA: 0x0041A4C8 File Offset: 0x004186C8
		public static LordJob_Ritual TargetOfRitual(this Thing thing)
		{
			if (!thing.Spawned)
			{
				return null;
			}
			foreach (Lord lord in thing.Map.lordManager.lords)
			{
				LordJob_Ritual lordJob_Ritual = lord.LordJob as LordJob_Ritual;
				if (lordJob_Ritual != null && lordJob_Ritual.selectedTarget.Thing == thing)
				{
					LordToilData_Gathering lordToilData_Gathering = lordJob_Ritual.lord.CurLordToil.data as LordToilData_Gathering;
					if (lordToilData_Gathering != null && lordToilData_Gathering.presentForTicks.Any<KeyValuePair<Pawn, int>>())
					{
						return lordJob_Ritual;
					}
				}
			}
			return null;
		}

		// Token: 0x0600DF94 RID: 57236 RVA: 0x0041A570 File Offset: 0x00418770
		public static bool GoodSpectateCellForRitual(IntVec3 spot, Pawn p, Map map)
		{
			return !spot.ContainsStaticFire(map) && !spot.GetTerrain(map).avoidWander && !PawnUtility.KnownDangerAt(spot, map, p);
		}

		// Token: 0x0600DF95 RID: 57237 RVA: 0x0041A598 File Offset: 0x00418798
		public static float CalculateQualityAbstract(Precept_Ritual ritual, TargetInfo ritualTarget, RitualRoleAssignments assignments, RitualObligation obligation = null)
		{
			RitualOutcomeEffectDef def = ritual.outcomeEffect.def;
			float num = def.startingQuality;
			foreach (RitualOutcomeComp ritualOutcomeComp in def.comps)
			{
				QualityFactor qualityFactor = ritualOutcomeComp.GetQualityFactor(ritual, ritualTarget, obligation, assignments, null);
				if (qualityFactor != null)
				{
					if (qualityFactor.uncertainOutcome)
					{
						Log.Warning("Tried to calculate abstract quality for a ritual with uncertain quality offset, that can't be calculated without actually executing the ritual.");
					}
					else
					{
						num += qualityFactor.quality;
					}
				}
			}
			return num;
		}

		// Token: 0x0600DF96 RID: 57238 RVA: 0x0041A620 File Offset: 0x00418820
		public static string QualityBreakdownAbstract(Precept_Ritual ritual, TargetInfo ritualTarget, RitualRoleAssignments assignments, RitualObligation obligation = null)
		{
			RitualOutcomeEffectDef def = ritual.outcomeEffect.def;
			float f = RitualUtility.CalculateQualityAbstract(ritual, ritualTarget, assignments, obligation);
			TaggedString taggedString = "RitualOutcomeQualitySpecific".Translate(ritual.Label, f.ToStringPercent()).CapitalizeFirst() + ":\n";
			if (def.startingQuality > 0f)
			{
				taggedString += "\n  - " + "StartingRitualQuality".Translate(def.startingQuality.ToStringPercent()) + ".";
			}
			foreach (RitualOutcomeComp ritualOutcomeComp in def.comps)
			{
				QualityFactor qualityFactor = ritualOutcomeComp.GetQualityFactor(ritual, ritualTarget, obligation, assignments, null);
				if (qualityFactor != null)
				{
					taggedString += "\n  - " + qualityFactor.label;
					taggedString += ": " + ritualOutcomeComp.GetDescAbstract(qualityFactor.positive, qualityFactor.quality) + ".";
				}
			}
			if (ritual.RepeatPenaltyActive)
			{
				taggedString += "\n  - " + "RitualOutcomePerformedRecently".Translate() + ": " + ritual.RepeatQualityPenalty.ToStringPercent();
			}
			Tuple<ExpectationDef, float> expectationsOffset = RitualOutcomeEffectWorker_FromQuality.GetExpectationsOffset(ritualTarget.Map, ritual.def);
			if (expectationsOffset != null)
			{
				taggedString += "\n  - " + "RitualQualityExpectations".Translate(expectationsOffset.Item1.LabelCap) + ": " + expectationsOffset.Item2.ToStringPercent();
			}
			return taggedString;
		}

		// Token: 0x0600DF97 RID: 57239 RVA: 0x0041A7F0 File Offset: 0x004189F0
		public static void AddArrivalTag(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
			if (lordJob_Ritual != null && !lordJob_Ritual.PawnTagSet(pawn, "Arrived"))
			{
				lordJob_Ritual.AddTagForPawn(pawn, "Arrived");
			}
		}

		// Token: 0x0600DF98 RID: 57240 RVA: 0x0041A832 File Offset: 0x00418A32
		public static IEnumerable<LordJob_Ritual> GetActiveRitualsForPrecept(Precept_Ritual precept)
		{
			List<Map> maps = Find.Maps;
			int num;
			for (int i = 0; i < maps.Count; i = num + 1)
			{
				List<Lord> lords = maps[i].lordManager.lords;
				for (int j = 0; j < lords.Count; j = num + 1)
				{
					LordJob_Ritual lordJob_Ritual = lords[j].LordJob as LordJob_Ritual;
					if (lordJob_Ritual != null && lordJob_Ritual.Ritual == precept)
					{
						yield return lordJob_Ritual;
					}
					num = j;
				}
				lords = null;
				num = i;
			}
			yield break;
		}

		// Token: 0x0600DF99 RID: 57241 RVA: 0x0041A844 File Offset: 0x00418A44
		public static string RoleChangeConfirmation(Pawn pawn, Precept_Role oldRole, Precept_Role newRole)
		{
			string text = "";
			if (oldRole != null)
			{
				text += " " + "ChooseRoleConfirmAssignHasOtherRole".Translate(pawn.Named("PAWN"), oldRole.Named("ROLE"));
			}
			Pawn pawn2 = newRole.ChosenPawns().FirstOrDefault<Pawn>();
			if (pawn2 != null && newRole is Precept_RoleSingle)
			{
				text += " " + "ChooseRoleConfirmAssignReplace".Translate(pawn2.Named("PAWN"));
			}
			else if (newRole.def.leaderRole)
			{
				foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
				{
					foreach (Precept precept in ideo.PreceptsListForReading)
					{
						if (precept != oldRole)
						{
							Precept_Role precept_Role = precept as Precept_Role;
							if (precept_Role != null)
							{
								Pawn pawn3 = precept_Role.ChosenPawnSingle();
								if (precept_Role.def.leaderRole && pawn3 != null && pawn3 != pawn && pawn3.IsFreeColonist)
								{
									text += " " + "ChooseRoleConfirmAssignReplaceLeader".Translate(pawn3.Named("PAWN"), newRole.Named("ROLE"), precept_Role.Named("OTHERROLE"));
									break;
								}
							}
						}
					}
				}
			}
			IEnumerable<WorkTypeDef> disabledWorkTypes = newRole.DisabledWorkTypes;
			if (disabledWorkTypes.Any<WorkTypeDef>())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += "ChooseRoleListWorkTypeRestrictions".Translate(pawn.Named("PAWN")) + ": \n" + (from x in disabledWorkTypes
				select x.labelShort.CapitalizeFirst()).ToLineList("  - ", false);
			}
			if (!newRole.def.grantedAbilities.NullOrEmpty<AbilityDef>())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += "ChooseRoleListAbilities".Translate(pawn.Named("PAWN")) + ": \n" + (from x in newRole.def.grantedAbilities
				select x.LabelCap.ToString()).ToLineList("  - ", false);
			}
			if (!newRole.apparelRequirements.NullOrEmpty<PreceptApparelRequirement>())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += "ChooseRoleListApparelDemands".Translate(newRole.Named("ROLE")) + ": \n" + newRole.AllApparelRequirementLabels(pawn.gender, pawn).ToLineList("  - ");
			}
			if (newRole.def.roleRequiredWorkTags != WorkTags.None)
			{
				List<string> list = new List<string>();
				foreach (WorkTags workTags in newRole.def.roleRequiredWorkTags.GetAllSelectedItems<WorkTags>())
				{
					if (pawn.WorkTagIsDisabled(workTags))
					{
						list.Add(workTags.LabelTranslated().CapitalizeFirst());
					}
				}
				if (list.Any<string>())
				{
					if (!text.NullOrEmpty())
					{
						text += "\n\n";
					}
					text += "ChooseRoleRequiredWorkTagsDisabled".Translate(pawn.Named("PAWN"), newRole.Named("ROLE")) + ": \n" + list.ToLineList("  - ");
				}
			}
			else if (newRole.def.roleRequiredWorkTagAny != WorkTags.None)
			{
				bool flag = false;
				List<string> list2 = new List<string>();
				foreach (WorkTags workTags2 in newRole.def.roleRequiredWorkTagAny.GetAllSelectedItems<WorkTags>())
				{
					if (!pawn.WorkTagIsDisabled(workTags2))
					{
						flag = true;
						break;
					}
					list2.Add(workTags2.LabelTranslated().CapitalizeFirst());
				}
				if (!flag)
				{
					if (!text.NullOrEmpty())
					{
						text += "\n\n";
					}
					text += "ChooseRoleRequiredWorkTagsDisabled".Translate(pawn.Named("PAWN"), newRole.Named("ROLE")) + ": \n" + list2.ToLineList("  - ");
				}
			}
			if (newRole.ChosenPawnSingle() == null && newRole is Precept_RoleSingle)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += "ChooseRoleHint".Translate();
			}
			return text;
		}

		// Token: 0x0600DF9A RID: 57242 RVA: 0x0041AD78 File Offset: 0x00418F78
		public static IEnumerable<Precept_Role> AllRolesForPawn(Pawn p)
		{
			if (p.Ideo == null)
			{
				yield break;
			}
			foreach (Precept_Role precept_Role in from r in p.Ideo.RolesListForReading
			where !r.def.leaderRole
			select r)
			{
				yield return precept_Role;
			}
			IEnumerator<Precept_Role> enumerator = null;
			Precept_Role precept_Role2 = Faction.OfPlayer.ideos.PrimaryIdeo.RolesListForReading.FirstOrDefault((Precept_Role r) => r.def.leaderRole);
			if (precept_Role2 != null)
			{
				yield return precept_Role2;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600DF9B RID: 57243 RVA: 0x0041AD88 File Offset: 0x00418F88
		public static IntVec3 RitualCrowdCenterFor(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
			if (lordJob_Ritual != null && pawn.CurJob != null && pawn.CurJob.speechFaceSpectatorsIfPossible)
			{
				return lordJob_Ritual.CurrentSpectatorCrowdCenter();
			}
			return IntVec3.Invalid;
		}
	}
}
