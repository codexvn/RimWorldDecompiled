using System;
using System.Linq;
using System.Text;

namespace Verse.Sound
{
	// Token: 0x02000E2C RID: 3628
	public static class DebugSoundEventsLog
	{
		// Token: 0x17000EA1 RID: 3745
		// (get) Token: 0x06005B86 RID: 23430 RVA: 0x001D4888 File Offset: 0x001D2A88
		public static string EventsListingDebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (LogMessage logMessage in DebugSoundEventsLog.queue.Messages.Reverse<LogMessage>())
				{
					stringBuilder.AppendLine(logMessage.ToString());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06005B87 RID: 23431 RVA: 0x001D48F0 File Offset: 0x001D2AF0
		public static void Notify_SoundEvent(SoundDef def, SoundInfo info)
		{
			if (!DebugViewSettings.writeSoundEventsRecord)
			{
				return;
			}
			string str;
			if (def == null)
			{
				str = "null: ";
			}
			else if (def.isUndefined)
			{
				str = "Undefined: ";
			}
			else
			{
				str = (def.sustain ? "SustainerSpawn: " : "OneShot: ");
			}
			string str2 = (def != null) ? def.defName : "null";
			DebugSoundEventsLog.CreateRecord(str + str2 + " - " + info.ToString());
		}

		// Token: 0x06005B88 RID: 23432 RVA: 0x001D4964 File Offset: 0x001D2B64
		public static void Notify_SustainerEnded(Sustainer sustainer, SoundInfo info)
		{
			DebugSoundEventsLog.CreateRecord("SustainerEnd: " + sustainer.def.defName + " - " + info.ToString());
		}

		// Token: 0x06005B89 RID: 23433 RVA: 0x001D4992 File Offset: 0x001D2B92
		private static void CreateRecord(string str)
		{
			DebugSoundEventsLog.queue.Enqueue(new LogMessage(str));
		}

		// Token: 0x0400426A RID: 17002
		private static LogMessageQueue queue = new LogMessageQueue();
	}
}
