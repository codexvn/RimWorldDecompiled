using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001695 RID: 5781
	public class PsychicRitualDef_InvocationCircle : PsychicRitualDef
	{
		// Token: 0x170014D2 RID: 5330
		// (get) Token: 0x06008845 RID: 34885 RVA: 0x0028AB1A File Offset: 0x00288D1A
		public virtual PsychicRitualRoleDef InvokerRole
		{
			get
			{
				return this.invokerRole;
			}
		}

		// Token: 0x170014D3 RID: 5331
		// (get) Token: 0x06008846 RID: 34886 RVA: 0x0028AB22 File Offset: 0x00288D22
		public virtual PsychicRitualRoleDef ChanterRole
		{
			get
			{
				return this.chanterRole;
			}
		}

		// Token: 0x170014D4 RID: 5332
		// (get) Token: 0x06008847 RID: 34887 RVA: 0x0028AB2A File Offset: 0x00288D2A
		public virtual PsychicRitualRoleDef TargetRole
		{
			get
			{
				return this.targetRole;
			}
		}

		// Token: 0x170014D5 RID: 5333
		// (get) Token: 0x06008848 RID: 34888 RVA: 0x0028AB32 File Offset: 0x00288D32
		public virtual PsychicRitualRoleDef DefenderRole
		{
			get
			{
				return this.defenderRole;
			}
		}

		// Token: 0x170014D6 RID: 5334
		// (get) Token: 0x06008849 RID: 34889 RVA: 0x0028AB3A File Offset: 0x00288D3A
		public virtual IngredientCount RequiredOffering
		{
			get
			{
				return this.requiredOffering;
			}
		}

		// Token: 0x170014D7 RID: 5335
		// (get) Token: 0x0600884A RID: 34890 RVA: 0x0028AB42 File Offset: 0x00288D42
		public TaggedString CooldownLabel
		{
			get
			{
				return "PsychicRitualCooldownLabel".Translate() + ": " + (this.cooldownHours * 2500).ToStringTicksToPeriod(true, false, true, true, false);
			}
		}

		// Token: 0x170014D8 RID: 5336
		// (get) Token: 0x0600884B RID: 34891 RVA: 0x0028AB74 File Offset: 0x00288D74
		public override List<PsychicRitualRoleDef> Roles
		{
			get
			{
				List<PsychicRitualRoleDef> roles = base.Roles;
				if (this.InvokerRole != null)
				{
					roles.Add(this.InvokerRole);
				}
				if (this.TargetRole != null)
				{
					roles.Add(this.TargetRole);
				}
				if (this.ChanterRole != null)
				{
					roles.Add(this.ChanterRole);
				}
				if (this.DefenderRole != null)
				{
					roles.Add(this.DefenderRole);
				}
				return roles;
			}
		}

		// Token: 0x0600884C RID: 34892 RVA: 0x0028ABDC File Offset: 0x00288DDC
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			IngredientCount ingredientCount = this.requiredOffering;
			if (ingredientCount != null)
			{
				ingredientCount.ResolveReferences();
			}
			this.invokerRole = (this.invokerRole ?? PsychicRitualRoleDefOf.Invoker);
			this.chanterRole = (this.chanterRole ?? PsychicRitualRoleDefOf.Chanter);
		}

		// Token: 0x0600884D RID: 34893 RVA: 0x0028AC2C File Offset: 0x00288E2C
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			float randomInRange = this.hoursUntilOutcome.RandomInRange;
			IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> readOnlyDictionary = this.GenerateRolePositions(psychicRitual.assignments);
			return new List<PsychicRitualToil>
			{
				new PsychicRitualToil_GatherForInvocation(psychicRitual, this, readOnlyDictionary),
				new PsychicRitualToil_InvokeHorax(this.InvokerRole, readOnlyDictionary.TryGetValue(this.InvokerRole, null), this.TargetRole, readOnlyDictionary.TryGetValue(this.TargetRole, null), this.ChanterRole, readOnlyDictionary.TryGetValue(this.ChanterRole, null), this.DefenderRole, readOnlyDictionary.TryGetValue(this.DefenderRole, null), this.RequiredOffering)
				{
					hoursUntilHoraxEffect = this.hoursUntilHoraxEffect.RandomInRange,
					hoursUntilOutcome = randomInRange
				},
				new PsychicRitualToil_Wait(120)
			};
		}

		// Token: 0x0600884E RID: 34894 RVA: 0x0028ACE8 File Offset: 0x00288EE8
		public override bool IsValidTarget(TargetInfo target, out AnyEnum reason)
		{
			using (IEnumerator<IntVec3> enumerator = GenRadial.RadialCellsAround(target.Cell, this.invocationCircleRadius, true).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Standable(target.Map))
					{
						reason = AnyEnum.FromEnum<PsychicRitualDef_InvocationCircle.InvalidTargetReasonEnum>(PsychicRitualDef_InvocationCircle.InvalidTargetReasonEnum.AreaNotClear);
						return false;
					}
				}
			}
			reason = AnyEnum.None;
			return true;
		}

		// Token: 0x0600884F RID: 34895 RVA: 0x0028AD68 File Offset: 0x00288F68
		public override TaggedString InvalidTargetReason(AnyEnum reason)
		{
			PsychicRitualDef_InvocationCircle.InvalidTargetReasonEnum? invalidTargetReasonEnum = reason.As<PsychicRitualDef_InvocationCircle.InvalidTargetReasonEnum>();
			if (invalidTargetReasonEnum == null)
			{
				return base.InvalidTargetReason(reason);
			}
			PsychicRitualDef_InvocationCircle.InvalidTargetReasonEnum valueOrDefault = invalidTargetReasonEnum.GetValueOrDefault();
			if (valueOrDefault == PsychicRitualDef_InvocationCircle.InvalidTargetReasonEnum.None)
			{
				return TaggedString.Empty;
			}
			if (valueOrDefault != PsychicRitualDef_InvocationCircle.InvalidTargetReasonEnum.AreaNotClear)
			{
				throw new InvalidOperationException(string.Format("Unknown reason {0}", valueOrDefault));
			}
			return "PsychicRitualDef_InvocationCircle_AreaMustBeClear".Translate();
		}

		// Token: 0x06008850 RID: 34896 RVA: 0x0028ADC5 File Offset: 0x00288FC5
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			return this.outcomeDescription.Formatted(Array.Empty<NamedArgument>());
		}

		// Token: 0x06008851 RID: 34897 RVA: 0x0028ADD7 File Offset: 0x00288FD7
		public override IEnumerable<TaggedString> OutcomeWarnings(PsychicRitualRoleAssignments assignments)
		{
			foreach (Pawn pawn in assignments.AssignedPawns(this.TargetRole))
			{
				if (pawn.HomeFaction != null && pawn.HomeFaction != Faction.OfPlayer && pawn.HomeFaction.def.humanlikeFaction && !pawn.HomeFaction.def.PermanentlyHostileTo(FactionDefOf.PlayerColony) && !pawn.HomeFaction.temporary && !pawn.HomeFaction.Hidden)
				{
					yield return "PsychicRitualFactionWarning".Translate(pawn.Named("PAWN"), pawn.HomeFaction.Named("FACTION")).Colorize(ColoredText.WarningColor);
				}
			}
			IEnumerator<Pawn> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06008852 RID: 34898 RVA: 0x0028ADF0 File Offset: 0x00288FF0
		public override TaggedString TimeAndOfferingLabel()
		{
			if (this.timeAndOfferingLabelCached != null)
			{
				return this.timeAndOfferingLabelCached;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.DurationLabel());
			stringBuilder.Append(this.CooldownLabel);
			if (!this.OfferingLabel().NullOrEmpty())
			{
				stringBuilder.AppendLine();
				stringBuilder.Append(this.OfferingLabel());
			}
			this.timeAndOfferingLabelCached = stringBuilder.ToString();
			return this.timeAndOfferingLabelCached;
		}

		// Token: 0x06008853 RID: 34899 RVA: 0x0028AE7C File Offset: 0x0028907C
		private TaggedString OfferingLabel()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.RequiredOffering != null)
			{
				stringBuilder.Append("PsychicRitualRequiredOffering".Translate().CapitalizeFirst());
				stringBuilder.Append(": ");
				stringBuilder.Append(this.RequiredOffering.SummaryFilterFirst);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06008854 RID: 34900 RVA: 0x0028AEE0 File Offset: 0x002890E0
		public TaggedString DurationLabel()
		{
			string value = ((int)(this.hoursUntilOutcome.Average * 2500f)).ToStringTicksToPeriod(true, false, true, true, false);
			TaggedString taggedString;
			if (this.hoursUntilOutcome.min == this.hoursUntilOutcome.max)
			{
				taggedString = "PsychicRitualExpectedDurationLabel".Translate().CapitalizeFirst();
			}
			else
			{
				taggedString = "ExpectedLordJobDuration".Translate().CapitalizeFirst();
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(taggedString);
			stringBuilder.Append(": ");
			stringBuilder.Append(value);
			return stringBuilder.ToString();
		}

		// Token: 0x06008855 RID: 34901 RVA: 0x0028AF7C File Offset: 0x0028917C
		private IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> GenerateRolePositions(PsychicRitualRoleAssignments assignments)
		{
			PsychicRitualDef_InvocationCircle.tmpParticipants.ClearAndPoolValueLists<PsychicRitualRoleDef, IntVec3>();
			foreach (PsychicRitualRoleDef key in this.Roles)
			{
				PsychicRitualDef_InvocationCircle.tmpParticipants[key] = SimplePool<List<IntVec3>>.Get();
			}
			int num = assignments.RoleAssignedCount(this.ChanterRole) + assignments.RoleAssignedCount(this.InvokerRole);
			int num2 = 0;
			foreach (Pawn pawn in assignments.AssignedPawns(this.InvokerRole))
			{
				int num3 = 0;
				IntVec3 intVec;
				do
				{
					intVec = assignments.Target.Cell;
					intVec += IntVec3.FromPolar(360f * (float)num2++ / (float)num, this.invocationCircleRadius);
				}
				while (!intVec.Walkable(assignments.Target.Map) && num3++ <= 10);
				if (num3 >= 10)
				{
					intVec = assignments.Target.Cell;
				}
				PsychicRitualDef_InvocationCircle.tmpParticipants[this.InvokerRole].Add(intVec);
			}
			foreach (Pawn pawn2 in assignments.AssignedPawns(this.ChanterRole))
			{
				IntVec3 intVec2 = assignments.Target.Cell;
				intVec2 += IntVec3.FromPolar(360f * (float)num2++ / (float)num, this.invocationCircleRadius);
				PsychicRitualDef_InvocationCircle.tmpParticipants[this.ChanterRole].Add(intVec2);
			}
			foreach (Pawn pawn3 in assignments.AssignedPawns(this.TargetRole))
			{
				PsychicRitualDef_InvocationCircle.tmpParticipants[this.TargetRole].Add(assignments.Target.Cell);
			}
			if (this.DefenderRole != null)
			{
				num2 = 0;
				int num4 = assignments.RoleAssignedCount(this.DefenderRole);
				bool playerRitual = assignments.AllAssignedPawns.Any((Pawn x) => x.Faction == Faction.OfPlayer);
				foreach (Pawn pawn4 in assignments.AssignedPawns(this.DefenderRole))
				{
					IntVec3 intVec3 = assignments.Target.Cell;
					intVec3 += IntVec3.FromPolar(360f * (float)num2++ / (float)num4, this.invocationCircleRadius + 5f);
					intVec3 = this.GetBestStandableRolePosition(playerRitual, intVec3, assignments.Target.Cell, assignments.Target.Map, 8f);
					PsychicRitualDef_InvocationCircle.tmpParticipants[this.DefenderRole].Add(intVec3);
				}
			}
			return PsychicRitualDef_InvocationCircle.tmpParticipants;
		}

		// Token: 0x06008856 RID: 34902 RVA: 0x0028B2D0 File Offset: 0x002894D0
		public override IEnumerable<string> BlockingIssues(PsychicRitualRoleAssignments assignments, Map map)
		{
			using (new ProfilerBlock("PsychicRitualDef.BlockingIssues"))
			{
				this.tmpGatheringPawns.Clear();
				foreach (KeyValuePair<PsychicRitualRoleDef, List<Pawn>> keyValuePair in assignments.RoleAssignments)
				{
					PsychicRitualRoleDef psychicRitualRoleDef;
					List<Pawn> list;
					keyValuePair.Deconstruct(out psychicRitualRoleDef, out list);
					PsychicRitualRoleDef psychicRitualRoleDef2 = psychicRitualRoleDef;
					List<Pawn> collection = list;
					if (psychicRitualRoleDef2.CanHandleOfferings)
					{
						this.tmpGatheringPawns.AddRange(collection);
					}
				}
				this.tmpGatheringPawns.RemoveAll(map, (Map _map, Pawn _pawn) => _pawn.MapHeld != _map, false);
				if (this.TargetRole != null && this.InvokerRole != null)
				{
					Pawn pawn = assignments.FirstAssignedPawn(this.TargetRole);
					if (pawn != null)
					{
						Pawn pawn2 = assignments.FirstAssignedPawn(this.InvokerRole);
						if (pawn2 != null && pawn.IsPrisoner && !map.reachability.CanReach(assignments.Target.Cell, pawn.PositionHeld, PathEndMode.Touch, TraverseParms.For(pawn2, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true)))
						{
							yield return "PsychicRitualTargetUnreachableByInvoker".Translate(pawn.Named("TARGET"), pawn2.Named("INVOKER"));
						}
					}
				}
				int value;
				if (this.RequiredOffering != null && !PsychicRitualDef.OfferingReachable(map, this.tmpGatheringPawns, this.RequiredOffering, out value))
				{
					yield return "PsychicRitualOfferingsInsufficient".Translate(this.RequiredOffering.SummaryFilterFirst, value);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06008857 RID: 34903 RVA: 0x0028B2F0 File Offset: 0x002894F0
		public override void CalculateMaxPower(PsychicRitualRoleAssignments assignments, List<QualityFactor> powerFactorsOut, out float power)
		{
			power = 0f;
			foreach (Pawn pawn in assignments.AssignedPawns(this.InvokerRole))
			{
				float statValue = pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1);
				float num = PsychicRitualDef_InvocationCircle.PsychicSensitivityToPowerFactor.Evaluate(statValue);
				num *= this.psychicSensitivityPowerFactor;
				if (powerFactorsOut != null)
				{
					powerFactorsOut.Add(new QualityFactor
					{
						label = "PsychicRitualDef_InvocationCircle_QualityFactor_PsychicSensitivity".Translate(pawn.Named("PAWN")),
						positive = (statValue >= 1f),
						count = statValue.ToStringPercent(),
						quality = num,
						toolTip = "PsychicRitualDef_InvocationCircle_QualityFactor_PsychicSensitivity_Tooltip".Translate(pawn.Named("PAWN"))
					});
				}
				power += num;
			}
			float num2;
			base.CalculateMaxPower(assignments, powerFactorsOut, out num2);
			power += num2;
			Building building = assignments.Target.Thing as Building;
			if (building != null)
			{
				PsychicRitualDef_InvocationCircle.CalculateFacilityQualityOffset(powerFactorsOut, ref power, building);
			}
			power = Mathf.Clamp01(power);
		}

		// Token: 0x06008858 RID: 34904 RVA: 0x0028B428 File Offset: 0x00289628
		private static void CalculateFacilityQualityOffset(List<QualityFactor> powerFactorsOut, ref float power, Building building)
		{
			Dictionary<ThingDef, PsychicRitualDef_InvocationCircle.RitualQualityOffsetCount> dictionary = new Dictionary<ThingDef, PsychicRitualDef_InvocationCircle.RitualQualityOffsetCount>();
			List<Thing> linkedFacilitiesListForReading = building.GetComp<CompAffectedByFacilities>().LinkedFacilitiesListForReading;
			for (int i = 0; i < linkedFacilitiesListForReading.Count; i++)
			{
				Thing thing = linkedFacilitiesListForReading[i];
				CompFacility compFacility = thing.TryGetComp<CompFacility>();
				if (((compFacility != null) ? compFacility.StatOffsets : null) != null)
				{
					for (int j = 0; j < compFacility.StatOffsets.Count; j++)
					{
						StatModifier statModifier = compFacility.StatOffsets[j];
						if (statModifier.stat == StatDefOf.PsychicRitualQuality)
						{
							PsychicRitualDef_InvocationCircle.RitualQualityOffsetCount ritualQualityOffsetCount;
							if (dictionary.TryGetValue(thing.def, out ritualQualityOffsetCount))
							{
								ritualQualityOffsetCount.count++;
								ritualQualityOffsetCount.offset += statModifier.value;
							}
							else
							{
								dictionary.Add(thing.def, new PsychicRitualDef_InvocationCircle.RitualQualityOffsetCount(1, statModifier.value));
							}
						}
					}
				}
			}
			foreach (KeyValuePair<ThingDef, PsychicRitualDef_InvocationCircle.RitualQualityOffsetCount> keyValuePair in dictionary)
			{
				if (powerFactorsOut != null)
				{
					powerFactorsOut.Add(new QualityFactor
					{
						label = Find.ActiveLanguageWorker.Pluralize(keyValuePair.Key.label, -1).CapitalizeFirst(),
						positive = true,
						count = keyValuePair.Value.count.ToString() + " / " + keyValuePair.Key.GetCompProperties<CompProperties_Facility>().maxSimultaneous.ToString(),
						quality = keyValuePair.Value.offset,
						toolTip = "PsychicRitualDef_InvocationCircle_QualityFactor_Increase_Tooltip".Translate().CapitalizeFirst().EndWithPeriod()
					});
				}
				power += keyValuePair.Value.offset;
			}
		}

		// Token: 0x06008859 RID: 34905 RVA: 0x0028B60C File Offset: 0x0028980C
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.requiredOffering != null)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.PsychicRituals, "StatsReport_Offering".Translate(), this.requiredOffering.SummaryFilterFirst, "StatsReport_Offering_Desc".Translate(), 1000, null, null, false, false);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.PsychicRituals, "StatsReport_RitualDuration".Translate(), Mathf.FloorToInt(this.hoursUntilOutcome.min * 2500f).ToStringTicksToPeriod(true, false, true, true, false), "StatsReport_RitualDuration_Desc".Translate(), 500, null, null, false, false);
			yield return new StatDrawEntry(StatCategoryDefOf.PsychicRituals, "StatsReport_RitualCooldown".Translate(), (this.cooldownHours * 2500).ToStringTicksToPeriod(true, false, true, true, false), "StatsReport_RitualCooldown_Desc".Translate(), 100, null, null, false, false);
			yield break;
			yield break;
		}

		// Token: 0x0600885A RID: 34906 RVA: 0x0028B624 File Offset: 0x00289824
		public override void CheckPsychicRitualCancelConditions(PsychicRitual psychicRitual)
		{
			base.CheckPsychicRitualCancelConditions(psychicRitual);
			if (psychicRitual.canceled)
			{
				return;
			}
			if (this.invokerRole != null)
			{
				Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.InvokerRole);
				if (pawn != null && pawn.DeadOrDowned)
				{
					psychicRitual.CancelPsychicRitual("PsychicRitualDef_InvocationCircle_InvokerLost".Translate(pawn.Named("PAWN")));
				}
			}
		}

		// Token: 0x040058DE RID: 22750
		public FloatRange hoursUntilHoraxEffect;

		// Token: 0x040058DF RID: 22751
		public FloatRange hoursUntilOutcome;

		// Token: 0x040058E0 RID: 22752
		public float invocationCircleRadius = 3.9f;

		// Token: 0x040058E1 RID: 22753
		[MustTranslate]
		public string outcomeDescription;

		// Token: 0x040058E2 RID: 22754
		public float psychicSensitivityPowerFactor = 0.25f;

		// Token: 0x040058E3 RID: 22755
		protected PsychicRitualRoleDef invokerRole;

		// Token: 0x040058E4 RID: 22756
		protected PsychicRitualRoleDef chanterRole;

		// Token: 0x040058E5 RID: 22757
		protected PsychicRitualRoleDef targetRole;

		// Token: 0x040058E6 RID: 22758
		protected PsychicRitualRoleDef defenderRole;

		// Token: 0x040058E7 RID: 22759
		protected IngredientCount requiredOffering;

		// Token: 0x040058E8 RID: 22760
		protected string timeAndOfferingLabelCached;

		// Token: 0x040058E9 RID: 22761
		public static readonly SimpleCurve PsychicSensitivityToPowerFactor = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 0.5f),
				true
			},
			{
				new CurvePoint(2f, 0.9f),
				true
			},
			{
				new CurvePoint(3f, 1f),
				true
			}
		};

		// Token: 0x040058EA RID: 22762
		protected const int DurationTicksWaitPostEffect = 120;

		// Token: 0x040058EB RID: 22763
		private static Dictionary<PsychicRitualRoleDef, List<IntVec3>> tmpParticipants = new Dictionary<PsychicRitualRoleDef, List<IntVec3>>(8);

		// Token: 0x040058EC RID: 22764
		private List<Pawn> tmpGatheringPawns = new List<Pawn>(8);

		// Token: 0x02001696 RID: 5782
		public enum InvalidTargetReasonEnum
		{
			// Token: 0x040058EE RID: 22766
			None,
			// Token: 0x040058EF RID: 22767
			AreaNotClear
		}

		// Token: 0x02001697 RID: 5783
		private class RitualQualityOffsetCount
		{
			// Token: 0x0600885E RID: 34910 RVA: 0x0028B726 File Offset: 0x00289926
			public RitualQualityOffsetCount(int count, float offset)
			{
				this.count = count;
				this.offset = offset;
			}

			// Token: 0x040058F0 RID: 22768
			public float offset;

			// Token: 0x040058F1 RID: 22769
			public int count;
		}
	}
}
