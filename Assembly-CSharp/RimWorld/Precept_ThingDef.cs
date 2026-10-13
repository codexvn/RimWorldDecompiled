using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002671 RID: 9841
	public class Precept_ThingDef : Precept
	{
		// Token: 0x17002118 RID: 8472
		// (get) Token: 0x0600D7FB RID: 55291 RVA: 0x00400493 File Offset: 0x003FE693
		public override string TipLabel
		{
			get
			{
				return this.def.tipLabelOverride ?? (this.def.issue.LabelCap + ": " + this.ThingDef.LabelCap);
			}
		}

		// Token: 0x17002119 RID: 8473
		// (get) Token: 0x0600D7FC RID: 55292 RVA: 0x004004D3 File Offset: 0x003FE6D3
		// (set) Token: 0x0600D7FD RID: 55293 RVA: 0x004004DB File Offset: 0x003FE6DB
		public ThingDef ThingDef
		{
			get
			{
				return this.thingDef;
			}
			set
			{
				if (this.thingDef != value)
				{
					this.thingDef = value;
					this.Notify_ThingDefSet();
				}
			}
		}

		// Token: 0x0600D7FE RID: 55294 RVA: 0x004004F4 File Offset: 0x003FE6F4
		public static List<StyleCategoryPair> AllPossibleStylesForBuilding(ThingDef building)
		{
			Precept_ThingDef.tmpStylesForBuilding.Clear();
			foreach (StyleCategoryDef styleCategoryDef in DefDatabase<StyleCategoryDef>.AllDefsListForReading)
			{
				foreach (ThingDefStyle thingDefStyle in styleCategoryDef.thingDefStyles)
				{
					if (thingDefStyle.ThingDef == building)
					{
						Precept_ThingDef.tmpStylesForBuilding.Add(new StyleCategoryPair
						{
							category = styleCategoryDef,
							styleDef = thingDefStyle.StyleDef
						});
					}
				}
			}
			return Precept_ThingDef.tmpStylesForBuilding;
		}

		// Token: 0x0600D7FF RID: 55295 RVA: 0x004005B4 File Offset: 0x003FE7B4
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			base.Init(ideo, null);
			IEnumerable<PreceptThingChance> source;
			if (!this.def.canUseAlreadyUsedThingDef)
			{
				Precept_ThingDef.usedThingDefsTmp.Clear();
				foreach (Precept precept in ideo.PreceptsListForReading)
				{
					if (precept != this)
					{
						Precept_ThingStyle precept_ThingStyle = precept as Precept_ThingStyle;
						if (precept_ThingStyle != null)
						{
							Precept_ThingDef.usedThingDefsTmp.Add(precept_ThingStyle.ThingDef);
						}
					}
				}
				source = from bd in this.def.Worker.ThingDefsForIdeo(ideo, generatingFor)
				where !Precept_ThingDef.usedThingDefsTmp.Contains(bd.def)
				select bd;
			}
			else
			{
				source = this.def.Worker.ThingDefsForIdeo(ideo, generatingFor);
			}
			if (this.ThingDef == null)
			{
				if (source.Any<PreceptThingChance>())
				{
					PreceptThingChance preceptThingChance;
					if (source.TryRandomElementByWeight((PreceptThingChance d) => d.chance, out preceptThingChance))
					{
						this.ThingDef = preceptThingChance.def;
						goto IL_16E;
					}
				}
				this.ThingDef = this.def.Worker.ThingDefsForIdeo(ideo, generatingFor).RandomElementByWeight((PreceptThingChance d) => d.chance).def;
				Log.Warning("Failed to generate a unique building for " + ideo.name + " for precept " + this.def.defName);
			}
			IL_16E:
			if (this.UsesGeneratedName)
			{
				base.RegenerateName();
			}
			this.Notify_ThingDefSet();
		}

		// Token: 0x0600D800 RID: 55296 RVA: 0x00400754 File Offset: 0x003FE954
		protected virtual void Notify_ThingDefSet()
		{
			this.ideo.style.ResetStyleForThing(this.ThingDef);
			if (this.ThingDef.canEditAnyStyle && this.ideo.GetStyleAndCategoryFor(this.ThingDef) == null)
			{
				StyleCategoryPair styleAndCat = Precept_ThingDef.AllPossibleStylesForBuilding(this.ThingDef).RandomElement<StyleCategoryPair>();
				this.ideo.style.SetStyleForThingDef(this.ThingDef, styleAndCat);
			}
		}

		// Token: 0x0600D801 RID: 55297 RVA: 0x003FC6DE File Offset: 0x003FA8DE
		public override string GenerateNameRaw()
		{
			return this.name;
		}

		// Token: 0x0600D802 RID: 55298 RVA: 0x004007C0 File Offset: 0x003FE9C0
		public override void DrawIcon(Rect rect)
		{
			Widgets.DefIcon(rect, this.ThingDef, GenStuff.DefaultStuffFor(this.ThingDef), 1f, this.ideo.GetStyleFor(this.ThingDef), false, null, null, null, 1f);
		}

		// Token: 0x0600D803 RID: 55299 RVA: 0x00400814 File Offset: 0x003FEA14
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.ThingDef == null)
			{
				Log.Error(base.GetType().Name + " had null thingDef after loading.");
				this.ThingDef = this.def.Worker.ThingDefs.RandomElement<PreceptThingChance>().def;
			}
		}

		// Token: 0x0600D804 RID: 55300 RVA: 0x00400881 File Offset: 0x003FEA81
		public override void CopyTo(Precept other)
		{
			base.CopyTo(other);
			((Precept_ThingDef)other).thingDef = this.thingDef;
		}

		// Token: 0x04009272 RID: 37490
		private ThingDef thingDef;

		// Token: 0x04009273 RID: 37491
		private static List<StyleCategoryPair> tmpStylesForBuilding = new List<StyleCategoryPair>();

		// Token: 0x04009274 RID: 37492
		private static List<ThingDef> usedThingDefsTmp = new List<ThingDef>();
	}
}
