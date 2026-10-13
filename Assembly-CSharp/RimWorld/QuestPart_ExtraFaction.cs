using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C44 RID: 7236
	public class QuestPart_ExtraFaction : QuestPartActivable
	{
		// Token: 0x170018C1 RID: 6337
		// (get) Token: 0x0600A10C RID: 41228 RVA: 0x002E57A7 File Offset: 0x002E39A7
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				ExtraFaction extraFaction = this.extraFaction;
				if (((extraFaction != null) ? extraFaction.faction : null) != null)
				{
					yield return this.extraFaction.faction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A10D RID: 41229 RVA: 0x002E57B7 File Offset: 0x002E39B7
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			base.Notify_PawnKilled(pawn, dinfo);
			if (pawn.HomeFaction != null && pawn.Faction != pawn.HomeFaction)
			{
				pawn.SetFaction(pawn.HomeFaction, null);
			}
		}

		// Token: 0x0600A10E RID: 41230 RVA: 0x002E57E4 File Offset: 0x002E39E4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (!(signal.tag == this.inSignalRemovePawn))
			{
				List<string> list = this.inSignalsRemovePawn;
				if (list == null || !list.Contains(signal.tag))
				{
					return;
				}
			}
			Pawn pawn;
			if (signal.args.TryGetArg<Pawn>("SUBJECT", out pawn) && this.affectedPawns.Contains(pawn))
			{
				this.affectedPawns.Remove(pawn);
				this.extraFaction.faction.Notify_MemberLeftExtraFaction(pawn);
			}
		}

		// Token: 0x0600A10F RID: 41231 RVA: 0x002E5866 File Offset: 0x002E3A66
		public override bool QuestPartReserves(Faction f)
		{
			return this.extraFaction.faction == f;
		}

		// Token: 0x0600A110 RID: 41232 RVA: 0x002E5878 File Offset: 0x002E3A78
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ExtraFaction>(ref this.extraFaction, "extraFaction", Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.affectedPawns, "affectedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.areHelpers, "areHelpers", false, false);
			Scribe_Values.Look<string>(ref this.inSignalRemovePawn, "inSignalRemovePawn", null, false);
			Scribe_Collections.Look<string>(ref this.inSignalsRemovePawn, "inSignalsRemovePawn", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				ExtraFaction extraFaction = this.extraFaction;
				if (((extraFaction != null) ? extraFaction.faction : null) == null)
				{
					this.affectedPawns.Clear();
					return;
				}
				this.affectedPawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600A111 RID: 41233 RVA: 0x002E5943 File Offset: 0x002E3B43
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.affectedPawns.Replace(replace, with);
		}

		// Token: 0x0600A112 RID: 41234 RVA: 0x002E5953 File Offset: 0x002E3B53
		public override void Cleanup()
		{
			base.Cleanup();
			this.SetRelationsGainTickForPawns();
		}

		// Token: 0x0600A113 RID: 41235 RVA: 0x002E5961 File Offset: 0x002E3B61
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.extraFaction.faction == faction)
			{
				this.affectedPawns.Clear();
				this.extraFaction.faction = null;
			}
		}

		// Token: 0x0600A114 RID: 41236 RVA: 0x002E5988 File Offset: 0x002E3B88
		protected override void Disable()
		{
			base.Disable();
			this.SetRelationsGainTickForPawns();
		}

		// Token: 0x0600A115 RID: 41237 RVA: 0x002E5998 File Offset: 0x002E3B98
		private void SetRelationsGainTickForPawns()
		{
			foreach (Pawn pawn in this.affectedPawns)
			{
				if (pawn.mindState != null)
				{
					pawn.mindState.SetNoAidRelationsGainUntilTick(Find.TickManager.TicksGame + 1800000);
				}
			}
		}

		// Token: 0x04006F55 RID: 28501
		public ExtraFaction extraFaction;

		// Token: 0x04006F56 RID: 28502
		public List<Pawn> affectedPawns = new List<Pawn>();

		// Token: 0x04006F57 RID: 28503
		public bool areHelpers;

		// Token: 0x04006F58 RID: 28504
		public string inSignalRemovePawn;

		// Token: 0x04006F59 RID: 28505
		public List<string> inSignalsRemovePawn;

		// Token: 0x04006F5A RID: 28506
		private const int RelationsGainAvailableInTicks = 1800000;
	}
}
