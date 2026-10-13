using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02003799 RID: 14233
	public class RoyalTitleDef : Def
	{
		// Token: 0x17003262 RID: 12898
		// (get) Token: 0x06013B52 RID: 80722 RVA: 0x005DAE51 File Offset: 0x005D9051
		public bool Awardable
		{
			get
			{
				return this.favorCost > 0;
			}
		}

		// Token: 0x17003263 RID: 12899
		// (get) Token: 0x06013B53 RID: 80723 RVA: 0x005DAE5C File Offset: 0x005D905C
		public IEnumerable<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				List<WorkTypeDef> list = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					if ((this.disabledWorkTags & list[i].workTags) != WorkTags.None)
					{
						yield return list[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17003264 RID: 12900
		// (get) Token: 0x06013B54 RID: 80724 RVA: 0x005DAE6C File Offset: 0x005D906C
		public RoyalTitleInheritanceWorker InheritanceWorkerOverride
		{
			get
			{
				if (this.inheritanceWorkerOverride == null && this.inheritanceWorkerOverrideClass != null)
				{
					this.inheritanceWorkerOverride = (RoyalTitleInheritanceWorker)Activator.CreateInstance(this.inheritanceWorkerOverrideClass);
				}
				return this.inheritanceWorkerOverride;
			}
		}

		// Token: 0x06013B55 RID: 80725 RVA: 0x005DAEA0 File Offset: 0x005D90A0
		public RoyalTitleInheritanceWorker GetInheritanceWorker(Faction faction)
		{
			if (this.inheritanceWorkerOverrideClass == null)
			{
				return faction.def.RoyalTitleInheritanceWorker;
			}
			return this.InheritanceWorkerOverride;
		}

		// Token: 0x17003265 RID: 12901
		// (get) Token: 0x06013B56 RID: 80726 RVA: 0x005DAEC4 File Offset: 0x005D90C4
		public float MinThroneRoomImpressiveness
		{
			get
			{
				if (this.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
				{
					return 0f;
				}
				RoomRequirement_Impressiveness roomRequirement_Impressiveness = this.throneRoomRequirements.OfType<RoomRequirement_Impressiveness>().FirstOrDefault<RoomRequirement_Impressiveness>();
				if (roomRequirement_Impressiveness == null)
				{
					return 0f;
				}
				return (float)roomRequirement_Impressiveness.impressiveness;
			}
		}

		// Token: 0x17003266 RID: 12902
		// (get) Token: 0x06013B57 RID: 80727 RVA: 0x005DAF05 File Offset: 0x005D9105
		public RoyalTitleAwardWorker AwardWorker
		{
			get
			{
				if (this.awardWorker == null)
				{
					this.awardWorker = (RoyalTitleAwardWorker)Activator.CreateInstance(this.awardWorkerClass);
					this.awardWorker.def = this;
				}
				return this.awardWorker;
			}
		}

		// Token: 0x06013B58 RID: 80728 RVA: 0x005DAF37 File Offset: 0x005D9137
		public string GetLabelFor(Pawn p)
		{
			if (p == null)
			{
				return this.GetLabelForBothGenders();
			}
			return this.GetLabelFor(p.gender);
		}

		// Token: 0x06013B59 RID: 80729 RVA: 0x005DAF4F File Offset: 0x005D914F
		public string GetLabelFor(Gender g)
		{
			if (g != Gender.Female)
			{
				return this.label;
			}
			if (string.IsNullOrEmpty(this.labelFemale))
			{
				return this.label;
			}
			return this.labelFemale;
		}

		// Token: 0x06013B5A RID: 80730 RVA: 0x005DAF76 File Offset: 0x005D9176
		public string GetLabelForBothGenders()
		{
			if (!string.IsNullOrEmpty(this.labelFemale))
			{
				return this.label + " / " + this.labelFemale;
			}
			return this.label;
		}

		// Token: 0x06013B5B RID: 80731 RVA: 0x005DAFA2 File Offset: 0x005D91A2
		public string GetLabelCapForBothGenders()
		{
			if (!string.IsNullOrEmpty(this.labelFemale))
			{
				return this.LabelCap + " / " + this.labelFemale.CapitalizeFirst();
			}
			return this.LabelCap;
		}

		// Token: 0x06013B5C RID: 80732 RVA: 0x005DAFE2 File Offset: 0x005D91E2
		public string GetLabelCapFor(Pawn p)
		{
			return this.GetLabelFor(p).CapitalizeFirst(this);
		}

		// Token: 0x06013B5D RID: 80733 RVA: 0x005DAFF1 File Offset: 0x005D91F1
		public IEnumerable<RoomRequirement> GetBedroomRequirements(Pawn p)
		{
			if (p.story.traits.HasTrait(TraitDefOf.Ascetic))
			{
				return null;
			}
			return this.bedroomRequirements;
		}

		// Token: 0x06013B5E RID: 80734 RVA: 0x005DB012 File Offset: 0x005D9212
		public string GetReportText(Faction faction)
		{
			return this.description + "\n\n" + RoyalTitleUtility.GetTitleProgressionInfo(faction, null);
		}

		// Token: 0x06013B5F RID: 80735 RVA: 0x005DB02B File Offset: 0x005D922B
		public bool JoyKindDisabled(JoyKindDef joyKind)
		{
			return this.disabledJoyKinds != null && this.disabledJoyKinds.Contains(joyKind);
		}

		// Token: 0x06013B60 RID: 80736 RVA: 0x005DB044 File Offset: 0x005D9244
		private bool HasSameRoomRequirement(RoomRequirement otherReq, List<RoomRequirement> list)
		{
			if (list == null)
			{
				return false;
			}
			using (List<RoomRequirement>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.SameOrSubsetOf(otherReq))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06013B61 RID: 80737 RVA: 0x005DB0A0 File Offset: 0x005D92A0
		public bool HasSameThroneroomRequirement(RoomRequirement otherReq)
		{
			return this.HasSameRoomRequirement(otherReq, this.throneRoomRequirements);
		}

		// Token: 0x06013B62 RID: 80738 RVA: 0x005DB0AF File Offset: 0x005D92AF
		public bool HasSameBedroomRequirement(RoomRequirement otherReq)
		{
			return this.HasSameRoomRequirement(otherReq, this.bedroomRequirements);
		}

		// Token: 0x06013B63 RID: 80739 RVA: 0x005DB0C0 File Offset: 0x005D92C0
		public int MaxAllowedPsylinkLevel(FactionDef faction)
		{
			int result = 0;
			for (int i = 0; i < faction.royalImplantRules.Count; i++)
			{
				RoyalImplantRule royalImplantRule = faction.royalImplantRules[i];
				if (royalImplantRule.implantHediff == HediffDefOf.PsychicAmplifier && royalImplantRule.minTitle.Awardable && royalImplantRule.minTitle.seniority <= this.seniority)
				{
					result = royalImplantRule.maxLevel;
				}
			}
			return result;
		}

		// Token: 0x06013B64 RID: 80740 RVA: 0x005DB128 File Offset: 0x005D9328
		public IEnumerable<ThingDef> SatisfyingMeals(bool includeDrugs = true)
		{
			if (includeDrugs)
			{
				if (this.satisfyingMealsCached == null)
				{
					this.satisfyingMealsCached = (from t in DefDatabase<ThingDef>.AllDefsListForReading
					where this.foodRequirement.Acceptable(t)
					orderby t.ingestible.preferability descending
					select t).ToList<ThingDef>();
				}
			}
			else if (this.satisfyingMealsNoDrugsCached == null)
			{
				this.satisfyingMealsNoDrugsCached = (from t in DefDatabase<ThingDef>.AllDefsListForReading
				where this.foodRequirement.Acceptable(t) && !t.IsDrug
				orderby t.ingestible.preferability descending
				select t).ToList<ThingDef>();
			}
			if (!includeDrugs)
			{
				return this.satisfyingMealsNoDrugsCached;
			}
			return this.satisfyingMealsCached;
		}

		// Token: 0x06013B65 RID: 80741 RVA: 0x005DB1E7 File Offset: 0x005D93E7
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (!this.permits.NullOrEmpty<RoyalTitlePermitDef>())
			{
				TaggedString taggedString = "RoyalTitleTooltipPermits".Translate();
				string valueString = (from r in this.permits
				select r.label).ToCommaList(false, false).CapitalizeFirst();
				string reportText = (from r in this.permits
				select r.LabelCap.ToString()).ToLineList("  - ", true);
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString, valueString, reportText, 99999, null, null, false, false);
			}
			if (this.requiredMinimumApparelQuality > QualityCategory.Awful)
			{
				TaggedString taggedString2 = "RoyalTitleTooltipRequiredApparelQuality".Translate();
				string text = this.requiredMinimumApparelQuality.GetLabel().CapitalizeFirst();
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString2, text, text, 99998, null, null, false, false);
			}
			if (!this.requiredApparel.NullOrEmpty<ApparelRequirement>())
			{
				TaggedString taggedString3 = "RoyalTitleTooltipRequiredApparel".Translate();
				TaggedString t2 = "Male".Translate().CapitalizeFirst() + ":\n" + this.RequiredApparelListForGender(Gender.Male, req.Pawn).ToLineList("  - ", false) + "\n\n" + "Female".Translate().CapitalizeFirst() + ":\n" + this.RequiredApparelListForGender(Gender.Female, req.Pawn).ToLineList("  - ", false);
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString3, "", "RoyalTitleRequiredApparelStatDescription".Translate() + ":\n\n" + t2, 99998, null, null, false, false);
			}
			if (!this.bedroomRequirements.NullOrEmpty<RoomRequirement>())
			{
				TaggedString taggedString4 = "RoyalTitleTooltipBedroomRequirements".Translate();
				string valueString2 = (from r in this.bedroomRequirements
				select r.Label(null)).ToCommaList(false, false).CapitalizeFirst();
				string reportText2 = (from r in this.bedroomRequirements
				select r.LabelCap(null)).ToLineList("  - ", false);
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString4, valueString2, reportText2, 99997, null, null, false, false);
			}
			if (!this.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
			{
				TaggedString taggedString5 = "RoyalTitleTooltipThroneroomRequirements".Translate();
				string valueString3 = (from r in this.throneRoomRequirements
				select r.Label(null)).ToCommaList(false, false).CapitalizeFirst();
				string reportText3 = (from r in this.throneRoomRequirements
				select r.LabelCap(null)).ToArray<string>().ToLineList("  - ");
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString5, valueString3, reportText3, 99997, null, null, false, false);
			}
			IEnumerable<string> enumerable = from t in this.disabledWorkTags.GetAllSelectedItems<WorkTags>()
			where t > WorkTags.None
			select t into w
			select w.LabelTranslated();
			if (enumerable.Any<string>())
			{
				TaggedString taggedString6 = "DisabledWorkTypes".Translate();
				string valueString4 = enumerable.ToCommaList(false, false).CapitalizeFirst();
				string reportText4 = enumerable.ToLineList(" -  ", true);
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, taggedString6, valueString4, reportText4, 99994, null, null, false, false);
			}
			if (this.foodRequirement.Defined && this.SatisfyingMeals(true).Any<ThingDef>())
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "RoyalTitleRequiredMeals".Translate(), (from m in this.SatisfyingMeals(true)
				select m.label).ToCommaList(false, false).CapitalizeFirst(), "RoyalTitleRequiredMealsDesc".Translate(), 99995, null, null, false, false);
			}
			yield break;
		}

		// Token: 0x06013B66 RID: 80742 RVA: 0x005DB1FE File Offset: 0x005D93FE
		private IEnumerable<string> RequiredApparelListForGender(Gender g, Pawn forPawn = null)
		{
			bool anyRequirementValid = false;
			foreach (ApparelRequirement apparelRequirement in this.requiredApparel)
			{
				string appendDisabledRequirement = null;
				string t;
				if (forPawn != null && !ApparelUtility.IsRequirementActive(apparelRequirement, ApparelRequirementSource.Title, forPawn, out t))
				{
					appendDisabledRequirement = " [" + "ApparelRequirementDisabledLabel".Translate() + ": " + t + "]";
				}
				else
				{
					anyRequirementValid = true;
				}
				foreach (TaggedString taggedString in from a in apparelRequirement.AllRequiredApparel(g).Distinct<ThingDef>()
				select a.LabelCap)
				{
					if (appendDisabledRequirement == null)
					{
						yield return taggedString;
					}
					else
					{
						yield return taggedString + " " + appendDisabledRequirement;
					}
				}
				IEnumerator<TaggedString> enumerator2 = null;
				appendDisabledRequirement = null;
			}
			List<ApparelRequirement>.Enumerator enumerator = default(List<ApparelRequirement>.Enumerator);
			if (anyRequirementValid)
			{
				yield return "ApparelRequirementAnyPrestigeArmor".Translate();
				yield return "ApparelRequirementAnyPsycasterApparel".Translate();
				if (ModsConfig.BiotechActive)
				{
					yield return "ApparelRequirementAnyMechlordApparel".Translate();
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06013B67 RID: 80743 RVA: 0x005DB21C File Offset: 0x005D941C
		public IEnumerable<DefHyperlink> GetHyperlinks(Faction faction)
		{
			IEnumerable<DefHyperlink> descriptionHyperlinks = this.descriptionHyperlinks;
			return descriptionHyperlinks ?? (from t in faction.def.RoyalTitlesAllInSeniorityOrderForReading
			where t != this
			select new DefHyperlink(t, faction));
		}

		// Token: 0x06013B68 RID: 80744 RVA: 0x005DB27B File Offset: 0x005D947B
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			ModLister.CheckRoyalty("Royal title");
			if (this.awardThought != null && !typeof(Thought_MemoryRoyalTitle).IsAssignableFrom(this.awardThought.thoughtClass))
			{
				yield return string.Format("Royal title {0} has awardThought with thoughtClass {1} which is not deriving from Thought_MemoryRoyalTitle!", this.defName, this.awardThought.thoughtClass.FullName);
			}
			if (this.lostThought != null && !typeof(Thought_MemoryRoyalTitle).IsAssignableFrom(this.lostThought.thoughtClass))
			{
				yield return string.Format("Royal title {0} has awardThought with thoughtClass {1} which is not deriving from Thought_MemoryRoyalTitle!", this.defName, this.awardThought.thoughtClass.FullName);
			}
			if (this.disabledJoyKinds != null)
			{
				foreach (JoyKindDef joyKindDef in this.disabledJoyKinds)
				{
					if (joyKindDef.titleRequiredAny != null && joyKindDef.titleRequiredAny.Contains(this))
					{
						yield return string.Format("Royal title {0} disables joy kind {1} which requires the title!", this.defName, joyKindDef.defName);
					}
				}
				List<JoyKindDef>.Enumerator enumerator2 = default(List<JoyKindDef>.Enumerator);
			}
			if (!typeof(RoyalTitleAwardWorker).IsAssignableFrom(this.awardWorkerClass))
			{
				yield return "awardWorkerClass does not derive from RoyalTitleAwardWorker";
			}
			if (this.Awardable && this.changeHeirQuestPoints < 0)
			{
				yield return "undefined changeHeirQuestPoints, it's required for awardable titles";
			}
			if (!this.throneRoomRequirements.NullOrEmpty<RoomRequirement>())
			{
				foreach (RoomRequirement req in this.throneRoomRequirements)
				{
					foreach (string arg in req.ConfigErrors())
					{
						yield return string.Format("Room requirement {0}: {1}", req.GetType().Name, arg);
					}
					enumerator = null;
					req = null;
				}
				List<RoomRequirement>.Enumerator enumerator3 = default(List<RoomRequirement>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x0400DA4A RID: 55882
		public int seniority;

		// Token: 0x0400DA4B RID: 55883
		public int favorCost;

		// Token: 0x0400DA4C RID: 55884
		[MustTranslate]
		public string labelFemale;

		// Token: 0x0400DA4D RID: 55885
		public int changeHeirQuestPoints = -1;

		// Token: 0x0400DA4E RID: 55886
		public float commonality = 1f;

		// Token: 0x0400DA4F RID: 55887
		public WorkTags disabledWorkTags;

		// Token: 0x0400DA50 RID: 55888
		public Type inheritanceWorkerOverrideClass;

		// Token: 0x0400DA51 RID: 55889
		public QualityCategory requiredMinimumApparelQuality;

		// Token: 0x0400DA52 RID: 55890
		public List<ApparelRequirement> requiredApparel;

		// Token: 0x0400DA53 RID: 55891
		public List<RoyalTitlePermitDef> permits;

		// Token: 0x0400DA54 RID: 55892
		public ExpectationDef minExpectation;

		// Token: 0x0400DA55 RID: 55893
		public List<JoyKindDef> disabledJoyKinds;

		// Token: 0x0400DA56 RID: 55894
		[NoTranslate]
		public List<string> tags;

		// Token: 0x0400DA57 RID: 55895
		public List<ThingDefCountClass> rewards;

		// Token: 0x0400DA58 RID: 55896
		public bool suppressIdleAlert;

		// Token: 0x0400DA59 RID: 55897
		public bool canBeInherited;

		// Token: 0x0400DA5A RID: 55898
		public bool allowDignifiedMeditationFocus = true;

		// Token: 0x0400DA5B RID: 55899
		public int permitPointsAwarded;

		// Token: 0x0400DA5C RID: 55900
		public Type awardWorkerClass = typeof(RoyalTitleAwardWorker);

		// Token: 0x0400DA5D RID: 55901
		public ThoughtDef awardThought;

		// Token: 0x0400DA5E RID: 55902
		public ThoughtDef lostThought;

		// Token: 0x0400DA5F RID: 55903
		public List<RoomRequirement> throneRoomRequirements;

		// Token: 0x0400DA60 RID: 55904
		public List<RoomRequirement> bedroomRequirements;

		// Token: 0x0400DA61 RID: 55905
		public float recruitmentResistanceOffset;

		// Token: 0x0400DA62 RID: 55906
		public RoyalTitleFoodRequirement foodRequirement;

		// Token: 0x0400DA63 RID: 55907
		public RoyalTitleDef replaceOnRecruited;

		// Token: 0x0400DA64 RID: 55908
		public float decreeMtbDays = -1f;

		// Token: 0x0400DA65 RID: 55909
		public float decreeMinIntervalDays = 2f;

		// Token: 0x0400DA66 RID: 55910
		public float decreeMentalBreakCommonality;

		// Token: 0x0400DA67 RID: 55911
		public List<string> decreeTags;

		// Token: 0x0400DA68 RID: 55912
		public List<AbilityDef> grantedAbilities = new List<AbilityDef>();

		// Token: 0x0400DA69 RID: 55913
		public IntRange speechCooldown;

		// Token: 0x0400DA6A RID: 55914
		public int maxPsylinkLevel;

		// Token: 0x0400DA6B RID: 55915
		[Unsaved(false)]
		private List<ThingDef> satisfyingMealsCached;

		// Token: 0x0400DA6C RID: 55916
		[Unsaved(false)]
		private List<ThingDef> satisfyingMealsNoDrugsCached;

		// Token: 0x0400DA6D RID: 55917
		private RoyalTitleAwardWorker awardWorker;

		// Token: 0x0400DA6E RID: 55918
		private RoyalTitleInheritanceWorker inheritanceWorkerOverride;
	}
}
