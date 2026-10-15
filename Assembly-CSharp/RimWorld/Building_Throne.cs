using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class Building_Throne : Building
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__9 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_Throne _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

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
		public _003CGetGizmos_003Ed__9(int _003C_003E1__state)
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
				Building_Throne building_Throne = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!ModLister.CheckRoyalty("Throne"))
					{
						return false;
					}
					_003C_003E7__wrap1 = ((Building)building_Throne).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Gizmo current = _003C_003E7__wrap1.Current;
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
			_003CGetGizmos_003Ed__9 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__9(0)
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

	public static IEnumerable<RoyalTitleDef> AllTitlesForThroneStature => from title in DefDatabase<RoyalTitleDef>.AllDefsListForReading
		where !title.throneRoomRequirements.NullOrEmpty()
		orderby title.seniority
		select title;

	public Pawn AssignedPawn
	{
		get
		{
			if (!ModLister.CheckRoyalty("Throne"))
			{
				return null;
			}
			if (CompAssignableToPawn == null || !CompAssignableToPawn.AssignedPawnsForReading.Any())
			{
				return null;
			}
			return CompAssignableToPawn.AssignedPawnsForReading[0];
		}
	}

	public CompAssignableToPawn_Throne CompAssignableToPawn => GetComp<CompAssignableToPawn_Throne>();

	public RoyalTitleDef TitleStature
	{
		get
		{
			Room room = this.GetRoom();
			if (room == null || room.OutdoorsForWork)
			{
				return null;
			}
			RoyalTitleDef result = null;
			foreach (RoyalTitleDef item in AllTitlesForThroneStature)
			{
				bool flag = true;
				for (int i = 0; i < item.throneRoomRequirements.Count; i++)
				{
					if (!(item.throneRoomRequirements[i] is RoomRequirement_HasAssignedThroneAnyOf) && !item.throneRoomRequirements[i].MetOrDisabled(room))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					result = item;
					continue;
				}
				break;
			}
			return result;
		}
	}

	public override string GetInspectString()
	{
		string inspectString = base.GetInspectString();
		Room room = this.GetRoom();
		Pawn p = ((CompAssignableToPawn.AssignedPawnsForReading.Count == 1) ? CompAssignableToPawn.AssignedPawnsForReading[0] : null);
		RoyalTitleDef titleStature = TitleStature;
		inspectString += "\n" + "ThroneMaxSatisfiedTitle".Translate() + ": " + ((titleStature == null) ? "None".Translate() : ((TaggedString)titleStature.GetLabelCapFor(p)));
		string text = RoomRoleWorker_ThroneRoom.Validate(room);
		if (text != null)
		{
			return inspectString + "\n" + text;
		}
		return inspectString;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__9))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__9(-2)
		{
			_003C_003E4__this = this
		};
	}
}
