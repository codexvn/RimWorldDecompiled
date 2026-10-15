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

public class PlanetLayerDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__57 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PlanetLayerDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private List<Type>.Enumerator _003C_003E7__wrap2;

		string IEnumerator<string>.Current
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
		public _003CConfigErrors_003Ed__57(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 5:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<Type>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				PlanetLayerDef planetLayerDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)planetLayerDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0082;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0082;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00d7;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0112;
				case 4:
					_003C_003E1__state = -4;
					goto IL_01b6;
				case 5:
					_003C_003E1__state = -5;
					goto IL_0279;
				case 6:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0082:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (planetLayerDef.tileType == null || !planetLayerDef.tileType.SameOrSubclassOf<Tile>())
					{
						_003C_003E2__current = "tileType type is not the same or subclass of Tile";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00d7;
					IL_01b6:
					while (_003C_003E7__wrap2.MoveNext())
					{
						Type current2 = _003C_003E7__wrap2.Current;
						if (!current2.IsSubclassOf(typeof(WorldDrawLayer)))
						{
							_003C_003E2__current = $"{"worldDrawLayers"} layer {current2} is type that is not a subclass of {"WorldDrawLayer"}";
							_003C_003E1__state = 4;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<Type>.Enumerator);
					_003C_003E7__wrap2 = planetLayerDef.worldTabs.GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_0279;
					IL_0279:
					while (_003C_003E7__wrap2.MoveNext())
					{
						Type current3 = _003C_003E7__wrap2.Current;
						if (!current3.IsSubclassOf(typeof(WITab)))
						{
							_003C_003E2__current = $"{"worldTabs"} layer {current3} is type that is not a subclass of {"WITab"}";
							_003C_003E1__state = 5;
							return true;
						}
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap2 = default(List<Type>.Enumerator);
					if (string.IsNullOrEmpty(planetLayerDef.viewGizmoTexPath))
					{
						_003C_003E2__current = "Must provide a texture to viewGizmoTexPath, use PlaceholderImage if required.";
						_003C_003E1__state = 6;
						return true;
					}
					break;
					IL_00d7:
					if (planetLayerDef.layerType == null || !planetLayerDef.layerType.SameOrSubclassOf<PlanetLayer>())
					{
						_003C_003E2__current = "layerType type is not the same or subclass of PlanetLayer";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0112;
					IL_0112:
					_003C_003E7__wrap2 = planetLayerDef.worldDrawLayers.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_01b6;
				}
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__57 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__57(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	private BiomeDef defaultBiome;

	private WorldObjectDef defaultMapWorldObject;

	private WorldObjectDef settlementWorldObjectDef;

	private WorldObjectDef destroyedSettlementWorldObjectDef;

	public bool onlyAllowWhitelistedIncidents;

	public bool onlyAllowWhitelistedBiomes;

	public bool onlyAllowWhitelistedGameConditions;

	public bool onlyAllowWhitelistedQuests;

	public bool onlyAllowWhitelistedArrivals;

	public bool onlyAllowWhitelistedArrivalModes;

	public bool alwaysRaycastable = true;

	public bool obstructsExpandingIcons = true;

	public bool canFormCaravans = true;

	public bool isSpace;

	public float rangeDistanceFactor = 1f;

	public float generatedLocationFactor = 1f;

	public float raidPointsFactor = 1f;

	public BiomeDef backgroundBiome;

	public bool ignoreNoBuildArea;

	public Color lineColor = Color.white;

	public float lineWidthFactor = 1f;

	private List<WorldGenStepDef> worldGenSteps = new List<WorldGenStepDef>();

	public List<Type> worldDrawLayers = new List<Type>();

	private List<Type> worldTabs = new List<Type>();

	public Type layerType = typeof(PlanetLayer);

	public Type tileType = typeof(Tile);

	[MustTranslate]
	public string elevationString = "{0}m";

	[MustTranslate]
	public string viewGizmoTooltip;

	[MustTranslate]
	public string gerundLabel;

	public bool viewGizmoOnlyVisibleWithDirectConnection = true;

	public FloatRange settlementsPer100kTiles = new FloatRange(75f, 85f);

	public SimpleCurve viewAngleSettlementsFactorCurve = new SimpleCurve
	{
		new CurvePoint(0f, 1f),
		new CurvePoint(1f, 1f)
	};

	public string viewGizmoTexPath;

	[Unsaved(false)]
	private List<WorldGenStepDef> cachedGenSteps;

	[Unsaved(false)]
	private readonly List<WITab> cachedTabs = new List<WITab>();

	[Unsaved(false)]
	private Texture2D cachedGizmoTexture;

	private bool usesSurfaceTiles;

	public bool SurfaceTiles => usesSurfaceTiles;

	public BiomeDef DefaultBiome => defaultBiome ?? BiomeDefOf.TemperateForest;

	public Texture2D ViewGizmoTexture
	{
		get
		{
			if (!cachedGizmoTexture)
			{
				return cachedGizmoTexture = ContentFinder<Texture2D>.Get(viewGizmoTexPath);
			}
			return cachedGizmoTexture;
		}
	}

	public WorldObjectDef DefaultWorldObject => defaultMapWorldObject ?? WorldObjectDefOf.Settlement;

	public WorldObjectDef SettlementWorldObjectDef => settlementWorldObjectDef ?? WorldObjectDefOf.Settlement;

	public WorldObjectDef DestroyedSettlementWorldObjectDef => destroyedSettlementWorldObjectDef ?? WorldObjectDefOf.DestroyedSettlement;

	public Material WorldLineMaterial => MaterialPool.MatFrom(GenDraw.OneSidedLineTexPath, ShaderDatabase.WorldOverlayTransparent, lineColor, 3590);

	public Material WorldLineMaterialHighVis => MaterialPool.MatFrom(GenDraw.OneSidedLineOpaqueTexPath, ShaderDatabase.WorldOverlayAdditiveTwoSided, lineColor, 3590);

	public List<WorldGenStepDef> GenStepsInOrder => cachedGenSteps ?? (cachedGenSteps = (from x in worldGenSteps
		orderby x.order, x.index
		select x).ToList());

	public List<WITab> Tabs => cachedTabs;

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__57))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__57(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostLoad()
	{
		base.PostLoad();
		usesSurfaceTiles = typeof(SurfaceTile).IsAssignableFrom(tileType);
		foreach (Type worldTab in worldTabs)
		{
			cachedTabs.Add((WITab)Activator.CreateInstance(worldTab));
		}
	}
}
