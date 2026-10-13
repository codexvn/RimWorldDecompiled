using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E4E RID: 11854
	public class CompSpawnImmortalSubplantsAround : ThingComp
	{
		// Token: 0x17002AC2 RID: 10946
		// (get) Token: 0x06010AF4 RID: 68340 RVA: 0x004CECD8 File Offset: 0x004CCED8
		public CompProperties_SpawnSubplant Props
		{
			get
			{
				return (CompProperties_SpawnSubplant)this.props;
			}
		}

		// Token: 0x17002AC3 RID: 10947
		// (get) Token: 0x06010AF5 RID: 68341 RVA: 0x004CF35B File Offset: 0x004CD55B
		public int CheckRespawnIntervalTicks
		{
			get
			{
				return this.Props.checkRespawnIntervalHours * 2500;
			}
		}

		// Token: 0x06010AF6 RID: 68342 RVA: 0x004CF36E File Offset: 0x004CD56E
		public void Disable()
		{
			this.disabled = true;
		}

		// Token: 0x06010AF7 RID: 68343 RVA: 0x004CF378 File Offset: 0x004CD578
		private void WipeAllExistingPlants(IntVec3 cell)
		{
			List<Thing> thingList = cell.GetThingList(this.parent.Map);
			for (int i = thingList.Count - 1; i >= 0; i--)
			{
				if (thingList[i].def.category == ThingCategory.Plant && thingList[i].def != this.Props.dontWipePlant)
				{
					thingList[i].Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06010AF8 RID: 68344 RVA: 0x004CF3E4 File Offset: 0x004CD5E4
		private Plant SpawnPlant(IntVec3 cell)
		{
			Plant plant = (Plant)GenSpawn.Spawn(this.Props.subplant, cell, this.parent.Map, WipeMode.Vanish);
			plant.Growth = Rand.Range(this.Props.minGrowthForSpawn, 1f);
			plant.Age = (plant.def.plant.LimitedLifespan ? Rand.Range(0, Mathf.Max(plant.def.plant.LifespanTicks - 2500, 0)) : 0);
			return plant;
		}

		// Token: 0x06010AF9 RID: 68345 RVA: 0x004CF470 File Offset: 0x004CD670
		private void RespawnCheck()
		{
			foreach (IntVec3 intVec in this.cells.InRandomOrder(null))
			{
				Plant plant = intVec.GetPlant(this.parent.Map);
				Thing thing;
				if (((plant != null) ? plant.def : null) != this.Props.subplant && this.Props.subplant.CanEverPlantAt(intVec, this.parent.Map, out thing, false, true, true))
				{
					if (this.parent.Map.thingGrid.CellContains(intVec, ThingCategory.Item))
					{
						break;
					}
					this.SpawnPlant(intVec);
					break;
				}
			}
		}

		// Token: 0x06010AFA RID: 68346 RVA: 0x004CF538 File Offset: 0x004CD738
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (this.disabled)
			{
				return;
			}
			if (respawningAfterLoad)
			{
				if (this.cells == null)
				{
					this.cells = new List<IntVec3>();
					foreach (IntVec3 intVec in GenRadial.RadialCellsAround(this.parent.Position, this.Props.maxRadius, false))
					{
						if (intVec.InBounds(this.parent.Map))
						{
							Plant plant = intVec.GetPlant(this.parent.Map);
							if (((plant != null) ? plant.def : null) == this.Props.subplant)
							{
								this.cells.Add(intVec);
							}
						}
					}
				}
				return;
			}
			if (this.cells == null)
			{
				this.cells = new List<IntVec3>();
				foreach (IntVec3 intVec2 in GenRadial.RadialCellsAround(this.parent.Position, this.Props.maxRadius, false).InRandomOrder(null))
				{
					if (intVec2.InBounds(this.parent.Map) && Rand.Chance(this.Props.chanceOverDistance.Evaluate(intVec2.DistanceTo(this.parent.Position))))
					{
						if (this.Props.maxPlants != -1 && this.cells.Count >= this.Props.maxPlants)
						{
							break;
						}
						Plant plant2 = intVec2.GetPlant(this.parent.Map);
						if (plant2 == null || plant2.def != this.Props.dontWipePlant)
						{
							this.WipeAllExistingPlants(intVec2);
							Thing thing;
							if (this.Props.subplant.CanEverPlantAt(intVec2, this.parent.Map, out thing, true, true, true))
							{
								this.cells.Add(intVec2);
								this.SpawnPlant(intVec2);
							}
						}
					}
				}
			}
		}

		// Token: 0x06010AFB RID: 68347 RVA: 0x004CF748 File Offset: 0x004CD948
		public override void CompTick()
		{
			if (this.disabled || !this.parent.Spawned)
			{
				return;
			}
			if (this.cells == null)
			{
				return;
			}
			for (int i = 0; i < 100; i++)
			{
				this.curCheckIndex++;
				if (this.curCheckIndex >= this.cells.Count)
				{
					this.curCheckIndex = 0;
					break;
				}
				Plant plant = this.cells[this.curCheckIndex].GetPlant(this.parent.Map);
				if (plant != null && plant.Spawned && plant.def == this.Props.subplant)
				{
					plant.Age = 0;
				}
			}
			if (this.parent.IsHashIntervalTick(this.CheckRespawnIntervalTicks))
			{
				this.RespawnCheck();
			}
		}

		// Token: 0x06010AFC RID: 68348 RVA: 0x004CF80B File Offset: 0x004CDA0B
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Respawn " + this.Props.subplant.label,
					action = new Action(this.RespawnCheck)
				};
			}
			yield break;
		}

		// Token: 0x06010AFD RID: 68349 RVA: 0x004CF81C File Offset: 0x004CDA1C
		public override void PostExposeData()
		{
			string str = this.Props.saveKeysPrefix.NullOrEmpty() ? null : (this.Props.saveKeysPrefix + "_");
			Scribe_Values.Look<bool>(ref this.disabled, str + "disabled", false, false);
			Scribe_Collections.Look<IntVec3>(ref this.cells, str + "cells", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x0400ADF7 RID: 44535
		private bool disabled;

		// Token: 0x0400ADF8 RID: 44536
		private List<IntVec3> cells;

		// Token: 0x0400ADF9 RID: 44537
		private int curCheckIndex;
	}
}
