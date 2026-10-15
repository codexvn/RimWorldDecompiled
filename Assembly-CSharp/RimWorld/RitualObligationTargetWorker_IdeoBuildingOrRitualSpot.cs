using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class RitualObligationTargetWorker_IdeoBuildingOrRitualSpot : RitualObligationTargetFilter
{
	[CompilerGenerated]
	private sealed class _003CGetTargetInfos_003Ed__4 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RitualObligationTargetWorker_IdeoBuildingOrRitualSpot _003C_003E4__this;

		private int _003Ci_003E5__2;

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
		public _003CGetTargetInfos_003Ed__4(int _003C_003E1__state)
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
			int num = _003C_003E1__state;
			RitualObligationTargetWorker_IdeoBuildingOrRitualSpot ritualObligationTargetWorker_IdeoBuildingOrRitualSpot = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				goto IL_0086;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0076;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = "RitualTargetGatherSpotInfo".Translate();
				_003C_003E1__state = 3;
				return true;
			case 3:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_0086:
				if (_003Ci_003E5__2 < ritualObligationTargetWorker_IdeoBuildingOrRitualSpot.parent.ideo.PreceptsListForReading.Count)
				{
					if (ritualObligationTargetWorker_IdeoBuildingOrRitualSpot.parent.ideo.PreceptsListForReading[_003Ci_003E5__2] is Precept_Building precept_Building)
					{
						_003C_003E2__current = precept_Building.LabelCap;
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_0076;
				}
				_003C_003E2__current = ThingDefOf.RitualSpot.label;
				_003C_003E1__state = 2;
				return true;
				IL_0076:
				_003Ci_003E5__2++;
				goto IL_0086;
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
			_003CGetTargetInfos_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetTargetInfos_003Ed__4(0)
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

	public RitualObligationTargetWorker_IdeoBuildingOrRitualSpot()
	{
	}

	public RitualObligationTargetWorker_IdeoBuildingOrRitualSpot(RitualObligationTargetFilterDef def)
		: base(def)
	{
	}

	public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
	{
		return Enumerable.Empty<TargetInfo>();
	}

	protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
	{
		if (!ModLister.CheckIdeology("Ideo building target"))
		{
			return false;
		}
		if (!(target.Thing is Building { Faction: not null } building) || !building.Faction.IsPlayer)
		{
			return false;
		}
		if (building.def == ThingDefOf.RitualSpot)
		{
			return true;
		}
		for (int i = 0; i < parent.ideo.PreceptsListForReading.Count; i++)
		{
			if (parent.ideo.PreceptsListForReading[i] is Precept_Building precept_Building && building.def == precept_Building.ThingDef)
			{
				return true;
			}
		}
		if (building.TryGetComp<CompGatherSpot>() != null)
		{
			return true;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetTargetInfos_003Ed__4))]
	public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTargetInfos_003Ed__4(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override List<string> MissingTargetBuilding(Ideo ideo)
	{
		return null;
	}
}
