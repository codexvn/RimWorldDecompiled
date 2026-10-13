using System;
using System.Runtime.CompilerServices;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000496 RID: 1174
	public sealed class MapInfo : IExposable
	{
		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x06001EE3 RID: 7907 RVA: 0x000A18B8 File Offset: 0x0009FAB8
		public PlanetTile Tile
		{
			get
			{
				MapParent mapParent = this.parent;
				if (mapParent == null)
				{
					return PlanetTile.Invalid;
				}
				return mapParent.Tile;
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001EE4 RID: 7908 RVA: 0x000A18CF File Offset: 0x0009FACF
		public int NumCells
		{
			get
			{
				return this.Size.x * this.Size.y * this.Size.z;
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x06001EE5 RID: 7909 RVA: 0x000A18F4 File Offset: 0x0009FAF4
		// (set) Token: 0x06001EE6 RID: 7910 RVA: 0x000A18FC File Offset: 0x0009FAFC
		public IntVec3 Size
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.sizeInt;
			}
			set
			{
				this.sizeInt = value;
			}
		}

		// Token: 0x06001EE7 RID: 7911 RVA: 0x000A1908 File Offset: 0x0009FB08
		public void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.sizeInt, "size", default(IntVec3), false);
			Scribe_Values.Look<bool>(ref this.isPocketMap, "isPocketMap", false, false);
			Scribe_Values.Look<bool>(ref this.disableSunShadows, "disableSunShadows", false, false);
			Scribe_References.Look<MapParent>(ref this.parent, "parent", false);
		}

		// Token: 0x04001A70 RID: 6768
		private IntVec3 sizeInt;

		// Token: 0x04001A71 RID: 6769
		public MapParent parent;

		// Token: 0x04001A72 RID: 6770
		public bool isPocketMap;

		// Token: 0x04001A73 RID: 6771
		public bool disableSunShadows;
	}
}
