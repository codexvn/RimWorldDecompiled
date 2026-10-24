using System;
using System.Runtime.InteropServices;

namespace Waylith.VideoDecode;

internal sealed class MediaFoundationReader : IDisposable
{
	private struct PropVariant
	{
		public ushort Type;

		public ushort Reserved1;

		public ushort Reserved2;

		public ushort Reserved3;

		public long Value;

		public nint Padding;
	}

	private const uint MfVersion = 131184u;

	private const uint MfStartupLite = 1u;

	private const uint FirstVideoStream = 4294967292u;

	private const uint AllStreams = 4294967294u;

	private const uint MediaSource = uint.MaxValue;

	private const uint FlagEndOfStream = 2u;

	private const uint FlagMediaTypeChanged = 32u;

	private const ushort VtI8 = 20;

	private static readonly Guid MajorType = new Guid("48eba18e-f8c9-4687-bf11-0a74c9f96a8f");

	private static readonly Guid Subtype = new Guid("f7e34c9a-42e8-4714-b74b-cb29d72c35e5");

	private static readonly Guid MediaTypeVideo = new Guid("73646976-0000-0010-8000-00aa00389b71");

	private static readonly Guid FormatNv12 = new Guid("3231564e-0000-0010-8000-00aa00389b71");

	private static readonly Guid FrameSize = new Guid("1652c33d-d6b2-4012-b834-72030849a37d");

	private static readonly Guid FrameRateKey = new Guid("c459a2e8-3d2c-4e44-b132-fee5156c7bb0");

	private static readonly Guid DefaultStride = new Guid("644b4e48-1e02-4516-b0eb-c01ca9d49ac6");

	private static readonly Guid MinimumDisplayAperture = new Guid("d7388766-18fe-48c6-a177-ee894867c8c4");

	private static readonly Guid YuvMatrixKey = new Guid("3e23d450-2c75-4d25-a00e-b91670d12327");

	private static readonly Guid NominalRange = new Guid("c21b8ee5-b956-4071-8daf-325edf5cab11");

	private static readonly Guid DurationKey = new Guid("6c990d33-bb8e-477a-8598-0d5d96fcd88a");

	private nint reader;

	private bool started;

	public int Width { get; private set; }

	public int Height { get; private set; }

	public int Stride { get; private set; }

	public int Rows { get; private set; }

	public int CropX { get; private set; }

	public int CropY { get; private set; }

	public double FrameRate { get; private set; }

	public double Length { get; private set; }

	public YuvMatrix Matrix { get; private set; }

	public bool ReportsColour { get; private set; }

	public bool FullRange { get; private set; }

	public static bool IsAvailable => OperatingSystem.IsWindows();

	[DllImport("mfplat.dll")]
	private static extern int MFStartup(uint version, uint flags);

	[DllImport("mfplat.dll")]
	private static extern int MFShutdown();

	[DllImport("mfplat.dll")]
	private static extern int MFCreateMediaType(out nint mediaType);

	[DllImport("mfreadwrite.dll", CharSet = CharSet.Unicode)]
	private static extern int MFCreateSourceReaderFromURL(string url, nint attributes, out nint reader);

	public unsafe MediaFoundationReader(string path)
	{
		Check(MFStartup(131184u, 1u), "MFStartup");
		started = true;
		Check(MFCreateSourceReaderFromURL(path, 0, out reader), "MFCreateSourceReaderFromURL");
		Check(Call<uint, int, int>(reader, 4, 4294967294u, 0), "SetStreamSelection");
		Check(Call<uint, int, int>(reader, 4, 4294967292u, 1), "SetStreamSelection");
		Check(MFCreateMediaType(out var mediaType), "MFCreateMediaType");
		try
		{
			SetGuid(mediaType, MajorType, MediaTypeVideo);
			SetGuid(mediaType, Subtype, FormatNv12);
			Check(Call<uint, nint, nint, int>(reader, 7, 4294967292u, 0, mediaType), "SetCurrentMediaType (no decoder for this video?)");
		}
		finally
		{
			Release(mediaType);
		}
		ReadOutputFormat();
		PropVariant propVariant = default(PropVariant);
		Guid durationKey = DurationKey;
		if (Call<uint, nint, nint, int>(reader, 12, uint.MaxValue, (nint)(&durationKey), (nint)(&propVariant)) >= 0)
		{
			Length = (double)propVariant.Value / 10000000.0;
		}
	}

