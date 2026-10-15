using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class StockGenerator_Slaves : StockGenerator
{
	[CompilerGenerated]
	private sealed class _003CGenerateThings_003Ed__2 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StockGenerator_Slaves _003C_003E4__this;

		private Faction faction;

		public Faction _003C_003E3__faction;

		private PlanetTile forTile;

		public PlanetTile _003C_003E3__forTile;

		private int _003Ccount_003E5__2;

		private int _003Ci_003E5__3;

		Thing IEnumerator<Thing>.Current
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
		public _003CGenerateThings_003Ed__2(int _003C_003E1__state)
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
			StockGenerator_Slaves stockGenerator_Slaves = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (stockGenerator_Slaves.respectPopulationIntent && Rand.Value > StorytellerUtilityPopulation.PopulationIntent)
				{
					return false;
				}
				if (faction != null && faction.ideos != null)
				{
					bool flag = true;
					foreach (Ideo allIdeo in faction.ideos.AllIdeos)
					{
						if (!allIdeo.IdeoApprovesOfSlavery())
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				_003Ccount_003E5__2 = stockGenerator_Slaves.countRange.RandomInRange;
				_003Ci_003E5__3 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__3++;
				break;
			}
			if (_003Ci_003E5__3 < _003Ccount_003E5__2)
			{
				if (!Find.FactionManager.AllFactionsVisible.Where((Faction fac) => fac != Faction.OfPlayer && fac.def.humanlikeFaction && !fac.temporary).TryRandomElement(out var result))
				{
					return false;
				}
				DevelopmentalStage developmentalStage = (Find.Storyteller.difficulty.ChildrenAllowed ? (DevelopmentalStage.Child | DevelopmentalStage.Adult) : DevelopmentalStage.Adult);
				PawnKindDef kind = ((stockGenerator_Slaves.slaveKindDef != null) ? stockGenerator_Slaves.slaveKindDef : PawnKindDefOf.Slave);
				Faction obj = result;
				PlanetTile? tile = forTile;
				bool forceAddFreeWarmLayerIfNeeded = !stockGenerator_Slaves.trader.orbital;
				DevelopmentalStage developmentalStages = developmentalStage;
				PawnGenerationRequest request = new PawnGenerationRequest(kind, obj, PawnGenerationContext.NonPlayer, tile, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, developmentalStages, null, null, null);
				_003C_003E2__current = PawnGenerator.GeneratePawn(request);
				_003C_003E1__state = 1;
				return true;
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CGenerateThings_003Ed__2 _003CGenerateThings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGenerateThings_003Ed__ = this;
			}
			else
			{
				_003CGenerateThings_003Ed__ = new _003CGenerateThings_003Ed__2(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGenerateThings_003Ed__.forTile = _003C_003E3__forTile;
			_003CGenerateThings_003Ed__.faction = _003C_003E3__faction;
			return _003CGenerateThings_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	private bool respectPopulationIntent;

	public PawnKindDef slaveKindDef;

	[IteratorStateMachine(typeof(_003CGenerateThings_003Ed__2))]
	public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGenerateThings_003Ed__2(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__forTile = forTile,
			_003C_003E3__faction = faction
		};
	}

	public override bool HandlesThingDef(ThingDef thingDef)
	{
		if (thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike)
		{
			return thingDef.tradeability != Tradeability.None;
		}
		return false;
	}
}
