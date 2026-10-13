using System;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x0200075B RID: 1883
	public class Hediff_MissingPart : HediffWithComps
	{
		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06002E1A RID: 11802 RVA: 0x000ED224 File Offset: 0x000EB424
		public override float SummaryHealthPercentImpact
		{
			get
			{
				if (!this.IsFreshNonSolidExtremity)
				{
					return 0f;
				}
				if (base.Part.def.tags.NullOrEmpty<BodyPartTagDef>() && base.Part.parts.NullOrEmpty<BodyPartRecord>() && !base.Bleeding)
				{
					return 0f;
				}
				return (float)base.Part.def.hitPoints / (75f * this.pawn.HealthScale);
			}
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06002E1B RID: 11803 RVA: 0x00002501 File Offset: 0x00000701
		public override bool ShouldRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06002E1C RID: 11804 RVA: 0x000ED29C File Offset: 0x000EB49C
		public override string LabelBase
		{
			get
			{
				if (this.lastInjury != null && this.lastInjury.injuryProps.useRemovedLabel)
				{
					return "RemovedBodyPart".Translate();
				}
				if (this.lastInjury != null && this.lastInjury.injuryProps.alwaysUseDestroyedLabel)
				{
					return this.lastInjury.injuryProps.destroyedLabel;
				}
				if (this.lastInjury == null || base.Part.depth == BodyPartDepth.Inside)
				{
					bool solid = base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs);
					return HealthUtility.GetGeneralDestroyedPartLabel(base.Part, this.IsFreshNonSolidExtremity, solid);
				}
				if (base.Part.def.socketed && !this.lastInjury.injuryProps.destroyedOutLabel.NullOrEmpty())
				{
					return this.lastInjury.injuryProps.destroyedOutLabel;
				}
				return this.lastInjury.injuryProps.destroyedLabel;
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06002E1D RID: 11805 RVA: 0x000ED39D File Offset: 0x000EB59D
		public override string Description
		{
			get
			{
				if (this.lastInjury != null && !string.IsNullOrEmpty(this.lastInjury.Description))
				{
					return this.lastInjury.Description;
				}
				return this.def.Description;
			}
		}

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06002E1E RID: 11806 RVA: 0x000ED3D0 File Offset: 0x000EB5D0
		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				if (this.IsFreshNonSolidExtremity)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append("FreshMissingBodyPart".Translate());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06002E1F RID: 11807 RVA: 0x000ED428 File Offset: 0x000EB628
		public override float BleedRate
		{
			get
			{
				if (this.pawn.Dead || !this.IsFreshNonSolidExtremity || this.ParentIsMissing)
				{
					return 0f;
				}
				if (!this.pawn.health.CanBleed)
				{
					return 0f;
				}
				return base.Part.def.GetMaxHealth(this.pawn) * this.def.injuryProps.bleedRate * base.Part.def.bleedRate * this.pawn.RaceProps.bleedRateFactor;
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06002E20 RID: 11808 RVA: 0x000ED4BC File Offset: 0x000EB6BC
		public override float PainOffset
		{
			get
			{
				if (this.pawn.Dead || this.causesNoPain || !this.IsFreshNonSolidExtremity || this.ParentIsMissing)
				{
					return 0f;
				}
				return base.Part.def.GetMaxHealth(this.pawn) * this.def.injuryProps.painPerSeverity / this.pawn.HealthScale;
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06002E21 RID: 11809 RVA: 0x000ED528 File Offset: 0x000EB728
		private bool ParentIsMissing
		{
			get
			{
				for (int i = 0; i < this.pawn.health.hediffSet.hediffs.Count; i++)
				{
					Hediff_MissingPart hediff_MissingPart = this.pawn.health.hediffSet.hediffs[i] as Hediff_MissingPart;
					if (hediff_MissingPart != null && hediff_MissingPart.Part == base.Part.parent)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06002E22 RID: 11810 RVA: 0x000ED594 File Offset: 0x000EB794
		// (set) Token: 0x06002E23 RID: 11811 RVA: 0x000ED5AC File Offset: 0x000EB7AC
		public bool IsFresh
		{
			get
			{
				return this.isFreshInt && !this.TicksAfterNoLongerFreshPassed;
			}
			set
			{
				if (this.isFreshInt == value)
				{
					return;
				}
				this.isFreshInt = value;
				this.pawn.Drawer.renderer.WoundOverlays.ClearCache();
				PortraitsCache.SetDirty(this.pawn);
				GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06002E24 RID: 11812 RVA: 0x000ED5FC File Offset: 0x000EB7FC
		public bool IsFreshNonSolidExtremity
		{
			get
			{
				return Current.ProgramState != ProgramState.Entry && this.IsFresh && base.Part.depth != BodyPartDepth.Inside && !base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs) && !this.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(base.Part.parent) && !this.ParentIsMissing;
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06002E25 RID: 11813 RVA: 0x000ED682 File Offset: 0x000EB882
		private bool TicksAfterNoLongerFreshPassed
		{
			get
			{
				return this.ageTicks >= 90000;
			}
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x000ED694 File Offset: 0x000EB894
		public override bool TendableNow(bool ignoreTimer = false)
		{
			return this.IsFreshNonSolidExtremity;
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x000ED69C File Offset: 0x000EB89C
		public override void TickInterval(int delta)
		{
			bool ticksAfterNoLongerFreshPassed = this.TicksAfterNoLongerFreshPassed;
			base.TickInterval(delta);
			bool ticksAfterNoLongerFreshPassed2 = this.TicksAfterNoLongerFreshPassed;
			if (ticksAfterNoLongerFreshPassed != ticksAfterNoLongerFreshPassed2)
			{
				this.pawn.health.Notify_HediffChanged(this);
			}
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x000ED6D1 File Offset: 0x000EB8D1
		public override void Tended(float quality, float maxQuality, int batchPosition = 0)
		{
			base.Tended(quality, maxQuality, batchPosition);
			this.IsFresh = false;
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x000ED6F4 File Offset: 0x000EB8F4
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (Current.ProgramState != ProgramState.Playing || PawnGenerator.IsBeingGenerated(this.pawn))
			{
				this.IsFresh = false;
			}
			this.pawn.health.RestorePart(base.Part, this, false);
			for (int i = 0; i < base.Part.parts.Count; i++)
			{
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(this.def, this.pawn, null);
				hediff_MissingPart.IsFresh = false;
				hediff_MissingPart.lastInjury = this.lastInjury;
				hediff_MissingPart.Part = base.Part.parts[i];
				this.pawn.health.hediffSet.AddDirect(hediff_MissingPart, null, null);
			}
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x000ED7BC File Offset: 0x000EB9BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<HediffDef>(ref this.lastInjury, "lastInjury");
			Scribe_Values.Look<bool>(ref this.isFreshInt, "isFresh", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error("Hediff_MissingPart has null part after loading.");
				this.pawn.health.hediffSet.hediffs.Remove(this);
				return;
			}
		}

		// Token: 0x0400244F RID: 9295
		public HediffDef lastInjury;

		// Token: 0x04002450 RID: 9296
		private bool isFreshInt;
	}
}
