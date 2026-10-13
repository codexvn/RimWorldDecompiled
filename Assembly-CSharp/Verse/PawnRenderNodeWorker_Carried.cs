using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000648 RID: 1608
	public class PawnRenderNodeWorker_Carried : PawnRenderNodeWorker
	{
		// Token: 0x06002886 RID: 10374 RVA: 0x000D6279 File Offset: 0x000D4479
		public override bool CanDrawNow(PawnRenderNode node, PawnDrawParms parms)
		{
			return base.CanDrawNow(node, parms) && !parms.Portrait && !parms.pawn.Dead && parms.pawn.Spawned;
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x000D62B0 File Offset: 0x000D44B0
		public override Vector3 OffsetFor(PawnRenderNode node, PawnDrawParms parms, out Vector3 pivot)
		{
			Vector3 result = base.OffsetFor(node, parms, out pivot);
			result.y = base.AltitudeFor(node, parms);
			return result;
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x000D62D7 File Offset: 0x000D44D7
		public override void AppendDrawRequests(PawnRenderNode node, PawnDrawParms parms, List<PawnGraphicDrawRequest> requests)
		{
			requests.Add(new PawnGraphicDrawRequest(node, null, null));
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x000D62E8 File Offset: 0x000D44E8
		public override void PostDraw(PawnRenderNode node, PawnDrawParms parms, Mesh mesh, Matrix4x4 matrix)
		{
			Vector3 vector2;
			Vector3 vector = parms.matrix.Position() + this.OffsetFor(node, parms, out vector2);
			Pawn_CarryTracker carryTracker = parms.pawn.carryTracker;
			if (((carryTracker != null) ? carryTracker.CarriedThing : null) != null)
			{
				PawnRenderUtility.DrawCarriedThing(parms.pawn, vector, parms.pawn.carryTracker.CarriedThing);
				return;
			}
			PawnRenderUtility.DrawEquipmentAndApparelExtras(parms.pawn, vector, parms.facing, parms.flags);
		}
	}
}
