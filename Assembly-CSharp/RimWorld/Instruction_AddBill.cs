using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class Instruction_AddBill : Lesson_Instruction
{
	[CompilerGenerated]
	private sealed class _003CThingsToSelect_003Ed__3 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Instruction_AddBill _003C_003E4__this;

		private List<Building>.Enumerator _003C_003E7__wrap1;

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
		public _003CThingsToSelect_003Ed__3(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<Building>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Instruction_AddBill instruction_AddBill = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (Find.Selector.SingleSelectedThing == null || Find.Selector.SingleSelectedThing.def != instruction_AddBill.def.thingDef)
					{
						_003C_003E7__wrap1 = instruction_AddBill.Map.listerBuildings.AllBuildingsColonistOfDef(instruction_AddBill.def.thingDef).GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_00a4;
					}
					return false;
				case 1:
					{
						_003C_003E1__state = -3;
						goto IL_00a4;
					}
					IL_00a4:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Building current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<Building>.Enumerator);
					return false;
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CThingsToSelect_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CThingsToSelect_003Ed__3(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	protected override float ProgressPercent
	{
		get
		{
			int num = def.recipeTargetCount + 1;
			int num2 = 0;
			Bill_Production bill_Production = RelevantBill();
			if (bill_Production != null)
			{
				num2++;
				if (bill_Production.repeatMode == BillRepeatModeDefOf.RepeatCount)
				{
					num2 += bill_Production.repeatCount;
				}
			}
			return (float)num2 / (float)num;
		}
	}

	private Bill_Production RelevantBill()
	{
		if (Find.Selector.SingleSelectedThing != null && Find.Selector.SingleSelectedThing.def == def.thingDef && Find.Selector.SingleSelectedThing is IBillGiver billGiver)
		{
			return (Bill_Production)billGiver.BillStack.Bills.FirstOrDefault((Bill b) => b.recipe == def.recipeDef);
		}
		return null;
	}

	[IteratorStateMachine(typeof(_003CThingsToSelect_003Ed__3))]
	private IEnumerable<Thing> ThingsToSelect()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CThingsToSelect_003Ed__3(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void LessonOnGUI()
	{
		foreach (Thing item in ThingsToSelect())
		{
			TutorUtility.DrawLabelOnThingOnGUI(item, def.onMapInstruction);
		}
		if (RelevantBill() == null)
		{
			UIHighlighter.HighlightTag("AddBill");
		}
		base.LessonOnGUI();
	}

	public override void LessonUpdate()
	{
		foreach (Thing item in ThingsToSelect())
		{
			GenDraw.DrawArrowPointingAt(item.DrawPos);
		}
		if (ProgressPercent > 0.999f)
		{
			Find.ActiveLesson.Deactivate();
		}
	}
}
