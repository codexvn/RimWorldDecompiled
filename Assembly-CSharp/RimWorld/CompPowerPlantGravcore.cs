using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002139 RID: 8505
	public class CompPowerPlantGravcore : CompPowerPlant
	{
		// Token: 0x17001C39 RID: 7225
		// (get) Token: 0x0600B9A2 RID: 47522 RVA: 0x00358F4D File Offset: 0x0035714D
		protected override float DesiredPowerOutput
		{
			get
			{
				if (!this.onSubstructure)
				{
					return 0f;
				}
				return base.DesiredPowerOutput;
			}
		}

		// Token: 0x0600B9A3 RID: 47523 RVA: 0x00358F63 File Offset: 0x00357163
		public override void CompTickInterval(int delta)
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (!this.parent.IsHashIntervalTick(60, delta))
			{
				return;
			}
			this.CheckSubstructure();
		}

		// Token: 0x0600B9A4 RID: 47524 RVA: 0x00358F8A File Offset: 0x0035718A
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.CheckSubstructure();
		}

		// Token: 0x0600B9A5 RID: 47525 RVA: 0x00358F9C File Offset: 0x0035719C
		private void CheckSubstructure()
		{
			TerrainDef terrainDef = this.parent.Map.terrainGrid.FoundationAt(this.parent.Position);
			this.onSubstructure = (terrainDef != null && terrainDef.IsSubstructure);
		}

		// Token: 0x0600B9A6 RID: 47526 RVA: 0x00358FDC File Offset: 0x003571DC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.onSubstructure, "onSubstructure", false, false);
		}

		// Token: 0x0600B9A7 RID: 47527 RVA: 0x00358FF8 File Offset: 0x003571F8
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (this.onSubstructure)
			{
				return text;
			}
			return text + "\n" + ("Disabled".Translate() + ": " + "MessageMustBePlacedOnSubstructure".Translate()).Colorize(ColorLibrary.RedReadable);
		}

		// Token: 0x04007FF3 RID: 32755
		private const int CheckSubstructureInterval = 60;

		// Token: 0x04007FF4 RID: 32756
		private bool onSubstructure;
	}
}
