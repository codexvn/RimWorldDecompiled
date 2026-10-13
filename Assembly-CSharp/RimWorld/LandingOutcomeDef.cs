using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A62 RID: 6754
	public class LandingOutcomeDef : Def
	{
		// Token: 0x17001736 RID: 5942
		// (get) Token: 0x060097C2 RID: 38850 RVA: 0x002C4F40 File Offset: 0x002C3140
		public LandingOutcomeWorker Worker
		{
			get
			{
				LandingOutcomeWorker result;
				if ((result = this.worker) == null)
				{
					result = (this.worker = (LandingOutcomeWorker)Activator.CreateInstance(this.workerClass, new object[]
					{
						this
					}));
				}
				return result;
			}
		}

		// Token: 0x0400651B RID: 25883
		public float weight;

		// Token: 0x0400651C RID: 25884
		[MustTranslate]
		public string letterLabel;

		// Token: 0x0400651D RID: 25885
		[MustTranslate]
		public string letterText;

		// Token: 0x0400651E RID: 25886
		public LetterDef letterDef;

		// Token: 0x0400651F RID: 25887
		public Type workerClass;

		// Token: 0x04006520 RID: 25888
		private LandingOutcomeWorker worker;
	}
}
