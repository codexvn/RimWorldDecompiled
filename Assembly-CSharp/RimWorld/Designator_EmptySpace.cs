using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003098 RID: 12440
	public class Designator_EmptySpace : Designator
	{
		// Token: 0x0601175E RID: 71518 RVA: 0x00503FBF File Offset: 0x005021BF
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth, GizmoRenderParms parms)
		{
			return new GizmoResult(GizmoState.Clear);
		}

		// Token: 0x0601175F RID: 71519 RVA: 0x00066B11 File Offset: 0x00064D11
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06011760 RID: 71520 RVA: 0x00066B11 File Offset: 0x00064D11
		public override void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}
	}
}
