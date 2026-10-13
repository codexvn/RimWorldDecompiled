using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200325B RID: 12891
	public class TransferableOneWay : Transferable
	{
		// Token: 0x17002EEB RID: 12011
		// (get) Token: 0x06012225 RID: 74277 RVA: 0x0054EBF5 File Offset: 0x0054CDF5
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

		// Token: 0x17002EEC RID: 12012
		// (get) Token: 0x06012226 RID: 74278 RVA: 0x0054EA7E File Offset: 0x0054CC7E
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

		// Token: 0x17002EED RID: 12013
		// (get) Token: 0x06012227 RID: 74279 RVA: 0x0054EC0D File Offset: 0x0054CE0D
		public override bool HasAnyThing
		{
			get
			{
				return this.things.Count != 0;
			}
		}

		// Token: 0x17002EEE RID: 12014
		// (get) Token: 0x06012228 RID: 74280 RVA: 0x004EC5CF File Offset: 0x004EA7CF
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x17002EEF RID: 12015
		// (get) Token: 0x06012229 RID: 74281 RVA: 0x0054EC1D File Offset: 0x0054CE1D
		public override bool Interactive
		{
			get
			{
				return this.interactive;
			}
		}

		// Token: 0x17002EF0 RID: 12016
		// (get) Token: 0x0601222A RID: 74282 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				return TransferablePositiveCountDirection.Destination;
			}
		}

		// Token: 0x17002EF1 RID: 12017
		// (get) Token: 0x0601222B RID: 74283 RVA: 0x004EC6A5 File Offset: 0x004EA8A5
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

		// Token: 0x17002EF2 RID: 12018
		// (get) Token: 0x0601222C RID: 74284 RVA: 0x0054EC25 File Offset: 0x0054CE25
		// (set) Token: 0x0601222D RID: 74285 RVA: 0x0054EC2D File Offset: 0x0054CE2D
		public override int CountToTransfer
		{
			get
			{
				return this.countToTransfer;
			}
			protected set
			{
				this.countToTransfer = value;
				base.EditBuffer = value.ToStringCached();
			}
		}

		// Token: 0x17002EF3 RID: 12019
		// (get) Token: 0x0601222E RID: 74286 RVA: 0x0054EC44 File Offset: 0x0054CE44
		public int MaxCount
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

		// Token: 0x0601222F RID: 74287 RVA: 0x00002501 File Offset: 0x00000701
		public override int GetMinimumToTransfer()
		{
			return 0;
		}

		// Token: 0x06012230 RID: 74288 RVA: 0x0054EC7E File Offset: 0x0054CE7E
		public override int GetMaximumToTransfer()
		{
			return this.MaxCount;
		}

		// Token: 0x06012231 RID: 74289 RVA: 0x0054EC86 File Offset: 0x0054CE86
		public override AcceptanceReport OverflowReport()
		{
			return new AcceptanceReport("ColonyHasNoMore".Translate());
		}

		// Token: 0x06012232 RID: 74290 RVA: 0x0054EC9C File Offset: 0x0054CE9C
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.things.RemoveAll((Thing x) => x.Destroyed);
			}
			Scribe_Values.Look<int>(ref this.countToTransfer, "countToTransfer", 0, false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.interactive, "interactive", true, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				base.EditBuffer = this.countToTransfer.ToStringCached();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.things.RemoveAll((Thing x) => x == null) != 0)
				{
					Log.Warning("Some of the things were null after loading.");
				}
			}
		}

		// Token: 0x0400BC44 RID: 48196
		public List<Thing> things = new List<Thing>();

		// Token: 0x0400BC45 RID: 48197
		private int countToTransfer;

		// Token: 0x0400BC46 RID: 48198
		public bool interactive = true;
	}
}
