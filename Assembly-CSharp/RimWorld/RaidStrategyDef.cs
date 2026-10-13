using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ACC RID: 6860
	public class RaidStrategyDef : Def
	{
		// Token: 0x17001791 RID: 6033
		// (get) Token: 0x06009990 RID: 39312 RVA: 0x002CB8B4 File Offset: 0x002C9AB4
		public RaidStrategyWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RaidStrategyWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x0400690B RID: 26891
		public Type workerClass;

		// Token: 0x0400690C RID: 26892
		public SimpleCurve selectionWeightPerPointsCurve;

		// Token: 0x0400690D RID: 26893
		public float minPawns = 1f;

		// Token: 0x0400690E RID: 26894
		public List<FactionCurve> selectionWeightCurvesPerFaction;

		// Token: 0x0400690F RID: 26895
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x04006910 RID: 26896
		public List<PlanetLayerDef> layerBlacklist;

		// Token: 0x04006911 RID: 26897
		[MustTranslate]
		public string arrivalTextFriendly;

		// Token: 0x04006912 RID: 26898
		[MustTranslate]
		public string arrivalTextEnemy;

		// Token: 0x04006913 RID: 26899
		[MustTranslate]
		public string letterLabelEnemy;

		// Token: 0x04006914 RID: 26900
		[MustTranslate]
		public string letterLabelFriendly;

		// Token: 0x04006915 RID: 26901
		public SimpleCurve pointsFactorCurve;

		// Token: 0x04006916 RID: 26902
		public bool pawnsCanBringFood;

		// Token: 0x04006917 RID: 26903
		public List<PawnsArrivalModeDef> arriveModes;

		// Token: 0x04006918 RID: 26904
		public float raidLootValueFactor = 1f;

		// Token: 0x04006919 RID: 26905
		private RaidStrategyWorker workerInt;
	}
}
