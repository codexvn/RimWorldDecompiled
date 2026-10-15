using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_ShieldMech : JobDriver_CastAbility
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__5 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_ShieldMech _003C_003E4__this;

		private IEnumerator<Toil> _003C_003E7__wrap1;

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
		public _003CMakeNewToils_003Ed__5(int _003C_003E1__state)
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
				JobDriver_ShieldMech CS_0024_003C_003E8__locals23 = _003C_003E4__this;
				Toil toil;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((JobDriver_CastAbility)CS_0024_003C_003E8__locals23).MakeNewToils().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0074;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0074;
				case 2:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0074:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Toil current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					CS_0024_003C_003E8__locals23.AddFinishAction(delegate
					{
						if (CS_0024_003C_003E8__locals23.mechShield != null)
						{
							if (!CS_0024_003C_003E8__locals23.mechShield.Destroyed)
							{
								CS_0024_003C_003E8__locals23.mechShield.Destroy();
							}
							CS_0024_003C_003E8__locals23.mechShield = null;
						}
					});
					toil = ToilMaker.MakeToil("MakeNewToils");
					toil.initAction = delegate
					{
						CS_0024_003C_003E8__locals23.pawn.pather.StopDead();
						CS_0024_003C_003E8__locals23.mechShield = (MechShield)GenSpawn.Spawn(ThingDefOf.MechShield, CS_0024_003C_003E8__locals23.TargetThingA.Position, CS_0024_003C_003E8__locals23.pawn.Map);
						CS_0024_003C_003E8__locals23.mechShield.SetTarget(CS_0024_003C_003E8__locals23.TargetThingA);
						int num2 = (int)CS_0024_003C_003E8__locals23.pawn.GetStatValue(StatDefOf.MechRemoteShieldEnergy);
						CS_0024_003C_003E8__locals23.ProjectileInterceptor.maxHitPointsOverride = num2;
						CS_0024_003C_003E8__locals23.ProjectileInterceptor.currentHitPoints = num2;
					};
					toil.tickIntervalAction = delegate
					{
						CS_0024_003C_003E8__locals23.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals23.TargetThingA);
					};
					toil.defaultCompleteMode = ToilCompleteMode.Delay;
					toil.defaultDuration = 1800;
					toil.handlingFacing = true;
					toil.PlaySustainerOrSound(SoundDefOf.ShieldMech);
					toil.AddFailCondition(delegate
					{
						if (CS_0024_003C_003E8__locals23.mechShield != null)
						{
							if (!CS_0024_003C_003E8__locals23.job.ability.verb.CanHitTargetFrom(CS_0024_003C_003E8__locals23.pawn.Position, CS_0024_003C_003E8__locals23.mechShield))
							{
								return true;
							}
							if (CS_0024_003C_003E8__locals23.ProjectileInterceptor != null && !CS_0024_003C_003E8__locals23.ProjectileInterceptor.Active)
							{
								return true;
							}
						}
						return false;
					});
					_003C_003E2__current = toil;
					_003C_003E1__state = 2;
					return true;
				}
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
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__5(0)
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

	private const int DurationTicks = 1800;

	private MechShield mechShield;

	private CompProjectileInterceptor projectileInterceptor;

	private CompProjectileInterceptor ProjectileInterceptor
	{
		get
		{
			if (projectileInterceptor == null && mechShield != null)
			{
				projectileInterceptor = mechShield.GetComp<CompProjectileInterceptor>();
			}
			return projectileInterceptor;
		}
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__5))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__5(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref mechShield, "mechShield");
	}
}
