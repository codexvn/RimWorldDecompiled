using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000714 RID: 1812
	public class HediffComp_VerbGiver : HediffComp, IVerbOwner
	{
		// Token: 0x170007D7 RID: 2007
		// (get) Token: 0x06002C40 RID: 11328 RVA: 0x000E5F2F File Offset: 0x000E412F
		public HediffCompProperties_VerbGiver Props
		{
			get
			{
				return (HediffCompProperties_VerbGiver)this.props;
			}
		}

		// Token: 0x170007D8 RID: 2008
		// (get) Token: 0x06002C41 RID: 11329 RVA: 0x000E5F3C File Offset: 0x000E413C
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x170007D9 RID: 2009
		// (get) Token: 0x06002C42 RID: 11330 RVA: 0x000E5F44 File Offset: 0x000E4144
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.Props.verbs;
			}
		}

		// Token: 0x170007DA RID: 2010
		// (get) Token: 0x06002C43 RID: 11331 RVA: 0x000E5F51 File Offset: 0x000E4151
		public List<Tool> Tools
		{
			get
			{
				return this.Props.tools;
			}
		}

		// Token: 0x170007DB RID: 2011
		// (get) Token: 0x06002C44 RID: 11332 RVA: 0x000E5F5E File Offset: 0x000E415E
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return base.Pawn;
			}
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06002C45 RID: 11333 RVA: 0x000E5F66 File Offset: 0x000E4166
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return this.Props.ownerTypeOverride ?? ImplementOwnerTypeDefOf.Hediff;
			}
		}

		// Token: 0x06002C46 RID: 11334 RVA: 0x000E5F7C File Offset: 0x000E417C
		public HediffComp_VerbGiver()
		{
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x000E5F90 File Offset: 0x000E4190
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.verbTracker == null)
			{
				this.verbTracker = new VerbTracker(this);
			}
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x000E5FCE File Offset: 0x000E41CE
		public override void CompPostTick(ref float _)
		{
			this.verbTracker.VerbsTick();
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x000E5FDC File Offset: 0x000E41DC
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return this.parent.GetUniqueLoadID() + "_" + this.parent.comps.IndexOf(this).ToString();
		}

		// Token: 0x06002C4A RID: 11338 RVA: 0x000E6017 File Offset: 0x000E4217
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p.health.hediffSet.hediffs.Contains(this.parent);
		}

		// Token: 0x04002389 RID: 9097
		public VerbTracker verbTracker;
	}
}
