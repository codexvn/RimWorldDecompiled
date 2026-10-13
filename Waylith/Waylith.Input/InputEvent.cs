using System.Numerics;

namespace Waylith.Input;

public class InputEvent
{
	public InputEventType type;

	public InputEventType rawType;

	public Key key;

	public MouseButton mouseButton = MouseButton.None;

	public Vector2 MousePosition;

	public Vector2 scrollDelta;

	public int scrollNotches;

	public int clickCount = 1;

	public bool shift;

	public bool control;

	public bool alt;

	public bool command;

	public bool overUI;

	public bool IsMouse
	{
		get
		{
			if (type != InputEventType.MouseDown)
			{
				return type == InputEventType.MouseUp;
			}
			return true;
		}
	}

	public bool IsKey
	{
		get
		{
			if (type != InputEventType.KeyDown)
			{
				return type == InputEventType.KeyUp;
			}
			return true;
		}
	}

	public bool IsScroll => type == InputEventType.ScrollWheel;

	public InputEvent(InputEventType type)
	{
		this.type = type;
		rawType = type;
	}

	public void Reset(InputEventType newType)
	{
		type = newType;
		rawType = newType;
		key = Key.None;
		mouseButton = MouseButton.None;
		MousePosition = default(Vector2);
		scrollDelta = default(Vector2);
		scrollNotches = 0;
		clickCount = 1;
		shift = false;
		control = false;
		alt = false;
		command = false;
		overUI = false;
	}

	public void Use()
	{
		type = InputEventType.Used;
	}
}
