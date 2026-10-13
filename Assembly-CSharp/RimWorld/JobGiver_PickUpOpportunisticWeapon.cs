using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200142B RID: 5163
	public class JobGiver_PickUpOpportunisticWeapon : ThinkNode_JobGiver
	{
		// Token: 0x1700139B RID: 5019
		// (get) Token: 0x06007D70 RID: 32112 RVA: 0x0025C494 File Offset: 0x0025A694
		private float MinMeleeWeaponDPSThreshold
		{
			get
			{
				List<Tool> tools = ThingDefOf.Human.tools;
				float num = 0f;
				for (int i = 0; i < tools.Count; i++)
				{
					if (tools[i].linkedBodyPartsGroup == BodyPartGroupDefOf.LeftHand || tools[i].linkedBodyPartsGroup == BodyPartGroupDefOf.RightHand)
					{
						num = tools[i].power / tools[i].cooldownTime;
						break;
					}
				}
				return num + 2f;
			}
		}

		// Token: 0x06007D71 RID: 32113 RVA: 0x0025C50C File Offset: 0x0025A70C
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_PickUpOpportunisticWeapon jobGiver_PickUpOpportunisticWeapon = (JobGiver_PickUpOpportunisticWeapon)base.DeepCopy(resolve);
			jobGiver_PickUpOpportunisticWeapon.preferBuildingDestroyers = this.preferBuildingDestroyers;
			jobGiver_PickUpOpportunisticWeapon.pickUpUtilityItems = this.pickUpUtilityItems;
			return jobGiver_PickUpOpportunisticWeapon;
		}

		// Token: 0x06007D72 RID: 32114 RVA: 0x0025C534 File Offset: 0x0025A734
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.equipment == null && pawn.apparel == null)
			{
				return null;
			}
			if (pawn.RaceProps.Humanlike && pawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				return null;
			}
			if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				return null;
			}
			if (pawn.GetRegion(RegionType.Set_Passable) == null)
			{
				return null;
			}
			if (pawn.equipment != null && !this.AlreadySatisfiedWithCurrentWeapon(pawn))
			{
				Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Weapon), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 8f, (Thing x) => pawn.CanReserve(x, 1, -1, null, false) && !x.IsBurning() && this.ShouldEquipWeapon(x, pawn), null, 0, 15, false, RegionType.Set_Passable, false, true);
				if (thing != null)
				{
					Job job = JobMaker.MakeJob(JobDefOf.Equip, thing);
					job.expiryInterval = 60;
					return job;
				}
				if (pawn.equipment.Primary != null && !SlaveRebellionUtility.WeaponUsableInRebellion(pawn.equipment.Primary))
				{
					return JobMaker.MakeJob(JobDefOf.DropEquipment, pawn.equipment.Primary);
				}
			}
			Pawn_EquipmentTracker equipment = pawn.equipment;
			bool flag;
			if (equipment == null)
			{
				flag = false;
			}
			else
			{
				ThingWithComps primary = equipment.Primary;
				bool? flag2;
				if (primary == null)
				{
					flag2 = null;
				}
				else
				{
					ThingDef def = primary.def;
					flag2 = ((def != null) ? new bool?(def.IsRangedWeapon) : null);
				}
				bool? flag3 = flag2;
				bool flag4 = true;
				flag = (flag3.GetValueOrDefault() == flag4 & flag3 != null);
			}
			if (flag)
			{
				foreach (Apparel apparel in pawn.apparel.WornApparel)
				{
					if (apparel.def == ThingDefOf.Apparel_ShieldBelt)
					{
						Job job2 = JobMaker.MakeJob(JobDefOf.RemoveApparel, apparel);
						job2.expiryInterval = 60;
						return job2;
					}
				}
			}
			if (this.pickUpUtilityItems && pawn.apparel != null && this.WouldPickupUtilityItem(pawn))
			{
				Thing thing2 = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Apparel), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 8f, (Thing x) => pawn.CanReserve(x, 1, -1, null, false) && !x.IsBurning() && this.ShouldEquipUtilityItem(x, pawn), null, 0, 15, false, RegionType.Set_Passable, false, true);
				if (thing2 != null)
				{
					Job job3 = JobMaker.MakeJob(JobDefOf.Wear, thing2);
					job3.expiryInterval = 60;
					return job3;
				}
			}
			return null;
		}

		// Token: 0x06007D73 RID: 32115 RVA: 0x0025C808 File Offset: 0x0025AA08
		private bool AlreadySatisfiedWithCurrentWeapon(Pawn pawn)
		{
			ThingWithComps primary = pawn.equipment.Primary;
			if (primary == null)
			{
				return false;
			}
			if (this.preferBuildingDestroyers)
			{
				if (!pawn.equipment.PrimaryEq.PrimaryVerb.verbProps.ai_IsBuildingDestroyer)
				{
					return false;
				}
			}
			else if (!primary.def.IsRangedWeapon || !SlaveRebellionUtility.WeaponUsableInRebellion(primary))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06007D74 RID: 32116 RVA: 0x0025C864 File Offset: 0x0025AA64
		private bool ShouldEquipWeapon(Thing newWep, Pawn pawn)
		{
			return (!newWep.def.IsRangedWeapon || !pawn.WorkTagIsDisabled(WorkTags.Shooting)) && (EquipmentUtility.CanEquip(newWep, pawn) && this.GetWeaponScore(newWep) > this.GetWeaponScore(pawn.equipment.Primary)) && SlaveRebellionUtility.WeaponUsableInRebellion(newWep);
		}

		// Token: 0x06007D75 RID: 32117 RVA: 0x0025C8B8 File Offset: 0x0025AAB8
		private int GetWeaponScore(Thing wep)
		{
			if (wep == null)
			{
				return 0;
			}
			if (wep.def.IsMeleeWeapon && wep.GetStatValue(StatDefOf.MeleeWeapon_AverageDPS, true, -1) < this.MinMeleeWeaponDPSThreshold)
			{
				return 0;
			}
			if (this.preferBuildingDestroyers && wep.TryGetComp<CompEquippable>().PrimaryVerb.verbProps.ai_IsBuildingDestroyer)
			{
				return 3;
			}
			if (wep.def.IsRangedWeapon)
			{
				return 2;
			}
			return 1;
		}

		// Token: 0x06007D76 RID: 32118 RVA: 0x0025C91F File Offset: 0x0025AB1F
		private bool WouldPickupUtilityItem(Pawn pawn)
		{
			Pawn_EquipmentTracker equipment = pawn.equipment;
			return ((equipment != null) ? equipment.Primary : null) == null && pawn.apparel.FirstApparelVerb == null;
		}

		// Token: 0x06007D77 RID: 32119 RVA: 0x0025C948 File Offset: 0x0025AB48
		private bool ShouldEquipUtilityItem(Thing thing, Pawn pawn)
		{
			Apparel apparel = thing as Apparel;
			return apparel != null && apparel.def.apparel.ai_pickUpOpportunistically && (EquipmentUtility.CanEquip(apparel, pawn) && ApparelUtility.HasPartsToWear(pawn, apparel.def)) && !pawn.apparel.WouldReplaceLockedApparel(apparel);
		}

		// Token: 0x0400544F RID: 21583
		private bool preferBuildingDestroyers;

		// Token: 0x04005450 RID: 21584
		private bool pickUpUtilityItems;
	}
}
