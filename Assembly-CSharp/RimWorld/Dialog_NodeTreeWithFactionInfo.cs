using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020031A0 RID: 12704
	public class Dialog_NodeTreeWithFactionInfo : Dialog_NodeTree
	{
		// Token: 0x06011DFF RID: 73215 RVA: 0x005308ED File Offset: 0x0052EAED
		public Dialog_NodeTreeWithFactionInfo(DiaNode nodeRoot, Faction faction, bool delayInteractivity = false, bool radioMode = false, string title = null) : base(nodeRoot, delayInteractivity, radioMode, title)
		{
			this.faction = faction;
			if (faction != null)
			{
				this.minOptionsAreaHeight = 60f;
			}
		}

		// Token: 0x06011E00 RID: 73216 RVA: 0x00530910 File Offset: 0x0052EB10
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			if (this.faction != null && !this.faction.Hidden)
			{
				float num = inRect.height - 79f;
				FactionUIUtility.DrawRelatedFactionInfo(inRect, this.faction, ref num);
			}
		}

		// Token: 0x0400B8D8 RID: 47320
		private Faction faction;

		// Token: 0x0400B8D9 RID: 47321
		private const float RelatedFactionInfoSize = 79f;
	}
}
