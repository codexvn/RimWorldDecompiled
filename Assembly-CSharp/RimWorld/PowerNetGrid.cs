using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002152 RID: 8530
	public class PowerNetGrid
	{
		// Token: 0x0600BA67 RID: 47719 RVA: 0x0035CBC8 File Offset: 0x0035ADC8
		public PowerNetGrid(Map map)
		{
			this.map = map;
			this.netGrid = new PowerNet[map.cellIndices.NumGridCells];
		}

		// Token: 0x0600BA68 RID: 47720 RVA: 0x0035CBF8 File Offset: 0x0035ADF8
		public PowerNet TransmittedPowerNetAt(IntVec3 c)
		{
			return this.netGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600BA69 RID: 47721 RVA: 0x0035CC14 File Offset: 0x0035AE14
		public void Notify_PowerNetCreated(PowerNet newNet)
		{
			if (this.powerNetCells.ContainsKey(newNet))
			{
				Log.Warning("Net " + ((newNet != null) ? newNet.ToString() : null) + " is already registered in PowerNetGrid.");
				this.powerNetCells.Remove(newNet);
			}
			List<IntVec3> list = new List<IntVec3>();
			this.powerNetCells.Add(newNet, list);
			for (int i = 0; i < newNet.transmitters.Count; i++)
			{
				CellRect cellRect = newNet.transmitters[i].parent.OccupiedRect();
				for (int j = cellRect.minZ; j <= cellRect.maxZ; j++)
				{
					for (int k = cellRect.minX; k <= cellRect.maxX; k++)
					{
						int num = this.map.cellIndices.CellToIndex(k, j);
						if (this.netGrid[num] != null)
						{
							Log.Warning(string.Concat(new string[]
							{
								"Two power nets on the same cell (",
								k.ToString(),
								", ",
								j.ToString(),
								"). First transmitters: ",
								newNet.transmitters[0].parent.LabelCap,
								" and ",
								this.netGrid[num].transmitters.NullOrEmpty<CompPower>() ? "[none]" : this.netGrid[num].transmitters[0].parent.LabelCap,
								"."
							}));
						}
						this.netGrid[num] = newNet;
						list.Add(new IntVec3(k, 0, j));
					}
				}
			}
		}

		// Token: 0x0600BA6A RID: 47722 RVA: 0x0035CDC0 File Offset: 0x0035AFC0
		public void Notify_PowerNetDeleted(PowerNet deadNet)
		{
			List<IntVec3> list;
			if (!this.powerNetCells.TryGetValue(deadNet, out list))
			{
				Log.Warning("Net " + ((deadNet != null) ? deadNet.ToString() : null) + " does not exist in PowerNetGrid's dictionary.");
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				int num = this.map.cellIndices.CellToIndex(list[i]);
				if (this.netGrid[num] == deadNet)
				{
					this.netGrid[num] = null;
				}
				else
				{
					Log.Warning("Multiple nets on the same cell " + list[i].ToString() + ". This is probably a result of an earlier error.");
				}
			}
			this.powerNetCells.Remove(deadNet);
		}

		// Token: 0x0600BA6B RID: 47723 RVA: 0x0035CE74 File Offset: 0x0035B074
		public void DrawDebugPowerNetGrid()
		{
			if (!DebugViewSettings.drawPowerNetGrid)
			{
				return;
			}
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (this.map != Find.CurrentMap)
			{
				return;
			}
			Rand.PushState();
			foreach (IntVec3 c in Find.CameraDriver.CurrentViewRect.ClipInsideMap(this.map))
			{
				PowerNet powerNet = this.netGrid[this.map.cellIndices.CellToIndex(c)];
				if (powerNet != null)
				{
					Rand.Seed = powerNet.GetHashCode();
					CellRenderer.RenderCell(c, Rand.Value);
				}
			}
			Rand.PopState();
		}

		// Token: 0x04008088 RID: 32904
		private Map map;

		// Token: 0x04008089 RID: 32905
		private PowerNet[] netGrid;

		// Token: 0x0400808A RID: 32906
		private Dictionary<PowerNet, List<IntVec3>> powerNetCells = new Dictionary<PowerNet, List<IntVec3>>();
	}
}
