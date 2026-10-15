using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_UseOutfitStand : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__8 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_UseOutfitStand _003C_003E4__this;

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
			JobDriver_UseOutfitStand jobDriver_UseOutfitStand = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				jobDriver_UseOutfitStand.FailOnBurningImmobile(TargetIndex.A);
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnDespawnedNullOrForbidden(TargetIndex.A);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
				toil2.WithProgressBarToilDelay(TargetIndex.A);
				toil2.defaultCompleteMode = ToilCompleteMode.Delay;
				toil2.defaultDuration = jobDriver_UseOutfitStand.duration;
				_003C_003E2__current = toil2;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.AddFinishAction(jobDriver_UseOutfitStand.DoTransfer);
				toil.defaultCompleteMode = ToilCompleteMode.Instant;
				_003C_003E2__current = toil;
				_003C_003E1__state = 3;
				return true;
			}
			case 3:
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

	private int duration;

	private HashSet<Apparel> wornApparelToTransferToStand;

	private HashSet<Apparel> standApparelToTransferToPawn;

	private Building_OutfitStand OutfitStand => (Building_OutfitStand)job.targetA.Thing;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref duration, "duration", 0);
		Scribe_Collections.Look(ref wornApparelToTransferToStand, "wornApparelToTransferToStand", LookMode.Reference);
		Scribe_Collections.Look(ref standApparelToTransferToPawn, "standApparelToTransferToPawn", LookMode.Reference);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
	}

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		standApparelToTransferToPawn = new HashSet<Apparel>();
		wornApparelToTransferToStand = new HashSet<Apparel>();
		List<Apparel> wornApparel = pawn.apparel.WornApparel;
		IReadOnlyList<Thing> heldItems = OutfitStand.HeldItems;
		duration = 0;
		foreach (Thing item in heldItems)
		{
			if (!(item is Apparel apparel) || !apparel.PawnCanWear(pawn) || !ApparelUtility.HasPartsToWear(pawn, apparel.def) || (CompBiocodable.IsBiocoded(apparel) && !CompBiocodable.IsBiocodedFor(apparel, pawn)))
			{
				continue;
			}
			bool flag = true;
			foreach (Apparel item2 in wornApparel)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, item2.def, pawn.RaceProps.body))
				{
					if (pawn.apparel.IsLocked(item2))
					{
						flag = false;
						break;
					}
					duration += (int)(item2.GetStatValue(StatDefOf.EquipDelay) * 60f);
					wornApparelToTransferToStand.Add(item2);
				}
			}
			if (flag)
			{
				standApparelToTransferToPawn.Add(apparel);
				duration += (int)(apparel.GetStatValue(StatDefOf.EquipDelay) * 60f);
			}
		}
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

	private void DoTransfer()
	{
		foreach (Apparel item in wornApparelToTransferToStand)
		{
			pawn.apparel.Remove(item);
		}
		foreach (Apparel item2 in standApparelToTransferToPawn)
		{
			OutfitStand.RemoveApparel(item2);
			pawn.apparel.Wear(item2);
			pawn.outfits.forcedHandler.SetForced(item2, forced: true);
		}
		foreach (Apparel item3 in wornApparelToTransferToStand)
		{
			OutfitStand.AddApparel(item3);
		}
		ThingWithComps heldWeapon = OutfitStand.HeldWeapon;
		if (heldWeapon != null && PawnCanWieldWeapon(heldWeapon, pawn) && OutfitStand.RemoveHeldWeapon(heldWeapon))
		{
			pawn.equipment.MakeRoomFor(heldWeapon, out var dropped);
			pawn.equipment.AddEquipment(heldWeapon);
			if (dropped != null)
			{
				dropped.DeSpawn();
				OutfitStand.TryAddHeldWeapon(dropped);
			}
		}
	}

	private static bool PawnCanWieldWeapon(Thing weapon, Pawn pawn)
	{
		if (weapon.def.IsWeapon && pawn.WorkTagIsDisabled(WorkTags.Violent))
		{
			return false;
		}
		if (weapon.def.IsRangedWeapon && pawn.WorkTagIsDisabled(WorkTags.Shooting))
		{
			return false;
		}
		if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
		{
			return false;
		}
		if (pawn.IsQuestLodger() && !EquipmentUtility.QuestLodgerCanEquip(weapon, pawn))
		{
			return false;
		}
		if (!EquipmentUtility.CanEquip(weapon, pawn, out var _))
		{
			return false;
		}
		return true;
	}
}
