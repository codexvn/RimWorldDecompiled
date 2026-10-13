using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020026DE RID: 9950
	public class RitualObligation : IExposable, ILoadReferenceable
	{
		// Token: 0x17002160 RID: 8544
		// (get) Token: 0x0600DA0E RID: 55822 RVA: 0x00407FEB File Offset: 0x004061EB
		public bool StillValid
		{
			get
			{
				return !this.expires || this.TicksUntilExpiration >= 0;
			}
		}

		// Token: 0x17002161 RID: 8545
		// (get) Token: 0x0600DA0F RID: 55823 RVA: 0x00408003 File Offset: 0x00406203
		public static int DaysToExpire
		{
			get
			{
				return RitualObligation.StageDays[RitualObligation.StageDays.Length - 1];
			}
		}

		// Token: 0x17002162 RID: 8546
		// (get) Token: 0x0600DA10 RID: 55824 RVA: 0x00408014 File Offset: 0x00406214
		public int ActiveForTicks
		{
			get
			{
				return Find.TickManager.TicksGame - this.triggeredTick;
			}
		}

		// Token: 0x17002163 RID: 8547
		// (get) Token: 0x0600DA11 RID: 55825 RVA: 0x00408027 File Offset: 0x00406227
		public int TicksUntilExpiration
		{
			get
			{
				return this.triggeredTick + RitualObligation.DaysToExpire * 60000 - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17002164 RID: 8548
		// (get) Token: 0x0600DA12 RID: 55826 RVA: 0x00408048 File Offset: 0x00406248
		public TargetInfo FirstValidTarget
		{
			get
			{
				if (this.targetA.IsValid)
				{
					return this.targetA;
				}
				if (this.targetB.IsValid)
				{
					return this.targetB;
				}
				if (this.targetC.IsValid)
				{
					return this.targetC;
				}
				return TargetInfo.Invalid;
			}
		}

		// Token: 0x17002165 RID: 8549
		// (get) Token: 0x0600DA13 RID: 55827 RVA: 0x00408098 File Offset: 0x00406298
		public string LetterLabel
		{
			get
			{
				string text = this.precept.obligationTargetFilter.LabelExtraPart(this);
				string result;
				if (text.NullOrEmpty())
				{
					result = "RitualOpportunity".Translate(this.precept.LabelCap);
				}
				else
				{
					result = "RitualOpportunityFor".Translate(this.precept.LabelCap, text);
				}
				return result;
			}
		}

		// Token: 0x17002166 RID: 8550
		// (get) Token: 0x0600DA14 RID: 55828 RVA: 0x00408108 File Offset: 0x00406308
		public TaggedString LetterText
		{
			get
			{
				Precept_Ritual precept_Ritual = this.precept;
				this.sbTemp.Clear();
				Pawn arg = this.targetA.Thing as Pawn;
				Corpse corpse = this.targetA.Thing as Corpse;
				if (corpse != null)
				{
					arg = corpse.InnerPawn;
				}
				RitualObligationTrigger_Date ritualObligationTrigger_Date = precept_Ritual.obligationTriggers.OfType<RitualObligationTrigger_Date>().FirstOrDefault<RitualObligationTrigger_Date>();
				TaggedString taggedString = ((precept_Ritual.ideo.adjective.NullOrEmpty() && !precept_Ritual.ritualExpectedDescNoAdjective.NullOrEmpty()) ? precept_Ritual.ritualExpectedDescNoAdjective : precept_Ritual.ritualExpectedDesc).Formatted(precept_Ritual.ideo.Named("IDEOLIGION"), precept_Ritual.Named("RITUAL"), arg.Named("PAWN"), precept_Ritual.ideo.memberName.Colorize(precept_Ritual.ideo.TextColor).Named("MEMBER"), precept_Ritual.ideo.MemberNamePlural.Colorize(precept_Ritual.ideo.TextColor).Named("MEMBERS"), (precept_Ritual.ideo.classicMode ? Faction.OfPlayer.Name : precept_Ritual.ideo.name).Colorize(precept_Ritual.ideo.TextColor).Named("IDEO"), (ritualObligationTrigger_Date != null) ? ritualObligationTrigger_Date.DateString.Named("DATE") : "");
				this.sbTemp.AppendLineTagged(taggedString);
				this.sbTemp.AppendLine();
				IEnumerable<string> targetInfos = precept_Ritual.obligationTargetFilter.GetTargetInfos(this);
				if (targetInfos.Count<string>() > 1)
				{
					if ("RitualMultiTargetDesc".CanTranslate())
					{
						this.sbTemp.AppendLine("RitualMultiTargetDesc".Translate(precept_Ritual.Named("RITUAL")) + ":\n" + targetInfos.ToLineList("  - ", true));
					}
					else
					{
						this.sbTemp.AppendLine("RitualTargetsExplanation".Translate(precept_Ritual.Named("RITUAL"), targetInfos.ToLineList("  - ", true)).CapitalizeFirst());
					}
				}
				else if ("RitualSingleTargetDesc".CanTranslate())
				{
					this.sbTemp.AppendLine("RitualSingleTargetDesc".Translate(precept_Ritual.Named("RITUAL"), targetInfos.First<string>().UncapitalizeFirst().Named("FOCUS")));
				}
				else
				{
					this.sbTemp.AppendLine("RitualTargetsExplanation".Translate(precept_Ritual.Named("RITUAL"), targetInfos.ToLineList("  - ", true)).CapitalizeFirst());
				}
				if (this.expires)
				{
					Map map = (Current.ProgramState == ProgramState.Playing) ? Find.AnyPlayerHomeMap : null;
					string value = GenDate.QuadrumDateStringAt((long)(Find.TickManager.TicksAbs + RitualObligation.StageDays[RitualObligation.StageDays.Length - 1] * 60000), (map != null) ? Find.WorldGrid.LongLatOf(map.Tile).x : 0f);
					this.sbTemp.AppendLine();
					this.sbTemp.AppendLineTagged("RitualExpiresIn".Translate((RitualObligation.StageDays[RitualObligation.StageDays.Length - 1] * 60000).ToStringTicksToPeriodVague(true, true), value));
				}
				this.unfilledRolesTemp.Clear();
				foreach (RitualRole ritualRole in precept_Ritual.behavior.def.RequiredRoles())
				{
					Precept_Role precept_Role = ritualRole.FindInstance(precept_Ritual.ideo);
					if (!ritualRole.substitutable && precept_Role != null && precept_Role.Active && precept_Role.ChosenPawnSingle() == null)
					{
						this.unfilledRolesTemp.Add(precept_Role.LabelCap);
					}
				}
				if (this.unfilledRolesTemp.Any<string>())
				{
					this.sbTemp.AppendLine();
					this.sbTemp.AppendLine();
					this.sbTemp.AppendLine("RitualUnfilledRoles".Translate(precept_Ritual.LabelCap) + ":");
					this.sbTemp.Append(this.unfilledRolesTemp.ToLineList("  - "));
				}
				return this.sbTemp.ToString().TrimEndNewlines();
			}
		}

		// Token: 0x0600DA15 RID: 55829 RVA: 0x00408584 File Offset: 0x00406784
		public RitualObligation()
		{
		}

		// Token: 0x0600DA16 RID: 55830 RVA: 0x004085B0 File Offset: 0x004067B0
		public RitualObligation(Precept_Ritual precept, TargetInfo targetA, TargetInfo targetB, TargetInfo targetC)
		{
			this.precept = precept;
			this.targetA = targetA;
			this.targetB = targetB;
			this.targetC = targetC;
			this.triggeredTick = Find.TickManager.TicksGame;
			this.ID = Find.UniqueIDsManager.GetNextRitualObligationID();
		}

		// Token: 0x0600DA17 RID: 55831 RVA: 0x00408624 File Offset: 0x00406824
		public RitualObligation(Precept_Ritual precept, TargetInfo targetA, TargetInfo targetB, bool expires = true) : this(precept, targetA, targetB, TargetInfo.Invalid)
		{
			this.expires = expires;
		}

		// Token: 0x0600DA18 RID: 55832 RVA: 0x0040863C File Offset: 0x0040683C
		public RitualObligation(Precept_Ritual precept, TargetInfo targetA, bool expires = true) : this(precept, targetA, TargetInfo.Invalid, TargetInfo.Invalid)
		{
			this.expires = expires;
		}

		// Token: 0x0600DA19 RID: 55833 RVA: 0x00408657 File Offset: 0x00406857
		public RitualObligation(Precept_Ritual precept, bool expires = true) : this(precept, TargetInfo.Invalid, TargetInfo.Invalid, TargetInfo.Invalid)
		{
			this.expires = expires;
		}

		// Token: 0x0600DA1A RID: 55834 RVA: 0x00408676 File Offset: 0x00406876
		public void DebugOffsetTriggeredTick(int ticks)
		{
			this.triggeredTick += ticks;
		}

		// Token: 0x0600DA1B RID: 55835 RVA: 0x00408688 File Offset: 0x00406888
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.triggeredTick, "triggeredTick", 0, false);
			Scribe_Values.Look<bool>(ref this.sendLetter, "sendLetter", false, false);
			Scribe_Values.Look<bool>(ref this.expires, "expires", false, false);
			Scribe_TargetInfo.Look(ref this.targetA, true, "targetA");
			Scribe_TargetInfo.Look(ref this.targetB, true, "targetB");
			Scribe_TargetInfo.Look(ref this.targetC, true, "targetC");
			Scribe_Collections.Look<Pawn>(ref this.onlyForPawns, "onlyForPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
		}

		// Token: 0x0600DA1C RID: 55836 RVA: 0x00408728 File Offset: 0x00406928
		public virtual void CopyTo(RitualObligation other)
		{
			other.ID = this.ID;
			other.precept = this.precept;
			other.triggeredTick = this.triggeredTick;
			other.sendLetter = this.sendLetter;
			other.expires = this.expires;
			other.targetA = this.targetA;
			other.targetB = this.targetB;
			other.targetC = this.targetC;
			other.onlyForPawns = this.onlyForPawns;
		}

		// Token: 0x0600DA1D RID: 55837 RVA: 0x004087A1 File Offset: 0x004069A1
		public string GetUniqueLoadID()
		{
			return "RitualObligation_" + this.ID.ToString();
		}

		// Token: 0x0400939C RID: 37788
		public Precept_Ritual precept;

		// Token: 0x0400939D RID: 37789
		private int triggeredTick;

		// Token: 0x0400939E RID: 37790
		public bool sendLetter = true;

		// Token: 0x0400939F RID: 37791
		public bool expires = true;

		// Token: 0x040093A0 RID: 37792
		public TargetInfo targetA;

		// Token: 0x040093A1 RID: 37793
		public TargetInfo targetB;

		// Token: 0x040093A2 RID: 37794
		public TargetInfo targetC;

		// Token: 0x040093A3 RID: 37795
		public List<Pawn> onlyForPawns;

		// Token: 0x040093A4 RID: 37796
		public int ID;

		// Token: 0x040093A5 RID: 37797
		public static readonly int[] StageDays = new int[]
		{
			3,
			9
		};

		// Token: 0x040093A6 RID: 37798
		private StringBuilder sbTemp = new StringBuilder();

		// Token: 0x040093A7 RID: 37799
		private List<string> unfilledRolesTemp = new List<string>();
	}
}
