using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000652 RID: 1618
	public class PawnRenderSubWorker
	{
		// Token: 0x060028B5 RID: 10421 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanDrawNowSub(PawnRenderNode node, PawnDrawParms parms)
		{
			return true;
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void EditMaterialPropertyBlock(PawnRenderNode node, Material material, PawnDrawParms parms, ref MaterialPropertyBlock block)
		{
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void EditMaterial(PawnRenderNode node, PawnDrawParms parms, ref Material material)
		{
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void TransformOffset(PawnRenderNode node, PawnDrawParms parms, ref Vector3 offset, ref Vector3 pivot)
		{
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void TransformLayer(PawnRenderNode node, PawnDrawParms parms, ref float layer)
		{
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void TransformRotation(PawnRenderNode node, PawnDrawParms parms, ref Quaternion rotation)
		{
		}

		// Token: 0x060028BB RID: 10427 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void TransformScale(PawnRenderNode node, PawnDrawParms parms, ref Vector3 scale)
		{
		}
	}
}
