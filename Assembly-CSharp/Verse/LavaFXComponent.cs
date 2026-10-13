using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200045E RID: 1118
	[StaticConstructorOnStartup]
	public class LavaFXComponent : MapComponent
	{
		// Token: 0x06001D4E RID: 7502 RVA: 0x000943D4 File Offset: 0x000925D4
		public LavaFXComponent(Map map) : base(map)
		{
			map.events.TerrainChanged += this.Notify_OnTerrainChanged;
			this.deepLavaSpawner = new CellFXSpawner(0.015f, new Action<Vector3>(this.EmitDeepLavaFX));
			this.shallowLavaSpawner = new CellFXSpawner(0.015f, new Action<Vector3>(this.EmitShallowLavaFX));
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x00094437 File Offset: 0x00092637
		private void Notify_OnTerrainChanged(IntVec3 cell)
		{
			if (!this.TryAddLavaCell(cell))
			{
				this.deepLavaSpawner.Cells.Remove(cell);
				this.shallowLavaSpawner.Cells.Remove(cell);
			}
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x00094466 File Offset: 0x00092666
		public override void FinalizeInit()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.RefreshLavaCells();
		}

		// Token: 0x06001D51 RID: 7505 RVA: 0x00094476 File Offset: 0x00092676
		public override void MapComponentTick()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			this.deepLavaSpawner.Tick();
			this.shallowLavaSpawner.Tick();
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x00094496 File Offset: 0x00092696
		public void EmitDeepLavaFX(Vector3 pos)
		{
			FleckMaker.ThrowMicroSparks(pos, this.map);
			this.ThrowLavaSmoke(pos, this.map, 1.5f);
		}

		// Token: 0x06001D53 RID: 7507 RVA: 0x000944B6 File Offset: 0x000926B6
		public void EmitShallowLavaFX(Vector3 pos)
		{
			FleckMaker.ThrowMicroSparks(pos, this.map);
			FleckMaker.ThrowSmoke(pos, this.map, 1.5f);
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x000944D8 File Offset: 0x000926D8
		private void RefreshLavaCells()
		{
			this.deepLavaSpawner.Cells.Clear();
			this.shallowLavaSpawner.Cells.Clear();
			for (int i = 0; i < this.map.Size.x; i++)
			{
				for (int j = 0; j < this.map.Size.z; j++)
				{
					this.TryAddLavaCell(new IntVec3(i, 0, j));
				}
			}
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x0009454C File Offset: 0x0009274C
		private bool TryAddLavaCell(IntVec3 cell)
		{
			if (this.map.terrainGrid.TerrainAt(cell) == TerrainDefOf.LavaDeep)
			{
				this.deepLavaSpawner.Cells.Add(cell);
				return true;
			}
			if (this.map.terrainGrid.TempTerrainAt(cell) == TerrainDefOf.LavaShallow)
			{
				this.shallowLavaSpawner.Cells.Add(cell);
				return true;
			}
			return false;
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x000945B0 File Offset: 0x000927B0
		private void ThrowLavaSmoke(Vector3 loc, Map map, float size)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.LavaSmoke, Rand.Range(1.5f, 2.5f) * size);
			dataStatic.rotationRate = Rand.Range(-30f, 30f);
			dataStatic.velocityAngle = (float)Rand.Range(30, 40);
			dataStatic.velocitySpeed = Rand.Range(0.5f, 0.7f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x04001984 RID: 6532
		private CellFXSpawner deepLavaSpawner;

		// Token: 0x04001985 RID: 6533
		private CellFXSpawner shallowLavaSpawner;

		// Token: 0x04001986 RID: 6534
		private const float DeepLavaFXPerTilePerSecond = 0.015f;

		// Token: 0x04001987 RID: 6535
		private const float ShallowLavaFXPerTilePerSecond = 0.015f;
	}
}
