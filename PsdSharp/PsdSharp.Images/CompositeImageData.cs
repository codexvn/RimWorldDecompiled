using System;
using System.Buffers.Binary;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using PsdSharp.Compression;
using PsdSharp.Parsing;

namespace PsdSharp.Images;

internal class CompositeImageData : ImageData
{
	[CompilerGenerated]
	private sealed class _003CGetChannels_003Ed__6 : IEnumerable<ChannelData>, IEnumerable, IEnumerator<ChannelData>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ChannelData _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompositeImageData _003C_003E4__this;

		private Stream _003CdataStream_003E5__2;

		private List<ChannelData>.Enumerator _003C_003E7__wrap2;

		private IEnumerator<ChannelData> _003C_003E7__wrap3;

		ChannelData IEnumerator<ChannelData>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetChannels_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -6:
			case -5:
			case -4:
			case 2:
			case 3:
				try
				{
					switch (num)
					{
					case -5:
					case 2:
						try
						{
						}
						finally
						{
							_003C_003Em__Finally3();
						}
						break;
					case -6:
					case 3:
						try
						{
						}
						finally
						{
							_003C_003Em__Finally4();
						}
						break;
					}
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003CdataStream_003E5__2 = null;
			_003C_003E7__wrap2 = default(List<ChannelData>.Enumerator);
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			bool result;
			try
			{
				int num = _003C_003E1__state;
				CompositeImageData compositeImageData = _003C_003E4__this;
				ChannelData current;
				ChannelData current3;
				switch (num)
				{
				default:
					result = false;
					goto end_IL_0000;
				case 0:
				{
					_003C_003E1__state = -1;
					if (compositeImageData._loadedChannels.Count > 0)
					{
						_003C_003E7__wrap2 = compositeImageData._loadedChannels.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0086;
					}
					if (compositeImageData._dataSource == null)
					{
						throw new InvalidOperationException("Cannot load channels when the ImageDataLoading option is set to 'Skip'.");
					}
					_003CdataStream_003E5__2 = compositeImageData._dataSource.GetData();
					_003C_003E1__state = -4;
					BigEndianReader bigEndianReader = new BigEndianReader(_003CdataStream_003E5__2);
					ImageCompression imageCompression = (ImageCompression)bigEndianReader.ReadUInt16();
					if (imageCompression == ImageCompression.Rle)
					{
						_003C_003E7__wrap3 = compositeImageData.StitchChannelsFromRleData(bigEndianReader).GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_0145;
					}
					byte[] data = ((imageCompression == ImageCompression.Raw) ? compositeImageData.StreamToByteArray(_003CdataStream_003E5__2) : compositeImageData.DecompressZipData(_003CdataStream_003E5__2, imageCompression));
					_003C_003E7__wrap3 = compositeImageData.SplitDataIntoChannels(data).GetEnumerator();
					_003C_003E1__state = -6;
					goto IL_01da;
				}
				case 1:
					_003C_003E1__state = -3;
					goto IL_0086;
				case 2:
					_003C_003E1__state = -5;
					goto IL_0145;
				case 3:
					{
						_003C_003E1__state = -6;
						goto IL_01da;
					}
					IL_0145:
					if (!_003C_003E7__wrap3.MoveNext())
					{
						_003C_003Em__Finally3();
						_003C_003E7__wrap3 = null;
						result = false;
						break;
					}
					current = _003C_003E7__wrap3.Current;
					compositeImageData._loadedChannels.Add(current);
					_003C_003E2__current = current;
					_003C_003E1__state = 2;
					result = true;
					goto end_IL_0000;
					IL_0086:
					if (_003C_003E7__wrap2.MoveNext())
					{
						ChannelData current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 1;
						result = true;
					}
					else
					{
						_003C_003Em__Finally1();
						_003C_003E7__wrap2 = default(List<ChannelData>.Enumerator);
						result = false;
					}
					goto end_IL_0000;
					IL_01da:
					if (!_003C_003E7__wrap3.MoveNext())
					{
						_003C_003Em__Finally4();
						_003C_003E7__wrap3 = null;
						result = false;
						break;
					}
					current3 = _003C_003E7__wrap3.Current;
					compositeImageData._loadedChannels.Add(current3);
					_003C_003E2__current = current3;
					_003C_003E1__state = 3;
					result = true;
					goto end_IL_0000;
				}
				_003C_003Em__Finally2();
				end_IL_0000:;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
			return result;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003CdataStream_003E5__2 != null)
			{
				((IDisposable)_003CdataStream_003E5__2).Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -4;
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -4;
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ChannelData> IEnumerable<ChannelData>.GetEnumerator()
		{
			_003CGetChannels_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetChannels_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ChannelData>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSplitDataIntoChannels_003Ed__19 : IEnumerable<ChannelData>, IEnumerable, IEnumerator<ChannelData>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ChannelData _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private byte[] data;

		public byte[] _003C_003E3__data;

		public CompositeImageData _003C_003E4__this;

		private int _003ClengthPerChannel_003E5__2;

		private Rectangle _003Cbounds_003E5__3;

		private short _003Ci_003E5__4;

		ChannelData IEnumerator<ChannelData>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CSplitDataIntoChannels_003Ed__19(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompositeImageData compositeImageData = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003ClengthPerChannel_003E5__2 = data.Length / compositeImageData._header.NumberOfChannels;
				_003Cbounds_003E5__3 = new Rectangle(new Point(0, 0), new Point((int)(compositeImageData._header.WidthInPixels - 1), (int)(compositeImageData._header.HeightInPixels - 1)));
				_003Ci_003E5__4 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__4++;
				break;
			}
			if (_003Ci_003E5__4 < compositeImageData._header.NumberOfChannels)
			{
				byte[] destinationArray = new byte[_003ClengthPerChannel_003E5__2];
				Array.Copy(data, _003Ci_003E5__4 * _003ClengthPerChannel_003E5__2, destinationArray, 0, _003ClengthPerChannel_003E5__2);
				ChannelData item = new ChannelData(_003Ci_003E5__4, ImageCompression.Raw, _003Cbounds_003E5__3, compositeImageData._header.ChannelDepth, destinationArray, compositeImageData._header.PsdFileType == PsdFileType.Psb);
				compositeImageData._loadedChannels.Add(item);
				_003C_003E2__current = item;
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ChannelData> IEnumerable<ChannelData>.GetEnumerator()
		{
			_003CSplitDataIntoChannels_003Ed__19 _003CSplitDataIntoChannels_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSplitDataIntoChannels_003Ed__ = this;
			}
			else
			{
				_003CSplitDataIntoChannels_003Ed__ = new _003CSplitDataIntoChannels_003Ed__19(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CSplitDataIntoChannels_003Ed__.data = _003C_003E3__data;
			return _003CSplitDataIntoChannels_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ChannelData>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CStitchChannelsFromRleData_003Ed__20 : IEnumerable<ChannelData>, IEnumerable, IEnumerator<ChannelData>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ChannelData _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompositeImageData _003C_003E4__this;

		private BigEndianReader reader;

		public BigEndianReader _003C_003E3__reader;

		private bool _003CisPsb_003E5__2;

		private Dictionary<short, List<int>> _003CrowCounts_003E5__3;

		private Rectangle _003Cbounds_003E5__4;

		private short _003Ci_003E5__5;

		ChannelData IEnumerator<ChannelData>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CStitchChannelsFromRleData_003Ed__20(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CrowCounts_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompositeImageData compositeImageData = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003CisPsb_003E5__2 = compositeImageData._header.PsdFileType == PsdFileType.Psb;
				_003CrowCounts_003E5__3 = new Dictionary<short, List<int>>();
				for (short num2 = 0; num2 < compositeImageData._header.NumberOfChannels; num2++)
				{
					_003CrowCounts_003E5__3.Add(num2, new List<int>());
					for (int i = 0; i < compositeImageData._header.HeightInPixels; i++)
					{
						if (_003CisPsb_003E5__2)
						{
							_003CrowCounts_003E5__3[num2].Add((int)reader.ReadUInt32());
						}
						else
						{
							_003CrowCounts_003E5__3[num2].Add(reader.ReadUInt16());
						}
					}
				}
				_003Cbounds_003E5__4 = new Rectangle(new Point(0, 0), new Point((int)(compositeImageData._header.WidthInPixels - 1), (int)(compositeImageData._header.HeightInPixels - 1)));
				_003Ci_003E5__5 = 0;
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__5++;
				break;
			}
			if (_003Ci_003E5__5 < compositeImageData._header.NumberOfChannels)
			{
				List<int> list = _003CrowCounts_003E5__3[_003Ci_003E5__5];
				int num3 = list.Sum();
				byte[] array = new byte[num3];
				reader.ReadIntoBuffer(array);
				int num4 = (_003CisPsb_003E5__2 ? 4 : 2);
				byte[] array2 = new byte[compositeImageData._header.HeightInPixels * num4 + num3];
				for (int j = 0; j < compositeImageData._header.HeightInPixels; j++)
				{
					if (_003CisPsb_003E5__2)
					{
						BinaryPrimitives.WriteUInt32BigEndian(array2.AsSpan(j * num4), (uint)list[j]);
					}
					else
					{
						BinaryPrimitives.WriteUInt16BigEndian(array2.AsSpan(j * num4), (ushort)list[j]);
					}
				}
				Array.Copy(array, 0L, array2, compositeImageData._header.HeightInPixels * num4, num3);
				ChannelData item = new ChannelData(_003Ci_003E5__5, ImageCompression.Rle, _003Cbounds_003E5__4, compositeImageData._header.ChannelDepth, array2, _003CisPsb_003E5__2);
				compositeImageData._loadedChannels.Add(item);
				_003C_003E2__current = item;
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ChannelData> IEnumerable<ChannelData>.GetEnumerator()
		{
			_003CStitchChannelsFromRleData_003Ed__20 _003CStitchChannelsFromRleData_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CStitchChannelsFromRleData_003Ed__ = this;
			}
			else
			{
				_003CStitchChannelsFromRleData_003Ed__ = new _003CStitchChannelsFromRleData_003Ed__20(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CStitchChannelsFromRleData_003Ed__.reader = _003C_003E3__reader;
			return _003CStitchChannelsFromRleData_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ChannelData>)this).GetEnumerator();
		}
	}

	private readonly PsdHeader _header;

	private readonly ImageDataSource? _dataSource;

	private readonly List<ChannelData> _loadedChannels = new List<ChannelData>();

	public override ushort NumberOfChannels => _header.NumberOfChannels;

	public override uint Width => _header.WidthInPixels;

	public override uint Height => _header.HeightInPixels;

	public override ColorMode ColorMode => _header.ColorMode;

	public override byte[] ColorModeData => _header.ColorModeData;

	public override ushort ChannelDepth => _header.ChannelDepth;

	public CompositeImageData(ParseContext ctx, PsdHeader header)
		: base(ctx.PsdLoadOptions.ImageDataLoading)
	{
		_header = header;
		switch (ImageDataLoading)
		{
		case ImageDataLoading.Skip:
			return;
		case ImageDataLoading.Auto:
		{
			long num = (long)header.NumberOfChannels * (long)header.WidthInPixels * header.HeightInPixels * header.ChannelDepth / 8;
			ImageDataLoading = ((num <= ctx.PsdLoadOptions.AutoImageLoadingDiskCacheThresholdBytes) ? ImageDataLoading.LoadImmediately : ImageDataLoading.CacheOnDisk);
			break;
		}
		}
		switch (ImageDataLoading)
		{
		case ImageDataLoading.LoadOnDemand:
			_dataSource = new SeekableStreamSource(ctx.Reader, null);
			break;
		case ImageDataLoading.CacheOnDisk:
			_dataSource = new TempFileSource(ctx.Reader, null);
			break;
		case ImageDataLoading.LoadImmediately:
			_dataSource = new BufferedSource(ctx.Reader, null);
			break;
		}
	}

	[IteratorStateMachine(typeof(_003CGetChannels_003Ed__6))]
	public override IEnumerable<ChannelData> GetChannels()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetChannels_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}

	private byte[] StreamToByteArray(Stream stream)
	{
		MemoryStream memoryStream = new MemoryStream();
		stream.CopyTo(memoryStream);
		return memoryStream.ToArray();
	}

	private byte[] DecompressZipData(Stream stream, ImageCompression compression)
	{
		return PsdSharp.Compression.Compression.Decompress(stream, compression, _header.WidthInPixels, _header.HeightInPixels, _header.ChannelDepth, _header.PsdFileType == PsdFileType.Psb);
	}

	[IteratorStateMachine(typeof(_003CSplitDataIntoChannels_003Ed__19))]
	private IEnumerable<ChannelData> SplitDataIntoChannels(byte[] data)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSplitDataIntoChannels_003Ed__19(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__data = data
		};
	}

	[IteratorStateMachine(typeof(_003CStitchChannelsFromRleData_003Ed__20))]
	private IEnumerable<ChannelData> StitchChannelsFromRleData(BigEndianReader reader)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CStitchChannelsFromRleData_003Ed__20(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__reader = reader
		};
	}
}
