using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001996 RID: 6550
	public class FilthProperties
	{
		// Token: 0x17001687 RID: 5767
		// (get) Token: 0x06009460 RID: 37984 RVA: 0x002B93B3 File Offset: 0x002B75B3
		public bool TerrainSourced
		{
			get
			{
				return (this.placementMask & FilthSourceFlags.Terrain) > FilthSourceFlags.None;
			}
		}

		// Token: 0x04006005 RID: 24581
		public float cleaningWorkToReduceThickness = 35f;

		// Token: 0x04006006 RID: 24582
		public bool canFilthAttach;

		// Token: 0x04006007 RID: 24583
		public bool rainWashes;

		// Token: 0x04006008 RID: 24584
		public bool allowsFire = true;

		// Token: 0x04006009 RID: 24585
		public bool ignoreFilthMultiplierStat;

		// Token: 0x0400600A RID: 24586
		public int maxThickness = 100;

		// Token: 0x0400600B RID: 24587
		public FloatRange disappearsInDays = FloatRange.Zero;

		// Token: 0x0400600C RID: 24588
		public FilthSourceFlags placementMask = FilthSourceFlags.Unnatural;

		// Token: 0x0400600D RID: 24589
		public SoundDef cleaningSound;
	}
}
