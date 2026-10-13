using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002291 RID: 8849
	public class CompAbilityEffect_StopMentalState : CompAbilityEffect
	{
		// Token: 0x17001D76 RID: 7542
		// (get) Token: 0x0600C0A4 RID: 49316 RVA: 0x0037C8F3 File Offset: 0x0037AAF3
		public new CompProperties_AbilityStopMentalState Props
		{
			get
			{
				return (CompProperties_AbilityStopMentalState)this.props;
			}
		}

		// Token: 0x17001D77 RID: 7543
		// (get) Token: 0x0600C0A5 RID: 49317 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool HideTargetPawnTooltip
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600C0A6 RID: 49318 RVA: 0x0037C900 File Offset: 0x0037AB00
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.CatatonicBreakdown, false);
			if (firstHediffOfDef != null)
			{
				pawn.health.RemoveHediff(firstHediffOfDef);
			}
			if (pawn != null)
			{
				MentalState mentalState = pawn.MentalState;
				if (mentalState == null)
				{
					return;
				}
				mentalState.RecoverFromState();
			}
		}

		// Token: 0x0600C0A7 RID: 49319 RVA: 0x0037369F File Offset: 0x0037189F
		public override bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Valid(target, false);
		}

		// Token: 0x0600C0A8 RID: 49320 RVA: 0x0037C958 File Offset: 0x0037AB58
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (!AbilityUtility.ValidateHasMentalState(pawn, throwMessages, this.parent))
				{
					return false;
				}
				if (pawn.MentalStateDef != null && this.Props.exceptions.Contains(pawn.MentalStateDef))
				{
					if (throwMessages)
					{
						Messages.Message("AbilityDoesntWorkOnMentalState".Translate(this.parent.def.label, pawn.MentalStateDef.label), pawn, MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
				float num = this.PsyfocusCostForTarget(target);
				if (num > this.parent.pawn.psychicEntropy.CurrentPsyfocus + 0.0005f)
				{
					Pawn pawn2 = this.parent.pawn;
					if (throwMessages)
					{
						TaggedString value = ("MentalBreakIntensity" + this.TargetMentalBreakIntensity(target).ToString()).Translate();
						Messages.Message("CommandPsycastNotEnoughPsyfocusForMentalBreak".Translate(num.ToStringPercent(), value, pawn2.psychicEntropy.CurrentPsyfocus.ToStringPercent("0.#"), this.parent.def.label.Named("PSYCASTNAME"), pawn2.Named("CASTERNAME")), pawn, MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C0A9 RID: 49321 RVA: 0x0037CAC4 File Offset: 0x0037ACC4
		public MentalBreakIntensity TargetMentalBreakIntensity(LocalTargetInfo target)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				MentalStateDef mentalStateDef = pawn.MentalStateDef;
				if (mentalStateDef != null)
				{
					List<MentalBreakDef> allDefsListForReading = DefDatabase<MentalBreakDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].mentalState == mentalStateDef)
						{
							return allDefsListForReading[i].intensity;
						}
					}
				}
				else if (pawn.health.hediffSet.HasHediff(HediffDefOf.CatatonicBreakdown, false))
				{
					return MentalBreakIntensity.Extreme;
				}
			}
			return MentalBreakIntensity.Minor;
		}

		// Token: 0x0600C0AA RID: 49322 RVA: 0x0037CB38 File Offset: 0x0037AD38
		public override float PsyfocusCostForTarget(LocalTargetInfo target)
		{
			switch (this.TargetMentalBreakIntensity(target))
			{
			case MentalBreakIntensity.Minor:
				return this.Props.psyfocusCostForMinor;
			case MentalBreakIntensity.Major:
				return this.Props.psyfocusCostForMajor;
			case MentalBreakIntensity.Extreme:
				return this.Props.psyfocusCostForExtreme;
			default:
				return 0f;
			}
		}

		// Token: 0x0600C0AB RID: 49323 RVA: 0x0037CB8C File Offset: 0x0037AD8C
		public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			if (target.Pawn != null && this.Valid(target, false))
			{
				return "AbilityPsyfocusCost".Translate() + ": " + this.PsyfocusCostForTarget(target).ToStringPercent("0.#");
			}
			return null;
		}
	}
}
