using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200310A RID: 12554
	[StaticConstructorOnStartup]
	public class Designator_PlantsCut : Designator_Plants
	{
		// Token: 0x06011A3A RID: 72250 RVA: 0x0050FBE4 File Offset: 0x0050DDE4
		public Designator_PlantsCut()
		{
			this.defaultLabel = "DesignatorCutPlants".Translate();
			this.defaultDesc = "DesignatorCutPlantsDesc".Translate();
			this.icon = Designator_PlantsCut.IconTex;
			this.soundDragSustain = SoundDefOf.Designate_DragStandard;
			this.soundDragChanged = SoundDefOf.Designate_DragStandard_Changed;
			this.useMouseIcon = true;
			this.soundSucceeded = SoundDefOf.Designate_CutPlants;
			this.hotKey = KeyBindingDefOf.Misc3;
			this.designationDef = DesignationDefOf.CutPlant;
		}

		// Token: 0x06011A3B RID: 72251 RVA: 0x0050FC6C File Offset: 0x0050DE6C
		public override AcceptanceReport CanDesignateThing(Thing t)
		{
			AcceptanceReport result = base.CanDesignateThing(t);
			if (!result.Accepted)
			{
				return result;
			}
			CompPlantPreventCutting compPlantPreventCutting;
			if (t.TryGetComp(out compPlantPreventCutting) && compPlantPreventCutting.PreventCutting)
			{
				return "MessageMustPlantCuttingForbidden".Translate();
			}
			return this.AffectsThing(t);
		}

		// Token: 0x06011A3C RID: 72252 RVA: 0x0050FCBA File Offset: 0x0050DEBA
		protected override bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return this.AffectsThing(target.Thing);
		}

		// Token: 0x06011A3D RID: 72253 RVA: 0x0050FCCC File Offset: 0x0050DECC
		private bool AffectsThing(Thing t)
		{
			Plant plant = t as Plant;
			return plant != null && (this.isOrder || !plant.def.plant.IsTree || !plant.HarvestableNow);
		}

		// Token: 0x06011A3E RID: 72254 RVA: 0x0050FD0A File Offset: 0x0050DF0A
		public override void DesignateThing(Thing t)
		{
			Designator_PlantsHarvestWood.PossiblyWarnPlayerImportantPlantDesignateCut(t);
			if (ModsConfig.IdeologyActive && t.def.plant.IsTree && t.def.plant.treeLoversCareIfChopped)
			{
				Designator_PlantsHarvestWood.PossiblyWarnPlayerOnDesignatingTreeCut();
			}
			base.DesignateThing(t);
		}

		// Token: 0x0400B5A6 RID: 46502
		public static readonly Texture2D IconTex = ContentFinder<Texture2D>.Get("UI/Designators/CutPlants", true);
	}
}
