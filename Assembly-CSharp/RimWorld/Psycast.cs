using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class Psycast : Ability
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__8 : IEnumerable<Command>, IEnumerable, IEnumerator<Command>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Command _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Psycast _003C_003E4__this;

		Command IEnumerator<Command>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetGizmos_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Psycast psycast = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ModLister.RoyaltyInstalled)
				{
					return false;
				}
				if (psycast.gizmo == null)
				{
					psycast.gizmo = new Command_Psycast(psycast, psycast.pawn);
				}
				_003C_003E2__current = psycast.gizmo;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Command> IEnumerable<Command>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Command>)this).GetEnumerator();
		}
	}

	private Mote moteCast;

	private static float MoteCastFadeTime = 0.4f;

	private static float MoteCastScale = 1f;

	private static Vector3 MoteCastOffset = new Vector3(0f, 0f, 0.48f);

	public override AcceptanceReport CanCast
	{
		get
		{
			AcceptanceReport canCast = base.CanCast;
			if (!canCast)
			{
				return canCast;
			}
			if (def.EntropyGain > float.Epsilon)
			{
				if (pawn.GetPsylinkLevel() < def.level && def.level > 0)
				{
					return false;
				}
				return !pawn.psychicEntropy.WouldOverflowEntropy(def.EntropyGain);
			}
			if (def.PsyfocusCost > pawn.psychicEntropy.CurrentPsyfocus + 0.0005f)
			{
				return false;
			}
			return true;
		}
	}

	public Psycast(Pawn pawn)
		: base(pawn)
	{
	}

	public Psycast(Pawn pawn, AbilityDef def)
		: base(pawn, def)
	{
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__8))]
	public override IEnumerable<Command> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override bool Activate(LocalTargetInfo target, LocalTargetInfo dest)
	{
		if (!ModLister.CheckRoyalty("Psycast"))
		{
			return false;
		}
		if (def.EntropyGain > float.Epsilon && !pawn.psychicEntropy.TryAddEntropy(def.EntropyGain))
		{
			return false;
		}
		float num = FinalPsyfocusCost(target);
		if (num > float.Epsilon)
		{
			pawn.psychicEntropy.OffsetPsyfocusDirectly(0f - num);
		}
		if (def.showPsycastEffects)
		{
			if (base.EffectComps.Any((CompAbilityEffect c) => c.Props.psychic))
			{
				if (def.HasAreaOfEffect)
				{
					FleckMaker.Static(target.Cell, pawn.Map, FleckDefOf.PsycastAreaEffect, def.EffectRadius);
					SoundDefOf.PsycastPsychicPulse.PlayOneShot(new TargetInfo(target.Cell, pawn.Map));
				}
				else
				{
					SoundDefOf.PsycastPsychicEffect.PlayOneShot(new TargetInfo(target.Cell, pawn.Map));
				}
			}
			else if (def.HasAreaOfEffect && def.canUseAoeToGetTargets)
			{
				SoundDefOf.Psycast_Skip_Pulse.PlayOneShot(new TargetInfo(target.Cell, pawn.Map));
			}
		}
		return base.Activate(target, dest);
	}

	public override bool Activate(GlobalTargetInfo target)
	{
		if (def.EntropyGain > float.Epsilon && !pawn.psychicEntropy.TryAddEntropy(def.EntropyGain))
		{
			return false;
		}
		float psyfocusCost = def.PsyfocusCost;
		if (psyfocusCost > float.Epsilon)
		{
			pawn.psychicEntropy.OffsetPsyfocusDirectly(0f - psyfocusCost);
		}
		return base.Activate(target);
	}

	protected override void ApplyEffects(IEnumerable<CompAbilityEffect> effects, LocalTargetInfo target, LocalTargetInfo dest)
	{
		if (CanApplyPsycastTo(target))
		{
			foreach (CompAbilityEffect effect in effects)
			{
				effect.Apply(target, dest);
			}
			return;
		}
		MoteMaker.ThrowText(target.CenterVector3, pawn.Map, "TextMote_Immune".Translate());
	}

	public bool CanApplyPsycastTo(LocalTargetInfo target)
	{
		if (!base.EffectComps.Any((CompAbilityEffect e) => e.Props.psychic))
		{
			return true;
		}
		Pawn pawn = target.Pawn;
		if (pawn != null)
		{
			if (pawn.GetStatValue(StatDefOf.PsychicSensitivity) < float.Epsilon)
			{
				return false;
			}
			if (pawn.Faction != null && pawn.Faction == Faction.OfMechanoids && base.EffectComps.Any((CompAbilityEffect e) => !e.Props.applicableToMechs))
			{
				return false;
			}
		}
		return true;
	}

	public override bool GizmoDisabled(out string reason)
	{
		if (pawn.psychicEntropy.PsychicSensitivity < float.Epsilon)
		{
			reason = "CommandPsycastZeroPsychicSensitivity".Translate();
			return true;
		}
		float num = PsycastUtility.TotalPsyfocusCostOfQueuedPsycasts(pawn);
		if (def.level > 0 && pawn.GetPsylinkLevel() < def.level)
		{
			reason = "CommandPsycastHigherLevelPsylinkRequired".Translate(def.level);
			return true;
		}
		if (def.PsyfocusCost + num > pawn.psychicEntropy.CurrentPsyfocus + 0.0005f)
		{
			reason = "CommandPsycastNotEnoughPsyfocus".Translate(def.PsyfocusCostPercent, (pawn.psychicEntropy.CurrentPsyfocus - num).ToStringPercent("0.#"), def.label.Named("PSYCASTNAME"), pawn.Named("CASTERNAME"));
			return true;
		}
		if (def.level > pawn.psychicEntropy.MaxAbilityLevel)
		{
			reason = "CommandPsycastLowPsyfocus".Translate(Pawn_PsychicEntropyTracker.PsyfocusBandPercentages[def.RequiredPsyfocusBand].ToStringPercent());
			return true;
		}
		if (def.EntropyGain > float.Epsilon && pawn.psychicEntropy.WouldOverflowEntropy(def.EntropyGain + PsycastUtility.TotalEntropyFromQueuedPsycasts(pawn)))
		{
			reason = "CommandPsycastWouldExceedEntropy".Translate(def.label);
			return true;
		}
		return base.GizmoDisabled(out reason);
	}

	public override void AbilityTick()
	{
		base.AbilityTick();
		if (pawn.Spawned && base.Casting)
		{
			if (moteCast == null || moteCast.Destroyed)
			{
				moteCast = MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_CastPsycast, MoteCastOffset, MoteCastScale, base.verb.verbProps.warmupTime - MoteCastFadeTime);
			}
			else
			{
				moteCast.Maintain();
			}
		}
	}
}
