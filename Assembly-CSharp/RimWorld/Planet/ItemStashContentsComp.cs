using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E2F RID: 15919
	public class ItemStashContentsComp : WorldObjectComp, IThingHolder
	{
		// Token: 0x06015BF5 RID: 89077 RVA: 0x00680877 File Offset: 0x0067EA77
		public ItemStashContentsComp()
		{
			this.contents = new ThingOwner<Thing>(this);
		}

		// Token: 0x06015BF6 RID: 89078 RVA: 0x0068088B File Offset: 0x0067EA8B
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.contents, "contents", new object[]
			{
				this
			});
		}

		// Token: 0x06015BF7 RID: 89079 RVA: 0x006808AD File Offset: 0x0067EAAD
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06015BF8 RID: 89080 RVA: 0x006808BB File Offset: 0x0067EABB
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.contents;
		}

		// Token: 0x06015BF9 RID: 89081 RVA: 0x006808C3 File Offset: 0x0067EAC3
		public override void PostDestroy()
		{
			base.PostDestroy();
			this.contents.ClearAndDestroyContents(DestroyMode.Vanish);
		}

		// Token: 0x0400F151 RID: 61777
		public ThingOwner contents;
	}
}
