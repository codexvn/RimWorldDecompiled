using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class StorytellerComp_Disease : StorytellerComp
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass3_0
	{
		public BiomeDef biome;

		internal float _003CMakeIntervalIncidents_003Eb__0(IncidentDef d)
		{
			return biome.CommonalityOfDisease(d);
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeIntervalIncidents_003Ed__3 : IEnumerable<FiringIncident>, IEnumerable, IEnumerator<FiringIncident>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FiringIncident _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IIncidentTarget target;

		public IIncidentTarget _003C_003E3__target;

		public StorytellerComp_Disease _003C_003E4__this;

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
		public _003CMakeIntervalIncidents_003Ed__3(int _003C_003E1__state)
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
			StorytellerComp_Disease storytellerComp_Disease = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass3_0 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass3_0();
				if (!DebugSettings.enableRandomDiseases)
				{
					return false;
				}
				if (target is World)
				{
					return false;
				}
				if (Find.Storyteller.difficulty.diseaseIntervalFactor >= 100f)
				{
					return false;
				}
				if (target is Map map)
				{
					CS_0024_003C_003E8__locals5.biome = map.Biome;
				}
				else
				{
					CS_0024_003C_003E8__locals5.biome = Find.WorldGrid[target.Tile]?.PrimaryBiome;
				}
				if (CS_0024_003C_003E8__locals5.biome == null)
				{
					return false;
				}
				float diseaseMtbDays = CS_0024_003C_003E8__locals5.biome.diseaseMtbDays;
				diseaseMtbDays *= Find.Storyteller.difficulty.diseaseIntervalFactor;
				if (target is Caravan)
				{
					diseaseMtbDays *= storytellerComp_Disease.CaravanDiseaseMTBFactor;
				}
				if (Rand.MTBEventOccurs(diseaseMtbDays, 60000f, 1000f))
				{
					IncidentParms parms = storytellerComp_Disease.GenerateParms(storytellerComp_Disease.Props.category, target);
					if (storytellerComp_Disease.UsableIncidentsInCategory(storytellerComp_Disease.Props.category, parms).TryRandomElementByWeight((IncidentDef d) => CS_0024_003C_003E8__locals5.biome.CommonalityOfDisease(d), out var result))
					{
						_003C_003E2__current = new FiringIncident(result, storytellerComp_Disease, parms);
						_003C_003E1__state = 1;
						return true;
					}
				}
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
			_003CMakeIntervalIncidents_003Ed__3 _003CMakeIntervalIncidents_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CMakeIntervalIncidents_003Ed__ = this;
			}
			else
			{
				_003CMakeIntervalIncidents_003Ed__ = new _003CMakeIntervalIncidents_003Ed__3(0)
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

	private float CaravanDiseaseMTBFactor = 4f;

	protected StorytellerCompProperties_Disease Props => (StorytellerCompProperties_Disease)props;

	[IteratorStateMachine(typeof(_003CMakeIntervalIncidents_003Ed__3))]
	public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeIntervalIncidents_003Ed__3(-2)
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
