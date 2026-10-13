using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000548 RID: 1352
	public class WeatherEventHandler
	{
		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x06002317 RID: 8983 RVA: 0x000B6989 File Offset: 0x000B4B89
		public List<WeatherEvent> LiveEventsListForReading
		{
			get
			{
				return this.liveEvents;
			}
		}

		// Token: 0x06002318 RID: 8984 RVA: 0x000B6991 File Offset: 0x000B4B91
		public void AddEvent(WeatherEvent newEvent)
		{
			this.liveEvents.Add(newEvent);
			newEvent.FireEvent();
		}

		// Token: 0x06002319 RID: 8985 RVA: 0x000B69A8 File Offset: 0x000B4BA8
		public void WeatherEventHandlerTick()
		{
			for (int i = this.liveEvents.Count - 1; i >= 0; i--)
			{
				this.liveEvents[i].WeatherEventTick();
				if (this.liveEvents[i].Expired)
				{
					this.liveEvents.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600231A RID: 8986 RVA: 0x000B6A00 File Offset: 0x000B4C00
		public void WeatherEventsDraw()
		{
			for (int i = 0; i < this.liveEvents.Count; i++)
			{
				this.liveEvents[i].WeatherEventDraw();
			}
		}

		// Token: 0x04001D4B RID: 7499
		private List<WeatherEvent> liveEvents = new List<WeatherEvent>();
	}
}
