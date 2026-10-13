using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000615 RID: 1557
	public class PawnRenderNode_Devourer : PawnRenderNode_AnimalPart_Body
	{
		// Token: 0x060027CE RID: 10190 RVA: 0x000D38AB File Offset: 0x000D1AAB
		public PawnRenderNode_Devourer(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x000D38B8 File Offset: 0x000D1AB8
		public override Graphic GraphicFor(Pawn pawn)
		{
			CompDevourer compDevourer;
			if (pawn.TryGetComp(out compDevourer) && compDevourer.Digesting)
			{
				Graphic graphic = pawn.ageTracker.CurKindLifeStage.bodyGraphicData.Graphic;
				return GraphicDatabase.Get<Graphic_Multi>(graphic.path + "_Closed", ShaderDatabase.Cutout, graphic.drawSize, Color.white);
			}
			return base.GraphicFor(pawn);
		}
	}
}
