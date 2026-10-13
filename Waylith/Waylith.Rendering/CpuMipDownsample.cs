using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

namespace Waylith.Rendering;

public static class CpuMipDownsample
{
	private static readonly Vector128<byte> PackChannels = Vector128.Create((byte)0, (byte)4, (byte)8, (byte)12, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0, (byte)0);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int ClampIndex(int value, int extent)
	{
		if (value >= extent)
		{
			return extent - 1;
		}
		return value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private unsafe static Vector128<float> LoadTexel(byte* texel)
	{
		return Vector128.ConvertToSingle(Vector128.WidenLower(Vector128.WidenLower(Vector128.CreateScalar(*(uint*)texel).AsByte())).AsInt32()) * Vector128.Create(0.003921569f);
	}

	public unsafe static void DownsampleRows(ReadOnlySpan<byte> source, int sourceWidth, int sourceHeight, Span<byte> destination, int destinationWidth, int destinationHeight, int firstRow, int rowCount, bool premultiplied = false)
	{
		Vector128<float> vector = Vector128.Create(0.25f);
		Vector128<float> vector2 = Vector128.Create(255f);
		Vector128<float> vector3 = Vector128.Create(0.5f);
		Vector128<float> one = Vector128<float>.One;
		fixed (byte* ptr = source)
		{
			fixed (byte* ptr4 = destination)
			{
				for (int i = firstRow; i < firstRow + rowCount && i < destinationHeight; i++)
				{
					byte* ptr2 = ptr + (long)ClampIndex(i * 2, sourceHeight) * (long)sourceWidth * 4;
					byte* ptr3 = ptr + (long)ClampIndex(i * 2 + 1, sourceHeight) * (long)sourceWidth * 4;
					byte* ptr5 = ptr4 + (long)i * (long)destinationWidth * 4;
					for (int j = 0; j < destinationWidth; j++)
					{
						int num = ClampIndex(j * 2, sourceWidth) * 4;
						int num2 = ClampIndex(j * 2 + 1, sourceWidth) * 4;
						Vector128<float> vector4 = LoadTexel(ptr2 + num);
						Vector128<float> vector5 = LoadTexel(ptr2 + num2);
						Vector128<float> vector6 = LoadTexel(ptr3 + num);
						Vector128<float> vector7 = LoadTexel(ptr3 + num2);
						Vector128<float> vector8 = Vector128.Create(vector4[3]);
						Vector128<float> vector9 = Vector128.Create(vector5[3]);
						Vector128<float> vector10 = Vector128.Create(vector6[3]);
						Vector128<float> vector11 = Vector128.Create(vector7[3]);
						Vector128<float> vector12 = vector8 + vector9 + vector10 + vector11;
						Vector128<int> vector13 = Vector128.ConvertToInt32(Vector128.Min(Vector128.Max(((!premultiplied && vector12[0] > 1E-05f && vector12[0] != 4f) ? ((vector4 * vector8 + vector5 * vector9 + vector6 * vector10 + vector7 * vector11) / vector12) : ((vector4 + vector5 + vector6 + vector7) * vector)).WithElement(3, vector12[0] * 0.25f), Vector128<float>.Zero), one) * vector2 + vector3);
						*(uint*)(ptr5 + j * 4) = Vector128.Shuffle(vector13.AsByte(), PackChannels).AsUInt32()[0];
					}
				}
			}
		}
	}
}
