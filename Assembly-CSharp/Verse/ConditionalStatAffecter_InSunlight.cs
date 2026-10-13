using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020001B9 RID: 441
	public class ConditionalStatAffecter_InSunlight : ConditionalStatAffecter
	{
		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000C47 RID: 3143 RVA: 0x0003A3CB File Offset: 0x000385CB
		public override string Label
		{
			get
			{
				return "StatsReport_InSunlight".Translate();
			}
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x0003A3DC File Offset: 0x000385DC
		public override bool Applies(StatRequest req)
		{
			return ModsConfig.BiotechActive && (req.HasThing && req.Thing.Spawned) && req.Thing.Position.InSunlight(req.Thing.Map);
		}
	}
}
