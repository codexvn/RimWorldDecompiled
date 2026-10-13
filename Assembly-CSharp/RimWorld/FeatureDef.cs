using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A2D RID: 6701
	public class FeatureDef : Def
	{
		// Token: 0x1700171D RID: 5917
		// (get) Token: 0x06009711 RID: 38673 RVA: 0x002C26CB File Offset: 0x002C08CB
		public FeatureWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (FeatureWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x040063F7 RID: 25591
		public Type workerClass = typeof(FeatureWorker);

		// Token: 0x040063F8 RID: 25592
		public float order;

		// Token: 0x040063F9 RID: 25593
		public int minSize = 50;

		// Token: 0x040063FA RID: 25594
		public int maxSize = int.MaxValue;

		// Token: 0x040063FB RID: 25595
		public bool canTouchWorldEdge = true;

		// Token: 0x040063FC RID: 25596
		public RulePackDef nameMaker;

		// Token: 0x040063FD RID: 25597
		public int maxPossiblyAllowedSizeToTake = 30;

		// Token: 0x040063FE RID: 25598
		public float maxPossiblyAllowedSizePctOfMeToTake = 0.5f;

		// Token: 0x040063FF RID: 25599
		public List<BiomeDef> rootBiomes = new List<BiomeDef>();

		// Token: 0x04006400 RID: 25600
		public List<BiomeDef> acceptableBiomes = new List<BiomeDef>();

		// Token: 0x04006401 RID: 25601
		public int maxSpaceBetweenRootGroups = 5;

		// Token: 0x04006402 RID: 25602
		public int minRootGroupsInCluster = 3;

		// Token: 0x04006403 RID: 25603
		public int minRootGroupSize = 10;

		// Token: 0x04006404 RID: 25604
		public int maxRootGroupSize = int.MaxValue;

		// Token: 0x04006405 RID: 25605
		public int maxPassageWidth = 3;

		// Token: 0x04006406 RID: 25606
		public float maxPctOfWholeArea = 0.1f;

		// Token: 0x04006407 RID: 25607
		[Unsaved(false)]
		private FeatureWorker workerInt;
	}
}
