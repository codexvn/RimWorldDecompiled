using System;
using System.Collections.Generic;
using LudeonTK;

namespace Verse
{
	// Token: 0x02000142 RID: 322
	public class LogMessageQueue
	{
		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060009C2 RID: 2498 RVA: 0x0002E080 File Offset: 0x0002C280
		public IEnumerable<LogMessage> Messages
		{
			get
			{
				return this.messages;
			}
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0002E088 File Offset: 0x0002C288
		public void Enqueue(LogMessage msg)
		{
			bool flag;
			this.Enqueue(msg, out flag);
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0002E0A0 File Offset: 0x0002C2A0
		public void Enqueue(LogMessage msg, out bool repeatsCapped)
		{
			repeatsCapped = false;
			if (this.lastMessage == null || !msg.CanCombineWith(this.lastMessage))
			{
				this.lastMessage = msg;
				this.messages.Enqueue(msg);
				if (this.messages.Count > this.maxMessages)
				{
					EditWindow_Log.Notify_MessageDequeued(this.messages.Dequeue());
				}
				return;
			}
			if (this.lastMessage.repeats >= 99)
			{
				repeatsCapped = true;
				return;
			}
			this.lastMessage.repeats++;
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0002E123 File Offset: 0x0002C323
		internal void Clear()
		{
			this.messages.Clear();
			this.lastMessage = null;
		}

		// Token: 0x040004EF RID: 1263
		public int maxMessages = 1000;

		// Token: 0x040004F0 RID: 1264
		private Queue<LogMessage> messages = new Queue<LogMessage>();

		// Token: 0x040004F1 RID: 1265
		private LogMessage lastMessage;
	}
}
