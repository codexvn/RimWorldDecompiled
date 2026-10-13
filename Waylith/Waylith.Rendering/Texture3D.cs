using System;
using System.IO;
using System.Runtime.InteropServices;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Waylith.Math;

namespace Waylith.Rendering;

public class Texture3D : Texture
{
	private int _width;

	private int _height;

	private readonly int _depth;

	public override int width
	{
		get
		{
			return _width;
		}
		set
		{
			_width = value;
		}
	}

	public override int height
	{
		get
		{
			return _height;
		}
		set
		{
			_height = value;
		}
	}

	public int depth => _depth;

	public override bool IsReadable => CpuData?.HasData ?? false;

	public TexturePixelData? CpuData { get; private set; }

	public Texture3D(int width, int height, int depth)
	{
		_width = width;
		_height = height;
		_depth = depth;
	}

	public Texture3D(int width, int height, int depth, LoadedTextureFormat format, bool linear, int mipCount)
		: this(width, height, depth)
	{
		base.MipCount = System.Math.Max(1, mipCount);
		CpuData = new TexturePixelData(width, height, format, linear, base.MipCount, allocate: true, depth);
	}

	public static Texture3D? LoadLutFromFile(string filePath)
	{
		LoadedTextureData loadedTexture;
		bool num;
		if (!File.Exists(filePath))
		{
			if (!GpuTextureService.TryLoadTextureFromCache(filePath, premultiplied: false, out loadedTexture))
			{
				goto IL_003c;
			}
			num = loadedTexture.Format != LoadedTextureFormat.Rgba32;
		}
		else
		{
			num = !GpuTextureService.TryLoadTextureFromMemory(File.ReadAllBytes(filePath), out loadedTexture);
		}
		if (!num)
		{
			int num2 = (int)System.Math.Round(System.Math.Cbrt(loadedTexture.Width));
			if (loadedTexture.Width != loadedTexture.Height || num2 * num2 * num2 != loadedTexture.Width)
			{
				LogCategory log = RenderLog.Log;
				bool shouldAppend;
				LogString<Error> message = new LogString<Error>(69, 3, log, out shouldAppend);
				if (shouldAppend)
				{
					message.AppendLiteral("Texture3D '");
					message.AppendFormatted(Path.GetFileNameWithoutExtension(filePath));
					message.AppendLiteral("' is not a square cubic-LUT flipbook (");
					message.AppendFormatted(loadedTexture.Width);
					message.AppendLiteral("x");
					message.AppendFormatted(loadedTexture.Height);
					message.AppendLiteral("); cannot slice it.");
				}
				log.Error(message, "LoadLutFromFile", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\Texture2D.cs", 722);
				return null;
			}
			int num3 = num2;
			int num4 = num2;
			int num5 = num3 * num4;
			int num6 = loadedTexture.Width / num4;
			int num7 = loadedTexture.Height / num3;
			Texture3D texture3D = new Texture3D(num6, num7, num5, LoadedTextureFormat.Rgba32, linear: false, 1)
			{
				Name = Path.GetFileNameWithoutExtension(filePath)
			};
			byte[] sourceArray = loadedTexture.Levels[0];
			byte[] array = new byte[num6 * num7 * num5 * 4];
			for (int i = 0; i < num5; i++)
			{
				int num8 = i % num4;
				int num9 = i / num4;
				int num10 = num8 * num6;
				int num11 = num9 * num7;
				for (int j = 0; j < num7; j++)
				{
					int sourceIndex = ((num11 + j) * loadedTexture.Width + num10) * 4;
					int destinationIndex = (i * num7 + j) * num6 * 4;
					Array.Copy(sourceArray, sourceIndex, array, destinationIndex, num6 * 4);
				}
			}
			Color32[] pixels = MemoryMarshal.Cast<byte, Color32>((ReadOnlySpan<byte>)array).ToArray();
			texture3D.CpuData.SetPixels32(pixels);
			texture3D.Apply();
			return texture3D;
		}
		goto IL_003c;
		IL_003c:
		return null;
	}

	public void Apply(bool updateMipmaps = true, bool makeNoLongerReadable = false)
	{
		if (CpuData != null)
		{
			GpuTextureAllocation gpuResource = base.GpuResource;
			if (gpuResource != null)
			{
				GpuTextureService.DeferDispose(gpuResource);
				base.GpuResource = null;
			}
			CpuData.Name = base.Name;
			base.GpuResource = GpuTextureService.CreateTexture3D(string.IsNullOrEmpty(base.Name) ? "Texture3D" : base.Name, _width, _height, _depth, base.MipCount, TexturePixelData.ToVkFormat(CpuData.Format), CpuData.BuildLevelUploads(), base.MemoryCategory);
			CpuData.TrackCurrentBytes();
			if (makeNoLongerReadable)
			{
				CpuData.ReleaseTracked();
				CpuData.Release();
			}
		}
	}

	public override void Dispose()
	{
		CpuData?.ReleaseTracked();
		CpuData?.Release();
		GpuTextureAllocation gpuResource = base.GpuResource;
		if (gpuResource != null)
		{
			GpuTextureService.DeferDispose(gpuResource);
			base.GpuResource = null;
		}
		base.Dispose();
		GC.SuppressFinalize(this);
	}
}
