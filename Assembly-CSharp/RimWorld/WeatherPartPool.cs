using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020021BC RID: 8636
	public static class WeatherPartPool
	{
		// Token: 0x0600BC8C RID: 48268 RVA: 0x0036A438 File Offset: 0x00368638
		public static SkyOverlay GetInstanceOf<T>() where T : SkyOverlay
		{
			for (int i = 0; i < WeatherPartPool.instances.Count; i++)
			{
				T t = WeatherPartPool.instances[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			SkyOverlay skyOverlay = Activator.CreateInstance<T>();
			WeatherPartPool.instances.Add(skyOverlay);
			return skyOverlay;
		}

		// Token: 0x040081D3 RID: 33235
		private static List<SkyOverlay> instances = new List<SkyOverlay>();
	}
}
