using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200031F RID: 799
	public class Battle : IExposable, ILoadReferenceable
	{
		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x060013DF RID: 5087 RVA: 0x0005DE2D File Offset: 0x0005C02D
		public int Importance
		{
			get
			{
				return this.entries.Count;
			}
		}

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x060013E0 RID: 5088 RVA: 0x0005DE3A File Offset: 0x0005C03A
		public int CreationTimestamp
		{
			get
			{
				return this.creationTimestamp;
			}
		}

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x060013E1 RID: 5089 RVA: 0x0005DE42 File Offset: 0x0005C042
		public int LastEntryTimestamp
		{
			get
			{
				if (this.entries.Count <= 0)
				{
					return 0;
				}
				return this.entries[this.entries.Count - 1].Timestamp;
			}
		}

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x060013E2 RID: 5090 RVA: 0x0005DE71 File Offset: 0x0005C071
		public Battle AbsorbedBy
		{
			get
			{
				return this.absorbedBy;
			}
		}

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x060013E3 RID: 5091 RVA: 0x0005DE79 File Offset: 0x0005C079
		public List<LogEntry> Entries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x0005DE9F File Offset: 0x0005C09F
		public static Battle Create()
		{
			return new Battle
			{
				loadID = Find.UniqueIDsManager.GetNextBattleID(),
				creationTimestamp = Find.TickManager.TicksGame
			};
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x0005DEC8 File Offset: 0x0005C0C8
		public string GetName()
		{
			if (this.battleName.NullOrEmpty())
			{
				HashSet<Faction> hashSet = new HashSet<Faction>(from p in this.concerns
				select p.Faction);
				GrammarRequest request = default(GrammarRequest);
				if (this.concerns.Count == 1)
				{
					if (hashSet.Count((Faction f) => f != null) < 2)
					{
						request.Includes.Add(RulePackDefOf.Battle_Solo);
						request.Rules.AddRange(GrammarUtility.RulesForPawn("PARTICIPANT1", this.concerns.First<Pawn>(), null, true, true));
						goto IL_1D9;
					}
				}
				if (this.concerns.Count == 2)
				{
					request.Includes.Add(RulePackDefOf.Battle_Duel);
					request.Rules.AddRange(GrammarUtility.RulesForPawn("PARTICIPANT1", this.concerns.First<Pawn>(), null, true, true));
					request.Rules.AddRange(GrammarUtility.RulesForPawn("PARTICIPANT2", this.concerns.Last<Pawn>(), null, true, true));
				}
				else if (hashSet.Count == 1)
				{
					request.Includes.Add(RulePackDefOf.Battle_Internal);
					request.Rules.AddRange(GrammarUtility.RulesForFaction("FACTION1", hashSet.First<Faction>(), request.Constants, true));
				}
				else if (hashSet.Count == 2)
				{
					request.Includes.Add(RulePackDefOf.Battle_War);
					request.Rules.AddRange(GrammarUtility.RulesForFaction("FACTION1", hashSet.First<Faction>(), request.Constants, true));
					request.Rules.AddRange(GrammarUtility.RulesForFaction("FACTION2", hashSet.Last<Faction>(), request.Constants, true));
				}
				else
				{
					request.Includes.Add(RulePackDefOf.Battle_Brawl);
				}
				IL_1D9:
				this.battleName = GrammarResolver.Resolve("r_battlename", request, null, false, null, null, null, true);
			}
			return this.battleName;
		}

		// Token: 0x060013E7 RID: 5095 RVA: 0x0005E0CC File Offset: 0x0005C2CC
		public void Add(LogEntry entry)
		{
			this.entries.Insert(0, entry);
			foreach (Thing thing in entry.GetConcerns())
			{
				if (thing is Pawn)
				{
					this.concerns.Add(thing as Pawn);
				}
			}
			this.battleName = null;
		}

		// Token: 0x060013E8 RID: 5096 RVA: 0x0005E140 File Offset: 0x0005C340
		public void Absorb(Battle battle)
		{
			this.creationTimestamp = Mathf.Min(this.creationTimestamp, battle.creationTimestamp);
			this.entries.AddRange(battle.entries);
			this.concerns.AddRange(battle.concerns);
			this.entries = (from e in this.entries
			orderby e.Age
			select e).ToList<LogEntry>();
			battle.entries.Clear();
			battle.concerns.Clear();
			battle.absorbedBy = this;
			this.battleName = null;
		}

		// Token: 0x060013E9 RID: 5097 RVA: 0x0005E1DF File Offset: 0x0005C3DF
		public bool Concerns(Pawn pawn)
		{
			return this.concerns.Contains(pawn);
		}

		// Token: 0x060013EA RID: 5098 RVA: 0x0005E1F0 File Offset: 0x0005C3F0
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			if (!this.concerns.Contains(p))
			{
				return;
			}
			for (int i = this.entries.Count - 1; i >= 0; i--)
			{
				if (this.entries[i].Concerns(p))
				{
					if (!silentlyRemoveReferences)
					{
						string[] array = new string[5];
						array[0] = "Discarding pawn ";
						array[1] = ((p != null) ? p.ToString() : null);
						array[2] = ", but he is referenced by a battle log entry ";
						int num = 3;
						LogEntry logEntry = this.entries[i];
						array[num] = ((logEntry != null) ? logEntry.ToString() : null);
						array[4] = ".";
						Log.Warning(string.Concat(array));
					}
					this.entries.RemoveAt(i);
				}
			}
			this.concerns.Remove(p);
		}

		// Token: 0x060013EB RID: 5099 RVA: 0x0005E2A8 File Offset: 0x0005C4A8
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.creationTimestamp, "creationTimestamp", 0, false);
			Scribe_Collections.Look<LogEntry>(ref this.entries, "entries", LookMode.Deep, Array.Empty<object>());
			Scribe_References.Look<Battle>(ref this.absorbedBy, "absorbedBy", false);
			Scribe_Values.Look<string>(ref this.battleName, "battleName", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.concerns.Clear();
				foreach (Pawn item in this.entries.SelectMany((LogEntry e) => e.GetConcerns()).OfType<Pawn>())
				{
					this.concerns.Add(item);
				}
			}
		}

		// Token: 0x060013EC RID: 5100 RVA: 0x0005E394 File Offset: 0x0005C594
		public string GetUniqueLoadID()
		{
			return "Battle_" + this.loadID.ToString();
		}

		// Token: 0x040012E7 RID: 4839
		public const int TicksForBattleExit = 5000;

		// Token: 0x040012E8 RID: 4840
		private List<LogEntry> entries = new List<LogEntry>();

		// Token: 0x040012E9 RID: 4841
		private string battleName;

		// Token: 0x040012EA RID: 4842
		private Battle absorbedBy;

		// Token: 0x040012EB RID: 4843
		private HashSet<Pawn> concerns = new HashSet<Pawn>();

		// Token: 0x040012EC RID: 4844
		private int loadID;

		// Token: 0x040012ED RID: 4845
		private int creationTimestamp;
	}
}
