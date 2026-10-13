using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200061E RID: 1566
	public class PawnRenderNode_Spastic : PawnRenderNode
	{
		// Token: 0x060027E7 RID: 10215 RVA: 0x000D1894 File Offset: 0x000CFA94
		public PawnRenderNode_Spastic(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x060027E8 RID: 10216 RVA: 0x000D3C18 File Offset: 0x000D1E18
		public override GraphicMeshSet MeshSetFor(Pawn pawn)
		{
			return new GraphicMeshSet(MeshPool.GridPlane(this.props.overrideMeshSize ?? this.props.drawSize));
		}

		// Token: 0x060027E9 RID: 10217 RVA: 0x000D3C58 File Offset: 0x000D1E58
		public bool CheckAndDoSpasm(PawnDrawParms parms, out PawnRenderNode_Spastic.SpasmData dat, out float progress)
		{
			if (!parms.pawn.Dead)
			{
				PawnRenderNodeProperties_Spastic pawnRenderNodeProperties_Spastic = this.props as PawnRenderNodeProperties_Spastic;
				if (pawnRenderNodeProperties_Spastic != null && !parms.Portrait && !parms.Cache && !parms.Statue)
				{
					if (this.spasmData == null)
					{
						this.spasmData = new PawnRenderNode_Spastic.SpasmData();
					}
					if (Find.TickManager.TicksGame >= this.spasmData.nextSpasm)
					{
						this.spasmData.tickStart = Find.TickManager.TicksGame;
						this.spasmData.duration = (float)this.GetNextSpasmDurationTicks();
						this.spasmData.nextSpasm = this.GetNextSpasmTick();
						this.spasmData.rotationStart = this.spasmData.rotationTarget;
						this.spasmData.rotationTarget = pawnRenderNodeProperties_Spastic.rotationRange.RandomInRange;
						this.spasmData.scaleStart = this.spasmData.scaleTarget;
						this.spasmData.scaleTarget = pawnRenderNodeProperties_Spastic.scaleRange.RandomInRange;
						this.spasmData.offsetStart = this.spasmData.offsetTarget;
						this.spasmData.offsetTarget = new Vector3(pawnRenderNodeProperties_Spastic.offsetRangeX.RandomInRange, 0f, pawnRenderNodeProperties_Spastic.offsetRangeZ.RandomInRange);
					}
					progress = (float)(Find.TickManager.TicksGame - this.spasmData.tickStart) / Mathf.Max(this.spasmData.duration, 0.0001f);
					dat = this.spasmData;
					return true;
				}
			}
			progress = 0f;
			dat = null;
			return false;
		}

		// Token: 0x060027EA RID: 10218 RVA: 0x000D3DE0 File Offset: 0x000D1FE0
		protected virtual int GetNextSpasmTick()
		{
			PawnRenderNodeProperties_Spastic pawnRenderNodeProperties_Spastic = this.props as PawnRenderNodeProperties_Spastic;
			if (pawnRenderNodeProperties_Spastic != null)
			{
				return this.spasmData.tickStart + (int)this.spasmData.duration + pawnRenderNodeProperties_Spastic.nextSpasmTicksRange.RandomInRange;
			}
			return 0;
		}

		// Token: 0x060027EB RID: 10219 RVA: 0x000D3E24 File Offset: 0x000D2024
		protected virtual int GetNextSpasmDurationTicks()
		{
			PawnRenderNodeProperties_Spastic pawnRenderNodeProperties_Spastic = this.props as PawnRenderNodeProperties_Spastic;
			if (pawnRenderNodeProperties_Spastic != null)
			{
				return pawnRenderNodeProperties_Spastic.durationTicksRange.RandomInRange;
			}
			return 0;
		}

		// Token: 0x040020FB RID: 8443
		protected PawnRenderNode_Spastic.SpasmData spasmData;

		// Token: 0x0200061F RID: 1567
		public class SpasmData
		{
			// Token: 0x060027EC RID: 10220 RVA: 0x000D3E50 File Offset: 0x000D2050
			public SpasmData()
			{
				this.duration = 1f;
				this.scaleStart = (this.scaleTarget = 1f);
			}

			// Token: 0x040020FC RID: 8444
			public float rotationStart;

			// Token: 0x040020FD RID: 8445
			public float rotationTarget;

			// Token: 0x040020FE RID: 8446
			public float scaleStart;

			// Token: 0x040020FF RID: 8447
			public float scaleTarget;

			// Token: 0x04002100 RID: 8448
			public Vector3 offsetStart;

			// Token: 0x04002101 RID: 8449
			public Vector3 offsetTarget;

			// Token: 0x04002102 RID: 8450
			public int tickStart;

			// Token: 0x04002103 RID: 8451
			public int nextSpasm;

			// Token: 0x04002104 RID: 8452
			public float duration;
		}
	}
}
