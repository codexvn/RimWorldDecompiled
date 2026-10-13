using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000613 RID: 1555
	public class PawnRenderNode_Apparel : PawnRenderNode
	{
		// Token: 0x060027C0 RID: 10176 RVA: 0x000D3667 File Offset: 0x000D1867
		public PawnRenderNode_Apparel(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
			this.useHeadMesh = (props.parentTagDef == PawnRenderNodeTagDefOf.ApparelHead);
			this.meshSet = this.MeshSetFor(pawn);
		}

		// Token: 0x060027C1 RID: 10177 RVA: 0x000D3692 File Offset: 0x000D1892
		public PawnRenderNode_Apparel(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree, Apparel apparel) : base(pawn, props, tree)
		{
			this.apparel = apparel;
			this.useHeadMesh = (props.parentTagDef == PawnRenderNodeTagDefOf.ApparelHead);
			this.meshSet = this.MeshSetFor(pawn);
		}

		// Token: 0x060027C2 RID: 10178 RVA: 0x000D36C5 File Offset: 0x000D18C5
		public PawnRenderNode_Apparel(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree, Apparel apparel, bool useHeadMesh) : base(pawn, props, tree)
		{
			this.apparel = apparel;
			this.useHeadMesh = useHeadMesh;
			this.meshSet = this.MeshSetFor(pawn);
		}

		// Token: 0x060027C3 RID: 10179 RVA: 0x000D36F0 File Offset: 0x000D18F0
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			if (this.apparel == null)
			{
				return base.MeshSetFor(pawn);
			}
			if (base.Props.overrideMeshSize != null)
			{
				return MeshPool.GetMeshSetForSize(base.Props.overrideMeshSize.Value.x, base.Props.overrideMeshSize.Value.y);
			}
			if (this.useHeadMesh)
			{
				return HumanlikeMeshPoolUtility.GetHumanlikeHeadSetForPawn(pawn, 1f, 1f);
			}
			return HumanlikeMeshPoolUtility.GetHumanlikeBodySetForPawn(pawn, 1f, 1f);
		}

		// Token: 0x060027C4 RID: 10180 RVA: 0x000D3778 File Offset: 0x000D1978
		protected override IEnumerable<Graphic> GraphicsFor(Pawn pawn)
		{
			ApparelGraphicRecord apparelGraphicRecord;
			if (ApparelGraphicRecordGetter.TryGetGraphicApparel(this.apparel, this.tree.pawn.story.bodyType, pawn.Drawer.renderer.StatueColor != null, out apparelGraphicRecord))
			{
				yield return apparelGraphicRecord.graphic;
			}
			yield break;
		}

		// Token: 0x060027C5 RID: 10181 RVA: 0x000D378F File Offset: 0x000D198F
		protected override int TexSeedFor(Pawn pawn)
		{
			return base.TexSeedFor(pawn) + this.apparel.thingIDNumber;
		}

		// Token: 0x040020F4 RID: 8436
		public bool useHeadMesh;
	}
}
