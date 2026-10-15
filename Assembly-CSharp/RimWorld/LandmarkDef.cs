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

public class LandmarkDef : Def
{
	public enum LandmarkDrawType
	{
		Standard,
		TerrainMask
	}

	public enum CoastRotateMode
	{
		None,
		ToCoast,
		AwayFromCoast
	}

	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__26 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public LandmarkDef _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__26(int _003C_003E1__state)
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
			LandmarkDef landmarkDef = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!landmarkDef.mutatorChances.Any((MutatorChance mc) => mc.chance >= 1f))
				{
					_003C_003E2__current = "LandmarkDef " + landmarkDef.defName + " has no mutators with a chance of 1 or higher.";
					_003C_003E1__state = 1;
					return true;
				}
				break;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__26 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__26(0)
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

	public Type workerClass = typeof(Landmark);

	public float commonality;

	public List<MutatorChance> mutatorChances = new List<MutatorChance>();

	public List<MutatorChance> comboLandmarkMutators = new List<MutatorChance>();

	public RulePackDef nameMaker;

	[NoTranslate]
	public string iconTexturePath;

	public IntVec2 atlasSize = IntVec2.One;

	public float drawScale = 1f;

	public LandmarkDrawType drawType;

	public CoastRotateMode coastRotateMode;

	public bool drawAboveRoads;

	[NoTranslate]
	public string category;

	public List<ShaderParameter> terrainParameters;

	private static readonly List<PlanetTile> neighbours = new List<PlanetTile>(8);

	private static Dictionary<PlanetTile, bool> cachedSettlementTiles = new Dictionary<PlanetTile, bool>();

	public Material Material => MaterialPool.MatFrom(iconTexturePath, ShaderDatabase.WorldOverlayTransparentLit, 3510);

	public Texture2D Texture => CachedTexture.Get(iconTexturePath);

	public bool EverValid()
	{
		if (!ModsConfig.OdysseyActive)
		{
			return false;
		}
		foreach (MutatorChance mutatorChance in mutatorChances)
		{
			if (mutatorChance.required && !mutatorChance.mutator.EverValid())
			{
				return false;
			}
		}
		return true;
	}

	public static void ClearCache()
	{
		cachedSettlementTiles.Clear();
	}

	public bool IsValidTile(PlanetTile tile, PlanetLayer layer, bool canUseCache = false)
	{
		if (!ModsConfig.OdysseyActive)
		{
			return false;
		}
		bool flag = false;
		Tile tile2 = layer[tile];
		if (tile2.PrimaryBiome.impassable || tile2.hilliness == Hilliness.Impassable)
		{
			return false;
		}
		if (Find.World.landmarks[tile] != null)
		{
			return false;
		}
		if (tile2.Mutators.Any((TileMutatorDef m) => m.preventsLandmarks))
		{
			return false;
		}
		foreach (MutatorChance mutatorChance in mutatorChances)
		{
			if (mutatorChance.required)
			{
				if (!mutatorChance.mutator.IsValidTile(tile, layer))
				{
					return false;
				}
				flag = true;
			}
			if (!flag && mutatorChance.chance >= 1f && mutatorChance.mutator.IsValidTile(tile, layer))
			{
				flag = true;
			}
		}
		if (!flag)
		{
			return false;
		}
		if (coastRotateMode != 0 && !IsValidRotatableTile(tile, layer, coastRotateMode))
		{
			return false;
		}
		if (canUseCache)
		{
			if (!cachedSettlementTiles.ContainsKey(tile))
			{
				cachedSettlementTiles[tile] = Find.World.worldObjects.AnySettlementAt(tile);
			}
			if (cachedSettlementTiles[tile])
			{
				return false;
			}
		}
		else if (Find.World.worldObjects.AnySettlementAt(tile))
		{
			return false;
		}
		return true;
	}

	private static bool IsValidRotatableTile(PlanetTile tile, PlanetLayer layer, CoastRotateMode mode)
	{
		layer.GetTileNeighbors(tile, neighbours);
		bool flag = false;
		for (int i = 0; i < neighbours.Count; i++)
		{
			if (layer[neighbours[i]].Landmark != null)
			{
				return false;
			}
			if (IsValidRotatableNeighbour(layer[neighbours[i]].PrimaryBiome, mode))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return false;
		}
		return true;
	}

	public static bool IsValidRotatableNeighbour(BiomeDef biome, CoastRotateMode mode)
	{
		if (mode == CoastRotateMode.ToCoast)
		{
			return !biome.isBackgroundBiome;
		}
		return biome.isBackgroundBiome;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__26))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__26(-2)
		{
			_003C_003E4__this = this
		};
	}
}
