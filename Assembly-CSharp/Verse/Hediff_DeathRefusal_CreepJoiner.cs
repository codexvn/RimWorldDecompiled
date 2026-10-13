using System;

namespace Verse
{
	// Token: 0x0200073D RID: 1853
	public class Hediff_DeathRefusal_CreepJoiner : Hediff_DeathRefusal
	{
		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06002D33 RID: 11571 RVA: 0x000E9647 File Offset: 0x000E7847
		public override int MaxUses
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x000E964A File Offset: 0x000E784A
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.usesLeft = this.MaxUses;
		}
	}
}
