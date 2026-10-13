using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020029C3 RID: 10691
	public class Building_Heater : Building_TempControl
	{
		// Token: 0x0600EBB4 RID: 60340 RVA: 0x004476C8 File Offset: 0x004458C8
		public override void TickRare()
		{
			if (!base.Spawned)
			{
				return;
			}
			if (!this.compPowerTrader.PowerOn)
			{
				return;
			}
			float ambientTemperature = base.AmbientTemperature;
			float num;
			if (ambientTemperature < 20f)
			{
				num = 1f;
			}
			else if (ambientTemperature > 120f)
			{
				num = 0f;
			}
			else
			{
				num = Mathf.InverseLerp(120f, 20f, ambientTemperature);
			}
			float energyLimit = this.compTempControl.Props.energyPerSecond * num * 4.1666665f;
			float num2 = GenTemperature.ControlTemperatureTempChange(base.Position, base.Map, energyLimit, this.compTempControl.TargetTemperature);
			bool flag = !Mathf.Approximately(num2, 0f);
			CompProperties_Power props = this.compPowerTrader.Props;
			if (flag)
			{
				this.GetRoom(RegionType.Set_All).Temperature += num2;
				this.compPowerTrader.PowerOutput = -props.PowerConsumption;
			}
			else
			{
				this.compPowerTrader.PowerOutput = -props.PowerConsumption * this.compTempControl.Props.lowPowerConsumptionFactor;
			}
			this.compTempControl.operatingAtHighPower = flag;
		}

		// Token: 0x04009C68 RID: 40040
		private const float EfficiencyFalloffSpan = 100f;
	}
}
