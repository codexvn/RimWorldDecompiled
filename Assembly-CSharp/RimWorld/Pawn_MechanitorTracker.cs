using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200245C RID: 9308
	public class Pawn_MechanitorTracker : IExposable
	{
		// Token: 0x17001E6F RID: 7791
		// (get) Token: 0x0600C99F RID: 51615 RVA: 0x003B0700 File Offset: 0x003AE900
		public List<Pawn> OverseenPawns
		{
			get
			{
				this.overseenPawns.Clear();
				List<DirectPawnRelation> directRelations = this.pawn.relations.DirectRelations;
				for (int i = 0; i < directRelations.Count; i++)
				{
					if (directRelations[i].def == PawnRelationDefOf.Overseer)
					{
						this.overseenPawns.Add(directRelations[i].otherPawn);
					}
				}
				return this.overseenPawns;
			}
		}

		// Token: 0x17001E70 RID: 7792
		// (get) Token: 0x0600C9A0 RID: 51616 RVA: 0x003B076A File Offset: 0x003AE96A
		public List<Pawn> ControlledPawns
		{
			get
			{
				return this.controlledPawns;
			}
		}

		// Token: 0x17001E71 RID: 7793
		// (get) Token: 0x0600C9A1 RID: 51617 RVA: 0x003B0772 File Offset: 0x003AE972
		public int UsedBandwidth
		{
			get
			{
				return this.UsedBandwidthFromSubjects + this.UsedBandwidthFromGestation;
			}
		}

		// Token: 0x17001E72 RID: 7794
		// (get) Token: 0x0600C9A2 RID: 51618 RVA: 0x003B0784 File Offset: 0x003AE984
		public int UsedBandwidthFromSubjects
		{
			get
			{
				return (int)(from p in this.OverseenPawns
				where !p.IsGestating()
				select p).Sum((Pawn p) => p.GetStatValue(StatDefOf.BandwidthCost, true, -1));
			}
		}

		// Token: 0x17001E73 RID: 7795
		// (get) Token: 0x0600C9A3 RID: 51619 RVA: 0x003B07E0 File Offset: 0x003AE9E0
		public int UsedBandwidthFromGestation
		{
			get
			{
				List<Bill_Mech> list = this.ActiveMechBills;
				int num = 0;
				for (int i = 0; i < list.Count; i++)
				{
					num += (int)list[i].BandwidthCost;
				}
				return num;
			}
		}

		// Token: 0x17001E74 RID: 7796
		// (get) Token: 0x0600C9A4 RID: 51620 RVA: 0x003B0818 File Offset: 0x003AEA18
		public int TotalBandwidth
		{
			get
			{
				return (int)this.pawn.GetStatValue(StatDefOf.MechBandwidth, true, -1);
			}
		}

		// Token: 0x17001E75 RID: 7797
		// (get) Token: 0x0600C9A5 RID: 51621 RVA: 0x003B082D File Offset: 0x003AEA2D
		public int TotalAvailableControlGroups
		{
			get
			{
				return (int)this.pawn.GetStatValue(StatDefOf.MechControlGroups, true, -1);
			}
		}

		// Token: 0x17001E76 RID: 7798
		// (get) Token: 0x0600C9A6 RID: 51622 RVA: 0x003B0844 File Offset: 0x003AEA44
		public List<Bill_Mech> ActiveMechBills
		{
			get
			{
				this.activeMechBills.Clear();
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsInGroup(ThingRequestGroup.MechGestator);
					for (int j = 0; j < list.Count; j++)
					{
						Building_MechGestator building_MechGestator = list[j] as Building_MechGestator;
						if (building_MechGestator != null && building_MechGestator.ActiveMechBill != null && building_MechGestator.ActiveMechBill.BoundPawn == this.pawn)
						{
							this.activeMechBills.Add(building_MechGestator.ActiveMechBill);
						}
					}
				}
				this.activeMechBills.SortBy((Bill_Mech b) => b.StartedTick);
				return this.activeMechBills;
			}
		}

		// Token: 0x17001E77 RID: 7799
		// (get) Token: 0x0600C9A7 RID: 51623 RVA: 0x003B0909 File Offset: 0x003AEB09
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x17001E78 RID: 7800
		// (get) Token: 0x0600C9A8 RID: 51624 RVA: 0x003B0914 File Offset: 0x003AEB14
		public bool AnySelectedDraftedMechs
		{
			get
			{
				List<Pawn> selectedPawns = Find.Selector.SelectedPawns;
				for (int i = 0; i < selectedPawns.Count; i++)
				{
					if (selectedPawns[i].GetOverseer() == this.pawn && selectedPawns[i].Drafted)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001E79 RID: 7801
		// (get) Token: 0x0600C9A9 RID: 51625 RVA: 0x003B0964 File Offset: 0x003AEB64
		public AcceptanceReport CanControlMechs
		{
			get
			{
				if (this.pawn.Downed)
				{
					return "MechControllerDowned".Translate(this.pawn.Named("PAWN"));
				}
				if (this.pawn.IsPrisoner)
				{
					return "MechControllerImprisoned".Translate(this.pawn.Named("PAWN"));
				}
				if (!this.pawn.Spawned)
				{
					Thing spawnedParentOrMe = this.pawn.SpawnedParentOrMe;
					if (spawnedParentOrMe is Building)
					{
						return "MechControllerInsideContainer".Translate(this.pawn.Named("PAWN"), spawnedParentOrMe.Named("CONTAINER"));
					}
					return false;
				}
				else
				{
					if (this.pawn.InMentalState)
					{
						return "MechControllerMentalState".Translate(this.pawn.Named("PAWN"), this.pawn.MentalStateDef.Named("MENTALSTATE"));
					}
					return true;
				}
			}
		}

		// Token: 0x0600C9AA RID: 51626 RVA: 0x003B0A68 File Offset: 0x003AEC68
		public Pawn_MechanitorTracker()
		{
		}

		// Token: 0x0600C9AB RID: 51627 RVA: 0x003B0AC0 File Offset: 0x003AECC0
		public Pawn_MechanitorTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600C9AC RID: 51628 RVA: 0x003B0B1C File Offset: 0x003AED1C
		public bool CanOverseeSubject(Pawn subject)
		{
			return subject.OverseerSubject != null && subject.GetStatValue(StatDefOf.BandwidthCost, true, -1) <= (float)this.TotalBandwidth;
		}

		// Token: 0x0600C9AD RID: 51629 RVA: 0x003B0B44 File Offset: 0x003AED44
		public MechanitorControlGroup GetControlGroup(Pawn mech)
		{
			foreach (MechanitorControlGroup mechanitorControlGroup in this.controlGroups)
			{
				if (mechanitorControlGroup.MechsForReading.Contains(mech))
				{
					return mechanitorControlGroup;
				}
			}
			return null;
		}

		// Token: 0x0600C9AE RID: 51630 RVA: 0x003B0BA8 File Offset: 0x003AEDA8
		public void AssignPawnControlGroup(Pawn pawn, MechWorkModeDef preferredWorkMode = null)
		{
			if (this.controlGroups.Count == 0)
			{
				this.Notify_ControlGroupAmountMayChanged();
				if (this.controlGroups.Count == 0)
				{
					Log.Warning("Wants to assign pawn to a control group, but there are none!");
					return;
				}
			}
			if (preferredWorkMode != null)
			{
				if (this.controlGroups.Any((MechanitorControlGroup c) => c.WorkMode == preferredWorkMode))
				{
					this.controlGroups.First((MechanitorControlGroup c) => c.WorkMode == preferredWorkMode).Assign(pawn);
				}
				else if (this.controlGroups.Any((MechanitorControlGroup c) => c.MechsForReading.Count == 0))
				{
					MechanitorControlGroup mechanitorControlGroup = this.controlGroups.First((MechanitorControlGroup c) => c.MechsForReading.Count == 0);
					mechanitorControlGroup.SetWorkMode(preferredWorkMode);
					mechanitorControlGroup.Assign(pawn);
				}
				else
				{
					this.controlGroups[0].Assign(pawn);
				}
			}
			else
			{
				this.controlGroups[0].Assign(pawn);
			}
			this.Notify_BandwidthChanged();
		}

		// Token: 0x0600C9AF RID: 51631 RVA: 0x003B0CCC File Offset: 0x003AEECC
		public void UnassignPawnFromAnyControlGroup(Pawn pawn)
		{
			foreach (MechanitorControlGroup mechanitorControlGroup in this.controlGroups)
			{
				mechanitorControlGroup.TryUnassign(pawn);
			}
		}

		// Token: 0x0600C9B0 RID: 51632 RVA: 0x003B0D20 File Offset: 0x003AEF20
		private void Notify_ControlGroupAmountMayChanged()
		{
			int totalAvailableControlGroups = this.TotalAvailableControlGroups;
			while (this.controlGroups.Count < totalAvailableControlGroups)
			{
				this.controlGroups.Add(new MechanitorControlGroup(this));
			}
			List<Pawn> list = null;
			while (this.controlGroups.Count > totalAvailableControlGroups)
			{
				MechanitorControlGroup mechanitorControlGroup = this.controlGroups[this.controlGroups.Count - 1];
				if (this.controlGroups.Count > 1)
				{
					if (!mechanitorControlGroup.MechsForReading.NullOrEmpty<Pawn>())
					{
						if (list == null)
						{
							list = new List<Pawn>();
						}
						list.AddRange(mechanitorControlGroup.MechsForReading);
					}
				}
				else
				{
					Log.Warning("Removed last mechanitor control group");
				}
				this.controlGroups.RemoveAt(this.controlGroups.Count - 1);
			}
			if (list != null && this.controlGroups.Count > 0)
			{
				foreach (Pawn pawn in list)
				{
					this.controlGroups[0].Assign(pawn);
				}
			}
			this.controlGroupGizmos.Clear();
			foreach (MechanitorControlGroup controlGroup in this.controlGroups)
			{
				this.controlGroupGizmos.Add(new MechanitorControlGroupGizmo(controlGroup));
			}
		}

		// Token: 0x0600C9B1 RID: 51633 RVA: 0x003B0E90 File Offset: 0x003AF090
		public void Notify_PawnSpawned(bool respawningAfterLoad)
		{
			if (respawningAfterLoad)
			{
				this.Notify_BandwidthChanged();
				this.Notify_ControlGroupAmountMayChanged();
			}
		}

		// Token: 0x0600C9B2 RID: 51634 RVA: 0x003B0EA4 File Offset: 0x003AF0A4
		public void Notify_BandwidthChanged()
		{
			this.CheckAvailableBandwidthForBills();
			Pawn_MechanitorTracker.tmpPrevControlledMechs.Clear();
			Pawn_MechanitorTracker.tmpPrevControlledMechs.AddRange(this.controlledPawns);
			this.controlledPawns.Clear();
			this.tmpMechsInAssignedOrder.Clear();
			MechanitorUtility.GetMechsInAssignedOrder(this.pawn, ref this.tmpMechsInAssignedOrder);
			float num = 0f;
			for (int i = this.tmpMechsInAssignedOrder.Count - 1; i >= 0; i--)
			{
				num += this.tmpMechsInAssignedOrder[i].GetStatValue(StatDefOf.BandwidthCost, true, -1);
				if (num <= (float)this.TotalBandwidth)
				{
					this.controlledPawns.Add(this.tmpMechsInAssignedOrder[i]);
				}
				else if (Pawn_MechanitorTracker.tmpPrevControlledMechs.Contains(this.tmpMechsInAssignedOrder[i]))
				{
					CompOverseerSubject overseerSubject = this.tmpMechsInAssignedOrder[i].OverseerSubject;
					if (overseerSubject != null)
					{
						overseerSubject.Notify_DisconnectedFromOverseer();
					}
				}
			}
			Pawn_MechanitorTracker.tmpPrevControlledMechs.Clear();
			this.tmpMechsInAssignedOrder.Clear();
		}

		// Token: 0x0600C9B3 RID: 51635 RVA: 0x003B0FA0 File Offset: 0x003AF1A0
		public void CheckAvailableBandwidthForBills()
		{
			List<Bill_Mech> list = this.ActiveMechBills;
			for (int i = 0; i < list.Count; i++)
			{
				list[i].suspended = !this.HasBandwidthForBill(list[i]);
			}
		}

		// Token: 0x0600C9B4 RID: 51636 RVA: 0x003B0FE4 File Offset: 0x003AF1E4
		public void Notify_MechlinkRemoved()
		{
			List<Pawn> list = this.OverseenPawns;
			for (int i = list.Count - 1; i >= 0; i--)
			{
				this.pawn.relations.TryRemoveDirectRelation(PawnRelationDefOf.Overseer, list[i]);
			}
			PawnComponentsUtility.AddAndRemoveDynamicComponents(this.pawn, false);
		}

		// Token: 0x0600C9B5 RID: 51637 RVA: 0x003B1034 File Offset: 0x003AF234
		public void Notify_ChangedGuestStatus()
		{
			if (this.pawn.IsPrisoner)
			{
				this.UndraftAllMechs();
				for (int i = 0; i < this.controlGroups.Count; i++)
				{
					this.controlGroups[i].SetWorkMode(MechWorkModeDefOf.SelfShutdown);
				}
			}
		}

		// Token: 0x0600C9B6 RID: 51638 RVA: 0x003B1080 File Offset: 0x003AF280
		public void Notify_ApparelChanged()
		{
			this.Notify_BandwidthChanged();
			this.Notify_ControlGroupAmountMayChanged();
		}

		// Token: 0x0600C9B7 RID: 51639 RVA: 0x003B0E90 File Offset: 0x003AF090
		public void Notify_HediffStateChange(Hediff hediff)
		{
			if (hediff != null)
			{
				this.Notify_BandwidthChanged();
				this.Notify_ControlGroupAmountMayChanged();
			}
		}

		// Token: 0x0600C9B8 RID: 51640 RVA: 0x003B1090 File Offset: 0x003AF290
		public void Notify_ChangedFaction()
		{
			List<Pawn> list = this.OverseenPawns;
			for (int i = 0; i < list.Count; i++)
			{
				list[i].SetFaction(this.pawn.Faction, null);
			}
		}

		// Token: 0x0600C9B9 RID: 51641 RVA: 0x003B10CD File Offset: 0x003AF2CD
		public void Notify_Downed()
		{
			this.UndraftAllMechs();
		}

		// Token: 0x0600C9BA RID: 51642 RVA: 0x003B10D5 File Offset: 0x003AF2D5
		public void Notify_DeSpawned(DestroyMode mode)
		{
			if (mode != DestroyMode.WillReplace)
			{
				this.UndraftAllMechs();
			}
		}

		// Token: 0x0600C9BB RID: 51643 RVA: 0x003B10E4 File Offset: 0x003AF2E4
		public void UndraftAllMechs()
		{
			this.tmpMechsInAssignedOrder.Clear();
			MechanitorUtility.GetMechsInAssignedOrder(this.pawn, ref this.tmpMechsInAssignedOrder);
			for (int i = 0; i < this.tmpMechsInAssignedOrder.Count; i++)
			{
				if (this.tmpMechsInAssignedOrder[i].Drafted)
				{
					this.tmpMechsInAssignedOrder[i].drafter.Drafted = false;
				}
			}
			this.tmpMechsInAssignedOrder.Clear();
		}

		// Token: 0x0600C9BC RID: 51644 RVA: 0x003B1158 File Offset: 0x003AF358
		public bool HasBandwidthForBill(Bill_Mech bill)
		{
			int totalBandwidth = this.TotalBandwidth;
			int num = this.UsedBandwidthFromSubjects;
			float bandwidthCost = bill.BandwidthCost;
			if ((float)totalBandwidth < bandwidthCost + (float)num)
			{
				return false;
			}
			List<Bill_Mech> list = this.ActiveMechBills;
			for (int i = 0; i < list.Count; i++)
			{
				num += (int)list[i].BandwidthCost;
				if (list[i] == bill && num <= totalBandwidth)
				{
					return true;
				}
				if (num >= totalBandwidth)
				{
					return false;
				}
			}
			return (float)num + bandwidthCost <= (float)totalBandwidth;
		}

		// Token: 0x0600C9BD RID: 51645 RVA: 0x003B11D4 File Offset: 0x003AF3D4
		public bool HasBandwidthToResurrect(Corpse corpse)
		{
			float statValue = corpse.InnerPawn.GetStatValue(StatDefOf.BandwidthCost, true, -1);
			return (float)this.UsedBandwidth + statValue <= (float)this.TotalBandwidth;
		}

		// Token: 0x0600C9BE RID: 51646 RVA: 0x003B1209 File Offset: 0x003AF409
		public void DrawCommandRadius()
		{
			if (this.pawn.Spawned && this.AnySelectedDraftedMechs)
			{
				GenDraw.DrawRadiusRing(this.pawn.Position, 24.9f, Color.white, (IntVec3 c) => this.CanCommandTo(c));
			}
		}

		// Token: 0x0600C9BF RID: 51647 RVA: 0x003B1246 File Offset: 0x003AF446
		public bool CanCommandTo(LocalTargetInfo target)
		{
			return target.Cell.InBounds(this.pawn.MapHeld) && (float)this.pawn.Position.DistanceToSquared(target.Cell) < 620.01f;
		}

		// Token: 0x0600C9C0 RID: 51648 RVA: 0x003B1282 File Offset: 0x003AF482
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (this.bandwidthGizmo == null)
			{
				this.bandwidthGizmo = new MechanitorBandwidthGizmo(this);
			}
			if (this.callBossgroupGizmo == null)
			{
				this.callBossgroupGizmo = new Command_CallBossgroup(this);
			}
			yield return this.bandwidthGizmo;
			yield return this.callBossgroupGizmo;
			foreach (MechanitorControlGroupGizmo mechanitorControlGroupGizmo in this.controlGroupGizmos)
			{
				yield return mechanitorControlGroupGizmo;
			}
			List<MechanitorControlGroupGizmo>.Enumerator enumerator = default(List<MechanitorControlGroupGizmo>.Enumerator);
			yield return new Command_Action
			{
				defaultLabel = "CommandSelectAllMechs".Translate(),
				defaultDesc = "CommandSelectAllMechsDesc".Translate(),
				icon = Pawn_MechanitorTracker.SelectAllMechsIcon.Texture,
				Order = -87f,
				action = delegate()
				{
					Find.Selector.ClearSelection();
					this.tmpMechsInAssignedOrder.Clear();
					MechanitorUtility.GetMechsInAssignedOrder(this.pawn, ref this.tmpMechsInAssignedOrder);
					for (int i = 0; i < this.tmpMechsInAssignedOrder.Count; i++)
					{
						Find.Selector.Select(this.tmpMechsInAssignedOrder[i], true, true);
					}
					this.tmpMechsInAssignedOrder.Clear();
				}
			};
			yield break;
			yield break;
		}

		// Token: 0x0600C9C1 RID: 51649 RVA: 0x003B1294 File Offset: 0x003AF494
		public void ExposeData()
		{
			Scribe_Collections.Look<MechanitorControlGroup>(ref this.controlGroups, "controlGroups", LookMode.Deep, new object[]
			{
				this
			});
			Scribe_Collections.Look<Pawn>(ref this.controlledPawns, "controlledPawns", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.controlledPawns.RemoveAll((Pawn x) => x == null);
				if (this.controlGroups == null)
				{
					this.controlGroups = new List<MechanitorControlGroup>();
				}
				this.Notify_ControlGroupAmountMayChanged();
			}
		}

		// Token: 0x04008955 RID: 35157
		private const float MechCommandRange = 24.9f;

		// Token: 0x04008956 RID: 35158
		private static readonly CachedTexture SelectAllMechsIcon = new CachedTexture("UI/Icons/SelectAllMechs");

		// Token: 0x04008957 RID: 35159
		private Pawn pawn;

		// Token: 0x04008958 RID: 35160
		private MechanitorBandwidthGizmo bandwidthGizmo;

		// Token: 0x04008959 RID: 35161
		private Command_CallBossgroup callBossgroupGizmo;

		// Token: 0x0400895A RID: 35162
		private List<MechanitorControlGroupGizmo> controlGroupGizmos = new List<MechanitorControlGroupGizmo>();

		// Token: 0x0400895B RID: 35163
		private List<Pawn> tmpMechsInAssignedOrder = new List<Pawn>();

		// Token: 0x0400895C RID: 35164
		private List<Pawn> overseenPawns = new List<Pawn>();

		// Token: 0x0400895D RID: 35165
		private List<Pawn> controlledPawns = new List<Pawn>();

		// Token: 0x0400895E RID: 35166
		public List<MechanitorControlGroup> controlGroups = new List<MechanitorControlGroup>();

		// Token: 0x0400895F RID: 35167
		private List<Bill_Mech> activeMechBills = new List<Bill_Mech>();

		// Token: 0x04008960 RID: 35168
		private static List<Pawn> tmpPrevControlledMechs = new List<Pawn>();
	}
}
