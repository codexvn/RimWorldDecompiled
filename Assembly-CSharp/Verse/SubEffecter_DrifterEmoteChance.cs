using System;

namespace Verse
{
	// Token: 0x020001B5 RID: 437
	public class SubEffecter_DrifterEmoteChance : SubEffecter_DrifterEmote
	{
		// Token: 0x06000C3D RID: 3133 RVA: 0x0003A2E7 File Offset: 0x000384E7
		public SubEffecter_DrifterEmoteChance(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06000C3E RID: 3134 RVA: 0x0003A323 File Offset: 0x00038523
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (Rand.Chance(this.def.chancePerTick))
			{
				base.MakeMote(A, -1);
			}
		}
	}
}
