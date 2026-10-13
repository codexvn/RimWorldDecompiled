using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000651 RID: 1617
	public class PawnRenderNodeWorker_TurretGun : PawnRenderNodeWorker
	{
		// Token: 0x060028B3 RID: 10419 RVA: 0x000D718C File Offset: 0x000D538C
		public override Quaternion RotationFor(PawnRenderNode node, PawnDrawParms parms)
		{
			Quaternion quaternion = base.RotationFor(node, parms);
			PawnRenderNode_TurretGun pawnRenderNode_TurretGun = node as PawnRenderNode_TurretGun;
			if (pawnRenderNode_TurretGun != null)
			{
				quaternion *= pawnRenderNode_TurretGun.turretComp.curRotation.ToQuat();
			}
			return quaternion;
		}
	}
}
