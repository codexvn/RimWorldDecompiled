using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002500 RID: 9472
	public class DrugPolicy : Policy
	{
		// Token: 0x17001FBD RID: 8125
		// (get) Token: 0x0600CEEE RID: 52974 RVA: 0x003CC2C8 File Offset: 0x003CA4C8
		public int Count
		{
			get
			{
				return this.entriesInt.Count;
			}
		}

		// Token: 0x17001FBE RID: 8126
		// (get) Token: 0x0600CEEF RID: 52975 RVA: 0x003CC2D5 File Offset: 0x003CA4D5
		protected override string LoadKey
		{
			get
			{
				return "DrugPolicy";
			}
		}

		// Token: 0x17001FBF RID: 8127
		public DrugPolicyEntry this[int index]
		{
			get
			{
				return this.entriesInt[index];
			}
			set
			{
				this.entriesInt[index] = value;
			}
		}

		// Token: 0x17001FC0 RID: 8128
		public DrugPolicyEntry this[ThingDef drugDef]
		{
			get
			{
				for (int i = 0; i < this.entriesInt.Count; i++)
				{
					if (this.entriesInt[i].drug == drugDef)
					{
						return this.entriesInt[i];
					}
				}
				throw new ArgumentException();
			}
		}

		// Token: 0x0600CEF3 RID: 52979 RVA: 0x003CC345 File Offset: 0x003CA545
		public DrugPolicy()
		{
		}

		// Token: 0x0600CEF4 RID: 52980 RVA: 0x003CC34D File Offset: 0x003CA54D
		public DrugPolicy(int id, string label) : base(id, label)
		{
			this.InitializeIfNeeded(true);
		}

		// Token: 0x0600CEF5 RID: 52981 RVA: 0x003CC360 File Offset: 0x003CA560
		private void InitializeIfNeeded(bool overwriteExisting = true)
		{
			if (overwriteExisting)
			{
				if (this.entriesInt != null)
				{
					return;
				}
				this.entriesInt = new List<DrugPolicyEntry>();
			}
			List<ThingDef> thingDefs = DefDatabase<ThingDef>.AllDefsListForReading;
			int i;
			Predicate<DrugPolicyEntry> <>9__1;
			int j;
			for (i = 0; i < thingDefs.Count; i = j + 1)
			{
				if (thingDefs[i].category == ThingCategory.Item && thingDefs[i].IsDrug)
				{
					if (!overwriteExisting)
					{
						List<DrugPolicyEntry> list = this.entriesInt;
						Predicate<DrugPolicyEntry> predicate;
						if ((predicate = <>9__1) == null)
						{
							predicate = (<>9__1 = ((DrugPolicyEntry x) => x.drug == thingDefs[i]));
						}
						if (list.Any(predicate))
						{
							goto IL_C2;
						}
					}
					this.entriesInt.Add(new DrugPolicyEntry
					{
						drug = thingDefs[i],
						allowedForAddiction = true
					});
				}
				IL_C2:
				j = i;
			}
			this.entriesInt.SortBy((DrugPolicyEntry e) => e.drug.GetCompProperties<CompProperties_Drug>().listOrder);
		}

		// Token: 0x0600CEF6 RID: 52982 RVA: 0x003CC480 File Offset: 0x003CA680
		public override void CopyFrom(Policy other)
		{
			DrugPolicy drugPolicy = other as DrugPolicy;
			if (drugPolicy == null)
			{
				return;
			}
			this.sourceDef = drugPolicy.sourceDef;
			this.entriesInt.Clear();
			foreach (DrugPolicyEntry other2 in drugPolicy.entriesInt)
			{
				DrugPolicyEntry drugPolicyEntry = new DrugPolicyEntry();
				drugPolicyEntry.CopyFrom(other2);
				this.entriesInt.Add(drugPolicyEntry);
			}
		}

		// Token: 0x0600CEF7 RID: 52983 RVA: 0x003CC508 File Offset: 0x003CA708
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<DrugPolicyEntry>(ref this.entriesInt, "drugs", LookMode.Deep, Array.Empty<object>());
			Scribe_Defs.Look<DrugPolicyDef>(ref this.sourceDef, "sourceDef");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.entriesInt != null)
			{
				if (this.entriesInt.RemoveAll((DrugPolicyEntry x) => x == null || x.drug == null) != 0)
				{
					Log.Error("Some DrugPolicyEntries were null after loading.");
				}
				this.InitializeIfNeeded(false);
			}
		}

		// Token: 0x04008CDA RID: 36058
		public DrugPolicyDef sourceDef;

		// Token: 0x04008CDB RID: 36059
		private List<DrugPolicyEntry> entriesInt;
	}
}
