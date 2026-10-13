using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020030FF RID: 12543
	public class Designator_ZoneAddStockpile_Dumping : Designator_ZoneAddStockpile
	{
		// Token: 0x06011A03 RID: 72195 RVA: 0x0050F040 File Offset: 0x0050D240
		public Designator_ZoneAddStockpile_Dumping()
		{
			this.preset = StorageSettingsPreset.DumpingStockpile;
			this.defaultLabel = this.preset.PresetName();
			this.defaultDesc = "DesignatorZoneCreateStorageDumpingDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/ZoneCreate_DumpingStockpile", true);
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd_Dumping;
		}

		// Token: 0x06011A04 RID: 72196 RVA: 0x0050F02B File Offset: 0x0050D22B
		protected override void FinalizeDesignationSucceeded()
		{
			base.FinalizeDesignationSucceeded();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.StorageTab, OpportunityType.GoodToKnow);
		}
	}
}
