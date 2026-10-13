using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200366B RID: 13931
	public class ShortcutKeys
	{
		// Token: 0x0601356A RID: 79210 RVA: 0x005C0DD0 File Offset: 0x005BEFD0
		public void ShortcutKeysOnGUI()
		{
			if (Current.ProgramState == ProgramState.Playing && !WorldComponent_GravshipController.CutsceneInProgress)
			{
				if (KeyBindingDefOf.NextColonist.KeyDownEvent)
				{
					ThingSelectionUtility.SelectNextColonist();
					Event.current.Use();
				}
				if (KeyBindingDefOf.PreviousColonist.KeyDownEvent)
				{
					ThingSelectionUtility.SelectPreviousColonist();
					Event.current.Use();
				}
			}
		}
	}
}
