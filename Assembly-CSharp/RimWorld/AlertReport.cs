using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public struct AlertReport
{
	[CompilerGenerated]
	private sealed class _003Cget_AllCulprits_003Ed__9 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public AlertReport _003C_003E4__this;

		public AlertReport _003C_003E3___003C_003E4__this;

		private int _003Ci_003E5__2;

		GlobalTargetInfo IEnumerator<GlobalTargetInfo>.Current
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
		public _003Cget_AllCulprits_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
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
				if (_003C_003E4__this.culpritsThings != null)
				{
					_003Ci_003E5__2 = 0;
					goto IL_0085;
				}
				goto IL_009d;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				goto IL_0085;
			case 2:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				goto IL_00f4;
			case 3:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				goto IL_0163;
			case 4:
				_003C_003E1__state = -1;
				goto IL_01b3;
			case 5:
				{
					_003C_003E1__state = -1;
					_003Ci_003E5__2++;
					goto IL_0205;
				}
				IL_0163:
				if (_003Ci_003E5__2 < _003C_003E4__this.culpritsCaravans.Count)
				{
					_003C_003E2__current = _003C_003E4__this.culpritsCaravans[_003Ci_003E5__2];
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_017b;
				IL_0085:
				if (_003Ci_003E5__2 < _003C_003E4__this.culpritsThings.Count)
				{
					_003C_003E2__current = _003C_003E4__this.culpritsThings[_003Ci_003E5__2];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_009d;
				IL_009d:
				if (_003C_003E4__this.culpritsPawns != null)
				{
					_003Ci_003E5__2 = 0;
					goto IL_00f4;
				}
				goto IL_010c;
				IL_017b:
				if (_003C_003E4__this.culpritTarget.HasValue)
				{
					_003C_003E2__current = _003C_003E4__this.culpritTarget.Value;
					_003C_003E1__state = 4;
					return true;
				}
				goto IL_01b3;
				IL_00f4:
				if (_003Ci_003E5__2 < _003C_003E4__this.culpritsPawns.Count)
				{
					_003C_003E2__current = _003C_003E4__this.culpritsPawns[_003Ci_003E5__2];
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_010c;
				IL_0205:
				if (_003Ci_003E5__2 < _003C_003E4__this.culpritsTargets.Count)
				{
					_003C_003E2__current = _003C_003E4__this.culpritsTargets[_003Ci_003E5__2];
					_003C_003E1__state = 5;
					return true;
				}
				break;
				IL_010c:
				if (_003C_003E4__this.culpritsCaravans != null)
				{
					_003Ci_003E5__2 = 0;
					goto IL_0163;
				}
				goto IL_017b;
				IL_01b3:
				if (_003C_003E4__this.culpritsTargets == null)
				{
					break;
				}
				_003Ci_003E5__2 = 0;
				goto IL_0205;
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
		IEnumerator<GlobalTargetInfo> IEnumerable<GlobalTargetInfo>.GetEnumerator()
		{
			_003Cget_AllCulprits_003Ed__9 _003Cget_AllCulprits_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003Cget_AllCulprits_003Ed__ = this;
			}
			else
			{
				_003Cget_AllCulprits_003Ed__ = new _003Cget_AllCulprits_003Ed__9(0);
			}
			_003Cget_AllCulprits_003Ed__._003C_003E4__this = _003C_003E3___003C_003E4__this;
			return _003Cget_AllCulprits_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GlobalTargetInfo>)this).GetEnumerator();
		}
	}

	public bool active;

	public List<Thing> culpritsThings;

	public List<Pawn> culpritsPawns;

	public List<Caravan> culpritsCaravans;

	public List<GlobalTargetInfo> culpritsTargets;

	public GlobalTargetInfo? culpritTarget;

	public bool AnyCulpritValid
	{
		get
		{
			if (!culpritsThings.NullOrEmpty() || !culpritsPawns.NullOrEmpty() || !culpritsCaravans.NullOrEmpty())
			{
				return true;
			}
			if (culpritTarget.HasValue && culpritTarget.Value.IsValid)
			{
				return true;
			}
			if (culpritsTargets != null)
			{
				for (int i = 0; i < culpritsTargets.Count; i++)
				{
					if (culpritsTargets[i].IsValid)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public IEnumerable<GlobalTargetInfo> AllCulprits
	{
		[IteratorStateMachine(typeof(_003Cget_AllCulprits_003Ed__9))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AllCulprits_003Ed__9(-2)
			{
				_003C_003E3___003C_003E4__this = this
			};
		}
	}

	public static AlertReport Active
	{
		get
		{
			AlertReport result = default(AlertReport);
			result.active = true;
			return result;
		}
	}

	public static AlertReport Inactive
	{
		get
		{
			AlertReport result = default(AlertReport);
			result.active = false;
			return result;
		}
	}

	public static AlertReport CulpritIs(GlobalTargetInfo culp)
	{
		AlertReport result = default(AlertReport);
		result.active = culp.IsValid;
		if (culp.IsValid)
		{
			result.culpritTarget = culp;
		}
		return result;
	}

	public static AlertReport CulpritsAre(List<Thing> culprits)
	{
		AlertReport result = default(AlertReport);
		result.culpritsThings = culprits;
		result.active = result.AnyCulpritValid;
		return result;
	}

	public static AlertReport CulpritsAre(List<Pawn> culprits)
	{
		AlertReport result = default(AlertReport);
		result.culpritsPawns = culprits;
		result.active = result.AnyCulpritValid;
		return result;
	}

	public static AlertReport CulpritsAre(List<Caravan> culprits)
	{
		AlertReport result = default(AlertReport);
		result.culpritsCaravans = culprits;
		result.active = result.AnyCulpritValid;
		return result;
	}

	public static AlertReport CulpritsAre(List<GlobalTargetInfo> culprits)
	{
		AlertReport result = default(AlertReport);
		result.culpritsTargets = culprits;
		result.active = result.AnyCulpritValid;
		return result;
	}

	public static implicit operator AlertReport(bool b)
	{
		AlertReport result = default(AlertReport);
		result.active = b;
		return result;
	}

	public static implicit operator AlertReport(Thing culprit)
	{
		return CulpritIs(culprit);
	}

	public static implicit operator AlertReport(WorldObject culprit)
	{
		return CulpritIs(culprit);
	}

	public static implicit operator AlertReport(GlobalTargetInfo culprit)
	{
		return CulpritIs(culprit);
	}
}
