using System.Numerics;
using UnityEngine;
using Waylith.Rendering;

namespace RimWorldWaylith.Rendering;

internal static class UnityBuiltinGlobals
{
	private static readonly int TimeId = UnityEngine.Shader.PropertyToID("_Time");

	private static readonly int ScreenParamsId = UnityEngine.Shader.PropertyToID("_ScreenParams");

	private static readonly int ProjectionId = UnityEngine.Shader.PropertyToID("glstate_matrix_projection");

	private static readonly int WorldSpaceCameraPosId = UnityEngine.Shader.PropertyToID("_WorldSpaceCameraPos");

	private static readonly int ProjectionParamsId = UnityEngine.Shader.PropertyToID("_ProjectionParams");

	private static readonly int InverseViewId = UnityEngine.Shader.PropertyToID("unity_MatrixInvV");

	public static void PublishScreen()
	{
		PublishTimeAndScreen(Screen.width, Screen.height);
	}

	public static void Publish(Camera camera, System.Numerics.Matrix4x4 view, System.Numerics.Matrix4x4 viewProjection)
	{
		PublishTimeAndScreen(camera.pixelWidth, camera.pixelHeight);
		UnityEngine.Vector3 position = camera.transform.position;
		GlobalShaderState.SetVector(WorldSpaceCameraPosId, new System.Numerics.Vector4(position.x, position.y, position.z, 1f));
		GlobalShaderState.SetVector(ProjectionParamsId, new System.Numerics.Vector4(-1f, camera.nearClipPlane, camera.farClipPlane, 1f / camera.farClipPlane));
		if (System.Numerics.Matrix4x4.Invert(view, out var result))
		{
			GlobalShaderState.SetMatrix(ProjectionId, viewProjection * result);
			GlobalShaderState.SetMatrix(InverseViewId, result);
		}
	}

	private static void PublishTimeAndScreen(float screenW, float screenH)
	{
		float time = Time.time;
		GlobalShaderState.SetVector(TimeId, new System.Numerics.Vector4(time / 20f, time, time * 2f, time * 3f));
		GlobalShaderState.SetVector(ScreenParamsId, new System.Numerics.Vector4(screenW, screenH, 1f + 1f / screenW, 1f + 1f / screenH));
	}
}
