using System;
using UnityStubs.Gui;

namespace UnityEngine;

public class Event
{
	public const float scrollWheelDeltaPerTick = 3f;

	[ThreadStatic]
	private static Event _current;

	private EventType _type = EventType.repaint;

	internal EventType _rawType = EventType.repaint;

	internal EventType originalType = EventType.repaint;

	public bool isKey
	{
		get
		{
			EventType eventType = type;
			if ((uint)(eventType - 4) <= 1u)
			{
				return true;
			}
			return false;
		}
	}

	public bool isScrollWheel => type == EventType.scrollWheel;

	public bool control => (modifiers & EventModifiers.Control) != 0;

	public bool command => (modifiers & EventModifiers.Command) != 0;

	public bool capsLock => (modifiers & EventModifiers.CapsLock) != 0;

	public bool functionKey => (modifiers & EventModifiers.FunctionKey) != 0;

	public bool numeric => (modifiers & EventModifiers.Numeric) != 0;

	public string commandName => string.Empty;

	public float pressure => 0f;

	public static Event current
	{
		get
		{
			return _current;
		}
		set
		{
			_current = value;
		}
	}

	public EventType type
	{
		get
		{
			bool flag;
			switch (_type)
			{
			case EventType.mouseDown:
			case EventType.mouseUp:
			case EventType.mouseDrag:
			case EventType.scrollWheel:
				flag = true;
				break;
			default:
				flag = false;
				break;
			}
			if (flag && GUIUtility.hotControl == 0)
			{
				IGuiBackend guiBackend = GuiBackend.Current;
				if (guiBackend != null && guiBackend.MouseOutsideHitClip)
				{
					return EventType.ignore;
				}
			}
			return _type;
		}
		set
		{
			_type = value;
		}
	}

	public EventType rawType
	{
		get
		{
			return _rawType;
		}
		set
		{
			_rawType = (originalType = value);
		}
	}

	public int button { get; set; }

	public char character { get; set; }

	public int clickCount { get; set; }

	public Vector2 delta { get; set; }

	public KeyCode keyCode { get; set; }

	public EventModifiers modifiers { get; set; }

	public Vector2 mousePosition { get; set; }

	public bool shift { get; set; }

	public bool alt { get; set; }

	public bool isMouse
	{
		get
		{
			EventType eventType = type;
			if ((uint)eventType <= 3u)
			{
				return true;
			}
			return false;
		}
	}

	public EventType GetTypeForControl(int controlID)
	{
		return type;
	}

	public Event()
	{
	}

	public Event(Event other)
	{
		if (other == null)
		{
			throw new ArgumentException("Event to copy from is null.");
		}
		_type = other._type;
		_rawType = other._rawType;
		originalType = other.originalType;
		button = other.button;
		character = other.character;
		clickCount = other.clickCount;
		delta = other.delta;
		keyCode = other.keyCode;
		modifiers = other.modifiers;
		mousePosition = other.mousePosition;
		shift = other.shift;
		alt = other.alt;
	}

	public void Use()
	{
		type = EventType.used;
		_rawType = EventType.used;
	}
}
