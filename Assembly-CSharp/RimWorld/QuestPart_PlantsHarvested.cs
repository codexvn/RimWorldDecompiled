using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class QuestPart_PlantsHarvested : QuestPartActivable
{
	[CompilerGenerated]
	private sealed class _003Cget_Hyperlinks_003Ed__6 : IEnumerable<Dialog_InfoCard.Hyperlink>, IEnumerable, IEnumerator<Dialog_InfoCard.Hyperlink>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Dialog_InfoCard.Hyperlink _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_PlantsHarvested _003C_003E4__this;

		private IEnumerator<Dialog_InfoCard.Hyperlink> _003C_003E7__wrap1;

		Dialog_InfoCard.Hyperlink IEnumerator<Dialog_InfoCard.Hyperlink>.Current
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
		public _003Cget_Hyperlinks_003Ed__6(int _003C_003E1__state)
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
				QuestPart_PlantsHarvested questPart_PlantsHarvested = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_PlantsHarvested).Hyperlinks.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Dialog_InfoCard.Hyperlink current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E2__current = new Dialog_InfoCard.Hyperlink(questPart_PlantsHarvested.plant);
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
		IEnumerator<Dialog_InfoCard.Hyperlink> IEnumerable<Dialog_InfoCard.Hyperlink>.GetEnumerator()
		{
			_003Cget_Hyperlinks_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Hyperlinks_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Dialog_InfoCard.Hyperlink>)this).GetEnumerator();
		}
	}

	public ThingDef plant;

	public int count;

	private int harvested;

	public override string DescriptionPart => string.Concat("PlantsHarvested".Translate().CapitalizeFirst() + ": ", harvested.ToString(), " / ", count.ToString());

	public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
	{
		[IteratorStateMachine(typeof(_003Cget_Hyperlinks_003Ed__6))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Hyperlinks_003Ed__6(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	protected override void Enable(SignalArgs receivedArgs)
	{
		base.Enable(receivedArgs);
		harvested = 0;
	}

	public override void Notify_PlantHarvested(Pawn actor, Thing harvested)
	{
		base.Notify_PlantHarvested(actor, harvested);
		if (base.State == QuestPartState.Enabled && harvested.def == plant)
		{
			this.harvested += harvested.stackCount;
			if (this.harvested >= count)
			{
				this.harvested = count;
				Complete();
			}
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref plant, "plant");
		Scribe_Values.Look(ref count, "count", 0);
		Scribe_Values.Look(ref harvested, "harvested", 0);
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		plant = ThingDefOf.RawPotatoes;
		count = 10;
	}
}
