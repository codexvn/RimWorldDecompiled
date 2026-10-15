using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld;

public class PawnGroupKindWorker_Trader : PawnGroupKindWorker
{
	[CompilerGenerated]
	private sealed class _003CGetSlavesAndAnimalsFromWares_003Ed__5 : IEnumerable<Pawn>, IEnumerable, IEnumerator<Pawn>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Pawn _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private List<Thing> wares;

		public List<Thing> _003C_003E3__wares;

		private PawnGroupMakerParms parms;

		public PawnGroupMakerParms _003C_003E3__parms;

		private int _003Ci_003E5__2;

		Pawn IEnumerator<Pawn>.Current
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
		public _003CGetSlavesAndAnimalsFromWares_003Ed__5(int _003C_003E1__state)
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
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0076;
			}
			_003C_003E1__state = -1;
			_003Ci_003E5__2 = 0;
			goto IL_0086;
			IL_0076:
			_003Ci_003E5__2++;
			goto IL_0086;
			IL_0086:
			if (_003Ci_003E5__2 < wares.Count)
			{
				if (wares[_003Ci_003E5__2] is Pawn pawn)
				{
					if (pawn.Faction != parms.faction)
					{
						pawn.SetFaction(parms.faction);
					}
					_003C_003E2__current = pawn;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0076;
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
		IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
		{
			_003CGetSlavesAndAnimalsFromWares_003Ed__5 _003CGetSlavesAndAnimalsFromWares_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetSlavesAndAnimalsFromWares_003Ed__ = this;
			}
			else
			{
				_003CGetSlavesAndAnimalsFromWares_003Ed__ = new _003CGetSlavesAndAnimalsFromWares_003Ed__5(0);
			}
			_003CGetSlavesAndAnimalsFromWares_003Ed__.parms = _003C_003E3__parms;
			_003CGetSlavesAndAnimalsFromWares_003Ed__.wares = _003C_003E3__wares;
			return _003CGetSlavesAndAnimalsFromWares_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Pawn>)this).GetEnumerator();
		}
	}

	public override float MinPointsToGenerateAnything(PawnGroupMaker groupMaker, FactionDef faction, PawnGroupMakerParms parms = null)
	{
		return 0f;
	}

	public override bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
	{
		if (base.CanGenerateFrom(parms, groupMaker) && groupMaker.traders.Any())
		{
			if (parms.tile.Valid)
			{
				return groupMaker.carriers.Any((PawnGenOption x) => Find.WorldGrid[parms.tile].PrimaryBiome.IsPackAnimalAllowed(x.kind.race));
			}
			return true;
		}
		return false;
	}

	protected override void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns, bool errorOnZeroResults = true)
	{
		if (!CanGenerateFrom(parms, groupMaker))
		{
			if (errorOnZeroResults)
			{
				Log.Error("Cannot generate trader caravan for " + parms.faction?.ToString() + ".");
			}
			return;
		}
		if (!parms.faction.def.caravanTraderKinds.Any())
		{
			Log.Error("Cannot generate trader caravan for " + parms.faction?.ToString() + " because it has no trader kinds.");
			return;
		}
		PawnGenOption pawnGenOption = groupMaker.traders.FirstOrDefault((PawnGenOption x) => !x.kind.trader);
		if (pawnGenOption != null)
		{
			Log.Error("Cannot generate arriving trader caravan for " + parms.faction?.ToString() + " because there is a pawn kind (" + pawnGenOption.kind.LabelCap + ") who is not a trader but is in a traders list.");
			return;
		}
		PawnGenOption pawnGenOption2 = groupMaker.carriers.FirstOrDefault((PawnGenOption x) => !x.kind.RaceProps.packAnimal);
		if (pawnGenOption2 != null)
		{
			Log.Error("Cannot generate arriving trader caravan for " + parms.faction?.ToString() + " because there is a pawn kind (" + pawnGenOption2.kind.LabelCap + ") who is not a carrier but is in a carriers list.");
			return;
		}
		if (parms.seed.HasValue)
		{
			Log.Warning("Deterministic seed not implemented for this pawn group kind worker. The result will be random anyway.");
		}
		TraderKindDef traderKindDef = ((parms.traderKind != null) ? parms.traderKind : parms.faction.def.caravanTraderKinds.RandomElementByWeight((TraderKindDef traderDef) => traderDef.CalculatedCommonality));
		Pawn pawn = GenerateTrader(parms, groupMaker, traderKindDef);
		outPawns.Add(pawn);
		ThingSetMakerParams parms2 = default(ThingSetMakerParams);
		parms2.traderDef = traderKindDef;
		parms2.tile = parms.tile;
		parms2.makingFaction = parms.faction;
		List<Thing> wares = ThingSetMakerDefOf.TraderStock.root.Generate(parms2).InRandomOrder().ToList();
		foreach (Pawn slavesAndAnimalsFromWare in GetSlavesAndAnimalsFromWares(parms, pawn, wares))
		{
			outPawns.Add(slavesAndAnimalsFromWare);
		}
		GenerateCarriers(parms, groupMaker, pawn, wares, outPawns);
		GenerateGuards(parms, groupMaker, pawn, wares, outPawns);
	}

	private Pawn GenerateTrader(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, TraderKindDef traderKind)
	{
		Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(groupMaker.traders.RandomElementByWeight((PawnGenOption x) => x.selectionWeight).kind, parms.faction, PawnGenerationContext.NonPlayer, parms.tile, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, fixedIdeo: parms.ideo, inhabitant: parms.inhabitants, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, biocodeWeaponChance: 0f, biocodeApparelChance: 0f, extraPawnForExtraRelationChance: null, relationWithExtraPawnChanceFactor: 1f, validatorPreGear: null, validatorPostGear: null, forcedTraits: null, prohibitedTraits: null, minChanceToRedressWorldPawn: null, fixedBiologicalAge: null, fixedChronologicalAge: null, fixedGender: null, fixedLastName: null, fixedBirthName: null, fixedTitle: null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, forcedXenogenes: null, forcedEndogenes: null, forcedXenotype: null, forcedCustomXenotype: null, allowedXenotypes: null, forceBaselinerChance: 0f, developmentalStages: DevelopmentalStage.Adult, pawnKindDefGetter: null, excludeBiologicalAgeRange: null, biologicalAgeRange: null));
		pawn.mindState.wantsToTradeWithColony = true;
		PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, actAsIfSpawned: true);
		pawn.trader.traderKind = traderKind;
		parms.points -= pawn.kindDef.combatPower;
		TradeUtility.CheckGiveTraderQuest(pawn);
		return pawn;
	}

	private void GenerateCarriers(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, Pawn trader, List<Thing> wares, List<Pawn> outPawns)
	{
		List<Thing> list = wares.Where((Thing x) => !(x is Pawn)).ToList();
		int i = 0;
		int num = Mathf.CeilToInt((float)list.Count / 8f);
		PawnKindDef kind = groupMaker.carriers.Where((PawnGenOption x) => !parms.tile.Valid || Find.WorldGrid[parms.tile].PrimaryBiome.IsPackAnimalAllowed(x.kind.race)).RandomElementByWeight((PawnGenOption x) => x.selectionWeight).kind;
		List<Pawn> list2 = new List<Pawn>();
		for (int j = 0; j < num; j++)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, parms.faction, PawnGenerationContext.NonPlayer, parms.tile, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, fixedIdeo: parms.ideo, inhabitant: parms.inhabitants, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, biocodeWeaponChance: 0f, biocodeApparelChance: 0f, extraPawnForExtraRelationChance: null, relationWithExtraPawnChanceFactor: 1f, validatorPreGear: null, validatorPostGear: null, forcedTraits: null, prohibitedTraits: null, minChanceToRedressWorldPawn: null, fixedBiologicalAge: null, fixedChronologicalAge: null, fixedGender: null, fixedLastName: null, fixedBirthName: null, fixedTitle: null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, forcedXenogenes: null, forcedEndogenes: null, forcedXenotype: null, forcedCustomXenotype: null, allowedXenotypes: null, forceBaselinerChance: 0f, developmentalStages: DevelopmentalStage.Adult, pawnKindDefGetter: null, excludeBiologicalAgeRange: null, biologicalAgeRange: null));
			if (i < list.Count)
			{
				pawn.inventory.innerContainer.TryAdd(list[i]);
				i++;
			}
			list2.Add(pawn);
			outPawns.Add(pawn);
		}
		for (; i < list.Count; i++)
		{
			list2.RandomElement().inventory.innerContainer.TryAdd(list[i]);
		}
	}

	[IteratorStateMachine(typeof(_003CGetSlavesAndAnimalsFromWares_003Ed__5))]
	private IEnumerable<Pawn> GetSlavesAndAnimalsFromWares(PawnGroupMakerParms parms, Pawn trader, List<Thing> wares)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetSlavesAndAnimalsFromWares_003Ed__5(-2)
		{
			_003C_003E3__parms = parms,
			_003C_003E3__wares = wares
		};
	}

	private void GenerateGuards(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, Pawn trader, List<Thing> wares, List<Pawn> outPawns)
	{
		if (!groupMaker.guards.Any())
		{
			return;
		}
		foreach (PawnGenOptionWithXenotype item2 in PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.guards, parms))
		{
			PawnKindDef kind = item2.Option.kind;
			Faction faction = parms.faction;
			XenotypeDef xenotype = item2.Xenotype;
			PlanetTile? tile = parms.tile;
			bool inhabitants = parms.inhabitants;
			Ideo ideo = parms.ideo;
			Pawn item = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, faction, PawnGenerationContext.NonPlayer, tile, forceGenerateNewPawn: false, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: true, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitants, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, ideo, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, xenotype, null, null, 0f, DevelopmentalStage.Adult, null, null, null));
			outPawns.Add(item);
		}
	}

	public override IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
	{
		throw new NotImplementedException();
	}
}
