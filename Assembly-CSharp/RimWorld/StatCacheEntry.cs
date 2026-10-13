using System;

namespace RimWorld
{
	// Token: 0x02003735 RID: 14133
	public struct StatCacheEntry
	{
		// Token: 0x06013972 RID: 80242 RVA: 0x005CE81A File Offset: 0x005CCA1A
		public StatCacheEntry(float statValue, int gameTick)
		{
			this.statValue = statValue;
			this.gameTick = gameTick;
		}

		// Token: 0x06013973 RID: 80243 RVA: 0x005CE82A File Offset: 0x005CCA2A
		public override string ToString()
		{
			return string.Format("StatCacheEntry({0}, {1})", this.statValue, this.gameTick);
		}

		// Token: 0x06013974 RID: 80244 RVA: 0x005CE84C File Offset: 0x005CCA4C
		public override int GetHashCode()
		{
			return this.statValue.GetHashCode() ^ this.gameTick.GetHashCode();
		}

		// Token: 0x06013975 RID: 80245 RVA: 0x005CE868 File Offset: 0x005CCA68
		public override bool Equals(object obj)
		{
			if (obj is StatCacheEntry)
			{
				StatCacheEntry statCacheEntry = (StatCacheEntry)obj;
				return this.Equals(statCacheEntry);
			}
			return false;
		}

		// Token: 0x0400D91D RID: 55581
		public float statValue;

		// Token: 0x0400D91E RID: 55582
		public int gameTick;
	}
}
