using System;

namespace Ionic.Zlib
{
	// Token: 0x02003EC8 RID: 16072
	public static class ZlibConstants
	{
		// Token: 0x0400F4F4 RID: 62708
		public const int WindowBitsMax = 15;

		// Token: 0x0400F4F5 RID: 62709
		public const int WindowBitsDefault = 15;

		// Token: 0x0400F4F6 RID: 62710
		public const int Z_OK = 0;

		// Token: 0x0400F4F7 RID: 62711
		public const int Z_STREAM_END = 1;

		// Token: 0x0400F4F8 RID: 62712
		public const int Z_NEED_DICT = 2;

		// Token: 0x0400F4F9 RID: 62713
		public const int Z_STREAM_ERROR = -2;

		// Token: 0x0400F4FA RID: 62714
		public const int Z_DATA_ERROR = -3;

		// Token: 0x0400F4FB RID: 62715
		public const int Z_BUF_ERROR = -5;

		// Token: 0x0400F4FC RID: 62716
		public const int WorkingBufferSizeDefault = 16384;

		// Token: 0x0400F4FD RID: 62717
		public const int WorkingBufferSizeMin = 1024;
	}
}
