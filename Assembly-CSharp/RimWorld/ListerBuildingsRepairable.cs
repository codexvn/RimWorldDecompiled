using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F69 RID: 8041
	public class ListerBuildingsRepairable
	{
		// Token: 0x0600B1AA RID: 45482 RVA: 0x0032C778 File Offset: 0x0032A978
		public List<Thing> RepairableBuildings(Faction fac)
		{
			return this.ListFor(fac);
		}

		// Token: 0x0600B1AB RID: 45483 RVA: 0x0032C781 File Offset: 0x0032A981
		public bool Contains(Faction fac, Building b)
		{
			return this.HashSetFor(fac).Contains(b);
		}

		// Token: 0x0600B1AC RID: 45484 RVA: 0x0032C790 File Offset: 0x0032A990
		public void Notify_BuildingSpawned(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.UpdateBuilding(b);
		}

		// Token: 0x0600B1AD RID: 45485 RVA: 0x0032C7A2 File Offset: 0x0032A9A2
		public void Notify_BuildingDeSpawned(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.ListFor(b.Faction).Remove(b);
			this.HashSetFor(b.Faction).Remove(b);
		}

		// Token: 0x0600B1AE RID: 45486 RVA: 0x0032C790 File Offset: 0x0032A990
		public void Notify_BuildingTookDamage(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.UpdateBuilding(b);
		}

		// Token: 0x0600B1AF RID: 45487 RVA: 0x0032C790 File Offset: 0x0032A990
		public void Notify_BuildingRepaired(Building b)
		{
			if (b.Faction == null)
			{
				return;
			}
			this.UpdateBuilding(b);
		}

		// Token: 0x0600B1B0 RID: 45488 RVA: 0x0032C7D4 File Offset: 0x0032A9D4
		private void UpdateBuilding(Building b)
		{
			if (b.Faction == null || !b.def.building.repairable)
			{
				return;
			}
			List<Thing> list = this.ListFor(b.Faction);
			HashSet<Thing> hashSet = this.HashSetFor(b.Faction);
			if (b.HitPoints < b.MaxHitPoints)
			{
				if (!list.Contains(b))
				{
					list.Add(b);
				}
				hashSet.Add(b);
				return;
			}
			list.Remove(b);
			hashSet.Remove(b);
		}

		// Token: 0x0600B1B1 RID: 45489 RVA: 0x0032C850 File Offset: 0x0032AA50
		private List<Thing> ListFor(Faction fac)
		{
			List<Thing> list;
			if (!this.repairables.TryGetValue(fac, out list))
			{
				list = new List<Thing>();
				this.repairables.Add(fac, list);
			}
			return list;
		}

		// Token: 0x0600B1B2 RID: 45490 RVA: 0x0032C884 File Offset: 0x0032AA84
		private HashSet<Thing> HashSetFor(Faction fac)
		{
			HashSet<Thing> hashSet;
			if (!this.repairablesSet.TryGetValue(fac, out hashSet))
			{
				hashSet = new HashSet<Thing>();
				this.repairablesSet.Add(fac, hashSet);
			}
			return hashSet;
		}

		// Token: 0x0600B1B3 RID: 45491 RVA: 0x0032C8B8 File Offset: 0x0032AAB8
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				List<Thing> list = this.ListFor(faction);
				if (!list.NullOrEmpty<Thing>())
				{
					StringBuilder stringBuilder2 = stringBuilder;
					string[] array = new string[5];
					array[0] = "=======";
					array[1] = faction.Name;
					array[2] = " (";
					int num = 3;
					FactionDef def = faction.def;
					array[num] = ((def != null) ? def.ToString() : null);
					array[4] = ")";
					stringBuilder2.AppendLine(string.Concat(array));
					foreach (Thing thing in list)
					{
						stringBuilder.AppendLine(thing.ThingID);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400798A RID: 31114
		private Dictionary<Faction, List<Thing>> repairables = new Dictionary<Faction, List<Thing>>();

		// Token: 0x0400798B RID: 31115
		private Dictionary<Faction, HashSet<Thing>> repairablesSet = new Dictionary<Faction, HashSet<Thing>>();
	}
}
