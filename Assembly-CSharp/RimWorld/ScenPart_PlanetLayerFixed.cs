using System;

namespace RimWorld
{
	// Token: 0x020028C7 RID: 10439
	public class ScenPart_PlanetLayerFixed : ScenPart_PlanetLayer
	{
		// Token: 0x170022B2 RID: 8882
		// (get) Token: 0x0600E44E RID: 58446 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool CanEdit
		{
			get
			{
				return false;
			}
		}
	}
}
