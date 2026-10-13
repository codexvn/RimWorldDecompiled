using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200377C RID: 14204
	public class Verb_DeployDeadlifePack : Verb
	{
		// Token: 0x06013ADB RID: 80603 RVA: 0x005D8BF2 File Offset: 0x005D6DF2
		protected override bool TryCastShot()
		{
			return Verb_DeployDeadlifePack.TryDeploy(base.EquipmentSource.TryGetComp<CompApparelReloadable>(), base.EquipmentSource.TryGetComp<CompReleaseGas>());
		}

		// Token: 0x06013ADC RID: 80604 RVA: 0x005D8C10 File Offset: 0x005D6E10
		public static bool TryDeploy(CompApparelReloadable reloadable, CompReleaseGas releaseGas)
		{
			if (!ModLister.CheckAnomaly("Deadlife packs"))
			{
				return false;
			}
			string text;
			if (reloadable == null || !reloadable.CanBeUsed(out text) || releaseGas == null)
			{
				return false;
			}
			reloadable.UsedOnce();
			releaseGas.StartRelease();
			return true;
		}
	}
}
