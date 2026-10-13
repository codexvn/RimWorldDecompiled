using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02001090 RID: 4240
	[StaticConstructorOnStartup]
	public class Lord : IExposable, ILoadReferenceable, ISignalReceiver, IDisposable
	{
		// Token: 0x17001004 RID: 4100
		// (get) Token: 0x060067B2 RID: 26546 RVA: 0x0020DB25 File Offset: 0x0020BD25
		public Map Map
		{
			get
			{
				return this.lordManager.map;
			}
		}

		// Token: 0x17001005 RID: 4101
		// (get) Token: 0x060067B3 RID: 26547 RVA: 0x0020DB32 File Offset: 0x0020BD32
		public StateGraph Graph
		{
			get
			{
				return this.graph;
			}
		}

		// Token: 0x17001006 RID: 4102
		// (get) Token: 0x060067B4 RID: 26548 RVA: 0x0020DB3A File Offset: 0x0020BD3A
		public LordToil CurLordToil
		{
			get
			{
				return this.curLordToil;
			}
		}

		// Token: 0x17001007 RID: 4103
		// (get) Token: 0x060067B5 RID: 26549 RVA: 0x0020DB42 File Offset: 0x0020BD42
		public LordJob LordJob
		{
			get
			{
				return this.curJob;
			}
		}

		// Token: 0x17001008 RID: 4104
		// (get) Token: 0x060067B6 RID: 26550 RVA: 0x0020DB4A File Offset: 0x0020BD4A
		private bool CanExistWithoutPawns
		{
			get
			{
				return this.curJob.ShouldExistWithoutPawns || this.Map.deferredSpawner.GetRequestByLord(this) != null;
			}
		}

		// Token: 0x17001009 RID: 4105
		// (get) Token: 0x060067B7 RID: 26551 RVA: 0x0020DB6F File Offset: 0x0020BD6F
		private bool ShouldExist
		{
			get
			{
				return this.ownedPawns.Count > 0 || this.CanExistWithoutPawns || (this.ownedBuildings.Count > 0 && this.curJob.KeepExistingWhileHasAnyBuilding);
			}
		}

		// Token: 0x1700100A RID: 4106
		// (get) Token: 0x060067B8 RID: 26552 RVA: 0x0020DBA4 File Offset: 0x0020BDA4
		public bool AnyActivePawn
		{
			get
			{
				for (int i = 0; i < this.ownedPawns.Count; i++)
				{
					if (this.ownedPawns[i].mindState != null && this.ownedPawns[i].mindState.Active)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060067B9 RID: 26553 RVA: 0x0020DBF5 File Offset: 0x0020BDF5
		private void Init()
		{
			this.initialized = true;
			this.initialColonyHealthTotal = this.Map.wealthWatcher.HealthTotal;
		}

		// Token: 0x060067BA RID: 26554 RVA: 0x0020DC14 File Offset: 0x0020BE14
		public string GetUniqueLoadID()
		{
			return "Lord_" + this.loadID.ToString();
		}

		// Token: 0x060067BB RID: 26555 RVA: 0x0020DC2C File Offset: 0x0020BE2C
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.extraForbiddenThings.RemoveAll((Thing x) => x.DestroyedOrNull());
				this.ownedPawns.RemoveAll((Pawn x) => x.DestroyedOrNull());
				this.ownedBuildings.RemoveAll((Building x) => x.DestroyedOrNull());
				this.ownedCorpses.RemoveAll((Corpse x) => x.DestroyedOrNull());
			}
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Collections.Look<Thing>(ref this.extraForbiddenThings, "extraForbiddenThings", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Pawn>(ref this.ownedPawns, "ownedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Building>(ref this.ownedBuildings, "ownedBuildings", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Corpse>(ref this.ownedCorpses, "ownedCorpses", LookMode.Reference, Array.Empty<object>());
			Scribe_Deep.Look<LordJob>(ref this.curJob, "lordJob", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.initialized, "initialized", true, false);
			Scribe_Values.Look<int>(ref this.ticksInToil, "ticksInToil", 0, false);
			Scribe_Values.Look<int>(ref this.numPawnsEverGained, "numPawnsEverGained", 0, false);
			Scribe_Values.Look<int>(ref this.numPawnsLostViolently, "numPawnsLostViolently", 0, false);
			Scribe_Values.Look<int>(ref this.initialColonyHealthTotal, "initialColonyHealthTotal", 0, false);
			Scribe_Values.Look<int>(ref this.lastPawnHarmTick, "lastPawnHarmTick", -99999, false);
			Scribe_Values.Look<string>(ref this.inSignalLeave, "inSignalLeave", null, false);
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.extraForbiddenThings.RemoveAll((Thing x) => x == null);
				this.ownedPawns.RemoveAll((Pawn x) => x == null);
				this.ownedBuildings.RemoveAll((Building x) => x == null);
				this.ownedCorpses.RemoveAll((Corpse x) => x == null);
				this.ownedPawns.ForEach(delegate(Pawn p)
				{
					p.lord = this;
				});
			}
			this.ExposeData_StateGraph();
		}

		// Token: 0x060067BC RID: 26556 RVA: 0x0020DEEC File Offset: 0x0020C0EC
		public AcceptanceReport AllowsDrafting(Pawn pawn)
		{
			if (this.curJob == null)
			{
				return false;
			}
			return this.curJob.AllowsDrafting(pawn);
		}

		// Token: 0x060067BD RID: 26557 RVA: 0x0020DF09 File Offset: 0x0020C109
		public AcceptanceReport AllowsFloatMenu(Pawn pawn)
		{
			if (this.curJob == null)
			{
				return true;
			}
			return this.curJob.AllowsFloatMenu(pawn);
		}

		// Token: 0x060067BE RID: 26558 RVA: 0x0020DF26 File Offset: 0x0020C126
		public bool BlocksSocialInteraction(Pawn pawn)
		{
			LordJob lordJob = this.curJob;
			return lordJob != null && lordJob.BlocksSocialInteraction(pawn);
		}

		// Token: 0x060067BF RID: 26559 RVA: 0x0020DF3A File Offset: 0x0020C13A
		public bool PrisonerSecure(Pawn pawn)
		{
			LordJob lordJob = this.curJob;
			return lordJob != null && lordJob.PrisonerSecure(pawn);
		}

		// Token: 0x060067C0 RID: 26560 RVA: 0x0020DF4E File Offset: 0x0020C14E
		public AcceptanceReport AbilityAllowed(Ability ability)
		{
			if (this.curJob == null)
			{
				return true;
			}
			return this.curJob.AbilityAllowed(ability);
		}

		// Token: 0x060067C1 RID: 26561 RVA: 0x0020DF6B File Offset: 0x0020C16B
		public ThinkResult Notify_DutyConstantResult(ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			if (this.curJob == null)
			{
				return result;
			}
			return this.curJob.Notify_DutyConstantResult(result, pawn, issueParams);
		}

		// Token: 0x060067C2 RID: 26562 RVA: 0x0020DF85 File Offset: 0x0020C185
		public ThinkResult Notify_DutyResult(ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			if (this.curJob == null)
			{
				return result;
			}
			return this.curJob.Notify_DutyResult(result, pawn, issueParams);
		}

		// Token: 0x060067C3 RID: 26563 RVA: 0x0020DFA0 File Offset: 0x0020C1A0
		private void ExposeData_StateGraph()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpLordToilData.Clear();
				for (int i = 0; i < this.graph.lordToils.Count; i++)
				{
					if (this.graph.lordToils[i].data != null)
					{
						this.tmpLordToilData.Add(i, this.graph.lordToils[i].data);
					}
				}
				this.tmpTriggerData.Clear();
				int num = 0;
				for (int j = 0; j < this.graph.transitions.Count; j++)
				{
					for (int k = 0; k < this.graph.transitions[j].triggers.Count; k++)
					{
						if (this.graph.transitions[j].triggers[k].data != null)
						{
							this.tmpTriggerData.Add(num, this.graph.transitions[j].triggers[k].data);
						}
						num++;
					}
				}
				this.tmpCurLordToilIdx = this.graph.lordToils.IndexOf(this.curLordToil);
			}
			Scribe_Collections.Look<int, LordToilData>(ref this.tmpLordToilData, "lordToilData", LookMode.Value, LookMode.Deep);
			Scribe_Collections.Look<int, TriggerData>(ref this.tmpTriggerData, "triggerData", LookMode.Value, LookMode.Deep);
			Scribe_Values.Look<int>(ref this.tmpCurLordToilIdx, "curLordToilIdx", -1, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.curJob.LostImportantReferenceDuringLoading)
				{
					this.lordManager.RemoveLord(this);
					return;
				}
				LordJob lordJob = this.curJob;
				this.curJob = null;
				this.SetJob(lordJob, true);
				foreach (KeyValuePair<int, LordToilData> keyValuePair in this.tmpLordToilData)
				{
					if (keyValuePair.Key < 0 || keyValuePair.Key >= this.graph.lordToils.Count)
					{
						string[] array = new string[6];
						array[0] = "Could not find lord toil for lord toil data of type \"";
						int num2 = 1;
						Type type = keyValuePair.Value.GetType();
						array[num2] = ((type != null) ? type.ToString() : null);
						array[2] = "\" (lord job: \"";
						int num3 = 3;
						Type type2 = this.curJob.GetType();
						array[num3] = ((type2 != null) ? type2.ToString() : null);
						array[4] = "\"), because lord toil index is out of bounds: ";
						array[5] = keyValuePair.Key.ToString();
						Log.Error(string.Concat(array));
					}
					else
					{
						this.graph.lordToils[keyValuePair.Key].data = keyValuePair.Value;
					}
				}
				this.tmpLordToilData.Clear();
				foreach (KeyValuePair<int, TriggerData> keyValuePair2 in this.tmpTriggerData)
				{
					Trigger triggerByIndex = this.GetTriggerByIndex(keyValuePair2.Key);
					if (triggerByIndex == null)
					{
						string[] array2 = new string[6];
						array2[0] = "Could not find trigger for trigger data of type \"";
						int num4 = 1;
						Type type3 = keyValuePair2.Value.GetType();
						array2[num4] = ((type3 != null) ? type3.ToString() : null);
						array2[2] = "\" (lord job: \"";
						int num5 = 3;
						Type type4 = this.curJob.GetType();
						array2[num5] = ((type4 != null) ? type4.ToString() : null);
						array2[4] = "\"), because trigger index is out of bounds: ";
						array2[5] = keyValuePair2.Key.ToString();
						Log.Error(string.Concat(array2));
					}
					else
					{
						triggerByIndex.data = keyValuePair2.Value;
					}
				}
				this.tmpTriggerData.Clear();
				if (this.tmpCurLordToilIdx < 0 || this.tmpCurLordToilIdx >= this.graph.lordToils.Count)
				{
					string str = "Current lord toil index out of bounds (lord job: \"";
					Type type5 = this.curJob.GetType();
					Log.Error(str + ((type5 != null) ? type5.ToString() : null) + "\"): " + this.tmpCurLordToilIdx.ToString());
					return;
				}
				this.curLordToil = this.graph.lordToils[this.tmpCurLordToilIdx];
			}
		}

		// Token: 0x060067C4 RID: 26564 RVA: 0x0020E3AC File Offset: 0x0020C5AC
		public void SetJob(LordJob lordJob, bool loading = false)
		{
			if (this.curJob != null)
			{
				this.curJob.Cleanup();
			}
			this.curJob = lordJob;
			this.curLordToil = null;
			lordJob.lord = this;
			if (!loading)
			{
				lordJob.Notify_AddedToLord();
			}
			Rand.PushState();
			Rand.Seed = this.loadID * 193;
			this.graph = lordJob.CreateGraph();
			Rand.PopState();
			this.graph.ErrorCheck();
			if (this.faction != null && !this.faction.IsPlayer && this.faction.def.autoFlee && !this.faction.neverFlee && lordJob.AddFleeToil && lordJob.lord.Map.CanEverExit)
			{
				LordToil_PanicFlee lordToil_PanicFlee = new LordToil_PanicFlee();
				lordToil_PanicFlee.useAvoidGrid = true;
				Action <>9__0;
				for (int i = 0; i < this.graph.lordToils.Count; i++)
				{
					Transition transition = new Transition(this.graph.lordToils[i], lordToil_PanicFlee, false, true);
					transition.AddPreAction(new TransitionAction_Message("MessageFightersFleeing".Translate(this.faction.def.pawnsPlural.CapitalizeFirst(), this.faction.Name), null, 1f));
					transition.AddTrigger(new Trigger_FractionPawnsLost(this.faction.def.attackersDownPercentageRangeForAutoFlee.RandomInRangeSeeded(this.loadID)));
					Transition transition2 = transition;
					Action action;
					if ((action = <>9__0) == null)
					{
						action = (<>9__0 = delegate()
						{
							QuestUtility.SendQuestTargetSignals(lordJob.lord.questTags, "Fleeing", lordJob.lord.Named("SUBJECT"));
						});
					}
					transition2.AddPostAction(new TransitionAction_Custom(action));
					this.graph.AddTransition(transition, true);
				}
				this.graph.AddToil(lordToil_PanicFlee);
			}
			for (int j = 0; j < this.graph.lordToils.Count; j++)
			{
				this.graph.lordToils[j].lord = this;
			}
			for (int k = 0; k < this.ownedPawns.Count; k++)
			{
				this.Map.attackTargetsCache.UpdateTarget(this.ownedPawns[k]);
			}
		}

		// Token: 0x060067C5 RID: 26565 RVA: 0x0020E614 File Offset: 0x0020C814
		public void Cleanup()
		{
			try
			{
				this.curJob.Cleanup();
			}
			catch (Exception ex)
			{
				string str = "Error in LordJob.Cleanup(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
			if (this.curLordToil != null)
			{
				try
				{
					this.curLordToil.Cleanup();
				}
				catch (Exception ex3)
				{
					string str2 = "Error in LordToil.Cleanup(): ";
					Exception ex4 = ex3;
					Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
				}
			}
			for (int i = 0; i < this.ownedPawns.Count; i++)
			{
				if (this.ownedPawns[i].mindState != null)
				{
					this.ownedPawns[i].mindState.duty = null;
				}
				if (this.ownedPawns[i].lord == this)
				{
					this.ownedPawns[i].lord = null;
				}
				this.Map.attackTargetsCache.UpdateTarget(this.ownedPawns[i]);
				if (this.curJob.EndPawnJobOnCleanup(this.ownedPawns[i]) && this.ownedPawns[i].Spawned && this.ownedPawns[i].CurJob != null && (!this.curJob.DontInterruptLayingPawnsOnCleanup || !RestUtility.IsLayingForJobCleanup(this.ownedPawns[i])))
				{
					this.ownedPawns[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
			try
			{
				this.curJob.PostCleanup();
			}
			catch (Exception ex5)
			{
				string str3 = "Error in LordJob.PostCleanup(): ";
				Exception ex6 = ex5;
				Log.Error(str3 + ((ex6 != null) ? ex6.ToString() : null));
			}
		}

		// Token: 0x060067C6 RID: 26566 RVA: 0x0020E7D8 File Offset: 0x0020C9D8
		public bool CanAddPawn(Pawn p)
		{
			return this.curLordToil.CanAddPawn(p);
		}

		// Token: 0x060067C7 RID: 26567 RVA: 0x0020E7E8 File Offset: 0x0020C9E8
		public void AddPawns(IEnumerable<Pawn> pawns, bool updateDuties = true)
		{
			foreach (Pawn p in pawns)
			{
				this.AddPawnInternal(p, false);
			}
			if (updateDuties)
			{
				try
				{
					this.curLordToil.UpdateAllDuties();
				}
				catch (Exception ex)
				{
					string str = "Error in LordToil.UpdateAllDuties(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x060067C8 RID: 26568 RVA: 0x0020E86C File Offset: 0x0020CA6C
		public void AddPawn(Pawn p)
		{
			if (!this.CanAddPawn(p))
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to add pawn ",
					(p != null) ? p.ToString() : null,
					" to lord ",
					(this != null) ? this.ToString() : null,
					" but this pawn can't be added to this lord."
				}));
				return;
			}
			this.AddPawnInternal(p, true);
		}

		// Token: 0x060067C9 RID: 26569 RVA: 0x0020E8D4 File Offset: 0x0020CAD4
		private void AddPawnInternal(Pawn p, bool updateDuties)
		{
			if (this.ownedPawns.Contains(p))
			{
				Log.Error(string.Concat(new string[]
				{
					"Lord for ",
					this.faction.ToStringSafe<Faction>(),
					" tried to add ",
					(p != null) ? p.ToString() : null,
					" whom it already controls."
				}));
				return;
			}
			if (p.GetLord() != null)
			{
				string[] array = new string[7];
				array[0] = "Tried to add pawn ";
				array[1] = ((p != null) ? p.ToString() : null);
				array[2] = " to lord with lord job";
				int num = 3;
				LordJob lordJob = this.LordJob;
				array[num] = ((lordJob != null) ? lordJob.ToString() : null);
				array[4] = " but this pawn is already a member of lord with lord job";
				int num2 = 5;
				LordJob lordJob2 = p.GetLord().LordJob;
				array[num2] = ((lordJob2 != null) ? lordJob2.ToString() : null);
				array[6] = ". Pawns can't be members of more than one lord at the same time.";
				Log.Error(string.Concat(array));
				return;
			}
			this.ownedPawns.Add(p);
			p.lord = this;
			this.numPawnsEverGained++;
			this.Map.attackTargetsCache.UpdateTarget(p);
			if (updateDuties)
			{
				try
				{
					this.curLordToil.UpdateAllDuties();
				}
				catch (Exception ex)
				{
					string str = "Error in LordToil.UpdateAllDuties(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			try
			{
				LordJob lordJob3 = this.curJob;
				if (lordJob3 != null)
				{
					lordJob3.Notify_PawnAdded(p);
				}
			}
			catch (Exception ex3)
			{
				string str2 = "Error in LordJob.Notify_PawnAdded(): ";
				Exception ex4 = ex3;
				Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
			}
		}

		// Token: 0x060067CA RID: 26570 RVA: 0x0020EA60 File Offset: 0x0020CC60
		public void AddBuilding(Building b)
		{
			if (this.ownedBuildings.Contains(b))
			{
				Log.Error(string.Concat(new string[]
				{
					"Lord for ",
					this.faction.ToStringSafe<Faction>(),
					" tried to add ",
					(b != null) ? b.ToString() : null,
					" which it already controls."
				}));
				return;
			}
			this.ownedBuildings.Add(b);
			try
			{
				this.curLordToil.UpdateAllDuties();
			}
			catch (Exception ex)
			{
				string str = "Error in LordToil.UpdateAllDuties(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
			try
			{
				this.curJob.Notify_BuildingAdded(b);
			}
			catch (Exception ex3)
			{
				string str2 = "Error in LordJob.Notify_BuildingAdded(): ";
				Exception ex4 = ex3;
				Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
			}
		}

		// Token: 0x060067CB RID: 26571 RVA: 0x0020EB44 File Offset: 0x0020CD44
		public void AddCorpse(Corpse c)
		{
			if (this.ownedCorpses.Contains(c))
			{
				Log.Error(string.Concat(new string[]
				{
					"Lord for ",
					this.faction.ToStringSafe<Faction>(),
					" tried to add ",
					(c != null) ? c.ToString() : null,
					" which it already controls."
				}));
				return;
			}
			this.ownedCorpses.Add(c);
			try
			{
				this.curJob.Notify_CorpseAdded(c);
			}
			catch (Exception ex)
			{
				string str = "Error in LordJob.Notify_CorpseAdded(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x060067CC RID: 26572 RVA: 0x0020EBF0 File Offset: 0x0020CDF0
		public void RemovePawn(Pawn p)
		{
			this.ownedPawns.Remove(p);
			if (p.mindState != null)
			{
				p.mindState.duty = null;
			}
			if (p.lord == this)
			{
				p.lord = null;
			}
			this.Map.attackTargetsCache.UpdateTarget(p);
		}

		// Token: 0x060067CD RID: 26573 RVA: 0x0020EC40 File Offset: 0x0020CE40
		public void RemovePawns(List<Pawn> pawns)
		{
			foreach (Pawn p in pawns)
			{
				this.RemovePawn(p);
			}
		}

		// Token: 0x060067CE RID: 26574 RVA: 0x0020EC90 File Offset: 0x0020CE90
		public void RemoveAllPawns(bool interruptPawnJobs = true)
		{
			foreach (Pawn pawn in this.ownedPawns)
			{
				if (pawn.mindState != null)
				{
					pawn.mindState.duty = null;
					if (interruptPawnJobs)
					{
						Pawn_JobTracker jobs = pawn.jobs;
						Lord lord;
						if (jobs == null)
						{
							lord = null;
						}
						else
						{
							Job job = jobs.curJob;
							lord = ((job != null) ? job.lord : null);
						}
						if (lord == this)
						{
							pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, false, true);
						}
					}
				}
				this.Map.attackTargetsCache.UpdateTarget(pawn);
				if (pawn.lord == this)
				{
					pawn.lord = null;
				}
			}
			this.ownedPawns.Clear();
		}

		// Token: 0x060067CF RID: 26575 RVA: 0x0020ED50 File Offset: 0x0020CF50
		public void RemoveAllBuildings()
		{
			this.ownedBuildings.Clear();
		}

		// Token: 0x060067D0 RID: 26576 RVA: 0x0020ED60 File Offset: 0x0020CF60
		public void GotoToil(LordToil newLordToil)
		{
			LordToil previousToil = this.curLordToil;
			if (this.curLordToil != null)
			{
				try
				{
					this.curLordToil.Cleanup();
				}
				catch (Exception ex)
				{
					string str = "Error in LordToil.Cleanup(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			this.curLordToil = newLordToil;
			this.ticksInToil = 0;
			if (this.curLordToil.lord != this)
			{
				Log.Error("curLordToil lord is " + ((this.curLordToil.lord == null) ? "null (forgot to add toil to graph?)" : this.curLordToil.lord.ToString()));
				this.curLordToil.lord = this;
			}
			try
			{
				this.curLordToil.Init();
			}
			catch (Exception ex3)
			{
				string str2 = "Error in LordToil.Init(): ";
				Exception ex4 = ex3;
				Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
			}
			for (int i = 0; i < this.graph.transitions.Count; i++)
			{
				if (this.graph.transitions[i].sources.Contains(this.curLordToil))
				{
					this.graph.transitions[i].SourceToilBecameActive(this.graph.transitions[i], previousToil);
				}
			}
			try
			{
				this.curLordToil.UpdateAllDuties();
			}
			catch (Exception ex5)
			{
				string str3 = "Error in LordToil.UpdateAllDuties(): ";
				Exception ex6 = ex5;
				Log.Error(str3 + ((ex6 != null) ? ex6.ToString() : null));
			}
		}

		// Token: 0x060067D1 RID: 26577 RVA: 0x0020EEF0 File Offset: 0x0020D0F0
		public void LordTick()
		{
			if (this.ticksInToil % 60 == 0)
			{
				for (int i = 0; i < this.ownedPawns.Count; i++)
				{
					Pawn pawn = this.ownedPawns[i];
					if (Find.WorldPawns.GetSituation(pawn) == WorldPawnSituation.Free)
					{
						Log.ErrorOnce(string.Format("Lord {0} ({1}) owns a free world pawn {2}. Is there WorldPawnSituation to be defined?", this, this.curJob, pawn.LabelShort), this.loadID ^ 1357461551);
					}
				}
			}
			if (!this.initialized)
			{
				this.Init();
			}
			this.curJob.LordJobTick();
			this.curLordToil.LordToilTick();
			this.CheckTransitionOnSignal(TriggerSignal.ForTick);
			this.ticksInToil++;
		}

		// Token: 0x060067D2 RID: 26578 RVA: 0x0020EFA0 File Offset: 0x0020D1A0
		private Trigger GetTriggerByIndex(int index)
		{
			int num = 0;
			for (int i = 0; i < this.graph.transitions.Count; i++)
			{
				for (int j = 0; j < this.graph.transitions[i].triggers.Count; j++)
				{
					if (num == index)
					{
						return this.graph.transitions[i].triggers[j];
					}
					num++;
				}
			}
			return null;
		}

		// Token: 0x060067D3 RID: 26579 RVA: 0x0020F016 File Offset: 0x0020D216
		public void ReceiveMemo(string memo)
		{
			this.CheckTransitionOnSignal(TriggerSignal.ForMemo(memo));
		}

		// Token: 0x060067D4 RID: 26580 RVA: 0x0020F028 File Offset: 0x0020D228
		public void Notify_FactionRelationsChanged(Faction otherFaction, FactionRelationKind previousRelationKind)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.FactionRelationsChanged,
				faction = otherFaction,
				previousRelationKind = new FactionRelationKind?(previousRelationKind)
			});
			for (int i = 0; i < this.ownedPawns.Count; i++)
			{
				if (this.ownedPawns[i].Spawned)
				{
					this.ownedPawns[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x060067D5 RID: 26581 RVA: 0x0020F0A5 File Offset: 0x0020D2A5
		private void Destroy()
		{
			this.lordManager.RemoveLord(this);
			this.curJob.Notify_LordDestroyed();
			if (this.faction != null)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "AllEnemiesDefeated");
			}
		}

		// Token: 0x060067D6 RID: 26582 RVA: 0x0020F0D8 File Offset: 0x0020D2D8
		public void Notify_PawnJobDone(Pawn pawn, JobCondition condition)
		{
			try
			{
				this.curJob.Notify_PawnJobDone(pawn, condition);
			}
			catch (Exception ex)
			{
				string str = "Error in LordJob.Notify_PawnJobDone(): ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
			}
			try
			{
				this.curLordToil.Notify_PawnJobDone(pawn, condition);
			}
			catch (Exception ex3)
			{
				string str2 = "Error in LordToil.Notify_PawnJobDone(): ";
				Exception ex4 = ex3;
				Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
			}
		}

		// Token: 0x060067D7 RID: 26583 RVA: 0x0020F160 File Offset: 0x0020D360
		public void Notify_PawnLost(Pawn pawn, PawnLostCondition cond, DamageInfo? dinfo = null)
		{
			if (!this.curJob.ShouldRemovePawn(pawn, cond))
			{
				return;
			}
			if (this.ownedPawns.Contains(pawn))
			{
				this.RemovePawn(pawn);
				if (cond == PawnLostCondition.Incapped || cond == PawnLostCondition.Killed || cond == PawnLostCondition.MadePrisoner)
				{
					this.numPawnsLostViolently++;
				}
				try
				{
					this.curJob.Notify_PawnLost(pawn, cond);
				}
				catch (Exception ex)
				{
					string str = "Error in LordJob.Notify_PawnLost(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
				if (this.lordManager.lords.Contains(this))
				{
					if (!this.ShouldExist)
					{
						this.Destroy();
						return;
					}
					try
					{
						this.curLordToil.Notify_PawnLost(pawn, cond);
					}
					catch (Exception ex3)
					{
						string str2 = "Error in LordToil.Notify_PawnLost(): ";
						Exception ex4 = ex3;
						Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
					}
					TriggerSignal signal = default(TriggerSignal);
					signal.type = TriggerSignalType.PawnLost;
					signal.thing = pawn;
					signal.condition = cond;
					if (dinfo != null)
					{
						signal.dinfo = dinfo.Value;
					}
					this.CheckTransitionOnSignal(signal);
				}
				return;
			}
			Log.Error("Lord lost pawn " + ((pawn != null) ? pawn.ToString() : null) + " it didn't have. Condition=" + cond.ToString());
		}

		// Token: 0x060067D8 RID: 26584 RVA: 0x0020F2B8 File Offset: 0x0020D4B8
		public void Notify_InMentalState(Pawn pawn, MentalStateDef def)
		{
			this.curJob.Notify_InMentalState(pawn, def);
		}

		// Token: 0x060067D9 RID: 26585 RVA: 0x0020F2C8 File Offset: 0x0020D4C8
		public void Notify_BuildingLost(Building building, DamageInfo? dinfo = null)
		{
			if (this.ownedBuildings.Contains(building))
			{
				this.ownedBuildings.Remove(building);
				this.curJob.Notify_BuildingLost(building);
				if (this.lordManager.lords.Contains(this))
				{
					if (!this.ShouldExist)
					{
						this.Destroy();
						return;
					}
					this.curLordToil.Notify_BuildingLost(building);
					TriggerSignal signal = default(TriggerSignal);
					signal.type = TriggerSignalType.BuildingLost;
					signal.thing = building;
					if (dinfo != null)
					{
						signal.dinfo = dinfo.Value;
					}
					this.CheckTransitionOnSignal(signal);
				}
				return;
			}
			Log.Error("Lord lost building " + ((building != null) ? building.ToString() : null) + " it didn't have.");
		}

		// Token: 0x060067DA RID: 26586 RVA: 0x0020F388 File Offset: 0x0020D588
		public void Notify_CorpseLost(Corpse corpse, DamageInfo? dinfo = null)
		{
			if (this.ownedCorpses.Contains(corpse))
			{
				this.ownedCorpses.Remove(corpse);
				this.curJob.Notify_CorpseLost(corpse);
				if (this.lordManager.lords.Contains(this))
				{
					if (!this.ShouldExist)
					{
						this.Destroy();
						return;
					}
					this.curLordToil.Notify_CorpseLost(corpse);
					TriggerSignal signal = default(TriggerSignal);
					signal.type = TriggerSignalType.CorpseLost;
					signal.thing = corpse;
					if (dinfo != null)
					{
						signal.dinfo = dinfo.Value;
					}
					this.CheckTransitionOnSignal(signal);
				}
				return;
			}
			Log.Error("Lord lost corpse " + ((corpse != null) ? corpse.ToString() : null) + " it didn't have.");
		}

		// Token: 0x060067DB RID: 26587 RVA: 0x0020F448 File Offset: 0x0020D648
		public void Notify_BuildingDamaged(Building building, DamageInfo dinfo)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.BuildingDamaged,
				thing = building,
				dinfo = dinfo
			});
		}

		// Token: 0x060067DC RID: 26588 RVA: 0x0020F480 File Offset: 0x0020D680
		public void Notify_PawnDamaged(Pawn victim, DamageInfo dinfo)
		{
			LordToil lordToil = this.curLordToil;
			if (lordToil != null)
			{
				lordToil.Notify_PawnDamaged(victim, dinfo);
			}
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.PawnDamaged,
				thing = victim,
				dinfo = dinfo
			});
		}

		// Token: 0x060067DD RID: 26589 RVA: 0x0020F4C8 File Offset: 0x0020D6C8
		public void Notify_PawnAttemptArrested(Pawn victim)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.PawnArrestAttempted,
				thing = victim
			});
		}

		// Token: 0x060067DE RID: 26590 RVA: 0x0020F4F8 File Offset: 0x0020D6F8
		public void Notify_Clamor(Thing source, ClamorDef clamorType)
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.Clamor,
				thing = source,
				clamorType = clamorType
			});
		}

		// Token: 0x060067DF RID: 26591 RVA: 0x0020F530 File Offset: 0x0020D730
		public void Notify_PawnAcquiredTarget(Pawn detector, Thing newTarg)
		{
			LordToil lordToil = this.curLordToil;
			if (lordToil != null)
			{
				lordToil.Notify_PawnAcquiredTarget(detector, newTarg);
			}
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.AcquiredTarget,
				thing = detector,
				otherThing = newTarg
			});
		}

		// Token: 0x060067E0 RID: 26592 RVA: 0x0020F579 File Offset: 0x0020D779
		public void Notify_BuildingSpawnedOnMap(Building b)
		{
			LordToil lordToil = this.curLordToil;
			if (lordToil == null)
			{
				return;
			}
			lordToil.Notify_BuildingSpawnedOnMap(b);
		}

		// Token: 0x060067E1 RID: 26593 RVA: 0x0020F58C File Offset: 0x0020D78C
		public void Notify_BuildingDespawnedOnMap(Building b)
		{
			LordToil lordToil = this.curLordToil;
			if (lordToil == null)
			{
				return;
			}
			lordToil.Notify_BuildingDespawnedOnMap(b);
		}

		// Token: 0x060067E2 RID: 26594 RVA: 0x0020F59F File Offset: 0x0020D79F
		public void Notify_ReachedDutyLocation(Pawn pawn)
		{
			this.curLordToil.Notify_ReachedDutyLocation(pawn);
		}

		// Token: 0x060067E3 RID: 26595 RVA: 0x0020F5AD File Offset: 0x0020D7AD
		public void Notify_MapRemoved()
		{
			LordJob lordJob = this.curJob;
			if (lordJob == null)
			{
				return;
			}
			lordJob.Notify_MapRemoved();
		}

		// Token: 0x060067E4 RID: 26596 RVA: 0x0020F5BF File Offset: 0x0020D7BF
		public void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
			this.curLordToil.Notify_ConstructionFailed(pawn, frame, newBlueprint);
		}

		// Token: 0x060067E5 RID: 26597 RVA: 0x0020F5CF File Offset: 0x0020D7CF
		public void Notify_ConstructionCompleted(Pawn pawn, Building building)
		{
			this.curLordToil.Notify_ConstructionCompleted(pawn, building);
		}

		// Token: 0x060067E6 RID: 26598 RVA: 0x0020F5E0 File Offset: 0x0020D7E0
		public void Notify_SignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignalLeave)
			{
				if (this.ownedPawns.Any<Pawn>() && this.faction != null)
				{
					Messages.Message("MessagePawnsLeaving".Translate(this.faction.def.pawnsPlural), this.ownedPawns, MessageTypeDefOf.NeutralEvent, true);
				}
				LordToil lordToil = this.Graph.lordToils.Find((LordToil st) => st is LordToil_PanicFlee);
				if (lordToil != null)
				{
					this.GotoToil(lordToil);
				}
				else
				{
					this.lordManager.RemoveLord(this);
				}
			}
			this.CheckTransitionOnSignal(TriggerSignal.ForSignal(signal));
		}

		// Token: 0x060067E7 RID: 26599 RVA: 0x0020F6A8 File Offset: 0x0020D8A8
		public void Notify_DormancyWakeup()
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.DormancyWakeup
			});
		}

		// Token: 0x060067E8 RID: 26600 RVA: 0x0020F6D0 File Offset: 0x0020D8D0
		public void Notify_MechClusterDefeated()
		{
			this.CheckTransitionOnSignal(new TriggerSignal
			{
				type = TriggerSignalType.MechClusterDefeated
			});
		}

		// Token: 0x060067E9 RID: 26601 RVA: 0x0020F6F6 File Offset: 0x0020D8F6
		public void Notify_PawnUndowned(Pawn pawn)
		{
			this.curJob.Notify_PawnUndowned(pawn);
		}

		// Token: 0x060067EA RID: 26602 RVA: 0x0020F704 File Offset: 0x0020D904
		public void Notify_PawnDowned(Pawn pawn)
		{
			this.curJob.Notify_PawnDowned(pawn);
		}

		// Token: 0x060067EB RID: 26603 RVA: 0x0020F714 File Offset: 0x0020D914
		private bool CheckTransitionOnSignal(TriggerSignal signal)
		{
			if (Trigger_PawnHarmed.SignalIsHarm(signal))
			{
				this.lastPawnHarmTick = Find.TickManager.TicksGame;
			}
			for (int i = 0; i < this.graph.transitions.Count; i++)
			{
				if (this.graph.transitions[i].sources.Contains(this.curLordToil) && this.graph.transitions[i].CheckSignal(this, signal))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060067EC RID: 26604 RVA: 0x0020F794 File Offset: 0x0020D994
		private Vector3 DebugCenter()
		{
			Vector3 result = this.Map.Center.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			if (this.ownedPawns.Any((Pawn p) => p.Spawned))
			{
				result.x = (from p in this.ownedPawns
				where p.Spawned
				select p).Average((Pawn p) => p.DrawPos.x);
				result.z = (from p in this.ownedPawns
				where p.Spawned
				select p).Average((Pawn p) => p.DrawPos.z);
			}
			return result;
		}

		// Token: 0x060067ED RID: 26605 RVA: 0x0020F894 File Offset: 0x0020DA94
		public void DebugDraw()
		{
			Vector3 a = this.DebugCenter();
			IntVec3 flagLoc = this.curLordToil.FlagLoc;
			if (flagLoc.IsValid)
			{
				Graphics.DrawMesh(MeshPool.plane14, flagLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Building), Quaternion.identity, Lord.FlagTex, 0);
			}
			GenDraw.DrawLineBetween(a, flagLoc.ToVector3Shifted(), SimpleColor.Red, 0.2f);
			foreach (Pawn pawn in this.ownedPawns)
			{
				SimpleColor color = (!pawn.InMentalState) ? SimpleColor.White : SimpleColor.Yellow;
				GenDraw.DrawLineBetween(a, pawn.DrawPos, color, 0.2f);
			}
		}

		// Token: 0x060067EE RID: 26606 RVA: 0x0020F950 File Offset: 0x0020DB50
		public void DebugOnGUI()
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Tiny;
			string label;
			if (this.CurLordToil != null)
			{
				string str = "toil ";
				string str2 = this.graph.lordToils.IndexOf(this.CurLordToil).ToString();
				string str3 = "\n";
				LordToil lordToil = this.CurLordToil;
				label = str + str2 + str3 + ((lordToil != null) ? lordToil.ToString() : null);
			}
			else
			{
				label = "toil=NULL";
			}
			Vector2 vector = this.DebugCenter().MapToUIPosition();
			Widgets.Label(new Rect(vector.x - 100f, vector.y - 100f, 200f, 200f), label);
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060067EF RID: 26607 RVA: 0x0020F9F8 File Offset: 0x0020DBF8
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Start steal threshold: " + StealAIUtility.StartStealingMarketValueThreshold(this).ToString("F0"));
			stringBuilder.AppendLine("Duties:");
			foreach (Pawn pawn in this.ownedPawns)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				string str = "   ";
				string labelCap = pawn.LabelCap;
				string str2 = " - ";
				PawnDuty duty = pawn.mindState.duty;
				stringBuilder2.AppendLine(str + labelCap + str2 + ((duty != null) ? duty.ToString() : null));
			}
			stringBuilder.AppendLine("Raw save data:");
			stringBuilder.AppendLine(Scribe.saver.DebugOutputFor(this));
			return stringBuilder.ToString();
		}

		// Token: 0x060067F0 RID: 26608 RVA: 0x0020FAD4 File Offset: 0x0020DCD4
		private bool ShouldDoDebugOutput()
		{
			IntVec3 a = UI.MouseCell();
			IntVec3 flagLoc = this.curLordToil.FlagLoc;
			if (flagLoc.IsValid && a == flagLoc)
			{
				return true;
			}
			for (int i = 0; i < this.ownedPawns.Count; i++)
			{
				if (a == this.ownedPawns[i].Position)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060067F1 RID: 26609 RVA: 0x0020FB39 File Offset: 0x0020DD39
		public void Dispose()
		{
			StateGraph stateGraph = this.graph;
			if (stateGraph != null)
			{
				stateGraph.Dispose();
			}
			LordJob lordJob = this.curJob;
			if (lordJob == null)
			{
				return;
			}
			lordJob.Dispose();
		}

		// Token: 0x040049F1 RID: 18929
		public LordManager lordManager;

		// Token: 0x040049F2 RID: 18930
		private LordToil curLordToil;

		// Token: 0x040049F3 RID: 18931
		private StateGraph graph;

		// Token: 0x040049F4 RID: 18932
		public int loadID = -1;

		// Token: 0x040049F5 RID: 18933
		private LordJob curJob;

		// Token: 0x040049F6 RID: 18934
		public Faction faction;

		// Token: 0x040049F7 RID: 18935
		public List<Pawn> ownedPawns = new List<Pawn>();

		// Token: 0x040049F8 RID: 18936
		public List<Building> ownedBuildings = new List<Building>();

		// Token: 0x040049F9 RID: 18937
		public List<Corpse> ownedCorpses = new List<Corpse>();

		// Token: 0x040049FA RID: 18938
		public List<Thing> extraForbiddenThings = new List<Thing>();

		// Token: 0x040049FB RID: 18939
		public List<string> questTags;

		// Token: 0x040049FC RID: 18940
		public string inSignalLeave;

		// Token: 0x040049FD RID: 18941
		private bool initialized;

		// Token: 0x040049FE RID: 18942
		public int ticksInToil;

		// Token: 0x040049FF RID: 18943
		public int numPawnsLostViolently;

		// Token: 0x04004A00 RID: 18944
		public int numPawnsEverGained;

		// Token: 0x04004A01 RID: 18945
		public int initialColonyHealthTotal;

		// Token: 0x04004A02 RID: 18946
		public int lastPawnHarmTick = -99999;

		// Token: 0x04004A03 RID: 18947
		private static readonly Material FlagTex = MaterialPool.MatFrom("UI/Overlays/SquadFlag");

		// Token: 0x04004A04 RID: 18948
		private int tmpCurLordToilIdx = -1;

		// Token: 0x04004A05 RID: 18949
		private Dictionary<int, LordToilData> tmpLordToilData = new Dictionary<int, LordToilData>();

		// Token: 0x04004A06 RID: 18950
		private Dictionary<int, TriggerData> tmpTriggerData = new Dictionary<int, TriggerData>();
	}
}
