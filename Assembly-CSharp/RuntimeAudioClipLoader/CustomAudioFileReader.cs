using System;
using System.IO;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NVorbis.NAudioSupport;
using UnityEngine;

namespace RuntimeAudioClipLoader
{
	// Token: 0x0200005D RID: 93
	internal class CustomAudioFileReader : WaveStream, ISampleProvider
	{
		// Token: 0x060001EE RID: 494 RVA: 0x0000B504 File Offset: 0x00009704
		public CustomAudioFileReader(Stream stream, AudioFormat format)
		{
			this.lockObject = new object();
			this.CreateReaderStream(stream, format);
			this.sourceBytesPerSample = this.readerStream.WaveFormat.BitsPerSample / 8 * this.readerStream.WaveFormat.Channels;
			this.sampleChannel = new SampleChannel(this.readerStream, false);
			this.destBytesPerSample = 4 * this.sampleChannel.WaveFormat.Channels;
			this.length = this.SourceToDest(this.readerStream.Length);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000B594 File Offset: 0x00009794
		private void CreateReaderStream(Stream stream, AudioFormat format)
		{
			if (format == AudioFormat.wav)
			{
				this.readerStream = new WaveFileReader(stream);
				if (this.readerStream.WaveFormat.Encoding != WaveFormatEncoding.Pcm && this.readerStream.WaveFormat.Encoding != WaveFormatEncoding.IeeeFloat)
				{
					this.readerStream = WaveFormatConversionStream.CreatePcmStream(this.readerStream);
					this.readerStream = new BlockAlignReductionStream(this.readerStream);
					return;
				}
			}
			else
			{
				if (format == AudioFormat.mp3)
				{
					this.readerStream = new Mp3FileReader(stream);
					return;
				}
				if (format == AudioFormat.aiff)
				{
					this.readerStream = new AiffFileReader(stream);
					return;
				}
				if (format == AudioFormat.ogg)
				{
					this.readerStream = new VorbisWaveReader(stream);
					return;
				}
				Debug.LogWarning(string.Format("Audio format {0} is not supported", format));
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000B641 File Offset: 0x00009841
		public override WaveFormat WaveFormat
		{
			get
			{
				return this.sampleChannel.WaveFormat;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000B64E File Offset: 0x0000984E
		public override long Length
		{
			get
			{
				return this.length;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000B656 File Offset: 0x00009856
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x0000B66C File Offset: 0x0000986C
		public override long Position
		{
			get
			{
				return this.SourceToDest(this.readerStream.Position);
			}
			set
			{
				object obj = this.lockObject;
				lock (obj)
				{
					this.readerStream.Position = this.DestToSource(value);
				}
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000B6B8 File Offset: 0x000098B8
		public override int Read(byte[] buffer, int offset, int count)
		{
			WaveBuffer waveBuffer = new WaveBuffer(buffer);
			int count2 = count / 4;
			return this.Read(waveBuffer.FloatBuffer, offset / 4, count2) * 4;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000B6E4 File Offset: 0x000098E4
		public int Read(float[] buffer, int offset, int count)
		{
			object obj = this.lockObject;
			int result;
			lock (obj)
			{
				result = this.sampleChannel.Read(buffer, offset, count);
			}
			return result;
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000B730 File Offset: 0x00009930
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x0000B73D File Offset: 0x0000993D
		public float Volume
		{
			get
			{
				return this.sampleChannel.Volume;
			}
			set
			{
				this.sampleChannel.Volume = value;
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000B74B File Offset: 0x0000994B
		private long SourceToDest(long sourceBytes)
		{
			return (long)this.destBytesPerSample * (sourceBytes / (long)this.sourceBytesPerSample);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000B75E File Offset: 0x0000995E
		private long DestToSource(long destBytes)
		{
			return (long)this.sourceBytesPerSample * (destBytes / (long)this.destBytesPerSample);
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000B771 File Offset: 0x00009971
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.readerStream != null)
			{
				this.readerStream.Dispose();
				this.readerStream = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x040000F0 RID: 240
		private WaveStream readerStream;

		// Token: 0x040000F1 RID: 241
		private readonly SampleChannel sampleChannel;

		// Token: 0x040000F2 RID: 242
		private readonly int destBytesPerSample;

		// Token: 0x040000F3 RID: 243
		private readonly int sourceBytesPerSample;

		// Token: 0x040000F4 RID: 244
		private readonly long length;

		// Token: 0x040000F5 RID: 245
		private readonly object lockObject;
	}
}
