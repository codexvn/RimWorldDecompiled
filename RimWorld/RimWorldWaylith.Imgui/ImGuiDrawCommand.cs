using Brutal.Numerics;

namespace RimWorldWaylith.Imgui;

internal struct ImGuiDrawCommand
{
	public float4 ClipRect;

	public nint TexId;

	public uint ElemCount;

	public int IdxOffset;

	public int VtxOffset;
}
