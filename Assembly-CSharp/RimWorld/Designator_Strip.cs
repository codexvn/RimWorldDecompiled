using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Designator_Strip : Designator
{
	[CompilerGenerated]
	private sealed class _003CStrippablesInCell_003Ed__9 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IntVec3 c;

		public IntVec3 _003C_003E3__c;

		public Designator_Strip _003C_003E4__this;

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
		public _003CStrippablesInCell_003Ed__9(int _003C_003E1__state)
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
			Designator_Strip designator_Strip = _003C_003E4__this;
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
			if (c.Fogged(designator_Strip.Map))
			{
				return false;
			}
			_003CthingList_003E5__2 = c.GetThingList(designator_Strip.Map);
			_003Ci_003E5__3 = 0;
			goto IL_00ab;
			IL_00ab:
			if (_003Ci_003E5__3 < _003CthingList_003E5__2.Count)
			{
				if (designator_Strip.CanDesignateThing(_003CthingList_003E5__2[_003Ci_003E5__3]).Accepted)
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
			_003CStrippablesInCell_003Ed__9 _003CStrippablesInCell_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CStrippablesInCell_003Ed__ = this;
			}
			else
			{
				_003CStrippablesInCell_003Ed__ = new _003CStrippablesInCell_003Ed__9(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CStrippablesInCell_003Ed__.c = _003C_003E3__c;
			return _003CStrippablesInCell_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	protected override DesignationDef Designation => DesignationDefOf.Strip;

	public override DrawStyleCategoryDef DrawStyleCategory => DrawStyleCategoryDefOf.FilledRectangle;

	public Designator_Strip()
	{
		defaultLabel = "DesignatorStrip".Translate();
		defaultDesc = "DesignatorStripDesc".Translate();
		icon = ContentFinder<Texture2D>.Get("UI/Designators/Strip");
		soundDragSustain = SoundDefOf.Designate_DragStandard;
		soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
		useMouseIcon = true;
		soundSucceeded = SoundDefOf.Designate_Claim;
		hotKey = KeyBindingDefOf.Misc11;
	}

	public override AcceptanceReport CanDesignateCell(IntVec3 c)
	{
		if (!c.InBounds(base.Map))
		{
			return false;
		}
		if (!StrippablesInCell(c).Any())
		{
			return "MessageMustDesignateStrippable".Translate();
		}
		return true;
	}

	public override void DesignateSingleCell(IntVec3 c)
	{
		foreach (Thing item in StrippablesInCell(c))
		{
			DesignateThing(item);
		}
	}

	public override AcceptanceReport CanDesignateThing(Thing t)
	{
		if (base.Map.designationManager.DesignationOn(t, Designation) != null)
		{
			return false;
		}
		return StrippableUtility.CanBeStrippedByColony(t);
	}

	public override void DesignateThing(Thing t)
	{
		base.Map.designationManager.AddDesignation(new Designation(t, Designation));
		StrippableUtility.CheckSendStrippingImpactsGoodwillMessage(t);
	}

	[IteratorStateMachine(typeof(_003CStrippablesInCell_003Ed__9))]
	private IEnumerable<Thing> StrippablesInCell(IntVec3 c)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CStrippablesInCell_003Ed__9(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__c = c
		};
	}
}
