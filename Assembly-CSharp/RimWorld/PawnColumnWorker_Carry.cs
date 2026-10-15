using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class PawnColumnWorker_Carry : PawnColumnWorker
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public Pawn pawn;

		public InventoryStockGroupDef group;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_1
	{
		public ThingDef thing;

		public _003C_003Ec__DisplayClass2_0 CS_0024_003C_003E8__locals1;

		internal void _003CGenerateThingButtons_003Eb__0()
		{
			CS_0024_003C_003E8__locals1.pawn.inventoryStock.SetThingForGroup(CS_0024_003C_003E8__locals1.group, thing);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass3_0
	{
		public Pawn pawn;

		public InventoryStockGroupDef group;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass3_1
	{
		public int localI;

		public _003C_003Ec__DisplayClass3_0 CS_0024_003C_003E8__locals1;

		internal void _003CGenerateCountButtons_003Eb__0()
		{
			CS_0024_003C_003E8__locals1.pawn.inventoryStock.SetCountForGroup(CS_0024_003C_003E8__locals1.group, localI);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGenerateCountButtons_003Ed__3 : IEnumerable<Widgets.DropdownMenuElement<int>>, IEnumerable, IEnumerator<Widgets.DropdownMenuElement<int>>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Widgets.DropdownMenuElement<int> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private InventoryStockGroupDef group;

		public InventoryStockGroupDef _003C_003E3__group;

		private _003C_003Ec__DisplayClass3_0 _003C_003E8__1;

		private int _003Ci_003E5__2;

		Widgets.DropdownMenuElement<int> IEnumerator<Widgets.DropdownMenuElement<int>>.Current
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
		public _003CGenerateCountButtons_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass3_0();
				_003C_003E8__1.pawn = pawn;
				_003C_003E8__1.group = group;
				_003Ci_003E5__2 = _003C_003E8__1.group.min;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				break;
			}
			if (_003Ci_003E5__2 <= _003C_003E8__1.group.max)
			{
				_003C_003Ec__DisplayClass3_1 CS_0024_003C_003E8__locals3 = new _003C_003Ec__DisplayClass3_1
				{
					CS_0024_003C_003E8__locals1 = _003C_003E8__1,
					localI = _003Ci_003E5__2
				};
				_003C_003E2__current = new Widgets.DropdownMenuElement<int>
				{
					option = new FloatMenuOption(_003Ci_003E5__2.ToString(), delegate
					{
						CS_0024_003C_003E8__locals3.CS_0024_003C_003E8__locals1.pawn.inventoryStock.SetCountForGroup(CS_0024_003C_003E8__locals3.CS_0024_003C_003E8__locals1.group, CS_0024_003C_003E8__locals3.localI);
					}),
					payload = _003Ci_003E5__2
				};
				_003C_003E1__state = 1;
				return true;
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
		IEnumerator<Widgets.DropdownMenuElement<int>> IEnumerable<Widgets.DropdownMenuElement<int>>.GetEnumerator()
		{
			_003CGenerateCountButtons_003Ed__3 _003CGenerateCountButtons_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGenerateCountButtons_003Ed__ = this;
			}
			else
			{
				_003CGenerateCountButtons_003Ed__ = new _003CGenerateCountButtons_003Ed__3(0);
			}
			_003CGenerateCountButtons_003Ed__.pawn = _003C_003E3__pawn;
			_003CGenerateCountButtons_003Ed__.group = _003C_003E3__group;
			return _003CGenerateCountButtons_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Widgets.DropdownMenuElement<int>>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGenerateThingButtons_003Ed__2 : IEnumerable<Widgets.DropdownMenuElement<ThingDef>>, IEnumerable, IEnumerator<Widgets.DropdownMenuElement<ThingDef>>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Widgets.DropdownMenuElement<ThingDef> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private InventoryStockGroupDef group;

		public InventoryStockGroupDef _003C_003E3__group;

		private _003C_003Ec__DisplayClass2_0 _003C_003E8__1;

		private List<ThingDef>.Enumerator _003C_003E7__wrap1;

		Widgets.DropdownMenuElement<ThingDef> IEnumerator<Widgets.DropdownMenuElement<ThingDef>>.Current
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
		public _003CGenerateThingButtons_003Ed__2(int _003C_003E1__state)
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
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = default(List<ThingDef>.Enumerator);
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
					_003C_003E8__1 = new _003C_003Ec__DisplayClass2_0();
					_003C_003E8__1.pawn = pawn;
					_003C_003E8__1.group = group;
					_003C_003E7__wrap1 = _003C_003E8__1.group.thingDefs.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					_003C_003Ec__DisplayClass2_1 CS_0024_003C_003E8__locals6 = new _003C_003Ec__DisplayClass2_1
					{
						CS_0024_003C_003E8__locals1 = _003C_003E8__1,
						thing = _003C_003E7__wrap1.Current
					};
					_003C_003E2__current = new Widgets.DropdownMenuElement<ThingDef>
					{
						option = new FloatMenuOption(CS_0024_003C_003E8__locals6.thing.LabelCap, delegate
						{
							CS_0024_003C_003E8__locals6.CS_0024_003C_003E8__locals1.pawn.inventoryStock.SetThingForGroup(CS_0024_003C_003E8__locals6.CS_0024_003C_003E8__locals1.group, CS_0024_003C_003E8__locals6.thing);
						}, CS_0024_003C_003E8__locals6.thing, null, forceBasicStyle: false, MenuOptionPriority.Default, null, null, 0f, null, null, playSelectionSound: true, 0, null),
						payload = CS_0024_003C_003E8__locals6.thing
					};
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<ThingDef>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Widgets.DropdownMenuElement<ThingDef>> IEnumerable<Widgets.DropdownMenuElement<ThingDef>>.GetEnumerator()
		{
			_003CGenerateThingButtons_003Ed__2 _003CGenerateThingButtons_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGenerateThingButtons_003Ed__ = this;
			}
			else
			{
				_003CGenerateThingButtons_003Ed__ = new _003CGenerateThingButtons_003Ed__2(0);
			}
			_003CGenerateThingButtons_003Ed__.pawn = _003C_003E3__pawn;
			_003CGenerateThingButtons_003Ed__.group = _003C_003E3__group;
			return _003CGenerateThingButtons_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Widgets.DropdownMenuElement<ThingDef>>)this).GetEnumerator();
		}
	}

	private const int TopAreaHeight = 65;

	public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
	{
		if (pawn.inventoryStock != null)
		{
			float num = rect.width - 4f;
			int num2 = Mathf.FloorToInt(num * (1f / 3f));
			float x = rect.x;
			InventoryStockGroupDef group = InventoryStockGroupDefOf.Medicine;
			Widgets.Dropdown(new Rect(x, rect.y + 2f, num2, rect.height - 4f), pawn, (Pawn p) => p.inventoryStock.GetDesiredThingForGroup(group), (Pawn p) => GenerateThingButtons(p, group), null, pawn.inventoryStock.GetDesiredThingForGroup(group).uiIcon, null, null, null, paintable: true);
			Widgets.Dropdown(new Rect(x + (float)num2 + 4f, width: Mathf.FloorToInt(num * (2f / 3f)), y: rect.y + 2f, height: rect.height - 4f), pawn, (Pawn p) => p.inventoryStock.GetDesiredCountForGroup(group), (Pawn p) => GenerateCountButtons(p, group), pawn.inventoryStock.GetDesiredCountForGroup(group).ToString(), null, null, null, null, paintable: true);
		}
	}

	[IteratorStateMachine(typeof(_003CGenerateThingButtons_003Ed__2))]
	private IEnumerable<Widgets.DropdownMenuElement<ThingDef>> GenerateThingButtons(Pawn pawn, InventoryStockGroupDef group)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGenerateThingButtons_003Ed__2(-2)
		{
			_003C_003E3__pawn = pawn,
			_003C_003E3__group = group
		};
	}

	[IteratorStateMachine(typeof(_003CGenerateCountButtons_003Ed__3))]
	private IEnumerable<Widgets.DropdownMenuElement<int>> GenerateCountButtons(Pawn pawn, InventoryStockGroupDef group)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGenerateCountButtons_003Ed__3(-2)
		{
			_003C_003E3__pawn = pawn,
			_003C_003E3__group = group
		};
	}

	public override int GetMinWidth(PawnTable table)
	{
		return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(54f));
	}

	public override int GetOptimalWidth(PawnTable table)
	{
		return Mathf.Clamp(Mathf.CeilToInt(104f), GetMinWidth(table), GetMaxWidth(table));
	}

	public override int GetMinHeaderHeight(PawnTable table)
	{
		return Mathf.Max(base.GetMinHeaderHeight(table), 65);
	}

	public override int Compare(Pawn a, Pawn b)
	{
		return GetValueToCompare(a).CompareTo(GetValueToCompare(b));
	}

	private int GetValueToCompare(Pawn pawn)
	{
		if (pawn.inventoryStock != null)
		{
			return pawn.inventoryStock.GetDesiredCountForGroup(InventoryStockGroupDefOf.Medicine);
		}
		return int.MinValue;
	}
}
