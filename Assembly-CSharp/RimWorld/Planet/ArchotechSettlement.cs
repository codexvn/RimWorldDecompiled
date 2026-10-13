using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DBC RID: 15804
	public class ArchotechSettlement : Settlement
	{
		// Token: 0x17003596 RID: 13718
		// (get) Token: 0x060158AE RID: 88238 RVA: 0x00673C0D File Offset: 0x00671E0D
		private IEnumerable<Building> AllStudiableArchotechBuildings
		{
			get
			{
				foreach (Building building in base.Map.listerBuildings.AllBuildingsNonColonistOfDef(ThingDefOf.GrandArchotechStructure))
				{
					yield return building;
				}
				IEnumerator<Building> enumerator = null;
				foreach (Building building2 in base.Map.listerBuildings.AllBuildingsNonColonistOfDef(ThingDefOf.MajorArchotechStructureStudiable))
				{
					yield return building2;
				}
				enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17003597 RID: 13719
		// (get) Token: 0x060158AF RID: 88239 RVA: 0x00673C20 File Offset: 0x00671E20
		public bool AnyArchotechBuildingRequiresStudy
		{
			get
			{
				foreach (Building thing in this.AllStudiableArchotechBuildings)
				{
					CompStudiable compStudiable = thing.TryGetComp<CompStudiable>();
					if (compStudiable != null && !compStudiable.Completed)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060158B0 RID: 88240 RVA: 0x00673C80 File Offset: 0x00671E80
		public override void Abandon(bool wasGravshipLaunch)
		{
			AbandonedArchotechStructures abandonedArchotechStructures = (AbandonedArchotechStructures)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedArchotechStructures);
			abandonedArchotechStructures.Tile = base.Tile;
			abandonedArchotechStructures.worldObjectDef = this.def;
			abandonedArchotechStructures.archotechStructures.AddRange(this.AllStudiableArchotechBuildings);
			foreach (Building building in abandonedArchotechStructures.archotechStructures)
			{
				building.DeSpawn(DestroyMode.Vanish);
			}
			Find.WorldObjects.Add(abandonedArchotechStructures);
			Find.LetterStack.ReceiveLetter("LetterLabelArchotechStructuresAbandoned".Translate(), "LetterTextArchotechStructuresAbandoned".Translate(this), LetterDefOf.NeutralEvent, abandonedArchotechStructures, null, null, null, null, 0, true);
			this.Destroy();
		}

		// Token: 0x060158B1 RID: 88241 RVA: 0x00673D50 File Offset: 0x00671F50
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			foreach (Building building in this.existingBuildings)
			{
				Building building2 = base.Map.listerBuildings.AllBuildingsNonColonistOfDef(building.def).FirstOrDefault((Building b) => !this.existingBuildings.Contains(b));
				if (building2 != null)
				{
					IntVec3 position = building2.Position;
					building2.DeSpawn(DestroyMode.Vanish);
					GenSpawn.Spawn(building, position, base.Map, WipeMode.Vanish);
				}
				else
				{
					building.Destroy(DestroyMode.Vanish);
				}
			}
			this.existingBuildings.Clear();
		}

		// Token: 0x060158B2 RID: 88242 RVA: 0x00673E00 File Offset: 0x00672000
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Building>(ref this.existingBuildings, "existingBuildings", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x0400EF8F RID: 61327
		public List<Building> existingBuildings = new List<Building>();
	}
}
