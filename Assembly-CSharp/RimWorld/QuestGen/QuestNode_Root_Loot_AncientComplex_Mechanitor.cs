using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B5A RID: 15194
	public class QuestNode_Root_Loot_AncientComplex_Mechanitor : QuestNode_Root_Loot_AncientComplex
	{
		// Token: 0x17003365 RID: 13157
		// (get) Token: 0x06014A60 RID: 84576 RVA: 0x005FCD37 File Offset: 0x005FAF37
		protected override LayoutDef LayoutDef
		{
			get
			{
				return LayoutDefOf.AncientComplex_Mechanitor_Loot;
			}
		}

		// Token: 0x17003366 RID: 13158
		// (get) Token: 0x06014A61 RID: 84577 RVA: 0x0062BC8A File Offset: 0x00629E8A
		protected override SitePartDef SitePartDef
		{
			get
			{
				return SitePartDefOf.AncientComplex_Mechanitor;
			}
		}

		// Token: 0x06014A62 RID: 84578 RVA: 0x0062BC91 File Offset: 0x00629E91
		protected override bool BeforeRunInt()
		{
			return ModLister.CheckBiotech("Ancient mechanitor complex");
		}

		// Token: 0x06014A63 RID: 84579 RVA: 0x0062BCA4 File Offset: 0x00629EA4
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			bool flag;
			if (!slate.TryGet<bool>("discovered", out flag, false))
			{
				slate.Set<bool>("discovered", false, false);
			}
			base.RunInt();
		}
	}
}
