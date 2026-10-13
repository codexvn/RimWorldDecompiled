using System;
using System.Xml;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C90 RID: 3216
	public sealed class ThingDefCountClass : IExposable
	{
		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x060052A1 RID: 21153 RVA: 0x001AAD6A File Offset: 0x001A8F6A
		public string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.thingDef, null, this.count);
			}
		}

		// Token: 0x17000CB1 RID: 3249
		// (get) Token: 0x060052A2 RID: 21154 RVA: 0x001AAD7E File Offset: 0x001A8F7E
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.thingDef);
			}
		}

		// Token: 0x17000CB2 RID: 3250
		// (get) Token: 0x060052A3 RID: 21155 RVA: 0x001AAD91 File Offset: 0x001A8F91
		public string Summary
		{
			get
			{
				return this.count.ToString() + "x " + ((this.thingDef != null) ? this.thingDef.label : "null");
			}
		}

		// Token: 0x17000CB3 RID: 3251
		// (get) Token: 0x060052A4 RID: 21156 RVA: 0x001AADC4 File Offset: 0x001A8FC4
		public float DropChance
		{
			get
			{
				float? num = this.chance;
				if (num == null)
				{
					return 1f;
				}
				return num.GetValueOrDefault();
			}
		}

		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x060052A5 RID: 21157 RVA: 0x001AADEE File Offset: 0x001A8FEE
		public bool IsChanceBased
		{
			get
			{
				return this.chance != null;
			}
		}

		// Token: 0x060052A6 RID: 21158 RVA: 0x001AADFB File Offset: 0x001A8FFB
		public ThingDefCountClass()
		{
		}

		// Token: 0x060052A7 RID: 21159 RVA: 0x001AAE14 File Offset: 0x001A9014
		public ThingDefCountClass(ThingDef thingDef, int count)
		{
			if (count < 0)
			{
				Log.Warning("Tried to set ThingDefCountClass count to " + count.ToString() + ". thingDef=" + ((thingDef != null) ? thingDef.ToString() : null));
				count = 0;
			}
			this.thingDef = thingDef;
			this.count = count;
		}

		// Token: 0x060052A8 RID: 21160 RVA: 0x001AAE74 File Offset: 0x001A9074
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
			Scribe_Values.Look<QualityCategory>(ref this.quality, "quality", QualityCategory.Awful, false);
			Scribe_Values.Look<Color?>(ref this.color, "color", null, false);
			Scribe_Values.Look<float?>(ref this.chance, "chance", null, false);
		}

		// Token: 0x060052A9 RID: 21161 RVA: 0x001AAEF9 File Offset: 0x001A90F9
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "thingDef", "count");
		}

		// Token: 0x060052AA RID: 21162 RVA: 0x001AAF0C File Offset: 0x001A910C
		public override string ToString()
		{
			return string.Format("({0}x {1})", this.count, (this.thingDef != null) ? this.thingDef.defName : "null");
		}

		// Token: 0x060052AB RID: 21163 RVA: 0x001AAF3D File Offset: 0x001A913D
		public override int GetHashCode()
		{
			return (int)this.thingDef.shortHash + this.count << 16;
		}

		// Token: 0x060052AC RID: 21164 RVA: 0x001AAF54 File Offset: 0x001A9154
		public IngredientCount ToIngredientCount()
		{
			IngredientCount ingredientCount = new IngredientCount();
			ingredientCount.SetBaseCount((float)this.count);
			ingredientCount.filter.SetAllow(this.thingDef, true);
			return ingredientCount;
		}

		// Token: 0x060052AD RID: 21165 RVA: 0x001AAF7A File Offset: 0x001A917A
		public static implicit operator ThingDefCountClass(ThingDefCount t)
		{
			return new ThingDefCountClass(t.ThingDef, t.Count);
		}

		// Token: 0x04003B10 RID: 15120
		public ThingDef thingDef;

		// Token: 0x04003B11 RID: 15121
		public int count = 1;

		// Token: 0x04003B12 RID: 15122
		public Color? color;

		// Token: 0x04003B13 RID: 15123
		public float? chance;

		// Token: 0x04003B14 RID: 15124
		public ThingDef stuff;

		// Token: 0x04003B15 RID: 15125
		public QualityCategory quality = QualityCategory.Normal;
	}
}
