using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000686 RID: 1670
	public class Hediff : IExposable, ILoadReferenceable
	{
		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x060029CA RID: 10698 RVA: 0x000DD249 File Offset: 0x000DB449
		public virtual string LabelBase
		{
			get
			{
				HediffStage curStage = this.CurStage;
				return ((curStage != null) ? curStage.overrideLabel : null) ?? this.def.label;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x060029CB RID: 10699 RVA: 0x000DD26C File Offset: 0x000DB46C
		public string LabelBaseCap
		{
			get
			{
				return this.LabelBase.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x060029CC RID: 10700 RVA: 0x000DD280 File Offset: 0x000DB480
		public virtual string Label
		{
			get
			{
				string labelInBrackets = this.LabelInBrackets;
				return this.LabelBase + (labelInBrackets.NullOrEmpty() ? "" : (" (" + labelInBrackets + ")"));
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x060029CD RID: 10701 RVA: 0x000DD2BE File Offset: 0x000DB4BE
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.def);
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x060029CE RID: 10702 RVA: 0x000DD2D1 File Offset: 0x000DB4D1
		public virtual Color LabelColor
		{
			get
			{
				return this.def.defaultLabelColor;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x060029CF RID: 10703 RVA: 0x000DD2DE File Offset: 0x000DB4DE
		public virtual string LabelInBrackets
		{
			get
			{
				if (this.CurStage != null && !this.CurStage.label.NullOrEmpty())
				{
					return this.CurStage.label;
				}
				return null;
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x060029D0 RID: 10704 RVA: 0x000DD307 File Offset: 0x000DB507
		public virtual string SeverityLabel
		{
			get
			{
				if (!this.IsLethal && !this.def.alwaysShowSeverity)
				{
					return null;
				}
				return (this.Severity / Mathf.Abs(this.def.lethalSeverity)).ToStringPercent();
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x060029D1 RID: 10705 RVA: 0x000DD33C File Offset: 0x000DB53C
		public virtual int UIGroupKey
		{
			get
			{
				return this.Label.GetHashCode();
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x060029D2 RID: 10706 RVA: 0x000DD34C File Offset: 0x000DB54C
		public virtual string TipStringExtra
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (StatDrawEntry statDrawEntry in HediffStatsUtility.SpecialDisplayStats(this.CurStage, this))
				{
					if (statDrawEntry.ShouldDisplay(null))
					{
						stringBuilder.Append("  - " + statDrawEntry.LabelCap + ": " + statDrawEntry.ValueString);
						HediffStage curStage = this.CurStage;
						if (((curStage != null) ? curStage.statOffsetEffectMultiplier : null) != null)
						{
							stringBuilder.Append(string.Format(" x {0}", this.CurStage.statOffsetEffectMultiplier.LabelCap));
						}
						else
						{
							HediffStage curStage2 = this.CurStage;
							if (((curStage2 != null) ? curStage2.statFactorEffectMultiplier : null) != null)
							{
								stringBuilder.Append(string.Format(" x {0}", this.CurStage.statFactorEffectMultiplier.LabelCap));
							}
						}
						stringBuilder.AppendLine();
					}
				}
				if (ModsConfig.AnomalyActive && !this.def.aptitudes.NullOrEmpty<Aptitude>())
				{
					stringBuilder.AppendLine((from x in this.def.aptitudes
					select x.skill.LabelCap.ToString() + " " + x.level.ToStringWithSign()).ToLineList("  - ", true));
				}
				HediffStage stage = this.CurStage;
				if (stage != null)
				{
					if (!stage.enablesNeeds.NullOrEmpty<NeedDef>())
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.AppendLine();
						}
						stringBuilder.AppendLine(("AddsNeeds".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
						stringBuilder.AppendLine((from x in stage.enablesNeeds
						select x.LabelCap.ToString()).ToLineList("  - ", false));
					}
					if (!stage.disablesNeeds.NullOrEmpty<NeedDef>())
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.AppendLine();
						}
						stringBuilder.AppendLine(("DisabledNeedsLabel".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
						stringBuilder.AppendLine((from x in stage.disablesNeeds
						select x.LabelCap.ToString()).ToLineList("  - ", false));
					}
					if (stage.disabledWorkTags != WorkTags.None)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.AppendLine();
						}
						stringBuilder.AppendLine(("DisabledWorkLabel".Translate().CapitalizeFirst() + ":").Colorize(ColoredText.TipSectionTitleColor));
						IEnumerable<string> items = from x in DefDatabase<WorkTypeDef>.AllDefsListForReading
						where (stage.disabledWorkTags & x.workTags) > WorkTags.None
						select x.labelShort;
						stringBuilder.Append("  - " + items.ToCommaList(false, false).CapitalizeFirst());
					}
				}
				if (this.def.CompProps<HediffCompProperties_GiveLovinMTBFactor>() != null)
				{
					stringBuilder.AppendLine("  - " + "IncreasesChanceOfLovin".Translate());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x060029D3 RID: 10707 RVA: 0x000DD6C4 File Offset: 0x000DB8C4
		public virtual HediffStage CurStage
		{
			get
			{
				if (!this.def.stages.NullOrEmpty<HediffStage>())
				{
					return this.def.stages[this.CurStageIndex];
				}
				return null;
			}
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x060029D4 RID: 10708 RVA: 0x000DD6F0 File Offset: 0x000DB8F0
		public virtual bool ShouldRemove
		{
			get
			{
				return this.Severity <= 0f;
			}
		}

		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x060029D5 RID: 10709 RVA: 0x000DD702 File Offset: 0x000DB902
		public virtual bool Visible
		{
			get
			{
				return this.visible || this.CurStage == null || this.CurStage.becomeVisible;
			}
		}

		// Token: 0x17000740 RID: 1856
		// (get) Token: 0x060029D6 RID: 10710 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float BleedRate
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000741 RID: 1857
		// (get) Token: 0x060029D7 RID: 10711 RVA: 0x000DD721 File Offset: 0x000DB921
		public virtual float BleedRateScaled
		{
			get
			{
				return this.BleedRate / this.pawn.HealthScale;
			}
		}

		// Token: 0x17000742 RID: 1858
		// (get) Token: 0x060029D8 RID: 10712 RVA: 0x000DD735 File Offset: 0x000DB935
		public bool Bleeding
		{
			get
			{
				return this.BleedRate > 1E-05f;
			}
		}

		// Token: 0x17000743 RID: 1859
		// (get) Token: 0x060029D9 RID: 10713 RVA: 0x000DD744 File Offset: 0x000DB944
		public virtual float PainOffset
		{
			get
			{
				if (this.CurStage != null && !this.causesNoPain)
				{
					return this.CurStage.painOffset;
				}
				return 0f;
			}
		}

		// Token: 0x17000744 RID: 1860
		// (get) Token: 0x060029DA RID: 10714 RVA: 0x000DD767 File Offset: 0x000DB967
		public virtual float PainFactor
		{
			get
			{
				HediffStage curStage = this.CurStage;
				if (curStage == null)
				{
					return 1f;
				}
				return curStage.painFactor;
			}
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x060029DB RID: 10715 RVA: 0x000DD77E File Offset: 0x000DB97E
		public List<PawnCapacityModifier> CapMods
		{
			get
			{
				HediffStage curStage = this.CurStage;
				if (curStage == null)
				{
					return null;
				}
				return curStage.capMods;
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x060029DC RID: 10716 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float SummaryHealthPercentImpact
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x060029DD RID: 10717 RVA: 0x000DD794 File Offset: 0x000DB994
		public virtual float TendPriority
		{
			get
			{
				float num = 0f;
				HediffStage curStage = this.CurStage;
				if (curStage != null && curStage.lifeThreatening)
				{
					num = Mathf.Max(num, 1f);
				}
				num = Mathf.Max(num, this.BleedRate * 1.5f);
				HediffComp_TendDuration hediffComp_TendDuration = this.TryGetComp<HediffComp_TendDuration>();
				if (hediffComp_TendDuration != null && hediffComp_TendDuration.TProps.severityPerDayTended < 0f)
				{
					num = Mathf.Max(num, 0.025f);
				}
				return num;
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x060029DE RID: 10718 RVA: 0x000DD801 File Offset: 0x000DBA01
		public virtual TextureAndColor StateIcon
		{
			get
			{
				return TextureAndColor.None;
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x060029DF RID: 10719 RVA: 0x000DD808 File Offset: 0x000DBA08
		public virtual int CurStageIndex
		{
			get
			{
				return this.def.StageAtSeverity(this.Severity);
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x060029E0 RID: 10720 RVA: 0x000DD81B File Offset: 0x000DBA1B
		// (set) Token: 0x060029E1 RID: 10721 RVA: 0x000DD824 File Offset: 0x000DBA24
		public virtual float Severity
		{
			get
			{
				return this.severityInt;
			}
			set
			{
				bool flag = false;
				if (this.IsLethal && value >= this.def.lethalSeverity)
				{
					value = this.def.lethalSeverity;
					flag = true;
				}
				bool flag2 = this is Hediff_Injury && value > this.severityInt && Mathf.RoundToInt(value) != Mathf.RoundToInt(this.severityInt);
				int curStageIndex = this.CurStageIndex;
				this.severityInt = Mathf.Clamp(value, this.def.minSeverity, this.def.maxSeverity);
				if (this.CurStageIndex != curStageIndex)
				{
					this.OnStageIndexChanged(this.CurStageIndex);
				}
				if ((this.CurStageIndex != curStageIndex || flag || flag2) && this.pawn.health.hediffSet.hediffs.Contains(this))
				{
					this.pawn.health.Notify_HediffChanged(this);
					if (!this.pawn.Dead && this.pawn.needs.mood != null)
					{
						this.pawn.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
					}
				}
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x060029E2 RID: 10722 RVA: 0x000DD93F File Offset: 0x000DBB3F
		// (set) Token: 0x060029E3 RID: 10723 RVA: 0x000DD947 File Offset: 0x000DBB47
		public BodyPartRecord Part
		{
			get
			{
				return this.part;
			}
			set
			{
				if (this.pawn == null && value != null)
				{
					Log.Error("Hediff: Cannot set Part without setting pawn first.");
					return;
				}
				this.part = value;
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x060029E4 RID: 10724 RVA: 0x000DD966 File Offset: 0x000DBB66
		public bool IsLethal
		{
			get
			{
				return this.def.lethalSeverity > 0f && this.canBeThreateningToPart;
			}
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x060029E5 RID: 10725 RVA: 0x000DD982 File Offset: 0x000DBB82
		public bool IsCurrentlyLifeThreatening
		{
			get
			{
				return this.IsStageLifeThreatening(this.CurStage);
			}
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x060029E6 RID: 10726 RVA: 0x000DD990 File Offset: 0x000DBB90
		public List<Ability> AllAbilitiesForReading
		{
			get
			{
				if (this.abilities == null && !this.def.abilities.NullOrEmpty<AbilityDef>())
				{
					this.abilities = new List<Ability>();
					foreach (AbilityDef abilityDef in this.def.abilities)
					{
						this.abilities.Add(AbilityUtility.MakeAbility(abilityDef, this.pawn));
					}
				}
				return this.abilities;
			}
		}

		// Token: 0x060029E7 RID: 10727 RVA: 0x000DDA24 File Offset: 0x000DBC24
		public virtual bool TendableNow(bool ignoreTimer = false)
		{
			if (!this.def.tendable || this.Severity <= 0f || this.FullyImmune() || !this.Visible || this.IsPermanent() || !this.pawn.RaceProps.IsFlesh || (this is Hediff_Injury && !this.pawn.health.CanBleed))
			{
				return false;
			}
			if (!ignoreTimer)
			{
				HediffComp_TendDuration hediffComp_TendDuration = this.TryGetComp<HediffComp_TendDuration>();
				if (hediffComp_TendDuration != null && !hediffComp_TendDuration.AllowTend)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060029E8 RID: 10728 RVA: 0x000DDAA9 File Offset: 0x000DBCA9
		public bool IsStageLifeThreatening(HediffStage stage)
		{
			return stage != null && stage.lifeThreatening && this.canBeThreateningToPart;
		}

		// Token: 0x060029E9 RID: 10729 RVA: 0x000DDAC0 File Offset: 0x000DBCC0
		public bool IsAnyStageLifeThreatening()
		{
			if (this.def.stages == null || !this.canBeThreateningToPart)
			{
				return false;
			}
			for (int i = 0; i < this.def.stages.Count; i++)
			{
				if (this.def.stages[i].lifeThreatening)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060029EA RID: 10730 RVA: 0x000DDB1A File Offset: 0x000DBD1A
		public bool CanEverKill()
		{
			return this.IsLethal || this.IsAnyStageLifeThreatening();
		}

		// Token: 0x060029EB RID: 10731 RVA: 0x000DDB2C File Offset: 0x000DBD2C
		public void SetVisible()
		{
			this.visible = true;
		}

		// Token: 0x060029EC RID: 10732 RVA: 0x000DDB35 File Offset: 0x000DBD35
		protected virtual void OnStageIndexChanged(int stageIndex)
		{
			if (this.CurStage.pctConditionalThoughtsNullified > 0f || this.CurStage.pctAllThoughtNullification > 0f)
			{
				this.pawn.health.hediffSet.CacheThoughtsNullified();
			}
		}

		// Token: 0x060029ED RID: 10733 RVA: 0x000DDB70 File Offset: 0x000DBD70
		public virtual void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.combatLogEntry != null)
			{
				LogEntry target = this.combatLogEntry.Target;
				if (target == null || !Current.Game.battleLog.IsEntryActive(target))
				{
					this.combatLogEntry = null;
				}
			}
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.ageTicks, "ageTicks", 0, false);
			Scribe_Values.Look<int>(ref this.tickAdded, "tickAdded", 0, false);
			Scribe_Values.Look<bool>(ref this.visible, "visible", false, false);
			Scribe_Values.Look<float>(ref this.severityInt, "severity", 0f, false);
			Scribe_Values.Look<bool>(ref this.recordedTale, "recordedTale", false, false);
			Scribe_Values.Look<bool>(ref this.causesNoPain, "causesNoPain", false, false);
			Scribe_Values.Look<string>(ref this.combatLogText, "combatLogText", null, false);
			Scribe_Values.Look<bool>(ref this.canBeThreateningToPart, "canBeThreateningToPart", false, false);
			Scribe_Defs.Look<HediffDef>(ref this.def, "def");
			Scribe_Defs.Look<ThingDef>(ref this.sourceDef, "source");
			Scribe_Defs.Look<HediffDef>(ref this.sourceHediffDef, "sourceHediffDef");
			Scribe_Defs.Look<BodyPartGroupDef>(ref this.sourceBodyPartGroup, "sourceBodyPartGroup");
			Scribe_Values.Look<string>(ref this.sourceLabel, "sourceLabel", null, false);
			Scribe_Values.Look<string>(ref this.sourceToolLabel, "sourceToolLabel", null, false);
			Scribe_BodyParts.Look(ref this.part, "part", null);
			Scribe_References.Look<LogEntry>(ref this.combatLogEntry, "combatLogEntry", false);
			Scribe_Collections.Look<Ability>(ref this.abilities, "abilities", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.abilities != null)
			{
				foreach (Ability ability in this.abilities)
				{
					ability.pawn = this.pawn;
					ability.verb.caster = this.pawn;
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Tick()
		{
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x000DDD68 File Offset: 0x000DBF68
		public virtual void TickInterval(int delta)
		{
			this.ageTicks += delta;
			if (this.def.hediffGivers != null && this.pawn.IsHashIntervalTick(60, delta))
			{
				for (int i = 0; i < this.def.hediffGivers.Count; i++)
				{
					this.def.hediffGivers[i].OnIntervalPassed(this.pawn, this);
				}
			}
			if (this.Visible && !this.visible)
			{
				this.visible = true;
				if (this.def.taleOnVisible != null)
				{
					TaleRecorder.RecordTale(this.def.taleOnVisible, new object[]
					{
						this.pawn,
						this.def
					});
				}
			}
			HediffStage curStage = this.CurStage;
			if (curStage != null)
			{
				if (curStage.hediffGivers != null && this.pawn.IsHashIntervalTick(60, delta))
				{
					for (int j = 0; j < curStage.hediffGivers.Count; j++)
					{
						curStage.hediffGivers[j].OnIntervalPassed(this.pawn, this);
					}
				}
				if (curStage.mentalStateGivers != null && this.pawn.IsHashIntervalTick(60, delta) && !this.pawn.InMentalState)
				{
					for (int k = 0; k < curStage.mentalStateGivers.Count; k++)
					{
						MentalStateGiver mentalStateGiver = curStage.mentalStateGivers[k];
						if (Rand.MTBEventOccurs(mentalStateGiver.mtbDays, 60000f, 60f))
						{
							this.pawn.mindState.mentalStateHandler.TryStartMentalState(mentalStateGiver.mentalState, "MentalStateReason_Hediff".Translate(this.Label), false, false, false, null, false, false, false);
						}
					}
				}
				if (curStage.mentalBreakMtbDays > 0f && this.pawn.IsHashIntervalTick(60, delta) && !this.pawn.InMentalState && !this.pawn.Downed && Rand.MTBEventOccurs(curStage.mentalBreakMtbDays, 60000f, 60f))
				{
					this.TryDoRandomMentalBreak();
				}
				if (curStage.vomitMtbDays > 0f && this.pawn.IsHashIntervalTick(600, delta) && Rand.MTBEventOccurs(curStage.vomitMtbDays, 60000f, 600f) && this.pawn.Spawned && this.pawn.Awake() && this.pawn.RaceProps.IsFlesh)
				{
					this.pawn.jobs.StartJob(JobMaker.MakeJob(JobDefOf.Vomit), JobCondition.InterruptForced, null, true, true, null, null, false, false, null, false, true, false);
				}
				if (curStage.forgetMemoryThoughtMtbDays > 0f)
				{
					Pawn_NeedsTracker needs = this.pawn.needs;
					Thought_Memory th;
					if (((needs != null) ? needs.mood : null) != null && this.pawn.IsHashIntervalTick(400, delta) && Rand.MTBEventOccurs(curStage.forgetMemoryThoughtMtbDays, 60000f, 400f) && this.pawn.needs.mood.thoughts.memories.Memories.TryRandomElement(out th))
					{
						this.pawn.needs.mood.thoughts.memories.RemoveMemory(th);
					}
				}
				if (!this.recordedTale && curStage.tale != null)
				{
					TaleRecorder.RecordTale(curStage.tale, new object[]
					{
						this.pawn
					});
					this.recordedTale = true;
				}
				if (curStage.destroyPart && this.Part != null && this.Part != this.pawn.RaceProps.body.corePart)
				{
					this.pawn.health.AddHediff(HediffDefOf.MissingBodyPart, this.Part, null, null);
				}
				if (curStage.deathMtbDays > 0f && this.pawn.IsHashIntervalTick(200, delta) && Rand.MTBEventOccurs(curStage.deathMtbDays, 60000f, 200f))
				{
					this.DoMTBDeath();
				}
			}
		}

		// Token: 0x060029F0 RID: 10736 RVA: 0x000DE170 File Offset: 0x000DC370
		private void DoMTBDeath()
		{
			HediffStage curStage = this.CurStage;
			if (!curStage.mtbDeathDestroysBrain && ModsConfig.BiotechActive)
			{
				Pawn_GeneTracker genes = this.pawn.genes;
				if (genes != null && genes.HasActiveGene(GeneDefOf.Deathless))
				{
					return;
				}
			}
			this.pawn.Kill(null, this);
			if (curStage.mtbDeathDestroysBrain)
			{
				BodyPartRecord brain = this.pawn.health.hediffSet.GetBrain();
				if (brain != null)
				{
					Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.pawn, brain);
					this.pawn.health.AddHediff(hediff, brain, null, null);
				}
			}
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x000DE218 File Offset: 0x000DC418
		private void TryDoRandomMentalBreak()
		{
			HediffStage curStage = this.CurStage;
			if (curStage == null)
			{
				return;
			}
			TaggedString taggedString = "MentalStateReason_Hediff".Translate(this.Label);
			if (!curStage.mentalBreakExplanation.NullOrEmpty())
			{
				taggedString += "\n\n" + curStage.mentalBreakExplanation.Formatted(this.pawn.Named("PAWN"));
			}
			if (this.pawn.NonHumanlikeOrWildMan())
			{
				this.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, taggedString, false, false, false, null, false, false, false);
				return;
			}
			MentalBreakDef mentalBreakDef;
			if ((from x in DefDatabase<MentalBreakDef>.AllDefsListForReading
			where x.Worker.BreakCanOccur(this.pawn) && (curStage.allowedMentalBreakIntensities == null || curStage.allowedMentalBreakIntensities.Contains(x.intensity))
			select x).TryRandomElementByWeight((MentalBreakDef x) => x.Worker.CommonalityFor(this.pawn, false), out mentalBreakDef))
			{
				mentalBreakDef.Worker.TryStart(this.pawn, taggedString.Resolve(), false);
			}
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x000DE31C File Offset: 0x000DC51C
		public virtual void PostMake()
		{
			this.Severity = Mathf.Max(this.Severity, this.def.initialSeverity);
			this.causesNoPain = (Rand.Value < this.def.chanceToCauseNoPain);
			if (this.def.onlyLifeThreateningTo != null)
			{
				bool flag = false;
				for (int i = 0; i < this.def.onlyLifeThreateningTo.Count; i++)
				{
					if (this.Part.def == this.def.onlyLifeThreateningTo[i])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.canBeThreateningToPart = false;
				}
			}
		}

		// Token: 0x060029F3 RID: 10739 RVA: 0x000DE3B4 File Offset: 0x000DC5B4
		public virtual void PostAdd(DamageInfo? dinfo)
		{
			this.tickAdded = Find.TickManager.TicksGame;
			if (!this.def.abilities.NullOrEmpty<AbilityDef>())
			{
				Pawn_AbilityTracker pawn_AbilityTracker = this.pawn.abilities;
				if (pawn_AbilityTracker != null)
				{
					pawn_AbilityTracker.Notify_TemporaryAbilitiesChanged();
				}
			}
			if (!this.def.removeWithTags.NullOrEmpty<string>())
			{
				for (int i = this.pawn.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
				{
					Hediff hediff = this.pawn.health.hediffSet.hediffs[i];
					if (hediff != this && !hediff.def.tags.NullOrEmpty<string>())
					{
						for (int j = 0; j < this.def.removeWithTags.Count; j++)
						{
							if (hediff.def.tags.Contains(this.def.removeWithTags[j]))
							{
								this.pawn.health.RemoveHediff(hediff);
								break;
							}
						}
					}
				}
			}
			if (!this.def.aptitudes.NullOrEmpty<Aptitude>())
			{
				this.pawn.skills.DirtyAptitudes();
			}
			if (this.def.clearsEgo)
			{
				this.pawn.everLostEgo = true;
			}
		}

		// Token: 0x060029F4 RID: 10740 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PreRemoved()
		{
		}

		// Token: 0x060029F5 RID: 10741 RVA: 0x000DE4FC File Offset: 0x000DC6FC
		public virtual void PostRemoved()
		{
			HediffStage curStage = this.CurStage;
			if (!this.pawn.Dead && this.def.chemicalNeed != null)
			{
				Pawn_NeedsTracker needs = this.pawn.needs;
				if (needs != null)
				{
					needs.AddOrRemoveNeedsAsAppropriate();
				}
			}
			else if (curStage != null && !this.pawn.Dead && (!curStage.disablesNeeds.NullOrEmpty<NeedDef>() || !curStage.enablesNeeds.NullOrEmpty<NeedDef>()))
			{
				Pawn_NeedsTracker needs2 = this.pawn.needs;
				if (needs2 != null)
				{
					needs2.AddOrRemoveNeedsAsAppropriate();
				}
			}
			if (!this.def.abilities.NullOrEmpty<AbilityDef>())
			{
				Pawn_AbilityTracker pawn_AbilityTracker = this.pawn.abilities;
				if (pawn_AbilityTracker != null)
				{
					pawn_AbilityTracker.Notify_TemporaryAbilitiesChanged();
				}
			}
			if (!this.def.aptitudes.NullOrEmpty<Aptitude>())
			{
				this.pawn.skills.DirtyAptitudes();
			}
		}

		// Token: 0x060029F6 RID: 10742 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostTick()
		{
		}

		// Token: 0x060029F7 RID: 10743 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostTickInterval(int delta)
		{
		}

		// Token: 0x060029F8 RID: 10744 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Tended(float quality, float maxQuality, int batchPosition = 0)
		{
		}

		// Token: 0x060029F9 RID: 10745 RVA: 0x000DE5CC File Offset: 0x000DC7CC
		public virtual void Heal(float amount)
		{
			if (amount <= 0f)
			{
				return;
			}
			this.Severity -= amount;
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x060029FA RID: 10746 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
		}

		// Token: 0x060029FB RID: 10747 RVA: 0x000DE5F6 File Offset: 0x000DC7F6
		public virtual bool TryMergeWith(Hediff other)
		{
			if (other == null || other.def != this.def || other.Part != this.Part)
			{
				return false;
			}
			this.Severity += other.Severity;
			this.ageTicks = 0;
			return true;
		}

		// Token: 0x060029FC RID: 10748 RVA: 0x000DE634 File Offset: 0x000DC834
		public virtual bool CauseDeathNow()
		{
			if (this.IsLethal)
			{
				bool flag = this.Severity >= this.def.lethalSeverity;
				if (flag && DebugViewSettings.logCauseOfDeath)
				{
					Log.Message("CauseOfDeath: lethal severity exceeded " + this.Severity.ToString() + " >= " + this.def.lethalSeverity.ToString());
				}
				return flag;
			}
			return false;
		}

		// Token: 0x060029FD RID: 10749 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_Downed()
		{
		}

		// Token: 0x060029FE RID: 10750 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
		}

		// Token: 0x060029FF RID: 10751 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnKilled()
		{
		}

		// Token: 0x06002A00 RID: 10752 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnDamagedThing(Thing thing, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo targets)
		{
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_EntropyGained(float baseAmount, float finalAmount, Thing source = null)
		{
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_RelationAdded(Pawn otherPawn, PawnRelationDef relationDef)
		{
		}

		// Token: 0x06002A05 RID: 10757 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
		}

		// Token: 0x06002A06 RID: 10758 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_KilledPawn(Pawn victim, DamageInfo? dinfo)
		{
		}

		// Token: 0x06002A07 RID: 10759 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_IngestedThing(Thing thing, int amount)
		{
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_Resurrected()
		{
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnCorpseSpawned()
		{
		}

		// Token: 0x06002A0A RID: 10762 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnCorpseDestroyed()
		{
		}

		// Token: 0x06002A0B RID: 10763 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_Regenerated(float hp)
		{
		}

		// Token: 0x06002A0C RID: 10764 RVA: 0x000DE69D File Offset: 0x000DC89D
		public virtual void Notify_SurgicallyRemoved(Pawn surgeon)
		{
			if (this.def.HasDefinedGraphicProperties || this.def.forceRenderTreeRecache)
			{
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x000DE69D File Offset: 0x000DC89D
		public virtual void Notify_SurgicallyReplaced(Pawn surgeon)
		{
			if (this.def.HasDefinedGraphicProperties || this.def.forceRenderTreeRecache)
			{
				this.pawn.Drawer.renderer.SetAllGraphicsDirty();
			}
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_Spawned()
		{
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			return null;
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x000DE6CE File Offset: 0x000DC8CE
		public virtual string GetInspectString()
		{
			return this.def.inspectString ?? string.Empty;
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06002A11 RID: 10769 RVA: 0x000DE6E4 File Offset: 0x000DC8E4
		public virtual string Description
		{
			get
			{
				return this.def.Description;
			}
		}

		// Token: 0x06002A12 RID: 10770 RVA: 0x000DE6F4 File Offset: 0x000DC8F4
		public virtual string GetTooltip(Pawn pawn, bool showHediffsDebugInfo)
		{
			Hediff.tipSb.Clear();
			HediffStage curStage = this.CurStage;
			if (!this.LabelCap.NullOrEmpty())
			{
				Hediff.tipSb.AppendTagged(this.LabelCap.Colorize(ColoredText.TipSectionTitleColor));
			}
			string severityLabel = this.SeverityLabel;
			if (!severityLabel.NullOrEmpty())
			{
				Hediff.tipSb.Append(": ").Append(severityLabel);
			}
			Hediff.tipSb.AppendLine();
			if (!this.def.overrideTooltip.NullOrEmpty())
			{
				Hediff.tipSb.AppendLine().AppendLineTagged(this.def.overrideTooltip.Formatted(pawn.Named("PAWN")));
			}
			else if (curStage != null && !curStage.overrideTooltip.NullOrEmpty())
			{
				Hediff.tipSb.AppendLine().AppendLineTagged(curStage.overrideTooltip.Formatted(pawn.Named("PAWN")));
			}
			else
			{
				string description = this.Description;
				if (!description.NullOrEmpty())
				{
					Hediff.tipSb.AppendLine().AppendLine(description);
				}
			}
			if (!this.def.extraTooltip.NullOrEmpty())
			{
				Hediff.tipSb.AppendLine().AppendLineTagged(this.def.extraTooltip.Formatted(pawn.Named("PAWN")));
			}
			if (curStage != null && !curStage.extraTooltip.NullOrEmpty())
			{
				Hediff.tipSb.AppendLine().AppendLineTagged(curStage.extraTooltip.Formatted(pawn.Named("PAWN")));
			}
			string tipStringExtra = this.TipStringExtra;
			if (!tipStringExtra.NullOrEmpty())
			{
				Hediff.tipSb.AppendLine().AppendLine(tipStringExtra.TrimEndNewlines());
			}
			TaggedString t;
			LogEntry logEntry;
			if (HealthCardUtility.GetCombatLogInfo(Gen.YieldSingle<Hediff>(this), out t, out logEntry) && !t.NullOrEmpty())
			{
				Hediff.tipSb.AppendLine().AppendLineTagged(("Cause".Translate() + ": " + t).Colorize(ColoredText.SubtleGrayColor));
			}
			if (showHediffsDebugInfo && !this.DebugString().NullOrEmpty() && !this.DebugString().NullOrEmpty())
			{
				Hediff.tipSb.AppendLine().AppendLine(this.DebugString().TrimEndNewlines());
			}
			return Hediff.tipSb.ToString().TrimEnd();
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x000DE93C File Offset: 0x000DCB3C
		public virtual void CopyFrom(Hediff other)
		{
			this.ageTicks = other.ageTicks;
			this.sourceLabel = other.sourceLabel;
			this.sourceDef = other.sourceDef;
			this.sourceBodyPartGroup = other.sourceBodyPartGroup;
			this.severityInt = other.severityInt;
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostDebugAdd()
		{
		}

		// Token: 0x06002A15 RID: 10773 RVA: 0x000DE97C File Offset: 0x000DCB7C
		public virtual string DebugString()
		{
			string text = "";
			if (!this.Visible)
			{
				text += "hidden\n";
			}
			text = text + "severity: " + this.Severity.ToString("F3") + ((this.Severity >= this.def.maxSeverity) ? " (reached max)" : "");
			if (this.TendableNow(false))
			{
				text = text + "\ntend priority: " + this.TendPriority.ToString();
			}
			return text.Indented("    ");
		}

		// Token: 0x06002A16 RID: 10774 RVA: 0x000DEA0F File Offset: 0x000DCC0F
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			foreach (StatDrawEntry statDrawEntry in this.def.SpecialDisplayStats(req))
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x000DEA28 File Offset: 0x000DCC28
		public override string ToString()
		{
			string[] array = new string[6];
			array[0] = "(";
			int num = 1;
			HediffDef hediffDef = this.def;
			array[num] = (((hediffDef != null) ? hediffDef.defName : null) ?? base.GetType().Name);
			array[2] = ((this.part != null) ? (" " + this.part.Label) : "");
			array[3] = " ticksSinceCreation=";
			array[4] = this.ageTicks.ToString();
			array[5] = ")";
			return string.Concat(array);
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x000DEAB1 File Offset: 0x000DCCB1
		public string GetUniqueLoadID()
		{
			return "Hediff_" + this.loadID.ToString();
		}

		// Token: 0x04002226 RID: 8742
		public HediffDef def;

		// Token: 0x04002227 RID: 8743
		public int ageTicks;

		// Token: 0x04002228 RID: 8744
		public int tickAdded = -1;

		// Token: 0x04002229 RID: 8745
		private BodyPartRecord part;

		// Token: 0x0400222A RID: 8746
		public string sourceLabel;

		// Token: 0x0400222B RID: 8747
		public ThingDef sourceDef;

		// Token: 0x0400222C RID: 8748
		public BodyPartGroupDef sourceBodyPartGroup;

		// Token: 0x0400222D RID: 8749
		public string sourceToolLabel;

		// Token: 0x0400222E RID: 8750
		public HediffDef sourceHediffDef;

		// Token: 0x0400222F RID: 8751
		public int loadID = -1;

		// Token: 0x04002230 RID: 8752
		protected float severityInt;

		// Token: 0x04002231 RID: 8753
		private bool recordedTale;

		// Token: 0x04002232 RID: 8754
		protected bool causesNoPain;

		// Token: 0x04002233 RID: 8755
		private bool visible;

		// Token: 0x04002234 RID: 8756
		public WeakReference<LogEntry> combatLogEntry;

		// Token: 0x04002235 RID: 8757
		public string combatLogText;

		// Token: 0x04002236 RID: 8758
		public int temp_partIndexToSetLater = -1;

		// Token: 0x04002237 RID: 8759
		public bool canBeThreateningToPart = true;

		// Token: 0x04002238 RID: 8760
		private List<Ability> abilities;

		// Token: 0x04002239 RID: 8761
		[Unsaved(false)]
		public Pawn pawn;

		// Token: 0x0400223A RID: 8762
		private static StringBuilder tipSb = new StringBuilder();
	}
}
