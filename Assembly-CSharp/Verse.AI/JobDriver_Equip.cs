using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse.Sound;

namespace Verse.AI;

public class JobDriver_Equip : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__10 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Equip _003C_003E4__this;

		private bool _003CtargetOnStand_003E5__2;

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
		public _003CMakeNewToils_003Ed__10(int _003C_003E1__state)
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
			JobDriver_Equip CS_0024_003C_003E8__locals18 = _003C_003E4__this;
			Toil toil;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals18.FailOnDestroyedOrNull(TargetIndex.A);
				CS_0024_003C_003E8__locals18.FailOnBurningImmobile(TargetIndex.A);
				_003C_003E2__current = Toils_General.Do(delegate
				{
					CS_0024_003C_003E8__locals18.pawn.mindState.droppedWeapon = null;
				});
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				_003CtargetOnStand_003E5__2 = CS_0024_003C_003E8__locals18.TargetIsOnOutfitStand;
				Toil f = Toils_Goto.GotoThing((!_003CtargetOnStand_003E5__2) ? TargetIndex.A : TargetIndex.B, PathEndMode.ClosestTouch);
				if (CS_0024_003C_003E8__locals18.job.ignoreForbidden)
				{
					_003C_003E2__current = f.FailOnDespawnedOrNull((!_003CtargetOnStand_003E5__2) ? TargetIndex.A : TargetIndex.B);
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003E2__current = f.FailOnDespawnedNullOrForbidden((!_003CtargetOnStand_003E5__2) ? TargetIndex.A : TargetIndex.B);
				_003C_003E1__state = 3;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				goto IL_00e7;
			case 3:
				_003C_003E1__state = -1;
				goto IL_00e7;
			case 4:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_00e7:
				toil = ToilMaker.MakeToil("MakeNewToils");
				if (_003CtargetOnStand_003E5__2)
				{
					toil.initAction = delegate
					{
						Building_OutfitStand outfitStand = CS_0024_003C_003E8__locals18.OutfitStand;
						ThingWithComps heldWeapon = outfitStand.HeldWeapon;
						if (heldWeapon == null || heldWeapon != CS_0024_003C_003E8__locals18.Target)
						{
							CS_0024_003C_003E8__locals18.EndJobWith(JobCondition.Errored);
						}
						else if (!outfitStand.RemoveHeldWeapon(heldWeapon))
						{
							CS_0024_003C_003E8__locals18.EndJobWith(JobCondition.Errored);
						}
						else
						{
							CS_0024_003C_003E8__locals18.pawn.equipment.MakeRoomFor(heldWeapon);
							CS_0024_003C_003E8__locals18.pawn.equipment.AddEquipment(heldWeapon);
							heldWeapon.def.soundInteract?.PlayOneShot(new TargetInfo(CS_0024_003C_003E8__locals18.pawn.Position, CS_0024_003C_003E8__locals18.pawn.Map));
						}
					};
				}
				else
				{
					toil.initAction = delegate
					{
						ThingWithComps thingWithComps = (ThingWithComps)CS_0024_003C_003E8__locals18.job.targetA.Thing;
						ThingWithComps thingWithComps2 = null;
						if (thingWithComps.def.stackLimit > 1 && thingWithComps.stackCount > 1)
						{
							thingWithComps2 = (ThingWithComps)thingWithComps.SplitOff(1);
						}
						else
						{
							thingWithComps2 = thingWithComps;
							thingWithComps2.DeSpawn();
						}
						CS_0024_003C_003E8__locals18.pawn.equipment.MakeRoomFor(thingWithComps2);
						CS_0024_003C_003E8__locals18.pawn.equipment.AddEquipment(thingWithComps2);
						thingWithComps.def.soundInteract?.PlayOneShot(new TargetInfo(CS_0024_003C_003E8__locals18.pawn.Position, CS_0024_003C_003E8__locals18.pawn.Map));
					};
				}
				toil.defaultCompleteMode = ToilCompleteMode.Instant;
				_003C_003E2__current = toil;
				_003C_003E1__state = 4;
				return true;
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
			_003CMakeNewToils_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__10(0)
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

	private const TargetIndex EquippableIndex = TargetIndex.A;

	private const TargetIndex OutfitStandIndex = TargetIndex.B;

	private Thing Target => job.GetTarget(TargetIndex.A).Thing;

	private bool TargetIsOnOutfitStand
	{
		get
		{
			Thing target = Target;
			if (target != null && !target.Spawned)
			{
				return target.ParentHolder is Building_OutfitStand;
			}
			return false;
		}
	}

	private Building_OutfitStand OutfitStand => (Building_OutfitStand)job.GetTarget(TargetIndex.B).Thing;

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		if (TargetIsOnOutfitStand)
		{
			job.targetB = (Building_OutfitStand)Target.ParentHolder;
		}
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (EquipmentUtility.AlreadyBondedToWeapon(Target, pawn))
		{
			return false;
		}
		int maxPawns = 1;
		int stackCount = -1;
		_ = TargetIsOnOutfitStand;
		if (job.targetA.HasThing && job.targetA.Thing.Spawned && job.targetA.Thing.def.IsIngestible)
		{
			maxPawns = 10;
			stackCount = 1;
		}
		if (TargetIsOnOutfitStand)
		{
			return pawn.Reserve((Building_OutfitStand)job.targetA.Thing.ParentHolder, job, maxPawns, stackCount, null, errorOnFailed);
		}
		return pawn.Reserve(job.targetA, job, maxPawns, stackCount, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__10))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__10(-2)
		{
			_003C_003E4__this = this
		};
	}
}
