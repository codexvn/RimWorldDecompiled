using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003717 RID: 14103
	public class StatPart_PlayerFactionLeader : StatPart
	{
		// Token: 0x060138D7 RID: 80087 RVA: 0x005CC5D0 File Offset: 0x005CA7D0
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.PawnIsLeader(req))
			{
				val += this.offset;
			}
		}

		// Token: 0x060138D8 RID: 80088 RVA: 0x005CC5E8 File Offset: 0x005CA7E8
		private bool PawnIsLeader(StatRequest req)
		{
			Thing thing = req.Thing;
			if (thing == null)
			{
				return false;
			}
			Faction faction = thing.Faction;
			return faction != null && faction.IsPlayer && faction.leader == thing;
		}

		// Token: 0x060138D9 RID: 80089 RVA: 0x005CC61F File Offset: 0x005CA81F
		public override string ExplanationPart(StatRequest req)
		{
			if (this.PawnIsLeader(req))
			{
				return "StatsReport_LeaderOffset".Translate() + ": " + this.offset.ToStringWithSign("0.#%");
			}
			return null;
		}

		// Token: 0x0400D8DF RID: 55519
		private float offset;
	}
}
