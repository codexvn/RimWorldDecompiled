using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003906 RID: 14598
	public class SymbolResolver_AncientCryptosleepCasket : SymbolResolver
	{
		// Token: 0x06014218 RID: 82456 RVA: 0x005FEDC8 File Offset: 0x005FCFC8
		public override void Resolve(ResolveParams rp)
		{
			int groupID = rp.ancientCryptosleepCasketGroupID ?? Find.UniqueIDsManager.GetNextAncientCryptosleepCasketGroupID();
			PodContentsType type = rp.podContentsType ?? Gen.RandomEnumValue<PodContentsType>(true);
			Rot4 rot = rp.thingRot ?? Rot4.North;
			Building_AncientCryptosleepCasket pod = RoomGenUtility.SpawnCryptoCasket(rp.rect.RandomCell, BaseGen.globalSettings.map, rot, groupID, type, ThingSetMakerDefOf.MapGen_AncientPodContents);
			if (rp.ancientCryptosleepCasketOpenSignalTag != null)
			{
				RoomGenUtility.SpawnOpenCryptoCasketSignal(pod, BaseGen.globalSettings.map, rp.ancientCryptosleepCasketOpenSignalTag);
			}
		}
	}
}
