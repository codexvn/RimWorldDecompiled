using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Verse
{
	// Token: 0x02000D3E RID: 3390
	public class MemoryMappedFileSpanWrapper : IDisposable
	{
		// Token: 0x06005633 RID: 22067 RVA: 0x001BD75E File Offset: 0x001BB95E
		[return: TupleElementNames(new string[]
		{
			"file",
			"length"
		})]
		private static ValueTuple<MemoryMappedFile, long> OpenExistingMmf(string path)
		{
			return MemoryMappedFileSpanWrapper.OpenExistingMmf(new FileInfo(path), MemoryMappedFileAccess.Read);
		}

		// Token: 0x06005634 RID: 22068 RVA: 0x001BD76C File Offset: 0x001BB96C
		[return: TupleElementNames(new string[]
		{
			"file",
			"length"
		})]
		private static ValueTuple<MemoryMappedFile, long> OpenExistingMmf(FileInfo info, MemoryMappedFileAccess access)
		{
			if (!info.Exists)
			{
				throw new FileNotFoundException("File not found", info.FullName);
			}
			long length = info.Length;
			return new ValueTuple<MemoryMappedFile, long>(MemoryMappedFile.CreateFromFile(info.FullName, FileMode.Open, null, length, access), length);
		}

		// Token: 0x06005635 RID: 22069 RVA: 0x001BD7AE File Offset: 0x001BB9AE
		private MemoryMappedFileSpanWrapper([TupleElementNames(new string[]
		{
			"file",
			"length"
		})] ValueTuple<MemoryMappedFile, long> data, MemoryMappedFileAccess access)
		{
			this._access = access;
			this._length = data.Item2;
			this._file = data.Item1;
		}

		// Token: 0x06005636 RID: 22070 RVA: 0x001BD7D5 File Offset: 0x001BB9D5
		public MemoryMappedFileSpanWrapper(string path) : this(MemoryMappedFileSpanWrapper.OpenExistingMmf(path), MemoryMappedFileAccess.Read)
		{
		}

		// Token: 0x06005637 RID: 22071 RVA: 0x001BD7E4 File Offset: 0x001BB9E4
		public MemoryMappedFileSpanWrapper(FileInfo info) : this(MemoryMappedFileSpanWrapper.OpenExistingMmf(info, MemoryMappedFileAccess.Read), MemoryMappedFileAccess.Read)
		{
		}

		// Token: 0x06005638 RID: 22072 RVA: 0x001BD7F4 File Offset: 0x001BB9F4
		public MemoryMappedFileSpanWrapper(string path, long newFileSize, bool create = true) : this(new ValueTuple<MemoryMappedFile, long>(MemoryMappedFile.CreateFromFile(path, create ? FileMode.Create : FileMode.Open, null, newFileSize, MemoryMappedFileAccess.ReadWrite), newFileSize), MemoryMappedFileAccess.ReadWrite)
		{
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x06005639 RID: 22073 RVA: 0x001BD813 File Offset: 0x001BBA13
		public long FileSize
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x0600563A RID: 22074 RVA: 0x001BD81B File Offset: 0x001BBA1B
		public void Dispose()
		{
			this.ReleaseView();
			MemoryMappedFile file = this._file;
			if (file == null)
			{
				return;
			}
			file.Dispose();
		}

		// Token: 0x0600563B RID: 22075 RVA: 0x001BD834 File Offset: 0x001BBA34
		private void EnsureReadPointer()
		{
			if (this._pointer != null)
			{
				return;
			}
			this._accessor = this._file.CreateViewAccessor(0L, this._length, this._access);
			this._accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref this._pointer);
			if (this._pointer == null)
			{
				this._accessor.SafeMemoryMappedViewHandle.ReleasePointer();
				this._accessor.Dispose();
				this._file.Dispose();
				throw new InvalidOperationException("Failed to acquire pointer to memory mapped file");
			}
		}

		// Token: 0x0600563C RID: 22076 RVA: 0x001BD8BC File Offset: 0x001BBABC
		private unsafe Span<byte> GetSpan(long offset, long length)
		{
			this.EnsureReadPointer();
			if (offset < 0L || offset >= this._length)
			{
				throw new ArgumentOutOfRangeException("offset", string.Format("Offset {0} is out of range [0, {1})", offset, this._length));
			}
			if (length < 0L || offset + length > this._length)
			{
				throw new ArgumentOutOfRangeException("length", string.Format("Length {0} is out of range [0, {1}) given offset {2} and total file length {3}", new object[]
				{
					length,
					this._length - offset,
					offset,
					this._length
				}));
			}
			if (length > 2147483647L)
			{
				throw new ArgumentOutOfRangeException("length", string.Format("Length must be less than int.MaxValue, was {0}", length));
			}
			return new Span<byte>((void*)(this._pointer + offset), (int)length);
		}

		// Token: 0x0600563D RID: 22077 RVA: 0x001BD992 File Offset: 0x001BBB92
		public Span<byte> GetSpan(long offset)
		{
			return this.GetSpan(offset, this._length - offset);
		}

		// Token: 0x0600563E RID: 22078 RVA: 0x001BD9A3 File Offset: 0x001BBBA3
		public ReadOnlySpan<byte> GetReadOnlySpan(long offset, long length)
		{
			return this.GetSpan(offset, length);
		}

		// Token: 0x0600563F RID: 22079 RVA: 0x001BD9B2 File Offset: 0x001BBBB2
		public T Read<[IsUnmanaged] T>(long offset) where T : struct, ValueType
		{
			return MemoryMarshal.Read<T>(this.GetReadOnlySpan(offset, (long)sizeof(T)));
		}

		// Token: 0x06005640 RID: 22080 RVA: 0x001BD9C7 File Offset: 0x001BBBC7
		public void Write<[IsUnmanaged] T>(long offset, T value) where T : struct, ValueType
		{
			MemoryMarshal.Write<T>(this.GetSpan(offset, (long)sizeof(T)), ref value);
		}

		// Token: 0x06005641 RID: 22081 RVA: 0x001BD9DE File Offset: 0x001BBBDE
		public void Write(long offset, ReadOnlySpan<byte> span)
		{
			span.CopyTo(this.GetSpan(offset, (long)span.Length));
		}

		// Token: 0x06005642 RID: 22082 RVA: 0x001BD9F8 File Offset: 0x001BBBF8
		public void CopyTo(Stream other, long start, long length = -1L)
		{
			if (length == -1L)
			{
				length = this._length - start;
			}
			long num = length / 2147483647L;
			long num2 = length % 2147483647L;
			for (long num3 = 0L; num3 < num; num3 += 1L)
			{
				long offset = start + num3 * 2147483647L;
				other.Write(this.GetReadOnlySpan(offset, 2147483647L));
			}
			if (num2 > 0L)
			{
				other.Write(this.GetReadOnlySpan(start + num * 2147483647L, num2));
			}
		}

		// Token: 0x06005643 RID: 22083 RVA: 0x001BDA70 File Offset: 0x001BBC70
		private unsafe Span<byte> GetBufferForCopyFrom(long pos, long length)
		{
			if (this._pointer != null)
			{
				this.ReleaseView();
			}
			Span<byte> result;
			try
			{
				length = Math.Min(67108864L, length);
				this._accessor = this._file.CreateViewAccessor(pos, length, this._access);
				byte* pointer = null;
				this._accessor.SafeMemoryMappedViewHandle.AcquirePointer(ref pointer);
				result = new Span<byte>((void*)pointer, (int)length);
			}
			catch (Exception innerException)
			{
				throw new IOException(string.Format("Failed to get buffer to write to MMF of size {0} at position {1} with length {2}", this._length, pos, length), innerException);
			}
			return result;
		}

		// Token: 0x06005644 RID: 22084 RVA: 0x001BDB10 File Offset: 0x001BBD10
		private void ReleaseView()
		{
			MemoryMappedViewAccessor accessor = this._accessor;
			if (accessor != null)
			{
				accessor.SafeMemoryMappedViewHandle.ReleasePointer();
			}
			MemoryMappedViewAccessor accessor2 = this._accessor;
			if (accessor2 != null)
			{
				accessor2.Dispose();
			}
			this._pointer = null;
			this._accessor = null;
		}

		// Token: 0x06005645 RID: 22085 RVA: 0x001BDB48 File Offset: 0x001BBD48
		public void CopyFrom(Stream other, long initialPos = 0L)
		{
			long num = initialPos;
			long num2 = this._length - initialPos;
			Span<byte> bufferForCopyFrom = this.GetBufferForCopyFrom(num, num2);
			for (;;)
			{
				int num3 = other.Read(bufferForCopyFrom);
				if (num3 == 0)
				{
					break;
				}
				num += (long)num3;
				num2 -= (long)num3;
				if (num2 <= 0L)
				{
					break;
				}
				bufferForCopyFrom = this.GetBufferForCopyFrom(num, num2);
			}
			this.ReleaseView();
		}

		// Token: 0x06005646 RID: 22086 RVA: 0x001BDB94 File Offset: 0x001BBD94
		public void CopyFrom(MemoryMappedFileSpanWrapper other, long readPos = 0L, long writePos = 0L)
		{
			long num = writePos;
			long num2 = Math.Min(this._length - writePos, other._length - readPos);
			Span<byte> bufferForCopyFrom = this.GetBufferForCopyFrom(num, num2);
			for (;;)
			{
				ReadOnlySpan<byte> readOnlySpan = other.GetReadOnlySpan(readPos, (long)bufferForCopyFrom.Length);
				readOnlySpan.CopyTo(bufferForCopyFrom);
				other.ReleaseView();
				this.ReleaseView();
				num += (long)readOnlySpan.Length;
				num2 -= (long)readOnlySpan.Length;
				readPos += (long)readOnlySpan.Length;
				if (num2 <= 0L)
				{
					break;
				}
				bufferForCopyFrom = this.GetBufferForCopyFrom(num, num2);
			}
			other.ReleaseView();
			this.ReleaseView();
		}

		// Token: 0x06005647 RID: 22087 RVA: 0x001BDC24 File Offset: 0x001BBE24
		public unsafe byte* GetDirectPointer()
		{
			return this._pointer;
		}

		// Token: 0x06005648 RID: 22088 RVA: 0x001BDC2C File Offset: 0x001BBE2C
		public MemoryMappedFileSpanWrapper.SimpleReadStream CreateReadStream(long start, long length = -1L)
		{
			if (length == -1L)
			{
				length = this._length - start;
			}
			return new MemoryMappedFileSpanWrapper.SimpleReadStream(this, start, length);
		}

		// Token: 0x04003ED9 RID: 16089
		private readonly MemoryMappedFileAccess _access;

		// Token: 0x04003EDA RID: 16090
		private readonly MemoryMappedFile _file;

		// Token: 0x04003EDB RID: 16091
		private MemoryMappedViewAccessor _accessor;

		// Token: 0x04003EDC RID: 16092
		private unsafe byte* _pointer;

		// Token: 0x04003EDD RID: 16093
		private readonly long _length;

		// Token: 0x02000D3F RID: 3391
		public class SimpleReadStream : Stream
		{
			// Token: 0x06005649 RID: 22089 RVA: 0x001BDC45 File Offset: 0x001BBE45
			internal SimpleReadStream(MemoryMappedFileSpanWrapper mmf, long start, long length)
			{
				this._mmf = mmf;
				this._pos = start;
				this._length = length;
			}

			// Token: 0x0600564A RID: 22090 RVA: 0x001BDC64 File Offset: 0x001BBE64
			public override int Read(byte[] buffer, int offset, int count)
			{
				if (this._pos + (long)count >= this._mmf._length)
				{
					int num = (int)(this._mmf._length - this._pos);
					if (num <= 0)
					{
						return 0;
					}
					count = num;
				}
				this._mmf.GetSpan(this._pos, (long)count).CopyTo(buffer.AsSpan(offset, count));
				this._pos += (long)count;
				return count;
			}

			// Token: 0x0600564B RID: 22091 RVA: 0x001BDCDC File Offset: 0x001BBEDC
			public override int Read(Span<byte> buffer)
			{
				if (this._pos + (long)buffer.Length >= this._mmf._length)
				{
					int num = (int)(this._mmf._length - this._pos);
					if (num <= 0)
					{
						return 0;
					}
					Span<byte> span = buffer;
					buffer = span.Slice(0, num);
				}
				this._mmf.GetReadOnlySpan(this._pos, (long)buffer.Length).CopyTo(buffer);
				this._pos += (long)buffer.Length;
				return buffer.Length;
			}

			// Token: 0x0600564C RID: 22092 RVA: 0x001BDD6C File Offset: 0x001BBF6C
			public override int ReadByte()
			{
				if (this._pos >= this._length)
				{
					return -1;
				}
				int result = (int)this._mmf.Read<byte>(this._pos);
				this._pos += 1L;
				return result;
			}

			// Token: 0x0600564D RID: 22093 RVA: 0x001BDDA0 File Offset: 0x001BBFA0
			public override long Seek(long offset, SeekOrigin origin)
			{
				switch (origin)
				{
				case SeekOrigin.Begin:
					this._pos = offset;
					break;
				case SeekOrigin.Current:
					this._pos += offset;
					break;
				case SeekOrigin.End:
					this._pos = this._length + offset;
					break;
				default:
					throw new ArgumentOutOfRangeException("origin", origin, null);
				}
				return this._pos;
			}

			// Token: 0x0600564E RID: 22094 RVA: 0x00002894 File Offset: 0x00000A94
			public override void Flush()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0600564F RID: 22095 RVA: 0x00002894 File Offset: 0x00000A94
			public override void SetLength(long value)
			{
				throw new NotSupportedException();
			}

			// Token: 0x06005650 RID: 22096 RVA: 0x00002894 File Offset: 0x00000A94
			public override void Write(byte[] buffer, int offset, int count)
			{
				throw new NotSupportedException();
			}

			// Token: 0x17000D66 RID: 3430
			// (get) Token: 0x06005651 RID: 22097 RVA: 0x000028E7 File Offset: 0x00000AE7
			public override bool CanRead
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000D67 RID: 3431
			// (get) Token: 0x06005652 RID: 22098 RVA: 0x000028E7 File Offset: 0x00000AE7
			public override bool CanSeek
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000D68 RID: 3432
			// (get) Token: 0x06005653 RID: 22099 RVA: 0x00002501 File Offset: 0x00000701
			public override bool CanWrite
			{
				get
				{
					return false;
				}
			}

			// Token: 0x17000D69 RID: 3433
			// (get) Token: 0x06005654 RID: 22100 RVA: 0x001BDE02 File Offset: 0x001BC002
			public override long Length
			{
				get
				{
					return this._length;
				}
			}

			// Token: 0x17000D6A RID: 3434
			// (get) Token: 0x06005655 RID: 22101 RVA: 0x001BDE0A File Offset: 0x001BC00A
			// (set) Token: 0x06005656 RID: 22102 RVA: 0x001BDE12 File Offset: 0x001BC012
			public override long Position
			{
				get
				{
					return this._pos;
				}
				set
				{
					this._pos = value;
				}
			}

			// Token: 0x04003EDE RID: 16094
			private readonly MemoryMappedFileSpanWrapper _mmf;

			// Token: 0x04003EDF RID: 16095
			private long _pos;

			// Token: 0x04003EE0 RID: 16096
			private readonly long _length;
		}
	}
}
