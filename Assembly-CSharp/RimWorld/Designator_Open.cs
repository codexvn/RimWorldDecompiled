using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Designator_Open : Designator
{
	[CompilerGenerated]
	private sealed class _003COpenablesInCell_003Ed__10 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IntVec3 c;

		public IntVec3 _003C_003E3__c;

		public Designator_Open _003C_003E4__this;

		private List<Thing> _003CthingList_003E5__2;

		private int _003Ci_003E5__3;

		Thing IEnumerator<Thing>.Current
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
		public _003COpenablesInCell_003Ed__10(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CthingList_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Designator_Open designator_Open = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_009b;
			}
			_003C_003E1__state = -1;
			if (c.Fogged(designator_Open.Map))
			{
				return false;
			}
			_003CthingList_003E5__2 = c.GetThingList(designator_Open.Map);
			_003Ci_003E5__3 = 0;
			goto IL_00ab;
			IL_00ab:
			if (_003Ci_003E5__3 < _003CthingList_003E5__2.Count)
			{
				if (designator_Open.CanDesignateThing(_003CthingList_003E5__2[_003Ci_003E5__3]).Accepted)
				{
					_003C_003E2__current = _003CthingList_003E5__2[_003Ci_003E5__3];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_009b;
			}
			return false;
			IL_009b:
			_003Ci_003E5__3++;
			goto IL_00ab;
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003COpenablesInCell_003Ed__10 _003COpenablesInCell_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003COpenablesInCell_003Ed__ = this;
			}
			else
			{
				_003COpenablesInCell_003Ed__ = new _003COpenablesInCell_003Ed__10(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003COpenablesInCell_003Ed__.c = _003C_003E3__c;
			return _003COpenablesInCell_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	protected override DesignationDef Designation => DesignationDefOf.Open;

	public override DrawStyleCategoryDef DrawStyleCategory => DrawStyleCategoryDefOf.FilledRectangle;

	public Designator_Open()
	{
		defaultLabel = "DesignatorOpen".Translate();
		defaultDesc = "DesignatorOpenDesc".Translate();
		icon = ContentFinder<Texture2D>.Get("UI/Designators/Open");
		soundDragSustain = SoundDefOf.Designate_DragStandard;
		soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
		hotKey = KeyBindingDefOf.Misc5;
		useMouseIcon = true;
		soundSucceeded = SoundDefOf.Designate_Claim;
	}

	protected override void FinalizeDesignationFailed()
	{
		base.FinalizeDesignationFailed();
		Messages.Message("MessageMustDesignateOpenable".Translate(), MessageTypeDefOf.RejectInput, historical: false);
	}

	public override AcceptanceReport CanDesignateCell(IntVec3 c)
	{
		if (!c.InBounds(base.Map))
		{
			return false;
		}
		if (!OpenablesInCell(c).Any())
		{
			return false;
		}
		return true;
	}

	public override void DesignateSingleCell(IntVec3 c)
	{
		foreach (Thing item in OpenablesInCell(c))
		{
			DesignateThing(item);
		}
	}

	public override AcceptanceReport CanDesignateThing(Thing t)
	{
		if (!(t is IOpenable { CanOpen: not false }) || base.Map.designationManager.DesignationOn(t, Designation) != null)
		{
			return false;
		}
		return true;
	}

	public override void DesignateThing(Thing t)
	{
		base.Map.designationManager.AddDesignation(new Designation(t, Designation));
	}

	[IteratorStateMachine(typeof(_003COpenablesInCell_003Ed__10))]
	private IEnumerable<Thing> OpenablesInCell(IntVec3 c)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003COpenablesInCell_003Ed__10(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__c = c
		};
	}
}
