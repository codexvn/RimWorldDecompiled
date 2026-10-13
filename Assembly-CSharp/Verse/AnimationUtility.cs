using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005E2 RID: 1506
	public static class AnimationUtility
	{
		// Token: 0x06002699 RID: 9881 RVA: 0x000CC7C0 File Offset: 0x000CA9C0
		public static Vector3 AdjustOffsetForRotationMode(RotationMode mode, Vector3 offset, Pawn pawn)
		{
			if (mode == RotationMode.None)
			{
				return offset;
			}
			if (mode == RotationMode.OneD)
			{
				if (!(pawn.Rotation == Rot4.South))
				{
					return offset;
				}
				return offset * -1f;
			}
			else
			{
				if (mode == RotationMode.TwoD)
				{
					return offset.RotatedBy(pawn.Rotation);
				}
				if (mode == RotationMode.PawnAimTarget && pawn.TargetCurrentlyAimingAt.IsValid)
				{
					return offset.RotatedBy((pawn.TargetCurrentlyAimingAt.CenterVector3 - pawn.Position.ToVector3()).ToAngleFlat());
				}
				return offset;
			}
		}
	}
}
