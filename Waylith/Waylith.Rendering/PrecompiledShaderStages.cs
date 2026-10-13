namespace Waylith.Rendering;

public sealed record PrecompiledShaderStages(byte[] Vertex, byte[] Fragment, bool ScaleOffsetTextureST = false);
