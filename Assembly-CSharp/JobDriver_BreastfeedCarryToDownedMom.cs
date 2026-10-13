using System;

// Token: 0x0200000A RID: 10
public class JobDriver_BreastfeedCarryToDownedMom : JobDriver_BreastfeedCarryToMom
{
	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000031 RID: 49 RVA: 0x000028E7 File Offset: 0x00000AE7
	protected override bool MomMustBeImmobile
	{
		get
		{
			return true;
		}
	}
}
