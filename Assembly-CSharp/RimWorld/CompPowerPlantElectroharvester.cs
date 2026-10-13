using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002138 RID: 8504
	public class CompPowerPlantElectroharvester : CompPowerPlant
	{
		// Token: 0x17001C36 RID: 7222
		// (get) Token: 0x0600B99C RID: 47516 RVA: 0x00358DFB File Offset: 0x00356FFB
		protected override float DesiredPowerOutput
		{
			get
			{
				return this.GetPowerOutput();
			}
		}

		// Token: 0x17001C37 RID: 7223
		// (get) Token: 0x0600B99D RID: 47517 RVA: 0x00358E04 File Offset: 0x00357004
		public CompFacility FacilityComp
		{
			get
			{
				CompFacility result;
				if ((result = this.facilityComp) == null)
				{
					result = (this.facilityComp = this.parent.GetComp<CompFacility>());
				}
				return result;
			}
		}

		// Token: 0x17001C38 RID: 7224
		// (get) Token: 0x0600B99E RID: 47518 RVA: 0x00358E2F File Offset: 0x0035702F
		public List<Thing> Platforms
		{
			get
			{
				return this.FacilityComp.LinkedBuildings;
			}
		}

		// Token: 0x0600B99F RID: 47519 RVA: 0x00358E3C File Offset: 0x0035703C
		private float GetPowerOutput()
		{
			float num = 0f;
			foreach (Thing thing in this.Platforms)
			{
				Building_HoldingPlatform building_HoldingPlatform = thing as Building_HoldingPlatform;
				if (building_HoldingPlatform != null && building_HoldingPlatform.Occupied)
				{
					num += (float)Mathf.RoundToInt(building_HoldingPlatform.HeldPawn.BodySize * -base.Props.PowerConsumption * 0.1f);
				}
			}
			return Mathf.Clamp(num, 0f, -base.Props.PowerConsumption);
		}

		// Token: 0x0600B9A0 RID: 47520 RVA: 0x00358EDC File Offset: 0x003570DC
		public override string CompInspectStringExtra()
		{
			string str = "";
			if (this.Platforms.Empty<Thing>())
			{
				str = "ElectroharvesterNoPlatform".Translate() + "\n";
			}
			else if (this.GetPowerOutput() == 0f)
			{
				str = "ElectroharvesterNoEntity".Translate() + "\n";
			}
			return str + base.CompInspectStringExtra();
		}

		// Token: 0x04007FF2 RID: 32754
		private CompFacility facilityComp;
	}
}
