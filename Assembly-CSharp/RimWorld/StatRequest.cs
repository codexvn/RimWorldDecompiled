using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003733 RID: 14131
	public struct StatRequest : IEquatable<StatRequest>
	{
		// Token: 0x17003230 RID: 12848
		// (get) Token: 0x0601394E RID: 80206 RVA: 0x005CE12B File Offset: 0x005CC32B
		public Thing Thing
		{
			get
			{
				return this.thingInt;
			}
		}

		// Token: 0x17003231 RID: 12849
		// (get) Token: 0x0601394F RID: 80207 RVA: 0x005CE133 File Offset: 0x005CC333
		public Def Def
		{
			get
			{
				return this.defInt;
			}
		}

		// Token: 0x17003232 RID: 12850
		// (get) Token: 0x06013950 RID: 80208 RVA: 0x005CE13B File Offset: 0x005CC33B
		public BuildableDef BuildableDef
		{
			get
			{
				return (BuildableDef)this.defInt;
			}
		}

		// Token: 0x17003233 RID: 12851
		// (get) Token: 0x06013951 RID: 80209 RVA: 0x005CE148 File Offset: 0x005CC348
		public AbilityDef AbilityDef
		{
			get
			{
				return (AbilityDef)this.defInt;
			}
		}

		// Token: 0x17003234 RID: 12852
		// (get) Token: 0x06013952 RID: 80210 RVA: 0x005CE155 File Offset: 0x005CC355
		public Faction Faction
		{
			get
			{
				return this.faction;
			}
		}

		// Token: 0x17003235 RID: 12853
		// (get) Token: 0x06013953 RID: 80211 RVA: 0x005CE15D File Offset: 0x005CC35D
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17003236 RID: 12854
		// (get) Token: 0x06013954 RID: 80212 RVA: 0x005CE165 File Offset: 0x005CC365
		public bool ForAbility
		{
			get
			{
				return this.defInt is AbilityDef;
			}
		}

		// Token: 0x17003237 RID: 12855
		// (get) Token: 0x06013955 RID: 80213 RVA: 0x005CE178 File Offset: 0x005CC378
		public List<StatModifier> StatBases
		{
			get
			{
				BuildableDef buildableDef = this.defInt as BuildableDef;
				if (buildableDef != null)
				{
					return buildableDef.statBases;
				}
				AbilityDef abilityDef = this.defInt as AbilityDef;
				if (abilityDef != null)
				{
					return abilityDef.statBases;
				}
				return null;
			}
		}

		// Token: 0x17003238 RID: 12856
		// (get) Token: 0x06013956 RID: 80214 RVA: 0x005CE1B2 File Offset: 0x005CC3B2
		public ThingDef StuffDef
		{
			get
			{
				return this.stuffDefInt;
			}
		}

		// Token: 0x17003239 RID: 12857
		// (get) Token: 0x06013957 RID: 80215 RVA: 0x005CE1BA File Offset: 0x005CC3BA
		public QualityCategory QualityCategory
		{
			get
			{
				return this.qualityCategoryInt;
			}
		}

		// Token: 0x1700323A RID: 12858
		// (get) Token: 0x06013958 RID: 80216 RVA: 0x005CE1C2 File Offset: 0x005CC3C2
		public bool HasThing
		{
			get
			{
				return this.Thing != null;
			}
		}

		// Token: 0x1700323B RID: 12859
		// (get) Token: 0x06013959 RID: 80217 RVA: 0x005CE1CD File Offset: 0x005CC3CD
		public bool Empty
		{
			get
			{
				return this.Def == null;
			}
		}

		// Token: 0x0601395A RID: 80218 RVA: 0x005CE1D8 File Offset: 0x005CC3D8
		public static StatRequest For(Thing thing)
		{
			if (thing == null)
			{
				Log.Error("StatRequest for null thing.");
				return StatRequest.ForEmpty();
			}
			StatRequest result = default(StatRequest);
			result.thingInt = thing;
			result.defInt = thing.def;
			result.stuffDefInt = thing.Stuff;
			thing.TryGetQuality(out result.qualityCategoryInt);
			return result;
		}

		// Token: 0x0601395B RID: 80219 RVA: 0x005CE234 File Offset: 0x005CC434
		public static StatRequest For(Thing thing, Pawn pawn)
		{
			if (thing == null)
			{
				Log.Error("StatRequest for null thing.");
				return StatRequest.ForEmpty();
			}
			StatRequest result = default(StatRequest);
			result.thingInt = thing;
			result.defInt = thing.def;
			result.stuffDefInt = thing.Stuff;
			result.pawn = pawn;
			thing.TryGetQuality(out result.qualityCategoryInt);
			return result;
		}

		// Token: 0x0601395C RID: 80220 RVA: 0x005CE298 File Offset: 0x005CC498
		public static StatRequest For(BuildableDef def, ThingDef stuffDef, QualityCategory quality = QualityCategory.Normal)
		{
			if (def == null)
			{
				Log.Error("StatRequest for null def.");
				return StatRequest.ForEmpty();
			}
			return new StatRequest
			{
				thingInt = null,
				defInt = def,
				stuffDefInt = stuffDef,
				qualityCategoryInt = quality
			};
		}

		// Token: 0x0601395D RID: 80221 RVA: 0x005CE2E4 File Offset: 0x005CC4E4
		public static StatRequest For(AbilityDef def, Pawn forPawn = null)
		{
			if (def == null)
			{
				Log.Error("StatRequest for null def.");
				return StatRequest.ForEmpty();
			}
			return new StatRequest
			{
				thingInt = null,
				stuffDefInt = null,
				defInt = def,
				qualityCategoryInt = QualityCategory.Normal,
				pawn = forPawn
			};
		}

		// Token: 0x0601395E RID: 80222 RVA: 0x005CE338 File Offset: 0x005CC538
		public static StatRequest For(RoyalTitleDef def, Faction faction, Pawn pawn = null)
		{
			if (def == null)
			{
				Log.Error("StatRequest for null def.");
				return StatRequest.ForEmpty();
			}
			return new StatRequest
			{
				thingInt = null,
				stuffDefInt = null,
				defInt = null,
				faction = faction,
				qualityCategoryInt = QualityCategory.Normal,
				pawn = pawn
			};
		}

		// Token: 0x0601395F RID: 80223 RVA: 0x005CE394 File Offset: 0x005CC594
		public static StatRequest ForEmpty()
		{
			return new StatRequest
			{
				thingInt = null,
				defInt = null,
				stuffDefInt = null,
				qualityCategoryInt = QualityCategory.Normal
			};
		}

		// Token: 0x06013960 RID: 80224 RVA: 0x005CE3CC File Offset: 0x005CC5CC
		public override string ToString()
		{
			if (this.Thing != null)
			{
				string str = "(";
				Thing thing = this.Thing;
				return str + ((thing != null) ? thing.ToString() : null) + ")";
			}
			string[] array = new string[5];
			array[0] = "(";
			int num = 1;
			Thing thing2 = this.Thing;
			array[num] = ((thing2 != null) ? thing2.ToString() : null);
			array[2] = ", ";
			array[3] = ((this.StuffDef != null) ? this.StuffDef.defName : "null");
			array[4] = ")";
			return string.Concat(array);
		}

		// Token: 0x06013961 RID: 80225 RVA: 0x005CE458 File Offset: 0x005CC658
		public override int GetHashCode()
		{
			int num = 0;
			num = Gen.HashCombineInt(num, (int)this.defInt.shortHash);
			if (this.thingInt != null)
			{
				num = Gen.HashCombineInt(num, this.thingInt.thingIDNumber);
			}
			if (this.stuffDefInt != null)
			{
				num = Gen.HashCombineInt(num, (int)this.stuffDefInt.shortHash);
			}
			num = Gen.HashCombineInt(num, this.qualityCategoryInt.GetHashCode());
			if (this.faction != null)
			{
				num = Gen.HashCombineInt(num, this.faction.GetHashCode());
			}
			if (this.pawn != null)
			{
				num = Gen.HashCombineInt(num, this.pawn.GetHashCode());
			}
			return num;
		}

		// Token: 0x06013962 RID: 80226 RVA: 0x005CE4FC File Offset: 0x005CC6FC
		public override bool Equals(object obj)
		{
			if (!(obj is StatRequest))
			{
				return false;
			}
			StatRequest statRequest = (StatRequest)obj;
			return statRequest.defInt == this.defInt && statRequest.thingInt == this.thingInt && statRequest.stuffDefInt == this.stuffDefInt;
		}

		// Token: 0x06013963 RID: 80227 RVA: 0x005CE548 File Offset: 0x005CC748
		public bool Equals(StatRequest other)
		{
			return other.defInt == this.defInt && other.thingInt == this.thingInt && other.stuffDefInt == this.stuffDefInt && other.qualityCategoryInt == this.qualityCategoryInt && other.faction == this.faction && other.pawn == this.pawn;
		}

		// Token: 0x06013964 RID: 80228 RVA: 0x005CE5AB File Offset: 0x005CC7AB
		public static bool operator ==(StatRequest lhs, StatRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06013965 RID: 80229 RVA: 0x005CE5B5 File Offset: 0x005CC7B5
		public static bool operator !=(StatRequest lhs, StatRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0400D917 RID: 55575
		private Thing thingInt;

		// Token: 0x0400D918 RID: 55576
		private Def defInt;

		// Token: 0x0400D919 RID: 55577
		private ThingDef stuffDefInt;

		// Token: 0x0400D91A RID: 55578
		private QualityCategory qualityCategoryInt;

		// Token: 0x0400D91B RID: 55579
		private Faction faction;

		// Token: 0x0400D91C RID: 55580
		private Pawn pawn;
	}
}
