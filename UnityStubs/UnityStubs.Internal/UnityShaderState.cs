using System;
using System.Collections.Generic;

namespace UnityStubs.Internal;

internal sealed class UnityShaderState
{
	public float SrcBlend = 1f;

	public float DstBlend;

	public float SrcBlendAlpha = 1f;

	public float DstBlendAlpha;

	public float BlendOp;

	public float ColorMask = 15f;

	public float ZTest = 4f;

	public float ZWrite = 1f;

	public float Culling = 2f;

	public float OffsetFactor;

	public float OffsetUnits;

	public float StencilRef;

	public float StencilReadMask = 255f;

	public float StencilWriteMask = 255f;

	public float StencilComp = 8f;

	public float StencilPass;

	public float StencilFail;

	public float StencilZFail;

	public readonly Dictionary<string, string> Tags = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
}
