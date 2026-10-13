using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001940 RID: 6464
	public abstract class ReadingOutcomeProperties
	{
		// Token: 0x17001648 RID: 5704
		// (get) Token: 0x060092FF RID: 37631
		public abstract Type DoerClass { get; }

		// Token: 0x06009300 RID: 37632 RVA: 0x002B3D8A File Offset: 0x002B1F8A
		public ReadingOutcomeProperties()
		{
		}

		// Token: 0x06009301 RID: 37633 RVA: 0x002B3DA2 File Offset: 0x002B1FA2
		public ReadingOutcomeProperties(Type doerClass)
		{
			this.doerClass = doerClass;
		}

		// Token: 0x04005DB5 RID: 23989
		[TranslationHandle]
		public Type doerClass = typeof(ReadingOutcomeDoer);
	}
}
