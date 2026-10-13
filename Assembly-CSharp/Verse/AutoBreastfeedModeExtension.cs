using System;

namespace Verse
{
	// Token: 0x020007C9 RID: 1993
	public static class AutoBreastfeedModeExtension
	{
		// Token: 0x0600325F RID: 12895 RVA: 0x00107822 File Offset: 0x00105A22
		public static TaggedString Translate(this AutofeedMode mode)
		{
			switch (mode)
			{
			case AutofeedMode.Never:
				return "AutofeedModeNever".Translate();
			case AutofeedMode.Childcare:
				return "AutofeedModeChildcare".Translate();
			case AutofeedMode.Urgent:
				return "AutofeedModeUrgent".Translate();
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06003260 RID: 12896 RVA: 0x00107860 File Offset: 0x00105A60
		public static TaggedString GetTooltip(this AutofeedMode mode, Pawn baby, Pawn feeder)
		{
			string key;
			switch (mode)
			{
			case AutofeedMode.Never:
				key = "AutofeedModeTooltipNever";
				break;
			case AutofeedMode.Childcare:
				key = "AutofeedModeTooltipChildcare";
				break;
			case AutofeedMode.Urgent:
				key = "AutofeedModeTooltipUrgent";
				break;
			default:
				throw new NotImplementedException();
			}
			return key.Translate(baby.Named("BABY"), feeder.Named("FEEDER"));
		}
	}
}
