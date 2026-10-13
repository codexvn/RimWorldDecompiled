using System;

namespace RimWorld
{
	// Token: 0x02002BD1 RID: 11217
	public class CompAncientSmokeVent : CompAncientVent
	{
		// Token: 0x0600FAEC RID: 64236 RVA: 0x0048E878 File Offset: 0x0048CA78
		protected override void ToggleIndividualVent(bool on)
		{
			this.parent.GetComp<CompFleckEmitterLongTerm>().Enabled = on;
		}
	}
}
