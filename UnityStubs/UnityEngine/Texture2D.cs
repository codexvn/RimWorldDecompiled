using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AssetsTools.NET.Texture;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Experimental.Rendering;
using UnityStubs;
using UnityStubs.Internal;
using UnityStubs.Rendering;
using Waylith.Math;
using Waylith.Rendering;

namespace UnityEngine;

public class Texture2D : Texture
{
	public const int streamingMipmapsPriorityMax = 127;

	public const int streamingMipmapsPriorityMin = -128;

	private static Texture2D _redTexture;

	private static Texture2D _whiteTexture;

	private static Texture2D _blackTexture;

	private static Texture2D _grayTexture;

	private static Texture2D _linearGrayTexture;

	private static Texture2D _normalTexture;

	private TextureFormat? _requestedFormat;

	private byte[] _rawView;

	private byte[] _rawViewSnapshot;

	public TextureFormat format
	{
		get
		{
			CoreTexture?.FinishPendingBuild();
			return (TextureFormat)(CoreTexture?.CpuData?.Format ?? LoadedTextureFormat.Rgba32);
		}
	}

	public bool isDataSRGB => false;

	public override GraphicsFormat graphicsFormat
	{
		get
		{
			switch (format)
			{
			case TextureFormat.Alpha8:
			case TextureFormat.R8:
				return GraphicsFormat.R8_UNorm;
			case TextureFormat.RGB24:
				return GraphicsFormat.R8G8B8_UNorm;
			case TextureFormat.RFloat:
				return GraphicsFormat.R32_SFloat;
			case TextureFormat.RGFloat:
				return GraphicsFormat.R32G32_SFloat;
			case TextureFormat.RGBAFloat:
				return GraphicsFormat.R32G32B32A32_SFloat;
			case TextureFormat.RGBAHalf:
				return GraphicsFormat.R16G16B16A16_SFloat;
			case TextureFormat.DXT1:
				return GraphicsFormat.RGB_DXT1_UNorm;
			case TextureFormat.DXT5:
				return GraphicsFormat.RGBA_DXT5_UNorm;
			case TextureFormat.BC4:
				return GraphicsFormat.R_BC4_UNorm;
			case TextureFormat.BC5:
				return GraphicsFormat.RG_BC5_UNorm;
			case TextureFormat.BC7:
				return GraphicsFormat.RGBA_BC7_UNorm;
			default:
				return GraphicsFormat.R8G8B8A8_UNorm;
			}
		}
	}

	public bool ignoreMipmapLimit { get; set; }

	public int requestedMipmapLevel { get; set; }

	public override Waylith.Rendering.Texture2D CoreTexture { get; }

	public GpuTextureAllocation GpuTexture => CoreTexture?.GpuResource;

	public int minimumMipmapLevel
	{
		get
		{
			return CoreTexture?.minimumMipmapLevel ?? 0;
		}
		set
		{
			if (CoreTexture != null)
			{
				CoreTexture.minimumMipmapLevel = value;
			}
		}
	}

	public static Texture2D redTexture => _redTexture ?? (_redTexture = new Texture2D(Waylith.Rendering.Texture2D.CreateSolid("RedTexture", new Waylith.Math.Color32(byte.MaxValue, 0, 0, byte.MaxValue))));

