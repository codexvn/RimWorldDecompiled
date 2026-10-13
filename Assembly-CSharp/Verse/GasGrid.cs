using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200042B RID: 1067
	public class GasGrid : IExposable
	{
		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06001B12 RID: 6930 RVA: 0x000888EE File Offset: 0x00086AEE
		public bool CalculateGasEffects
		{
			get
			{
				return this.anyGasEverAdded;
			}
		}

		// Token: 0x06001B13 RID: 6931 RVA: 0x000888F8 File Offset: 0x00086AF8
		public GasGrid(Map map)
		{
			this.map = map;
			this.gasDensity = new uint[map.cellIndices.NumGridCells];
			this.cardinalDirections = new List<IntVec3>();
			this.cardinalDirections.AddRange(GenAdj.CardinalDirections);
			this.cycleIndexDiffusion = Rand.Range(0, map.Area / 2);
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x00088958 File Offset: 0x00086B58
		public void RecalculateEverHadGas()
		{
			this.anyGasEverAdded = false;
			for (int i = 0; i < this.gasDensity.Length; i++)
			{
				if (this.gasDensity[i] > 0U)
				{
					this.anyGasEverAdded = true;
					return;
				}
			}
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x00088994 File Offset: 0x00086B94
		public void Tick()
		{
			if (!this.CalculateGasEffects)
			{
				return;
			}
			int area = this.map.Area;
			int num = Mathf.CeilToInt((float)area * 0.015625f);
			this.cellsInRandomOrder = this.map.cellsInRandomOrder.GetAll();
			using (new ProfilerBlock("Dissipation"))
			{
				for (int i = 0; i < num; i++)
				{
					if (this.cycleIndexDissipation >= area)
					{
						this.cycleIndexDissipation = 0;
					}
					using (new ProfilerBlock("TryDissipateGases"))
					{
						this.TryDissipateGases(CellIndicesUtility.CellToIndex(this.cellsInRandomOrder[this.cycleIndexDissipation], this.map.Size.x));
					}
					this.cycleIndexDissipation++;
				}
			}
			using (new ProfilerBlock("Diffusion"))
			{
				num = Mathf.CeilToInt((float)area * 0.03125f);
				for (int j = 0; j < num; j++)
				{
					if (this.cycleIndexDiffusion >= area)
					{
						this.cycleIndexDiffusion = 0;
					}
					using (new ProfilerBlock("TryDiffuseGases"))
					{
						this.TryDiffuseGases(this.cellsInRandomOrder[this.cycleIndexDiffusion]);
					}
					this.cycleIndexDiffusion++;
				}
			}
			if (this.map.IsHashIntervalTick(600))
			{
				this.RecalculateEverHadGas();
			}
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x00088B40 File Offset: 0x00086D40
		public bool AnyGasAt(IntVec3 cell)
		{
			return this.AnyGasAt(CellIndicesUtility.CellToIndex(cell, this.map.Size.x));
		}

		// Token: 0x06001B17 RID: 6935 RVA: 0x00088B5E File Offset: 0x00086D5E
		private bool AnyGasAt(int idx)
		{
			return this.gasDensity[idx] > 0U;
		}

		// Token: 0x06001B18 RID: 6936 RVA: 0x00088B6B File Offset: 0x00086D6B
		public byte DensityAt(IntVec3 cell, GasType gasType)
		{
			return this.DensityAt(CellIndicesUtility.CellToIndex(cell, this.map.info.Size.x), gasType);
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x00088B8F File Offset: 0x00086D8F
		private byte DensityAt(int index, GasType gasType)
		{
			return (byte)(this.gasDensity[index] >> (int)gasType & 255U);
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x00088BA5 File Offset: 0x00086DA5
		public float DensityPercentAt(IntVec3 cell, GasType gasType)
		{
			return (float)this.DensityAt(cell, gasType) / 255f;
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x00088BB8 File Offset: 0x00086DB8
		public void AddGas(IntVec3 cell, GasType gasType, int amount, bool canOverflow = true)
		{
			if (amount <= 0 || !this.GasCanMoveTo(cell))
			{
				return;
			}
			this.anyGasEverAdded = true;
			int index = CellIndicesUtility.CellToIndex(cell, this.map.Size.x);
			byte b = this.DensityAt(index, GasType.BlindSmoke);
			byte b2 = this.DensityAt(index, GasType.ToxGas);
			byte b3 = this.DensityAt(index, GasType.RotStink);
			byte b4 = this.DensityAt(index, GasType.DeadlifeDust);
			int num = 0;
			if (gasType <= GasType.ToxGas)
			{
				if (gasType == GasType.BlindSmoke)
				{
					b = this.AdjustedDensity((int)b + amount, out num);
					goto IL_CE;
				}
				if (gasType == GasType.ToxGas)
				{
					if (!ModLister.CheckBiotech("Tox gas"))
					{
						return;
					}
					b2 = this.AdjustedDensity((int)b2 + amount, out num);
					goto IL_CE;
				}
			}
			else
			{
				if (gasType == GasType.RotStink)
				{
					b3 = this.AdjustedDensity((int)b3 + amount, out num);
					goto IL_CE;
				}
				if (gasType == GasType.DeadlifeDust)
				{
					if (!ModLister.CheckAnomaly("Deadlife dust"))
					{
						return;
					}
					b4 = this.AdjustedDensity((int)b4 + amount, out num);
					goto IL_CE;
				}
			}
			Log.Error("Trying to add unknown gas type.");
			return;
			IL_CE:
			this.SetDirect(index, b, b2, b3, b4);
			this.map.mapDrawer.MapMeshDirty(cell, MapMeshFlagDefOf.Gas);
			if (canOverflow && num > 0)
			{
				this.Overflow(cell, gasType, num);
			}
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x00088CCD File Offset: 0x00086ECD
		private byte AdjustedDensity(int newDensity, out int overflow)
		{
			if (newDensity > 255)
			{
				overflow = newDensity - 255;
				return byte.MaxValue;
			}
			overflow = 0;
			if (newDensity < 0)
			{
				return 0;
			}
			return (byte)newDensity;
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x00088CF4 File Offset: 0x00086EF4
		public Vector4 DensitiesAt(IntVec3 cell)
		{
			int index = CellIndicesUtility.CellToIndex(cell, this.map.Size.x);
			float x = (float)this.DensityAt(index, GasType.BlindSmoke);
			float y = (float)this.DensityAt(index, GasType.ToxGas);
			float z = (float)this.DensityAt(index, GasType.RotStink);
			float w = (float)this.DensityAt(index, GasType.DeadlifeDust);
			return new Vector4(x, y, z, w);
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x00088D4C File Offset: 0x00086F4C
		public void Notify_ThingSpawned(Thing thing)
		{
			if (thing.Spawned && thing.def.Fillage == FillCategory.Full)
			{
				foreach (IntVec3 intVec in thing.OccupiedRect())
				{
					if (this.AnyGasAt(intVec))
					{
						this.gasDensity[CellIndicesUtility.CellToIndex(intVec, this.map.Size.x)] = 0U;
						this.map.mapDrawer.MapMeshDirty(intVec, MapMeshFlagDefOf.Gas);
					}
				}
			}
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x00088DF8 File Offset: 0x00086FF8
		public uint GetDirect(IntVec3 c)
		{
			return this.gasDensity[CellIndicesUtility.CellToIndex(c, this.map.Size.x)];
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x00088E17 File Offset: 0x00087017
		public uint GetDirect(int index)
		{
			return this.gasDensity[index];
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x00088E21 File Offset: 0x00087021
		public void SetDirect(IntVec3 c, byte smoke, byte toxic, byte rotStink, byte deadlife)
		{
			this.SetDirect(CellIndicesUtility.CellToIndex(c, this.map.Size.x), smoke, toxic, rotStink, deadlife);
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x00088E45 File Offset: 0x00087045
		public void SetDirect(int index, byte smoke, byte toxic, byte rotStink, byte deadlife)
		{
			if (!ModsConfig.BiotechActive)
			{
				toxic = 0;
			}
			if (!ModsConfig.AnomalyActive)
			{
				deadlife = 0;
			}
			this.gasDensity[index] = (uint)((int)rotStink << 16 | (int)toxic << 8 | (int)smoke | (int)deadlife << 24);
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x00088E74 File Offset: 0x00087074
		public void SetDirect(IntVec3 c, uint packedDensities)
		{
			this.SetDirect(CellIndicesUtility.CellToIndex(c, this.map.Size.x), packedDensities);
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x00088E94 File Offset: 0x00087094
		public void SetDirect(int index, uint packedDensities)
		{
			uint num = uint.MaxValue;
			if (!ModsConfig.BiotechActive)
			{
				num &= 4294902015U;
			}
			if (!ModsConfig.AnomalyActive)
			{
				num &= 16777215U;
			}
			this.gasDensity[index] = (packedDensities & num);
			this.anyGasEverAdded = true;
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x00088ED4 File Offset: 0x000870D4
		private void Overflow(IntVec3 cell, GasType gasType, int amount)
		{
			if (amount <= 0)
			{
				return;
			}
			int remainingAmount = amount;
			this.map.floodFiller.FloodFill(cell, new Predicate<IntVec3>(this.GasCanMoveTo), delegate(IntVec3 c)
			{
				int num = Mathf.Min(remainingAmount, (int)(byte.MaxValue - this.DensityAt(c, gasType)));
				if (num > 0)
				{
					this.AddGas(c, gasType, num, false);
					remainingAmount -= num;
				}
				return remainingAmount <= 0;
			}, GenRadial.NumCellsInRadius(40f), true, null);
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x00088F38 File Offset: 0x00087138
		public void EstimateGasDiffusion(IntVec3 cell, GasType gasType, int amount, Action<IntVec3> processor)
		{
			processor(cell);
			int num = Mathf.Min(amount, (int)(48 - this.DensityAt(cell, gasType)));
			amount -= num;
			this.map.floodFiller.FloodFill(cell, new Predicate<IntVec3>(this.GasCanMoveTo), delegate(IntVec3 c)
			{
				int num2 = Mathf.Min(amount, (int)(48 - this.DensityAt(c, gasType)));
				if (num2 > 0)
				{
					processor(c);
					amount -= num2;
				}
				return amount <= 0;
			}, GenRadial.NumCellsInRadius(40f), true, null);
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x00088FD4 File Offset: 0x000871D4
		private void TryDissipateGases(int index)
		{
			if (!this.AnyGasAt(index))
			{
				return;
			}
			IntVec3 intVec = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			float vacuum = intVec.GetVacuum(this.map);
			float num = intVec.Roofed(this.map) ? 0.5f : 1f;
			num += vacuum * 25f;
			bool flag = false;
			int num2 = (int)this.DensityAt(index, GasType.BlindSmoke);
			if (num2 > 0)
			{
				num2 = Math.Max(num2 - Mathf.RoundToInt(4f * num), 0);
				if (num2 == 0)
				{
					flag = true;
				}
			}
			int num3 = (int)this.DensityAt(index, GasType.ToxGas);
			if (num3 > 0)
			{
				num3 = Math.Max(num3 - Mathf.RoundToInt(3f * num), 0);
				if (num3 == 0)
				{
					flag = true;
				}
			}
			int num4 = (int)this.DensityAt(index, GasType.RotStink);
			if (num4 > 0)
			{
				num4 = Math.Max(num4 - Mathf.RoundToInt(4f * num), 0);
				if (num4 == 0)
				{
					flag = true;
				}
			}
			int num5 = (int)this.DensityAt(index, GasType.DeadlifeDust);
			if (num5 > 0)
			{
				num5 = Math.Max(num5 - Mathf.RoundToInt(3f * num), 0);
				if (num5 == 0)
				{
					flag = true;
				}
			}
			this.SetDirect(index, (byte)num2, (byte)num3, (byte)num4, (byte)num5);
			if (flag)
			{
				this.map.mapDrawer.MapMeshDirty(intVec, MapMeshFlagDefOf.Gas);
			}
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x0008911C File Offset: 0x0008731C
		private void TryDiffuseGases(IntVec3 cell)
		{
			int index = CellIndicesUtility.CellToIndex(cell, this.map.info.Size.x);
			int num = (int)this.DensityAt(index, GasType.ToxGas);
			int num2 = (int)this.DensityAt(index, GasType.RotStink);
			int num3 = (int)this.DensityAt(index, GasType.DeadlifeDust);
			if (num + num2 + num3 < 17)
			{
				return;
			}
			bool flag = false;
			this.cardinalDirections.Shuffle<IntVec3>();
			for (int i = 0; i < this.cardinalDirections.Count; i++)
			{
				IntVec3 intVec = cell + this.cardinalDirections[i];
				if (this.GasCanMoveTo(intVec))
				{
					int index2 = CellIndicesUtility.CellToIndex(intVec, this.map.Size.x);
					int num4 = (int)this.DensityAt(index2, GasType.ToxGas);
					int num5 = (int)this.DensityAt(index2, GasType.RotStink);
					int num6 = (int)this.DensityAt(index2, GasType.DeadlifeDust);
					if (false | this.TryDiffuseIndividualGas(ref num, ref num4) | this.TryDiffuseIndividualGas(ref num2, ref num5) | this.TryDiffuseIndividualGas(ref num3, ref num6))
					{
						this.SetDirect(index2, this.DensityAt(index2, GasType.BlindSmoke), (byte)num4, (byte)num5, (byte)num6);
						this.map.mapDrawer.MapMeshDirty(intVec, MapMeshFlagDefOf.Gas);
						flag = true;
						if (num + num2 < 17)
						{
							break;
						}
					}
				}
			}
			if (flag)
			{
				this.SetDirect(index, this.DensityAt(index, GasType.BlindSmoke), (byte)num, (byte)num2, (byte)num3);
				this.map.mapDrawer.MapMeshDirty(cell, MapMeshFlagDefOf.Gas);
			}
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x00089290 File Offset: 0x00087490
		private bool TryDiffuseIndividualGas(ref int gasA, ref int gasB)
		{
			if (gasA < 17)
			{
				return false;
			}
			int num = Mathf.Abs(gasA - gasB) / 2;
			if (gasA > gasB && num >= 17)
			{
				gasA -= num;
				gasB += num;
				return true;
			}
			return false;
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x000892CC File Offset: 0x000874CC
		public bool GasCanMoveTo(IntVec3 cell)
		{
			if (!cell.InBounds(this.map))
			{
				return false;
			}
			Building edifice = cell.GetEdifice(this.map);
			if (edifice != null && edifice.def.Fillage == FillCategory.Full)
			{
				Building_Door building_Door = edifice as Building_Door;
				return building_Door != null && building_Door.Open;
			}
			return true;
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x0008931C File Offset: 0x0008751C
		public void EqualizeGasThroughBuilding(Building b, bool twoWay)
		{
			GasGrid.<>c__DisplayClass46_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			if (!this.CalculateGasEffects)
			{
				return;
			}
			for (int i = 0; i < GasGrid.beqCells.Length; i++)
			{
				GasGrid.beqCells[i] = IntVec3.Invalid;
			}
			CS$<>8__locals1.beqCellCount = 0;
			CS$<>8__locals1.totalRotStink = 0;
			CS$<>8__locals1.totalToxGas = 0;
			CS$<>8__locals1.totalDeadlifeDust = 0;
			if (twoWay)
			{
				for (int j = 0; j < 2; j++)
				{
					IntVec3 cell = (j == 0) ? (b.Position + b.Rotation.FacingCell) : (b.Position - b.Rotation.FacingCell);
					this.<EqualizeGasThroughBuilding>g__VisitCell|46_0(cell, ref CS$<>8__locals1);
				}
			}
			else
			{
				for (int k = 0; k < 4; k++)
				{
					IntVec3 cell2 = b.Position + GenAdj.CardinalDirections[k];
					this.<EqualizeGasThroughBuilding>g__VisitCell|46_0(cell2, ref CS$<>8__locals1);
				}
			}
			if (CS$<>8__locals1.beqCellCount <= 1)
			{
				return;
			}
			byte toxic = (byte)Mathf.Min(CS$<>8__locals1.totalToxGas / CS$<>8__locals1.beqCellCount, 255);
			byte rotStink = (byte)Mathf.Min(CS$<>8__locals1.totalRotStink / CS$<>8__locals1.beqCellCount, 255);
			byte deadlife = (byte)Mathf.Min(CS$<>8__locals1.totalDeadlifeDust / CS$<>8__locals1.beqCellCount, 255);
			for (int l = 0; l < CS$<>8__locals1.beqCellCount; l++)
			{
				if (GasGrid.beqCells[l].IsValid)
				{
					this.SetDirect(this.map.cellIndices.CellToIndex(GasGrid.beqCells[l]), this.DensityAt(GasGrid.beqCells[l], GasType.BlindSmoke), toxic, rotStink, deadlife);
					this.map.mapDrawer.MapMeshDirty(GasGrid.beqCells[l], MapMeshFlagDefOf.Gas);
				}
			}
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x000894ED File Offset: 0x000876ED
		public void ClearCellUnsafe(IntVec3 cell)
		{
			this.gasDensity[CellIndicesUtility.CellToIndex(cell, this.map.Size.x)] = 0U;
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x00089510 File Offset: 0x00087710
		public void Debug_ClearAll()
		{
			for (int i = 0; i < this.gasDensity.Length; i++)
			{
				this.gasDensity[i] = 0U;
			}
			this.anyGasEverAdded = false;
			this.map.mapDrawer.WholeMapChanged(MapMeshFlagDefOf.Gas);
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x0008955C File Offset: 0x0008775C
		public void Debug_FillAll()
		{
			for (int i = 0; i < this.gasDensity.Length; i++)
			{
				if (this.GasCanMoveTo(this.map.cellIndices.IndexToCell(i)))
				{
					this.SetDirect(i, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
				}
			}
			this.anyGasEverAdded = true;
			this.map.mapDrawer.WholeMapChanged(MapMeshFlagDefOf.Gas);
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x000895D4 File Offset: 0x000877D4
		public void ExposeData()
		{
			MapExposeUtility.ExposeUint(this.map, (IntVec3 c) => this.gasDensity[this.map.cellIndices.CellToIndex(c)], delegate(IntVec3 c, uint val)
			{
				this.gasDensity[this.map.cellIndices.CellToIndex(c)] = val;
			}, "gasDensity");
			Scribe_Values.Look<int>(ref this.cycleIndexDiffusion, "cycleIndexDiffusion", 0, false);
			Scribe_Values.Look<int>(ref this.cycleIndexDissipation, "cycleIndexDissipation", 0, false);
		}

		// Token: 0x06001B31 RID: 6961 RVA: 0x0008963C File Offset: 0x0008783C
		[CompilerGenerated]
		private void <EqualizeGasThroughBuilding>g__VisitCell|46_0(IntVec3 cell, ref GasGrid.<>c__DisplayClass46_0 A_2)
		{
			if (cell.IsValid && this.GasCanMoveTo(cell))
			{
				if (this.AnyGasAt(cell))
				{
					A_2.totalRotStink += (int)this.DensityAt(cell, GasType.RotStink);
					if (ModsConfig.BiotechActive)
					{
						A_2.totalToxGas += (int)this.DensityAt(cell, GasType.ToxGas);
					}
					if (ModsConfig.AnomalyActive)
					{
						A_2.totalDeadlifeDust += (int)this.DensityAt(cell, GasType.DeadlifeDust);
					}
				}
				GasGrid.beqCells[A_2.beqCellCount] = cell;
				int beqCellCount = A_2.beqCellCount;
				A_2.beqCellCount = beqCellCount + 1;
			}
		}

		// Token: 0x040017BC RID: 6076
		private uint[] gasDensity;

		// Token: 0x040017BD RID: 6077
		private Map map;

		// Token: 0x040017BE RID: 6078
		private int cycleIndexDiffusion;

		// Token: 0x040017BF RID: 6079
		private int cycleIndexDissipation;

		// Token: 0x040017C0 RID: 6080
		[Unsaved(false)]
		private List<IntVec3> cardinalDirections;

		// Token: 0x040017C1 RID: 6081
		[Unsaved(false)]
		private List<IntVec3> cellsInRandomOrder;

		// Token: 0x040017C2 RID: 6082
		[Unsaved(false)]
		private bool anyGasEverAdded;

		// Token: 0x040017C3 RID: 6083
		public const int MaxGasPerCell = 255;

		// Token: 0x040017C4 RID: 6084
		private const int EstimatedMaxGasPerCell = 48;

		// Token: 0x040017C5 RID: 6085
		private const float CellsToDissipatePerTickFactor = 0.015625f;

		// Token: 0x040017C6 RID: 6086
		private const float CellsToDiffusePerTickFactor = 0.03125f;

		// Token: 0x040017C7 RID: 6087
		private const float VacuumDissipatePerTickFactor = 25f;

		// Token: 0x040017C8 RID: 6088
		private const float MaxOverflowFloodfillRadius = 40f;

		// Token: 0x040017C9 RID: 6089
		private const int DissipationAmount_BlindSmoke = 4;

		// Token: 0x040017CA RID: 6090
		private const int DissipationAmount_ToxGas = 3;

		// Token: 0x040017CB RID: 6091
		private const int DissipationAmount_RotStink = 4;

		// Token: 0x040017CC RID: 6092
		private const int DissipationAmount_DeadlifeDust = 3;

		// Token: 0x040017CD RID: 6093
		private const int MinDiffusion = 17;

		// Token: 0x040017CE RID: 6094
		private const int AnyGasCheckIntervalTicks = 600;

		// Token: 0x040017CF RID: 6095
		private static readonly IntVec3[] beqCells = new IntVec3[4];
	}
}
