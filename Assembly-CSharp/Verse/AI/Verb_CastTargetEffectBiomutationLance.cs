using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E70 RID: 3696
	public class Verb_CastTargetEffectBiomutationLance : Verb_CastTargetEffect
	{
		// Token: 0x06005D22 RID: 23842 RVA: 0x001DAA40 File Offset: 0x001D8C40
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (!pawn.RaceProps.Humanlike && !pawn.IsAnimal)
				{
					if (showMessages)
					{
						Messages.Message("MessageBiomutationLanceInvalidTargetRace".Translate(pawn), this.caster, MessageTypeDefOf.RejectInput, null, false);
					}
					return false;
				}
				if (pawn.BodySize > 2.5f)
				{
					if (showMessages)
					{
						Messages.Message("MessageBiomutationLanceTargetTooBig".Translate(pawn), this.caster, MessageTypeDefOf.RejectInput, null, false);
					}
					return false;
				}
			}
			return base.ValidateTarget(target, showMessages);
		}
	}
}
