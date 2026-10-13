namespace UnityEngine;

public class MeshFilter : Component
{
	private Mesh _sharedMesh;

	public Mesh sharedMesh
	{
		get
		{
			return _sharedMesh;
		}
		set
		{
			_sharedMesh = value;
		}
	}

	public Mesh mesh
	{
		get
		{
			return _sharedMesh ?? (_sharedMesh = new Mesh());
		}
		set
		{
			_sharedMesh = value;
		}
	}
}
