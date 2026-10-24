using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using PsdSharp.Parsing;

namespace PsdSharp.Images;

internal class PerChannelImageData : ImageData
{
	[CompilerGenerated]
	private sealed class _003CGetChannels_003Ed__9 : IEnumerable<ChannelData>, IEnumerable, IEnumerator<ChannelData>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ChannelData _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PerChannelImageData _003C_003E4__this;

		private Stream _003CdataStream_003E5__2;

		private BigEndianReader _003Creader_003E5__3;

		private IEnumerator<ChannelData> _003C_003E7__wrap3;

		private (short ChannelId, long DataLength)[] _003C_003E7__wrap4;

		private int _003C_003E7__wrap5;

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
		public _003CGetChannels_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
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
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003CdataStream_003E5__2 = null;
			_003Creader_003E5__3 = null;
			_003C_003E7__wrap3 = null;
			_003C_003E7__wrap4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			bool result;
			try
			{
				int num = _003C_003E1__state;
				PerChannelImageData perChannelImageData = _003C_003E4__this;
				switch (num)
				{
				default:
					result = false;
					break;
				case 0:
					_003C_003E1__state = -1;
					if (perChannelImageData._loadedChannels.Count > 0)
					{
						_003C_003E7__wrap3 = perChannelImageData._loadedChannels.Where((ChannelData x) => x.ChannelId >= 0).GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_00a7;
					}
					if (perChannelImageData._dataSource == null)
					{
						throw new InvalidOperationException("Cannot load channels when the ImageDataLoading option is set to 'Skip'.");
					}
					_003CdataStream_003E5__2 = perChannelImageData._dataSource.GetData();
					_003C_003E1__state = -4;
					_003Creader_003E5__3 = new BigEndianReader(_003CdataStream_003E5__2);
					_003C_003E7__wrap4 = perChannelImageData._channels;
					_003C_003E7__wrap5 = 0;
					goto IL_01e3;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00a7;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_01d5;
					}
					IL_01e3:
					if (_003C_003E7__wrap5 < _003C_003E7__wrap4.Length)
					{
						(short ChannelId, long DataLength) tuple = _003C_003E7__wrap4[_003C_003E7__wrap5];
						ImageCompression imageCompression = (ImageCompression)_003Creader_003E5__3.ReadUInt16();
						if (!Enum.IsDefined(typeof(ImageCompression), imageCompression))
						{
							throw ParserUtils.DataCorrupt();
						}
						byte[] array = new byte[tuple.DataLength - 2];
						_003Creader_003E5__3.ReadIntoBuffer(array);
						ChannelData channelData = new ChannelData(tuple.ChannelId, imageCompression, perChannelImageData._bounds, perChannelImageData._psdHeader.ChannelDepth, array, perChannelImageData._isPsb);
						perChannelImageData._loadedChannels.Add(channelData);
						if (channelData.ChannelId >= 0)
						{
							_003C_003E2__current = channelData;
							_003C_003E1__state = 2;
							result = true;
							break;
						}
						goto IL_01d5;
					}
					_003C_003E7__wrap4 = null;
					result = false;
					_003C_003Em__Finally2();
					break;
					IL_01d5:
					_003C_003E7__wrap5++;
					goto IL_01e3;
					IL_00a7:
					if (_003C_003E7__wrap3.MoveNext())
					{
						ChannelData current = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						result = true;
					}
					else
					{
						_003C_003Em__Finally1();
						_003C_003E7__wrap3 = null;
						result = false;
					}
					break;
				}
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
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003CdataStream_003E5__2 != null)
			{
				((IDisposable)_003CdataStream_003E5__2).Dispose();
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
			_003CGetChannels_003Ed__9 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetChannels_003Ed__9(0)
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

	private readonly PsdHeader _psdHeader;

	private readonly Rectangle _bounds;

	private readonly (short ChannelId, long DataLength)[] _channels;

	private readonly ImageDataSource? _dataSource;

	private readonly List<ChannelData> _loadedChannels = new List<ChannelData>();

	private readonly bool _isPsb;

	public override ushort NumberOfChannels => (ushort)_channels.Length;

	public override uint Width => _bounds.Width;

	public override uint Height => _bounds.Height;

	public override ColorMode ColorMode => _psdHeader.ColorMode;

	public override byte[] ColorModeData => _psdHeader.ColorModeData;

	public override ushort ChannelDepth => _psdHeader.ChannelDepth;

	public PerChannelImageData(ParseContext ctx, PsdHeader header, Rectangle bounds, (short ChannelId, long DataLength)[] channels)
		: base(ctx.PsdLoadOptions.ImageDataLoading)
	{
		_bounds = bounds;
		_psdHeader = header;
		_channels = channels;
		_isPsb = ctx.Traits.PsdFileType == PsdFileType.Psb;
		long num = _channels.Aggregate(0L, (long acc, (short ChannelId, long DataLength) info) => acc + info.DataLength);
		if (num == 0L)
		{
			return;
		}
		if (ImageDataLoading == ImageDataLoading.Skip)
		{
			ctx.Reader.Skip(num);
			return;
		}
		switch (ImageDataLoading)
		{
		default:
			return;
		case ImageDataLoading.LoadOnDemand:
			_dataSource = new SeekableStreamSource(ctx.Reader, num);
			return;
		case ImageDataLoading.Auto:
			if (num <= ctx.PsdLoadOptions.AutoImageLoadingDiskCacheThresholdBytes)
			{
				break;
			}
			goto case ImageDataLoading.CacheOnDisk;
		case ImageDataLoading.CacheOnDisk:
			_dataSource = new TempFileSource(ctx.Reader, num);
			return;
		case ImageDataLoading.LoadImmediately:
			break;
		}
		_dataSource = new BufferedSource(ctx.Reader, num);
	}

	[IteratorStateMachine(typeof(_003CGetChannels_003Ed__9))]
	public override IEnumerable<ChannelData> GetChannels()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetChannels_003Ed__9(-2)
		{
			_003C_003E4__this = this
		};
	}
}
