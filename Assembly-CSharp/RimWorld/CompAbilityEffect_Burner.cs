using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompAbilityEffect_Burner : CompAbilityEffect
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public Map map;

		public Func<IntVec3, bool> _003C_003E9__1;

		internal bool _003CGetPreCastActions_003Eb__1(IntVec3 c)
		{
			return c.CanBeSeenOverFast(map);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetPreCastActions_003Ed__2 : IEnumerable<PreCastAction>, IEnumerable, IEnumerator<PreCastAction>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private PreCastAction _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompAbilityEffect_Burner _003C_003E4__this;

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
		public _003CGetPreCastActions_003Ed__2(int _003C_003E1__state)
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
			CompAbilityEffect_Burner CS_0024_003C_003E8__locals19 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new PreCastAction
				{
					action = delegate(LocalTargetInfo a, LocalTargetInfo _)
					{
						_003C_003Ec__DisplayClass2_0 CS_0024_003C_003E8__locals20 = new _003C_003Ec__DisplayClass2_0();
						Vector3 drawPos = CS_0024_003C_003E8__locals19.parent.pawn.DrawPos;
						IntVec3 intVec = drawPos.Yto0().ToIntVec3();
						CS_0024_003C_003E8__locals20.map = CS_0024_003C_003E8__locals19.parent.pawn.Map;
						IncineratorSpray incineratorSpray = GenSpawn.Spawn(ThingDefOf.IncineratorSpray, intVec, CS_0024_003C_003E8__locals20.map) as IncineratorSpray;
						int numStreams = CS_0024_003C_003E8__locals19.Props.numStreams;
						Vector3 normalized = (a.CenterVector3 - drawPos).normalized;
						for (int i = 0; i < numStreams; i++)
						{
							float angle = Rand.Range(0f - CS_0024_003C_003E8__locals19.Props.coneSizeDegrees, CS_0024_003C_003E8__locals19.Props.coneSizeDegrees);
							Vector3 vector = normalized.RotatedBy(angle);
							Vector3 vect = drawPos + vector * (CS_0024_003C_003E8__locals19.Props.range + Rand.Value * CS_0024_003C_003E8__locals19.Props.rangeNoise);
							IntVec3 intVec2 = GenSight.LastPointOnLineOfSight(intVec, vect.ToIntVec3(), (IntVec3 c) => c.CanBeSeenOverFast(CS_0024_003C_003E8__locals20.map), skipFirstCell: true);
							if (!intVec2.IsValid)
							{
								intVec2 = vect.ToIntVec3();
							}
							float num2 = Vector3.Distance(intVec2.ToVector3(), drawPos);
							float num3 = Mathf.Clamp01(num2 / CS_0024_003C_003E8__locals19.Props.sizeReductionDistanceThreshold);
							if (!(Vector3.Dot((intVec2.ToVector3() - drawPos).normalized, vector) <= 0.5f))
							{
								MoteDualAttached mote = MoteMaker.MakeInteractionOverlay(ThingDefOf.Mote_IncineratorBurst, new TargetInfo(intVec, CS_0024_003C_003E8__locals20.map), new TargetInfo(intVec2, CS_0024_003C_003E8__locals20.map));
								incineratorSpray.Add(new IncineratorProjectileMotion
								{
									mote = mote,
									targetDest = a.Cell,
									worldSource = drawPos + vector * CS_0024_003C_003E8__locals19.Props.barrelOffsetDistance,
									worldTarget = intVec2.ToVector3(),
									moveVector = vector,
									startScale = Rand.Range(0.8f, 1.2f) * num3,
									endScale = (1f + Rand.Range(0.1f, 0.4f)) * num3,
									lifespanTicks = Mathf.FloorToInt(num2 * 5f) + Rand.Range(-CS_0024_003C_003E8__locals19.Props.lifespanNoise, CS_0024_003C_003E8__locals19.Props.lifespanNoise)
								});
								CS_0024_003C_003E8__locals20.map.effecterMaintainer.AddEffecterToMaintain(CS_0024_003C_003E8__locals19.Props.effecterDef.Spawn(intVec2, CS_0024_003C_003E8__locals20.map), intVec2, 100);
							}
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
			_003CGetPreCastActions_003Ed__2 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetPreCastActions_003Ed__2(0)
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

	public new CompProperties_AbilityBurner Props => (CompProperties_AbilityBurner)props;

	[IteratorStateMachine(typeof(_003CGetPreCastActions_003Ed__2))]
	public override IEnumerable<PreCastAction> GetPreCastActions()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPreCastActions_003Ed__2(-2)
		{
			_003C_003E4__this = this
		};
	}
}