	public static Texture2D whiteTexture => _whiteTexture ?? (_whiteTexture = new Texture2D(Waylith.Rendering.Texture2D.CreateSolid("WhiteTexture", new Waylith.Math.Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue))));

	public static Texture2D blackTexture => _blackTexture ?? (_blackTexture = new Texture2D(Waylith.Rendering.Texture2D.CreateSolid("BlackTexture", new Waylith.Math.Color32(0, 0, 0, byte.MaxValue))));

	public static Texture2D grayTexture => _grayTexture ?? (_grayTexture = new Texture2D(Waylith.Rendering.Texture2D.CreateSolid("GrayTexture", new Waylith.Math.Color32(128, 128, 128, byte.MaxValue))));

	public static Texture2D linearGrayTexture => _linearGrayTexture ?? (_linearGrayTexture = new Texture2D(Waylith.Rendering.Texture2D.CreateSolid("LinearGrayTexture", new Waylith.Math.Color32(128, 128, 128, byte.MaxValue))));

	public static Texture2D normalTexture => _normalTexture ?? (_normalTexture = new Texture2D(Waylith.Rendering.Texture2D.CreateSolid("NormalTexture", new Waylith.Math.Color32(128, 128, byte.MaxValue, byte.MaxValue))));

	internal bool HasCpuMipStorage => CoreTexture?.IsReadable ?? false;

	public void Compress(bool _)
	{
		if (CoreTexture == null || TextureCompressionSettings.SelectTarget(base.width, base.height, CoreTexture.Opaque, CoreTexture.SourcePath) == BcEncodeTarget.None || CoreTexture.TryDeferCompression(TextureCompressionSettings.Target, TextureCompressionSettings.Quality))
		{
			return;
		}
		TexturePixelData cpuData = CoreTexture.CpuData;
		if (cpuData == null || TexturePixelData.IsBlockCompressed(cpuData.Format))
		{
			return;
		}
		bool hasData = cpuData.HasData;
		if (CoreTexture.CompressInPlace(TextureCompressionSettings.Target, TextureCompressionSettings.Quality))
		{
			if (CoreTexture.SourcePath != null)
			{
				GpuTextureService.WriteTextureCache(CoreTexture.SourcePath, CoreTexture);
			}
			if (!hasData)
			{
				cpuData.ReleaseTracked();
				cpuData.Release();
			}
		}
	}

	internal static void CompressLikeImporter(Texture2D tex)
	{
		tex.Compress(_: true);
	}

	public Texture2D(int width, int height)
		: this(width, height, TextureFormat.RGBA32, mipChain: false)
	{
	}

	public Texture2D(int width, int height, TextureFormat textureFormat, bool mipChain)
		: this(width, height, textureFormat, mipChain, linear: false)
	{
	}

	public Texture2D(int width, int height, TextureFormat textureFormat, bool mipChain, bool linear)
		: this(width, height, textureFormat, (!mipChain) ? 1 : TexturePixelData.CalculateMipCount(width, height), linear, createUninitialized: false)
	{
	}

	public Texture2D(int width, int height, TextureFormat textureFormat, int mipCount, bool linear)
		: this(width, height, textureFormat, mipCount, linear, createUninitialized: false)
	{
	}

	public Texture2D(int width, int height, TextureFormat textureFormat, int mipCount, bool linear, bool createUninitialized)
		: this(new Waylith.Rendering.Texture2D(width, height, ToLoadedFormat(textureFormat), linear, Math.Max(1, mipCount), createUninitialized))
	{
		_requestedFormat = textureFormat;
	}

	public Texture2D(int width, int height, GraphicsFormat format, int mipCount, TextureCreationFlags flags)
		: this(width, height, ToTextureFormat(format), Math.Max(1, mipCount), IsLinear(format), IsUninitialized(flags))
	{
	}

	public Texture2D(int width, int height, GraphicsFormat format, TextureCreationFlags flags)
		: this(width, height, ToTextureFormat(format), ((flags & TextureCreationFlags.MipChain) == 0) ? 1 : TexturePixelData.CalculateMipCount(width, height), IsLinear(format), IsUninitialized(flags))
	{
	}

	internal Texture2D(Waylith.Rendering.Texture2D core)
	{
		CoreTexture = core;
		if (!string.IsNullOrEmpty(core.Name))
		{
			base.name = core.Name;
		}
	}

	public void Apply()
	{
		Apply(updateMipmaps: true, makeNoLongerReadable: false);
	}

	public void Apply(bool updateMipmaps)
	{
		Apply(updateMipmaps, makeNoLongerReadable: false);
	}

	public void Apply(bool updateMipmaps, bool makeNoLongerReadable)
	{
		TexturePixelData cpuData = CoreTexture.CpuData;
		if (cpuData != null && !cpuData.HasData)
		{
			return;
		}
		if (_rawView != null && !((ReadOnlySpan<byte>)_rawView.AsSpan()).SequenceEqual((ReadOnlySpan<byte>)_rawViewSnapshot))
		{
			LoadRawTextureData(_rawView);
			_rawView.CopyTo(_rawViewSnapshot, 0);
		}
		bool flag;
		switch (_requestedFormat)
		{
		case TextureFormat.RGB24:
		case TextureFormat.RGB565:
		case TextureFormat.RGB48:
			flag = true;
			break;
		default:
			flag = false;
			break;
		}
		if (flag)
		{
			TexturePixelData cpuData2 = CoreTexture.CpuData;
			if (cpuData2 != null && cpuData2.Format == LoadedTextureFormat.Rgba32)
			{
				ForceOpaque(cpuData2);
			}
		}
		CoreTexture.Apply(updateMipmaps, makeNoLongerReadable);
	}

	public unsafe NativeArray<T> GetRawTextureData<T>() where T : struct
	{
		byte[] rawTextureData = GetRawTextureData();
		if (_rawView == null || _rawView.Length != rawTextureData.Length)
		{
			_rawView = GC.AllocateUninitializedArray<byte>(rawTextureData.Length, pinned: true);
			_rawViewSnapshot = new byte[rawTextureData.Length];
		}
		rawTextureData.CopyTo(_rawView, 0);
		rawTextureData.CopyTo(_rawViewSnapshot, 0);
		return new NativeArray<T>(Unsafe.AsPointer(in MemoryMarshal.GetArrayDataReference(_rawView)), rawTextureData.Length / UnsafeUtility.SizeOf<T>(), Allocator.None, ownsBuffer: false);
	}

	public byte[] GetRawTextureData()
	{
		TexturePixelData texturePixelData = CoreTexture?.EnsureCpuData();
		if (texturePixelData == null || !texturePixelData.HasData)
		{
			return Array.Empty<byte>();
		}
		MemoryStream memoryStream = new MemoryStream();
		for (int i = 0; i < texturePixelData.Levels.Length && texturePixelData.Levels[i] != null; i++)
		{
			memoryStream.Write(AssetBundleTextures.ToUnityOrder(texturePixelData.Levels[i], texturePixelData.Format, Math.Max(1, base.width >> i), Math.Max(1, base.height >> i)));
		}
		return memoryStream.ToArray();
	}

	public void LoadRawTextureData(byte[] data)
	{
		if (data == null)
		{
			return;
		}
		LoadedTextureFormat loadedTextureFormat = CoreTexture.CpuData?.Format ?? LoadedTextureFormat.Rgba32;
		TextureFormat? requestedFormat = _requestedFormat;
		TextureFormat num;
		if (requestedFormat.HasValue)
		{
			TextureFormat valueOrDefault = requestedFormat.GetValueOrDefault();
			if (ToLoadedFormat(valueOrDefault) == loadedTextureFormat)
			{
				num = valueOrDefault;
				goto IL_0047;
			}
		}
		num = format;
		goto IL_0047;
		IL_0047:
		TextureFormat value = num;
		int num2 = base.mipmapCount;
		int mipCount = num2;
		LoadedTextureFormat loadedFormat;
		byte[][] array = AssetBundleTextures.ConvertUnityLevels(data, (AssetsTools.NET.Texture.TextureFormat)value, base.width, base.height, ref mipCount, out loadedFormat);
		if (array == null)
		{
			Debug.LogError($"LoadRawTextureData: {data.Length} bytes do not fit texture '{base.name}' ({base.width}x{base.height} {value}).");
		}
		else
		{
			int mipCount2 = (TexturePixelData.IsBlockCompressed(loadedFormat) ? mipCount : num2);
			CoreTexture.ReplaceCpuData(base.width, base.height, loadedFormat, array, mipCount2, CoreTexture.CpuData?.Linear ?? false);
		}
	}

	public void LoadRawTextureData<T>(NativeArray<T> data) where T : struct
	{
		LoadRawTextureData(MemoryMarshal.AsBytes((ReadOnlySpan<T>)data.ToArray()).ToArray());
	}

	public NativeArray<T> GetPixelData<T>(int mipLevel) where T : struct
	{
		byte[][] array = CoreTexture?.EnsureCpuData()?.Levels;
		byte[] array2 = ((array != null && (uint)mipLevel < (uint)array.Length) ? array[mipLevel] : null);
		return new NativeArray<T>((array2 == null) ? Array.Empty<T>() : MemoryMarshal.Cast<byte, T>((ReadOnlySpan<byte>)array2).ToArray(), Allocator.Temp);
	}

	public void SetPixelData<T>(NativeArray<T> data, int mipLevel, int sourceDataStartIndex = 0) where T : struct
	{
		SetPixelData(data.ToArray(), mipLevel, sourceDataStartIndex);
	}

	public void SetPixelData<T>(T[] data, int mipLevel, int sourceDataStartIndex = 0) where T : struct
	{
		if (mipLevel != 0)
		{
			StubWarner.LogStub("SetPixelData", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Texture2D.cs", 215);
		}
		else
		{
			LoadRawTextureData(MemoryMarshal.AsBytes(data.AsSpan(sourceDataStartIndex)).ToArray());
		}
	}

	public void LoadRawTextureData(nint data, int size)
	{
		if (data != IntPtr.Zero && size > 0)
		{
			byte[] array = new byte[size];
			Marshal.Copy(data, array, 0, size);
			LoadRawTextureData(array);
		}
	}

	public byte[] GetRgba32OrNull()
	{
		return CoreTexture?.EnsureCpuData()?.GetRgba32OrNull();
	}

	public byte[] GetPremultipliedRgba32OrNull()
	{
		return CoreTexture?.EnsureCpuData()?.GetPremultipliedRgba32OrNull();
	}

	private static int FlipRow(int y, int levelHeight)
	{
		return levelHeight - 1 - y;
	}

	private static void ReverseRows(Span<byte> level, int rowBytes, int rows)
	{
		Span<byte> span = ((rowBytes > 4096) ? ((Span<byte>)new byte[rowBytes]) : stackalloc byte[rowBytes]);
		Span<byte> destination = span;
		int num = 0;
		int num2 = rows - 1;
		while (num < num2)
		{
			Span<byte> destination2 = level.Slice(num * rowBytes, rowBytes);
			Span<byte> destination3 = level.Slice(num2 * rowBytes, rowBytes);
			destination2.CopyTo(destination);
			destination3.CopyTo(destination2);
			destination.CopyTo(destination3);
			num++;
			num2--;
		}
	}

	public Color GetPixel(int x, int y)
	{
		return Color.FromCore(CoreTexture.GetPixel(WrapCoord(x, base.width), FlipRow(WrapCoord(y, base.height), base.height)));
	}

	private int WrapCoord(int v, int size)
	{
		if ((uint)v < (uint)size)
		{
			return v;
		}
		if (base.wrapMode == TextureWrapMode.Repeat)
		{
			return (v % size + size) % size;
		}
		if (v >= 0)
		{
			return size - 1;
		}
		return 0;
	}

	public Color[] GetPixels()
	{
		return GetPixels(0);
	}

	public Color[] GetPixels(int miplevel)
	{
		Color32[] pixels = GetPixels32(miplevel);
		Color[] array = new Color[pixels.Length];
		for (int i = 0; i < pixels.Length; i++)
		{
			array[i] = pixels[i];
		}
		return array;
	}

	public Color32[] GetPixels32()
	{
		return GetPixels32(0);
	}

	public Color32[] GetPixels32(int miplevel)
	{
		TexturePixelData texturePixelData = RequireCpuData();
		miplevel = Math.Clamp(miplevel, 0, Math.Max(0, texturePixelData.Levels.Length - 1));
		int num = Math.Max(1, base.width >> miplevel);
		int num2 = Math.Max(1, base.height >> miplevel);
		Color32[] array = new Color32[num * num2];
		Span<byte> span = MemoryMarshal.AsBytes(array.AsSpan());
		Rgba32Level(texturePixelData, miplevel).AsSpan(0, span.Length).CopyTo(span);
		ReverseRows(span, num * 4, num2);
		return array;
	}

	public Color[] GetPixels(int x, int y, int blockWidth, int blockHeight)
	{
		return GetPixels(x, y, blockWidth, blockHeight, 0);
	}

	public Color[] GetPixels(int x, int y, int blockWidth, int blockHeight, int miplevel)
	{
		TexturePixelData texturePixelData = RequireCpuData();
		miplevel = Math.Clamp(miplevel, 0, Math.Max(0, texturePixelData.Levels.Length - 1));
		int num = Math.Max(1, base.width >> miplevel);
		int num2 = Math.Max(1, base.height >> miplevel);
		byte[] array = Rgba32Level(texturePixelData, miplevel);
		Color[] array2 = new Color[Math.Max(0, blockWidth) * Math.Max(0, blockHeight)];
		for (int i = 0; i < blockHeight; i++)
		{
			int num3 = FlipRow(y + i, num2);
			if ((uint)num3 >= (uint)num2)
			{
				continue;
			}
			for (int j = 0; j < blockWidth; j++)
			{
				int num4 = x + j;
				if ((uint)num4 < (uint)num)
				{
					int num5 = (num3 * num + num4) * 4;
					array2[i * blockWidth + j] = new Color((float)(int)array[num5] / 255f, (float)(int)array[num5 + 1] / 255f, (float)(int)array[num5 + 2] / 255f, (float)(int)array[num5 + 3] / 255f);
				}
			}
		}
		return array2;
	}

	public Color GetPixelBilinear(float u, float v)
	{
		u -= MathF.Floor(u);
		v -= MathF.Floor(v);
		float num = u * (float)base.width - 0.5f;
		float num2 = v * (float)base.height - 0.5f;
		int num3 = (int)MathF.Floor(num);
		int num4 = (int)MathF.Floor(num2);
		float t = num - (float)num3;
		float t2 = num2 - (float)num4;
		Color a = Color.LerpUnclamped(Sample(num3, num4), Sample(num3 + 1, num4), t);
		Color b = Color.LerpUnclamped(Sample(num3, num4 + 1), Sample(num3 + 1, num4 + 1), t);
		return Color.LerpUnclamped(a, b, t2);
		Color Sample(int x, int y)
		{
			return GetPixel((x % base.width + base.width) % base.width, (y % base.height + base.height) % base.height);
		}
	}

	public void SetPixel(int x, int y, Color color)
	{
		CoreTexture.SetPixel(WrapCoord(x, base.width), FlipRow(WrapCoord(y, base.height), base.height), Color.ToCore(color));
	}

	public void SetPixels(Color[] colors)
	{
		ArgumentNullException.ThrowIfNull(colors, "colors");
		if (colors.Length != base.width * base.height)
		{
			throw new ArgumentException("Pixel array size does not match texture dimensions.", "colors");
		}
		Waylith.Math.Color[] array = new Waylith.Math.Color[colors.Length];
		for (int i = 0; i < base.height; i++)
		{
			int num = i * base.width;
			int num2 = FlipRow(i, base.height) * base.width;
			for (int j = 0; j < base.width; j++)
			{
				array[num2 + j] = Color.ToCore(colors[num + j]);
			}
		}
		CoreTexture.SetPixels(array);
	}

	public void SetPixels(int x, int y, int blockWidth, int blockHeight, Color[] colors)
	{
		SetPixels(x, y, blockWidth, blockHeight, colors, 0);
	}

	public void SetPixels(int x, int y, int blockWidth, int blockHeight, Color[] colors, int miplevel)
	{
		ArgumentNullException.ThrowIfNull(colors, "colors");
		Color32[] array = new Color32[colors.Length];
		for (int i = 0; i < colors.Length; i++)
		{
			array[i] = colors[i];
		}
		SetPixels32Region(miplevel, x, y, blockWidth, blockHeight, array);
	}

	public void SetPixels32(int x, int y, int blockWidth, int blockHeight, Color32[] colors)
	{
		SetPixels32Region(0, x, y, blockWidth, blockHeight, colors);
	}

	public bool LoadImage(byte[] data)
	{
		return LoadEncodedImage(data);
	}

	public bool Reinitialize(int width, int height)
	{
		TextureFormat num = _requestedFormat ?? format;
		Waylith.Rendering.Texture2D coreTexture = CoreTexture;
		return Reinitialize(width, height, num, coreTexture != null && coreTexture.CpuData?.MipCount > 1);
	}

	public bool Reinitialize(int width, int height, TextureFormat format, bool hasMipMap)
	{
		if (width <= 0 || height <= 0)
		{
			return false;
		}
		_requestedFormat = format;
		LoadedTextureFormat loadedTextureFormat = ToLoadedFormat(format);
		CoreTexture.ReplaceCpuData(width, height, loadedTextureFormat, new byte[1][] { new byte[width * height * TexturePixelData.BytesPerPixel(loadedTextureFormat)] }, 1, CoreTexture.CpuData?.Linear ?? false);
		return true;
	}

	public void SetPixels32(Color32[] colors)
	{
		ArgumentNullException.ThrowIfNull(colors, "colors");
		if (colors.Length != base.width * base.height)
		{
			throw new ArgumentException("Pixel array size does not match texture dimensions.", "colors");
		}
		TexturePixelData texturePixelData = RequireCpuData();
		texturePixelData.EnsureWritableMipZero();
		MemoryMarshal.AsBytes(colors.AsSpan()).CopyTo(texturePixelData.Levels[0]);
		ReverseRows(texturePixelData.Levels[0], base.width * 4, base.height);
	}

	public Rect[] PackTextures(Texture2D[] textures, int padding, int maximumAtlasSize, bool makeNoLongerReadable)
	{
		StubWarner.LogStub("PackTextures", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Texture2D.cs", 391);
		return null;
	}

	public void ReadPixels(Rect source, int destX, int destY, bool recalculateMipMaps)
	{
		ReadPixels(source, destX, destY);
	}

	public void ReadPixels(Rect source, int destX, int destY)
	{
		RenderTexture active = RenderTexture.active;
		TexturePixelData texturePixelData = RequireCpuData();
		texturePixelData.EnsureWritableMipZero();
		if (destX < 0 || destY < 0 || destX >= base.width || destY >= base.height)
		{
			return;
		}
		if (active == null)
		{
			ReadScreenPixels(texturePixelData, source, destX, destY);
			return;
		}
		ICameraTextureRenderer current = CameraTextureRenderer.Current;
		Color color;
		if (current != null && current.TryReadPixels(active, out var rgbaTopDown))
		{
			int num = Math.Max(0, (int)source.x);
			int num2 = Math.Max(0, (int)source.y);
			int num3 = Math.Min((int)source.width, Math.Min(active.width - num, base.width - destX));
			int num4 = Math.Min((int)source.height, Math.Min(active.height - num2, base.height - destY));
			for (int i = 0; i < num4; i++)
			{
				int num5 = (num2 + i) * active.width;
				for (int j = 0; j < num3; j++)
				{
					int num6 = (num5 + num + j) * 4;
					texturePixelData.WritePixel(destX + j, destY + i, new Waylith.Math.Color((float)(int)rgbaTopDown[num6] / 255f, (float)(int)rgbaTopDown[num6 + 1] / 255f, (float)(int)rgbaTopDown[num6 + 2] / 255f, (float)(int)rgbaTopDown[num6 + 3] / 255f));
				}
			}
		}
		else if (RenderTexture.TryReadPixels(active, source, out color))
		{
			texturePixelData.WritePixel(destX, destY, Color.ToCore(color));
		}
	}

	private void ReadScreenPixels(TexturePixelData data, Rect source, int destX, int destY)
	{
		IScreenCaptureBackend current = ScreenCaptureBackend.Current;
		if (current == null || !current.TryReadScreen(out var rgbaTopDown, out var num, out var num2))
		{
			ScreenCapture.WarnUnreadable("Texture2D.ReadPixels");
			return;
		}
		int num3 = Math.Max(0, (int)source.x);
		int num4 = Math.Max(0, (int)source.y);
		int num5 = Math.Min((int)source.width, Math.Min(num - num3, base.width - destX));
		int num6 = Math.Min((int)source.height, Math.Min(num2 - num4, base.height - destY));
		for (int i = 0; i < num6; i++)
		{
			int num7 = FlipRow(num4 + i, num2) * num;
			int y = FlipRow(destY + i, base.height);
			for (int j = 0; j < num5; j++)
			{
				int num8 = (num7 + num3 + j) * 4;
				data.WritePixel(destX + j, y, new Waylith.Math.Color((float)(int)rgbaTopDown[num8] / 255f, (float)(int)rgbaTopDown[num8 + 1] / 255f, (float)(int)rgbaTopDown[num8 + 2] / 255f, (float)(int)rgbaTopDown[num8 + 3] / 255f));
			}
		}
	}

	public static Texture2D LoadFromFile(string filePath)
	{
		Texture2D texture2D = new Texture2D(Waylith.Rendering.Texture2D.LoadFromFile(filePath));
		if (ImportDefaults.IsOfficial(filePath))
		{
			texture2D.wrapMode = TextureWrapMode.Clamp;
		}
		return texture2D;
	}

	public static Texture2D LoadFromTextureCache(string filePath)
	{
		Waylith.Rendering.Texture2D texture2D = Waylith.Rendering.Texture2D.LoadFromTextureCache(filePath);
		if (texture2D == null)
		{
			return null;
		}
		return new Texture2D(texture2D);
	}

	public void PersistToTextureCache(string filePath)
	{
		CoreTexture?.PersistToTextureCache(filePath);
	}

	internal static Texture2D CreatePlaceholder(string name, Color32 color)
	{
		return new Texture2D(Waylith.Rendering.Texture2D.CreatePlaceholder(name, Color32.ToCore(color)));
	}

	internal bool LoadEncodedImage(byte[] data)
	{
		_requestedFormat = null;
		return CoreTexture.LoadEncodedImage(data);
	}

	internal void UploadChangedTextureData()
	{
		CoreTexture.UploadCpuData();
	}

	internal bool TryEnsureGpuTextureAllocated()
	{
		return CoreTexture?.TryEnsureGpuAllocated() ?? false;
	}

	public void EvictGpuResource()
	{
		CoreTexture?.Evict();
	}

	public bool EnsureGpuResident()
	{
		return CoreTexture?.EnsureResident() ?? false;
	}

	public bool CompressInPlace(BcEncodeTarget target, float quality)
	{
		return CoreTexture?.CompressInPlace(target, quality) ?? false;
	}

	internal Color32 SampleColor32(int mipLevel, float u, float v)
	{
		TexturePixelData texturePixelData = RequireCpuData();
		mipLevel = Math.Clamp(mipLevel, 0, Math.Max(0, texturePixelData.Levels.Length - 1));
		int num = Math.Max(1, base.width >> mipLevel);
		int num2 = Math.Max(1, base.height >> mipLevel);
		byte[] array = Rgba32Level(texturePixelData, mipLevel);
		u = Math.Clamp(u, 0f, 0.999999f);
		v = Math.Clamp(v, 0f, 0.999999f);
		int num3 = Math.Clamp((int)MathF.Floor(u * (float)num), 0, num - 1);
		int num4 = (Math.Clamp((int)MathF.Floor(v * (float)num2), 0, num2 - 1) * num + num3) * 4;
		return new Color32(array[num4], array[num4 + 1], array[num4 + 2], array[num4 + 3]);
	}

	internal void SetPixels32Region(int mipLevel, int dstX, int dstY, int regionWidth, int regionHeight, Color32[] pixels)
	{
		TexturePixelData texturePixelData = RequireCpuData();
		mipLevel = Math.Clamp(mipLevel, 0, Math.Max(0, texturePixelData.MipCount - 1));
		texturePixelData.EnsureWritableMipLevel(mipLevel);
		int num = Math.Max(1, base.width >> mipLevel);
		int num2 = Math.Max(1, base.height >> mipLevel);
		byte[] array = texturePixelData.Levels[mipLevel];
		Span<byte> span = MemoryMarshal.AsBytes(pixels.AsSpan());
		for (int i = 0; i < regionHeight; i++)
		{
			int num3 = FlipRow(dstY + i, num2);
			if ((uint)num3 < (uint)num2)
			{
				int num4 = i * regionWidth * 4;
				int start = (num3 * num + dstX) * 4;
				int num5 = Math.Min(regionWidth, num - dstX) * 4;
				if (num5 > 0 && num4 + num5 <= span.Length)
				{
					span.Slice(num4, num5).CopyTo(array.AsSpan(start));
				}
			}
		}
	}

	internal ReadOnlySpan<byte> Rgba32TopDown()
	{
		return Rgba32Level(RequireCpuData(), 0).AsSpan(0, base.width * base.height * 4);
	}

	private static void ForceOpaque(TexturePixelData data)
	{
		byte[][] levels = data.Levels;
		foreach (byte[] array in levels)
		{
			for (int j = 3; j < array.Length; j += 4)
			{
				array[j] = byte.MaxValue;
			}
		}
	}

	private TexturePixelData RequireCpuData()
	{
		return CoreTexture?.EnsureCpuData() ?? throw new InvalidOperationException("Texture '" + base.name + "' is not readable.");
	}

	private byte[] Rgba32Level(TexturePixelData data, int mipLevel)
	{
		byte[] array = data.Levels[mipLevel];
		if (data.Format == LoadedTextureFormat.Rgba32)
		{
			return array;
		}
		return AssetBundleTextures.DecodeToRgba32(array, data.Format, Math.Max(1, base.width >> mipLevel), Math.Max(1, base.height >> mipLevel)) ?? throw new InvalidOperationException($"Texture '{base.name}' ({data.Format}) cannot be read back as RGBA32.");
	}

	private static bool IsUninitialized(TextureCreationFlags flags)
	{
		if ((flags & TextureCreationFlags.DontInitializePixels) == 0)
		{
			return (flags & TextureCreationFlags.DontUploadUponCreate) != 0;
		}
		return true;
	}

	private static LoadedTextureFormat ToLoadedFormat(TextureFormat format)
	{
		return format switch
		{
			TextureFormat.Alpha8 => LoadedTextureFormat.Alpha8, 
			TextureFormat.R8 => LoadedTextureFormat.R8, 
			TextureFormat.RFloat => LoadedTextureFormat.RFloat, 
			TextureFormat.RGFloat => LoadedTextureFormat.RGFloat, 
			TextureFormat.RGBAFloat => LoadedTextureFormat.RgbaFloat, 
			TextureFormat.BC7 => LoadedTextureFormat.Bc7, 
			TextureFormat.BC5 => LoadedTextureFormat.Bc5, 
			TextureFormat.BC4 => LoadedTextureFormat.Bc4, 
			TextureFormat.DXT1 => LoadedTextureFormat.Dxt1, 
			TextureFormat.DXT5 => LoadedTextureFormat.Dxt5, 
			_ => LoadedTextureFormat.Rgba32, 
		};
	}

	private static TextureFormat ToTextureFormat(GraphicsFormat format)
	{
		return format switch
		{
			GraphicsFormat.R8_UNorm => TextureFormat.R8, 
			GraphicsFormat.R32_SFloat => TextureFormat.RFloat, 
			GraphicsFormat.R32G32_SFloat => TextureFormat.RGFloat, 
			_ => TextureFormat.RGBA32, 
		};
	}

	private static bool IsLinear(GraphicsFormat format)
	{
		if (format == GraphicsFormat.R8G8B8A8_SRGB)
		{
			return false;
		}
		return true;
	}
}
