namespace UnityEngine;

public class MeshRenderer : Renderer
{
	public Mesh additionalVertexStreams { get; set; }

	public Mesh enlightenVertexStream { get; set; }

	public int subMeshStartIndex => 0;

	internal void FrameUpdate()
	{
		if (!DrawnByOwner && base.enabled && base.gameObject.activeInHierarchy)
		{
			MeshFilter component = GetComponent<MeshFilter>();
			Mesh mesh = ((component != null) ? component.sharedMesh : null);
			if (mesh != null)
			{
				DrawSubMeshes(mesh, base.transform.localToWorldMatrix);
			}
		}
	}
}
