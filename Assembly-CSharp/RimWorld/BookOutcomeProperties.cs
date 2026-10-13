using System;

namespace RimWorld
{
	// Token: 0x02001922 RID: 6434
	public abstract class BookOutcomeProperties : ReadingOutcomeProperties
	{
		// Token: 0x0600925B RID: 37467 RVA: 0x002B2177 File Offset: 0x002B0377
		public BookOutcomeProperties()
		{
		}

		// Token: 0x0600925C RID: 37468 RVA: 0x002B217F File Offset: 0x002B037F
		public BookOutcomeProperties(Type doerClass)
		{
			this.doerClass = doerClass;
		}
	}
}
