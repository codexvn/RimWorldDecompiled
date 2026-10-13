using System;
using Brutal.GlfwApi;

namespace Waylith.Input;

internal static class GlfwKeyMap
{
	private static readonly GlfwKey[] reverse = BuildReverse();

	public static Key ToKey(GlfwKey key)
	{
		return key switch
		{
			GlfwKey.A => Key.A, 
			GlfwKey.B => Key.B, 
			GlfwKey.C => Key.C, 
			GlfwKey.D => Key.D, 
			GlfwKey.E => Key.E, 
			GlfwKey.F => Key.F, 
			GlfwKey.G => Key.G, 
			GlfwKey.H => Key.H, 
			GlfwKey.I => Key.I, 
			GlfwKey.J => Key.J, 
			GlfwKey.K => Key.K, 
			GlfwKey.L => Key.L, 
			GlfwKey.M => Key.M, 
			GlfwKey.N => Key.N, 
			GlfwKey.O => Key.O, 
			GlfwKey.P => Key.P, 
			GlfwKey.Q => Key.Q, 
			GlfwKey.R => Key.R, 
			GlfwKey.S => Key.S, 
			GlfwKey.T => Key.T, 
			GlfwKey.U => Key.U, 
			GlfwKey.V => Key.V, 
			GlfwKey.W => Key.W, 
			GlfwKey.X => Key.X, 
			GlfwKey.Y => Key.Y, 
			GlfwKey.Z => Key.Z, 
			GlfwKey.Number0 => Key.Digit0, 
			GlfwKey.Number1 => Key.Digit1, 
			GlfwKey.Number2 => Key.Digit2, 
			GlfwKey.Number3 => Key.Digit3, 
			GlfwKey.Number4 => Key.Digit4, 
			GlfwKey.Number5 => Key.Digit5, 
			GlfwKey.Number6 => Key.Digit6, 
			GlfwKey.Number7 => Key.Digit7, 
			GlfwKey.Number8 => Key.Digit8, 
			GlfwKey.Number9 => Key.Digit9, 
			GlfwKey.Space => Key.Space, 
			GlfwKey.Tab => Key.Tab, 
			GlfwKey.Enter => Key.Enter, 
			GlfwKey.Escape => Key.Escape, 
			GlfwKey.Backspace => Key.Backspace, 
			GlfwKey.Delete => Key.Delete, 
			GlfwKey.Insert => Key.Insert, 
			GlfwKey.Home => Key.Home, 
			GlfwKey.End => Key.End, 
			GlfwKey.PageUp => Key.PageUp, 
			GlfwKey.PageDown => Key.PageDown, 
			GlfwKey.Left => Key.LeftArrow, 
			GlfwKey.Right => Key.RightArrow, 
			GlfwKey.Up => Key.UpArrow, 
			GlfwKey.Down => Key.DownArrow, 
			GlfwKey.LeftShift => Key.LeftShift, 
			GlfwKey.RightShift => Key.RightShift, 
			GlfwKey.LeftControl => Key.LeftCtrl, 
			GlfwKey.RightControl => Key.RightCtrl, 
			GlfwKey.LeftAlt => Key.LeftAlt, 
			GlfwKey.RightAlt => Key.RightAlt, 
			GlfwKey.LeftSuper => Key.LeftMeta, 
			GlfwKey.RightSuper => Key.RightMeta, 
			GlfwKey.Menu => Key.ContextMenu, 
			GlfwKey.Apostrophe => Key.Quote, 
			GlfwKey.Comma => Key.Comma, 
			GlfwKey.Minus => Key.Minus, 
			GlfwKey.Period => Key.Period, 
			GlfwKey.Slash => Key.Slash, 
			GlfwKey.Semicolon => Key.Semicolon, 
			GlfwKey.Equal => Key.Equals, 
			GlfwKey.LeftBracket => Key.LeftBracket, 
			GlfwKey.Backslash => Key.Backslash, 
			GlfwKey.RightBracket => Key.RightBracket, 
			GlfwKey.GraveAccent => Key.Backquote, 
			GlfwKey.CapsLock => Key.CapsLock, 
			GlfwKey.ScrollLock => Key.ScrollLock, 
			GlfwKey.NumLock => Key.NumLock, 
			GlfwKey.PrintScreen => Key.PrintScreen, 
			GlfwKey.Pause => Key.Pause, 
			GlfwKey.Kp0 => Key.Numpad0, 
			GlfwKey.Kp1 => Key.Numpad1, 
			GlfwKey.Kp2 => Key.Numpad2, 
			GlfwKey.Kp3 => Key.Numpad3, 
			GlfwKey.Kp4 => Key.Numpad4, 
			GlfwKey.Kp5 => Key.Numpad5, 
			GlfwKey.Kp6 => Key.Numpad6, 
			GlfwKey.Kp7 => Key.Numpad7, 
			GlfwKey.Kp8 => Key.Numpad8, 
			GlfwKey.Kp9 => Key.Numpad9, 
			GlfwKey.KpDecimal => Key.NumpadPeriod, 
			GlfwKey.KpDivide => Key.NumpadDivide, 
			GlfwKey.KpMultiply => Key.NumpadMultiply, 
			GlfwKey.KpSubtract => Key.NumpadMinus, 
			GlfwKey.KpAdd => Key.NumpadPlus, 
			GlfwKey.KpEnter => Key.NumpadEnter, 
			GlfwKey.KpEqual => Key.NumpadEquals, 
			GlfwKey.F1 => Key.F1, 
			GlfwKey.F2 => Key.F2, 
			GlfwKey.F3 => Key.F3, 
			GlfwKey.F4 => Key.F4, 
			GlfwKey.F5 => Key.F5, 
			GlfwKey.F6 => Key.F6, 
			GlfwKey.F7 => Key.F7, 
			GlfwKey.F8 => Key.F8, 
			GlfwKey.F9 => Key.F9, 
			GlfwKey.F10 => Key.F10, 
			GlfwKey.F11 => Key.F11, 
			GlfwKey.F12 => Key.F12, 
			GlfwKey.F13 => Key.F13, 
			GlfwKey.F14 => Key.F14, 
			GlfwKey.F15 => Key.F15, 
			GlfwKey.F16 => Key.F16, 
			GlfwKey.F17 => Key.F17, 
			GlfwKey.F18 => Key.F18, 
			GlfwKey.F19 => Key.F19, 
			GlfwKey.F20 => Key.F20, 
			GlfwKey.F21 => Key.F21, 
			GlfwKey.F22 => Key.F22, 
			GlfwKey.F23 => Key.F23, 
			GlfwKey.F24 => Key.F24, 
			GlfwKey.World1 => Key.OEM1, 
			GlfwKey.World2 => Key.OEM2, 
			_ => Key.None, 
		};
	}

	private static GlfwKey[] BuildReverse()
	{
		GlfwKey[] array = new GlfwKey[120];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = GlfwKey.Unknown;
		}
		GlfwKey[] values = Enum.GetValues<GlfwKey>();
		foreach (GlfwKey glfwKey in values)
		{
			Key key = ToKey(glfwKey);
			if (key != 0 && array[(int)key] == GlfwKey.Unknown)
			{
				array[(int)key] = glfwKey;
			}
		}
		return array;
	}

	public static GlfwKey ToGlfwKey(Key key)
	{
		if ((uint)key >= 120u)
		{
			return GlfwKey.Unknown;
		}
		return reverse[(int)key];
	}
}
