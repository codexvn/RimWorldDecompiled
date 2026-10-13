using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003259 RID: 12889
	public class TransferableImmutable : Transferable
	{
		// Token: 0x17002EE0 RID: 12000
		// (get) Token: 0x06012210 RID: 74256 RVA: 0x0054EA66 File Offset: 0x0054CC66
		public override Thing AnyThing
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return null;
				}
				return this.things[0];
			}
		}

		// Token: 0x17002EE1 RID: 12001
		// (get) Token: 0x06012211 RID: 74257 RVA: 0x0054EA7E File Offset: 0x0054CC7E
		public override ThingDef ThingDef
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return null;
				}
				return this.AnyThing.def;
			}
		}

		// Token: 0x17002EE2 RID: 12002
		// (get) Token: 0x06012212 RID: 74258 RVA: 0x0054EA95 File Offset: 0x0054CC95
		public override bool HasAnyThing
		{
			get
			{
				return this.things.Count != 0;
			}
		}

		// Token: 0x17002EE3 RID: 12003
		// (get) Token: 0x06012213 RID: 74259 RVA: 0x004EC5CF File Offset: 0x004EA7CF
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x17002EE4 RID: 12004
		// (get) Token: 0x06012214 RID: 74260 RVA: 0x00002501 File Offset: 0x00000701
		public override bool Interactive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002EE5 RID: 12005
		// (get) Token: 0x06012215 RID: 74261 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				return TransferablePositiveCountDirection.Destination;
			}
		}

		// Token: 0x17002EE6 RID: 12006
		// (get) Token: 0x06012216 RID: 74262 RVA: 0x004EC6A5 File Offset: 0x004EA8A5
		public override string TipDescription
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return "";
				}
				return this.AnyThing.DescriptionDetailed;
			}
		}

		// Token: 0x17002EE7 RID: 12007
		// (get) Token: 0x06012217 RID: 74263 RVA: 0x00002501 File Offset: 0x00000701
		// (set) Token: 0x06012218 RID: 74264 RVA: 0x0054EAA5 File Offset: 0x0054CCA5
		public override int CountToTransfer
		{
			get
			{
				return 0;
			}
			protected set
			{
				if (value != 0)
				{
					throw new InvalidOperationException("immutable transferable");
				}
			}
		}

		// Token: 0x17002EE8 RID: 12008
		// (get) Token: 0x06012219 RID: 74265 RVA: 0x0054EAB8 File Offset: 0x0054CCB8
		public string LabelWithTotalStackCount
		{
			get
			{
				string text = this.Label;
				int totalStackCount = this.TotalStackCount;
				if (totalStackCount != 1)
				{
					text = text + " x" + totalStackCount.ToStringCached();
				}
				return text;
			}
		}

		// Token: 0x17002EE9 RID: 12009
		// (get) Token: 0x0601221A RID: 74266 RVA: 0x0054EAEA File Offset: 0x0054CCEA
		public string LabelCapWithTotalStackCount
		{
			get
			{
				return this.LabelWithTotalStackCount.CapitalizeFirst(this.ThingDef);
			}
		}

		// Token: 0x17002EEA RID: 12010
		// (get) Token: 0x0601221B RID: 74267 RVA: 0x0054EB00 File Offset: 0x0054CD00
		public int TotalStackCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.things.Count; i++)
				{
					num += this.things[i].stackCount;
				}
				return num;
			}
		}

		// Token: 0x0601221C RID: 74268 RVA: 0x00002501 File Offset: 0x00000701
		public override int GetMinimumToTransfer()
		{
			return 0;
		}

		// Token: 0x0601221D RID: 74269 RVA: 0x00002501 File Offset: 0x00000701
		public override int GetMaximumToTransfer()
		{
			return 0;
		}

		// Token: 0x0601221E RID: 74270 RVA: 0x000F9927 File Offset: 0x000F7B27
		public override AcceptanceReport OverflowReport()
		{
			return false;
		}

		// Token: 0x0601221F RID: 74271 RVA: 0x0054EB3C File Offset: 0x0054CD3C
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.things.RemoveAll((Thing x) => x.Destroyed);
			}
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.things.RemoveAll((Thing x) => x == null) != 0)
				{
					Log.Warning("Some of the things were null after loading.");
				}
			}
		}

		// Token: 0x0400BC40 RID: 48192
		public List<Thing> things = new List<Thing>();
	}
}
