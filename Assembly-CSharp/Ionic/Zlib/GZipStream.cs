using System;
using System.IO;
using System.Text;

namespace Ionic.Zlib
{
	// Token: 0x02003EAF RID: 16047
	public class GZipStream : Stream
	{
		// Token: 0x17003719 RID: 14105
		// (get) Token: 0x06016017 RID: 90135 RVA: 0x00693FC4 File Offset: 0x006921C4
		// (set) Token: 0x06016018 RID: 90136 RVA: 0x00693FCC File Offset: 0x006921CC
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._Comment = value;
			}
		}

		// Token: 0x1700371A RID: 14106
		// (get) Token: 0x06016019 RID: 90137 RVA: 0x00693FE8 File Offset: 0x006921E8
		// (set) Token: 0x0601601A RID: 90138 RVA: 0x00693FF0 File Offset: 0x006921F0
		public string FileName
		{
			get
			{
				return this._FileName;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._FileName = value;
				if (this._FileName == null)
				{
					return;
				}
				if (this._FileName.IndexOf("/") != -1)
				{
					this._FileName = this._FileName.Replace("/", "\\");
				}
				if (this._FileName.EndsWith("\\"))
				{
					throw new Exception("Illegal filename");
				}
				if (this._FileName.IndexOf("\\") != -1)
				{
					this._FileName = Path.GetFileName(this._FileName);
				}
			}
		}

		// Token: 0x1700371B RID: 14107
		// (get) Token: 0x0601601B RID: 90139 RVA: 0x0069408F File Offset: 0x0069228F
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x0601601C RID: 90140 RVA: 0x00694097 File Offset: 0x00692297
		public GZipStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x0601601D RID: 90141 RVA: 0x006940A3 File Offset: 0x006922A3
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x0601601E RID: 90142 RVA: 0x006940AF File Offset: 0x006922AF
		public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x0601601F RID: 90143 RVA: 0x006940BB File Offset: 0x006922BB
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.GZIP, leaveOpen);
		}

		// Token: 0x1700371C RID: 14108
		// (get) Token: 0x06016020 RID: 90144 RVA: 0x006940D8 File Offset: 0x006922D8
		// (set) Token: 0x06016021 RID: 90145 RVA: 0x006940E5 File Offset: 0x006922E5
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
					throw new ObjectDisposedException("GZipStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x1700371D RID: 14109
		// (get) Token: 0x06016022 RID: 90146 RVA: 0x00694106 File Offset: 0x00692306
		// (set) Token: 0x06016023 RID: 90147 RVA: 0x00694114 File Offset: 0x00692314
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
					throw new ObjectDisposedException("GZipStream");
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

		// Token: 0x1700371E RID: 14110
		// (get) Token: 0x06016024 RID: 90148 RVA: 0x00694180 File Offset: 0x00692380
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x1700371F RID: 14111
		// (get) Token: 0x06016025 RID: 90149 RVA: 0x00694192 File Offset: 0x00692392
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x06016026 RID: 90150 RVA: 0x006941A4 File Offset: 0x006923A4
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
						this._Crc32 = this._baseStream.Crc32;
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x17003720 RID: 14112
		// (get) Token: 0x06016027 RID: 90151 RVA: 0x00694204 File Offset: 0x00692404
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x17003721 RID: 14113
		// (get) Token: 0x06016028 RID: 90152 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003722 RID: 14114
		// (get) Token: 0x06016029 RID: 90153 RVA: 0x00694229 File Offset: 0x00692429
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x0601602A RID: 90154 RVA: 0x0069424E File Offset: 0x0069244E
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17003723 RID: 14115
		// (get) Token: 0x0601602B RID: 90155 RVA: 0x00066B11 File Offset: 0x00064D11
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17003724 RID: 14116
		// (get) Token: 0x0601602C RID: 90156 RVA: 0x00694270 File Offset: 0x00692470
		// (set) Token: 0x0601602D RID: 90157 RVA: 0x00066B11 File Offset: 0x00064D11
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut + (long)this._headerByteCount;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn + (long)this._baseStream._gzipHeaderByteCount;
				}
				return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x0601602E RID: 90158 RVA: 0x006942D4 File Offset: 0x006924D4
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			int result = this._baseStream.Read(buffer, offset, count);
			if (!this._firstReadDone)
			{
				this._firstReadDone = true;
				this.FileName = this._baseStream._GzipFileName;
				this.Comment = this._baseStream._GzipComment;
			}
			return result;
		}

		// Token: 0x0601602F RID: 90159 RVA: 0x00066B11 File Offset: 0x00064D11
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06016030 RID: 90160 RVA: 0x00066B11 File Offset: 0x00064D11
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06016031 RID: 90161 RVA: 0x00694334 File Offset: 0x00692534
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				if (!this._baseStream._wantCompress)
				{
					throw new InvalidOperationException();
				}
				this._headerByteCount = this.EmitHeader();
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x06016032 RID: 90162 RVA: 0x00694394 File Offset: 0x00692594
		private int EmitHeader()
		{
			byte[] array = (this.Comment == null) ? null : GZipStream.iso8859dash1.GetBytes(this.Comment);
			byte[] array2 = (this.FileName == null) ? null : GZipStream.iso8859dash1.GetBytes(this.FileName);
			int num = (this.Comment == null) ? 0 : (array.Length + 1);
			int num2 = (this.FileName == null) ? 0 : (array2.Length + 1);
			byte[] array3 = new byte[10 + num + num2];
			int num3 = 0;
			array3[num3++] = 31;
			array3[num3++] = 139;
			array3[num3++] = 8;
			byte b = 0;
			if (this.Comment != null)
			{
				b ^= 16;
			}
			if (this.FileName != null)
			{
				b ^= 8;
			}
			array3[num3++] = b;
			if (this.LastModified == null)
			{
				this.LastModified = new DateTime?(DateTime.Now);
			}
			Array.Copy(BitConverter.GetBytes((int)(this.LastModified.Value - GZipStream._unixEpoch).TotalSeconds), 0, array3, num3, 4);
			num3 += 4;
			array3[num3++] = 0;
			array3[num3++] = byte.MaxValue;
			if (num2 != 0)
			{
				Array.Copy(array2, 0, array3, num3, num2 - 1);
				num3 += num2 - 1;
				array3[num3++] = 0;
			}
			if (num != 0)
			{
				Array.Copy(array, 0, array3, num3, num - 1);
				num3 += num - 1;
				array3[num3++] = 0;
			}
			this._baseStream._stream.Write(array3, 0, array3.Length);
			return array3.Length;
		}

		// Token: 0x06016033 RID: 90163 RVA: 0x00694530 File Offset: 0x00692730
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06016034 RID: 90164 RVA: 0x00694578 File Offset: 0x00692778
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06016035 RID: 90165 RVA: 0x006945C0 File Offset: 0x006927C0
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x06016036 RID: 90166 RVA: 0x00694604 File Offset: 0x00692804
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x0400F3DD RID: 62429
		public DateTime? LastModified;

		// Token: 0x0400F3DE RID: 62430
		private int _headerByteCount;

		// Token: 0x0400F3DF RID: 62431
		internal ZlibBaseStream _baseStream;

		// Token: 0x0400F3E0 RID: 62432
		private bool _disposed;

		// Token: 0x0400F3E1 RID: 62433
		private bool _firstReadDone;

		// Token: 0x0400F3E2 RID: 62434
		private string _FileName;

		// Token: 0x0400F3E3 RID: 62435
		private string _Comment;

		// Token: 0x0400F3E4 RID: 62436
		private int _Crc32;

		// Token: 0x0400F3E5 RID: 62437
		internal static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x0400F3E6 RID: 62438
		internal static readonly Encoding iso8859dash1 = Encoding.GetEncoding("iso-8859-1");
	}
}
