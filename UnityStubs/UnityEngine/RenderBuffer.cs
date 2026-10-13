using System;

namespace UnityEngine;

public struct RenderBuffer
{
	internal RenderTexture RenderTexture { get; }

	internal RenderBuffer(RenderTexture owner)
	{
		RenderTexture = owner;
	}

	public nint GetNativeRenderBufferPtr()
	{
		return RenderTexture?.GetNativeTexturePtr() ?? IntPtr.Zero;
	}
}
