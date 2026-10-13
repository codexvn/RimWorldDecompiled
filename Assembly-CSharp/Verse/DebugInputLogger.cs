using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BE3 RID: 3043
	public static class DebugInputLogger
	{
		// Token: 0x06004DDE RID: 19934 RVA: 0x0018BCB8 File Offset: 0x00189EB8
		public static void InputLogOnGUI()
		{
			if (!DebugViewSettings.logInput)
			{
				return;
			}
			if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp || Event.current.type == EventType.KeyDown || Event.current.type == EventType.KeyUp || Event.current.type == EventType.ScrollWheel)
			{
				Log.Message("Frame " + Time.frameCount.ToString() + ": " + Event.current.ToStringFull());
			}
		}

		// Token: 0x06004DDF RID: 19935 RVA: 0x0018BD38 File Offset: 0x00189F38
		public static string ToStringFull(this Event ev)
		{
			return string.Concat(new string[]
			{
				"(EVENT\ntype=",
				ev.type.ToString(),
				"\nbutton=",
				ev.button.ToString(),
				"\nkeyCode=",
				ev.keyCode.ToString(),
				"\ndelta=",
				ev.delta.ToString(),
				"\nalt=",
				ev.alt.ToString(),
				"\ncapsLock=",
				ev.capsLock.ToString(),
				"\ncharacter=",
				((ev.character != '\0') ? ev.character : ' ').ToString(),
				"\nclickCount=",
				ev.clickCount.ToString(),
				"\ncommand=",
				ev.command.ToString(),
				"\ncommandName=",
				ev.commandName,
				"\ncontrol=",
				ev.control.ToString(),
				"\nfunctionKey=",
				ev.functionKey.ToString(),
				"\nisKey=",
				ev.isKey.ToString(),
				"\nisMouse=",
				ev.isMouse.ToString(),
				"\nmodifiers=",
				ev.modifiers.ToString(),
				"\nmousePosition=",
				ev.mousePosition.ToString(),
				"\nnumeric=",
				ev.numeric.ToString(),
				"\npressure=",
				ev.pressure.ToString(),
				"\nrawType=",
				ev.rawType.ToString(),
				"\nshift=",
				ev.shift.ToString(),
				"\n)"
			});
		}
	}
}
