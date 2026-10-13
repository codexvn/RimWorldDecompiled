using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A41 RID: 10817
	public class Building_PlantGrower : Building, IPlantToGrowSettable
	{
		// Token: 0x1700255F RID: 9567
		// (get) Token: 0x0600F022 RID: 61474 RVA: 0x0045A94F File Offset: 0x00458B4F
		public IEnumerable<Plant> PlantsOnMe
		{
			get
			{
				if (!base.Spawned)
				{
					yield break;
				}
				foreach (IntVec3 c in this.OccupiedRect())
				{
					List<Thing> thingList = base.Map.thingGrid.ThingsListAt(c);
					int num;
					for (int i = 0; i < thingList.Count; i = num + 1)
					{
						Plant plant = thingList[i] as Plant;
						if (plant != null)
						{
							yield return plant;
						}
						num = i;
					}
					thingList = null;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17002560 RID: 9568
		// (get) Token: 0x0600F023 RID: 61475 RVA: 0x0045A960 File Offset: 0x00458B60
		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return this.OccupiedRect().Cells;
			}
		}

		// Token: 0x0600F024 RID: 61476 RVA: 0x0045A97B File Offset: 0x00458B7B
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return PlantToGrowSettableUtility.SetPlantToGrowCommand(this);
			yield break;
			yield break;
		}

		// Token: 0x0600F025 RID: 61477 RVA: 0x0045A98B File Offset: 0x00458B8B
		public override void PostMake()
		{
			base.PostMake();
			this.plantDefToGrow = this.def.building.defaultPlantToGrow;
		}

		// Token: 0x0600F026 RID: 61478 RVA: 0x0045A9A9 File Offset: 0x00458BA9
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compPower = base.GetComp<CompPowerTrader>();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
		}

		// Token: 0x0600F027 RID: 61479 RVA: 0x0045A9CA File Offset: 0x00458BCA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
		}

		// Token: 0x0600F028 RID: 61480 RVA: 0x0045A9E4 File Offset: 0x00458BE4
		public override void TickRare()
		{
			base.TickRare();
			if (this.compPower != null && !this.compPower.PowerOn)
			{
				foreach (Thing thing in this.PlantsOnMe)
				{
					DamageInfo dinfo = new DamageInfo(DamageDefOf.Rotting, 1f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
					thing.TakeDamage(dinfo);
				}
			}
		}

		// Token: 0x0600F029 RID: 61481 RVA: 0x0045AA70 File Offset: 0x00458C70
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				foreach (Plant plant in this.PlantsOnMe.ToList<Plant>())
				{
					plant.Destroy(DestroyMode.Vanish);
				}
			}
			base.DeSpawn(mode);
		}

		// Token: 0x0600F02A RID: 61482 RVA: 0x0045AAD4 File Offset: 0x00458CD4
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (base.Spawned)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				if (PlantUtility.GrowthSeasonNow(base.Position, base.Map, this.plantDefToGrow))
				{
					text += "GrowSeasonHereNow".Translate();
				}
				else
				{
					text += "CannotGrowBadSeasonTemperature".Translate();
				}
			}
			return text;
		}

		// Token: 0x0600F02B RID: 61483 RVA: 0x0045AB4C File Offset: 0x00458D4C
		public ThingDef GetPlantDefToGrow()
		{
			return this.plantDefToGrow;
		}

		// Token: 0x0600F02C RID: 61484 RVA: 0x0045AB54 File Offset: 0x00458D54
		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		// Token: 0x0600F02D RID: 61485 RVA: 0x0045AB5D File Offset: 0x00458D5D
		public bool CanAcceptSowNow()
		{
			return this.compPower == null || this.compPower.PowerOn;
		}

		// Token: 0x04009E8F RID: 40591
		private ThingDef plantDefToGrow;

		// Token: 0x04009E90 RID: 40592
		private CompPowerTrader compPower;
	}
}
