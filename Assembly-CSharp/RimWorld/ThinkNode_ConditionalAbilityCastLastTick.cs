using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017CB RID: 6091
	public class ThinkNode_ConditionalAbilityCastLastTick : ThinkNode_Conditional
	{
		// Token: 0x06008DE9 RID: 36329 RVA: 0x002A17DC File Offset: 0x0029F9DC
		protected override bool Satisfied(Pawn pawn)
		{
			Pawn_AbilityTracker abilities = pawn.abilities;
			Ability ability = (abilities != null) ? abilities.GetAbility(this.ability, false) : null;
			if (ability == null)
			{
				return false;
			}
			if (ability.lastCastTick < 0)
			{
				return false;
			}
			int num = Find.TickManager.TicksGame - ability.lastCastTick;
			return num <= this.maxTicksAgo && num >= this.minTicksAgo;
		}

		// Token: 0x06008DEA RID: 36330 RVA: 0x002A183C File Offset: 0x0029FA3C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalAbilityCastLastTick thinkNode_ConditionalAbilityCastLastTick = (ThinkNode_ConditionalAbilityCastLastTick)base.DeepCopy(resolve);
			thinkNode_ConditionalAbilityCastLastTick.ability = this.ability;
			thinkNode_ConditionalAbilityCastLastTick.maxTicksAgo = this.maxTicksAgo;
			thinkNode_ConditionalAbilityCastLastTick.minTicksAgo = this.minTicksAgo;
			return thinkNode_ConditionalAbilityCastLastTick;
		}

		// Token: 0x04005BDC RID: 23516
		public int maxTicksAgo = int.MaxValue;

		// Token: 0x04005BDD RID: 23517
		public int minTicksAgo;

		// Token: 0x04005BDE RID: 23518
		public AbilityDef ability;
	}
}