	private unsafe void ReadOutputFormat()
	{
		nint num = default(nint);
		Check(Call<uint, nint, int>(reader, 6, 4294967292u, (nint)(&num)), "GetCurrentMediaType");
		try
		{
			ulong num2 = default(ulong);
			Check(GetUInt64(num, FrameSize, &num2), "MF_MT_FRAME_SIZE");
			Width = (int)(num2 >> 32);
			Height = (int)(num2 & 0xFFFFFFFFu);
			Rows = Height;
			int cropX = (CropY = 0);
			CropX = cropX;
			uint value = default(uint);
			Stride = ((GetUInt32(num, DefaultStride, &value) >= 0) ? System.Math.Abs((int)value) : Width);
			ulong num4 = default(ulong);
			if (GetUInt64(num, FrameRateKey, &num4) >= 0 && (num4 & 0xFFFFFFFFu) != 0L)
			{
				FrameRate = (double)(num4 >> 32) / (double)(num4 & 0xFFFFFFFFu);
			}
			byte* ptr = stackalloc byte[16];
			Guid minimumDisplayAperture = MinimumDisplayAperture;
			if (Call<nint, nint, uint, nint, int>(num, 15, (nint)(&minimumDisplayAperture), (nint)ptr, 16u, 0) >= 0)
			{
				CropX = *(short*)(ptr + 2);
				CropY = *(short*)(ptr + 6);
				Width = *(int*)(ptr + 8);
				Height = *(int*)(ptr + 12);
			}
			uint num5 = default(uint);
			ReportsColour = GetUInt32(num, YuvMatrixKey, &num5) >= 0 && num5 != 0;
			Matrix = (((!ReportsColour) ? (Height >= 720) : (num5 == 1)) ? YuvMatrix.Bt709 : YuvMatrix.Bt601);
			uint num6 = default(uint);
			FullRange = GetUInt32(num, NominalRange, &num6) >= 0 && num6 == 1;
		}
		finally
		{
			Release(num);
		}
	}

	public unsafe bool ReadFrame(ref byte[] nv12, out long timestamp)
	{
		uint num = default(uint);
		uint num2 = default(uint);
		long num3 = default(long);
		nint num4 = default(nint);
		while (true)
		{
			Check(Call<uint, uint, nint, nint, nint, nint, int>(reader, 9, 4294967292u, 0u, (nint)(&num), (nint)(&num2), (nint)(&num3), (nint)(&num4)), "ReadSample");
			if ((num2 & 0x20) != 0)
			{
				ReadOutputFormat();
			}
			if (num4 != 0)
			{
				break;
			}
			if ((num2 & 2) != 0)
			{
				timestamp = 0L;
				return false;
			}
		}
		try
		{
			nint num5 = default(nint);
			Check(Call<nint, int>(num4, 41, (nint)(&num5)), "ConvertToContiguousBuffer");
			try
			{
				byte* pointer = default(byte*);
				uint num6 = default(uint);
				uint num7 = default(uint);
				Check(Call<nint, nint, nint, int>(num5, 3, (nint)(&pointer), (nint)(&num6), (nint)(&num7)), "Lock");
				int num8 = (Width + 15) & -16;
				int num9 = (Height + 15) & -16;
				if (Stride % 16 != 0 && num7 == (uint)(num8 * num9 * 3 / 2))
				{
					Stride = num8;
					Rows = num9;
				}
				int num10 = System.Math.Max(Stride * Rows * 3 / 2, (int)num7);
				if (nv12 == null || nv12.Length < num10)
				{
					nv12 = new byte[num10];
				}
				new ReadOnlySpan<byte>(pointer, (int)num7).CopyTo(nv12);
				Call<int>(num5, 4);
			}
			finally
			{
				Release(num5);
			}
		}
		finally
		{
			Release(num4);
		}
		timestamp = num3;
		return true;
	}

