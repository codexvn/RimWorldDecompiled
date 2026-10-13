using System;
using System.Collections.Generic;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005E3 RID: 1507
	public class AnimationWorker_Curves : BaseAnimationWorker
	{
		// Token: 0x0600269A RID: 9882 RVA: 0x000CC849 File Offset: 0x000CAA49
		public override bool Enabled(AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			return def.playWhenDowned || !parms.pawn.Downed;
		}

		// Token: 0x0600269B RID: 9883 RVA: 0x000026EA File Offset: 0x000008EA
		public override void PostDraw(AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms, Matrix4x4 matrix)
		{
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x000CC864 File Offset: 0x000CAA64
		public override Vector3 OffsetAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			CurveKeyAnimationPart curveKeyAnimationPart = (CurveKeyAnimationPart)part;
			Pawn pawn = parms.pawn;
			float time = (float)tick;
			if (curveKeyAnimationPart.normalized)
			{
				time = Mathf.InverseLerp(0f, (float)def.durationTicks, (float)tick);
			}
			Vector3Curve offset = curveKeyAnimationPart.offset;
			Vector3 offset2 = (offset != null) ? offset.Evaluate(time, 0f) : Vector3.zero;
			return AnimationUtility.AdjustOffsetForRotationMode(part.rotationMode, offset2, pawn);
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x000CC8CC File Offset: 0x000CAACC
		public override float AngleAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			CurveKeyAnimationPart curveKeyAnimationPart = (CurveKeyAnimationPart)part;
			float time = (float)tick;
			if (curveKeyAnimationPart.normalized)
			{
				time = Mathf.InverseLerp(0f, (float)def.durationTicks, (float)tick);
			}
			ComplexCurve angle = curveKeyAnimationPart.angle;
			if (angle == null)
			{
				return 0f;
			}
			return angle.Evaluate(time);
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x000CC914 File Offset: 0x000CAB14
		public override Vector3 ScaleAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			CurveKeyAnimationPart curveKeyAnimationPart = (CurveKeyAnimationPart)part;
			float time = (float)tick;
			if (curveKeyAnimationPart.normalized)
			{
				time = Mathf.InverseLerp(0f, (float)def.durationTicks, (float)tick);
			}
			Vector3Curve scale = curveKeyAnimationPart.scale;
			if (scale == null)
			{
				return Vector3.one;
			}
			return scale.Evaluate(time, 1f);
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x000CC961 File Offset: 0x000CAB61
		public override GraphicStateDef GraphicStateAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			return AnimationWorker_Curves.GetKeyframeForTick(tick, part).graphicState;
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x000CC970 File Offset: 0x000CAB70
		protected static CurveKey GetKeyframeForTick(int tick, AnimationPart p)
		{
			CurveKeyAnimationPart curveKeyAnimationPart = (CurveKeyAnimationPart)p;
			if (tick <= curveKeyAnimationPart.keyframes[0].tick)
			{
				return curveKeyAnimationPart.keyframes[0];
			}
			List<CurveKey> keyframes = curveKeyAnimationPart.keyframes;
			if (tick >= keyframes[keyframes.Count - 1].tick)
			{
				List<CurveKey> keyframes2 = curveKeyAnimationPart.keyframes;
				return keyframes2[keyframes2.Count - 1];
			}
			for (int i = 0; i < curveKeyAnimationPart.keyframes.Count; i++)
			{
				if (tick <= curveKeyAnimationPart.keyframes[i].tick)
				{
					return curveKeyAnimationPart.keyframes[i];
				}
			}
			Log.Error(string.Format("Attempted to get a curve keyframe for a tick ({0}) which wasn't handled", tick));
			return null;
		}
	}
}
