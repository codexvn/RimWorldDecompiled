using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003794 RID: 14228
	public class InfectionPathwayDef : Def
	{
		// Token: 0x1700325C RID: 12892
		// (get) Token: 0x06013B45 RID: 80709 RVA: 0x005DAD82 File Offset: 0x005D8F82
		public float ExpiryDays
		{
			get
			{
				return this.expiryDays;
			}
		}

		// Token: 0x1700325D RID: 12893
		// (get) Token: 0x06013B46 RID: 80710 RVA: 0x005DAD8A File Offset: 0x005D8F8A
		public int ExpiryTicks
		{
			get
			{
				return (int)this.expiryDays * 60000;
			}
		}

		// Token: 0x1700325E RID: 12894
		// (get) Token: 0x06013B47 RID: 80711 RVA: 0x005DAD99 File Offset: 0x005D8F99
		public bool PawnRequired
		{
			get
			{
				return this.pawnRequired;
			}
		}

		// Token: 0x0400DA41 RID: 55873
		private float expiryDays = 30f;

		// Token: 0x0400DA42 RID: 55874
		private bool pawnRequired;
	}
}
