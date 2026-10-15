using UnityEngine;

namespace Verse;

public static class DebugInputLogger
{
	public static void InputLogOnGUI()
	{
		if (DebugViewSettings.logInput && (Event.current.type == EventType.mouseDown || Event.current.type == EventType.mouseUp || Event.current.type == EventType.keyDown || Event.current.type == EventType.keyUp || Event.current.type == EventType.scrollWheel))
		{
			Log.Message("Frame " + Time.frameCount + ": " + Event.current.ToStringFull());
		}
	}

	public static string ToStringFull(this Event ev)
	{
		return "(EVENT\ntype=" + ev.type.ToString() + "\nbutton=" + ev.button + "\nkeyCode=" + ev.keyCode.ToString() + "\ndelta=" + ev.delta.ToString() + "\nalt=" + ev.alt + "\ncapsLock=" + ev.capsLock + "\ncharacter=" + ((ev.character != 0) ? ev.character : ' ') + "\nclickCount=" + ev.clickCount + "\ncommand=" + ev.command + "\ncommandName=" + ev.commandName + "\ncontrol=" + ev.control + "\nfunctionKey=" + ev.functionKey + "\nisKey=" + ev.isKey + "\nisMouse=" + ev.isMouse + "\nmodifiers=" + ev.modifiers.ToString() + "\nmousePosition=" + ev.mousePosition.ToString() + "\nnumeric=" + ev.numeric + "\npressure=" + ev.pressure + "\nrawType=" + ev.rawType.ToString() + "\nshift=" + ev.shift + "\n)";
	}
}
