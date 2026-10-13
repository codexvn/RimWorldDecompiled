namespace Waylith.Rendering;

public readonly record struct GraphicsShaderJob(string SourceFile, RuntimeShaderStage Stage, ShaderVariantKey Variant);
