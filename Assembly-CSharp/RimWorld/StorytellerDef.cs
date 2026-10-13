using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AF1 RID: 6897
	public class StorytellerDef : Def
	{
		// Token: 0x06009A1E RID: 39454 RVA: 0x002CD81C File Offset: 0x002CBA1C
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (!this.portraitTiny.NullOrEmpty())
				{
					this.portraitTinyTex = ContentFinder<Texture2D>.Get(this.portraitTiny, true);
					this.portraitLargeTex = ContentFinder<Texture2D>.Get(this.portraitLarge, true);
				}
			});
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].ResolveReferences(this);
			}
		}

		// Token: 0x06009A1F RID: 39455 RVA: 0x002CD868 File Offset: 0x002CBA68
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.pointsFactorFromAdaptDays == null)
			{
				yield return "pointsFactorFromAdaptDays is null";
			}
			if (this.adaptDaysLossFromColonistLostByPostPopulation == null)
			{
				yield return "adaptDaysLossFromColonistLostByPostPopulation is null";
			}
			if (this.adaptDaysLossFromColonistViolentlyDownedByPopulation == null)
			{
				yield return "adaptDaysLossFromColonistViolentlyDownedByPopulation is null";
			}
			if (this.adaptDaysGrowthRateCurve == null)
			{
				yield return "adaptDaysGrowthRateCurve is null";
			}
			if (this.pointsFactorFromDaysPassed == null)
			{
				yield return "pointsFactorFromDaysPassed is null";
			}
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
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

		// Token: 0x04006A09 RID: 27145
		public int listOrder = 9999;

		// Token: 0x04006A0A RID: 27146
		public bool listVisible = true;

		// Token: 0x04006A0B RID: 27147
		public bool tutorialMode;

		// Token: 0x04006A0C RID: 27148
		public bool disableAdaptiveTraining;

		// Token: 0x04006A0D RID: 27149
		public bool disableAlerts;

		// Token: 0x04006A0E RID: 27150
		public bool disablePermadeath;

		// Token: 0x04006A0F RID: 27151
		public DifficultyDef forcedDifficulty;

		// Token: 0x04006A10 RID: 27152
		[NoTranslate]
		private string portraitLarge;

		// Token: 0x04006A11 RID: 27153
		[NoTranslate]
		private string portraitTiny;

		// Token: 0x04006A12 RID: 27154
		public List<StorytellerCompProperties> comps = new List<StorytellerCompProperties>();

		// Token: 0x04006A13 RID: 27155
		public SimpleCurve populationIntentFactorFromPopCurve;

		// Token: 0x04006A14 RID: 27156
		public SimpleCurve populationIntentFactorFromPopAdaptDaysCurve;

		// Token: 0x04006A15 RID: 27157
		public SimpleCurve pointsFactorFromDaysPassed;

		// Token: 0x04006A16 RID: 27158
		public float adaptDaysMin;

		// Token: 0x04006A17 RID: 27159
		public float adaptDaysMax = 100f;

		// Token: 0x04006A18 RID: 27160
		public float adaptDaysGameStartGraceDays;

		// Token: 0x04006A19 RID: 27161
		public SimpleCurve pointsFactorFromAdaptDays;

		// Token: 0x04006A1A RID: 27162
		public SimpleCurve adaptDaysLossFromColonistLostByPostPopulation;

		// Token: 0x04006A1B RID: 27163
		public SimpleCurve adaptDaysLossFromColonistViolentlyDownedByPopulation;

		// Token: 0x04006A1C RID: 27164
		public SimpleCurve adaptDaysGrowthRateCurve;

		// Token: 0x04006A1D RID: 27165
		[Unsaved(false)]
		public Texture2D portraitLargeTex;

		// Token: 0x04006A1E RID: 27166
		[Unsaved(false)]
		public Texture2D portraitTinyTex;
	}
}
