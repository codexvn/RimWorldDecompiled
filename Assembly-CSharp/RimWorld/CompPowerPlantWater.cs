using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200213C RID: 8508
	[StaticConstructorOnStartup]
	public class CompPowerPlantWater : CompPowerPlant
	{
		// Token: 0x17001C3C RID: 7228
		// (get) Token: 0x0600B9B2 RID: 47538 RVA: 0x003592C6 File Offset: 0x003574C6
		protected override float DesiredPowerOutput
		{
			get
			{
				if (this.cacheDirty)
				{
					this.RebuildCache();
				}
				if (!this.waterUsable)
				{
					return 0f;
				}
				if (this.waterDoubleUsed)
				{
					return base.DesiredPowerOutput * 0.3f;
				}
				return base.DesiredPowerOutput;
			}
		}

		// Token: 0x0600B9B3 RID: 47539 RVA: 0x003592FF File Offset: 0x003574FF
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.spinPosition = Rand.Range(0f, 15f);
			this.RebuildCache();
			this.ForceOthersToRebuildCache(this.parent.Map);
		}

		// Token: 0x0600B9B4 RID: 47540 RVA: 0x00359334 File Offset: 0x00357534
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			this.ForceOthersToRebuildCache(map);
		}

		// Token: 0x0600B9B5 RID: 47541 RVA: 0x00359345 File Offset: 0x00357545
		private void ClearCache()
		{
			this.cacheDirty = true;
		}

		// Token: 0x0600B9B6 RID: 47542 RVA: 0x00359350 File Offset: 0x00357550
		private void RebuildCache()
		{
			this.waterUsable = true;
			foreach (IntVec3 c in this.WaterCells())
			{
				if (c.InBounds(this.parent.Map) && !c.GetAffordances(this.parent.Map).Contains(TerrainAffordanceDefOf.MovingFluid))
				{
					this.waterUsable = false;
					break;
				}
			}
			this.waterDoubleUsed = false;
			List<Building> list = this.parent.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.WatermillGenerator);
			foreach (IntVec3 c2 in this.WaterUseCells())
			{
				if (c2.InBounds(this.parent.Map))
				{
					foreach (Building building in list)
					{
						if (building != this.parent && building.GetComp<CompPowerPlantWater>().WaterUseRect().Contains(c2))
						{
							this.waterDoubleUsed = true;
							break;
						}
					}
				}
			}
			if (!this.waterUsable)
			{
				this.spinRate = 0f;
				return;
			}
			Vector3 vector = Vector3.zero;
			foreach (IntVec3 intVec in this.WaterCells())
			{
				vector += this.parent.Map.waterInfo.GetWaterMovement(intVec.ToVector3Shifted());
			}
			this.spinRate = Mathf.Sign(Vector3.Dot(vector, this.parent.Rotation.Rotated(RotationDirection.Clockwise).FacingCell.ToVector3()));
			this.spinRate *= Rand.RangeSeeded(0.9f, 1.1f, this.parent.thingIDNumber * 60509 + 33151);
			if (this.waterDoubleUsed)
			{
				this.spinRate *= 0.5f;
			}
			this.cacheDirty = false;
		}

		// Token: 0x0600B9B7 RID: 47543 RVA: 0x003595A8 File Offset: 0x003577A8
		private void ForceOthersToRebuildCache(Map map)
		{
			foreach (Building building in map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.WatermillGenerator))
			{
				building.GetComp<CompPowerPlantWater>().ClearCache();
			}
		}

		// Token: 0x0600B9B8 RID: 47544 RVA: 0x00359608 File Offset: 0x00357808
		public override void CompTick()
		{
			base.CompTick();
			if (base.PowerOutput > 0.01f)
			{
				this.spinPosition = (this.spinPosition + 0.006666667f * this.spinRate + 6.2831855f) % 6.2831855f;
			}
			if (this.parent.IsHashIntervalTick(1200))
			{
				this.RebuildCache();
			}
		}

		// Token: 0x0600B9B9 RID: 47545 RVA: 0x00359665 File Offset: 0x00357865
		public IEnumerable<IntVec3> WaterCells()
		{
			return CompPowerPlantWater.WaterCells(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x0600B9BA RID: 47546 RVA: 0x00359682 File Offset: 0x00357882
		public static IEnumerable<IntVec3> WaterCells(IntVec3 loc, Rot4 rot)
		{
			IntVec3 perpOffset = rot.Rotated(RotationDirection.Counterclockwise).FacingCell;
			yield return loc + rot.FacingCell * 3;
			yield return loc + rot.FacingCell * 3 - perpOffset;
			yield return loc + rot.FacingCell * 3 - perpOffset * 2;
			yield return loc + rot.FacingCell * 3 + perpOffset;
			yield return loc + rot.FacingCell * 3 + perpOffset * 2;
			yield break;
		}

		// Token: 0x0600B9BB RID: 47547 RVA: 0x00359699 File Offset: 0x00357899
		public CellRect WaterUseRect()
		{
			return CompPowerPlantWater.WaterUseRect(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x0600B9BC RID: 47548 RVA: 0x003596B8 File Offset: 0x003578B8
		public static CellRect WaterUseRect(IntVec3 loc, Rot4 rot)
		{
			int width = rot.IsHorizontal ? 7 : 13;
			int height = rot.IsHorizontal ? 13 : 7;
			return CellRect.CenteredOn(loc + rot.FacingCell * 4, width, height);
		}

		// Token: 0x0600B9BD RID: 47549 RVA: 0x003596FD File Offset: 0x003578FD
		public IEnumerable<IntVec3> WaterUseCells()
		{
			return CompPowerPlantWater.WaterUseCells(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x0600B9BE RID: 47550 RVA: 0x0035971A File Offset: 0x0035791A
		public static IEnumerable<IntVec3> WaterUseCells(IntVec3 loc, Rot4 rot)
		{
			foreach (IntVec3 intVec in CompPowerPlantWater.WaterUseRect(loc, rot))
			{
				yield return intVec;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600B9BF RID: 47551 RVA: 0x00359731 File Offset: 0x00357931
		public IEnumerable<IntVec3> GroundCells()
		{
			return CompPowerPlantWater.GroundCells(this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x0600B9C0 RID: 47552 RVA: 0x0035974E File Offset: 0x0035794E
		public static IEnumerable<IntVec3> GroundCells(IntVec3 loc, Rot4 rot)
		{
			IntVec3 perpOffset = rot.Rotated(RotationDirection.Counterclockwise).FacingCell;
			yield return loc - rot.FacingCell;
			yield return loc - rot.FacingCell - perpOffset;
			yield return loc - rot.FacingCell + perpOffset;
			yield return loc;
			yield return loc - perpOffset;
			yield return loc + perpOffset;
			yield break;
		}

		// Token: 0x0600B9C1 RID: 47553 RVA: 0x00359768 File Offset: 0x00357968
		public override void PostDraw()
		{
			base.PostDraw();
			Vector3 a = this.parent.TrueCenter();
			a += this.parent.Rotation.FacingCell.ToVector3() * 2.36f;
			for (int i = 0; i < 9; i++)
			{
				float num = this.spinPosition + 6.2831855f * (float)i / 9f;
				float x = Mathf.Abs(4f * Mathf.Sin(num));
				bool flag = num % 6.2831855f < 3.1415927f;
				Vector2 vector = new Vector2(x, 1f);
				Vector3 s = new Vector3(vector.x, 1f, vector.y);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(a + Vector3.up * 0.03658537f * Mathf.Cos(num), this.parent.Rotation.AsQuat, s);
				Graphics.DrawMesh(flag ? MeshPool.plane10 : MeshPool.plane10Flip, matrix, CompPowerPlantWater.BladesMat, 0);
			}
		}

		// Token: 0x0600B9C2 RID: 47554 RVA: 0x00359888 File Offset: 0x00357A88
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (this.waterUsable && this.waterDoubleUsed)
			{
				text += "\n" + "Watermill_WaterUsedTwice".Translate();
			}
			return text;
		}

		// Token: 0x04007FFB RID: 32763
		private float spinPosition;

		// Token: 0x04007FFC RID: 32764
		private bool cacheDirty = true;

		// Token: 0x04007FFD RID: 32765
		private bool waterUsable;

		// Token: 0x04007FFE RID: 32766
		private bool waterDoubleUsed;

		// Token: 0x04007FFF RID: 32767
		private float spinRate = 1f;

		// Token: 0x04008000 RID: 32768
		private const float PowerFactorIfWaterDoubleUsed = 0.3f;

		// Token: 0x04008001 RID: 32769
		private const float SpinRateFactor = 0.006666667f;

		// Token: 0x04008002 RID: 32770
		private const float BladeOffset = 2.36f;

		// Token: 0x04008003 RID: 32771
		private const int BladeCount = 9;

		// Token: 0x04008004 RID: 32772
		private const int RebuildCacheIntervalTicks = 1200;

		// Token: 0x04008005 RID: 32773
		public static readonly Material BladesMat = MaterialPool.MatFrom("Things/Building/Power/WatermillGenerator/WatermillGeneratorBlades");
	}
}
