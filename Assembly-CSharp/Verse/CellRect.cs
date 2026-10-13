using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000B9 RID: 185
	public struct CellRect : IEquatable<CellRect>, IEnumerable<IntVec3>, IEnumerable
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x00017222 File Offset: 0x00015422
		public static CellRect Empty
		{
			get
			{
				return new CellRect(0, 0, 0, 0);
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x0001722D File Offset: 0x0001542D
		public readonly bool IsEmpty
		{
			get
			{
				return this.Width <= 0 || this.Height <= 0;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x00017246 File Offset: 0x00015446
		public readonly int Area
		{
			get
			{
				return this.Width * this.Height;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x00017255 File Offset: 0x00015455
		// (set) Token: 0x0600040D RID: 1037 RVA: 0x00017276 File Offset: 0x00015476
		public int Width
		{
			readonly get
			{
				if (this.minX > this.maxX)
				{
					return 0;
				}
				return this.maxX - this.minX + 1;
			}
			set
			{
				this.maxX = this.minX + Mathf.Max(value, 0) - 1;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x0001728E File Offset: 0x0001548E
		// (set) Token: 0x0600040F RID: 1039 RVA: 0x000172AF File Offset: 0x000154AF
		public int Height
		{
			readonly get
			{
				if (this.minZ > this.maxZ)
				{
					return 0;
				}
				return this.maxZ - this.minZ + 1;
			}
			set
			{
				this.maxZ = this.minZ + Mathf.Max(value, 0) - 1;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x000172C7 File Offset: 0x000154C7
		public IntVec2 Size
		{
			get
			{
				return new IntVec2(this.Width, this.Height);
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x000172DA File Offset: 0x000154DA
		public readonly IEnumerable<IntVec3> Corners
		{
			get
			{
				if (this.IsEmpty)
				{
					yield break;
				}
				yield return new IntVec3(this.minX, 0, this.minZ);
				if (this.Height > 1)
				{
					yield return new IntVec3(this.minX, 0, this.maxZ);
					if (this.Width > 1)
					{
						yield return new IntVec3(this.maxX, 0, this.maxZ);
					}
				}
				if (this.Width > 1)
				{
					yield return new IntVec3(this.maxX, 0, this.minZ);
				}
				yield break;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000412 RID: 1042 RVA: 0x000172EF File Offset: 0x000154EF
		// (set) Token: 0x06000413 RID: 1043 RVA: 0x00017303 File Offset: 0x00015503
		public IntVec3 Min
		{
			get
			{
				return new IntVec3(this.minX, 0, this.minZ);
			}
			set
			{
				this.minX = value.x;
				this.minZ = value.z;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x0001731D File Offset: 0x0001551D
		// (set) Token: 0x06000415 RID: 1045 RVA: 0x00017331 File Offset: 0x00015531
		public IntVec3 Max
		{
			get
			{
				return new IntVec3(this.maxX, 0, this.maxZ);
			}
			set
			{
				this.maxX = value.x;
				this.maxZ = value.z;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000416 RID: 1046 RVA: 0x0001734B File Offset: 0x0001554B
		public IntVec3 RandomCell
		{
			get
			{
				return new IntVec3(Rand.RangeInclusive(this.minX, this.maxX), 0, Rand.RangeInclusive(this.minZ, this.maxZ));
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x00017375 File Offset: 0x00015575
		public IntVec3 CenterCell
		{
			get
			{
				return new IntVec3(this.minX + this.Width / 2, 0, this.minZ + this.Height / 2);
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x0001739B File Offset: 0x0001559B
		public Vector3 CenterVector3
		{
			get
			{
				return new Vector3((float)this.minX + (float)this.Width / 2f, 0f, (float)this.minZ + (float)this.Height / 2f);
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x000173D1 File Offset: 0x000155D1
		public Vector3 RandomVector3
		{
			get
			{
				return new Vector3(Rand.Range((float)this.minX, (float)this.maxX + 1f), 0f, Rand.Range((float)this.minZ, (float)this.maxZ + 1f));
			}
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x0001740F File Offset: 0x0001560F
		public readonly bool AreSidesEqualOrGreater(int width, int height)
		{
			return this.Width >= width && this.Height >= height;
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00017428 File Offset: 0x00015628
		public readonly IEnumerable<IntVec3> Cells
		{
			get
			{
				int num;
				for (int z = this.minZ; z <= this.maxZ; z = num + 1)
				{
					for (int x = this.minX; x <= this.maxX; x = num + 1)
					{
						yield return new IntVec3(x, 0, z);
						num = x;
					}
					num = z;
				}
				yield break;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x0001743D File Offset: 0x0001563D
		public readonly IEnumerable<IntVec2> Cells2D
		{
			get
			{
				int num;
				for (int z = this.minZ; z <= this.maxZ; z = num + 1)
				{
					for (int x = this.minX; x <= this.maxX; x = num + 1)
					{
						yield return new IntVec2(x, z);
						num = x;
					}
					num = z;
				}
				yield break;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x00017452 File Offset: 0x00015652
		public readonly IEnumerable<IntVec3> EdgeCells
		{
			get
			{
				if (this.IsEmpty)
				{
					yield break;
				}
				int x = this.minX;
				int z = this.minZ;
				int num;
				while (x <= this.maxX)
				{
					yield return new IntVec3(x, 0, z);
					num = x;
					x = num + 1;
				}
				num = x;
				x = num - 1;
				num = z;
				for (z = num + 1; z <= this.maxZ; z = num + 1)
				{
					yield return new IntVec3(x, 0, z);
					num = z;
				}
				num = z;
				z = num - 1;
				num = x;
				for (x = num - 1; x >= this.minX; x = num - 1)
				{
					yield return new IntVec3(x, 0, z);
					num = x;
				}
				num = x;
				x = num + 1;
				num = z;
				for (z = num - 1; z > this.minZ; z = num - 1)
				{
					yield return new IntVec3(x, 0, z);
					num = z;
				}
				yield break;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x00017467 File Offset: 0x00015667
		public IEnumerable<IntVec3> EdgeCellsNoCorners
		{
			get
			{
				foreach (IntVec3 intVec in this.EdgeCells)
				{
					if (!this.IsCorner(intVec))
					{
						yield return intVec;
					}
				}
				IEnumerator<IntVec3> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x0001747C File Offset: 0x0001567C
		public int EdgeCellsCount
		{
			get
			{
				if (this.Area == 0)
				{
					return 0;
				}
				if (this.Area == 1)
				{
					return 1;
				}
				return this.Width * 2 + (this.Height - 2) * 2;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x000174A6 File Offset: 0x000156A6
		public IEnumerable<IntVec3> AdjacentCellsCardinal
		{
			get
			{
				if (this.IsEmpty)
				{
					yield break;
				}
				int num;
				for (int x = this.minX; x <= this.maxX; x = num + 1)
				{
					yield return new IntVec3(x, 0, this.minZ - 1);
					yield return new IntVec3(x, 0, this.maxZ + 1);
					num = x;
				}
				for (int x = this.minZ; x <= this.maxZ; x = num + 1)
				{
					yield return new IntVec3(this.minX - 1, 0, x);
					yield return new IntVec3(this.maxX + 1, 0, x);
					num = x;
				}
				yield break;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x000174BB File Offset: 0x000156BB
		public IEnumerable<IntVec3> AdjacentCells
		{
			get
			{
				if (this.IsEmpty)
				{
					yield break;
				}
				foreach (IntVec3 intVec in this.AdjacentCellsCardinal)
				{
					yield return intVec;
				}
				IEnumerator<IntVec3> enumerator = null;
				yield return new IntVec3(this.minX - 1, 0, this.minZ - 1);
				yield return new IntVec3(this.maxX + 1, 0, this.minZ - 1);
				yield return new IntVec3(this.minX - 1, 0, this.maxZ + 1);
				yield return new IntVec3(this.maxX + 1, 0, this.maxZ + 1);
				yield break;
				yield break;
			}
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x000174D0 File Offset: 0x000156D0
		public IntVec3 FarthestPoint(IntVec3 startingPoint, Rot4 direction)
		{
			if (!this.Contains(startingPoint))
			{
				throw new ArgumentException(startingPoint.ToString());
			}
			switch (direction.AsInt)
			{
			case 0:
				return new IntVec3(startingPoint.x, 0, this.maxZ);
			case 1:
				return new IntVec3(this.maxX, 0, startingPoint.z);
			case 2:
				return new IntVec3(startingPoint.x, 0, this.minZ);
			case 3:
				return new IntVec3(this.minX, 0, startingPoint.z);
			default:
				throw new ArgumentException(direction.ToString());
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00017577 File Offset: 0x00015777
		public static bool operator ==(CellRect lhs, CellRect rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00017581 File Offset: 0x00015781
		public static bool operator !=(CellRect lhs, CellRect rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0001758D File Offset: 0x0001578D
		public CellRect(int minX, int minZ, int width, int height)
		{
			this.minX = minX;
			this.minZ = minZ;
			this.maxX = minX + width - 1;
			this.maxZ = minZ + height - 1;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x000175B4 File Offset: 0x000157B4
		public static CellRect WholeMap(Map map)
		{
			return new CellRect(0, 0, map.Size.x, map.Size.z);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x000175D4 File Offset: 0x000157D4
		public static CellRect FromLimits(int minX, int minZ, int maxX, int maxZ)
		{
			return new CellRect
			{
				minX = Mathf.Min(minX, maxX),
				minZ = Mathf.Min(minZ, maxZ),
				maxX = Mathf.Max(maxX, minX),
				maxZ = Mathf.Max(maxZ, minZ)
			};
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00017624 File Offset: 0x00015824
		public static CellRect FromLimits(IntVec3 first, IntVec3 second)
		{
			return new CellRect
			{
				minX = Mathf.Min(first.x, second.x),
				minZ = Mathf.Min(first.z, second.z),
				maxX = Mathf.Max(first.x, second.x),
				maxZ = Mathf.Max(first.z, second.z)
			};
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0001769C File Offset: 0x0001589C
		public static CellRect FromCellList(IEnumerable<IntVec3> cells)
		{
			CellRect cellRect = new CellRect
			{
				minX = int.MaxValue,
				minZ = int.MaxValue
			};
			foreach (IntVec3 intVec in cells)
			{
				if (intVec.x < cellRect.minX)
				{
					cellRect.minX = intVec.x;
				}
				if (intVec.z < cellRect.minZ)
				{
					cellRect.minZ = intVec.z;
				}
				if (intVec.x > cellRect.maxX)
				{
					cellRect.maxX = intVec.x;
				}
				if (intVec.z > cellRect.maxZ)
				{
					cellRect.maxZ = intVec.z;
				}
			}
			return cellRect;
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0001776C File Offset: 0x0001596C
		public static CellRect CenteredOn(IntVec3 center, int radius)
		{
			return new CellRect
			{
				minX = center.x - radius,
				maxX = center.x + radius,
				minZ = center.z - radius,
				maxZ = center.z + radius
			};
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x000177C0 File Offset: 0x000159C0
		public static CellRect CenteredOn(IntVec3 center, int width, int height)
		{
			CellRect cellRect = new CellRect
			{
				minX = center.x - width / 2,
				minZ = center.z - height / 2
			};
			cellRect.maxX = cellRect.minX + width - 1;
			cellRect.maxZ = cellRect.minZ + height - 1;
			return cellRect;
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0001781C File Offset: 0x00015A1C
		public static CellRect CenteredOn(IntVec3 center, IntVec2 size)
		{
			return CellRect.CenteredOn(center, size.x, size.z);
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00017830 File Offset: 0x00015A30
		public static CellRect ViewRect(Map map)
		{
			if (Current.ProgramState != ProgramState.Playing || Find.CurrentMap != map || WorldRendererUtility.WorldSelected)
			{
				return CellRect.Empty;
			}
			return Find.CameraDriver.CurrentViewRect;
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00017859 File Offset: 0x00015A59
		public static CellRect SingleCell(IntVec3 c)
		{
			return new CellRect(c.x, c.z, 1, 1);
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0001786E File Offset: 0x00015A6E
		public bool InBounds(Map map)
		{
			return this.minX >= 0 && this.minZ >= 0 && this.maxX < map.Size.x && this.maxZ < map.Size.z;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x000178AC File Offset: 0x00015AAC
		public bool FullyContainedWithin(CellRect within)
		{
			CellRect rhs = this;
			rhs.ClipInsideRect(within);
			return this == rhs;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x000178D8 File Offset: 0x00015AD8
		public bool Overlaps(CellRect other)
		{
			return !this.IsEmpty && !other.IsEmpty && (this.minX <= other.maxX && this.maxX >= other.minX && this.maxZ >= other.minZ) && this.minZ <= other.maxZ;
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00017938 File Offset: 0x00015B38
		public bool OverlapsCardinal(CellRect other)
		{
			if (this.IsEmpty || other.IsEmpty)
			{
				return false;
			}
			CellRect other2 = this;
			other2.minZ++;
			other2.maxZ--;
			CellRect other3 = this;
			other3.minX++;
			other3.maxX--;
			return other.Overlaps(other2) || other.Overlaps(other3);
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x000179AC File Offset: 0x00015BAC
		public bool Overlaps(Bounds other)
		{
			return !this.IsEmpty && !(other.size == Vector3.zero) && ((float)this.minX <= other.max.x && (float)this.maxX >= other.min.x && (float)this.maxZ >= other.min.z) && (float)this.minZ <= other.max.z;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x00017A30 File Offset: 0x00015C30
		public Bounds ToBounds()
		{
			return new Bounds
			{
				center = this.CenterVector3,
				size = new Vector3((float)this.Width, 1f, (float)this.Height)
			};
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x00017A74 File Offset: 0x00015C74
		public bool IsOnEdge(IntVec3 c)
		{
			return (c.x == this.minX && c.z >= this.minZ && c.z <= this.maxZ) || (c.x == this.maxX && c.z >= this.minZ && c.z <= this.maxZ) || (c.z == this.minZ && c.x >= this.minX && c.x <= this.maxX) || (c.z == this.maxZ && c.x >= this.minX && c.x <= this.maxX);
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00017B34 File Offset: 0x00015D34
		public bool IsOnEdge(IntVec3 c, Rot4 rot)
		{
			if (rot == Rot4.West)
			{
				return c.x == this.minX && c.z >= this.minZ && c.z <= this.maxZ;
			}
			if (rot == Rot4.East)
			{
				return c.x == this.maxX && c.z >= this.minZ && c.z <= this.maxZ;
			}
			if (rot == Rot4.South)
			{
				return c.z == this.minZ && c.x >= this.minX && c.x <= this.maxX;
			}
			return c.z == this.maxZ && c.x >= this.minX && c.x <= this.maxX;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x00017C28 File Offset: 0x00015E28
		public bool IsOnEdge(IntVec3 c, int edgeWidth)
		{
			return this.Contains(c) && (c.x < this.minX + edgeWidth || c.z < this.minZ + edgeWidth || c.x >= this.maxX + 1 - edgeWidth || c.z >= this.maxZ + 1 - edgeWidth);
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x00017C8C File Offset: 0x00015E8C
		public bool IsCorner(IntVec3 c)
		{
			return (c.x == this.minX && c.z == this.minZ) || (c.x == this.maxX && c.z == this.minZ) || (c.x == this.minX && c.z == this.maxZ) || (c.x == this.maxX && c.z == this.maxZ);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00017D10 File Offset: 0x00015F10
		public IntVec3 ClosestCorner(IntVec3 cell)
		{
			IntVec3 corner = this.GetCorner(Rot4.North, true);
			IntVec3 corner2 = this.GetCorner(Rot4.East, true);
			IntVec3 corner3 = this.GetCorner(Rot4.South, true);
			IntVec3 corner4 = this.GetCorner(Rot4.West, true);
			IntVec3 intVec = cell - corner;
			IntVec3 intVec2 = cell - corner2;
			IntVec3 intVec3 = cell - corner3;
			IntVec3 intVec4 = cell - corner4;
			return GenMath.MinBy<IntVec3>(corner, intVec.SqrMagnitude, corner2, intVec2.SqrMagnitude, corner3, intVec3.SqrMagnitude, corner4, intVec4.SqrMagnitude);
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00017D9C File Offset: 0x00015F9C
		public Rot4 GetClosestEdge(IntVec3 c)
		{
			int num = Mathf.Abs(c.x - this.minX);
			int num2 = Mathf.Abs(c.x - this.maxX);
			int num3 = Mathf.Abs(c.z - this.maxZ);
			int num4 = Mathf.Abs(c.z - this.minZ);
			return GenMath.MinBy<Rot4>(Rot4.West, (float)num, Rot4.East, (float)num2, Rot4.North, (float)num3, Rot4.South, (float)num4);
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00017E18 File Offset: 0x00016018
		public Rot4 GetEdgeCellRot(IntVec3 c)
		{
			if (c.x == this.maxX && c.z >= this.minZ && c.z <= this.maxZ)
			{
				return Rot4.East;
			}
			if (c.x == this.minX && c.z >= this.minZ && c.z <= this.maxZ)
			{
				return Rot4.West;
			}
			if (c.z == this.maxZ && c.x >= this.minX && c.x <= this.maxX)
			{
				return Rot4.North;
			}
			if (c.z == this.minZ && c.x >= this.minX && c.x <= this.maxX)
			{
				return Rot4.South;
			}
			Log.Error(string.Format("Attempted to get edge cell rotation but cell was not on an edge, cell: {0}, rect: {1}", c, this.ToString()));
			return Rot4.North;
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00017F0B File Offset: 0x0001610B
		public bool IsDiagonalCorner(IntVec3 c)
		{
			return (c.x == this.minX || c.x == this.maxX) && (c.z == this.minZ || c.z == this.maxZ);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00017F49 File Offset: 0x00016149
		public IntVec3 GetCenterCellOnEdge(Rot4 rot)
		{
			return this.GetCellOnEdge(rot, this.CenterCell);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00017F58 File Offset: 0x00016158
		public IntVec3 GetCellOnEdge(Rot4 rot, IntVec3 point)
		{
			if (rot == Rot4.North)
			{
				return new IntVec3(point.x, point.y, this.maxZ);
			}
			if (rot == Rot4.East)
			{
				return new IntVec3(this.maxX, point.y, point.z);
			}
			if (rot == Rot4.South)
			{
				return new IntVec3(point.x, point.y, this.minZ);
			}
			if (rot == Rot4.West)
			{
				return new IntVec3(this.minX, point.y, point.z);
			}
			return IntVec3.Invalid;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00018000 File Offset: 0x00016200
		public IntVec3 GetCenterCellOnEdge(Rot4 rot, int offset)
		{
			if (rot == Rot4.North)
			{
				return new IntVec3(this.CenterCell.x + offset, this.CenterCell.y, this.maxZ);
			}
			if (rot == Rot4.East)
			{
				return new IntVec3(this.maxX, this.CenterCell.y, this.CenterCell.z + offset);
			}
			if (rot == Rot4.South)
			{
				return new IntVec3(this.CenterCell.x + offset, this.CenterCell.y, this.minZ);
			}
			if (rot == Rot4.West)
			{
				return new IntVec3(this.minX, this.CenterCell.y, this.CenterCell.z + offset);
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x000180D6 File Offset: 0x000162D6
		public IEnumerable<IntVec3> GetCenterCellsOnEdge(Rot4 rot, int range)
		{
			int num;
			for (int i = 0; i < range * 2 + 1; i = num + 1)
			{
				IntVec3 centerCellOnEdge = this.GetCenterCellOnEdge(rot, i - range);
				if (centerCellOnEdge.IsValid && this.Contains(centerCellOnEdge))
				{
					yield return centerCellOnEdge;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x000180F9 File Offset: 0x000162F9
		public IEnumerable<IntVec3> GetCellsCenterOutOnEdge(Rot4 rot)
		{
			return this.GetCenterCellsOnEdge(rot, this.GetSideLength(rot));
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00018109 File Offset: 0x00016309
		public int GetSideLength(Rot4 rot)
		{
			if (rot == Rot4.North || rot == Rot4.South)
			{
				return this.Width;
			}
			return this.Height;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00018132 File Offset: 0x00016332
		public IEnumerable<IntVec3> GetCellsOnEdge(Rot4 rot)
		{
			if (this.IsEmpty)
			{
				yield break;
			}
			if (rot == Rot4.North)
			{
				int num;
				for (int x = this.minX; x <= this.maxX; x = num + 1)
				{
					yield return new IntVec3(x, 0, this.maxZ);
					num = x;
				}
			}
			else if (rot == Rot4.East)
			{
				int num;
				for (int x = this.minZ; x <= this.maxZ; x = num + 1)
				{
					yield return new IntVec3(this.maxX, 0, x);
					num = x;
				}
			}
			else if (rot == Rot4.South)
			{
				int num;
				for (int x = this.minX; x <= this.maxX; x = num + 1)
				{
					yield return new IntVec3(x, 0, this.minZ);
					num = x;
				}
			}
			else if (rot == Rot4.West)
			{
				int num;
				for (int x = this.minZ; x <= this.maxZ; x = num + 1)
				{
					yield return new IntVec3(this.minX, 0, x);
					num = x;
				}
			}
			yield break;
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00018150 File Offset: 0x00016350
		public CellRect ClipInsideMap(Map map)
		{
			if (this.minX < 0)
			{
				this.minX = 0;
			}
			if (this.minZ < 0)
			{
				this.minZ = 0;
			}
			if (this.maxX > map.Size.x - 1)
			{
				this.maxX = map.Size.x - 1;
			}
			if (this.maxZ > map.Size.z - 1)
			{
				this.maxZ = map.Size.z - 1;
			}
			return this;
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x000181D4 File Offset: 0x000163D4
		public CellRect ClipInsideRect(CellRect otherRect)
		{
			if (this.minX < otherRect.minX)
			{
				this.minX = otherRect.minX;
			}
			if (this.maxX > otherRect.maxX)
			{
				this.maxX = otherRect.maxX;
			}
			if (this.minZ < otherRect.minZ)
			{
				this.minZ = otherRect.minZ;
			}
			if (this.maxZ > otherRect.maxZ)
			{
				this.maxZ = otherRect.maxZ;
			}
			return this;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x00018250 File Offset: 0x00016450
		public Rect ToScreenRect(Camera camera)
		{
			Vector3 vector = camera.WorldToScreenPoint(this.Min.ToVector3());
			Vector3 vector2 = camera.WorldToScreenPoint(this.Max.ToVector3() + new Vector3(1f, 0f, 1f));
			return Rect.MinMaxRect(vector.x, vector.y, vector2.x, vector2.y);
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x000182C0 File Offset: 0x000164C0
		[return: TupleElementNames(new string[]
		{
			"bottom",
			"up"
		})]
		public ValueTuple<CellRect, CellRect> SplitVertical(int separation = 0)
		{
			IntVec3 centerCell = this.CenterCell;
			CellRect item = this;
			CellRect item2 = this;
			item.maxZ = centerCell.z - separation;
			item2.minZ = centerCell.z + separation;
			return new ValueTuple<CellRect, CellRect>(item, item2);
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00018308 File Offset: 0x00016508
		[return: TupleElementNames(new string[]
		{
			"left",
			"right"
		})]
		public ValueTuple<CellRect, CellRect> SplitHorizontal(int separation = 0)
		{
			IntVec3 centerCell = this.CenterCell;
			CellRect item = this;
			CellRect item2 = this;
			item.maxX = centerCell.x - separation;
			item2.minX = centerCell.x + separation;
			return new ValueTuple<CellRect, CellRect>(item, item2);
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00018350 File Offset: 0x00016550
		[return: TupleElementNames(new string[]
		{
			"leftBottom",
			"leftUp",
			"rightBottom",
			"rightUp"
		})]
		public ValueTuple<CellRect, CellRect, CellRect, CellRect> Subdivide(int separation)
		{
			ValueTuple<CellRect, CellRect> valueTuple = this.SplitHorizontal(separation);
			CellRect item = valueTuple.Item1;
			CellRect item2 = valueTuple.Item2;
			ValueTuple<CellRect, CellRect> valueTuple2 = item.SplitVertical(separation);
			CellRect item3 = valueTuple2.Item1;
			CellRect item4 = valueTuple2.Item2;
			ValueTuple<CellRect, CellRect> valueTuple3 = item2.SplitVertical(separation);
			CellRect item5 = valueTuple3.Item1;
			CellRect item6 = valueTuple3.Item2;
			return new ValueTuple<CellRect, CellRect, CellRect, CellRect>(item3, item4, item5, item6);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x000183A8 File Offset: 0x000165A8
		public List<CellRect> Subdivide(int times, int separation)
		{
			List<CellRect> list = new List<CellRect>
			{
				this
			};
			for (int i = 0; i < times; i++)
			{
				int count = list.Count;
				for (int j = 0; j < count; j++)
				{
					ValueTuple<CellRect, CellRect, CellRect, CellRect> valueTuple = list[j].Subdivide(separation);
					CellRect item = valueTuple.Item1;
					CellRect item2 = valueTuple.Item2;
					CellRect item3 = valueTuple.Item3;
					CellRect item4 = valueTuple.Item4;
					list.Add(item);
					list.Add(item2);
					list.Add(item3);
					list.Add(item4);
				}
				list.RemoveRange(0, count);
			}
			return list;
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00018440 File Offset: 0x00016640
		public void SubdivideToMaxLength(int size, List<CellRect> rects)
		{
			rects.Clear();
			rects.Add(this);
			for (int i = 0; i < rects.Count; i++)
			{
				CellRect cellRect = rects[i];
				while (cellRect.Width > size)
				{
					rects.RemoveAt(i);
					ValueTuple<CellRect, CellRect> valueTuple = cellRect.SplitHorizontal(1);
					CellRect item = valueTuple.Item1;
					CellRect item2 = valueTuple.Item2;
					cellRect = item;
					rects.Add(item);
					rects.Add(item2);
				}
				while (cellRect.Height > size)
				{
					rects.RemoveAt(i);
					ValueTuple<CellRect, CellRect> valueTuple2 = cellRect.SplitVertical(1);
					CellRect item3 = valueTuple2.Item1;
					CellRect item4 = valueTuple2.Item2;
					cellRect = item3;
					rects.Add(item3);
					rects.Add(item4);
				}
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x000184F4 File Offset: 0x000166F4
		public bool TryContractToRemove(IntVec3 point, out CellRect rect)
		{
			rect = this;
			IntVec3 centerCell = this.CenterCell;
			if (!(point == centerCell))
			{
				if (point.x <= this.maxX && point.x >= this.minX)
				{
					if (point.x >= centerCell.x)
					{
						rect.maxX = point.x - 1;
					}
					else
					{
						rect.minX = point.x + 1;
					}
				}
				if (point.z <= this.maxZ && point.z >= this.minZ)
				{
					if (point.z >= centerCell.z)
					{
						rect.maxZ = point.z - 1;
					}
					else
					{
						rect.minZ = point.z + 1;
					}
				}
				return true;
			}
			if (this.Width < 3 && this.Height < 3)
			{
				rect = default(CellRect);
				return false;
			}
			if (this.Width > this.Height)
			{
				if (this.minX < point.x - 1)
				{
					rect.maxX = point.x - 1;
				}
				else
				{
					rect.minX = point.x + 1;
				}
			}
			else if (this.minZ < point.z - 1)
			{
				rect.maxZ = point.z - 1;
			}
			else
			{
				rect.minZ = point.z + 1;
			}
			return true;
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0001863C File Offset: 0x0001683C
		public readonly bool Contains(IntVec3 c)
		{
			return c.x >= this.minX && c.x <= this.maxX && c.z >= this.minZ && c.z <= this.maxZ;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0001867B File Offset: 0x0001687B
		public readonly bool InlineWith(CellRect other)
		{
			return (this.minX == other.minX && this.maxX == other.maxX) || (this.minZ == other.minZ && this.maxZ == other.maxZ);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x000186BC File Offset: 0x000168BC
		public IntVec3 GetCorner(Rot4 rot, bool clockwise = true)
		{
			if (rot == Rot4.North)
			{
				return new IntVec3(clockwise ? this.maxX : this.minX, 0, this.maxZ);
			}
			if (rot == Rot4.East)
			{
				return new IntVec3(clockwise ? this.maxX : this.minX, 0, this.minZ);
			}
			if (rot == Rot4.South)
			{
				return new IntVec3(clockwise ? this.minX : this.maxX, 0, this.minZ);
			}
			if (rot == Rot4.West)
			{
				return new IntVec3(clockwise ? this.minX : this.maxX, 0, this.maxZ);
			}
			return IntVec3.Invalid;
		}

		// Token: 0x06000450 RID: 1104 RVA: 0x0001877C File Offset: 0x0001697C
		public void GetInternalCorners(out IntVec3 BL, out IntVec3 TL, out IntVec3 TR, out IntVec3 BR)
		{
			BL = new IntVec3(this.minX, 0, this.minZ);
			TL = new IntVec3(this.minX, 0, this.maxZ);
			TR = new IntVec3(this.maxX, 0, this.maxZ);
			BR = new IntVec3(this.maxX, 0, this.minZ);
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x000187EC File Offset: 0x000169EC
		public void GetAdjacentCorners(out IntVec3 BL, out IntVec3 TL, out IntVec3 TR, out IntVec3 BR)
		{
			BL = new IntVec3(this.minX - 1, 0, this.minZ - 1);
			TL = new IntVec3(this.minX - 1, 0, this.maxZ + 1);
			TR = new IntVec3(this.maxX + 1, 0, this.maxZ + 1);
			BR = new IntVec3(this.maxX + 1, 0, this.minZ - 1);
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0001886C File Offset: 0x00016A6C
		public float ClosestDistSquaredTo(IntVec3 c)
		{
			if (this.Contains(c))
			{
				return 0f;
			}
			if (c.x < this.minX)
			{
				if (c.z < this.minZ)
				{
					return (float)(c - new IntVec3(this.minX, 0, this.minZ)).LengthHorizontalSquared;
				}
				if (c.z > this.maxZ)
				{
					return (float)(c - new IntVec3(this.minX, 0, this.maxZ)).LengthHorizontalSquared;
				}
				return (float)((this.minX - c.x) * (this.minX - c.x));
			}
			else if (c.x > this.maxX)
			{
				if (c.z < this.minZ)
				{
					return (float)(c - new IntVec3(this.maxX, 0, this.minZ)).LengthHorizontalSquared;
				}
				if (c.z > this.maxZ)
				{
					return (float)(c - new IntVec3(this.maxX, 0, this.maxZ)).LengthHorizontalSquared;
				}
				return (float)((c.x - this.maxX) * (c.x - this.maxX));
			}
			else
			{
				if (c.z < this.minZ)
				{
					return (float)((this.minZ - c.z) * (this.minZ - c.z));
				}
				return (float)((c.z - this.maxZ) * (c.z - this.maxZ));
			}
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x000189E5 File Offset: 0x00016BE5
		public float ClosestDistanceTo(IntVec3 c)
		{
			return this.ClosestCellTo(c).DistanceTo(c);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x000189F4 File Offset: 0x00016BF4
		public float DistanceToEdge(IntVec3 v)
		{
			return (float)Mathf.Max(Mathf.Min(Mathf.Min(Mathf.Min(v.x, v.z), this.Width - v.x - 1), this.Height - v.z - 1), 0);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00018A44 File Offset: 0x00016C44
		public IntVec3 ClosestCellTo(IntVec3 c)
		{
			if (this.Contains(c))
			{
				return c;
			}
			if (c.x < this.minX)
			{
				if (c.z < this.minZ)
				{
					return new IntVec3(this.minX, 0, this.minZ);
				}
				if (c.z > this.maxZ)
				{
					return new IntVec3(this.minX, 0, this.maxZ);
				}
				return new IntVec3(this.minX, 0, c.z);
			}
			else if (c.x > this.maxX)
			{
				if (c.z < this.minZ)
				{
					return new IntVec3(this.maxX, 0, this.minZ);
				}
				if (c.z > this.maxZ)
				{
					return new IntVec3(this.maxX, 0, this.maxZ);
				}
				return new IntVec3(this.maxX, 0, c.z);
			}
			else
			{
				if (c.z < this.minZ)
				{
					return new IntVec3(c.x, 0, this.minZ);
				}
				return new IntVec3(c.x, 0, this.maxZ);
			}
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00018B58 File Offset: 0x00016D58
		public bool InNoBuildEdgeArea(Map map)
		{
			return !this.IsEmpty && (this.minX < 10 || this.minZ < 10 || this.maxX >= map.Size.x - 10 || this.maxZ >= map.Size.z - 10);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x00018BB4 File Offset: 0x00016DB4
		public IEnumerable<IntVec3> GetEdgeCells(Rot4 dir)
		{
			if (dir == Rot4.North)
			{
				int num;
				for (int x = this.minX; x <= this.maxX; x = num + 1)
				{
					yield return new IntVec3(x, 0, this.maxZ);
					num = x;
				}
			}
			else if (dir == Rot4.South)
			{
				int num;
				for (int x = this.minX; x <= this.maxX; x = num + 1)
				{
					yield return new IntVec3(x, 0, this.minZ);
					num = x;
				}
			}
			else if (dir == Rot4.West)
			{
				int num;
				for (int x = this.minZ; x <= this.maxZ; x = num + 1)
				{
					yield return new IntVec3(this.minX, 0, x);
					num = x;
				}
			}
			else if (dir == Rot4.East)
			{
				int num;
				for (int x = this.minZ; x <= this.maxZ; x = num + 1)
				{
					yield return new IntVec3(this.maxX, 0, x);
					num = x;
				}
			}
			yield break;
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00018BD0 File Offset: 0x00016DD0
		public readonly CellRect GetEdgeRect(Rot4 rot)
		{
			if (rot == Rot4.North)
			{
				return CellRect.FromLimits(new IntVec3(this.minX, 0, this.maxZ), new IntVec3(this.maxX, 0, this.maxZ));
			}
			if (rot == Rot4.South)
			{
				return CellRect.FromLimits(new IntVec3(this.minX, 0, this.minZ), new IntVec3(this.maxX, 0, this.minZ));
			}
			if (rot == Rot4.West)
			{
				return CellRect.FromLimits(new IntVec3(this.minX, 0, this.minZ), new IntVec3(this.minX, 0, this.maxZ));
			}
			return CellRect.FromLimits(new IntVec3(this.maxX, 0, this.minZ), new IntVec3(this.maxX, 0, this.maxZ));
		}

		// Token: 0x06000459 RID: 1113 RVA: 0x00018CAC File Offset: 0x00016EAC
		public bool TryFindRandomInnerRectTouchingEdge(IntVec2 size, out CellRect rect, Predicate<CellRect> predicate = null)
		{
			if (this.Width < size.x || this.Height < size.z)
			{
				rect = CellRect.Empty;
				return false;
			}
			if (size.x <= 0 || size.z <= 0 || this.IsEmpty)
			{
				rect = CellRect.Empty;
				return false;
			}
			CellRect cellRect = this;
			cellRect.maxX -= size.x - 1;
			cellRect.maxZ -= size.z - 1;
			IntVec3 intVec;
			if (cellRect.EdgeCells.Where(delegate(IntVec3 x)
			{
				if (predicate == null)
				{
					return true;
				}
				CellRect obj = new CellRect(x.x, x.z, size.x, size.z);
				return predicate(obj);
			}).TryRandomElement(out intVec))
			{
				rect = new CellRect(intVec.x, intVec.z, size.x, size.z);
				return true;
			}
			rect = CellRect.Empty;
			return false;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x00018DC4 File Offset: 0x00016FC4
		public readonly bool TryFindRandomInnerRectTouchingCorner(IntVec2 size, out CellRect rect, Predicate<CellRect> predicate = null)
		{
			if (this.Width < size.x || this.Height < size.z)
			{
				rect = CellRect.Empty;
				return false;
			}
			if (size.x <= 0 || size.z <= 0 || this.IsEmpty)
			{
				rect = CellRect.Empty;
				return false;
			}
			CellRect cellRect = this;
			cellRect.maxX -= size.x - 1;
			cellRect.maxZ -= size.z - 1;
			IntVec3 intVec;
			if (cellRect.Corners.Where(delegate(IntVec3 x)
			{
				if (predicate == null)
				{
					return true;
				}
				CellRect obj = new CellRect(x.x, x.z, size.x, size.z);
				return predicate(obj);
			}).TryRandomElement(out intVec))
			{
				rect = new CellRect(intVec.x, intVec.z, size.x, size.z);
				return true;
			}
			rect = CellRect.Empty;
			return false;
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00018EDC File Offset: 0x000170DC
		public readonly bool TryFindRandomCell(out IntVec3 cell, Predicate<IntVec3> validator = null)
		{
			foreach (IntVec3 intVec in this.Cells.InRandomOrder(null))
			{
				if (validator == null || validator(intVec))
				{
					cell = intVec;
					return true;
				}
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x00018F50 File Offset: 0x00017150
		public readonly bool TryFindRandomInnerRect(IntVec2 size, out CellRect rect, Predicate<CellRect> validator = null)
		{
			if (this.Width < size.x || this.Height < size.z)
			{
				rect = CellRect.Empty;
				return false;
			}
			if (size.x <= 0 || size.z <= 0 || this.IsEmpty)
			{
				rect = CellRect.Empty;
				return false;
			}
			CellRect cellRect = this;
			cellRect.maxX -= size.x - 1;
			cellRect.maxZ -= size.z - 1;
			IntVec3 intVec;
			if (cellRect.Cells.Where(delegate(IntVec3 x)
			{
				if (validator == null)
				{
					return true;
				}
				CellRect obj = new CellRect(x.x, x.z, size.x, size.z);
				return validator(obj);
			}).TryRandomElement(out intVec))
			{
				rect = new CellRect(intVec.x, intVec.z, size.x, size.z);
				return true;
			}
			rect = CellRect.Empty;
			return false;
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00019068 File Offset: 0x00017268
		public readonly bool TryFindNearestInnerRectTo(IntVec2 size, IntVec3 position, out CellRect rect)
		{
			if (this.Width < size.x || this.Height < size.z)
			{
				rect = CellRect.Empty;
				return false;
			}
			if (size.x <= 0 || size.z <= 0 || this.IsEmpty)
			{
				rect = CellRect.Empty;
				return false;
			}
			CellRect cellRect = this;
			cellRect.maxX -= size.x - 1;
			cellRect.maxZ -= size.z - 1;
			CellRect cellRect2 = CellRect.Empty;
			float num = float.MaxValue;
			foreach (IntVec3 intVec in cellRect.Cells)
			{
				CellRect cellRect3 = new CellRect(intVec.x, intVec.z, size.x, size.z);
				int num2 = position.DistanceToSquared(cellRect3.CenterCell);
				if ((float)num2 < num)
				{
					cellRect2 = cellRect3;
					num = (float)num2;
				}
			}
			rect = cellRect2;
			return true;
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00019180 File Offset: 0x00017380
		public readonly CellRect ExpandedBy(int dist)
		{
			CellRect result = this;
			result.minX -= dist;
			result.minZ -= dist;
			result.maxX += dist;
			result.maxZ += dist;
			return result;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x000191C8 File Offset: 0x000173C8
		public readonly CellRect ExpandedBy(int x, int z)
		{
			CellRect result = this;
			result.minX -= x;
			result.minZ -= z;
			result.maxX += x;
			result.maxZ += z;
			return result;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00019210 File Offset: 0x00017410
		public readonly CellRect ExpandedBy(IntVec3 offset)
		{
			CellRect result = this;
			result.minX -= offset.x;
			result.minZ -= offset.z;
			result.maxX += offset.x;
			result.maxZ += offset.z;
			return result;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x0001926C File Offset: 0x0001746C
		public readonly CellRect MaxExpandedBy(int dist)
		{
			CellRect result = this;
			result.maxX += dist;
			result.maxZ += dist;
			return result;
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001929C File Offset: 0x0001749C
		public readonly CellRect MaxExpandedBy(int x, int z)
		{
			CellRect result = this;
			result.maxX += x;
			result.maxZ += z;
			return result;
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x000192CC File Offset: 0x000174CC
		public readonly CellRect MaxExpandedBy(IntVec3 offset)
		{
			CellRect result = this;
			result.maxX += offset.x;
			result.maxZ += offset.z;
			return result;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00019304 File Offset: 0x00017504
		public readonly CellRect MinExpandedBy(int dist)
		{
			CellRect result = this;
			result.minX -= dist;
			result.minZ -= dist;
			return result;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00019334 File Offset: 0x00017534
		public readonly CellRect MinExpandedBy(int x, int z)
		{
			CellRect result = this;
			result.minX -= x;
			result.minZ -= z;
			return result;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00019364 File Offset: 0x00017564
		public readonly CellRect MinExpandedBy(IntVec3 offset)
		{
			CellRect result = this;
			result.minX -= offset.x;
			result.minZ -= offset.z;
			return result;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0001939B File Offset: 0x0001759B
		public readonly CellRect ContractedBy(int x, int z)
		{
			return this.ExpandedBy(new IntVec3(-x, 0, -z));
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x000193AD File Offset: 0x000175AD
		public readonly CellRect ContractedBy(IntVec3 dist)
		{
			return this.ExpandedBy(-dist);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x000193BB File Offset: 0x000175BB
		public readonly CellRect ContractedBy(int dist)
		{
			return this.ExpandedBy(-dist);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x000193C8 File Offset: 0x000175C8
		public CellRect ContractedBy(int x, int z, int negativeX, int negativeZ)
		{
			CellRect result = this;
			result.minX += x;
			result.minZ += z;
			result.maxX -= negativeX;
			result.maxZ -= negativeZ;
			return result;
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0001940E File Offset: 0x0001760E
		public CellRect MovedBy(int x, int z)
		{
			return this.MovedBy(new IntVec3(x, 0, z));
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0001941E File Offset: 0x0001761E
		public CellRect MovedBy(IntVec2 offset)
		{
			return this.MovedBy(offset.ToIntVec3);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00019430 File Offset: 0x00017630
		public CellRect MovedBy(IntVec3 offset)
		{
			CellRect result = this;
			result.minX += offset.x;
			result.minZ += offset.z;
			result.maxX += offset.x;
			result.maxZ += offset.z;
			return result;
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0001948C File Offset: 0x0001768C
		public CellRect Encapsulate(CellRect otherRect)
		{
			CellRect result = this;
			result.minX = Mathf.Min(this.minX, otherRect.minX);
			result.minZ = Mathf.Min(this.minZ, otherRect.minZ);
			result.maxX = Mathf.Max(this.maxX, otherRect.maxX);
			result.maxZ = Mathf.Max(this.maxZ, otherRect.maxZ);
			return result;
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00019504 File Offset: 0x00017704
		public CellRect Encapsulate(IntVec3 point)
		{
			CellRect result = this;
			result.minX = Mathf.Min(this.minX, point.x);
			result.minZ = Mathf.Min(this.minZ, point.z);
			result.maxX = Mathf.Max(this.maxX, point.x);
			result.maxZ = Mathf.Max(this.maxZ, point.z);
			return result;
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00019579 File Offset: 0x00017779
		public IEnumerable<IntVec3> UnionCells(CellRect container)
		{
			int num;
			for (int z = container.minZ; z <= container.maxZ; z = num + 1)
			{
				for (int x = container.minX; x <= container.maxX; x = num + 1)
				{
					IntVec3 intVec = new IntVec3(x, 0, z);
					if (this.Contains(intVec))
					{
						yield return intVec;
					}
					num = x;
				}
				num = z;
			}
			yield break;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00019595 File Offset: 0x00017795
		public IEnumerable<IntVec3> DifferenceCells(CellRect container)
		{
			int num;
			for (int z = container.minZ; z <= container.maxZ; z = num + 1)
			{
				for (int x = container.minX; x <= container.maxX; x = num + 1)
				{
					IntVec3 intVec = new IntVec3(x, 0, z);
					if (!this.Contains(intVec))
					{
						yield return intVec;
					}
					num = x;
				}
				num = z;
			}
			yield break;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x000195B1 File Offset: 0x000177B1
		public int IndexOf(IntVec3 location)
		{
			return location.x - this.minX + (location.z - this.minZ) * this.Width;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x000195D8 File Offset: 0x000177D8
		public void DebugDraw()
		{
			float y = AltitudeLayer.MetaOverlays.AltitudeFor();
			Vector3 vector = new Vector3((float)this.minX, y, (float)this.minZ);
			Vector3 vector2 = new Vector3((float)this.minX, y, (float)(this.maxZ + 1));
			Vector3 vector3 = new Vector3((float)(this.maxX + 1), y, (float)(this.maxZ + 1));
			Vector3 vector4 = new Vector3((float)(this.maxX + 1), y, (float)this.minZ);
			GenDraw.DrawLineBetween(vector, vector2);
			GenDraw.DrawLineBetween(vector2, vector3);
			GenDraw.DrawLineBetween(vector3, vector4);
			GenDraw.DrawLineBetween(vector4, vector);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0001966B File Offset: 0x0001786B
		public CellRect.Enumerator GetEnumerator()
		{
			return new CellRect.Enumerator(this);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00019678 File Offset: 0x00017878
		IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00019678 File Offset: 0x00017878
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00019688 File Offset: 0x00017888
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.minX.ToString(),
				",",
				this.minZ.ToString(),
				",",
				this.maxX.ToString(),
				",",
				this.maxZ.ToString(),
				")"
			});
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00019701 File Offset: 0x00017901
		public void Reset()
		{
			this = default(CellRect);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0001970C File Offset: 0x0001790C
		public static CellRect FromString(string str)
		{
			str = str.TrimStart('(');
			str = str.TrimEnd(')');
			string[] array = str.Split(',', StringSplitOptions.None);
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			int num = Convert.ToInt32(array[0], invariantCulture);
			int num2 = Convert.ToInt32(array[1], invariantCulture);
			int num3 = Convert.ToInt32(array[2], invariantCulture);
			int num4 = Convert.ToInt32(array[3], invariantCulture);
			return new CellRect(num, num2, num3 - num + 1, num4 - num2 + 1);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00019776 File Offset: 0x00017976
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(Gen.HashCombineInt(0, this.minX), this.maxX), this.minZ), this.maxZ);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x000197A8 File Offset: 0x000179A8
		public override bool Equals(object obj)
		{
			if (obj is CellRect)
			{
				CellRect other = (CellRect)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x000197CF File Offset: 0x000179CF
		public bool Equals(CellRect other)
		{
			return this.minX == other.minX && this.maxX == other.maxX && this.minZ == other.minZ && this.maxZ == other.maxZ;
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00017859 File Offset: 0x00015A59
		public static CellRect FromCell(IntVec3 cell)
		{
			return new CellRect(cell.x, cell.z, 1, 1);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0001980B File Offset: 0x00017A0B
		public readonly IEnumerable<CellRect> EnumerateRectanglesCovering(Predicate<IntVec3> validator)
		{
			CellRect.tmpUnvistedCells.Clear();
			using (IEnumerator<IntVec3> enumerator = this.Cells.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IntVec3 intVec = enumerator.Current;
					if (validator(intVec))
					{
						CellRect.tmpUnvistedCells.Add(intVec);
					}
				}
				goto IL_171;
			}
			IL_6F:
			IntVec3 intVec2 = CellRect.tmpUnvistedCells.First<IntVec3>();
			CellRect.tmpUnvistedCells.Remove(intVec2);
			int num = intVec2.x;
			int num2 = intVec2.z;
			while (CellRect.tmpUnvistedCells.Contains(new IntVec3(num + 1, 0, num2)))
			{
				num++;
			}
			for (;;)
			{
				bool flag = false;
				for (int i = intVec2.x; i <= num; i++)
				{
					if (!CellRect.tmpUnvistedCells.Contains(new IntVec3(i, 0, num2 + 1)))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
				num2++;
			}
			CellRect cellRect = new CellRect(intVec2.x, intVec2.z, num - intVec2.x + 1, num2 - intVec2.z + 1);
			foreach (IntVec3 item in cellRect.Cells)
			{
				CellRect.tmpUnvistedCells.Remove(item);
			}
			yield return cellRect;
			IL_171:
			if (CellRect.tmpUnvistedCells.Count <= 0)
			{
				yield break;
			}
			goto IL_6F;
		}

		// Token: 0x04000273 RID: 627
		public int minX;

		// Token: 0x04000274 RID: 628
		public int maxX;

		// Token: 0x04000275 RID: 629
		public int minZ;

		// Token: 0x04000276 RID: 630
		public int maxZ;

		// Token: 0x04000277 RID: 631
		private static readonly HashSet<IntVec3> tmpUnvistedCells = new HashSet<IntVec3>();

		// Token: 0x020000BA RID: 186
		public struct Enumerator : IEnumerator<IntVec3>, IEnumerator, IDisposable
		{
			// Token: 0x170000C2 RID: 194
			// (get) Token: 0x06000480 RID: 1152 RVA: 0x00019833 File Offset: 0x00017A33
			public IntVec3 Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			// Token: 0x170000C3 RID: 195
			// (get) Token: 0x06000481 RID: 1153 RVA: 0x00019847 File Offset: 0x00017A47
			object IEnumerator.Current
			{
				get
				{
					return new IntVec3(this.x, 0, this.z);
				}
			}

			// Token: 0x06000482 RID: 1154 RVA: 0x00019860 File Offset: 0x00017A60
			public Enumerator(CellRect ir)
			{
				this.ir = ir;
				this.x = ir.minX - 1;
				this.z = ir.minZ;
			}

			// Token: 0x06000483 RID: 1155 RVA: 0x00019884 File Offset: 0x00017A84
			public bool MoveNext()
			{
				this.x++;
				if (this.x > this.ir.maxX)
				{
					this.x = this.ir.minX;
					this.z++;
				}
				return this.z <= this.ir.maxZ;
			}

			// Token: 0x06000484 RID: 1156 RVA: 0x000198E7 File Offset: 0x00017AE7
			public void Reset()
			{
				this.x = this.ir.minX - 1;
				this.z = this.ir.minZ;
			}

			// Token: 0x06000485 RID: 1157 RVA: 0x000026EA File Offset: 0x000008EA
			void IDisposable.Dispose()
			{
			}

			// Token: 0x04000278 RID: 632
			private CellRect ir;

			// Token: 0x04000279 RID: 633
			private int x;

			// Token: 0x0400027A RID: 634
			private int z;
		}
	}
}
