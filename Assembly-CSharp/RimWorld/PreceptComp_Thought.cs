using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002641 RID: 9793
	public abstract class PreceptComp_Thought : PreceptComp
	{
		// Token: 0x170020CF RID: 8399
		// (get) Token: 0x0600D68A RID: 54922 RVA: 0x003FA5A4 File Offset: 0x003F87A4
		public bool AffectsMood
		{
			get
			{
				for (int i = 0; i < this.thought.stages.Count; i++)
				{
					if (this.thought.stages[i].baseMoodEffect != 0f)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x0600D68B RID: 54923 RVA: 0x003FA5EC File Offset: 0x003F87EC
		private string ParseDescription(string description, int thoughtStage = -1)
		{
			bool flag = thoughtStage != -1;
			if (!flag)
			{
				thoughtStage = 0;
			}
			if (!description.NullOrEmpty())
			{
				IPreceptCompDescriptionArgs preceptCompDescriptionArgs = this.thought.Worker as IPreceptCompDescriptionArgs;
				if (preceptCompDescriptionArgs != null)
				{
					description = description.Formatted(preceptCompDescriptionArgs.GetDescriptionArgs());
				}
			}
			if (description.NullOrEmpty())
			{
				description = ((!this.thought.stages[thoughtStage].LabelAbstractCap.NullOrEmpty()) ? this.thought.stages[thoughtStage].LabelAbstractCap : this.thought.LabelCap.Resolve());
				description = description.Formatted(Gender.Male.Named("PAWN_gender"));
			}
			if (this.AffectsMood)
			{
				ThoughtStage thoughtStage2 = this.thought.stages[thoughtStage];
				if (this.thought.minExpectation != null)
				{
					description += " (" + "MinExpectation".Translate() + ": " + this.thought.minExpectation.LabelCap + ")";
				}
				else if (this.thought.minExpectationForNegativeThought != null && thoughtStage2.baseMoodEffect < 0f)
				{
					description += " (" + "MinExpectation".Translate() + ": " + this.thought.minExpectationForNegativeThought.LabelCap + ")";
				}
				string str;
				if (this.tooltipShowMoodRange && !flag)
				{
					float num = float.PositiveInfinity;
					float num2 = float.NegativeInfinity;
					for (int i = 0; i < this.thought.stages.Count; i++)
					{
						num = Mathf.Min(num, this.thought.stages[i].baseMoodEffect);
						num2 = Mathf.Max(num2, this.thought.stages[i].baseMoodEffect);
					}
					str = "PreceptThoughtMoodRange".Translate(num.ToStringWithSign("F0"), num2.ToStringWithSign("F0"));
				}
				else
				{
					str = thoughtStage2.baseMoodEffect.ToStringWithSign("F0");
				}
				return description + ": " + str;
			}
			return description + ": " + this.thought.stages[thoughtStage].baseOpinionOffset.ToStringWithSign("F0");
		}

		// Token: 0x0600D68C RID: 54924 RVA: 0x003FA884 File Offset: 0x003F8A84
		public override IEnumerable<string> GetDescriptions()
		{
			if (!this.thoughtStageDescriptions.NullOrEmpty<string>())
			{
				int stage = 0;
				foreach (string description in this.thoughtStageDescriptions)
				{
					yield return this.ParseDescription(description, stage);
					int num = stage;
					stage = num + 1;
				}
				List<string>.Enumerator enumerator = default(List<string>.Enumerator);
			}
			else
			{
				foreach (string description2 in base.GetDescriptions())
				{
					yield return this.ParseDescription(description2, -1);
				}
				IEnumerator<string> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x040091D2 RID: 37330
		public ThoughtDef thought;

		// Token: 0x040091D3 RID: 37331
		[MustTranslate]
		public List<string> thoughtStageDescriptions;

		// Token: 0x040091D4 RID: 37332
		public bool tooltipShowMoodRange;
	}
}
