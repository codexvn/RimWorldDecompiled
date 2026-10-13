using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000BCA RID: 3018
	[StaticConstructorOnStartup]
	public static class Messages
	{
		// Token: 0x06004D42 RID: 19778 RVA: 0x00188F00 File Offset: 0x00187100
		public static void Update()
		{
			if (Current.ProgramState == ProgramState.Playing && Messages.mouseoverMessageIndex >= 0 && Messages.mouseoverMessageIndex < Messages.liveMessages.Count)
			{
				Messages.liveMessages[Messages.mouseoverMessageIndex].lookTargets.TryHighlight(true, true, false);
			}
			Messages.mouseoverMessageIndex = -1;
			Messages.liveMessages.RemoveAll((Message m) => m.Expired);
		}

		// Token: 0x06004D43 RID: 19779 RVA: 0x00188F7A File Offset: 0x0018717A
		public static void Message(string text, LookTargets lookTargets, MessageTypeDef def, Quest quest, bool historical = true)
		{
			if (!Messages.AcceptsMessage(text, lookTargets))
			{
				return;
			}
			Messages.Message(new Message(text.CapitalizeFirst(), def, lookTargets, quest), historical);
		}

		// Token: 0x06004D44 RID: 19780 RVA: 0x00188F9B File Offset: 0x0018719B
		public static void Message(string text, LookTargets lookTargets, MessageTypeDef def, bool historical = true)
		{
			if (!Messages.AcceptsMessage(text, lookTargets))
			{
				return;
			}
			Messages.Message(new Message(text.CapitalizeFirst(), def, lookTargets), historical);
		}

		// Token: 0x06004D45 RID: 19781 RVA: 0x00188FBA File Offset: 0x001871BA
		public static void Message(string text, MessageTypeDef def, bool historical = true)
		{
			if (!Messages.AcceptsMessage(text, TargetInfo.Invalid))
			{
				return;
			}
			Messages.Message(new Message(text.CapitalizeFirst(), def), historical);
		}

		// Token: 0x06004D46 RID: 19782 RVA: 0x00188FE4 File Offset: 0x001871E4
		public static void Message(Message msg, bool historical = true)
		{
			if (!Messages.AcceptsMessage(msg.text, msg.lookTargets))
			{
				return;
			}
			if (historical && Find.Archive != null)
			{
				Find.Archive.Add(msg);
			}
			Messages.liveMessages.Add(msg);
			while (Messages.liveMessages.Count > 12)
			{
				Messages.liveMessages.RemoveAt(0);
			}
			if (msg.def.sound != null)
			{
				msg.def.sound.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x0018905F File Offset: 0x0018725F
		public static bool IsLive(Message msg)
		{
			return Messages.liveMessages.Contains(msg);
		}

		// Token: 0x06004D48 RID: 19784 RVA: 0x0018906C File Offset: 0x0018726C
		public static void MessagesDoGUI()
		{
			Text.Font = GameFont.Small;
			int xOffset = (int)Messages.MessagesTopLeftStandard.x;
			int num = (int)Messages.MessagesTopLeftStandard.y;
			if (Current.Game != null && Find.ActiveLesson.ActiveLessonVisible)
			{
				num += (int)Find.ActiveLesson.Current.MessagesYOffset;
			}
			for (int i = Messages.liveMessages.Count - 1; i >= 0; i--)
			{
				Messages.liveMessages[i].Draw(xOffset, num);
				num += 26;
			}
		}

		// Token: 0x06004D49 RID: 19785 RVA: 0x001890EC File Offset: 0x001872EC
		public static bool CollidesWithAnyMessage(Rect rect, out float messageAlpha)
		{
			bool result = false;
			float num = 0f;
			for (int i = 0; i < Messages.liveMessages.Count; i++)
			{
				Message message = Messages.liveMessages[i];
				if (rect.Overlaps(message.lastDrawRect))
				{
					result = true;
					num = Mathf.Max(num, message.Alpha);
				}
			}
			messageAlpha = num;
			return result;
		}

		// Token: 0x06004D4A RID: 19786 RVA: 0x00189144 File Offset: 0x00187344
		public static void Clear()
		{
			Messages.liveMessages.Clear();
		}

		// Token: 0x06004D4B RID: 19787 RVA: 0x00189150 File Offset: 0x00187350
		public static void Notify_LoadedLevelChanged()
		{
			for (int i = 0; i < Messages.liveMessages.Count; i++)
			{
				Messages.liveMessages[i].lookTargets = null;
			}
		}

		// Token: 0x06004D4C RID: 19788 RVA: 0x00189184 File Offset: 0x00187384
		private static bool AcceptsMessage(string text, LookTargets lookTargets)
		{
			if (text.NullOrEmpty())
			{
				return false;
			}
			int i = 0;
			while (i < Messages.liveMessages.Count)
			{
				if (Messages.liveMessages[i].text == text && LookTargets.SameTargets(Messages.liveMessages[i].lookTargets, lookTargets))
				{
					if (Messages.liveMessages[i].startingFrame == RealTime.frameCount)
					{
						return false;
					}
					Messages.liveMessages[i].ResetTimer();
					Messages.liveMessages[i].Flash();
					Messages.liveMessages[i].def.sound.PlayOneShotOnCamera(null);
					return false;
				}
				else
				{
					i++;
				}
			}
			return true;
		}

		// Token: 0x06004D4D RID: 19789 RVA: 0x0018923D File Offset: 0x0018743D
		public static void Notify_Mouseover(Message msg)
		{
			Messages.mouseoverMessageIndex = Messages.liveMessages.IndexOf(msg);
		}

		// Token: 0x040036EE RID: 14062
		private static List<Message> liveMessages = new List<Message>();

		// Token: 0x040036EF RID: 14063
		private static int mouseoverMessageIndex = -1;

		// Token: 0x040036F0 RID: 14064
		public static readonly Vector2 MessagesTopLeftStandard = new Vector2(140f, 16f);

		// Token: 0x040036F1 RID: 14065
		private const int MessageYInterval = 26;

		// Token: 0x040036F2 RID: 14066
		private const int MaxLiveMessages = 12;
	}
}
