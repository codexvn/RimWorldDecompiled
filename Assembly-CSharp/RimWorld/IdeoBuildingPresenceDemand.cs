using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200264B RID: 9803
	public class IdeoBuildingPresenceDemand : IExposable, ILoadReferenceable
	{
		// Token: 0x170020D9 RID: 8409
		// (get) Token: 0x0600D6C0 RID: 54976 RVA: 0x003FB084 File Offset: 0x003F9284
		public List<string> RoomRequirementsInfo
		{
			get
			{
				if (this.roomRequirements.NullOrEmpty<RoomRequirement>())
				{
					return null;
				}
				if (this.roomRequirementsInfoCached == null || Find.TickManager.TicksGame - this.roomRequirementsInfoCacheTick >= 20)
				{
					if (this.roomRequirementsInfoCached == null)
					{
						this.roomRequirementsInfoCached = new List<string>();
					}
					else
					{
						this.roomRequirementsInfoCached.Clear();
					}
					Thing building = (Find.CurrentMap != null) ? this.BestBuilding(Find.CurrentMap, false) : null;
					Room effectiveRoom = this.GetEffectiveRoom(building);
					foreach (RoomRequirement roomRequirement in this.roomRequirements)
					{
						if (effectiveRoom == null || !roomRequirement.MetOrDisabled(effectiveRoom, null))
						{
							this.roomRequirementsInfoCached.Add(roomRequirement.LabelCap(effectiveRoom));
						}
					}
					this.roomRequirementsInfoCacheTick = Find.TickManager.TicksGame;
				}
				return this.roomRequirementsInfoCached;
			}
		}

		// Token: 0x170020DA RID: 8410
		// (get) Token: 0x0600D6C1 RID: 54977 RVA: 0x003FB174 File Offset: 0x003F9374
		public Alert_IdeoBuildingMissing AlertCachedMissingMissing
		{
			get
			{
				if (this.alertCachedMissing == null)
				{
					this.alertCachedMissing = new Alert_IdeoBuildingMissing(this);
				}
				return this.alertCachedMissing;
			}
		}

		// Token: 0x170020DB RID: 8411
		// (get) Token: 0x0600D6C2 RID: 54978 RVA: 0x003FB190 File Offset: 0x003F9390
		public Alert_IdeoBuildingDisrespected AlertCachedMissingDisrespected
		{
			get
			{
				if (this.alertCachedDisrespected == null)
				{
					this.alertCachedDisrespected = new Alert_IdeoBuildingDisrespected(this);
				}
				return this.alertCachedDisrespected;
			}
		}

		// Token: 0x0600D6C3 RID: 54979 RVA: 0x003FB1AC File Offset: 0x003F93AC
		public IdeoBuildingPresenceDemand()
		{
		}

		// Token: 0x0600D6C4 RID: 54980 RVA: 0x003FB1D4 File Offset: 0x003F93D4
		public IdeoBuildingPresenceDemand(Precept_Building precept)
		{
			this.parent = precept;
			this.ID = Find.UniqueIDsManager.GetNextPresenceDemandID();
		}

		// Token: 0x0600D6C5 RID: 54981 RVA: 0x003FB214 File Offset: 0x003F9414
		public bool AppliesTo(Map map)
		{
			return map.IsPlayerHome && (this.parent.ThingDef.ritualFocus == null || !this.parent.ThingDef.ritualFocus.consumable) && (DebugSettings.activateAllBuildingDemands || ExpectationsUtility.CurrentExpectationFor(map).order >= this.minExpectation.order);
		}

		// Token: 0x0600D6C6 RID: 54982 RVA: 0x003FB27C File Offset: 0x003F947C
		public bool BuildingPresent(Map map)
		{
			Faction playerFact = Faction.OfPlayer;
			return map.listerThings.ThingsOfDef(this.parent.ThingDef).Any(delegate(Thing t)
			{
				Building building = t as Building;
				return building != null && building.StyleSourcePrecept == this.parent && building.Faction == playerFact;
			});
		}

		// Token: 0x0600D6C7 RID: 54983 RVA: 0x003FB2C8 File Offset: 0x003F94C8
		public bool RequirementsSatisfied(Map map)
		{
			if (this.roomRequirements.NullOrEmpty<RoomRequirement>())
			{
				return true;
			}
			foreach (Thing thing in map.listerThings.ThingsOfDef(this.parent.ThingDef))
			{
				Building building = thing as Building;
				if (building != null && building.StyleSourcePrecept == this.parent && building.GetRoom(RegionType.Set_All) != null && this.NumRequirementsMet(building) == this.roomRequirements.Count)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D6C8 RID: 54984 RVA: 0x003FB370 File Offset: 0x003F9570
		private int NumRequirementsMet(Thing building)
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (this.buildingRequirementsMetCacheTick != ticksGame)
			{
				this.buildingRequirementsMetCache.Clear();
				this.buildingRequirementsMetCacheTick = ticksGame;
			}
			int num;
			if (this.buildingRequirementsMetCache.TryGetValue(building, out num))
			{
				return num;
			}
			num = this.NumRequirementsMetCalc(building);
			this.buildingRequirementsMetCache[building] = num;
			return num;
		}

		// Token: 0x0600D6C9 RID: 54985 RVA: 0x003FB3CC File Offset: 0x003F95CC
		private int NumRequirementsMetCalc(Thing building)
		{
			if (this.roomRequirements.NullOrEmpty<RoomRequirement>())
			{
				return 0;
			}
			Room effectiveRoom = this.GetEffectiveRoom(building);
			if (effectiveRoom == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < this.roomRequirements.Count; i++)
			{
				if (this.roomRequirements[i].MetOrDisabled(effectiveRoom, null))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600D6CA RID: 54986 RVA: 0x003FB428 File Offset: 0x003F9628
		public Room GetEffectiveRoom(Thing building)
		{
			Room room = (building != null) ? building.GetRoom(RegionType.Set_All) : null;
			if (room == null || room.PsychologicallyOutdoors)
			{
				return null;
			}
			return room;
		}

		// Token: 0x0600D6CB RID: 54987 RVA: 0x003FB454 File Offset: 0x003F9654
		public Thing BestBuilding(Map map, bool ignoreFullyMetRequirements = false)
		{
			Thing result = null;
			int num = -1;
			foreach (Thing thing in map.listerThings.ThingsOfDef(this.parent.ThingDef))
			{
				Building building = thing as Building;
				if (building != null && building.StyleSourcePrecept == this.parent)
				{
					int num2 = this.NumRequirementsMet(building);
					if (this.roomRequirements == null || num2 != this.roomRequirements.Count || !ignoreFullyMetRequirements)
					{
						if (this.GetEffectiveRoom(building) != null)
						{
							num2++;
						}
						if (num2 > num)
						{
							result = building;
							num = num2;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600D6CC RID: 54988 RVA: 0x003FB50C File Offset: 0x003F970C
		public IEnumerable<Thing> AllBuildings(Map map)
		{
			foreach (Thing thing in map.listerThings.ThingsOfDef(this.parent.ThingDef))
			{
				Building building = thing as Building;
				if (building != null && building.StyleSourcePrecept == this.parent)
				{
					yield return building;
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600D6CD RID: 54989 RVA: 0x003FB524 File Offset: 0x003F9724
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Defs.Look<ExpectationDef>(ref this.minExpectation, "minExpectation");
			Scribe_Collections.Look<RoomRequirement>(ref this.roomRequirements, "roomRequirements", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (GameDataSaveLoader.IsSavingOrLoadingExternalIdeo)
				{
					this.ID = Find.UniqueIDsManager.GetNextPresenceDemandID();
				}
				if (this.roomRequirements != null)
				{
					if (this.roomRequirements.RemoveAll((RoomRequirement x) => x == null) != 0)
					{
						Log.Error("Removing null room requirements.");
					}
				}
			}
		}

		// Token: 0x0600D6CE RID: 54990 RVA: 0x003FB5C6 File Offset: 0x003F97C6
		public string GetUniqueLoadID()
		{
			return "IdeoBuildingPresenceDemand_" + this.ID.ToString();
		}

		// Token: 0x0600D6CF RID: 54991 RVA: 0x003FB5DD File Offset: 0x003F97DD
		public IdeoBuildingPresenceDemand Copy()
		{
			return new IdeoBuildingPresenceDemand
			{
				ID = this.ID,
				parent = this.parent,
				minExpectation = this.minExpectation,
				roomRequirements = this.roomRequirements
			};
		}

		// Token: 0x040091F0 RID: 37360
		public Precept_Building parent;

		// Token: 0x040091F1 RID: 37361
		public ExpectationDef minExpectation;

		// Token: 0x040091F2 RID: 37362
		public List<RoomRequirement> roomRequirements;

		// Token: 0x040091F3 RID: 37363
		public int ID = -1;

		// Token: 0x040091F4 RID: 37364
		private List<string> roomRequirementsInfoCached;

		// Token: 0x040091F5 RID: 37365
		private int roomRequirementsInfoCacheTick = -1;

		// Token: 0x040091F6 RID: 37366
		private Dictionary<Thing, int> buildingRequirementsMetCache = new Dictionary<Thing, int>();

		// Token: 0x040091F7 RID: 37367
		private int buildingRequirementsMetCacheTick = -1;

		// Token: 0x040091F8 RID: 37368
		private Alert_IdeoBuildingMissing alertCachedMissing;

		// Token: 0x040091F9 RID: 37369
		private Alert_IdeoBuildingDisrespected alertCachedDisrespected;
	}
}
