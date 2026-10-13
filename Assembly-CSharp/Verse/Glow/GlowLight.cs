using System;
using RimWorld;
using Unity.Burst;
using UnityEngine;

namespace Verse.Glow
{
	// Token: 0x02000DF5 RID: 3573
	[BurstCompile]
	public struct GlowLight
	{
		// Token: 0x06005AE1 RID: 23265 RVA: 0x001D2AFC File Offset: 0x001D0CFC
		public GlowLight(CompGlower glower, int glowPoolIndex)
		{
			this.overlightRadius = glower.Props.overlightRadius;
			this.glowRadius = glower.GlowRadius;
			this.glowColor = glower.GlowColor;
			bool flag;
			if (!glower.Props.overrideIsCavePlant)
			{
				PlantProperties plant = glower.parent.def.plant;
				flag = (plant != null && plant.cavePlant);
			}
			else
			{
				flag = true;
			}
			this.isCavePlant = flag;
			this.id = glower.parent.thingIDNumber;
			this.localGlowPoolIndex = glowPoolIndex;
			this.dirty = true;
			this.isTerrain = false;
			this.position = glower.parent.Position;
			this.diameter = Mathf.CeilToInt(glower.GlowRadius * 2f + 1f);
			this.radius = Mathf.CeilToInt(glower.GlowRadius);
			this.localGlowGridStartPos = this.position - new IntVec3(this.radius, 0, this.radius);
		}

		// Token: 0x06005AE2 RID: 23266 RVA: 0x001D2BF0 File Offset: 0x001D0DF0
		public GlowLight(IntVec3 cell, Map map, int glowPoolIndex)
		{
			this.id = map.cellIndices.CellToIndex(cell);
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(this.id);
			this.overlightRadius = 0f;
			this.glowRadius = terrainDef.glowRadius;
			this.glowColor = terrainDef.glowColor;
			this.isCavePlant = false;
			this.isTerrain = true;
			this.localGlowPoolIndex = glowPoolIndex;
			this.dirty = true;
			this.position = cell;
			this.diameter = Mathf.CeilToInt(this.glowRadius * 2f + 1f);
			this.radius = Mathf.CeilToInt(this.glowRadius);
			this.localGlowGridStartPos = this.position - new IntVec3(this.radius, 0, this.radius);
		}

		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x06005AE3 RID: 23267 RVA: 0x001D2CB8 File Offset: 0x001D0EB8
		public CellRect AffectedRect
		{
			get
			{
				return new CellRect(this.localGlowGridStartPos.x, this.localGlowGridStartPos.z, this.diameter, this.diameter);
			}
		}

		// Token: 0x06005AE4 RID: 23268 RVA: 0x001D2CE4 File Offset: 0x001D0EE4
		[BurstCompile]
		public int WorldToLocalIndex(in IntVec3 world)
		{
			IntVec3 intVec = world - this.position;
			return this.DeltaToLocalIndex(intVec);
		}

		// Token: 0x06005AE5 RID: 23269 RVA: 0x001D2D0B File Offset: 0x001D0F0B
		[BurstCompile]
		public int DeltaToLocalIndex(in IntVec3 delta)
		{
			return CellIndicesUtility.CellToIndex(new IntVec3(this.radius, 0, this.radius) + delta, this.diameter);
		}

		// Token: 0x06005AE6 RID: 23270 RVA: 0x001D2D35 File Offset: 0x001D0F35
		[BurstCompile]
		public IntVec3 IndexToLocalDelta(int index)
		{
			return CellIndicesUtility.IndexToCell(index, this.diameter) - new IntVec3(this.radius, 0, this.radius);
		}

		// Token: 0x06005AE7 RID: 23271 RVA: 0x001D2D5C File Offset: 0x001D0F5C
		public override string ToString()
		{
			return string.Format("{0} with radius {1} [ID: {2}, Rect: {3}], rgba: {4}, isCavePlant: {5}, isTerrain: {6}, localGlowPoolIndex: {7}", new object[]
			{
				this.position,
				this.glowRadius,
				this.id,
				this.AffectedRect,
				this.glowColor,
				this.isCavePlant,
				this.isTerrain,
				this.localGlowPoolIndex
			});
		}

		// Token: 0x040041D1 RID: 16849
		public readonly float overlightRadius;

		// Token: 0x040041D2 RID: 16850
		public readonly float glowRadius;

		// Token: 0x040041D3 RID: 16851
		public readonly ColorInt glowColor;

		// Token: 0x040041D4 RID: 16852
		public readonly bool isCavePlant;

		// Token: 0x040041D5 RID: 16853
		public readonly bool isTerrain;

		// Token: 0x040041D6 RID: 16854
		public readonly IntVec3 position;

		// Token: 0x040041D7 RID: 16855
		public readonly IntVec3 localGlowGridStartPos;

		// Token: 0x040041D8 RID: 16856
		public readonly int radius;

		// Token: 0x040041D9 RID: 16857
		public readonly int diameter;

		// Token: 0x040041DA RID: 16858
		public readonly int id;

		// Token: 0x040041DB RID: 16859
		public int localGlowPoolIndex;

		// Token: 0x040041DC RID: 16860
		public bool dirty;
	}
}
