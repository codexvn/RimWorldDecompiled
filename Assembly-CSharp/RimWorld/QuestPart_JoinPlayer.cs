using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CF0 RID: 7408
	public class QuestPart_JoinPlayer : QuestPart
	{
		// Token: 0x17001985 RID: 6533
		// (get) Token: 0x0600A56B RID: 42347 RVA: 0x002F5DC3 File Offset: 0x002F3FC3
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null)
				{
					yield return this.mapParent;
				}
				foreach (Pawn t in PawnsArriveQuestPartUtility.GetQuestLookTargets(this.pawns))
				{
					yield return t;
				}
				IEnumerator<Pawn> enumerator2 = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x17001986 RID: 6534
		// (get) Token: 0x0600A56C RID: 42348 RVA: 0x002F5DD3 File Offset: 0x002F3FD3
		public override bool IncreasesPopulation
		{
			get
			{
				return PawnsArriveQuestPartUtility.IncreasesPopulation(this.pawns, this.joinPlayer, this.makePrisoners);
			}
		}

		// Token: 0x0600A56D RID: 42349 RVA: 0x002F5DEC File Offset: 0x002F3FEC
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.pawns.RemoveAll((Pawn x) => x.Destroyed);
				if (this.joinPlayer)
				{
					using (List<Pawn>.Enumerator enumerator = this.pawns.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Pawn pawn = enumerator.Current;
							if (pawn.IsColonist || pawn.IsPrisonerOfColony || pawn.IsSlaveOfColony)
							{
								RecruitUtility.Recruit(pawn, Faction.OfPlayer, null);
							}
							else if (pawn.Faction != Faction.OfPlayer)
							{
								pawn.SetFaction(Faction.OfPlayer, null);
							}
						}
						return;
					}
				}
				if (this.makePrisoners)
				{
					for (int i = 0; i < this.pawns.Count; i++)
					{
						if (this.pawns[i].RaceProps.Humanlike)
						{
							if (!this.pawns[i].IsPrisonerOfColony)
							{
								this.pawns[i].guest.SetGuestStatus(Faction.OfPlayer, GuestStatus.Prisoner);
							}
							HealthUtility.TryAnesthetize(this.pawns[i]);
						}
					}
				}
			}
		}

		// Token: 0x0600A56E RID: 42350 RVA: 0x002F5F44 File Offset: 0x002F4144
		public override bool QuestPartReserves(Pawn p)
		{
			return this.pawns.Contains(p);
		}

		// Token: 0x0600A56F RID: 42351 RVA: 0x002F5F52 File Offset: 0x002F4152
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x0600A570 RID: 42352 RVA: 0x002F5F64 File Offset: 0x002F4164
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalResult, "outSignalResult", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.joinPlayer, "joinPlayer", false, false);
			Scribe_Values.Look<bool>(ref this.makePrisoners, "makePrisoners", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04007245 RID: 29253
		public string inSignal;

		// Token: 0x04007246 RID: 29254
		public string outSignalResult;

		// Token: 0x04007247 RID: 29255
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04007248 RID: 29256
		public bool joinPlayer;

		// Token: 0x04007249 RID: 29257
		public bool makePrisoners;

		// Token: 0x0400724A RID: 29258
		public MapParent mapParent;
	}
}
