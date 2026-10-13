using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace UnityEngine;

public struct RenderTextureDescriptor
{
	private int _depthBufferBits;

	private RenderTextureCreationFlags _flags;

	public int width { get; set; }

	public int height { get; set; }

	public int msaaSamples { get; set; }

	public int volumeDepth { get; set; }

	public int mipCount { get; set; }

	public GraphicsFormat graphicsFormat { get; set; }

	public GraphicsFormat stencilFormat { get; set; }

	public GraphicsFormat depthStencilFormat { get; set; }

	public TextureDimension dimension { get; set; }

	public ShadowSamplingMode shadowSamplingMode { get; set; }

	public VRTextureUsage vrUsage { get; set; }

	public RenderTextureMemoryless memoryless { get; set; }

	public RenderTextureCreationFlags flags => _flags;

	public RenderTextureFormat colorFormat
	{
		get
		{
			return RenderTexture.ToRenderTextureFormat(graphicsFormat);
		}
		set
		{
			graphicsFormat = RenderTexture.ToGraphicsFormat(value);
		}
	}

	public bool sRGB
	{
		get
		{
			if (graphicsFormat != GraphicsFormat.R8G8B8A8_SRGB)
			{
				return graphicsFormat == GraphicsFormat.B8G8R8A8_SRGB;
			}
			return true;
		}
		set
		{
			if (value && graphicsFormat == GraphicsFormat.R8G8B8A8_UNorm)
			{
				graphicsFormat = GraphicsFormat.R8G8B8A8_SRGB;
			}
			else if (!value && graphicsFormat == GraphicsFormat.R8G8B8A8_SRGB)
			{
				graphicsFormat = GraphicsFormat.R8G8B8A8_UNorm;
			}
		}
	}

	public int depthBufferBits
	{
		get
		{
			return _depthBufferBits;
		}
		set
		{
			_depthBufferBits = ((value > 0) ? ((value <= 16) ? 16 : ((value <= 24) ? 24 : 32)) : 0);
			depthStencilFormat = _depthBufferBits switch
			{
				0 => GraphicsFormat.None, 
				16 => GraphicsFormat.D16_UNorm, 
				24 => GraphicsFormat.D24_UNorm_S8_UInt, 
				_ => GraphicsFormat.D32_SFloat, 
			};
		}
	}

	public bool useMipMap
	{
		get
		{
			return (_flags & RenderTextureCreationFlags.MipMap) != 0;
		}
		set
		{
			SetOrClear(RenderTextureCreationFlags.MipMap, value);
		}
	}

	public bool autoGenerateMips
	{
		get
		{
			return (_flags & RenderTextureCreationFlags.AutoGenerateMips) != 0;
		}
		set
		{
			SetOrClear(RenderTextureCreationFlags.AutoGenerateMips, value);
		}
	}

	public bool enableRandomWrite
	{
		get
		{
			return (_flags & RenderTextureCreationFlags.EnableRandomWrite) != 0;
		}
		set
		{
			SetOrClear(RenderTextureCreationFlags.EnableRandomWrite, value);
		}
	}

	public bool bindMS
	{
		get
		{
			return (_flags & RenderTextureCreationFlags.BindMS) != 0;
		}
		set
		{
			SetOrClear(RenderTextureCreationFlags.BindMS, value);
		}
	}

	internal bool createdFromScript
	{
		get
		{
			return (_flags & RenderTextureCreationFlags.CreatedFromScript) != 0;
		}
		set
		{
			SetOrClear(RenderTextureCreationFlags.CreatedFromScript, value);
		}
	}

	public bool useDynamicScale
	{
		get
		{
			return (_flags & RenderTextureCreationFlags.DynamicallyScalable) != 0;
		}
		set
		{
			SetOrClear(RenderTextureCreationFlags.DynamicallyScalable, value);
		}
	}

	public RenderTextureDescriptor(int width, int height)
		: this(width, height, RenderTextureFormat.Default, 0)
	{
	}

	public RenderTextureDescriptor(int width, int height, RenderTextureFormat colorFormat)
		: this(width, height, colorFormat, 0)
	{
	}

	public RenderTextureDescriptor(int width, int height, RenderTextureFormat colorFormat, int depthBufferBits)
		: this(width, height, colorFormat, depthBufferBits, -1)
	{
	}

	public RenderTextureDescriptor(int width, int height, GraphicsFormat colorFormat, int depthBufferBits)
		: this(width, height, colorFormat, depthBufferBits, -1)
	{
	}

	public RenderTextureDescriptor(int width, int height, RenderTextureFormat colorFormat, int depthBufferBits, int mipCount)
		: this(width, height, RenderTexture.ToGraphicsFormat(colorFormat), depthBufferBits, mipCount)
	{
	}

	public RenderTextureDescriptor(int width, int height, RenderTextureFormat colorFormat, int depthBufferBits, int mipCount, RenderTextureReadWrite readWrite)
		: this(width, height, RenderTexture.ToGraphicsFormat(colorFormat), depthBufferBits, mipCount)
	{
		sRGB = readWrite == RenderTextureReadWrite.sRGB;
	}

	public RenderTextureDescriptor(int width, int height, GraphicsFormat colorFormat, GraphicsFormat depthStencilFormat)
		: this(width, height, colorFormat, depthStencilFormat, -1)
	{
	}

	public RenderTextureDescriptor(int width, int height, GraphicsFormat colorFormat, int depthBufferBits, int mipCount)
	{
		this = default(RenderTextureDescriptor);
		this.width = width;
		this.height = height;
		volumeDepth = 1;
		msaaSamples = 1;
		graphicsFormat = colorFormat;
		this.depthBufferBits = depthBufferBits;
		this.mipCount = mipCount;
		dimension = TextureDimension.Tex2D;
		shadowSamplingMode = ShadowSamplingMode.None;
		vrUsage = VRTextureUsage.None;
		memoryless = RenderTextureMemoryless.None;
		_flags = RenderTextureCreationFlags.AutoGenerateMips | RenderTextureCreationFlags.AllowVerticalFlip;
	}

	public RenderTextureDescriptor(int width, int height, GraphicsFormat colorFormat, GraphicsFormat depthStencilFormat, int mipCount)
		: this(width, height, colorFormat, 0, mipCount)
	{
		this.depthStencilFormat = depthStencilFormat;
		_depthBufferBits = depthStencilFormat switch
		{
			GraphicsFormat.None => 0, 
			GraphicsFormat.D16_UNorm => 16, 
			GraphicsFormat.D24_UNorm_S8_UInt => 24, 
			_ => 32, 
		};
	}

	private void SetOrClear(RenderTextureCreationFlags flag, bool value)
	{
		if (value)
		{
			_flags |= flag;
		}
		else
		{
			_flags &= ~flag;
		}
	}
}
