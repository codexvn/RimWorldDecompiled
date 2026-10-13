using System;

namespace Verse
{
	// Token: 0x02000502 RID: 1282
	public abstract class RegionProcessorDelegateCache
	{
		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06002144 RID: 8516 RVA: 0x000AE044 File Offset: 0x000AC244
		public RegionProcessor RegionProcessorDelegate
		{
			get
			{
				return this.regionProcessor;
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06002145 RID: 8517 RVA: 0x000AE04C File Offset: 0x000AC24C
		public RegionEntryPredicate RegionEntryPredicateDelegate
		{
			get
			{
				return this.regionEntryPredicate;
			}
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x000AE054 File Offset: 0x000AC254
		public RegionProcessorDelegateCache()
		{
			this.regionProcessor = new RegionProcessor(this.RegionProcessor);
			this.regionEntryPredicate = new RegionEntryPredicate(this.RegionEntryPredicate);
		}

		// Token: 0x06002147 RID: 8519
		protected abstract bool RegionEntryPredicate(Region from, Region to);

		// Token: 0x06002148 RID: 8520
		protected abstract bool RegionProcessor(Region reg);

		// Token: 0x04001C30 RID: 7216
		private RegionProcessor regionProcessor;

		// Token: 0x04001C31 RID: 7217
		private RegionEntryPredicate regionEntryPredicate;
	}
}
