using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005E6 RID: 1510
	public class AnimationWorker_RevenantSpasm : BaseAnimationWorker
	{
		// Token: 0x060026B3 RID: 9907 RVA: 0x000CD053 File Offset: 0x000CB253
		private static float AnimationProgress(AnimationWorker_RevenantSpasm.RevenantSpasmData data)
		{
			return ((float)Find.TickManager.TicksGame - data.spasmStart) / data.spasmLength;
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x000CD06E File Offset: 0x000CB26E
		private static float Rotation(AnimationWorker_RevenantSpasm.RevenantSpasmData data)
		{
			return Mathf.Lerp(data.startHeadRot, data.targetHeadRot, AnimationWorker_RevenantSpasm.AnimationProgress(data));
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x000CD088 File Offset: 0x000CB288
		private static Vector3 Offset(AnimationWorker_RevenantSpasm.RevenantSpasmData data)
		{
			return new Vector3(Mathf.Lerp(data.startHeadOffset.x, data.targetHeadOffset.x, AnimationWorker_RevenantSpasm.AnimationProgress(data)), 0f, Mathf.Lerp(data.startHeadOffset.z, data.targetHeadOffset.z, AnimationWorker_RevenantSpasm.AnimationProgress(data)));
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x000CC849 File Offset: 0x000CAA49
		public override bool Enabled(AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			return def.playWhenDowned || !parms.pawn.Downed;
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x000026EA File Offset: 0x000008EA
		public override void PostDraw(AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms, Matrix4x4 matrix)
		{
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x000CD0E4 File Offset: 0x000CB2E4
		public override Vector3 OffsetAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			AnimationWorker_RevenantSpasm.RevenantSpasmData animationWorkerData = node.GetAnimationWorkerData<AnimationWorker_RevenantSpasm.RevenantSpasmData>();
			AnimationWorker_RevenantSpasm.CheckAndSpasm(animationWorkerData);
			if (parms.facing == Rot4.East || parms.facing == Rot4.West)
			{
				return AnimationWorker_RevenantSpasm.Offset(animationWorkerData) / 2f;
			}
			return AnimationWorker_RevenantSpasm.Offset(animationWorkerData);
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x000CD13C File Offset: 0x000CB33C
		public override float AngleAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			AnimationWorker_RevenantSpasm.RevenantSpasmData animationWorkerData = node.GetAnimationWorkerData<AnimationWorker_RevenantSpasm.RevenantSpasmData>();
			AnimationWorker_RevenantSpasm.CheckAndSpasm(animationWorkerData);
			if (parms.facing == Rot4.East || parms.facing == Rot4.West)
			{
				return AnimationWorker_RevenantSpasm.Rotation(animationWorkerData) / 2f;
			}
			return AnimationWorker_RevenantSpasm.Rotation(animationWorkerData);
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x000CD18F File Offset: 0x000CB38F
		public override Vector3 ScaleAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			return Vector3.one;
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x00002C42 File Offset: 0x00000E42
		public override GraphicStateDef GraphicStateAtTick(int tick, AnimationDef def, PawnRenderNode node, AnimationPart part, PawnDrawParms parms)
		{
			return null;
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x000CD198 File Offset: 0x000CB398
		private static void CheckAndSpasm(AnimationWorker_RevenantSpasm.RevenantSpasmData data)
		{
			if ((float)Find.TickManager.TicksGame > data.nextSpasm)
			{
				data.startHeadRot = AnimationWorker_RevenantSpasm.Rotation(data);
				data.startHeadOffset = AnimationWorker_RevenantSpasm.Offset(data);
				data.targetHeadRot = (float)Rand.Range(-20, 20);
				data.targetHeadOffset = new Vector3(Rand.Range(-0.1f, 0.1f), 0f, Rand.Range(-0.05f, 0.05f));
				data.spasmStart = (float)Find.TickManager.TicksGame;
				data.spasmLength = (float)(Rand.Bool ? AnimationWorker_RevenantSpasm.SpasmLength.RandomInRange : 6);
				data.nextSpasm = (float)(Find.TickManager.TicksGame + (Rand.Bool ? AnimationWorker_RevenantSpasm.SpasmIntervalShort.RandomInRange : AnimationWorker_RevenantSpasm.SpasmIntervalLong.RandomInRange));
			}
		}

		// Token: 0x0400200E RID: 8206
		private static readonly IntRange SpasmIntervalShort = new IntRange(6, 18);

		// Token: 0x0400200F RID: 8207
		private static readonly IntRange SpasmIntervalLong = new IntRange(120, 180);

		// Token: 0x04002010 RID: 8208
		private static readonly IntRange SpasmLength = new IntRange(30, 60);

		// Token: 0x04002011 RID: 8209
		private const int ShortSpasmLength = 6;

		// Token: 0x020005E7 RID: 1511
		private class RevenantSpasmData
		{
			// Token: 0x04002012 RID: 8210
			public float nextSpasm = -99999f;

			// Token: 0x04002013 RID: 8211
			public float spasmStart = -99999f;

			// Token: 0x04002014 RID: 8212
			public float spasmLength;

			// Token: 0x04002015 RID: 8213
			public float startHeadRot = 90f;

			// Token: 0x04002016 RID: 8214
			public Vector3 startHeadOffset = Vector3.zero;

			// Token: 0x04002017 RID: 8215
			public float targetHeadRot = 90f;

			// Token: 0x04002018 RID: 8216
			public Vector3 targetHeadOffset = Vector3.zero;
		}
	}
}
