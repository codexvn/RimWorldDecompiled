using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003789 RID: 14217
	public class Verb_PowerBeam : Verb_CastBase
	{
		// Token: 0x06013B21 RID: 80673 RVA: 0x005DA414 File Offset: 0x005D8614
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			PowerBeam powerBeam = (PowerBeam)GenSpawn.Spawn(ThingDefOf.PowerBeam, this.currentTarget.Cell, this.caster.Map, WipeMode.Vanish);
			powerBeam.duration = 600;
			powerBeam.instigator = this.caster;
			powerBeam.weaponDef = ((base.EquipmentSource != null) ? base.EquipmentSource.def : null);
			powerBeam.StartStrike();
			CompApparelReloadable reloadableCompSource = base.ReloadableCompSource;
			if (reloadableCompSource != null)
			{
				reloadableCompSource.UsedOnce();
			}
			return true;
		}

		// Token: 0x06013B22 RID: 80674 RVA: 0x005DA4BD File Offset: 0x005D86BD
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 15f;
		}

		// Token: 0x0400DA1C RID: 55836
		private const int DurationTicks = 600;
	}
}
