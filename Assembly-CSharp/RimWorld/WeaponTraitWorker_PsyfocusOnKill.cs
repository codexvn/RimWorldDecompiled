using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B2D RID: 6957
	public class WeaponTraitWorker_PsyfocusOnKill : WeaponTraitWorker
	{
		// Token: 0x06009B60 RID: 39776 RVA: 0x002D1710 File Offset: 0x002CF910
		public override void Notify_KilledPawn(Pawn pawn)
		{
			base.Notify_KilledPawn(pawn);
			Pawn_PsychicEntropyTracker psychicEntropy = pawn.psychicEntropy;
			if (psychicEntropy == null)
			{
				return;
			}
			psychicEntropy.OffsetPsyfocusDirectly(0.2f);
		}

		// Token: 0x04006BBE RID: 27582
		private const float PsyfocusOffset = 0.2f;
	}
}
