using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007F2 RID: 2034
	public class Stance_Cooldown : Stance_Busy
	{
		// Token: 0x060033FC RID: 13308 RVA: 0x00110EF9 File Offset: 0x0010F0F9
		public Stance_Cooldown()
		{
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x00110F01 File Offset: 0x0010F101
		public Stance_Cooldown(int ticks, LocalTargetInfo focusTarg, Verb verb) : base(ticks, focusTarg, verb)
		{
		}

		// Token: 0x060033FE RID: 13310 RVA: 0x00110F0C File Offset: 0x0010F10C
		public override void StanceDraw()
		{
			if (Find.Selector.IsSelected(this.stanceTracker.pawn))
			{
				float radius = Mathf.Min(0.5f, (float)this.ticksLeft * 0.002f);
				GenDraw.DrawCooldownCircle(this.stanceTracker.pawn.Drawer.DrawPos + new Vector3(0f, 0.2f, 0f), radius);
			}
		}

		// Token: 0x0400273B RID: 10043
		private const float RadiusPerTick = 0.002f;

		// Token: 0x0400273C RID: 10044
		private const float MaxRadius = 0.5f;
	}
}
