using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200254B RID: 9547
	public class Trait : IExposable
	{
		// Token: 0x17002035 RID: 8245
		// (get) Token: 0x0600D0F1 RID: 53489 RVA: 0x003D6A54 File Offset: 0x003D4C54
		public int Degree
		{
			get
			{
				return this.degree;
			}
		}

		// Token: 0x17002036 RID: 8246
		// (get) Token: 0x0600D0F2 RID: 53490 RVA: 0x003D6A5C File Offset: 0x003D4C5C
		public TraitDegreeData CurrentData
		{
			get
			{
				return this.def.DataAtDegree(this.degree);
			}
		}

		// Token: 0x17002037 RID: 8247
		// (get) Token: 0x0600D0F3 RID: 53491 RVA: 0x003D6A6F File Offset: 0x003D4C6F
		public string Label
		{
			get
			{
				return this.CurrentData.GetLabelFor(this.pawn);
			}
		}

		// Token: 0x17002038 RID: 8248
		// (get) Token: 0x0600D0F4 RID: 53492 RVA: 0x003D6A82 File Offset: 0x003D4C82
		public string LabelCap
		{
			get
			{
				return this.CurrentData.GetLabelCapFor(this.pawn);
			}
		}

		// Token: 0x17002039 RID: 8249
		// (get) Token: 0x0600D0F5 RID: 53493 RVA: 0x003D6A95 File Offset: 0x003D4C95
		public bool ScenForced
		{
			get
			{
				return this.scenForced;
			}
		}

		// Token: 0x1700203A RID: 8250
		// (get) Token: 0x0600D0F6 RID: 53494 RVA: 0x003D6AA0 File Offset: 0x003D4CA0
		public bool Suppressed
		{
			get
			{
				if (ModsConfig.BiotechActive)
				{
					if (this.suppressedByTrait || this.suppressedByGene != null)
					{
						return true;
					}
					if (this.sourceGene != null && this.sourceGene.Overridden && this.sourceGene.overriddenByGene.def != this.sourceGene.def)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600D0F7 RID: 53495 RVA: 0x003D6AFB File Offset: 0x003D4CFB
		public Trait()
		{
		}

		// Token: 0x0600D0F8 RID: 53496 RVA: 0x003D6B0E File Offset: 0x003D4D0E
		public Trait(TraitDef def, int degree = 0, bool forced = false)
		{
			this.def = def;
			this.degree = degree;
			this.scenForced = forced;
		}

		// Token: 0x0600D0F9 RID: 53497 RVA: 0x003D6B38 File Offset: 0x003D4D38
		public void ExposeData()
		{
			Scribe_Defs.Look<TraitDef>(ref this.def, "def");
			Scribe_References.Look<Gene>(ref this.sourceGene, "sourceGene", false);
			Scribe_Values.Look<int>(ref this.degree, "degree", 0, false);
			Scribe_Values.Look<bool>(ref this.scenForced, "scenForced", false, false);
			Scribe_Values.Look<bool>(ref this.suppressedByTrait, "suppressedByTrait", false, false);
			Scribe_References.Look<Gene>(ref this.suppressedByGene, "suppressedBy", false);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && this.def == null)
			{
				this.def = DefDatabase<TraitDef>.GetRandom();
				this.degree = PawnGenerator.RandomTraitDegree(this.def);
			}
		}

		// Token: 0x0600D0FA RID: 53498 RVA: 0x003D6BDC File Offset: 0x003D4DDC
		public float OffsetOfStat(StatDef stat)
		{
			if (this.Suppressed)
			{
				return 0f;
			}
			float num = 0f;
			TraitDegreeData currentData = this.CurrentData;
			if (currentData.statOffsets != null)
			{
				for (int i = 0; i < currentData.statOffsets.Count; i++)
				{
					if (currentData.statOffsets[i].stat == stat)
					{
						num += currentData.statOffsets[i].value;
					}
				}
			}
			return num;
		}

		// Token: 0x0600D0FB RID: 53499 RVA: 0x003D6C4C File Offset: 0x003D4E4C
		public float MultiplierOfStat(StatDef stat)
		{
			if (this.Suppressed)
			{
				return 1f;
			}
			float num = 1f;
			TraitDegreeData currentData = this.CurrentData;
			if (currentData.statFactors != null)
			{
				for (int i = 0; i < currentData.statFactors.Count; i++)
				{
					if (currentData.statFactors[i].stat == stat)
					{
						num *= currentData.statFactors[i].value;
					}
				}
			}
			return num;
		}

		// Token: 0x0600D0FC RID: 53500 RVA: 0x003D6CBC File Offset: 0x003D4EBC
		public string TipString(Pawn pawn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.suppressedReasons.Clear();
			TraitDegreeData currentData = this.CurrentData;
			stringBuilder.AppendLine(currentData.description.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn, "PAWN", true).Resolve());
			bool flag = this.CurrentData.skillGains.Count > 0;
			bool flag2 = this.GetPermaThoughts().Any<ThoughtDef>();
			bool flag3 = currentData.statOffsets != null;
			bool flag4 = currentData.statFactors != null;
			if (flag || flag2 || flag3 || flag4 || !Mathf.Approximately(currentData.painFactor, 1f) || currentData.painOffset != 0f || !currentData.aptitudes.NullOrEmpty<Aptitude>())
			{
				stringBuilder.AppendLine();
			}
			if (flag)
			{
				foreach (SkillGain skillGain in this.CurrentData.skillGains)
				{
					if (skillGain.amount != 0)
					{
						stringBuilder.AppendLine("    " + skillGain.skill.skillLabel.CapitalizeFirst() + ":   " + skillGain.amount.ToString("+##;-##"));
					}
				}
			}
			if (flag2)
			{
				foreach (ThoughtDef thoughtDef in this.GetPermaThoughts())
				{
					stringBuilder.AppendLine("    " + "PermanentMoodEffect".Translate() + " " + thoughtDef.stages[0].baseMoodEffect.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Offset));
				}
			}
			if (flag3)
			{
				for (int i = 0; i < currentData.statOffsets.Count; i++)
				{
					StatModifier statModifier = currentData.statOffsets[i];
					string valueToStringAsOffset = statModifier.ValueToStringAsOffset;
					string value = "    " + statModifier.stat.LabelCap + " " + valueToStringAsOffset;
					stringBuilder.AppendLine(value);
				}
			}
			if (currentData.painOffset != 0f)
			{
				stringBuilder.AppendLine("    " + "Pain".Translate() + " " + currentData.painOffset.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Offset));
			}
			if (flag4)
			{
				for (int j = 0; j < currentData.statFactors.Count; j++)
				{
					StatModifier statModifier2 = currentData.statFactors[j];
					string toStringAsFactor = statModifier2.ToStringAsFactor;
					string value2 = "    " + statModifier2.stat.LabelCap + " " + toStringAsFactor;
					stringBuilder.AppendLine(value2);
				}
			}
			if (!Mathf.Approximately(currentData.hungerRateFactor, 1f))
			{
				string t = currentData.hungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor);
				string value3 = "    " + "HungerRate".Translate() + " " + t;
				stringBuilder.AppendLine(value3);
			}
			if (!Mathf.Approximately(currentData.painFactor, 1f))
			{
				stringBuilder.AppendLine("    " + "Pain".Translate() + " " + currentData.painFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
			}
			if (!currentData.aptitudes.NullOrEmpty<Aptitude>())
			{
				stringBuilder.AppendLine().AppendLineTagged(("Aptitudes".Translate().CapitalizeFirst() + ":").AsTipTitle());
				stringBuilder.AppendLine((from x in currentData.aptitudes
				select x.skill.LabelCap.ToString() + " " + x.level.ToStringWithSign()).ToLineList("  - ", true));
			}
			if (!currentData.enablesNeeds.NullOrEmpty<NeedDef>())
			{
				stringBuilder.AppendLine().AppendLineTagged(("AddsNeeds".Translate().CapitalizeFirst() + ":").AsTipTitle());
				stringBuilder.AppendLine((from x in currentData.enablesNeeds
				select x.LabelCap.ToString()).ToLineList("  - ", true));
			}
			if (!currentData.disablesNeeds.NullOrEmpty<NeedDef>())
			{
				stringBuilder.AppendLine().AppendLineTagged(("DisablesNeeds".Translate().CapitalizeFirst() + ":").AsTipTitle());
				stringBuilder.AppendLine((from x in currentData.disablesNeeds
				select x.LabelCap.ToString()).ToLineList("  - ", true));
			}
			if (ModsConfig.RoyaltyActive)
			{
				List<MeditationFocusDef> allowedMeditationFocusTypes = this.CurrentData.allowedMeditationFocusTypes;
				if (!allowedMeditationFocusTypes.NullOrEmpty<MeditationFocusDef>())
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("EnablesMeditationFocusType".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + (from f in allowedMeditationFocusTypes
					select f.LabelCap.Resolve()).ToLineList("  - ", false));
				}
			}
			if (ModsConfig.IdeologyActive)
			{
				List<IssueDef> affectedIssues = this.CurrentData.GetAffectedIssues(this.def);
				if (affectedIssues.Count != 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("OverridesSomePrecepts".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + (from x in affectedIssues
					select x.LabelCap.Resolve()).ToLineList("  - ", false));
				}
				List<MemeDef> affectedMemes = this.CurrentData.GetAffectedMemes(this.def, true);
				if (affectedMemes.Count > 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("AgreeableMemes".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + (from x in affectedMemes
					select x.LabelCap.Resolve()).ToLineList("  - ", false));
				}
				List<MemeDef> affectedMemes2 = this.CurrentData.GetAffectedMemes(this.def, false);
				if (affectedMemes2.Count > 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("DisagreeableMemes".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + (from x in affectedMemes2
					select x.LabelCap.Resolve()).ToLineList("  - ", false));
				}
			}
			if (ModsConfig.BiotechActive)
			{
				if (this.sourceGene != null)
				{
					stringBuilder.AppendLine().AppendLine(("AddedByGene".Translate() + ": " + this.sourceGene.LabelCap).Colorize(ColoredText.GeneColor));
				}
				if (this.Suppressed)
				{
					foreach (Trait trait in pawn.story.traits.allTraits)
					{
						if (trait != this && trait.def.CanSuppress(this))
						{
							this.suppressedReasons.Add(string.Format("{0} ({1})", trait.Label, "Trait".Translate()));
						}
					}
					if (this.sourceGene != null && this.sourceGene.Overridden && this.sourceGene.overriddenByGene.def != this.sourceGene.def)
					{
						this.suppressedReasons.Add(string.Format("{0} ({1})", this.sourceGene.overriddenByGene.Label, "Gene".Translate()));
					}
					if (this.suppressedByGene != null)
					{
						this.suppressedReasons.Add(string.Format("{0} ({1})", this.suppressedByGene.Label, "Gene".Translate()));
					}
					if (this.suppressedReasons.Any<string>())
					{
						stringBuilder.AppendLine().AppendLine(("Suppressed".Translate().CapitalizeFirst() + ": " + "SuppressedDesc".Translate() + ":\n" + this.suppressedReasons.ToLineList("  - ", true)).Colorize(ColoredText.SubtleGrayColor));
					}
				}
			}
			if (stringBuilder.Length > 0)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				if (stringBuilder2[stringBuilder2.Length - 1] == '\n')
				{
					if (stringBuilder.Length > 1)
					{
						StringBuilder stringBuilder3 = stringBuilder;
						if (stringBuilder3[stringBuilder3.Length - 2] == '\r')
						{
							stringBuilder.Remove(stringBuilder.Length - 2, 2);
							goto IL_919;
						}
					}
					stringBuilder.Remove(stringBuilder.Length - 1, 1);
				}
			}
			IL_919:
			return stringBuilder.ToString();
		}

		// Token: 0x0600D0FD RID: 53501 RVA: 0x003D7610 File Offset: 0x003D5810
		public override string ToString()
		{
			if (this.def == null)
			{
				return "Trait(null)";
			}
			return string.Concat(new string[]
			{
				"Trait(",
				this.def.ToString(),
				"-",
				this.degree.ToString(),
				")"
			});
		}

		// Token: 0x0600D0FE RID: 53502 RVA: 0x003D766A File Offset: 0x003D586A
		private IEnumerable<ThoughtDef> GetPermaThoughts()
		{
			TraitDegreeData degree = this.CurrentData;
			List<ThoughtDef> allThoughts = DefDatabase<ThoughtDef>.AllDefsListForReading;
			int num;
			for (int i = 0; i < allThoughts.Count; i = num + 1)
			{
				if (allThoughts[i].IsSituational && allThoughts[i].Worker is ThoughtWorker_AlwaysActive && allThoughts[i].requiredTraits != null && allThoughts[i].requiredTraits.Contains(this.def) && (!allThoughts[i].RequiresSpecificTraitsDegree || allThoughts[i].requiredTraitsDegree == degree.degree))
				{
					yield return allThoughts[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600D0FF RID: 53503 RVA: 0x003D767A File Offset: 0x003D587A
		private bool AllowsWorkType(WorkTypeDef workDef)
		{
			return (this.def.disabledWorkTags & workDef.workTags) == WorkTags.None;
		}

		// Token: 0x0600D100 RID: 53504 RVA: 0x003D7691 File Offset: 0x003D5891
		public void Notify_MentalStateEndedOn(Pawn pawn, bool causedByMood)
		{
			if (causedByMood)
			{
				this.Notify_MentalStateEndedOn(pawn);
			}
		}

		// Token: 0x0600D101 RID: 53505 RVA: 0x003D76A0 File Offset: 0x003D58A0
		public void Notify_MentalStateEndedOn(Pawn pawn)
		{
			TraitDegreeData currentData = this.CurrentData;
			if (currentData.mentalBreakInspirationGainSet.NullOrEmpty<InspirationDef>() || Rand.Value > currentData.mentalBreakInspirationGainChance)
			{
				return;
			}
			pawn.mindState.inspirationHandler.TryStartInspiration(currentData.mentalBreakInspirationGainSet.RandomElement<InspirationDef>(), currentData.mentalBreakInspirationGainReasonText, true);
		}

		// Token: 0x0600D102 RID: 53506 RVA: 0x003D76F4 File Offset: 0x003D58F4
		public List<WorkTypeDef> GetDisabledWorkTypes()
		{
			Trait.tmpDisabledWorktypes.Clear();
			for (int i = 0; i < this.def.disabledWorkTypes.Count; i++)
			{
				Trait.tmpDisabledWorktypes.Add(this.def.disabledWorkTypes[i]);
			}
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int j = 0; j < allDefsListForReading.Count; j++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[j];
				if (!this.AllowsWorkType(workTypeDef))
				{
					Trait.tmpDisabledWorktypes.Add(workTypeDef);
				}
			}
			return Trait.tmpDisabledWorktypes;
		}

		// Token: 0x04008DEE RID: 36334
		public Pawn pawn;

		// Token: 0x04008DEF RID: 36335
		public Gene sourceGene;

		// Token: 0x04008DF0 RID: 36336
		public Gene suppressedByGene;

		// Token: 0x04008DF1 RID: 36337
		public TraitDef def;

		// Token: 0x04008DF2 RID: 36338
		private int degree;

		// Token: 0x04008DF3 RID: 36339
		private bool scenForced;

		// Token: 0x04008DF4 RID: 36340
		[LoadAlias("suppressed")]
		public bool suppressedByTrait;

		// Token: 0x04008DF5 RID: 36341
		private List<string> suppressedReasons = new List<string>();

		// Token: 0x04008DF6 RID: 36342
		private static List<WorkTypeDef> tmpDisabledWorktypes = new List<WorkTypeDef>();
	}
}
