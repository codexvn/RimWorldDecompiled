using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020001BC RID: 444
	public class ConditionalStatAffecter_Child : ConditionalStatAffecter
	{
		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x0003A580 File Offset: 0x00038780
		public override string Label
		{
			get
			{
				return "Child".Translate();
			}
		}

		// Token: 0x06000C51 RID: 3153 RVA: 0x0003A594 File Offset: 0x00038794
		public override bool Applies(StatRequest req)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.RaceProps.Humanlike)
				{
					return pawn.DevelopmentalStage.Child();
				}
			}
			return false;
		}
	}
}
