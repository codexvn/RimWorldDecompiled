using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F91 RID: 3985
	public class MentalState_EntityKiller : MentalState_SlaughterThing
	{
		// Token: 0x17000F84 RID: 3972
		// (get) Token: 0x0600624D RID: 25165 RVA: 0x001F0A90 File Offset: 0x001EEC90
		protected override bool SlaughterTargetAvailable
		{
			get
			{
				return AnomalyUtility.FindEntityOnPlatform(this.pawn.Map, EntityQueryType.ForSlaughter) != null;
			}
		}

		// Token: 0x17000F85 RID: 3973
		// (get) Token: 0x0600624E RID: 25166 RVA: 0x001F0AA6 File Offset: 0x001EECA6
		protected override int MinTicksBetweenSlaughter
		{
			get
			{
				return 625;
			}
		}

		// Token: 0x17000F86 RID: 3974
		// (get) Token: 0x0600624F RID: 25167 RVA: 0x0002F443 File Offset: 0x0002D643
		protected override int MaxThingsSlaughtered
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x06006250 RID: 25168 RVA: 0x001F0AAD File Offset: 0x001EECAD
		public override void Notify_SlaughteredTarget()
		{
			base.Notify_SlaughteredTarget();
			if (this.pawn.MentalState == this && Rand.Chance(0.5f))
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x0400466F RID: 18031
		private const float ChanceToRecoverAfterKill = 0.5f;
	}
}
