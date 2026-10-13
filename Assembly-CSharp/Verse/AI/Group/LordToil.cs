using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x0200109B RID: 4251
	public abstract class LordToil : IDisposable
	{
		// Token: 0x1700101D RID: 4125
		// (get) Token: 0x06006862 RID: 26722 RVA: 0x00210A9C File Offset: 0x0020EC9C
		public Map Map
		{
			get
			{
				return this.lord.lordManager.map;
			}
		}

		// Token: 0x1700101E RID: 4126
		// (get) Token: 0x06006863 RID: 26723 RVA: 0x00210AAE File Offset: 0x0020ECAE
		public virtual IntVec3 FlagLoc
		{
			get
			{
				return IntVec3.Invalid;
			}
		}

		// Token: 0x1700101F RID: 4127
		// (get) Token: 0x06006864 RID: 26724 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool AllowSatisfyLongNeeds
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001020 RID: 4128
		// (get) Token: 0x06006865 RID: 26725 RVA: 0x00210AB8 File Offset: 0x0020ECB8
		public virtual float? CustomWakeThreshold
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001021 RID: 4129
		// (get) Token: 0x06006866 RID: 26726 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool AllowRestingInBed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001022 RID: 4130
		// (get) Token: 0x06006867 RID: 26727 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool AllowAggressiveTargetingOfRoamers
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001023 RID: 4131
		// (get) Token: 0x06006868 RID: 26728 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool AllowSelfTend
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001024 RID: 4132
		// (get) Token: 0x06006869 RID: 26729 RVA: 0x00210AD0 File Offset: 0x0020ECD0
		public virtual bool ShouldFail
		{
			get
			{
				for (int i = 0; i < this.failConditions.Count; i++)
				{
					if (this.failConditions[i]())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001025 RID: 4133
		// (get) Token: 0x0600686A RID: 26730 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ForceHighStoryDanger
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001026 RID: 4134
		// (get) Token: 0x0600686B RID: 26731 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool AssignsDuties
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600686C RID: 26732 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Init()
		{
		}

		// Token: 0x0600686D RID: 26733
		public abstract void UpdateAllDuties();

		// Token: 0x0600686E RID: 26734 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void LordToilTick()
		{
		}

		// Token: 0x0600686F RID: 26735 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Cleanup()
		{
		}

		// Token: 0x06006870 RID: 26736 RVA: 0x00002501 File Offset: 0x00000701
		public virtual ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return ThinkTreeDutyHook.None;
		}

		// Token: 0x06006871 RID: 26737 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DrawPawnGUIOverlay(Pawn pawn)
		{
		}

		// Token: 0x06006872 RID: 26738 RVA: 0x000FB0F4 File Offset: 0x000F92F4
		public virtual IEnumerable<FloatMenuOption> ExtraFloatMenuOptions(Pawn target, Pawn forPawn)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}

		// Token: 0x06006873 RID: 26739 RVA: 0x000B7861 File Offset: 0x000B5A61
		public virtual IEnumerable<Gizmo> GetPawnGizmos(Pawn p)
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x06006874 RID: 26740 RVA: 0x000B7861 File Offset: 0x000B5A61
		public virtual IEnumerable<Gizmo> GetBuildingGizmos(Building building)
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x06006875 RID: 26741 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanAddPawn(Pawn p)
		{
			return true;
		}

		// Token: 0x06006876 RID: 26742 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnLost(Pawn victim, PawnLostCondition cond)
		{
		}

		// Token: 0x06006877 RID: 26743 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnJobDone(Pawn p, JobCondition condition)
		{
		}

		// Token: 0x06006878 RID: 26744 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnAcquiredTarget(Pawn detector, Thing newTarg)
		{
		}

		// Token: 0x06006879 RID: 26745 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnDamaged(Pawn victim, DamageInfo dinfo)
		{
		}

		// Token: 0x0600687A RID: 26746 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_BuildingLost(Building b)
		{
		}

		// Token: 0x0600687B RID: 26747 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_CorpseLost(Corpse c)
		{
		}

		// Token: 0x0600687C RID: 26748 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_ReachedDutyLocation(Pawn pawn)
		{
		}

		// Token: 0x0600687D RID: 26749 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
		}

		// Token: 0x0600687E RID: 26750 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_ConstructionCompleted(Pawn pawn, Building building)
		{
		}

		// Token: 0x0600687F RID: 26751 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_BuildingSpawnedOnMap(Building b)
		{
		}

		// Token: 0x06006880 RID: 26752 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_BuildingDespawnedOnMap(Building b)
		{
		}

		// Token: 0x06006881 RID: 26753 RVA: 0x00210B09 File Offset: 0x0020ED09
		public void AddFailCondition(Func<bool> failCondition)
		{
			this.failConditions.Add(failCondition);
		}

		// Token: 0x06006882 RID: 26754 RVA: 0x00210B18 File Offset: 0x0020ED18
		public override string ToString()
		{
			string text = base.GetType().ToString();
			if (text.Contains('.'))
			{
				text = text.Substring(text.LastIndexOf('.') + 1);
			}
			if (text.Contains('_'))
			{
				text = text.Substring(text.LastIndexOf('_') + 1);
			}
			return text;
		}

		// Token: 0x06006883 RID: 26755 RVA: 0x00210B68 File Offset: 0x0020ED68
		public void Dispose()
		{
			IDisposable disposable = this.data as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}

		// Token: 0x04004A2C RID: 18988
		public Lord lord;

		// Token: 0x04004A2D RID: 18989
		public LordToilData data;

		// Token: 0x04004A2E RID: 18990
		private List<Func<bool>> failConditions = new List<Func<bool>>();

		// Token: 0x04004A2F RID: 18991
		public bool useAvoidGrid;
	}
}
