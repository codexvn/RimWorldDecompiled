using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E42 RID: 15938
	public class WorldPawnGC : IExposable
	{
		// Token: 0x06015CAF RID: 89263 RVA: 0x00682ADC File Offset: 0x00680CDC
		public void WorldPawnGCTick()
		{
			if (this.lastSuccessfulGCTick < Find.TickManager.TicksGame / 15000 * 15000)
			{
				if (this.activeGCProcess == null)
				{
					this.activeGCProcess = this.PawnGCPass().GetEnumerator();
					if (DebugViewSettings.logWorldPawnGC)
					{
						Log.Message(string.Format("World pawn GC started at rate {0}", this.currentGCRate));
					}
				}
				if (this.activeGCProcess != null)
				{
					bool flag = false;
					int num = 0;
					while (num < this.currentGCRate && !flag)
					{
						flag = !this.activeGCProcess.MoveNext();
						num++;
					}
					if (flag)
					{
						this.lastSuccessfulGCTick = Find.TickManager.TicksGame;
						this.currentGCRate = 1;
						this.activeGCProcess = null;
						if (DebugViewSettings.logWorldPawnGC)
						{
							Log.Message("World pawn GC complete");
						}
					}
				}
			}
		}

		// Token: 0x06015CB0 RID: 89264 RVA: 0x00682BA3 File Offset: 0x00680DA3
		public void CancelGCPass()
		{
			if (this.activeGCProcess != null)
			{
				this.activeGCProcess = null;
				this.currentGCRate = Mathf.Min(this.currentGCRate * 2, 16777216);
				if (DebugViewSettings.logWorldPawnGC)
				{
					Log.Message("World pawn GC cancelled");
				}
			}
		}

		// Token: 0x06015CB1 RID: 89265 RVA: 0x00682BDD File Offset: 0x00680DDD
		private IEnumerable AccumulatePawnGCData(Dictionary<Pawn, string> keptPawns)
		{
			foreach (Pawn pawn in Find.WorldPawns.AllPawnsAliveOrDead)
			{
				string criticalPawnReason = this.GetCriticalPawnReason(pawn);
				if (!criticalPawnReason.NullOrEmpty())
				{
					keptPawns[pawn] = criticalPawnReason;
					if (this.logDotgraph != null)
					{
						this.logDotgraph.AppendLine(string.Format("{0} [label=<{0}<br/><font point-size=\"10\">{1}</font>> color=\"{2}\" shape=\"{3}\"];", new object[]
						{
							WorldPawnGC.DotgraphIdentifier(pawn),
							criticalPawnReason,
							(pawn.relations != null && pawn.relations.everSeenByPlayer) ? "black" : "grey",
							pawn.RaceProps.Humanlike ? "oval" : "box"
						}));
					}
				}
				else if (this.logDotgraph != null)
				{
					this.logDotgraph.AppendLine(string.Format("{0} [color=\"{1}\" shape=\"{2}\"];", WorldPawnGC.DotgraphIdentifier(pawn), (pawn.relations != null && pawn.relations.everSeenByPlayer) ? "black" : "grey", pawn.RaceProps.Humanlike ? "oval" : "box"));
				}
			}
			IEnumerable<Pawn> allPawnsAlive = Find.WorldPawns.AllPawnsAlive;
			Func<Pawn, bool> <>9__0;
			Func<Pawn, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((Pawn p) => p.RaceProps.Humanlike && !keptPawns.ContainsKey(p) && Rand.ChanceSeeded(0.25f, p.thingIDNumber ^ 980675837)));
			}
			foreach (Pawn key in allPawnsAlive.Where(predicate).Take(10))
			{
				keptPawns[key] = "RandomlyKept";
			}
			Pawn[] criticalPawns = keptPawns.Keys.ToArray<Pawn>();
			foreach (Pawn pawn2 in criticalPawns)
			{
				this.AddAllRelationships(pawn2, keptPawns);
				yield return null;
			}
			Pawn[] array = null;
			foreach (Pawn pawn3 in criticalPawns)
			{
				this.AddAllMemories(pawn3, keptPawns);
				yield return null;
			}
			array = null;
			yield break;
		}

		// Token: 0x06015CB2 RID: 89266 RVA: 0x00682BF4 File Offset: 0x00680DF4
		public Dictionary<Pawn, string> AccumulatePawnGCDataImmediate()
		{
			Dictionary<Pawn, string> dictionary = new Dictionary<Pawn, string>();
			this.AccumulatePawnGCData(dictionary).ExecuteEnumerable();
			return dictionary;
		}

		// Token: 0x06015CB3 RID: 89267 RVA: 0x00682C14 File Offset: 0x00680E14
		public string PawnGCDebugResults()
		{
			Dictionary<Pawn, string> dictionary = this.AccumulatePawnGCDataImmediate();
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			foreach (Pawn key in Find.WorldPawns.AllPawnsAliveOrDead)
			{
				string text = "Discarded";
				if (dictionary.ContainsKey(key))
				{
					text = dictionary[key];
				}
				if (!dictionary2.ContainsKey(text))
				{
					dictionary2[text] = 0;
				}
				Dictionary<string, int> dictionary3 = dictionary2;
				string key2 = text;
				int value = dictionary3[key2] + 1;
				dictionary3[key2] = value;
			}
			return (from kvp in dictionary2
			orderby kvp.Value descending
			select string.Format("{0}: {1}", kvp.Value, kvp.Key)).ToLineList(null, false);
		}

		// Token: 0x06015CB4 RID: 89268 RVA: 0x00682D08 File Offset: 0x00680F08
		public IEnumerable PawnGCPass()
		{
			Dictionary<Pawn, string> keptPawns = new Dictionary<Pawn, string>();
			Pawn[] worldPawnsSnapshot = Find.WorldPawns.AllPawnsAliveOrDead.ToArray();
			foreach (object obj in this.AccumulatePawnGCData(keptPawns))
			{
				yield return null;
			}
			IEnumerator enumerator = null;
			foreach (Pawn pawn in worldPawnsSnapshot)
			{
				if (pawn.IsWorldPawn() && !keptPawns.ContainsKey(pawn))
				{
					pawn.markedForDiscard = true;
				}
			}
			foreach (Pawn pawn2 in worldPawnsSnapshot)
			{
				if (pawn2.IsWorldPawn() && !keptPawns.ContainsKey(pawn2))
				{
					Find.WorldPawns.RemoveAndDiscardPawnViaGC(pawn2);
				}
			}
			Find.RelationshipRecords.CleanupUnusedRecords();
			yield break;
			yield break;
		}

		// Token: 0x06015CB5 RID: 89269 RVA: 0x00682D18 File Offset: 0x00680F18
		private string GetCriticalPawnReason(Pawn pawn)
		{
			if (pawn.Discarded)
			{
				return null;
			}
			if (PawnUtility.EverBeenColonistOrTameAnimal(pawn) && pawn.RaceProps.Humanlike)
			{
				return "Colonist";
			}
			if (pawn.RaceProps.Animal && TrainableUtility.GetAllColonistBondsFor(pawn).Any<Pawn>())
			{
				return "BondedAnimal";
			}
			if (PawnGenerator.IsBeingGenerated(pawn))
			{
				return "Generating";
			}
			if (PawnUtility.IsFactionLeader(pawn))
			{
				return "FactionLeader";
			}
			if (PawnUtility.IsKidnappedPawn(pawn))
			{
				return "Kidnapped";
			}
			if (pawn.IsCaravanMember())
			{
				return "CaravanMember";
			}
			if (PawnUtility.IsTravelingInTransportPodWorldObject(pawn))
			{
				return "TransportPod";
			}
			if (PawnUtility.ForSaleBySettlement(pawn))
			{
				return "ForSale";
			}
			if (Find.WorldPawns.ForcefullyKeptPawns.Contains(pawn))
			{
				return "ForceKept";
			}
			if (pawn.SpawnedOrAnyParentSpawned)
			{
				return "Spawned";
			}
			if (!pawn.Corpse.DestroyedOrNull())
			{
				return "CorpseExists";
			}
			if (pawn.RaceProps.Humanlike && Current.ProgramState == ProgramState.Playing)
			{
				if (Find.PlayLog.AnyEntryConcerns(pawn))
				{
					return "InPlayLog";
				}
				if (Find.BattleLog.AnyEntryConcerns(pawn))
				{
					return "InBattleLog";
				}
			}
			if (Current.ProgramState == ProgramState.Playing && Find.TaleManager.AnyActiveTaleConcerns(pawn))
			{
				return "InActiveTale";
			}
			if (QuestUtility.IsReservedByQuestOrQuestBeingGenerated(pawn))
			{
				return "ReservedByQuest";
			}
			if (Find.WorldPawns.PawnSourced(pawn))
			{
				return "CompPawnSource";
			}
			return null;
		}

		// Token: 0x06015CB6 RID: 89270 RVA: 0x00682E70 File Offset: 0x00681070
		public void AddAllRelationships(Pawn pawn, Dictionary<Pawn, string> keptPawns)
		{
			if (pawn.relations == null)
			{
				return;
			}
			if (!pawn.Spawned)
			{
				if (!pawn.RaceProps.Humanlike)
				{
					return;
				}
				if (!pawn.Faction.IsPlayerSafe())
				{
					return;
				}
			}
			foreach (Pawn pawn2 in pawn.relations.RelatedPawns)
			{
				if (this.logDotgraph != null)
				{
					string text = string.Format("{0}->{1} [label=<{2}> color=\"purple\"];", WorldPawnGC.DotgraphIdentifier(pawn), WorldPawnGC.DotgraphIdentifier(pawn2), pawn.GetRelations(pawn2).FirstOrDefault<PawnRelationDef>().ToString());
					if (!this.logDotgraphUniqueLinks.Contains(text))
					{
						this.logDotgraphUniqueLinks.Add(text);
						this.logDotgraph.AppendLine(text);
					}
				}
				if (!keptPawns.ContainsKey(pawn2))
				{
					keptPawns[pawn2] = "Relationship";
				}
			}
		}

		// Token: 0x06015CB7 RID: 89271 RVA: 0x00682F58 File Offset: 0x00681158
		public void AddAllMemories(Pawn pawn, Dictionary<Pawn, string> keptPawns)
		{
			if (pawn.needs == null || pawn.needs.mood == null || pawn.needs.mood.thoughts == null || pawn.needs.mood.thoughts.memories == null)
			{
				return;
			}
			if (!pawn.Spawned)
			{
				if (!pawn.RaceProps.Humanlike)
				{
					return;
				}
				if (!pawn.Faction.IsPlayerSafe())
				{
					return;
				}
			}
			foreach (Thought_Memory thought_Memory in pawn.needs.mood.thoughts.memories.Memories)
			{
				if (thought_Memory.otherPawn != null)
				{
					if (this.logDotgraph != null)
					{
						string text = string.Format("{0}->{1} [label=<{2}> color=\"orange\"];", WorldPawnGC.DotgraphIdentifier(pawn), WorldPawnGC.DotgraphIdentifier(thought_Memory.otherPawn), thought_Memory.def);
						if (!this.logDotgraphUniqueLinks.Contains(text))
						{
							this.logDotgraphUniqueLinks.Add(text);
							this.logDotgraph.AppendLine(text);
						}
					}
					if (!keptPawns.ContainsKey(thought_Memory.otherPawn))
					{
						keptPawns[thought_Memory.otherPawn] = "Memory";
					}
				}
			}
		}

		// Token: 0x06015CB8 RID: 89272 RVA: 0x0068309C File Offset: 0x0068129C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastSuccessfulGCTick, "lastSuccessfulGCTick", 0, false);
			Scribe_Values.Look<int>(ref this.currentGCRate, "nextGCRate", 1, false);
		}

		// Token: 0x06015CB9 RID: 89273 RVA: 0x006830C2 File Offset: 0x006812C2
		public void LogGC()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= GC =======");
			stringBuilder.AppendLine(this.PawnGCDebugResults());
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06015CBA RID: 89274 RVA: 0x006830EC File Offset: 0x006812EC
		public void RunGC()
		{
			this.CancelGCPass();
			PerfLogger.Reset();
			foreach (object obj in this.PawnGCPass())
			{
			}
			float num = PerfLogger.Duration() * 1000f;
			PerfLogger.Flush();
			Log.Message(string.Format("World pawn GC run complete in {0} ms", num));
		}

		// Token: 0x06015CBB RID: 89275 RVA: 0x0068316C File Offset: 0x0068136C
		public void LogDotgraph()
		{
			this.logDotgraph = new StringBuilder();
			this.logDotgraphUniqueLinks = new HashSet<string>();
			this.logDotgraph.AppendLine("digraph { rankdir=LR;");
			this.AccumulatePawnGCDataImmediate();
			this.logDotgraph.AppendLine("}");
			GUIUtility.systemCopyBuffer = this.logDotgraph.ToString();
			Log.Message("Dotgraph copied to clipboard");
			this.logDotgraph = null;
			this.logDotgraphUniqueLinks = null;
		}

		// Token: 0x06015CBC RID: 89276 RVA: 0x006831E0 File Offset: 0x006813E0
		public static string DotgraphIdentifier(Pawn pawn)
		{
			return new string((from ch in pawn.LabelShort
			where char.IsLetter(ch)
			select ch).ToArray<char>()) + "_" + pawn.thingIDNumber.ToString();
		}

		// Token: 0x0400F197 RID: 61847
		private int lastSuccessfulGCTick;

		// Token: 0x0400F198 RID: 61848
		private int currentGCRate = 1;

		// Token: 0x0400F199 RID: 61849
		private const int AdditionalRandomHumanlikes = 10;

		// Token: 0x0400F19A RID: 61850
		private const float AdditionalRandomHumanlikeKeepChance = 0.25f;

		// Token: 0x0400F19B RID: 61851
		private const int GCUpdateInterval = 15000;

		// Token: 0x0400F19C RID: 61852
		private IEnumerator activeGCProcess;

		// Token: 0x0400F19D RID: 61853
		private StringBuilder logDotgraph;

		// Token: 0x0400F19E RID: 61854
		private HashSet<string> logDotgraphUniqueLinks;
	}
}
