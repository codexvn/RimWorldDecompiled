using System.Numerics;

namespace Waylith.Rendering;

public readonly record struct CameraView(Matrix4x4 View, Matrix4x4 Projection, Matrix4x4 ViewProjection, int CullingMask);
