using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StorytellerComp_RandomMain : StorytellerComp
{
	[CompilerGenerated]
	private sealed class _003CMakeIntervalIncidents_003Ed__2 : IEnumerable<FiringIncident>, IEnumerable, IEnumerator<FiringIncident>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FiringIncident _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StorytellerComp_RandomMain _003C_003E4__this;

		private IIncidentTarget target;

		public IIncidentTarget _003C_003E3__target;

		FiringIncident IEnumerator<FiringIncident>.Current
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
		public _003CMakeIntervalIncidents_003Ed__2(int _003C_003E1__state)
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
			StorytellerComp_RandomMain storytellerComp_RandomMain = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				float num2 = storytellerComp_RandomMain.Props.mtbDays;
				if (target.Tile.Valid && target.Tile.LayerDef.isSpace)
				{
					num2 *= storytellerComp_RandomMain.Props.spaceMtbDayFactor;
				}
				if (!Rand.MTBEventOccurs(num2, 60000f, 1000f))
				{
					return false;
				}
				if (target.Tile.Valid && target.Tile.LayerDef.isSpace && GenTicks.TicksGame - Find.Storyteller.LastIncidentTick < GenDate.DaysToTicks(storytellerComp_RandomMain.Props.spaceMinSpacingDays))
				{
					return false;
				}
				bool flag = target.IncidentTargetTags().Contains(IncidentTargetTagDefOf.Map_RaidBeacon);
				List<IncidentCategoryDef> list = new List<IncidentCategoryDef>();
				do
				{
					IncidentCategoryDef incidentCategoryDef = storytellerComp_RandomMain.ChooseRandomCategory(target, list);
					IncidentParms parms = storytellerComp_RandomMain.GenerateParms(incidentCategoryDef, target);
					if (storytellerComp_RandomMain.TrySelectRandomIncident(storytellerComp_RandomMain.UsableIncidentsInCategory(incidentCategoryDef, parms), out var foundDef, target))
					{
						if (storytellerComp_RandomMain.Props.skipThreatBigIfRaidBeacon && flag && foundDef.category == IncidentCategoryDefOf.ThreatBig)
						{
							break;
						}
						_003C_003E2__current = new FiringIncident(foundDef, storytellerComp_RandomMain, parms);
						_003C_003E1__state = 1;
						return true;
					}
					list.Add(incidentCategoryDef);
				}
				while (list.Count < storytellerComp_RandomMain.Props.categoryWeights.Count);
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
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
		IEnumerator<FiringIncident> IEnumerable<FiringIncident>.GetEnumerator()
		{
			_003CMakeIntervalIncidents_003Ed__2 _003CMakeIntervalIncidents_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CMakeIntervalIncidents_003Ed__ = this;
			}
			else
			{
				_003CMakeIntervalIncidents_003Ed__ = new _003CMakeIntervalIncidents_003Ed__2(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CMakeIntervalIncidents_003Ed__.target = _003C_003E3__target;
			return _003CMakeIntervalIncidents_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FiringIncident>)this).GetEnumerator();
		}
	}

	protected StorytellerCompProperties_RandomMain Props => (StorytellerCompProperties_RandomMain)props;

	[IteratorStateMachine(typeof(_003CMakeIntervalIncidents_003Ed__2))]
	public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeIntervalIncidents_003Ed__2(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__target = target
		};
	}

	private IncidentCategoryDef ChooseRandomCategory(IIncidentTarget target, List<IncidentCategoryDef> skipCategories)
	{
		if (!skipCategories.Contains(IncidentCategoryDefOf.ThreatBig) && (float)(Find.TickManager.TicksGame - target.StoryState.LastThreatBigTick) > 60000f * Props.maxThreatBigIntervalDays)
		{
			return IncidentCategoryDefOf.ThreatBig;
		}
		return Props.categoryWeights.Where((IncidentCategoryEntry cw) => !skipCategories.Contains(cw.category)).RandomElementByWeight((IncidentCategoryEntry cw) => cw.weight).category;
	}

	public override IncidentParms GenerateParms(IncidentCategoryDef incCat, IIncidentTarget target)
	{
		IncidentParms incidentParms = StorytellerUtility.DefaultParmsNow(incCat, target);
		if (incidentParms.points >= 0f)
		{
			incidentParms.points *= Props.randomPointsFactorRange.RandomInRange;
		}
		return incidentParms;
	}
}
