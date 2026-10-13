using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001934 RID: 6452
	public class BookOutcomeDoerGainSkillExp : BookOutcomeDoer
	{
		// Token: 0x17001638 RID: 5688
		// (get) Token: 0x060092B2 RID: 37554 RVA: 0x002B31EC File Offset: 0x002B13EC
		public new BookOutcomeProperties_GainSkillExp Props
		{
			get
			{
				return (BookOutcomeProperties_GainSkillExp)this.props;
			}
		}

		// Token: 0x17001639 RID: 5689
		// (get) Token: 0x060092B3 RID: 37555 RVA: 0x002B31F9 File Offset: 0x002B13F9
		public IReadOnlyDictionary<SkillDef, float> Values
		{
			get
			{
				return this.values;
			}
		}

		// Token: 0x060092B4 RID: 37556 RVA: 0x002B3204 File Offset: 0x002B1404
		public override bool DoesProvidesOutcome(Pawn reader)
		{
			foreach (KeyValuePair<SkillDef, float> keyValuePair in this.values)
			{
				SkillDef skillDef;
				float num;
				keyValuePair.Deconstruct(out skillDef, out num);
				SkillDef skill = skillDef;
				if (BookOutcomeDoerGainSkillExp.CanProgressSkill(reader, skill, base.Quality))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060092B5 RID: 37557 RVA: 0x002B3278 File Offset: 0x002B1478
		public override void OnBookGenerated(Pawn author = null)
		{
			List<SkillDef> availableSkills = this.GetAvailableSkills(null);
			int count = Rand.Chance(0.25f) ? 2 : 1;
			List<SkillDef> list = availableSkills.TakeRandomDistinct(count);
			float num = BookUtility.GetSkillExpForQuality(base.Quality);
			if (list.Count > 1)
			{
				num *= 1.25f;
			}
			float value = num / (float)list.Count;
			for (int i = 0; i < list.Count; i++)
			{
				this.values[list[i]] = value;
			}
		}

		// Token: 0x060092B6 RID: 37558 RVA: 0x002B32F4 File Offset: 0x002B14F4
		protected virtual List<SkillDef> GetAvailableSkills(Pawn author = null)
		{
			List<SkillDef> list = new List<SkillDef>();
			if (this.Props.skills.Count > 0)
			{
				using (List<BookOutcomeProperties_GainSkillExp.BookStatReward>.Enumerator enumerator = this.Props.skills.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BookOutcomeProperties_GainSkillExp.BookStatReward bookStatReward = enumerator.Current;
						list.Add(bookStatReward.skill);
					}
					return list;
				}
			}
			list = DefDatabase<SkillDef>.AllDefsListForReading;
			return list;
		}

		// Token: 0x060092B7 RID: 37559 RVA: 0x002B3374 File Offset: 0x002B1574
		public override void Reset()
		{
			this.values.Clear();
		}

		// Token: 0x060092B8 RID: 37560 RVA: 0x002B3384 File Offset: 0x002B1584
		public override void OnReadingTick(Pawn reader, float factor)
		{
			foreach (KeyValuePair<SkillDef, float> keyValuePair in this.values)
			{
				SkillDef skillDef;
				float num;
				keyValuePair.Deconstruct(out skillDef, out num);
				SkillDef skillDef2 = skillDef;
				float num2 = num;
				if (BookOutcomeDoerGainSkillExp.CanProgressSkill(reader, skillDef2, base.Quality))
				{
					reader.skills.GetSkill(skillDef2).Learn(num2 * factor, false, false);
				}
			}
		}

		// Token: 0x060092B9 RID: 37561 RVA: 0x002B3408 File Offset: 0x002B1608
		public override string GetBenefitsString(Pawn reader = null)
		{
			if (this.values.Count == 0)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<SkillDef, float> keyValuePair in this.values)
			{
				SkillDef skillDef;
				float num;
				keyValuePair.Deconstruct(out skillDef, out num);
				SkillDef skillDef2 = skillDef;
				float num2 = num;
				float num3 = 1f;
				if (reader != null)
				{
					num3 = reader.skills.GetSkill(skillDef2).LearnRateFactor(false);
					num2 *= num3;
				}
				float f = num2 * 2500f;
				string text = string.Format("{0}: {1}", skillDef2.LabelCap, "XpPerHour".Translate(f.ToStringDecimalIfSmall()));
				int maxSkillLevel = BookOutcomeDoerGainSkillExp.GetMaxSkillLevel(base.Quality);
				text += string.Format(" ({0})", "BookMaxLevel".Translate(maxSkillLevel));
				if (!Mathf.Approximately(num3, 1f))
				{
					text += string.Format(" (x{0} {1})", num3.ToStringPercent("0"), "BookLearningModifier".Translate());
				}
				stringBuilder.AppendLine(" - " + text);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060092BA RID: 37562 RVA: 0x002B357C File Offset: 0x002B177C
		public override IEnumerable<RulePack> GetTopicRulePacks()
		{
			return from x in this.values.Keys
			select x.generalRules;
		}

		// Token: 0x060092BB RID: 37563 RVA: 0x002B35AD File Offset: 0x002B17AD
		public override void PostExposeData()
		{
			Scribe_Collections.Look<SkillDef, float>(ref this.values, "values", LookMode.Undefined, LookMode.Undefined);
		}

		// Token: 0x060092BC RID: 37564 RVA: 0x002B35C1 File Offset: 0x002B17C1
		private static bool CanProgressSkill(Pawn pawn, SkillDef skill, QualityCategory quality)
		{
			return !pawn.skills.GetSkill(skill).TotallyDisabled && pawn.skills.GetSkill(skill).GetLevel(false) < BookOutcomeDoerGainSkillExp.GetMaxSkillLevel(quality);
		}

		// Token: 0x060092BD RID: 37565 RVA: 0x002B35F2 File Offset: 0x002B17F2
		private static int GetMaxSkillLevel(QualityCategory quality)
		{
			return Mathf.RoundToInt(BookOutcomeDoerGainSkillExp.QualityMaxLevel.Evaluate((float)quality));
		}

		// Token: 0x04005D9A RID: 23962
		private Dictionary<SkillDef, float> values = new Dictionary<SkillDef, float>();

		// Token: 0x04005D9B RID: 23963
		private const float MultipleSkillFactor = 1.25f;

		// Token: 0x04005D9C RID: 23964
		private static readonly SimpleCurve QualityMaxLevel = new SimpleCurve
		{
			{
				new CurvePoint(0f, 2f),
				true
			},
			{
				new CurvePoint(1f, 5f),
				true
			},
			{
				new CurvePoint(2f, 8f),
				true
			},
			{
				new CurvePoint(3f, 10f),
				true
			},
			{
				new CurvePoint(4f, 12f),
				true
			},
			{
				new CurvePoint(5f, 14f),
				true
			},
			{
				new CurvePoint(6f, 16f),
				true
			}
		};
	}
}
