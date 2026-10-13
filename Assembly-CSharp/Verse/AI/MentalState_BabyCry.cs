using System;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000F89 RID: 3977
	public class MentalState_BabyCry : MentalState_BabyFit
	{
		// Token: 0x06006226 RID: 25126 RVA: 0x001EFF08 File Offset: 0x001EE108
		public override void MentalStateTick(int delta)
		{
			base.MentalStateTick(delta);
			float num = this.pawn.Drawer.renderer.BodyAngle(PawnRenderFlags.None);
			Pawn pawn = this.pawn.SpawnedParentOrMe as Pawn;
			if (pawn != null && !pawn.Position.Fogged(pawn.Map))
			{
				int num2 = this.ticksUntilLeftTear - 1;
				this.ticksUntilLeftTear = num2;
				if (num2 <= 0)
				{
					pawn.Map.flecks.CreateFleck(new FleckCreationData
					{
						spawnPosition = this.pawn.DrawPosHeld.Value + new Vector3(-0.15f, 0f, 0.066f).RotatedBy(num),
						velocitySpeed = -0.66f,
						velocityAngle = 90f + num - MentalState_BabyCry.randAngle.RandomInRange,
						def = FleckDefOf.FleckBabyCrying,
						scale = MentalState_BabyCry.randScale.RandomInRange
					});
					this.ticksUntilLeftTear = MentalState_BabyCry.TicksBetweenTears.RandomInRange;
				}
				num2 = this.ticksUntilRightTear - 1;
				this.ticksUntilRightTear = num2;
				if (num2 <= 0)
				{
					pawn.Map.flecks.CreateFleck(new FleckCreationData
					{
						spawnPosition = this.pawn.DrawPosHeld.Value + new Vector3(0.15f, 0f, 0.066f).RotatedBy(num),
						velocitySpeed = 0.66f,
						velocityAngle = 90f + num + MentalState_BabyCry.randAngle.RandomInRange,
						def = FleckDefOf.FleckBabyCrying,
						scale = MentalState_BabyCry.randScale.RandomInRange,
						exactScale = new Vector3?(new Vector3(-1f, 1f, 1f))
					});
					this.ticksUntilRightTear = MentalState_BabyCry.TicksBetweenTears.RandomInRange;
				}
				if (this.pawn.IsHashIntervalTick(35, delta))
				{
					MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_BabyCryingDots, new Vector3(0.27f, 0f, 0.066f).RotatedBy(num), 1f, -1f).exactRotation = Rand.Value * 180f;
					MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_BabyCryingDots, new Vector3(-0.27f, 0f, 0.066f).RotatedBy(num), 1f, -1f).exactRotation = Rand.Value * 180f;
				}
			}
		}

		// Token: 0x06006227 RID: 25127 RVA: 0x001F01A0 File Offset: 0x001EE3A0
		protected override void AuraEffect(Thing source, Pawn hearer)
		{
			hearer.HearClamor(source, ClamorDefOf.BabyCry);
			Pawn pawn = source as Pawn;
			if (pawn != null && hearer.needs.mood != null)
			{
				if (hearer == pawn.GetMother() || hearer == pawn.GetFather())
				{
					hearer.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.MyCryingBaby, pawn, null);
				}
				else
				{
					hearer.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.CryingBaby, pawn, null);
				}
				hearer.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.BabyCriedSocial, pawn, null);
			}
		}

		// Token: 0x04004659 RID: 18009
		private int ticksUntilLeftTear;

		// Token: 0x0400465A RID: 18010
		private int ticksUntilRightTear;

		// Token: 0x0400465B RID: 18011
		private const int TicksBetweenTearDots = 35;

		// Token: 0x0400465C RID: 18012
		private static readonly IntRange TicksBetweenTears = new IntRange(25, 40);

		// Token: 0x0400465D RID: 18013
		private const float speed = 0.66f;

		// Token: 0x0400465E RID: 18014
		private static readonly FloatRange randAngle = new FloatRange(10f, 30f);

		// Token: 0x0400465F RID: 18015
		private static readonly FloatRange randScale = new FloatRange(0.6f, 1f);
	}
}
