using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019EA RID: 6634
	public class BabyPlayDef : Def
	{
		// Token: 0x170016D7 RID: 5847
		// (get) Token: 0x060095D0 RID: 38352 RVA: 0x002BDE8B File Offset: 0x002BC08B
		public BabyPlayGiver Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (BabyPlayGiver)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x040061D8 RID: 25048
		public Type workerClass;

		// Token: 0x040061D9 RID: 25049
		public JobDef jobDef;

		// Token: 0x040061DA RID: 25050
		private BabyPlayGiver workerInt;
	}
}
