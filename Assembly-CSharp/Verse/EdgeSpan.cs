using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020004F9 RID: 1273
	public struct EdgeSpan
	{
		// Token: 0x170005E3 RID: 1507
		// (get) Token: 0x0600210E RID: 8462 RVA: 0x000AD1F3 File Offset: 0x000AB3F3
		public bool IsValid
		{
			get
			{
				return this.length > 0;
			}
		}

		// Token: 0x170005E4 RID: 1508
		// (get) Token: 0x0600210F RID: 8463 RVA: 0x000AD1FE File Offset: 0x000AB3FE
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				int num;
				for (int i = 0; i < this.length; i = num + 1)
				{
					if (this.dir == SpanDirection.North)
					{
						yield return new IntVec3(this.root.x, 0, this.root.z + i);
					}
					else if (this.dir == SpanDirection.East)
					{
						yield return new IntVec3(this.root.x + i, 0, this.root.z);
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x000AD214 File Offset: 0x000AB414
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "(root=";
			int num = 1;
			IntVec3 intVec = this.root;
			array[num] = intVec.ToString();
			array[2] = ", dir=";
			array[3] = this.dir.ToString();
			array[4] = " + length=";
			array[5] = this.length.ToString();
			array[6] = ")";
			return string.Concat(array);
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x000AD285 File Offset: 0x000AB485
		public EdgeSpan(IntVec3 root, SpanDirection dir, int length)
		{
			this.root = root;
			this.dir = dir;
			this.length = length;
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x000AD29C File Offset: 0x000AB49C
		public ulong UniqueHashCode()
		{
			ulong num = this.root.UniqueHashCode();
			if (this.dir == SpanDirection.East)
			{
				num += 17592186044416UL;
			}
			return num + (ulong)(281474976710656L * (long)this.length);
		}

		// Token: 0x04001C19 RID: 7193
		public IntVec3 root;

		// Token: 0x04001C1A RID: 7194
		public SpanDirection dir;

		// Token: 0x04001C1B RID: 7195
		public int length;
	}
}
