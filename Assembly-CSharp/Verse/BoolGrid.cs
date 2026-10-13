using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000420 RID: 1056
	public class BoolGrid : IExposable
	{
		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x06001AAE RID: 6830 RVA: 0x00087591 File Offset: 0x00085791
		public int TrueCount
		{
			get
			{
				return this.trueCountInt;
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x06001AAF RID: 6831 RVA: 0x00087599 File Offset: 0x00085799
		public int Width
		{
			get
			{
				return this.width;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001AB0 RID: 6832 RVA: 0x000875A1 File Offset: 0x000857A1
		public int Height
		{
			get
			{
				return this.height;
			}
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001AB1 RID: 6833 RVA: 0x000875A9 File Offset: 0x000857A9
		public IEnumerable<IntVec3> ActiveCells
		{
			get
			{
				if (this.trueCountInt == 0)
				{
					yield break;
				}
				int yieldedCount = 0;
				bool canSetMinPossibleTrueIndex = this.minPossibleTrueIndexDirty;
				int num = this.minPossibleTrueIndexDirty ? 0 : this.minPossibleTrueIndexCached;
				int num2;
				for (int i = num; i < this.arr.Length; i = num2 + 1)
				{
					if (this.arr[i])
					{
						if (canSetMinPossibleTrueIndex && this.minPossibleTrueIndexDirty)
						{
							canSetMinPossibleTrueIndex = false;
							this.minPossibleTrueIndexDirty = false;
							this.minPossibleTrueIndexCached = i;
						}
						yield return CellIndicesUtility.IndexToCell(i, this.width);
						num2 = yieldedCount;
						yieldedCount = num2 + 1;
						if (yieldedCount >= this.trueCountInt)
						{
							yield break;
						}
					}
					num2 = i;
				}
				yield break;
			}
		}

		// Token: 0x170004C8 RID: 1224
		public bool this[int index]
		{
			get
			{
				return this.arr[index];
			}
			set
			{
				this.Set(index, value);
			}
		}

		// Token: 0x170004C9 RID: 1225
		public bool this[IntVec3 c]
		{
			get
			{
				return this.arr[CellIndicesUtility.CellToIndex(c, this.width)];
			}
			set
			{
				this.Set(c, value);
			}
		}

		// Token: 0x170004CA RID: 1226
		public bool this[int x, int z]
		{
			get
			{
				return this.arr[CellIndicesUtility.CellToIndex(x, z, this.width)];
			}
			set
			{
				this.Set(CellIndicesUtility.CellToIndex(x, z, this.width), value);
			}
		}

		// Token: 0x06001AB8 RID: 6840 RVA: 0x00087618 File Offset: 0x00085818
		public BoolGrid()
		{
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x00087627 File Offset: 0x00085827
		public BoolGrid(Map map)
		{
			this.ClearAndResizeTo(map);
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x0008763D File Offset: 0x0008583D
		public BoolGrid(int x, int z)
		{
			this.ClearAndResizeTo(x, z);
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x00087654 File Offset: 0x00085854
		public bool MapSizeMatches(Map map)
		{
			return this.width == map.Size.x && this.height == map.Size.z;
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x0008767E File Offset: 0x0008587E
		public void ClearAndResizeTo(Map map)
		{
			if (this.MapSizeMatches(map))
			{
				if (this.arr != null)
				{
					this.Clear();
				}
				return;
			}
			this.ClearAndResizeTo(map.Size.x, map.Size.z);
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x000876B4 File Offset: 0x000858B4
		public void ClearAndResizeTo(int x, int z)
		{
			if (this.width == x && this.height == z)
			{
				if (this.arr != null)
				{
					this.Clear();
				}
				return;
			}
			this.width = x;
			this.height = z;
			this.arr = new bool[this.width * this.height];
			this.trueCountInt = 0;
			this.minPossibleTrueIndexCached = -1;
			this.minPossibleTrueIndexDirty = false;
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x00087720 File Offset: 0x00085920
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.trueCountInt, "trueCount", 0, false);
			Scribe_Values.Look<int>(ref this.width, "mapSizeX", 0, false);
			Scribe_Values.Look<int>(ref this.height, "mapSizeZ", 0, false);
			DataExposeUtility.LookBoolArray(ref this.arr, this.width * this.height, "arr");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.minPossibleTrueIndexDirty = true;
			}
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x0008778F File Offset: 0x0008598F
		public void Clear()
		{
			Array.Clear(this.arr, 0, this.arr.Length);
			this.trueCountInt = 0;
			this.minPossibleTrueIndexCached = -1;
			this.minPossibleTrueIndexDirty = false;
		}

		// Token: 0x06001AC0 RID: 6848 RVA: 0x000877BA File Offset: 0x000859BA
		public virtual void Set(IntVec3 c, bool value)
		{
			this.Set(CellIndicesUtility.CellToIndex(c, this.width), value);
		}

		// Token: 0x06001AC1 RID: 6849 RVA: 0x000877D0 File Offset: 0x000859D0
		public virtual void Set(int index, bool value)
		{
			if (this.arr[index] == value)
			{
				return;
			}
			this.arr[index] = value;
			if (value)
			{
				this.trueCountInt++;
				if (this.trueCountInt == 1 || index < this.minPossibleTrueIndexCached)
				{
					this.minPossibleTrueIndexCached = index;
					return;
				}
			}
			else
			{
				this.trueCountInt--;
				if (index == this.minPossibleTrueIndexCached)
				{
					this.minPossibleTrueIndexDirty = true;
				}
			}
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x0008783C File Offset: 0x00085A3C
		public void Invert()
		{
			for (int i = 0; i < this.arr.Length; i++)
			{
				this.arr[i] = !this.arr[i];
			}
			this.trueCountInt = this.arr.Length - this.trueCountInt;
			this.minPossibleTrueIndexDirty = true;
		}

		// Token: 0x0400178D RID: 6029
		private bool[] arr;

		// Token: 0x0400178E RID: 6030
		private int trueCountInt;

		// Token: 0x0400178F RID: 6031
		private int width;

		// Token: 0x04001790 RID: 6032
		private int height;

		// Token: 0x04001791 RID: 6033
		private int minPossibleTrueIndexCached = -1;

		// Token: 0x04001792 RID: 6034
		private bool minPossibleTrueIndexDirty;
	}
}
