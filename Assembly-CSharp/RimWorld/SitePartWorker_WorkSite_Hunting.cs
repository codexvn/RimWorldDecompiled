using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class SitePartWorker_WorkSite_Hunting : SitePartWorker_WorkSite
{
	[CompilerGenerated]
	private sealed class _003CLootThings_003Ed__5 : IEnumerable<CampLootThingStruct>, IEnumerable, IEnumerator<CampLootThingStruct>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private CampLootThingStruct _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private PlanetTile tile;

		public PlanetTile _003C_003E3__tile;

		private float _003CleatherWeight_003E5__2;

		private IEnumerator<ThingDef> _003C_003E7__wrap2;

		CampLootThingStruct IEnumerator<CampLootThingStruct>.Current
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
		public _003CLootThings_003Ed__5(int _003C_003E1__state)
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
			_003C_003E7__wrap2 = null;
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
					IEnumerable<ThingDef> enumerable = Find.WorldGrid[tile].PrimaryBiome.AllWildAnimals.Select((PawnKindDef a) => a.RaceProps.leatherDef);
					int num = enumerable.Count((ThingDef x) => x != null);
					if (num <= 0)
					{
						return false;
					}
					_003CleatherWeight_003E5__2 = 1f / (float)num;
					_003C_003E7__wrap2 = enumerable.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap2.MoveNext())
				{
					ThingDef current = _003C_003E7__wrap2.Current;
					if (current != null)
					{
						_003C_003E2__current = new CampLootThingStruct
						{
							thing = current,
							thing2 = ThingDefOf.Pemmican,
							weight = _003CleatherWeight_003E5__2
						};
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = null;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<CampLootThingStruct> IEnumerable<CampLootThingStruct>.GetEnumerator()
		{
			_003CLootThings_003Ed__5 _003CLootThings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CLootThings_003Ed__ = this;
			}
			else
			{
				_003CLootThings_003Ed__ = new _003CLootThings_003Ed__5(0);
			}
			_003CLootThings_003Ed__.tile = _003C_003E3__tile;
			return _003CLootThings_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<CampLootThingStruct>)this).GetEnumerator();
		}
	}

	public override IEnumerable<PreceptDef> DisallowedPrecepts => DefDatabase<PreceptDef>.AllDefs.Where((PreceptDef p) => p.disallowHuntingCamps);

	public override PawnGroupKindDef WorkerGroupKind => PawnGroupKindDefOf.Hunters;

	public override bool CanSpawnOn(PlanetTile tile)
	{
		if (Find.WorldGrid[tile].AnimalDensity > BiomeDefOf.Desert.animalDensity)
		{
			return base.CanSpawnOn(tile);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CLootThings_003Ed__5))]
	public override IEnumerable<CampLootThingStruct> LootThings(PlanetTile tile)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CLootThings_003Ed__5(-2)
		{
			_003C_003E3__tile = tile
		};
	}
}
