using System;

namespace RimWorld
{
	// Token: 0x02002583 RID: 9603
	public struct FactionGeneratorParms
	{
		// Token: 0x0600D247 RID: 53831 RVA: 0x003DF3B3 File Offset: 0x003DD5B3
		public FactionGeneratorParms(FactionDef factionDef, IdeoGenerationParms ideoGenerationParms = default(IdeoGenerationParms), bool? hidden = null)
		{
			this.factionDef = factionDef;
			this.ideoGenerationParms = ideoGenerationParms;
			this.hidden = hidden;
		}

		// Token: 0x04008EDA RID: 36570
		public FactionDef factionDef;

		// Token: 0x04008EDB RID: 36571
		public IdeoGenerationParms ideoGenerationParms;

		// Token: 0x04008EDC RID: 36572
		public bool? hidden;
	}
}
