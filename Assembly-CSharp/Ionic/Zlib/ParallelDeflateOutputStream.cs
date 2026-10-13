using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Ionic.Crc;

namespace Ionic.Zlib
{
	// Token: 0x02003EB8 RID: 16056
	public class ParallelDeflateOutputStream : Stream
	{
		// Token: 0x06016059 RID: 90201 RVA: 0x00697B6E File Offset: 0x00695D6E
		public ParallelDeflateOutputStream(Stream stream) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x0601605A RID: 90202 RVA: 0x00697B7A File Offset: 0x00695D7A
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level) : this(stream, level, CompressionStrategy.Default, false)
		{
		}

		// Token: 0x0601605B RID: 90203 RVA: 0x00697B86 File Offset: 0x00695D86
		public ParallelDeflateOutputStream(Stream stream, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x0601605C RID: 90204 RVA: 0x00697B92 File Offset: 0x00695D92
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
		{
		}

		// Token: 0x0601605D RID: 90205 RVA: 0x00697BA0 File Offset: 0x00695DA0
		public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, CompressionStrategy strategy, bool leaveOpen)
		{
			this._outStream = stream;
			this._compressLevel = level;
			this.Strategy = strategy;
			this._leaveOpen = leaveOpen;
			this.MaxBufferPairs = 16;
		}

		// Token: 0x17003726 RID: 14118
		// (get) Token: 0x0601605E RID: 90206 RVA: 0x00697C0F File Offset: 0x00695E0F
		// (set) Token: 0x0601605F RID: 90207 RVA: 0x00697C17 File Offset: 0x00695E17
		public CompressionStrategy Strategy { get; private set; }

		// Token: 0x17003727 RID: 14119
		// (get) Token: 0x06016060 RID: 90208 RVA: 0x00697C20 File Offset: 0x00695E20
		// (set) Token: 0x06016061 RID: 90209 RVA: 0x00697C28 File Offset: 0x00695E28
		public int MaxBufferPairs
		{
			get
			{
				return this._maxBufferPairs;
			}
			set
			{
				if (value < 4)
				{
					throw new ArgumentException("MaxBufferPairs", "Value must be 4 or greater.");
				}
				this._maxBufferPairs = value;
			}
		}

		// Token: 0x17003728 RID: 14120
		// (get) Token: 0x06016062 RID: 90210 RVA: 0x00697C45 File Offset: 0x00695E45
		// (set) Token: 0x06016063 RID: 90211 RVA: 0x00697C4D File Offset: 0x00695E4D
		public int BufferSize
		{
			get
			{
				return this._bufferSize;
			}
			set
			{
				if (value < 1024)
				{
					throw new ArgumentOutOfRangeException("BufferSize", "BufferSize must be greater than 1024 bytes");
				}
				this._bufferSize = value;
			}
		}

		// Token: 0x17003729 RID: 14121
		// (get) Token: 0x06016064 RID: 90212 RVA: 0x00697C6E File Offset: 0x00695E6E
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x1700372A RID: 14122
		// (get) Token: 0x06016065 RID: 90213 RVA: 0x00697C76 File Offset: 0x00695E76
		public long BytesProcessed
		{
			get
			{
				return this._totalBytesProcessed;
			}
		}

		// Token: 0x06016066 RID: 90214 RVA: 0x00697C80 File Offset: 0x00695E80
		private void _InitializePoolOfWorkItems()
		{
			this._toWrite = new Queue<int>();
			this._toFill = new Queue<int>();
			this._pool = new List<WorkItem>();
			int num = ParallelDeflateOutputStream.BufferPairsPerCore * Environment.ProcessorCount;
			num = Math.Min(num, this._maxBufferPairs);
			for (int i = 0; i < num; i++)
			{
				this._pool.Add(new WorkItem(this._bufferSize, this._compressLevel, this.Strategy, i));
				this._toFill.Enqueue(i);
			}
			this._newlyCompressedBlob = new AutoResetEvent(false);
			this._runningCrc = new CRC32();
			this._currentlyFilling = -1;
			this._lastFilled = -1;
			this._lastWritten = -1;
			this._latestCompressed = -1;
		}

