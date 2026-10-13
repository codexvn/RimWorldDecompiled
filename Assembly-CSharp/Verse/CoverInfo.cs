using System;

namespace Verse
{
	// Token: 0x02000D58 RID: 3416
	public struct CoverInfo
	{
		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x060056F5 RID: 22261 RVA: 0x001BF566 File Offset: 0x001BD766
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x060056F6 RID: 22262 RVA: 0x001BF56E File Offset: 0x001BD76E
		public float BlockChance
		{
			get
			{
				return this.blockChanceInt;
			}
		}

		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x060056F7 RID: 22263 RVA: 0x001BF576 File Offset: 0x001BD776
		public static CoverInfo Invalid
		{
			get
			{
				return new CoverInfo(null, -999f);
			}
		}

		// Token: 0x060056F8 RID: 22264 RVA: 0x001BF583 File Offset: 0x001BD783
		public CoverInfo(Thing thing, float blockChance)
		{
			this.thingInt = thing;
			this.blockChanceInt = blockChance;
		}

		// Token: 0x04003F4E RID: 16206
		private Thing thingInt;

		// Token: 0x04003F4F RID: 16207
		private float blockChanceInt;
	}
}
