using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002516 RID: 9494
	public class SkillRecord : IExposable, IComparable<SkillRecord>
	{
		// Token: 0x17001FD3 RID: 8147
		// (get) Token: 0x0600CF64 RID: 53092 RVA: 0x003CDB50 File Offset: 0x003CBD50
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17001FD4 RID: 8148
		// (get) Token: 0x0600CF65 RID: 53093 RVA: 0x003CDB58 File Offset: 0x003CBD58
		// (set) Token: 0x0600CF66 RID: 53094 RVA: 0x003CDB61 File Offset: 0x003CBD61
		public int Level
		{
			get
			{
				return this.GetLevel(true);
			}
			set
			{
				this.levelInt = Mathf.Clamp(value, 0, 20);
			}
		}

		// Token: 0x17001FD5 RID: 8149
		// (get) Token: 0x0600CF67 RID: 53095 RVA: 0x003CDB72 File Offset: 0x003CBD72
		public float XpRequiredForLevelUp
		{
			get
			{
				return SkillRecord.XpRequiredToLevelUpFrom(this.levelInt);
			}
		}

		// Token: 0x17001FD6 RID: 8150
		// (get) Token: 0x0600CF68 RID: 53096 RVA: 0x003CDB7F File Offset: 0x003CBD7F
		public float XpProgressPercent
		{
			get
			{
				return this.xpSinceLastLevel / this.XpRequiredForLevelUp;
			}
		}

		// Token: 0x17001FD7 RID: 8151
		// (get) Token: 0x0600CF69 RID: 53097 RVA: 0x003CDB90 File Offset: 0x003CBD90
		public float XpTotalEarned
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.levelInt; i++)
				{
					num += SkillRecord.XpRequiredToLevelUpFrom(i);
				}
				return num;
			}
		}

		// Token: 0x17001FD8 RID: 8152
		// (get) Token: 0x0600CF6A RID: 53098 RVA: 0x003CDBBE File Offset: 0x003CBDBE
		public bool TotallyDisabled
		{
			get
			{
				if (this.cachedTotallyDisabled == BoolUnknown.Unknown)
				{
					this.cachedTotallyDisabled = (this.CalculateTotallyDisabled() ? BoolUnknown.True : BoolUnknown.False);
				}
				return this.PermanentlyDisabled || this.cachedTotallyDisabled == BoolUnknown.True;
			}
		}

		// Token: 0x17001FD9 RID: 8153
		// (get) Token: 0x0600CF6B RID: 53099 RVA: 0x003CDBEE File Offset: 0x003CBDEE
		public bool PermanentlyDisabled
		{
			get
			{
				if (this.cachedPermanentlyDisabled == BoolUnknown.Unknown)
				{
					this.cachedPermanentlyDisabled = (this.CalculatePermanentlyDisabled() ? BoolUnknown.True : BoolUnknown.False);
				}
				return this.cachedPermanentlyDisabled == BoolUnknown.True;
			}
		}

		// Token: 0x17001FDA RID: 8154
		// (get) Token: 0x0600CF6C RID: 53100 RVA: 0x003CDC14 File Offset: 0x003CBE14
		public string LevelDescriptor
		{
			get
			{
				switch (this.GetLevelForUI(true))
				{
				case 0:
					return "Skill0".Translate();
				case 1:
					return "Skill1".Translate();
				case 2:
					return "Skill2".Translate();
				case 3:
					return "Skill3".Translate();
				case 4:
					return "Skill4".Translate();
				case 5:
					return "Skill5".Translate();
				case 6:
					return "Skill6".Translate();
				case 7:
					return "Skill7".Translate();
				case 8:
					return "Skill8".Translate();
				case 9:
					return "Skill9".Translate();
				case 10:
					return "Skill10".Translate();
				case 11:
					return "Skill11".Translate();
				case 12:
					return "Skill12".Translate();
				case 13:
					return "Skill13".Translate();
				case 14:
					return "Skill14".Translate();
				case 15:
					return "Skill15".Translate();
				case 16:
					return "Skill16".Translate();
				case 17:
					return "Skill17".Translate();
				case 18:
					return "Skill18".Translate();
				case 19:
					return "Skill19".Translate();
				case 20:
					return "Skill20".Translate();
				default:
					return "Unknown";
				}
			}
		}

		// Token: 0x17001FDB RID: 8155
		// (get) Token: 0x0600CF6D RID: 53101 RVA: 0x003CDDDD File Offset: 0x003CBFDD
		public bool LearningSaturatedToday
		{
			get
			{
				return this.xpSinceMidnight > 4000f;
			}
		}

		// Token: 0x17001FDC RID: 8156
		// (get) Token: 0x0600CF6E RID: 53102 RVA: 0x003CDDEC File Offset: 0x003CBFEC
		public int Aptitude
		{
			get
			{
				if (this.aptitudeCached == null)
				{
					this.aptitudeCached = new int?(0);
					if (ModsConfig.BiotechActive && this.pawn.genes != null)
					{
						foreach (Gene gene in this.pawn.genes.GenesListForReading)
						{
							if (gene.Active)
							{
								this.aptitudeCached += gene.def.AptitudeFor(this.def);
							}
						}
					}
					if (ModsConfig.AnomalyActive)
					{
						Pawn_StoryTracker story = this.pawn.story;
						if (((story != null) ? story.traits : null) != null)
						{
							foreach (Trait trait in this.pawn.story.traits.TraitsSorted)
							{
								if (!trait.Suppressed)
								{
									this.aptitudeCached += trait.def.DataAtDegree(trait.Degree).AptitudeFor(this.def);
								}
							}
						}
						Pawn_HealthTracker health = this.pawn.health;
						if (((health != null) ? health.hediffSet : null) != null)
						{
							foreach (Hediff hediff in this.pawn.health.hediffSet.hediffs)
							{
								this.aptitudeCached += hediff.def.AptitudeFor(this.def);
							}
						}
					}
				}
				return this.aptitudeCached.Value;
			}
		}

		// Token: 0x0600CF6F RID: 53103 RVA: 0x003CE044 File Offset: 0x003CC244
		public SkillRecord()
		{
		}

		// Token: 0x0600CF70 RID: 53104 RVA: 0x003CE05A File Offset: 0x003CC25A
		public SkillRecord(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CF71 RID: 53105 RVA: 0x003CE077 File Offset: 0x003CC277
		public SkillRecord(Pawn pawn, SkillDef def)
		{
			this.pawn = pawn;
			this.def = def;
		}

		// Token: 0x0600CF72 RID: 53106 RVA: 0x003CE09C File Offset: 0x003CC29C
		public void ExposeData()
		{
			Scribe_Defs.Look<SkillDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.levelInt, "level", 0, false);
			Scribe_Values.Look<float>(ref this.xpSinceLastLevel, "xpSinceLastLevel", 0f, false);
			Scribe_Values.Look<Passion>(ref this.passion, "passion", Passion.None, false);
			Scribe_Values.Look<float>(ref this.xpSinceMidnight, "xpSinceMidnight", 0f, false);
		}

		// Token: 0x0600CF73 RID: 53107 RVA: 0x003CE10C File Offset: 0x003CC30C
		public void Interval()
		{
			if (ModsConfig.AnomalyActive && this.pawn.story.traits.HasTrait(TraitDefOf.PerfectMemory))
			{
				return;
			}
			float num = this.pawn.story.traits.HasTrait(TraitDefOf.GreatMemory) ? 0.5f : 1f;
			switch (this.levelInt)
			{
			case 10:
				this.Learn(-0.1f * num, false, false);
				return;
			case 11:
				this.Learn(-0.2f * num, false, false);
				return;
			case 12:
				this.Learn(-0.4f * num, false, false);
				return;
			case 13:
				this.Learn(-0.6f * num, false, false);
				return;
			case 14:
				this.Learn(-1f * num, false, false);
				return;
			case 15:
				this.Learn(-1.8f * num, false, false);
				return;
			case 16:
				this.Learn(-2.8f * num, false, false);
				return;
			case 17:
				this.Learn(-4f * num, false, false);
				return;
			case 18:
				this.Learn(-6f * num, false, false);
				return;
			case 19:
				this.Learn(-8f * num, false, false);
				return;
			case 20:
				this.Learn(-12f * num, false, false);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600CF74 RID: 53108 RVA: 0x003CE252 File Offset: 0x003CC452
		public static float XpRequiredToLevelUpFrom(int startingLevel)
		{
			return SkillRecord.XpForLevelUpCurve.Evaluate((float)startingLevel);
		}

		// Token: 0x0600CF75 RID: 53109 RVA: 0x003CE260 File Offset: 0x003CC460
		public void Learn(float xp, bool direct = false, bool ignoreLearnRate = false)
		{
			if (this.TotallyDisabled)
			{
				return;
			}
			if (xp < 0f && this.levelInt == 0)
			{
				return;
			}
			bool flag = false;
			if (xp > 0f && !ignoreLearnRate)
			{
				xp *= this.LearnRateFactor(direct);
			}
			this.xpSinceLastLevel += xp;
			if (!direct)
			{
				this.xpSinceMidnight += xp;
			}
			if (this.levelInt == 20 && this.xpSinceLastLevel > this.XpRequiredForLevelUp - 1f)
			{
				this.xpSinceLastLevel = this.XpRequiredForLevelUp - 1f;
			}
			while (this.xpSinceLastLevel >= this.XpRequiredForLevelUp)
			{
				this.xpSinceLastLevel -= this.XpRequiredForLevelUp;
				this.levelInt++;
				flag = true;
				if (this.levelInt == 14)
				{
					if (this.passion == Passion.None)
					{
						TaleRecorder.RecordTale(TaleDefOf.GainedMasterSkillWithoutPassion, new object[]
						{
							this.pawn,
							this.def
						});
					}
					else
					{
						TaleRecorder.RecordTale(TaleDefOf.GainedMasterSkillWithPassion, new object[]
						{
							this.pawn,
							this.def
						});
					}
				}
				if (this.levelInt >= 20)
				{
					this.levelInt = 20;
					this.xpSinceLastLevel = Mathf.Clamp(this.xpSinceLastLevel, 0f, this.XpRequiredForLevelUp - 1f);
					IL_190:
					while (this.xpSinceLastLevel <= -1000f)
					{
						this.levelInt--;
						this.xpSinceLastLevel += this.XpRequiredForLevelUp;
						if (this.levelInt <= 0)
						{
							this.levelInt = 0;
							this.xpSinceLastLevel = 0f;
							break;
						}
					}
					if (flag && this.pawn.IsColonist && this.pawn.SpawnedOrAnyParentSpawned)
					{
						MoteMaker.ThrowText(this.pawn.DrawPosHeld ?? this.pawn.PositionHeld.ToVector3Shifted(), this.pawn.MapHeld, this.def.LabelCap + "\n" + "TextMote_SkillUp".Translate(this.Level), -1f);
					}
					return;
				}
			}
			goto IL_190;
		}

		// Token: 0x0600CF76 RID: 53110 RVA: 0x003CE4A8 File Offset: 0x003CC6A8
		public int CompareTo(SkillRecord other)
		{
			int result = this.Level - other.Level;
			if (this.Level != other.Level)
			{
				return result;
			}
			if (this.xpSinceLastLevel == other.xpSinceLastLevel)
			{
				return 0;
			}
			if (this.xpSinceLastLevel <= other.xpSinceLastLevel)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x0600CF77 RID: 53111 RVA: 0x003CE4F4 File Offset: 0x003CC6F4
		public int GetLevel(bool includeAptitudes = true)
		{
			if (this.TotallyDisabled)
			{
				return 0;
			}
			int num = this.levelInt;
			if (includeAptitudes)
			{
				num += this.Aptitude;
			}
			return Mathf.Clamp(num, 0, 20);
		}

		// Token: 0x0600CF78 RID: 53112 RVA: 0x003CE528 File Offset: 0x003CC728
		public int GetLevelForUI(bool includeAptitudes = true)
		{
			if (this.PermanentlyDisabled)
			{
				return 0;
			}
			int num = this.levelInt;
			if (includeAptitudes)
			{
				num += this.Aptitude;
			}
			return Mathf.Clamp(num, 0, 20);
		}

		// Token: 0x0600CF79 RID: 53113 RVA: 0x003CE55B File Offset: 0x003CC75B
		public int GetUnclampedLevel()
		{
			if (this.PermanentlyDisabled)
			{
				return 0;
			}
			return this.levelInt + this.Aptitude;
		}

		// Token: 0x0600CF7A RID: 53114 RVA: 0x003CE574 File Offset: 0x003CC774
		public float LearnRateFactor(bool direct = false)
		{
			if (DebugSettings.fastLearning)
			{
				return 200f;
			}
			float num;
			switch (this.passion)
			{
			case Passion.None:
				num = 0.35f;
				break;
			case Passion.Minor:
				num = 1f;
				break;
			case Passion.Major:
				num = 1.5f;
				break;
			default:
				throw new NotImplementedException("Passion level " + this.passion.ToString());
			}
			if (!direct)
			{
				num *= this.pawn.GetStatValue(StatDefOf.GlobalLearningFactor, true, -1);
				if (this.def == SkillDefOf.Animals)
				{
					num *= this.pawn.GetStatValue(StatDefOf.AnimalsLearningFactor, true, -1);
				}
				if (this.LearningSaturatedToday)
				{
					num *= 0.2f;
				}
			}
			return num;
		}

		// Token: 0x0600CF7B RID: 53115 RVA: 0x003CE630 File Offset: 0x003CC830
		public void EnsureMinLevelWithMargin(int minLevel)
		{
			if (this.TotallyDisabled)
			{
				return;
			}
			if (this.levelInt < minLevel || (this.levelInt == minLevel && this.xpSinceLastLevel < this.XpRequiredForLevelUp / 2f))
			{
				this.levelInt = minLevel;
				this.xpSinceLastLevel = this.XpRequiredForLevelUp / 2f;
			}
		}

		// Token: 0x0600CF7C RID: 53116 RVA: 0x003CE685 File Offset: 0x003CC885
		public void Notify_SkillDisablesChanged()
		{
			this.cachedTotallyDisabled = BoolUnknown.Unknown;
			this.cachedPermanentlyDisabled = BoolUnknown.Unknown;
		}

		// Token: 0x0600CF7D RID: 53117 RVA: 0x003CE695 File Offset: 0x003CC895
		public void DirtyAptitudes()
		{
			this.aptitudeCached = null;
		}

		// Token: 0x0600CF7E RID: 53118 RVA: 0x003CE6A3 File Offset: 0x003CC8A3
		private bool CalculateTotallyDisabled()
		{
			return this.def.IsDisabled(this.pawn.CombinedDisabledWorkTags, this.pawn.GetDisabledWorkTypes(false));
		}

		// Token: 0x0600CF7F RID: 53119 RVA: 0x003CE6C7 File Offset: 0x003CC8C7
		private bool CalculatePermanentlyDisabled()
		{
			return this.def.IsDisabled(this.pawn.CombinedDisabledWorkTags, this.pawn.GetDisabledWorkTypes(true));
		}

		// Token: 0x0600CF80 RID: 53120 RVA: 0x003CE6EC File Offset: 0x003CC8EC
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.def.defName,
				": ",
				this.levelInt.ToString(),
				" (",
				this.xpSinceLastLevel.ToString(),
				"xp)"
			});
		}

		// Token: 0x04008D0C RID: 36108
		private Pawn pawn;

		// Token: 0x04008D0D RID: 36109
		public SkillDef def;

		// Token: 0x04008D0E RID: 36110
		public int levelInt;

		// Token: 0x04008D0F RID: 36111
		public Passion passion;

		// Token: 0x04008D10 RID: 36112
		public float xpSinceLastLevel;

		// Token: 0x04008D11 RID: 36113
		public float xpSinceMidnight;

		// Token: 0x04008D12 RID: 36114
		private BoolUnknown cachedTotallyDisabled = BoolUnknown.Unknown;

		// Token: 0x04008D13 RID: 36115
		private BoolUnknown cachedPermanentlyDisabled = BoolUnknown.Unknown;

		// Token: 0x04008D14 RID: 36116
		private int? aptitudeCached;

		// Token: 0x04008D15 RID: 36117
		public const int IntervalTicks = 200;

		// Token: 0x04008D16 RID: 36118
		public const int MinLevel = 0;

		// Token: 0x04008D17 RID: 36119
		public const int MaxLevel = 20;

		// Token: 0x04008D18 RID: 36120
		public const int MaxFullRateXpPerDay = 4000;

		// Token: 0x04008D19 RID: 36121
		public const int MasterSkillThreshold = 14;

		// Token: 0x04008D1A RID: 36122
		public const float SaturatedLearningFactor = 0.2f;

		// Token: 0x04008D1B RID: 36123
		public const float LearnFactorPassionNone = 0.35f;

		// Token: 0x04008D1C RID: 36124
		public const float LearnFactorPassionMinor = 1f;

		// Token: 0x04008D1D RID: 36125
		public const float LearnFactorPassionMajor = 1.5f;

		// Token: 0x04008D1E RID: 36126
		public const float MinXPAmount = -1000f;

		// Token: 0x04008D1F RID: 36127
		private static readonly SimpleCurve XpForLevelUpCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1000f),
				true
			},
			{
				new CurvePoint(9f, 10000f),
				true
			},
			{
				new CurvePoint(19f, 30000f),
				true
			}
		};
	}
}
