using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200377B RID: 14203
	public class Verb_DeployToxPack : Verb
	{
		// Token: 0x06013AD8 RID: 80600 RVA: 0x005D8B99 File Offset: 0x005D6D99
		protected override bool TryCastShot()
		{
			return Verb_DeployToxPack.TryDeploy(base.EquipmentSource.TryGetComp<CompApparelReloadable>(), base.EquipmentSource.TryGetComp<CompReleaseGas>());
		}

		// Token: 0x06013AD9 RID: 80601 RVA: 0x005D8BB8 File Offset: 0x005D6DB8
		public static bool TryDeploy(CompApparelReloadable reloadable, CompReleaseGas releaseGas)
		{
			if (!ModLister.CheckBiotech("Tox packs"))
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
