using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class RoomRequirement_ThingAnyOf : RoomRequirement
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__4 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RoomRequirement_ThingAnyOf _003C_003E4__this;

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
				RoomRequirement_ThingAnyOf roomRequirement_ThingAnyOf = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((RoomRequirement)roomRequirement_ThingAnyOf).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (roomRequirement_ThingAnyOf.things.NullOrEmpty())
					{
						_003C_003E2__current = "things are null or empty";
						_003C_003E1__state = 2;
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
			_003CConfigErrors_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public List<ThingDef> things;

	public override string Label(Room r = null)
	{
		return ((!labelKey.NullOrEmpty()) ? ((string)labelKey.Translate()) : things[0].label) + ((r != null) ? " 0/1" : "");
	}

	public override bool Met(Room r, Pawn p = null)
	{
		foreach (ThingDef thing in things)
		{
			if (r.ContainsThing(thing))
			{
				return true;
			}
		}
		return false;
	}

	public override bool SameOrSubsetOf(RoomRequirement other)
	{
		if (!base.SameOrSubsetOf(other))
		{
			return false;
		}
		RoomRequirement_ThingAnyOf roomRequirement_ThingAnyOf = (RoomRequirement_ThingAnyOf)other;
		foreach (ThingDef thing in things)
		{
			if (!roomRequirement_ThingAnyOf.things.Contains(thing))
			{
				return false;
			}
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__4))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__4(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override bool PlayerCanBuildNow()
	{
		for (int i = 0; i < things.Count; i++)
		{
			if (things[i].IsResearchFinished)
			{
				return true;
			}
		}
		return false;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref things, "things", LookMode.Def);
	}
}
