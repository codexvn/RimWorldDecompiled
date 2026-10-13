using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000141 RID: 321
	public class LogMessage
	{
		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x0002DF24 File Offset: 0x0002C124
		public Color Color
		{
			get
			{
				switch (this.type)
				{
				case LogMessageType.Message:
					return Color.white;
				case LogMessageType.Warning:
					return Color.yellow;
				case LogMessageType.Error:
					return ColorLibrary.LogError;
				default:
					return Color.white;
				}
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x0002DF63 File Offset: 0x0002C163
		public string StackTrace
		{
			get
			{
				if (this.stackTrace != null)
				{
					return this.stackTrace;
				}
				return "No stack trace.";
			}
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0002DF7C File Offset: 0x0002C17C
		public LogMessage(string text)
		{
			this.text = text;
			this.type = LogMessageType.Message;
			this.stackTrace = null;
			this.timestamp = DateTime.Now.ToString("HH:mm:ss");
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0002DFC4 File Offset: 0x0002C1C4
		public LogMessage(LogMessageType type, string text, string stackTrace)
		{
			this.text = text;
			this.type = type;
			this.stackTrace = stackTrace;
			this.timestamp = DateTime.Now.ToString("HH:mm:ss");
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0002E00C File Offset: 0x0002C20C
		public override string ToString()
		{
			string text = "[" + this.timestamp + "] " + this.text;
			if (this.repeats > 1)
			{
				return "(" + this.repeats.ToString() + ") " + text;
			}
			return text;
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0002E05B File Offset: 0x0002C25B
		public bool CanCombineWith(LogMessage other)
		{
			return this.text == other.text && this.type == other.type;
		}

		// Token: 0x040004EA RID: 1258
		public string text;

		// Token: 0x040004EB RID: 1259
		public LogMessageType type;

		// Token: 0x040004EC RID: 1260
		public int repeats = 1;

		// Token: 0x040004ED RID: 1261
		private string stackTrace;

		// Token: 0x040004EE RID: 1262
		private string timestamp;
	}
}
