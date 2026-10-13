using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BB2 RID: 11186
	public abstract class CompAutoCut : ThingComp
	{
		// Token: 0x17002733 RID: 10035
		// (get) Token: 0x0600FA13 RID: 64019 RVA: 0x0048B5FF File Offset: 0x004897FF
		public ThingFilter AutoCutFilter
		{
			get
			{
				return this.autoCutFilter;
			}
		}

		// Token: 0x17002734 RID: 10036
		// (get) Token: 0x0600FA14 RID: 64020 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanDesignatePlants
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600FA15 RID: 64021 RVA: 0x0048B607 File Offset: 0x00489807
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (this.autoCutFilter == null)
			{
				this.autoCutFilter = new ThingFilter();
				this.autoCutFilter.CopyAllowancesFrom(this.GetDefaultAutoCutFilter());
			}
		}

		// Token: 0x0600FA16 RID: 64022
		public abstract ThingFilter GetDefaultAutoCutFilter();

		// Token: 0x0600FA17 RID: 64023
		public abstract ThingFilter GetFixedAutoCutFilter();

		// Token: 0x0600FA18 RID: 64024 RVA: 0x0048B634 File Offset: 0x00489834
		public override void CompTickLong()
		{
			base.CompTickLong();
			if (!this.autoCut)
			{
				return;
			}
			this.DesignatePlantsToCut();
		}

		// Token: 0x0600FA19 RID: 64025
		public abstract IEnumerable<IntVec3> GetAutoCutCells();

		// Token: 0x0600FA1A RID: 64026 RVA: 0x0048B64C File Offset: 0x0048984C
		public void DesignatePlantsToCut()
		{
			if (!this.CanDesignatePlants)
			{
				return;
			}
			Map map = this.parent.Map;
			foreach (IntVec3 c in this.GetAutoCutCells())
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Plant plant = thingList[i] as Plant;
					if (plant != null && this.CanDesignatePlant(plant))
					{
						map.designationManager.AddDesignation(new Designation(plant, DesignationDefOf.CutPlant, null));
					}
				}
			}
		}

		// Token: 0x0600FA1B RID: 64027 RVA: 0x0048B6F8 File Offset: 0x004898F8
		protected virtual bool CanDesignatePlant(Plant plant)
		{
			return plant.def.plant.allowAutoCut && !plant.DeliberatelyCultivated() && this.autoCutFilter.Allows(plant) && !plant.Map.designationManager.HasMapDesignationOn(plant);
		}

		// Token: 0x0600FA1C RID: 64028 RVA: 0x0048B74C File Offset: 0x0048994C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.autoCut, "autoCut", false, false);
			Scribe_Values.Look<int?>(ref this.lastCheckedAutoCutTick, "lastCheckedAutoCutTick", null, false);
			Scribe_Deep.Look<ThingFilter>(ref this.autoCutFilter, "autoCutFilter", Array.Empty<object>());
		}

		// Token: 0x0400A466 RID: 42086
		public bool autoCut;

		// Token: 0x0400A467 RID: 42087
		private int? lastCheckedAutoCutTick;

		// Token: 0x0400A468 RID: 42088
		private ThingFilter autoCutFilter;
	}
}
