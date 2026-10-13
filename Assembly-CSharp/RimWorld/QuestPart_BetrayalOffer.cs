using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BBF RID: 7103
	public class QuestPart_BetrayalOffer : QuestPartActivable
	{
		// Token: 0x1700183F RID: 6207
		// (get) Token: 0x06009E16 RID: 40470 RVA: 0x002DBE23 File Offset: 0x002DA023
		public override string ExpiryInfoPart
		{
			get
			{
				return "QuestBetrayalOffer".Translate(this.PawnsAliveCount, this.extraFaction.faction.Name);
			}
		}

		// Token: 0x17001840 RID: 6208
		// (get) Token: 0x06009E17 RID: 40471 RVA: 0x002DBE54 File Offset: 0x002DA054
		public override string ExpiryInfoPartTip
		{
			get
			{
				return "QuestBetrayalOfferTip".Translate(this.asker.NameFullColored, this.extraFaction.faction.Name);
			}
		}

		// Token: 0x17001841 RID: 6209
		// (get) Token: 0x06009E18 RID: 40472 RVA: 0x002DBE8A File Offset: 0x002DA08A
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				int num;
				for (int i = 0; i < this.pawns.Count; i = num + 1)
				{
					if (!this.pawns[i].Destroyed)
					{
						yield return this.pawns[i];
					}
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17001842 RID: 6210
		// (get) Token: 0x06009E19 RID: 40473 RVA: 0x002DBE9C File Offset: 0x002DA09C
		private int PawnsAliveCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].Destroyed)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17001843 RID: 6211
		// (get) Token: 0x06009E1A RID: 40474 RVA: 0x002DBEDC File Offset: 0x002DA0DC
		private bool AnyPawnDespawnedButAlive
		{
			get
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].SpawnedOrAnyParentSpawned && !this.pawns[i].Dead)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001844 RID: 6212
		// (get) Token: 0x06009E1B RID: 40475 RVA: 0x002DBF28 File Offset: 0x002DA128
		private bool AllSurvivingPawnsRecruited
		{
			get
			{
				foreach (Pawn pawn in this.pawns)
				{
					if (!pawn.Destroyed && (!pawn.Faction.IsPlayerSafe() || pawn.GetExtraFaction(this.extraFaction.factionType, this.quest) == this.extraFaction.faction))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x06009E1C RID: 40476 RVA: 0x002DBFB4 File Offset: 0x002DA1B4
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			if (!this.AnyPawnDespawnedButAlive && this.PawnsAliveCount > 0)
			{
				if (!this.outSignalEnabled.NullOrEmpty())
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalEnabled, receivedArgs, false));
					return;
				}
			}
			else
			{
				base.Complete();
			}
		}

		// Token: 0x06009E1D RID: 40477 RVA: 0x002DC004 File Offset: 0x002DA204
		protected override void ProcessQuestSignal(Signal signal)
		{
			base.ProcessQuestSignal(signal);
			if (this.inSignals.Contains(signal.tag))
			{
				if (this.AnyPawnDespawnedButAlive)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalFailure, signal.args, false));
					base.Complete();
					return;
				}
				if (this.PawnsAliveCount == 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalSuccess, signal.args, false));
					base.Complete();
					return;
				}
				if (this.AllSurvivingPawnsRecruited)
				{
					string tag = this.outSignalFailureRecruited ?? this.outSignalFailure;
					Find.SignalManager.SendSignal(new Signal(tag, signal.args, false));
					base.Complete();
				}
			}
		}

		// Token: 0x06009E1E RID: 40478 RVA: 0x002DC0BA File Offset: 0x002DA2BA
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.extraFaction.faction == f)
			{
				this.extraFaction.faction = null;
			}
		}

		// Token: 0x06009E1F RID: 40479 RVA: 0x002DC0D6 File Offset: 0x002DA2D6
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			base.Notify_PawnKilled(pawn, dinfo);
			if (pawn == this.asker && base.State == QuestPartState.Enabled)
			{
				this.Disable();
			}
		}

		// Token: 0x06009E20 RID: 40480 RVA: 0x002DC0F8 File Offset: 0x002DA2F8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<ExtraFaction>(ref this.extraFaction, "extraFaction", Array.Empty<object>());
			Scribe_References.Look<Pawn>(ref this.asker, "asker", false);
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignalSuccess, "outSignalSuccess", null, false);
			Scribe_Values.Look<string>(ref this.outSignalFailure, "outSignalFailure", null, false);
			Scribe_Values.Look<string>(ref this.outSignalFailureRecruited, "outSignalFailureRecruited", null, false);
			Scribe_Values.Look<string>(ref this.outSignalEnabled, "outSignalEnabled", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
				if (this.asker == null && base.State == QuestPartState.Enabled)
				{
					this.Disable();
				}
			}
		}

		// Token: 0x04006D70 RID: 28016
		public ExtraFaction extraFaction;

		// Token: 0x04006D71 RID: 28017
		public Pawn asker;

		// Token: 0x04006D72 RID: 28018
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04006D73 RID: 28019
		public List<string> inSignals = new List<string>();

		// Token: 0x04006D74 RID: 28020
		public string outSignalEnabled;

		// Token: 0x04006D75 RID: 28021
		public string outSignalSuccess;

		// Token: 0x04006D76 RID: 28022
		public string outSignalFailure;

		// Token: 0x04006D77 RID: 28023
		public string outSignalFailureRecruited;
	}
}
