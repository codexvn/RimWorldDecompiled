using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Dance : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__8 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Dance _003C_003E4__this;

		Toil IEnumerator<Toil>.Current
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
		public _003CMakeNewToils_003Ed__8(int _003C_003E1__state)
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
			JobDriver_Dance CS_0024_003C_003E8__locals8 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (!ModLister.CheckIdeology("Dance job"))
				{
					return false;
				}
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				CS_0024_003C_003E8__locals8.jumping = Rand.Bool;
				toil.tickIntervalAction = delegate
				{
					if (CS_0024_003C_003E8__locals8.AgeTicks % CS_0024_003C_003E8__locals8.moveChangeInterval == 0)
					{
						CS_0024_003C_003E8__locals8.jumping = !CS_0024_003C_003E8__locals8.jumping;
					}
					if (CS_0024_003C_003E8__locals8.AgeTicks % 120 == 0 && !CS_0024_003C_003E8__locals8.jumping)
					{
						CS_0024_003C_003E8__locals8.pawn.Rotation = Rot4.Random;
					}
				};
				toil.socialMode = RandomSocialMode.SuperActive;
				toil.defaultCompleteMode = ToilCompleteMode.Never;
				toil.handlingFacing = true;
				_003C_003E2__current = toil;
				_003C_003E1__state = 1;
				return true;
			}
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
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	private bool jumping;

	private int moveChangeInterval = 240;

	public int AgeTicks => Find.TickManager.TicksGame - startTick;

	public override Vector3 ForcedBodyOffset
	{
		get
		{
			float num = Mathf.Sin((float)AgeTicks / 60f * 8f);
			if (jumping)
			{
				float z = Mathf.Max(Mathf.Pow((num + 1f) * 0.5f, 2f) * 0.2f - 0.06f, 0f);
				return new Vector3(0f, 0f, z);
			}
			float num2 = Mathf.Sign(num);
			return new Vector3(EasingFunctions.EaseInOutQuad(Mathf.Abs(num) * 0.6f) * 0.09f * num2, 0f, 0f);
		}
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return true;
	}

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		pawn.Rotation = Rot4.Random;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__8))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}
}
