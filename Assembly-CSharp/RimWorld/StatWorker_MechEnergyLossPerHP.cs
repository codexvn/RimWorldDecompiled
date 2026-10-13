using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200375D RID: 14173
	public class StatWorker_MechEnergyLossPerHP : StatWorker
	{
		// Token: 0x06013A50 RID: 80464 RVA: 0x005D5A0B File Offset: 0x005D3C0B
		public override string ValueToString(float val, bool finalized, ToStringNumberSense numberSense = ToStringNumberSense.Absolute)
		{
			return base.ValueToString(val * 100f, finalized, numberSense);
		}
	}
}
