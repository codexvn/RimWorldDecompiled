using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200386B RID: 14443
	public struct ThingStuffPair : IEquatable<ThingStuffPair>
	{
		// Token: 0x17003300 RID: 13056
		// (get) Token: 0x06013F74 RID: 81780 RVA: 0x005ECEEC File Offset: 0x005EB0EC
		public float Price
		{
			get
			{
				return this.cachedPrice;
			}
		}

		// Token: 0x17003301 RID: 13057
		// (get) Token: 0x06013F75 RID: 81781 RVA: 0x005ECEF4 File Offset: 0x005EB0F4
		public float InsulationCold
		{
			get
			{
				return this.cachedInsulationCold;
			}
		}

		// Token: 0x17003302 RID: 13058
		// (get) Token: 0x06013F76 RID: 81782 RVA: 0x005ECEFC File Offset: 0x005EB0FC
		public float VacuumResistance
		{
			get
			{
				return this.cachedVacuumResistance;
			}
		}

		// Token: 0x17003303 RID: 13059
		// (get) Token: 0x06013F77 RID: 81783 RVA: 0x005ECF04 File Offset: 0x005EB104
		public float InsulationHeat
		{
			get
			{
				return this.cachedInsulationHeat;
			}
		}

		// Token: 0x17003304 RID: 13060
		// (get) Token: 0x06013F78 RID: 81784 RVA: 0x005ECF0C File Offset: 0x005EB10C
		public float ToxicEnvironmentResistance
		{
			get
			{
				return this.cachedToxicEnvironmentResistance;
			}
		}

		// Token: 0x17003305 RID: 13061
		// (get) Token: 0x06013F79 RID: 81785 RVA: 0x005ECF14 File Offset: 0x005EB114
		public float Commonality
		{
			get
			{
				float num = this.commonalityMultiplier;
				num *= this.thing.generateCommonality;
				if (this.stuff != null)
				{
					num *= this.stuff.stuffProps.commonality;
				}
				if (PawnWeaponGenerator.IsDerpWeapon(this.thing, this.stuff) || PawnApparelGenerator.IsDerpApparel(this.thing, this.stuff))
				{
					num *= 0.01f;
				}
				return num;
			}
		}

		// Token: 0x06013F7A RID: 81786 RVA: 0x005ECF80 File Offset: 0x005EB180
		public ThingStuffPair(ThingDef thing, ThingDef stuff, float commonalityMultiplier = 1f)
		{
			this.thing = thing;
			this.stuff = stuff;
			this.commonalityMultiplier = commonalityMultiplier;
			if (stuff != null && !thing.MadeFromStuff)
			{
				Log.Warning(string.Format("Created ThingStuffPairWithQuality with stuff {0} but {1} is not made from stuff.", stuff, thing));
				stuff = null;
			}
			this.cachedPrice = thing.GetStatValueAbstract(StatDefOf.MarketValue, stuff);
			this.cachedInsulationCold = thing.GetStatValueAbstract(StatDefOf.Insulation_Cold, stuff);
			this.cachedInsulationHeat = thing.GetStatValueAbstract(StatDefOf.Insulation_Heat, stuff);
			this.cachedToxicEnvironmentResistance = (ModsConfig.BiotechActive ? thing.equippedStatOffsets.GetStatOffsetFromList(StatDefOf.ToxicEnvironmentResistance) : 0f);
			this.cachedVacuumResistance = (ModsConfig.OdysseyActive ? thing.equippedStatOffsets.GetStatOffsetFromList(StatDefOf.VacuumResistance) : 0f);
		}

		// Token: 0x06013F7B RID: 81787 RVA: 0x005ED040 File Offset: 0x005EB240
		public static List<ThingStuffPair> AllWith(Predicate<ThingDef> thingValidator)
		{
			List<ThingStuffPair> list = new List<ThingStuffPair>();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				ThingDef thingDef = allDefsListForReading[i];
				if (thingValidator(thingDef))
				{
					if (!thingDef.MadeFromStuff)
					{
						list.Add(new ThingStuffPair(thingDef, null, 1f));
					}
					else
					{
						ThingStuffPair.tmpUsableStuffs.Clear();
						ThingStuffPair.tmpUsableStuffs.AddRange(from st in DefDatabase<ThingDef>.AllDefs
						where st.IsStuff && st.stuffProps.CanMake(thingDef)
						select st);
						int count = ThingStuffPair.tmpUsableStuffs.Count;
						float num = ThingStuffPair.tmpUsableStuffs.Average((ThingDef st) => st.stuffProps.commonality);
						float num2;
						if (Mathf.Approximately(num, 0f))
						{
							num2 = 1f;
						}
						else
						{
							num2 = 1f / (float)count / num;
						}
						foreach (ThingDef thingDef2 in ThingStuffPair.tmpUsableStuffs)
						{
							list.Add(new ThingStuffPair(thingDef, thingDef2, num2));
						}
					}
				}
			}
			return (from p in list
			orderby p.Price descending
			select p).ToList<ThingStuffPair>();
		}

		// Token: 0x06013F7C RID: 81788 RVA: 0x005ED1C4 File Offset: 0x005EB3C4
		public override string ToString()
		{
			if (this.thing == null)
			{
				return "(null)";
			}
			string text;
			if (this.stuff == null)
			{
				text = this.thing.label;
			}
			else
			{
				text = this.thing.label + " " + this.stuff.LabelAsStuff;
			}
			return string.Concat(new string[]
			{
				text,
				" $",
				this.Price.ToString("F0"),
				" c=",
				this.Commonality.ToString("F4")
			});
		}

		// Token: 0x06013F7D RID: 81789 RVA: 0x005ED261 File Offset: 0x005EB461
		public static bool operator ==(ThingStuffPair a, ThingStuffPair b)
		{
			return a.thing == b.thing && a.stuff == b.stuff && Mathf.Approximately(a.commonalityMultiplier, b.commonalityMultiplier);
		}

		// Token: 0x06013F7E RID: 81790 RVA: 0x005ED292 File Offset: 0x005EB492
		public static bool operator !=(ThingStuffPair a, ThingStuffPair b)
		{
			return !(a == b);
		}

		// Token: 0x06013F7F RID: 81791 RVA: 0x005ED29E File Offset: 0x005EB49E
		public override bool Equals(object obj)
		{
			return obj is ThingStuffPair && this.Equals((ThingStuffPair)obj);
		}

		// Token: 0x06013F80 RID: 81792 RVA: 0x005ED2B6 File Offset: 0x005EB4B6
		public bool Equals(ThingStuffPair other)
		{
			return this == other;
		}

		// Token: 0x06013F81 RID: 81793 RVA: 0x005ED2C4 File Offset: 0x005EB4C4
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<float>(Gen.HashCombine<ThingDef>(Gen.HashCombine<ThingDef>(0, this.thing), this.stuff), this.commonalityMultiplier);
		}

		// Token: 0x06013F82 RID: 81794 RVA: 0x005ED2E8 File Offset: 0x005EB4E8
		public static explicit operator ThingStuffPair(ThingStuffPairWithQuality p)
		{
			return new ThingStuffPair(p.thing, p.stuff, 1f);
		}

		// Token: 0x0400DC75 RID: 56437
		public ThingDef thing;

		// Token: 0x0400DC76 RID: 56438
		public ThingDef stuff;

		// Token: 0x0400DC77 RID: 56439
		public float commonalityMultiplier;

		// Token: 0x0400DC78 RID: 56440
		private float cachedPrice;

		// Token: 0x0400DC79 RID: 56441
		private float cachedInsulationCold;

		// Token: 0x0400DC7A RID: 56442
		private float cachedVacuumResistance;

		// Token: 0x0400DC7B RID: 56443
		private float cachedInsulationHeat;

		// Token: 0x0400DC7C RID: 56444
		private float cachedToxicEnvironmentResistance;

		// Token: 0x0400DC7D RID: 56445
		private static readonly List<ThingDef> tmpUsableStuffs = new List<ThingDef>();
	}
}
