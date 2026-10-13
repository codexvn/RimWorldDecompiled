using UnityEngine;
using UnityStubs.Internal;

namespace UnityStandardAssets.ImageEffects;

public class PostEffectsBase : MonoBehaviour
{
	protected bool supportHDRTextures = true;

	protected bool supportDX11;

	protected bool isSupported = true;

	protected Material CheckShaderAndCreateMaterial(Shader s, Material m2Create)
	{
		if (s == null)
		{
			NotSupported();
			return null;
		}
		if (m2Create != null && m2Create.shader == s)
		{
			return m2Create;
		}
		return CreateMaterial(s, m2Create);
	}

	protected Material CreateMaterial(Shader s, Material m2Create)
	{
		if (s == null)
		{
			return null;
		}
		if (m2Create != null && m2Create.shader == s)
		{
			return m2Create;
		}
		return new Material(s)
		{
			hideFlags = HideFlags.DontSave
		};
	}

	protected bool CheckSupport()
	{
		return CheckSupport(needDepth: false);
	}

	protected bool CheckSupport(bool needDepth)
	{
		return CheckSupport(needDepth, needHdr: false);
	}

	protected bool CheckSupport(bool needDepth, bool needHdr)
	{
		isSupported = true;
		supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
		supportDX11 = SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders;
		return isSupported;
	}

	public virtual bool CheckResources()
	{
		return isSupported;
	}

	public bool Dx11Support()
	{
		return supportDX11;
	}

	protected void Start()
	{
		CheckResources();
	}

	protected void ReportAutoDisable()
	{
		Debug.LogWarning($"The image effect {this} has been disabled as it's not supported on the current platform.");
	}

	protected void NotSupported()
	{
		base.enabled = false;
		isSupported = false;
	}

	protected void DrawBorder(RenderTexture dest, Material material)
	{
		StubWarner.LogStub("DrawBorder", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\RimWorldPort\\UnityStandardAssets.cs", 57);
	}
}
