using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003393 RID: 13203
	public class ITab_ContentsCasket : ITab_ContentsBase
	{
		// Token: 0x17003083 RID: 12419
		// (get) Token: 0x060128EF RID: 76015 RVA: 0x0057A488 File Offset: 0x00578688
		public override IList<Thing> container
		{
			get
			{
				Building_Casket building_Casket = base.SelThing as Building_Casket;
				this.listInt.Clear();
				if (building_Casket != null && building_Casket.ContainedThing != null)
				{
					this.listInt.Add(building_Casket.ContainedThing);
				}
				return this.listInt;
			}
		}

		// Token: 0x060128F0 RID: 76016 RVA: 0x0057A4CE File Offset: 0x005786CE
		public ITab_ContentsCasket()
		{
			this.labelKey = "TabCasketContents";
			this.containedItemsKey = "ContainedItems";
			this.canRemoveThings = false;
		}

		// Token: 0x0400C0CF RID: 49359
		private List<Thing> listInt = new List<Thing>();
	}
}
