using System;
using System.Collections.Generic;

namespace UnityStubs.Internal;

internal sealed class UnityShaderPass
{
	public UnityShaderState State;

	public byte[] VertexSpirv;

	public byte[] FragmentSpirv;

	public string[] Keywords = Array.Empty<string>();

	public readonly List<UnityShaderCBuffer> CBuffers = new List<UnityShaderCBuffer>();

	public readonly List<UnityShaderBinding> Bindings = new List<UnityShaderBinding>();

	public readonly List<(int channel, int location)> VertexChannels = new List<(int, int)>();
}
