using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02003D72 RID: 15730
	public class Caravan_Tweener
	{
		// Token: 0x1700355E RID: 13662
		// (get) Token: 0x060156E6 RID: 87782 RVA: 0x0066A0AC File Offset: 0x006682AC
		public Vector3 TweenedPos
		{
			get
			{
				return this.tweenedPos;
			}
		}

		// Token: 0x1700355F RID: 13663
		// (get) Token: 0x060156E7 RID: 87783 RVA: 0x0066A0B4 File Offset: 0x006682B4
		public Vector3 LastTickTweenedVelocity
		{
			get
			{
				return this.TweenedPos - this.lastTickSpringPos;
			}
		}

		// Token: 0x17003560 RID: 13664
		// (get) Token: 0x060156E8 RID: 87784 RVA: 0x0066A0C7 File Offset: 0x006682C7
		public Vector3 TweenedPosRoot
		{
			get
			{
				return CaravanTweenerUtility.PatherTweenedPosRoot(this.caravan) + CaravanTweenerUtility.CaravanCollisionPosOffsetFor(this.caravan);
			}
		}

		// Token: 0x060156E9 RID: 87785 RVA: 0x0066A0E4 File Offset: 0x006682E4
		public Caravan_Tweener(Caravan caravan)
		{
			this.caravan = caravan;
		}

		// Token: 0x060156EA RID: 87786 RVA: 0x0066A100 File Offset: 0x00668300
		public void TweenerTickInterval(int delta)
		{
			this.lastTickSpringPos = this.tweenedPos;
			Vector3 a = this.TweenedPosRoot - this.tweenedPos;
			this.tweenedPos += a * 0.09f;
		}

		// Token: 0x060156EB RID: 87787 RVA: 0x0066A147 File Offset: 0x00668347
		public void ResetTweenedPosToRoot()
		{
			this.tweenedPos = this.TweenedPosRoot;
			this.lastTickSpringPos = this.tweenedPos;
		}

		// Token: 0x0400EE85 RID: 61061
		private Caravan caravan;

		// Token: 0x0400EE86 RID: 61062
		private Vector3 tweenedPos = Vector3.zero;

		// Token: 0x0400EE87 RID: 61063
		private Vector3 lastTickSpringPos;

		// Token: 0x0400EE88 RID: 61064
		private const float SpringTightness = 0.09f;
	}
}
