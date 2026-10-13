using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F8E RID: 8078
	public class Chunks
	{
		// Token: 0x17001B20 RID: 6944
		public CellRect this[IntVec3 cell]
		{
			get
			{
				return this.CellToRect(cell);
			}
		}

		// Token: 0x17001B21 RID: 6945
		public CellRect this[int index]
		{
			get
			{
				return this.data[index];
			}
		}

		// Token: 0x17001B22 RID: 6946
		// (get) Token: 0x0600B26E RID: 45678 RVA: 0x00331892 File Offset: 0x0032FA92
		public int UsedCount
		{
			get
			{
				return this.usedCount;
			}
		}

		// Token: 0x17001B23 RID: 6947
		// (get) Token: 0x0600B26F RID: 45679 RVA: 0x0033189A File Offset: 0x0032FA9A
		public int ApproximateArea
		{
			get
			{
				return this.usedCount * this.regionSize * this.regionSize;
			}
		}

		// Token: 0x17001B24 RID: 6948
		// (get) Token: 0x0600B270 RID: 45680 RVA: 0x003318B0 File Offset: 0x0032FAB0
		public IReadOnlyList<CellRect> EnumeratedRects
		{
			get
			{
				if (this.enumerated.Empty<CellRect>())
				{
					this.RecacheEnumeratedChunks();
				}
				return this.enumerated;
			}
		}

		// Token: 0x0600B271 RID: 45681 RVA: 0x003318CC File Offset: 0x0032FACC
		public Chunks(CellRect rect, int regionSize)
		{
			int subdivisionCount = MapGenUtility.GetSubdivisionCount(rect, ref regionSize);
			this.regionSize = regionSize;
			this.chunksX = 1;
			for (int i = 0; i < subdivisionCount; i++)
			{
				this.chunksX *= 2;
			}
			this.data = new CellRect[this.chunksX * this.chunksX];
			this.used.EnsureCapacity(this.data.Length);
			this.rect = new CellRect(0, 0, this.chunksX, this.chunksX);
			for (int j = 0; j < this.data.Length; j++)
			{
				IntVec3 a = this.IndexToCell(j);
				IntVec3 first = rect.Min + a * regionSize;
				IntVec3 second = rect.Min + a * regionSize + new IntVec3(regionSize, 0, regionSize);
				this.data[j] = CellRect.FromLimits(first, second);
				this.boundary = ((this.boundary == default(CellRect)) ? this.data[j] : this.boundary.Encapsulate(this.data[j]));
			}
		}

		// Token: 0x0600B272 RID: 45682 RVA: 0x00331A1A File Offset: 0x0032FC1A
		public bool Contains(int ind)
		{
			return ind >= 0 && ind < this.chunksX * this.chunksX;
		}

		// Token: 0x0600B273 RID: 45683 RVA: 0x00331A32 File Offset: 0x0032FC32
		public bool Contains(IntVec3 cell)
		{
			return this.rect.Contains(cell);
		}

		// Token: 0x0600B274 RID: 45684 RVA: 0x00331A40 File Offset: 0x0032FC40
		public bool Used(int ind)
		{
			return this.Contains(ind) && this.Used(this.IndexToCell(ind));
		}

		// Token: 0x0600B275 RID: 45685 RVA: 0x00331A5A File Offset: 0x0032FC5A
		public bool Used(IntVec3 cell)
		{
			return this.Contains(cell) && this.used.Contains(cell);
		}

		// Token: 0x17001B25 RID: 6949
		// (get) Token: 0x0600B276 RID: 45686 RVA: 0x00331A74 File Offset: 0x0032FC74
		public IntVec3 RandomChunk
		{
			get
			{
				return this.rect.RandomCell;
			}
		}

		// Token: 0x17001B26 RID: 6950
		// (get) Token: 0x0600B277 RID: 45687 RVA: 0x00331A8F File Offset: 0x0032FC8F
		public IntVec3 RandomUsedChunk
		{
			get
			{
				return this.used.RandomElement<IntVec3>();
			}
		}

		// Token: 0x0600B278 RID: 45688 RVA: 0x00331A9C File Offset: 0x0032FC9C
		public void SetUsed(int index, bool set)
		{
			if (!this.Contains(index))
			{
				return;
			}
			this.SetUsed(this.IndexToCell(index), set);
		}

		// Token: 0x0600B279 RID: 45689 RVA: 0x00331AB8 File Offset: 0x0032FCB8
		public void SetUsed(IntVec3 cell, bool set)
		{
			if (!this.Contains(cell))
			{
				return;
			}
			if (set && !this.used.Contains(cell))
			{
				this.usedCount++;
				this.used.Add(cell);
				this.enumerated.Clear();
				return;
			}
			if (!set && this.used.Contains(cell))
			{
				this.usedCount--;
				this.used.Remove(cell);
				this.enumerated.Clear();
			}
		}

		// Token: 0x0600B27A RID: 45690 RVA: 0x00331B3E File Offset: 0x0032FD3E
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public CellRect CellToRect(IntVec3 c)
		{
			return this.data[this.CellToIndex(c)];
		}

		// Token: 0x0600B27B RID: 45691 RVA: 0x00331B52 File Offset: 0x0032FD52
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IntVec3 IndexToCell(int ind)
		{
			return CellIndicesUtility.IndexToCell(ind, this.chunksX);
		}

		// Token: 0x0600B27C RID: 45692 RVA: 0x00331B60 File Offset: 0x0032FD60
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int CellToIndex(IntVec3 c)
		{
			return CellIndicesUtility.CellToIndex(c, this.chunksX);
		}

		// Token: 0x0600B27D RID: 45693 RVA: 0x00331B70 File Offset: 0x0032FD70
		public bool TryGetFreeEdge([TupleElementNames(new string[]
		{
			"chunk",
			"rot"
		})] out ValueTuple<IntVec3, Rot4> edge, bool inBounds = false, int spaces = 1, Predicate<IntVec3> validator = null)
		{
			List<ValueTuple<IntVec3, Rot4>> list = new List<ValueTuple<IntVec3, Rot4>>();
			foreach (IntVec3 intVec in this.used.InRandomOrder(null))
			{
				Rot4 item;
				if (this.TryGetFreeEdge(intVec, out item, inBounds, spaces, validator))
				{
					list.Add(new ValueTuple<IntVec3, Rot4>(intVec, item));
				}
			}
			if (list.Empty<ValueTuple<IntVec3, Rot4>>())
			{
				edge = default(ValueTuple<IntVec3, Rot4>);
				return false;
			}
			edge = list.RandomElement<ValueTuple<IntVec3, Rot4>>();
			return true;
		}

		// Token: 0x0600B27E RID: 45694 RVA: 0x00331BFC File Offset: 0x0032FDFC
		public bool TryGetFreeEdge(IntVec3 chunk, out Rot4 rot, bool inBounds = false, int spaces = 1, Predicate<IntVec3> validator = null)
		{
			List<Rot4> list = new List<Rot4>();
			for (int i = 0; i < 4; i++)
			{
				rot = new Rot4(i);
				bool flag = true;
				for (int j = 0; j < spaces; j++)
				{
					IntVec3 intVec = chunk + rot.FacingCell * (j + 1);
					if (this.Used(intVec) || (inBounds && !this.rect.Contains(intVec)) || (validator != null && !validator(intVec)))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					list.Add(rot);
				}
			}
			if (list.Empty<Rot4>())
			{
				rot = default(Rot4);
				return false;
			}
			rot = list.RandomElement<Rot4>();
			return true;
		}

		// Token: 0x0600B27F RID: 45695 RVA: 0x00331CA8 File Offset: 0x0032FEA8
		private void RecacheEnumeratedChunks()
		{
			this.enumerated.Clear();
			foreach (CellRect cellRect in this.rect.EnumerateRectanglesCovering(new Predicate<IntVec3>(this.<RecacheEnumeratedChunks>g__Validator|34_0)))
			{
				CellRect cellRect2 = CellRect.Empty;
				foreach (IntVec3 cell in cellRect.Cells)
				{
					CellRect cellRect3 = this[cell];
					cellRect2 = ((cellRect2 == CellRect.Empty) ? cellRect3 : cellRect2.Encapsulate(cellRect3));
				}
				this.enumerated.Add(cellRect2);
			}
			this.enumerated.SortByDescending((CellRect r) => r.Area);
		}

		// Token: 0x0600B280 RID: 45696 RVA: 0x00331DA4 File Offset: 0x0032FFA4
		[CompilerGenerated]
		private bool <RecacheEnumeratedChunks>g__Validator|34_0(IntVec3 index)
		{
			return this.Used(index);
		}

		// Token: 0x04007A3F RID: 31295
		private readonly int chunksX;

		// Token: 0x04007A40 RID: 31296
		public readonly int regionSize;

		// Token: 0x04007A41 RID: 31297
		public readonly CellRect rect;

		// Token: 0x04007A42 RID: 31298
		public readonly CellRect boundary;

		// Token: 0x04007A43 RID: 31299
		private readonly CellRect[] data;

		// Token: 0x04007A44 RID: 31300
		private readonly HashSet<IntVec3> used = new HashSet<IntVec3>();

		// Token: 0x04007A45 RID: 31301
		private readonly List<CellRect> enumerated = new List<CellRect>();

		// Token: 0x04007A46 RID: 31302
		private int usedCount;
	}
}
