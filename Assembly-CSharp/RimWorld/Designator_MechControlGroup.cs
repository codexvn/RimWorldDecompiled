using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Designator_MechControlGroup : Designator
{
	[CompilerGenerated]
	private sealed class _003Cget_SelectedControlGroups_003Ed__2 : IEnumerable<MechanitorControlGroup>, IEnumerable, IEnumerator<MechanitorControlGroup>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private MechanitorControlGroup _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private List<Pawn>.Enumerator _003C_003E7__wrap1;

		private List<MechanitorControlGroup>.Enumerator _003C_003E7__wrap2;

		MechanitorControlGroup IEnumerator<MechanitorControlGroup>.Current
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
		public _003Cget_SelectedControlGroups_003Ed__2(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
			_003C_003E7__wrap2 = default(List<MechanitorControlGroup>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -4;
					goto IL_00cd;
				}
				_003C_003E1__state = -1;
				tmpControlGroups.Clear();
				_003C_003E7__wrap1 = Find.Selector.SelectedPawns.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_00ec;
				IL_00cd:
				while (_003C_003E7__wrap2.MoveNext())
				{
					MechanitorControlGroup current = _003C_003E7__wrap2.Current;
					if (current != null && tmpControlGroups.Add(current))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = default(List<MechanitorControlGroup>.Enumerator);
				goto IL_00ec;
				IL_00ec:
				Pawn current2;
				while (true)
				{
					if (_003C_003E7__wrap1.MoveNext())
					{
						current2 = _003C_003E7__wrap1.Current;
						if (current2 != null)
						{
							Pawn pawn = current2;
							if (pawn.IsColonyMech)
							{
								break;
							}
						}
						continue;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<Pawn>.Enumerator);
					return false;
				}
				Pawn overseer = current2.GetOverseer();
				_003C_003E7__wrap2 = overseer.mechanitor.controlGroups.GetEnumerator();
				_003C_003E1__state = -4;
				goto IL_00cd;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<MechanitorControlGroup> IEnumerable<MechanitorControlGroup>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003Cget_SelectedControlGroups_003Ed__2(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<MechanitorControlGroup>)this).GetEnumerator();
		}
	}

	private static readonly HashSet<MechanitorControlGroup> tmpControlGroups = new HashSet<MechanitorControlGroup>();

	private static List<MechanitorControlGroup> tmpSelectedControlGroups = new List<MechanitorControlGroup>();

	public static IEnumerable<MechanitorControlGroup> SelectedControlGroups
	{
		[IteratorStateMachine(typeof(_003Cget_SelectedControlGroups_003Ed__2))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_SelectedControlGroups_003Ed__2(-2);
		}
	}

	public Designator_MechControlGroup()
	{
		defaultLabel = "DesignatorMechControlGroup".Translate() + "...";
		defaultDesc = "DesignatorMechControlGroupDesc".Translate();
		icon = ContentFinder<Texture2D>.Get("UI/Gizmos/MechControlGroup");
		showReverseDesignatorDisabledReason = true;
	}

	public override AcceptanceReport CanDesignateCell(IntVec3 c)
	{
		if (!c.InBounds(base.Map))
		{
			return false;
		}
		foreach (Thing thing in c.GetThingList(base.Map))
		{
			if (CanDesignateThing(thing).Accepted)
			{
				return true;
			}
		}
		return false;
	}

	public override AcceptanceReport CanDesignateThing(Thing t)
	{
		if (!(t is Pawn { IsColonyMech: not false } pawn) || pawn.GetOverseer() == null)
		{
			return false;
		}
		Pawn pawn2 = null;
		foreach (Pawn selectedPawn in Find.Selector.SelectedPawns)
		{
			Pawn overseer = selectedPawn.GetOverseer();
			if (pawn2 != null && pawn2 != overseer)
			{
				return false;
			}
			pawn2 = overseer;
		}
		AcceptanceReport canControlMechs = pawn2.mechanitor.CanControlMechs;
		if (!canControlMechs)
		{
			if (!canControlMechs.Reason.NullOrEmpty())
			{
				return ("DisabledCommand".Translate() + ": " + canControlMechs.Reason).Colorize(ColorLibrary.RedReadable);
			}
			return false;
		}
		return true;
	}

	public override void DesignateThing(Thing t)
	{
		ProcessInput(null);
	}

	private bool CanAssignToControlGroup(Pawn pawn, MechanitorControlGroup group)
	{
		Pawn overseer = pawn.GetOverseer();
		if (overseer != null)
		{
			return group.Tracker.Pawn == overseer;
		}
		return false;
	}

	public override void ProcessInput(Event ev)
	{
		tmpSelectedControlGroups.Clear();
		tmpSelectedControlGroups.AddRange(SelectedControlGroups);
		List<FloatMenuOption> list = new List<FloatMenuOption>();
		for (int i = 0; i < tmpSelectedControlGroups.Count; i++)
		{
			MechanitorControlGroup selected = tmpSelectedControlGroups[i];
			if (Find.Selector.SelectedPawns.All((Pawn p) => p.GetMechControlGroup() == selected))
			{
				FloatMenuOption item = new FloatMenuOption("CannotAssignMechToControlGroup".Translate(selected.Index) + " (" + selected.WorkMode.LabelCap.ToString() + ")" + ": " + "AssignMechAlreadyAssigned".Translate(), null);
				list.Add(item);
				continue;
			}
			FloatMenuOption item2 = new FloatMenuOption("AssignMechToControlGroup".Translate(selected.Index) + " (" + selected.WorkMode.LabelCap.ToString() + ")", delegate
			{
				foreach (Pawn selectedPawn in Find.Selector.SelectedPawns)
				{
					if (CanAssignToControlGroup(selectedPawn, selected))
					{
						selected.Assign(selectedPawn);
					}
				}
			});
			list.Add(item2);
		}
		Find.WindowStack.Add(new FloatMenu(list));
		tmpSelectedControlGroups.Clear();
	}

	public override void SelectedUpdate()
	{
	}
}
