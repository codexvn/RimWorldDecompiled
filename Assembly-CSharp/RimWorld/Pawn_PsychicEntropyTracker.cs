using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020024CD RID: 9421
	public class Pawn_PsychicEntropyTracker : IExposable
	{
		// Token: 0x17001F64 RID: 8036
		// (get) Token: 0x0600CD3D RID: 52541 RVA: 0x003C2CB1 File Offset: 0x003C0EB1
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17001F65 RID: 8037
		// (get) Token: 0x0600CD3E RID: 52542 RVA: 0x003C2CB9 File Offset: 0x003C0EB9
		public float MaxEntropy
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.PsychicEntropyMax, true, -1);
			}
		}

		// Token: 0x17001F66 RID: 8038
		// (get) Token: 0x0600CD3F RID: 52543 RVA: 0x003C2CCD File Offset: 0x003C0ECD
		public float MaxPotentialEntropy
		{
			get
			{
				return Mathf.Max(this.pawn.GetStatValue(StatDefOf.PsychicEntropyMax, true, -1), this.MaxEntropy);
			}
		}

		// Token: 0x17001F67 RID: 8039
		// (get) Token: 0x0600CD40 RID: 52544 RVA: 0x003C2CEC File Offset: 0x003C0EEC
		public float RecoveryRate
		{
			get
			{
				return this.pawn.GetStatValue(StatDefOf.PsychicEntropyRecoveryRate, true, -1);
			}
		}

		// Token: 0x17001F68 RID: 8040
		// (get) Token: 0x0600CD41 RID: 52545 RVA: 0x003C2D00 File Offset: 0x003C0F00
		public float EntropyValue
		{
			get
			{
				return this.currentEntropy;
			}
		}

		// Token: 0x17001F69 RID: 8041
		// (get) Token: 0x0600CD42 RID: 52546 RVA: 0x003C2D08 File Offset: 0x003C0F08
		public float CurrentPsyfocus
		{
			get
			{
				return this.currentPsyfocus;
			}
		}

		// Token: 0x17001F6A RID: 8042
		// (get) Token: 0x0600CD43 RID: 52547 RVA: 0x003C2D10 File Offset: 0x003C0F10
		public float TargetPsyfocus
		{
			get
			{
				return this.targetPsyfocus;
			}
		}

		// Token: 0x17001F6B RID: 8043
		// (get) Token: 0x0600CD44 RID: 52548 RVA: 0x003C2D18 File Offset: 0x003C0F18
		public int MaxAbilityLevel
		{
			get
			{
				return Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand[this.PsyfocusBand];
			}
		}

		// Token: 0x17001F6C RID: 8044
		// (get) Token: 0x0600CD45 RID: 52549 RVA: 0x003C2D2A File Offset: 0x003C0F2A
		public bool IsCurrentlyMeditating
		{
			get
			{
				return Find.TickManager.TicksGame < this.lastMeditationTick + 10;
			}
		}

		// Token: 0x17001F6D RID: 8045
		// (get) Token: 0x0600CD46 RID: 52550 RVA: 0x003C2D44 File Offset: 0x003C0F44
		public float PsychicSensitivity
		{
			get
			{
				if (this.psychicSensitivityCachedTick != Find.TickManager.TicksGame)
				{
					this.psychicSensitivityCached = this.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1);
					this.psychicSensitivityCachedTick = Find.TickManager.TicksGame;
				}
				return this.psychicSensitivityCached;
			}
		}

		// Token: 0x17001F6E RID: 8046
		// (get) Token: 0x0600CD47 RID: 52551 RVA: 0x003C2D91 File Offset: 0x003C0F91
		public bool IsPsychicallySensitive
		{
			get
			{
				return this.PsychicSensitivity > float.Epsilon;
			}
		}

		// Token: 0x17001F6F RID: 8047
		// (get) Token: 0x0600CD48 RID: 52552 RVA: 0x003C2DA0 File Offset: 0x003C0FA0
		public float EntropyRelativeValue
		{
			get
			{
				return this.EntropyToRelativeValue(this.currentEntropy);
			}
		}

		// Token: 0x17001F70 RID: 8048
		// (get) Token: 0x0600CD49 RID: 52553 RVA: 0x003C2DB0 File Offset: 0x003C0FB0
		public PsychicEntropySeverity Severity
		{
			get
			{
				PsychicEntropySeverity result = PsychicEntropySeverity.Safe;
				foreach (PsychicEntropySeverity psychicEntropySeverity in Pawn_PsychicEntropyTracker.EntropyThresholds.Keys)
				{
					if (Pawn_PsychicEntropyTracker.EntropyThresholds[psychicEntropySeverity] >= this.EntropyRelativeValue)
					{
						break;
					}
					result = psychicEntropySeverity;
				}
				return result;
			}
		}

		// Token: 0x17001F71 RID: 8049
		// (get) Token: 0x0600CD4A RID: 52554 RVA: 0x003C2E18 File Offset: 0x003C1018
		public int PsyfocusBand
		{
			get
			{
				if (this.currentPsyfocus < Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[1])
				{
					return 0;
				}
				if (this.currentPsyfocus < Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[2])
				{
					return 1;
				}
				return 2;
			}
		}

		// Token: 0x17001F72 RID: 8050
		// (get) Token: 0x0600CD4B RID: 52555 RVA: 0x003C2E45 File Offset: 0x003C1045
		public Hediff_Psylink Psylink
		{
			get
			{
				if (this.psylinkCachedForTick != Find.TickManager.TicksGame)
				{
					this.psylinkCached = this.pawn.GetMainPsylinkSource();
					this.psylinkCachedForTick = Find.TickManager.TicksGame;
				}
				return this.psylinkCached;
			}
		}

		// Token: 0x17001F73 RID: 8051
		// (get) Token: 0x0600CD4C RID: 52556 RVA: 0x003C2E80 File Offset: 0x003C1080
		public bool NeedsPsyfocus
		{
			get
			{
				return this.Psylink != null && !this.pawn.Suspended && (this.pawn.Spawned || this.pawn.IsCaravanMember());
			}
		}

		// Token: 0x17001F74 RID: 8052
		// (get) Token: 0x0600CD4D RID: 52557 RVA: 0x003C2EB8 File Offset: 0x003C10B8
		private float PsyfocusFallPerDay
		{
			get
			{
				if (this.pawn.GetPsylinkLevel() == 0)
				{
					return 0f;
				}
				return Pawn_PsychicEntropyTracker.FallRatePerPsyfocusBand[this.PsyfocusBand];
			}
		}

		// Token: 0x0600CD4E RID: 52558 RVA: 0x003C2EE0 File Offset: 0x003C10E0
		public Pawn_PsychicEntropyTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CD4F RID: 52559 RVA: 0x003C2F2C File Offset: 0x003C112C
		public static void ResetStaticData()
		{
			Pawn_PsychicEntropyTracker.EntropyThresholdSounds = new Dictionary<PsychicEntropySeverity, SoundDef>
			{
				{
					PsychicEntropySeverity.Overloaded,
					SoundDefOf.PsychicEntropyOverloaded
				},
				{
					PsychicEntropySeverity.VeryOverloaded,
					SoundDefOf.PsychicEntropyHyperloaded
				},
				{
					PsychicEntropySeverity.Extreme,
					SoundDefOf.PsychicEntropyBrainCharring
				},
				{
					PsychicEntropySeverity.Overwhelming,
					SoundDefOf.PsychicEntropyBrainRoasting
				}
			};
		}

		// Token: 0x0600CD50 RID: 52560 RVA: 0x003C2F68 File Offset: 0x003C1168
		public void PsychicEntropyTrackerTickInterval(int delta)
		{
			if (this.currentEntropy > 1E-45f)
			{
				this.currentEntropy = Mathf.Max(this.currentEntropy - 1.TicksToSeconds() * this.RecoveryRate * (float)delta, 0f);
			}
			if (this.currentEntropy > 1E-45f && !this.pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicEntropy, false))
			{
				this.pawn.health.AddHediff(HediffDefOf.PsychicEntropy, null, null, null);
			}
			if (this.currentEntropy > 1E-45f)
			{
				if (this.ticksSinceLastMote >= Pawn_PsychicEntropyTracker.TicksBetweenMotes[(int)this.Severity])
				{
					if (this.pawn.Spawned)
					{
						FleckMaker.AttachedOverlay(this.pawn, FleckDefOf.EntropyPulse, Vector3.zero, 1f, -1f);
					}
					this.ticksSinceLastMote = 0;
				}
				else
				{
					this.ticksSinceLastMote += delta;
				}
			}
			else
			{
				this.ticksSinceLastMote = 0;
			}
			if (this.NeedsPsyfocus && this.pawn.IsHashIntervalTick(150, delta))
			{
				float num = 400f;
				if (!this.IsCurrentlyMeditating)
				{
					this.currentPsyfocus = Mathf.Clamp(this.currentPsyfocus - this.PsyfocusFallPerDay / num, 0f, 1f);
				}
				MeditationUtility.CheckMeditationScheduleTeachOpportunity(this.pawn);
			}
		}

		// Token: 0x0600CD51 RID: 52561 RVA: 0x003C30B9 File Offset: 0x003C12B9
		public bool WouldOverflowEntropy(float value)
		{
			return this.limitEntropyAmount && this.currentEntropy + value * this.pawn.GetStatValue(StatDefOf.PsychicEntropyGain, true, -1) > this.MaxEntropy;
		}

		// Token: 0x0600CD52 RID: 52562 RVA: 0x003C30E8 File Offset: 0x003C12E8
		public bool TryAddEntropy(float value, Thing source = null, bool scale = true, bool overLimit = false)
		{
			PsychicEntropySeverity severity = this.Severity;
			float num = scale ? (value * this.pawn.GetStatValue(StatDefOf.PsychicEntropyGain, true, -1)) : value;
			if (!this.WouldOverflowEntropy(num) || overLimit)
			{
				this.currentEntropy = Mathf.Max(this.currentEntropy + num, 0f);
				foreach (Hediff hediff in this.pawn.health.hediffSet.hediffs)
				{
					hediff.Notify_EntropyGained(value, num, source);
				}
				if (severity != this.Severity && num > 0f && this.Severity != PsychicEntropySeverity.Safe)
				{
					Pawn_PsychicEntropyTracker.EntropyThresholdSounds[this.Severity].PlayOneShot(new TargetInfo(this.pawn.Position, this.pawn.Map, false));
					if (severity < PsychicEntropySeverity.Overloaded && this.Severity >= PsychicEntropySeverity.Overloaded)
					{
						Messages.Message("MessageWentOverPsychicEntropyLimit".Translate(this.pawn.Named("PAWN")), this.pawn, MessageTypeDefOf.NegativeEvent, true);
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600CD53 RID: 52563 RVA: 0x003C3230 File Offset: 0x003C1430
		public void RemoveAllEntropy()
		{
			this.currentEntropy = 0f;
		}

		// Token: 0x0600CD54 RID: 52564 RVA: 0x003C323D File Offset: 0x003C143D
		[Obsolete]
		public void GainPsyfocus(Thing focus = null)
		{
			this.GainPsyfocus_NewTemp(1, focus);
		}

		// Token: 0x0600CD55 RID: 52565 RVA: 0x003C3248 File Offset: 0x003C1448
		public void GainPsyfocus_NewTemp(int delta, Thing focus = null)
		{
			this.currentPsyfocus = Mathf.Clamp(this.currentPsyfocus + MeditationUtility.PsyfocusGainPerTick(this.pawn, focus) * (float)delta, 0f, 1f);
			if (focus != null && !focus.Destroyed)
			{
				CompMeditationFocus compMeditationFocus = focus.TryGetComp<CompMeditationFocus>();
				if (compMeditationFocus == null)
				{
					return;
				}
				compMeditationFocus.Used(this.pawn);
			}
		}

		// Token: 0x0600CD56 RID: 52566 RVA: 0x003C32A1 File Offset: 0x003C14A1
		public void Notify_Meditated()
		{
			this.lastMeditationTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600CD57 RID: 52567 RVA: 0x003C32B3 File Offset: 0x003C14B3
		public void OffsetPsyfocusDirectly(float offset)
		{
			this.currentPsyfocus = Mathf.Clamp(this.currentPsyfocus + offset, 0f, 1f);
		}

		// Token: 0x0600CD58 RID: 52568 RVA: 0x003C32D2 File Offset: 0x003C14D2
		public void RechargePsyfocus()
		{
			this.currentPsyfocus = 1f;
		}

		// Token: 0x0600CD59 RID: 52569 RVA: 0x003C32DF File Offset: 0x003C14DF
		public void SetInitialPsyfocusLevel()
		{
			if (this.pawn.IsColonist && !this.pawn.IsQuestLodger())
			{
				this.currentPsyfocus = 0.75f;
				return;
			}
			this.currentPsyfocus = Rand.Range(0.5f, 0.7f);
		}

		// Token: 0x0600CD5A RID: 52570 RVA: 0x003C331C File Offset: 0x003C151C
		public void SetPsyfocusTarget(float val)
		{
			this.targetPsyfocus = Mathf.Clamp(val, 0f, 1f);
		}

		// Token: 0x0600CD5B RID: 52571 RVA: 0x003C3334 File Offset: 0x003C1534
		public float EntropyToRelativeValue(float val)
		{
			if (val < 1E-45f)
			{
				return 0f;
			}
			if (val < this.MaxEntropy)
			{
				if (this.MaxEntropy <= 1E-45f)
				{
					return 0f;
				}
				return val / this.MaxEntropy;
			}
			else
			{
				if (this.MaxPotentialEntropy <= 1E-45f)
				{
					return 0f;
				}
				return 1f + (val - this.MaxEntropy) / this.MaxPotentialEntropy;
			}
		}

		// Token: 0x0600CD5C RID: 52572 RVA: 0x003C339C File Offset: 0x003C159C
		public void Notify_GainedPsylink()
		{
			this.currentPsyfocus = Mathf.Max(this.currentPsyfocus, 0.75f);
		}

		// Token: 0x0600CD5D RID: 52573 RVA: 0x003C33B4 File Offset: 0x003C15B4
		public void Notify_PawnDied()
		{
			this.currentEntropy = 0f;
			this.currentPsyfocus = 0f;
		}

		// Token: 0x0600CD5E RID: 52574 RVA: 0x003C33CC File Offset: 0x003C15CC
		public bool NeedToShowGizmo()
		{
			return ModsConfig.RoyaltyActive && this.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) > float.Epsilon && (this.EntropyValue > float.Epsilon || this.pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicAmplifier, false));
		}

		// Token: 0x0600CD5F RID: 52575 RVA: 0x003C3427 File Offset: 0x003C1627
		public Gizmo GetGizmo()
		{
			if (this.gizmo == null)
			{
				this.gizmo = new PsychicEntropyGizmo(this);
			}
			return this.gizmo;
		}

		// Token: 0x0600CD60 RID: 52576 RVA: 0x003C3444 File Offset: 0x003C1644
		public string PsyfocusTipString(float psyfocusTargetOverride = -1f)
		{
			if (Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached == null)
			{
				for (int i = 0; i < Pawn_PsychicEntropyTracker.PsyfocusBandPercentages.Count - 1; i++)
				{
					Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached += "PsyfocusLevelInfoRange".Translate((Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[i] * 100f).ToStringDecimalIfSmall(), (Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[i + 1] * 100f).ToStringDecimalIfSmall()) + ": " + "PsyfocusLevelInfoPsycasts".Translate(Pawn_PsychicEntropyTracker.MaxAbilityLevelPerPsyfocusBand[i]) + "\n";
				}
				Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached += "\n";
				for (int j = 0; j < Pawn_PsychicEntropyTracker.PsyfocusBandPercentages.Count - 1; j++)
				{
					Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached += "PsyfocusLevelInfoRange".Translate((Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[j] * 100f).ToStringDecimalIfSmall(), (Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[j + 1] * 100f).ToStringDecimalIfSmall()) + ": " + "PsyfocusLevelInfoFallRate".Translate(Pawn_PsychicEntropyTracker.FallRatePerPsyfocusBand[j].ToStringPercent()) + "\n";
				}
			}
			return (("Psyfocus".Translate() + ": ").Colorize(ColoredText.TipSectionTitleColor) + this.currentPsyfocus.ToStringPercent("0.#") + "\n" + "DesiredPsyfocus".Translate() + ": " + ((psyfocusTargetOverride >= 0f) ? psyfocusTargetOverride : this.targetPsyfocus).ToStringPercent("0.#") + "\n\n" + "DesiredPsyfocusDesc".Translate(this.pawn.Named("PAWN")) + "\n\n" + "PsyfocusDesc".Translate() + ":\n\n" + Pawn_PsychicEntropyTracker.psyfocusLevelInfoCached).Resolve();
		}

		// Token: 0x0600CD61 RID: 52577 RVA: 0x003C3690 File Offset: 0x003C1890
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.currentEntropy, "currentEntropy", 0f, false);
			Scribe_Values.Look<float>(ref this.currentPsyfocus, "currentPsyfocus", -1f, false);
			Scribe_Values.Look<float>(ref this.targetPsyfocus, "targetPsyfocus", 0.5f, false);
			Scribe_Values.Look<int>(ref this.lastMeditationTick, "lastMeditationTick", -1, false);
			Scribe_Values.Look<bool>(ref this.limitEntropyAmount, "limitEntropyAmount", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.currentPsyfocus < 0f)
			{
				this.SetInitialPsyfocusLevel();
			}
		}

		// Token: 0x04008BBA RID: 35770
		private Pawn pawn;

		// Token: 0x04008BBB RID: 35771
		private float currentEntropy;

		// Token: 0x04008BBC RID: 35772
		private int ticksSinceLastMote;

		// Token: 0x04008BBD RID: 35773
		public bool limitEntropyAmount = true;

		// Token: 0x04008BBE RID: 35774
		private float currentPsyfocus = -1f;

		// Token: 0x04008BBF RID: 35775
		private float targetPsyfocus = 0.5f;

		// Token: 0x04008BC0 RID: 35776
		private int lastMeditationTick = -1;

		// Token: 0x04008BC1 RID: 35777
		private Gizmo gizmo;

		// Token: 0x04008BC2 RID: 35778
		private Hediff_Psylink psylinkCached;

		// Token: 0x04008BC3 RID: 35779
		private int psylinkCachedForTick = -1;

		// Token: 0x04008BC4 RID: 35780
		private static readonly int[] TicksBetweenMotes = new int[]
		{
			300,
			200,
			100,
			75,
			50
		};

		// Token: 0x04008BC5 RID: 35781
		public const float PercentageAfterGainingPsylink = 0.75f;

		// Token: 0x04008BC6 RID: 35782
		public const int PsyfocusUpdateInterval = 150;

		// Token: 0x04008BC7 RID: 35783
		public const float PsyfocusCostTolerance = 0.0005f;

		// Token: 0x04008BC8 RID: 35784
		public static readonly Dictionary<PsychicEntropySeverity, float> EntropyThresholds = new Dictionary<PsychicEntropySeverity, float>
		{
			{
				PsychicEntropySeverity.Safe,
				0f
			},
			{
				PsychicEntropySeverity.Overloaded,
				1f
			},
			{
				PsychicEntropySeverity.VeryOverloaded,
				1.33f
			},
			{
				PsychicEntropySeverity.Extreme,
				1.66f
			},
			{
				PsychicEntropySeverity.Overwhelming,
				2f
			}
		};

		// Token: 0x04008BC9 RID: 35785
		public static readonly List<float> PsyfocusBandPercentages = new List<float>
		{
			0f,
			0.25f,
			0.5f,
			1f
		};

		// Token: 0x04008BCA RID: 35786
		public static readonly List<float> FallRatePerPsyfocusBand = new List<float>
		{
			0.035f,
			0.055f,
			0.075f
		};

		// Token: 0x04008BCB RID: 35787
		public static readonly List<int> MaxAbilityLevelPerPsyfocusBand = new List<int>
		{
			2,
			4,
			6
		};

		// Token: 0x04008BCC RID: 35788
		public static Dictionary<PsychicEntropySeverity, SoundDef> EntropyThresholdSounds;

		// Token: 0x04008BCD RID: 35789
		private float psychicSensitivityCached;

		// Token: 0x04008BCE RID: 35790
		private int psychicSensitivityCachedTick = -1;

		// Token: 0x04008BCF RID: 35791
		public static string psyfocusLevelInfoCached = null;
	}
}
