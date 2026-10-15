using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class ScenarioMaker
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass5_0
	{
		public ScenPartCategory cat;

		internal bool _003CRandomScenPartsOfCategory_003Eb__0(ScenPartDef d)
		{
			if (d.canBeRandomlyAdded)
			{
				return d.category == cat;
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CRandomScenPartsOfCategory_003Ed__5 : IEnumerable<ScenPart>, IEnumerable, IEnumerator<ScenPart>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ScenPart _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ScenPartCategory cat;

		public ScenPartCategory _003C_003E3__cat;

		private int count;

		public int _003C_003E3__count;

		private Scenario scen;

		public Scenario _003C_003E3__scen;

		private _003C_003Ec__DisplayClass5_0 _003C_003E8__1;

		private IEnumerable<ScenPartDef> _003CallowedParts_003E5__2;

		private int _003CnumYielded_003E5__3;

		private int _003CnumTries_003E5__4;

		ScenPart IEnumerator<ScenPart>.Current
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
		public _003CRandomScenPartsOfCategory_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003CallowedParts_003E5__2 = null;
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
				_003CnumYielded_003E5__3++;
				goto IL_00ef;
			}
			_003C_003E1__state = -1;
			_003C_003E8__1 = new _003C_003Ec__DisplayClass5_0();
			_003C_003E8__1.cat = cat;
			if (count <= 0)
			{
				return false;
			}
			_003CallowedParts_003E5__2 = from d in AddableParts(scen)
				where d.canBeRandomlyAdded && d.category == _003C_003E8__1.cat
				select d;
			_003CnumYielded_003E5__3 = 0;
			_003CnumTries_003E5__4 = 0;
			goto IL_0161;
			IL_0161:
			if (_003CnumYielded_003E5__3 < count)
			{
				if (!_003CallowedParts_003E5__2.Any())
				{
					return false;
				}
				ScenPart newPart = MakeScenPart(_003CallowedParts_003E5__2.RandomElementByWeight((ScenPartDef d) => d.selectionWeight));
				if (CanAddPart(scen, newPart))
				{
					_003C_003E2__current = newPart;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00ef;
			}
			return false;
			IL_00ef:
			_003CnumTries_003E5__4++;
			if (_003CnumTries_003E5__4 > 100)
			{
				Log.Error("Could not add ScenPart of category " + _003C_003E8__1.cat.ToString() + " to scenario " + scen?.ToString() + " after 50 tries.");
				return false;
			}
			goto IL_0161;
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
		IEnumerator<ScenPart> IEnumerable<ScenPart>.GetEnumerator()
		{
			_003CRandomScenPartsOfCategory_003Ed__5 _003CRandomScenPartsOfCategory_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRandomScenPartsOfCategory_003Ed__ = this;
			}
			else
			{
				_003CRandomScenPartsOfCategory_003Ed__ = new _003CRandomScenPartsOfCategory_003Ed__5(0);
			}
			_003CRandomScenPartsOfCategory_003Ed__.scen = _003C_003E3__scen;
			_003CRandomScenPartsOfCategory_003Ed__.cat = _003C_003E3__cat;
			_003CRandomScenPartsOfCategory_003Ed__.count = _003C_003E3__count;
			return _003CRandomScenPartsOfCategory_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ScenPart>)this).GetEnumerator();
		}
	}

	private static Scenario scen;

	public static Scenario GeneratingScenario => scen;

	public static Scenario GenerateNewRandomScenario(string seed)
	{
		Rand.PushState();
		Rand.Seed = seed.GetHashCode();
		int @int = Rand.Int;
		int[] array = new int[10];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Rand.Int;
		}
		int int2 = Rand.Int;
		scen = new Scenario();
		scen.Category = ScenarioCategory.CustomLocal;
		scen.name = NameGenerator.GenerateName(RulePackDefOf.NamerScenario);
		scen.description = null;
		scen.summary = null;
		Rand.Seed = @int;
		scen.playerFaction = (ScenPart_PlayerFaction)MakeScenPart(ScenPartDefOf.PlayerFaction);
		scen.parts.Add(MakeScenPart(ScenPartDefOf.ConfigPage_ConfigureStartingPawns));
		scen.parts.Add(MakeScenPart(ScenPartDefOf.PlayerPawnsArriveMethod));
		scen.surfaceLayer = new ScenPart_PlanetLayer
		{
			def = ScenPartDefOf.PlanetLayerFixed,
			layer = PlanetLayerDefOf.Surface,
			settingsDef = PlanetLayerSettingsDefOf.Surface,
			hide = true,
			tag = "Surface"
		};
		if (ModsConfig.OdysseyActive)
		{
			ScenPart_PlanetLayer scenPart_PlanetLayer = new ScenPart_PlanetLayer
			{
				def = ScenPartDefOf.PlanetLayerFixed,
				layer = PlanetLayerDefOf.Orbit,
				settingsDef = PlanetLayerSettingsDefOf.Orbit,
				hide = true,
				tag = "Orbit"
			};
			scen.parts.Add(scenPart_PlanetLayer);
			scen.surfaceLayer.connections.Add(new LayerConnection
			{
				tag = scenPart_PlanetLayer.tag,
				zoomMode = LayerConnection.ZoomMode.ZoomOut
			});
			scenPart_PlanetLayer.connections.Add(new LayerConnection
			{
				tag = scen.surfaceLayer.tag,
				zoomMode = LayerConnection.ZoomMode.ZoomIn
			});
		}
		Rand.Seed = array[0];
		AddCategoryScenParts(scen, ScenPartCategory.PlayerPawnFilter, Rand.RangeInclusive(-1, 2));
		Rand.Seed = array[1];
		AddCategoryScenParts(scen, ScenPartCategory.StartingImportant, Rand.RangeInclusive(0, 2));
		Rand.Seed = array[2];
		AddCategoryScenParts(scen, ScenPartCategory.PlayerPawnModifier, Rand.RangeInclusive(-1, 2));
		Rand.Seed = array[3];
		AddCategoryScenParts(scen, ScenPartCategory.Rule, Rand.RangeInclusive(-2, 3));
		Rand.Seed = array[4];
		AddCategoryScenParts(scen, ScenPartCategory.StartingItem, Rand.RangeInclusive(0, 6));
		Rand.Seed = array[5];
		AddCategoryScenParts(scen, ScenPartCategory.WorldThing, Rand.RangeInclusive(-3, 6));
		Rand.Seed = array[6];
		AddCategoryScenParts(scen, ScenPartCategory.GameCondition, Rand.RangeInclusive(-1, 2));
		Rand.Seed = int2;
		foreach (ScenPart allPart in scen.AllParts)
		{
			allPart.Randomize();
		}
		for (int j = 0; j < scen.parts.Count; j++)
		{
			for (int k = 0; k < scen.parts.Count; k++)
			{
				if (j != k && scen.parts[j].TryMerge(scen.parts[k]))
				{
					scen.parts.RemoveAt(k);
					k--;
					if (j > k)
					{
						j--;
					}
				}
			}
		}
		for (int l = 0; l < scen.parts.Count; l++)
		{
			for (int m = 0; m < scen.parts.Count; m++)
			{
				if (l != m && !scen.parts[l].CanCoexistWith(scen.parts[m]))
				{
					scen.parts.RemoveAt(m);
					m--;
					if (l > m)
					{
						l--;
					}
				}
			}
		}
		foreach (string item in scen.ConfigErrors())
		{
			Log.Error(item);
		}
		Rand.PopState();
		Scenario result = scen;
		scen = null;
		return result;
	}

	private static void AddCategoryScenParts(Scenario scen, ScenPartCategory cat, int count)
	{
		scen.parts.AddRange(RandomScenPartsOfCategory(scen, cat, count));
	}

	[IteratorStateMachine(typeof(_003CRandomScenPartsOfCategory_003Ed__5))]
	private static IEnumerable<ScenPart> RandomScenPartsOfCategory(Scenario scen, ScenPartCategory cat, int count)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRandomScenPartsOfCategory_003Ed__5(-2)
		{
			_003C_003E3__scen = scen,
			_003C_003E3__cat = cat,
			_003C_003E3__count = count
		};
	}

	public static IEnumerable<ScenPartDef> AddableParts(Scenario scen)
	{
		return DefDatabase<ScenPartDef>.AllDefs.Where((ScenPartDef d) => scen.AllParts.Count((ScenPart p) => p.def == d) < d.maxUses);
	}

	private static bool CanAddPart(Scenario scen, ScenPart newPart)
	{
		for (int i = 0; i < scen.parts.Count; i++)
		{
			if (!newPart.CanCoexistWith(scen.parts[i]))
			{
				return false;
			}
		}
		return true;
	}

	public static ScenPart MakeScenPart(ScenPartDef def)
	{
		ScenPart obj = (ScenPart)Activator.CreateInstance(def.scenPartClass);
		obj.def = def;
		return obj;
	}
}
