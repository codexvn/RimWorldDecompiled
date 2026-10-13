using System;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200053B RID: 1339
	public class TemperatureVacuumSaveLoad
	{
		// Token: 0x060022C9 RID: 8905 RVA: 0x000B4FE8 File Offset: 0x000B31E8
		public TemperatureVacuumSaveLoad(Map map)
		{
			this.map = map;
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x000B4FF8 File Offset: 0x000B31F8
		public void DoExposeWork()
		{
			byte[] arr = null;
			byte[] arr2 = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.inVacuum = this.map.Biome.inVacuum;
				Scribe_Values.Look<bool>(ref this.inVacuum, "inVacuum", false, false);
				int num = Mathf.RoundToInt(this.map.mapTemperature.OutdoorTemp);
				ushort num2 = this.TempFloatToShort((float)num);
				byte b = this.VacuumFloatToByte(1f);
				ushort[] worldTempGrid = new ushort[this.map.cellIndices.NumGridCells];
				byte[] worldVacGrid = null;
				if (this.map.Biome.inVacuum)
				{
					worldVacGrid = new byte[this.map.cellIndices.NumGridCells];
				}
				for (int i = 0; i < this.map.cellIndices.NumGridCells; i++)
				{
					worldTempGrid[i] = num2;
					if (worldVacGrid != null)
					{
						worldVacGrid[i] = b;
					}
				}
				foreach (Region region in this.map.regionGrid.AllRegions_NoRebuild_InvalidAllowed)
				{
					if (region.Room != null)
					{
						ushort num3 = this.TempFloatToShort(region.Room.Temperature);
						byte b2 = this.VacuumFloatToByte(region.Room.Vacuum);
						foreach (IntVec3 c2 in region.Cells)
						{
							worldTempGrid[this.map.cellIndices.CellToIndex(c2)] = num3;
							if (worldVacGrid != null)
							{
								worldVacGrid[this.map.cellIndices.CellToIndex(c2)] = b2;
							}
						}
					}
				}
				arr = MapSerializeUtility.SerializeUshort(this.map, (IntVec3 c) => worldTempGrid[this.map.cellIndices.CellToIndex(c)]);
				if (worldVacGrid != null)
				{
					arr2 = MapSerializeUtility.SerializeByte(this.map, (IntVec3 c) => worldVacGrid[this.map.cellIndices.CellToIndex(c)]);
				}
			}
			DataExposeUtility.LookByteArray(ref arr, "temperatures");
			DataExposeUtility.LookByteArray(ref arr2, "vacuum");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Scribe_Values.Look<bool>(ref this.inVacuum, "inVacuum", false, false);
				this.tempGrid = new ushort[this.map.cellIndices.NumGridCells];
				if (this.inVacuum)
				{
					this.vacuumGrid = new byte[this.map.cellIndices.NumGridCells];
				}
				MapSerializeUtility.LoadUshort(arr, this.map, delegate(IntVec3 c, ushort val)
				{
					this.tempGrid[this.map.cellIndices.CellToIndex(c)] = val;
				});
				if (this.inVacuum)
				{
					MapSerializeUtility.LoadByte(arr2, this.map, delegate(IntVec3 c, byte val)
					{
						this.vacuumGrid[this.map.cellIndices.CellToIndex(c)] = val;
					});
				}
			}
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x000B52E4 File Offset: 0x000B34E4
		public void ApplyLoadedDataToRegions()
		{
			if (this.tempGrid == null && this.vacuumGrid == null)
			{
				return;
			}
			CellIndices cellIndices = this.map.cellIndices;
			foreach (Region region in this.map.regionGrid.AllRegions_NoRebuild_InvalidAllowed)
			{
				if (region.Room != null)
				{
					int num = cellIndices.CellToIndex(region.Cells.First<IntVec3>());
					if (this.tempGrid != null)
					{
						region.Room.Temperature = this.TempShortToFloat(this.tempGrid[num]);
					}
					if (this.vacuumGrid != null)
					{
						region.Room.Vacuum = this.VacuumByteToFloat(this.vacuumGrid[num]);
					}
				}
			}
			this.tempGrid = null;
			this.vacuumGrid = null;
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x000B53BC File Offset: 0x000B35BC
		private ushort TempFloatToShort(float temp)
		{
			temp = Mathf.Clamp(temp, -273.15f, 1000f);
			temp *= 16f;
			return (ushort)((int)temp + 32768);
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x000B53E2 File Offset: 0x000B35E2
		private float TempShortToFloat(ushort temp)
		{
			return ((float)temp - 32768f) / 16f;
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x000B53F2 File Offset: 0x000B35F2
		private byte VacuumFloatToByte(float vacuum)
		{
			return (byte)(vacuum * 255f);
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x000B53FC File Offset: 0x000B35FC
		private float VacuumByteToFloat(byte vacuum)
		{
			return (float)vacuum / 255f;
		}

		// Token: 0x04001D21 RID: 7457
		private readonly Map map;

		// Token: 0x04001D22 RID: 7458
		private ushort[] tempGrid;

		// Token: 0x04001D23 RID: 7459
		private byte[] vacuumGrid;

		// Token: 0x04001D24 RID: 7460
		private bool inVacuum;
	}
}
