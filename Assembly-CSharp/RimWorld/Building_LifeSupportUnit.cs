using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020029C4 RID: 10692
	public class Building_LifeSupportUnit : Building
	{
		// Token: 0x17002454 RID: 9300
		// (get) Token: 0x0600EBB6 RID: 60342 RVA: 0x004477D8 File Offset: 0x004459D8
		private Room Room
		{
			get
			{
				Room result;
				if ((result = this.cachedRoom) == null)
				{
					result = (this.cachedRoom = this.GetRoom(RegionType.Set_All));
				}
				return result;
			}
		}

		// Token: 0x0600EBB7 RID: 60343 RVA: 0x00447800 File Offset: 0x00445A00
		public override void TickRare()
		{
			if (!base.Spawned)
			{
				return;
			}
			this.ComputeTempChange();
			this.ComputeVacuum();
			this.cachedRoom = null;
		}

		// Token: 0x0600EBB8 RID: 60344 RVA: 0x00447820 File Offset: 0x00445A20
		private void ComputeTempChange()
		{
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
			float energyLimit = 30f * num * 4.1666665f;
			float num2 = GenTemperature.ControlTemperatureTempChange(base.Position, base.Map, energyLimit, 20f);
			if (!Mathf.Approximately(num2, 0f))
			{
				this.Room.Temperature += num2;
			}
		}

		// Token: 0x0600EBB9 RID: 60345 RVA: 0x004478AC File Offset: 0x00445AAC
		private void ComputeVacuum()
		{
			if (!base.Map.Biome.inVacuum || this.Room.ExposedToSpace)
			{
				return;
			}
			float num = 100f / (float)this.Room.CellCount * 0.05f * 4.1666665f;
			this.Room.Vacuum -= num;
		}

		// Token: 0x04009C69 RID: 40041
		private Room cachedRoom;

		// Token: 0x04009C6A RID: 40042
		private const float EfficiencyFalloffSpan = 100f;

		// Token: 0x04009C6B RID: 40043
		private const float TargetTemperature = 20f;

		// Token: 0x04009C6C RID: 40044
		private const float EnergyPerSecond = 30f;

		// Token: 0x04009C6D RID: 40045
		private const float AirPerSecondPerHundredCells = 0.05f;

		// Token: 0x04009C6E RID: 40046
		private const float IntervalToPerSecond = 4.1666665f;
	}
}
