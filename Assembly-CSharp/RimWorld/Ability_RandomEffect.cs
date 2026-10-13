using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020021E6 RID: 8678
	public class Ability_RandomEffect : Ability
	{
		// Token: 0x0600BE1D RID: 48669 RVA: 0x003723E8 File Offset: 0x003705E8
		public Ability_RandomEffect(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x0600BE1E RID: 48670 RVA: 0x003723F1 File Offset: 0x003705F1
		public Ability_RandomEffect(Pawn pawn, AbilityDef def) : base(pawn, def)
		{
		}

		// Token: 0x0600BE1F RID: 48671 RVA: 0x003723FC File Offset: 0x003705FC
		public override bool CanApplyOn(LocalTargetInfo target)
		{
			using (List<CompAbilityEffect>.Enumerator enumerator = base.EffectComps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CanApplyOn(target, null))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600BE20 RID: 48672 RVA: 0x0037245C File Offset: 0x0037065C
		protected override void ApplyEffects(IEnumerable<CompAbilityEffect> effects, LocalTargetInfo target, LocalTargetInfo dest)
		{
			(from x in effects
			where x.Props.weight > 0f && x.CanApplyOn(target, dest)
			select x).RandomElementByWeight((CompAbilityEffect x) => x.Props.weight).Apply(target, dest);
		}
	}
}
