using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E6B RID: 11883
	public class CompStunOnDamage : ThingComp
	{
		// Token: 0x17002AF2 RID: 10994
		// (get) Token: 0x06010BBE RID: 68542 RVA: 0x004D2EF2 File Offset: 0x004D10F2
		private CompProperties_StunOnDamage Props
		{
			get
			{
				return (CompProperties_StunOnDamage)this.props;
			}
		}

		// Token: 0x06010BBF RID: 68543 RVA: 0x004D2F00 File Offset: 0x004D1100
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (dinfo.Def == this.Props.damage)
			{
				Pawn_StanceTracker stances = ((Pawn)this.parent).stances;
				if (stances == null)
				{
					return;
				}
				StunHandler stunner = stances.stunner;
				if (stunner == null)
				{
					return;
				}
				stunner.StunFor(this.Props.delayTicks, dinfo.Instigator, false, true, false);
			}
		}
	}
}
