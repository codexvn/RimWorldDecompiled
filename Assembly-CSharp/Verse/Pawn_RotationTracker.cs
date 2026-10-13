using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007EC RID: 2028
	public class Pawn_RotationTracker : IExposable
	{
		// Token: 0x060033CE RID: 13262 RVA: 0x0010FD8F File Offset: 0x0010DF8F
		public Pawn_RotationTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x060033CF RID: 13263 RVA: 0x0010FD9E File Offset: 0x0010DF9E
		public void Notify_Spawned()
		{
			this.UpdateRotation();
		}

		// Token: 0x060033D0 RID: 13264 RVA: 0x0010FDA8 File Offset: 0x0010DFA8
		public void UpdateRotation()
		{
			if (this.pawn.Destroyed)
			{
				return;
			}
			if (this.pawn.kindDef.useFixedRotation)
			{
				this.pawn.Rotation = this.pawn.kindDef.fixedRotation;
				return;
			}
			if (this.pawn.jobs.HandlingFacing)
			{
				return;
			}
			if (this.pawn.stances.stunner.Stunned && this.pawn.stances.stunner.DisableRotation)
			{
				return;
			}
			Stance_Busy stance_Busy = this.pawn.stances.curStance as Stance_Busy;
			if (stance_Busy != null && stance_Busy.focusTarg.IsValid)
			{
				if (stance_Busy.focusTarg.HasThing)
				{
					this.Face(stance_Busy.focusTarg.Thing.DrawPos);
					return;
				}
				this.FaceCell(stance_Busy.focusTarg.Cell);
				return;
			}
			else
			{
				if (!this.pawn.pather.Moving)
				{
					if (this.pawn.jobs.curJob != null)
					{
						LocalTargetInfo target = this.pawn.CurJob.GetTarget(this.pawn.jobs.curDriver.rotateToFace);
						this.FaceTarget(target);
					}
					if (this.pawn.Drafted)
					{
						this.pawn.Rotation = Rot4.South;
					}
					return;
				}
				if (this.pawn.pather.curPath == null || this.pawn.pather.curPath.NodesLeftCount < 1)
				{
					return;
				}
				this.FaceAdjacentCell(this.pawn.pather.nextCell);
				return;
			}
		}

		// Token: 0x060033D1 RID: 13265 RVA: 0x0010FD9E File Offset: 0x0010DF9E
		public void ProcessPostTickVisuals(int ticksPassed)
		{
			this.UpdateRotation();
		}

		// Token: 0x060033D2 RID: 13266 RVA: 0x0010FF40 File Offset: 0x0010E140
		private void FaceAdjacentCell(IntVec3 c)
		{
			if (c == this.pawn.Position)
			{
				return;
			}
			IntVec3 intVec = c - this.pawn.Position;
			if (intVec.x > 0)
			{
				this.pawn.Rotation = Rot4.East;
				return;
			}
			if (intVec.x < 0)
			{
				this.pawn.Rotation = Rot4.West;
				return;
			}
			if (intVec.z > 0)
			{
				this.pawn.Rotation = Rot4.North;
				return;
			}
			this.pawn.Rotation = Rot4.South;
		}

		// Token: 0x060033D3 RID: 13267 RVA: 0x0010FFD4 File Offset: 0x0010E1D4
		public void FaceCell(IntVec3 c)
		{
			if (c == this.pawn.Position)
			{
				return;
			}
			float angle = (c - this.pawn.Position).ToVector3().AngleFlat();
			this.pawn.Rotation = Pawn_RotationTracker.RotFromAngleBiased(angle);
		}

		// Token: 0x060033D4 RID: 13268 RVA: 0x00110028 File Offset: 0x0010E228
		public void Face(Vector3 p)
		{
			if (p == this.pawn.DrawPos)
			{
				return;
			}
			float angle = (p - this.pawn.DrawPos).AngleFlat();
			this.pawn.Rotation = Pawn_RotationTracker.RotFromAngleBiased(angle);
		}

		// Token: 0x060033D5 RID: 13269 RVA: 0x00110074 File Offset: 0x0010E274
		public void FaceTarget(LocalTargetInfo target)
		{
			if (!target.IsValid)
			{
				return;
			}
			if (target.HasThing)
			{
				Thing thing = target.Thing.Spawned ? target.Thing : ThingOwnerUtility.GetFirstSpawnedParentThing(target.Thing);
				if (thing != null && thing.Spawned)
				{
					bool flag = false;
					IntVec3 c = default(IntVec3);
					CellRect cellRect = thing.OccupiedRect();
					for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
					{
						for (int j = cellRect.minX; j <= cellRect.maxX; j++)
						{
							if (this.pawn.Position == new IntVec3(j, 0, i))
							{
								this.Face(thing.DrawPos);
								return;
							}
						}
					}
					for (int k = cellRect.minZ; k <= cellRect.maxZ; k++)
					{
						for (int l = cellRect.minX; l <= cellRect.maxX; l++)
						{
							IntVec3 intVec = new IntVec3(l, 0, k);
							if (intVec.AdjacentToCardinal(this.pawn.Position))
							{
								this.FaceAdjacentCell(intVec);
								return;
							}
							if (intVec.AdjacentTo8Way(this.pawn.Position))
							{
								flag = true;
								c = intVec;
							}
						}
					}
					if (flag)
					{
						if (DebugViewSettings.drawPawnRotatorTarget)
						{
							this.pawn.Map.debugDrawer.FlashCell(this.pawn.Position, 0.6f, "jbthing", 50);
							GenDraw.DrawLineBetween(this.pawn.Position.ToVector3Shifted(), c.ToVector3Shifted());
						}
						this.FaceAdjacentCell(c);
						return;
					}
					this.Face(thing.DrawPos);
					return;
				}
			}
			else
			{
				if (this.pawn.Position.AdjacentTo8Way(target.Cell))
				{
					if (DebugViewSettings.drawPawnRotatorTarget)
					{
						this.pawn.Map.debugDrawer.FlashCell(this.pawn.Position, 0.2f, "jbloc", 50);
						GenDraw.DrawLineBetween(this.pawn.Position.ToVector3Shifted(), target.Cell.ToVector3Shifted());
					}
					this.FaceAdjacentCell(target.Cell);
					return;
				}
				if (target.Cell.IsValid && target.Cell != this.pawn.Position)
				{
					this.Face(target.Cell.ToVector3());
					return;
				}
			}
		}

		// Token: 0x060033D6 RID: 13270 RVA: 0x001102E0 File Offset: 0x0010E4E0
		public static Rot4 RotFromAngleBiased(float angle)
		{
			if (angle < 30f)
			{
				return Rot4.North;
			}
			if (angle < 150f)
			{
				return Rot4.East;
			}
			if (angle < 210f)
			{
				return Rot4.South;
			}
			if (angle < 330f)
			{
				return Rot4.West;
			}
			return Rot4.North;
		}

		// Token: 0x060033D7 RID: 13271 RVA: 0x000026EA File Offset: 0x000008EA
		public void ExposeData()
		{
		}

		// Token: 0x04002721 RID: 10017
		private Pawn pawn;
	}
}
