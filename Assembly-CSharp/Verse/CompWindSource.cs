using System;

namespace Verse
{
	// Token: 0x02000904 RID: 2308
	public class CompWindSource : ThingComp
	{
		// Token: 0x06003B94 RID: 15252 RVA: 0x0013A71C File Offset: 0x0013891C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.wind, "wind", 0f, false);
		}

		// Token: 0x04002AF9 RID: 11001
		public float wind;
	}
}
