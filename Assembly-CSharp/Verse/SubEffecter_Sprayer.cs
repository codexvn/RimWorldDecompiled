using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CBF RID: 3263
	public abstract class SubEffecter_Sprayer : SubEffecter
	{
		// Token: 0x06005374 RID: 21364 RVA: 0x0003A14D File Offset: 0x0003834D
		public SubEffecter_Sprayer(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005375 RID: 21365 RVA: 0x001ADDEC File Offset: 0x001ABFEC
		public Vector3 GetAttachedSpawnLoc(TargetInfo tgt)
		{
			Vector3 centerVector = tgt.CenterVector3;
			CompAttachPoints compAttachPoints;
			if (this.def.attachPoint != AttachPointType.RootNone && tgt.HasThing && tgt.Thing.TryGetComp(out compAttachPoints))
			{
				return compAttachPoints.points.GetWorldPos(this.def.attachPoint);
			}
			return centerVector;
		}

		// Token: 0x06005376 RID: 21366 RVA: 0x001ADE40 File Offset: 0x001AC040
		protected void MakeMote(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1)
		{
			Vector3 vector = Vector3.zero;
			switch (base.EffectiveSpawnLocType)
			{
			case MoteSpawnLocType.OnSource:
				vector = this.GetAttachedSpawnLoc(A);
				break;
			case MoteSpawnLocType.BetweenPositions:
			{
				Vector3 vector2 = A.HasThing ? A.Thing.DrawPos : A.Cell.ToVector3Shifted();
				Vector3 vector3 = B.HasThing ? B.Thing.DrawPos : B.Cell.ToVector3Shifted();
				if (A.HasThing && !A.Thing.Spawned)
				{
					vector = vector3;
				}
				else if (B.HasThing && !B.Thing.Spawned)
				{
					vector = vector2;
				}
				else
				{
					vector = vector2 * this.def.positionLerpFactor + vector3 * (1f - this.def.positionLerpFactor);
				}
				break;
			}
			case MoteSpawnLocType.BetweenTouchingCells:
				vector = A.Cell.ToVector3Shifted() + (B.Cell - A.Cell).ToVector3().normalized * 0.5f;
				break;
			case MoteSpawnLocType.RandomCellOnTarget:
			{
				CellRect cellRect;
				if (B.HasThing)
				{
					cellRect = B.Thing.OccupiedRect();
				}
				else
				{
					cellRect = CellRect.CenteredOn(B.Cell, 0);
				}
				vector = cellRect.RandomCell.ToVector3Shifted();
				break;
			}
			case MoteSpawnLocType.RandomDrawPosOnTarget:
				if (B.Thing.DrawSize != Vector2.one && B.Thing.DrawSize != Vector2.zero)
				{
					Vector2 vector4 = B.Thing.DrawSize.RotatedBy(B.Thing.Rotation);
					Vector3 b = new Vector3(vector4.x * Rand.Value, 0f, vector4.y * Rand.Value);
					vector = B.CenterVector3 + b - new Vector3(vector4.x / 2f, 0f, vector4.y / 2f);
				}
				else
				{
					Vector3 b2 = new Vector3(Rand.Value, 0f, Rand.Value);
					vector = B.CenterVector3 + b2 - new Vector3(0.5f, 0f, 0.5f);
				}
				break;
			case MoteSpawnLocType.OnTarget:
				vector = this.GetAttachedSpawnLoc(B);
				break;
			}
			if (this.parent != null)
			{
				Rand.PushState(this.parent.GetHashCode());
				if (A.CenterVector3 != B.CenterVector3)
				{
					vector += (B.CenterVector3 - A.CenterVector3).normalized * this.parent.def.offsetTowardsTarget.RandomInRange;
				}
				Vector3 a = Gen.RandomHorizontalVector(this.parent.def.positionRadius);
				Rand.PopState();
				if (base.EffectiveDimensions != null)
				{
					a += Gen.Random2DVector(base.EffectiveDimensions.Value);
				}
				vector += a + this.parent.offset;
			}
			Map map = A.Map ?? B.Map;
			float num;
			if (this.def.absoluteAngle)
			{
				num = 0f;
			}
			else if (this.def.useTargetAInitialRotation && A.HasThing)
			{
				num = A.Thing.Rotation.AsAngle;
			}
			else if (this.def.useTargetBInitialRotation && B.HasThing)
			{
				num = B.Thing.Rotation.AsAngle;
			}
			else
			{
				num = (B.Cell - A.Cell).AngleFlat;
			}
			float num2 = (this.parent != null) ? this.parent.scale : 1f;
			if (map != null)
			{
				int randomInRange = this.def.burstCount.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					Vector3 vector5 = base.EffectiveOffset;
					if (this.def.useTargetAInitialRotation && A.HasThing)
					{
						vector5 = vector5.RotatedBy(A.Thing.Rotation);
					}
					else if (this.def.useTargetBInitialRotation && B.HasThing)
					{
						vector5 = vector5.RotatedBy(B.Thing.Rotation);
					}
					else
					{
						if (this.def.useTargetABodyAngle && A.HasThing)
						{
							Pawn pawn = A.Thing as Pawn;
							if (pawn != null)
							{
								vector5 = vector5.RotatedBy(pawn.Drawer.renderer.BodyAngle(PawnRenderFlags.None));
								goto IL_528;
							}
						}
						if (this.def.useTargetBBodyAngle && B.HasThing)
						{
							Pawn pawn2 = B.Thing as Pawn;
							if (pawn2 != null)
							{
								vector5 = vector5.RotatedBy(pawn2.Drawer.renderer.BodyAngle(PawnRenderFlags.None));
							}
						}
					}
					IL_528:
					if (!this.def.perRotationOffsets.NullOrEmpty<Vector3>())
					{
						vector5 += this.def.perRotationOffsets[((base.EffectiveSpawnLocType == MoteSpawnLocType.OnSource) ? A.Thing.Rotation : B.Thing.Rotation).AsInt];
					}
					for (int j = 0; j < 5; j++)
					{
						vector5 = vector5 * num2 + Rand.InsideAnnulusVector3(this.def.positionRadiusMin, this.def.positionRadius) * num2;
						if (this.def.avoidLastPositionRadius < 1E-45f || this.lastOffset == null || (vector5 - this.lastOffset.Value).MagnitudeHorizontal() > this.def.avoidLastPositionRadius)
						{
							break;
						}
					}
					this.lastOffset = new Vector3?(vector5);
					Vector3 vector6 = vector + vector5;
					if (this.def.rotateTowardsTargetCenter)
					{
						num = (vector6 - B.CenterVector3).AngleFlat();
					}
					if (this.def.moteDef != null && vector.ShouldSpawnMotesAt(map, this.def.moteDef.drawOffscreen))
					{
						this.mote = (Mote)ThingMaker.MakeThing(this.def.moteDef, null);
						GenSpawn.Spawn(this.mote, vector.ToIntVec3(), map, WipeMode.Vanish);
						this.mote.Scale = this.def.scale.RandomInRange * num2;
						this.mote.exactPosition = vector6;
						this.mote.rotationRate = this.def.rotationRate.RandomInRange;
						this.mote.exactRotation = this.def.rotation.RandomInRange + num;
						this.mote.instanceColor = base.EffectiveColor;
						this.mote.yOffset = vector5.y;
						Mote mote = this.mote;
						List<CurvedScaler> scalers = this.def.moteDef.mote.scalers;
						mote.curvedScale = ((scalers != null) ? scalers.ScaleAtTime(0f) : Vector3.one);
						if (overrideSpawnTick != -1)
						{
							this.mote.ForceSpawnTick(overrideSpawnTick);
						}
						MoteThrown moteThrown = this.mote as MoteThrown;
						if (moteThrown != null)
						{
							moteThrown.airTimeLeft = this.def.airTime.RandomInRange;
							moteThrown.SetVelocity(this.def.angle.RandomInRange + num, this.def.speed.RandomInRange);
						}
						this.TryAttachMote(A, B, vector5);
						this.mote.Maintain();
					}
					else if (this.def.fleckDef != null && vector6.ShouldSpawnMotesAt(map, this.def.fleckDef.drawOffscreen))
					{
						float velocityAngle = this.def.fleckUsesAngleForVelocity ? (this.def.angle.RandomInRange + num) : 0f;
						FleckAttachLink invalid = FleckAttachLink.Invalid;
						if (this.def.fleckDef.useAttachLink && base.EffectiveSpawnLocType == MoteSpawnLocType.OnSource && A.IsValid)
						{
							invalid = new FleckAttachLink(A);
						}
						if (this.def.fleckDef.useAttachLink && base.EffectiveSpawnLocType == MoteSpawnLocType.OnTarget && B.IsValid)
						{
							invalid = new FleckAttachLink(B);
						}
						map.flecks.CreateFleck(new FleckCreationData
						{
							def = this.def.fleckDef,
							scale = this.def.scale.RandomInRange * num2,
							spawnPosition = vector6,
							rotationRate = this.def.rotationRate.RandomInRange,
							rotation = this.def.rotation.RandomInRange + num,
							instanceColor = new Color?(base.EffectiveColor),
							velocitySpeed = this.def.speed.RandomInRange,
							velocityAngle = velocityAngle,
							ageTicksOverride = overrideSpawnTick,
							orbitSpeed = (this.def.orbitOrigin ? this.def.orbitSpeed.RandomInRange : 0f),
							orbitSnapStrength = this.def.orbitSnapStrength,
							link = invalid
						});
					}
				}
			}
		}

		// Token: 0x06005377 RID: 21367 RVA: 0x001AE7D4 File Offset: 0x001AC9D4
		private void TryAttachMote(TargetInfo A, TargetInfo B, Vector3 posOffset)
		{
			if (this.def.attachToSpawnThing)
			{
				MoteAttached moteAttached = this.mote as MoteAttached;
				if (moteAttached != null)
				{
					bool updateOffsetToMatchTargetRotation = this.def.moteDef.mote.updateOffsetToMatchTargetRotation;
					Vector3 offset = updateOffsetToMatchTargetRotation ? base.EffectiveOffset : posOffset;
					if (base.EffectiveSpawnLocType == MoteSpawnLocType.OnSource && A.HasThing)
					{
						moteAttached.Attach(A, offset, updateOffsetToMatchTargetRotation);
					}
					else if (base.EffectiveSpawnLocType == MoteSpawnLocType.OnTarget && B.HasThing)
					{
						moteAttached.Attach(B, offset, updateOffsetToMatchTargetRotation);
					}
					if (moteAttached.link1.Linked && moteAttached.link1.rotateWithTarget && moteAttached.link1.Target.HasThing)
					{
						moteAttached.link1.UpdateDrawPos();
						moteAttached.Rotation = moteAttached.link1.Target.Thing.Rotation;
					}
				}
				MoteDualAttached moteDualAttached = this.mote as MoteDualAttached;
				if (moteDualAttached != null)
				{
					bool updateOffsetToMatchTargetRotation2 = this.def.moteDef.mote.updateOffsetToMatchTargetRotation;
					Vector3 vector = updateOffsetToMatchTargetRotation2 ? base.EffectiveOffset : posOffset;
					if (A.HasThing && B.HasThing)
					{
						moteDualAttached.Attach(A, B, vector, Vector3.zero);
						return;
					}
					if (base.EffectiveSpawnLocType == MoteSpawnLocType.OnSource && A.HasThing)
					{
						moteDualAttached.Attach(A, vector, updateOffsetToMatchTargetRotation2);
						return;
					}
					if (base.EffectiveSpawnLocType == MoteSpawnLocType.OnTarget && B.HasThing)
					{
						moteDualAttached.Attach(B, vector, updateOffsetToMatchTargetRotation2);
					}
				}
			}
		}

		// Token: 0x06005378 RID: 21368 RVA: 0x001AE94B File Offset: 0x001ACB4B
		public override void SubTrigger(TargetInfo A, TargetInfo B, int overrideSpawnTick = -1, bool force = false)
		{
			if (this.def.makeMoteOnSubtrigger)
			{
				this.MakeMote(A, B, -1);
			}
		}

		// Token: 0x06005379 RID: 21369 RVA: 0x001AE963 File Offset: 0x001ACB63
		public override void SubCleanup()
		{
			if (this.def.destroyMoteOnCleanup)
			{
				Mote mote = this.mote;
				if (mote != null)
				{
					mote.Destroy(DestroyMode.Vanish);
				}
			}
			base.SubCleanup();
		}

		// Token: 0x0600537A RID: 21370 RVA: 0x001AE98A File Offset: 0x001ACB8A
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			base.SubEffectTick(A, B);
			if (this.mote != null && this.mote.def.mote.needsMaintenance)
			{
				this.mote.Maintain();
			}
		}

		// Token: 0x04003C3D RID: 15421
		private Mote mote;

		// Token: 0x04003C3E RID: 15422
		private Vector3? lastOffset;
	}
}
