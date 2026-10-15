using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LudeonTK;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class StockGenerator_Animals : StockGenerator
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_0
	{
		public StockGenerator_Animals _003C_003E4__this;

		public PlanetTile forTile;

		public List<PawnKindDef> kinds;

		public Func<PawnKindDef, bool> _003C_003E9__3;

		public Func<PawnKindDef, float> _003C_003E9__4;

		internal bool _003CGenerateThings_003Eb__0(PawnKindDef k)
		{
			if (k.race.tradeTags == null || _003C_003E4__this.createMatingPair.NullOrEmpty())
			{
				return false;
			}
			for (int i = 0; i < k.race.tradeTags.Count; i++)
			{
				if (_003C_003E4__this.createMatingPair.Contains(k.race.tradeTags[i]))
				{
					return true;
				}
			}
			return false;
		}

		internal float _003CGenerateThings_003Eb__2(PawnKindDef k)
		{
			return (PawnUtility.PlayerHasReproductivePair(k) ? 0.5f : 1f) * _003C_003E4__this.SelectionChance(k);
		}

		internal bool _003CGenerateThings_003Eb__3(PawnKindDef k)
		{
			if (!kinds.Contains(k))
			{
				return _003C_003E4__this.PawnKindAllowed(k, forTile);
			}
			return false;
		}

		internal float _003CGenerateThings_003Eb__4(PawnKindDef k)
		{
			return _003C_003E4__this.SelectionChance(k);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_1
	{
		public Func<PawnKindDef, bool> CanCreateMatingPair;

		public _003C_003Ec__DisplayClass9_0 CS_0024_003C_003E8__locals1;

		internal bool _003CGenerateThings_003Eb__1(PawnKindDef k)
		{
			if (CS_0024_003C_003E8__locals1._003C_003E4__this.PawnKindAllowed(k, CS_0024_003C_003E8__locals1.forTile))
			{
				return CanCreateMatingPair(k);
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGenerateThings_003Ed__9 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StockGenerator_Animals _003C_003E4__this;

		private PlanetTile forTile;

		public PlanetTile _003C_003E3__forTile;

		private _003C_003Ec__DisplayClass9_1 _003C_003E8__1;

		private _003C_003Ec__DisplayClass9_0 _003C_003E8__2;

		private int _003CnumKinds_003E5__2;

		private int _003Ccount_003E5__3;

		private PawnKindDef _003CmatingKind_003E5__4;

		private int _003Ci_003E5__5;

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
		public _003CGenerateThings_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003C_003E8__2 = null;
			_003CmatingKind_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			StockGenerator_Animals stockGenerator_Animals = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__2 = new _003C_003Ec__DisplayClass9_0();
				_003C_003E8__2._003C_003E4__this = _003C_003E4__this;
				_003C_003E8__2.forTile = forTile;
				_003CnumKinds_003E5__2 = stockGenerator_Animals.kindCountRange.RandomInRange;
				_003Ccount_003E5__3 = stockGenerator_Animals.countRange.RandomInRange;
				if (_003Ccount_003E5__3 > 1 && !stockGenerator_Animals.createMatingPair.NullOrEmpty())
				{
					_003C_003E8__1 = new _003C_003Ec__DisplayClass9_1();
					_003C_003E8__1.CS_0024_003C_003E8__locals1 = _003C_003E8__2;
					_003C_003E8__1.CanCreateMatingPair = delegate(PawnKindDef k)
					{
						if (k.race.tradeTags == null || _003C_003E8__1.CS_0024_003C_003E8__locals1._003C_003E4__this.createMatingPair.NullOrEmpty())
						{
							return false;
						}
						for (int i = 0; i < k.race.tradeTags.Count; i++)
						{
							if (_003C_003E8__1.CS_0024_003C_003E8__locals1._003C_003E4__this.createMatingPair.Contains(k.race.tradeTags[i]))
							{
								return true;
							}
						}
						return false;
					};
					DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef k) => _003C_003E8__1.CS_0024_003C_003E8__locals1._003C_003E4__this.PawnKindAllowed(k, _003C_003E8__1.CS_0024_003C_003E8__locals1.forTile) && _003C_003E8__1.CanCreateMatingPair(k)).TryRandomElementByWeight((PawnKindDef k) => (PawnUtility.PlayerHasReproductivePair(k) ? 0.5f : 1f) * _003C_003E8__1.CS_0024_003C_003E8__locals1._003C_003E4__this.SelectionChance(k), out _003CmatingKind_003E5__4);
					if (_003CmatingKind_003E5__4 != null)
					{
						PawnKindDef kind2 = _003CmatingKind_003E5__4;
						PlanetTile? tile2 = _003C_003E8__1.CS_0024_003C_003E8__locals1.forTile;
						Gender? fixedGender = Gender.Female;
						PawnGenerationRequest request2 = new PawnGenerationRequest(kind2, null, PawnGenerationContext.NonPlayer, tile2, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, fixedGender, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null);
						_003C_003E2__current = PawnGenerator.GeneratePawn(request2);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_02a3;
				}
				goto IL_02b1;
			case 1:
			{
				_003C_003E1__state = -1;
				PawnKindDef kind = _003CmatingKind_003E5__4;
				PlanetTile? tile = _003C_003E8__1.CS_0024_003C_003E8__locals1.forTile;
				Gender? fixedGender = Gender.Male;
				PawnGenerationRequest request = new PawnGenerationRequest(kind, null, PawnGenerationContext.NonPlayer, tile, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, fixedGender, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null);
				_003C_003E2__current = PawnGenerator.GeneratePawn(request);
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				_003Ccount_003E5__3 -= 2;
				goto IL_02a3;
			case 3:
				{
					_003C_003E1__state = -1;
					_003Ci_003E5__5++;
					break;
				}
				IL_02a3:
				_003C_003E8__1 = null;
				_003CmatingKind_003E5__4 = null;
				goto IL_02b1;
				IL_02b1:
				if (_003Ccount_003E5__3 <= 0)
				{
					return false;
				}
				_003C_003E8__2.kinds = new List<PawnKindDef>();
				for (int j = 0; j < _003CnumKinds_003E5__2; j++)
				{
					if (!DefDatabase<PawnKindDef>.AllDefs.Where((PawnKindDef k) => !_003C_003E8__2.kinds.Contains(k) && _003C_003E8__2._003C_003E4__this.PawnKindAllowed(k, _003C_003E8__2.forTile)).TryRandomElementByWeight((PawnKindDef k) => _003C_003E8__2._003C_003E4__this.SelectionChance(k), out var result))
					{
						break;
					}
					_003C_003E8__2.kinds.Add(result);
				}
				_003Ci_003E5__5 = 0;
				break;
			}
			if (_003Ci_003E5__5 < _003Ccount_003E5__3)
			{
				if (!_003C_003E8__2.kinds.TryRandomElement(out var result2))
				{
					return false;
				}
				PawnGenerationRequest request3 = new PawnGenerationRequest(result2, null, PawnGenerationContext.NonPlayer, _003C_003E8__2.forTile, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null);
				_003C_003E2__current = PawnGenerator.GeneratePawn(request3);
				_003C_003E1__state = 3;
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
			_003CGenerateThings_003Ed__9 _003CGenerateThings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGenerateThings_003Ed__ = this;
			}
			else
			{
				_003CGenerateThings_003Ed__ = new _003CGenerateThings_003Ed__9(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGenerateThings_003Ed__.forTile = _003C_003E3__forTile;
			return _003CGenerateThings_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	[NoTranslate]
	private List<string> tradeTagsSell = new List<string>();

	[NoTranslate]
	private List<string> tradeTagsBuy = new List<string>();

	[NoTranslate]
	private List<string> createMatingPair = new List<string>();

	private IntRange kindCountRange = new IntRange(1, 1);

	private float minWildness;

	private float maxWildness = 1f;

	private bool checkTemperature = true;

	private static readonly SimpleCurve SelectionChanceFromWildnessCurve = new SimpleCurve
	{
		new CurvePoint(0f, 100f),
		new CurvePoint(0.25f, 60f),
		new CurvePoint(0.5f, 30f),
		new CurvePoint(0.75f, 12f),
		new CurvePoint(1f, 2f)
	};

	private const float SelectionChanceFactorIfExistingMatingPair = 0.5f;

	public IEnumerable<string> AllRelevantTradeTags => tradeTagsSell.Concat(tradeTagsBuy).Concat(createMatingPair);

	[IteratorStateMachine(typeof(_003CGenerateThings_003Ed__9))]
	public override IEnumerable<Thing> GenerateThings(PlanetTile forTile, Faction faction = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGenerateThings_003Ed__9(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__forTile = forTile
		};
	}

	private float SelectionChance(PawnKindDef k)
	{
		return SelectionChanceFromWildnessCurve.Evaluate(k.race.GetStatValueAbstract(StatDefOf.Wildness));
	}

	public override bool HandlesThingDef(ThingDef thingDef)
	{
		if (thingDef.category == ThingCategory.Pawn && thingDef.race.Animal && thingDef.tradeability != 0)
		{
			if (!tradeTagsSell.Any((string tag) => thingDef.tradeTags != null && thingDef.tradeTags.Contains(tag)))
			{
				return tradeTagsBuy.Any((string tag) => thingDef.tradeTags != null && thingDef.tradeTags.Contains(tag));
			}
			return true;
		}
		return false;
	}

	public override Tradeability TradeabilityFor(ThingDef thingDef)
	{
		if (!HandlesThingDef(thingDef))
		{
			return Tradeability.None;
		}
		bool flag = false;
		bool flag2 = false;
		if ((thingDef.tradeability == Tradeability.All || thingDef.tradeability == Tradeability.Buyable) && tradeTagsSell.Any((string tag) => thingDef.tradeTags != null && thingDef.tradeTags.Contains(tag)))
		{
			flag = true;
		}
		if ((thingDef.tradeability == Tradeability.All || thingDef.tradeability == Tradeability.Sellable) && tradeTagsBuy.Any((string tag) => thingDef.tradeTags != null && thingDef.tradeTags.Contains(tag)))
		{
			flag2 = true;
		}
		if (flag2 && flag)
		{
			return Tradeability.All;
		}
		if (flag2)
		{
			return Tradeability.Sellable;
		}
		if (flag)
		{
			return Tradeability.Buyable;
		}
		return Tradeability.None;
	}

	private bool PawnKindAllowed(PawnKindDef kind, PlanetTile forTile)
	{
		float statValueAbstract = kind.race.GetStatValueAbstract(StatDefOf.Wildness);
		if (!kind.RaceProps.Animal || statValueAbstract < minWildness || statValueAbstract > maxWildness || statValueAbstract > 1f)
		{
			return false;
		}
		if (checkTemperature)
		{
			PlanetTile tile = forTile;
			if (!tile.Valid && Find.AnyPlayerHomeMap != null)
			{
				tile = Find.AnyPlayerHomeMap.Tile;
			}
			if (tile.Valid && !Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, kind.race))
			{
				return false;
			}
		}
		if (kind.race.tradeTags == null)
		{
			return false;
		}
		if (!tradeTagsSell.Any((string x) => kind.race.tradeTags.Contains(x)))
		{
			return false;
		}
		if (!kind.race.tradeability.TraderCanSell())
		{
			return false;
		}
		return true;
	}

	public void LogAnimalChances()
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (PawnKindDef allDef in DefDatabase<PawnKindDef>.AllDefs)
		{
			stringBuilder.AppendLine(allDef.defName + ": " + SelectionChance(allDef).ToString("F2"));
		}
		Log.Message(stringBuilder.ToString());
	}

	[DebugOutput]
	private static void StockGenerationAnimals()
	{
		StockGenerator_Animals stockGenerator_Animals = new StockGenerator_Animals();
		stockGenerator_Animals.tradeTagsSell = new List<string>();
		stockGenerator_Animals.tradeTagsSell.Add("AnimalCommon");
		stockGenerator_Animals.tradeTagsSell.Add("AnimalUncommon");
		stockGenerator_Animals.LogAnimalChances();
	}

	public string GetInterestInTradeTag(string tag)
	{
		bool flag = tradeTagsBuy.Contains(tag);
		bool flag2 = tradeTagsSell.Contains(tag);
		if (flag && flag2)
		{
			return "Buy/Sell";
		}
		if (flag)
		{
			return "Buy";
		}
		if (flag2)
		{
			return "Sell";
		}
		return "-";
	}
}
