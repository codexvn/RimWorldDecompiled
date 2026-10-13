using System;

namespace Verse
{
	// Token: 0x02000708 RID: 1800
	public class Hediff_HemogenCraving : HediffWithComps
	{
		// Token: 0x170007C5 RID: 1989
		// (get) Token: 0x06002C08 RID: 11272 RVA: 0x000E4FF4 File Offset: 0x000E31F4
		public override string SeverityLabel
		{
			get
			{
				if (this.Severity == 0f)
				{
					return null;
				}
				return this.Severity.ToStringPercent();
			}
		}
	}
}
