using System;

namespace Verse
{
	// Token: 0x020002C0 RID: 704
	public class PawnKindLifeStage
	{
		// Token: 0x06001134 RID: 4404 RVA: 0x00050DC5 File Offset: 0x0004EFC5
		public void PostLoad()
		{
			this.untranslatedLabel = this.label;
			this.untranslatedLabelMale = this.labelMale;
			this.untranslatedLabelFemale = this.labelFemale;
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x00050DEC File Offset: 0x0004EFEC
		public void ResolveReferences()
		{
			if (this.bodyGraphicData != null && this.bodyGraphicData.graphicClass == null)
			{
				this.bodyGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.femaleGraphicData != null && this.femaleGraphicData.graphicClass == null)
			{
				this.femaleGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.dessicatedBodyGraphicData != null && this.dessicatedBodyGraphicData.graphicClass == null)
			{
				this.dessicatedBodyGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.femaleDessicatedBodyGraphicData != null && this.femaleDessicatedBodyGraphicData.graphicClass == null)
			{
				this.femaleDessicatedBodyGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.corpseGraphicData != null && this.corpseGraphicData.graphicClass == null)
			{
				this.corpseGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.femaleCorpseGraphicData != null && this.femaleCorpseGraphicData.graphicClass == null)
			{
				this.femaleCorpseGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.silhouetteGraphicData != null && this.silhouetteGraphicData.graphicClass == null)
			{
				this.silhouetteGraphicData.graphicClass = typeof(Graphic_Single);
			}
			if (this.stationaryGraphicData != null && this.stationaryGraphicData.graphicClass == null)
			{
				this.stationaryGraphicData.graphicClass = typeof(Graphic_Multi);
			}
			if (this.femaleStationaryGraphicData != null && this.femaleStationaryGraphicData.graphicClass == null)
			{
				this.femaleStationaryGraphicData.graphicClass = typeof(Graphic_Multi);
			}
		}

		// Token: 0x04000F38 RID: 3896
		[MustTranslate]
		public string label;

		// Token: 0x04000F39 RID: 3897
		[MustTranslate]
		public string labelPlural;

		// Token: 0x04000F3A RID: 3898
		[MustTranslate]
		public string labelMale;

		// Token: 0x04000F3B RID: 3899
		[MustTranslate]
		public string labelMalePlural;

		// Token: 0x04000F3C RID: 3900
		[MustTranslate]
		public string labelFemale;

		// Token: 0x04000F3D RID: 3901
		[MustTranslate]
		public string labelFemalePlural;

		// Token: 0x04000F3E RID: 3902
		[Unsaved(false)]
		[TranslationHandle(Priority = 200)]
		public string untranslatedLabel;

		// Token: 0x04000F3F RID: 3903
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedLabelMale;

		// Token: 0x04000F40 RID: 3904
		[Unsaved(false)]
		[TranslationHandle]
		public string untranslatedLabelFemale;

		// Token: 0x04000F41 RID: 3905
		public GraphicData bodyGraphicData;

		// Token: 0x04000F42 RID: 3906
		public GraphicData femaleGraphicData;

		// Token: 0x04000F43 RID: 3907
		public GraphicData dessicatedBodyGraphicData;

		// Token: 0x04000F44 RID: 3908
		public GraphicData femaleDessicatedBodyGraphicData;

		// Token: 0x04000F45 RID: 3909
		public GraphicData corpseGraphicData;

		// Token: 0x04000F46 RID: 3910
		public GraphicData swimmingGraphicData;

		// Token: 0x04000F47 RID: 3911
		public GraphicData femaleSwimmingGraphicData;

		// Token: 0x04000F48 RID: 3912
		public GraphicData femaleCorpseGraphicData;

		// Token: 0x04000F49 RID: 3913
		public GraphicData silhouetteGraphicData;

		// Token: 0x04000F4A RID: 3914
		public GraphicData rottingGraphicData;

		// Token: 0x04000F4B RID: 3915
		public GraphicData femaleRottingGraphicData;

		// Token: 0x04000F4C RID: 3916
		public GraphicData stationaryGraphicData;

		// Token: 0x04000F4D RID: 3917
		public GraphicData femaleStationaryGraphicData;

		// Token: 0x04000F4E RID: 3918
		public AnimationDef flyingAnimationEast;

		// Token: 0x04000F4F RID: 3919
		public AnimationDef flyingAnimationNorth;

		// Token: 0x04000F50 RID: 3920
		public AnimationDef flyingAnimationSouth;

		// Token: 0x04000F51 RID: 3921
		public AnimationDef flyingAnimationEastFemale;

		// Token: 0x04000F52 RID: 3922
		public AnimationDef flyingAnimationNorthFemale;

		// Token: 0x04000F53 RID: 3923
		public AnimationDef flyingAnimationSouthFemale;

		// Token: 0x04000F54 RID: 3924
		public BodyPartToDrop butcherBodyPart;
	}
}
