using System;
using System.Collections.Generic;
using LudeonTK;
using RimWorld;
using Unity.Collections;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02001046 RID: 4166
	public class AvoidGrid : IDisposable
	{
		// Token: 0x17000FD2 RID: 4050
		// (get) Token: 0x06006585 RID: 25989 RVA: 0x002029E6 File Offset: 0x00200BE6
		public NativeArray<byte>.ReadOnly Grid
		{
			get
			{
				if (this.gridDirty)
				{
					this.Regenerate();
				}
				return this.grid.AsReadOnly();
			}
		}

		// Token: 0x17000FD3 RID: 4051
		public byte this[IntVec3 c]
		{
			get
			{
				return this.grid[this.map.cellIndices.CellToIndex(c)];
			}
			private set
			{
				this.grid[this.map.cellIndices.CellToIndex(c)] = value;
			}
		}

		// Token: 0x06006588 RID: 25992 RVA: 0x00202A40 File Offset: 0x00200C40
		public AvoidGrid(Map map)
		{
			this.map = map;
			this.grid = new NativeArray<byte>(map.cellIndices.NumGridCells, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			map.events.BuildingSpawned += this.Notify_BuildingSpawned;
			map.events.BuildingDespawned += this.Notify_BuildingDespawned;
		}

		// Token: 0x06006589 RID: 25993 RVA: 0x00202AA7 File Offset: 0x00200CA7
		public void Dispose()
		{
			this.grid.Dispose();
		}

		// Token: 0x0600658A RID: 25994 RVA: 0x00202AB4 File Offset: 0x00200CB4
		public void Regenerate()
		{
			this.gridDirty = false;
			this.grid.Clear<byte>();
			List<Building> allBuildingsColonist = this.map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				if (allBuildingsColonist[i].def.building.ai_combatDangerous)
				{
					Building_TurretGun building_TurretGun = allBuildingsColonist[i] as Building_TurretGun;
					if (building_TurretGun != null)
					{
						this.PrintAvoidGridAroundTurret(building_TurretGun);
					}
				}
			}
			this.ExpandAvoidGridIntoEdifices();
		}

		// Token: 0x0600658B RID: 25995 RVA: 0x00202B2A File Offset: 0x00200D2A
		private void Notify_BuildingSpawned(Building building)
		{
			if (building.def.building.ai_combatDangerous || !building.CanBeSeenOver())
			{
				this.gridDirty = true;
			}
		}

		// Token: 0x0600658C RID: 25996 RVA: 0x00202B2A File Offset: 0x00200D2A
		private void Notify_BuildingDespawned(Building building)
		{
			if (building.def.building.ai_combatDangerous || !building.CanBeSeenOver())
			{
				this.gridDirty = true;
			}
		}

		// Token: 0x0600658D RID: 25997 RVA: 0x00202B4D File Offset: 0x00200D4D
		public void DebugDrawOnMap()
		{
			if (DebugViewSettings.drawAvoidGrid && Find.CurrentMap == this.map)
			{
				this.DebugDraw();
			}
		}

		// Token: 0x0600658E RID: 25998 RVA: 0x00202B6C File Offset: 0x00200D6C
		private void PrintAvoidGridAroundTurret(Building_TurretGun tur)
		{
			float range = tur.GunCompEq.PrimaryVerb.verbProps.range;
			float num = tur.GunCompEq.PrimaryVerb.verbProps.EffectiveMinRange(true);
			int num2 = GenRadial.NumCellsInRadius(range + 4f);
			for (int i = (num < 1f) ? 0 : GenRadial.NumCellsInRadius(num); i < num2; i++)
			{
				IntVec3 intVec = tur.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(tur.Map) && intVec.WalkableByNormal(tur.Map) && GenSight.LineOfSight(intVec, tur.Position, tur.Map, true, null, 0, 0))
				{
					this.IncrementAvoidGrid(intVec, 45);
				}
			}
		}

		// Token: 0x0600658F RID: 25999 RVA: 0x00202C24 File Offset: 0x00200E24
		private void IncrementAvoidGrid(IntVec3 c, int num)
		{
			byte b = this[c];
			b = (byte)Mathf.Min(255, (int)b + num);
			this[c] = b;
		}

		// Token: 0x06006590 RID: 26000 RVA: 0x00202C50 File Offset: 0x00200E50
		private void ExpandAvoidGridIntoEdifices()
		{
			int numGridCells = this.map.cellIndices.NumGridCells;
			for (int i = 0; i < numGridCells; i++)
			{
				if (this.grid[i] != 0 && this.map.edificeGrid[i] == null)
				{
					for (int j = 0; j < 8; j++)
					{
						IntVec3 c = this.map.cellIndices.IndexToCell(i) + GenAdj.AdjacentCells[j];
						if (c.InBounds(this.map) && c.GetEdifice(this.map) != null)
						{
							this[c] = (byte)Mathf.Min(255, Mathf.Max((int)this[c], (int)this.grid[i]));
						}
					}
				}
			}
		}

		// Token: 0x06006591 RID: 26001 RVA: 0x00202D18 File Offset: 0x00200F18
		private void DebugDraw()
		{
			for (int i = 0; i < this.grid.Length; i++)
			{
				byte b = this.grid[i];
				if (b > 0)
				{
					CellRenderer.RenderCell(this.map.cellIndices.IndexToCell(i), (float)b / 255f * 0.5f);
				}
			}
		}

		// Token: 0x040048AB RID: 18603
		public readonly Map map;

		// Token: 0x040048AC RID: 18604
		private NativeArray<byte> grid;

		// Token: 0x040048AD RID: 18605
		private bool gridDirty = true;
	}
}