	public unsafe void Seek(double seconds)
	{
		PropVariant propVariant = default(PropVariant);
		propVariant.Type = 20;
		propVariant.Value = (long)(seconds * 10000000.0);
		PropVariant propVariant2 = propVariant;
		Guid empty = Guid.Empty;
		Check(Call<nint, nint, int>(reader, 8, (nint)(&empty), (nint)(&propVariant2)), "SetCurrentPosition");
	}

	private static void Check(int hr, string what)
	{
		if (hr < 0)
		{
			throw new InvalidOperationException($"Media Foundation {what} failed (0x{hr:X8})");
		}
	}

	private unsafe static void SetGuid(nint attributes, Guid key, Guid value)
	{
		Check(Call<nint, nint, int>(attributes, 24, (nint)(&key), (nint)(&value)), "SetGUID");
	}

	private unsafe static int GetUInt32(nint attributes, Guid key, uint* value)
	{
		return Call<nint, nint, int>(attributes, 7, (nint)(&key), (nint)value);
	}

	private unsafe static int GetUInt64(nint attributes, Guid key, ulong* value)
	{
		return Call<nint, nint, int>(attributes, 8, (nint)(&key), (nint)value);
	}

	private static void Release(nint unknown)
	{
		if (unknown != 0)
		{
			Call<uint>(unknown, 2);
		}
	}

	private unsafe static void* Slot(nint instance, int index)
	{
		return *(void**)((nint)(*(IntPtr*)instance) + (nint)index * (nint)sizeof(void*));
	}

	private unsafe static TResult Call<TResult>(nint instance, int slot)
	{
		return ((delegate* unmanaged[Stdcall]<nint, TResult>)Slot(instance, slot))(instance);
	}

	private unsafe static TResult Call<T1, TResult>(nint instance, int slot, T1 a)
	{
		return ((delegate* unmanaged[Stdcall]<nint, T1, TResult>)Slot(instance, slot))(instance, a);
	}

	private unsafe static TResult Call<T1, T2, TResult>(nint instance, int slot, T1 a, T2 b)
	{
		return ((delegate* unmanaged[Stdcall]<nint, T1, T2, TResult>)Slot(instance, slot))(instance, a, b);
	}

	private unsafe static TResult Call<T1, T2, T3, TResult>(nint instance, int slot, T1 a, T2 b, T3 c)
	{
		return ((delegate* unmanaged[Stdcall]<nint, T1, T2, T3, TResult>)Slot(instance, slot))(instance, a, b, c);
	}

	private unsafe static TResult Call<T1, T2, T3, T4, TResult>(nint instance, int slot, T1 a, T2 b, T3 c, T4 d)
	{
		return ((delegate* unmanaged[Stdcall]<nint, T1, T2, T3, T4, TResult>)Slot(instance, slot))(instance, a, b, c, d);
	}

	private unsafe static TResult Call<T1, T2, T3, T4, T5, T6, TResult>(nint instance, int slot, T1 a, T2 b, T3 c, T4 d, T5 e, T6 f)
	{
		return ((delegate* unmanaged[Stdcall]<nint, T1, T2, T3, T4, T5, T6, TResult>)Slot(instance, slot))(instance, a, b, c, d, e, f);
	}

	public void Dispose()
	{
		Release(reader);
		reader = 0;
		if (started)
		{
			MFShutdown();
			started = false;
		}
	}
}
