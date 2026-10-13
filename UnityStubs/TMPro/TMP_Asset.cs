using UnityEngine;

namespace TMPro;

public abstract class TMP_Asset : ScriptableObject
{
	public int hashCode;

	public Material material;

	public int materialHashCode;

	public int instanceID => GetInstanceID();
}
