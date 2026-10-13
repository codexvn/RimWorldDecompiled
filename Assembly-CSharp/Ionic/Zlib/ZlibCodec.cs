using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x02003EC7 RID: 16071
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000D")]
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public sealed class ZlibCodec
	{
		// Token: 0x17003739 RID: 14137
		// (get) Token: 0x060160AA RID: 90282 RVA: 0x00699AD0 File Offset: 0x00697CD0
		public int Adler32
		{
			get
			{
				return (int)this._Adler32;
			}
		}

		// Token: 0x060160AB RID: 90283 RVA: 0x00699AD8 File Offset: 0x00697CD8
		public ZlibCodec()
		{
		}

		// Token: 0x060160AC RID: 90284 RVA: 0x00699AF0 File Offset: 0x00697CF0
		public ZlibCodec(CompressionMode mode)
		{
			if (mode == CompressionMode.Compress)
			{
				if (this.InitializeDeflate() != 0)
				{
					throw new ZlibException("Cannot initialize for deflate.");
				}
			}
			else
			{
				if (mode != CompressionMode.Decompress)
				{
					throw new ZlibException("Invalid ZlibStreamFlavor.");
				}
				if (this.InitializeInflate() != 0)
				{
					throw new ZlibException("Cannot initialize for inflate.");
				}
			}
		}

		// Token: 0x060160AD RID: 90285 RVA: 0x00699B4A File Offset: 0x00697D4A
		public int InitializeInflate()
		{
			return this.InitializeInflate(this.WindowBits);
		}

		// Token: 0x060160AE RID: 90286 RVA: 0x00699B58 File Offset: 0x00697D58
		public int InitializeInflate(bool expectRfc1950Header)
		{
			return this.InitializeInflate(this.WindowBits, expectRfc1950Header);
		}

		// Token: 0x060160AF RID: 90287 RVA: 0x00699B67 File Offset: 0x00697D67
		public int InitializeInflate(int windowBits)
		{
			this.WindowBits = windowBits;
			return this.InitializeInflate(windowBits, true);
		}

		// Token: 0x060160B0 RID: 90288 RVA: 0x00699B78 File Offset: 0x00697D78
		public int InitializeInflate(int windowBits, bool expectRfc1950Header)
		{
			this.WindowBits = windowBits;
			if (this.dstate != null)
			{
				throw new ZlibException("You may not call InitializeInflate() after calling InitializeDeflate().");
			}
			this.istate = new InflateManager(expectRfc1950Header);
			return this.istate.Initialize(this, windowBits);
		}

		// Token: 0x060160B1 RID: 90289 RVA: 0x00699BAD File Offset: 0x00697DAD
		public int Inflate(FlushType flush)
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Inflate(flush);
		}

		// Token: 0x060160B2 RID: 90290 RVA: 0x00699BCE File Offset: 0x00697DCE
		public int EndInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			int result = this.istate.End();
			this.istate = null;
			return result;
		}

		// Token: 0x060160B3 RID: 90291 RVA: 0x00699BF5 File Offset: 0x00697DF5
		public int SyncInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Sync();
		}

		// Token: 0x060160B4 RID: 90292 RVA: 0x00699C15 File Offset: 0x00697E15
		public int InitializeDeflate()
		{
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x060160B5 RID: 90293 RVA: 0x00699C1E File Offset: 0x00697E1E
		public int InitializeDeflate(CompressionLevel level)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x060160B6 RID: 90294 RVA: 0x00699C2E File Offset: 0x00697E2E
		public int InitializeDeflate(CompressionLevel level, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x060160B7 RID: 90295 RVA: 0x00699C3E File Offset: 0x00697E3E
		public int InitializeDeflate(CompressionLevel level, int bits)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x060160B8 RID: 90296 RVA: 0x00699C55 File Offset: 0x00697E55
		public int InitializeDeflate(CompressionLevel level, int bits, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x060160B9 RID: 90297 RVA: 0x00699C6C File Offset: 0x00697E6C
		private int _InternalInitializeDeflate(bool wantRfc1950Header)
		{
			if (this.istate != null)
			{
				throw new ZlibException("You may not call InitializeDeflate() after calling InitializeInflate().");
			}
			this.dstate = new DeflateManager();
			this.dstate.WantRfc1950HeaderBytes = wantRfc1950Header;
			return this.dstate.Initialize(this, this.CompressLevel, this.WindowBits, this.Strategy);
		}

		// Token: 0x060160BA RID: 90298 RVA: 0x00699CC1 File Offset: 0x00697EC1
		public int Deflate(FlushType flush)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.Deflate(flush);
		}

		// Token: 0x060160BB RID: 90299 RVA: 0x00699CE2 File Offset: 0x00697EE2
		public int EndDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate = null;
			return 0;
		}

		// Token: 0x060160BC RID: 90300 RVA: 0x00699CFF File Offset: 0x00697EFF
		public void ResetDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate.Reset();
		}

		// Token: 0x060160BD RID: 90301 RVA: 0x00699D1F File Offset: 0x00697F1F
		public int SetDeflateParams(CompressionLevel level, CompressionStrategy strategy)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.SetParams(level, strategy);
		}

		// Token: 0x060160BE RID: 90302 RVA: 0x00699D41 File Offset: 0x00697F41
		public int SetDictionary(byte[] dictionary)
		{
			if (this.istate != null)
			{
				return this.istate.SetDictionary(dictionary);
			}
			if (this.dstate != null)
			{
				return this.dstate.SetDictionary(dictionary);
			}
			throw new ZlibException("No Inflate or Deflate state!");
		}

		// Token: 0x060160BF RID: 90303 RVA: 0x00699D78 File Offset: 0x00697F78
		internal void flush_pending()
		{
			int num = this.dstate.pendingCount;
			if (num > this.AvailableBytesOut)
			{
				num = this.AvailableBytesOut;
			}
			if (num == 0)
			{
				return;
			}
			if (this.dstate.pending.Length <= this.dstate.nextPending || this.OutputBuffer.Length <= this.NextOut || this.dstate.pending.Length < this.dstate.nextPending + num || this.OutputBuffer.Length < this.NextOut + num)
			{
				throw new ZlibException(string.Format("Invalid State. (pending.Length={0}, pendingCount={1})", this.dstate.pending.Length, this.dstate.pendingCount));
			}
			Array.Copy(this.dstate.pending, this.dstate.nextPending, this.OutputBuffer, this.NextOut, num);
			this.NextOut += num;
			this.dstate.nextPending += num;
			this.TotalBytesOut += (long)num;
			this.AvailableBytesOut -= num;
			this.dstate.pendingCount -= num;
			if (this.dstate.pendingCount == 0)
			{
				this.dstate.nextPending = 0;
			}
		}

		// Token: 0x060160C0 RID: 90304 RVA: 0x00699EC4 File Offset: 0x006980C4
		internal int read_buf(byte[] buf, int start, int size)
		{
			int num = this.AvailableBytesIn;
			if (num > size)
			{
				num = size;
			}
			if (num == 0)
			{
				return 0;
			}
			this.AvailableBytesIn -= num;
			if (this.dstate.WantRfc1950HeaderBytes)
			{
				this._Adler32 = Adler.Adler32(this._Adler32, this.InputBuffer, this.NextIn, num);
			}
			Array.Copy(this.InputBuffer, this.NextIn, buf, start, num);
			this.NextIn += num;
			this.TotalBytesIn += (long)num;
			return num;
		}

		// Token: 0x0400F4E5 RID: 62693
		public byte[] InputBuffer;

		// Token: 0x0400F4E6 RID: 62694
		public int NextIn;

		// Token: 0x0400F4E7 RID: 62695
		public int AvailableBytesIn;

		// Token: 0x0400F4E8 RID: 62696
		public long TotalBytesIn;

		// Token: 0x0400F4E9 RID: 62697
		public byte[] OutputBuffer;

		// Token: 0x0400F4EA RID: 62698
		public int NextOut;

		// Token: 0x0400F4EB RID: 62699
		public int AvailableBytesOut;

		// Token: 0x0400F4EC RID: 62700
		public long TotalBytesOut;

		// Token: 0x0400F4ED RID: 62701
		public string Message;

		// Token: 0x0400F4EE RID: 62702
		internal DeflateManager dstate;

		// Token: 0x0400F4EF RID: 62703
		internal InflateManager istate;

		// Token: 0x0400F4F0 RID: 62704
		internal uint _Adler32;

		// Token: 0x0400F4F1 RID: 62705
		public CompressionLevel CompressLevel = CompressionLevel.Default;

		// Token: 0x0400F4F2 RID: 62706
		public int WindowBits = 15;

		// Token: 0x0400F4F3 RID: 62707
		public CompressionStrategy Strategy;
	}
}
