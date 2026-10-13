using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000DA RID: 218
	public struct Rot4 : IEquatable<Rot4>
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x0001D4E8 File Offset: 0x0001B6E8
		public bool IsValid
		{
			get
			{
				return this.rotInt < 100;
			}
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0001D4F4 File Offset: 0x0001B6F4
		public static implicit operator Rot4(RotEnum rot)
		{
			if (rot == RotEnum.East)
			{
				return Rot4.East;
			}
			if (rot == RotEnum.South)
			{
				return Rot4.South;
			}
			if (rot == RotEnum.West)
			{
				return Rot4.West;
			}
			return Rot4.North;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001D519 File Offset: 0x0001B719
		public static implicit operator RotEnum(Rot4 rot)
		{
			if (rot == Rot4.East)
			{
				return RotEnum.East;
			}
			if (rot == Rot4.South)
			{
				return RotEnum.South;
			}
			if (rot == Rot4.West)
			{
				return RotEnum.West;
			}
			return RotEnum.North;
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x0001D549 File Offset: 0x0001B749
		// (set) Token: 0x060005D3 RID: 1491 RVA: 0x0001D551 File Offset: 0x0001B751
		public byte AsByte
		{
			get
			{
				return this.rotInt;
			}
			set
			{
				this.rotInt = (byte)GenMath.PositiveMod((int)value, 4);
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x0001D549 File Offset: 0x0001B749
		// (set) Token: 0x060005D5 RID: 1493 RVA: 0x0001D551 File Offset: 0x0001B751
		public int AsInt
		{
			get
			{
				return (int)this.rotInt;
			}
			set
			{
				this.rotInt = (byte)GenMath.PositiveMod(value, 4);
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060005D6 RID: 1494 RVA: 0x0001D564 File Offset: 0x0001B764
		public float AsAngle
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return 0f;
				case 1:
					return 90f;
				case 2:
					return 180f;
				case 3:
					return 270f;
				default:
					return 0f;
				}
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x0001D5B0 File Offset: 0x0001B7B0
		public SpectateRectSide AsSpectateSide
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return SpectateRectSide.Up;
				case 1:
					return SpectateRectSide.Right;
				case 2:
					return SpectateRectSide.Down;
				case 3:
					return SpectateRectSide.Left;
				default:
					return SpectateRectSide.None;
				}
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060005D8 RID: 1496 RVA: 0x0001D5E8 File Offset: 0x0001B7E8
		public Quaternion AsQuat
		{
			get
			{
				switch (this.rotInt)
				{
				case 0:
					return Quaternion.identity;
				case 1:
					return Quaternion.LookRotation(Vector3.right);
				case 2:
					return Quaternion.LookRotation(Vector3.back);
				case 3:
					return Quaternion.LookRotation(Vector3.left);
				default:
					Log.Error("ToQuat with Rot = " + this.AsInt.ToString());
					return Quaternion.identity;
				}
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x0001D660 File Offset: 0x0001B860
		public Vector2 AsVector2
		{
			get
			{
				switch (this.rotInt)
				{
				case 0:
					return Vector2.up;
				case 1:
					return Vector2.right;
				case 2:
					return Vector2.down;
				case 3:
					return Vector2.left;
				default:
					throw new Exception("rotInt's value cannot be >3 but it is:" + this.rotInt.ToString());
				}
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0001D6C0 File Offset: 0x0001B8C0
		public IntVec3 AsIntVec3
		{
			get
			{
				switch (this.rotInt)
				{
				case 0:
					return IntVec3.North;
				case 1:
					return IntVec3.East;
				case 2:
					return IntVec3.South;
				case 3:
					return IntVec3.West;
				default:
					throw new Exception("rotInt's value cannot be >3 but it is:" + this.rotInt.ToString());
				}
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x0001D71E File Offset: 0x0001B91E
		public bool IsHorizontal
		{
			get
			{
				return this.rotInt == 1 || this.rotInt == 3;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x0001D734 File Offset: 0x0001B934
		public bool IsVertical
		{
			get
			{
				return this.rotInt == 0 || this.rotInt == 2;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060005DD RID: 1501 RVA: 0x0001D749 File Offset: 0x0001B949
		public static Rot4 Random
		{
			get
			{
				return new Rot4(Rand.RangeInclusive(0, 3));
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x0001D757 File Offset: 0x0001B957
		public static IEnumerable<Rot4> AllRotations
		{
			get
			{
				yield return Rot4.North;
				yield return Rot4.East;
				yield return Rot4.South;
				yield return Rot4.West;
				yield break;
			}
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0001D551 File Offset: 0x0001B751
		public Rot4(byte newRot)
		{
			this.rotInt = (byte)GenMath.PositiveMod((int)newRot, 4);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0001D551 File Offset: 0x0001B751
		public Rot4(int newRot)
		{
			this.rotInt = (byte)GenMath.PositiveMod(newRot, 4);
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x0001D760 File Offset: 0x0001B960
		public IntVec3 FacingCell
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return new IntVec3(0, 0, 1);
				case 1:
					return new IntVec3(1, 0, 0);
				case 2:
					return new IntVec3(0, 0, -1);
				case 3:
					return new IntVec3(-1, 0, 0);
				default:
					return default(IntVec3);
				}
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x0001D7BC File Offset: 0x0001B9BC
		public IntVec3 RighthandCell
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return new IntVec3(1, 0, 0);
				case 1:
					return new IntVec3(0, 0, -1);
				case 2:
					return new IntVec3(-1, 0, 0);
				case 3:
					return new IntVec3(0, 0, 1);
				default:
					return default(IntVec3);
				}
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x0001D818 File Offset: 0x0001BA18
		public Rot4 Opposite
		{
			get
			{
				switch (this.AsInt)
				{
				case 0:
					return new Rot4(2);
				case 1:
					return new Rot4(3);
				case 2:
					return new Rot4(0);
				case 3:
					return new Rot4(1);
				default:
					return default(Rot4);
				}
			}
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0001D869 File Offset: 0x0001BA69
		public void Rotate(RotationDirection RotDir)
		{
			this.AsByte = (byte)(this.AsByte + RotDir);
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0001D87C File Offset: 0x0001BA7C
		public Rot4 Rotated(RotationDirection RotDir)
		{
			Rot4 result = this;
			result.Rotate(RotDir);
			return result;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0001D89C File Offset: 0x0001BA9C
		public static Rot4 FromAngleFlat(float angle)
		{
			angle = GenMath.PositiveMod(angle, 360f);
			if (angle < 45f)
			{
				return Rot4.North;
			}
			if (angle < 135f)
			{
				return Rot4.East;
			}
			if (angle < 225f)
			{
				return Rot4.South;
			}
			if (angle < 315f)
			{
				return Rot4.West;
			}
			return Rot4.North;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0001D8F4 File Offset: 0x0001BAF4
		public static Rot4 FromIntVec3(IntVec3 offset)
		{
			if (offset.x == 1)
			{
				return Rot4.East;
			}
			if (offset.x == -1)
			{
				return Rot4.West;
			}
			if (offset.z == 1)
			{
				return Rot4.North;
			}
			if (offset.z == -1)
			{
				return Rot4.South;
			}
			string str = "FromIntVec3 with bad offset ";
			IntVec3 intVec = offset;
			Log.Error(str + intVec.ToString());
			return Rot4.North;
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0001D960 File Offset: 0x0001BB60
		public static Rot4 FromIntVec2(IntVec2 offset)
		{
			return Rot4.FromIntVec3(offset.ToIntVec3);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0001D970 File Offset: 0x0001BB70
		public static RotationDirection GetRelativeRotation(Rot4 from, Rot4 to)
		{
			if (!from.IsValid || !to.IsValid)
			{
				Log.Error(string.Format("Both from ({0}) and to ({1}) must be valid.", from, to));
				return RotationDirection.None;
			}
			return (RotationDirection)GenMath.PositiveMod((int)(to.AsByte - from.AsByte), 4);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0001D9C2 File Offset: 0x0001BBC2
		public static bool operator ==(Rot4 a, Rot4 b)
		{
			return a.AsInt == b.AsInt;
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0001D9D4 File Offset: 0x0001BBD4
		public static bool operator !=(Rot4 a, Rot4 b)
		{
			return a.AsInt != b.AsInt;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0001D9EC File Offset: 0x0001BBEC
		public override int GetHashCode()
		{
			switch (this.rotInt)
			{
			case 0:
				return 235515;
			case 1:
				return 5612938;
			case 2:
				return 1215650;
			case 3:
				return 9231792;
			default:
				return (int)this.rotInt;
			}
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001DA36 File Offset: 0x0001BC36
		public override string ToString()
		{
			return this.rotInt.ToString();
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0001DA44 File Offset: 0x0001BC44
		public string ToStringHuman()
		{
			switch (this.rotInt)
			{
			case 0:
				return "North".Translate();
			case 1:
				return "East".Translate();
			case 2:
				return "South".Translate();
			case 3:
				return "West".Translate();
			default:
				return "error";
			}
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0001DAB8 File Offset: 0x0001BCB8
		public string ToStringWord()
		{
			switch (this.rotInt)
			{
			case 0:
				return "North";
			case 1:
				return "East";
			case 2:
				return "South";
			case 3:
				return "West";
			default:
				return "error";
			}
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0001DB04 File Offset: 0x0001BD04
		public static Rot4 FromString(string str)
		{
			int num;
			byte newRot;
			if (int.TryParse(str, out num))
			{
				newRot = (byte)num;
			}
			else if (!(str == "North"))
			{
				if (!(str == "East"))
				{
					if (!(str == "South"))
					{
						if (!(str == "West"))
						{
							newRot = 0;
							Log.Error("Invalid rotation: " + str);
						}
						else
						{
							newRot = 3;
						}
					}
					else
					{
						newRot = 2;
					}
				}
				else
				{
					newRot = 1;
				}
			}
			else
			{
				newRot = 0;
			}
			return new Rot4(newRot);
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0001DB7E File Offset: 0x0001BD7E
		public override bool Equals(object obj)
		{
			return obj is Rot4 && this.Equals((Rot4)obj);
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0001DB96 File Offset: 0x0001BD96
		public bool Equals(Rot4 other)
		{
			return this.rotInt == other.rotInt;
		}

		// Token: 0x04000307 RID: 775
		public const int NorthInt = 0;

		// Token: 0x04000308 RID: 776
		public const int EastInt = 1;

		// Token: 0x04000309 RID: 777
		public const int SouthInt = 2;

		// Token: 0x0400030A RID: 778
		public const int WestInt = 3;

		// Token: 0x0400030B RID: 779
		public const int RotationCount = 4;

		// Token: 0x0400030C RID: 780
		private byte rotInt;

		// Token: 0x0400030D RID: 781
		public static readonly Rot4 North = new Rot4(0);

		// Token: 0x0400030E RID: 782
		public static readonly Rot4 East = new Rot4(1);

		// Token: 0x0400030F RID: 783
		public static readonly Rot4 South = new Rot4(2);

		// Token: 0x04000310 RID: 784
		public static readonly Rot4 West = new Rot4(3);

		// Token: 0x04000311 RID: 785
		public static readonly Rot4 Invalid = new Rot4
		{
			rotInt = 200
		};
	}
}
