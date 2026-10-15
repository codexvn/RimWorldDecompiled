using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Pawn_MechanitorTracker : IExposable
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__56 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Pawn_MechanitorTracker _003C_003E4__this;

		private List<MechanitorControlGroupGizmo>.Enumerator _003C_003E7__wrap1;

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
		public _003CGetGizmos_003Ed__56(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 3)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<MechanitorControlGroupGizmo>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Pawn_MechanitorTracker CS_0024_003C_003E8__locals15 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (CS_0024_003C_003E8__locals15.bandwidthGizmo == null)
					{
						CS_0024_003C_003E8__locals15.bandwidthGizmo = new MechanitorBandwidthGizmo(CS_0024_003C_003E8__locals15);
					}
					if (CS_0024_003C_003E8__locals15.callBossgroupGizmo == null)
					{
						CS_0024_003C_003E8__locals15.callBossgroupGizmo = new Command_CallBossgroup(CS_0024_003C_003E8__locals15);
					}
					_003C_003E2__current = CS_0024_003C_003E8__locals15.bandwidthGizmo;
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E2__current = CS_0024_003C_003E8__locals15.callBossgroupGizmo;
					_003C_003E1__state = 2;
					return true;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = CS_0024_003C_003E8__locals15.controlGroupGizmos.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00e6;
				case 3:
					_003C_003E1__state = -3;
					goto IL_00e6;
				case 4:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_00e6:
					if (_003C_003E7__wrap1.MoveNext())
					{
						MechanitorControlGroupGizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<MechanitorControlGroupGizmo>.Enumerator);
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "CommandSelectAllMechs".Translate(),
						defaultDesc = "CommandSelectAllMechsDesc".Translate(),
						icon = SelectAllMechsIcon.Texture,
						Order = -87f,
						action = delegate
						{
							Find.Selector.ClearSelection();
							CS_0024_003C_003E8__locals15.tmpMechsInAssignedOrder.Clear();
							MechanitorUtility.GetMechsInAssignedOrder(CS_0024_003C_003E8__locals15.pawn, ref CS_0024_003C_003E8__locals15.tmpMechsInAssignedOrder);
							for (int i = 0; i < CS_0024_003C_003E8__locals15.tmpMechsInAssignedOrder.Count; i++)
							{
								Find.Selector.Select(CS_0024_003C_003E8__locals15.tmpMechsInAssignedOrder[i]);
							}
							CS_0024_003C_003E8__locals15.tmpMechsInAssignedOrder.Clear();
						}
					};
					_003C_003E1__state = 4;
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__56 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__56(0)
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

	private const float MechCommandRange = 24.9f;

	private static readonly CachedTexture SelectAllMechsIcon = new CachedTexture("UI/Icons/SelectAllMechs");

	private Pawn pawn;

	private MechanitorBandwidthGizmo bandwidthGizmo;

	private Command_CallBossgroup callBossgroupGizmo;

	private List<MechanitorControlGroupGizmo> controlGroupGizmos = new List<MechanitorControlGroupGizmo>();

	private List<Pawn> tmpMechsInAssignedOrder = new List<Pawn>();

	private List<Pawn> overseenPawns = new List<Pawn>();

	private List<Pawn> controlledPawns = new List<Pawn>();

	public List<MechanitorControlGroup> controlGroups = new List<MechanitorControlGroup>();

	private List<Bill_Mech> activeMechBills = new List<Bill_Mech>();

	private static List<Pawn> tmpPrevControlledMechs = new List<Pawn>();

	public List<Pawn> OverseenPawns
	{
		get
		{
			overseenPawns.Clear();
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (directRelations[i].def == PawnRelationDefOf.Overseer)
				{
					overseenPawns.Add(directRelations[i].otherPawn);
				}
			}
			return overseenPawns;
		}
	}

	public List<Pawn> ControlledPawns => controlledPawns;

	public int UsedBandwidth => UsedBandwidthFromSubjects + UsedBandwidthFromGestation;

	public int UsedBandwidthFromSubjects => (int)OverseenPawns.Where((Pawn p) => !p.IsGestating()).Sum((Pawn p) => p.GetStatValue(StatDefOf.BandwidthCost));

	public int UsedBandwidthFromGestation
	{
		get
		{
			List<Bill_Mech> list = ActiveMechBills;
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				num += (int)list[i].BandwidthCost;
			}
			return num;
		}
	}

	public int TotalBandwidth => (int)pawn.GetStatValue(StatDefOf.MechBandwidth);

	public int TotalAvailableControlGroups => (int)pawn.GetStatValue(StatDefOf.MechControlGroups);

	public List<Bill_Mech> ActiveMechBills
	{
		get
		{
			activeMechBills.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				List<Thing> list = maps[i].listerThings.ThingsInGroup(ThingRequestGroup.MechGestator);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j] is Building_MechGestator { ActiveMechBill: not null } building_MechGestator && building_MechGestator.ActiveMechBill.BoundPawn == pawn)
					{
						activeMechBills.Add(building_MechGestator.ActiveMechBill);
					}
				}
			}
			activeMechBills.SortBy((Bill_Mech b) => b.StartedTick);
			return activeMechBills;
		}
	}

	public Pawn Pawn => pawn;

	public bool AnySelectedDraftedMechs
	{
		get
		{
			List<Pawn> selectedPawns = Find.Selector.SelectedPawns;
			for (int i = 0; i < selectedPawns.Count; i++)
			{
				if (selectedPawns[i].GetOverseer() == pawn && selectedPawns[i].Drafted)
				{
					return true;
				}
			}
			return false;
		}
	}

	public AcceptanceReport CanControlMechs
	{
		get
		{
			if (pawn.Downed)
			{
				return "MechControllerDowned".Translate(pawn.Named("PAWN"));
			}
			if (pawn.IsPrisoner)
			{
				return "MechControllerImprisoned".Translate(pawn.Named("PAWN"));
			}
			if (!pawn.Spawned)
			{
				Thing spawnedParentOrMe = pawn.SpawnedParentOrMe;
				if (spawnedParentOrMe is Building)
				{
					return "MechControllerInsideContainer".Translate(pawn.Named("PAWN"), spawnedParentOrMe.Named("CONTAINER"));
				}
				return false;
			}
			if (pawn.InMentalState)
			{
				return "MechControllerMentalState".Translate(pawn.Named("PAWN"), pawn.MentalStateDef.Named("MENTALSTATE"));
			}
			return true;
		}
	}

	public Pawn_MechanitorTracker()
	{
	}

	public Pawn_MechanitorTracker(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public bool CanOverseeSubject(Pawn subject)
	{
		if (subject.OverseerSubject != null)
		{
			return subject.GetStatValue(StatDefOf.BandwidthCost) <= (float)TotalBandwidth;
		}
		return false;
	}

	public MechanitorControlGroup GetControlGroup(Pawn mech)
	{
		foreach (MechanitorControlGroup controlGroup in controlGroups)
		{
			if (controlGroup.MechsForReading.Contains(mech))
			{
				return controlGroup;
			}
		}
		return null;
	}

	public void AssignPawnControlGroup(Pawn pawn, MechWorkModeDef preferredWorkMode = null)
	{
		if (controlGroups.Count == 0)
		{
			Notify_ControlGroupAmountMayChanged();
			if (controlGroups.Count == 0)
			{
				Log.Warning("Wants to assign pawn to a control group, but there are none!");
				return;
			}
		}
		if (preferredWorkMode != null)
		{
			if (controlGroups.Any((MechanitorControlGroup c) => c.WorkMode == preferredWorkMode))
			{
				controlGroups.First((MechanitorControlGroup c) => c.WorkMode == preferredWorkMode).Assign(pawn);
			}
			else if (controlGroups.Any((MechanitorControlGroup c) => c.MechsForReading.Count == 0))
			{
				MechanitorControlGroup mechanitorControlGroup = controlGroups.First((MechanitorControlGroup c) => c.MechsForReading.Count == 0);
				mechanitorControlGroup.SetWorkMode(preferredWorkMode);
				mechanitorControlGroup.Assign(pawn);
			}
			else
			{
				controlGroups[0].Assign(pawn);
			}
		}
		else
		{
			controlGroups[0].Assign(pawn);
		}
		Notify_BandwidthChanged();
	}

	public void UnassignPawnFromAnyControlGroup(Pawn pawn)
	{
		foreach (MechanitorControlGroup controlGroup in controlGroups)
		{
			controlGroup.TryUnassign(pawn);
		}
	}

	private void Notify_ControlGroupAmountMayChanged()
	{
		int totalAvailableControlGroups = TotalAvailableControlGroups;
		while (controlGroups.Count < totalAvailableControlGroups)
		{
			controlGroups.Add(new MechanitorControlGroup(this));
		}
		List<Pawn> list = null;
		while (controlGroups.Count > totalAvailableControlGroups)
		{
			MechanitorControlGroup mechanitorControlGroup = controlGroups[controlGroups.Count - 1];
			if (controlGroups.Count > 1)
			{
				if (!mechanitorControlGroup.MechsForReading.NullOrEmpty())
				{
					if (list == null)
					{
						list = new List<Pawn>();
					}
					list.AddRange(mechanitorControlGroup.MechsForReading);
				}
			}
			else
			{
				Log.Warning("Removed last mechanitor control group");
			}
			controlGroups.RemoveAt(controlGroups.Count - 1);
		}
		if (list != null && controlGroups.Count > 0)
		{
			foreach (Pawn item in list)
			{
				controlGroups[0].Assign(item);
			}
		}
		controlGroupGizmos.Clear();
		foreach (MechanitorControlGroup controlGroup in controlGroups)
		{
			controlGroupGizmos.Add(new MechanitorControlGroupGizmo(controlGroup));
		}
	}

	public void Notify_PawnSpawned(bool respawningAfterLoad)
	{
		if (respawningAfterLoad)
		{
			Notify_BandwidthChanged();
			Notify_ControlGroupAmountMayChanged();
		}
	}

	public void Notify_BandwidthChanged()
	{
		CheckAvailableBandwidthForBills();
		tmpPrevControlledMechs.Clear();
		tmpPrevControlledMechs.AddRange(controlledPawns);
		controlledPawns.Clear();
		tmpMechsInAssignedOrder.Clear();
		MechanitorUtility.GetMechsInAssignedOrder(pawn, ref tmpMechsInAssignedOrder);
		float num = 0f;
		for (int num2 = tmpMechsInAssignedOrder.Count - 1; num2 >= 0; num2--)
		{
			num += tmpMechsInAssignedOrder[num2].GetStatValue(StatDefOf.BandwidthCost);
			if (num <= (float)TotalBandwidth)
			{
				controlledPawns.Add(tmpMechsInAssignedOrder[num2]);
			}
			else if (tmpPrevControlledMechs.Contains(tmpMechsInAssignedOrder[num2]))
			{
				tmpMechsInAssignedOrder[num2].OverseerSubject?.Notify_DisconnectedFromOverseer();
			}
		}
		tmpPrevControlledMechs.Clear();
		tmpMechsInAssignedOrder.Clear();
	}

	public void CheckAvailableBandwidthForBills()
	{
		List<Bill_Mech> list = ActiveMechBills;
		for (int i = 0; i < list.Count; i++)
		{
			list[i].suspended = !HasBandwidthForBill(list[i]);
		}
	}

	public void Notify_MechlinkRemoved()
	{
		List<Pawn> list = OverseenPawns;
		for (int num = list.Count - 1; num >= 0; num--)
		{
			pawn.relations.TryRemoveDirectRelation(PawnRelationDefOf.Overseer, list[num]);
		}
		PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn);
	}

	public void Notify_ChangedGuestStatus()
	{
		if (pawn.IsPrisoner)
		{
			UndraftAllMechs();
			for (int i = 0; i < controlGroups.Count; i++)
			{
				controlGroups[i].SetWorkMode(MechWorkModeDefOf.SelfShutdown);
			}
		}
	}

	public void Notify_ApparelChanged()
	{
		Notify_BandwidthChanged();
		Notify_ControlGroupAmountMayChanged();
	}

	public void Notify_HediffStateChange(Hediff hediff)
	{
		if (hediff != null)
		{
			Notify_BandwidthChanged();
			Notify_ControlGroupAmountMayChanged();
		}
	}

	public void Notify_ChangedFaction()
	{
		List<Pawn> list = OverseenPawns;
		for (int i = 0; i < list.Count; i++)
		{
			list[i].SetFaction(pawn.Faction);
			list[i].Notify_DisabledWorkTypesChanged();
		}
	}

	public void Notify_Downed()
	{
		UndraftAllMechs();
	}

	public void Notify_DeSpawned(DestroyMode mode)
	{
		if (mode != DestroyMode.WillReplace)
		{
			UndraftAllMechs();
		}
	}

	public void UndraftAllMechs()
	{
		tmpMechsInAssignedOrder.Clear();
		MechanitorUtility.GetMechsInAssignedOrder(pawn, ref tmpMechsInAssignedOrder);
		for (int i = 0; i < tmpMechsInAssignedOrder.Count; i++)
		{
			if (tmpMechsInAssignedOrder[i].Drafted)
			{
				tmpMechsInAssignedOrder[i].drafter.Drafted = false;
			}
		}
		tmpMechsInAssignedOrder.Clear();
	}

	public bool HasBandwidthForBill(Bill_Mech bill)
	{
		int totalBandwidth = TotalBandwidth;
		int num = UsedBandwidthFromSubjects;
		float bandwidthCost = bill.BandwidthCost;
		if ((float)totalBandwidth < bandwidthCost + (float)num)
		{
			return false;
		}
		List<Bill_Mech> list = ActiveMechBills;
		for (int i = 0; i < list.Count; i++)
		{
			num += (int)list[i].BandwidthCost;
			if (list[i] == bill && num <= totalBandwidth)
			{
				return true;
			}
			if (num >= totalBandwidth)
			{
				return false;
			}
		}
		return (float)num + bandwidthCost <= (float)totalBandwidth;
	}

	public bool HasBandwidthToResurrect(Corpse corpse)
	{
		float statValue = corpse.InnerPawn.GetStatValue(StatDefOf.BandwidthCost);
		return (float)UsedBandwidth + statValue <= (float)TotalBandwidth;
	}

	public void DrawCommandRadius()
	{
		if (pawn.Spawned && AnySelectedDraftedMechs)
		{
			GenDraw.DrawRadiusRing(pawn.Position, 24.9f, Color.white, (IntVec3 c) => CanCommandTo(c));
		}
	}

	public bool CanCommandTo(LocalTargetInfo target)
	{
		if (!target.Cell.InBounds(pawn.MapHeld))
		{
			return false;
		}
		return (float)pawn.Position.DistanceToSquared(target.Cell) < 620.01f;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__56))]
	public IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__56(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void ExposeData()
	{
		Scribe_Collections.Look(ref controlGroups, "controlGroups", LookMode.Deep, this);
		Scribe_Collections.Look(ref controlledPawns, "controlledPawns", LookMode.Reference);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			controlledPawns.RemoveAll((Pawn x) => x == null);
			if (controlGroups == null)
			{
				controlGroups = new List<MechanitorControlGroup>();
			}
			Notify_ControlGroupAmountMayChanged();
		}
	}
}
