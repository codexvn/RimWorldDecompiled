using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EAE RID: 7854
	public class ComplexThreatWorker_SecurityCrate : ComplexThreatWorker
	{
		// Token: 0x17001AA4 RID: 6820
		// (get) Token: 0x0600AE2D RID: 44589 RVA: 0x0031B310 File Offset: 0x00319510
		public IEnumerable<ComplexThreatDef> SubThreats
		{
			get
			{
				yield return ComplexThreatDefOf.SleepingInsects;
				yield return ComplexThreatDefOf.SleepingMechanoids;
				yield return ComplexThreatDefOf.Infestation;
				yield return ComplexThreatDefOf.CryptosleepPods;
				yield break;
			}
		}

		// Token: 0x0600AE2E RID: 44590 RVA: 0x0031B31C File Offset: 0x0031951C
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			IntVec3 intVec;
			return base.CanResolveInt(parms) && ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientSecurityCrate, parms.room, parms.map, out intVec, 1, null) && this.SubThreats.Any((ComplexThreatDef st) => st.Worker.CanResolve(parms));
		}

		// Token: 0x0600AE2F RID: 44591 RVA: 0x0031B38C File Offset: 0x0031958C
		protected override void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings)
		{
			IntVec3 loc;
			ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientSecurityCrate, parms.room, parms.map, out loc, 1, null);
			Building_Crate building_Crate = (Building_Crate)GenSpawn.Spawn(ThingDefOf.AncientSecurityCrate, loc, parms.map, WipeMode.Vanish);
			ThingSetMakerParams parms2 = default(ThingSetMakerParams);
			List<Thing> list = ThingSetMakerDefOf.MapGen_AncientComplex_SecurityCrate.root.Generate(parms2);
			for (int i = list.Count - 1; i >= 0; i--)
			{
				if (!building_Crate.TryAcceptThing(list[i], false))
				{
					list[i].Destroy(DestroyMode.Vanish);
				}
			}
			outSpawnedThings.Add(building_Crate);
			parms.spawnedThings.Add(building_Crate);
			if (building_Crate.openedSignal.NullOrEmpty())
			{
				building_Crate.openedSignal = "CrateOpened" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			}
			parms.triggerSignal = building_Crate.openedSignal;
			ComplexThreatDef complexThreatDef;
			if ((from st in this.SubThreats
			where st.Worker.CanResolve(parms)
			select st).TryRandomElement(out complexThreatDef))
			{
				float num = 0f;
				complexThreatDef.Worker.Resolve(parms, ref num, outSpawnedThings, null);
				threatPointsUsed += num;
				return;
			}
			Log.Warning("Failed to find a viable subthreat when placing a security crate threat.");
		}
	}
}
