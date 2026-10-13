using System.Collections.Generic;

namespace UnityStubs.Internal;

internal sealed class UnityShaderCBuffer
{
	public string Name;

	public int Size;

	public readonly List<UnityShaderParam> Params = new List<UnityShaderParam>();
}
