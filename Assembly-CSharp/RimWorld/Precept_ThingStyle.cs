using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002673 RID: 9843
	public class Precept_ThingStyle : Precept_ThingDef
	{
		// Token: 0x1700211A RID: 8474
		// (get) Token: 0x0600D80C RID: 55308 RVA: 0x003FC6B6 File Offset: 0x003FA8B6
		public override string UIInfoFirstLine
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x1700211B RID: 8475
		// (get) Token: 0x0600D80D RID: 55309 RVA: 0x004008D2 File Offset: 0x003FEAD2
		public override string UIInfoSecondLine
		{
			get
			{
				return base.LabelCap;
			}
		}

		// Token: 0x1700211C RID: 8476
		// (get) Token: 0x0600D80E RID: 55310 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool UsesGeneratedName
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700211D RID: 8477
		// (get) Token: 0x0600D80F RID: 55311 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CanRegenerate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700211E RID: 8478
		// (get) Token: 0x0600D810 RID: 55312 RVA: 0x00002501 File Offset: 0x00000701
		public override bool SortByImpact
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700211F RID: 8479
		// (get) Token: 0x0600D811 RID: 55313 RVA: 0x004008DA File Offset: 0x003FEADA
		protected virtual string NameRootSymbol
		{
			get
			{
				return "root";
			}
		}

		// Token: 0x17002120 RID: 8480
		// (get) Token: 0x0600D812 RID: 55314 RVA: 0x003FC40E File Offset: 0x003FA60E
		protected virtual string ThingLabelCap
		{
			get
			{
				return base.ThingDef.LabelCap;
			}
		}

		// Token: 0x0600D813 RID: 55315 RVA: 0x004008E4 File Offset: 0x003FEAE4
		public override string GenerateNameRaw()
		{
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(this.def.nameMaker);
			base.AddIdeoRulesTo(ref request);
			if (base.ThingDef.ideoBuildingNamerBase != null)
			{
				request.Includes.Add(base.ThingDef.ideoBuildingNamerBase);
			}
			string thingLabelCap = this.ThingLabelCap;
			if (thingLabelCap != null)
			{
				request.Rules.Add(new Rule_String("thingLabel", thingLabelCap));
			}
			string text = null;
			int i = 100;
			while (i > 0)
			{
				text = GrammarResolver.Resolve(this.NameRootSymbol, request, null, false, null, null, null, false);
				i--;
				if (text.Length <= this.def.nameMaxLength)
				{
					break;
				}
			}
			return GenText.CapitalizeAsTitle(text);
		}

		// Token: 0x0600D814 RID: 55316 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_ThingLost(Thing thing, bool destroyed = false)
		{
		}

		// Token: 0x0600D815 RID: 55317 RVA: 0x000444B9 File Offset: 0x000426B9
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(Thing thing)
		{
			return Enumerable.Empty<StatDrawEntry>();
		}

		// Token: 0x0600D816 RID: 55318 RVA: 0x00009C2B File Offset: 0x00007E2B
		public virtual string TransformThingLabel(string label)
		{
			return label;
		}

		// Token: 0x0600D817 RID: 55319 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string InspectStringExtra(Thing thing)
		{
			return null;
		}
	}
}
