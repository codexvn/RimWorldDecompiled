using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001749 RID: 5961
	public class LordToil_Party : LordToil_Gathering
	{
		// Token: 0x06008C05 RID: 35845 RVA: 0x0029AFE5 File Offset: 0x002991E5
		public LordToil_Party(IntVec3 spot, GatheringDef gatheringDef, float joyPerTick = 3.5E-05f) : base(spot, gatheringDef)
		{
			this.joyPerTick = joyPerTick;
		}

		// Token: 0x06008C06 RID: 35846 RVA: 0x0029B004 File Offset: 0x00299204
		public override void LordToilTick()
		{
			base.LordToilTick();
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (GatheringsUtility.InGatheringArea(ownedPawns[i].Position, this.spot, base.Map))
				{
					Need_Joy joy = ownedPawns[i].needs.joy;
					if (joy != null)
					{
						joy.GainJoy(this.joyPerTick, JoyKindDefOf.Social);
					}
				}
			}
		}

		// Token: 0x04005B02 RID: 23298
		private float joyPerTick = 3.5E-05f;

		// Token: 0x04005B03 RID: 23299
		public const float DefaultJoyPerTick = 3.5E-05f;
	}
}
