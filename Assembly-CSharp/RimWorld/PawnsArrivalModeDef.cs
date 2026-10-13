using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AAD RID: 6829
	public class PawnsArrivalModeDef : Def
	{
		// Token: 0x17001774 RID: 6004
		// (get) Token: 0x060098FE RID: 39166 RVA: 0x002C9598 File Offset: 0x002C7798
		public PawnsArrivalModeWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (PawnsArrivalModeWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x040067D1 RID: 26577
		public Type workerClass = typeof(PawnsArrivalModeWorker);

		// Token: 0x040067D2 RID: 26578
		public SimpleCurve selectionWeightCurve;

		// Token: 0x040067D3 RID: 26579
		public SimpleCurve pointsFactorCurve;

		// Token: 0x040067D4 RID: 26580
		public TechLevel minTechLevel;

		// Token: 0x040067D5 RID: 26581
		public bool forQuickMilitaryAid;

		// Token: 0x040067D6 RID: 26582
		public bool walkIn;

		// Token: 0x040067D7 RID: 26583
		public bool canBeBackup = true;

		// Token: 0x040067D8 RID: 26584
		public List<BiomeDef> biomeWhitelist;

		// Token: 0x040067D9 RID: 26585
		public List<BiomeDef> biomeBlacklist;

		// Token: 0x040067DA RID: 26586
		public List<PlanetLayerDef> layerWhitelist;

		// Token: 0x040067DB RID: 26587
		public List<PlanetLayerDef> layerBlacklist;

		// Token: 0x040067DC RID: 26588
		[MustTranslate]
		public string textEnemy;

		// Token: 0x040067DD RID: 26589
		[MustTranslate]
		public string textFriendly;

		// Token: 0x040067DE RID: 26590
		[MustTranslate]
		public string textWillArrive;

		// Token: 0x040067DF RID: 26591
		public List<FactionCurve> selectionWeightCurvesPerFaction;

		// Token: 0x040067E0 RID: 26592
		[Unsaved(false)]
		private PawnsArrivalModeWorker workerInt;
	}
}
