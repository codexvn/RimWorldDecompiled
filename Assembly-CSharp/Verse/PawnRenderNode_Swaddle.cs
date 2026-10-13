using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000605 RID: 1541
	public class PawnRenderNode_Swaddle : PawnRenderNode
	{
		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x0600276B RID: 10091 RVA: 0x000D2166 File Offset: 0x000D0366
		public new PawnRenderNodeProperties_Swaddle Props
		{
			get
			{
				return (PawnRenderNodeProperties_Swaddle)this.props;
			}
		}

		// Token: 0x0600276C RID: 10092 RVA: 0x000D1894 File Offset: 0x000CFA94
		public PawnRenderNode_Swaddle(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree) : base(pawn, props, tree)
		{
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000D2174 File Offset: 0x000D0374
		public override Color ColorFor(Pawn pawn)
		{
			Rand.PushState(pawn.thingIDNumber);
			PawnRenderNodeProperties_Swaddle props = this.Props;
			float randomInRange = props.brightnessRange.RandomInRange;
			float num = Rand.Range(-props.swaddleColorOffset, props.swaddleColorOffset);
			float num2 = Rand.Range(-props.swaddleColorOffset, props.swaddleColorOffset);
			float num3 = Rand.Range(-props.swaddleColorOffset, props.swaddleColorOffset);
			Rand.PopState();
			return new Color(randomInRange + num, randomInRange + num2, randomInRange + num3);
		}
	}
}
