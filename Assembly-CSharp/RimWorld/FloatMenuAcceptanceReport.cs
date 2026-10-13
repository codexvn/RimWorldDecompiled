using System;

namespace RimWorld
{
	// Token: 0x02002838 RID: 10296
	public struct FloatMenuAcceptanceReport
	{
		// Token: 0x1700225B RID: 8795
		// (get) Token: 0x0600E158 RID: 57688 RVA: 0x004217BB File Offset: 0x0041F9BB
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x1700225C RID: 8796
		// (get) Token: 0x0600E159 RID: 57689 RVA: 0x004217C3 File Offset: 0x0041F9C3
		public string FailMessage
		{
			get
			{
				return this.failMessageInt;
			}
		}

		// Token: 0x1700225D RID: 8797
		// (get) Token: 0x0600E15A RID: 57690 RVA: 0x004217CB File Offset: 0x0041F9CB
		public string FailReason
		{
			get
			{
				return this.failReasonInt;
			}
		}

		// Token: 0x1700225E RID: 8798
		// (get) Token: 0x0600E15B RID: 57691 RVA: 0x004217D4 File Offset: 0x0041F9D4
		public static FloatMenuAcceptanceReport WasAccepted
		{
			get
			{
				return new FloatMenuAcceptanceReport
				{
					acceptedInt = true
				};
			}
		}

		// Token: 0x1700225F RID: 8799
		// (get) Token: 0x0600E15C RID: 57692 RVA: 0x004217F4 File Offset: 0x0041F9F4
		public static FloatMenuAcceptanceReport WasRejected
		{
			get
			{
				return new FloatMenuAcceptanceReport
				{
					acceptedInt = false
				};
			}
		}

		// Token: 0x0600E15D RID: 57693 RVA: 0x00421812 File Offset: 0x0041FA12
		public static implicit operator FloatMenuAcceptanceReport(bool value)
		{
			if (value)
			{
				return FloatMenuAcceptanceReport.WasAccepted;
			}
			return FloatMenuAcceptanceReport.WasRejected;
		}

		// Token: 0x0600E15E RID: 57694 RVA: 0x00421822 File Offset: 0x0041FA22
		public static implicit operator bool(FloatMenuAcceptanceReport rep)
		{
			return rep.Accepted;
		}

		// Token: 0x0600E15F RID: 57695 RVA: 0x0042182C File Offset: 0x0041FA2C
		public static FloatMenuAcceptanceReport WithFailReason(string failReason)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failReasonInt = failReason
			};
		}

		// Token: 0x0600E160 RID: 57696 RVA: 0x00421854 File Offset: 0x0041FA54
		public static FloatMenuAcceptanceReport WithFailMessage(string failMessage)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failMessageInt = failMessage
			};
		}

		// Token: 0x0600E161 RID: 57697 RVA: 0x0042187C File Offset: 0x0041FA7C
		public static FloatMenuAcceptanceReport WithFailReasonAndMessage(string failReason, string failMessage)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failReasonInt = failReason,
				failMessageInt = failMessage
			};
		}

		// Token: 0x0400977F RID: 38783
		private string failMessageInt;

		// Token: 0x04009780 RID: 38784
		private string failReasonInt;

		// Token: 0x04009781 RID: 38785
		private bool acceptedInt;
	}
}
