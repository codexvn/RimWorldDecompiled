using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020022E5 RID: 8933
	public static class PawnDrawUtility
	{
		// Token: 0x0600C22C RID: 49708 RVA: 0x00384B50 File Offset: 0x00382D50
		public static bool AnchorUsable(Pawn pawn, BodyTypeDef.WoundAnchor anchor, Rot4 pawnRot)
		{
			return (anchor.rotation == null || anchor.rotation.Value == pawnRot || (anchor.canMirror && anchor.rotation.Value == pawnRot.Opposite)) && (anchor.narrowCrown == null || (pawn.story != null && pawn.story.headType.narrow == anchor.narrowCrown.Value)) && (pawn.health.hediffSet.HasHead || anchor.layer != PawnOverlayDrawer.OverlayLayer.Head);
		}

		// Token: 0x0600C22D RID: 49709 RVA: 0x00384BF2 File Offset: 0x00382DF2
		public static IEnumerable<BodyTypeDef.WoundAnchor> FindAnchors(Pawn pawn, BodyPartRecord curPart)
		{
			if (pawn.story == null || pawn.story.bodyType == null || pawn.story.bodyType.woundAnchors.NullOrEmpty<BodyTypeDef.WoundAnchor>())
			{
				yield break;
			}
			int iterations = 0;
			int found = 0;
			while (found == 0 && curPart != null && iterations < 100)
			{
				int num;
				if (curPart.woundAnchorTag != null)
				{
					foreach (BodyTypeDef.WoundAnchor woundAnchor in pawn.story.bodyType.woundAnchors)
					{
						if (woundAnchor.tag == curPart.woundAnchorTag)
						{
							num = found;
							found = num + 1;
							yield return woundAnchor;
						}
					}
					List<BodyTypeDef.WoundAnchor>.Enumerator enumerator = default(List<BodyTypeDef.WoundAnchor>.Enumerator);
				}
				else
				{
					foreach (BodyTypeDef.WoundAnchor woundAnchor2 in pawn.story.bodyType.woundAnchors)
					{
						if (curPart.IsInGroup(woundAnchor2.group))
						{
							num = found;
							found = num + 1;
							yield return woundAnchor2;
						}
					}
					List<BodyTypeDef.WoundAnchor>.Enumerator enumerator = default(List<BodyTypeDef.WoundAnchor>.Enumerator);
				}
				curPart = curPart.parent;
				num = iterations;
				iterations = num + 1;
			}
			if (iterations == 100)
			{
				Log.Error("PawnWoundDrawer.RenderOverBody.FindAnchors while() loop ran into iteration limit! This is never supposed to happen! Is there a cyclic body part parent reference?");
			}
			yield break;
			yield break;
		}

		// Token: 0x0600C22E RID: 49710 RVA: 0x00384C0C File Offset: 0x00382E0C
		public static void CalcAnchorData(Pawn pawn, BodyTypeDef.WoundAnchor anchor, Rot4 pawnRot, out Vector3 anchorOffset, out float range)
		{
			anchorOffset = anchor.offset;
			Rot4? rotation = anchor.rotation;
			Rot4 opposite = pawnRot.Opposite;
			if (rotation != null && (rotation == null || rotation.GetValueOrDefault() == opposite))
			{
				anchorOffset.x *= -1f;
			}
			if ((anchor.tag == "LeftEye" || anchor.tag == "RightEye") && pawnRot.IsHorizontal && (!ModsConfig.BiotechActive || !pawn.DevelopmentalStage.Juvenile()))
			{
				Vector3? eyeOffsetEastWest = pawn.story.headType.eyeOffsetEastWest;
				if (eyeOffsetEastWest != null)
				{
					if (pawnRot == Rot4.East)
					{
						anchorOffset = eyeOffsetEastWest.Value;
					}
					else
					{
						anchorOffset = eyeOffsetEastWest.Value.ScaledBy(new Vector3(-1f, 1f, 1f));
					}
				}
			}
			range = anchor.range;
		}

		// Token: 0x04008493 RID: 33939
		public const string AnchorTagEyeLeft = "LeftEye";

		// Token: 0x04008494 RID: 33940
		public const string AnchorTagEyeRight = "RightEye";
	}
}
