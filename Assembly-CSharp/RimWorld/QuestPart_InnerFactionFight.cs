using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001BCB RID: 7115
	public class QuestPart_InnerFactionFight : QuestPartActivable
	{
		// Token: 0x17001852 RID: 6226
		// (get) Token: 0x06009E65 RID: 40549 RVA: 0x002DD0A4 File Offset: 0x002DB2A4
		private bool OneSideDefeated
		{
			get
			{
				if (this.secondFaction == null)
				{
					return false;
				}
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].Destroyed && !this.pawns[i].Downed)
					{
						if (this.pawns[i].Faction == this.firstFaction)
						{
							num++;
						}
						else if (this.pawns[i].Faction == this.secondFaction)
						{
							num2++;
						}
					}
				}
				return num == 0 || num2 == 0;
			}
		}

		// Token: 0x06009E66 RID: 40550 RVA: 0x002DD140 File Offset: 0x002DB340
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			if (!this.OneSideDefeated)
			{
				List<FactionRelation> list = new List<FactionRelation>();
				foreach (Faction faction in Find.FactionManager.AllFactionsListForReading)
				{
					if (faction == this.firstFaction)
					{
						list.Add(new FactionRelation(faction, FactionRelationKind.Hostile));
					}
					else if (!faction.def.PermanentlyHostileTo(this.firstFaction.def))
					{
						list.Add(new FactionRelation(faction, FactionRelationKind.Neutral));
					}
				}
				this.secondFaction = FactionGenerator.NewGeneratedFactionWithRelations(this.secondFactionDef, list, true);
				this.secondFaction.temporary = true;
				Find.FactionManager.Add(this.secondFaction);
				List<Thing> list2 = new List<Thing>();
				List<Thing> list3 = new List<Thing>();
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].Destroyed)
					{
						if (i % 2 == 0)
						{
							this.pawns[i].SetFaction(this.secondFaction, null);
							list3.Add(this.pawns[i]);
						}
						else
						{
							list2.Add(this.pawns[i]);
							Lord lord = this.pawns[i].GetLord();
							if (lord != null)
							{
								lord.RemovePawn(this.pawns[i]);
							}
						}
					}
				}
				Lord lord2 = LordMaker.MakeNewLord(this.firstFaction, new LordJob_AssaultThings(this.firstFaction, list3, 1f, false), this.mapParent.Map, null);
				foreach (Thing thing in list2)
				{
					lord2.AddPawn((Pawn)thing);
				}
				lord2.inSignalLeave = this.outSignalComplete;
				Lord lord3 = LordMaker.MakeNewLord(this.secondFaction, new LordJob_AssaultThings(this.secondFaction, list2, 1f, false), this.mapParent.Map, null);
				foreach (Thing thing2 in list3)
				{
					lord3.AddPawn((Pawn)thing2);
				}
				lord3.inSignalLeave = this.outSignalComplete;
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

		// Token: 0x06009E67 RID: 40551 RVA: 0x002DD3F0 File Offset: 0x002DB5F0
		public override void QuestPartTick()
		{
			if (this.OneSideDefeated)
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignalComplete, false));
				base.Complete();
			}
		}

		// Token: 0x06009E68 RID: 40552 RVA: 0x002DD416 File Offset: 0x002DB616
		public override bool QuestPartReserves(Faction f)
		{
			return f == this.firstFaction || (this.secondFaction != null && f == this.secondFaction);
		}

		// Token: 0x06009E69 RID: 40553 RVA: 0x002DD436 File Offset: 0x002DB636
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.firstFaction == f)
			{
				this.firstFaction = null;
				return;
			}
			if (this.secondFaction == f)
			{
				this.secondFaction = null;
			}
		}

		// Token: 0x06009E6A RID: 40554 RVA: 0x002DD45C File Offset: 0x002DB65C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.factionBecameHostileSignal && this.secondFaction != null)
			{
				this.secondFaction.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, false, null, null);
			}
		}

		// Token: 0x06009E6B RID: 40555 RVA: 0x002DD4A8 File Offset: 0x002DB6A8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<Faction>(ref this.firstFaction, "firstFaction", false);
			Scribe_Defs.Look<FactionDef>(ref this.secondFactionDef, "secondFactionDef");
			Scribe_References.Look<Faction>(ref this.secondFaction, "secondFaction", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Collections.Look<string>(ref this.inSignals, "inSignals", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.outSignalEnabled, "outSignalEnabled", null, false);
			Scribe_Values.Look<string>(ref this.outSignalComplete, "outSignalComplete", null, false);
			Scribe_Values.Look<IntVec3>(ref this.meetingPos, "meetingPos", default(IntVec3), false);
			Scribe_Values.Look<string>(ref this.factionBecameHostileSignal, "factionBecameHostileSignal", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04006DA7 RID: 28071
		public Faction firstFaction;

		// Token: 0x04006DA8 RID: 28072
		public FactionDef secondFactionDef;

		// Token: 0x04006DA9 RID: 28073
		public Faction secondFaction;

		// Token: 0x04006DAA RID: 28074
		public IntVec3 meetingPos;

		// Token: 0x04006DAB RID: 28075
		public MapParent mapParent;

		// Token: 0x04006DAC RID: 28076
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04006DAD RID: 28077
		public List<string> inSignals = new List<string>();

		// Token: 0x04006DAE RID: 28078
		public string outSignalEnabled;

		// Token: 0x04006DAF RID: 28079
		public string outSignalComplete;

		// Token: 0x04006DB0 RID: 28080
		public string factionBecameHostileSignal;
	}
}
