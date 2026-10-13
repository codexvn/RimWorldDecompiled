using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002461 RID: 9313
	public class MechanitorControlGroup : IExposable
	{
		// Token: 0x17001E7C RID: 7804
		// (get) Token: 0x0600C9DC RID: 51676 RVA: 0x003B16A0 File Offset: 0x003AF8A0
		public List<Pawn> MechsForReading
		{
			get
			{
				if (this.mechsForReading == null)
				{
					this.mechsForReading = new List<Pawn>();
				}
				this.mechsForReading.Clear();
				for (int i = 0; i < this.assignedMechs.Count; i++)
				{
					if (!this.assignedMechs[i].pawn.IsGestating())
					{
						this.mechsForReading.Add(this.assignedMechs[i].pawn);
					}
				}
				return this.mechsForReading;
			}
		}

		// Token: 0x17001E7D RID: 7805
		// (get) Token: 0x0600C9DD RID: 51677 RVA: 0x003B171B File Offset: 0x003AF91B
		public List<AssignedMech> AssignedMechs
		{
			get
			{
				return this.assignedMechs;
			}
		}

		// Token: 0x17001E7E RID: 7806
		// (get) Token: 0x0600C9DE RID: 51678 RVA: 0x003B1723 File Offset: 0x003AF923
		public Pawn_MechanitorTracker Tracker
		{
			get
			{
				return this.tracker;
			}
		}

		// Token: 0x17001E7F RID: 7807
		// (get) Token: 0x0600C9DF RID: 51679 RVA: 0x003B172B File Offset: 0x003AF92B
		public MechWorkModeDef WorkMode
		{
			get
			{
				return this.workMode;
			}
		}

		// Token: 0x17001E80 RID: 7808
		// (get) Token: 0x0600C9E0 RID: 51680 RVA: 0x003B1733 File Offset: 0x003AF933
		public GlobalTargetInfo Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x17001E81 RID: 7809
		// (get) Token: 0x0600C9E1 RID: 51681 RVA: 0x003B173B File Offset: 0x003AF93B
		public int Index
		{
			get
			{
				return this.Tracker.controlGroups.IndexOf(this) + 1;
			}
		}

		// Token: 0x17001E82 RID: 7810
		// (get) Token: 0x0600C9E2 RID: 51682 RVA: 0x003B1750 File Offset: 0x003AF950
		public string LabelIndexWithWorkMode
		{
			get
			{
				return this.Index.ToString() + " (" + this.WorkMode.LabelCap.ToString() + ")";
			}
		}

		// Token: 0x0600C9E3 RID: 51683 RVA: 0x003B1793 File Offset: 0x003AF993
		public MechanitorControlGroup(Pawn_MechanitorTracker tracker)
		{
			this.tracker = tracker;
			this.workMode = MechWorkModeDefOf.Work;
		}

		// Token: 0x0600C9E4 RID: 51684 RVA: 0x003B17D0 File Offset: 0x003AF9D0
		public bool TryUnassign(Pawn pawn)
		{
			return this.assignedMechs.RemoveAll((AssignedMech x) => x.pawn == pawn) > 0;
		}

		// Token: 0x0600C9E5 RID: 51685 RVA: 0x003B1804 File Offset: 0x003AFA04
		public void Assign(Pawn pawn)
		{
			foreach (MechanitorControlGroup mechanitorControlGroup in this.tracker.controlGroups)
			{
				mechanitorControlGroup.TryUnassign(pawn);
			}
			this.assignedMechs.Add(new AssignedMech(pawn));
			this.SetWorkModeForPawn(pawn, this.workMode);
		}

		// Token: 0x0600C9E6 RID: 51686 RVA: 0x003B187C File Offset: 0x003AFA7C
		public void SetWorkMode(MechWorkModeDef workMode)
		{
			this.SetWorkMode(workMode, GlobalTargetInfo.Invalid);
		}

		// Token: 0x0600C9E7 RID: 51687 RVA: 0x003B188C File Offset: 0x003AFA8C
		public void SetWorkMode(MechWorkModeDef workMode, GlobalTargetInfo target)
		{
			if (this.workMode == workMode && this.target == target)
			{
				return;
			}
			this.workMode = workMode;
			this.target = target;
			foreach (AssignedMech assignedMech in this.assignedMechs)
			{
				this.SetWorkModeForPawn(assignedMech.pawn, workMode);
			}
			Dictionary<Pawn, string> dictionary = this.tags;
			if (dictionary != null)
			{
				dictionary.Clear();
			}
			if (workMode == MechWorkModeDefOf.Escort)
			{
				Pawn pawn = this.Tracker.Pawn;
				if (pawn.IsFormingCaravan())
				{
					Lord formAndSendCaravanLord = CaravanFormingUtility.GetFormAndSendCaravanLord(pawn);
					List<Pawn> list = Dialog_FormCaravan.AllSendablePawns(pawn.Map, false);
					for (int i = 0; i < this.assignedMechs.Count; i++)
					{
						Pawn pawn2 = this.assignedMechs[i].pawn;
						if (list.Contains(pawn2) && CaravanFormingUtility.GetFormAndSendCaravanLord(pawn2) != formAndSendCaravanLord)
						{
							Messages.Message("MessageCaravanAddingEscortingMech".Translate(pawn2.Named("MECH"), pawn.Named("OVERSEER")), pawn2, MessageTypeDefOf.RejectInput, false);
							CaravanFormingUtility.LateJoinFormingCaravan(pawn2, formAndSendCaravanLord);
						}
					}
				}
			}
		}

		// Token: 0x0600C9E8 RID: 51688 RVA: 0x003B19D8 File Offset: 0x003AFBD8
		private void SetWorkModeForPawn(Pawn pawn, MechWorkModeDef workMode)
		{
			PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, true);
			if (workMode != MechWorkModeDefOf.Recharge && pawn.CurJobDef == JobDefOf.MechCharge && pawn.IsCharging())
			{
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			CompCanBeDormant compCanBeDormant = pawn.TryGetComp<CompCanBeDormant>();
			if (compCanBeDormant != null)
			{
				compCanBeDormant.WakeUp();
			}
			Pawn_JobTracker jobs = pawn.jobs;
			if (jobs == null)
			{
				return;
			}
			jobs.CheckForJobOverride(0f, true);
		}

		// Token: 0x0600C9E9 RID: 51689 RVA: 0x003B1A3F File Offset: 0x003AFC3F
		public void SetTag(Pawn pawn, string tag)
		{
			if (this.tags == null)
			{
				this.tags = new Dictionary<Pawn, string>();
			}
			this.tags.Add(pawn, tag);
		}

		// Token: 0x0600C9EA RID: 51690 RVA: 0x003B1A64 File Offset: 0x003AFC64
		public string GetTag(Pawn pawn)
		{
			string result;
			if (this.tags == null || !this.tags.TryGetValue(pawn, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600C9EB RID: 51691 RVA: 0x003B1A8C File Offset: 0x003AFC8C
		public void ExposeData()
		{
			Scribe_Collections.Look<AssignedMech>(ref this.assignedMechs, "assignedMechs", LookMode.Deep, Array.Empty<object>());
			Scribe_Defs.Look<MechWorkModeDef>(ref this.workMode, "workMode");
			Scribe_TargetInfo.Look(ref this.target, "target");
			Scribe_Collections.Look<Pawn, string>(ref this.tags, "tags", LookMode.Reference, LookMode.Value, ref this.tmpTagPawns, ref this.tmpTags, true, false, false);
			Scribe_Values.Look<FloatRange>(ref this.mechRechargeThresholds, "mechRechargeThresholds", MechanitorControlGroup.DefaultMechRechargeThresholds, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.assignedMechs.RemoveAll((AssignedMech x) => x.pawn == null);
			}
		}

		// Token: 0x04008970 RID: 35184
		public static readonly FloatRange DefaultMechRechargeThresholds = new FloatRange(0.05f, 1f);

		// Token: 0x04008971 RID: 35185
		private List<AssignedMech> assignedMechs = new List<AssignedMech>();

		// Token: 0x04008972 RID: 35186
		private Pawn_MechanitorTracker tracker;

		// Token: 0x04008973 RID: 35187
		private MechWorkModeDef workMode;

		// Token: 0x04008974 RID: 35188
		private GlobalTargetInfo target = GlobalTargetInfo.Invalid;

		// Token: 0x04008975 RID: 35189
		private Dictionary<Pawn, string> tags;

		// Token: 0x04008976 RID: 35190
		public FloatRange mechRechargeThresholds = MechanitorControlGroup.DefaultMechRechargeThresholds;

		// Token: 0x04008977 RID: 35191
		private List<Pawn> tmpTagPawns;

		// Token: 0x04008978 RID: 35192
		private List<string> tmpTags;

		// Token: 0x04008979 RID: 35193
		private List<Pawn> mechsForReading;
	}
}
