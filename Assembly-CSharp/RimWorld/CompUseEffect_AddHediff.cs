using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F53 RID: 12115
	public class CompUseEffect_AddHediff : CompUseEffect
	{
		// Token: 0x17002C2D RID: 11309
		// (get) Token: 0x0601114C RID: 69964 RVA: 0x004E7CB4 File Offset: 0x004E5EB4
		public CompProperties_UseEffectAddHediff Props
		{
			get
			{
				return (CompProperties_UseEffectAddHediff)this.props;
			}
		}

		// Token: 0x0601114D RID: 69965 RVA: 0x004E7CC4 File Offset: 0x004E5EC4
		public override void DoEffect(Pawn user)
		{
			user.health.AddHediff(this.Props.hediffDef, null, null, null);
		}

		// Token: 0x0601114E RID: 69966 RVA: 0x004E7CF4 File Offset: 0x004E5EF4
		public override AcceptanceReport CanBeUsedBy(Pawn p)
		{
			if (!this.Props.allowRepeatedUse && p.health.hediffSet.HasHediff(this.Props.hediffDef, false))
			{
				return "AlreadyHasHediff".Translate(this.Props.hediffDef.label);
			}
			return true;
		}
	}
}
