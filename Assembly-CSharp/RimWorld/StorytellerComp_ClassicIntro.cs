using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class StorytellerComp_ClassicIntro : StorytellerComp
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public IIncidentTarget target;

		public StorytellerComp_ClassicIntro _003C_003E4__this;

		internal float _003CMakeIntervalIncidents_003Eb__2(IncidentDef def)
		{
			return _003C_003E4__this.IncidentChanceFinal(def, target);
		}

		internal bool _003CMakeIntervalIncidents_003Eb__3(IncidentDef def)
		{
			if (def.TargetAllowed(target))
			{
				return def.category == IncidentCategoryDefOf.Misc;
			}
			return false;
		}

		internal float _003CMakeIntervalIncidents_003Eb__4(IncidentDef def)
		{
			return _003C_003E4__this.IncidentChanceFinal(def, target);
		}

		internal bool _003CMakeIntervalIncidents_003Eb__5(IncidentDef def)
		{
			if (def.TargetAllowed(target))
			{
				return def.category == IncidentCategoryDefOf.Misc;
			}
			return false;
		}

		internal float _003CMakeIntervalIncidents_003Eb__6(IncidentDef def)
		{
			return _003C_003E4__this.IncidentChanceFinal(def, target);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_1
	{
		public IncidentCategoryDef threatCategory;

		public _003C_003Ec__DisplayClass2_0 CS_0024_003C_003E8__locals1;

		internal bool _003CMakeIntervalIncidents_003Eb__1(IncidentDef def)
		{
			if (def.TargetAllowed(CS_0024_003C_003E8__locals1.target))
			{
				return def.category == threatCategory;
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeIntervalIncidents_003Ed__2 : IEnumerable<FiringIncident>, IEnumerable, IEnumerator<FiringIncident>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FiringIncident _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IIncidentTarget target;

		public IIncidentTarget _003C_003E3__target;

		public StorytellerComp_ClassicIntro _003C_003E4__this;

		private _003C_003Ec__DisplayClass2_0 _003C_003E8__1;

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
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			StorytellerComp_ClassicIntro storytellerComp_ClassicIntro = _003C_003E4__this;
			IncidentDef incidentDef;
			IncidentDef result;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass2_0();
				_003C_003E8__1.target = target;
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				if (_003C_003E8__1.target != Find.Maps.Find((Map x) => x.IsPlayerHome))
				{
					return false;
				}
				if (storytellerComp_ClassicIntro.IntervalsPassed == 150)
				{
					IncidentDef visitorGroup = IncidentDefOf.VisitorGroup;
					if (visitorGroup.TargetAllowed(_003C_003E8__1.target))
					{
						_003C_003E2__current = new FiringIncident(visitorGroup, storytellerComp_ClassicIntro)
						{
							parms = 
							{
								target = _003C_003E8__1.target,
								points = Rand.Range(40, 100)
							}
						};
						_003C_003E1__state = 1;
						return true;
					}
				}
				goto IL_0107;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0107;
			case 2:
				_003C_003E1__state = -1;
				goto IL_01c3;
			case 3:
				_003C_003E1__state = -1;
				goto IL_0246;
			case 4:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0246:
				if (storytellerComp_ClassicIntro.IntervalsPassed != 324)
				{
					break;
				}
				incidentDef = IncidentDefOf.RaidEnemy;
				if (!Find.Storyteller.difficulty.allowIntroThreats)
				{
					incidentDef = DefDatabase<IncidentDef>.AllDefs.Where((IncidentDef def) => def.TargetAllowed(_003C_003E8__1.target) && def.category == IncidentCategoryDefOf.Misc).RandomElementByWeightWithFallback((IncidentDef def) => _003C_003E8__1._003C_003E4__this.IncidentChanceFinal(def, _003C_003E8__1.target));
				}
				if (incidentDef != null && incidentDef.TargetAllowed(_003C_003E8__1.target))
				{
					FiringIncident firingIncident = new FiringIncident(incidentDef, storytellerComp_ClassicIntro);
					firingIncident.parms = storytellerComp_ClassicIntro.GenerateParms(incidentDef.category, _003C_003E8__1.target);
					firingIncident.parms.points = 40f;
					firingIncident.parms.raidForceOneDowned = true;
					firingIncident.parms.raidNeverFleeIndividual = true;
					_003C_003E2__current = firingIncident;
					_003C_003E1__state = 4;
					return true;
				}
				break;
				IL_01c3:
				if (storytellerComp_ClassicIntro.IntervalsPassed == 264 && DefDatabase<IncidentDef>.AllDefs.Where((IncidentDef def) => def.TargetAllowed(_003C_003E8__1.target) && def.category == IncidentCategoryDefOf.Misc).TryRandomElementByWeight((IncidentDef def) => _003C_003E8__1._003C_003E4__this.IncidentChanceFinal(def, _003C_003E8__1.target), out result))
				{
					_003C_003E2__current = new FiringIncident(result, storytellerComp_ClassicIntro)
					{
						parms = StorytellerUtility.DefaultParmsNow(result.category, _003C_003E8__1.target)
					};
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_0246;
				IL_0107:
				if (storytellerComp_ClassicIntro.IntervalsPassed == 204)
				{
					_003C_003Ec__DisplayClass2_1 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass2_1
					{
						CS_0024_003C_003E8__locals1 = _003C_003E8__1,
						threatCategory = (Find.Storyteller.difficulty.allowIntroThreats ? IncidentCategoryDefOf.ThreatSmall : IncidentCategoryDefOf.Misc)
					};
					if (DefDatabase<IncidentDef>.AllDefs.Where((IncidentDef def) => def.TargetAllowed(CS_0024_003C_003E8__locals5.CS_0024_003C_003E8__locals1.target) && def.category == CS_0024_003C_003E8__locals5.threatCategory).TryRandomElementByWeight((IncidentDef def) => CS_0024_003C_003E8__locals5.CS_0024_003C_003E8__locals1._003C_003E4__this.IncidentChanceFinal(def, CS_0024_003C_003E8__locals5.CS_0024_003C_003E8__locals1.target), out var result2))
					{
						_003C_003E2__current = new FiringIncident(result2, storytellerComp_ClassicIntro)
						{
							parms = StorytellerUtility.DefaultParmsNow(result2.category, CS_0024_003C_003E8__locals5.CS_0024_003C_003E8__locals1.target)
						};
						_003C_003E1__state = 2;
						return true;
					}
				}
				goto IL_01c3;
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

	protected int IntervalsPassed => Find.TickManager.TicksGame / 1000;

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
}
