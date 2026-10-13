using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020022E9 RID: 8937
	public class Hediff_High : HediffWithComps
	{
		// Token: 0x17001DC0 RID: 7616
		// (get) Token: 0x0600C24C RID: 49740 RVA: 0x000EF0B7 File Offset: 0x000ED2B7
		public override string SeverityLabel
		{
			get
			{
				if (this.Severity <= 0f)
				{
					return null;
				}
				return this.Severity.ToStringPercent("F0");
			}
		}
	}
}
