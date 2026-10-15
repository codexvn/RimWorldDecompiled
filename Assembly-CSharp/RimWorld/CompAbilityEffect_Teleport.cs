using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class CompAbilityEffect_Teleport : CompAbilityEffect_WithDest
{
	[CompilerGenerated]
	private sealed class _003CGetPreCastActions_003Ed__3 : IEnumerable<PreCastAction>, IEnumerable, IEnumerator<PreCastAction>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private PreCastAction _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompAbilityEffect_Teleport _003C_003E4__this;

		PreCastAction IEnumerator<PreCastAction>.Current
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
		public _003CGetPreCastActions_003Ed__3(int _003C_003E1__state)
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
			CompAbilityEffect_Teleport CS_0024_003C_003E8__locals8 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new PreCastAction
				{
					action = delegate(LocalTargetInfo t, LocalTargetInfo d)
					{
						if (!CS_0024_003C_003E8__locals8.parent.def.HasAreaOfEffect)
						{
							Pawn pawn = t.Pawn;
							if (pawn != null)
							{
								FleckCreationData dataAttachedOverlay = FleckMaker.GetDataAttachedOverlay(pawn, FleckDefOf.PsycastSkipFlashEntry, new Vector3(-0.5f, 0f, -0.5f));
								dataAttachedOverlay.link.detachAfterTicks = 5;
								pawn.Map.flecks.CreateFleck(dataAttachedOverlay);
							}
							else
							{
								FleckMaker.Static(t.CenterVector3, CS_0024_003C_003E8__locals8.parent.pawn.Map, FleckDefOf.PsycastSkipFlashEntry);
							}
							FleckMaker.Static(d.Cell, CS_0024_003C_003E8__locals8.parent.pawn.Map, FleckDefOf.PsycastSkipInnerExit);
						}
						if (CS_0024_003C_003E8__locals8.Props.destination != AbilityEffectDestination.RandomInRange)
						{
							FleckMaker.Static(d.Cell, CS_0024_003C_003E8__locals8.parent.pawn.Map, FleckDefOf.PsycastSkipOuterRingExit);
						}
						if (!CS_0024_003C_003E8__locals8.parent.def.HasAreaOfEffect)
						{
							SoundDefOf.Psycast_Skip_Entry.PlayOneShot(new TargetInfo(t.Cell, CS_0024_003C_003E8__locals8.parent.pawn.Map));
							SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(d.Cell, CS_0024_003C_003E8__locals8.parent.pawn.Map));
						}
					},
					ticksAwayFromCast = 5
				};
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
		IEnumerator<PreCastAction> IEnumerable<PreCastAction>.GetEnumerator()
		{
			_003CGetPreCastActions_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetPreCastActions_003Ed__3(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<PreCastAction>)this).GetEnumerator();
		}
	}

	public static string SkipUsedSignalTag = "CompAbilityEffect.SkipUsed";

	public new CompProperties_AbilityTeleport Props => (CompProperties_AbilityTeleport)props;

	[IteratorStateMachine(typeof(_003CGetPreCastActions_003Ed__3))]
	public override IEnumerable<PreCastAction> GetPreCastActions()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPreCastActions_003Ed__3(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
	{
		if (!target.HasThing)
		{
			return;
		}
		base.Apply(target, dest);
		LocalTargetInfo destination = GetDestination(dest.IsValid ? dest : target);
		if (!destination.IsValid)
		{
			return;
		}
		Pawn pawn = parent.pawn;
		if (!parent.def.HasAreaOfEffect)
		{
			parent.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(target.Thing, pawn.Map), target.Thing.Position, 60);
		}
		else
		{
			parent.AddEffecterToMaintain(EffecterDefOf.Skip_EntryNoDelay.Spawn(target.Thing, pawn.Map), target.Thing.Position, 60);
		}
		if (Props.destination == AbilityEffectDestination.Selected)
		{
			parent.AddEffecterToMaintain(EffecterDefOf.Skip_Exit.Spawn(destination.Cell, pawn.Map), destination.Cell, 60);
		}
		else
		{
			parent.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(destination.Cell, pawn.Map), destination.Cell, 60);
		}
		target.Thing.TryGetComp<CompCanBeDormant>()?.WakeUp();
		target.Thing.Position = destination.Cell;
		if (target.Thing is Pawn pawn2)
		{
			if ((pawn2.Faction == Faction.OfPlayer || pawn2.IsPlayerControlled) && pawn2.Position.Fogged(pawn2.Map))
			{
				FloodFillerFog.FloodUnfog(pawn2.Position, pawn2.Map);
			}
			pawn2.stances.stunner.StunFor(Props.stunTicks.RandomInRange, parent.pawn, addBattleLog: false, showMote: false);
			pawn2.Notify_Teleported();
			SendSkipUsedSignal(pawn2.Position, pawn2);
		}
		if (Props.destClamorType != null)
		{
			GenClamor.DoClamor(pawn, target.Cell, Props.destClamorRadius, Props.destClamorType);
		}
	}

	public override bool CanHitTarget(LocalTargetInfo target)
	{
		if (!CanPlaceSelectedTargetAt(target))
		{
			return false;
		}
		return base.CanHitTarget(target);
	}

	public override bool Valid(LocalTargetInfo target, bool showMessages = true)
	{
		AcceptanceReport acceptanceReport = CanSkipTarget(target);
		if (!acceptanceReport)
		{
			if (showMessages && !acceptanceReport.Reason.NullOrEmpty() && target.Thing is Pawn pawn)
			{
				Messages.Message("CannotSkipTarget".Translate(pawn.Named("PAWN")) + ": " + acceptanceReport.Reason, pawn, MessageTypeDefOf.RejectInput, historical: false);
			}
			return false;
		}
		return base.Valid(target, showMessages);
	}

	private AcceptanceReport CanSkipTarget(LocalTargetInfo target)
	{
		if (target.Thing is Pawn pawn)
		{
			if (pawn.BodySize > Props.maxBodySize)
			{
				return "CannotSkipTargetTooLarge".Translate();
			}
			if (pawn.kindDef.skipResistant)
			{
				return "CannotSkipTargetPsychicResistant".Translate();
			}
		}
		return true;
	}

	public override string ExtraLabelMouseAttachment(LocalTargetInfo target)
	{
		return CanSkipTarget(target).Reason;
	}

	public static void SendSkipUsedSignal(LocalTargetInfo target, Thing initiator)
	{
		Find.SignalManager.SendSignal(new Signal(SkipUsedSignalTag, target.Named("POSITION"), initiator.Named("SUBJECT")));
	}
}
