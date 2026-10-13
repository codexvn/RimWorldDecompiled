using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D3F RID: 7487
	public class GameCondition_GrayPall : GameCondition_ForceWeather
	{
		// Token: 0x170019E8 RID: 6632
		// (get) Token: 0x0600A7C0 RID: 42944 RVA: 0x00300EAA File Offset: 0x002FF0AA
		public override int TransitionTicks
		{
			get
			{
				return 180;
			}
		}

		// Token: 0x0600A7C1 RID: 42945 RVA: 0x00300EB1 File Offset: 0x002FF0B1
		public override void Init()
		{
			if (!ModLister.CheckAnomaly("Grey pall"))
			{
				this.End();
				return;
			}
			base.Init();
		}

		// Token: 0x0600A7C2 RID: 42946 RVA: 0x0030053D File Offset: 0x002FE73D
		public override void End()
		{
			base.End();
			base.SingleMap.weatherDecider.StartNextWeather();
		}
	}
}
