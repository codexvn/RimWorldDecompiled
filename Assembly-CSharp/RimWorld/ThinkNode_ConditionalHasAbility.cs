using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017CA RID: 6090
	public class ThinkNode_ConditionalHasAbility : ThinkNode_Conditional
	{
		// Token: 0x06008DE6 RID: 36326 RVA: 0x002A17A3 File Offset: 0x0029F9A3
		protected override bool Satisfied(Pawn pawn)
		{
			Pawn_AbilityTracker abilities = pawn.abilities;
			return ((abilities != null) ? abilities.GetAbility(this.ability, false) : null) != null;
		}

		// Token: 0x06008DE7 RID: 36327 RVA: 0x002A17C1 File Offset: 0x0029F9C1
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalHasAbility thinkNode_ConditionalHasAbility = (ThinkNode_ConditionalHasAbility)base.DeepCopy(resolve);
			thinkNode_ConditionalHasAbility.ability = this.ability;
			return thinkNode_ConditionalHasAbility;
		}

		// Token: 0x04005BDB RID: 23515
		public AbilityDef ability;
	}
}
