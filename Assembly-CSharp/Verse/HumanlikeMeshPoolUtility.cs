using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200081B RID: 2075
	public static class HumanlikeMeshPoolUtility
	{
		// Token: 0x060034C4 RID: 13508 RVA: 0x0011517E File Offset: 0x0011337E
		public static float HumanlikeBodyWidthForPawn(Pawn pawn)
		{
			if (ModsConfig.BiotechActive && pawn.ageTracker.CurLifeStage.bodyWidth != null)
			{
				return pawn.ageTracker.CurLifeStage.bodyWidth.Value;
			}
			return 1.5f;
		}

		// Token: 0x060034C5 RID: 13509 RVA: 0x001151BC File Offset: 0x001133BC
		public static float HumanlikeHeadWidthForPawn(Pawn pawn)
		{
			float num = 1.5f;
			if (ModsConfig.BiotechActive && pawn.ageTracker.CurLifeStage.headSizeFactor != null)
			{
				num *= pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
			}
			return num;
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x00115208 File Offset: 0x00113408
		public static GraphicMeshSet GetHumanlikeBodySetForPawn(Pawn pawn, float wFactor = 1f, float hFactor = 1f)
		{
			float num = HumanlikeMeshPoolUtility.HumanlikeBodyWidthForPawn(pawn);
			return MeshPool.GetMeshSetForSize(num * wFactor, num * hFactor);
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x00115228 File Offset: 0x00113428
		public static GraphicMeshSet GetHumanlikeHeadSetForPawn(Pawn pawn, float wFactor = 1f, float hFactor = 1f)
		{
			float num = HumanlikeMeshPoolUtility.HumanlikeHeadWidthForPawn(pawn);
			return MeshPool.GetMeshSetForSize(num * wFactor, num * hFactor);
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x00115248 File Offset: 0x00113448
		public static GraphicMeshSet GetHumanlikeHairSetForPawn(Pawn pawn, float wFactor = 1f, float hFactor = 1f)
		{
			Vector2 vector = pawn.story.headType.hairMeshSize * new Vector2(wFactor, hFactor);
			if (ModsConfig.BiotechActive && pawn.ageTracker.CurLifeStage.headSizeFactor != null)
			{
				vector *= pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
			}
			return MeshPool.GetMeshSetForSize(vector.x, vector.y);
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x001152C0 File Offset: 0x001134C0
		public static GraphicMeshSet GetHumanlikeBeardSetForPawn(Pawn pawn, float wFactor = 1f, float hFactor = 1f)
		{
			Vector2 vector = pawn.story.headType.beardMeshSize * new Vector2(wFactor, hFactor);
			if (ModsConfig.BiotechActive && pawn.ageTracker.CurLifeStage.headSizeFactor != null)
			{
				vector *= pawn.ageTracker.CurLifeStage.headSizeFactor.Value;
			}
			return MeshPool.GetMeshSetForSize(vector.x, vector.y);
		}
	}
}
