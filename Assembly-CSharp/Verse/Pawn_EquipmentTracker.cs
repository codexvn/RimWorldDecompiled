using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse;

public class Pawn_EquipmentTracker : IThingHolder, IExposable
{
	[CompilerGenerated]
	private sealed class _003C_003CGetGizmos_003Eg__YieldGizmos_007C30_0_003Ed : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingWithComps eq;

		public ThingWithComps _003C_003E3__eq;

		private KeyBindingDef preferredHotKey;

		public KeyBindingDef _003C_003E3__preferredHotKey;

		private IEnumerator<Command> _003C_003E7__wrap1;

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
		public _003C_003CGetGizmos_003Eg__YieldGizmos_007C30_0_003Ed(int _003C_003E1__state)
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
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = eq.GetComp<CompEquippable>().GetVerbsCommands().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Command current = _003C_003E7__wrap1.Current;
					if (tmpKeybindings.Count > 0)
					{
						if (preferredHotKey != null && tmpKeybindings.Contains(preferredHotKey))
						{
							current.hotKey = preferredHotKey;
							tmpKeybindings.Remove(preferredHotKey);
						}
						else
						{
							current.hotKey = tmpKeybindings.Pop();
						}
					}
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003C_003CGetGizmos_003Eg__YieldGizmos_007C30_0_003Ed _003C_003CGetGizmos_003Eg__YieldGizmos_007C30_0_003Ed;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003C_003CGetGizmos_003Eg__YieldGizmos_007C30_0_003Ed = this;
			}
			else
			{
				_003C_003CGetGizmos_003Eg__YieldGizmos_007C30_0_003Ed = new _003C_003CGetGizmos_003Eg__YieldGizmos_007C30_0_003Ed(0);
			}
			_003C_003CGetGizmos_003Eg__YieldGizmos_007C30_0_003Ed.eq = _003C_003E3__eq;
			_003C_003CGetGizmos_003Eg__YieldGizmos_007C30_0_003Ed.preferredHotKey = _003C_003E3__preferredHotKey;
			return _003C_003CGetGizmos_003Eg__YieldGizmos_007C30_0_003Ed;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__30 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Pawn_EquipmentTracker _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		private List<ThingWithComps> _003Clist_003E5__3;

		private ThingWithComps _003CprimaryMelee_003E5__4;

		private ThingWithComps _003CprimaryRanged_003E5__5;

		private int _003Ci_003E5__6;

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
		public _003CGetGizmos_003Ed__30(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -7:
			case -6:
			case -5:
			case -4:
			case 2:
			case 3:
			case 4:
				try
				{
					switch (num)
					{
					case -5:
					case 2:
						try
						{
						}
						finally
						{
							_003C_003Em__Finally3();
						}
						break;
					case -6:
					case 3:
						try
						{
						}
						finally
						{
							_003C_003Em__Finally4();
						}
						break;
					case -7:
					case 4:
						try
						{
						}
						finally
						{
							_003C_003Em__Finally5();
						}
						break;
					}
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003Clist_003E5__3 = null;
			_003CprimaryMelee_003E5__4 = null;
			_003CprimaryRanged_003E5__5 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Pawn_EquipmentTracker pawn_EquipmentTracker = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (pawn_EquipmentTracker.Primary != null)
					{
						_003C_003E7__wrap1 = pawn_EquipmentTracker.PrimaryEq.CompGetEquippedGizmosExtra().GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0087;
					}
					goto IL_00a1;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0087;
				case 2:
					_003C_003E1__state = -5;
					goto IL_01a8;
				case 3:
					_003C_003E1__state = -6;
					goto IL_021e;
				case 4:
					{
						_003C_003E1__state = -7;
						goto IL_02b0;
					}
					IL_02ca:
					_003Ci_003E5__6++;
					goto IL_02dc;
					IL_0087:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					goto IL_00a1;
					IL_01a8:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					goto IL_01c2;
					IL_0238:
					_003Ci_003E5__6 = 0;
					goto IL_02dc;
					IL_00a1:
					if (!PawnAttackGizmoUtility.CanShowEquipmentGizmos())
					{
						break;
					}
					_003C_003E1__state = -4;
					tmpKeybindings.Add(KeyBindingDefOf.Misc1);
					tmpKeybindings.Add(KeyBindingDefOf.Misc2);
					tmpKeybindings.Add(KeyBindingDefOf.Misc3);
					_003Clist_003E5__3 = pawn_EquipmentTracker.AllEquipmentListForReading;
					_003CprimaryMelee_003E5__4 = _003Clist_003E5__3.FirstOrDefault((ThingWithComps w) => w.def.IsMeleeWeapon);
					_003CprimaryRanged_003E5__5 = _003Clist_003E5__3.FirstOrDefault((ThingWithComps w) => w.def.IsRangedWeapon);
					if (_003CprimaryMelee_003E5__4 != null)
					{
						KeyBindingDef misc = KeyBindingDefOf.Misc2;
						_003C_003E7__wrap1 = _003CGetGizmos_003Eg__YieldGizmos_007C30_0(_003CprimaryMelee_003E5__4, misc).GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_01a8;
					}
					goto IL_01c2;
					IL_02dc:
					if (_003Ci_003E5__6 < _003Clist_003E5__3.Count)
					{
						ThingWithComps thingWithComps = _003Clist_003E5__3[_003Ci_003E5__6];
						if (thingWithComps != _003CprimaryMelee_003E5__4 && thingWithComps != _003CprimaryRanged_003E5__5)
						{
							_003C_003E7__wrap1 = _003CGetGizmos_003Eg__YieldGizmos_007C30_0(thingWithComps).GetEnumerator();
							_003C_003E1__state = -7;
							goto IL_02b0;
						}
						goto IL_02ca;
					}
					_003Clist_003E5__3 = null;
					_003CprimaryMelee_003E5__4 = null;
					_003CprimaryRanged_003E5__5 = null;
					_003C_003Em__Finally2();
					break;
					IL_01c2:
					if (_003CprimaryRanged_003E5__5 != null)
					{
						KeyBindingDef misc2 = KeyBindingDefOf.Misc1;
						_003C_003E7__wrap1 = _003CGetGizmos_003Eg__YieldGizmos_007C30_0(_003CprimaryRanged_003E5__5, misc2).GetEnumerator();
						_003C_003E1__state = -6;
						goto IL_021e;
					}
					goto IL_0238;
					IL_02b0:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current3 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally5();
					_003C_003E7__wrap1 = null;
					goto IL_02ca;
					IL_021e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current4 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current4;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap1 = null;
					goto IL_0238;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			tmpKeybindings.Clear();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -4;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -4;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally5()
		{
			_003C_003E1__state = -4;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__30 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__30(0)
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

	[CompilerGenerated]
	private sealed class _003Cget_AllEquipmentVerbs_003Ed__12 : IEnumerable<Verb>, IEnumerable, IEnumerator<Verb>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Verb _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Pawn_EquipmentTracker _003C_003E4__this;

		private List<ThingWithComps> _003Clist_003E5__2;

		private int _003Ci_003E5__3;

		private List<Verb> _003Cverbs_003E5__4;

		private int _003Cj_003E5__5;

		Verb IEnumerator<Verb>.Current
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
		public _003Cget_AllEquipmentVerbs_003Ed__12(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Clist_003E5__2 = null;
			_003Cverbs_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Pawn_EquipmentTracker pawn_EquipmentTracker = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				_003Cj_003E5__5++;
				goto IL_0099;
			}
			_003C_003E1__state = -1;
			_003Clist_003E5__2 = pawn_EquipmentTracker.AllEquipmentListForReading;
			_003Ci_003E5__3 = 0;
			goto IL_00c3;
			IL_0099:
			if (_003Cj_003E5__5 < _003Cverbs_003E5__4.Count)
			{
				_003C_003E2__current = _003Cverbs_003E5__4[_003Cj_003E5__5];
				_003C_003E1__state = 1;
				return true;
			}
			_003Cverbs_003E5__4 = null;
			_003Ci_003E5__3++;
			goto IL_00c3;
			IL_00c3:
			if (_003Ci_003E5__3 < _003Clist_003E5__2.Count)
			{
				ThingWithComps thingWithComps = _003Clist_003E5__2[_003Ci_003E5__3];
				_003Cverbs_003E5__4 = thingWithComps.GetComp<CompEquippable>().AllVerbs;
				_003Cj_003E5__5 = 0;
				goto IL_0099;
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
		IEnumerator<Verb> IEnumerable<Verb>.GetEnumerator()
		{
			_003Cget_AllEquipmentVerbs_003Ed__12 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_AllEquipmentVerbs_003Ed__12(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Verb>)this).GetEnumerator();
		}
	}

	public Pawn pawn;

	private ThingOwner<ThingWithComps> equipment;

	public Thing bondedWeapon;

	private static List<KeyBindingDef> tmpKeybindings = new List<KeyBindingDef>();

	public ThingWithComps Primary
	{
		get
		{
			for (int i = 0; i < equipment.Count; i++)
			{
				if (equipment[i].def.equipmentType == EquipmentType.Primary)
				{
					return equipment[i];
				}
			}
			return null;
		}
		private set
		{
			if (Primary == value)
			{
				return;
			}
			if (value != null && value.def.equipmentType != EquipmentType.Primary)
			{
				Log.Error("Tried to set non-primary equipment as primary.");
				return;
			}
			if (Primary != null)
			{
				equipment.Remove(Primary);
			}
			if (value != null)
			{
				equipment.TryAdd(value);
			}
			if (pawn.drafter != null)
			{
				pawn.drafter.Notify_PrimaryWeaponChanged();
			}
		}
	}

	public CompEquippable PrimaryEq => Primary?.GetComp<CompEquippable>();

	public List<ThingWithComps> AllEquipmentListForReading => equipment.InnerListForReading;

	public IEnumerable<Verb> AllEquipmentVerbs
	{
		[IteratorStateMachine(typeof(_003Cget_AllEquipmentVerbs_003Ed__12))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AllEquipmentVerbs_003Ed__12(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public IThingHolder ParentHolder => pawn;

	public Pawn_EquipmentTracker(Pawn newPawn)
	{
		pawn = newPawn;
		equipment = new ThingOwner<ThingWithComps>(this);
	}

	public void ExposeData()
	{
		Scribe_Deep.Look(ref equipment, "equipment", this);
		Scribe_References.Look(ref bondedWeapon, "bondedWeapon");
		if (Scribe.mode != LoadSaveMode.PostLoadInit)
		{
			return;
		}
		List<ThingWithComps> allEquipmentListForReading = AllEquipmentListForReading;
		for (int i = 0; i < allEquipmentListForReading.Count; i++)
		{
			foreach (Verb allVerb in allEquipmentListForReading[i].GetComp<CompEquippable>().AllVerbs)
			{
				allVerb.caster = pawn;
			}
		}
	}

	public void EquipmentTrackerTick()
	{
		List<ThingWithComps> allEquipmentListForReading = AllEquipmentListForReading;
		for (int i = 0; i < allEquipmentListForReading.Count; i++)
		{
			ThingWithComps thingWithComps = allEquipmentListForReading[i];
			if (thingWithComps.def.tickerType != TickerType.Normal)
			{
				thingWithComps.GetComp<CompEquippable>().verbTracker.VerbsTick();
			}
		}
	}

	public bool HasAnything()
	{
		return equipment.Any;
	}

	public void MakeRoomFor(ThingWithComps eq)
	{
		MakeRoomFor(eq, out var _);
	}

	public void MakeRoomFor(ThingWithComps eq, out ThingWithComps dropped)
	{
		dropped = null;
		if (eq.def.equipmentType != EquipmentType.Primary || Primary == null)
		{
			return;
		}
		if (TryDropEquipment(Primary, out dropped, pawn.Position))
		{
			if (dropped != null)
			{
				dropped.SetForbidden(value: false);
			}
		}
		else
		{
			Log.Error(pawn?.ToString() + " couldn't make room for equipment " + eq);
		}
	}

	public void Remove(ThingWithComps eq)
	{
		equipment.Remove(eq);
	}

	public bool TryDropEquipment(ThingWithComps eq, out ThingWithComps resultingEq, IntVec3 pos, bool forbid = true)
	{
		if (!pos.IsValid)
		{
			Log.Error(pawn?.ToString() + " tried to drop " + eq?.ToString() + " at invalid cell.");
			resultingEq = null;
			return false;
		}
		if (equipment.TryDrop(eq, pos, pawn.MapHeld, ThingPlaceMode.Near, out resultingEq))
		{
			if (resultingEq != null)
			{
				resultingEq.SetForbidden(forbid, warnOnFail: false);
			}
			return true;
		}
		return false;
	}

	public void DropAllEquipment(IntVec3 pos, bool forbid = true, bool rememberPrimary = false)
	{
		for (int num = equipment.Count - 1; num >= 0; num--)
		{
			bool flag = equipment[num] == Primary;
			if (TryDropEquipment(equipment[num], out var resultingEq, pos, forbid) && rememberPrimary && flag)
			{
				pawn.mindState.droppedWeapon = resultingEq;
			}
		}
	}

	public bool TryTransferEquipmentToContainer(ThingWithComps eq, ThingOwner container)
	{
		return equipment.TryTransferToContainer(eq, container);
	}

	public void DestroyEquipment(ThingWithComps eq)
	{
		if (!equipment.Contains(eq))
		{
			Log.Warning("Tried to destroy equipment " + eq?.ToString() + " but it's not here.");
			return;
		}
		Remove(eq);
		eq.Destroy();
	}

	public void DestroyAllEquipment(DestroyMode mode = DestroyMode.Vanish)
	{
		equipment.ClearAndDestroyContents(mode);
	}

	public bool Contains(Thing eq)
	{
		return equipment.Contains(eq);
	}

	internal void Notify_PrimaryDestroyed()
	{
		if (Primary != null)
		{
			Remove(Primary);
		}
		if (pawn.Spawned)
		{
			pawn.stances.CancelBusyStanceSoft();
		}
	}

	public void AddEquipment(ThingWithComps newEq)
	{
		if (newEq.def.equipmentType == EquipmentType.Primary && Primary != null)
		{
			Log.Error("Pawn " + pawn.LabelCap + " got primaryInt equipment " + newEq?.ToString() + " while already having primaryInt equipment " + Primary);
		}
		else if (equipment.TryAdd(newEq) && newEq.def.equipmentType == EquipmentType.Primary)
		{
			pawn.mindState.droppedWeapon = null;
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__30))]
	public IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__30(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void Notify_EquipmentAdded(ThingWithComps eq)
	{
		foreach (Verb allVerb in eq.GetComp<CompEquippable>().AllVerbs)
		{
			allVerb.caster = pawn;
			allVerb.Notify_PickedUp();
		}
		eq.Notify_Equipped(pawn);
		if (ModsConfig.RoyaltyActive && eq.def.equipmentType == EquipmentType.Primary && bondedWeapon != null && !bondedWeapon.Destroyed)
		{
			bondedWeapon.TryGetComp<CompBladelinkWeapon>()?.Notify_WieldedOtherWeapon();
		}
	}

	public void Notify_EquipmentRemoved(ThingWithComps eq)
	{
		eq.Notify_Unequipped(pawn);
		if (ModsConfig.RoyaltyActive)
		{
			eq.TryGetComp<CompBladelinkWeapon>()?.Notify_EquipmentLost(pawn);
		}
		if (ModsConfig.OdysseyActive)
		{
			eq.TryGetComp<CompUniqueWeapon>()?.Notify_EquipmentLost(pawn);
		}
	}

	public void Notify_AbilityUsed(Ability ability)
	{
		if (PrimaryEq is CompEquippableAbility compEquippableAbility && ability == compEquippableAbility.AbilityForReading)
		{
			compEquippableAbility.UsedOnce();
		}
	}

	public void Notify_PawnSpawned()
	{
		if (!pawn.BeingTransportedOnGravship && HasAnything() && pawn.Downed && !pawn.GetPosture().InBed())
		{
			if (pawn.kindDef.destroyGearOnDrop)
			{
				DestroyAllEquipment();
			}
			else
			{
				DropAllEquipment(pawn.Position);
			}
		}
	}

	public void Notify_PawnDied()
	{
		if (ModsConfig.RoyaltyActive && bondedWeapon != null)
		{
			bondedWeapon.TryGetComp<CompBladelinkWeapon>()?.UnCode();
		}
	}

	public void Notify_KilledPawn()
	{
		foreach (ThingWithComps item in equipment)
		{
			item.Notify_KilledPawn(pawn);
		}
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return equipment;
	}

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
	}

	[IteratorStateMachine(typeof(_003C_003CGetGizmos_003Eg__YieldGizmos_007C30_0_003Ed))]
	[CompilerGenerated]
	internal static IEnumerable<Gizmo> _003CGetGizmos_003Eg__YieldGizmos_007C30_0(ThingWithComps eq, KeyBindingDef preferredHotKey = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003C_003CGetGizmos_003Eg__YieldGizmos_007C30_0_003Ed(-2)
		{
			_003C_003E3__eq = eq,
			_003C_003E3__preferredHotKey = preferredHotKey
		};
	}
}
