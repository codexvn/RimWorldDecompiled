using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003779 RID: 14201
	public class Verb_Bombardment : Verb_CastBase
	{
		// Token: 0x06013AD1 RID: 80593 RVA: 0x005D89E4 File Offset: 0x005D6BE4
		protected override bool TryCastShot()
		{
			if (this.currentTarget.HasThing && this.currentTarget.Thing.Map != this.caster.Map)
			{
				return false;
			}
			Bombardment bombardment = (Bombardment)GenSpawn.Spawn(ThingDefOf.Bombardment, this.currentTarget.Cell, this.caster.Map, WipeMode.Vanish);
			bombardment.duration = 540;
			bombardment.instigator = this.caster;
			bombardment.weaponDef = ((base.EquipmentSource != null) ? base.EquipmentSource.def : null);
			CompApparelReloadable reloadableCompSource = base.ReloadableCompSource;
			if (reloadableCompSource != null)
			{
				reloadableCompSource.UsedOnce();
			}
			return true;
		}

		// Token: 0x06013AD2 RID: 80594 RVA: 0x005D8A87 File Offset: 0x005D6C87
		public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
		{
			needLOSToCenter = false;
			return 23f;
		}

		// Token: 0x0400DA03 RID: 55811
		public const int DurationTicks = 540;
	}
}
