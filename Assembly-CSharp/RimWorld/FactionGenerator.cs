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

public static class FactionGenerator
{
	[CompilerGenerated]
	private sealed class _003Cget_ConfigurableFactions_003Ed__2 : IEnumerable<FactionDef>, IEnumerable, IEnumerator<FactionDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FactionDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerator<FactionDef> _003C_003E7__wrap1;

		FactionDef IEnumerator<FactionDef>.Current
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
		public _003Cget_ConfigurableFactions_003Ed__2(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
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
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = (from f in DefDatabase<FactionDef>.AllDefs
						where f.maxConfigurableAtWorldCreation > 0
						orderby f.configurationListOrderPriority
						select f).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					FactionDef current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FactionDef> IEnumerable<FactionDef>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003Cget_ConfigurableFactions_003Ed__2(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FactionDef>)this).GetEnumerator();
		}
	}

	private const int MaxPreferredFactionNameLength = 20;

	public static IEnumerable<FactionDef> ConfigurableFactions
	{
		[IteratorStateMachine(typeof(_003Cget_ConfigurableFactions_003Ed__2))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_ConfigurableFactions_003Ed__2(-2);
		}
	}

	public static void GenerateFactionsIntoWorldLayer(PlanetLayer layer, List<FactionDef> factions = null)
	{
		InitializeFactions(layer, factions);
		IEnumerable<Faction> source = Find.World.factionManager.AllFactionsListForReading.Where(Validator);
		if (source.Any())
		{
			float num = layer.Def.viewAngleSettlementsFactorCurve.Evaluate(Mathf.Clamp01(layer.ViewAngle / 180f));
			float randomInRange = layer.Def.settlementsPer100kTiles.RandomInRange;
			float scaleFactor = Find.World.info.overallPopulation.GetScaleFactor();
			int num2 = GenMath.RoundRandom((float)layer.TilesCount / 100000f * randomInRange * scaleFactor * num);
			num2 -= Find.WorldObjects.AllSettlementsOnLayer(layer).Count;
			for (int i = 0; i < num2; i++)
			{
				Faction faction = source.RandomElementByWeight((Faction x) => x.def.settlementGenerationWeight);
				WorldObject worldObject = WorldObjectMaker.MakeWorldObject(layer.Def.SettlementWorldObjectDef);
				worldObject.SetFaction(faction);
				worldObject.Tile = TileFinder.RandomSettlementTileFor(layer, faction);
				if (worldObject is INameableWorldObject nameableWorldObject)
				{
					nameableWorldObject.Name = SettlementNameGenerator.GenerateSettlementName(worldObject);
				}
				Find.WorldObjects.Add(worldObject);
			}
		}
		Find.IdeoManager.SortIdeos();
		bool Validator(Faction x)
		{
			if (!x.def.isPlayer && !x.Hidden && !x.temporary)
			{
				return CanExistOnLayer(layer, x.def);
			}
			return false;
		}
	}

	private static void InitializeFactions(PlanetLayer layer, List<FactionDef> factions)
	{
		if (factions != null)
		{
			foreach (FactionDef faction in factions)
			{
				if (CanExistOnLayer(layer, faction))
				{
					AddFactionToManager(layer, faction);
				}
			}
			return;
		}
		IOrderedEnumerable<FactionDef> orderedEnumerable = DefDatabase<FactionDef>.AllDefs.OrderBy((FactionDef x) => x.hidden);
		foreach (FactionDef facDef in orderedEnumerable)
		{
			if (!orderedEnumerable.Any((FactionDef x) => x.requiredCountAtGameStart > 0 && x.replacesFaction == facDef) && CanExistOnLayer(layer, facDef))
			{
				for (int i = 0; i < facDef.requiredCountAtGameStart; i++)
				{
					AddFactionToManager(layer, facDef);
				}
			}
		}
	}

	private static bool CanExistOnLayer(PlanetLayer layer, FactionDef f)
	{
		if (!f.layerBlacklist.NullOrEmpty() && f.layerBlacklist.Contains(layer.Def))
		{
			return false;
		}
		if (!f.layerWhitelist.NullOrEmpty() || !layer.IsRootSurface)
		{
			return f.layerWhitelist.Contains(layer.Def);
		}
		return true;
	}

	private static void AddFactionToManager(PlanetLayer layer, FactionDef facDef)
	{
		CreateFactionAndAddToManager(layer, facDef);
	}

	public static void CreateFactionAndAddToManager(FactionDef facDef)
	{
		CreateFactionAndAddToManager(Find.WorldGrid.Surface, facDef);
	}

	public static void CreateFactionAndAddToManager(PlanetLayer layer, FactionDef facDef)
	{
		if (facDef.fixedIdeo)
		{
			IdeoGenerationParms ideoGenerationParms = new IdeoGenerationParms(facDef, forceNoExpansionIdeo: false, null, null, name: facDef.ideoName, styles: facDef.styles, deities: facDef.deityPresets, hidden: facDef.hiddenIdeo, description: facDef.ideoDescription, forcedMemes: facDef.forcedMemes, classicExtra: false, forceNoWeaponPreference: false, forNewFluidIdeo: false, fixedIdeo: true, requiredPreceptsOnly: facDef.requiredPreceptsOnly);
			Find.FactionManager.Add(NewGeneratedFaction(layer, new FactionGeneratorParms(facDef, ideoGenerationParms, null)));
		}
		else
		{
			IdeoGenerationParms ideoGenerationParms2 = new IdeoGenerationParms(facDef, forceNoExpansionIdeo: false, null, null, name: facDef.ideoName, styles: facDef.styles, deities: facDef.deityPresets, hidden: facDef.hiddenIdeo, description: facDef.ideoDescription, forcedMemes: facDef.forcedMemes, classicExtra: false, forceNoWeaponPreference: false, forNewFluidIdeo: false, fixedIdeo: false, requiredPreceptsOnly: facDef.requiredPreceptsOnly);
			Find.FactionManager.Add(NewGeneratedFaction(layer, new FactionGeneratorParms(facDef, ideoGenerationParms2, null)));
		}
	}

	public static Faction NewGeneratedFaction(FactionGeneratorParms parms)
	{
		return NewGeneratedFaction(Find.WorldGrid.Surface, parms);
	}

	public static Faction NewGeneratedFaction(PlanetLayer layer, FactionGeneratorParms parms)
	{
		FactionDef factionDef = parms.factionDef;
		parms.ideoGenerationParms.forFaction = factionDef;
		Faction faction = new Faction();
		faction.def = factionDef;
		faction.loadID = Find.UniqueIDsManager.GetNextFactionID();
		faction.colorFromSpectrum = NewRandomColorFromSpectrum(faction);
		faction.hidden = parms.hidden;
		if (factionDef.humanlikeFaction)
		{
			faction.ideos = new FactionIdeosTracker(faction);
			if (!faction.IsPlayer || !ModsConfig.IdeologyActive || !Find.GameInitData.startedFromEntry)
			{
				faction.ideos.ChooseOrGenerateIdeo(parms.ideoGenerationParms);
			}
		}
		if (!factionDef.isPlayer)
		{
			if (factionDef.fixedName != null)
			{
				faction.Name = factionDef.fixedName;
			}
			else
			{
				string text = "";
				for (int i = 0; i < 10; i++)
				{
					string text2 = NameGenerator.GenerateName(faction.def.factionNameMaker, Find.FactionManager.AllFactionsVisible.Select((Faction fac) => fac.Name));
					if (text2.Length <= 20)
					{
						text = text2;
					}
				}
				if (text.NullOrEmpty())
				{
					text = NameGenerator.GenerateName(faction.def.factionNameMaker, Find.FactionManager.AllFactionsVisible.Select((Faction fac) => fac.Name));
				}
				faction.Name = text;
			}
		}
		foreach (Faction item in Find.FactionManager.AllFactionsListForReading)
		{
			faction.TryMakeInitialRelationsWith(item);
		}
		if (!faction.Hidden && !factionDef.isPlayer)
		{
			WorldObject worldObject = WorldObjectMaker.MakeWorldObject(layer.Def.SettlementWorldObjectDef);
			worldObject.SetFaction(faction);
			worldObject.Tile = TileFinder.RandomSettlementTileFor(layer, faction);
			if (worldObject is INameableWorldObject nameableWorldObject)
			{
				nameableWorldObject.Name = SettlementNameGenerator.GenerateSettlementName(worldObject);
			}
			Find.WorldObjects.Add(worldObject);
		}
		faction.TryGenerateNewLeader();
		return faction;
	}

	public static Faction NewGeneratedFactionWithRelations(FactionDef facDef, List<FactionRelation> relations, bool hidden = false)
	{
		SurfaceLayer surface = Find.WorldGrid.Surface;
		bool? hidden2 = hidden;
		return NewGeneratedFactionWithRelations(surface, new FactionGeneratorParms(facDef, default(IdeoGenerationParms), hidden2), relations);
	}

	public static Faction NewGeneratedFactionWithRelations(PlanetLayer layer, FactionDef facDef, List<FactionRelation> relations, bool hidden = false)
	{
		bool? hidden2 = hidden;
		return NewGeneratedFactionWithRelations(layer, new FactionGeneratorParms(facDef, default(IdeoGenerationParms), hidden2), relations);
	}

	public static Faction NewGeneratedFactionWithRelations(FactionGeneratorParms parms, List<FactionRelation> relations)
	{
		return NewGeneratedFactionWithRelations(Find.WorldGrid.Surface, parms, relations);
	}

	public static Faction NewGeneratedFactionWithRelations(PlanetLayer layer, FactionGeneratorParms parms, List<FactionRelation> relations)
	{
		Faction faction = NewGeneratedFaction(layer, parms);
		for (int i = 0; i < relations.Count; i++)
		{
			faction.SetRelation(relations[i]);
		}
		return faction;
	}

	public static float NewRandomColorFromSpectrum(Faction faction)
	{
		float num = -1f;
		float result = 0f;
		for (int i = 0; i < 20; i++)
		{
			float value = Rand.Value;
			float num2 = 1f;
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int j = 0; j < allFactionsListForReading.Count; j++)
			{
				Faction faction2 = allFactionsListForReading[j];
				if (faction2.def == faction.def)
				{
					float num3 = Mathf.Abs(value - faction2.colorFromSpectrum);
					if (num3 < num2)
					{
						num2 = num3;
					}
				}
			}
			if (num2 > num)
			{
				num = num2;
				result = value;
			}
		}
		return result;
	}
}
