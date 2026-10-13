using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200310C RID: 12556
	public class Designator_PlantsHarvestWood : Designator_Plants
	{
		// Token: 0x17002DDD RID: 11741
		// (get) Token: 0x06011A43 RID: 72259 RVA: 0x0050FE6C File Offset: 0x0050E06C
		private bool CanDesignateStumpsNow
		{
			get
			{
				if (Find.DesignatorManager.SelectedDesignator != this)
				{
					return true;
				}
				foreach (IntVec3 c in Find.DesignatorManager.Dragger.DragCells)
				{
					Plant plant = c.GetPlant(base.Map);
					if (plant != null && plant.def.plant.IsTree && !plant.def.plant.isStump && plant.HarvestableNow && base.Map.designationManager.DesignationOn(plant, this.designationDef) == null)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06011A44 RID: 72260 RVA: 0x0050FF2C File Offset: 0x0050E12C
		public Designator_PlantsHarvestWood()
		{
			this.defaultLabel = "DesignatorHarvestWood".Translate();
			this.defaultDesc = "DesignatorHarvestWoodDesc".Translate();
			this.icon = ContentFinder<Texture2D>.Get("UI/Designators/HarvestWood", true);
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_HarvestWood;
			this.hotKey = KeyBindingDefOf.Misc1;
			this.designationDef = DesignationDefOf.HarvestPlant;
			this.tutorTag = "PlantsHarvestWood";
		}

		// Token: 0x06011A45 RID: 72261 RVA: 0x0050FFC4 File Offset: 0x0050E1C4
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result = base.CanDesignateThing(t);
			if (!result.Accepted)
			{
				return result;
			}
			Plant plant = (Plant)t;
			if (!plant.HarvestableNow || !plant.def.plant.IsTree)
			{
				return "MessageMustDesignateHarvestableWood".Translate();
			}
			CompPlantPreventCutting compPlantPreventCutting;
			if (t.TryGetComp(out compPlantPreventCutting) && compPlantPreventCutting.PreventCutting)
			{
				return "MessageMustPlantCuttingForbidden".Translate();
			}
			if (t.def.plant.isStump && !this.CanDesignateStumpsNow)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06011A46 RID: 72262 RVA: 0x0051005E File Offset: 0x0050E25E
		protected override bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return target.Thing.def.plant.IsTree;
		}

		// Token: 0x06011A47 RID: 72263 RVA: 0x0050FD0A File Offset: 0x0050DF0A
		public override void DesignateThing(Thing t)
		{
			Designator_PlantsHarvestWood.PossiblyWarnPlayerImportantPlantDesignateCut(t);
			if (ModsConfig.IdeologyActive && t.def.plant.IsTree && t.def.plant.treeLoversCareIfChopped)
			{
				Designator_PlantsHarvestWood.PossiblyWarnPlayerOnDesignatingTreeCut();
			}
			base.DesignateThing(t);
		}

		// Token: 0x06011A48 RID: 72264 RVA: 0x00510078 File Offset: 0x0050E278
		public static void PossiblyWarnPlayerImportantPlantDesignateCut(Thing t)
		{
			Plant plant = t as Plant;
			if (plant != null && plant.def.plant.warnIfMarkedForCut && (!plant.HarvestableNow || plant.def.plant.IsTree))
			{
				Messages.Message("MessageWarningCutImportantPlant".Translate(plant.LabelCap), plant, MessageTypeDefOf.CautionInput, false);
			}
		}

		// Token: 0x06011A49 RID: 72265 RVA: 0x005100E8 File Offset: 0x0050E2E8
		public static void PossiblyWarnPlayerOnDesignatingTreeCut()
		{
			Designator_PlantsHarvestWood.tmpIdeoMemberNames.Clear();
			foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
			{
				if (ideo.WarnPlayerOnDesignateChopTree)
				{
					Designator_PlantsHarvestWood.tmpIdeoMemberNames.Add(Find.ActiveLanguageWorker.Pluralize(ideo.memberName, -1));
				}
			}
			if (Designator_PlantsHarvestWood.tmpIdeoMemberNames.Any<string>())
			{
				Messages.Message("MessageWarningPlayerDesignatedTreeChopped".Translate(Designator_PlantsHarvestWood.tmpIdeoMemberNames.ToCommaList(true, false)), MessageTypeDefOf.CautionInput, false);
			}
			Designator_PlantsHarvestWood.tmpIdeoMemberNames.Clear();
		}

		// Token: 0x0400B5A7 RID: 46503
		private static readonly List<string> tmpIdeoMemberNames = new List<string>();
	}
}
