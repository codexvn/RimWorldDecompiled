using UnityEngine;
using UnityEngine.UI;

namespace TMPro;

public abstract class TMP_Text : MaskableGraphic
{
	public virtual string text { get; set; } = string.Empty;

	public virtual float fontSize { get; set; } = 1f;

	public virtual TMP_FontAsset font { get; set; }

	public virtual Mesh mesh => null;

	public virtual Bounds bounds { get; protected set; }

	public virtual TMP_TextInfo textInfo { get; protected set; } = new TMP_TextInfo();

	public abstract void ForceMeshUpdate(bool ignoreActiveState = false, bool forceTextReparsing = false);

	public abstract void UpdateVertexData(TMP_VertexDataUpdateFlags flags);
}
