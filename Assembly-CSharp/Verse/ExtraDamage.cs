using System;

namespace Verse
{
	// Token: 0x02000266 RID: 614
	public class ExtraDamage
	{
		// Token: 0x06000F83 RID: 3971 RVA: 0x00049690 File Offset: 0x00047890
		public float AdjustedDamageAmount(Verb verb, Pawn caster)
		{
			return this.amount * verb.verbProps.GetDamageFactorFor(verb, caster);
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x000496A6 File Offset: 0x000478A6
		public float AdjustedArmorPenetration(Verb verb, Pawn caster)
		{
			if (this.armorPenetration < 0f)
			{
				return this.AdjustedDamageAmount(verb, caster) * 0.015f;
			}
			return this.armorPenetration;
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x000496CA File Offset: 0x000478CA
		public float AdjustedArmorPenetration()
		{
			if (this.armorPenetration < 0f)
			{
				return this.amount * 0.015f;
			}
			return this.armorPenetration;
		}

		// Token: 0x04000B80 RID: 2944
		public DamageDef def;

		// Token: 0x04000B81 RID: 2945
		public float amount;

		// Token: 0x04000B82 RID: 2946
		public float armorPenetration = -1f;

		// Token: 0x04000B83 RID: 2947
		public float chance = 1f;
	}
}
