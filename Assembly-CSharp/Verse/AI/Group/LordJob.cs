using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Unity.Collections;

namespace Verse.AI.Group
{
	// Token: 0x02001093 RID: 4243
	public abstract class LordJob : IExposable, IDisposable
	{
		// Token: 0x1700100B RID: 4107
		// (get) Token: 0x06006807 RID: 26631 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool LostImportantReferenceDuringLoading
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700100C RID: 4108
		// (get) Token: 0x06006808 RID: 26632 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool AllowStartNewGatherings
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700100D RID: 4109
		// (get) Token: 0x06006809 RID: 26633 RVA: 0x0020FC4B File Offset: 0x0020DE4B
		public virtual bool AllowStartNewRituals
		{
			get
			{
				return this.AllowStartNewGatherings;
			}
		}

		// Token: 0x1700100E RID: 4110
		// (get) Token: 0x0600680A RID: 26634 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool NeverInRestraints
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700100F RID: 4111
		// (get) Token: 0x0600680B RID: 26635 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool GuiltyOnDowned
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001010 RID: 4112
		// (get) Token: 0x0600680C RID: 26636 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanBlockHostileVisitors
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001011 RID: 4113
		// (get) Token: 0x0600680D RID: 26637 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool AddFleeToil
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001012 RID: 4114
		// (get) Token: 0x0600680E RID: 26638 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool OrganizerIsStartingPawn
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001013 RID: 4115
		// (get) Token: 0x0600680F RID: 26639 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool KeepExistingWhileHasAnyBuilding
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001014 RID: 4116
		// (get) Token: 0x06006810 RID: 26640 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool AlwaysShowWeapon
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001015 RID: 4117
		// (get) Token: 0x06006811 RID: 26641 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool IsCaravanSendable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001016 RID: 4118
		// (get) Token: 0x06006812 RID: 26642 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ManagesRopableAnimals
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001017 RID: 4119
		// (get) Token: 0x06006813 RID: 26643 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool DontInterruptLayingPawnsOnCleanup
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001018 RID: 4120
		// (get) Token: 0x06006814 RID: 26644 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanAutoAddPawns
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001019 RID: 4121
		// (get) Token: 0x06006815 RID: 26645 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ShouldExistWithoutPawns
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700101A RID: 4122
		// (get) Token: 0x06006816 RID: 26646 RVA: 0x0020FC53 File Offset: 0x0020DE53
		public Map Map
		{
			get
			{
				return this.lord.lordManager.map;
			}
		}

		// Token: 0x06006817 RID: 26647
		public abstract StateGraph CreateGraph();

		// Token: 0x06006818 RID: 26648 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void LordJobTick()
		{
		}

		// Token: 0x06006819 RID: 26649 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ExposeData()
		{
		}

		// Token: 0x0600681A RID: 26650 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Cleanup()
		{
		}

		// Token: 0x0600681B RID: 26651 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostCleanup()
		{
		}

		// Token: 0x0600681C RID: 26652 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_AddedToLord()
		{
		}

		// Token: 0x0600681D RID: 26653 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnAdded(Pawn p)
		{
		}

		// Token: 0x0600681E RID: 26654 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
		}

		// Token: 0x0600681F RID: 26655 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PawnJobDone(Pawn p, JobCondition condition)
		{
		}

		// Token: 0x06006820 RID: 26656 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_InMentalState(Pawn pawn, MentalStateDef stateDef)
		{
		}

		// Token: 0x06006821 RID: 26657 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_BuildingAdded(Building b)
		{
		}

		// Token: 0x06006822 RID: 26658 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_CorpseAdded(Corpse c)
		{
		}

		// Token: 0x06006823 RID: 26659 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_BuildingLost(Building b)
		{
		}

		// Token: 0x06006824 RID: 26660 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_CorpseLost(Corpse c)
		{
		}

		// Token: 0x06006825 RID: 26661 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_LordDestroyed()
		{
		}

		// Token: 0x06006826 RID: 26662 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_MapRemoved()
		{
		}

		// Token: 0x06006827 RID: 26663 RVA: 0x0020FC65 File Offset: 0x0020DE65
		public virtual void Notify_PawnUndowned(Pawn p)
		{
			LordToil curLordToil = this.lord.CurLordToil;
			if (curLordToil == null)
			{
				return;
			}
			curLordToil.UpdateAllDuties();
		}

		// Token: 0x06006828 RID: 26664 RVA: 0x0020FC65 File Offset: 0x0020DE65
		public virtual void Notify_PawnDowned(Pawn p)
		{
			LordToil curLordToil = this.lord.CurLordToil;
			if (curLordToil == null)
			{
				return;
			}
			curLordToil.UpdateAllDuties();
		}

		// Token: 0x06006829 RID: 26665 RVA: 0x00009C2B File Offset: 0x00007E2B
		public virtual ThinkResult Notify_DutyConstantResult(ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			return result;
		}

		// Token: 0x0600682A RID: 26666 RVA: 0x00009C2B File Offset: 0x00007E2B
		public virtual ThinkResult Notify_DutyResult(ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			return result;
		}

		// Token: 0x0600682B RID: 26667 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string GetJobReport(Pawn pawn)
		{
			return null;
		}

		// Token: 0x0600682C RID: 26668 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string GetReport(Pawn pawn)
		{
			return null;
		}

		// Token: 0x0600682D RID: 26669 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool CanOpenAnyDoor(Pawn p)
		{
			return false;
		}

		// Token: 0x0600682E RID: 26670 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool ShouldRemovePawn(Pawn p, PawnLostCondition reason)
		{
			return true;
		}

		// Token: 0x0600682F RID: 26671 RVA: 0x000B7861 File Offset: 0x000B5A61
		public virtual IEnumerable<Gizmo> GetPawnGizmos(Pawn p)
		{
			return Enumerable.Empty<Gizmo>();
		}

		// Token: 0x06006830 RID: 26672 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool EndPawnJobOnCleanup(Pawn p)
		{
			return true;
		}

		// Token: 0x06006831 RID: 26673 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool BlocksSocialInteraction(Pawn pawn)
		{
			return false;
		}

		// Token: 0x06006832 RID: 26674 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool DutyActiveWhenDown(Pawn pawn)
		{
			return false;
		}

		// Token: 0x06006833 RID: 26675 RVA: 0x0020FC7C File Offset: 0x0020DE7C
		public virtual AcceptanceReport AllowsFloatMenu(Pawn pawn)
		{
			return true;
		}

		// Token: 0x06006834 RID: 26676 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool PrisonerSecure(Pawn pawn)
		{
			return false;
		}

		// Token: 0x06006835 RID: 26677 RVA: 0x0020FC7C File Offset: 0x0020DE7C
		public virtual AcceptanceReport AbilityAllowed(Ability ability)
		{
			return true;
		}

		// Token: 0x06006836 RID: 26678 RVA: 0x0020FC7C File Offset: 0x0020DE7C
		public virtual AcceptanceReport AllowsDrafting(Pawn pawn)
		{
			return true;
		}

		// Token: 0x06006837 RID: 26679 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool ValidateAttackTarget(Pawn searcher, Thing target)
		{
			return true;
		}

		// Token: 0x06006838 RID: 26680 RVA: 0x0020FC84 File Offset: 0x0020DE84
		public virtual NativeBitArray GetWalkGrid(Pawn pawn)
		{
			return default(NativeBitArray);
		}

		// Token: 0x06006839 RID: 26681 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Dispose()
		{
		}

		// Token: 0x04004A18 RID: 18968
		public Lord lord;
	}
}
