using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EAB RID: 7851
	public class ComplexThreatWorker_CryptosleepPods : ComplexThreatWorker
	{
		// Token: 0x0600AE22 RID: 44578 RVA: 0x0031ABC8 File Offset: 0x00318DC8
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			IntVec3 intVec;
			return base.CanResolveInt(parms) && ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientCryptosleepPod, parms.room, parms.map, out intVec, 1, null) && parms.points >= PawnKindDefOf.AncientSoldier.combatPower && (parms.hostileFaction == null || parms.hostileFaction == Faction.OfAncientsHostile);
		}

		// Token: 0x0600AE23 RID: 44579 RVA: 0x0031AC30 File Offset: 0x00318E30
		protected override void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings)
		{
			List<Thing> list = this.SpawnCasketsWithHostiles(parms.room, parms.points, parms.triggerSignal, parms.map);
			SignalAction_OpenCasket signalAction_OpenCasket = (SignalAction_OpenCasket)ThingMaker.MakeThing(ThingDefOf.SignalAction_OpenCasket, null);
			signalAction_OpenCasket.signalTag = parms.triggerSignal;
			signalAction_OpenCasket.caskets.AddRange(list);
			signalAction_OpenCasket.completedSignalTag = "CompletedOpenAction" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			if (parms.delayTicks != null)
			{
				signalAction_OpenCasket.delayTicks = parms.delayTicks.Value;
				SignalAction_Message signalAction_Message = (SignalAction_Message)ThingMaker.MakeThing(ThingDefOf.SignalAction_Message, null);
				signalAction_Message.signalTag = parms.triggerSignal;
				signalAction_Message.lookTargets = list;
				signalAction_Message.messageType = MessageTypeDefOf.ThreatBig;
				signalAction_Message.message = "MessageSleepingThreatDelayActivated".Translate(Faction.OfAncientsHostile, signalAction_OpenCasket.delayTicks.ToStringTicksToPeriod(true, false, true, true, false));
				GenSpawn.Spawn(signalAction_Message, parms.room.rects[0].CenterCell, parms.map, WipeMode.Vanish);
			}
			GenSpawn.Spawn(signalAction_OpenCasket, parms.map.Center, parms.map, WipeMode.Vanish);
			for (int i = 0; i < list.Count; i++)
			{
				Building_Casket building_Casket = list[i] as Building_Casket;
				if (building_Casket != null)
				{
					foreach (Thing thing in ((IEnumerable<Thing>)building_Casket.GetDirectlyHeldThings()))
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null)
						{
							threatPointsUsed += pawn.kindDef.combatPower;
						}
					}
				}
			}
			SignalAction_Message signalAction_Message2 = (SignalAction_Message)ThingMaker.MakeThing(ThingDefOf.SignalAction_Message, null);
			signalAction_Message2.signalTag = signalAction_OpenCasket.completedSignalTag;
			signalAction_Message2.lookTargets = list;
			signalAction_Message2.messageType = MessageTypeDefOf.ThreatBig;
			signalAction_Message2.message = "MessageSleepingPawnsWokenUp".Translate(Faction.OfAncientsHostile.def.pawnsPlural.CapitalizeFirst());
			GenSpawn.Spawn(signalAction_Message2, parms.room.rects[0].CenterCell, parms.map, WipeMode.Vanish);
		}

		// Token: 0x0600AE24 RID: 44580 RVA: 0x0031AE7C File Offset: 0x0031907C
		private List<Thing> SpawnCasketsWithHostiles(LayoutRoom room, float threatPoints, string openSignal, Map map)
		{
			int num = Mathf.FloorToInt(threatPoints / PawnKindDefOf.AncientSoldier.combatPower);
			List<Thing> list = new List<Thing>();
			int num2 = 0;
			IntVec3 loc;
			while (num2 < num && ComplexUtility.TryFindRandomSpawnCell(ThingDefOf.AncientCryptosleepPod, room, map, out loc, 1, null))
			{
				Building_AncientCryptosleepPod building_AncientCryptosleepPod = (Building_AncientCryptosleepPod)GenSpawn.Spawn(ThingDefOf.AncientCryptosleepPod, loc, map, WipeMode.Vanish);
				building_AncientCryptosleepPod.groupID = Find.UniqueIDsManager.GetNextAncientCryptosleepCasketGroupID();
				building_AncientCryptosleepPod.openedSignal = openSignal;
				ThingSetMakerParams parms = default(ThingSetMakerParams);
				parms.podContentsType = new PodContentsType?(PodContentsType.AncientHostile);
				List<Thing> list2 = ThingSetMakerDefOf.MapGen_AncientPodContents.root.Generate(parms);
				for (int i = 0; i < list2.Count; i++)
				{
					Pawn pawn = list2[i] as Pawn;
					if (!building_AncientCryptosleepPod.TryAcceptThing(list2[i], false))
					{
						if (pawn != null)
						{
							Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
						}
						else
						{
							list2[num2].Destroy(DestroyMode.Vanish);
						}
					}
				}
				list.Add(building_AncientCryptosleepPod);
				num2++;
			}
			return list;
		}

		// Token: 0x040077CA RID: 30666
		private const string TriggerOpenAction = "TriggerOpenAction";

		// Token: 0x040077CB RID: 30667
		private const string CompletedOpenAction = "CompletedOpenAction";

		// Token: 0x040077CC RID: 30668
		private const float RoomEntryTriggerChance = 0.25f;
	}
}
