namespace Waylith.Input;

public readonly struct KeyCombo
{
	public readonly Key key;

	public readonly MouseButton mouseButton;

	public readonly bool scrollUp;

	public readonly bool scrollDown;

	public readonly Key modifier1;

	public readonly Key modifier2;

	public bool IsValid
	{
		get
		{
			if (key == Key.None && mouseButton == MouseButton.None && !scrollUp)
			{
				return scrollDown;
			}
			return true;
		}
	}

	public KeyCombo(Key key, Key modifier1 = Key.None, Key modifier2 = Key.None)
		: this(key, MouseButton.None, scrollUp: false, scrollDown: false, modifier1, modifier2)
	{
	}

	public KeyCombo(MouseButton button, Key modifier1 = Key.None, Key modifier2 = Key.None)
		: this(Key.None, button, scrollUp: false, scrollDown: false, modifier1, modifier2)
	{
	}

	public KeyCombo(Key key, MouseButton mouseButton, bool scrollUp, bool scrollDown, Key modifier1 = Key.None, Key modifier2 = Key.None)
	{
		this.key = key;
		this.mouseButton = mouseButton;
		this.scrollUp = scrollUp;
		this.scrollDown = scrollDown;
		this.modifier1 = modifier1;
		this.modifier2 = modifier2;
	}

	public static KeyCombo ScrollCombo(bool up, Key modifier1 = Key.None, Key modifier2 = Key.None)
	{
		return new KeyCombo(Key.None, MouseButton.None, up, !up, modifier1, modifier2);
	}

	private bool ModifiersHeld()
	{
		if (modifier1 != 0 && !InputState.IsKeyDown(modifier1))
		{
			return false;
		}
		if (modifier2 != 0 && !InputState.IsKeyDown(modifier2))
		{
			return false;
		}
		return true;
	}

	public bool IsPressed()
	{
		if (!ModifiersHeld())
		{
			return false;
		}
		if (key != 0)
		{
			return InputState.IsKeyDown(key);
		}
		if (mouseButton != MouseButton.None)
		{
			return InputState.IsMouseButtonDown(mouseButton);
		}
		return false;
	}

	public bool JustPressed(InputEvent evt, bool allowPartialScroll = false)
	{
		switch (evt.type)
		{
		case InputEventType.KeyDown:
			if (key != 0 && evt.key == key)
			{
				return ModifiersHeld();
			}
			return false;
		case InputEventType.MouseDown:
			if (mouseButton != MouseButton.None && evt.mouseButton == mouseButton)
			{
				return ModifiersHeld();
			}
			return false;
		case InputEventType.ScrollWheel:
		{
			if (!scrollUp && !scrollDown)
			{
				return false;
			}
			float num = (allowPartialScroll ? evt.scrollDelta.Y : ((float)evt.scrollNotches));
			if (num == 0f)
			{
				return false;
			}
			if (num > 0f == scrollUp)
			{
				return ModifiersHeld();
			}
			return false;
		}
		default:
			return false;
		}
	}

	public bool JustReleased(InputEvent evt)
	{
		switch (evt.type)
		{
		case InputEventType.KeyUp:
			if (key != 0)
			{
				return evt.key == key;
			}
			return false;
		case InputEventType.MouseUp:
			if (mouseButton != MouseButton.None)
			{
				return evt.mouseButton == mouseButton;
			}
			return false;
		default:
			return false;
		}
	}
}
