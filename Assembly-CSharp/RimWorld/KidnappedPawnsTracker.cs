using System;
using System.Collections.Generic;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020025A0 RID: 9632
	public class KidnappedPawnsTracker : IExposable
	{
		// Token: 0x1700207F RID: 8319
		// (get) Token: 0x0600D316 RID: 54038 RVA: 0x003E2B7D File Offset: 0x003E0D7D
		public List<Pawn> KidnappedPawnsListForReading
		{
			get
			{
				return this.kidnappedPawns;
			}
		}

		// Token: 0x0600D317 RID: 54039 RVA: 0x003E2B85 File Offset: 0x003E0D85
		public KidnappedPawnsTracker(Faction faction)
		{
			this.faction = faction;
		}

		// Token: 0x0600D318 RID: 54040 RVA: 0x003E2BA0 File Offset: 0x003E0DA0
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.kidnappedPawns.RemoveAll((Pawn x) => x.Destroyed);
			}
			Scribe_Collections.Look<Pawn>(ref this.kidnappedPawns, "kidnappedPawns", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x0600D319 RID: 54041 RVA: 0x003E2BF8 File Offset: 0x003E0DF8
		public void Kidnap(Pawn pawn, Pawn kidnapper)
		{
			if (this.kidnappedPawns.Contains(pawn))
			{
				Log.Error("Tried to kidnap already kidnapped pawn " + ((pawn != null) ? pawn.ToString() : null));
				return;
			}
			if (pawn.Faction == this.faction)
			{
				Log.Error("Tried to kidnap pawn with the same faction: " + ((pawn != null) ? pawn.ToString() : null));
				return;
			}
			pawn.PreKidnapped(kidnapper);
			pawn.DeSpawnOrDeselect(DestroyMode.Vanish);
			this.kidnappedPawns.Add(pawn);
			if (!Find.WorldPawns.Contains(pawn))
			{
				Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Decide);
				if (!Find.WorldPawns.Contains(pawn))
				{
					Log.Error("WorldPawns discarded kidnapped pawn.");
					this.kidnappedPawns.Remove(pawn);
				}
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(pawn, null, PawnDiedOrDownedThoughtsKind.Lost);
				BillUtility.Notify_ColonistUnavailable(pawn);
				if (kidnapper != null)
				{
					Find.LetterStack.ReceiveLetter("LetterLabelPawnsKidnapped".Translate(pawn.Named("PAWN")), "LetterPawnsKidnapped".Translate(pawn.Named("PAWN"), kidnapper.Faction.Named("FACTION")), LetterDefOf.NegativeEvent, null, 0, true);
				}
			}
			QuestUtility.SendQuestTargetSignals(pawn.questTags, "Kidnapped", pawn.Named("SUBJECT"), kidnapper.Named("KIDNAPPER"));
			Find.GameEnder.CheckOrUpdateGameOver();
		}

		// Token: 0x0600D31A RID: 54042 RVA: 0x003E2D58 File Offset: 0x003E0F58
		public void RemoveKidnappedPawn(Pawn pawn)
		{
			if (this.kidnappedPawns.Remove(pawn))
			{
				if (pawn.Faction == Faction.OfPlayer)
				{
					PawnDiedOrDownedThoughtsUtility.RemoveLostThoughts(pawn);
					return;
				}
			}
			else
			{
				Log.Warning("Tried to remove kidnapped pawn " + ((pawn != null) ? pawn.ToString() : null) + " but he's not here.");
			}
		}

		// Token: 0x0600D31B RID: 54043 RVA: 0x003E2DA8 File Offset: 0x003E0FA8
		public void LogKidnappedPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.faction.Name + ":");
			for (int i = 0; i < this.kidnappedPawns.Count; i++)
			{
				stringBuilder.AppendLine(this.kidnappedPawns[i].Name.ToStringFull);
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600D31C RID: 54044 RVA: 0x003E2E18 File Offset: 0x003E1018
		public void KidnappedPawnsTrackerTick()
		{
			for (int i = this.kidnappedPawns.Count - 1; i >= 0; i--)
			{
				if (this.kidnappedPawns[i].DestroyedOrNull())
				{
					this.kidnappedPawns.RemoveAt(i);
				}
			}
			if (Find.TickManager.TicksGame % 15051 == 0)
			{
				for (int j = this.kidnappedPawns.Count - 1; j >= 0; j--)
				{
					if (Rand.MTBEventOccurs(30f, 60000f, 15051f))
					{
						this.kidnappedPawns[j].SetFaction(this.faction, null);
						this.kidnappedPawns.RemoveAt(j);
					}
				}
			}
		}

		// Token: 0x04008F3D RID: 36669
		private Faction faction;

		// Token: 0x04008F3E RID: 36670
		private List<Pawn> kidnappedPawns = new List<Pawn>();

		// Token: 0x04008F3F RID: 36671
		private const int TryRecruitInterval = 15051;

		// Token: 0x04008F40 RID: 36672
		private const float RecruitMTBDays = 30f;
	}
}
