using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002133 RID: 8499
	public class CompLowPowerInSpace : ThingComp
	{
		// Token: 0x17001C2C RID: 7212
		// (get) Token: 0x0600B96D RID: 47469 RVA: 0x003581A2 File Offset: 0x003563A2
		private CompProperties_LowPowerUnlessVacuum Props
		{
			get
			{
				return (CompProperties_LowPowerUnlessVacuum)this.props;
			}
		}

		// Token: 0x17001C2D RID: 7213
		// (get) Token: 0x0600B96E RID: 47470 RVA: 0x003581B0 File Offset: 0x003563B0
		private CompPowerTrader PowerTrader
		{
			get
			{
				CompPowerTrader result;
				if ((result = this.intPowerTrader) == null)
				{
					result = (this.intPowerTrader = this.parent.GetComp<CompPowerTrader>());
				}
				return result;
			}
		}

		// Token: 0x0600B96F RID: 47471 RVA: 0x003581DB File Offset: 0x003563DB
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.CheckUpdatePowerMode();
		}

		// Token: 0x0600B970 RID: 47472 RVA: 0x003581E3 File Offset: 0x003563E3
		public override void CompTick()
		{
			if (this.parent.IsHashIntervalTick(250))
			{
				this.CheckUpdatePowerMode();
			}
		}

		// Token: 0x0600B971 RID: 47473 RVA: 0x003581DB File Offset: 0x003563DB
		public override void CompTickRare()
		{
			this.CheckUpdatePowerMode();
		}

		// Token: 0x0600B972 RID: 47474 RVA: 0x00358200 File Offset: 0x00356400
		private void CheckUpdatePowerMode()
		{
			if (this.PowerTrader.Off)
			{
				return;
			}
			this.lowPowerMode = !this.NeedsPower();
			if (this.lowPowerMode)
			{
				this.PowerTrader.PowerOutput = -this.PowerTrader.Props.PowerConsumption * this.Props.lowPowerConsumptionFactor;
				return;
			}
			this.PowerTrader.PowerOutput = -this.PowerTrader.Props.PowerConsumption;
		}

		// Token: 0x0600B973 RID: 47475 RVA: 0x00358278 File Offset: 0x00356478
		private bool NeedsPower()
		{
			if (this.Props.checkRoomVacuum)
			{
				Room room = this.parent.GetRoom(RegionType.Set_All);
				if (room.ExposedToSpace)
				{
					return false;
				}
				if (Mathf.Approximately(room.Vacuum, 0f))
				{
					return false;
				}
			}
			return this.parent.Map.Biome.inVacuum;
		}

		// Token: 0x0600B974 RID: 47476 RVA: 0x003582D8 File Offset: 0x003564D8
		public override string CompInspectStringExtra()
		{
			string str;
			if (this.PowerTrader.Off)
			{
				str = "PowerConsumptionOff".Translate();
			}
			else if (this.lowPowerMode)
			{
				str = "PowerConsumptionLow".Translate();
			}
			else
			{
				str = "PowerConsumptionHigh".Translate();
			}
			return string.Format("{0}: {1}", "PowerConsumptionMode".Translate(), str.CapitalizeFirst());
		}

		// Token: 0x04007FDF RID: 32735
		private bool lowPowerMode;

		// Token: 0x04007FE0 RID: 32736
		private CompPowerTrader intPowerTrader;
	}
}
