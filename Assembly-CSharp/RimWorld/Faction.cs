using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200256F RID: 9583
	public class Faction : IExposable, ILoadReferenceable, ICommunicable
	{
		// Token: 0x17002046 RID: 8262
		// (get) Token: 0x0600D1A6 RID: 53670 RVA: 0x003DA4CF File Offset: 0x003D86CF
		// (set) Token: 0x0600D1A7 RID: 53671 RVA: 0x003DA4F0 File Offset: 0x003D86F0
		public string Name
		{
			get
			{
				if (this.HasName)
				{
					return this.name;
				}
				return this.def.LabelCap;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17002047 RID: 8263
		// (get) Token: 0x0600D1A8 RID: 53672 RVA: 0x003DA4F9 File Offset: 0x003D86F9
		public bool HasName
		{
			get
			{
				return this.name != null;
			}
		}

		// Token: 0x17002048 RID: 8264
		// (get) Token: 0x0600D1A9 RID: 53673 RVA: 0x003DA504 File Offset: 0x003D8704
		public bool IsPlayer
		{
			get
			{
				return this.def.isPlayer;
			}
		}

		// Token: 0x17002049 RID: 8265
		// (get) Token: 0x0600D1AA RID: 53674 RVA: 0x003DA511 File Offset: 0x003D8711
		public int PlayerGoodwill
		{
			get
			{
				return this.GoodwillWith(Faction.OfPlayer);
			}
		}

		// Token: 0x1700204A RID: 8266
		// (get) Token: 0x0600D1AB RID: 53675 RVA: 0x003DA51E File Offset: 0x003D871E
		public FactionRelationKind PlayerRelationKind
		{
			get
			{
				return this.RelationKindWith(Faction.OfPlayer);
			}
		}

		// Token: 0x1700204B RID: 8267
		// (get) Token: 0x0600D1AC RID: 53676 RVA: 0x003DA52B File Offset: 0x003D872B
		public int NaturalGoodwill
		{
			get
			{
				return Find.GoodwillSituationManager.GetNaturalGoodwill(this);
			}
		}

		// Token: 0x1700204C RID: 8268
		// (get) Token: 0x0600D1AD RID: 53677 RVA: 0x003DA538 File Offset: 0x003D8738
		public Color Color
		{
			get
			{
				if (this.color != null)
				{
					return this.color.Value;
				}
				if (this.def.colorSpectrum.NullOrEmpty<Color>())
				{
					return Color.white;
				}
				return ColorsFromSpectrum.Get(this.def.colorSpectrum, this.colorFromSpectrum);
			}
		}

		// Token: 0x1700204D RID: 8269
		// (get) Token: 0x0600D1AE RID: 53678 RVA: 0x003DA58C File Offset: 0x003D878C
		// (set) Token: 0x0600D1AF RID: 53679 RVA: 0x003DA5D0 File Offset: 0x003D87D0
		public Color AllegianceColor
		{
			get
			{
				Color valueOrDefault = this.allegianceColor.GetValueOrDefault();
				if (this.allegianceColor == null)
				{
					valueOrDefault = this.Color;
					this.allegianceColor = new Color?(valueOrDefault);
				}
				return this.allegianceColor.Value;
			}
			set
			{
				this.allegianceColor = new Color?(value);
			}
		}

		// Token: 0x1700204E RID: 8270
		// (get) Token: 0x0600D1B0 RID: 53680 RVA: 0x003DA5E0 File Offset: 0x003D87E0
		public string LeaderTitle
		{
			get
			{
				if (this.ideos == null || this.ideos.PrimaryIdeo == null || this.ideos.PrimaryIdeo.leaderTitleMale.NullOrEmpty())
				{
					if (this.leader != null && this.leader.gender == Gender.Female && !this.def.leaderTitleFemale.NullOrEmpty())
					{
						return this.def.leaderTitleFemale;
					}
					return this.def.leaderTitle;
				}
				else
				{
					if (this.leader != null && this.leader.gender == Gender.Female)
					{
						return this.ideos.PrimaryIdeo.leaderTitleFemale;
					}
					return this.ideos.PrimaryIdeo.leaderTitleMale;
				}
			}
		}

		// Token: 0x1700204F RID: 8271
		// (get) Token: 0x0600D1B1 RID: 53681 RVA: 0x003DA690 File Offset: 0x003D8890
		private bool ShouldHaveLeader
		{
			get
			{
				return !this.IsPlayer && !this.Hidden && !this.temporary && this.def.humanlikeFaction;
			}
		}

		// Token: 0x17002050 RID: 8272
		// (get) Token: 0x0600D1B2 RID: 53682 RVA: 0x003DA6B7 File Offset: 0x003D88B7
		public TaggedString NameColored
		{
			get
			{
				if (this.HasName)
				{
					return this.name.CapitalizeFirst().ApplyTag(this);
				}
				return this.def.LabelCap;
			}
		}

		// Token: 0x17002051 RID: 8273
		// (get) Token: 0x0600D1B3 RID: 53683 RVA: 0x003DA6DE File Offset: 0x003D88DE
		public bool CanEverGiveGoodwillRewards
		{
			get
			{
				return !this.def.permanentEnemy && this.HasGoodwill;
			}
		}

		// Token: 0x17002052 RID: 8274
		// (get) Token: 0x0600D1B4 RID: 53684 RVA: 0x003DA6F5 File Offset: 0x003D88F5
		public string GetReportText
		{
			get
			{
				return this.def.description + (this.def.HasRoyalTitles ? ("\n\n" + RoyalTitleUtility.GetTitleProgressionInfo(this, null)) : "");
			}
		}

		// Token: 0x17002053 RID: 8275
		// (get) Token: 0x0600D1B5 RID: 53685 RVA: 0x003DA72C File Offset: 0x003D892C
		public bool Hidden
		{
			get
			{
				bool? flag = this.hidden;
				if (flag == null)
				{
					return this.def.hidden;
				}
				return flag.GetValueOrDefault();
			}
		}

		// Token: 0x17002054 RID: 8276
		// (get) Token: 0x0600D1B6 RID: 53686 RVA: 0x003DA75C File Offset: 0x003D895C
		public bool HasGoodwill
		{
			get
			{
				return !this.Hidden && !this.temporary;
			}
		}

		// Token: 0x0600D1B7 RID: 53687 RVA: 0x003DA774 File Offset: 0x003D8974
		public Faction()
		{
			this.randomKey = Rand.Range(0, int.MaxValue);
			this.kidnapped = new KidnappedPawnsTracker(this);
		}

		// Token: 0x0600D1B8 RID: 53688 RVA: 0x003DA814 File Offset: 0x003D8A14
		public void ExposeData()
		{
			Scribe_References.Look<Pawn>(ref this.leader, "leader", false);
			Scribe_Defs.Look<FactionDef>(ref this.def, "def");
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.randomKey, "randomKey", 0, false);
			Scribe_Values.Look<float>(ref this.colorFromSpectrum, "colorFromSpectrum", 0f, false);
			Scribe_Collections.Look<FactionRelation>(ref this.relations, "relations", LookMode.Deep, Array.Empty<object>());
			Scribe_Deep.Look<KidnappedPawnsTracker>(ref this.kidnapped, "kidnapped", new object[]
			{
				this
			});
			Scribe_Deep.Look<FactionIdeosTracker>(ref this.ideos, "ideos", new object[]
			{
				this
			});
			Scribe_Collections.Look<PredatorThreat>(ref this.predatorThreats, "predatorThreats", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.defeated, "defeated", false, false);
			Scribe_Values.Look<int>(ref this.lastTraderRequestTick, "lastTraderRequestTick", -9999999, false);
			Scribe_Values.Look<int>(ref this.lastOrbitalTraderRequestTick, "lastOrbitalTraderRequestTick", -9999999, false);
			Scribe_Values.Look<int>(ref this.lastMilitaryAidRequestTick, "lastMilitaryAidRequestTick", -9999999, false);
			Scribe_Values.Look<int>(ref this.lastExecutionTick, "lastExecutionTick", -9999999, false);
			Scribe_Values.Look<int>(ref this.naturalGoodwillTimer, "naturalGoodwillTimer", 0, false);
			Scribe_Values.Look<bool>(ref this.allowRoyalFavorRewards, "allowRoyalFavorRewards", true, false);
			Scribe_Values.Look<bool>(ref this.allowGoodwillRewards, "allowGoodwillRewards", true, false);
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<bool?>(ref this.hidden, "hidden", null, false);
			Scribe_Values.Look<bool>(ref this.temporary, "temporary", false, false);
			Scribe_Values.Look<bool>(ref this.factionHostileOnHarmByPlayer, "factionHostileOnHarmByPlayer", false, false);
			Scribe_Values.Look<Color?>(ref this.color, "color", null, false);
			Scribe_Values.Look<bool>(ref this.neverFlee, "neverFlee", false, false);
			Scribe_Values.Look<Color?>(ref this.allegianceColor, "mechColor", null, false);
			Scribe_Values.Look<bool>(ref this.deactivated, "deactivated", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.predatorThreats.RemoveAll((PredatorThreat x) => x.predator == null);
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600D1B9 RID: 53689 RVA: 0x003DAA74 File Offset: 0x003D8C74
		public void FactionTick()
		{
			this.CheckReachNaturalGoodwill();
			this.kidnapped.KidnappedPawnsTrackerTick();
			for (int i = this.predatorThreats.Count - 1; i >= 0; i--)
			{
				PredatorThreat predatorThreat = this.predatorThreats[i];
				if (predatorThreat.Expired)
				{
					this.predatorThreats.RemoveAt(i);
					if (predatorThreat.predator.Spawned)
					{
						predatorThreat.predator.Map.attackTargetsCache.UpdateTarget(predatorThreat.predator);
					}
				}
			}
			if (Find.TickManager.TicksGame % 1000 == 200 && this.IsPlayer)
			{
				if (NamePlayerFactionAndSettlementUtility.CanNameFactionNow())
				{
					Settlement settlement = Find.WorldObjects.Settlements.Find((Settlement x) => NamePlayerFactionAndSettlementUtility.CanNameSettlementSoon(x));
					if (settlement != null)
					{
						Find.WindowStack.Add(new Dialog_NamePlayerFactionAndSettlement(settlement));
					}
					else
					{
						Find.WindowStack.Add(new Dialog_NamePlayerFaction());
					}
				}
				else
				{
					Settlement settlement2 = Find.WorldObjects.Settlements.Find((Settlement x) => NamePlayerFactionAndSettlementUtility.CanNameSettlementNow(x));
					if (settlement2 != null)
					{
						if (NamePlayerFactionAndSettlementUtility.CanNameFactionSoon())
						{
							Find.WindowStack.Add(new Dialog_NamePlayerFactionAndSettlement(settlement2));
						}
						else
						{
							Find.WindowStack.Add(new Dialog_NamePlayerSettlement(settlement2));
						}
					}
				}
			}
			if (this.ShouldHaveLeader && this.leader == null)
			{
				Log.ErrorOnce("Faction leader for " + this.Name + " is null.", this.loadID ^ 441821);
			}
		}

		// Token: 0x0600D1BA RID: 53690 RVA: 0x003DAC08 File Offset: 0x003D8E08
		private void CheckReachNaturalGoodwill()
		{
			if (this.IsPlayer || !this.HasGoodwill || this.def.permanentEnemy)
			{
				return;
			}
			int num = this.BaseGoodwillWith(Faction.OfPlayer);
			IntRange intRange = new IntRange(this.NaturalGoodwill - 50, this.NaturalGoodwill + 50);
			if (intRange.Includes(num))
			{
				this.naturalGoodwillTimer = 0;
				return;
			}
			this.naturalGoodwillTimer++;
			if (num < intRange.min)
			{
				int num2 = 3000000;
				if (this.naturalGoodwillTimer >= num2)
				{
					Faction ofPlayer = Faction.OfPlayer;
					int goodwillChange = Mathf.Min(10, intRange.min - num);
					bool canSendMessage = true;
					HistoryEventDef reachNaturalGoodwill = HistoryEventDefOf.ReachNaturalGoodwill;
					this.TryAffectGoodwillWith(ofPlayer, goodwillChange, canSendMessage, !this.temporary, reachNaturalGoodwill, null);
					this.naturalGoodwillTimer = 0;
					return;
				}
			}
			else if (num > intRange.max)
			{
				int num3 = 3000000;
				if (this.naturalGoodwillTimer >= num3)
				{
					Faction ofPlayer2 = Faction.OfPlayer;
					int goodwillChange2 = -Mathf.Min(10, num - intRange.max);
					bool canSendMessage2 = true;
					HistoryEventDef reachNaturalGoodwill = HistoryEventDefOf.ReachNaturalGoodwill;
					this.TryAffectGoodwillWith(ofPlayer2, goodwillChange2, canSendMessage2, !this.temporary, reachNaturalGoodwill, null);
					this.naturalGoodwillTimer = 0;
				}
			}
		}

		// Token: 0x0600D1BB RID: 53691 RVA: 0x003DAD2C File Offset: 0x003D8F2C
		public void TryMakeInitialRelationsWith(Faction other)
		{
			if (this.RelationWith(other, true) != null)
			{
				return;
			}
			int a = Faction.<TryMakeInitialRelationsWith>g__GetInitialGoodwill|67_0(this, other);
			int b = Faction.<TryMakeInitialRelationsWith>g__GetInitialGoodwill|67_0(other, this);
			int num = Mathf.Min(a, b);
			FactionRelationKind kind;
			if (num <= -10)
			{
				kind = FactionRelationKind.Hostile;
			}
			else if (num >= 75)
			{
				kind = FactionRelationKind.Ally;
			}
			else
			{
				kind = FactionRelationKind.Neutral;
			}
			FactionRelation factionRelation = new FactionRelation();
			factionRelation.other = other;
			factionRelation.baseGoodwill = num;
			factionRelation.kind = kind;
			this.relations.Add(factionRelation);
			FactionRelation factionRelation2 = new FactionRelation();
			factionRelation2.other = this;
			factionRelation2.baseGoodwill = num;
			factionRelation2.kind = kind;
			other.relations.Add(factionRelation2);
		}

		// Token: 0x0600D1BC RID: 53692 RVA: 0x003DADC4 File Offset: 0x003D8FC4
		public void SetRelation(FactionRelation relation)
		{
			if (relation.other == this)
			{
				Log.Error("Tried to set relation between faction " + ((this != null) ? this.ToString() : null) + " and itself.");
				return;
			}
			if (relation.other == null)
			{
				Log.Error("Relation is missing faction.");
				return;
			}
			this.relations.RemoveAll((FactionRelation r) => r.other == relation.other);
			relation.other.relations.RemoveAll((FactionRelation r) => r.other == this);
			this.relations.Add(relation);
			FactionRelation factionRelation = new FactionRelation();
			factionRelation.other = this;
			factionRelation.kind = relation.kind;
			relation.other.relations.Add(factionRelation);
		}

		// Token: 0x0600D1BD RID: 53693 RVA: 0x003DAEAC File Offset: 0x003D90AC
		public PawnKindDef RandomPawnKind()
		{
			Faction.allPawnKinds.Clear();
			if (this.def.pawnGroupMakers != null)
			{
				for (int i = 0; i < this.def.pawnGroupMakers.Count; i++)
				{
					List<PawnGenOption> options = this.def.pawnGroupMakers[i].options;
					for (int j = 0; j < options.Count; j++)
					{
						if (options[j].kind.RaceProps.Humanlike)
						{
							Faction.allPawnKinds.Add(options[j].kind);
						}
					}
				}
			}
			if (!Faction.allPawnKinds.Any<PawnKindDef>())
			{
				return this.def.basicMemberKind;
			}
			PawnKindDef result = Faction.allPawnKinds.RandomElement<PawnKindDef>();
			Faction.allPawnKinds.Clear();
			return result;
		}

		// Token: 0x0600D1BE RID: 53694 RVA: 0x003DAF6D File Offset: 0x003D916D
		public bool IsPlayerGoodwillMinimum()
		{
			return this.def.permanentEnemy || this.PlayerGoodwill <= -100;
		}

		// Token: 0x0600D1BF RID: 53695 RVA: 0x003DAF8C File Offset: 0x003D918C
		public FactionRelation RelationWith(Faction other, bool allowNull = false)
		{
			if (other == this)
			{
				Log.Error("Tried to get relation between faction " + ((this != null) ? this.ToString() : null) + " and itself.");
				return new FactionRelation();
			}
			for (int i = 0; i < this.relations.Count; i++)
			{
				if (this.relations[i].other == other)
				{
					return this.relations[i];
				}
			}
			if (!allowNull)
			{
				Log.Error(string.Concat(new string[]
				{
					"Faction ",
					this.name,
					" has null relation with ",
					(other != null) ? other.ToString() : null,
					". Returning dummy relation."
				}));
				return new FactionRelation();
			}
			return null;
		}

		// Token: 0x0600D1C0 RID: 53696 RVA: 0x003DB046 File Offset: 0x003D9246
		public int BaseGoodwillWith(Faction other)
		{
			return this.RelationWith(other, false).baseGoodwill;
		}

		// Token: 0x0600D1C1 RID: 53697 RVA: 0x003DB058 File Offset: 0x003D9258
		public int GoodwillWith(Faction other)
		{
			int num = this.BaseGoodwillWith(other);
			if (this.IsPlayer)
			{
				num = Mathf.Min(num, Find.GoodwillSituationManager.GetMaxGoodwill(other));
			}
			else if (other.IsPlayer)
			{
				num = Mathf.Min(num, Find.GoodwillSituationManager.GetMaxGoodwill(this));
			}
			return num;
		}

		// Token: 0x0600D1C2 RID: 53698 RVA: 0x003DB0A4 File Offset: 0x003D92A4
		public FactionRelationKind RelationKindWith(Faction other)
		{
			return this.RelationWith(other, false).kind;
		}

		// Token: 0x0600D1C3 RID: 53699 RVA: 0x003DB0B4 File Offset: 0x003D92B4
		public bool CanChangeGoodwillFor(Faction other, int goodwillChange)
		{
			return this.HasGoodwill && other.HasGoodwill && !this.def.permanentEnemy && !other.def.permanentEnemy && !this.defeated && !other.defeated && other != this && (!this.def.permanentEnemyToEveryoneExceptPlayer || other.IsPlayer) && (!other.def.permanentEnemyToEveryoneExceptPlayer || this.IsPlayer) && (this.def.permanentEnemyToEveryoneExcept == null || this.def.permanentEnemyToEveryoneExcept.Contains(other.def)) && (other.def.permanentEnemyToEveryoneExcept == null || other.def.permanentEnemyToEveryoneExcept.Contains(this.def)) && (goodwillChange <= 0 || ((!this.IsPlayer || !SettlementUtility.IsPlayerAttackingAnySettlementOf(other)) && (!other.IsPlayer || !SettlementUtility.IsPlayerAttackingAnySettlementOf(this)))) && !QuestUtility.IsGoodwillLockedByQuest(this, other);
		}

		// Token: 0x0600D1C4 RID: 53700 RVA: 0x003DB1B6 File Offset: 0x003D93B6
		public int GoodwillToMakeHostile(Faction other)
		{
			if (this.HostileTo(other))
			{
				return 0;
			}
			return -75 - this.GoodwillWith(other);
		}

		// Token: 0x0600D1C5 RID: 53701 RVA: 0x003DB1D0 File Offset: 0x003D93D0
		public int CalculateAdjustedGoodwillChange(Faction other, int goodwillChange)
		{
			int num = this.GoodwillWith(other);
			if (this.IsPlayer || other.IsPlayer)
			{
				int num2 = this.IsPlayer ? other.NaturalGoodwill : this.NaturalGoodwill;
				if ((num2 < num && goodwillChange < 0) || (num2 > num && goodwillChange > 0))
				{
					int num3 = Mathf.Min(Mathf.Abs(num - num2), Mathf.Abs(goodwillChange));
					int num4 = Mathf.RoundToInt(0.25f * (float)num3);
					if (goodwillChange < 0)
					{
						num4 = -num4;
					}
					goodwillChange += num4;
				}
			}
			return goodwillChange;
		}

		// Token: 0x0600D1C6 RID: 53702 RVA: 0x003DB24C File Offset: 0x003D944C
		public bool TryAffectGoodwillWith(Faction other, int goodwillChange, bool canSendMessage = true, bool canSendHostilityLetter = true, HistoryEventDef reason = null, GlobalTargetInfo? lookTarget = null)
		{
			if (!this.CanChangeGoodwillFor(other, goodwillChange))
			{
				return false;
			}
			if (goodwillChange == 0)
			{
				return true;
			}
			int num = this.GoodwillWith(other);
			goodwillChange = this.CalculateAdjustedGoodwillChange(other, goodwillChange);
			int num2 = this.BaseGoodwillWith(other);
			int num3 = Mathf.Clamp(num2 + goodwillChange, -100, 100);
			if (num2 == num3)
			{
				return true;
			}
			if (reason != null && (this.IsPlayer || other.IsPlayer))
			{
				Faction arg = this.IsPlayer ? other : this;
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(reason, arg.Named(HistoryEventArgsNames.AffectedFaction), goodwillChange.Named(HistoryEventArgsNames.CustomGoodwill)), true);
			}
			FactionRelation factionRelation = this.RelationWith(other, false);
			factionRelation.baseGoodwill = num3;
			bool flag;
			factionRelation.CheckKindThresholds(this, canSendHostilityLetter, (reason != null) ? reason.LabelCap : null, lookTarget ?? GlobalTargetInfo.Invalid, out flag);
			FactionRelation factionRelation2 = other.RelationWith(this, false);
			FactionRelationKind kind = factionRelation2.kind;
			factionRelation2.baseGoodwill = factionRelation.baseGoodwill;
			factionRelation2.kind = factionRelation.kind;
			bool flag2;
			if (kind != factionRelation2.kind)
			{
				other.Notify_RelationKindChanged(this, kind, canSendHostilityLetter, (reason != null) ? reason.LabelCap : null, lookTarget ?? GlobalTargetInfo.Invalid, out flag2);
			}
			else
			{
				flag2 = false;
			}
			int num4 = this.GoodwillWith(other);
			if (canSendMessage && num != num4 && !flag && !flag2 && Current.ProgramState == ProgramState.Playing && (this.IsPlayer || other.IsPlayer))
			{
				Faction faction = this.IsPlayer ? other : this;
				string text;
				if (reason != null)
				{
					text = "MessageGoodwillChangedWithReason".Translate(faction.name, num.ToString("F0"), num4.ToString("F0"), reason.label);
				}
				else
				{
					text = "MessageGoodwillChanged".Translate(faction.name, num.ToString("F0"), num4.ToString("F0"));
				}
				Messages.Message(text, lookTarget ?? GlobalTargetInfo.Invalid, ((float)goodwillChange > 0f) ? MessageTypeDefOf.PositiveEvent : MessageTypeDefOf.NegativeEvent, true);
			}
			return true;
		}

		// Token: 0x0600D1C7 RID: 53703 RVA: 0x003DB4D4 File Offset: 0x003D96D4
		public void Notify_GoodwillSituationsChanged(Faction other, bool canSendHostilityLetter, string reason, GlobalTargetInfo? lookTarget)
		{
			FactionRelation factionRelation = this.RelationWith(other, false);
			bool flag;
			factionRelation.CheckKindThresholds(this, canSendHostilityLetter, reason, lookTarget ?? GlobalTargetInfo.Invalid, out flag);
			FactionRelation factionRelation2 = other.RelationWith(this, false);
			FactionRelationKind kind = factionRelation2.kind;
			factionRelation2.kind = factionRelation.kind;
			if (kind != factionRelation2.kind)
			{
				other.Notify_RelationKindChanged(this, kind, canSendHostilityLetter, reason, lookTarget ?? GlobalTargetInfo.Invalid, out flag);
			}
		}

		// Token: 0x0600D1C8 RID: 53704 RVA: 0x003DB55C File Offset: 0x003D975C
		public void SetRelationDirect(Faction other, FactionRelationKind kind, bool canSendHostilityLetter = true, string reason = null, GlobalTargetInfo? lookTarget = null)
		{
			if (this.HasGoodwill && other.HasGoodwill)
			{
				Log.Error("Tried to use SetRelationDirect for factions which use goodwill. The relation would be overriden by goodwill anyway. faction=" + ((this != null) ? this.ToString() : null) + ", other=" + ((other != null) ? other.ToString() : null));
				return;
			}
			FactionRelation factionRelation = this.RelationWith(other, false);
			if (factionRelation.kind != kind)
			{
				FactionRelationKind kind2 = factionRelation.kind;
				factionRelation.kind = kind;
				bool flag;
				this.Notify_RelationKindChanged(other, kind2, canSendHostilityLetter, reason, lookTarget ?? GlobalTargetInfo.Invalid, out flag);
				other.RelationWith(this, false).kind = kind;
				other.Notify_RelationKindChanged(this, kind2, canSendHostilityLetter, reason, lookTarget ?? GlobalTargetInfo.Invalid, out flag);
			}
		}

		// Token: 0x0600D1C9 RID: 53705 RVA: 0x003DB624 File Offset: 0x003D9824
		public void RemoveAllRelations()
		{
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				if (faction != this)
				{
					faction.relations.RemoveAll((FactionRelation x) => x.other == this);
				}
			}
			this.relations.Clear();
		}

		// Token: 0x0600D1CA RID: 53706 RVA: 0x003DB69C File Offset: 0x003D989C
		public void TryAppendRelationKindChangedInfo(StringBuilder text, FactionRelationKind previousKind, FactionRelationKind newKind, string reason = null)
		{
			TaggedString taggedString = null;
			this.TryAppendRelationKindChangedInfo(ref taggedString, previousKind, newKind, reason);
			if (!taggedString.NullOrEmpty())
			{
				text.AppendLine();
				text.AppendLine();
				text.AppendTagged(taggedString);
			}
		}

		// Token: 0x0600D1CB RID: 53707 RVA: 0x003DB6DC File Offset: 0x003D98DC
		public void TryAppendRelationKindChangedInfo(ref TaggedString text, FactionRelationKind previousKind, FactionRelationKind newKind, string reason = null)
		{
			if (previousKind == newKind)
			{
				return;
			}
			if (!text.NullOrEmpty())
			{
				text += "\n\n";
			}
			if (newKind == FactionRelationKind.Hostile)
			{
				text += "LetterRelationsChange_Hostile".Translate(this.NameColored);
				if (this.HasGoodwill)
				{
					text += "\n\n" + "LetterRelationsChange_HostileGoodwillDescription".Translate(this.PlayerGoodwill.ToStringWithSign(), -75.ToStringWithSign(), 0.ToStringWithSign());
				}
				if (!reason.NullOrEmpty())
				{
					text += "\n\n" + "FinalStraw".Translate(reason.CapitalizeFirst());
					return;
				}
			}
			else if (newKind == FactionRelationKind.Ally)
			{
				text += "LetterRelationsChange_Ally".Translate(this.NameColored);
				if (this.HasGoodwill)
				{
					text += "\n\n" + "LetterRelationsChange_AllyGoodwillDescription".Translate(this.PlayerGoodwill.ToStringWithSign(), 75.ToStringWithSign(), 0.ToStringWithSign());
				}
				if (!reason.NullOrEmpty())
				{
					text += "\n\n" + "LastFactionRelationsEvent".Translate() + ": " + reason.CapitalizeFirst();
					return;
				}
			}
			else if (newKind == FactionRelationKind.Neutral)
			{
				if (previousKind == FactionRelationKind.Hostile)
				{
					text += "LetterRelationsChange_NeutralFromHostile".Translate(this.NameColored);
					if (this.HasGoodwill)
					{
						text += "\n\n" + "LetterRelationsChange_NeutralFromHostileGoodwillDescription".Translate(this.NameColored, this.PlayerGoodwill.ToStringWithSign(), 0.ToStringWithSign(), -75.ToStringWithSign(), 75.ToStringWithSign());
					}
					if (!reason.NullOrEmpty())
					{
						text += "\n\n" + "LastFactionRelationsEvent".Translate() + ": " + reason.CapitalizeFirst();
						return;
					}
				}
				else
				{
					text += "LetterRelationsChange_NeutralFromAlly".Translate(this.NameColored);
					if (this.HasGoodwill)
					{
						text += "\n\n" + "LetterRelationsChange_NeutralFromAllyGoodwillDescription".Translate(this.NameColored, this.PlayerGoodwill.ToStringWithSign(), 0.ToStringWithSign(), -75.ToStringWithSign(), 75.ToStringWithSign());
					}
					if (!reason.NullOrEmpty())
					{
						text += "\n\n" + "Reason".Translate() + ": " + reason.CapitalizeFirst();
					}
				}
			}
		}

		// Token: 0x0600D1CC RID: 53708 RVA: 0x003DBA50 File Offset: 0x003D9C50
		public void Notify_MemberTookDamage(Pawn member, DamageInfo dinfo)
		{
			if (dinfo.Instigator == null)
			{
				return;
			}
			if (this.IsPlayer)
			{
				return;
			}
			Pawn pawn = dinfo.Instigator as Pawn;
			if (pawn != null && pawn.CurJob != null && pawn.CurJob.def == JobDefOf.PredatorHunt)
			{
				this.TookDamageFromPredator(pawn);
			}
			if (dinfo.Instigator.Faction == null || !dinfo.Def.ExternalViolenceFor(member) || this.HostileTo(dinfo.Instigator.Faction))
			{
				return;
			}
			if (this.factionHostileOnHarmByPlayer && dinfo.Instigator.Faction == Faction.OfPlayer)
			{
				this.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, true, null, null);
			}
			if (member.InAggroMentalState)
			{
				return;
			}
			if (pawn != null && pawn.InAggroMentalState)
			{
				return;
			}
			if (member.InMentalState && member.MentalStateDef.IsExtreme && member.MentalStateDef.category == MentalStateCategory.Malicious && this.PlayerRelationKind == FactionRelationKind.Ally)
			{
				return;
			}
			if (dinfo.Instigator.Faction == Faction.OfPlayer && (PrisonBreakUtility.IsPrisonBreaking(member) || member.IsQuestHelper()))
			{
				return;
			}
			if (pawn != null && SlaveRebellionUtility.IsRebelling(pawn))
			{
				return;
			}
			if (dinfo.Instigator.Faction == Faction.OfPlayer && !this.IsMutuallyHostileCrossfire(dinfo) && !member.IsSlaveOfColony)
			{
				float num = Mathf.Min(100f, dinfo.Amount);
				int goodwillChange = (int)(-1.3f * num);
				Faction.OfPlayer.TryAffectGoodwillWith(this, goodwillChange, true, !this.temporary, HistoryEventDefOf.AttackedMember, new GlobalTargetInfo?(member));
			}
		}

		// Token: 0x0600D1CD RID: 53709 RVA: 0x003DBBE0 File Offset: 0x003D9DE0
		public void Notify_BuildingRemoved(Building building, Pawn deconstructor)
		{
			if (this.IsPlayer)
			{
				return;
			}
			if (this.factionHostileOnHarmByPlayer && deconstructor != null && deconstructor.Faction == Faction.OfPlayer)
			{
				this.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, true, null, null);
			}
		}

		// Token: 0x0600D1CE RID: 53710 RVA: 0x003DBC28 File Offset: 0x003D9E28
		public void Notify_BuildingTookDamage(Building building, DamageInfo dinfo)
		{
			if (dinfo.Instigator == null || this.IsPlayer)
			{
				return;
			}
			if (dinfo.Instigator.Faction == null || !dinfo.Def.ExternalViolenceFor(building) || this.HostileTo(dinfo.Instigator.Faction))
			{
				return;
			}
			if (this.factionHostileOnHarmByPlayer && dinfo.Instigator.Faction == Faction.OfPlayer)
			{
				this.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, true, null, null);
			}
			if (dinfo.Instigator.Faction == Faction.OfPlayer && !this.IsMutuallyHostileCrossfire(dinfo))
			{
				float num = Mathf.Min(100f, dinfo.Amount);
				int goodwillChange = (int)(-1f * num);
				Faction.OfPlayer.TryAffectGoodwillWith(this, goodwillChange, true, true, HistoryEventDefOf.AttackedBuilding, new GlobalTargetInfo?(building));
			}
		}

		// Token: 0x0600D1CF RID: 53711 RVA: 0x003DBD04 File Offset: 0x003D9F04
		public void Notify_MemberCaptured(Pawn member, Faction violator)
		{
			if (this.ideos != null)
			{
				this.ideos.Notify_MemberGainedOrLost();
			}
			if (violator == this)
			{
				return;
			}
			if (!this.temporary && !member.IsSlaveOfColony)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(this, Faction.OfPlayer.GoodwillToMakeHostile(this), true, !this.temporary, HistoryEventDefOf.MemberCaptured, new GlobalTargetInfo?(member));
			}
		}

		// Token: 0x0600D1D0 RID: 53712 RVA: 0x003DBD6C File Offset: 0x003D9F6C
		public void Notify_MemberStripped(Pawn member, Faction violator)
		{
			if (violator == this || this.Hidden || member.Dead)
			{
				return;
			}
			if (violator == Faction.OfPlayer && this.RelationKindWith(violator) != FactionRelationKind.Hostile)
			{
				Faction.OfPlayer.TryAffectGoodwillWith(this, -40, true, !this.temporary, HistoryEventDefOf.MemberStripped, new GlobalTargetInfo?(member));
			}
		}

		// Token: 0x0600D1D1 RID: 53713 RVA: 0x003DBDC8 File Offset: 0x003D9FC8
		public void Notify_MemberDied(Pawn member, DamageInfo? dinfo, bool wasWorldPawn, bool wasGuilty, Map map)
		{
			if (this.ideos != null)
			{
				this.ideos.Notify_MemberGainedOrLost();
			}
			if (this.IsPlayer)
			{
				if (ModsConfig.BiotechActive && map != null && member.IsBloodfeeder())
				{
					this.CheckForPrisonersAssignedToBloodfeederInteractionMode(map, member);
				}
				return;
			}
			if (!wasWorldPawn && !PawnGenerator.IsBeingGenerated(member) && Current.ProgramState == ProgramState.Playing && map != null && !member.IsSubhuman && !member.IsSlaveOfColony && map.IsPlayerHome && !this.HostileTo(Faction.OfPlayer))
			{
				if (dinfo != null && dinfo.Value.Category == DamageInfo.SourceCategory.Collapse)
				{
					bool canSendMessage = MessagesRepeatAvoider.MessageShowAllowed("FactionRelationAdjustmentCrushed-" + this.Name, 5f);
					int goodwillChange = member.RaceProps.Humanlike ? -25 : -15;
					Faction.OfPlayer.TryAffectGoodwillWith(this, goodwillChange, canSendMessage, !this.temporary, HistoryEventDefOf.MemberCrushed, new GlobalTargetInfo?(new TargetInfo(member.Position, map, false)));
				}
				else if (dinfo != null && (dinfo.Value.Instigator == null || dinfo.Value.Instigator.Faction == null))
				{
					Pawn pawn = dinfo.Value.Instigator as Pawn;
					if (pawn != null && pawn.RaceProps.Animal)
					{
						Pawn_MindState mindState = pawn.mindState;
						if (((mindState != null) ? mindState.mentalStateHandler.CurStateDef : null) == MentalStateDefOf.ManhunterPermanent)
						{
							goto IL_251;
						}
					}
					int goodwillChange2 = member.RaceProps.Humanlike ? -5 : -3;
					Faction.OfPlayer.TryAffectGoodwillWith(this, goodwillChange2, true, !this.temporary, HistoryEventDefOf.MemberNeutrallyDied, new GlobalTargetInfo?(member));
				}
				else if ((member.kindDef.factionHostileOnDeath || (member.kindDef.factionHostileOnKill && dinfo != null && dinfo.Value.Instigator != null && dinfo.Value.Instigator.Faction == Faction.OfPlayer)) && !wasGuilty)
				{
					Faction.OfPlayer.TryAffectGoodwillWith(this, Faction.OfPlayer.GoodwillToMakeHostile(this), true, !this.temporary, HistoryEventDefOf.MemberKilled, null);
				}
			}
			IL_251:
			if (member == this.leader)
			{
				this.Notify_LeaderDied();
			}
		}

		// Token: 0x0600D1D2 RID: 53714 RVA: 0x003DC038 File Offset: 0x003DA238
		private void CheckForPrisonersAssignedToBloodfeederInteractionMode(Map map, Pawn member)
		{
			List<Pawn> freeColonistsAndPrisoners = map.mapPawns.FreeColonistsAndPrisoners;
			foreach (Pawn pawn in freeColonistsAndPrisoners)
			{
				if (pawn.IsPrisonerOfColony)
				{
					if (pawn.guest.HasInteractionWith((PrisonerInteractionModeDef interaction) => interaction.hideIfNoBloodfeeders))
					{
						this.tmpPrisonersRequiringBloodfeeders.Add(pawn);
					}
				}
			}
			if (!this.tmpPrisonersRequiringBloodfeeders.Any<Pawn>())
			{
				this.tmpPrisonersRequiringBloodfeeders.Clear();
				return;
			}
			foreach (Pawn pawn2 in freeColonistsAndPrisoners)
			{
				if (!pawn2.IsPrisonerOfColony && pawn2.IsBloodfeeder())
				{
					this.tmpPrisonersRequiringBloodfeeders.Clear();
					return;
				}
			}
			Messages.Message("MessageNoBloodfeedersPrisonerInteractionReset".Translate(), this.tmpPrisonersRequiringBloodfeeders, MessageTypeDefOf.NeutralEvent, true);
			foreach (Pawn pawn3 in this.tmpPrisonersRequiringBloodfeeders)
			{
				pawn3.guest.ToggleNonExclusiveInteraction(PrisonerInteractionModeDefOf.Bloodfeed, false);
			}
			this.tmpPrisonersRequiringBloodfeeders.Clear();
		}

		// Token: 0x0600D1D3 RID: 53715 RVA: 0x003DC1B4 File Offset: 0x003DA3B4
		public void Notify_PawnJoined(Pawn p)
		{
			if (this.ideos != null)
			{
				this.ideos.Notify_MemberGainedOrLost();
			}
			if (p.RaceProps.Humanlike && !this.def.humanlikeFaction && !p.IsSubhuman && !p.IsCreepJoiner && !p.IsDuplicate && !p.Dead)
			{
				Log.Error("Humanlike pawn " + p.LabelShort + " was added to non-humanlike faction " + this.def.label);
			}
		}

		// Token: 0x0600D1D4 RID: 53716 RVA: 0x003DC234 File Offset: 0x003DA434
		public void Notify_LeaderDied()
		{
			Pawn pawn = this.leader;
			string str = "LetterLeadersDeathLabel".Translate(this.name, this.LeaderTitle).Resolve().CapitalizeFirst();
			string text = "LetterLeadersDeath".Translate(this.NameColored, this.LeaderTitle, pawn.Named("OLDLEADER")).Resolve().CapitalizeFirst();
			if (this.TryGenerateNewLeader())
			{
				string str2 = "LetterNewLeader".Translate(this.LeaderTitle, this.leader.Named("NEWLEADER")).Resolve().CapitalizeFirst();
				if (!this.temporary)
				{
					Find.LetterStack.ReceiveLetter(str, text + "\n\n" + str2, LetterDefOf.NeutralEvent, GlobalTargetInfo.Invalid, this, null, null, null, 0, true);
				}
				QuestUtility.SendQuestTargetSignals(pawn.questTags, "NoLongerFactionLeader", pawn.Named("SUBJECT"), this.leader.Named("NEWFACTIONLEADER"));
				return;
			}
			if (!this.temporary)
			{
				Find.LetterStack.ReceiveLetter(str, text, LetterDefOf.NeutralEvent, GlobalTargetInfo.Invalid, this, null, null, null, 0, true);
			}
			QuestUtility.SendQuestTargetSignals(pawn.questTags, "NoLongerFactionLeader", pawn.Named("SUBJECT"));
		}

		// Token: 0x0600D1D5 RID: 53717 RVA: 0x003DC3A8 File Offset: 0x003DA5A8
		public void Notify_LeaderLost()
		{
			Pawn pawn = this.leader;
			if (this.TryGenerateNewLeader())
			{
				if (!this.temporary)
				{
					Find.LetterStack.ReceiveLetter("LetterLeaderChangedLabel".Translate(this.name, this.LeaderTitle).Resolve().CapitalizeFirst(), "LetterLeaderChanged".Translate(this.NameColored, this.LeaderTitle, pawn.Named("OLDLEADER")).Resolve().CapitalizeFirst() + "\n\n" + "LetterNewLeader".Translate(this.LeaderTitle, this.leader.Named("NEWLEADER")).Resolve().CapitalizeFirst(), LetterDefOf.NeutralEvent, GlobalTargetInfo.Invalid, this, null, null, null, 0, true);
				}
				QuestUtility.SendQuestTargetSignals(pawn.questTags, "NoLongerFactionLeader", pawn.Named("SUBJECT"), this.leader.Named("NEWFACTIONLEADER"));
				return;
			}
			QuestUtility.SendQuestTargetSignals(pawn.questTags, "NoLongerFactionLeader", pawn.Named("SUBJECT"));
		}

		// Token: 0x0600D1D6 RID: 53718 RVA: 0x003DC4E4 File Offset: 0x003DA6E4
		public void Notify_RelationKindChanged(Faction other, FactionRelationKind previousKind, bool canSendLetter, string reason, GlobalTargetInfo lookTarget, out bool sentLetter)
		{
			if (Current.ProgramState != ProgramState.Playing || other != Faction.OfPlayer)
			{
				canSendLetter = false;
			}
			sentLetter = false;
			ColoredText.ClearCache();
			FactionRelationKind factionRelationKind = this.RelationKindWith(other);
			if (factionRelationKind == FactionRelationKind.Hostile)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_Alive.ToList<Pawn>())
					{
						if ((pawn.Faction == this && pawn.HostFaction == other) || (pawn.Faction == other && pawn.HostFaction == this))
						{
							pawn.guest.SetGuestStatus(pawn.HostFaction, GuestStatus.Prisoner);
						}
					}
				}
				if (other == Faction.OfPlayer)
				{
					QuestUtility.SendQuestTargetSignals(this.questTags, "BecameHostileToPlayer", this.Named("SUBJECT"));
				}
			}
			if (other == Faction.OfPlayer && !this.HostileTo(Faction.OfPlayer))
			{
				List<Site> list = new List<Site>();
				List<Site> sites = Find.WorldObjects.Sites;
				for (int i = 0; i < sites.Count; i++)
				{
					if (sites[i].factionMustRemainHostile && sites[i].Faction == this && !sites[i].HasMap)
					{
						list.Add(sites[i]);
					}
				}
				if (list.Any<Site>())
				{
					string str;
					string str2;
					if (list.Count == 1)
					{
						str = "LetterLabelSiteNoLongerHostile".Translate();
						str2 = "LetterSiteNoLongerHostile".Translate(this.NameColored, list[0].Label);
					}
					else
					{
						StringBuilder stringBuilder = new StringBuilder();
						for (int j = 0; j < list.Count; j++)
						{
							if (stringBuilder.Length != 0)
							{
								stringBuilder.AppendLine();
							}
							stringBuilder.Append("  - " + list[j].LabelCap);
							ImportantPawnComp component = list[j].GetComponent<ImportantPawnComp>();
							if (component != null && component.pawn.Any)
							{
								stringBuilder.Append(" (" + component.pawn[0].LabelCap + ")");
							}
						}
						str = "LetterLabelSiteNoLongerHostileMulti".Translate();
						string str3 = "LetterSiteNoLongerHostileMulti".Translate(this.NameColored) + ":\n\n";
						StringBuilder stringBuilder2 = stringBuilder;
						str2 = str3 + ((stringBuilder2 != null) ? stringBuilder2.ToString() : null);
					}
					Find.LetterStack.ReceiveLetter(str, str2, LetterDefOf.NeutralEvent, new LookTargets(from x in list
					select new GlobalTargetInfo(x.Tile)), null, null, null, null, 0, true);
					for (int k = 0; k < list.Count; k++)
					{
						list[k].Destroy();
					}
				}
			}
			if (other == Faction.OfPlayer && this.HostileTo(Faction.OfPlayer))
			{
				List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
				for (int l = 0; l < allWorldObjects.Count; l++)
				{
					if (allWorldObjects[l].Faction == this)
					{
						TradeRequestComp component2 = allWorldObjects[l].GetComponent<TradeRequestComp>();
						if (component2 != null && component2.ActiveRequest)
						{
							component2.Disable();
						}
					}
				}
				foreach (Map map in Find.Maps)
				{
					map.passingShipManager.RemoveAllShipsOfFaction(this);
				}
			}
			if (canSendLetter)
			{
				TaggedString text = "";
				this.TryAppendRelationKindChangedInfo(ref text, previousKind, factionRelationKind, reason);
				if (factionRelationKind == FactionRelationKind.Hostile)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelRelationsChange_Hostile".Translate(this.Name), text, LetterDefOf.NegativeEvent, lookTarget, this, null, null, null, 0, true);
					sentLetter = true;
				}
				else if (factionRelationKind == FactionRelationKind.Ally)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelRelationsChange_Ally".Translate(this.Name), text, LetterDefOf.PositiveEvent, lookTarget, this, null, null, null, 0, true);
					sentLetter = true;
				}
				else if (factionRelationKind == FactionRelationKind.Neutral)
				{
					if (previousKind == FactionRelationKind.Hostile)
					{
						Find.LetterStack.ReceiveLetter("LetterLabelRelationsChange_NeutralFromHostile".Translate(this.Name), text, LetterDefOf.PositiveEvent, lookTarget, this, null, null, null, 0, true);
						sentLetter = true;
					}
					else
					{
						Find.LetterStack.ReceiveLetter("LetterLabelRelationsChange_NeutralFromAlly".Translate(this.Name), text, LetterDefOf.NeutralEvent, lookTarget, this, null, null, null, 0, true);
						sentLetter = true;
					}
				}
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				List<Map> maps = Find.Maps;
				for (int m = 0; m < maps.Count; m++)
				{
					maps[m].attackTargetsCache.Notify_FactionHostilityChanged(this, other);
					LordManager lordManager = maps[m].lordManager;
					for (int n = 0; n < lordManager.lords.Count; n++)
					{
						Lord lord = lordManager.lords[n];
						if (lord.faction == other)
						{
							lord.Notify_FactionRelationsChanged(this, previousKind);
						}
						else if (lord.faction == this)
						{
							lord.Notify_FactionRelationsChanged(other, previousKind);
						}
					}
				}
			}
		}

		// Token: 0x0600D1D7 RID: 53719 RVA: 0x003DCA5C File Offset: 0x003DAC5C
		public void Notify_PlayerTraded(float marketValueSentByPlayer, Pawn playerNegotiator)
		{
			int goodwillChange = (int)(marketValueSentByPlayer / 600f);
			Faction.OfPlayer.TryAffectGoodwillWith(this, goodwillChange, true, !this.temporary, HistoryEventDefOf.Traded, new GlobalTargetInfo?(playerNegotiator));
		}

		// Token: 0x0600D1D8 RID: 53720 RVA: 0x003DCA9C File Offset: 0x003DAC9C
		public void Notify_MemberExitedMap(Pawn member, bool freed)
		{
			bool flag = freed && (member.HostFaction == Faction.OfPlayer || member.Faction == Faction.OfPlayer);
			bool flag2 = !freed && !member.Faction.HostileTo(Faction.OfPlayer);
			if (flag || flag2)
			{
				bool flag3;
				bool flag4;
				int goodwillGainForExit = this.GetGoodwillGainForExit(member, freed, out flag3, out flag4);
				if (member.mindState.AvailableForGoodwillReward && (freed || goodwillGainForExit >= 4))
				{
					HistoryEventDef reason = freed ? HistoryEventDefOf.MemberExitedMapHealthy : HistoryEventDefOf.FriendlyExitedMapTended;
					Faction.OfPlayer.TryAffectGoodwillWith(this, goodwillGainForExit, true, !this.temporary, reason, null);
				}
			}
			member.mindState.timesGuestTendedToByPlayer = 0;
		}

		// Token: 0x0600D1D9 RID: 53721 RVA: 0x003DCB4C File Offset: 0x003DAD4C
		public int GetGoodwillGainForExit(Pawn member, bool freed, out bool isHealthy, out bool isInMentalState)
		{
			isHealthy = false;
			isInMentalState = member.InMentalState;
			float num = 0f;
			bool flag = !freed && !member.Faction.HostileTo(Faction.OfPlayer) && member.mindState.timesGuestTendedToByPlayer > 0;
			if (member.health.hediffSet.BleedRateTotal < 0.001f && (freed || flag))
			{
				isHealthy = true;
				if (!isInMentalState)
				{
					num += 12f;
					if (PawnUtility.IsFactionLeader(member))
					{
						num += 40f;
					}
				}
			}
			return (int)(num + (float)Mathf.Min(member.mindState.timesGuestTendedToByPlayer, 10) * 1f);
		}

		// Token: 0x0600D1DA RID: 53722 RVA: 0x003DCBEA File Offset: 0x003DADEA
		public void Notify_MemberLeftExtraFaction(Pawn member)
		{
			if (member.HomeFaction == this)
			{
				return;
			}
			if (this.leader == member)
			{
				this.Notify_LeaderLost();
			}
		}

		// Token: 0x0600D1DB RID: 53723 RVA: 0x003DCC05 File Offset: 0x003DAE05
		[Obsolete("Will be removed in the future")]
		public void GenerateNewLeader()
		{
			this.TryGenerateNewLeader();
		}

		// Token: 0x0600D1DC RID: 53724 RVA: 0x003DCC10 File Offset: 0x003DAE10
		public bool TryGenerateNewLeader()
		{
			Pawn pawn = this.leader;
			this.leader = null;
			if (this.def.generateNewLeaderFromMapMembersOnly)
			{
				for (int i = 0; i < Find.Maps.Count; i++)
				{
					Map map = Find.Maps[i];
					for (int j = 0; j < map.mapPawns.AllPawnsCount; j++)
					{
						if (map.mapPawns.AllPawns[j] != pawn && !map.mapPawns.AllPawns[j].Destroyed && map.mapPawns.AllPawns[j].HomeFaction == this)
						{
							this.leader = map.mapPawns.AllPawns[j];
						}
					}
				}
			}
			else if (this.def.pawnGroupMakers != null)
			{
				List<PawnKindDef> list = new List<PawnKindDef>();
				foreach (PawnGroupMaker pawnGroupMaker in from x in this.def.pawnGroupMakers
				where x.kindDef == PawnGroupKindDefOf.Combat
				select x)
				{
					foreach (PawnGenOption pawnGenOption in pawnGroupMaker.options)
					{
						if (pawnGenOption.kind.factionLeader)
						{
							list.Add(pawnGenOption.kind);
						}
					}
				}
				if (this.def.fixedLeaderKinds != null)
				{
					list.AddRange(this.def.fixedLeaderKinds);
				}
				PawnKindDef pawnKindDef;
				if (list.TryRandomElement(out pawnKindDef))
				{
					PawnKindDef kind = pawnKindDef;
					PawnGenerationContext context = PawnGenerationContext.NonPlayer;
					bool leaderForceGenerateNewPawn = this.def.leaderForceGenerateNewPawn;
					PawnGenerationRequest request = new PawnGenerationRequest(kind, this, context, null, leaderForceGenerateNewPawn, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
					Gender supremeGender = this.ideos.PrimaryIdeo.SupremeGender;
					if (supremeGender != Gender.None)
					{
						request.FixedGender = new Gender?(supremeGender);
					}
					this.leader = PawnGenerator.GeneratePawn(request);
					if (this.leader.RaceProps.IsFlesh)
					{
						this.leader.relations.everSeenByPlayer = true;
					}
					if (!Find.WorldPawns.Contains(this.leader))
					{
						Find.WorldPawns.PassToWorld(this.leader, PawnDiscardDecideMode.KeepForever);
					}
				}
			}
			return this.leader != null;
		}

		// Token: 0x0600D1DD RID: 53725 RVA: 0x003DCEFC File Offset: 0x003DB0FC
		public string GetCallLabel()
		{
			return this.name;
		}

		// Token: 0x0600D1DE RID: 53726 RVA: 0x003DCF04 File Offset: 0x003DB104
		public string GetInfoText()
		{
			return this.def.LabelCap + ("\n" + "goodwill".Translate().CapitalizeFirst() + ": " + this.PlayerGoodwill.ToStringWithSign());
		}

		// Token: 0x0600D1DF RID: 53727 RVA: 0x000FD030 File Offset: 0x000FB230
		Faction ICommunicable.GetFaction()
		{
			return this;
		}

		// Token: 0x0600D1E0 RID: 53728 RVA: 0x003DCF64 File Offset: 0x003DB164
		public void TryOpenComms(Pawn negotiator)
		{
			Dialog_Negotiation dialog_Negotiation = new Dialog_Negotiation(negotiator, this, FactionDialogMaker.FactionDialogFor(negotiator, this), true);
			dialog_Negotiation.soundAmbient = SoundDefOf.RadioComms_Ambience;
			Find.WindowStack.Add(dialog_Negotiation);
		}

		// Token: 0x0600D1E1 RID: 53729 RVA: 0x003DCF98 File Offset: 0x003DB198
		private bool LeaderIsAvailableToTalk()
		{
			return this.leader != null && (!this.leader.Spawned || (!this.leader.Downed && !this.leader.IsPrisoner && this.leader.Awake() && !this.leader.InMentalState));
		}

		// Token: 0x0600D1E2 RID: 53730 RVA: 0x003DCFF4 File Offset: 0x003DB1F4
		public FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator)
		{
			if (this.IsPlayer)
			{
				return null;
			}
			string text = "CallOnRadio".Translate(this.GetCallLabel());
			text = string.Concat(new string[]
			{
				text,
				" (",
				this.PlayerRelationKind.GetLabelCap(),
				", ",
				this.PlayerGoodwill.ToStringWithSign(),
				")"
			});
			if (!this.LeaderIsAvailableToTalk())
			{
				string str;
				if (this.leader != null)
				{
					str = "LeaderUnavailable".Translate(this.leader.LabelShort, this.leader);
				}
				else
				{
					str = "LeaderUnavailableNoLeader".Translate();
				}
				return new FloatMenuOption(text + " (" + str + ")", null, this.def.FactionIcon, this.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(text, delegate()
			{
				console.GiveUseCommsJob(negotiator, this);
			}, this.def.FactionIcon, this.Color, MenuOptionPriority.InitiateSocial, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false), negotiator, console, "ReservedBy", null);
		}

		// Token: 0x0600D1E3 RID: 53731 RVA: 0x003DD158 File Offset: 0x003DB358
		private void TookDamageFromPredator(Pawn predator)
		{
			for (int i = 0; i < this.predatorThreats.Count; i++)
			{
				if (this.predatorThreats[i].predator == predator)
				{
					this.predatorThreats[i].lastAttackTicks = Find.TickManager.TicksGame;
					return;
				}
			}
			PredatorThreat predatorThreat = new PredatorThreat();
			predatorThreat.predator = predator;
			predatorThreat.lastAttackTicks = Find.TickManager.TicksGame;
			this.predatorThreats.Add(predatorThreat);
		}

		// Token: 0x0600D1E4 RID: 53732 RVA: 0x003DD1D4 File Offset: 0x003DB3D4
		public bool HasPredatorRecentlyAttackedAnyone(Pawn predator)
		{
			for (int i = 0; i < this.predatorThreats.Count; i++)
			{
				if (this.predatorThreats[i].predator == predator)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D1E5 RID: 53733 RVA: 0x003DD210 File Offset: 0x003DB410
		private bool IsMutuallyHostileCrossfire(DamageInfo dinfo)
		{
			return dinfo.Instigator != null && dinfo.IntendedTarget != null && ((dinfo.IntendedTarget.HostileTo(dinfo.Instigator) && dinfo.IntendedTarget.HostileTo(this)) || (dinfo.IntendedTarget.Faction.HostileTo(dinfo.Instigator.Faction) && dinfo.IntendedTarget.Faction.HostileTo(this)));
		}

		// Token: 0x17002055 RID: 8277
		// (get) Token: 0x0600D1E6 RID: 53734 RVA: 0x003DD28A File Offset: 0x003DB48A
		public static Faction OfPlayer
		{
			get
			{
				Faction ofPlayerSilentFail = Faction.OfPlayerSilentFail;
				if (ofPlayerSilentFail == null)
				{
					Log.Error("Could not find player faction.");
				}
				return ofPlayerSilentFail;
			}
		}

		// Token: 0x17002056 RID: 8278
		// (get) Token: 0x0600D1E7 RID: 53735 RVA: 0x003DD29E File Offset: 0x003DB49E
		public static Faction OfMechanoids
		{
			get
			{
				return Find.FactionManager.OfMechanoids;
			}
		}

		// Token: 0x17002057 RID: 8279
		// (get) Token: 0x0600D1E8 RID: 53736 RVA: 0x003DD2AA File Offset: 0x003DB4AA
		public static Faction OfInsects
		{
			get
			{
				return Find.FactionManager.OfInsects;
			}
		}

		// Token: 0x17002058 RID: 8280
		// (get) Token: 0x0600D1E9 RID: 53737 RVA: 0x003DD2B6 File Offset: 0x003DB4B6
		public static Faction OfAncients
		{
			get
			{
				return Find.FactionManager.OfAncients;
			}
		}

		// Token: 0x17002059 RID: 8281
		// (get) Token: 0x0600D1EA RID: 53738 RVA: 0x003DD2C2 File Offset: 0x003DB4C2
		public static Faction OfAncientsHostile
		{
			get
			{
				return Find.FactionManager.OfAncientsHostile;
			}
		}

		// Token: 0x1700205A RID: 8282
		// (get) Token: 0x0600D1EB RID: 53739 RVA: 0x003DD2CE File Offset: 0x003DB4CE
		public static Faction OfEmpire
		{
			get
			{
				return Find.FactionManager.OfEmpire;
			}
		}

		// Token: 0x1700205B RID: 8283
		// (get) Token: 0x0600D1EC RID: 53740 RVA: 0x003DD2DA File Offset: 0x003DB4DA
		public static Faction OfPirates
		{
			get
			{
				return Find.FactionManager.OfPirates;
			}
		}

		// Token: 0x1700205C RID: 8284
		// (get) Token: 0x0600D1ED RID: 53741 RVA: 0x003DD2E6 File Offset: 0x003DB4E6
		public static Faction OfHoraxCult
		{
			get
			{
				return Find.FactionManager.OfHoraxCult;
			}
		}

		// Token: 0x1700205D RID: 8285
		// (get) Token: 0x0600D1EE RID: 53742 RVA: 0x003DD2F2 File Offset: 0x003DB4F2
		public static Faction OfEntities
		{
			get
			{
				return Find.FactionManager.OfEntities;
			}
		}

		// Token: 0x1700205E RID: 8286
		// (get) Token: 0x0600D1EF RID: 53743 RVA: 0x003DD2FE File Offset: 0x003DB4FE
		public static Faction OfTradersGuild
		{
			get
			{
				return Find.FactionManager.OfTradersGuild;
			}
		}

		// Token: 0x1700205F RID: 8287
		// (get) Token: 0x0600D1F0 RID: 53744 RVA: 0x003DD30A File Offset: 0x003DB50A
		public static Faction OfSalvagers
		{
			get
			{
				return Find.FactionManager.OfSalvagers;
			}
		}

		// Token: 0x17002060 RID: 8288
		// (get) Token: 0x0600D1F1 RID: 53745 RVA: 0x003DD318 File Offset: 0x003DB518
		public static Faction OfPlayerSilentFail
		{
			get
			{
				if (Current.ProgramState != ProgramState.Playing)
				{
					GameInitData gameInitData = Find.GameInitData;
					if (gameInitData != null && gameInitData.playerFaction != null)
					{
						return gameInitData.playerFaction;
					}
				}
				return Find.FactionManager.OfPlayer;
			}
		}

		// Token: 0x0600D1F2 RID: 53746 RVA: 0x003DD350 File Offset: 0x003DB550
		public void Notify_RoyalThingUseViolation(Def implantOrWeapon, Pawn pawn, string violationSourceName, float detectionChance, int violationSourceLevel = 0)
		{
			if (!this.HostileTo(Faction.OfPlayer))
			{
				RoyalTitleDef minTitleToUse = ThingRequiringRoyalPermissionUtility.GetMinTitleToUse(implantOrWeapon, this, violationSourceLevel);
				string arg = (minTitleToUse == null) ? "None".Translate() : minTitleToUse.GetLabelCapFor(pawn);
				Faction.OfPlayer.TryAffectGoodwillWith(this, -4, true, true, HistoryEventDefOf.UsedForbiddenThing, new GlobalTargetInfo?(pawn));
				Find.LetterStack.ReceiveLetter("LetterLawViolationDetectedLabel".Translate(pawn.Named("PAWN")).CapitalizeFirst(), "LetterLawViolationDetectedForbiddenThingUse".Translate(arg.Named("TITLE"), pawn.Named("PAWN"), violationSourceName.Named("CULPRIT"), this.Named("FACTION"), 4.ToString().Named("GOODWILL"), detectionChance.ToStringPercent().Named("CHANCE")), LetterDefOf.NegativeEvent, pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600D1F3 RID: 53747 RVA: 0x003DD448 File Offset: 0x003DB648
		public RoyalTitleDef GetMinTitleForImplant(HediffDef implantDef, int level = 0)
		{
			if (this.def.royalImplantRules == null || this.def.royalImplantRules.Count == 0)
			{
				return null;
			}
			foreach (RoyalImplantRule royalImplantRule in this.def.royalImplantRules)
			{
				if (royalImplantRule.implantHediff == implantDef && (royalImplantRule.maxLevel >= level || level == 0))
				{
					return royalImplantRule.minTitle;
				}
			}
			return null;
		}

		// Token: 0x0600D1F4 RID: 53748 RVA: 0x003DD4DC File Offset: 0x003DB6DC
		public RoyalImplantRule GetMaxAllowedImplantLevel(HediffDef implantDef, RoyalTitleDef title)
		{
			if (this.def.royalImplantRules == null || this.def.royalImplantRules.Count == 0)
			{
				return null;
			}
			if (title == null)
			{
				return null;
			}
			int myTitleIdx = this.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(title);
			return this.def.royalImplantRules.Where(delegate(RoyalImplantRule r)
			{
				if (r.implantHediff == implantDef)
				{
					RoyalTitleDef minTitleForImplant = this.GetMinTitleForImplant(implantDef, r.maxLevel);
					int num = this.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(minTitleForImplant);
					return myTitleIdx == -1 || myTitleIdx >= num;
				}
				return false;
			}).Last<RoyalImplantRule>();
		}

		// Token: 0x0600D1F5 RID: 53749 RVA: 0x003DD55C File Offset: 0x003DB75C
		public static Pair<Faction, RoyalTitleDef> GetMinTitleForImplantAllFactions(HediffDef implantDef)
		{
			foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
			{
				RoyalTitleDef minTitleForImplant = faction.GetMinTitleForImplant(implantDef, 0);
				if (minTitleForImplant != null)
				{
					return new Pair<Faction, RoyalTitleDef>(faction, minTitleForImplant);
				}
			}
			return default(Pair<Faction, RoyalTitleDef>);
		}

		// Token: 0x0600D1F6 RID: 53750 RVA: 0x003DD5D0 File Offset: 0x003DB7D0
		public void ChangeGoodwill_Debug(Faction other, int goodwillChange)
		{
			int baseGoodwill = Mathf.Clamp(this.BaseGoodwillWith(other) + goodwillChange, -100, 100);
			FactionRelation factionRelation = this.RelationWith(other, false);
			factionRelation.baseGoodwill = baseGoodwill;
			bool flag;
			factionRelation.CheckKindThresholds(this, false, null, GlobalTargetInfo.Invalid, out flag);
			FactionRelation factionRelation2 = other.RelationWith(this, false);
			factionRelation2.baseGoodwill = factionRelation.baseGoodwill;
			factionRelation2.kind = factionRelation.kind;
		}

		// Token: 0x0600D1F7 RID: 53751 RVA: 0x003DD62F File Offset: 0x003DB82F
		public string GetUniqueLoadID()
		{
			return "Faction_" + this.loadID.ToString();
		}

		// Token: 0x0600D1F8 RID: 53752 RVA: 0x003DD646 File Offset: 0x003DB846
		public override string ToString()
		{
			if (this.name != null)
			{
				return this.name;
			}
			if (this.def != null)
			{
				return this.def.defName;
			}
			return "[faction of no def]";
		}

		// Token: 0x0600D1FA RID: 53754 RVA: 0x003DD67C File Offset: 0x003DB87C
		[CompilerGenerated]
		internal static int <TryMakeInitialRelationsWith>g__GetInitialGoodwill|67_0(Faction a, Faction b)
		{
			if (a.def.permanentEnemy)
			{
				return -100;
			}
			if (a.def.permanentEnemyToEveryoneExceptPlayer && !b.IsPlayer)
			{
				return -100;
			}
			if (a.def.permanentEnemyToEveryoneExcept != null && !a.def.permanentEnemyToEveryoneExcept.Contains(b.def))
			{
				return -100;
			}
			if (a.def.naturalEnemy)
			{
				return -80;
			}
			return 0;
		}

		// Token: 0x04008E7D RID: 36477
		public FactionDef def;

		// Token: 0x04008E7E RID: 36478
		private string name;

		// Token: 0x04008E7F RID: 36479
		public int loadID = -1;

		// Token: 0x04008E80 RID: 36480
		public int randomKey;

		// Token: 0x04008E81 RID: 36481
		public float colorFromSpectrum = -999f;

		// Token: 0x04008E82 RID: 36482
		private List<FactionRelation> relations = new List<FactionRelation>();

		// Token: 0x04008E83 RID: 36483
		public Pawn leader;

		// Token: 0x04008E84 RID: 36484
		public KidnappedPawnsTracker kidnapped;

		// Token: 0x04008E85 RID: 36485
		private List<PredatorThreat> predatorThreats = new List<PredatorThreat>();

		// Token: 0x04008E86 RID: 36486
		public bool defeated;

		// Token: 0x04008E87 RID: 36487
		public int lastTraderRequestTick = -9999999;

		// Token: 0x04008E88 RID: 36488
		public int lastOrbitalTraderRequestTick = -9999999;

		// Token: 0x04008E89 RID: 36489
		public int lastMilitaryAidRequestTick = -9999999;

		// Token: 0x04008E8A RID: 36490
		public int lastExecutionTick = -9999999;

		// Token: 0x04008E8B RID: 36491
		private int naturalGoodwillTimer;

		// Token: 0x04008E8C RID: 36492
		public bool allowRoyalFavorRewards = true;

		// Token: 0x04008E8D RID: 36493
		public bool allowGoodwillRewards = true;

		// Token: 0x04008E8E RID: 36494
		public List<string> questTags;

		// Token: 0x04008E8F RID: 36495
		public Color? color;

		// Token: 0x04008E90 RID: 36496
		public Color? allegianceColor;

		// Token: 0x04008E91 RID: 36497
		public bool? hidden;

		// Token: 0x04008E92 RID: 36498
		public bool temporary;

		// Token: 0x04008E93 RID: 36499
		public bool factionHostileOnHarmByPlayer;

		// Token: 0x04008E94 RID: 36500
		public bool neverFlee;

		// Token: 0x04008E95 RID: 36501
		public FactionIdeosTracker ideos;

		// Token: 0x04008E96 RID: 36502
		public bool deactivated;

		// Token: 0x04008E97 RID: 36503
		private List<Map> avoidGridsBasicKeysWorkingList;

		// Token: 0x04008E98 RID: 36504
		private List<ByteGrid> avoidGridsBasicValuesWorkingList;

		// Token: 0x04008E99 RID: 36505
		private List<Map> avoidGridsSmartKeysWorkingList;

		// Token: 0x04008E9A RID: 36506
		private List<ByteGrid> avoidGridsSmartValuesWorkingList;

		// Token: 0x04008E9B RID: 36507
		private const int MinGoodwillForGainFriendlyExit = 4;

		// Token: 0x04008E9C RID: 36508
		private static List<PawnKindDef> allPawnKinds = new List<PawnKindDef>();

		// Token: 0x04008E9D RID: 36509
		private List<Pawn> tmpPrisonersRequiringBloodfeeders = new List<Pawn>();

		// Token: 0x04008E9E RID: 36510
		public const int RoyalThingUseViolationGoodwillImpact = 4;
	}
}
