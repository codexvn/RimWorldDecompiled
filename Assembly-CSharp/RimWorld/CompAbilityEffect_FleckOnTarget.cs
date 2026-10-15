using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class CompAbilityEffect_FleckOnTarget : CompAbilityEffect
{
	[CompilerGenerated]
	private sealed class _003CGetPreCastActions_003Ed__3 : IEnumerable<PreCastAction>, IEnumerable, IEnumerator<PreCastAction>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private PreCastAction _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompAbilityEffect_FleckOnTarget _003C_003E4__this;

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
			CompAbilityEffect_FleckOnTarget CS_0024_003C_003E8__locals5 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals5.Props.preCastTicks > 0)
				{
					_003C_003E2__current = new PreCastAction
					{
						action = delegate(LocalTargetInfo t, LocalTargetInfo d)
						{
							CS_0024_003C_003E8__locals5.SpawnAll(t);
							CS_0024_003C_003E8__locals5.Props.sound?.PlayOneShot(new TargetInfo(t.Cell, CS_0024_003C_003E8__locals5.parent.pawn.Map));
						},
						ticksAwayFromCast = CS_0024_003C_003E8__locals5.Props.preCastTicks
					};
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			return false;
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

	public new CompProperties_AbilityFleckOnTarget Props => (CompProperties_AbilityFleckOnTarget)props;

	public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
	{
		if (Props.preCastTicks <= 0)
		{
			Props.sound?.PlayOneShot(new TargetInfo(target.Cell, parent.pawn.Map));
			SpawnAll(target);
		}
	}

	[IteratorStateMachine(typeof(_003CGetPreCastActions_003Ed__3))]
	public override IEnumerable<PreCastAction> GetPreCastActions()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPreCastActions_003Ed__3(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void SpawnAll(LocalTargetInfo target)
	{
		if (!Props.fleckDefs.NullOrEmpty())
		{
			for (int i = 0; i < Props.fleckDefs.Count; i++)
			{
				SpawnFleck(target, Props.fleckDefs[i]);
			}
		}
		else
		{
			SpawnFleck(target, Props.fleckDef);
		}
	}

	private void SpawnFleck(LocalTargetInfo target, FleckDef def)
	{
		if (target.HasThing && target.Thing.SpawnedOrAnyParentSpawned)
		{
			FleckMaker.AttachedOverlay(target.Thing, def, Vector3.zero, Props.scale);
		}
		else
		{
			FleckMaker.Static(target.Cell, parent.pawn.Map, def, Props.scale);
		}
	}
}
