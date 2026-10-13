using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020034E5 RID: 13541
	public struct EventPack
	{
		// Token: 0x1700318F RID: 12687
		// (get) Token: 0x0601306E RID: 77934 RVA: 0x005A39CE File Offset: 0x005A1BCE
		public string Tag
		{
			get
			{
				return this.tagInt;
			}
		}

		// Token: 0x17003190 RID: 12688
		// (get) Token: 0x0601306F RID: 77935 RVA: 0x005A39D6 File Offset: 0x005A1BD6
		public IntVec3 Cell
		{
			get
			{
				return this.cellInt;
			}
		}

		// Token: 0x17003191 RID: 12689
		// (get) Token: 0x06013070 RID: 77936 RVA: 0x005A39DE File Offset: 0x005A1BDE
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				return this.cellsInt;
			}
		}

		// Token: 0x06013071 RID: 77937 RVA: 0x005A39E6 File Offset: 0x005A1BE6
		public EventPack(string tag)
		{
			this.tagInt = tag;
			this.cellInt = IntVec3.Invalid;
			this.cellsInt = null;
		}

		// Token: 0x06013072 RID: 77938 RVA: 0x005A3A01 File Offset: 0x005A1C01
		public EventPack(string tag, IntVec3 cell)
		{
			this.tagInt = tag;
			this.cellInt = cell;
			this.cellsInt = null;
		}

		// Token: 0x06013073 RID: 77939 RVA: 0x005A3A18 File Offset: 0x005A1C18
		public EventPack(string tag, IEnumerable<IntVec3> cells)
		{
			this.tagInt = tag;
			this.cellInt = IntVec3.Invalid;
			this.cellsInt = cells;
		}

		// Token: 0x06013074 RID: 77940 RVA: 0x005A3A33 File Offset: 0x005A1C33
		public static implicit operator EventPack(string s)
		{
			return new EventPack(s);
		}

		// Token: 0x06013075 RID: 77941 RVA: 0x005A3A3C File Offset: 0x005A1C3C
		public override string ToString()
		{
			if (this.Cell.IsValid)
			{
				return this.Tag + "-" + this.Cell.ToString();
			}
			return this.Tag;
		}

		// Token: 0x0400C526 RID: 50470
		private string tagInt;

		// Token: 0x0400C527 RID: 50471
		private IntVec3 cellInt;

		// Token: 0x0400C528 RID: 50472
		private IEnumerable<IntVec3> cellsInt;
	}
}
