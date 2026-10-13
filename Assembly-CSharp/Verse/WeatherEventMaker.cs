using System;

namespace Verse
{
	// Token: 0x02000549 RID: 1353
	public class WeatherEventMaker
	{
		// Token: 0x0600231C RID: 8988 RVA: 0x000B6A48 File Offset: 0x000B4C48
		public void WeatherEventMakerTick(Map map, float strength)
		{
			if (Rand.Value < 1f / this.averageInterval * strength)
			{
				WeatherEvent newEvent = (WeatherEvent)Activator.CreateInstance(this.eventClass, new object[]
				{
					map
				});
				map.weatherManager.eventHandler.AddEvent(newEvent);
			}
		}

		// Token: 0x04001D4C RID: 7500
		public float averageInterval = 100f;

		// Token: 0x04001D4D RID: 7501
		public Type eventClass;
	}
}
