using System.Numerics;
using UnityEngine;
using Waylith.Rendering;

namespace UnityStubs.Rendering;

public static class CameraViewFactory
{
	public static CameraView GetView(Camera cam)
	{
		System.Numerics.Matrix4x4 matrix4x = UnityEngine.Matrix4x4.ToNumerics(cam.projectionMatrix);
		System.Numerics.Matrix4x4 matrix4x2 = UnityEngine.Matrix4x4.ToNumerics(cam.worldToCameraMatrix);
		return new CameraView(matrix4x2, matrix4x, matrix4x * matrix4x2, cam.cullingMask);
	}

	public static bool TryGetView(Camera cam, out CameraView view)
	{
		if (cam == null)
		{
			view = default(CameraView);
			return false;
		}
		view = GetView(cam);
		return true;
	}
}
