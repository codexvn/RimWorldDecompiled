using System;

namespace RimWorld
{
	// Token: 0x02001E96 RID: 7830
	public static class StoryDangerUtility
	{
		// Token: 0x0600ADD4 RID: 44500 RVA: 0x00319323 File Offset: 0x00317523
		public static float Scale(this StoryDanger d)
		{
			switch (d)
			{
			case StoryDanger.None:
				return 0f;
			case StoryDanger.Low:
				return 1f;
			case StoryDanger.High:
				return 2f;
			default:
				return 0f;
			}
		}
	}
}
