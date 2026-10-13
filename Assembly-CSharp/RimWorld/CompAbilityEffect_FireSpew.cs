using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200222A RID: 8746
	public class CompAbilityEffect_FireSpew : CompAbilityEffect
	{
		// Token: 0x17001D2F RID: 7471
		// (get) Token: 0x0600BF2C RID: 48940 RVA: 0x0037703D File Offset: 0x0037523D
		private new CompProperties_AbilityFireSpew Props
		{
			get
			{
				return (CompProperties_AbilityFireSpew)this.props;
			}
		}

		// Token: 0x17001D30 RID: 7472
		// (get) Token: 0x0600BF2D RID: 48941 RVA: 0x00375CA4 File Offset: 0x00373EA4
		private Pawn Pawn
		{
			get
			{
				return this.parent.pawn;
			}
		}

		// Token: 0x0600BF2E RID: 48942 RVA: 0x0037704C File Offset: 0x0037524C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			IntVec3 cell = target.Cell;
			Map mapHeld = this.parent.pawn.MapHeld;
			float radius = 0f;
			DamageDef flame = DamageDefOf.Flame;
			Thing pawn = this.Pawn;
			ThingDef filthDef = this.Props.filthDef;
			int damAmount = this.Props.damAmount;
			float armorPenetration = -1f;
			SoundDef explosionSound = null;
			ThingDef weapon = null;
			ThingDef projectile = null;
			Thing intendedTarget = null;
			ThingDef postExplosionSpawnThingDef = filthDef;
			float postExplosionSpawnChance = 1f;
			int postExplosionSpawnThingCount = 1;
			SimpleCurve flammabilityAttachFireChanceCurve = this.parent.verb.verbProps.flammabilityAttachFireChanceCurve;
			List<IntVec3> overrideCells = this.AffectedCells(target);
			GenExplosion.DoExplosion(cell, mapHeld, radius, flame, pawn, damAmount, armorPenetration, explosionSound, weapon, projectile, intendedTarget, postExplosionSpawnThingDef, postExplosionSpawnChance, postExplosionSpawnThingCount, null, null, 255, false, null, 0f, 1, 1f, false, null, null, null, false, 0.6f, 0f, false, null, 1f, flammabilityAttachFireChanceCurve, overrideCells, null, null);
			base.Apply(target, dest);
		}

		// Token: 0x0600BF2F RID: 48943 RVA: 0x00377123 File Offset: 0x00375323
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			if (this.Props.effecterDef != null)
			{
				yield return new PreCastAction
				{
					action = delegate(LocalTargetInfo a, LocalTargetInfo b)
					{
						this.parent.AddEffecterToMaintain(this.Props.effecterDef.Spawn(this.parent.pawn.Position, a.Cell, this.parent.pawn.Map, 1f), this.Pawn.Position, a.Cell, 17, this.Pawn.MapHeld);
					},
					ticksAwayFromCast = 17
				};
			}
			yield break;
		}

		// Token: 0x0600BF30 RID: 48944 RVA: 0x00377133 File Offset: 0x00375333
		public override void DrawEffectPreview(LocalTargetInfo target)
		{
			GenDraw.DrawFieldEdges(this.AffectedCells(target), 2900);
		}

		// Token: 0x0600BF31 RID: 48945 RVA: 0x00377148 File Offset: 0x00375348
		public override bool AICanTargetNow(LocalTargetInfo target)
		{
			if (this.Pawn.Faction != null)
			{
				foreach (IntVec3 c in this.AffectedCells(target))
				{
					List<Thing> thingList = c.GetThingList(this.Pawn.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i].Faction == this.Pawn.Faction)
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x0600BF32 RID: 48946 RVA: 0x003771E4 File Offset: 0x003753E4
		private List<IntVec3> AffectedCells(LocalTargetInfo target)
		{
			this.tmpCells.Clear();
			Vector3 b = this.Pawn.Position.ToVector3Shifted().Yto0();
			IntVec3 intVec = target.Cell.ClampInsideMap(this.Pawn.Map);
			if (this.Pawn.Position == intVec)
			{
				return this.tmpCells;
			}
			float lengthHorizontal = (intVec - this.Pawn.Position).LengthHorizontal;
			float num = (float)(intVec.x - this.Pawn.Position.x) / lengthHorizontal;
			float num2 = (float)(intVec.z - this.Pawn.Position.z) / lengthHorizontal;
			intVec.x = Mathf.RoundToInt((float)this.Pawn.Position.x + num * this.Props.range);
			intVec.z = Mathf.RoundToInt((float)this.Pawn.Position.z + num2 * this.Props.range);
			float target2 = Vector3.SignedAngle(intVec.ToVector3Shifted().Yto0() - b, Vector3.right, Vector3.up);
			float num3 = this.Props.lineWidthEnd / 2f;
			float num4 = Mathf.Sqrt(Mathf.Pow((intVec - this.Pawn.Position).LengthHorizontal, 2f) + Mathf.Pow(num3, 2f));
			float num5 = 57.29578f * Mathf.Asin(num3 / num4);
			int num6 = GenRadial.NumCellsInRadius(this.Props.range);
			for (int i = 0; i < num6; i++)
			{
				IntVec3 intVec2 = this.Pawn.Position + GenRadial.RadialPattern[i];
				if (this.<AffectedCells>g__CanUseCell|9_0(intVec2) && Mathf.Abs(Mathf.DeltaAngle(Vector3.SignedAngle(intVec2.ToVector3Shifted().Yto0() - b, Vector3.right, Vector3.up), target2)) <= num5)
				{
					this.tmpCells.Add(intVec2);
				}
			}
			List<IntVec3> list = GenSight.BresenhamCellsBetween(this.Pawn.Position, intVec);
			for (int j = 0; j < list.Count; j++)
			{
				IntVec3 intVec3 = list[j];
				if (!this.tmpCells.Contains(intVec3) && this.<AffectedCells>g__CanUseCell|9_0(intVec3))
				{
					this.tmpCells.Add(intVec3);
				}
			}
			return this.tmpCells;
		}

		// Token: 0x0600BF35 RID: 48949 RVA: 0x003774E8 File Offset: 0x003756E8
		[CompilerGenerated]
		private bool <AffectedCells>g__CanUseCell|9_0(IntVec3 c)
		{
			ShootLine shootLine;
			return c.InBounds(this.Pawn.Map) && !(c == this.Pawn.Position) && (this.Props.canHitFilledCells || !c.Filled(this.Pawn.Map)) && c.InHorDistOf(this.Pawn.Position, this.Props.range) && this.parent.verb.TryFindShootLineFromTo(this.parent.pawn.Position, c, out shootLine, false);
		}

		// Token: 0x04008340 RID: 33600
		private readonly List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
