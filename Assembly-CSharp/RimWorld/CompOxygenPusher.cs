using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D98 RID: 11672
	public class CompOxygenPusher : ThingComp
	{
		// Token: 0x170029D4 RID: 10708
		// (get) Token: 0x0601069E RID: 67230 RVA: 0x004BC835 File Offset: 0x004BAA35
		private CompProperties_OxygenPusher Props
		{
			get
			{
				return (CompProperties_OxygenPusher)this.props;
			}
		}

		// Token: 0x170029D5 RID: 10709
		// (get) Token: 0x0601069F RID: 67231 RVA: 0x004BC844 File Offset: 0x004BAA44
		public CompPowerTrader PowerTrader
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

		// Token: 0x060106A0 RID: 67232 RVA: 0x004BC870 File Offset: 0x004BAA70
		public override void CompTickRare()
		{
			if (this.Props.requiresPower && this.PowerTrader.Off)
			{
				return;
			}
			Room room = this.parent.GetRoom(RegionType.Set_All);
			float num = 100f / (float)room.CellCount * this.Props.airPerSecondPerHundredCells * 4.1666665f;
			room.Vacuum = room.UnsanitizedVacuum - num;
		}

		// Token: 0x0400AB3E RID: 43838
		private const float IntervalToPerSecond = 4.1666665f;

		// Token: 0x0400AB3F RID: 43839
		private CompPowerTrader intPowerTrader;
	}
}
