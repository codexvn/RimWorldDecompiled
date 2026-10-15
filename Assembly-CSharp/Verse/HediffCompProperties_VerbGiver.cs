using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Verse;

public class HediffCompProperties_VerbGiver : HediffCompProperties
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass5_0
	{
		public Tool lhs;

		internal bool _003CConfigErrors_003Eb__1(Tool rhs)
		{
			if (lhs != rhs)
			{
				return lhs.id == rhs.id;
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__5 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public HediffCompProperties_VerbGiver _003C_003E4__this;

		private HediffDef parentDef;

		public HediffDef _003C_003E3__parentDef;

		private IEnumerator<string> _003C_003E7__wrap1;

		private List<Tool>.Enumerator _003C_003E7__wrap2;

		string IEnumerator<string>.Current
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
		public _003CConfigErrors_003Ed__5(int _003C_003E1__state)
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
			case 3:
				try
				{
					if (num == -5 || num == 3)
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
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<Tool>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				HediffCompProperties_VerbGiver CS_0024_003C_003E8__locals8 = _003C_003E4__this;
				Tool tool;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((HediffCompProperties)CS_0024_003C_003E8__locals8).ConfigErrors(parentDef).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007c;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007c;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00ef;
				case 3:
					{
						_003C_003E1__state = -5;
						goto IL_015b;
					}
					IL_007c:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals8.tools == null)
					{
						break;
					}
					tool = CS_0024_003C_003E8__locals8.tools.SelectMany(delegate(Tool lhs)
					{
						_003C_003Ec__DisplayClass5_0 CS_0024_003C_003E8__locals7 = new _003C_003Ec__DisplayClass5_0
						{
							lhs = lhs
						};
						return CS_0024_003C_003E8__locals8.tools.Where((Tool rhs) => CS_0024_003C_003E8__locals7.lhs != rhs && CS_0024_003C_003E8__locals7.lhs.id == rhs.id);
					}).FirstOrDefault();
					if (tool != null)
					{
						_003C_003E2__current = $"duplicate hediff tool id {tool.id}";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00ef;
					IL_0175:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Tool current2 = _003C_003E7__wrap2.Current;
						_003C_003E7__wrap1 = current2.ConfigErrors().GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_015b;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<Tool>.Enumerator);
					break;
					IL_015b:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current3 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					goto IL_0175;
					IL_00ef:
					_003C_003E7__wrap2 = CS_0024_003C_003E8__locals8.tools.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0175;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__5 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__5(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CConfigErrors_003Ed__.parentDef = _003C_003E3__parentDef;
			return _003CConfigErrors_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public List<VerbProperties> verbs;

	public List<Tool> tools;

	public ImplementOwnerTypeDef ownerTypeOverride;

	public HediffCompProperties_VerbGiver()
	{
		compClass = typeof(HediffComp_VerbGiver);
	}

	public override void PostLoad()
	{
		base.PostLoad();
		if (tools != null)
		{
			for (int i = 0; i < tools.Count; i++)
			{
				tools[i].id = i.ToString();
			}
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__5))]
	public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__5(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parentDef = parentDef
		};
	}
}
