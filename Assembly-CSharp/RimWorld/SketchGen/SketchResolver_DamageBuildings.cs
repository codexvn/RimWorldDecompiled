using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038CA RID: 14538
	public class SketchResolver_DamageBuildings : SketchResolver
	{
		// Token: 0x17003324 RID: 13092
		// (get) Token: 0x06014112 RID: 82194 RVA: 0x005F71CC File Offset: 0x005F53CC
		protected virtual float MaxPctOfTotalDestroyed
		{
			get
			{
				return 0.65f;
			}
		}

		// Token: 0x17003325 RID: 13093
		// (get) Token: 0x06014113 RID: 82195 RVA: 0x005F71D3 File Offset: 0x005F53D3
		protected virtual float HpRandomFactor
		{
			get
			{
				return 1.2f;
			}
		}

		// Token: 0x17003326 RID: 13094
		// (get) Token: 0x06014114 RID: 82196 RVA: 0x005F71DA File Offset: 0x005F53DA
		protected virtual float DestroyChanceExp
		{
			get
			{
				return 1.32f;
			}
		}

		// Token: 0x06014115 RID: 82197 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return true;
		}

		// Token: 0x06014116 RID: 82198 RVA: 0x005F71E4 File Offset: 0x005F53E4
		protected override void ResolveInt(SketchResolveParams parms)
		{
			CellRect occupiedRect = parms.sketch.OccupiedRect;
			Rot4 random = Rot4.Random;
			int num = 0;
			int count = parms.sketch.Buildables.Count;
			foreach (SketchBuildable sketchBuildable in parms.sketch.Buildables.InRandomOrder(null).ToList<SketchBuildable>())
			{
				bool flag;
				this.Damage(sketchBuildable, occupiedRect, random, parms.sketch, out flag, parms.destroyChanceExp);
				if (flag)
				{
					num++;
					this.ClearDisconnectedDoors(parms, sketchBuildable.pos);
					if ((float)num > (float)count * this.MaxPctOfTotalDestroyed)
					{
						break;
					}
				}
			}
		}

		// Token: 0x06014117 RID: 82199 RVA: 0x005F72A4 File Offset: 0x005F54A4
		private void ClearDisconnectedDoors(SketchResolveParams parms, IntVec3 position)
		{
			IntVec3[] cardinalDirectionsAround = GenAdj.CardinalDirectionsAround;
			for (int i = 0; i < cardinalDirectionsAround.Length; i++)
			{
				IntVec3 position2 = cardinalDirectionsAround[i] + position;
				SketchThing door = parms.sketch.GetDoor(position2);
				if (door != null && !this.AdjacentToWall(parms.sketch, door))
				{
					parms.sketch.Remove(door);
				}
			}
		}

		// Token: 0x06014118 RID: 82200 RVA: 0x005F7300 File Offset: 0x005F5500
		private bool AdjacentToWall(Sketch sketch, SketchEntity entity)
		{
			foreach (IntVec3 b in GenAdj.CardinalDirections)
			{
				if (sketch.ThingsAt(entity.pos + b).Any((SketchThing t) => t.def == ThingDefOf.Wall))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06014119 RID: 82201 RVA: 0x005F7364 File Offset: 0x005F5564
		private void Damage(SketchBuildable buildable, CellRect rect, Rot4 dir, Sketch sketch, out bool destroyed, float? destroyChanceExp = null)
		{
			float num;
			if (dir.IsHorizontal)
			{
				num = (float)(buildable.pos.x - rect.minX) / (float)rect.Width;
			}
			else
			{
				num = (float)(buildable.pos.z - rect.minZ) / (float)rect.Height;
			}
			if (dir == Rot4.East || dir == Rot4.South)
			{
				num = 1f - num;
			}
			float num2 = num;
			num2 = Mathf.Pow(num2, destroyChanceExp ?? this.DestroyChanceExp);
			bool flag = false;
			LayoutSketch layoutSketch = sketch as LayoutSketch;
			if (layoutSketch != null)
			{
				foreach (IntVec3 pos in buildable.OccupiedRect.ExpandedBy(1).EdgeCells)
				{
					LayoutRoom layoutRoom;
					if (layoutSketch.structureLayout.TryGetRoom(pos, out layoutRoom) && layoutRoom.DontDestroyWallsDoors)
					{
						flag = true;
						break;
					}
				}
			}
			if (Rand.Chance(num2) && !flag)
			{
				sketch.Remove(buildable);
				destroyed = true;
				SketchTerrain sketchTerrain = buildable as SketchTerrain;
				if (sketchTerrain != null && sketchTerrain.def.burnedDef != null)
				{
					sketch.AddTerrain(sketchTerrain.def.burnedDef, sketchTerrain.pos, true);
					return;
				}
			}
			else
			{
				destroyed = false;
				SketchThing sketchThing = buildable as SketchThing;
				if (sketchThing != null)
				{
					sketchThing.hitPoints = new int?(Mathf.Clamp(Mathf.RoundToInt((float)sketchThing.MaxHitPoints * (1f - num) * Rand.Range(1f, this.HpRandomFactor)), 1, sketchThing.MaxHitPoints));
				}
			}
		}
	}
}
