using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019E3 RID: 6627
	public class AbilityDef : Def
	{
		// Token: 0x170016C5 RID: 5829
		// (get) Token: 0x06009599 RID: 38297 RVA: 0x002BCC34 File Offset: 0x002BAE34
		public float EntropyGain
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_EntropyGain, 0f);
			}
		}

		// Token: 0x170016C6 RID: 5830
		// (get) Token: 0x0600959A RID: 38298 RVA: 0x002BCC4B File Offset: 0x002BAE4B
		public float PsyfocusCost
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_PsyfocusCost, 0f);
			}
		}

		// Token: 0x170016C7 RID: 5831
		// (get) Token: 0x0600959B RID: 38299 RVA: 0x002BCC62 File Offset: 0x002BAE62
		public float EffectRadius
		{
			get
			{
				return this.statBases.GetStatValueFromList(StatDefOf.Ability_EffectRadius, 0f);
			}
		}

		// Token: 0x170016C8 RID: 5832
		// (get) Token: 0x0600959C RID: 38300 RVA: 0x002BCC79 File Offset: 0x002BAE79
		public bool HasAreaOfEffect
		{
			get
			{
				return this.EffectRadius > float.Epsilon;
			}
		}

		// Token: 0x170016C9 RID: 5833
		// (get) Token: 0x0600959D RID: 38301 RVA: 0x002BCC88 File Offset: 0x002BAE88
		public float DetectionChance
		{
			get
			{
				if (this.detectionChanceOverride < 0f)
				{
					return this.GetStatValueAbstract(StatDefOf.Ability_DetectChancePerEntropy, null);
				}
				return this.detectionChanceOverride;
			}
		}

		// Token: 0x170016CA RID: 5834
		// (get) Token: 0x0600959E RID: 38302 RVA: 0x002BCCAA File Offset: 0x002BAEAA
		public bool IsPsycast
		{
			get
			{
				return typeof(Psycast).IsAssignableFrom(this.abilityClass);
			}
		}

		// Token: 0x170016CB RID: 5835
		// (get) Token: 0x0600959F RID: 38303 RVA: 0x002BCCC1 File Offset: 0x002BAEC1
		public string PsyfocusCostPercent
		{
			get
			{
				if (this.psyfocusCostPercent.NullOrEmpty())
				{
					this.psyfocusCostPercent = this.PsyfocusCost.ToStringPercent();
				}
				return this.psyfocusCostPercent;
			}
		}

		// Token: 0x170016CC RID: 5836
		// (get) Token: 0x060095A0 RID: 38304 RVA: 0x002BCCE7 File Offset: 0x002BAEE7
		public string PsyfocusCostPercentMax
		{
			get
			{
				if (this.psyfocusCostPercentMax.NullOrEmpty())
				{
					this.psyfocusCostPercentMax = this.PsyfocusCostRange.max.ToStringPercent();
				}
				return this.psyfocusCostPercentMax;
			}
		}

		// Token: 0x170016CD RID: 5837
		// (get) Token: 0x060095A1 RID: 38305 RVA: 0x002BCD14 File Offset: 0x002BAF14
		public int RequiredPsyfocusBand
		{
			get
			{
				if (this.requiredPsyfocusBandCached == -1)
				{
					this.requiredPsyfocusBandCached = Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand.Count - 1;
					for (int i = 0; i < Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand.Count; i++)
					{
						int num = Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand[i];
						if (this.level <= num)
						{
							this.requiredPsyfocusBandCached = i;
							break;
						}
					}
				}
				return this.requiredPsyfocusBandCached;
			}
		}

		// Token: 0x170016CE RID: 5838
		// (get) Token: 0x060095A2 RID: 38306 RVA: 0x002BCD78 File Offset: 0x002BAF78
		public bool AnyCompOverridesPsyfocusCost
		{
			get
			{
				if (this.anyCompOverridesPsyfocusCost == null)
				{
					this.anyCompOverridesPsyfocusCost = new bool?(false);
					if (this.comps != null)
					{
						using (List<AbilityCompProperties>.Enumerator enumerator = this.comps.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.OverridesPsyfocusCost)
								{
									this.anyCompOverridesPsyfocusCost = new bool?(true);
									break;
								}
							}
						}
					}
				}
				return this.anyCompOverridesPsyfocusCost.Value;
			}
		}

		// Token: 0x170016CF RID: 5839
		// (get) Token: 0x060095A3 RID: 38307 RVA: 0x002BCE04 File Offset: 0x002BB004
		public FloatRange PsyfocusCostRange
		{
			get
			{
				if (this.psyfocusCostRange.min < 0f)
				{
					if (!this.AnyCompOverridesPsyfocusCost)
					{
						this.psyfocusCostRange = new FloatRange(this.PsyfocusCost, this.PsyfocusCost);
					}
					else
					{
						foreach (AbilityCompProperties abilityCompProperties in this.comps)
						{
							if (abilityCompProperties.OverridesPsyfocusCost)
							{
								this.psyfocusCostRange = abilityCompProperties.PsyfocusCostRange;
								break;
							}
						}
					}
				}
				return this.psyfocusCostRange;
			}
		}

		// Token: 0x060095A4 RID: 38308 RVA: 0x002BCEA0 File Offset: 0x002BB0A0
		public IEnumerable<string> StatSummary(Pawn forPawn = null)
		{
			string text = null;
			foreach (AbilityCompProperties abilityCompProperties in this.comps)
			{
				if (abilityCompProperties.OverridesPsyfocusCost)
				{
					text = abilityCompProperties.PsyfocusCostExplanation;
					break;
				}
			}
			if (text == null)
			{
				if (this.PsyfocusCost > 1E-45f)
				{
					yield return "AbilityPsyfocusCost".Translate() + ": " + this.PsyfocusCost.ToStringPercent();
				}
			}
			else
			{
				yield return text;
			}
			if (this.EntropyGain > 1E-45f)
			{
				yield return "AbilityEntropyGain".Translate() + ": " + this.EntropyGain.ToString();
			}
			if (this.verbProperties.warmupTime > 1E-45f)
			{
				yield return "AbilityCastingTime".Translate() + ": " + this.verbProperties.warmupTime.ToString() + "LetterSecond".Translate();
			}
			if (this.cooldownTicksRange.min == this.cooldownTicksRange.max && this.cooldownTicksRange.min > 0)
			{
				yield return "StatsReport_Cooldown".Translate() + ": " + this.cooldownTicksRange.min.ToStringTicksToPeriod(true, false, true, false, false);
			}
			float num = this.EffectDuration(forPawn);
			if (num > 1E-45f)
			{
				int num2 = num.SecondsToTicks();
				yield return "AbilityDuration".Translate() + ": " + ((num2 >= 2500) ? num2.ToStringTicksToPeriod(true, false, true, true, false) : (num.ToString() + "LetterSecond".Translate()));
			}
			if (this.HasAreaOfEffect)
			{
				yield return "AbilityEffectRadius".Translate() + ": " + Mathf.Ceil(this.EffectRadius).ToString();
			}
			if (this.comps != null)
			{
				int num3;
				for (int i = 0; i < this.comps.Count; i = num3 + 1)
				{
					foreach (string text2 in this.comps[i].ExtraStatSummary())
					{
						yield return text2;
					}
					IEnumerator<string> enumerator2 = null;
					num3 = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x060095A5 RID: 38309 RVA: 0x002BCEB7 File Offset: 0x002BB0B7
		public float EffectDuration(Pawn forPawn = null)
		{
			return this.GetStatValueAbstract(StatDefOf.Ability_Duration, forPawn);
		}

		// Token: 0x060095A6 RID: 38310 RVA: 0x002BCEC5 File Offset: 0x002BB0C5
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

		// Token: 0x060095A7 RID: 38311 RVA: 0x002BCEE8 File Offset: 0x002BB0E8
		public string GetTooltip(Pawn pawn = null)
		{
			if (this.cachedTooltip == null || this.cachedTooltipPawn != pawn)
			{
				this.cachedTooltip = this.LabelCap.Colorize(ColoredText.TipSectionTitleColor) + ((this.level > 0) ? ("\n" + "Level".Translate().CapitalizeFirst() + " " + this.level.ToString()) : "") + "\n\n" + this.description;
				this.cachedTooltipPawn = pawn;
				string text = this.StatSummary(pawn).ToLineList(null, false);
				if (!text.NullOrEmpty())
				{
					this.cachedTooltip = this.cachedTooltip + "\n\n" + text;
				}
			}
			if (pawn != null && ModsConfig.RoyaltyActive && this.IsPsycast && this.level > 0)
			{
				Faction first = Faction.GetMinTitleForImplantAllFactions(HediffDefOf.PsychicAmplifier).First;
				if (first != null)
				{
					RoyalTitleDef minTitleForImplant = first.GetMinTitleForImplant(HediffDefOf.PsychicAmplifier, this.level);
					RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(first);
					if (minTitleForImplant != null && (currentTitle == null || currentTitle.seniority < minTitleForImplant.seniority) && this.DetectionChance > 0f)
					{
						return this.cachedTooltip + "\n\n" + "PsycastIsIllegal".Translate(pawn.Named("PAWN"), minTitleForImplant.GetLabelCapFor(pawn).Named("TITLE")).Colorize(ColoredText.WarningColor);
					}
				}
			}
			return this.cachedTooltip;
		}

		// Token: 0x060095A8 RID: 38312 RVA: 0x002BD082 File Offset: 0x002BB282
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			if (this.cachedTargets == null)
			{
				this.cachedTargets = new List<string>();
				if (this.verbProperties.targetParams.canTargetPawns && this.verbProperties.targetParams.canTargetSelf)
				{
					this.cachedTargets.Add("TargetSelf".Translate());
				}
				if (this.verbProperties.targetParams.canTargetLocations)
				{
					this.cachedTargets.Add("TargetGround".Translate());
				}
				if (this.verbProperties.targetParams.canTargetPawns && this.verbProperties.targetParams.canTargetHumans)
				{
					this.cachedTargets.Add("TargetHuman".Translate());
				}
				if (this.verbProperties.targetParams.canTargetPawns && this.verbProperties.targetParams.canTargetAnimals)
				{
					this.cachedTargets.Add("TargetAnimal".Translate());
				}
			}
			int num = this.comps.OfType<CompProperties_AbilityEffect>().Sum((CompProperties_AbilityEffect e) => e.goodwillImpact);
			if (num != 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Ability, StatDefOf.Ability_GoodwillImpact, (float)num, req, ToStringNumberSense.Undefined, null, false);
			}
			if (this.IsPsycast && this.level != 0)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Ability, StatDefOf.Ability_RequiredPsylink, (float)this.level, req, ToStringNumberSense.Undefined, null, false);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Ability, StatDefOf.Ability_CastingTime, this.verbProperties.warmupTime, req, ToStringNumberSense.Undefined, null, false);
			if (this.verbProperties.range > 0f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Ability, StatDefOf.Ability_Range, this.verbProperties.range, req, ToStringNumberSense.Undefined, null, false);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Ability, "Target".Translate(), this.cachedTargets.ToCommaList(false, false).CapitalizeFirst(), "AbilityTargetDesc".Translate(), 1001, null, null, false, false);
			yield return new StatDrawEntry(StatCategoryDefOf.Ability, "AbilityRequiresLOS".Translate(), this.verbProperties.requireLineOfSight ? "Yes".Translate() : "No".Translate(), "", 1000, null, null, false, false);
			yield break;
		}

		// Token: 0x170016D0 RID: 5840
		// (get) Token: 0x060095A9 RID: 38313 RVA: 0x002BD099 File Offset: 0x002BB299
		public Texture2D WarmupMoteSocialSymbol
		{
			get
			{
				if (!this.warmupMoteSocialSymbol.NullOrEmpty() && this.warmupMoteSocialSymbolCached == null)
				{
					this.warmupMoteSocialSymbolCached = ContentFinder<Texture2D>.Get(this.warmupMoteSocialSymbol, true);
				}
				return this.warmupMoteSocialSymbolCached;
			}
		}

		// Token: 0x060095AA RID: 38314 RVA: 0x002BD0CE File Offset: 0x002BB2CE
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.abilityClass == null)
			{
				yield return "abilityClass is null";
			}
			if (this.verbProperties == null)
			{
				yield return "verbProperties are null";
			}
			if (this.label.NullOrEmpty())
			{
				yield return "no label";
			}
			if (this.statBases != null)
			{
				using (List<StatModifier>.Enumerator enumerator2 = this.statBases.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						StatModifier statBase = enumerator2.Current;
						if (this.statBases.Count((StatModifier st) => st.stat == statBase.stat) > 1)
						{
							string str = "defines the stat base ";
							StatDef stat = statBase.stat;
							yield return str + ((stat != null) ? stat.ToString() : null) + " more than once.";
						}
					}
				}
				List<StatModifier>.Enumerator enumerator2 = default(List<StatModifier>.Enumerator);
			}
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				foreach (string text2 in this.comps[i].ConfigErrors(this))
				{
					yield return text2;
				}
				enumerator = null;
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x04006179 RID: 24953
		public Type abilityClass = typeof(Ability);

		// Token: 0x0400617A RID: 24954
		public Type gizmoClass = typeof(Command_Ability);

		// Token: 0x0400617B RID: 24955
		public List<AbilityCompProperties> comps = new List<AbilityCompProperties>();

		// Token: 0x0400617C RID: 24956
		public AbilityCategoryDef category;

		// Token: 0x0400617D RID: 24957
		public int displayOrder;

		// Token: 0x0400617E RID: 24958
		public List<StatModifier> statBases;

		// Token: 0x0400617F RID: 24959
		public VerbProperties verbProperties;

		// Token: 0x04006180 RID: 24960
		public KeyBindingDef hotKey;

		// Token: 0x04006181 RID: 24961
		public JobDef jobDef;

		// Token: 0x04006182 RID: 24962
		public ThingDef warmupMote;

		// Token: 0x04006183 RID: 24963
		public EffecterDef warmupEffecter;

		// Token: 0x04006184 RID: 24964
		public FleckDef emittedFleck;

		// Token: 0x04006185 RID: 24965
		public int emissionInterval;

		// Token: 0x04006186 RID: 24966
		public string warmupMoteSocialSymbol;

		// Token: 0x04006187 RID: 24967
		public SoundDef warmupStartSound;

		// Token: 0x04006188 RID: 24968
		public SoundDef warmupSound;

		// Token: 0x04006189 RID: 24969
		public SoundDef warmupPreEndSound;

		// Token: 0x0400618A RID: 24970
		public float warmupPreEndSoundSeconds;

		// Token: 0x0400618B RID: 24971
		public Vector3 moteDrawOffset;

		// Token: 0x0400618C RID: 24972
		public float moteOffsetAmountTowardsTarget;

		// Token: 0x0400618D RID: 24973
		public bool canUseAoeToGetTargets = true;

		// Token: 0x0400618E RID: 24974
		public bool useAverageTargetPositionForWarmupEffecter;

		// Token: 0x0400618F RID: 24975
		public bool targetRequired = true;

		// Token: 0x04006190 RID: 24976
		public bool targetWorldCell;

		// Token: 0x04006191 RID: 24977
		public bool showGizmoOnWorldView;

		// Token: 0x04006192 RID: 24978
		public bool aiCanUse;

		// Token: 0x04006193 RID: 24979
		public bool ai_SearchAOEForTargets;

		// Token: 0x04006194 RID: 24980
		public bool ai_IsOffensive = true;

		// Token: 0x04006195 RID: 24981
		public bool ai_IsIncendiary = true;

		// Token: 0x04006196 RID: 24982
		public bool groupAbility;

		// Token: 0x04006197 RID: 24983
		public int level;

		// Token: 0x04006198 RID: 24984
		public IntRange cooldownTicksRange;

		// Token: 0x04006199 RID: 24985
		public bool cooldownPerCharge;

		// Token: 0x0400619A RID: 24986
		public bool hasExternallyHandledCooldown;

		// Token: 0x0400619B RID: 24987
		public int charges = -1;

		// Token: 0x0400619C RID: 24988
		public AbilityGroupDef groupDef;

		// Token: 0x0400619D RID: 24989
		public bool overrideGroupCooldown;

		// Token: 0x0400619E RID: 24990
		public List<MemeDef> requiredMemes;

		// Token: 0x0400619F RID: 24991
		public bool sendLetterOnCooldownComplete;

		// Token: 0x040061A0 RID: 24992
		public bool sendMessageOnCooldownComplete;

		// Token: 0x040061A1 RID: 24993
		public bool displayGizmoWhileUndrafted;

		// Token: 0x040061A2 RID: 24994
		public bool disableGizmoWhileUndrafted = true;

		// Token: 0x040061A3 RID: 24995
		public bool writeCombatLog;

		// Token: 0x040061A4 RID: 24996
		public bool stunTargetWhileCasting;

		// Token: 0x040061A5 RID: 24997
		public bool showPsycastEffects = true;

		// Token: 0x040061A6 RID: 24998
		public bool showCastingProgressBar;

		// Token: 0x040061A7 RID: 24999
		public float detectionChanceOverride = -1f;

		// Token: 0x040061A8 RID: 25000
		public float uiOrder;

		// Token: 0x040061A9 RID: 25001
		public bool waitForJobEnd;

		// Token: 0x040061AA RID: 25002
		public bool showWhenDrafted = true;

		// Token: 0x040061AB RID: 25003
		public bool showOnCharacterCard = true;

		// Token: 0x040061AC RID: 25004
		public bool hostile = true;

		// Token: 0x040061AD RID: 25005
		public bool casterMustBeCapableOfViolence = true;

		// Token: 0x040061AE RID: 25006
		[MustTranslate]
		public string confirmationDialogText;

		// Token: 0x040061AF RID: 25007
		[NoTranslate]
		public string iconPath;

		// Token: 0x040061B0 RID: 25008
		public Texture2D uiIcon = BaseContent.BadTex;

		// Token: 0x040061B1 RID: 25009
		private string cachedTooltip;

		// Token: 0x040061B2 RID: 25010
		private Pawn cachedTooltipPawn;

		// Token: 0x040061B3 RID: 25011
		private List<string> cachedTargets;

		// Token: 0x040061B4 RID: 25012
		private int requiredPsyfocusBandCached = -1;

		// Token: 0x040061B5 RID: 25013
		private bool? anyCompOverridesPsyfocusCost;

		// Token: 0x040061B6 RID: 25014
		private FloatRange psyfocusCostRange = new FloatRange(-1f, -1f);

		// Token: 0x040061B7 RID: 25015
		private string psyfocusCostPercent;

		// Token: 0x040061B8 RID: 25016
		private string psyfocusCostPercentMax;

		// Token: 0x040061B9 RID: 25017
		private Texture2D warmupMoteSocialSymbolCached;
	}
}
