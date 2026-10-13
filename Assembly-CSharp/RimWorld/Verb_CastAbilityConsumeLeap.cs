using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020022BC RID: 8892
	public class Verb_CastAbilityConsumeLeap : Verb_CastAbilityJump
	{
		// Token: 0x17001DA2 RID: 7586
		// (get) Token: 0x0600C174 RID: 49524 RVA: 0x0038049F File Offset: 0x0037E69F
		public override ThingDef JumpFlyerDef
		{
			get
			{
				return ThingDefOf.PawnFlyer_ConsumeLeap;
			}
		}
	}
}
