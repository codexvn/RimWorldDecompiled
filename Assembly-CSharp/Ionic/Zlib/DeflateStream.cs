using System;
using System.IO;

namespace Ionic.Zlib
{
	// Token: 0x02003EAE RID: 16046
	public class DeflateStream : Stream
	{
		// Token: 0x06015FFB RID: 90107 RVA: 0x00693C1C File Offset: 0x00691E1C
		public DeflateStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x06015FFC RID: 90108 RVA: 0x00693C28 File Offset: 0x00691E28
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x06015FFD RID: 90109 RVA: 0x00693C34 File Offset: 0x00691E34
		public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x06015FFE RID: 90110 RVA: 0x00693C40 File Offset: 0x00691E40
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._innerStream = stream;
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.DEFLATE, leaveOpen);
		}

		// Token: 0x1700370F RID: 14095
		// (get) Token: 0x06015FFF RID: 90111 RVA: 0x00693C64 File Offset: 0x00691E64
		// (set) Token: 0x06016000 RID: 90112 RVA: 0x00693C71 File Offset: 0x00691E71
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
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x17003710 RID: 14096
		// (get) Token: 0x06016001 RID: 90113 RVA: 0x00693C92 File Offset: 0x00691E92
		// (set) Token: 0x06016002 RID: 90114 RVA: 0x00693CA0 File Offset: 0x00691EA0
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
					throw new ObjectDisposedException("DeflateStream");
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

		// Token: 0x17003711 RID: 14097
		// (get) Token: 0x06016003 RID: 90115 RVA: 0x00693D0C File Offset: 0x00691F0C
		// (set) Token: 0x06016004 RID: 90116 RVA: 0x00693D19 File Offset: 0x00691F19
		public CompressionStrategy Strategy
		{
			get
			{
				return this._baseStream.Strategy;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream.Strategy = value;
			}
		}

		// Token: 0x17003712 RID: 14098
		// (get) Token: 0x06016005 RID: 90117 RVA: 0x00693D3A File Offset: 0x00691F3A
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x17003713 RID: 14099
		// (get) Token: 0x06016006 RID: 90118 RVA: 0x00693D4C File Offset: 0x00691F4C
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x06016007 RID: 90119 RVA: 0x00693D60 File Offset: 0x00691F60
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

		// Token: 0x17003714 RID: 14100
		// (get) Token: 0x06016008 RID: 90120 RVA: 0x00693DAC File Offset: 0x00691FAC
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x17003715 RID: 14101
		// (get) Token: 0x06016009 RID: 90121 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003716 RID: 14102
		// (get) Token: 0x0601600A RID: 90122 RVA: 0x00693DD1 File Offset: 0x00691FD1
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x0601600B RID: 90123 RVA: 0x00693DF6 File Offset: 0x00691FF6
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17003717 RID: 14103
		// (get) Token: 0x0601600C RID: 90124 RVA: 0x00066B11 File Offset: 0x00064D11
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17003718 RID: 14104
		// (get) Token: 0x0601600D RID: 90125 RVA: 0x00693E18 File Offset: 0x00692018
		// (set) Token: 0x0601600E RID: 90126 RVA: 0x00066B11 File Offset: 0x00064D11
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
				throw new NotImplementedException();
			}
		}

		// Token: 0x0601600F RID: 90127 RVA: 0x00693E64 File Offset: 0x00692064
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x06016010 RID: 90128 RVA: 0x00066B11 File Offset: 0x00064D11
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06016011 RID: 90129 RVA: 0x00066B11 File Offset: 0x00064D11
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06016012 RID: 90130 RVA: 0x00693E87 File Offset: 0x00692087
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x06016013 RID: 90131 RVA: 0x00693EAC File Offset: 0x006920AC
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06016014 RID: 90132 RVA: 0x00693EF4 File Offset: 0x006920F4
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06016015 RID: 90133 RVA: 0x00693F3C File Offset: 0x0069213C
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x06016016 RID: 90134 RVA: 0x00693F80 File Offset: 0x00692180
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0400F3DA RID: 62426
		internal ZlibBaseStream _baseStream;

		// Token: 0x0400F3DB RID: 62427
		internal Stream _innerStream;

		// Token: 0x0400F3DC RID: 62428
		private bool _disposed;
	}
}
