using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200250F RID: 9487
	public abstract class Policy : IExposable, ILoadReferenceable, IRenameable
	{
		// Token: 0x17001FCD RID: 8141
		// (get) Token: 0x0600CF42 RID: 53058 RVA: 0x003CD481 File Offset: 0x003CB681
		// (set) Token: 0x0600CF43 RID: 53059 RVA: 0x003CD489 File Offset: 0x003CB689
		public string RenamableLabel
		{
			get
			{
				return this.label;
			}
			set
			{
				this.label = value;
			}
		}

		// Token: 0x17001FCE RID: 8142
		// (get) Token: 0x0600CF44 RID: 53060 RVA: 0x003CD492 File Offset: 0x003CB692
		public string BaseLabel
		{
			get
			{
				return "UnnamedPolicy".Translate();
			}
		}

		// Token: 0x17001FCF RID: 8143
		// (get) Token: 0x0600CF45 RID: 53061 RVA: 0x003CD4A3 File Offset: 0x003CB6A3
		public string InspectLabel
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x17001FD0 RID: 8144
		// (get) Token: 0x0600CF46 RID: 53062
		protected abstract string LoadKey { get; }

		// Token: 0x0600CF47 RID: 53063 RVA: 0x000024DF File Offset: 0x000006DF
		protected Policy()
		{
		}

		// Token: 0x0600CF48 RID: 53064 RVA: 0x003CD4AB File Offset: 0x003CB6AB
		protected Policy(int id, string label)
		{
			this.id = id;
			this.label = label;
		}

		// Token: 0x0600CF49 RID: 53065
		public abstract void CopyFrom(Policy other);

		// Token: 0x0600CF4A RID: 53066 RVA: 0x003CD4C1 File Offset: 0x003CB6C1
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
		}

		// Token: 0x0600CF4B RID: 53067 RVA: 0x003CD4E7 File Offset: 0x003CB6E7
		public string GetUniqueLoadID()
		{
			return string.Format("{0}_{1}_{2}", this.LoadKey, this.label, this.id);
		}

		// Token: 0x04008CFA RID: 36090
		public int id;

		// Token: 0x04008CFB RID: 36091
		public string label;
	}
}
