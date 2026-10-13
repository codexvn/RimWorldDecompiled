using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F5F RID: 12127
	public class CompUseEffect_GainAbility : CompUseEffect
	{
		// Token: 0x17002C33 RID: 11315
		// (get) Token: 0x06011178 RID: 70008 RVA: 0x004E862B File Offset: 0x004E682B
		public CompProperties_UseEffect_GainAbility Props
		{
			get
			{
				return (CompProperties_UseEffect_GainAbility)this.props;
			}
		}

		// Token: 0x06011179 RID: 70009 RVA: 0x004E8638 File Offset: 0x004E6838
		public override void DoEffect(Pawn user)
		{
			base.DoEffect(user);
			user.abilities.GainAbility(this.Props.ability);
			if (PawnUtility.ShouldSendNotificationAbout(user))
			{
				Messages.Message("AbilityNeurotrainerUsed".Translate(user.Named("USER"), this.Props.ability.LabelCap), user, MessageTypeDefOf.PositiveEvent, true);
			}
		}

		// Token: 0x0601117A RID: 70010 RVA: 0x004E86AC File Offset: 0x004E68AC
		public override AcceptanceReport CanBeUsedBy(Pawn p)
		{
			if (!p.health.hediffSet.HasHediff(HediffDefOf.PsychicAmplifier, false))
			{
				return "PsycastNeurotrainerNoPsylink".TranslateWithBackup("PsycastNeurotrainerNoPsychicAmplifier");
			}
			if (p.abilities != null && p.abilities.abilities.Any((Ability a) => a.def == this.Props.ability))
			{
				return "PsycastNeurotrainerAbilityAlreadyLearned".Translate(p.Named("USER"), this.Props.ability.LabelCap);
			}
			return base.CanBeUsedBy(p);
		}

		// Token: 0x0601117B RID: 70011 RVA: 0x004E8748 File Offset: 0x004E6948
		public override TaggedString ConfirmMessage(Pawn p)
		{
			if (!this.Props.ability.IsPsycast)
			{
				return null;
			}
			Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicAmplifier, false);
			if (firstHediffOfDef == null)
			{
				return null;
			}
			if (this.Props.ability.level > ((Hediff_Level)firstHediffOfDef).level)
			{
				return "PsylinkTooLowForGainAbility".Translate(p.Named("PAWN"), this.Props.ability.label.Named("ABILITY"));
			}
			return null;
		}

		// Token: 0x0601117C RID: 70012 RVA: 0x004E87E4 File Offset: 0x004E69E4
		public override bool AllowStackWith(Thing other)
		{
			if (!base.AllowStackWith(other))
			{
				return false;
			}
			CompUseEffect_GainAbility compUseEffect_GainAbility = other.TryGetComp<CompUseEffect_GainAbility>();
			return compUseEffect_GainAbility != null && compUseEffect_GainAbility.Props.ability == this.Props.ability;
		}
	}
}
