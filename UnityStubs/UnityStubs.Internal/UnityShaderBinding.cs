namespace UnityStubs.Internal;

internal sealed class UnityShaderBinding
{
	public const int VertexStage = 4;

	public const int FragmentStage = 8;

	public string Name;

	public UnityShaderBindingKind Kind;

	public int PackedIndex;

	public int PackedSampler = -1;

	public int Stages => (PackedIndex >> 24) & 0xFF;

	public int Set => (PackedIndex >> 16) & 0xFF;

	public int Binding => PackedIndex & 0xFFFF;

	public bool HasSeparateSampler => PackedSampler != -1;

	public int SamplerSet => (PackedSampler >> 16) & 0xFF;

	public int SamplerBinding => PackedSampler & 0xFFFF;
}
