using System;
using System.Collections.Generic;
using LudeonTK;

namespace Verse
{
	// Token: 0x02000D5A RID: 3418
	public static class ShootLeanUtility
	{
		// Token: 0x06005700 RID: 22272 RVA: 0x001BF938 File Offset: 0x001BDB38
		public static void LeanShootingSourcesFromTo(IntVec3 shooterLoc, IntVec3 targetPos, Map map, List<IntVec3> listToFill)
		{
			listToFill.Clear();
			float angleFlat = (targetPos - shooterLoc).AngleFlat;
			bool flag = angleFlat > 270f || angleFlat < 90f;
			bool flag2 = angleFlat > 90f && angleFlat < 270f;
			bool flag3 = angleFlat > 180f;
			bool flag4 = angleFlat < 180f;
			ByteBits byteBits = default(ByteBits);
			for (int i = 0; i < 8; i++)
			{
				byteBits[i] = !(shooterLoc + GenAdj.AdjacentCells[i]).CanBeSeenOver(map);
			}
			if (!byteBits[1] && ((byteBits[0] && !byteBits[5] && flag) || (byteBits[2] && !byteBits[4] && flag2)))
			{
				listToFill.Add(shooterLoc + IntVec3.East);
			}
			if (!byteBits[3] && ((byteBits[0] && !byteBits[6] && flag) || (byteBits[2] && !byteBits[7] && flag2)))
			{
				listToFill.Add(shooterLoc + IntVec3.West);
			}
			if (!byteBits[2] && ((byteBits[3] && !byteBits[7] && flag3) || (byteBits[1] && !byteBits[4] && flag4)))
			{
				listToFill.Add(shooterLoc + IntVec3.South);
			}
			if (!byteBits[0] && ((byteBits[3] && !byteBits[6] && flag3) || (byteBits[1] && !byteBits[5] && flag4)))
			{
				listToFill.Add(shooterLoc + IntVec3.North);
			}
			if (shooterLoc.CanBeSeenOver(map))
			{
				listToFill.Add(shooterLoc);
			}
			for (int j = 0; j < 4; j++)
			{
				if (!byteBits[j])
				{
					switch (j)
					{
					case 0:
						if (!flag)
						{
							goto IL_25E;
						}
						break;
					case 1:
						if (!flag4)
						{
							goto IL_25E;
						}
						break;
					case 2:
						if (!flag2)
						{
							goto IL_25E;
						}
						break;
					case 3:
						if (!flag3)
						{
							goto IL_25E;
						}
						break;
					}
					if ((shooterLoc + GenAdj.AdjacentCells[j]).GetCover(map) != null)
					{
						listToFill.Add(shooterLoc + GenAdj.AdjacentCells[j]);
					}
				}
				IL_25E:;
			}
		}

		// Token: 0x06005701 RID: 22273 RVA: 0x001BFBB0 File Offset: 0x001BDDB0
		public static void CalcShootableCellsOf(List<IntVec3> outCells, Thing target, IntVec3 shooterPos)
		{
			outCells.Clear();
			if (target is Pawn)
			{
				ShootLeanUtility.LeanShootingSourcesFromTo(target.Position, shooterPos, target.Map, outCells);
				return;
			}
			outCells.Add(target.Position);
			if (target.def.size.x == 1 && target.def.size.z == 1)
			{
				return;
			}
			foreach (IntVec3 intVec in target.OccupiedRect())
			{
				if (intVec != target.Position)
				{
					outCells.Add(intVec);
				}
			}
		}

		// Token: 0x06005702 RID: 22274 RVA: 0x001BFC6C File Offset: 0x001BDE6C
		public static bool CellCanSeeCell(IntVec3 source, IntVec3 dest, Map map)
		{
			bool result;
			try
			{
				if (!source.InBounds(map) || !dest.InBounds(map))
				{
					result = false;
				}
				else if (!source.CanBeSeenOver(map) || !dest.CanBeSeenOver(map))
				{
					result = false;
				}
				else
				{
					ShootLeanUtility.LeanShootingSourcesFromTo(dest, source, map, ShootLeanUtility.tempDestList);
					for (int i = 0; i < ShootLeanUtility.tempDestList.Count; i++)
					{
						if (GenSight.LineOfSight(source, dest, map, true, null, 0, 0))
						{
							return true;
						}
					}
					ShootLeanUtility.LeanShootingSourcesFromTo(source, dest, map, ShootLeanUtility.tempSourceList);
					for (int j = 0; j < ShootLeanUtility.tempSourceList.Count; j++)
					{
						for (int k = 0; k < ShootLeanUtility.tempDestList.Count; k++)
						{
							if (GenSight.LineOfSight(ShootLeanUtility.tempSourceList[j], ShootLeanUtility.tempDestList[k], map, true, null, 0, 0))
							{
								return true;
							}
						}
					}
					result = false;
				}
			}
			finally
			{
				ShootLeanUtility.tempSourceList.Clear();
				ShootLeanUtility.tempDestList.Clear();
			}
			return result;
		}

		// Token: 0x04003F51 RID: 16209
		private static readonly List<IntVec3> tempSourceList = new List<IntVec3>();

		// Token: 0x04003F52 RID: 16210
		private static readonly List<IntVec3> tempDestList = new List<IntVec3>();
	}
}
