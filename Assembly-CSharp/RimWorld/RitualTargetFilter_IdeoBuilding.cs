using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class RitualTargetFilter_IdeoBuilding : RitualTargetFilter
{
	[CompilerGenerated]
	private sealed class _003CGetTargetInfos_003Ed__6 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private TargetInfo initiator;

		public TargetInfo _003C_003E3__initiator;

		private HashSet<Precept_Building>.Enumerator _003C_003E7__wrap1;

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
		public _003CGetTargetInfos_003Ed__6(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(HashSet<Precept_Building>.Enumerator);
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
				{
					_003C_003E1__state = -1;
					Ideo ideo = ((Pawn)initiator.Thing).Ideo;
					_003C_003E7__wrap1 = ideo.cachedPossibleBuildings.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0083;
				}
				case 1:
					_003C_003E1__state = -3;
					goto IL_0083;
				case 2:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0083:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Precept_Building current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current.LabelCap;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(HashSet<Precept_Building>.Enumerator);
					_003C_003E2__current = "RitualTargetGatherSpotInfo".Translate();
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CGetTargetInfos_003Ed__6 _003CGetTargetInfos_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetTargetInfos_003Ed__ = this;
			}
			else
			{
				_003CGetTargetInfos_003Ed__ = new _003CGetTargetInfos_003Ed__6(0);
			}
			_003CGetTargetInfos_003Ed__.initiator = _003C_003E3__initiator;
			return _003CGetTargetInfos_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	private List<Thing> candidateSpots = new List<Thing>();

	public RitualTargetFilter_IdeoBuilding()
	{
	}

	public RitualTargetFilter_IdeoBuilding(RitualTargetFilterDef def)
		: base(def)
	{
	}

	public override bool CanStart(TargetInfo initiator, TargetInfo selectedTarget, out string rejectionReason)
	{
		TargetInfo targetInfo = BestTarget(initiator, selectedTarget);
		rejectionReason = "";
		if (!targetInfo.IsValid)
		{
			rejectionReason = "AbilitySpeechDisabledNoSpot".Translate();
			return false;
		}
		return true;
	}

	public override TargetInfo BestTarget(TargetInfo initiator, TargetInfo selectedTarget)
	{
		Pawn pawn = (Pawn)initiator.Thing;
		Ideo ideo = pawn.Ideo;
		candidateSpots.Clear();
		for (int i = 0; i < ideo.PreceptsListForReading.Count; i++)
		{
			if (ideo.PreceptsListForReading[i] is Precept_Building precept_Building)
			{
				Thing thing = precept_Building.presenceDemand.BestBuilding(pawn.Map);
				if (thing != null && pawn.CanReach(thing, PathEndMode.Touch, pawn.NormalMaxDanger()))
				{
					candidateSpots.Add(thing);
				}
			}
		}
		candidateSpots.AddRange(ExtraCandidates(initiator));
		if (!candidateSpots.NullOrEmpty())
		{
			return candidateSpots.RandomElement();
		}
		if (!def.fallBackToGatherSpot)
		{
			return TargetInfo.Invalid;
		}
		candidateSpots.Clear();
		for (int j = 0; j < pawn.Map.gatherSpotLister.activeSpots.Count; j++)
		{
			ThingWithComps parent = pawn.Map.gatherSpotLister.activeSpots[j].parent;
			if (pawn.CanReach(parent, PathEndMode.Touch, pawn.NormalMaxDanger()))
			{
				candidateSpots.Add(parent);
			}
		}
		if (!candidateSpots.NullOrEmpty())
		{
			return candidateSpots.OrderBy((Thing s) => s.Position.DistanceTo(pawn.Position)).First();
		}
		return TargetInfo.Invalid;
	}

	protected virtual IEnumerable<Thing> ExtraCandidates(TargetInfo initiator)
	{
		return Enumerable.Empty<Thing>();
	}

	[IteratorStateMachine(typeof(_003CGetTargetInfos_003Ed__6))]
	public override IEnumerable<string> GetTargetInfos(TargetInfo initiator)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTargetInfos_003Ed__6(-2)
		{
			_003C_003E3__initiator = initiator
		};
	}
}
