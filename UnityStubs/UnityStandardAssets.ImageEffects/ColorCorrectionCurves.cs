using UnityEngine;

namespace UnityStandardAssets.ImageEffects;

public class ColorCorrectionCurves : PostEffectsBase
{
	public enum ColorCorrectionMode
	{
		Simple,
		Advanced
	}

	public AnimationCurve redChannel = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

	public AnimationCurve greenChannel = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

	public AnimationCurve blueChannel = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

	public bool useDepthCorrection;

	public AnimationCurve zCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

	public AnimationCurve depthRedChannel = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

	public AnimationCurve depthGreenChannel = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

	public AnimationCurve depthBlueChannel = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

	public float saturation = 1f;

	public bool selectiveCc;

	public Color selectiveFromColor = Color.white;

	public Color selectiveToColor = Color.white;

	public ColorCorrectionMode mode;

	public bool updateTextures = true;

	public Shader colorCorrectionCurvesShader;

	public Shader simpleColorCorrectionCurvesShader;

	public Shader colorCorrectionSelectiveShader;

	public override bool CheckResources()
	{
		return isSupported;
	}

	public void UpdateParameters()
	{
		updateTextures = false;
	}

	public void UpdateTextures()
	{
		UpdateParameters();
	}
}
