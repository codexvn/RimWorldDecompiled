using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C5F RID: 15455
	public readonly struct PlanetTile : IEquatable<PlanetTile>
	{
		// Token: 0x17003415 RID: 13333
		// (get) Token: 0x06014FD4 RID: 85972 RVA: 0x00648D3F File Offset: 0x00646F3F
		public Tile Tile
		{
			get
			{
				return this.Layer[this.tileId];
			}
		}

		// Token: 0x17003416 RID: 13334
		// (get) Token: 0x06014FD5 RID: 85973 RVA: 0x00648D52 File Offset: 0x00646F52
		public PlanetLayer Layer
		{
			get
			{
				if (this.layerId < 0)
				{
					return Find.WorldGrid.Surface;
				}
				return Find.WorldGrid.PlanetLayers[this.layerId];
			}
		}

		// Token: 0x17003417 RID: 13335
		// (get) Token: 0x06014FD6 RID: 85974 RVA: 0x00648D7D File Offset: 0x00646F7D
		public PlanetLayerDef LayerDef
		{
			get
			{
				return this.Layer.Def;
			}
		}

		// Token: 0x17003418 RID: 13336
		// (get) Token: 0x06014FD7 RID: 85975 RVA: 0x00648D8A File Offset: 0x00646F8A
		public bool Valid
		{
			get
			{
				return this.tileId >= 0;
			}
		}

		// Token: 0x06014FD8 RID: 85976 RVA: 0x00648D98 File Offset: 0x00646F98
		public PlanetTile(int tileId, PlanetLayer layer)
		{
			this = new PlanetTile(tileId, layer.LayerID);
		}

		// Token: 0x06014FD9 RID: 85977 RVA: 0x00648DA7 File Offset: 0x00646FA7
		public PlanetTile(int tileId)
		{
			this.tileId = tileId;
			this.layerId = 0;
		}

		// Token: 0x06014FDA RID: 85978 RVA: 0x00648DB7 File Offset: 0x00646FB7
		public PlanetTile(int tileId, int layerId)
		{
			this.tileId = tileId;
			this.layerId = layerId;
		}

		// Token: 0x06014FDB RID: 85979 RVA: 0x00648DC8 File Offset: 0x00646FC8
		public bool Equals(PlanetTile other)
		{
			if (this.tileId != other.tileId)
			{
				return false;
			}
			bool flag = this.layerId < 0 || this.Layer.IsRootSurface;
			bool flag2 = other.layerId < 0 || other.Layer.IsRootSurface;
			return (flag && flag2) || this.layerId == other.layerId;
		}

		// Token: 0x06014FDC RID: 85980 RVA: 0x00648E2C File Offset: 0x0064702C
		public override bool Equals(object obj)
		{
			if (obj is PlanetTile)
			{
				PlanetTile other = (PlanetTile)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06014FDD RID: 85981 RVA: 0x00648E54 File Offset: 0x00647054
		public override int GetHashCode()
		{
			bool flag = this.layerId < 0 || this.Layer.IsRootSurface;
			return this.tileId * 397 ^ (flag ? 0 : this.layerId);
		}

		// Token: 0x06014FDE RID: 85982 RVA: 0x00648E92 File Offset: 0x00647092
		public static bool operator ==(PlanetTile lhs, PlanetTile rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06014FDF RID: 85983 RVA: 0x00648E9C File Offset: 0x0064709C
		public static bool operator !=(PlanetTile lhs, PlanetTile rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06014FE0 RID: 85984 RVA: 0x00648EA8 File Offset: 0x006470A8
		public static implicit operator int(PlanetTile tile)
		{
			return tile.tileId;
		}

		// Token: 0x06014FE1 RID: 85985 RVA: 0x00648EB0 File Offset: 0x006470B0
		public static implicit operator PlanetTile(int tileId)
		{
			return new PlanetTile(tileId);
		}

		// Token: 0x06014FE2 RID: 85986 RVA: 0x00648EB8 File Offset: 0x006470B8
		public static bool TryParse(string str, out PlanetTile tile)
		{
			string[] array = str.Split(',', StringSplitOptions.None);
			tile = PlanetTile.Invalid;
			int num;
			int num2;
			if (array.Length == 1 && int.TryParse(array[0], out num))
			{
				tile = new PlanetTile(num);
			}
			else if (array.Length == 2 && int.TryParse(array[0], out num) && int.TryParse(array[1], out num2))
			{
				tile = new PlanetTile(num, num2);
			}
			return tile.Valid;
		}

		// Token: 0x06014FE3 RID: 85987 RVA: 0x00648F2C File Offset: 0x0064712C
		public static PlanetTile FromString(string str)
		{
			PlanetTile result;
			if (PlanetTile.TryParse(str, out result))
			{
				return result;
			}
			return PlanetTile.Invalid;
		}

		// Token: 0x06014FE4 RID: 85988 RVA: 0x00648F4A File Offset: 0x0064714A
		public override string ToString()
		{
			return string.Format("{0},{1}", this.tileId, this.layerId);
		}

		// Token: 0x0400EA21 RID: 59937
		public readonly int tileId;

		// Token: 0x0400EA22 RID: 59938
		private readonly int layerId;

		// Token: 0x0400EA23 RID: 59939
		public static readonly PlanetTile Invalid = new PlanetTile(-1);
	}
}
