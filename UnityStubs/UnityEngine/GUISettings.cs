namespace UnityEngine;

public class GUISettings
{
	public bool doubleClickSelectsWord { get; set; }

	public Color cursorColor { get; set; } = Color.white;

	public float cursorFlashSpeed { get; set; } = 2f;

	public Color selectionColor { get; set; } = new Color(0.5f, 0.5f, 1f, 1f);
}
