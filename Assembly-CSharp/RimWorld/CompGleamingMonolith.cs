using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CBA RID: 11450
	public class CompGleamingMonolith : CompVoidStructure
	{
		// Token: 0x170028A4 RID: 10404
		// (get) Token: 0x0601011C RID: 65820 RVA: 0x004A66A4 File Offset: 0x004A48A4
		protected override bool Activatable
		{
			get
			{
				return Find.Anomaly.LevelDef == MonolithLevelDefOf.Gleaming;
			}
		}

		// Token: 0x170028A5 RID: 10405
		// (get) Token: 0x0601011D RID: 65821 RVA: 0x00002501 File Offset: 0x00000701
		public override bool Active
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170028A6 RID: 10406
		// (get) Token: 0x0601011E RID: 65822 RVA: 0x00002C42 File Offset: 0x00000E42
		protected override SoundDef AmbientSound
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170028A7 RID: 10407
		// (get) Token: 0x0601011F RID: 65823 RVA: 0x004A66B7 File Offset: 0x004A48B7
		public override int TicksToActivate
		{
			get
			{
				if (!this.activated)
				{
					return base.Props.ticksToActivate;
				}
				return 600;
			}
		}

		// Token: 0x06010120 RID: 65824 RVA: 0x004A66D2 File Offset: 0x004A48D2
		protected override void OnInteracted(Pawn caster)
		{
			base.OnInteracted(caster);
			this.progress = 0f;
			caster.stances.stunner.StunFor(960, this.parent, true, false, true);
		}

		// Token: 0x0400A7EA RID: 42986
		private const int TicksToReactivate = 600;

		// Token: 0x0400A7EB RID: 42987
		private const int ActivateStunTicks = 960;
	}
}
