using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200049B RID: 1179
	public abstract class GenStep_Scatterer : GenStep
	{
		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x06001EF3 RID: 7923 RVA: 0x000A1AE8 File Offset: 0x0009FCE8
		private bool HasFallbackValidators
		{
			get
			{
				return this.fallbackValidators != null && this.fallbackValidators.Count > 0;
			}
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x000A1B04 File Offset: 0x0009FD04
		public override void Generate(Map map, GenStepParams parms)
		{
			this.useFallback = false;
			if (this.ShouldSkipMap(map))
			{
				return;
			}
			this.usedSpots.Clear();
			int num = this.CalculateFinalCount(map);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec;
				if (!this.TryFindScatterCell(map, out intVec))
				{
					if (!this.HasFallbackValidators)
					{
						return;
					}
					this.useFallback = true;
					if (!this.TryFindScatterCell(map, out intVec))
					{
						return;
					}
				}
				this.ScatterAt(intVec, map, parms, 1);
				this.usedSpots.Add(intVec);
			}
			this.usedSpots.Clear();
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x000A1B8C File Offset: 0x0009FD8C
		protected virtual bool ShouldSkipMap(Map map)
		{
			return (!this.allowInWaterBiome && map.TileInfo.WaterCovered) || (this.onlyOnStartingMap && !map.IsStartingMap) || (ModsConfig.BiotechActive && map.TileInfo.pollution < this.minPollution) || (ModsConfig.BiotechActive && Find.History.mechanoidDatacoreReadOrLost && !this.allowMechanoidDatacoreReadOrLost);
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x000A1BFC File Offset: 0x0009FDFC
		protected virtual bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			if (this.nearMapCenter)
			{
				if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => this.CanScatterAt(x, map), map, out result))
				{
					return true;
				}
			}
			else
			{
				if (this.nearPlayerStart)
				{
					result = CellFinder.RandomClosewalkCellNear(MapGenerator.PlayerStartSpot, map, 20, (IntVec3 x) => this.CanScatterAt(x, map));
					return true;
				}
				if (CellFinderLoose.TryFindRandomNotEdgeCellWith(5, (IntVec3 x) => this.CanScatterAt(x, map), map, out result))
				{
					return true;
				}
			}
			if (this.warnOnFail)
			{
				if (this.HasFallbackValidators && !this.useFallback)
				{
					Log.Warning(string.Concat(new string[]
					{
						"Scatterer ",
						this.ToString(),
						" from def ",
						this.def.defName,
						" could not find cell to generate at, trying fallback validators."
					}));
				}
				else
				{
					Log.Warning(string.Concat(new string[]
					{
						"Scatterer ",
						this.ToString(),
						" from def ",
						this.def.defName,
						" could not find cell to generate at."
					}));
				}
			}
			return false;
		}

		// Token: 0x06001EF7 RID: 7927
		protected abstract void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1);

		// Token: 0x06001EF8 RID: 7928 RVA: 0x000A1D28 File Offset: 0x0009FF28
		protected virtual bool CanScatterAt(IntVec3 loc, Map map)
		{
			using (List<LayoutStructureSketch>.Enumerator enumerator = map.layoutStructureSketches.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.layoutSketch.OccupiedRect.Contains(loc))
					{
						return false;
					}
				}
			}
			if (this.extraNoBuildEdgeDist > 0 && loc.CloseToEdge(map, this.extraNoBuildEdgeDist + 10))
			{
				return false;
			}
			if (this.minEdgeDist > 0 && loc.CloseToEdge(map, this.minEdgeDist))
			{
				return false;
			}
			if (this.minEdgeDistPct > 0f && loc.CloseToEdge(map, (int)(this.minEdgeDistPct * (float)Mathf.Min(map.Size.x, map.Size.z))))
			{
				return false;
			}
			if (this.NearUsedSpot(loc, this.CalculateFinalMinSpacing(map)))
			{
				return false;
			}
			if (!this.useFallback)
			{
				if (this.minDistToPlayerStart > 0 && (map.Center - loc).LengthHorizontalSquared < this.minDistToPlayerStart * this.minDistToPlayerStart)
				{
					return false;
				}
				if (this.minDistToPlayerStartPct > 0f && (map.Center - loc).LengthHorizontal < this.minDistToPlayerStartPct * (float)Mathf.Min(map.Size.x, map.Size.z))
				{
					return false;
				}
			}
			if (this.spotMustBeStandable && !loc.Standable(map))
			{
				return false;
			}
			if (!this.allowFoggedPositions && loc.Fogged(map))
			{
				return false;
			}
			if (!this.allowRoofed && loc.Roofed(map))
			{
				return false;
			}
			if (this.useFallback)
			{
				if (this.fallbackValidators != null)
				{
					for (int i = 0; i < this.fallbackValidators.Count; i++)
					{
						if (!this.fallbackValidators[i].Allows(loc, map))
						{
							return false;
						}
					}
				}
			}
			else if (this.validators != null)
			{
				for (int j = 0; j < this.validators.Count; j++)
				{
					if (!this.validators[j].Allows(loc, map))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x000A1F4C File Offset: 0x000A014C
		protected bool NearUsedSpot(IntVec3 c, float dist)
		{
			for (int i = 0; i < this.usedSpots.Count; i++)
			{
				if ((float)(this.usedSpots[i] - c).LengthHorizontalSquared <= dist * dist)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x000A1F92 File Offset: 0x000A0192
		protected virtual int CalculateFinalCount(Map map)
		{
			if (this.count < 0)
			{
				return Mathf.RoundToInt((float)GenStep_Scatterer.CountFromPer10kCells(this.countPer10kCellsRange.RandomInRange, map, -1) * this.GetPlacementFactor(map));
			}
			return Mathf.RoundToInt((float)this.count * this.GetPlacementFactor(map));
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x000A1FD4 File Offset: 0x000A01D4
		protected virtual float GetPlacementFactor(Map map)
		{
			float num = 1f;
			if (this.isJunk)
			{
				foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
				{
					num *= tileMutatorDef.junkDensityFactor;
				}
			}
			return num;
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x000A2038 File Offset: 0x000A0238
		public static int CountFromPer10kCells(float countPer10kCells, Map map, int mapSize = -1)
		{
			if (mapSize < 0)
			{
				mapSize = map.Size.x;
			}
			int num = Mathf.RoundToInt(10000f / countPer10kCells);
			return Mathf.RoundToInt((float)(mapSize * mapSize) / (float)num);
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x000A2070 File Offset: 0x000A0270
		public virtual float CalculateFinalMinSpacing(Map map)
		{
			float placementFactor = this.GetPlacementFactor(map);
			if (placementFactor <= 0f)
			{
				return 0f;
			}
			return this.minSpacing / placementFactor;
		}

		// Token: 0x06001EFE RID: 7934 RVA: 0x000A209C File Offset: 0x000A029C
		public void ForceScatterAt(IntVec3 loc, Map map)
		{
			this.ScatterAt(loc, map, default(GenStepParams), 1);
		}

		// Token: 0x04001A7D RID: 6781
		public int count = -1;

		// Token: 0x04001A7E RID: 6782
		public FloatRange countPer10kCellsRange = FloatRange.Zero;

		// Token: 0x04001A7F RID: 6783
		public bool nearPlayerStart;

		// Token: 0x04001A80 RID: 6784
		public bool nearMapCenter;

		// Token: 0x04001A81 RID: 6785
		public float minSpacing = 10f;

		// Token: 0x04001A82 RID: 6786
		public bool spotMustBeStandable;

		// Token: 0x04001A83 RID: 6787
		public int minDistToPlayerStart;

		// Token: 0x04001A84 RID: 6788
		public float minDistToPlayerStartPct;

		// Token: 0x04001A85 RID: 6789
		public int minEdgeDist;

		// Token: 0x04001A86 RID: 6790
		public float minEdgeDistPct;

		// Token: 0x04001A87 RID: 6791
		public int extraNoBuildEdgeDist;

		// Token: 0x04001A88 RID: 6792
		public List<ScattererValidator> validators = new List<ScattererValidator>();

		// Token: 0x04001A89 RID: 6793
		public List<ScattererValidator> fallbackValidators = new List<ScattererValidator>();

		// Token: 0x04001A8A RID: 6794
		public bool allowInWaterBiome = true;

		// Token: 0x04001A8B RID: 6795
		public bool allowFoggedPositions = true;

		// Token: 0x04001A8C RID: 6796
		public bool allowRoofed = true;

		// Token: 0x04001A8D RID: 6797
		public bool onlyOnStartingMap;

		// Token: 0x04001A8E RID: 6798
		public float minPollution;

		// Token: 0x04001A8F RID: 6799
		public bool allowMechanoidDatacoreReadOrLost = true;

		// Token: 0x04001A90 RID: 6800
		public bool isJunk;

		// Token: 0x04001A91 RID: 6801
		public bool warnOnFail = true;

		// Token: 0x04001A92 RID: 6802
		[Unsaved(false)]
		protected List<IntVec3> usedSpots = new List<IntVec3>();

		// Token: 0x04001A93 RID: 6803
		[Unsaved(false)]
		protected bool useFallback;

		// Token: 0x04001A94 RID: 6804
		private const int ScatterNearPlayerRadius = 20;
	}
}