		// Token: 0x06016067 RID: 90215 RVA: 0x00697D38 File Offset: 0x00695F38
		public override void Write(byte[] buffer, int offset, int count)
		{
			bool mustWait = false;
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (count == 0)
			{
				return;
			}
			if (!this._firstWriteDone)
			{
				this._InitializePoolOfWorkItems();
				this._firstWriteDone = true;
			}
			for (;;)
			{
				this.EmitPendingBuffers(false, mustWait);
				mustWait = false;
				int num;
				if (this._currentlyFilling >= 0)
				{
					num = this._currentlyFilling;
					goto IL_98;
				}
				if (this._toFill.Count != 0)
				{
					num = this._toFill.Dequeue();
					this._lastFilled++;
					goto IL_98;
				}
				mustWait = true;
				IL_145:
				if (count <= 0)
				{
					return;
				}
				continue;
				IL_98:
				WorkItem workItem = this._pool[num];
				int num2 = (workItem.buffer.Length - workItem.inputBytesAvailable > count) ? count : (workItem.buffer.Length - workItem.inputBytesAvailable);
				workItem.ordinal = this._lastFilled;
				Buffer.BlockCopy(buffer, offset, workItem.buffer, workItem.inputBytesAvailable, num2);
				count -= num2;
				offset += num2;
				workItem.inputBytesAvailable += num2;
				if (workItem.inputBytesAvailable == workItem.buffer.Length)
				{
					if (!ThreadPool.QueueUserWorkItem(new WaitCallback(this._DeflateOne), workItem))
					{
						break;
					}
					this._currentlyFilling = -1;
				}
				else
				{
					this._currentlyFilling = num;
				}
				goto IL_145;
			}
			throw new Exception("Cannot enqueue workitem");
		}

		// Token: 0x06016068 RID: 90216 RVA: 0x00697E94 File Offset: 0x00696094
		private void _FlushFinish()
		{
			byte[] array = new byte[128];
			ZlibCodec zlibCodec = new ZlibCodec();
			int num = zlibCodec.InitializeDeflate(this._compressLevel, false);
			zlibCodec.InputBuffer = null;
			zlibCodec.NextIn = 0;
			zlibCodec.AvailableBytesIn = 0;
			zlibCodec.OutputBuffer = array;
			zlibCodec.NextOut = 0;
			zlibCodec.AvailableBytesOut = array.Length;
			num = zlibCodec.Deflate(FlushType.Finish);
			if (num != 1 && num != 0)
			{
				throw new Exception("deflating: " + zlibCodec.Message);
			}
			if (array.Length - zlibCodec.AvailableBytesOut > 0)
			{
				this._outStream.Write(array, 0, array.Length - zlibCodec.AvailableBytesOut);
			}
			zlibCodec.EndDeflate();
			this._Crc32 = this._runningCrc.Crc32Result;
		}

		// Token: 0x06016069 RID: 90217 RVA: 0x00697F50 File Offset: 0x00696150
		private void _Flush(bool lastInput)
		{
			if (this._isClosed)
			{
				throw new InvalidOperationException();
			}
			if (this.emitting)
			{
				return;
			}
			if (this._currentlyFilling >= 0)
			{
				WorkItem wi = this._pool[this._currentlyFilling];
				this._DeflateOne(wi);
				this._currentlyFilling = -1;
			}
			if (lastInput)
			{
				this.EmitPendingBuffers(true, false);
				this._FlushFinish();
				return;
			}
			this.EmitPendingBuffers(false, false);
		}

