using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Rendering;
using UnityStubs.Internal;
using UnityStubs.Rendering;
using Waylith.Rendering;

namespace UnityEngine;

public class Camera : Behaviour
{
	public enum StereoscopicEye
	{
		Left,
		Right
	}

	public enum MonoOrStereoscopicEye
	{
		Left = 0,
		Mono = 2,
		Right = 1
	}

	public delegate void CameraCallback(Camera cam);

	private float _aspect;

	private CameraClearFlags _clearFlags = CameraClearFlags.Skybox;

	private float _depth;

	private DepthTextureMode _depthTextureMode;

	private float _farClipPlane = 1000f;

	private float _fieldOfView = 60f;

	private bool _orthographic;

	private float _orthographicSize = 5f;

	private Rect _rect = new Rect(0f, 0f, 1f, 1f);

	private RenderTexture _targetTexture;

	private int _targetDisplay;

	private float _nearClipPlane = 0.3f;

	private int _cullingMask = -1;

	private Color _backgroundColor = new Color(0.19215687f, 0.3019608f, 0.47058824f, 0f);

	private CameraType _cameraType = CameraType.Game;

	public const float kMaxAperture = 32f;

	public const int kMaxBladeCount = 11;

	public const float kMinAperture = 0.7f;

	public const int kMinBladeCount = 3;

	private Matrix4x4? _projectionOverride;

	private Matrix4x4? _viewOverride;

	private Matrix4x4? _cullingOverride;

	public static CameraCallback onPostRender;

	public static CameraCallback onPreRender;

	public static CameraCallback onPreCull;

	private static readonly Dictionary<(Type, string), MethodInfo> CameraMessageCache = new Dictionary<(Type, string), MethodInfo>();

	public bool allowHDR { get; set; } = true;

	public RenderTexture activeTexture => targetTexture;

