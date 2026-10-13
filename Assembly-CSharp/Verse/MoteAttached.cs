using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008BD RID: 2237
	public class MoteAttached : Mote
	{
		// Token: 0x06003898 RID: 14488 RVA: 0x0012D6BE File Offset: 0x0012B8BE
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.exactPosition += this.def.mote.attachedDrawOffset;
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x0012D6EC File Offset: 0x0012B8EC
		protected override void TimeInterval(float deltaTime)
		{
			base.TimeInterval(deltaTime);
			if (!this.link1.Linked)
			{
				return;
			}
			bool flag = this.detachAfterTicks == -1 || Find.TickManager.TicksGame - this.spawnTick < this.detachAfterTicks;
			if (!this.link1.Target.ThingDestroyed && flag)
			{
				this.link1.UpdateDrawPos();
				if (this.link1.rotateWithTarget)
				{
					base.Rotation = this.link1.Target.Thing.Rotation;
				}
			}
			Vector3 b = this.def.mote.attachedDrawOffset;
			if (this.def.mote.attachedToHead)
			{
				Pawn pawn = this.link1.Target.Thing as Pawn;
				if (pawn != null)
				{
					bool humanlike = pawn.RaceProps.Humanlike;
					List<Vector3> headPosPerRotation = pawn.RaceProps.headPosPerRotation;
					Rot4 rotation = (pawn.GetPosture() == PawnPosture.Standing) ? (humanlike ? Rot4.North : pawn.Rotation) : pawn.Drawer.renderer.LayingFacing();
					if (humanlike)
					{
						b = pawn.Drawer.renderer.BaseHeadOffsetAt(rotation).RotatedBy(pawn.Drawer.renderer.BodyAngle(PawnRenderFlags.None));
					}
					else
					{
						float bodySizeFactor = pawn.ageTracker.CurLifeStage.bodySizeFactor;
						Vector2 vector = pawn.ageTracker.CurKindLifeStage.bodyGraphicData.drawSize * bodySizeFactor;
						b = ((!headPosPerRotation.NullOrEmpty<Vector3>()) ? headPosPerRotation[rotation.AsInt].ScaledBy(new Vector3(vector.x, 1f, vector.y)) : (MoteAttached.animalHeadOffsets[rotation.AsInt] * pawn.BodySize));
					}
				}
			}
			this.exactPosition = this.link1.LastDrawPos + b;
			IntVec3 intVec = this.exactPosition.ToIntVec3();
			if (base.Spawned && !intVec.InBounds(base.Map))
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.Position = intVec;
		}

		// Token: 0x040029E8 RID: 10728
		private static readonly List<Vector3> animalHeadOffsets = new List<Vector3>
		{
			new Vector3(0f, 0f, 0.4f),
			new Vector3(0.4f, 0f, 0.25f),
			new Vector3(0f, 0f, 0.1f),
			new Vector3(-0.4f, 0f, 0.25f)
		};
	}
}