		// Token: 0x0601606A RID: 90218 RVA: 0x00697FB7 File Offset: 0x006961B7
		public override void Flush()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (this._handlingException)
			{
				return;
			}
			this._Flush(false);
		}

		// Token: 0x0601606B RID: 90219 RVA: 0x00697FEC File Offset: 0x006961EC
		public override void Close()
		{
			if (this._pendingException != null)
			{
				this._handlingException = true;
				Exception pendingException = this._pendingException;
				this._pendingException = null;
				throw pendingException;
			}
			if (this._handlingException)
			{
				return;
			}
			if (this._isClosed)
			{
				return;
			}
			this._Flush(true);
			if (!this._leaveOpen)
			{
				this._outStream.Close();
			}
			this._isClosed = true;
		}

		// Token: 0x0601606C RID: 90220 RVA: 0x0069804F File Offset: 0x0069624F
		public new void Dispose()
		{
			this.Close();
			this._pool = null;
			this.Dispose(true);
		}

		// Token: 0x0601606D RID: 90221 RVA: 0x00698065 File Offset: 0x00696265
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		// Token: 0x0601606E RID: 90222 RVA: 0x00698070 File Offset: 0x00696270
		public void Reset(Stream stream)
		{
			if (!this._firstWriteDone)
			{
				return;
			}
			this._toWrite.Clear();
			this._toFill.Clear();
			foreach (WorkItem workItem in this._pool)
			{
				this._toFill.Enqueue(workItem.index);
				workItem.ordinal = -1;
			}
			this._firstWriteDone = false;
			this._totalBytesProcessed = 0L;
			this._runningCrc = new CRC32();
			this._isClosed = false;
			this._currentlyFilling = -1;
			this._lastFilled = -1;
			this._lastWritten = -1;
			this._latestCompressed = -1;
			this._outStream = stream;
		}

		// Token: 0x0601606F RID: 90223 RVA: 0x00698138 File Offset: 0x00696338
		private void EmitPendingBuffers(bool doAll, bool mustWait)
		{
			if (this.emitting)
			{
				return;
			}
			this.emitting = true;
			if (doAll || mustWait)
			{
				this._newlyCompressedBlob.WaitOne();
			}
			do
			{
				int num = -1;
				int num2 = doAll ? 200 : (mustWait ? -1 : 0);
				int num3 = -1;
				do
				{
					if (Monitor.TryEnter(this._toWrite, num2))
					{
						num3 = -1;
						try
						{
							if (this._toWrite.Count > 0)
							{
								num3 = this._toWrite.Dequeue();
							}
						}
						finally
						{
							Monitor.Exit(this._toWrite);
						}
						if (num3 >= 0)
						{
							WorkItem workItem = this._pool[num3];
							if (workItem.ordinal != this._lastWritten + 1)
							{
								Queue<int> toWrite = this._toWrite;
								lock (toWrite)
								{
									this._toWrite.Enqueue(num3);
								}
								if (num == num3)
								{
									this._newlyCompressedBlob.WaitOne();
									num = -1;
								}
								else if (num == -1)
								{
									num = num3;
								}
							}
							else
							{
								num = -1;
								this._outStream.Write(workItem.compressed, 0, workItem.compressedBytesAvailable);
								this._runningCrc.Combine(workItem.crc, workItem.inputBytesAvailable);
								this._totalBytesProcessed += (long)workItem.inputBytesAvailable;
								workItem.inputBytesAvailable = 0;
								this._lastWritten = workItem.ordinal;
								this._toFill.Enqueue(workItem.index);
								if (num2 == -1)
								{
									num2 = 0;
								}
							}
						}
					}
					else
					{
						num3 = -1;
					}
				}
				while (num3 >= 0);
			}
			while (doAll && this._lastWritten != this._latestCompressed);
			this.emitting = false;
		}

		// Token: 0x06016070 RID: 90224 RVA: 0x006982D8 File Offset: 0x006964D8
		private void _DeflateOne(object wi)
		{
			WorkItem workItem = (WorkItem)wi;
			try
			{
				CRC32 crc = new CRC32();
				crc.SlurpBlock(workItem.buffer, 0, workItem.inputBytesAvailable);
				this.DeflateOneSegment(workItem);
				workItem.crc = crc.Crc32Result;
				object obj = this._latestLock;
				lock (obj)
				{
					if (workItem.ordinal > this._latestCompressed)
					{
						this._latestCompressed = workItem.ordinal;
					}
				}
				Queue<int> toWrite = this._toWrite;
				lock (toWrite)
				{
					this._toWrite.Enqueue(workItem.index);
				}
				this._newlyCompressedBlob.Set();
			}
			catch (Exception pendingException)
			{
				object obj = this._eLock;
				lock (obj)
				{
					if (this._pendingException != null)
					{
						this._pendingException = pendingException;
					}
				}
			}
		}

		// Token: 0x06016071 RID: 90225 RVA: 0x006983F8 File Offset: 0x006965F8
		private bool DeflateOneSegment(WorkItem workitem)
		{
			ZlibCodec compressor = workitem.compressor;
			compressor.ResetDeflate();
			compressor.NextIn = 0;
			compressor.AvailableBytesIn = workitem.inputBytesAvailable;
			compressor.NextOut = 0;
			compressor.AvailableBytesOut = workitem.compressed.Length;
			do
			{
				compressor.Deflate(FlushType.None);
			}
			while (compressor.AvailableBytesIn > 0 || compressor.AvailableBytesOut == 0);
			compressor.Deflate(FlushType.Sync);
			workitem.compressedBytesAvailable = (int)compressor.TotalBytesOut;
			return true;
		}

		// Token: 0x06016072 RID: 90226 RVA: 0x0069846C File Offset: 0x0069666C
		[Conditional("Trace")]
		private void TraceOutput(ParallelDeflateOutputStream.TraceBits bits, string format, params object[] varParams)
		{
			if ((bits & this._DesiredTrace) != ParallelDeflateOutputStream.TraceBits.None)
			{
				object outputLock = this._outputLock;
				lock (outputLock)
				{
					int hashCode = Thread.CurrentThread.GetHashCode();
					Console.ForegroundColor = hashCode % 8 + ConsoleColor.DarkGray;
					Console.Write("{0:000} PDOS ", hashCode);
					Console.WriteLine(format, varParams);
					Console.ResetColor();
				}
			}
		}

		// Token: 0x1700372B RID: 14123
		// (get) Token: 0x06016073 RID: 90227 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700372C RID: 14124
		// (get) Token: 0x06016074 RID: 90228 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700372D RID: 14125
		// (get) Token: 0x06016075 RID: 90229 RVA: 0x006984E4 File Offset: 0x006966E4
		public override bool CanWrite
		{
			get
			{
				return this._outStream.CanWrite;
			}
		}

		// Token: 0x1700372E RID: 14126
		// (get) Token: 0x06016076 RID: 90230 RVA: 0x00002894 File Offset: 0x00000A94
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x1700372F RID: 14127
		// (get) Token: 0x06016077 RID: 90231 RVA: 0x006984F1 File Offset: 0x006966F1
		// (set) Token: 0x06016078 RID: 90232 RVA: 0x00002894 File Offset: 0x00000A94
		public override long Position
		{
			get
			{
				return this._outStream.Position;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06016079 RID: 90233 RVA: 0x00002894 File Offset: 0x00000A94
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0601607A RID: 90234 RVA: 0x00002894 File Offset: 0x00000A94
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0601607B RID: 90235 RVA: 0x00002894 File Offset: 0x00000A94
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400F45D RID: 62557
		private static readonly int IO_BUFFER_SIZE_DEFAULT = 65536;

		// Token: 0x0400F45E RID: 62558
		private static readonly int BufferPairsPerCore = 4;

		// Token: 0x0400F45F RID: 62559
		private List<WorkItem> _pool;

		// Token: 0x0400F460 RID: 62560
		private bool _leaveOpen;

		// Token: 0x0400F461 RID: 62561
		private bool emitting;

		// Token: 0x0400F462 RID: 62562
		private Stream _outStream;

		// Token: 0x0400F463 RID: 62563
		private int _maxBufferPairs;

		// Token: 0x0400F464 RID: 62564
		private int _bufferSize = ParallelDeflateOutputStream.IO_BUFFER_SIZE_DEFAULT;

		// Token: 0x0400F465 RID: 62565
		private AutoResetEvent _newlyCompressedBlob;

		// Token: 0x0400F466 RID: 62566
		private object _outputLock = new object();

		// Token: 0x0400F467 RID: 62567
		private bool _isClosed;

		// Token: 0x0400F468 RID: 62568
		private bool _firstWriteDone;

		// Token: 0x0400F469 RID: 62569
		private int _currentlyFilling;

		// Token: 0x0400F46A RID: 62570
		private int _lastFilled;

		// Token: 0x0400F46B RID: 62571
		private int _lastWritten;

		// Token: 0x0400F46C RID: 62572
		private int _latestCompressed;

		// Token: 0x0400F46D RID: 62573
		private int _Crc32;

		// Token: 0x0400F46E RID: 62574
		private CRC32 _runningCrc;

		// Token: 0x0400F46F RID: 62575
		private object _latestLock = new object();

		// Token: 0x0400F470 RID: 62576
		private Queue<int> _toWrite;

		// Token: 0x0400F471 RID: 62577
		private Queue<int> _toFill;

		// Token: 0x0400F472 RID: 62578
		private long _totalBytesProcessed;

		// Token: 0x0400F473 RID: 62579
		private CompressionLevel _compressLevel;

		// Token: 0x0400F474 RID: 62580
		private volatile Exception _pendingException;

		// Token: 0x0400F475 RID: 62581
		private bool _handlingException;

		// Token: 0x0400F476 RID: 62582
		private object _eLock = new object();

		// Token: 0x0400F477 RID: 62583
		private ParallelDeflateOutputStream.TraceBits _DesiredTrace = ParallelDeflateOutputStream.TraceBits.EmitLock | ParallelDeflateOutputStream.TraceBits.EmitEnter | ParallelDeflateOutputStream.TraceBits.EmitBegin | ParallelDeflateOutputStream.TraceBits.EmitDone | ParallelDeflateOutputStream.TraceBits.EmitSkip | ParallelDeflateOutputStream.TraceBits.Session | ParallelDeflateOutputStream.TraceBits.Compress | ParallelDeflateOutputStream.TraceBits.WriteEnter | ParallelDeflateOutputStream.TraceBits.WriteTake;

		// Token: 0x02003EB9 RID: 16057
		[Flags]
		private enum TraceBits : uint
		{
			// Token: 0x0400F47A RID: 62586
			None = 0U,
			// Token: 0x0400F47B RID: 62587
			NotUsed1 = 1U,
			// Token: 0x0400F47C RID: 62588
			EmitLock = 2U,
			// Token: 0x0400F47D RID: 62589
			EmitEnter = 4U,
			// Token: 0x0400F47E RID: 62590
			EmitBegin = 8U,
			// Token: 0x0400F47F RID: 62591
			EmitDone = 16U,
			// Token: 0x0400F480 RID: 62592
			EmitSkip = 32U,
			// Token: 0x0400F481 RID: 62593
			EmitAll = 58U,
			// Token: 0x0400F482 RID: 62594
			Flush = 64U,
			// Token: 0x0400F483 RID: 62595
			Lifecycle = 128U,
			// Token: 0x0400F484 RID: 62596
			Session = 256U,
			// Token: 0x0400F485 RID: 62597
			Synch = 512U,
			// Token: 0x0400F486 RID: 62598
			Instance = 1024U,
			// Token: 0x0400F487 RID: 62599
			Compress = 2048U,
			// Token: 0x0400F488 RID: 62600
			Write = 4096U,
			// Token: 0x0400F489 RID: 62601
			WriteEnter = 8192U,
			// Token: 0x0400F48A RID: 62602
			WriteTake = 16384U,
			// Token: 0x0400F48B RID: 62603
			All = 4294967295U
		}
	}
}
