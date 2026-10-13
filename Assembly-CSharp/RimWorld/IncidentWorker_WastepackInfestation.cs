using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DEC RID: 7660
	public class IncidentWorker_WastepackInfestation : IncidentWorker
	{
		// Token: 0x17001A21 RID: 6689
		// (get) Token: 0x0600AAAC RID: 43692 RVA: 0x0030CB3D File Offset: 0x0030AD3D
		public override float BaseChanceThisGame
		{
			get
			{
				return base.BaseChanceThisGame * Find.Storyteller.difficulty.wastepackInfestationChanceFactor;
			}
		}

		// Token: 0x0600AAAD RID: 43693 RVA: 0x0030CB58 File Offset: 0x0030AD58
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			Map map = (Map)parms.target;
			return base.CanFireNowSub(parms) && Faction.OfInsects != null && this.GetSpawnCenter(parms, map).IsValid && CocoonInfestationUtility.GetCocoonsToSpawn(parms.points).Any<ThingDef>();
		}

		// Token: 0x0600AAAE RID: 43694 RVA: 0x0030CBB0 File Offset: 0x0030ADB0
		private IntVec3 GetSpawnCenter(IncidentParms parms, Map map)
		{
			if (CocoonInfestationUtility.CanSpawnCocoonAt(parms.spawnCenter, map))
			{
				return parms.spawnCenter;
			}
			IntVec3 result;
			if (RCellFinder.TryFindRandomCellNearWith(parms.spawnCenter, (IntVec3 c) => CocoonInfestationUtility.CanSpawnCocoonAt(c, map), map, out result, 5, 2147483647))
			{
				return result;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x0600AAAF RID: 43695 RVA: 0x0030CC14 File Offset: 0x0030AE14
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			List<Thing> targets = CocoonInfestationUtility.SpawnCocoonInfestation(this.GetSpawnCenter(parms, map), (Map)parms.target, parms.points);
			base.SendStandardLetter(parms, new LookTargets(targets), Array.Empty<NamedArgument>());
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			return true;
		}
	}
}
