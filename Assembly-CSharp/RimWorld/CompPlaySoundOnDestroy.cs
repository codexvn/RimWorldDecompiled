using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002DAE RID: 11694
	public class CompPlaySoundOnDestroy : ThingComp
	{
		// Token: 0x170029EA RID: 10730
		// (get) Token: 0x06010712 RID: 67346 RVA: 0x004BE59A File Offset: 0x004BC79A
		private CompProperties_PlaySoundOnDestroy Props
		{
			get
			{
				return (CompProperties_PlaySoundOnDestroy)this.props;
			}
		}

		// Token: 0x06010713 RID: 67347 RVA: 0x004BE5A7 File Offset: 0x004BC7A7
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (mode == DestroyMode.Vanish && this.Props.ignoreOnVanish)
			{
				return;
			}
			if (this.Props.onlyWhenKilled && mode != DestroyMode.KillFinalize && mode != DestroyMode.KillFinalizeLeavingsOnly)
			{
				return;
			}
			if (previousMap != null)
			{
				this.Props.sound.PlayOneShotOnCamera(previousMap);
			}
		}
	}
}
