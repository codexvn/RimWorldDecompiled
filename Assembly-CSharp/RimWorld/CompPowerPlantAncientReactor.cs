using System;

namespace RimWorld
{
	// Token: 0x02002137 RID: 8503
	public class CompPowerPlantAncientReactor : CompPowerPlant
	{
		// Token: 0x17001C34 RID: 7220
		// (get) Token: 0x0600B999 RID: 47513 RVA: 0x00358DAA File Offset: 0x00356FAA
		protected override float DesiredPowerOutput
		{
			get
			{
				if (!this.HackableComp.IsHacked)
				{
					return base.DesiredPowerOutput;
				}
				return 0f;
			}
		}

		// Token: 0x17001C35 RID: 7221
		// (get) Token: 0x0600B99A RID: 47514 RVA: 0x00358DC8 File Offset: 0x00356FC8
		public CompHackable HackableComp
		{
			get
			{
				CompHackable result;
				if ((result = this.hackableComp) == null)
				{
					result = (this.hackableComp = this.parent.GetComp<CompHackable>());
				}
				return result;
			}
		}

		// Token: 0x04007FF1 RID: 32753
		private CompHackable hackableComp;
	}
}
