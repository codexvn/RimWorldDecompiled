using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse;

public class JobDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__37 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		string IEnumerator<string>.Current
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
		public _003CConfigErrors_003Ed__37(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				JobDef jobDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)jobDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (jobDef.joySkill != null && jobDef.joyXpPerTick == 0f)
					{
						_003C_003E2__current = "funSkill is not null but funXpPerTick is zero";
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__37 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__37(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public Type driverClass;

	[MustTranslate]
	public string reportString = "Doing something.";

	public bool playerInterruptible = true;

	public bool forceCompleteBeforeNextJob;

	public CheckJobOverrideOnDamageMode checkOverrideOnDamage = CheckJobOverrideOnDamageMode.Always;

	public bool alwaysShowWeapon;

	public bool neverShowWeapon;

	public bool suspendable = true;

	public bool casualInterruptible = true;

	public bool allowOpportunisticPrefix;

	public bool collideWithPawns;

	public bool isIdle;

	public TaleDef taleOnCompletion;

	public bool neverFleeFromEnemies;

	public bool sleepCanInterrupt = true;

	public bool makeTargetPrisoner;

	public int waitAfterArriving;

	public bool carryThingAfterJob;

	public bool dropThingBeforeJob = true;

	public bool isCrawlingIfDowned = true;

	public bool alwaysShowReport;

	public bool abilityCasting;

	public bool tryStartFlying;

	public bool ifFlyingKeepFlying;

	public float overrideFlyChance = -1f;

	public bool displayAsAreaInFloatMenu = true;

	public bool ignoreFenceBlocked;

	public int joyDuration = 4000;

	public int joyMaxParticipants = 1;

	public float joyGainRate = 1f;

	public SkillDef joySkill;

	public float joyXpPerTick;

	public JoyKindDef joyKind;

	public Rot4 faceDir = Rot4.Invalid;

	public int learningDuration = 20000;

	public ReservationLayerDef containerReservationLayer;

	public bool boardingGravship;

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__37))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__37(-2)
		{
			_003C_003E4__this = this
		};
	}
}
