using System;
using System.IO;
using NAudio.Wave;

namespace NVorbis.NAudioSupport
{
	// Token: 0x0200005B RID: 91
	internal class VorbisWaveReader : WaveStream, IDisposable, ISampleProvider, IWaveProvider
	{
		// Token: 0x060001D6 RID: 470 RVA: 0x0000B1CA File Offset: 0x000093CA
		public VorbisWaveReader(string fileName)
		{
			this._reader = new VorbisReader(fileName);
			this._waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this._reader.SampleRate, this._reader.Channels);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000B1FF File Offset: 0x000093FF
		public VorbisWaveReader(Stream sourceStream)
		{
			this._reader = new VorbisReader(sourceStream, false);
			this._waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(this._reader.SampleRate, this._reader.Channels);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000B235 File Offset: 0x00009435
		protected override void Dispose(bool disposing)
		{
			if (disposing && this._reader != null)
			{
				this._reader.Dispose();
				this._reader = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000B25B File Offset: 0x0000945B
		public override WaveFormat WaveFormat
		{
			get
			{
				return this._waveFormat;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0000B264 File Offset: 0x00009464
		public override long Length
		{
			get
			{
				return (long)(this._reader.TotalTime.TotalSeconds * (double)this._waveFormat.SampleRate * (double)this._waveFormat.Channels * 4.0);
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001DB RID: 475 RVA: 0x0000B2AC File Offset: 0x000094AC
		// (set) Token: 0x060001DC RID: 476 RVA: 0x0000B2F4 File Offset: 0x000094F4
		public override long Position
		{
			get
			{
				return (long)(this._reader.DecodedTime.TotalMilliseconds * (double)this._reader.SampleRate * (double)this._reader.Channels * 4.0);
			}
			set
			{
				if (value < 0L || value > this.Length)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._reader.DecodedTime = TimeSpan.FromSeconds((double)value / (double)this._reader.SampleRate / (double)this._reader.Channels / 4.0);
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000B350 File Offset: 0x00009550
		public override int Read(byte[] buffer, int offset, int count)
		{
			count /= 4;
			count -= count % this._reader.Channels;
			float[] array;
			if ((array = VorbisWaveReader._conversionBuffer) == null)
			{
				array = (VorbisWaveReader._conversionBuffer = new float[count]);
			}
			float[] array2 = array;
			if (array2.Length < count)
			{
				array2 = (VorbisWaveReader._conversionBuffer = new float[count]);
			}
			int num = this.Read(array2, 0, count) * 4;
			Buffer.BlockCopy(array2, 0, buffer, offset, num);
			return num;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000B3B3 File Offset: 0x000095B3
		public int Read(float[] buffer, int offset, int count)
		{
			return this._reader.ReadSamples(buffer, offset, count);
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0000B3C3 File Offset: 0x000095C3
		public bool IsParameterChange
		{
			get
			{
				return this._reader.IsParameterChange;
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000B3D0 File Offset: 0x000095D0
		public void ClearParameterChange()
		{
			this._reader.ClearParameterChange();
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000B3DD File Offset: 0x000095DD
		public int StreamCount
		{
			get
			{
				return this._reader.StreamCount;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000B3EA File Offset: 0x000095EA
		// (set) Token: 0x060001E3 RID: 483 RVA: 0x0000B3F2 File Offset: 0x000095F2
		public int? NextStreamIndex { get; set; }

		// Token: 0x060001E4 RID: 484 RVA: 0x0000B3FC File Offset: 0x000095FC
		public bool GetNextStreamIndex()
		{
			if (this.NextStreamIndex == null)
			{
				int streamCount = this._reader.StreamCount;
				if (this._reader.FindNextStream())
				{
					this.NextStreamIndex = new int?(streamCount);
					return true;
				}
			}
			return false;
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000B441 File Offset: 0x00009641
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x0000B450 File Offset: 0x00009650
		public int CurrentStream
		{
			get
			{
				return this._reader.StreamIndex;
			}
			set
			{
				if (!this._reader.SwitchStreams(value))
				{
					throw new InvalidDataException("The selected stream is not a valid Vorbis stream!");
				}
				if (this.NextStreamIndex != null && value == this.NextStreamIndex.Value)
				{
					this.NextStreamIndex = null;
				}
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000B4A6 File Offset: 0x000096A6
		public int UpperBitrate
		{
			get
			{
				return this._reader.UpperBitrate;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000B4B3 File Offset: 0x000096B3
		public int NominalBitrate
		{
			get
			{
				return this._reader.NominalBitrate;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000B4C0 File Offset: 0x000096C0
		public int LowerBitrate
		{
			get
			{
				return this._reader.LowerBitrate;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000B4CD File Offset: 0x000096CD
		public string Vendor
		{
			get
			{
				return this._reader.Vendor;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000B4DA File Offset: 0x000096DA
		public string[] Comments
		{
			get
			{
				return this._reader.Comments;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000B4E7 File Offset: 0x000096E7
		public long ContainerOverheadBits
		{
			get
			{
				return this._reader.ContainerOverheadBits;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000B4F4 File Offset: 0x000096F4
		public IVorbisStreamStatus[] Stats
		{
			get
			{
				return this._reader.Stats;
			}
		}

		// Token: 0x040000E6 RID: 230
		private VorbisReader _reader;

		// Token: 0x040000E7 RID: 231
		private WaveFormat _waveFormat;

		// Token: 0x040000E8 RID: 232
		[ThreadStatic]
		private static float[] _conversionBuffer;
	}
}
