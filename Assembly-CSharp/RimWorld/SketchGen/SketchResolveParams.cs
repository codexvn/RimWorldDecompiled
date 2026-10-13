using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x0200389E RID: 14494
	public struct SketchResolveParams
	{
		// Token: 0x06014066 RID: 82022 RVA: 0x005F2AE8 File Offset: 0x005F0CE8
		public void SetCustom<T>(string name, T obj, bool inherit = false)
		{
			ResolveParamsUtility.SetCustom<T>(ref this.custom, name, obj, inherit);
		}

		// Token: 0x06014067 RID: 82023 RVA: 0x005F2AF8 File Offset: 0x005F0CF8
		public void RemoveCustom(string name)
		{
			ResolveParamsUtility.RemoveCustom(ref this.custom, name);
		}

		// Token: 0x06014068 RID: 82024 RVA: 0x005F2B06 File Offset: 0x005F0D06
		public bool TryGetCustom<T>(string name, out T obj)
		{
			return ResolveParamsUtility.TryGetCustom<T>(this.custom, name, out obj);
		}

		// Token: 0x06014069 RID: 82025 RVA: 0x005F2B15 File Offset: 0x005F0D15
		public T GetCustom<T>(string name)
		{
			return ResolveParamsUtility.GetCustom<T>(this.custom, name);
		}

		// Token: 0x0601406A RID: 82026 RVA: 0x005F2B24 File Offset: 0x005F0D24
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"sketch=",
				(this.sketch != null) ? this.sketch.ToString() : "null",
				", rect=",
				(this.rect != null) ? this.rect.ToString() : "null",
				", allowWood=",
				(this.allowWood != null) ? this.allowWood.ToString() : "null",
				", custom=",
				(this.custom != null) ? this.custom.Count.ToString() : "null",
				", symmetryOrigin=",
				(this.symmetryOrigin != null) ? this.symmetryOrigin.ToString() : "null",
				", symmetryVertical=",
				(this.symmetryVertical != null) ? this.symmetryVertical.ToString() : "null",
				", symmetryOriginIncluded=",
				(this.symmetryOriginIncluded != null) ? this.symmetryOriginIncluded.ToString() : "null",
				", symmetryClear=",
				(this.symmetryClear != null) ? this.symmetryClear.ToString() : "null",
				", connectedGroupsSameStuff=",
				(this.connectedGroupsSameStuff != null) ? this.connectedGroupsSameStuff.ToString() : "null",
				", assignRandomStuffTo=",
				(this.assignRandomStuffTo != null) ? this.assignRandomStuffTo.ToString() : "null",
				", cornerThing=",
				(this.cornerThing != null) ? this.cornerThing.ToString() : "null",
				", floorFillRoomsOnly=",
				(this.floorFillRoomsOnly != null) ? this.floorFillRoomsOnly.ToString() : "null",
				", singleFloorType=",
				(this.singleFloorType != null) ? this.singleFloorType.ToString() : "null",
				", onlyStoneFloors=",
				(this.onlyStoneFloors != null) ? this.onlyStoneFloors.ToString() : "null",
				", thingCentral=",
				(this.thingCentral != null) ? this.thingCentral.ToString() : "null",
				", wallEdgeThing=",
				(this.wallEdgeThing != null) ? this.wallEdgeThing.ToString() : "null",
				", monumentSize=",
				(this.monumentSize != null) ? this.monumentSize.ToString() : "null",
				", monumentOpen=",
				(this.monumentOpen != null) ? this.monumentOpen.ToString() : "null",
				", allowMonumentDoors=",
				(this.allowMonumentDoors != null) ? this.allowMonumentDoors.ToString() : "null",
				", allowedMonumentThings=",
				(this.allowedMonumentThings != null) ? this.allowedMonumentThings.ToString() : "null",
				", useOnlyStonesAvailableOnMap=",
				(this.useOnlyStonesAvailableOnMap != null) ? this.useOnlyStonesAvailableOnMap.ToString() : "null",
				", allowConcrete=",
				(this.allowConcrete != null) ? this.allowConcrete.ToString() : "null",
				", allowFlammableWalls=",
				(this.allowFlammableWalls != null) ? this.allowFlammableWalls.ToString() : "null",
				", onlyBuildableByPlayer=",
				(this.onlyBuildableByPlayer != null) ? this.onlyBuildableByPlayer.ToString() : "null",
				", addFloor=",
				(this.addFloors != null) ? this.addFloors.ToString() : "null",
				", requireFloor=",
				(this.requireFloor != null) ? this.requireFloor.ToString() : "null",
				", mechClusterSize=",
				(this.mechClusterSize != null) ? this.mechClusterSize.ToString() : "null",
				", mechClusterDormant=",
				(this.mechClusterDormant != null) ? this.mechClusterDormant.ToString() : "null",
				", mechClusterForMap=",
				(this.mechClusterForMap != null) ? this.mechClusterForMap.ToString() : "null"
			});
		}

		// Token: 0x0400DD30 RID: 56624
		public Sketch sketch;

		// Token: 0x0400DD31 RID: 56625
		public CellRect? rect;

		// Token: 0x0400DD32 RID: 56626
		public bool? allowWood;

		// Token: 0x0400DD33 RID: 56627
		public float? points;

		// Token: 0x0400DD34 RID: 56628
		public float? totalPoints;

		// Token: 0x0400DD35 RID: 56629
		public float? chance;

		// Token: 0x0400DD36 RID: 56630
		public int? symmetryOrigin;

		// Token: 0x0400DD37 RID: 56631
		public bool? symmetryVertical;

		// Token: 0x0400DD38 RID: 56632
		public bool? symmetryOriginIncluded;

		// Token: 0x0400DD39 RID: 56633
		public bool? symmetryClear;

		// Token: 0x0400DD3A RID: 56634
		public bool? connectedGroupsSameStuff;

		// Token: 0x0400DD3B RID: 56635
		public ThingDef assignRandomStuffTo;

		// Token: 0x0400DD3C RID: 56636
		public ThingDef cornerThing;

		// Token: 0x0400DD3D RID: 56637
		public bool? floorFillRoomsOnly;

		// Token: 0x0400DD3E RID: 56638
		public bool? singleFloorType;

		// Token: 0x0400DD3F RID: 56639
		public bool? onlyStoneFloors;

		// Token: 0x0400DD40 RID: 56640
		public ThingDef thingCentral;

		// Token: 0x0400DD41 RID: 56641
		public ThingDef wallEdgeThing;

		// Token: 0x0400DD42 RID: 56642
		public IntVec2? monumentSize;

		// Token: 0x0400DD43 RID: 56643
		public bool? monumentOpen;

		// Token: 0x0400DD44 RID: 56644
		public bool? allowMonumentDoors;

		// Token: 0x0400DD45 RID: 56645
		public ThingFilter allowedMonumentThings;

		// Token: 0x0400DD46 RID: 56646
		public Map useOnlyStonesAvailableOnMap;

		// Token: 0x0400DD47 RID: 56647
		public bool? allowConcrete;

		// Token: 0x0400DD48 RID: 56648
		public bool? allowFlammableWalls;

		// Token: 0x0400DD49 RID: 56649
		public bool? onlyBuildableByPlayer;

		// Token: 0x0400DD4A RID: 56650
		public bool? addFloors;

		// Token: 0x0400DD4B RID: 56651
		public bool? requireFloor;

		// Token: 0x0400DD4C RID: 56652
		public IntVec2? mechClusterSize;

		// Token: 0x0400DD4D RID: 56653
		public bool? mechClusterDormant;

		// Token: 0x0400DD4E RID: 56654
		public Map mechClusterForMap;

		// Token: 0x0400DD4F RID: 56655
		public bool? forceNoConditionCauser;

		// Token: 0x0400DD50 RID: 56656
		public IntVec2? utilityBuildingSize;

		// Token: 0x0400DD51 RID: 56657
		public float? destroyChanceExp;

		// Token: 0x0400DD52 RID: 56658
		public IntVec2? landingPadSize;

		// Token: 0x0400DD53 RID: 56659
		private Dictionary<string, object> custom;
	}
}
