using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020022B7 RID: 8887
	public class Psycast : Ability
	{
		// Token: 0x17001D98 RID: 7576
		// (get) Token: 0x0600C146 RID: 49478 RVA: 0x0037F5D8 File Offset: 0x0037D7D8
		public override AcceptanceReport CanCast
		{
			get
			{
				AcceptanceReport canCast = base.CanCast;
				if (!canCast)
				{
					return canCast;
				}
				if (this.def.EntropyGain > 1E-45f)
				{
					if (this.pawn.GetPsylinkLevel() < this.def.level && this.def.level > 0)
					{
						return false;
					}
					return !this.pawn.psychicEntropy.WouldOverflowEntropy(this.def.EntropyGain);
				}
				else
				{
					if (this.def.PsyfocusCost > this.pawn.psychicEntropy.CurrentPsyfocus + 0.0005f)
					{
						return false;
					}
					return true;
				}
			}
		}

		// Token: 0x0600C147 RID: 49479 RVA: 0x003723E8 File Offset: 0x003705E8
		public Psycast(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x0600C148 RID: 49480 RVA: 0x003723F1 File Offset: 0x003705F1
		public Psycast(Pawn pawn, AbilityDef def) : base(pawn, def)
		{
		}

		// Token: 0x0600C149 RID: 49481 RVA: 0x0037F689 File Offset: 0x0037D889
		public override IEnumerable<Command> GetGizmos()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				yield break;
			}
			if (this.gizmo == null)
			{
				this.gizmo = new Command_Psycast(this, this.pawn);
			}
			yield return this.gizmo;
			yield break;
		}

		// Token: 0x0600C14A RID: 49482 RVA: 0x0037F69C File Offset: 0x0037D89C
		public override bool Activate(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (!ModLister.CheckRoyalty("Psycast"))
			{
				return false;
			}
			if (this.def.EntropyGain > 1E-45f && !this.pawn.psychicEntropy.TryAddEntropy(this.def.EntropyGain, null, true, false))
			{
				return false;
			}
			float num = base.FinalPsyfocusCost(target);
			if (num > 1E-45f)
			{
				this.pawn.psychicEntropy.OffsetPsyfocusDirectly(-num);
			}
			if (this.def.showPsycastEffects)
			{
				if (base.EffectComps.Any((CompAbilityEffect c) => c.Props.psychic))
				{
					if (this.def.HasAreaOfEffect)
					{
						FleckMaker.Static(target.Cell, this.pawn.Map, FleckDefOf.PsycastAreaEffect, this.def.EffectRadius);
						SoundDefOf.PsycastPsychicPulse.PlayOneShot(new TargetInfo(target.Cell, this.pawn.Map, false));
					}
					else
					{
						SoundDefOf.PsycastPsychicEffect.PlayOneShot(new TargetInfo(target.Cell, this.pawn.Map, false));
					}
				}
				else if (this.def.HasAreaOfEffect && this.def.canUseAoeToGetTargets)
				{
					SoundDefOf.Psycast_Skip_Pulse.PlayOneShot(new TargetInfo(target.Cell, this.pawn.Map, false));
				}
			}
			return base.Activate(target, dest);
		}

		// Token: 0x0600C14B RID: 49483 RVA: 0x0037F81C File Offset: 0x0037DA1C
		public override bool Activate(GlobalTargetInfo target)
		{
			if (this.def.EntropyGain > 1E-45f && !this.pawn.psychicEntropy.TryAddEntropy(this.def.EntropyGain, null, true, false))
			{
				return false;
			}
			float psyfocusCost = this.def.PsyfocusCost;
			if (psyfocusCost > 1E-45f)
			{
				this.pawn.psychicEntropy.OffsetPsyfocusDirectly(-psyfocusCost);
			}
			return base.Activate(target);
		}

		// Token: 0x0600C14C RID: 49484 RVA: 0x0037F88C File Offset: 0x0037DA8C
		protected override void ApplyEffects(IEnumerable<CompAbilityEffect> effects, LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (this.CanApplyPsycastTo(target))
			{
				using (IEnumerator<CompAbilityEffect> enumerator = effects.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CompAbilityEffect compAbilityEffect = enumerator.Current;
						compAbilityEffect.Apply(target, dest);
					}
					return;
				}
			}
			MoteMaker.ThrowText(target.CenterVector3, this.pawn.Map, "TextMote_Immune".Translate(), -1f);
		}

		// Token: 0x0600C14D RID: 49485 RVA: 0x0037F908 File Offset: 0x0037DB08
		public bool CanApplyPsycastTo(LocalTargetInfo target)
		{
			if (!base.EffectComps.Any((CompAbilityEffect e) => e.Props.psychic))
			{
				return true;
			}
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) < 1E-45f)
				{
					return false;
				}
				if (pawn.Faction != null && pawn.Faction == Faction.OfMechanoids)
				{
					if (base.EffectComps.Any((CompAbilityEffect e) => !e.Props.applicableToMechs))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600C14E RID: 49486 RVA: 0x0037F9A8 File Offset: 0x0037DBA8
		public override bool GizmoDisabled(out string reason)
		{
			if (this.pawn.psychicEntropy.PsychicSensitivity < 1E-45f)
			{
				reason = "CommandPsycastZeroPsychicSensitivity".Translate();
				return true;
			}
			float num = PsycastUtility.TotalPsyfocusCostOfQueuedPsycasts(this.pawn);
			if (this.def.level > 0 && this.pawn.GetPsylinkLevel() < this.def.level)
			{
				reason = "CommandPsycastHigherLevelPsylinkRequired".Translate(this.def.level);
				return true;
			}
			if (this.def.PsyfocusCost + num > this.pawn.psychicEntropy.CurrentPsyfocus + 0.0005f)
			{
				reason = "CommandPsycastNotEnoughPsyfocus".Translate(this.def.PsyfocusCostPercent, (this.pawn.psychicEntropy.CurrentPsyfocus - num).ToStringPercent("0.#"), this.def.label.Named("PSYCASTNAME"), this.pawn.Named("CASTERNAME"));
				return true;
			}
			if (this.def.level > this.pawn.psychicEntropy.MaxAbilityLevel)
			{
				reason = "CommandPsycastLowPsyfocus".Translate(Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[this.def.RequiredPsyfocusBand].ToStringPercent());
				return true;
			}
			if (this.def.EntropyGain > 1E-45f && this.pawn.psychicEntropy.WouldOverflowEntropy(this.def.EntropyGain + PsycastUtility.TotalEntropyFromQueuedPsycasts(this.pawn)))
			{
				reason = "CommandPsycastWouldExceedEntropy".Translate(this.def.label);
				return true;
			}
			return base.GizmoDisabled(out reason);
		}

		// Token: 0x0600C14F RID: 49487 RVA: 0x0037FB78 File Offset: 0x0037DD78
		public override void AbilityTick()
		{
			base.AbilityTick();
			if (this.pawn.Spawned && base.Casting)
			{
				if (this.moteCast == null || this.moteCast.Destroyed)
				{
					this.moteCast = MoteMaker.MakeAttachedOverlay(this.pawn, ThingDefOf.Mote_CastPsycast, Psycast.MoteCastOffset, Psycast.MoteCastScale, base.verb.verbProps.warmupTime - Psycast.MoteCastFadeTime);
					return;
				}
				this.moteCast.Maintain();
			}
		}

		// Token: 0x04008430 RID: 33840
		private Mote moteCast;

		// Token: 0x04008431 RID: 33841
		private static float MoteCastFadeTime = 0.4f;

		// Token: 0x04008432 RID: 33842
		private static float MoteCastScale = 1f;

		// Token: 0x04008433 RID: 33843
		private static Vector3 MoteCastOffset = new Vector3(0f, 0f, 0.48f);
	}
}
