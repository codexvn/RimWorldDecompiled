using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C45 RID: 15429
	public struct GlobalTargetInfo : IEquatable<GlobalTargetInfo>
	{
		// Token: 0x170033D5 RID: 13269
		// (get) Token: 0x06014F18 RID: 85784 RVA: 0x00644FE8 File Offset: 0x006431E8
		public bool IsValid
		{
			get
			{
				if (this.thingInt != null)
				{
					Pawn pawn = this.Pawn;
					if (pawn == null || !pawn.IsHiddenFromPlayer())
					{
						return true;
					}
				}
				if (!this.cellInt.IsValid && this.worldObjectInt == null)
				{
					return this.tileInt.Valid;
				}
				return true;
			}
		}

		// Token: 0x170033D6 RID: 13270
		// (get) Token: 0x06014F19 RID: 85785 RVA: 0x00645036 File Offset: 0x00643236
		public bool IsMapTarget
		{
			get
			{
				return this.HasThing || this.cellInt.IsValid;
			}
		}

		// Token: 0x170033D7 RID: 13271
		// (get) Token: 0x06014F1A RID: 85786 RVA: 0x0064504D File Offset: 0x0064324D
		public bool IsWorldTarget
		{
			get
			{
				return this.HasWorldObject || this.tileInt.Valid;
			}
		}

		// Token: 0x170033D8 RID: 13272
		// (get) Token: 0x06014F1B RID: 85787 RVA: 0x00645064 File Offset: 0x00643264
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x170033D9 RID: 13273
		// (get) Token: 0x06014F1C RID: 85788 RVA: 0x0064506F File Offset: 0x0064326F
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x170033DA RID: 13274
		// (get) Token: 0x06014F1D RID: 85789 RVA: 0x00645077 File Offset: 0x00643277
		public Pawn Pawn
		{
			get
			{
				return this.thingInt as Pawn;
			}
		}

		// Token: 0x170033DB RID: 13275
		// (get) Token: 0x06014F1E RID: 85790 RVA: 0x00645084 File Offset: 0x00643284
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x170033DC RID: 13276
		// (get) Token: 0x06014F1F RID: 85791 RVA: 0x0064509B File Offset: 0x0064329B
		public bool HasWorldObject
		{
			get
			{
				return this.WorldObject != null;
			}
		}

		// Token: 0x170033DD RID: 13277
		// (get) Token: 0x06014F20 RID: 85792 RVA: 0x006450A6 File Offset: 0x006432A6
		public WorldObject WorldObject
		{
			get
			{
				return this.worldObjectInt;
			}
		}

		// Token: 0x170033DE RID: 13278
		// (get) Token: 0x06014F21 RID: 85793 RVA: 0x006450AE File Offset: 0x006432AE
		public static GlobalTargetInfo Invalid
		{
			get
			{
				return new GlobalTargetInfo(IntVec3.Invalid, null, false);
			}
		}

		// Token: 0x170033DF RID: 13279
		// (get) Token: 0x06014F22 RID: 85794 RVA: 0x006450BC File Offset: 0x006432BC
		public string Label
		{
			get
			{
				if (this.thingInt != null)
				{
					return this.thingInt.LabelShort;
				}
				if (this.worldObjectInt != null)
				{
					return this.worldObjectInt.LabelShort;
				}
				return "Location".Translate();
			}
		}

		// Token: 0x170033E0 RID: 13280
		// (get) Token: 0x06014F23 RID: 85795 RVA: 0x006450F5 File Offset: 0x006432F5
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

		// Token: 0x170033E1 RID: 13281
		// (get) Token: 0x06014F24 RID: 85796 RVA: 0x00645111 File Offset: 0x00643311
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

		// Token: 0x170033E2 RID: 13282
		// (get) Token: 0x06014F25 RID: 85797 RVA: 0x00645130 File Offset: 0x00643330
		public PlanetTile Tile
		{
			get
			{
				if (this.worldObjectInt != null)
				{
					return this.worldObjectInt.Tile;
				}
				if (this.tileInt.Valid)
				{
					return this.tileInt;
				}
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

		// Token: 0x06014F26 RID: 85798 RVA: 0x006451B4 File Offset: 0x006433B4
		public GlobalTargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = null;
			this.tileInt = PlanetTile.Invalid;
		}

		// Token: 0x06014F27 RID: 85799 RVA: 0x006451E4 File Offset: 0x006433E4
		public GlobalTargetInfo(IntVec3 cell, Map map, bool allowNullMap = false)
		{
			if (!allowNullMap && cell.IsValid && map == null)
			{
				string str = "Constructed GlobalTargetInfo with cell=";
				IntVec3 intVec = cell;
				Log.Warning(str + intVec.ToString() + " and a null map.");
			}
			this.thingInt = null;
			this.cellInt = cell;
			this.mapInt = map;
			this.worldObjectInt = null;
			this.tileInt = PlanetTile.Invalid;
		}

		// Token: 0x06014F28 RID: 85800 RVA: 0x0064524A File Offset: 0x0064344A
		public GlobalTargetInfo(WorldObject worldObject)
		{
			this.thingInt = null;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = worldObject;
			this.tileInt = PlanetTile.Invalid;
		}

		// Token: 0x06014F29 RID: 85801 RVA: 0x00645277 File Offset: 0x00643477
		public GlobalTargetInfo(PlanetTile tile)
		{
			this.thingInt = null;
			this.cellInt = IntVec3.Invalid;
			this.mapInt = null;
			this.worldObjectInt = null;
			this.tileInt = tile;
		}

		// Token: 0x06014F2A RID: 85802 RVA: 0x006452A0 File Offset: 0x006434A0
		public static implicit operator GlobalTargetInfo(TargetInfo target)
		{
			if (target.HasThing)
			{
				return new GlobalTargetInfo(target.Thing);
			}
			return new GlobalTargetInfo(target.Cell, target.Map, false);
		}

		// Token: 0x06014F2B RID: 85803 RVA: 0x006452CC File Offset: 0x006434CC
		public static implicit operator GlobalTargetInfo(Thing t)
		{
			return new GlobalTargetInfo(t);
		}

		// Token: 0x06014F2C RID: 85804 RVA: 0x006452D4 File Offset: 0x006434D4
		public static implicit operator GlobalTargetInfo(WorldObject o)
		{
			return new GlobalTargetInfo(o);
		}

		// Token: 0x06014F2D RID: 85805 RVA: 0x006452DC File Offset: 0x006434DC
		public static explicit operator LocalTargetInfo(GlobalTargetInfo targ)
		{
			if (targ.worldObjectInt != null)
			{
				string str = "Casted GlobalTargetInfo to LocalTargetInfo but it had WorldObject ";
				WorldObject worldObject = targ.worldObjectInt;
				Log.ErrorOnce(str + ((worldObject != null) ? worldObject.ToString() : null), 134566);
				return LocalTargetInfo.Invalid;
			}
			if (targ.tileInt.Valid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to LocalTargetInfo but it had tile " + targ.tileInt.ToString(), 7833122);
				return LocalTargetInfo.Invalid;
			}
			if (!targ.IsValid)
			{
				return LocalTargetInfo.Invalid;
			}
			if (targ.thingInt != null)
			{
				return new LocalTargetInfo(targ.thingInt);
			}
			return new LocalTargetInfo(targ.cellInt);
		}

		// Token: 0x06014F2E RID: 85806 RVA: 0x00645388 File Offset: 0x00643588
		public static explicit operator TargetInfo(GlobalTargetInfo targ)
		{
			if (targ.worldObjectInt != null)
			{
				string str = "Casted GlobalTargetInfo to TargetInfo but it had WorldObject ";
				WorldObject worldObject = targ.worldObjectInt;
				Log.ErrorOnce(str + ((worldObject != null) ? worldObject.ToString() : null), 134566);
				return TargetInfo.Invalid;
			}
			if (targ.tileInt.Valid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to TargetInfo but it had tile " + targ.tileInt.ToString(), 7833122);
				return TargetInfo.Invalid;
			}
			if (!targ.IsValid)
			{
				return TargetInfo.Invalid;
			}
			if (targ.thingInt != null)
			{
				return new TargetInfo(targ.thingInt);
			}
			return new TargetInfo(targ.cellInt, targ.mapInt, false);
		}

		// Token: 0x06014F2F RID: 85807 RVA: 0x00645438 File Offset: 0x00643638
		public static explicit operator IntVec3(GlobalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				string str = "Casted GlobalTargetInfo to IntVec3 but it had Thing ";
				Thing thing = targ.thingInt;
				Log.ErrorOnce(str + ((thing != null) ? thing.ToString() : null), 6324165);
			}
			if (targ.worldObjectInt != null)
			{
				string str2 = "Casted GlobalTargetInfo to IntVec3 but it had WorldObject ";
				WorldObject worldObject = targ.worldObjectInt;
				Log.ErrorOnce(str2 + ((worldObject != null) ? worldObject.ToString() : null), 134566);
			}
			if (targ.tileInt.Valid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to IntVec3 but it had tile " + targ.tileInt.ToString(), 7833122);
			}
			return targ.Cell;
		}

		// Token: 0x06014F30 RID: 85808 RVA: 0x006454DC File Offset: 0x006436DC
		public static explicit operator Thing(GlobalTargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				string str = "Casted GlobalTargetInfo to Thing but it had cell ";
				IntVec3 intVec = targ.cellInt;
				Log.ErrorOnce(str + intVec.ToString(), 631672);
			}
			if (targ.worldObjectInt != null)
			{
				string str2 = "Casted GlobalTargetInfo to Thing but it had WorldObject ";
				WorldObject worldObject = targ.worldObjectInt;
				Log.ErrorOnce(str2 + ((worldObject != null) ? worldObject.ToString() : null), 134566);
			}
			if (targ.tileInt.Valid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to Thing but it had tile " + targ.tileInt.ToString(), 7833122);
			}
			return targ.thingInt;
		}

		// Token: 0x06014F31 RID: 85809 RVA: 0x00645588 File Offset: 0x00643788
		public static explicit operator WorldObject(GlobalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				string str = "Casted GlobalTargetInfo to WorldObject but it had Thing ";
				Thing thing = targ.thingInt;
				Log.ErrorOnce(str + ((thing != null) ? thing.ToString() : null), 6324165);
			}
			if (targ.cellInt.IsValid)
			{
				string str2 = "Casted GlobalTargetInfo to WorldObject but it had cell ";
				IntVec3 intVec = targ.cellInt;
				Log.ErrorOnce(str2 + intVec.ToString(), 631672);
			}
			if (targ.tileInt.Valid)
			{
				Log.ErrorOnce("Casted GlobalTargetInfo to WorldObject but it had tile " + targ.tileInt.ToString(), 7833122);
			}
			return targ.worldObjectInt;
		}

		// Token: 0x06014F32 RID: 85810 RVA: 0x00645634 File Offset: 0x00643834
		public static bool operator ==(GlobalTargetInfo a, GlobalTargetInfo b)
		{
			if (a.Thing != null || b.Thing != null)
			{
				return a.Thing == b.Thing;
			}
			if (a.cellInt.IsValid || b.cellInt.IsValid)
			{
				return a.cellInt == b.cellInt && a.mapInt == b.mapInt;
			}
			if (a.WorldObject != null || b.WorldObject != null)
			{
				return a.WorldObject == b.WorldObject;
			}
			return (!a.tileInt.Valid && !b.tileInt.Valid) || a.tileInt == b.tileInt;
		}

		// Token: 0x06014F33 RID: 85811 RVA: 0x006456F6 File Offset: 0x006438F6
		public static bool operator !=(GlobalTargetInfo a, GlobalTargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x06014F34 RID: 85812 RVA: 0x00645704 File Offset: 0x00643904
		public override bool Equals(object obj)
		{
			if (obj is GlobalTargetInfo)
			{
				GlobalTargetInfo other = (GlobalTargetInfo)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06014F35 RID: 85813 RVA: 0x0064572B File Offset: 0x0064392B
		public bool Equals(GlobalTargetInfo other)
		{
			return this == other;
		}

		// Token: 0x06014F36 RID: 85814 RVA: 0x0064573C File Offset: 0x0064393C
		public override int GetHashCode()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetHashCode();
			}
			if (this.cellInt.IsValid)
			{
				return Gen.HashCombine<Map>(this.cellInt.GetHashCode(), this.mapInt);
			}
			if (this.worldObjectInt != null)
			{
				return this.worldObjectInt.GetHashCode();
			}
			if (this.tileInt.Valid)
			{
				return this.tileInt.GetHashCode();
			}
			return -1;
		}

		// Token: 0x06014F37 RID: 85815 RVA: 0x006457BC File Offset: 0x006439BC
		public override string ToString()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetUniqueLoadID();
			}
			if (this.cellInt.IsValid)
			{
				return this.cellInt.ToString() + ", " + ((this.mapInt != null) ? this.mapInt.GetUniqueLoadID() : "null");
			}
			if (this.worldObjectInt != null)
			{
				return "@" + this.worldObjectInt.GetUniqueLoadID();
			}
			if (this.tileInt.Valid)
			{
				return this.tileInt.ToString();
			}
			return "null";
		}

		// Token: 0x0400E96A RID: 59754
		private Thing thingInt;

		// Token: 0x0400E96B RID: 59755
		private IntVec3 cellInt;

		// Token: 0x0400E96C RID: 59756
		private Map mapInt;

		// Token: 0x0400E96D RID: 59757
		private WorldObject worldObjectInt;

		// Token: 0x0400E96E RID: 59758
		private PlanetTile tileInt;

		// Token: 0x0400E96F RID: 59759
		public const char WorldObjectLoadIDMarker = '@';
	}
}
