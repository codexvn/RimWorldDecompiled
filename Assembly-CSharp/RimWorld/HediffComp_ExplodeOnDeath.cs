using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003856 RID: 14422
	public class HediffComp_ExplodeOnDeath : HediffComp
	{
		// Token: 0x170032EB RID: 13035
		// (get) Token: 0x06013F13 RID: 81683 RVA: 0x005EABB5 File Offset: 0x005E8DB5
		public HediffCompProperties_ExplodeOnDeath Props
		{
			get
			{
				return (HediffCompProperties_ExplodeOnDeath)this.props;
			}
		}

		// Token: 0x06013F14 RID: 81684 RVA: 0x005EABC4 File Offset: 0x005E8DC4
		public override void Notify_PawnKilled()
		{
			GenExplosion.DoExplosion(base.Pawn.Position, base.Pawn.Map, this.Props.explosionRadius, this.Props.damageDef, base.Pawn, this.Props.damageAmount, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
			if (this.Props.destroyGear)
			{
				base.Pawn.equipment.DestroyAllEquipment(DestroyMode.Vanish);
				base.Pawn.apparel.DestroyAll(DestroyMode.Vanish);
			}
		}

		// Token: 0x06013F15 RID: 81685 RVA: 0x005EACA0 File Offset: 0x005E8EA0
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			if (this.Props.destroyBody)
			{
				base.Pawn.Corpse.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
