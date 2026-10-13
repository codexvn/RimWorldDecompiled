using System;
using System.IO;

namespace Ionic.Zlib
{
	// Token: 0x02003EC9 RID: 16073
	public class ZlibStream : Stream
	{
		// Token: 0x060160C1 RID: 90305 RVA: 0x00699F4E File Offset: 0x0069814E
		public ZlibStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x060160C2 RID: 90306 RVA: 0x00699F5A File Offset: 0x0069815A
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x060160C3 RID: 90307 RVA: 0x00699F66 File Offset: 0x00698166
		public ZlibStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x060160C4 RID: 90308 RVA: 0x00699F72 File Offset: 0x00698172
		public ZlibStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.ZLIB, leaveOpen);
		}

		// Token: 0x1700373A RID: 14138
		// (get) Token: 0x060160C5 RID: 90309 RVA: 0x00699F8F File Offset: 0x0069818F
		// (set) Token: 0x060160C6 RID: 90310 RVA: 0x00699F9C File Offset: 0x0069819C
		public virtual FlushType FlushMode
		{
			get
			{
				return this._baseStream._flushMode;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x1700373B RID: 14139
		// (get) Token: 0x060160C7 RID: 90311 RVA: 0x00699FBD File Offset: 0x006981BD
		// (set) Token: 0x060160C8 RID: 90312 RVA: 0x00699FCC File Offset: 0x006981CC
		public int BufferSize
		{
			get
			{
				return this._baseStream._bufferSize;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				if (this._baseStream._workingBuffer != null)
				{
					throw new ZlibException("The working buffer is already set.");
				}
				if (value < 1024)
				{
					throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.", value, 1024));
				}
				this._baseStream._bufferSize = value;
			}
		}

		// Token: 0x1700373C RID: 14140
		// (get) Token: 0x060160C9 RID: 90313 RVA: 0x0069A038 File Offset: 0x00698238
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x1700373D RID: 14141
		// (get) Token: 0x060160CA RID: 90314 RVA: 0x0069A04A File Offset: 0x0069824A
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x060160CB RID: 90315 RVA: 0x0069A05C File Offset: 0x0069825C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x1700373E RID: 14142
		// (get) Token: 0x060160CC RID: 90316 RVA: 0x0069A0A8 File Offset: 0x006982A8
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x1700373F RID: 14143
		// (get) Token: 0x060160CD RID: 90317 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003740 RID: 14144
		// (get) Token: 0x060160CE RID: 90318 RVA: 0x0069A0CD File Offset: 0x006982CD
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("ZlibStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x060160CF RID: 90319 RVA: 0x0069A0F2 File Offset: 0x006982F2
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17003741 RID: 14145
		// (get) Token: 0x060160D0 RID: 90320 RVA: 0x00002894 File Offset: 0x00000A94
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17003742 RID: 14146
		// (get) Token: 0x060160D1 RID: 90321 RVA: 0x0069A114 File Offset: 0x00698314
		// (set) Token: 0x060160D2 RID: 90322 RVA: 0x00002894 File Offset: 0x00000A94
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn;
				}
				return 0L;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060160D3 RID: 90323 RVA: 0x0069A160 File Offset: 0x00698360
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x060160D4 RID: 90324 RVA: 0x00002894 File Offset: 0x00000A94
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060160D5 RID: 90325 RVA: 0x00002894 File Offset: 0x00000A94
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060160D6 RID: 90326 RVA: 0x0069A183 File Offset: 0x00698383
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("ZlibStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x060160D7 RID: 90327 RVA: 0x0069A1A8 File Offset: 0x006983A8
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060160D8 RID: 90328 RVA: 0x0069A1F0 File Offset: 0x006983F0
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new ZlibStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x060160D9 RID: 90329 RVA: 0x0069A238 File Offset: 0x00698438
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x060160DA RID: 90330 RVA: 0x0069A27C File Offset: 0x0069847C
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new ZlibStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0400F4FE RID: 62718
		internal ZlibBaseStream _baseStream;

		// Token: 0x0400F4FF RID: 62719
		private bool _disposed;
	}
}
