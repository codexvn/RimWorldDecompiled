using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002474 RID: 9332
	public class Need_Chemical_Any : Need
	{
		// Token: 0x17001EAC RID: 7852
		// (get) Token: 0x0600CA7A RID: 51834 RVA: 0x003B4EC9 File Offset: 0x003B30C9
		private Trait TraitDrugDesire
		{
			get
			{
				return this.pawn.story.traits.GetTrait(TraitDefOf.DrugDesire);
			}
		}

		// Token: 0x17001EAD RID: 7853
		// (get) Token: 0x0600CA7B RID: 51835 RVA: 0x003B4EE5 File Offset: 0x003B30E5
		private SimpleCurve FallCurve
		{
			get
			{
				if (this.TraitDrugDesire.Degree == 2)
				{
					return Need_Chemical_Any.FascinationDegreeFallCurve;
				}
				return Need_Chemical_Any.InterestDegreeFallCurve;
			}
		}

		// Token: 0x17001EAE RID: 7854
		// (get) Token: 0x0600CA7C RID: 51836 RVA: 0x003B4F00 File Offset: 0x003B3100
		private float FallPerNeedIntervalTick
		{
			get
			{
				Trait traitDrugDesire = this.TraitDrugDesire;
				float num = 1f;
				if (traitDrugDesire.Degree == 2)
				{
					num = 1.25f;
				}
				num *= this.FallCurve.Evaluate(this.CurLevel);
				return this.def.fallPerDay * num / 60000f * 150f;
			}
		}

		// Token: 0x17001EAF RID: 7855
		// (get) Token: 0x0600CA7D RID: 51837 RVA: 0x003B4F54 File Offset: 0x003B3154
		private Need_Chemical_Any.LevelThresholds CurrentLevelThresholds
		{
			get
			{
				if (this.TraitDrugDesire.Degree == 2)
				{
					return Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood;
				}
				return Need_Chemical_Any.InterestDegreeLevelThresholdsForMood;
			}
		}

		// Token: 0x17001EB0 RID: 7856
		// (get) Token: 0x0600CA7E RID: 51838 RVA: 0x003B4F70 File Offset: 0x003B3170
		public Need_Chemical_Any.MoodBuff MoodBuffForCurrentLevel
		{
			get
			{
				if (this.Disabled)
				{
					return Need_Chemical_Any.MoodBuff.Neutral;
				}
				Need_Chemical_Any.LevelThresholds currentLevelThresholds = this.CurrentLevelThresholds;
				float curLevel = this.CurLevel;
				if (curLevel <= currentLevelThresholds.extremelyNegative)
				{
					return Need_Chemical_Any.MoodBuff.ExtremelyNegative;
				}
				if (curLevel <= currentLevelThresholds.veryNegative)
				{
					return Need_Chemical_Any.MoodBuff.VeryNegative;
				}
				if (curLevel <= currentLevelThresholds.negative)
				{
					return Need_Chemical_Any.MoodBuff.Negative;
				}
				if (curLevel <= currentLevelThresholds.positive)
				{
					return Need_Chemical_Any.MoodBuff.Neutral;
				}
				if (curLevel <= currentLevelThresholds.veryPositive)
				{
					return Need_Chemical_Any.MoodBuff.Positive;
				}
				return Need_Chemical_Any.MoodBuff.VeryPositive;
			}
		}

		// Token: 0x17001EB1 RID: 7857
		// (get) Token: 0x0600CA7F RID: 51839 RVA: 0x00002501 File Offset: 0x00000701
		public override int GUIChangeArrow
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17001EB2 RID: 7858
		// (get) Token: 0x0600CA80 RID: 51840 RVA: 0x003B4FCD File Offset: 0x003B31CD
		public override bool ShowOnNeedList
		{
			get
			{
				return !this.Disabled;
			}
		}

		// Token: 0x17001EB3 RID: 7859
		// (get) Token: 0x0600CA81 RID: 51841 RVA: 0x003B4FD8 File Offset: 0x003B31D8
		private bool Disabled
		{
			get
			{
				return this.TraitDrugDesire == null || this.TraitDrugDesire.Degree < 1;
			}
		}

		// Token: 0x0600CA82 RID: 51842 RVA: 0x003B4FF4 File Offset: 0x003B31F4
		public void Notify_IngestedDrug(Thing drug)
		{
			if (this.Disabled)
			{
				return;
			}
			DrugCategory drugCategory = drug.def.ingestible.drugCategory;
			if (drugCategory == DrugCategory.Social)
			{
				this.CurLevel += 0.2f;
				return;
			}
			if (drugCategory != DrugCategory.Hard)
			{
				return;
			}
			this.CurLevel += 0.3f;
		}

		// Token: 0x0600CA83 RID: 51843 RVA: 0x003B5049 File Offset: 0x003B3249
		public Need_Chemical_Any(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x0600CA84 RID: 51844 RVA: 0x003B5052 File Offset: 0x003B3252
		public override void SetInitialLevel()
		{
			this.CurLevel = 0.5f;
		}

		// Token: 0x0600CA85 RID: 51845 RVA: 0x003B5060 File Offset: 0x003B3260
		public override void DrawOnGUI(Rect rect, int maxThresholdMarkers = 2147483647, float customMargin = -1f, bool drawArrows = true, bool doTooltip = true, Rect? rectForTooltip = null, bool drawLabel = true)
		{
			Trait traitDrugDesire = this.TraitDrugDesire;
			if (traitDrugDesire != null && this.lastThresholdUpdateTraitRef != traitDrugDesire)
			{
				this.lastThresholdUpdateTraitRef = traitDrugDesire;
				this.threshPercents = new List<float>();
				Need_Chemical_Any.LevelThresholds currentLevelThresholds = this.CurrentLevelThresholds;
				this.threshPercents.Add(currentLevelThresholds.extremelyNegative);
				this.threshPercents.Add(currentLevelThresholds.veryNegative);
				this.threshPercents.Add(currentLevelThresholds.negative);
				this.threshPercents.Add(currentLevelThresholds.positive);
				this.threshPercents.Add(currentLevelThresholds.veryPositive);
			}
			base.DrawOnGUI(rect, maxThresholdMarkers, customMargin, drawArrows, doTooltip, rectForTooltip, drawLabel);
		}

		// Token: 0x0600CA86 RID: 51846 RVA: 0x003B50FF File Offset: 0x003B32FF
		public override void NeedInterval()
		{
			if (this.Disabled)
			{
				this.SetInitialLevel();
				return;
			}
			if (this.IsFrozen)
			{
				return;
			}
			this.CurLevel -= this.FallPerNeedIntervalTick;
		}

		// Token: 0x040089CE RID: 35278
		public const int InterestTraitDegree = 1;

		// Token: 0x040089CF RID: 35279
		public const int FascinationTraitDegree = 2;

		// Token: 0x040089D0 RID: 35280
		private const float FallPerTickFactorForChemicalFascination = 1.25f;

		// Token: 0x040089D1 RID: 35281
		public const float GainForHardDrugIngestion = 0.3f;

		// Token: 0x040089D2 RID: 35282
		public const float GainForSocialDrugIngestion = 0.2f;

		// Token: 0x040089D3 RID: 35283
		private static readonly SimpleCurve InterestDegreeFallCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.3f),
				true
			},
			{
				new CurvePoint(Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood.negative, 0.6f),
				true
			},
			{
				new CurvePoint(Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood.negative + 0.001f, 1f),
				true
			},
			{
				new CurvePoint(Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood.positive, 1f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			}
		};

		// Token: 0x040089D4 RID: 35284
		private static readonly SimpleCurve FascinationDegreeFallCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.4f),
				true
			},
			{
				new CurvePoint(Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood.negative, 0.7f),
				true
			},
			{
				new CurvePoint(Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood.negative + 0.001f, 1f),
				true
			},
			{
				new CurvePoint(Need_Chemical_Any.FascinationDegreeLevelThresholdsForMood.positive, 1f),
				true
			},
			{
				new CurvePoint(1f, 1.15f),
				true
			}
		};

		// Token: 0x040089D5 RID: 35285
		private static readonly Need_Chemical_Any.LevelThresholds FascinationDegreeLevelThresholdsForMood = new Need_Chemical_Any.LevelThresholds
		{
			extremelyNegative = 0.1f,
			veryNegative = 0.25f,
			negative = 0.4f,
			positive = 0.7f,
			veryPositive = 0.85f
		};

		// Token: 0x040089D6 RID: 35286
		private static readonly Need_Chemical_Any.LevelThresholds InterestDegreeLevelThresholdsForMood = new Need_Chemical_Any.LevelThresholds
		{
			extremelyNegative = 0.01f,
			veryNegative = 0.15f,
			negative = 0.3f,
			positive = 0.6f,
			veryPositive = 0.75f
		};

		// Token: 0x040089D7 RID: 35287
		private Trait lastThresholdUpdateTraitRef;

		// Token: 0x02002475 RID: 9333
		public enum MoodBuff
		{
			// Token: 0x040089D9 RID: 35289
			ExtremelyNegative,
			// Token: 0x040089DA RID: 35290
			VeryNegative,
			// Token: 0x040089DB RID: 35291
			Negative,
			// Token: 0x040089DC RID: 35292
			Neutral,
			// Token: 0x040089DD RID: 35293
			Positive,
			// Token: 0x040089DE RID: 35294
			VeryPositive
		}

		// Token: 0x02002476 RID: 9334
		public struct LevelThresholds
		{
			// Token: 0x040089DF RID: 35295
			public float extremelyNegative;

			// Token: 0x040089E0 RID: 35296
			public float veryNegative;

			// Token: 0x040089E1 RID: 35297
			public float negative;

			// Token: 0x040089E2 RID: 35298
			public float positive;

			// Token: 0x040089E3 RID: 35299
			public float veryPositive;
		}
	}
}
