using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public class EnterCooldownComp : WorldObjectComp
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__16 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public EnterCooldownComp _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CGetGizmos_003Ed__16(int _003C_003E1__state)
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
			EnterCooldownComp CS_0024_003C_003E8__locals2 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (DebugSettings.ShowDevGizmos)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Set enter cooldown to 1 hour",
						action = delegate
						{
							CS_0024_003C_003E8__locals2.endTick = GenTicks.TicksGame + 2500;
						}
					};
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Reset enter cooldown",
					action = delegate
					{
						CS_0024_003C_003E8__locals2.endTick = GenTicks.TicksGame;
					}
				};
				_003C_003E1__state = 2;
				return true;
			case 2:
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__16 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__16(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private int endTick;

	public WorldObjectCompProperties_EnterCooldown Props => (WorldObjectCompProperties_EnterCooldown)props;

	public bool Active => endTick > GenTicks.TicksGame;

	public bool BlocksEntering
	{
		get
		{
			if (Active)
			{
				return !base.ParentHasMap;
			}
			return false;
		}
	}

	public int TicksLeft
	{
		get
		{
			if (!Active)
			{
				return 0;
			}
			return endTick - GenTicks.TicksGame;
		}
	}

	public float DaysLeft => (float)TicksLeft / 60000f;

	public void Start(float? durationDays = null)
	{
		float num = durationDays ?? Props.durationDays;
		endTick = GenTicks.TicksGame + Mathf.RoundToInt(num * 60000f);
	}

	public void Stop()
	{
		endTick = 0;
	}

	public override void PostMapGenerate()
	{
		base.PostMapGenerate();
		if (Active)
		{
			Stop();
		}
	}

	public override void PostMyMapRemoved()
	{
		base.PostMyMapRemoved();
		if (Props.autoStartOnMapRemoved)
		{
			Start(null);
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		int value = 0;
		Scribe_Values.Look(ref value, "ticksLeft", 0);
		Scribe_Values.Look(ref endTick, "endTick", 0);
		if (Scribe.mode == LoadSaveMode.PostLoadInit && value > 0 && endTick == 0)
		{
			endTick = GenTicks.TicksGame + value;
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__16))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__16(-2)
		{
			_003C_003E4__this = this
		};
	}
}
