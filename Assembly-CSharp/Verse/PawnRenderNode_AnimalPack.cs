using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005FF RID: 1535
	public class PawnRenderNode_AnimalPack : PawnRenderNode_AnimalPart
	{
		// Token: 0x06002752 RID: 10066 RVA: 0x000D180E File Offset: 0x000CFA0E
		public PawnRenderNode_AnimalPack(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x06002753 RID: 10067 RVA: 0x000D181C File Offset: 0x000CFA1C
		public override Graphic GraphicFor(Pawn pawn)
		{
			if (!pawn.RaceProps.packAnimal)
			{
				return null;
			}
			PawnKindLifeStage curKindLifeStage = pawn.ageTracker.CurKindLifeStage;
			Graphic graphic;
			if (pawn.gender != Gender.Female || curKindLifeStage.femaleGraphicData == null)
			{
				graphic = curKindLifeStage.bodyGraphicData.Graphic;
			}
			else
			{
				graphic = curKindLifeStage.femaleGraphicData.Graphic;
			}
			return GraphicDatabase.Get<Graphic_Multi>(graphic.path + "Pack", ShaderDatabase.Cutout, graphic.drawSize, Color.white);
		}
	}
}
