using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200192E RID: 6446
	public class ReadingOutcomeDoerGainResearch : BookOutcomeDoer
	{
		// Token: 0x17001633 RID: 5683
		// (get) Token: 0x0600928E RID: 37518 RVA: 0x002B2926 File Offset: 0x002B0B26
		public new BookOutcomeProperties_GainResearch Props
		{
			get
			{
				return (BookOutcomeProperties_GainResearch)this.props;
			}
		}

		// Token: 0x17001634 RID: 5684
		// (get) Token: 0x0600928F RID: 37519 RVA: 0x002B2933 File Offset: 0x002B0B33
		public virtual int RoundTo { get; } = 10;

		// Token: 0x06009290 RID: 37520 RVA: 0x002B293C File Offset: 0x002B0B3C
		private bool IsValid(ResearchProjectDef project)
		{
			return project.generalRules != null && (this.Props.exclude.Count <= 0 || !this.Props.exclude.ContainsAny((BookOutcomeProperties_GainResearch.BookResearchItem i) => i.project == project)) && ((!this.Props.ignoreZeroBaseCost || project.baseCost != 0f) && (this.Props.tabs.NullOrEmpty<BookOutcomeProperties_GainResearch.BookTabItem>() || this.Props.tabs.Exists((BookOutcomeProperties_GainResearch.BookTabItem x) => x.tab == project.tab))) && project.TechprintCount == 0;
		}

		// Token: 0x06009291 RID: 37521 RVA: 0x002B29F8 File Offset: 0x002B0BF8
		public override bool BenefitDetailsCanChange(Pawn reader = null)
		{
			foreach (KeyValuePair<ResearchProjectDef, float> keyValuePair in this.values)
			{
				ResearchProjectDef researchProjectDef;
				float num;
				keyValuePair.Deconstruct(out researchProjectDef, out num);
				ResearchProjectDef researchProjectDef2 = researchProjectDef;
				if (researchProjectDef2.IsHidden)
				{
					return true;
				}
				if (!researchProjectDef2.IsFinished)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009292 RID: 37522 RVA: 0x002B2A70 File Offset: 0x002B0C70
		public override void OnBookGenerated(Pawn author = null)
		{
			List<ResearchProjectDef> allDefsListForReading = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
			List<ResearchProjectDef> list = new List<ResearchProjectDef>(allDefsListForReading.Count);
			if (this.Props.include.Count > 0)
			{
				for (int i = 0; i < this.Props.include.Count; i++)
				{
					ResearchProjectDef project = this.Props.include[i].project;
					if (!project.IsFinished && this.IsValid(project))
					{
						list.Add(project);
					}
				}
			}
			else
			{
				for (int j = 0; j < allDefsListForReading.Count; j++)
				{
					if (allDefsListForReading[j].PrerequisitesCompleted && !allDefsListForReading[j].IsFinished && this.IsValid(allDefsListForReading[j]))
					{
						list.Add(allDefsListForReading[j]);
					}
				}
			}
			if (list.Count == 0)
			{
				for (int k = 0; k < allDefsListForReading.Count; k++)
				{
					if (this.IsValid(allDefsListForReading[k]))
					{
						list.Add(allDefsListForReading[k]);
					}
				}
			}
			if (list.Empty<ResearchProjectDef>())
			{
				string[] array = new string[6];
				array[0] = "Research book had no valid projects and failed to find any backup projects.\n";
				array[1] = string.Format("ignoring zero base cost: {0}).\n", this.Props.ignoreZeroBaseCost);
				array[2] = "tabs: ";
				array[3] = (from x in this.Props.tabs
				select x.tab.generalTitle).ToCommaList(false, false);
				array[4] = ".\nexcludes: ";
				int num = 5;
				string text;
				if (!this.Props.exclude.Empty<BookOutcomeProperties_GainResearch.BookResearchItem>())
				{
					text = (from x in this.Props.exclude
					select x.project.label).ToCommaList(false, false);
				}
				else
				{
					text = "(none)";
				}
				array[num] = text;
				Log.ErrorOnce(string.Concat(array), 15242436);
				return;
			}
			int count = Rand.Chance(0.25f) ? 2 : 1;
			List<ResearchProjectDef> list2 = list.TakeRandomDistinct(count);
			float num2 = this.GetBaseValue();
			if (list2.Count > 1)
			{
				num2 *= 1.25f;
			}
			float value = num2 / (float)list2.Count;
			for (int l = 0; l < list2.Count; l++)
			{
				this.values[list2[l]] = value;
			}
		}

		// Token: 0x06009293 RID: 37523 RVA: 0x002B2CCE File Offset: 0x002B0ECE
		public override void Reset()
		{
			this.values.Clear();
		}

		// Token: 0x06009294 RID: 37524 RVA: 0x002B2CDC File Offset: 0x002B0EDC
		public override bool DoesProvidesOutcome(Pawn reader)
		{
			foreach (KeyValuePair<ResearchProjectDef, float> keyValuePair in this.values)
			{
				ResearchProjectDef researchProjectDef;
				float num;
				keyValuePair.Deconstruct(out researchProjectDef, out num);
				if (!researchProjectDef.IsFinished)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009295 RID: 37525 RVA: 0x002B2D44 File Offset: 0x002B0F44
		protected virtual float GetBaseValue()
		{
			return BookUtility.GetResearchExpForQuality(base.Quality);
		}

		// Token: 0x06009296 RID: 37526 RVA: 0x002B2D54 File Offset: 0x002B0F54
		public override void OnReadingTick(Pawn reader, float factor)
		{
			foreach (KeyValuePair<ResearchProjectDef, float> keyValuePair in this.values)
			{
				ResearchProjectDef researchProjectDef;
				float num;
				keyValuePair.Deconstruct(out researchProjectDef, out num);
				ResearchProjectDef researchProjectDef2 = researchProjectDef;
				float num2 = num;
				if (this.IsProjectVisible(researchProjectDef2) && !researchProjectDef2.IsFinished)
				{
					Find.ResearchManager.AddProgress(researchProjectDef2, num2 * factor, null);
				}
			}
		}

		// Token: 0x06009297 RID: 37527 RVA: 0x002B2DD4 File Offset: 0x002B0FD4
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetHyperlinks()
		{
			foreach (KeyValuePair<ResearchProjectDef, float> keyValuePair in this.values)
			{
				ResearchProjectDef researchProjectDef;
				float num;
				keyValuePair.Deconstruct(out researchProjectDef, out num);
				ResearchProjectDef researchProjectDef2 = researchProjectDef;
				if (this.IsProjectVisible(researchProjectDef2))
				{
					yield return new Dialog_InfoCard.Hyperlink(researchProjectDef2);
				}
			}
			Dictionary<ResearchProjectDef, float>.Enumerator enumerator = default(Dictionary<ResearchProjectDef, float>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06009298 RID: 37528 RVA: 0x002B2DE4 File Offset: 0x002B0FE4
		public override string GetBenefitsString(Pawn reader = null)
		{
			if (this.values.Count == 0)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<ResearchProjectDef, float> keyValuePair in this.values)
			{
				ResearchProjectDef researchProjectDef;
				float num;
				keyValuePair.Deconstruct(out researchProjectDef, out num);
				ResearchProjectDef researchProjectDef2 = researchProjectDef;
				float num2 = num * 2500f;
				if (this.RoundTo != 0)
				{
					num2 = Mathf.Round(num2 / (float)this.RoundTo) * (float)this.RoundTo;
				}
				string text = string.Format("{0}: {1}", researchProjectDef2.LabelCap, "PerHour".Translate(num2.ToStringDecimalIfSmall()));
				if (researchProjectDef2.IsFinished)
				{
					text += string.Format(" ({0})", "AlreadyUnlocked".Translate());
					text = text.Colorize(Color.gray);
				}
				else if (!this.IsProjectVisible(researchProjectDef2))
				{
					text += string.Format(" ({0})", "WhenDiscovered".Translate());
					text = text.Colorize(Color.gray);
				}
				stringBuilder.AppendLine(" - " + text);
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06009299 RID: 37529 RVA: 0x002B2F54 File Offset: 0x002B1154
		private bool IsProjectVisible(ResearchProjectDef project)
		{
			return !this.Props.usesHiddenProjects || project.CanStartNow;
		}

		// Token: 0x0600929A RID: 37530 RVA: 0x002B2F6B File Offset: 0x002B116B
		public override IEnumerable<RulePack> GetTopicRulePacks()
		{
			return from x in this.values.Keys
			select x.generalRules;
		}

		// Token: 0x0600929B RID: 37531 RVA: 0x002B2F9C File Offset: 0x002B119C
		public override void PostExposeData()
		{
			Scribe_Collections.Look<ResearchProjectDef, float>(ref this.values, "values", LookMode.Undefined, LookMode.Undefined);
		}

		// Token: 0x04005D8B RID: 23947
		protected Dictionary<ResearchProjectDef, float> values = new Dictionary<ResearchProjectDef, float>();

		// Token: 0x04005D8C RID: 23948
		private const float MultipleProjectFactor = 1.25f;
	}
}
