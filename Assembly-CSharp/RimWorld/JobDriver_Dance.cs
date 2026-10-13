using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200119F RID: 4511
	public class JobDriver_Dance : JobDriver
	{
		// Token: 0x17001116 RID: 4374
		// (get) Token: 0x06006E4B RID: 28235 RVA: 0x00226FF7 File Offset: 0x002251F7
		public int AgeTicks
		{
			get
			{
				return Find.TickManager.TicksGame - this.startTick;
			}
		}

		// Token: 0x17001117 RID: 4375
		// (get) Token: 0x06006E4C RID: 28236 RVA: 0x0022700C File Offset: 0x0022520C
		public override Vector3 ForcedBodyOffset
		{
			get
			{
				float num = Mathf.Sin((float)this.AgeTicks / 60f * 8f);
				if (this.jumping)
				{
					float z = Mathf.Max(Mathf.Pow((num + 1f) * 0.5f, 2f) * 0.2f - 0.06f, 0f);
					return new Vector3(0f, 0f, z);
				}
				float num2 = Mathf.Sign(num);
				return new Vector3(EasingFunctions.EaseInOutQuad(Mathf.Abs(num) * 0.6f) * 0.09f * num2, 0f, 0f);
			}
		}

		// Token: 0x06006E4D RID: 28237 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06006E4E RID: 28238 RVA: 0x002270A8 File Offset: 0x002252A8
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.pawn.Rotation = Rot4.Random;
		}

		// Token: 0x06006E4F RID: 28239 RVA: 0x002270C0 File Offset: 0x002252C0
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Dance job"))
			{
				yield break;
			}
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			this.jumping = Rand.Bool;
			toil.tickIntervalAction = delegate(int delta)
			{
				if (this.AgeTicks % this.moveChangeInterval == 0)
				{
					this.jumping = !this.jumping;
				}
				if (this.AgeTicks % 120 == 0 && !this.jumping)
				{
					this.pawn.Rotation = Rot4.Random;
				}
			};
			toil.socialMode = RandomSocialMode.SuperActive;
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.handlingFacing = true;
			yield return toil;
			yield break;
		}

		// Token: 0x04004D0B RID: 19723
		private bool jumping;

		// Token: 0x04004D0C RID: 19724
		private int moveChangeInterval = 240;
	}
}
