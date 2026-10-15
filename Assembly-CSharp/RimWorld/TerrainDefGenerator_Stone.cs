using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class TerrainDefGenerator_Stone
{
	[CompilerGenerated]
	private sealed class _003CImpliedTerrainDefs_003Ed__0 : IEnumerable<TerrainDef>, IEnumerable, IEnumerator<TerrainDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private TerrainDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private bool hotReload;

		public bool _003C_003E3__hotReload;

		private int _003Ci_003E5__2;

		private IEnumerator<ThingDef> _003C_003E7__wrap2;

		private TerrainDef _003Chewn_003E5__4;

		private TerrainDef _003Csmooth_003E5__5;

		private bool _003ChewnExists_003E5__6;

		TerrainDef IEnumerator<TerrainDef>.Current
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
		public _003CImpliedTerrainDefs_003Ed__0(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || (uint)(num - 1) <= 2u)
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
			_003Chewn_003E5__4 = null;
			_003Csmooth_003E5__5 = null;
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
					_003Ci_003E5__2 = 0;
					IEnumerable<ThingDef> enumerable = DefDatabase<ThingDef>.AllDefs.Where((ThingDef def) => def.building != null && def.building.isNaturalRock && !def.building.isResourceRock);
					_003C_003E7__wrap2 = enumerable.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_07b2;
				}
				case 1:
					_003C_003E1__state = -3;
					goto IL_0744;
				case 2:
					_003C_003E1__state = -3;
					goto IL_076b;
				case 3:
					{
						_003C_003E1__state = -3;
						goto IL_0792;
					}
					IL_0792:
					_003Ci_003E5__2++;
					_003Chewn_003E5__4 = null;
					_003Csmooth_003E5__5 = null;
					goto IL_07b2;
					IL_0744:
					if (!_003ChewnExists_003E5__6)
					{
						_003C_003E2__current = _003Chewn_003E5__4;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_076b;
					IL_07b2:
					if (_003C_003E7__wrap2.MoveNext())
					{
						ThingDef current = _003C_003E7__wrap2.Current;
						string defName = current.defName + "_Rough";
						string defName2 = current.defName + "_RoughHewn";
						string defName3 = current.defName + "_Smooth";
						_003Csmooth_003E5__5 = null;
						bool flag = current.building.naturalTerrain != null;
						_003ChewnExists_003E5__6 = current.building.leaveTerrain != null;
						TerrainDef terrainDef;
						if (current.building.naturalTerrain != null)
						{
							terrainDef = current.building.naturalTerrain;
						}
						else
						{
							terrainDef = (hotReload ? (DefDatabase<TerrainDef>.GetNamed(defName, errorOnFail: false) ?? new TerrainDef()) : new TerrainDef());
							terrainDef.texturePath = "Terrain/Surfaces/RoughStone";
							terrainDef.edgeType = TerrainDef.TerrainEdgeType.FadeRough;
							terrainDef.pathCost = 2;
							StatUtility.SetStatValueInList(ref terrainDef.statBases, StatDefOf.Beauty, -1f);
							terrainDef.scatterType = "Rocky";
							terrainDef.affordances = new List<TerrainAffordanceDef>
							{
								TerrainAffordanceDefOf.Light,
								TerrainAffordanceDefOf.Medium,
								TerrainAffordanceDefOf.Heavy,
								TerrainAffordanceDefOf.SmoothableStone,
								TerrainAffordanceDefOf.Walkable
							};
							terrainDef.fertility = 0f;
							terrainDef.filthAcceptanceMask = FilthSourceFlags.Terrain | FilthSourceFlags.Unnatural;
							terrainDef.modContentPack = current.modContentPack;
							terrainDef.categoryType = TerrainDef.TerrainCategoryType.Stone;
							terrainDef.tags = new List<string> { "NaturalRock" };
							terrainDef.renderPrecedence = 190 + _003Ci_003E5__2;
							terrainDef.defName = defName;
							terrainDef.label = "RoughStoneTerrainLabel".Translate(current.label);
							terrainDef.description = "RoughStoneTerrainDesc".Translate(current.label);
							terrainDef.color = current.graphicData.color;
							terrainDef.natural = true;
							terrainDef.pollutedTexturePath = "Terrain/Surfaces/RoughStonePolluted";
							terrainDef.pollutionOverlayTexturePath = "Terrain/Surfaces/RoughStonePollutionOverlay";
							terrainDef.pollutionShaderType = ShaderTypeDefOf.TerrainFadeRoughLinearAdd;
							terrainDef.pollutionColor = new Color(1f, 1f, 1f, 1f);
							SetAsteroidEdgeData(terrainDef);
							if (current == ThingDefOf.Vacstone)
							{
								terrainDef.scatterType = "Craters";
							}
							current.building.naturalTerrain = terrainDef;
						}
						if (current.building.leaveTerrain != null)
						{
							_003Chewn_003E5__4 = current.building.leaveTerrain;
						}
						else
						{
							_003Chewn_003E5__4 = (hotReload ? (DefDatabase<TerrainDef>.GetNamed(defName2, errorOnFail: false) ?? new TerrainDef()) : new TerrainDef());
							_003Chewn_003E5__4.texturePath = "Terrain/Surfaces/RoughHewnRock";
							_003Chewn_003E5__4.edgeType = TerrainDef.TerrainEdgeType.FadeRough;
							_003Chewn_003E5__4.pathCost = 1;
							StatUtility.SetStatValueInList(ref _003Chewn_003E5__4.statBases, StatDefOf.Beauty, -1f);
							_003Chewn_003E5__4.scatterType = "Rocky";
							_003Chewn_003E5__4.affordances = new List<TerrainAffordanceDef>
							{
								TerrainAffordanceDefOf.Light,
								TerrainAffordanceDefOf.Medium,
								TerrainAffordanceDefOf.Heavy,
								TerrainAffordanceDefOf.SmoothableStone,
								TerrainAffordanceDefOf.Walkable
							};
							_003Chewn_003E5__4.fertility = 0f;
							_003Chewn_003E5__4.filthAcceptanceMask = FilthSourceFlags.Any;
							_003Chewn_003E5__4.modContentPack = current.modContentPack;
							_003Chewn_003E5__4.pollutedTexturePath = "Terrain/Surfaces/RoughHewnRockPolluted";
							_003Chewn_003E5__4.pollutionOverlayTexturePath = "Terrain/Surfaces/RoughStonePollutionOverlay";
							_003Chewn_003E5__4.pollutionShaderType = ShaderTypeDefOf.TerrainFadeRoughLinearAdd;
							_003Chewn_003E5__4.pollutionColor = new Color(1f, 1f, 1f, 1f);
							_003Chewn_003E5__4.categoryType = TerrainDef.TerrainCategoryType.Stone;
							_003Chewn_003E5__4.renderPrecedence = 50 + _003Ci_003E5__2;
							_003Chewn_003E5__4.defName = defName2;
							_003Chewn_003E5__4.label = "RoughHewnStoneTerrainLabel".Translate(current.label);
							_003Chewn_003E5__4.description = "RoughHewnStoneTerrainDesc".Translate(current.label);
							_003Chewn_003E5__4.color = current.graphicData.color;
							if (current == ThingDefOf.Vacstone)
							{
								_003Chewn_003E5__4.scatterType = "Craters";
							}
							current.building.leaveTerrain = _003Chewn_003E5__4;
						}
						if (!flag || !_003ChewnExists_003E5__6)
						{
							_003Csmooth_003E5__5 = (hotReload ? (DefDatabase<TerrainDef>.GetNamed(defName3, errorOnFail: false) ?? new TerrainDef()) : new TerrainDef());
							_003Csmooth_003E5__5.texturePath = "Terrain/Surfaces/SmoothStone";
							_003Csmooth_003E5__5.edgeType = TerrainDef.TerrainEdgeType.FadeRough;
							_003Csmooth_003E5__5.pathCost = 0;
							_003Csmooth_003E5__5.isPaintable = true;
							StatUtility.SetStatValueInList(ref _003Csmooth_003E5__5.statBases, StatDefOf.Beauty, 2f);
							StatUtility.SetStatValueInList(ref _003Csmooth_003E5__5.statBases, StatDefOf.MarketValue, 5f);
							_003Csmooth_003E5__5.scatterType = "Rocky";
							_003Csmooth_003E5__5.affordances = new List<TerrainAffordanceDef>
							{
								TerrainAffordanceDefOf.Light,
								TerrainAffordanceDefOf.Medium,
								TerrainAffordanceDefOf.Heavy,
								TerrainAffordanceDefOf.Walkable
							};
							_003Csmooth_003E5__5.fertility = 0f;
							_003Csmooth_003E5__5.filthAcceptanceMask = FilthSourceFlags.Any;
							_003Csmooth_003E5__5.modContentPack = current.modContentPack;
							_003Csmooth_003E5__5.tags = new List<string> { "Floor" };
							_003Csmooth_003E5__5.pollutedTexturePath = "Terrain/Surfaces/SmoothStonePolluted";
							_003Csmooth_003E5__5.pollutionOverlayTexturePath = "Terrain/Surfaces/RoughStonePollutionOverlay";
							_003Csmooth_003E5__5.pollutionShaderType = ShaderTypeDefOf.TerrainFadeRoughLinearAdd;
							_003Csmooth_003E5__5.pollutionColor = new Color(1f, 1f, 1f, 0.8f);
							_003Csmooth_003E5__5.categoryType = TerrainDef.TerrainCategoryType.Stone;
							_003Csmooth_003E5__5.renderPrecedence = 140 + _003Ci_003E5__2;
							_003Csmooth_003E5__5.defName = defName3;
							_003Csmooth_003E5__5.label = "SmoothStoneTerrainLabel".Translate(current.label);
							_003Csmooth_003E5__5.description = "SmoothStoneTerrainDesc".Translate(current.label);
							_003Csmooth_003E5__5.color = current.graphicData.color;
							if (!flag)
							{
								terrainDef.smoothedTerrain = _003Csmooth_003E5__5;
							}
							if (!_003ChewnExists_003E5__6)
							{
								_003Chewn_003E5__4.smoothedTerrain = _003Csmooth_003E5__5;
							}
						}
						if (!flag)
						{
							_003C_003E2__current = terrainDef;
							_003C_003E1__state = 1;
							return true;
						}
						goto IL_0744;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					return false;
					IL_076b:
					if (_003Csmooth_003E5__5 != null)
					{
						_003C_003E2__current = _003Csmooth_003E5__5;
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0792;
				}
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
		IEnumerator<TerrainDef> IEnumerable<TerrainDef>.GetEnumerator()
		{
			_003CImpliedTerrainDefs_003Ed__0 _003CImpliedTerrainDefs_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CImpliedTerrainDefs_003Ed__ = this;
			}
			else
			{
				_003CImpliedTerrainDefs_003Ed__ = new _003CImpliedTerrainDefs_003Ed__0(0);
			}
			_003CImpliedTerrainDefs_003Ed__.hotReload = _003C_003E3__hotReload;
			return _003CImpliedTerrainDefs_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TerrainDef>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CImpliedTerrainDefs_003Ed__0))]
	public static IEnumerable<TerrainDef> ImpliedTerrainDefs(bool hotReload = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CImpliedTerrainDefs_003Ed__0(-2)
		{
			_003C_003E3__hotReload = hotReload
		};
	}

	private static void SetAsteroidEdgeData(TerrainDef terrain)
	{
		if (ModsConfig.OdysseyActive)
		{
			if (terrain.spaceEdgeGraphicData == null)
			{
				terrain.spaceEdgeGraphicData = new TerrainDef.SpaceEdgeGraphicData
				{
					OShapeTexPath = "Terrain/Surfaces/Asteroid/BorderInside/AsteroidBorderInside_OShape",
					UShapeTexPath = "Terrain/Surfaces/Asteroid/BorderInside/AsteroidBorderInside_UShape",
					CornerInnerTexPath = "Terrain/Surfaces/Asteroid/BorderInside/AsteroidBorderInside_CornerInner",
					CornerOuterTexPath = "Terrain/Surfaces/Asteroid/BorderInside/AsteroidBorderInside_CornerOuter",
					FlatTexPath = "Terrain/Surfaces/Asteroid/BorderInside/AsteroidBorderInside_Flat",
					LoopLeftTexPath = "Terrain/Surfaces/Asteroid/AsteroidEdge_Left",
					LoopRightTexPath = "Terrain/Surfaces/Asteroid/AsteroidEdge_Right",
					LoopSingleTexPath = "Terrain/Surfaces/Asteroid/AsteroidEdge_Single",
					LoopTexPaths = new List<string> { "Terrain/Surfaces/Asteroid/Loop/AsteroidEdge_Loop1", "Terrain/Surfaces/Asteroid/Loop/AsteroidEdge_Loop2", "Terrain/Surfaces/Asteroid/Loop/AsteroidEdge_Loop3", "Terrain/Surfaces/Asteroid/Loop/AsteroidEdge_Loop4", "Terrain/Surfaces/Asteroid/Loop/AsteroidEdge_Loop5" }
				};
			}
		}
	}
}
