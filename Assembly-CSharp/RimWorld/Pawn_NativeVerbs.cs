using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020024CA RID: 9418
	public class Pawn_NativeVerbs : IVerbOwner, IExposable
	{
		// Token: 0x17001F55 RID: 8021
		// (get) Token: 0x0600CD13 RID: 52499 RVA: 0x003C2388 File Offset: 0x003C0588
		public Verb_BeatFire BeatFireVerb
		{
			get
			{
				if (this.cachedBeatFireVerb == null)
				{
					this.cachedBeatFireVerb = (Verb_BeatFire)this.verbTracker.GetVerb(VerbCategory.BeatFire);
				}
				return this.cachedBeatFireVerb;
			}
		}

		// Token: 0x17001F56 RID: 8022
		// (get) Token: 0x0600CD14 RID: 52500 RVA: 0x003C23AF File Offset: 0x003C05AF
		public Verb_Ignite IgniteVerb
		{
			get
			{
				if (this.cachedIgniteVerb == null)
				{
					this.cachedIgniteVerb = (Verb_Ignite)this.verbTracker.GetVerb(VerbCategory.Ignite);
				}
				return this.cachedIgniteVerb;
			}
		}

		// Token: 0x17001F57 RID: 8023
		// (get) Token: 0x0600CD15 RID: 52501 RVA: 0x003C23D6 File Offset: 0x003C05D6
		VerbTracker IVerbOwner.VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x17001F58 RID: 8024
		// (get) Token: 0x0600CD16 RID: 52502 RVA: 0x003C23DE File Offset: 0x003C05DE
		List<VerbProperties> IVerbOwner.VerbProperties
		{
			get
			{
				this.CheckCreateVerbProperties();
				return this.cachedVerbProperties;
			}
		}

		// Token: 0x17001F59 RID: 8025
		// (get) Token: 0x0600CD17 RID: 52503 RVA: 0x00002C42 File Offset: 0x00000E42
		List<Tool> IVerbOwner.Tools
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001F5A RID: 8026
		// (get) Token: 0x0600CD18 RID: 52504 RVA: 0x0036EC81 File Offset: 0x0036CE81
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.NativeVerb;
			}
		}

		// Token: 0x0600CD19 RID: 52505 RVA: 0x003C23EC File Offset: 0x003C05EC
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return "NativeVerbs_" + this.pawn.ThingID;
		}

		// Token: 0x0600CD1A RID: 52506 RVA: 0x003C2403 File Offset: 0x003C0603
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p == this.pawn;
		}

		// Token: 0x17001F5B RID: 8027
		// (get) Token: 0x0600CD1B RID: 52507 RVA: 0x003C240E File Offset: 0x003C060E
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17001F5C RID: 8028
		// (get) Token: 0x0600CD1C RID: 52508 RVA: 0x003C2416 File Offset: 0x003C0616
		private Thing ConstantCaster { get; }

		// Token: 0x0600CD1D RID: 52509 RVA: 0x003C241E File Offset: 0x003C061E
		public Pawn_NativeVerbs(Pawn pawn)
		{
			this.pawn = pawn;
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x0600CD1E RID: 52510 RVA: 0x003C2439 File Offset: 0x003C0639
		public void NativeVerbsTick()
		{
			this.verbTracker.VerbsTick();
		}

		// Token: 0x0600CD1F RID: 52511 RVA: 0x003C2448 File Offset: 0x003C0648
		public bool TryStartIgnite(Thing target)
		{
			if (this.IgniteVerb == null)
			{
				Pawn pawn = this.pawn;
				Log.ErrorOnce(((pawn != null) ? pawn.ToString() : null) + " tried to ignite " + ((target != null) ? target.ToString() : null) + " but has no ignite verb.", 76453432);
				return false;
			}
			return !this.pawn.stances.FullBodyBusy && this.IgniteVerb.TryStartCastOn(target, false, true, false, false);
		}

		// Token: 0x0600CD20 RID: 52512 RVA: 0x003C24C0 File Offset: 0x003C06C0
		public bool TryBeatFire(Fire targetFire)
		{
			if (this.BeatFireVerb == null)
			{
				Pawn pawn = this.pawn;
				Log.ErrorOnce(((pawn != null) ? pawn.ToString() : null) + " tried to beat fire " + ((targetFire != null) ? targetFire.ToString() : null) + " but has no beat fire verb.", 935137531);
				return false;
			}
			return !this.pawn.stances.FullBodyBusy && this.BeatFireVerb.TryStartCastOn(targetFire, false, true, false, false);
		}

		// Token: 0x0600CD21 RID: 52513 RVA: 0x003C2538 File Offset: 0x003C0738
		public void ExposeData()
		{
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600CD22 RID: 52514 RVA: 0x003C255C File Offset: 0x003C075C
		private void CheckCreateVerbProperties()
		{
			if (this.cachedVerbProperties != null)
			{
				return;
			}
			if (this.pawn.RaceProps.intelligence >= Intelligence.ToolUser || this.pawn.RaceProps.giveNonToolUserBeatFireVerb)
			{
				this.cachedVerbProperties = new List<VerbProperties>();
				this.cachedVerbProperties.Add(NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.BeatFire));
				if (!this.pawn.RaceProps.IsMechanoid && !this.pawn.RaceProps.disableIgniteVerb && this.pawn.RaceProps.intelligence >= Intelligence.ToolUser)
				{
					this.cachedVerbProperties.Add(NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.Ignite));
				}
			}
		}

		// Token: 0x04008BA8 RID: 35752
		private Pawn pawn;

		// Token: 0x04008BA9 RID: 35753
		public VerbTracker verbTracker;

		// Token: 0x04008BAA RID: 35754
		private Verb_BeatFire cachedBeatFireVerb;

		// Token: 0x04008BAB RID: 35755
		private Verb_Ignite cachedIgniteVerb;

		// Token: 0x04008BAC RID: 35756
		private List<VerbProperties> cachedVerbProperties;
	}
}
