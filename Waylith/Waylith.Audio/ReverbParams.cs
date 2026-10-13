namespace Waylith.Audio;

public struct ReverbParams
{
	public float dryLevelMb;

	public float roomMb;

	public float reflectionsLevelMb;

	public float roomHfMb;

	public float roomLfMb;

	public float decayTimeSeconds;

	public float decayHfRatio;

	public float reflectionsDelaySeconds;

	public float reverbLevelMb;

	public float reverbDelaySeconds;

	public float hfReferenceHz;

	public float lfReferenceHz;

	public float diffusionPercent;

	public float densityPercent;

	public static ReverbParams Default
	{
		get
		{
			ReverbParams result = default(ReverbParams);
			result.dryLevelMb = 0f;
			result.roomMb = 0f;
			result.reflectionsLevelMb = -10000f;
			result.roomHfMb = 0f;
			result.roomLfMb = 0f;
			result.decayTimeSeconds = 1f;
			result.decayHfRatio = 0.5f;
			result.reflectionsDelaySeconds = 0f;
			result.reverbLevelMb = 0f;
			result.reverbDelaySeconds = 0.04f;
			result.hfReferenceHz = 5000f;
			result.lfReferenceHz = 250f;
			result.diffusionPercent = 100f;
			result.densityPercent = 100f;
			return result;
		}
	}
}
