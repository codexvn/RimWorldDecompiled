using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200223F RID: 8767
	public class CompAbilityEffect_GiveMentalState : CompAbilityEffect
	{
		// Token: 0x17001D3B RID: 7483
		// (get) Token: 0x0600BF75 RID: 49013 RVA: 0x0037822A File Offset: 0x0037642A
		public new CompProperties_AbilityGiveMentalState Props
		{
			get
			{
				return (CompProperties_AbilityGiveMentalState)this.props;
			}
		}

		// Token: 0x0600BF76 RID: 49014 RVA: 0x00378238 File Offset: 0x00376438
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = this.Props.applyToSelf ? this.parent.pawn : (target.Thing as Pawn);
			if (pawn != null && !pawn.InMentalState)
			{
				CompAbilityEffect_GiveMentalState.TryGiveMentalState(pawn.RaceProps.IsMechanoid ? (this.Props.stateDefForMechs ?? this.Props.stateDef) : this.Props.stateDef, pawn, this.parent.def, this.Props.durationMultiplier, this.parent.pawn, this.Props.forced);
				RestUtility.WakeUp(pawn, true);
				if (this.Props.casterEffect != null)
				{
					Effecter effecter = this.Props.casterEffect.SpawnAttached(this.parent.pawn, this.parent.pawn.MapHeld, 1f);
					effecter.Trigger(this.parent.pawn, null, -1);
					effecter.Cleanup();
				}
				if (this.Props.targetEffect != null)
				{
					Effecter effecter2 = this.Props.targetEffect.SpawnAttached(this.parent.pawn, this.parent.pawn.MapHeld, 1f);
					effecter2.Trigger(pawn, null, -1);
					effecter2.Cleanup();
				}
			}
		}

		// Token: 0x0600BF77 RID: 49015 RVA: 0x003783A8 File Offset: 0x003765A8
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (!base.Valid(target, throwMessages))
			{
				return false;
			}
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (!AbilityUtility.ValidateNoMentalState(pawn, throwMessages, this.parent))
				{
					return false;
				}
				if (this.Props.excludeNPCFactions && pawn.Faction != null && !pawn.Faction.IsPlayer)
				{
					if (throwMessages)
					{
						Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "TargetBelongsToNPCFaction".Translate(), pawn, MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600BF78 RID: 49016 RVA: 0x00378458 File Offset: 0x00376658
		public static void TryGiveMentalState(MentalStateDef def, Pawn p, AbilityDef ability, StatDef multiplierStat, Pawn caster, bool forced = false)
		{
			if (p.mindState.mentalStateHandler.TryStartMentalState(def, null, forced, true, false, caster, false, false, ability.IsPsycast))
			{
				float num = ability.GetStatValueAbstract(StatDefOf.Ability_Duration, caster);
				if (multiplierStat != null)
				{
					num *= p.GetStatValue(multiplierStat, true, -1);
				}
				if (num > 0f)
				{
					p.mindState.mentalStateHandler.CurState.forceRecoverAfterTicks = num.SecondsToTicks();
				}
				p.mindState.mentalStateHandler.CurState.sourceFaction = caster.Faction;
			}
		}
	}
}
