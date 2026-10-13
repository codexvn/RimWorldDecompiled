using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000656 RID: 1622
	public class Gene : IExposable, ILoadReferenceable
	{
		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x060028D6 RID: 10454 RVA: 0x000D7ACB File Offset: 0x000D5CCB
		public virtual string Label
		{
			get
			{
				return this.def.label;
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x060028D7 RID: 10455 RVA: 0x000D7AD8 File Offset: 0x000D5CD8
		public virtual string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x060028D8 RID: 10456 RVA: 0x000D7AE5 File Offset: 0x000D5CE5
		public bool Overridden
		{
			get
			{
				return this.overriddenByGene != null;
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x060028D9 RID: 10457 RVA: 0x000D7AF0 File Offset: 0x000D5CF0
		public virtual bool Active
		{
			get
			{
				if (this.Overridden)
				{
					return false;
				}
				Pawn pawn = this.pawn;
				if (((pawn != null) ? pawn.ageTracker : null) != null && (float)this.pawn.ageTracker.AgeBiologicalYears < this.def.minAgeActive)
				{
					return false;
				}
				Pawn pawn2 = this.pawn;
				return ((pawn2 != null) ? pawn2.mutant : null) == null || !this.pawn.mutant.Def.disablesGenes.Contains(this.def);
			}
		}

		// Token: 0x1700072A RID: 1834
		// (get) Token: 0x060028DA RID: 10458 RVA: 0x000D7B74 File Offset: 0x000D5D74
		public IEnumerable<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				if (!this.Active)
				{
					yield break;
				}
				List<WorkTypeDef> list = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					if ((this.def.disabledWorkTags & list[i].workTags) != WorkTags.None)
					{
						yield return list[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostMake()
		{
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x000D7B84 File Offset: 0x000D5D84
		public virtual void PostAdd()
		{
			if (this.def.HasDefinedGraphicProperties)
			{
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x000D7B84 File Offset: 0x000D5D84
		public virtual void PostRemove()
		{
			if (this.def.HasDefinedGraphicProperties)
			{
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Tick()
		{
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x000D7BA8 File Offset: 0x000D5DA8
		public virtual void TickInterval(int delta)
		{
			if (ModsConfig.BiotechActive && this.def.mentalBreakMtbDays > 0f && this.def.mentalBreakDef != null && this.pawn.Spawned && this.pawn.IsHashIntervalTick(60, delta) && !this.pawn.InMentalState && !this.pawn.Downed && Rand.MTBEventOccurs(this.def.mentalBreakMtbDays, 60000f, 60f) && this.def.mentalBreakDef.Worker.BreakCanOccur(this.pawn))
			{
				this.def.mentalBreakDef.Worker.TryStart(this.pawn, "MentalStateReason_Gene".Translate() + ": " + this.LabelCap, false);
			}
		}

		// Token: 0x060028E0 RID: 10464 RVA: 0x000D7C9E File Offset: 0x000D5E9E
		public void OverrideBy(Gene overriddenBy)
		{
			if (ModsConfig.BiotechActive)
			{
				this.overriddenByGene = overriddenBy;
			}
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x000D7CB0 File Offset: 0x000D5EB0
		public Passion NewPassionForOnRemoval(SkillRecord skillRecord)
		{
			if (this.def.passionMod == null)
			{
				return skillRecord.passion;
			}
			PassionMod.PassionModType modType = this.def.passionMod.modType;
			if (modType != PassionMod.PassionModType.AddOneLevel)
			{
				if (modType == PassionMod.PassionModType.DropAll)
				{
					if (this.passionPreAdd != null)
					{
						return this.passionPreAdd.Value;
					}
					return skillRecord.passion;
				}
			}
			else
			{
				Passion passion = skillRecord.passion;
				if (passion == Passion.Minor)
				{
					return Passion.None;
				}
				if (passion == Passion.Major)
				{
					return Passion.Minor;
				}
			}
			return skillRecord.passion;
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_IngestedThing(Thing thing, int numTaken)
		{
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_NewColony()
		{
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x000D7D25 File Offset: 0x000D5F25
		public virtual void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			if (this.def.deathHistoryEvent != null)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(this.def.deathHistoryEvent), true);
			}
		}

		// Token: 0x060028E5 RID: 10469 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Reset()
		{
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			return null;
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			return null;
		}

		// Token: 0x060028E8 RID: 10472 RVA: 0x000D7D4F File Offset: 0x000D5F4F
		public string GetUniqueLoadID()
		{
			return "Gene_" + this.loadID.ToString();
		}

		// Token: 0x060028E9 RID: 10473 RVA: 0x000D7D68 File Offset: 0x000D5F68
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<GeneDef>(ref this.def, "def");
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", true);
			Scribe_References.Look<Gene>(ref this.overriddenByGene, "overriddenByGene", false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<Passion?>(ref this.passionPreAdd, "passionPreAdd", null, false);
		}

		// Token: 0x0400218D RID: 8589
		public GeneDef def;

		// Token: 0x0400218E RID: 8590
		public Pawn pawn;

		// Token: 0x0400218F RID: 8591
		public int loadID;

		// Token: 0x04002190 RID: 8592
		public Gene overriddenByGene;

		// Token: 0x04002191 RID: 8593
		public Passion? passionPreAdd;
	}
}
