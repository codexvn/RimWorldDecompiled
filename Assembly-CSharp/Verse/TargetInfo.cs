using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C89 RID: 3209
	public struct TargetInfo : IEquatable<TargetInfo>
	{
		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x06005256 RID: 21078 RVA: 0x001AA1F3 File Offset: 0x001A83F3
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x06005257 RID: 21079 RVA: 0x001AA20A File Offset: 0x001A840A
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x06005258 RID: 21080 RVA: 0x001AA215 File Offset: 0x001A8415
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x06005259 RID: 21081 RVA: 0x001AA21D File Offset: 0x001A841D
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x0600525A RID: 21082 RVA: 0x001AA234 File Offset: 0x001A8434
		public static TargetInfo Invalid
		{
			get
			{
				return new TargetInfo(IntVec3.Invalid, null, false);
			}
		}

		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x0600525B RID: 21083 RVA: 0x001AA242 File Offset: 0x001A8442
		public bool Fogged
		{
			get
			{
				if (!this.HasThing)
				{
					return this.cellInt.Fogged(this.mapInt);
				}
				return this.thingInt.Fogged();
			}
		}

		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x0600525C RID: 21084 RVA: 0x001AA269 File Offset: 0x001A8469
		public string Label
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.LabelShort;
				}
				return "Location".Translate();
			}
		}

		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x0600525D RID: 21085 RVA: 0x001AA28E File Offset: 0x001A848E
		public IntVec3 Cell
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.PositionHeld;
				}
				return this.cellInt;
			}
		}

		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x0600525E RID: 21086 RVA: 0x001AA2AC File Offset: 0x001A84AC
		public IntVec3 CenterCell
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.OccupiedRect().CenterCell;
				}
				return this.cellInt;
			}
		}

		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x0600525F RID: 21087 RVA: 0x001AA2DC File Offset: 0x001A84DC
		public PlanetTile Tile
		{
			get
			{
				if (this.thingInt != null && this.thingInt.Tile.Valid)
				{
					return this.thingInt.Tile;
				}
				if (this.cellInt.IsValid && this.mapInt != null)
				{
					return this.mapInt.Tile;
				}
				return PlanetTile.Invalid;
			}
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x06005260 RID: 21088 RVA: 0x001AA338 File Offset: 0x001A8538
		public Vector3 CenterVector3
		{
			get
			{
				return ((LocalTargetInfo)this).CenterVector3;
			}
		}

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x06005261 RID: 21089 RVA: 0x001AA358 File Offset: 0x001A8558
		public Map Map
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.MapHeld;
				}
				return this.mapInt;
			}
		}

		// Token: 0x06005262 RID: 21090 RVA: 0x001AA374 File Offset: 0x001A8574
		public TargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
		}

		// Token: 0x06005263 RID: 21091 RVA: 0x001AA390 File Offset: 0x001A8590
		public TargetInfo(IntVec3 cell, Map map, bool allowNullMap = false)
		{
			if (!allowNullMap && cell.IsValid && map == null)
			{
				string str = "Constructed TargetInfo with cell=";
				IntVec3 intVec = cell;
				Log.Warning(str + intVec.ToString() + " and a null map.");
			}
			this.thingInt = null;
			this.cellInt = cell;
			this.mapInt = map;
		}

		// Token: 0x06005264 RID: 21092 RVA: 0x001AA3E4 File Offset: 0x001A85E4
		public static implicit operator TargetInfo(Thing t)
		{
			return new TargetInfo(t);
		}

		// Token: 0x06005265 RID: 21093 RVA: 0x001AA3EC File Offset: 0x001A85EC
		public static explicit operator LocalTargetInfo(TargetInfo t)
		{
			if (t.HasThing)
			{
				return new LocalTargetInfo(t.Thing);
			}
			return new LocalTargetInfo(t.Cell);
		}

		// Token: 0x06005266 RID: 21094 RVA: 0x001AA410 File Offset: 0x001A8610
		public static explicit operator IntVec3(TargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				string str = "Casted TargetInfo to IntVec3 but it had Thing ";
				Thing thing = targ.thingInt;
				Log.ErrorOnce(str + ((thing != null) ? thing.ToString() : null), 6324165);
			}
			return targ.Cell;
		}

		// Token: 0x06005267 RID: 21095 RVA: 0x001AA448 File Offset: 0x001A8648
		public static explicit operator Thing(TargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				string str = "Casted TargetInfo to Thing but it had cell ";
				IntVec3 intVec = targ.cellInt;
				Log.ErrorOnce(str + intVec.ToString(), 631672);
			}
			return targ.thingInt;
		}

		// Token: 0x06005268 RID: 21096 RVA: 0x001AA494 File Offset: 0x001A8694
		public static bool operator ==(TargetInfo a, TargetInfo b)
		{
			if (a.Thing != null || b.Thing != null)
			{
				return a.Thing == b.Thing;
			}
			return (!a.cellInt.IsValid && !b.cellInt.IsValid) || (a.cellInt == b.cellInt && a.mapInt == b.mapInt);
		}

		// Token: 0x06005269 RID: 21097 RVA: 0x001AA505 File Offset: 0x001A8705
		public static bool operator !=(TargetInfo a, TargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x0600526A RID: 21098 RVA: 0x001AA511 File Offset: 0x001A8711
		public override bool Equals(object obj)
		{
			return obj is TargetInfo && this.Equals((TargetInfo)obj);
		}

		// Token: 0x0600526B RID: 21099 RVA: 0x001AA529 File Offset: 0x001A8729
		public bool Equals(TargetInfo other)
		{
			return this == other;
		}

		// Token: 0x0600526C RID: 21100 RVA: 0x001AA537 File Offset: 0x001A8737
		public override int GetHashCode()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetHashCode();
			}
			return Gen.HashCombine<Map>(this.cellInt.GetHashCode(), this.mapInt);
		}

		// Token: 0x0600526D RID: 21101 RVA: 0x001AA56C File Offset: 0x001A876C
		public override string ToString()
		{
			if (this.Thing != null)
			{
				return this.Thing.GetUniqueLoadID();
			}
			if (this.Cell.IsValid)
			{
				return this.Cell.ToString() + ", " + ((this.mapInt != null) ? this.mapInt.GetUniqueLoadID() : "null");
			}
			return "null";
		}

		// Token: 0x04003B04 RID: 15108
		private Thing thingInt;

		// Token: 0x04003B05 RID: 15109
		private IntVec3 cellInt;

		// Token: 0x04003B06 RID: 15110
		private Map mapInt;
	}
}
