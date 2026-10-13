namespace UnityEngine.Rendering;

public enum MeshUpdateFlags
{
	Default = 0,
	DontNotifyMeshUsers = 4,
	DontRecalculateBounds = 8,
	DontResetBoneBounds = 2,
	DontValidateIndices = 1,
	DontValidateLodRanges = 16
}
