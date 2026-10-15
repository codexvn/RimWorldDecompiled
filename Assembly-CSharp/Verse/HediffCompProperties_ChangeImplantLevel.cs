using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse;

public class HediffCompProperties_ChangeImplantLevel : HediffCompProperties
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__4 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public HediffCompProperties_ChangeImplantLevel _003C_003E4__this;

		private HediffDef parentDef;

		public HediffDef _003C_003E3__parentDef;

		private IEnumerator<string> _003C_003E7__wrap1;

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
		public _003CConfigErrors_003Ed__4(int _003C_003E1__state)
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
				HediffCompProperties_ChangeImplantLevel hediffCompProperties_ChangeImplantLevel = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((HediffCompProperties)hediffCompProperties_ChangeImplantLevel).ConfigErrors(parentDef).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0088;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0088;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0108;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0108;
				case 4:
					_003C_003E1__state = -1;
					goto IL_012d;
				case 5:
					_003C_003E1__state = -1;
					break;
				case 6:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0088:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (hediffCompProperties_ChangeImplantLevel.implant == null)
					{
						_003C_003E2__current = "implant is null";
						_003C_003E1__state = 2;
						return true;
					}
					if (!typeof(Hediff_Level).IsAssignableFrom(hediffCompProperties_ChangeImplantLevel.implant.hediffClass))
					{
						_003C_003E2__current = "implant is not Hediff_Level";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0108;
					IL_0108:
					if (hediffCompProperties_ChangeImplantLevel.levelOffset == 0)
					{
						_003C_003E2__current = "levelOffset is 0";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_012d;
					IL_012d:
					if (hediffCompProperties_ChangeImplantLevel.probabilityPerStage == null)
					{
						_003C_003E2__current = "probabilityPerStage is not defined";
						_003C_003E1__state = 5;
						return true;
					}
					if (hediffCompProperties_ChangeImplantLevel.probabilityPerStage.Count != parentDef.stages.Count)
					{
						_003C_003E2__current = "probabilityPerStage count doesn't match Hediffs number of stages";
						_003C_003E1__state = 6;
						return true;
					}
					break;
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__4 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__4(0)
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

	public HediffDef implant;

	public int levelOffset;

	public List<ChangeImplantLevel_Probability> probabilityPerStage;

	public HediffCompProperties_ChangeImplantLevel()
	{
		compClass = typeof(HediffComp_ChangeImplantLevel);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__4))]
	public override IEnumerable<string> ConfigErrors(HediffDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__4(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parentDef = parentDef
		};
	}
}
