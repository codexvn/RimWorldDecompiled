using System;
using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Verse
{
	// Token: 0x020005AF RID: 1455
	public static class CellConnectionExtensions
	{
		// Token: 0x06002594 RID: 9620 RVA: 0x000C5694 File Offset: 0x000C3894
		public static IntVec3 Offset(this CellConnection connection)
		{
			if (connection == CellConnection.Self)
			{
				return IntVec3.Zero;
			}
			if (math.countbits((int)connection) != 1)
			{
				throw new ArgumentException("connection");
			}
			return CellConnectionExtensions.OffsetFromBitIndex[connection.BitIndex()];
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x000C56C4 File Offset: 0x000C38C4
		public static CellConnection Inverse(this CellConnection connection)
		{
			if (connection <= CellConnection.NorthEast)
			{
				switch (connection)
				{
				case CellConnection.North:
					return CellConnection.South;
				case CellConnection.South:
					return CellConnection.North;
				case CellConnection.North | CellConnection.South:
					break;
				case CellConnection.East:
					return CellConnection.West;
				default:
					if (connection == CellConnection.West)
					{
						return CellConnection.East;
					}
					if (connection == CellConnection.NorthEast)
					{
						return CellConnection.SouthWest;
					}
					break;
				}
			}
			else
			{
				if (connection == CellConnection.SouthEast)
				{
					return CellConnection.NorthWest;
				}
				if (connection == CellConnection.SouthWest)
				{
					return CellConnection.NorthEast;
				}
				if (connection == CellConnection.NorthWest)
				{
					return CellConnection.SouthEast;
				}
			}
			throw new ArgumentException("connection");
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x000C572E File Offset: 0x000C392E
		public static CellConnectionEnumerator GetEnumerator(this CellConnection connections)
		{
			return new CellConnectionEnumerator(connections);
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x000C5738 File Offset: 0x000C3938
		public static CellConnection? ConnectionFromCells(IntVec3 first, IntVec3 second)
		{
			IntVec3 b = second - first;
			for (int i = 0; i < CellConnectionExtensions.OffsetFromBitIndex.Length; i++)
			{
				if (CellConnectionExtensions.OffsetFromBitIndex[i] == b)
				{
					return new CellConnection?(CellConnectionExtensions.FlagFromBitIndex[i]);
				}
			}
			return null;
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x000C5788 File Offset: 0x000C3988
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Diagonal(this CellConnection connection)
		{
			return (connection & CellConnection.DiagonalNeighbors) > CellConnection.North;
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x000C5794 File Offset: 0x000C3994
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Cardinal(this CellConnection connection)
		{
			return (connection & CellConnection.CardinalNeighbors) > CellConnection.North;
		}

		// Token: 0x0600259A RID: 9626 RVA: 0x000C579D File Offset: 0x000C399D
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int BitIndex(this CellConnection connection)
		{
			return math.tzcnt((uint)connection);
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x000C57A5 File Offset: 0x000C39A5
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int BitLoopEndIndex(this CellConnection connections)
		{
			return 8 - (math.lzcnt((uint)connections) - 8) % 8;
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x000C57B3 File Offset: 0x000C39B3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool HasBit(this CellConnection connections, CellConnection testFlag)
		{
			return (connections & testFlag) > CellConnection.Self;
		}

		// Token: 0x04001EE5 RID: 7909
		public static readonly IntVec3[] OffsetFromBitIndex = new IntVec3[]
		{
			new IntVec3(0, 0, 1),
			new IntVec3(0, 0, -1),
			new IntVec3(1, 0, 0),
			new IntVec3(-1, 0, 0),
			new IntVec3(1, 0, 1),
			new IntVec3(1, 0, -1),
			new IntVec3(-1, 0, -1),
			new IntVec3(-1, 0, 1)
		};

		// Token: 0x04001EE6 RID: 7910
		public static readonly CellConnection[] FlagFromBitIndex = new CellConnection[]
		{
			CellConnection.North,
			CellConnection.South,
			CellConnection.East,
			CellConnection.West,
			CellConnection.NorthEast,
			CellConnection.SouthEast,
			CellConnection.SouthWest,
			CellConnection.NorthWest
		};

		// Token: 0x04001EE7 RID: 7911
		public static readonly CellConnection[] InverseFromBitIndex = new CellConnection[]
		{
			CellConnection.South,
			CellConnection.North,
			CellConnection.West,
			CellConnection.East,
			CellConnection.SouthWest,
			CellConnection.NorthWest,
			CellConnection.NorthEast,
			CellConnection.SouthEast
		};
	}
}
