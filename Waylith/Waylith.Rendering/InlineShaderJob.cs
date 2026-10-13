namespace Waylith.Rendering;

public readonly record struct InlineShaderJob(string Source, string DiagnosticName, RuntimeShaderStage Stage);
