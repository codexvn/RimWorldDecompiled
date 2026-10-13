using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002804 RID: 10244
	public abstract class CompRitualEffect_SpawnOnPawn : CompRitualEffect_IntervalSpawn
	{
		// Token: 0x17002232 RID: 8754
		// (get) Token: 0x0600E005 RID: 57349 RVA: 0x0041C69C File Offset: 0x0041A89C
		protected new CompProperties_RitualEffectSpawnOnPawn Props
		{
			get
			{
				return (CompProperties_RitualEffectSpawnOnPawn)this.props;
			}
		}

		// Token: 0x0600E006 RID: 57350 RVA: 0x001E5B28 File Offset: 0x001E3D28
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return Vector3.zero;
		}

		// Token: 0x0600E007 RID: 57351
		protected abstract Pawn GetPawn(LordJob_Ritual ritual);

		// Token: 0x0600E008 RID: 57352 RVA: 0x0041C6AC File Offset: 0x0041A8AC
		public override void SpawnFleck(LordJob_Ritual ritual, Vector3? forcedPos = null, float? exactRotation = null)
		{
			if (this.Props.fleckDef != null)
			{
				Pawn pawn = this.GetPawn(ritual);
				if (pawn != null && (this.Props.requiredTag.NullOrEmpty() || ritual.PawnTagSet(pawn, this.Props.requiredTag)))
				{
					Vector3 vector = this.props.offset.RotatedBy(pawn.Rotation);
					if (pawn.Rotation == Rot4.East)
					{
						vector += this.Props.eastRotationOffset;
					}
					else if (pawn.Rotation == Rot4.West)
					{
						vector += this.Props.westRotationOffset;
					}
					else if (pawn.Rotation == Rot4.North)
					{
						vector += this.Props.northRotationOffset;
					}
					else if (pawn.Rotation == Rot4.South)
					{
						vector += this.Props.southRotationOffset;
					}
					base.SpawnFleck(this.parent.ritual, new Vector3?(pawn.Position.ToVector3Shifted() + vector), new float?(pawn.Rotation.AsAngle));
				}
				this.burstsDone++;
				this.lastSpawnTick = GenTicks.TicksGame;
			}
		}

		// Token: 0x0600E009 RID: 57353 RVA: 0x0041C804 File Offset: 0x0041AA04
		public override Mote SpawnMote(LordJob_Ritual ritual, Vector3? forcedPos = null)
		{
			Mote result = null;
			if (this.Props.moteDef != null)
			{
				Pawn pawn = this.GetPawn(ritual);
				if (pawn != null && (this.Props.requiredTag.NullOrEmpty() || ritual.PawnTagSet(pawn, this.Props.requiredTag)))
				{
					Vector3 b = this.props.offset.RotatedBy(pawn.Rotation);
					result = base.SpawnMote(this.parent.ritual, new Vector3?(pawn.Position.ToVector3Shifted() + b));
				}
				this.burstsDone++;
				this.lastSpawnTick = GenTicks.TicksGame;
			}
			return result;
		}

		// Token: 0x0600E00A RID: 57354 RVA: 0x0041C8B0 File Offset: 0x0041AAB0
		public override Effecter SpawnEffecter(LordJob_Ritual ritual, TargetInfo target, Vector3? forcedPos = null)
		{
			Effecter result = null;
			if (this.Props.effecterDef != null)
			{
				Pawn pawn = this.GetPawn(ritual);
				if (pawn != null && (this.Props.requiredTag.NullOrEmpty() || ritual.PawnTagSet(pawn, this.Props.requiredTag)))
				{
					Vector3 value = this.props.offset.RotatedBy(pawn.Rotation);
					result = base.SpawnEffecter(this.parent.ritual, pawn, new Vector3?(value));
				}
				this.burstsDone++;
				this.lastSpawnTick = GenTicks.TicksGame;
			}
			return result;
		}
	}
}
