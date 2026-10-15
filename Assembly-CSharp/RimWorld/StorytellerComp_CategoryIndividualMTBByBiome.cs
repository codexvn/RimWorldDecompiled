using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class StorytellerComp_CategoryIndividualMTBByBiome : StorytellerComp
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public BiomeDef biome;

		internal bool _003CMakeIntervalIncidents_003Eb__0(MTBByBiome x)
		{
			return x.biome == biome;
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

		public StorytellerComp_CategoryIndividualMTBByBiome _003C_003E4__this;

		private List<IncidentDef> _003CallIncidents_003E5__2;

		private int _003Ci_003E5__3;

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
			_003CallIncidents_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			StorytellerComp_CategoryIndividualMTBByBiome storytellerComp_CategoryIndividualMTBByBiome = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_01a0;
			}
			_003C_003E1__state = -1;
			if (target is World)
			{
				return false;
			}
			_003CallIncidents_003E5__2 = DefDatabase<IncidentDef>.AllDefsListForReading;
			_003Ci_003E5__3 = 0;
			goto IL_01b2;
			IL_01b2:
			if (_003Ci_003E5__3 < _003CallIncidents_003E5__2.Count)
			{
				IncidentDef incidentDef = _003CallIncidents_003E5__2[_003Ci_003E5__3];
				if (incidentDef.category == storytellerComp_CategoryIndividualMTBByBiome.Props.category)
				{
					_003C_003Ec__DisplayClass2_0 CS_0024_003C_003E8__locals1 = new _003C_003Ec__DisplayClass2_0
					{
						biome = Find.WorldGrid[target.Tile].PrimaryBiome
					};
					if (incidentDef.mtbDaysByBiome != null)
					{
						MTBByBiome mTBByBiome = incidentDef.mtbDaysByBiome.Find((MTBByBiome x) => x.biome == CS_0024_003C_003E8__locals1.biome);
						if (mTBByBiome != null)
						{
							float num2 = mTBByBiome.mtbDays;
							if (storytellerComp_CategoryIndividualMTBByBiome.Props.applyCaravanVisibility)
							{
								if (target is Caravan caravan)
								{
									num2 /= caravan.Visibility;
								}
								else if (target is Map map && map.Parent.def.isTempIncidentMapOwner)
								{
									IEnumerable<Pawn> pawns = map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer).Concat(map.mapPawns.PrisonersOfColonySpawned);
									num2 /= CaravanVisibilityCalculator.Visibility(pawns, caravanMovingNow: false);
								}
							}
							if (Rand.MTBEventOccurs(num2, 60000f, 1000f))
							{
								IncidentParms parms = storytellerComp_CategoryIndividualMTBByBiome.GenerateParms(incidentDef.category, target);
								if (incidentDef.Worker.CanFireNow(parms))
								{
									_003C_003E2__current = new FiringIncident(incidentDef, storytellerComp_CategoryIndividualMTBByBiome, parms);
									_003C_003E1__state = 1;
									return true;
								}
							}
						}
					}
				}
				goto IL_01a0;
			}
			return false;
			IL_01a0:
			_003Ci_003E5__3++;
			goto IL_01b2;
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

	protected StorytellerCompProperties_CategoryIndividualMTBByBiome Props => (StorytellerCompProperties_CategoryIndividualMTBByBiome)props;

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

	public override string ToString()
	{
		return base.ToString() + " " + Props.category;
	}
}
