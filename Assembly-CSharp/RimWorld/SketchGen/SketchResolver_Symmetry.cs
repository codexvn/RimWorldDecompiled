using System;
using System.Linq;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x020038DC RID: 14556
	public class SketchResolver_Symmetry : SketchResolver
	{
		// Token: 0x06014162 RID: 82274 RVA: 0x005F9D0C File Offset: 0x005F7F0C
		protected override void ResolveInt(SketchResolveParams parms)
		{
			bool flag = parms.symmetryClear ?? true;
			int valueOrDefault = parms.symmetryOrigin.GetValueOrDefault();
			bool valueOrDefault2 = parms.symmetryVertical.GetValueOrDefault();
			bool valueOrDefault3 = parms.requireFloor.GetValueOrDefault();
			bool valueOrDefault4 = parms.symmetryOriginIncluded.GetValueOrDefault();
			if (flag)
			{
				this.Clear(parms.sketch, valueOrDefault, valueOrDefault2, valueOrDefault4);
			}
			foreach (SketchBuildable sketchBuildable in parms.sketch.Buildables.ToList<SketchBuildable>())
			{
				if (!this.ShouldKeepAlreadySymmetricalInTheMiddle(sketchBuildable, valueOrDefault, valueOrDefault2, valueOrDefault4))
				{
					SketchBuildable sketchBuildable2 = (SketchBuildable)sketchBuildable.DeepCopy();
					SketchThing sketchThing = sketchBuildable2 as SketchThing;
					if (sketchThing != null && sketchThing.def.rotatable)
					{
						if (valueOrDefault2)
						{
							if (!sketchThing.rot.IsHorizontal)
							{
								sketchThing.rot = sketchThing.rot.Opposite;
							}
						}
						else if (sketchThing.rot.IsHorizontal)
						{
							sketchThing.rot = sketchThing.rot.Opposite;
						}
					}
					this.MoveUntilSymmetrical(sketchBuildable2, sketchBuildable.OccupiedRect, valueOrDefault, valueOrDefault2, valueOrDefault4);
					if (valueOrDefault3 && sketchBuildable2.Buildable != ThingDefOf.Wall && sketchBuildable2.Buildable != ThingDefOf.Door)
					{
						bool flag2 = true;
						foreach (IntVec3 pos in sketchBuildable2.OccupiedRect)
						{
							if (!parms.sketch.AnyTerrainAt(pos))
							{
								flag2 = false;
								break;
							}
						}
						if (flag2)
						{
							parms.sketch.Add(sketchBuildable2, true);
						}
					}
					else
					{
						parms.sketch.Add(sketchBuildable2, true);
					}
				}
			}
		}

		// Token: 0x06014163 RID: 82275 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool CanResolveInt(SketchResolveParams parms)
		{
			return true;
		}

		// Token: 0x06014164 RID: 82276 RVA: 0x005F9F10 File Offset: 0x005F8110
		private void Clear(Sketch sketch, int origin, bool vertical, bool originIncluded)
		{
			foreach (SketchBuildable sketchBuildable in sketch.Buildables.ToList<SketchBuildable>())
			{
				CellRect occupiedRect = sketchBuildable.OccupiedRect;
				if (((occupiedRect.maxX >= origin && !vertical) || (occupiedRect.maxZ >= origin && vertical)) && !this.ShouldKeepAlreadySymmetricalInTheMiddle(sketchBuildable, origin, vertical, originIncluded))
				{
					sketch.Remove(sketchBuildable);
				}
			}
		}

		// Token: 0x06014165 RID: 82277 RVA: 0x005F9F9C File Offset: 0x005F819C
		private bool ShouldKeepAlreadySymmetricalInTheMiddle(SketchBuildable buildable, int origin, bool vertical, bool originIncluded)
		{
			CellRect occupiedRect = buildable.OccupiedRect;
			if (vertical)
			{
				if (originIncluded)
				{
					return occupiedRect.maxZ - origin == origin - occupiedRect.minZ;
				}
				return occupiedRect.maxZ - origin + 1 == origin - occupiedRect.minZ;
			}
			else
			{
				if (originIncluded)
				{
					return occupiedRect.maxX - origin == origin - occupiedRect.minX;
				}
				return occupiedRect.maxX - origin + 1 == origin - occupiedRect.minX;
			}
		}

		// Token: 0x06014166 RID: 82278 RVA: 0x005FA00C File Offset: 0x005F820C
		private void MoveUntilSymmetrical(SketchBuildable buildable, CellRect initial, int origin, bool vertical, bool originIncluded)
		{
			if (vertical)
			{
				buildable.pos.x = buildable.pos.x + (initial.minX - buildable.OccupiedRect.minX);
				int num;
				if (originIncluded)
				{
					num = origin - initial.maxZ + origin;
				}
				else
				{
					num = origin - initial.maxZ - 1 + origin;
				}
				buildable.pos.z = buildable.pos.z + (num - buildable.OccupiedRect.minZ);
				return;
			}
			buildable.pos.z = buildable.pos.z + (initial.minZ - buildable.OccupiedRect.minZ);
			int num2;
			if (originIncluded)
			{
				num2 = origin - initial.maxX + origin;
			}
			else
			{
				num2 = origin - initial.maxX - 1 + origin;
			}
			buildable.pos.x = buildable.pos.x + (num2 - buildable.OccupiedRect.minX);
		}
	}
}
