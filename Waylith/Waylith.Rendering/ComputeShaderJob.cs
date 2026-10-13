using Waylith.Rendering.Compute;

namespace Waylith.Rendering;

public readonly record struct ComputeShaderJob(string SourceFile, string[] EntryPoints, ShaderSourceLanguage Language);
