using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000191 RID: 401
	public class DeathActionProperties_Divide : DeathActionProperties
	{
		// Token: 0x06000B5A RID: 2906 RVA: 0x00035D72 File Offset: 0x00033F72
		public DeathActionProperties_Divide()
		{
			this.workerClass = typeof(DeathActionWorker_Divide);
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00035DA0 File Offset: 0x00033FA0
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.dividePawnCount <= 0 && this.dividePawnKindAdditionalForced.NullOrEmpty<PawnKindDef>())
			{
				yield return "deathActionWorkerClass is DeathActionWorker_Divide or subclass, but dividePawnCount <= 0.";
			}
			if (this.dividePawnKindOptions.NullOrEmpty<PawnKindDef>() && this.dividePawnKindAdditionalForced.NullOrEmpty<PawnKindDef>())
			{
				yield return "deathActionWorkerClass is DeathActionWorker_Divide or subclass, but dividePawnKindOptions is null or empty.";
			}
			yield break;
		}

		// Token: 0x04000680 RID: 1664
		public List<PawnKindDef> dividePawnKindOptions = new List<PawnKindDef>();

		// Token: 0x04000681 RID: 1665
		public int dividePawnCount;

		// Token: 0x04000682 RID: 1666
		public List<PawnKindDef> dividePawnKindAdditionalForced = new List<PawnKindDef>();

		// Token: 0x04000683 RID: 1667
		public IntRange divideBloodFilthCountRange;
	}
}
