using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B99 RID: 2969
	public static class GUIEventFilterForOSX
	{
		// Token: 0x06004BF2 RID: 19442 RVA: 0x00182D54 File Offset: 0x00180F54
		public static void CheckRejectGUIEvent()
		{
			if (UnityData.platform != RuntimePlatform.OSXPlayer)
			{
				return;
			}
			if (Event.current.type != EventType.MouseDown && Event.current.type != EventType.MouseUp)
			{
				return;
			}
			if (Time.frameCount != GUIEventFilterForOSX.lastRecordedFrame)
			{
				GUIEventFilterForOSX.eventsThisFrame.Clear();
				GUIEventFilterForOSX.lastRecordedFrame = Time.frameCount;
			}
			for (int i = 0; i < GUIEventFilterForOSX.eventsThisFrame.Count; i++)
			{
				if (GUIEventFilterForOSX.EventsAreEquivalent(GUIEventFilterForOSX.eventsThisFrame[i], Event.current))
				{
					GUIEventFilterForOSX.RejectEvent();
				}
			}
			GUIEventFilterForOSX.eventsThisFrame.Add(Event.current);
		}

		// Token: 0x06004BF3 RID: 19443 RVA: 0x00182DE4 File Offset: 0x00180FE4
		private static bool EventsAreEquivalent(Event A, Event B)
		{
			return A.button == B.button && A.keyCode == B.keyCode && A.type == B.type;
		}

		// Token: 0x06004BF4 RID: 19444 RVA: 0x00182E14 File Offset: 0x00181014
		private static void RejectEvent()
		{
			if (DebugViewSettings.logInput)
			{
				Log.Message("Frame " + Time.frameCount.ToString() + ": REJECTED " + Event.current.ToStringFull());
			}
			Event.current.Use();
		}

		// Token: 0x04003640 RID: 13888
		private static List<Event> eventsThisFrame = new List<Event>();

		// Token: 0x04003641 RID: 13889
		private static int lastRecordedFrame = -1;
	}
}
