using System;
using System.IO;

namespace Ionic.Crc
{
	// Token: 0x02003ECB RID: 16075
	public class CrcCalculatorStream : Stream, IDisposable
	{
		// Token: 0x060160EE RID: 90350 RVA: 0x0069A76E File Offset: 0x0069896E
		public CrcCalculatorStream(Stream stream) : this(true, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x060160EF RID: 90351 RVA: 0x0069A77E File Offset: 0x0069897E
		public CrcCalculatorStream(Stream stream, bool leaveOpen) : this(leaveOpen, CrcCalculatorStream.UnsetLengthLimit, stream, null)
		{
		}

		// Token: 0x060160F0 RID: 90352 RVA: 0x0069A78E File Offset: 0x0069898E
		public CrcCalculatorStream(Stream stream, long length) : this(true, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x060160F1 RID: 90353 RVA: 0x0069A7AA File Offset: 0x006989AA
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen) : this(leaveOpen, length, stream, null)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x060160F2 RID: 90354 RVA: 0x0069A7C6 File Offset: 0x006989C6
		public CrcCalculatorStream(Stream stream, long length, bool leaveOpen, CRC32 crc32) : this(leaveOpen, length, stream, crc32)
		{
			if (length < 0L)
			{
				throw new ArgumentException("length");
			}
		}

		// Token: 0x060160F3 RID: 90355 RVA: 0x0069A7E4 File Offset: 0x006989E4
		private CrcCalculatorStream(bool leaveOpen, long length, Stream stream, CRC32 crc32)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			this._innerStream = stream;
			this._crc32 = (crc32 ?? new CRC32());
			this._lengthLimit = length;
			this._leaveOpen = leaveOpen;
		}

		// Token: 0x17003745 RID: 14149
		// (get) Token: 0x060160F4 RID: 90356 RVA: 0x0069A834 File Offset: 0x00698A34
		public long TotalBytesSlurped
		{
			get
			{
				return this._crc32.TotalBytesRead;
			}
		}

		// Token: 0x17003746 RID: 14150
		// (get) Token: 0x060160F5 RID: 90357 RVA: 0x0069A841 File Offset: 0x00698A41
		public int Crc
		{
			get
			{
				return this._crc32.Crc32Result;
			}
		}

		// Token: 0x17003747 RID: 14151
		// (get) Token: 0x060160F6 RID: 90358 RVA: 0x0069A84E File Offset: 0x00698A4E
		// (set) Token: 0x060160F7 RID: 90359 RVA: 0x0069A856 File Offset: 0x00698A56
		public bool LeaveOpen
		{
			get
			{
				return this._leaveOpen;
			}
			set
			{
				this._leaveOpen = value;
			}
		}

		// Token: 0x060160F8 RID: 90360 RVA: 0x0069A860 File Offset: 0x00698A60
		public override int Read(byte[] buffer, int offset, int count)
		{
			int count2 = count;
			if (this._lengthLimit != CrcCalculatorStream.UnsetLengthLimit)
			{
				if (this._crc32.TotalBytesRead >= this._lengthLimit)
				{
					return 0;
				}
				long num = this._lengthLimit - this._crc32.TotalBytesRead;
				if (num < (long)count)
				{
					count2 = (int)num;
				}
			}
			int num2 = this._innerStream.Read(buffer, offset, count2);
			if (num2 > 0)
			{
				this._crc32.SlurpBlock(buffer, offset, num2);
			}
			return num2;
		}

		// Token: 0x060160F9 RID: 90361 RVA: 0x0069A8CE File Offset: 0x00698ACE
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (count > 0)
			{
				this._crc32.SlurpBlock(buffer, offset, count);
			}
			this._innerStream.Write(buffer, offset, count);
		}

		// Token: 0x17003748 RID: 14152
		// (get) Token: 0x060160FA RID: 90362 RVA: 0x0069A8F0 File Offset: 0x00698AF0
		public override bool CanRead
		{
			get
			{
				return this._innerStream.CanRead;
			}
		}

		// Token: 0x17003749 RID: 14153
		// (get) Token: 0x060160FB RID: 90363 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700374A RID: 14154
		// (get) Token: 0x060160FC RID: 90364 RVA: 0x0069A8FD File Offset: 0x00698AFD
		public override bool CanWrite
		{
			get
			{
				return this._innerStream.CanWrite;
			}
		}

		// Token: 0x060160FD RID: 90365 RVA: 0x0069A90A File Offset: 0x00698B0A
		public override void Flush()
		{
			this._innerStream.Flush();
		}

		// Token: 0x1700374B RID: 14155
		// (get) Token: 0x060160FE RID: 90366 RVA: 0x0069A917 File Offset: 0x00698B17
		public override long Length
		{
			get
			{
				if (this._lengthLimit == CrcCalculatorStream.UnsetLengthLimit)
				{
					return this._innerStream.Length;
				}
				return this._lengthLimit;
			}
		}

		// Token: 0x1700374C RID: 14156
		// (get) Token: 0x060160FF RID: 90367 RVA: 0x0069A834 File Offset: 0x00698A34
		// (set) Token: 0x06016100 RID: 90368 RVA: 0x00002894 File Offset: 0x00000A94
		public override long Position
		{
			get
			{
				return this._crc32.TotalBytesRead;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06016101 RID: 90369 RVA: 0x00002894 File Offset: 0x00000A94
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06016102 RID: 90370 RVA: 0x00002894 File Offset: 0x00000A94
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06016103 RID: 90371 RVA: 0x0069A938 File Offset: 0x00698B38
		void IDisposable.Dispose()
		{
			this.Close();
		}

		// Token: 0x06016104 RID: 90372 RVA: 0x0069A940 File Offset: 0x00698B40
		public override void Close()
		{
			base.Close();
			if (!this._leaveOpen)
			{
				this._innerStream.Close();
			}
		}

		// Token: 0x0400F506 RID: 62726
		private static readonly long UnsetLengthLimit = -99L;

		// Token: 0x0400F507 RID: 62727
		private readonly Stream _innerStream;

		// Token: 0x0400F508 RID: 62728
		private readonly CRC32 _crc32;

		// Token: 0x0400F509 RID: 62729
		private readonly long _lengthLimit = -99L;

		// Token: 0x0400F50A RID: 62730
		private bool _leaveOpen;
	}
}
