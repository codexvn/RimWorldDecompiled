using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AC9 RID: 6857
	public class RaidAgeRestrictionDef : Def
	{
		// Token: 0x17001790 RID: 6032
		// (get) Token: 0x06009988 RID: 39304 RVA: 0x002CB733 File Offset: 0x002C9933
		public RaidAgeRestrictionWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RaidAgeRestrictionWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x04006902 RID: 26882
		public Type workerClass = typeof(RaidAgeRestrictionWorker);

		// Token: 0x04006903 RID: 26883
		public DevelopmentalStage developmentStage;

		// Token: 0x04006904 RID: 26884
		public FloatRange? ageRange;

		// Token: 0x04006905 RID: 26885
		public float threatPointsFactor = 1f;

		// Token: 0x04006906 RID: 26886
		public int earliestDay;

		// Token: 0x04006907 RID: 26887
		public float chance;

		// Token: 0x04006908 RID: 26888
		[MustTranslate]
		public string arrivalTextExtra;

		// Token: 0x04006909 RID: 26889
		private RaidAgeRestrictionWorker workerInt;
	}
}
