using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020006F8 RID: 1784
	public class HediffComp_RemoveIfApparelDropped : HediffComp
	{
		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06002BD3 RID: 11219 RVA: 0x000E4506 File Offset: 0x000E2706
		public HediffCompProperties_RemoveIfApparelDropped Props
		{
			get
			{
				return (HediffCompProperties_RemoveIfApparelDropped)this.props;
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x000E4513 File Offset: 0x000E2713
		public override bool CompShouldRemove
		{
			get
			{
				return !this.parent.pawn.apparel.Wearing(this.wornApparel);
			}
		}

		// Token: 0x06002BD5 RID: 11221 RVA: 0x000E4533 File Offset: 0x000E2733
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_References.Look<Apparel>(ref this.wornApparel, "wornApparel", false);
		}

		// Token: 0x0400233E RID: 9022
		public Apparel wornApparel;
	}
}
