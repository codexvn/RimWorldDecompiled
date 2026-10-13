using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200386E RID: 14446
	public struct ThingStuffPairWithQuality : IEquatable<ThingStuffPairWithQuality>, IExposable
	{
		// Token: 0x17003306 RID: 13062
		// (get) Token: 0x06013F8A RID: 81802 RVA: 0x005ED338 File Offset: 0x005EB538
		public QualityCategory Quality
		{
			get
			{
				QualityCategory? qualityCategory = this.quality;
				if (qualityCategory == null)
				{
					return QualityCategory.Normal;
				}
				return qualityCategory.GetValueOrDefault();
			}
		}

		// Token: 0x06013F8B RID: 81803 RVA: 0x005ED360 File Offset: 0x005EB560
		public ThingStuffPairWithQuality(ThingDef thing, ThingDef stuff, QualityCategory quality)
		{
			this.thing = thing;
			this.stuff = stuff;
			this.quality = new QualityCategory?(quality);
			if (quality != QualityCategory.Normal && !thing.HasComp(typeof(CompQuality)))
			{
				Log.Warning(string.Concat(new string[]
				{
					"Created ThingStuffPairWithQuality with quality",
					quality.ToString(),
					" but ",
					(thing != null) ? thing.ToString() : null,
					" doesn't have CompQuality."
				}));
				quality = QualityCategory.Normal;
			}
			if (stuff != null && !thing.MadeFromStuff)
			{
				string[] array = new string[5];
				array[0] = "Created ThingStuffPairWithQuality with stuff ";
				int num = 1;
				ThingDef thingDef = stuff;
				array[num] = ((thingDef != null) ? thingDef.ToString() : null);
				array[2] = " but ";
				array[3] = ((thing != null) ? thing.ToString() : null);
				array[4] = " is not made from stuff.";
				Log.Warning(string.Concat(array));
				stuff = null;
			}
		}

		// Token: 0x06013F8C RID: 81804 RVA: 0x005ED43E File Offset: 0x005EB63E
		public float GetStatValue(StatDef stat)
		{
			return stat.Worker.GetValue(StatRequest.For(this.thing, this.stuff, this.Quality), true);
		}

		// Token: 0x06013F8D RID: 81805 RVA: 0x005ED464 File Offset: 0x005EB664
		public static bool operator ==(ThingStuffPairWithQuality a, ThingStuffPairWithQuality b)
		{
			if (a.thing == b.thing && a.stuff == b.stuff)
			{
				QualityCategory? qualityCategory = a.quality;
				QualityCategory? qualityCategory2 = b.quality;
				return qualityCategory.GetValueOrDefault() == qualityCategory2.GetValueOrDefault() & qualityCategory != null == (qualityCategory2 != null);
			}
			return false;
		}

		// Token: 0x06013F8E RID: 81806 RVA: 0x005ED4BE File Offset: 0x005EB6BE
		public static bool operator !=(ThingStuffPairWithQuality a, ThingStuffPairWithQuality b)
		{
			return !(a == b);
		}

		// Token: 0x06013F8F RID: 81807 RVA: 0x005ED4CA File Offset: 0x005EB6CA
		public override bool Equals(object obj)
		{
			return obj is ThingStuffPairWithQuality && this.Equals((ThingStuffPairWithQuality)obj);
		}

		// Token: 0x06013F90 RID: 81808 RVA: 0x005ED4E2 File Offset: 0x005EB6E2
		public bool Equals(ThingStuffPairWithQuality other)
		{
			return this == other;
		}

		// Token: 0x06013F91 RID: 81809 RVA: 0x005ED4F0 File Offset: 0x005EB6F0
		public override int GetHashCode()
		{
			return Gen.HashCombine<QualityCategory?>(Gen.HashCombine<ThingDef>(Gen.HashCombine<ThingDef>(0, this.thing), this.stuff), this.quality);
		}

		// Token: 0x06013F92 RID: 81810 RVA: 0x005ED514 File Offset: 0x005EB714
		public static explicit operator ThingStuffPairWithQuality(ThingStuffPair p)
		{
			return new ThingStuffPairWithQuality(p.thing, p.stuff, QualityCategory.Normal);
		}

		// Token: 0x06013F93 RID: 81811 RVA: 0x005ED528 File Offset: 0x005EB728
		public Thing MakeThing(bool forceQuality = false)
		{
			Thing result = ThingMaker.MakeThing(this.thing, this.stuff);
			if (!forceQuality && result.HasComp<CompUniqueWeapon>())
			{
				return result;
			}
			CompQuality compQuality = result.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				compQuality.SetQuality(this.Quality, new ArtGenerationContext?(ArtGenerationContext.Outsider));
			}
			return result;
		}

		// Token: 0x06013F94 RID: 81812 RVA: 0x005ED574 File Offset: 0x005EB774
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thing, "thing");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<QualityCategory?>(ref this.quality, "quality", null, false);
		}

		// Token: 0x0400DC82 RID: 56450
		public ThingDef thing;

		// Token: 0x0400DC83 RID: 56451
		public ThingDef stuff;

		// Token: 0x0400DC84 RID: 56452
		public QualityCategory? quality;
	}
}
