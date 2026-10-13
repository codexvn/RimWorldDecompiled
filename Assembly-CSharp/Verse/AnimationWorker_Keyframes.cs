using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005E4 RID: 1508
	public class AnimationWorker_Keyframes : BaseAnimationWorker
	{
		// Token: 0x060026A2 RID: 9890 RVA: 0x000CC849 File Offset: 0x000CAA49
		public override bool Enabled(AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			return def.playWhenDowned || !parms.pawn.Downed;
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x000026EA File Offset: 0x000008EA
		public override void PostDraw(AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms, Matrix4x4 matrix)
		{
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x000CCA2C File Offset: 0x000CAC2C
		public override Vector3 OffsetAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			Pawn pawn = parms.pawn;
			Vector3 keyframeData = AnimationWorker_Keyframes.GetKeyframeData<Vector3>(tick, part, this.cachedGetKeyframeOffset, AnimationWorker_Keyframes.cachedLerpVector3);
			return AnimationUtility.AdjustOffsetForRotationMode(part.rotationMode, keyframeData, pawn);
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x000CCA63 File Offset: 0x000CAC63
		public override float AngleAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			return AnimationWorker_Keyframes.GetKeyframeData<float>(tick, part, this.cachedGetKeyframeAngle, AnimationWorker_Keyframes.cachedLerpMathf);
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x000CCA78 File Offset: 0x000CAC78
		public override Vector3 ScaleAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			return AnimationWorker_Keyframes.GetKeyframeData<Vector3>(tick, part, this.cachedGetKeyframeScale, AnimationWorker_Keyframes.cachedLerpVector3);
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x000CCA8D File Offset: 0x000CAC8D
		public override GraphicStateDef GraphicStateAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			return AnimationWorker_Keyframes.GetKeyframeForTick(tick, part).graphicState;
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x000CCA9C File Offset: 0x000CAC9C
		protected static T GetKeyframeData<T>(int tick, AnimationPart p, Func<Keyframe, T> func, Func<T, T, float, T> lerpFunc) where T : struct
		{
			KeyframeAnimationPart keyframeAnimationPart = (KeyframeAnimationPart)p;
			if (tick <= keyframeAnimationPart.keyframes[0].tick)
			{
				return func(keyframeAnimationPart.keyframes[0]);
			}
			List<Keyframe> keyframes = keyframeAnimationPart.keyframes;
			if (tick >= keyframes[keyframes.Count - 1].tick)
			{
				List<Keyframe> keyframes2 = keyframeAnimationPart.keyframes;
				return func(keyframes2[keyframes2.Count - 1]);
			}
			Keyframe keyframe = keyframeAnimationPart.keyframes[0];
			List<Keyframe> keyframes3 = keyframeAnimationPart.keyframes;
			Keyframe keyframe2 = keyframes3[keyframes3.Count - 1];
			int i = 0;
			while (i < keyframeAnimationPart.keyframes.Count)
			{
				if (tick <= keyframeAnimationPart.keyframes[i].tick)
				{
					keyframe2 = keyframeAnimationPart.keyframes[i];
					if (i > 0)
					{
						keyframe = keyframeAnimationPart.keyframes[i - 1];
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			float arg = (float)(tick - keyframe.tick) / (float)(keyframe2.tick - keyframe.tick);
			return lerpFunc(func(keyframe), func(keyframe2), arg);
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x000CCBB0 File Offset: 0x000CADB0
		protected static Keyframe GetKeyframeForTick(int tick, AnimationPart p)
		{
			KeyframeAnimationPart keyframeAnimationPart = (KeyframeAnimationPart)p;
			if (tick <= keyframeAnimationPart.keyframes[0].tick)
			{
				return keyframeAnimationPart.keyframes[0];
			}
			List<Keyframe> keyframes = keyframeAnimationPart.keyframes;
			if (tick >= keyframes[keyframes.Count - 1].tick)
			{
				List<Keyframe> keyframes2 = keyframeAnimationPart.keyframes;
				return keyframes2[keyframes2.Count - 1];
			}
			for (int i = 0; i < keyframeAnimationPart.keyframes.Count; i++)
			{
				if (tick <= keyframeAnimationPart.keyframes[i].tick)
				{
					return keyframeAnimationPart.keyframes[i];
				}
			}
			Log.Error(string.Format("Attempted to get a keyframe for a tick ({0}) which wasn't handled", tick));
			return null;
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x000CCC61 File Offset: 0x000CAE61
		private static Vector3 GetKeyframeScale(Keyframe keyframe)
		{
			return keyframe.scale;
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x000CCC69 File Offset: 0x000CAE69
		private static Vector3 GetKeyframeOffset(Keyframe keyframe)
		{
			return keyframe.offset;
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x000CCC71 File Offset: 0x000CAE71
		private static float GetKeyframeAngle(Keyframe keyframe)
		{
			return keyframe.angle;
		}

		// Token: 0x04002001 RID: 8193
		private static readonly Func<Vector3, Vector3, float, Vector3> cachedLerpVector3 = new Func<Vector3, Vector3, float, Vector3>(Vector3.Lerp);

		// Token: 0x04002002 RID: 8194
		private static readonly Func<float, float, float, float> cachedLerpMathf = new Func<float, float, float, float>(Mathf.Lerp);

		// Token: 0x04002003 RID: 8195
		private readonly Func<Keyframe, Vector3> cachedGetKeyframeOffset = new Func<Keyframe, Vector3>(AnimationWorker_Keyframes.GetKeyframeOffset);

		// Token: 0x04002004 RID: 8196
		private readonly Func<Keyframe, float> cachedGetKeyframeAngle = new Func<Keyframe, float>(AnimationWorker_Keyframes.GetKeyframeAngle);

		// Token: 0x04002005 RID: 8197
		private readonly Func<Keyframe, Vector3> cachedGetKeyframeScale = new Func<Keyframe, Vector3>(AnimationWorker_Keyframes.GetKeyframeScale);
	}
}
