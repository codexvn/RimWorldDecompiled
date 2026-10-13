using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200335E RID: 13150
	public class ChoiceLetter_AcceptVisitors : ChoiceLetter
	{
		// Token: 0x17003030 RID: 12336
		// (get) Token: 0x060127BE RID: 75710 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003031 RID: 12337
		// (get) Token: 0x060127BF RID: 75711 RVA: 0x00574310 File Offset: 0x00572510
		public override bool CanShowInLetterStack
		{
			get
			{
				if (!base.CanShowInLetterStack)
				{
					return false;
				}
				if (this.quest == null || this.quest.State != QuestState.Ongoing)
				{
					return false;
				}
				bool result = false;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.CanStillAccept(this.pawns[i]))
					{
						result = true;
						break;
					}
				}
				return result;
			}
		}

		// Token: 0x17003032 RID: 12338
		// (get) Token: 0x060127C0 RID: 75712 RVA: 0x00574370 File Offset: 0x00572570
		private DiaOption Option_Accept
		{
			get
			{
				DiaOption diaOption = new DiaOption("AcceptButton".Translate());
				diaOption.action = delegate()
				{
					this.pawns.RemoveAll((Pawn x) => !this.CanStillAccept(x));
					if (!this.acceptedSignal.NullOrEmpty())
					{
						foreach (Pawn pawn in this.pawns)
						{
							QuestUtility.SendQuestTargetSignals(pawn.questTags, "Recruited", pawn.Named("SUBJECT"));
						}
						object arg = (this.pawns.Count == 1) ? this.pawns[0] : this.pawns;
						Find.SignalManager.SendSignal(new Signal(this.acceptedSignal, arg.Named("SUBJECT")));
					}
					Find.LetterStack.RemoveLetter(this);
				};
				diaOption.resolveTree = true;
				bool flag = false;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (this.CanStillAccept(this.pawns[i]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					diaOption.Disable(null);
				}
				return diaOption;
			}
		}

		// Token: 0x17003033 RID: 12339
		// (get) Token: 0x060127C1 RID: 75713 RVA: 0x005743E6 File Offset: 0x005725E6
		private DiaOption Option_RejectWithCharityConfirmation
		{
			get
			{
				return new DiaOption("RejectLetter".Translate())
				{
					action = delegate()
					{
						Action action = delegate()
						{
							if (!this.rejectedSignal.NullOrEmpty())
							{
								object arg = (this.pawns.Count == 1) ? this.pawns[0] : this.pawns;
								Find.SignalManager.SendSignal(new Signal(this.rejectedSignal, arg.Named("SUBJECT")));
							}
							Find.LetterStack.RemoveLetter(this);
						};
						if (!ModsConfig.IdeologyActive || !this.charity)
						{
							action();
							return;
						}
						IEnumerable<Pawn> source = IdeoUtility.AllColonistsWithCharityPrecept();
						if (source.Any<Pawn>())
						{
							string text = "";
							foreach (IGrouping<Ideo, Pawn> grouping in from c in source
							group c by c.Ideo)
							{
								text = text + "\n- " + "BelieversIn".Translate(grouping.Key.name.Colorize(grouping.Key.TextColor), grouping.Select((Pawn f) => f.NameShortColored.Resolve()).ToCommaList(false, false)).Resolve();
							}
							Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmationCharityJoiner".Translate(text), action, false, null, WindowLayer.Dialog));
							return;
						}
						action();
					},
					resolveTree = true
				};
			}
		}

		// Token: 0x17003034 RID: 12340
		// (get) Token: 0x060127C2 RID: 75714 RVA: 0x00574415 File Offset: 0x00572615
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (!base.ArchivedOnly)
				{
					yield return this.Option_Accept;
					yield return this.Option_RejectWithCharityConfirmation;
					yield return base.Option_Postpone;
				}
				else
				{
					yield return base.Option_Close;
				}
				if (this.lookTargets.IsValid())
				{
					yield return base.Option_JumpToLocationAndPostpone;
				}
				if (this.quest != null && !this.quest.hidden)
				{
					yield return base.Option_ViewInQuestsTab("ViewRelatedQuest", true);
				}
				yield break;
			}
		}

		// Token: 0x060127C3 RID: 75715 RVA: 0x00574428 File Offset: 0x00572628
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.acceptedSignal, "acceptedSignal", null, false);
			Scribe_Values.Look<string>(ref this.rejectedSignal, "rejectedSignal", null, false);
			Scribe_Values.Look<bool>(ref this.charity, "charity", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x060127C4 RID: 75716 RVA: 0x005744BC File Offset: 0x005726BC
		private bool CanStillAccept(Pawn p)
		{
			if (p.DestroyedOrNull() || !p.SpawnedOrAnyParentSpawned)
			{
				return false;
			}
			if (p.CurJob != null && p.CurJob.exitMapOnArrival)
			{
				return false;
			}
			if (p.Faction != null && p.Faction.HostileTo(Faction.OfPlayer))
			{
				return false;
			}
			Lord lord = p.GetLord();
			if (lord != null)
			{
				if (lord.CurLordToil is LordToil_ExitMap || lord.CurLordToil is LordToil_ExitMapRandom)
				{
					return false;
				}
				LordJob_VisitColony lordJob_VisitColony = lord.LordJob as LordJob_VisitColony;
				if (lordJob_VisitColony != null && lordJob_VisitColony.exitSubgraph.lordToils.Contains(lord.CurLordToil))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400C00F RID: 49167
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x0400C010 RID: 49168
		public string acceptedSignal;

		// Token: 0x0400C011 RID: 49169
		public string rejectedSignal;

		// Token: 0x0400C012 RID: 49170
		public bool charity;
	}
}
