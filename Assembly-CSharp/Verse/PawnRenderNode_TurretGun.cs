using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000621 RID: 1569
	public class PawnRenderNode_TurretGun : PawnRenderNode
	{
		// Token: 0x060027EF RID: 10223 RVA: 0x000D1894 File Offset: 0x000CFA94
		public PawnRenderNode_TurretGun(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x000D3EAA File Offset: 0x000D20AA
		public override Graphic GraphicFor(Pawn pawn)
		{
			return GraphicDatabase.Get<Graphic_Single>(this.turretComp.Props.turretDef.graphicData.texPath, ShaderDatabase.Cutout);
		}

		// Token: 0x04002106 RID: 8454
		public CompTurretGun turretComp;
	}
}
