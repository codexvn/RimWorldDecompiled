using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020027EC RID: 10220
	public abstract class RitualVisualEffectComp : IExposable
	{
		// Token: 0x0600DFBF RID: 57279 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ShouldSpawnNow(LordJob_Ritual ritual)
		{
			return false;
		}

		// Token: 0x0600DFC0 RID: 57280
		protected abstract Vector3 SpawnPos(LordJob_Ritual ritual);

		// Token: 0x17002228 RID: 8744
		// (get) Token: 0x0600DFC1 RID: 57281 RVA: 0x0041B574 File Offset: 0x00419774
		protected virtual ThingDef MoteDef
		{
			get
			{
				return this.props.moteDef;
			}
		}

		// Token: 0x0600DFC2 RID: 57282 RVA: 0x0041B584 File Offset: 0x00419784
		public virtual Mote SpawnMote(LordJob_Ritual ritual, Vector3? forcedPos = null)
		{
			Vector3? vector = this.ScaledPos(ritual, forcedPos);
			if (vector == null)
			{
				return null;
			}
			Mote mote = MoteMaker.MakeStaticMote(vector.Value, ritual.Map, this.MoteDef, 1f, false, 0f);
			if (mote == null)
			{
				return null;
			}
			mote.exactRotation = this.props.rotation.RandomInRange;
			mote.Scale = this.props.scale.RandomInRange * (this.props.scaleWithRoom ? this.ScaleForRoom(ritual) : 1f);
			mote.rotationRate = this.props.rotationRate.RandomInRange;
			if (!this.props.overrideColors.NullOrEmpty<Color>())
			{
				mote.instanceColor = this.props.overrideColors.RandomElement<Color>();
			}
			if (mote.def.mote.needsMaintenance)
			{
				mote.Maintain();
			}
			return mote;
		}

		// Token: 0x0600DFC3 RID: 57283 RVA: 0x0041B670 File Offset: 0x00419870
		public virtual void SpawnFleck(LordJob_Ritual ritual, Vector3? forcedPos = null, float? exactRotation = null)
		{
			Vector3? vector = this.ScaledPos(ritual, forcedPos);
			if (vector == null)
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(vector.Value, ritual.Map, this.props.fleckDef, 1f);
			if (exactRotation != null)
			{
				dataStatic.rotation = exactRotation.Value;
			}
			else
			{
				dataStatic.rotation = this.props.rotation.RandomInRange;
			}
			dataStatic.scale = this.props.scale.RandomInRange * (this.props.scaleWithRoom ? this.ScaleForRoom(ritual) : 1f);
			dataStatic.rotationRate = this.props.rotationRate.RandomInRange;
			dataStatic.velocity = new Vector3?(this.props.velocityDir * this.props.velocity.RandomInRange);
			if (!this.props.overrideColors.NullOrEmpty<Color>())
			{
				dataStatic.instanceColor = new Color?(this.props.overrideColors.RandomElement<Color>());
			}
			else if (this.props.colorOverride != null)
			{
				dataStatic.instanceColor = this.props.colorOverride;
			}
			ritual.Map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x0600DFC4 RID: 57284 RVA: 0x0041B7C0 File Offset: 0x004199C0
		public Vector3? ScaledPos(LordJob_Ritual ritual, Vector3? forcedPos = null)
		{
			Vector3 vector = ritual.Spot.ToVector3Shifted();
			Vector3 vector2 = (forcedPos ?? this.SpawnPos(ritual)) + this.props.offset;
			if (this.props.scalePositionWithRoom)
			{
				float d = this.ScaleForRoom(ritual);
				Vector3 b = (vector2 - vector) * d;
				vector2 = vector + b;
			}
			if (this.props.onlySpawnInSameRoom && (new IntVec3(vector2) + this.props.roomCheckOffset).GetRoom(ritual.Map) != ritual.GetRoom)
			{
				return null;
			}
			return new Vector3?(vector2);
		}

		// Token: 0x0600DFC5 RID: 57285 RVA: 0x0041B880 File Offset: 0x00419A80
		public virtual Effecter SpawnEffecter(LordJob_Ritual ritual, TargetInfo target, Vector3? forcedPos = null)
		{
			TargetInfo a = target.IsValid ? target : ritual.selectedTarget;
			Effecter effecter = this.props.effecterDef.Spawn(a.Cell, this.parent.ritual.Map, this.props.offset, 1f);
			effecter.Trigger(a, TargetInfo.Invalid, -1);
			return effecter;
		}

		// Token: 0x0600DFC6 RID: 57286 RVA: 0x0041B8E5 File Offset: 0x00419AE5
		public virtual void OnSetup(RitualVisualEffect parent, LordJob_Ritual ritual, bool loading)
		{
			this.parent = parent;
		}

		// Token: 0x0600DFC7 RID: 57287 RVA: 0x0041B8F0 File Offset: 0x00419AF0
		public virtual void TickInterval(int delta)
		{
			if (!this.ShouldSpawnNow(this.parent.ritual))
			{
				return;
			}
			if (this.MoteDef != null)
			{
				Mote mote = this.SpawnMote(this.parent.ritual, null);
				if (mote != null && mote.def.mote.needsMaintenance)
				{
					this.parent.AddMoteToMaintain(mote);
				}
			}
			if (this.props.fleckDef != null)
			{
				this.SpawnFleck(this.parent.ritual, null, null);
			}
			if (this.props.effecterDef != null)
			{
				this.SpawnEffecter(this.parent.ritual, TargetInfo.Invalid, null);
			}
		}

		// Token: 0x0600DFC8 RID: 57288 RVA: 0x0041B9B4 File Offset: 0x00419BB4
		public float ScaleForRoom(LordJob_Ritual ritual)
		{
			Room getRoom = ritual.GetRoom;
			if (getRoom == null || getRoom.PsychologicallyOutdoors || !getRoom.ProperRoom || ritual.RoomBoundsCached.IsInvalid)
			{
				return 1f;
			}
			float value = (float)(ritual.RoomBoundsCached.x + ritual.RoomBoundsCached.z) / 2f;
			return Mathf.Lerp(0.35f, 1f, Mathf.Clamp01(Mathf.InverseLerp((float)RitualVisualEffectComp.roomDimensionRange.min, (float)RitualVisualEffectComp.roomDimensionRange.max, value)));
		}

		// Token: 0x0600DFC9 RID: 57289 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ExposeData()
		{
		}

		// Token: 0x0400969E RID: 38558
		protected RitualVisualEffect parent;

		// Token: 0x0400969F RID: 38559
		public CompProperties_RitualVisualEffect props;

		// Token: 0x040096A0 RID: 38560
		public const float minScaleForRoom = 0.35f;

		// Token: 0x040096A1 RID: 38561
		public static readonly IntRange roomDimensionRange = new IntRange(7, 18);
	}
}
