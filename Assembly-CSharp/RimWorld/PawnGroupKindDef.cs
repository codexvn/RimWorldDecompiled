using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AA8 RID: 6824
	public class PawnGroupKindDef : Def
	{
		// Token: 0x17001770 RID: 6000
		// (get) Token: 0x060098D9 RID: 39129 RVA: 0x002C9024 File Offset: 0x002C7224
		public PawnGroupKindWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnGroupKindWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x040067B2 RID: 26546
		public Type workerClass = typeof(PawnGroupKindWorker);

		// Token: 0x040067B3 RID: 26547
		[Unsaved(false)]
		private PawnGroupKindWorker workerInt;
	}
}
