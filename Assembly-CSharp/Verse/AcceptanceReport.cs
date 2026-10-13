using System;

namespace Verse
{
	// Token: 0x02000C77 RID: 3191
	public struct AcceptanceReport
	{
		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x06005178 RID: 20856 RVA: 0x001A77E7 File Offset: 0x001A59E7
		public string Reason
		{
			get
			{
				return this.reasonTextInt;
			}
		}

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x06005179 RID: 20857 RVA: 0x001A77EF File Offset: 0x001A59EF
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x0600517A RID: 20858 RVA: 0x001A77F8 File Offset: 0x001A59F8
		public static AcceptanceReport WasAccepted
		{
			get
			{
				return new AcceptanceReport("")
				{
					acceptedInt = true
				};
			}
		}

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x0600517B RID: 20859 RVA: 0x001A781C File Offset: 0x001A5A1C
		public static AcceptanceReport WasRejected
		{
			get
			{
				return new AcceptanceReport("")
				{
					acceptedInt = false
				};
			}
		}

		// Token: 0x0600517C RID: 20860 RVA: 0x001A783E File Offset: 0x001A5A3E
		public AcceptanceReport(string reasonText)
		{
			this.acceptedInt = false;
			this.reasonTextInt = reasonText;
		}

		// Token: 0x0600517D RID: 20861 RVA: 0x001A784E File Offset: 0x001A5A4E
		public static implicit operator bool(AcceptanceReport report)
		{
			return report.Accepted;
		}

		// Token: 0x0600517E RID: 20862 RVA: 0x001A7857 File Offset: 0x001A5A57
		public static implicit operator AcceptanceReport(bool value)
		{
			if (value)
			{
				return AcceptanceReport.WasAccepted;
			}
			return AcceptanceReport.WasRejected;
		}

		// Token: 0x0600517F RID: 20863 RVA: 0x001A7867 File Offset: 0x001A5A67
		public static implicit operator AcceptanceReport(string value)
		{
			return new AcceptanceReport(value);
		}

		// Token: 0x06005180 RID: 20864 RVA: 0x001A786F File Offset: 0x001A5A6F
		public static implicit operator AcceptanceReport(TaggedString value)
		{
			return new AcceptanceReport(value);
		}

		// Token: 0x04003AAD RID: 15021
		private string reasonTextInt;

		// Token: 0x04003AAE RID: 15022
		private bool acceptedInt;
	}
}
