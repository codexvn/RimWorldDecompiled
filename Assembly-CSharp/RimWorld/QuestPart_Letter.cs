using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C5E RID: 7262
	public class QuestPart_Letter : QuestPart
	{
		// Token: 0x170018DF RID: 6367
		// (get) Token: 0x0600A1C5 RID: 41413 RVA: 0x002E7E36 File Offset: 0x002E6036
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				GlobalTargetInfo globalTargetInfo2 = this.letter.lookTargets.TryGetPrimaryTarget();
				if (globalTargetInfo2.IsValid)
				{
					yield return globalTargetInfo2;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x170018E0 RID: 6368
		// (get) Token: 0x0600A1C6 RID: 41414 RVA: 0x002E7E46 File Offset: 0x002E6046
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.letter.relatedFaction != null)
				{
					yield return this.letter.relatedFaction;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A1C7 RID: 41415 RVA: 0x002E7E56 File Offset: 0x002E6056
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (this.letter.relatedFaction == faction)
			{
				this.letter.relatedFaction = null;
			}
		}

		// Token: 0x0600A1C8 RID: 41416 RVA: 0x002E7E74 File Offset: 0x002E6074
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (!string.IsNullOrEmpty(this.getColonistsFromSignal) && signal.tag == this.getColonistsFromSignal)
			{
				NamedArgument namedArgument;
				if (signal.args.TryGetArg("SUBJECT", out namedArgument))
				{
					this.<Notify_QuestSignalReceived>g__ReadPawns|16_0(namedArgument.arg);
				}
				NamedArgument namedArgument2;
				if (signal.args.TryGetArg("SENT", out namedArgument2))
				{
					this.<Notify_QuestSignalReceived>g__ReadPawns|16_0(namedArgument2.arg);
				}
			}
			if (signal.tag == this.inSignal)
			{
				Letter letter = Gen.MemberwiseClone<Letter>(this.letter);
				letter.ID = Find.UniqueIDsManager.GetNextLetterID();
				ChoiceLetter choiceLetter = letter as ChoiceLetter;
				if (choiceLetter != null)
				{
					choiceLetter.quest = this.quest;
				}
				ChoiceLetter_ChoosePawn choiceLetter_ChoosePawn = letter as ChoiceLetter_ChoosePawn;
				if (choiceLetter_ChoosePawn != null)
				{
					if (this.useColonistsOnMap != null && this.useColonistsOnMap.HasMap)
					{
						choiceLetter_ChoosePawn.pawns.Clear();
						choiceLetter_ChoosePawn.pawns.AddRange(this.useColonistsOnMap.Map.mapPawns.FreeColonists);
						choiceLetter_ChoosePawn.chosenPawnSignal = this.chosenPawnSignal;
					}
					Caravan caravan;
					if (this.useColonistsFromCaravanArg && signal.args.TryGetArg<Caravan>("CARAVAN", out caravan) && caravan != null)
					{
						choiceLetter_ChoosePawn.pawns.Clear();
						choiceLetter_ChoosePawn.pawns.AddRange(from x in caravan.PawnsListForReading
						where x.IsFreeColonist
						select x);
						choiceLetter_ChoosePawn.chosenPawnSignal = this.chosenPawnSignal;
					}
					if (!string.IsNullOrEmpty(this.getColonistsFromSignal))
					{
						this.colonistsFromSignal.RemoveAll((Pawn x) => x.Dead);
						choiceLetter_ChoosePawn.pawns.Clear();
						choiceLetter_ChoosePawn.pawns.AddRange(this.colonistsFromSignal);
						choiceLetter_ChoosePawn.chosenPawnSignal = this.chosenPawnSignal;
					}
				}
				ChoiceLetter_AcceptVisitors choiceLetter_AcceptVisitors = letter as ChoiceLetter_AcceptVisitors;
				if (choiceLetter_AcceptVisitors != null)
				{
					choiceLetter_AcceptVisitors.acceptedSignal = this.acceptedVisitorsSignal;
					if (this.visitors != null)
					{
						choiceLetter_AcceptVisitors.pawns.AddRange(this.visitors);
					}
				}
				LookTargets lookTargets;
				if (this.getLookTargetsFromSignal && !letter.lookTargets.IsValid() && SignalArgsUtility.TryGetLookTargets(signal.args, "SUBJECT", out lookTargets))
				{
					letter.lookTargets = lookTargets;
				}
				letter.Label = signal.args.GetFormattedText(letter.Label);
				ChoiceLetter choiceLetter2 = letter as ChoiceLetter;
				bool flag = true;
				if (choiceLetter2 != null)
				{
					choiceLetter2.title = signal.args.GetFormattedText(choiceLetter2.title);
					choiceLetter2.Text = signal.args.GetFormattedText(choiceLetter2.Text);
					if (choiceLetter2.Text.NullOrEmpty())
					{
						flag = false;
					}
				}
				if (this.filterDeadPawnsFromLookTargets)
				{
					for (int i = letter.lookTargets.targets.Count - 1; i >= 0; i--)
					{
						Thing thing = letter.lookTargets.targets[i].Thing;
						Pawn pawn = thing as Pawn;
						if (pawn != null && pawn.Dead)
						{
							letter.lookTargets.targets.Remove(thing);
						}
					}
				}
				if (flag)
				{
					Find.LetterStack.ReceiveLetter(letter, null, 0, true);
				}
			}
		}

		// Token: 0x0600A1C9 RID: 41417 RVA: 0x002E81C8 File Offset: 0x002E63C8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Deep.Look<Letter>(ref this.letter, "letter", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.getLookTargetsFromSignal, "getLookTargetsFromSignal", true, false);
			Scribe_References.Look<MapParent>(ref this.useColonistsOnMap, "useColonistsOnMap", false);
			Scribe_Values.Look<bool>(ref this.useColonistsFromCaravanArg, "useColonistsFromCaravanArg", false, false);
			Scribe_Values.Look<string>(ref this.chosenPawnSignal, "chosenPawnSignal", null, false);
			Scribe_Values.Look<bool>(ref this.filterDeadPawnsFromLookTargets, "filterDeadPawnsFromLookTargets", false, false);
			Scribe_Values.Look<string>(ref this.getColonistsFromSignal, "getColonistsFromSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.colonistsFromSignal, "colonistsFromSignal", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				this.colonistsFromSignal.RemoveAll((Pawn x) => x == null);
			}
			Scribe_Values.Look<string>(ref this.acceptedVisitorsSignal, "acceptedVisitorsSignal", null, false);
			Scribe_Collections.Look<Pawn>(ref this.visitors, "visitors", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<Pawn> list = this.visitors;
				if (list == null)
				{
					return;
				}
				list.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x0600A1CA RID: 41418 RVA: 0x002E8318 File Offset: 0x002E6518
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			this.letter = LetterMaker.MakeLetter("DEV: Test", "Test text", LetterDefOf.PositiveEvent, null, null);
		}

		// Token: 0x0600A1CE RID: 41422 RVA: 0x002E8388 File Offset: 0x002E6588
		[CompilerGenerated]
		private void <Notify_QuestSignalReceived>g__ReadPawns|16_0(object obj)
		{
			Pawn pawn = obj as Pawn;
			if (pawn != null && !this.colonistsFromSignal.Contains(pawn))
			{
				this.colonistsFromSignal.Add(pawn);
			}
			List<Pawn> list = obj as List<Pawn>;
			if (list != null)
			{
				this.colonistsFromSignal.AddRange(from p in list
				where !this.colonistsFromSignal.Contains(p)
				select p);
			}
			List<Thing> list2 = obj as List<Thing>;
			if (list2 != null)
			{
				this.colonistsFromSignal.AddRange(from Pawn p in 
					from t in list2
					where t is Pawn
					select t
				where !this.colonistsFromSignal.Contains(p)
				select p);
			}
		}

		// Token: 0x04006FC4 RID: 28612
		public string inSignal;

		// Token: 0x04006FC5 RID: 28613
		public Letter letter;

		// Token: 0x04006FC6 RID: 28614
		public bool getLookTargetsFromSignal = true;

		// Token: 0x04006FC7 RID: 28615
		public MapParent useColonistsOnMap;

		// Token: 0x04006FC8 RID: 28616
		public string getColonistsFromSignal;

		// Token: 0x04006FC9 RID: 28617
		public bool useColonistsFromCaravanArg;

		// Token: 0x04006FCA RID: 28618
		public string chosenPawnSignal;

		// Token: 0x04006FCB RID: 28619
		public string acceptedVisitorsSignal;

		// Token: 0x04006FCC RID: 28620
		public List<Pawn> visitors;

		// Token: 0x04006FCD RID: 28621
		public bool filterDeadPawnsFromLookTargets;

		// Token: 0x04006FCE RID: 28622
		private List<Pawn> colonistsFromSignal = new List<Pawn>();
	}
}
