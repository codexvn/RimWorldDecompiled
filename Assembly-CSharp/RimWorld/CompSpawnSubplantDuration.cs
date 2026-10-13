using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002E50 RID: 11856
	public class CompSpawnSubplantDuration : ThingComp
	{
		// Token: 0x17002AC6 RID: 10950
		// (get) Token: 0x06010B07 RID: 68359 RVA: 0x004CECD8 File Offset: 0x004CCED8
		public CompProperties_SpawnSubplant Props
		{
			get
			{
				return (CompProperties_SpawnSubplant)this.props;
			}
		}

		// Token: 0x06010B08 RID: 68360 RVA: 0x004CF97B File Offset: 0x004CDB7B
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.SetupNextSubplantTick();
			}
		}

		// Token: 0x06010B09 RID: 68361 RVA: 0x004CF986 File Offset: 0x004CDB86
		public override void CompTick()
		{
			if (Find.TickManager.TicksGame >= this.nextSubplantTick)
			{
				this.DoGrowSubplant(false);
				this.SetupNextSubplantTick();
			}
		}

		// Token: 0x06010B0A RID: 68362 RVA: 0x004CF9A7 File Offset: 0x004CDBA7
		public void SetupNextSubplantTick()
		{
			this.nextSubplantTick = Find.TickManager.TicksGame + (int)(60000f * this.Props.subplantSpawnDays);
		}

		// Token: 0x06010B0B RID: 68363 RVA: 0x004CF9CC File Offset: 0x004CDBCC
		public void DoGrowSubplant(bool force = false)
		{
			if (!ModLister.CheckIdeology("Subplant duration spawning"))
			{
				return;
			}
			if (!force && ((Plant)this.parent).Growth < this.Props.minGrowthForSpawn)
			{
				return;
			}
			IntVec3 position = this.parent.Position;
			int num = GenRadial.NumCellsInRadius(this.Props.maxRadius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.parent.Map) && WanderUtility.InSameRoom(position, intVec, this.parent.Map))
				{
					bool flag = false;
					List<Thing> thingList = intVec.GetThingList(this.parent.Map);
					foreach (Thing thing in thingList)
					{
						if (thing.def == this.Props.subplant)
						{
							flag = true;
							break;
						}
						if (!this.Props.plantsToNotOverwrite.NullOrEmpty<ThingDef>())
						{
							for (int j = 0; j < this.Props.plantsToNotOverwrite.Count; j++)
							{
								if (thing.def == this.Props.plantsToNotOverwrite[j])
								{
									flag = true;
									break;
								}
							}
						}
					}
					if (!flag)
					{
						if (!this.Props.canSpawnOverPlayerSownPlants)
						{
							Plant plant = intVec.GetPlant(this.parent.Map);
							Zone zone = this.parent.Map.zoneManager.ZoneAt(intVec);
							if (plant != null && plant.sown && zone != null && zone is Zone_Growing)
							{
								goto IL_251;
							}
						}
						if (this.Props.subplant.CanEverPlantAt(intVec, this.parent.Map, true, false))
						{
							for (int k = thingList.Count - 1; k >= 0; k--)
							{
								if (thingList[k].def.category == ThingCategory.Plant)
								{
									thingList[k].Destroy(DestroyMode.Vanish);
								}
							}
							Plant plant2 = (Plant)GenSpawn.Spawn(this.Props.subplant, intVec, this.parent.Map, WipeMode.Vanish);
							if (this.Props.initialGrowthRange != null)
							{
								plant2.Growth = this.Props.initialGrowthRange.Value.RandomInRange;
							}
							return;
						}
					}
				}
				IL_251:;
			}
		}

		// Token: 0x06010B0C RID: 68364 RVA: 0x004CFC48 File Offset: 0x004CDE48
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.nextSubplantTick, "nextSubplantTick", 0, false);
		}

		// Token: 0x0400ADFE RID: 44542
		private int nextSubplantTick;
	}
}
