using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020030D9 RID: 12505
	public class Designator_Replant : Designator_Install
	{
		// Token: 0x17002D90 RID: 11664
		// (get) Token: 0x0601190F RID: 71951 RVA: 0x0050BF18 File Offset: 0x0050A118
		public override string Label
		{
			get
			{
				return "CommandReplant".Translate();
			}
		}

		// Token: 0x17002D91 RID: 11665
		// (get) Token: 0x06011910 RID: 71952 RVA: 0x0050BF29 File Offset: 0x0050A129
		public override string Desc
		{
			get
			{
				return "CommandReplantDesc".Translate();
			}
		}

		// Token: 0x06011911 RID: 71953 RVA: 0x0050BF3A File Offset: 0x0050A13A
		public Designator_Replant()
		{
			this.icon = TexCommand.Replant;
			this.soundSucceeded = SoundDefOf.Designate_ExtractTree;
		}

		// Token: 0x06011912 RID: 71954 RVA: 0x0050BF58 File Offset: 0x0050A158
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			Plant plant = (Plant)base.ThingToInstall;
			Thing thing;
			AcceptanceReport report = plant.def.CanEverPlantAt(c, base.Map, out thing, true, true, false);
			if (!report)
			{
				return new AcceptanceReport("CannotBePlantedHere".Translate() + ": " + report.Reason.CapitalizeFirst());
			}
			if (plant.def.plant.interferesWithRoof && c.Roofed(base.Map))
			{
				return "CannotBePlantedHere".Translate() + ": " + "BlockedByRoof".Translate().CapitalizeFirst();
			}
			if (!plant.def.CanNowPlantAt(c, base.Map, true))
			{
				return new AcceptanceReport("CannotBePlantedHere".Translate());
			}
			foreach (Thing thing2 in c.GetThingList(base.Map))
			{
				Blueprint_Install blueprint_Install = thing2 as Blueprint_Install;
				if (blueprint_Install != null && blueprint_Install.ThingToInstall.def.plant != null && blueprint_Install.ThingToInstall.def.plant.IsTree)
				{
					return "IdenticalThingExists".Translate();
				}
			}
			return base.CanDesignateCell(c);
		}
	}
}
