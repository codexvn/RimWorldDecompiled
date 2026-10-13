using System;

namespace RimWorld
{
	// Token: 0x02001D39 RID: 7481
	public class GameCondition_DisableElectricity : GameCondition
	{
		// Token: 0x170019E4 RID: 6628
		// (get) Token: 0x0600A7A3 RID: 42915 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool ElectricityDisabled
		{
			get
			{
				return true;
			}
		}
	}
}