	public static int allCamerasCount => Object.FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None).Length;

	public float aspect
	{
		get
		{
			if (!(_aspect > 0f))
			{
				return (float)pixelWidth / (float)Math.Max(1, pixelHeight);
			}
			return _aspect;
		}
		set
		{
			_aspect = value;
		}
	}

	public Matrix4x4 cameraToWorldMatrix
	{
		get
		{
			if (!_viewOverride.HasValue)
			{
				return Matrix4x4.Translate(base.transform.position) * Matrix4x4.Rotate(base.transform.rotation) * Matrix4x4.Scale(new Vector3(1f, 1f, -1f));
			}
			return _viewOverride.Value.inverse;
		}
	}

	public CameraType cameraType
	{
		get
		{
			return _cameraType;
		}
		set
		{
			_cameraType = value;
		}
	}

	public CameraClearFlags clearFlags
	{
		get
		{
			return _clearFlags;
		}
		set
		{
			_clearFlags = value;
		}
	}

	public float depth
	{
		get
		{
			return _depth;
		}
		set
		{
			_depth = value;
		}
	}

	public DepthTextureMode depthTextureMode
	{
		get
		{
			return _depthTextureMode;
		}
		set
		{
			_depthTextureMode = value;
		}
	}

	public float farClipPlane
	{
		get
		{
			return _farClipPlane;
		}
		set
		{
			_farClipPlane = value;
		}
	}

	public float fieldOfView
	{
		get
		{
			return _fieldOfView;
		}
		set
		{
			_fieldOfView = value;
		}
	}

	public static Camera current => main;

	public bool forceIntoRenderTexture { get; set; }

	public static Camera main
	{
		get
		{
			GameObject gameObject = GameObject.Find("Camera");
			if (!(gameObject != null))
			{
				return null;
			}
			return gameObject.GetComponent<Camera>();
		}
	}

	public float nearClipPlane
	{
		get
		{
			return _nearClipPlane;
		}
		set
		{
			_nearClipPlane = value;
		}
	}

	public bool orthographic
	{
		get
		{
			return _orthographic;
		}
		set
		{
			_orthographic = value;
		}
	}

	public float orthographicSize
	{
		get
		{
			return _orthographicSize;
		}
		set
		{
			_orthographicSize = value;
		}
	}

	public int pixelHeight
	{
		get
		{
			RenderTexture renderTexture = targetTexture;
			if ((object)renderTexture == null || renderTexture.height <= 0)
			{
				return Math.Max(1, (int)((float)Screen.height * rect.height));
			}
			return targetTexture.height;
		}
	}

	public int pixelWidth
	{
		get
		{
			RenderTexture renderTexture = targetTexture;
			if ((object)renderTexture == null || renderTexture.width <= 0)
			{
				return Math.Max(1, (int)((float)Screen.width * rect.width));
			}
			return targetTexture.width;
		}
	}

	public Rect pixelRect
	{
		get
		{
			if (!(targetTexture != null))
			{
				return new Rect((float)Screen.width * rect.x, (float)Screen.height * rect.y, pixelWidth, pixelHeight);
			}
			return new Rect(0f, 0f, pixelWidth, pixelHeight);
		}
		set
		{
			rect = new Rect(value.x / (float)Math.Max(1, Screen.width), value.y / (float)Math.Max(1, Screen.height), value.width / (float)Math.Max(1, Screen.width), value.height / (float)Math.Max(1, Screen.height));
		}
	}

	public Matrix4x4 projectionMatrix
	{
		get
		{
			if (_projectionOverride.HasValue)
			{
				return _projectionOverride.Value;
			}
			if (orthographic)
			{
				float num = orthographicSize;
				float num2 = num * aspect;
				return Matrix4x4.Ortho(0f - num2, num2, 0f - num, num, nearClipPlane, farClipPlane);
			}
			return Matrix4x4.Perspective(fieldOfView, aspect, nearClipPlane, farClipPlane);
		}
		set
		{
			_projectionOverride = value;
		}
	}

	public Rect rect
	{
		get
		{
			return _rect;
		}
		set
		{
			_rect = value;
		}
	}

	public MonoOrStereoscopicEye stereoActiveEye
	{
		get
		{
			StubWarner.LogStub("stereoActiveEye", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Camera.cs", 144);
			return MonoOrStereoscopicEye.Left;
		}
	}

	public bool stereoEnabled
	{
		get
		{
			StubWarner.LogStub("stereoEnabled", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Camera.cs", 145);
			return false;
		}
	}

	public int targetDisplay
	{
		get
		{
			return _targetDisplay;
		}
		set
		{
			_targetDisplay = value;
		}
	}

	public RenderTexture targetTexture
	{
		get
		{
			return _targetTexture;
		}
		set
		{
			_targetTexture = value;
		}
	}

	public Matrix4x4 worldToCameraMatrix
	{
		get
		{
			return _viewOverride ?? (Matrix4x4.Scale(new Vector3(1f, 1f, -1f)) * Matrix4x4.Rotate(Quaternion.Inverse(base.transform.rotation)) * Matrix4x4.Translate(-base.transform.position));
		}
		set
		{
			_viewOverride = value;
		}
	}

	public Matrix4x4 cullingMatrix
	{
		get
		{
			return _cullingOverride ?? (projectionMatrix * worldToCameraMatrix);
		}
		set
		{
			_cullingOverride = value;
		}
	}

	public static Camera[] allCameras => Object.FindObjectsByType<Camera>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

	public bool allowDynamicResolution { get; set; }

	public int commandBufferCount => 0;

	public bool allowMSAA
	{
		set
		{
		}
	}

	public Color backgroundColor
	{
		get
		{
			return _backgroundColor;
		}
		set
		{
			_backgroundColor = value;
		}
	}

	public int cullingMask
	{
		get
		{
			return _cullingMask;
		}
		set
		{
			_cullingMask = value;
		}
	}

	public OpaqueSortMode opaqueSortMode
	{
		set
		{
		}
	}

	public RenderingPath renderingPath { get; set; }

	public bool useOcclusionCulling { get; set; }

	public void CopyFrom(Camera other)
	{
		if (!(other == null))
		{
			_aspect = other._aspect;
			_clearFlags = other._clearFlags;
			_depth = other._depth;
			_depthTextureMode = other._depthTextureMode;
			_farClipPlane = other._farClipPlane;
			_fieldOfView = other._fieldOfView;
			_orthographic = other._orthographic;
			_orthographicSize = other._orthographicSize;
			_rect = other._rect;
			_targetTexture = other._targetTexture;
			_targetDisplay = other._targetDisplay;
			_nearClipPlane = other._nearClipPlane;
			_cullingMask = other._cullingMask;
			_backgroundColor = other._backgroundColor;
			allowHDR = other.allowHDR;
			renderingPath = other.renderingPath;
			useOcclusionCulling = other.useOcclusionCulling;
			base.transform.position = other.transform.position;
			base.transform.rotation = other.transform.rotation;
		}
	}

	public Vector3 WorldToViewportPoint(Vector3 position)
	{
		Vector4 vector = worldToCameraMatrix * new Vector4(position.x, position.y, position.z, 1f);
		Vector4 vector2 = projectionMatrix * vector;
		if (MathF.Abs(vector2.w) <= 1E-06f)
		{
			return Vector3.zero;
		}
		float num = 1f / vector2.w;
		return new Vector3((vector2.x * num + 1f) * 0.5f, ((0f - vector2.y) * num + 1f) * 0.5f, 0f - vector.z);
	}

	public Vector3 ViewportToScreenPoint(Vector3 position)
	{
		float num = ((targetTexture == null) ? ((float)Screen.width * rect.x) : 0f);
		return new Vector3(y: ((targetTexture == null) ? ((float)Screen.height * rect.y) : 0f) + position.y * (float)pixelHeight, x: num + position.x * (float)pixelWidth, z: position.z);
	}

	public void ResetAspect()
	{
		_aspect = 0f;
	}

	public void ResetProjectionMatrix()
	{
		_projectionOverride = null;
	}

	internal void ScreenPointToRay_Injected(ref Vector2 pos, MonoOrStereoscopicEye eye, out Ray ret)
	{
		ret = ScreenPointToRay(new Vector3(pos.x, pos.y, 0f));
	}

	public void ResetWorldToCameraMatrix()
	{
		_viewOverride = null;
	}

	public void ResetCullingMatrix()
	{
		_cullingOverride = null;
	}

	public void AddCommandBuffer(CameraEvent evt, CommandBuffer buffer)
	{
		StubWarner.LogStub("AddCommandBuffer", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Camera.cs", 163);
	}

	public void AddCommandBufferAsync(CameraEvent evt, CommandBuffer buffer, ComputeQueueType queueType)
	{
		StubWarner.LogStub("AddCommandBufferAsync", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Camera.cs", 164);
	}

	public void RemoveCommandBuffer(CameraEvent evt, CommandBuffer buffer)
	{
	}

	public void RemoveCommandBuffers(CameraEvent evt)
	{
	}

	public void RemoveAllCommandBuffers()
	{
	}

	public CommandBuffer[] GetCommandBuffers(CameraEvent evt)
	{
		return Array.Empty<CommandBuffer>();
	}

	public Matrix4x4 GetStereoProjectionMatrix(StereoscopicEye eye)
	{
		StubWarner.LogStub("GetStereoProjectionMatrix", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Camera.cs", 169);
		return default(Matrix4x4);
	}

	public Matrix4x4 GetStereoViewMatrix(StereoscopicEye eye)
	{
		StubWarner.LogStub("GetStereoViewMatrix", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\Camera.cs", 170);
		return default(Matrix4x4);
	}

	public void Render()
	{
		RenderTexture renderTexture = _targetTexture;
		ICameraTextureRenderer cameraTextureRenderer = CameraTextureRenderer.Current;
		if (renderTexture == null || cameraTextureRenderer == null)
		{
			onPreCull?.Invoke(this);
			InvokeCameraMessage("OnPreCull");
			onPreRender?.Invoke(this);
			InvokeCameraMessage("OnPreRender");
			onPostRender?.Invoke(this);
			InvokeCameraMessage("OnPostRender");
			return;
		}
		List<MeshDrawCommand> list = new List<MeshDrawCommand>();
		MeshDrawSubmission.CopyPendingCommands(list, _cullingMask);
		List<MeshDrawCommand> immediateCapture = Graphics.ImmediateCapture;
		Graphics.ImmediateCapture = list;
		try
		{
			onPreCull?.Invoke(this);
			InvokeCameraMessage("OnPreCull");
			onPreRender?.Invoke(this);
			InvokeCameraMessage("OnPreRender");
			onPostRender?.Invoke(this);
			InvokeCameraMessage("OnPostRender");
		}
		finally
		{
			Graphics.ImmediateCapture = immediateCapture;
		}
		Color? clearColor = ((_clearFlags == CameraClearFlags.Skybox || _clearFlags == CameraClearFlags.Color) ? new Color?(_backgroundColor) : ((Color?)null));
		cameraTextureRenderer.Render(renderTexture, list, CameraViewFactory.GetView(this), _rect, clearColor);
	}

	internal static void DispatchPreCullForScreenCameras()
	{
		Camera[] array = Object.FindObjectsByType<Camera>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
		foreach (Camera camera in array)
		{
			if (camera != null && camera.enabled && camera._targetTexture == null)
			{
				onPreCull?.Invoke(camera);
				camera.InvokeCameraMessage("OnPreCull");
			}
		}
	}

	private void InvokeCameraMessage(string method)
	{
		GameObject gameObject = base.gameObject;
		if (gameObject == null)
		{
			return;
		}
		MonoBehaviour[] components = gameObject.GetComponents<MonoBehaviour>();
		if (components == null)
		{
			return;
		}
		MonoBehaviour[] array = components;
		foreach (MonoBehaviour monoBehaviour in array)
		{
			if (!(monoBehaviour == null))
			{
				Type type = monoBehaviour.GetType();
				if (!CameraMessageCache.TryGetValue((type, method), out var value))
				{
					value = type.GetMethod(method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
					CameraMessageCache[(type, method)] = value;
				}
				value?.Invoke(monoBehaviour, null);
			}
		}
	}

	public void RenderWithShader(Shader shader, string replacementTag)
	{
		Render();
	}

	public Ray ScreenPointToRay(Vector3 pos)
	{
		float num = ((targetTexture == null) ? ((float)Screen.width * rect.x) : 0f);
		float num2 = ((targetTexture == null) ? ((float)Screen.height * rect.y) : 0f);
		float num3 = pos.x - num;
		float num4 = pos.y - num2;
		float x = num3 / Math.Max(1f, pixelWidth) * 2f - 1f;
		float y = 1f - num4 / Math.Max(1f, pixelHeight) * 2f;
		Matrix4x4 inverse = (projectionMatrix * worldToCameraMatrix).inverse;
		Vector4 vector = inverse * new Vector4(x, y, 1f, 1f);
		Vector4 vector2 = inverse * new Vector4(x, y, 0f, 1f);
		if (MathF.Abs(vector.w) > 1E-06f)
		{
			vector = new Vector4(vector.x / vector.w, vector.y / vector.w, vector.z / vector.w, 1f);
		}
		if (MathF.Abs(vector2.w) > 1E-06f)
		{
			vector2 = new Vector4(vector2.x / vector2.w, vector2.y / vector2.w, vector2.z / vector2.w, 1f);
		}
		Vector3 vector3 = new Vector3(vector.x, vector.y, vector.z);
		Vector3 vector4 = new Vector3(vector2.x, vector2.y, vector2.z);
		return new Ray(vector3, vector4 - vector3);
	}

	public void SetTargetBuffers(RenderBuffer colorBuffer, RenderBuffer depthBuffer)
	{
		RenderTexture renderTexture = colorBuffer.RenderTexture ?? depthBuffer.RenderTexture;
		if (renderTexture != null)
		{
			_targetTexture = renderTexture;
		}
	}

	public Vector3 WorldToScreenPoint(Vector3 position)
	{
		Vector4 vector = new Vector4(position.x, position.y, position.z, 1f);
		Vector4 vector2 = worldToCameraMatrix * vector;
		Vector4 vector3 = projectionMatrix * vector2;
		if (MathF.Abs(vector3.w) <= 1E-06f)
		{
			return Vector3.zero;
		}
		float num = 1f / vector3.w;
		float num2 = vector3.x * num;
		float num3 = vector3.y * num;
		float num4 = ((targetTexture == null) ? ((float)Screen.width * rect.x) : 0f);
		float num5 = ((targetTexture == null) ? ((float)Screen.height * rect.y) : 0f);
		float x = num4 + (num2 + 1f) * 0.5f * (float)pixelWidth;
		float y = num5 + (1f - num3) * 0.5f * (float)pixelHeight;
		return new Vector3(x, y, 0f - vector2.z);
	}

	public Vector3 ViewportToWorldPoint(Vector3 position, MonoOrStereoscopicEye eye)
	{
		return ViewportToWorldPoint(position);
	}

	public Ray ViewportPointToRay(Vector3 pos, MonoOrStereoscopicEye eye)
	{
		return ViewportPointToRay(pos);
	}

	public Vector3 ViewportToWorldPoint(Vector3 position)
	{
		UnprojectViewport(position, out var nearPoint, out var farPoint);
		float num = farClipPlane - nearClipPlane;
		float num2 = ((MathF.Abs(num) <= 1E-06f) ? 0f : ((position.z - nearClipPlane) / num));
		return nearPoint + (farPoint - nearPoint) * num2;
	}

	public Ray ViewportPointToRay(Vector3 pos)
	{
		UnprojectViewport(pos, out var nearPoint, out var farPoint);
		return new Ray(nearPoint, farPoint - nearPoint);
	}

	private void UnprojectViewport(Vector3 viewport, out Vector3 nearPoint, out Vector3 farPoint)
	{
		float x = viewport.x * 2f - 1f;
		float y = 1f - viewport.y * 2f;
		Matrix4x4 inverse = (projectionMatrix * worldToCameraMatrix).inverse;
		nearPoint = Divide(inverse * new Vector4(x, y, 1f, 1f));
		farPoint = Divide(inverse * new Vector4(x, y, 0f, 1f));
		static Vector3 Divide(Vector4 clip)
		{
			float num = ((MathF.Abs(clip.w) > 1E-06f) ? (1f / clip.w) : 1f);
			return new Vector3(clip.x * num, clip.y * num, clip.z * num);
		}
	}
}
