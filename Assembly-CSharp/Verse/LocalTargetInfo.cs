using System;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C7C RID: 3196
	public struct LocalTargetInfo : IEquatable<LocalTargetInfo>
	{
		// Token: 0x17000C72 RID: 3186
		// (get) Token: 0x060051BE RID: 20926 RVA: 0x001A80CE File Offset: 0x001A62CE
		public bool IsValid
		{
			get
			{
				return this.thingInt != null || this.cellInt.IsValid;
			}
		}

		// Token: 0x17000C73 RID: 3187
		// (get) Token: 0x060051BF RID: 20927 RVA: 0x001A80E5 File Offset: 0x001A62E5
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x17000C74 RID: 3188
		// (get) Token: 0x060051C0 RID: 20928 RVA: 0x001A80F0 File Offset: 0x001A62F0
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17000C75 RID: 3189
		// (get) Token: 0x060051C1 RID: 20929 RVA: 0x001A80F8 File Offset: 0x001A62F8
		public Pawn Pawn
		{
			get
			{
				return this.Thing as Pawn;
			}
		}

		// Token: 0x17000C76 RID: 3190
		// (get) Token: 0x060051C2 RID: 20930 RVA: 0x001A8105 File Offset: 0x001A6305
		public bool ThingDestroyed
		{
			get
			{
				return this.Thing != null && this.Thing.Destroyed;
			}
		}

		// Token: 0x17000C77 RID: 3191
		// (get) Token: 0x060051C3 RID: 20931 RVA: 0x001A811C File Offset: 0x001A631C
		public bool ThingDiscarded
		{
			get
			{
				return this.Thing != null && this.Thing.Discarded;
			}
		}

		// Token: 0x17000C78 RID: 3192
		// (get) Token: 0x060051C4 RID: 20932 RVA: 0x001A8133 File Offset: 0x001A6333
		public static LocalTargetInfo Invalid
		{
			get
			{
				return new LocalTargetInfo(IntVec3.Invalid);
			}
		}

		// Token: 0x17000C79 RID: 3193
		// (get) Token: 0x060051C5 RID: 20933 RVA: 0x001A813F File Offset: 0x001A633F
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

		// Token: 0x17000C7A RID: 3194
		// (get) Token: 0x060051C6 RID: 20934 RVA: 0x001A8164 File Offset: 0x001A6364
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

		// Token: 0x17000C7B RID: 3195
		// (get) Token: 0x060051C7 RID: 20935 RVA: 0x001A8180 File Offset: 0x001A6380
		public Vector3 CenterVector3
		{
			get
			{
				if (this.thingInt != null)
				{
					if (this.thingInt.Spawned)
					{
						return this.thingInt.DrawPos;
					}
					if (this.thingInt.SpawnedOrAnyParentSpawned)
					{
						return this.thingInt.PositionHeld.ToVector3Shifted();
					}
					return this.thingInt.Position.ToVector3Shifted();
				}
				else
				{
					if (this.cellInt.IsValid)
					{
						return this.cellInt.ToVector3Shifted();
					}
					return default(Vector3);
				}
			}
		}

		// Token: 0x060051C8 RID: 20936 RVA: 0x001A8205 File Offset: 0x001A6405
		public LocalTargetInfo(Thing thing)
		{
			this.thingInt = thing;
			this.cellInt = IntVec3.Invalid;
		}

		// Token: 0x060051C9 RID: 20937 RVA: 0x001A8219 File Offset: 0x001A6419
		public LocalTargetInfo(IntVec3 cell)
		{
			this.thingInt = null;
			this.cellInt = cell;
		}

		// Token: 0x060051CA RID: 20938 RVA: 0x001A8229 File Offset: 0x001A6429
		public bool TryGetPawn(out Pawn pawn)
		{
			pawn = this.Pawn;
			return pawn != null;
		}

		// Token: 0x060051CB RID: 20939 RVA: 0x001A8238 File Offset: 0x001A6438
		public static implicit operator LocalTargetInfo(Thing t)
		{
			return new LocalTargetInfo(t);
		}

		// Token: 0x060051CC RID: 20940 RVA: 0x001A8240 File Offset: 0x001A6440
		public static implicit operator LocalTargetInfo(IntVec3 c)
		{
			return new LocalTargetInfo(c);
		}

		// Token: 0x060051CD RID: 20941 RVA: 0x001A8248 File Offset: 0x001A6448
		public static explicit operator IntVec3(LocalTargetInfo targ)
		{
			if (targ.thingInt != null)
			{
				string str = "Casted LocalTargetInfo to IntVec3 but it had Thing ";
				Thing thing = targ.thingInt;
				Log.ErrorOnce(str + ((thing != null) ? thing.ToString() : null), 6324165);
			}
			return targ.Cell;
		}

		// Token: 0x060051CE RID: 20942 RVA: 0x001A8280 File Offset: 0x001A6480
		public static explicit operator Thing(LocalTargetInfo targ)
		{
			if (targ.cellInt.IsValid)
			{
				string str = "Casted LocalTargetInfo to Thing but it had cell ";
				IntVec3 intVec = targ.cellInt;
				Log.ErrorOnce(str + intVec.ToString(), 631672);
			}
			return targ.thingInt;
		}

		// Token: 0x060051CF RID: 20943 RVA: 0x001A82C9 File Offset: 0x001A64C9
		public TargetInfo ToTargetInfo(Map map)
		{
			if (!this.IsValid)
			{
				return TargetInfo.Invalid;
			}
			if (this.Thing != null)
			{
				return new TargetInfo(this.Thing);
			}
			return new TargetInfo(this.Cell, map, false);
		}

		// Token: 0x060051D0 RID: 20944 RVA: 0x001A82FA File Offset: 0x001A64FA
		public GlobalTargetInfo ToGlobalTargetInfo(Map map)
		{
			if (!this.IsValid)
			{
				return GlobalTargetInfo.Invalid;
			}
			if (this.Thing != null)
			{
				return new GlobalTargetInfo(this.Thing);
			}
			return new GlobalTargetInfo(this.Cell, map, false);
		}

		// Token: 0x060051D1 RID: 20945 RVA: 0x001A832C File Offset: 0x001A652C
		public static bool operator ==(LocalTargetInfo a, LocalTargetInfo b)
		{
			if (a.thingInt != null || b.thingInt != null)
			{
				return a.thingInt == b.thingInt;
			}
			return (!a.cellInt.IsValid && !b.cellInt.IsValid) || a.cellInt == b.cellInt;
		}

		// Token: 0x060051D2 RID: 20946 RVA: 0x001A8387 File Offset: 0x001A6587
		public static bool operator !=(LocalTargetInfo a, LocalTargetInfo b)
		{
			return !(a == b);
		}

		// Token: 0x060051D3 RID: 20947 RVA: 0x001A8393 File Offset: 0x001A6593
		public override bool Equals(object obj)
		{
			return obj is LocalTargetInfo && this.Equals((LocalTargetInfo)obj);
		}

		// Token: 0x060051D4 RID: 20948 RVA: 0x001A83AB File Offset: 0x001A65AB
		public bool Equals(LocalTargetInfo other)
		{
			return this == other;
		}

		// Token: 0x060051D5 RID: 20949 RVA: 0x001A83B9 File Offset: 0x001A65B9
		public override int GetHashCode()
		{
			if (this.thingInt != null)
			{
				return this.thingInt.GetHashCode();
			}
			return this.cellInt.GetHashCode();
		}

		// Token: 0x060051D6 RID: 20950 RVA: 0x001A83E0 File Offset: 0x001A65E0
		public override string ToString()
		{
			if (this.Thing != null)
			{
				return this.Thing.GetUniqueLoadID();
			}
			if (this.Cell.IsValid)
			{
				return this.Cell.ToString();
			}
			return "null";
		}

		// Token: 0x04003ACF RID: 15055
		private Thing thingInt;

		// Token: 0x04003AD0 RID: 15056
		private IntVec3 cellInt;
	}
}
