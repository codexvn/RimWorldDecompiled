using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020029C6 RID: 10694
	public class Building_Vent : Building_TempControl
	{
		// Token: 0x17002455 RID: 9301
		// (get) Token: 0x0600EBBD RID: 60349 RVA: 0x0044792D File Offset: 0x00445B2D
		public override Graphic Graphic
		{
			get
			{
				return this.flickableComp.CurrentGraphic;
			}
		}

		// Token: 0x17002456 RID: 9302
		// (get) Token: 0x0600EBBE RID: 60350 RVA: 0x0044793C File Offset: 0x00445B3C
		private VacuumComponent Vacuum
		{
			get
			{
				VacuumComponent result;
				if ((result = this.intVacuum) == null)
				{
					result = (this.intVacuum = base.Map.GetComponent<VacuumComponent>());
				}
				return result;
			}
		}

		// Token: 0x17002457 RID: 9303
		// (get) Token: 0x0600EBBF RID: 60351 RVA: 0x00447967 File Offset: 0x00445B67
		public override bool ExchangeVacuum
		{
			get
			{
				return base.ExchangeVacuum || FlickUtility.WantsToBeOn(this);
			}
		}

		// Token: 0x0600EBC0 RID: 60352 RVA: 0x00447979 File Offset: 0x00445B79
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.flickableComp = base.GetComp<CompFlickable>();
		}

		// Token: 0x0600EBC1 RID: 60353 RVA: 0x0044798F File Offset: 0x00445B8F
		public override void TickRare()
		{
			if (!FlickUtility.WantsToBeOn(this))
			{
				return;
			}
			GenTemperature.EqualizeTemperaturesThroughBuilding(this, 14f, true);
			base.Map.gasGrid.EqualizeGasThroughBuilding(this, true);
		}

		// Token: 0x0600EBC2 RID: 60354 RVA: 0x004479B8 File Offset: 0x00445BB8
		protected override void ReceiveCompSignal(string signal)
		{
			if (signal == "FlickedOn" || signal == "FlickedOff")
			{
				this.Vacuum.Dirty();
			}
		}

		// Token: 0x0600EBC3 RID: 60355 RVA: 0x004479E0 File Offset: 0x00445BE0
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (!FlickUtility.WantsToBeOn(this))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("VentClosed".Translate());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04009C71 RID: 40049
		private CompFlickable flickableComp;

		// Token: 0x04009C72 RID: 40050
		private VacuumComponent intVacuum;
	}
}
