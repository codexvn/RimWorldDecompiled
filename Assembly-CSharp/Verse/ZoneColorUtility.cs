using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000556 RID: 1366
	public static class ZoneColorUtility
	{
		// Token: 0x06002380 RID: 9088 RVA: 0x000B7F2C File Offset: 0x000B612C
		static ZoneColorUtility()
		{
			foreach (Color color in ZoneColorUtility.GrowingZoneColors())
			{
				Color item = new Color(color.r, color.g, color.b, 0.09f);
				ZoneColorUtility.growingZoneColors.Add(item);
			}
			foreach (Color color2 in ZoneColorUtility.StorageZoneColors())
			{
				Color item2 = new Color(color2.r, color2.g, color2.b, 0.09f);
				ZoneColorUtility.storageZoneColors.Add(item2);
			}
			foreach (Color color3 in ZoneColorUtility.FishingZoneColors())
			{
				Color item3 = new Color(color3.r, color3.g, color3.b, 0.09f);
				ZoneColorUtility.fishingZoneColors.Add(item3);
			}
		}

		// Token: 0x06002381 RID: 9089 RVA: 0x000B808C File Offset: 0x000B628C
		public static Color NextGrowingZoneColor()
		{
			Color result = ZoneColorUtility.growingZoneColors[ZoneColorUtility.nextGrowingZoneColorIndex];
			ZoneColorUtility.nextGrowingZoneColorIndex++;
			if (ZoneColorUtility.nextGrowingZoneColorIndex >= ZoneColorUtility.growingZoneColors.Count)
			{
				ZoneColorUtility.nextGrowingZoneColorIndex = 0;
			}
			return result;
		}

		// Token: 0x06002382 RID: 9090 RVA: 0x000B80C0 File Offset: 0x000B62C0
		public static Color NextStorageZoneColor()
		{
			Color result = ZoneColorUtility.storageZoneColors[ZoneColorUtility.nextStorageZoneColorIndex];
			ZoneColorUtility.nextStorageZoneColorIndex++;
			if (ZoneColorUtility.nextStorageZoneColorIndex >= ZoneColorUtility.storageZoneColors.Count)
			{
				ZoneColorUtility.nextStorageZoneColorIndex = 0;
			}
			return result;
		}

		// Token: 0x06002383 RID: 9091 RVA: 0x000B80F4 File Offset: 0x000B62F4
		public static Color NextFishingZoneColor()
		{
			Color result = ZoneColorUtility.fishingZoneColors[ZoneColorUtility.nextFishingZoneColorIndex];
			ZoneColorUtility.nextFishingZoneColorIndex++;
			if (ZoneColorUtility.nextFishingZoneColorIndex >= ZoneColorUtility.fishingZoneColors.Count)
			{
				ZoneColorUtility.nextFishingZoneColorIndex = 0;
			}
			return result;
		}

		// Token: 0x06002384 RID: 9092 RVA: 0x000B8128 File Offset: 0x000B6328
		public static IEnumerable<Color> GrowingZoneColors()
		{
			yield return Color.Lerp(new Color(0f, 1f, 0f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(1f, 1f, 0f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0.5f, 1f, 0f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(1f, 1f, 0.5f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0.5f, 1f, 0.5f), Color.gray, 0.5f);
			yield break;
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x000B8131 File Offset: 0x000B6331
		public static IEnumerable<Color> StorageZoneColors()
		{
			yield return Color.Lerp(new Color(1f, 0f, 0f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(1f, 0f, 1f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0f, 0f, 1f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(1f, 0f, 0.5f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0f, 0.5f, 1f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0.5f, 0f, 1f), Color.gray, 0.5f);
			yield break;
		}

		// Token: 0x06002386 RID: 9094 RVA: 0x000B813A File Offset: 0x000B633A
		public static IEnumerable<Color> FishingZoneColors()
		{
			yield return Color.Lerp(new Color(1f, 0.8f, 0f), Color.gray, 0.25f);
			yield return Color.Lerp(new Color(1f, 0.48235294f, 0f), Color.gray, 0.25f);
			yield return Color.Lerp(new Color(1f, 0.36862746f, 0f), Color.gray, 0.25f);
			yield return Color.Lerp(new Color(1f, 0.96862745f, 0f), Color.gray, 0.25f);
			yield return Color.Lerp(new Color(1f, 0.14901961f, 0f), Color.gray, 0.25f);
			yield break;
		}

		// Token: 0x04001D86 RID: 7558
		private static List<Color> growingZoneColors = new List<Color>();

		// Token: 0x04001D87 RID: 7559
		private static List<Color> storageZoneColors = new List<Color>();

		// Token: 0x04001D88 RID: 7560
		private static List<Color> fishingZoneColors = new List<Color>();

		// Token: 0x04001D89 RID: 7561
		private static int nextGrowingZoneColorIndex = 0;

		// Token: 0x04001D8A RID: 7562
		private static int nextStorageZoneColorIndex = 0;

		// Token: 0x04001D8B RID: 7563
		private static int nextFishingZoneColorIndex = 0;

		// Token: 0x04001D8C RID: 7564
		private const float ZoneOpacity = 0.09f;
	}
}
