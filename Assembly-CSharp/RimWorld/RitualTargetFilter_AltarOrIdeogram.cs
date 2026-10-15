using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class RitualTargetFilter_AltarOrIdeogram : RitualTargetFilter_Altar
{
	[CompilerGenerated]
	private sealed class _003CCandidateBuildings_003Ed__4 : IEnumerable<Building>, IEnumerable, IEnumerator<Building>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Building _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Map map;

		public Map _003C_003E3__map;

		private Ideo ideo;

		public Ideo _003C_003E3__ideo;

		private IEnumerator<Precept_Building> _003C_003E7__wrap1;

		private List<Building>.Enumerator _003C_003E7__wrap2;

		Building IEnumerator<Building>.Current
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
		public _003CCandidateBuildings_003Ed__4(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if ((uint)(num - -4) <= 1u || num == 1)
			{
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<Building>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -4;
					goto IL_00cf;
				}
				_003C_003E1__state = -1;
				if (map == null)
				{
					return false;
				}
				_003C_003E7__wrap1 = ideo.cachedPossibleBuildings.Where((Precept_Building b) => b.ThingDef.isAltar || b.ThingDef == ThingDefOf.Ideogram).GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_00ee;
				IL_00ee:
				if (_003C_003E7__wrap1.MoveNext())
				{
					Precept_Building current = _003C_003E7__wrap1.Current;
					_003C_003E7__wrap2 = map.listerBuildings.AllBuildingsColonistOfDef(current.ThingDef).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00cf;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				return false;
				IL_00cf:
				if (_003C_003E7__wrap2.MoveNext())
				{
					Building current2 = _003C_003E7__wrap2.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = default(List<Building>.Enumerator);
				goto IL_00ee;
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
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Building> IEnumerable<Building>.GetEnumerator()
		{
			_003CCandidateBuildings_003Ed__4 _003CCandidateBuildings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCandidateBuildings_003Ed__ = this;
			}
			else
			{
				_003CCandidateBuildings_003Ed__ = new _003CCandidateBuildings_003Ed__4(0);
			}
			_003CCandidateBuildings_003Ed__.ideo = _003C_003E3__ideo;
			_003CCandidateBuildings_003Ed__.map = _003C_003E3__map;
			return _003CCandidateBuildings_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Building>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetTargetInfos_003Ed__3 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

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
		public _003CGetTargetInfos_003Ed__3(int _003C_003E1__state)
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
				_003C_003E2__current = "RitualTargetGatherAltarIdeogramOrRitualSpotInfo".Translate();
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CGetTargetInfos_003Ed__3(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public RitualTargetFilter_AltarOrIdeogram()
	{
	}

	public RitualTargetFilter_AltarOrIdeogram(RitualTargetFilterDef def)
		: base(def)
	{
	}

	public override bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason)
	{
		TargetInfo targetInfo = BestTarget(initiator, selectedTarget);
		rejectionReason = "";
		if (!targetInfo.IsValid)
		{
			rejectionReason = "AbilityDisabledNoAltarIdeogramOrRitualsSpot".Translate();
			return false;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetTargetInfos_003Ed__3))]
	public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTargetInfos_003Ed__3(-2);
	}

	[IteratorStateMachine(typeof(_003CCandidateBuildings_003Ed__4))]
	protected override IEnumerable<Building> CandidateBuildings(Ideo ideo, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCandidateBuildings_003Ed__4(-2)
		{
			_003C_003E3__ideo = ideo,
			_003C_003E3__map = map
		};
	}
}
