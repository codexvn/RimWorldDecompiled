using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

// Token: 0x0200000F RID: 15
public class LayoutWorkerComplex : LayoutWorker_Structure
{
	// Token: 0x06000037 RID: 55 RVA: 0x00002C2C File Offset: 0x00000E2C
	public LayoutWorkerComplex(LayoutDef def) : base(def)
	{
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000038 RID: 56 RVA: 0x00002C35 File Offset: 0x00000E35
	public new ComplexLayoutDef Def
	{
		get
		{
			return (ComplexLayoutDef)base.Def;
		}
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002C42 File Offset: 0x00000E42
	public virtual Faction GetFixedHostileFactionForThreats()
	{
		return null;
	}

	// Token: 0x0600003A RID: 58 RVA: 0x000026EA File Offset: 0x000008EA
	protected virtual void PreSpawnThreats(List<LayoutRoom> rooms, Map map, List<Thing> allSpawnedThings)
	{
	}

	// Token: 0x0600003B RID: 59 RVA: 0x00002C48 File Offset: 0x00000E48
	protected override StructureLayout GetStructureLayout(StructureGenParams parms, CellRect rect)
	{
		LayoutStructureSketch sketch = parms.sketch;
		float areaPrunePercent = this.Def.areaPrunePercent;
		int minRoomHeight = this.Def.minRoomHeight;
		int minRoomWidth = this.Def.minRoomWidth;
		int minRoomHeight2 = minRoomHeight;
		float areaPrunePercent2 = areaPrunePercent;
		bool canRemoveRooms = true;
		bool generateDoors = false;
		IntRange? maxMergeRoomsRange = new IntRange?(LayoutWorkerComplex.MergeRange);
		return RoomLayoutGenerator.GenerateRandomLayout(sketch, rect, minRoomWidth, minRoomHeight2, areaPrunePercent2, canRemoveRooms, generateDoors, this.Def.corridorDef, 2, maxMergeRoomsRange, this.Def.corridorShapes, this.Def.canDisconnectRooms, false);
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00002CB8 File Offset: 0x00000EB8
	public override void Spawn(LayoutStructureSketch layoutStructureSketch, Map map, IntVec3 pos, float? threatPoints = null, List<Thing> allSpawnedThings = null, bool roofs = true, bool canReuseSketch = false, Faction faction = null)
	{
		List<Thing> list = allSpawnedThings ?? new List<Thing>();
		base.Spawn(layoutStructureSketch, map, pos, threatPoints, list, roofs, canReuseSketch, faction);
		List<LayoutRoom> rooms = layoutStructureSketch.structureLayout.Rooms;
		LayoutWorkerComplex.SpawnThings(layoutStructureSketch, map, rooms, list);
		if (threatPoints != null && !this.Def.threats.NullOrEmpty<ComplexThreat>())
		{
			this.PreSpawnThreats(rooms, map, list);
			this.SpawnThreats(layoutStructureSketch, map, pos, threatPoints.Value, list, rooms);
		}
		this.PostSpawnStructure(rooms, map, list);
		LayoutWorkerComplex.tmpSpawnedThreatThings.Clear();
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00002D44 File Offset: 0x00000F44
	private static void SpawnThings(LayoutStructureSketch layoutStructureSketch, Map map, List<LayoutRoom> rooms, List<Thing> spawnedThings)
	{
		if (layoutStructureSketch.thingsToSpawn.NullOrEmpty<Thing>())
		{
			return;
		}
		HashSet<LayoutRoom> usedRooms = new HashSet<LayoutRoom>();
		for (int i = layoutStructureSketch.thingsToSpawn.Count - 1; i >= 0; i--)
		{
			Thing thing = layoutStructureSketch.thingsToSpawn[i];
			LayoutRoom layoutRoom;
			Rot4 rot;
			IntVec3 loc = LayoutWorker.FindBestSpawnLocation(rooms, thing.def, map, out layoutRoom, out rot, usedRooms);
			if (!loc.IsValid)
			{
				loc = LayoutWorker.FindBestSpawnLocation(rooms, thing.def, map, out layoutRoom, out rot, null);
			}
			if (!loc.IsValid)
			{
				thing.Destroy(DestroyMode.Vanish);
				layoutStructureSketch.thingsToSpawn.RemoveAt(i);
			}
			else
			{
				GenSpawn.Spawn(thing, loc, map, rot, WipeMode.Vanish, false, false);
				spawnedThings.Add(thing);
				layoutStructureSketch.thingsToSpawn.RemoveAt(i);
				if (!layoutStructureSketch.thingDiscoveredMessage.NullOrEmpty())
				{
					string signalTag = "ThingDiscovered" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
					foreach (CellRect rect in layoutRoom.rects)
					{
						RectTrigger rectTrigger = (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
						rectTrigger.signalTag = signalTag;
						rectTrigger.Rect = rect;
						GenSpawn.Spawn(rectTrigger, rect.CenterCell, map, WipeMode.Vanish);
					}
					SignalAction_Message signalAction_Message = (SignalAction_Message)ThingMaker.MakeThing(ThingDefOf.SignalAction_Message, null);
					signalAction_Message.signalTag = signalTag;
					signalAction_Message.message = layoutStructureSketch.thingDiscoveredMessage;
					signalAction_Message.messageType = MessageTypeDefOf.PositiveEvent;
					signalAction_Message.lookTargets = thing;
					GenSpawn.Spawn(signalAction_Message, loc, map, WipeMode.Vanish);
				}
			}
		}
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00002EE8 File Offset: 0x000010E8
	protected virtual void PostSpawnStructure(List<LayoutRoom> rooms, Map map, List<Thing> allSpawnedThings)
	{
		if (ModsConfig.IdeologyActive)
		{
			this.SpawnRoomRewards(rooms, map, allSpawnedThings);
			LayoutWorkerComplex.SpawnCommsConsole(rooms, map);
		}
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00002F04 File Offset: 0x00001104
	private static void SpawnCommsConsole(List<LayoutRoom> rooms, Map map)
	{
		Func<IntVec3, bool> <>9__0;
		foreach (LayoutRoom layoutRoom in rooms.InRandomOrder(null))
		{
			ThingDef commsConsole = ThingDefOf.CommsConsole;
			Map map2 = map;
			Rot4? rot = null;
			int contractedBy = 1;
			int padding = 0;
			Func<IntVec3, bool> validator;
			if ((validator = <>9__0) == null)
			{
				validator = (<>9__0 = ((IntVec3 c) => LayoutWorkerComplex.CanPlaceCommsConsoleAt(c, map)));
			}
			IntVec3 loc;
			if (layoutRoom.TryGetRandomCellInRoom(commsConsole, map2, out loc, rot, contractedBy, padding, validator, false))
			{
				GenSpawn.Spawn(ThingDefOf.AncientCommsConsole, loc, map, WipeMode.Vanish);
				break;
			}
		}
	}

	// Token: 0x06000040 RID: 64 RVA: 0x00002FB0 File Offset: 0x000011B0
	private static bool CanPlaceCommsConsoleAt(IntVec3 cell, Map map)
	{
		using (CellRect.Enumerator enumerator = GenAdj.OccupiedRect(cell, Rot4.North, ThingDefOf.AncientCommsConsole.Size).ExpandedBy(1).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.GetEdifice(map) != null)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00003028 File Offset: 0x00001228
	private void SpawnRoomRewards(List<LayoutRoom> rooms, Map map, List<Thing> allSpawnedThings)
	{
		LayoutWorkerComplex.<>c__DisplayClass14_0 CS$<>8__locals1 = new LayoutWorkerComplex.<>c__DisplayClass14_0();
		CS$<>8__locals1.map = map;
		if (this.Def.roomRewardCrateFactor <= 0f)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < allSpawnedThings.Count; i++)
		{
			if (allSpawnedThings[i] is Building_Crate)
			{
				num++;
			}
		}
		int num2 = Mathf.RoundToInt((float)rooms.Count * this.Def.roomRewardCrateFactor) - num;
		if (num2 <= 0)
		{
			return;
		}
		ThingSetMakerDef thingSetMakerDef = this.Def.rewardThingSetMakerDef ?? ThingSetMakerDefOf.Reward_ItemsStandard;
		foreach (LayoutRoom layoutRoom in rooms.InRandomOrder(null))
		{
			if (layoutRoom.requiredDef == null)
			{
				LayoutRoom layoutRoom2 = layoutRoom;
				ThingDef ancientHermeticCrate = ThingDefOf.AncientHermeticCrate;
				Map map2 = CS$<>8__locals1.map;
				Func<IntVec3, bool> validator = new Func<IntVec3, bool>(CS$<>8__locals1.<SpawnRoomRewards>g__CanSpawnAt|0);
				IntVec3 loc;
				if (layoutRoom2.TryGetRandomCellInRoom(ancientHermeticCrate, map2, out loc, null, 2, 0, validator, false))
				{
					Building_Crate building_Crate = (Building_Crate)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientHermeticCrate, null), loc, CS$<>8__locals1.map, Rot4.South, WipeMode.Vanish, false, false);
					List<Thing> list = thingSetMakerDef.root.Generate(default(ThingSetMakerParams));
					for (int j = list.Count - 1; j >= 0; j--)
					{
						Thing thing = list[j];
						if (!building_Crate.TryAcceptThing(thing, false))
						{
							thing.Destroy(DestroyMode.Vanish);
						}
					}
					num2--;
				}
				if (num2 <= 0)
				{
					break;
				}
			}
		}
	}

	// Token: 0x06000042 RID: 66 RVA: 0x000031B8 File Offset: 0x000013B8
	private static List<ComplexThreatDef> GetUsedThreats(LayoutRoom room)
	{
		int roomMergedLowestID = LayoutWorkerComplex.GetRoomMergedLowestID(room);
		return LayoutWorkerComplex.usedThreatsByRoom[roomMergedLowestID];
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000031D7 File Offset: 0x000013D7
	private static bool TryGetRoomUsedThreats(LayoutRoom room, out List<ComplexThreatDef> threats)
	{
		return LayoutWorkerComplex.usedThreatsByRoom.TryGetValue(LayoutWorkerComplex.GetRoomMergedLowestID(room), out threats);
	}

	// Token: 0x06000044 RID: 68 RVA: 0x000031EC File Offset: 0x000013EC
	private static void AddUsedThreat(LayoutRoom room, ComplexThreatDef threat)
	{
		int roomMergedLowestID = LayoutWorkerComplex.GetRoomMergedLowestID(room);
		List<ComplexThreatDef> list;
		if (!LayoutWorkerComplex.usedThreatsByRoom.TryGetValue(roomMergedLowestID, out list))
		{
			list = (LayoutWorkerComplex.usedThreatsByRoom[roomMergedLowestID] = new List<ComplexThreatDef>());
		}
		list.Add(threat);
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00003228 File Offset: 0x00001428
	private static int GetRoomMergedLowestID(LayoutRoom room)
	{
		int loadId = room.loadId;
		for (int i = 0; i < room.merged.Count; i++)
		{
			if (room.merged[i].loadId < loadId)
			{
				loadId = room.merged[i].loadId;
			}
		}
		return loadId;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x0000327C File Offset: 0x0000147C
	private void SpawnThreats(LayoutStructureSketch structureSketch, Map map, IntVec3 center, float threatPoints, List<Thing> spawnedThings, List<LayoutRoom> rooms)
	{
		ComplexResolveParams threatParams = new ComplexResolveParams
		{
			map = map,
			complexRect = structureSketch.layoutSketch.OccupiedRect.MovedBy(center),
			hostileFaction = this.GetFixedHostileFactionForThreats(),
			allRooms = rooms,
			points = threatPoints
		};
		StringBuilder stringBuilder = null;
		if (DebugViewSettings.logComplexGenPoints)
		{
			stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("----- Logging points for " + this.Def.defName + ". -----");
			stringBuilder.AppendLine(string.Format("Total threat points: {0}", threatPoints));
			stringBuilder.AppendLine(string.Format("Room count: {0}", rooms.Count));
			stringBuilder.AppendLine(string.Format("Approx points per room: {0}", threatParams.points));
			if (threatParams.hostileFaction != null)
			{
				stringBuilder.AppendLine(string.Format("Faction: {0}", threatParams.hostileFaction));
			}
		}
		LayoutWorkerComplex.useableThreats.Clear();
		LayoutWorkerComplex.usedThreatsByRoom.Clear();
		LayoutWorkerComplex.useableThreats.AddRange(from t in this.Def.threats
		where Rand.Chance(t.chancePerComplex)
		select t);
		float num = 0f;
		int num2 = 100;
		while (num < threatPoints && num2 > 0)
		{
			num2--;
			LayoutRoom room = rooms.RandomElement<LayoutRoom>();
			threatParams.room = room;
			threatParams.spawnedThings = spawnedThings;
			float b = threatPoints - num;
			threatParams.points = Mathf.Min(LayoutWorkerComplex.ThreatPointsFactorRange.RandomInRange * threatPoints, b);
			ComplexThreat complexThreat;
			if (LayoutWorkerComplex.useableThreats.Where(delegate(ComplexThreat t)
			{
				int num4 = 0;
				Predicate<ComplexThreatDef> <>9__4;
				foreach (KeyValuePair<int, List<ComplexThreatDef>> keyValuePair in LayoutWorkerComplex.usedThreatsByRoom)
				{
					int num5;
					List<ComplexThreatDef> list;
					keyValuePair.Deconstruct(out num5, out list);
					List<ComplexThreatDef> list2 = list;
					int num6 = num4;
					List<ComplexThreatDef> list3 = list2;
					Predicate<ComplexThreatDef> predicate;
					if ((predicate = <>9__4) == null)
					{
						predicate = (<>9__4 = ((ComplexThreatDef td) => td == t.def));
					}
					num4 = num6 + list3.Count(predicate);
				}
				List<ComplexThreatDef> list4;
				return num4 < t.maxPerComplex && (!LayoutWorkerComplex.TryGetRoomUsedThreats(room, out list4) || list4.Count((ComplexThreatDef td) => td == t.def) < t.maxPerRoom) && t.def.Worker.CanResolve(threatParams);
			}).TryRandomElementByWeight((ComplexThreat t) => t.selectionWeight, out complexThreat))
			{
				if (stringBuilder != null)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("-> Resolving threat " + complexThreat.def.defName);
				}
				float num3 = 0f;
				complexThreat.def.Worker.Resolve(threatParams, ref num3, LayoutWorkerComplex.tmpSpawnedThreatThings, stringBuilder);
				num += num3;
				LayoutWorkerComplex.AddUsedThreat(room, complexThreat.def);
			}
		}
		if (stringBuilder != null)
		{
			stringBuilder.AppendLine(string.Format("Total threat points spent: {0}", num));
			Log.Message(stringBuilder.ToString());
		}
		LayoutWorkerComplex.useableThreats.Clear();
		LayoutWorkerComplex.usedThreatsByRoom.Clear();
	}

	// Token: 0x04000011 RID: 17
	private static readonly FloatRange ThreatPointsFactorRange = new FloatRange(0.25f, 0.35f);

	// Token: 0x04000012 RID: 18
	private static readonly List<Thing> tmpSpawnedThreatThings = new List<Thing>();

	// Token: 0x04000013 RID: 19
	private static readonly IntRange MergeRange = new IntRange(1, 4);

	// Token: 0x04000014 RID: 20
	private static readonly List<ComplexThreat> useableThreats = new List<ComplexThreat>();

	// Token: 0x04000015 RID: 21
	private static readonly Dictionary<int, List<ComplexThreatDef>> usedThreatsByRoom = new Dictionary<int, List<ComplexThreatDef>>();
}
