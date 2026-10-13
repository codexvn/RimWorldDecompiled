namespace UnityEngine.UI;

public interface IClippable
{
	GameObject gameObject { get; }

	RectTransform rectTransform { get; }

	void Cull(Rect clipRect, bool validRect);

	void RecalculateClipping();

	void SetClipRect(Rect value, bool validRect);

	void SetClipSoftness(Vector2 clipSoftness);
}
