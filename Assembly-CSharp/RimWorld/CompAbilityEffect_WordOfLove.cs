using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020022AF RID: 8879
	public class CompAbilityEffect_WordOfLove : CompAbilityEffect_WithDest
	{
		// Token: 0x17001D91 RID: 7569
		// (get) Token: 0x0600C129 RID: 49449 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D92 RID: 7570
		// (get) Token: 0x0600C12A RID: 49450 RVA: 0x0037EE12 File Offset: 0x0037D012
		public override TargetingParameters targetParams
		{
			get
			{
				return new TargetingParameters
				{
					canTargetSelf = true,
					canTargetBuildings = false,
					canTargetAnimals = false,
					canTargetMechs = false
				};
			}
		}

		// Token: 0x0600C12B RID: 49451 RVA: 0x0037EE38 File Offset: 0x0037D038
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicLove, false);
			if (firstHediffOfDef != null)
			{
				pawn.health.RemoveHediff(firstHediffOfDef);
			}
			Hediff_PsychicLove hediff_PsychicLove = (Hediff_PsychicLove)HediffMaker.MakeHediff(HediffDefOf.PsychicLove, pawn, pawn.health.hediffSet.GetBrain());
			hediff_PsychicLove.target = dest.Thing;
			HediffComp_Disappears hediffComp_Disappears = hediff_PsychicLove.TryGetComp<HediffComp_Disappears>();
			if (hediffComp_Disappears != null)
			{
				float num = this.parent.def.EffectDuration(this.parent.pawn);
				num *= pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1);
				hediffComp_Disappears.ticksToDisappear = num.SecondsToTicks();
			}
			pawn.health.AddHediff(hediff_PsychicLove, null, null, null);
		}

		// Token: 0x0600C12C RID: 49452 RVA: 0x0037369F File Offset: 0x0037189F
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x0600C12D RID: 49453 RVA: 0x0037EF08 File Offset: 0x0037D108
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			Pawn pawn = this.selectedTarget.Pawn;
			Pawn pawn2 = target.Pawn;
			if (pawn == pawn2)
			{
				return false;
			}
			if (pawn.ageTracker.AgeBiologicalYearsFloat < 16f)
			{
				Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityCantApplyTooYoung".Translate(pawn), pawn, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (pawn2.ageTracker.AgeBiologicalYearsFloat < 16f)
			{
				Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityCantApplyTooYoung".Translate(pawn2), pawn2, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (pawn != null && pawn2 != null && !pawn.story.traits.HasTrait(TraitDefOf.Bisexual))
			{
				Gender gender = pawn.gender;
				Gender gender2 = pawn.story.traits.HasTrait(TraitDefOf.Gay) ? gender : gender.Opposite();
				if (pawn2.gender != gender2)
				{
					Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityCantApplyWrongAttractionGender".Translate(pawn, pawn2), pawn, MessageTypeDefOf.RejectInput, false);
					return false;
				}
			}
			return base.ValidateTarget(target, true);
		}

		// Token: 0x0600C12E RID: 49454 RVA: 0x0037F0BC File Offset: 0x0037D2BC
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (pawn.story.traits.HasTrait(TraitDefOf.Asexual))
				{
					if (throwMessages)
					{
						Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityCantApplyOnAsexual".Translate(), pawn, MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
				if (!AbilityUtility.ValidateNoMentalState(pawn, throwMessages, this.parent))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C12F RID: 49455 RVA: 0x0037F150 File Offset: 0x0037D350
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			if (this.selectedTarget.IsValid)
			{
				return "PsychicLoveFor".Translate();
			}
			return "PsychicLoveInduceIn".Translate();
		}

		// Token: 0x0400841F RID: 33823
		private const int MinAge = 16;
	}
}
