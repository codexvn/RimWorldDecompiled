using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200176B RID: 5995
	public class Pawn_WorkSettings : IExposable
	{
		// Token: 0x1700159B RID: 5531
		// (get) Token: 0x06008C9A RID: 35994 RVA: 0x0029D031 File Offset: 0x0029B231
		public bool EverWork
		{
			get
			{
				return this.priorities != null;
			}
		}

		// Token: 0x1700159C RID: 5532
		// (get) Token: 0x06008C9B RID: 35995 RVA: 0x0029D03C File Offset: 0x0029B23C
		public List<WorkGiver> WorkGiversInOrderNormal
		{
			get
			{
				if (this.workGiversDirty)
				{
					this.CacheWorkGiversInOrder();
				}
				return this.workGiversInOrderNormal;
			}
		}

		// Token: 0x1700159D RID: 5533
		// (get) Token: 0x06008C9C RID: 35996 RVA: 0x0029D052 File Offset: 0x0029B252
		public List<WorkGiver> WorkGiversInOrderEmergency
		{
			get
			{
				if (this.workGiversDirty)
				{
					this.CacheWorkGiversInOrder();
				}
				return this.workGiversInOrderEmerg;
			}
		}

		// Token: 0x1700159E RID: 5534
		// (get) Token: 0x06008C9D RID: 35997 RVA: 0x0029D068 File Offset: 0x0029B268
		public bool LimitInitialActiveWorks
		{
			get
			{
				return !this.pawn.RaceProps.IsMechanoid;
			}
		}

		// Token: 0x1700159F RID: 5535
		// (get) Token: 0x06008C9E RID: 35998 RVA: 0x0029D031 File Offset: 0x0029B231
		public bool Initialized
		{
			get
			{
				return this.priorities != null;
			}
		}

		// Token: 0x06008C9F RID: 35999 RVA: 0x0029D07D File Offset: 0x0029B27D
		public Pawn_WorkSettings()
		{
		}

		// Token: 0x06008CA0 RID: 36000 RVA: 0x0029D0A2 File Offset: 0x0029B2A2
		public Pawn_WorkSettings(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06008CA1 RID: 36001 RVA: 0x0029D0D0 File Offset: 0x0029B2D0
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<WorkTypeDef, int>>(ref this.priorities, "priorities", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.priorities != null)
			{
				this.pawn.Notify_DisabledWorkTypesChanged();
				List<WorkTypeDef> disabledWorkTypes = this.pawn.GetDisabledWorkTypes(false);
				for (int i = 0; i < disabledWorkTypes.Count; i++)
				{
					this.Disable(disabledWorkTypes[i]);
				}
			}
		}

		// Token: 0x06008CA2 RID: 36002 RVA: 0x0029D138 File Offset: 0x0029B338
		public void EnableAndInitializeIfNotAlreadyInitialized()
		{
			if (this.priorities == null)
			{
				this.EnableAndInitialize();
			}
		}

		// Token: 0x06008CA3 RID: 36003 RVA: 0x0029D148 File Offset: 0x0029B348
		public void EnableAndInitialize()
		{
			if (this.priorities == null)
			{
				this.priorities = new DefMap<WorkTypeDef, int>();
			}
			this.priorities.SetAll(0);
			int num = 0;
			foreach (WorkTypeDef w3 in (from w in DefDatabase<WorkTypeDef>.AllDefs
			where !w.alwaysStartActive && !this.pawn.WorkTypeIsDisabled(w)
			select w).OrderByDescending(delegate(WorkTypeDef w)
			{
				Pawn_SkillTracker skills = this.pawn.skills;
				if (skills == null)
				{
					return 1f;
				}
				return skills.AverageOfRelevantSkillsFor(w);
			}))
			{
				this.SetPriority(w3, 3);
				num++;
				if (this.LimitInitialActiveWorks && num >= 6)
				{
					break;
				}
			}
			foreach (WorkTypeDef w2 in from w in DefDatabase<WorkTypeDef>.AllDefs
			where w.alwaysStartActive
			select w)
			{
				if (!this.pawn.WorkTypeIsDisabled(w2))
				{
					this.SetPriority(w2, 3);
				}
			}
			if (ModsConfig.BiotechActive && this.pawn.RaceProps.IsMechanoid && !this.pawn.RaceProps.mechWorkTypePriorities.NullOrEmpty<MechWorkTypePriority>())
			{
				for (int i = 0; i < this.pawn.RaceProps.mechWorkTypePriorities.Count; i++)
				{
					MechWorkTypePriority mechWorkTypePriority = this.pawn.RaceProps.mechWorkTypePriorities[i];
					this.SetPriority(mechWorkTypePriority.def, mechWorkTypePriority.priority);
				}
			}
			List<WorkTypeDef> disabledWorkTypes = this.pawn.GetDisabledWorkTypes(false);
			for (int j = 0; j < disabledWorkTypes.Count; j++)
			{
				this.Disable(disabledWorkTypes[j]);
			}
		}

		// Token: 0x06008CA4 RID: 36004 RVA: 0x0029D30C File Offset: 0x0029B50C
		private void ConfirmInitializedDebug()
		{
			if (this.priorities == null)
			{
				Pawn pawn = this.pawn;
				Log.Error(((pawn != null) ? pawn.ToString() : null) + " did not have work settings initialized.");
				this.EnableAndInitialize();
			}
		}

		// Token: 0x06008CA5 RID: 36005 RVA: 0x0029D340 File Offset: 0x0029B540
		public void SetPriority(WorkTypeDef w, int priority)
		{
			this.ConfirmInitializedDebug();
			if (priority != 0 && this.pawn.WorkTypeIsDisabled(w))
			{
				string str = "Tried to change priority on disabled worktype ";
				string str2 = (w != null) ? w.ToString() : null;
				string str3 = " for pawn ";
				Pawn pawn = this.pawn;
				Log.Error(str + str2 + str3 + ((pawn != null) ? pawn.ToString() : null));
				return;
			}
			if (priority < 0 || priority > 4)
			{
				Log.Message("Trying to set work to invalid priority " + priority.ToString());
			}
			this.priorities[w] = priority;
			this.workGiversDirty = true;
			if (priority == 0 && this.pawn.jobs != null)
			{
				this.pawn.jobs.Notify_WorkTypeDisabled(w);
			}
		}

		// Token: 0x06008CA6 RID: 36006 RVA: 0x0029D3EC File Offset: 0x0029B5EC
		public int GetPriority(WorkTypeDef w)
		{
			this.ConfirmInitializedDebug();
			int num = this.priorities[w];
			if (this.pawn.RaceProps.Humanlike && num > 0 && !Find.PlaySettings.useWorkPriorities)
			{
				return 3;
			}
			return num;
		}

		// Token: 0x06008CA7 RID: 36007 RVA: 0x0029D431 File Offset: 0x0029B631
		public bool WorkIsActive(WorkTypeDef w)
		{
			this.ConfirmInitializedDebug();
			return this.GetPriority(w) > 0;
		}

		// Token: 0x06008CA8 RID: 36008 RVA: 0x0029D443 File Offset: 0x0029B643
		public void Disable(WorkTypeDef w)
		{
			this.ConfirmInitializedDebug();
			this.SetPriority(w, 0);
		}

		// Token: 0x06008CA9 RID: 36009 RVA: 0x0029D453 File Offset: 0x0029B653
		public void DisableAll()
		{
			this.ConfirmInitializedDebug();
			this.priorities.SetAll(0);
			this.workGiversDirty = true;
		}

		// Token: 0x06008CAA RID: 36010 RVA: 0x0029D46E File Offset: 0x0029B66E
		public void Notify_UseWorkPrioritiesChanged()
		{
			this.workGiversDirty = true;
		}

		// Token: 0x06008CAB RID: 36011 RVA: 0x0029D478 File Offset: 0x0029B678
		public void Notify_DisabledWorkTypesChanged()
		{
			if (this.priorities == null)
			{
				return;
			}
			List<WorkTypeDef> disabledWorkTypes = this.pawn.GetDisabledWorkTypes(false);
			for (int i = 0; i < disabledWorkTypes.Count; i++)
			{
				this.Disable(disabledWorkTypes[i]);
			}
		}

		// Token: 0x06008CAC RID: 36012 RVA: 0x0029D4BC File Offset: 0x0029B6BC
		private void CacheWorkGiversInOrder()
		{
			Pawn_WorkSettings.wtsByPrio.Clear();
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			int num = 999;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				int priority = this.GetPriority(workTypeDef);
				if (priority > 0)
				{
					if (priority < num)
					{
						if (workTypeDef.workGiversByPriority.Any((WorkGiverDef wg) => !wg.emergency))
						{
							num = priority;
						}
					}
					Pawn_WorkSettings.wtsByPrio.Add(workTypeDef);
				}
			}
			Pawn_WorkSettings.wtsByPrio.InsertionSort(delegate(WorkTypeDef a, WorkTypeDef b)
			{
				float value = (float)(a.naturalPriority + (4 - this.GetPriority(a)) * 100000);
				return ((float)(b.naturalPriority + (4 - this.GetPriority(b)) * 100000)).CompareTo(value);
			});
			this.workGiversInOrderEmerg.Clear();
			for (int j = 0; j < Pawn_WorkSettings.wtsByPrio.Count; j++)
			{
				WorkTypeDef workTypeDef2 = Pawn_WorkSettings.wtsByPrio[j];
				for (int k = 0; k < workTypeDef2.workGiversByPriority.Count; k++)
				{
					WorkGiver worker = workTypeDef2.workGiversByPriority[k].Worker;
					if (worker.def.emergency && this.GetPriority(worker.def.workType) <= num)
					{
						this.workGiversInOrderEmerg.Add(worker);
					}
				}
			}
			this.workGiversInOrderNormal.Clear();
			for (int l = 0; l < Pawn_WorkSettings.wtsByPrio.Count; l++)
			{
				WorkTypeDef workTypeDef3 = Pawn_WorkSettings.wtsByPrio[l];
				for (int m = 0; m < workTypeDef3.workGiversByPriority.Count; m++)
				{
					WorkGiver worker2 = workTypeDef3.workGiversByPriority[m].Worker;
					if (!worker2.def.emergency || this.GetPriority(worker2.def.workType) > num)
					{
						this.workGiversInOrderNormal.Add(worker2);
					}
				}
			}
			this.workGiversDirty = false;
		}

		// Token: 0x06008CAD RID: 36013 RVA: 0x0029D68C File Offset: 0x0029B88C
		public string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = stringBuilder;
			string str = "WorkSettings for ";
			Pawn pawn = this.pawn;
			stringBuilder2.AppendLine(str + ((pawn != null) ? pawn.ToString() : null));
			stringBuilder.AppendLine("Cached emergency WorkGivers in order:");
			for (int i = 0; i < this.WorkGiversInOrderEmergency.Count; i++)
			{
				stringBuilder.AppendLine("   " + i.ToString() + ": " + this.DebugStringFor(this.WorkGiversInOrderEmergency[i].def));
			}
			stringBuilder.AppendLine("Cached normal WorkGivers in order:");
			for (int j = 0; j < this.WorkGiversInOrderNormal.Count; j++)
			{
				stringBuilder.AppendLine("   " + j.ToString() + ": " + this.DebugStringFor(this.WorkGiversInOrderNormal[j].def));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06008CAE RID: 36014 RVA: 0x0029D774 File Offset: 0x0029B974
		private string DebugStringFor(WorkGiverDef wg)
		{
			return string.Concat(new string[]
			{
				"[",
				this.GetPriority(wg.workType).ToString(),
				" ",
				wg.workType.defName,
				"] - ",
				wg.defName,
				" (",
				wg.priorityInType.ToString(),
				")"
			});
		}

		// Token: 0x04005B64 RID: 23396
		private Pawn pawn;

		// Token: 0x04005B65 RID: 23397
		private DefMap<WorkTypeDef, int> priorities;

		// Token: 0x04005B66 RID: 23398
		private bool workGiversDirty = true;

		// Token: 0x04005B67 RID: 23399
		private List<WorkGiver> workGiversInOrderEmerg = new List<WorkGiver>();

		// Token: 0x04005B68 RID: 23400
		private List<WorkGiver> workGiversInOrderNormal = new List<WorkGiver>();

		// Token: 0x04005B69 RID: 23401
		public const int LowestPriority = 4;

		// Token: 0x04005B6A RID: 23402
		public const int DefaultPriority = 3;

		// Token: 0x04005B6B RID: 23403
		private const int MaxInitialActiveWorks = 6;

		// Token: 0x04005B6C RID: 23404
		private static List<WorkTypeDef> wtsByPrio = new List<WorkTypeDef>();
	}
}
