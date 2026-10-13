using System;

namespace Verse.AI
{
	// Token: 0x02000F4F RID: 3919
	public static class JobFailReason
	{
		// Token: 0x17000F57 RID: 3927
		// (get) Token: 0x060060F8 RID: 24824 RVA: 0x001EB9C4 File Offset: 0x001E9BC4
		public static string Reason
		{
			get
			{
				return JobFailReason.lastReason;
			}
		}

		// Token: 0x17000F58 RID: 3928
		// (get) Token: 0x060060F9 RID: 24825 RVA: 0x001EB9CB File Offset: 0x001E9BCB
		public static bool HaveReason
		{
			get
			{
				return JobFailReason.lastReason != null;
			}
		}

		// Token: 0x17000F59 RID: 3929
		// (get) Token: 0x060060FA RID: 24826 RVA: 0x001EB9D5 File Offset: 0x001E9BD5
		public static bool Silent
		{
			get
			{
				return JobFailReason.silent;
			}
		}

		// Token: 0x17000F5A RID: 3930
		// (get) Token: 0x060060FB RID: 24827 RVA: 0x001EB9DC File Offset: 0x001E9BDC
		public static string CustomJobString
		{
			get
			{
				return JobFailReason.lastCustomJobString;
			}
		}

		// Token: 0x060060FC RID: 24828 RVA: 0x001EB9E3 File Offset: 0x001E9BE3
		public static void Is(string reason, string customJobString = null)
		{
			JobFailReason.lastReason = reason;
			JobFailReason.lastCustomJobString = customJobString;
		}

		// Token: 0x060060FD RID: 24829 RVA: 0x001EB9F1 File Offset: 0x001E9BF1
		public static void IsSilent()
		{
			JobFailReason.silent = true;
		}

		// Token: 0x060060FE RID: 24830 RVA: 0x001EB9F9 File Offset: 0x001E9BF9
		public static void Clear()
		{
			JobFailReason.lastReason = null;
			JobFailReason.lastCustomJobString = null;
			JobFailReason.silent = false;
		}

		// Token: 0x040045EB RID: 17899
		private static string lastReason;

		// Token: 0x040045EC RID: 17900
		private static string lastCustomJobString;

		// Token: 0x040045ED RID: 17901
		private static bool silent;
	}
}
