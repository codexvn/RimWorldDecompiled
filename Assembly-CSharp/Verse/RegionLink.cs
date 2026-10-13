using System;
using System.Linq;

namespace Verse
{
	// Token: 0x020004FB RID: 1275
	public class RegionLink
	{
		// Token: 0x170005E7 RID: 1511
		// (get) Token: 0x0600211B RID: 8475 RVA: 0x000AD463 File Offset: 0x000AB663
		// (set) Token: 0x0600211C RID: 8476 RVA: 0x000AD46D File Offset: 0x000AB66D
		public Region RegionA
		{
			get
			{
				return this.regions[0];
			}
			set
			{
				this.regions[0] = value;
			}
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x0600211D RID: 8477 RVA: 0x000AD478 File Offset: 0x000AB678
		// (set) Token: 0x0600211E RID: 8478 RVA: 0x000AD482 File Offset: 0x000AB682
		public Region RegionB
		{
			get
			{
				return this.regions[1];
			}
			set
			{
				this.regions[1] = value;
			}
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x000AD490 File Offset: 0x000AB690
		public void Register(Region reg)
		{
			if (this.regions[0] == reg || this.regions[1] == reg)
			{
				Log.Error(string.Format("Tried to double-register region {0} in {1}", reg, this));
				return;
			}
			if (this.RegionA == null || !this.RegionA.valid)
			{
				this.RegionA = reg;
				return;
			}
			if (this.RegionB == null || !this.RegionB.valid)
			{
				this.RegionB = reg;
				return;
			}
			Log.Error(string.Format("Could not register region {0} in link {1}: > 2 regions on link!\nRegionA: {2}\nRegionB: {3}", new object[]
			{
				reg,
				this,
				this.RegionA.DebugString,
				this.RegionB.DebugString
			}));
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x000AD538 File Offset: 0x000AB738
		public void Deregister(Region reg)
		{
			if (this.RegionA == reg)
			{
				this.RegionA = null;
				if (this.RegionB == null)
				{
					Map map = reg.Map;
					if (map == null)
					{
						return;
					}
					map.regionLinkDatabase.Notify_LinkHasNoRegions(this);
					return;
				}
			}
			else if (this.RegionB == reg)
			{
				this.RegionB = null;
				if (this.RegionA == null)
				{
					Map map2 = reg.Map;
					if (map2 == null)
					{
						return;
					}
					map2.regionLinkDatabase.Notify_LinkHasNoRegions(this);
				}
			}
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x000AD5A2 File Offset: 0x000AB7A2
		public Region GetOtherRegion(Region reg)
		{
			if (reg != this.RegionA)
			{
				return this.RegionA;
			}
			return this.RegionB;
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x000AD5BA File Offset: 0x000AB7BA
		public ulong UniqueHashCode()
		{
			return this.span.UniqueHashCode();
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x000AD5C8 File Offset: 0x000AB7C8
		public override string ToString()
		{
			string text = (from r in this.regions
			where r != null
			select r.id.ToString()).ToCommaList(false, false);
			string text2 = "span=" + this.span.ToString() + " hash=" + this.UniqueHashCode().ToString();
			return string.Concat(new string[]
			{
				"(",
				text2,
				", regions=",
				text,
				")"
			});
		}

		// Token: 0x04001C22 RID: 7202
		public Region[] regions = new Region[2];

		// Token: 0x04001C23 RID: 7203
		public EdgeSpan span;
	}
}
