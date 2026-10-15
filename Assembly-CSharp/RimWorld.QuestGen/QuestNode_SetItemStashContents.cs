using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen;

public class QuestNode_SetItemStashContents : QuestNode
{
	public class ThingCategoryCount
	{
		public ThingCategoryDef category;

		public IntRange amount;

		public bool allowDuplicates = true;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass6_0
	{
		public ThingCategoryCount c;

		public Func<ThingDef, bool> _003C_003E9__0;

		internal bool _003CGetContents_003Eb__0(ThingDef x)
		{
			if (x.thingCategories != null && x.thingCategories.Contains(c.category))
			{
				if (!c.allowDuplicates)
				{
					return !tmpItems.Contains(x);
				}
				return true;
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetContents_003Ed__6 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestNode_SetItemStashContents _003C_003E4__this;

		private Slate slate;

		public Slate _003C_003E3__slate;

		private _003C_003Ec__DisplayClass6_0 _003C_003E8__1;

		private IEnumerator<ThingDef> _003C_003E7__wrap1;

		private List<ThingCategoryCount>.Enumerator _003C_003E7__wrap2;

		private int _003Camt_003E5__4;

		private int _003Ci_003E5__5;

		ThingDef IEnumerator<ThingDef>.Current
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
		public _003CGetContents_003Ed__6(int _003C_003E1__state)
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
			case -5:
			case -4:
			case 2:
				try
				{
					if (num == -5 || num == 2)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally3();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<ThingCategoryCount>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				QuestNode_SetItemStashContents questNode_SetItemStashContents = _003C_003E4__this;
				List<ThingCategoryCount> value2;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					IEnumerable<ThingDef> value = questNode_SetItemStashContents.items.GetValue(slate);
					if (value != null)
					{
						_003C_003E7__wrap1 = value.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0084;
					}
					goto IL_009e;
				}
				case 1:
					_003C_003E1__state = -3;
					goto IL_0084;
				case 2:
					{
						_003C_003E1__state = -5;
						goto IL_018f;
					}
					IL_01bf:
					if (_003C_003E7__wrap2.MoveNext())
					{
						_003C_003E8__1 = new _003C_003Ec__DisplayClass6_0();
						_003C_003E8__1.c = _003C_003E7__wrap2.Current;
						_003C_003E1__state = -5;
						_003Camt_003E5__4 = Mathf.Max(_003C_003E8__1.c.amount.RandomInRange, 1);
						_003Ci_003E5__5 = 0;
						goto IL_01a1;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<ThingCategoryCount>.Enumerator);
					break;
					IL_0084:
					if (_003C_003E7__wrap1.MoveNext())
					{
						ThingDef current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					goto IL_009e;
					IL_018f:
					_003Ci_003E5__5++;
					goto IL_01a1;
					IL_009e:
					value2 = questNode_SetItemStashContents.categories.GetValue(slate);
					if (value2 == null)
					{
						break;
					}
					_003C_003E7__wrap2 = value2.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_01bf;
					IL_01a1:
					if (_003Ci_003E5__5 < _003Camt_003E5__4)
					{
						if (DefDatabase<ThingDef>.AllDefs.Where((ThingDef x) => x.thingCategories != null && x.thingCategories.Contains(_003C_003E8__1.c.category) && (_003C_003E8__1.c.allowDuplicates || !tmpItems.Contains(x))).TryRandomElement(out var result))
						{
							tmpItems.Add(result);
							_003C_003E2__current = result;
							_003C_003E1__state = 2;
							return true;
						}
						goto IL_018f;
					}
					_003C_003Em__Finally3();
					_003C_003E8__1 = null;
					goto IL_01bf;
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -4;
			tmpItems.Clear();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			_003CGetContents_003Ed__6 _003CGetContents_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetContents_003Ed__ = this;
			}
			else
			{
				_003CGetContents_003Ed__ = new _003CGetContents_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetContents_003Ed__.slate = _003C_003E3__slate;
			return _003CGetContents_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	public SlateRef<IEnumerable<ThingDef>> items;

	public SlateRef<List<ThingCategoryCount>> categories;

	private static List<ThingDef> tmpItems = new List<ThingDef>();

	protected override bool TestRunInt(Slate slate)
	{
		return true;
	}

	protected override void RunInt()
	{
		Slate slate = QuestGen.slate;
		slate.Set("itemStashThings", GetContents(slate));
	}

	[IteratorStateMachine(typeof(_003CGetContents_003Ed__6))]
	private IEnumerable<ThingDef> GetContents(Slate slate)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetContents_003Ed__6(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__slate = slate
		};
	}
}
