using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200168A RID: 5770
	public class PsychicRitualDef : Def
	{
		// Token: 0x060087EE RID: 34798 RVA: 0x00289A81 File Offset: 0x00287C81
		public override void PostLoad()
		{
			if (!string.IsNullOrEmpty(this.iconPath))
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.uiIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				});
			}
		}

		// Token: 0x060087EF RID: 34799 RVA: 0x00289AA1 File Offset: 0x00287CA1
		public virtual AcceptanceReport AllowsDrafting(Pawn pawn)
		{
			if (this.allowsDrafting)
			{
				return true;
			}
			return new AcceptanceReport("ParticipatingInPsychicRitual".Translate(pawn, this.label));
		}

		// Token: 0x060087F0 RID: 34800 RVA: 0x00289AD7 File Offset: 0x00287CD7
		public virtual AcceptanceReport AllowsFloatMenu(Pawn pawn)
		{
			if (this.allowsFloatMenu)
			{
				return true;
			}
			return new AcceptanceReport("ParticipatingInPsychicRitual".Translate(pawn, this.label));
		}

		// Token: 0x060087F1 RID: 34801 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool BlocksSocialInteraction(Pawn pawn)
		{
			return true;
		}

		// Token: 0x060087F2 RID: 34802 RVA: 0x00289B0D File Offset: 0x00287D0D
		public virtual AcceptanceReport AbilityAllowed(Ability ability)
		{
			return new AcceptanceReport("AbilityDisabledInPsychicRitual".Translate(ability.pawn, this.label));
		}

		// Token: 0x170014C8 RID: 5320
		// (get) Token: 0x060087F3 RID: 34803 RVA: 0x00289B39 File Offset: 0x00287D39
		public virtual List<PsychicRitualRoleDef> Roles
		{
			get
			{
				this.rolesBackingList.Clear();
				return this.rolesBackingList;
			}
		}

		// Token: 0x060087F4 RID: 34804 RVA: 0x00289B4C File Offset: 0x00287D4C
		public virtual PsychicRitualGraph CreateGraph()
		{
			return new PsychicRitualGraph();
		}

		// Token: 0x060087F5 RID: 34805 RVA: 0x00289B53 File Offset: 0x00287D53
		public virtual List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			throw new NotImplementedException("You must subclass PsychicRitualDef and override CreateToils() to return a list of toils.");
		}

		// Token: 0x060087F6 RID: 34806 RVA: 0x00289B60 File Offset: 0x00287D60
		public virtual PsychicRitualCandidatePool FindCandidatePool()
		{
			return new PsychicRitualCandidatePool(new List<Pawn>(from p in Find.CurrentMap.mapPawns.FreeColonistsAndPrisonersSpawned
			where !p.IsSubhuman
			select p), new List<Pawn>());
		}

		// Token: 0x060087F7 RID: 34807 RVA: 0x00289BAF File Offset: 0x00287DAF
		public virtual PsychicRitualRoleAssignments BuildRoleAssignments(TargetInfo target)
		{
			return new PsychicRitualRoleAssignments(this.Roles, target, null);
		}

		// Token: 0x060087F8 RID: 34808 RVA: 0x00289BC0 File Offset: 0x00287DC0
		public static bool OfferingReachable(Map map, List<Pawn> pawns, IngredientCount offering, out int reachableCount)
		{
			bool result;
			using (new ProfilerBlock("Offering reachable"))
			{
				reachableCount = 0;
				float num = offering.GetBaseCount();
				if (num <= 0f)
				{
					result = true;
				}
				else
				{
					List<Thing> list;
					using (new ProfilerBlock("ThingsMatchingFilter"))
					{
						list = map.listerThings.ThingsMatchingFilter(offering.filter);
					}
					foreach (Thing thing in list)
					{
						if (num <= 0f)
						{
							break;
						}
						foreach (Pawn pawn in pawns)
						{
							if (pawn.CanReserveAndReach(thing, PathEndMode.Touch, pawn.NormalMaxDanger(), 1, -1, null, false) && !thing.IsForbidden(pawn) && !thing.Fogged())
							{
								num -= (float)thing.stackCount;
								reachableCount += thing.stackCount;
								break;
							}
						}
					}
					result = (num <= 0f);
				}
			}
			return result;
		}

		// Token: 0x060087F9 RID: 34809 RVA: 0x00035AE5 File Offset: 0x00033CE5
		public virtual IEnumerable<string> BlockingIssues(PsychicRitualRoleAssignments assignments, Map map)
		{
			return Enumerable.Empty<string>();
		}

		// Token: 0x060087FA RID: 34810 RVA: 0x00289D58 File Offset: 0x00287F58
		public virtual TaggedString PsychicRitualBegunMessage(PsychicRitualRoleAssignments assignments)
		{
			return "PsychicRitualBegun".Translate(this.label);
		}

		// Token: 0x060087FB RID: 34811 RVA: 0x00289D6F File Offset: 0x00287F6F
		public virtual TaggedString PsychicRitualCompletedMessage()
		{
			return "PsychicRitualCompleted".Translate(this.label);
		}

		// Token: 0x060087FC RID: 34812 RVA: 0x00289D86 File Offset: 0x00287F86
		public virtual TaggedString PsychicRitualCanceledMessage(TaggedString reason)
		{
			if (reason.NullOrEmpty())
			{
				return "PsychicRitualCanceled".Translate(this.label);
			}
			return "PsychicRitualCanceledBecause".Translate(this.label, reason);
		}

		// Token: 0x060087FD RID: 34813 RVA: 0x00289DC4 File Offset: 0x00287FC4
		public virtual TaggedString LeftPsychicRitualMessage(Pawn pawn, TaggedString reason)
		{
			if (reason.NullOrEmpty())
			{
				return "PsychicRitualLeft".Translate(pawn, this.label);
			}
			return "PsychicRitualLeftBecause".Translate(pawn, this.label, reason);
		}

		// Token: 0x060087FE RID: 34814 RVA: 0x00289E17 File Offset: 0x00288017
		public virtual string GetPawnReport(PsychicRitual psychicRitual, Pawn pawn)
		{
			return "PsychicRitualAttending".Translate(this.label.Named("RITUALNAME"));
		}

		// Token: 0x060087FF RID: 34815 RVA: 0x00289E38 File Offset: 0x00288038
		public virtual Lord MakeNewLord(PsychicRitualRoleAssignments assignments)
		{
			Lord lord = LordMaker.MakeNewLord(Faction.OfPlayer, new LordJob_PsychicRitual(this, assignments, -1f), Find.CurrentMap, assignments.AllAssignedPawns);
			Building building = assignments.Target.Thing as Building;
			if (building != null)
			{
				lord.AddBuilding(building);
			}
			return lord;
		}

		// Token: 0x06008800 RID: 34816 RVA: 0x00289E86 File Offset: 0x00288086
		public virtual bool IsValidTarget(TargetInfo target, out AnyEnum reason)
		{
			reason = AnyEnum.None;
			return true;
		}

		// Token: 0x06008801 RID: 34817 RVA: 0x00289E94 File Offset: 0x00288094
		public virtual TaggedString InvalidTargetReason(AnyEnum reason)
		{
			if (reason == AnyEnum.None)
			{
				return TaggedString.Empty;
			}
			throw new InvalidOperationException("Unknown enum type " + reason.enumType.ToStringSafe<Type>() + "; did you forget to override `InvalidTargetReason` in a child class?");
		}

		// Token: 0x170014C9 RID: 5321
		// (get) Token: 0x06008802 RID: 34818 RVA: 0x00289EC8 File Offset: 0x002880C8
		public bool Visible
		{
			get
			{
				return this.playerCastable && (DebugSettings.godMode || this.researchPrerequisite == null || this.researchPrerequisite.IsFinished);
			}
		}

		// Token: 0x06008803 RID: 34819 RVA: 0x00289EF8 File Offset: 0x002880F8
		public virtual void CalculateMaxPower(PsychicRitualRoleAssignments assignments, List<QualityFactor> powerFactorsOut, out float power)
		{
			power = 0f;
			int num = 0;
			IReadOnlyDictionary<PsychicRitualRoleDef, List<Pawn>> roleAssignments = assignments.RoleAssignments;
			foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> keyValuePair in roleAssignments)
			{
				PsychicRitualRoleDef psychicRitualRoleDef;
				List<Pawn> list;
				keyValuePair.Deconstruct(out psychicRitualRoleDef, out list);
				PsychicRitualRoleDef psychicRitualRoleDef2 = psychicRitualRoleDef;
				List<Pawn> list2 = list;
				if (psychicRitualRoleDef2.applyPowerOffset)
				{
					num += list2.Count;
				}
				if (psychicRitualRoleDef2.MaxCount != psychicRitualRoleDef2.MinCount)
				{
					float num2 = (float)(list2.Count - psychicRitualRoleDef2.MinCount) / (float)(psychicRitualRoleDef2.MaxCount - psychicRitualRoleDef2.MinCount);
					power += this.rolePowerFactor * num2;
					if (powerFactorsOut != null)
					{
						powerFactorsOut.Add(new QualityFactor
						{
							count = string.Format("{0} / {1}", list2.Count, psychicRitualRoleDef2.MaxCount),
							label = Find.ActiveLanguageWorker.Pluralize(psychicRitualRoleDef2.LabelCap, -1),
							positive = (list2.Count >= psychicRitualRoleDef2.MinCount),
							quality = this.rolePowerFactor * num2,
							toolTip = psychicRitualRoleDef2.description.CapitalizeFirst().EndWithPeriod()
						});
					}
				}
			}
			if (num > 0)
			{
				float num3 = 0f;
				foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> keyValuePair in roleAssignments)
				{
					PsychicRitualRoleDef psychicRitualRoleDef;
					List<Pawn> list;
					keyValuePair.Deconstruct(out psychicRitualRoleDef, out list);
					PsychicRitualRoleDef psychicRitualRoleDef3 = psychicRitualRoleDef;
					List<Pawn> list3 = list;
					if (psychicRitualRoleDef3.applyPowerOffset)
					{
						foreach (Pawn thing in list3)
						{
							num3 += thing.GetStatValue(StatDefOf.PsychicRitualQualityOffset, true, -1) / (float)num;
						}
					}
				}
				if (!Mathf.Approximately(num3, 0f))
				{
					power += num3;
					if (powerFactorsOut != null)
					{
						powerFactorsOut.Add(new QualityFactor
						{
							label = "PsychicRitualDef_InvocationCircle_QualityFactor_Ideoligion".Translate(),
							positive = (num3 > 0f),
							count = num3.ToStringPercent(),
							quality = num3,
							toolTip = "PsychicRitualDef_InvocationCircle_QualityFactor_Ideoligion_Tooltip".Translate()
						});
					}
				}
			}
			power = Mathf.Clamp01(power);
		}

		// Token: 0x06008804 RID: 34820 RVA: 0x0028A188 File Offset: 0x00288388
		public virtual void RemoveIncapablePawns(PsychicRitual psychicRitual)
		{
			foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> keyValuePair in psychicRitual.assignments.RoleAssignments)
			{
				PsychicRitualRoleDef psychicRitualRoleDef;
				List<Pawn> list;
				keyValuePair.Deconstruct(out psychicRitualRoleDef, out list);
				PsychicRitualRoleDef psychicRitualRoleDef2 = psychicRitualRoleDef;
				List<Pawn> collection = list;
				PsychicRitualDef.tmpPawnsIterationList.Clear();
				PsychicRitualDef.tmpPawnsIterationList.AddRange(collection);
				foreach (Pawn pawn in PsychicRitualDef.tmpPawnsIterationList)
				{
					PsychicRitualRoleDef.Reason reason;
					if (!psychicRitualRoleDef2.PawnCanDo(PsychicRitualRoleDef.Context.Runtime, pawn, psychicRitual.assignments.Target, out reason) && psychicRitual.LeaveOrCancelPsychicRitual(psychicRitualRoleDef2, pawn, reason.ToPlayerReadable()) == PsychicRitual.LeftOrCanceled.Canceled)
					{
						return;
					}
				}
			}
		}

		// Token: 0x06008805 RID: 34821 RVA: 0x0028A268 File Offset: 0x00288468
		public virtual void CheckPsychicRitualCancelConditions(PsychicRitual psychicRitual)
		{
			TargetInfo target = psychicRitual.assignments.Target;
			if (target.ThingDestroyed)
			{
				psychicRitual.CancelPsychicRitual("PsychicRitualDef_TargetDestroyed".Translate(target.Thing.Named("TARGET")));
			}
		}

		// Token: 0x06008806 RID: 34822 RVA: 0x0028A2AB File Offset: 0x002884AB
		public virtual TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			return TaggedString.Empty;
		}

		// Token: 0x06008807 RID: 34823 RVA: 0x0028A2B2 File Offset: 0x002884B2
		public virtual IEnumerable<TaggedString> OutcomeWarnings(PsychicRitualRoleAssignments assignments)
		{
			return Enumerable.Empty<TaggedString>();
		}

		// Token: 0x06008808 RID: 34824 RVA: 0x0028A2AB File Offset: 0x002884AB
		public virtual TaggedString TimeAndOfferingLabel()
		{
			return TaggedString.Empty;
		}

		// Token: 0x06008809 RID: 34825 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void InitializeCast(Map map)
		{
		}

		// Token: 0x0600880A RID: 34826 RVA: 0x0028A2BC File Offset: 0x002884BC
		public virtual IntVec3 GetBestStandableRolePosition(bool playerRitual, IntVec3 origin, IntVec3 ritualPosition, Map map, float radius = 8f)
		{
			if (playerRitual)
			{
				return origin;
			}
			IntVec3 result = CellFinder.StandableCellNear(origin, map, radius, (IntVec3 c) => map.reachability.CanReach(ritualPosition, c, PathEndMode.OnCell, TraverseMode.NoPassClosedDoorsOrWater));
			if (result.IsValid)
			{
				return result;
			}
			return origin;
		}

		// Token: 0x0600880B RID: 34827 RVA: 0x0028A30A File Offset: 0x0028850A
		public virtual IEnumerable<string> GetPawnTooltipExtras(Pawn pawn)
		{
			yield break;
		}

		// Token: 0x040058AC RID: 22700
		public ResearchProjectDef researchPrerequisite;

		// Token: 0x040058AD RID: 22701
		public bool allowsDrafting;

		// Token: 0x040058AE RID: 22702
		public bool allowsFloatMenu;

		// Token: 0x040058AF RID: 22703
		public int cooldownHours;

		// Token: 0x040058B0 RID: 22704
		public bool nonRequiredPawnsMayLeave;

		// Token: 0x040058B1 RID: 22705
		public float rolePowerFactor = 0.2f;

		// Token: 0x040058B2 RID: 22706
		public bool aiCastable;

		// Token: 0x040058B3 RID: 22707
		public bool playerCastable = true;

		// Token: 0x040058B4 RID: 22708
		public float minThreatPoints;

		// Token: 0x040058B5 RID: 22709
		public bool castableOnPocketMaps = true;

		// Token: 0x040058B6 RID: 22710
		public List<PlanetLayerDef> layerWhitelist = new List<PlanetLayerDef>();

		// Token: 0x040058B7 RID: 22711
		[MustTranslate]
		public string letterAICompleteLabel;

		// Token: 0x040058B8 RID: 22712
		[MustTranslate]
		public string letterAICompleteText;

		// Token: 0x040058B9 RID: 22713
		[MustTranslate]
		public string letterAIArrivedText;

		// Token: 0x040058BA RID: 22714
		[NoTranslate]
		public string iconPath;

		// Token: 0x040058BB RID: 22715
		public Texture2D uiIcon = BaseContent.BadTex;

		// Token: 0x040058BC RID: 22716
		private List<PsychicRitualRoleDef> rolesBackingList = new List<PsychicRitualRoleDef>(8);

		// Token: 0x040058BD RID: 22717
		private static readonly List<Pawn> tmpPawnsIterationList = new List<Pawn>(16);
	}
}
