using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D5B RID: 3419
	public struct ShootLine
	{
		// Token: 0x17000DA4 RID: 3492
		// (get) Token: 0x06005704 RID: 22276 RVA: 0x001BFD82 File Offset: 0x001BDF82
		public IntVec3 Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000DA5 RID: 3493
		// (get) Token: 0x06005705 RID: 22277 RVA: 0x001BFD8A File Offset: 0x001BDF8A
		public IntVec3 Dest
		{
			get
			{
				return this.dest;
			}
		}

		// Token: 0x06005706 RID: 22278 RVA: 0x001BFD92 File Offset: 0x001BDF92
		public ShootLine(IntVec3 source, IntVec3 dest)
		{
			this.source = source;
			this.dest = dest;
		}

		// Token: 0x06005707 RID: 22279 RVA: 0x001BFDA4 File Offset: 0x001BDFA4
		public void ChangeDestToMissWild(float aimOnChance, bool flyOverhead, Map map)
		{
			float num = ShootTuning.MissDistanceFromAimOnChanceCurves.Evaluate(aimOnChance, Rand.Value);
			if (num < 0f)
			{
				Log.ErrorOnce("Attempted to wild-miss less than zero tiles away", 94302089);
			}
			IntVec3 a;
			do
			{
				Vector2 unitVector = Rand.UnitVector2;
				Vector3 b = new Vector3(unitVector.x * num, 0f, unitVector.y * num);
				a = (this.dest.ToVector3Shifted() + b).ToIntVec3();
			}
			while (Vector3.Dot((this.dest - this.source).ToVector3(), (a - this.source).ToVector3()) < 0f);
			this.dest = a;
			if (!flyOverhead && map != null && !ShootLeanUtility.CellCanSeeCell(this.source, this.dest, map))
			{
				foreach (IntVec3 intVec in this.Points())
				{
					this.dest = intVec;
					if (intVec != this.source && intVec.InBounds(map) && intVec.Filled(map) && !this.source.InHorDistOf(this.dest, 1.5f))
					{
						Building_Door door = intVec.GetDoor(map);
						if (door == null)
						{
							break;
						}
						if (!door.Open)
						{
							break;
						}
					}
				}
			}
		}

		// Token: 0x06005708 RID: 22280 RVA: 0x001BFF14 File Offset: 0x001BE114
		public IEnumerable<IntVec3> Points()
		{
			return GenSight.PointsOnLineOfSight(this.source, this.dest);
		}

		// Token: 0x06005709 RID: 22281 RVA: 0x001BFF28 File Offset: 0x001BE128
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "(";
			int num = 1;
			IntVec3 intVec = this.source;
			array[num] = intVec.ToString();
			array[2] = "->";
			int num2 = 3;
			intVec = this.dest;
			array[num2] = intVec.ToString();
			array[4] = ")";
			return string.Concat(array);
		}

		// Token: 0x0600570A RID: 22282 RVA: 0x001BFF88 File Offset: 0x001BE188
		[DebugOutput]
		public static void WildMissResults()
		{
			IntVec3 intVec = new IntVec3(100, 0, 0);
			ShootLine shootLine = new ShootLine(IntVec3.Zero, intVec);
			IEnumerable<int> enumerable = Enumerable.Range(0, 101);
			IEnumerable<int> colValues = Enumerable.Range(0, 12);
			int[,] results = new int[enumerable.Count<int>(), colValues.Count<int>()];
			foreach (int num in enumerable)
			{
				for (int i = 0; i < 10000; i++)
				{
					ShootLine shootLine2 = shootLine;
					shootLine2.ChangeDestToMissWild((float)num / 100f, false, null);
					if (shootLine2.dest.z == 0 && shootLine2.dest.x > intVec.x)
					{
						results[num, shootLine2.dest.x - intVec.x]++;
					}
				}
			}
			DebugTables.MakeTablesDialog<int, int>(colValues, (int cells) => cells.ToString() + "-away\ncell\nhit%", enumerable, (int hitchance) => ((float)hitchance / 100f).ToStringPercent() + " aimon chance", delegate(int cells, int hitchance)
			{
				float num2 = (float)hitchance / 100f;
				if (cells == 0)
				{
					return num2.ToStringPercent();
				}
				return ((float)results[hitchance, cells] / 10000f * (1f - num2)).ToStringPercent();
			}, "");
		}

		// Token: 0x04003F53 RID: 16211
		private IntVec3 source;

		// Token: 0x04003F54 RID: 16212
		private IntVec3 dest;
	}
}
