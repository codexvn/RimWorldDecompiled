namespace UnityEngine;

public struct CombineInstance
{
	public Mesh mesh { get; set; }

	public int subMeshIndex { get; set; }

	public Matrix4x4 transform { get; set; }
}
