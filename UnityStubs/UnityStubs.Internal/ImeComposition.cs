using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace UnityStubs.Internal;

public static class ImeComposition
{
	public static IMECompositionMode Mode = IMECompositionMode.Auto;

	public static string Text { get; private set; } = "";

	public static int Cursor { get; private set; }

	public static Vector2 CandidatePos { get; private set; }

	public static bool Active => Text.Length > 0;

	public static void Set(string text, int cursor)
	{
		if (text == null)
		{
			text = "";
		}
		cursor = Mathf.Clamp(cursor, 0, text.Length);
		if (!(text == Text) || cursor != Cursor)
		{
			Text = text;
			Cursor = cursor;
			Keyboard.current?.OnIMECompositionChanged(new IMECompositionString(text));
		}
	}

	public static void SetCandidatePos(Vector2 framebufferPoint)
	{
		CandidatePos = framebufferPoint;
	}
}
