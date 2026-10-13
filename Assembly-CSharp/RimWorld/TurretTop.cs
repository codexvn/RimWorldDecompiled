using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002988 RID: 10632
	public class TurretTop
	{
		// Token: 0x170023BB RID: 9147
		// (get) Token: 0x0600E9C2 RID: 59842 RVA: 0x00440587 File Offset: 0x0043E787
		// (set) Token: 0x0600E9C3 RID: 59843 RVA: 0x00440590 File Offset: 0x0043E790
		public float CurRotation
		{
			get
			{
				return this.curRotationInt;
			}
			set
			{
				this.curRotationInt = value;
				if (this.curRotationInt > 360f)
				{
					this.curRotationInt -= 360f;
				}
				if (this.curRotationInt < 0f)
				{
					this.curRotationInt += 360f;
				}
			}
		}

		// Token: 0x0600E9C4 RID: 59844 RVA: 0x004405E4 File Offset: 0x0043E7E4
		public void SetRotationFromOrientation()
		{
			this.CurRotation = this.parentTurret.Rotation.AsAngle;
		}

		// Token: 0x0600E9C5 RID: 59845 RVA: 0x0044060A File Offset: 0x0043E80A
		public TurretTop(Building_Turret ParentTurret)
		{
			this.parentTurret = ParentTurret;
		}

		// Token: 0x0600E9C6 RID: 59846 RVA: 0x0044061C File Offset: 0x0043E81C
		public void ForceFaceTarget(LocalTargetInfo targ)
		{
			if (targ.IsValid)
			{
				float curRotation = (targ.Cell.ToVector3Shifted() - this.parentTurret.DrawPos).AngleFlat();
				this.CurRotation = curRotation;
			}
		}

		// Token: 0x0600E9C7 RID: 59847 RVA: 0x00440660 File Offset: 0x0043E860
		public void TurretTopTick()
		{
			LocalTargetInfo currentTarget = this.parentTurret.CurrentTarget;
			if (currentTarget.IsValid)
			{
				float curRotation = (currentTarget.Cell.ToVector3Shifted() - this.parentTurret.DrawPos).AngleFlat();
				this.CurRotation = curRotation;
				this.ticksUntilIdleTurn = Rand.RangeInclusive(150, 350);
				return;
			}
			if (this.ticksUntilIdleTurn > 0)
			{
				this.ticksUntilIdleTurn--;
				if (this.ticksUntilIdleTurn == 0)
				{
					if (Rand.Value < 0.5f)
					{
						this.idleTurnClockwise = true;
					}
					else
					{
						this.idleTurnClockwise = false;
					}
					this.idleTurnTicksLeft = 140;
					return;
				}
			}
			else
			{
				if (this.idleTurnClockwise)
				{
					this.CurRotation += 0.26f;
				}
				else
				{
					this.CurRotation -= 0.26f;
				}
				this.idleTurnTicksLeft--;
				if (this.idleTurnTicksLeft <= 0)
				{
					this.ticksUntilIdleTurn = Rand.RangeInclusive(150, 350);
				}
			}
		}

		// Token: 0x0600E9C8 RID: 59848 RVA: 0x00440768 File Offset: 0x0043E968
		public void DrawTurret(Vector3 drawLoc, Vector3 recoilDrawOffset, float recoilAngleOffset)
		{
			Vector3 vector = new Vector3(this.parentTurret.def.building.turretTopOffset.x, 0f, this.parentTurret.def.building.turretTopOffset.y).RotatedBy(this.CurRotation);
			float turretTopDrawSize = this.parentTurret.def.building.turretTopDrawSize;
			vector = vector.RotatedBy(recoilAngleOffset);
			vector += recoilDrawOffset;
			Verb currentEffectiveVerb = this.parentTurret.CurrentEffectiveVerb;
			float num = ((currentEffectiveVerb != null) ? currentEffectiveVerb.AimAngleOverride : null) ?? this.CurRotation;
			Vector3 pos = drawLoc + Altitudes.AltIncVect + vector;
			Quaternion q = ((float)TurretTop.ArtworkRotation + num).ToQuat();
			Vector3 s = new Vector3(turretTopDrawSize, 1f, turretTopDrawSize);
			Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(pos, q, s), this.parentTurret.TurretTopMaterial, 0);
		}

		// Token: 0x04009B8A RID: 39818
		private Building_Turret parentTurret;

		// Token: 0x04009B8B RID: 39819
		private float curRotationInt;

		// Token: 0x04009B8C RID: 39820
		private int ticksUntilIdleTurn;

		// Token: 0x04009B8D RID: 39821
		private int idleTurnTicksLeft;

		// Token: 0x04009B8E RID: 39822
		private bool idleTurnClockwise;

		// Token: 0x04009B8F RID: 39823
		private const float IdleTurnDegreesPerTick = 0.26f;

		// Token: 0x04009B90 RID: 39824
		private const int IdleTurnDuration = 140;

		// Token: 0x04009B91 RID: 39825
		private const int IdleTurnIntervalMin = 150;

		// Token: 0x04009B92 RID: 39826
		private const int IdleTurnIntervalMax = 350;

		// Token: 0x04009B93 RID: 39827
		public static readonly int ArtworkRotation = -90;
	}
}
