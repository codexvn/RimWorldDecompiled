using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002762 RID: 10082
	public abstract class RitualOutcomeEffectWorker : IExposable
	{
		// Token: 0x170021D6 RID: 8662
		// (get) Token: 0x0600DD1C RID: 56604 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ApplyOnFailure
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170021D7 RID: 8663
		// (get) Token: 0x0600DD1D RID: 56605 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170021D8 RID: 8664
		// (get) Token: 0x0600DD1E RID: 56606 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool ShowQuality
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170021D9 RID: 8665
		// (get) Token: 0x0600DD1F RID: 56607 RVA: 0x00410F83 File Offset: 0x0040F183
		public virtual string Description
		{
			get
			{
				return this.def.Description;
			}
		}

		// Token: 0x0600DD20 RID: 56608 RVA: 0x000024DF File Offset: 0x000006DF
		public RitualOutcomeEffectWorker()
		{
		}

		// Token: 0x0600DD21 RID: 56609 RVA: 0x00410F90 File Offset: 0x0040F190
		public RitualOutcomeEffectWorker(RitualOutcomeEffectDef def)
		{
			this.def = def;
			this.FillCompData();
		}

		// Token: 0x0600DD22 RID: 56610 RVA: 0x00410FA8 File Offset: 0x0040F1A8
		public void ResetCompDatas()
		{
			foreach (RitualOutcomeComp_Data ritualOutcomeComp_Data in this.compDatas)
			{
				if (ritualOutcomeComp_Data != null)
				{
					ritualOutcomeComp_Data.Reset();
				}
			}
		}

		// Token: 0x0600DD23 RID: 56611 RVA: 0x00411000 File Offset: 0x0040F200
		public void FillCompData()
		{
			if ((this.compDatas == null || this.compDatas.Count != this.def.comps.Count) && !this.def.comps.NullOrEmpty<RitualOutcomeComp>())
			{
				this.compDatas = new List<RitualOutcomeComp_Data>();
				foreach (RitualOutcomeComp ritualOutcomeComp in this.def.comps)
				{
					this.compDatas.Add(ritualOutcomeComp.MakeData());
				}
			}
		}

		// Token: 0x0600DD24 RID: 56612 RVA: 0x004110A4 File Offset: 0x0040F2A4
		public RitualOutcomeComp_Data DataForComp(RitualOutcomeComp comp)
		{
			int num = this.def.comps.IndexOf(comp);
			if (num != -1)
			{
				return this.compDatas[num];
			}
			Log.Error("Can't find index for " + comp.GetType().Name);
			return null;
		}

		// Token: 0x0600DD25 RID: 56613
		public abstract void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual);

		// Token: 0x0600DD26 RID: 56614 RVA: 0x004110F0 File Offset: 0x0040F2F0
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<RitualOutcomeEffectDef>(ref this.def, "def");
			Scribe_Collections.Look<RitualOutcomeComp_Data>(ref this.compDatas, "compDatas", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && (this.compDatas.NullOrEmpty<RitualOutcomeComp_Data>() || this.compDatas.Count != this.def.comps.Count))
			{
				this.FillCompData();
			}
		}

		// Token: 0x0600DD27 RID: 56615 RVA: 0x0041115B File Offset: 0x0040F35B
		public Thought_Memory MakeMemory(Pawn p, LordJob_Ritual ritual, ThoughtDef overrideDef = null)
		{
			Thought_Memory thought_Memory = (Thought_Memory)ThoughtMaker.MakeThought(overrideDef ?? this.def.memoryDef);
			thought_Memory.sourcePrecept = ritual.Ritual;
			return thought_Memory;
		}

		// Token: 0x0600DD28 RID: 56616 RVA: 0x00411184 File Offset: 0x0040F384
		public virtual void Tick(LordJob_Ritual ritual, float progressAmount = 1f)
		{
			if (!this.def.comps.NullOrEmpty<RitualOutcomeComp>())
			{
				foreach (RitualOutcomeComp ritualOutcomeComp in this.def.comps)
				{
					ritualOutcomeComp.Tick(ritual, this.DataForComp(ritualOutcomeComp), progressAmount);
				}
			}
		}

		// Token: 0x0600DD29 RID: 56617 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string ExtraAlertParagraph(Precept_Ritual ritual)
		{
			return null;
		}

		// Token: 0x0600DD2A RID: 56618 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string ExpectedQualityLabel()
		{
			return null;
		}

		// Token: 0x0600DD2B RID: 56619 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual RitualOutcomePossibility GetForcedOutcome(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			return null;
		}

		// Token: 0x0600DD2C RID: 56620 RVA: 0x004111F8 File Offset: 0x0040F3F8
		public virtual IEnumerable<string> BlockingIssues(Precept_Ritual ritual, TargetInfo target, RitualRoleAssignments assignments)
		{
			if (!this.def.comps.NullOrEmpty<RitualOutcomeComp>())
			{
				foreach (RitualOutcomeComp ritualOutcomeComp in this.def.comps)
				{
					foreach (string text in ritualOutcomeComp.BlockingIssues(ritual, target, assignments))
					{
						yield return text;
					}
					IEnumerator<string> enumerator2 = null;
				}
				List<RitualOutcomeComp>.Enumerator enumerator = default(List<RitualOutcomeComp>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x0400952C RID: 38188
		public RitualOutcomeEffectDef def;

		// Token: 0x0400952D RID: 38189
		public List<RitualOutcomeComp_Data> compDatas;
	}
}
