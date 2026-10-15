using RimWorld;
using UnityEngine;

namespace Verse;

public class ScreenshotModeHandler
{
	private bool active;

	public bool Active
	{
		get
		{
			return active;
		}
		set
		{
			active = value;
		}
	}

	public bool FiltersCurrentEvent
	{
		get
		{
			if (!active)
			{
				return false;
			}
			if (Event.current.type == EventType.repaint || Event.current.type == EventType.layout)
			{
				return true;
			}
			if (Event.current.type == EventType.mouseDown || Event.current.type == EventType.mouseUp || Event.current.type == EventType.mouseDrag)
			{
				return true;
			}
			return false;
		}
	}

	public void ScreenshotModesOnGUI()
	{
		if (KeyBindingDefOf.ToggleScreenshotMode.KeyDownEvent && !WorldComponent_GravshipController.CutsceneInProgress)
		{
			active = !active;
			Event.current.Use();
		}
	}
}
