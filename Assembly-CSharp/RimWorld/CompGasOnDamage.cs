using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CAA RID: 11434
	public class CompGasOnDamage : ThingComp
	{
		// Token: 0x17002892 RID: 10386
		// (get) Token: 0x060100D0 RID: 65744 RVA: 0x004A592D File Offset: 0x004A3B2D
		private CompProperties_GasOnDamage Props
		{
			get
			{
				return (CompProperties_GasOnDamage)this.props;
			}
		}

		// Token: 0x060100D1 RID: 65745 RVA: 0x004A593C File Offset: 0x004A3B3C
		public override void PostPreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PostPreApplyDamage(ref dinfo, out absorbed);
			if (dinfo.Def.ExternalViolenceFor(this.parent))
			{
				float num = Mathf.Min(dinfo.Amount, (float)this.parent.HitPoints) * this.Props.damageFactor;
				if (this.Props.useStackCountAsFactor)
				{
					num *= (float)this.parent.stackCount;
				}
				GasUtility.AddGas(this.parent.PositionHeld, this.parent.MapHeld, this.Props.type, (int)num);
			}
		}
	}
}
