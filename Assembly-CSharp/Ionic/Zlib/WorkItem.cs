using System;

namespace Ionic.Zlib
{
	// Token: 0x02003EB7 RID: 16055
	internal class WorkItem
	{
		// Token: 0x06016058 RID: 90200 RVA: 0x00697AF0 File Offset: 0x00695CF0
		public WorkItem(int size, CompressionLevel compressLevel, CompressionStrategy strategy, int ix)
		{
			this.buffer = new byte[size];
			int num = size + (size / 32768 + 1) * 5 * 2;
			this.compressed = new byte[num];
			this.compressor = new ZlibCodec();
			this.compressor.InitializeDeflate(compressLevel, false);
			this.compressor.OutputBuffer = this.compressed;
			this.compressor.InputBuffer = this.buffer;
			this.index = ix;
		}

		// Token: 0x0400F455 RID: 62549
		public byte[] buffer;

		// Token: 0x0400F456 RID: 62550
		public byte[] compressed;

		// Token: 0x0400F457 RID: 62551
		public int crc;

		// Token: 0x0400F458 RID: 62552
		public int index;

		// Token: 0x0400F459 RID: 62553
		public int ordinal;

		// Token: 0x0400F45A RID: 62554
		public int inputBytesAvailable;

		// Token: 0x0400F45B RID: 62555
		public int compressedBytesAvailable;

		// Token: 0x0400F45C RID: 62556
		public ZlibCodec compressor;
	}
}
