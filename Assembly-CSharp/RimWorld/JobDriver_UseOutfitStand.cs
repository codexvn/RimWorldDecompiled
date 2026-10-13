using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013C5 RID: 5061
	public class JobDriver_UseOutfitStand : JobDriver
	{
		// Token: 0x17001385 RID: 4997
		// (get) Token: 0x06007BEC RID: 31724 RVA: 0x0025519B File Offset: 0x0025339B
		private Building_OutfitStand OutfitStand
		{
			get
			{
				return (Building_OutfitStand)this.job.targetA.Thing;
			}
		}

		// Token: 0x06007BED RID: 31725 RVA: 0x002551B2 File Offset: 0x002533B2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Collections.Look<Apparel>(ref this.wornApparelToTransferToStand, "wornApparelToTransferToStand", LookMode.Reference);
			Scribe_Collections.Look<Apparel>(ref this.standApparelToTransferToPawn, "standApparelToTransferToPawn", LookMode.Reference);
		}

		// Token: 0x06007BEE RID: 31726 RVA: 0x001E06E3 File Offset: 0x001DE8E3
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.targetA, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007BEF RID: 31727 RVA: 0x002551F0 File Offset: 0x002533F0
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.standApparelToTransferToPawn = new HashSet<Apparel>();
			this.wornApparelToTransferToStand = new HashSet<Apparel>();
			List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
			IEnumerable<Thing> heldItems = this.OutfitStand.HeldItems;
			this.duration = 0;
			foreach (Thing thing in heldItems)
			{
				Apparel apparel = thing as Apparel;
				if (apparel != null && apparel.PawnCanWear(this.pawn, false) && ApparelUtility.HasPartsToWear(this.pawn, apparel.def) && (!CompBiocodable.IsBiocoded(apparel) || CompBiocodable.IsBiocodedFor(apparel, this.pawn)))
				{
					bool flag = true;
					foreach (Apparel apparel2 in wornApparel)
					{
						if (!ApparelUtility.CanWearTogether(apparel.def, apparel2.def, this.pawn.RaceProps.body))
						{
							if (this.pawn.apparel.IsLocked(apparel2))
							{
								flag = false;
								break;
							}
							this.duration += (int)(apparel2.GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f);
							this.wornApparelToTransferToStand.Add(apparel2);
						}
					}
					if (flag)
					{
						this.standApparelToTransferToPawn.Add(apparel);
						this.duration += (int)(apparel.GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f);
					}
				}
			}
		}

		// Token: 0x06007BF0 RID: 31728 RVA: 0x002553B0 File Offset: 0x002535B0
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnBurningImmobile(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell, false).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.duration;
			yield return toil;
			Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
			toil2.AddFinishAction(new Action(this.DoTransfer));
			toil2.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil2;
			yield break;
		}

		// Token: 0x06007BF1 RID: 31729 RVA: 0x002553C0 File Offset: 0x002535C0
		private void DoTransfer()
		{
			foreach (Apparel ap in this.wornApparelToTransferToStand)
			{
				this.pawn.apparel.Remove(ap);
			}
			foreach (Apparel apparel in this.standApparelToTransferToPawn)
			{
				this.OutfitStand.RemoveApparel(apparel);
				this.pawn.apparel.Wear(apparel, true, false);
				this.pawn.outfits.forcedHandler.SetForced(apparel, true);
			}
			foreach (Apparel apparel2 in this.wornApparelToTransferToStand)
			{
				this.OutfitStand.AddApparel(apparel2);
			}
			ThingWithComps heldWeapon = this.OutfitStand.HeldWeapon;
			if (heldWeapon != null && JobDriver_UseOutfitStand.PawnCanWieldWeapon(heldWeapon, this.pawn) && this.OutfitStand.RemoveHeldWeapon(heldWeapon))
			{
				ThingWithComps thingWithComps;
				this.pawn.equipment.MakeRoomFor(heldWeapon, out thingWithComps);
				this.pawn.equipment.AddEquipment(heldWeapon);
				if (thingWithComps != null)
				{
					thingWithComps.DeSpawn(DestroyMode.Vanish);
					this.OutfitStand.TryAddHeldWeapon(thingWithComps);
				}
			}
		}

		// Token: 0x06007BF2 RID: 31730 RVA: 0x00255544 File Offset: 0x00253744
		private static bool PawnCanWieldWeapon(Thing weapon, Pawn pawn)
		{
			string text;
			return (!weapon.def.IsWeapon || !pawn.WorkTagIsDisabled(WorkTags.Violent)) && (!weapon.def.IsRangedWeapon || !pawn.WorkTagIsDisabled(WorkTags.Shooting)) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) && (!pawn.IsQuestLodger() || EquipmentUtility.QuestLodgerCanEquip(weapon, pawn)) && EquipmentUtility.CanEquip(weapon, pawn, out text, true);
		}

		// Token: 0x040053B4 RID: 21428
		private int duration;

		// Token: 0x040053B5 RID: 21429
		private HashSet<Apparel> wornApparelToTransferToStand;

		// Token: 0x040053B6 RID: 21430
		private HashSet<Apparel> standApparelToTransferToPawn;
	}
}
