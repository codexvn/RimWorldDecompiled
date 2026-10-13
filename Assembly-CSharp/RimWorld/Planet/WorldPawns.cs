using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E49 RID: 15945
	public class WorldPawns : IExposable
	{
		// Token: 0x17003678 RID: 13944
		// (get) Token: 0x06015CD6 RID: 89302 RVA: 0x0068388B File Offset: 0x00681A8B
		public List<Pawn> AllPawnsAliveOrDead
		{
			get
			{
				this.allPawnsAliveOrDeadResult.Clear();
				this.allPawnsAliveOrDeadResult.AddRange(this.AllPawnsAlive);
				this.allPawnsAliveOrDeadResult.AddRange(this.AllPawnsDead);
				return this.allPawnsAliveOrDeadResult;
			}
		}

		// Token: 0x17003679 RID: 13945
		// (get) Token: 0x06015CD7 RID: 89303 RVA: 0x006838C0 File Offset: 0x00681AC0
		public List<Pawn> AllPawnsAlive
		{
			get
			{
				this.allPawnsAliveResult.Clear();
				this.allPawnsAliveResult.AddRange(this.pawnsAlive);
				this.allPawnsAliveResult.AddRange(this.pawnsMothballed);
				return this.allPawnsAliveResult;
			}
		}

		// Token: 0x1700367A RID: 13946
		// (get) Token: 0x06015CD8 RID: 89304 RVA: 0x006838F5 File Offset: 0x00681AF5
		public HashSet<Pawn> AllPawnsDead
		{
			get
			{
				return this.pawnsDead;
			}
		}

		// Token: 0x1700367B RID: 13947
		// (get) Token: 0x06015CD9 RID: 89305 RVA: 0x006838FD File Offset: 0x00681AFD
		public HashSet<Pawn> ForcefullyKeptPawns
		{
			get
			{
				return this.pawnsForcefullyKeptAsWorldPawns;
			}
		}

		// Token: 0x06015CDA RID: 89306 RVA: 0x00683908 File Offset: 0x00681B08
		public void WorldPawnsTick()
		{
			using (new ProfilerBlock("AlivePawnsTick"))
			{
				WorldPawns.tmpPawnsToTick.Clear();
				WorldPawns.tmpPawnsToTick.AddRange(this.pawnsAlive);
				for (int i = 0; i < WorldPawns.tmpPawnsToTick.Count; i++)
				{
					try
					{
						WorldPawns.tmpPawnsToTick[i].DoTick();
					}
					catch (Exception ex)
					{
						string str = "Exception ticking world pawn ";
						string str2 = WorldPawns.tmpPawnsToTick[i].ToStringSafe<Pawn>();
						string str3 = ". Suppressing further errors. ";
						Exception ex2 = ex;
						Log.ErrorOnce(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null), WorldPawns.tmpPawnsToTick[i].thingIDNumber ^ 1148571423);
					}
					try
					{
						if (this.ShouldAutoTendTo(WorldPawns.tmpPawnsToTick[i]))
						{
							TendUtility.DoTend(null, WorldPawns.tmpPawnsToTick[i], null);
							GeneUtility.SatisfyChemicalGenes(WorldPawns.tmpPawnsToTick[i]);
						}
					}
					catch (Exception ex3)
					{
						string str4 = "Exception tending to a world pawn ";
						string str5 = WorldPawns.tmpPawnsToTick[i].ToStringSafe<Pawn>();
						string str6 = ". Suppressing further errors. ";
						Exception ex4 = ex3;
						Log.ErrorOnce(str4 + str5 + str6 + ((ex4 != null) ? ex4.ToString() : null), WorldPawns.tmpPawnsToTick[i].thingIDNumber ^ 8765780);
					}
				}
				WorldPawns.tmpPawnsToTick.Clear();
			}
			using (new ProfilerBlock("MothballUpdate"))
			{
				if (Find.TickManager.TicksGame % 15000 == 0)
				{
					this.DoMothballProcessing();
				}
			}
			using (new ProfilerBlock("Discard"))
			{
				WorldPawns.tmpPawnsToRemove.Clear();
				foreach (Pawn pawn in this.pawnsDead)
				{
					if (pawn == null)
					{
						Log.ErrorOnce("Dead null world pawn detected, discarding.", 94424128);
						WorldPawns.tmpPawnsToRemove.Add(pawn);
					}
					else if (pawn.Discarded)
					{
						string str7 = "World pawn ";
						Pawn pawn2 = pawn;
						Log.Error(str7 + ((pawn2 != null) ? pawn2.ToString() : null) + " has been discarded while still being a world pawn. This should never happen, because discard destroy mode means that the pawn is no longer managed by anything. Pawn should have been removed from the world first.");
						WorldPawns.tmpPawnsToRemove.Add(pawn);
					}
				}
				for (int j = 0; j < WorldPawns.tmpPawnsToRemove.Count; j++)
				{
					this.pawnsDead.Remove(WorldPawns.tmpPawnsToRemove[j]);
				}
				WorldPawns.tmpPawnsToRemove.Clear();
			}
			using (new ProfilerBlock("WorldPawnGCTick"))
			{
				try
				{
					this.gc.WorldPawnGCTick();
				}
				catch (Exception ex5)
				{
					string str8 = "Error in WorldPawnGCTick(): ";
					Exception ex6 = ex5;
					Log.Error(str8 + ((ex6 != null) ? ex6.ToString() : null));
				}
			}
		}

		// Token: 0x06015CDB RID: 89307 RVA: 0x00683C7C File Offset: 0x00681E7C
		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.pawnsForcefullyKeptAsWorldPawns, true, "pawnsForcefullyKeptAsWorldPawns", LookMode.Reference);
			Scribe_Collections.Look<Pawn>(ref this.pawnsAlive, "pawnsAlive", LookMode.Deep);
			Scribe_Collections.Look<Pawn>(ref this.pawnsMothballed, "pawnsMothballed", LookMode.Deep);
			Scribe_Collections.Look<Pawn>(ref this.pawnsDead, true, "pawnsDead", LookMode.Deep);
			Scribe_Deep.Look<WorldPawnGC>(ref this.gc, "gc", Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.pawnsMothballed == null)
				{
					this.pawnsMothballed = new HashSet<Pawn>();
				}
				if (this.pawnsForcefullyKeptAsWorldPawns.RemoveWhere((Pawn x) => x == null) != 0)
				{
					Log.Error("Some pawnsForcefullyKeptAsWorldPawns were null after loading.");
				}
				if (this.pawnsAlive.RemoveWhere((Pawn x) => x == null) != 0)
				{
					Log.Error("Some pawnsAlive were null after loading.");
				}
				if (this.pawnsMothballed.RemoveWhere((Pawn x) => x == null) != 0)
				{
					Log.Error("Some pawnsMothballed were null after loading.");
				}
				if (this.pawnsDead.RemoveWhere((Pawn x) => x == null) != 0)
				{
					Log.Error("Some pawnsDead were null after loading.");
				}
				if (this.pawnsAlive.RemoveWhere((Pawn x) => x.def == null || x.kindDef == null) != 0)
				{
					Log.Error("Some pawnsAlive had null def after loading.");
				}
				if (this.pawnsMothballed.RemoveWhere((Pawn x) => x.def == null || x.kindDef == null) != 0)
				{
					Log.Error("Some pawnsMothballed had null def after loading.");
				}
				if (this.pawnsDead.RemoveWhere((Pawn x) => x.def == null || x.kindDef == null) != 0)
				{
					Log.Error("Some pawnsDead had null def after loading.");
				}
			}
		}

		// Token: 0x06015CDC RID: 89308 RVA: 0x00683E82 File Offset: 0x00682082
		public bool Contains(Pawn p)
		{
			return this.pawnsAlive.Contains(p) || this.pawnsMothballed.Contains(p) || this.pawnsDead.Contains(p);
		}

		// Token: 0x06015CDD RID: 89309 RVA: 0x00683EB0 File Offset: 0x006820B0
		public void PassToWorld(Pawn pawn, PawnDiscardDecideMode discardMode = PawnDiscardDecideMode.Decide)
		{
			if (pawn.Spawned)
			{
				Log.Error("Tried to call PassToWorld with spawned pawn: " + ((pawn != null) ? pawn.ToString() : null) + ". Despawn him first.");
				return;
			}
			if (this.Contains(pawn))
			{
				Log.Error("Tried to pass pawn " + ((pawn != null) ? pawn.ToString() : null) + " to world, but it's already here.");
				return;
			}
			if (discardMode == PawnDiscardDecideMode.KeepForever && pawn.Discarded)
			{
				Log.Error("Tried to pass a discarded pawn " + ((pawn != null) ? pawn.ToString() : null) + " to world with discardMode=Keep. Discarded pawns should never be stored in WorldPawns.");
				discardMode = PawnDiscardDecideMode.Decide;
			}
			if (PawnComponentsUtility.HasSpawnedComponents(pawn))
			{
				PawnComponentsUtility.RemoveComponentsOnDespawned(pawn);
			}
			switch (discardMode)
			{
			case PawnDiscardDecideMode.Decide:
				this.AddPawn(pawn);
				return;
			case PawnDiscardDecideMode.KeepForever:
				this.pawnsForcefullyKeptAsWorldPawns.Add(pawn);
				this.AddPawn(pawn);
				return;
			case PawnDiscardDecideMode.Discard:
				this.DiscardPawn(pawn, false);
				return;
			default:
				return;
			}
		}

		// Token: 0x06015CDE RID: 89310 RVA: 0x00683F88 File Offset: 0x00682188
		public void RemovePawn(Pawn p)
		{
			if (!this.Contains(p))
			{
				string[] array = new string[5];
				array[0] = "Tried to remove pawn ";
				array[1] = ((p != null) ? p.ToString() : null);
				array[2] = " from ";
				int num = 3;
				Type type = base.GetType();
				array[num] = ((type != null) ? type.ToString() : null);
				array[4] = ", but it's not here.";
				Log.Error(string.Concat(array));
			}
			this.gc.CancelGCPass();
			if (this.pawnsMothballed.Contains(p) && Find.TickManager.TicksGame % 15000 != 0)
			{
				try
				{
					p.TickMothballed(Find.TickManager.TicksGame % 15000);
				}
				catch (Exception ex)
				{
					string str = "Exception ticking mothballed world pawn (just before removing): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			this.pawnsAlive.Remove(p);
			this.pawnsMothballed.Remove(p);
			this.pawnsDead.Remove(p);
			this.pawnsForcefullyKeptAsWorldPawns.Remove(p);
			p.becameWorldPawnTickAbs = -1;
		}

		// Token: 0x06015CDF RID: 89311 RVA: 0x00684098 File Offset: 0x00682298
		public void RemoveAndDiscardPawnViaGC(Pawn p)
		{
			this.RemovePawn(p);
			this.DiscardPawn(p, true);
		}

		// Token: 0x06015CE0 RID: 89312 RVA: 0x006840AC File Offset: 0x006822AC
		public WorldPawnSituation GetSituation(Pawn p)
		{
			if (!this.Contains(p))
			{
				return WorldPawnSituation.None;
			}
			if (p.Dead || p.Destroyed)
			{
				return WorldPawnSituation.Dead;
			}
			if (PawnUtility.IsFactionLeader(p))
			{
				return WorldPawnSituation.FactionLeader;
			}
			if (p.wasLeftBehindStartingPawn)
			{
				return WorldPawnSituation.StartingPawnLeftBehind;
			}
			if (PawnUtility.IsKidnappedPawn(p))
			{
				return WorldPawnSituation.Kidnapped;
			}
			if (p.IsCaravanMember())
			{
				return WorldPawnSituation.CaravanMember;
			}
			if (QuestUtility.IsReservedByQuestOrQuestBeingGenerated(p))
			{
				return WorldPawnSituation.ReservedByQuest;
			}
			if (p.IsBorrowedByAnyFaction())
			{
				return WorldPawnSituation.Borrowed;
			}
			if (PawnUtility.IsTravelingInTransportPodWorldObject(p))
			{
				return WorldPawnSituation.InTravelingTransportPod;
			}
			if (PawnUtility.ForSaleBySettlement(p))
			{
				return WorldPawnSituation.ForSaleBySettlement;
			}
			if (p.teleporting)
			{
				return WorldPawnSituation.Teleporting;
			}
			return WorldPawnSituation.Free;
		}

		// Token: 0x06015CE1 RID: 89313 RVA: 0x00684134 File Offset: 0x00682334
		public IEnumerable<Pawn> GetPawnsBySituation(WorldPawnSituation situation)
		{
			return from x in this.AllPawnsAliveOrDead
			where this.GetSituation(x) == situation
			select x;
		}

		// Token: 0x06015CE2 RID: 89314 RVA: 0x0068416C File Offset: 0x0068236C
		public int GetPawnsBySituationCount(WorldPawnSituation situation)
		{
			int num = 0;
			foreach (Pawn p in this.pawnsAlive)
			{
				if (this.GetSituation(p) == situation)
				{
					num++;
				}
			}
			foreach (Pawn p2 in this.pawnsDead)
			{
				if (this.GetSituation(p2) == situation)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06015CE3 RID: 89315 RVA: 0x00684214 File Offset: 0x00682414
		private bool ShouldAutoTendTo(Pawn pawn)
		{
			return !pawn.Dead && !pawn.Destroyed && pawn.IsHashIntervalTick(7500) && !pawn.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(pawn);
		}

		// Token: 0x06015CE4 RID: 89316 RVA: 0x00684246 File Offset: 0x00682446
		public bool IsBeingDiscarded(Pawn p)
		{
			return this.pawnsBeingDiscarded.Contains(p);
		}

		// Token: 0x06015CE5 RID: 89317 RVA: 0x00684254 File Offset: 0x00682454
		public void Notify_PawnDestroyed(Pawn p)
		{
			if (this.pawnsAlive.Contains(p) || this.pawnsMothballed.Contains(p))
			{
				this.pawnsAlive.Remove(p);
				this.pawnsMothballed.Remove(p);
				this.pawnsDead.Add(p);
			}
		}

		// Token: 0x06015CE6 RID: 89318 RVA: 0x006842A4 File Offset: 0x006824A4
		private bool ShouldMothball(Pawn p)
		{
			return this.DefPreventingMothball(p) == null && !p.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(p);
		}

		// Token: 0x06015CE7 RID: 89319 RVA: 0x006842C4 File Offset: 0x006824C4
		private HediffDef DefPreventingMothball(Pawn p)
		{
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			WorldPawnSituation situation = this.GetSituation(p);
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (!hediffs[i].def.AlwaysAllowMothball && !hediffs[i].IsPermanent())
				{
					Hediff_MissingPart hediff_MissingPart = hediffs[i] as Hediff_MissingPart;
					if ((hediff_MissingPart == null || hediff_MissingPart.Bleeding) && (!hediffs[i].def.allowMothballIfLowPriorityWorldPawn || !WorldPawns.lowPrioritySituations.Contains(situation)))
					{
						return hediffs[i].def;
					}
				}
			}
			return null;
		}

		// Token: 0x06015CE8 RID: 89320 RVA: 0x00684364 File Offset: 0x00682564
		private void AddPawn(Pawn p)
		{
			this.gc.CancelGCPass();
			if (p.Dead || p.Destroyed)
			{
				this.pawnsDead.Add(p);
			}
			else
			{
				try
				{
					int num = 0;
					while (this.ShouldAutoTendTo(p) && num < 30)
					{
						TendUtility.DoTend(null, p, null);
						num++;
					}
				}
				catch (Exception ex)
				{
					string str = "Exception tending to a world pawn ";
					string str2 = p.ToStringSafe<Pawn>();
					string str3 = ". Suppressing further errors. ";
					Exception ex2 = ex;
					Log.ErrorOnce(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null), p.thingIDNumber ^ 8765780);
				}
				this.pawnsAlive.Add(p);
			}
			p.Notify_PassedToWorld();
		}

		// Token: 0x06015CE9 RID: 89321 RVA: 0x00684418 File Offset: 0x00682618
		private void DiscardPawn(Pawn p, bool silentlyRemoveReferences = false)
		{
			this.pawnsBeingDiscarded.Push(p);
			try
			{
				if (!p.Destroyed)
				{
					p.Destroy(DestroyMode.Vanish);
				}
				if (!p.Discarded)
				{
					p.Discard(silentlyRemoveReferences);
				}
			}
			finally
			{
				this.pawnsBeingDiscarded.Pop();
			}
		}

		// Token: 0x06015CEA RID: 89322 RVA: 0x00684470 File Offset: 0x00682670
		private void DoMothballProcessing()
		{
			WorldPawns.tmpPawnsToTick.AddRange(this.pawnsMothballed);
			for (int i = 0; i < WorldPawns.tmpPawnsToTick.Count; i++)
			{
				try
				{
					WorldPawns.tmpPawnsToTick[i].TickMothballed(15000);
				}
				catch (Exception ex)
				{
					string str = "Exception ticking mothballed world pawn. Suppressing further errors. ";
					Exception ex2 = ex;
					Log.ErrorOnce(str + ((ex2 != null) ? ex2.ToString() : null), WorldPawns.tmpPawnsToTick[i].thingIDNumber ^ 1535437893);
				}
			}
			WorldPawns.tmpPawnsToTick.Clear();
			WorldPawns.tmpPawnsToTick.AddRange(this.pawnsAlive);
			for (int j = 0; j < WorldPawns.tmpPawnsToTick.Count; j++)
			{
				Pawn pawn = WorldPawns.tmpPawnsToTick[j];
				if (this.ShouldMothball(pawn))
				{
					this.pawnsAlive.Remove(pawn);
					this.pawnsMothballed.Add(pawn);
				}
			}
			WorldPawns.tmpPawnsToTick.Clear();
		}

		// Token: 0x06015CEB RID: 89323 RVA: 0x00684568 File Offset: 0x00682768
		public void DebugRunMothballProcessing()
		{
			this.DoMothballProcessing();
			Log.Message(string.Format("World pawn mothball run complete", Array.Empty<object>()));
		}

		// Token: 0x06015CEC RID: 89324 RVA: 0x00684584 File Offset: 0x00682784
		public void UnpinAllForcefullyKeptPawns()
		{
			this.pawnsForcefullyKeptAsWorldPawns.Clear();
		}

		// Token: 0x06015CED RID: 89325 RVA: 0x00684591 File Offset: 0x00682791
		public bool PawnSourced(Pawn pawn)
		{
			return this.sourcedPawns.ContainsKey(pawn) || this.hediffPawns.ContainsKey(pawn);
		}

		// Token: 0x06015CEE RID: 89326 RVA: 0x006845AF File Offset: 0x006827AF
		public void AddPawnSource(Pawn pawn, CompHasPawnSources source)
		{
			if (!this.sourcedPawns.ContainsKey(pawn))
			{
				this.sourcedPawns[pawn] = new List<CompHasPawnSources>();
			}
			this.sourcedPawns[pawn].Add(source);
		}

		// Token: 0x06015CEF RID: 89327 RVA: 0x006845E4 File Offset: 0x006827E4
		public void RemovePawnSources(List<Pawn> pawns, CompHasPawnSources source)
		{
			foreach (Pawn key in pawns)
			{
				this.sourcedPawns[key].Remove(source);
				if (this.sourcedPawns[key].NullOrEmpty<CompHasPawnSources>())
				{
					this.sourcedPawns.Remove(key);
				}
			}
		}

		// Token: 0x06015CF0 RID: 89328 RVA: 0x00684660 File Offset: 0x00682860
		public void AddPreservedPawnHediff(Pawn pawn, Hediff hediff)
		{
			if (pawn == null || hediff == null)
			{
				return;
			}
			if (!this.hediffPawns.ContainsKey(pawn))
			{
				this.hediffPawns[pawn] = new List<Hediff>();
			}
			if (!this.hediffPawns[pawn].Contains(hediff))
			{
				this.hediffPawns[pawn].Add(hediff);
			}
		}

		// Token: 0x06015CF1 RID: 89329 RVA: 0x006846BC File Offset: 0x006828BC
		public void RemovePreservedPawnHediff(Pawn pawn, Hediff hediff)
		{
			if (pawn == null || hediff == null)
			{
				return;
			}
			if (this.hediffPawns[pawn].Contains(hediff))
			{
				this.hediffPawns[pawn].Remove(hediff);
			}
			if (this.hediffPawns[pawn].NullOrEmpty<Hediff>())
			{
				this.hediffPawns.Remove(pawn);
			}
		}

		// Token: 0x06015CF2 RID: 89330 RVA: 0x00684718 File Offset: 0x00682918
		public void LogWorldPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= World Pawns =======");
			StringBuilder stringBuilder2 = stringBuilder;
			string str = "Count: ";
			int i = this.AllPawnsAliveOrDead.Count<Pawn>();
			stringBuilder2.AppendLine(str + i.ToString());
			stringBuilder.AppendLine(string.Format("(Live: {0} - Mothballed: {1} - Dead: {2}; {3} forcefully kept)", new object[]
			{
				this.pawnsAlive.Count,
				this.pawnsMothballed.Count,
				this.pawnsDead.Count,
				this.pawnsForcefullyKeptAsWorldPawns.Count
			}));
			Dictionary<Pawn, string> dictionary = this.gc.AccumulatePawnGCDataImmediate();
			foreach (WorldPawnSituation worldPawnSituation in (WorldPawnSituation[])Enum.GetValues(typeof(WorldPawnSituation)))
			{
				if (worldPawnSituation != WorldPawnSituation.None)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("== " + worldPawnSituation.ToString() + " ==");
					foreach (Pawn pawn in this.GetPawnsBySituation(worldPawnSituation).OrderBy(delegate(Pawn x)
					{
						if (x.Faction != null)
						{
							return x.Faction.loadID;
						}
						return -1;
					}))
					{
						string text = (pawn.Name != null) ? pawn.Name.ToStringFull : pawn.LabelCap;
						text = text + ", " + pawn.KindLabel;
						if (pawn.royalty != null && pawn.royalty.AllTitlesForReading.Count > 0)
						{
							foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesForReading)
							{
								text = text + ", " + royalTitle.def.GetLabelFor(pawn);
							}
						}
						string str2 = text;
						string str3 = ", ";
						Faction faction = pawn.Faction;
						text = str2 + str3 + ((faction != null) ? faction.ToString() : null);
						text = text + " (" + pawn.ageTracker.AgeBiologicalYearsFloat.ToString() + ")";
						text = text + " - " + dictionary.GetValueOrDefault(pawn, "[will be discarded on next GC pass]");
						stringBuilder.AppendLine(text);
					}
				}
			}
			stringBuilder.AppendLine("===========================");
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06015CF3 RID: 89331 RVA: 0x006849E4 File Offset: 0x00682BE4
		public void LogWorldPawnMothballPrevention()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= World Pawns Mothball Prevention =======");
			stringBuilder.AppendLine(string.Format("Count: {0}", this.pawnsAlive.Count<Pawn>()));
			int num = 0;
			Dictionary<HediffDef, int> dictionary = new Dictionary<HediffDef, int>();
			foreach (Pawn p in this.pawnsAlive)
			{
				HediffDef hediffDef = this.DefPreventingMothball(p);
				if (hediffDef == null)
				{
					num++;
				}
				else
				{
					if (!dictionary.ContainsKey(hediffDef))
					{
						dictionary[hediffDef] = 0;
					}
					Dictionary<HediffDef, int> dictionary2 = dictionary;
					HediffDef key = hediffDef;
					int value = dictionary2[key] + 1;
					dictionary2[key] = value;
				}
			}
			stringBuilder.AppendLine(string.Format("Will be mothballed: {0}", num));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Reasons to avoid mothballing:");
			foreach (KeyValuePair<HediffDef, int> keyValuePair in from kvp in dictionary
			orderby kvp.Value descending
			select kvp)
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", keyValuePair.Value, keyValuePair.Key));
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0400F1C7 RID: 61895
		private static readonly HashSet<WorldPawnSituation> lowPrioritySituations = new HashSet<WorldPawnSituation>
		{
			WorldPawnSituation.Free,
			WorldPawnSituation.ForSaleBySettlement,
			WorldPawnSituation.FactionLeader,
			WorldPawnSituation.Kidnapped
		};

		// Token: 0x0400F1C8 RID: 61896
		private HashSet<Pawn> pawnsAlive = new HashSet<Pawn>();

		// Token: 0x0400F1C9 RID: 61897
		private HashSet<Pawn> pawnsMothballed = new HashSet<Pawn>();

		// Token: 0x0400F1CA RID: 61898
		private HashSet<Pawn> pawnsDead = new HashSet<Pawn>();

		// Token: 0x0400F1CB RID: 61899
		private HashSet<Pawn> pawnsForcefullyKeptAsWorldPawns = new HashSet<Pawn>();

		// Token: 0x0400F1CC RID: 61900
		public WorldPawnGC gc = new WorldPawnGC();

		// Token: 0x0400F1CD RID: 61901
		private Stack<Pawn> pawnsBeingDiscarded = new Stack<Pawn>();

		// Token: 0x0400F1CE RID: 61902
		private Dictionary<Pawn, List<CompHasPawnSources>> sourcedPawns = new Dictionary<Pawn, List<CompHasPawnSources>>();

		// Token: 0x0400F1CF RID: 61903
		private Dictionary<Pawn, List<Hediff>> hediffPawns = new Dictionary<Pawn, List<Hediff>>();

		// Token: 0x0400F1D0 RID: 61904
		private const int TendIntervalTicks = 7500;

		// Token: 0x0400F1D1 RID: 61905
		private const int MothballUpdateInterval = 15000;

		// Token: 0x0400F1D2 RID: 61906
		private List<Pawn> allPawnsAliveOrDeadResult = new List<Pawn>();

		// Token: 0x0400F1D3 RID: 61907
		private List<Pawn> allPawnsAliveResult = new List<Pawn>();

		// Token: 0x0400F1D4 RID: 61908
		private static List<Pawn> tmpPawnsToTick = new List<Pawn>();

		// Token: 0x0400F1D5 RID: 61909
		private static List<Pawn> tmpPawnsToRemove = new List<Pawn>();
	}
}
