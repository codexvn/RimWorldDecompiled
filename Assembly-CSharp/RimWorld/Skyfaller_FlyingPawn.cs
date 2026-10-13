using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B58 RID: 11096
	public class Skyfaller_FlyingPawn : Skyfaller
	{
		// Token: 0x170026F2 RID: 9970
		// (get) Token: 0x0600F825 RID: 63525 RVA: 0x00482F94 File Offset: 0x00481194
		public Pawn Pawn
		{
			get
			{
				return (Pawn)this.innerContainer[0];
			}
		}

		// Token: 0x0600F826 RID: 63526 RVA: 0x00482FA8 File Offset: 0x004811A8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			AnimationDef bestFlyAnimation = Pawn_FlightTracker.GetBestFlyAnimation(this.Pawn, new Rot4?(Rot4.FromAngleFlat(this.angle)));
			if (bestFlyAnimation != null)
			{
				this.Pawn.Drawer.renderer.SetAnimation(bestFlyAnimation);
			}
		}

		// Token: 0x0600F827 RID: 63527 RVA: 0x00482FF4 File Offset: 0x004811F4
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			float num;
			this.GetDrawPositionAndRotation(ref drawLoc, out num);
			Pawn pawn = this.Pawn;
			if (pawn != null)
			{
				pawn.DrawNowAt(drawLoc, flip);
			}
			this.DrawDropSpotShadow();
		}

		// Token: 0x0600F828 RID: 63528 RVA: 0x00483024 File Offset: 0x00481224
		protected override void SpawnThings()
		{
			Pawn pawn = this.Pawn;
			if (pawn != null)
			{
				GenSpawn.Spawn(pawn, base.Position, base.Map, WipeMode.Vanish);
				pawn.Rotation = Rot4.East;
				pawn.Drawer.renderer.SetAnimation(null);
			}
		}

		// Token: 0x0600F829 RID: 63529 RVA: 0x0048306C File Offset: 0x0048126C
		protected override void DrawDropSpotShadow()
		{
			Material shadowMaterial = base.ShadowMaterial;
			if (shadowMaterial != null)
			{
				Vector3 drawPos = this.DrawPos;
				drawPos.y = AltitudeLayer.Shadows.AltitudeFor();
				drawPos.z = base.Position.ToVector3Shifted().z;
				Skyfaller.DrawDropSpotShadow(drawPos, base.Rotation, shadowMaterial, this.def.skyfaller.shadowSize, this.ticksToImpact);
			}
		}
	}
}
