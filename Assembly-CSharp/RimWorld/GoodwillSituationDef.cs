using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A42 RID: 6722
	public class GoodwillSituationDef : Def
	{
		// Token: 0x17001721 RID: 5921
		// (get) Token: 0x0600975C RID: 38748 RVA: 0x002C3581 File Offset: 0x002C1781
		public GoodwillSituationWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (GoodwillSituationWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x0400645D RID: 25693
		public Type workerClass = typeof(GoodwillSituationWorker);

		// Token: 0x0400645E RID: 25694
		public int baseMaxGoodwill = 100;

		// Token: 0x0400645F RID: 25695
		public MemeDef meme;

		// Token: 0x04006460 RID: 25696
		public MemeDef otherMeme;

		// Token: 0x04006461 RID: 25697
		public int naturalGoodwillOffset;

		// Token: 0x04006462 RID: 25698
		public bool versusAll;

		// Token: 0x04006463 RID: 25699
		[Unsaved(false)]
		private GoodwillSituationWorker workerInt;
	}
}
