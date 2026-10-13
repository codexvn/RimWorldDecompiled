using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020029D6 RID: 10710
	public class Building_Bed : Building
	{
		// Token: 0x17002460 RID: 9312
		// (get) Token: 0x0600EC0B RID: 60427 RVA: 0x0044898F File Offset: 0x00446B8F
		public List<Pawn> OwnersForReading
		{
			get
			{
				return this.CompAssignableToPawn.AssignedPawnsForReading;
			}
		}

		// Token: 0x17002461 RID: 9313
		// (get) Token: 0x0600EC0C RID: 60428 RVA: 0x0044899C File Offset: 0x00446B9C
		public CompAssignableToPawn CompAssignableToPawn
		{
			get
			{
				return base.GetComp<CompAssignableToPawn>();
			}
		}

		// Token: 0x17002462 RID: 9314
		// (get) Token: 0x0600EC0D RID: 60429 RVA: 0x004489A4 File Offset: 0x00446BA4
		// (set) Token: 0x0600EC0E RID: 60430 RVA: 0x004489B0 File Offset: 0x00446BB0
		public bool ForPrisoners
		{
			get
			{
				return this.forOwnerType == BedOwnerType.Prisoner;
			}
			set
			{
				if (value == this.ForPrisoners || !this.def.building.bed_humanlike || this.ForHumanBabies)
				{
					return;
				}
				if (Current.ProgramState != ProgramState.Playing && Scribe.mode != LoadSaveMode.Inactive)
				{
					Log.Error("Tried to set ForPrisoners while game mode was " + Current.ProgramState.ToString());
					return;
				}
				this.RemoveAllOwners(false);
				if (value)
				{
					this.forOwnerType = BedOwnerType.Prisoner;
				}
				else
				{
					this.forOwnerType = BedOwnerType.Colonist;
					Log.Error("Bed ForPrisoners=false, but should it be for for colonists or slaves?  Set ForOwnerType instead.");
				}
				this.Notify_ColorChanged();
				this.NotifyRoomBedTypeChanged();
			}
		}

		// Token: 0x17002463 RID: 9315
		// (get) Token: 0x0600EC0F RID: 60431 RVA: 0x00448A43 File Offset: 0x00446C43
		public bool ForSlaves
		{
			get
			{
				return this.ForOwnerType == BedOwnerType.Slave;
			}
		}

		// Token: 0x17002464 RID: 9316
		// (get) Token: 0x0600EC10 RID: 60432 RVA: 0x00448A4E File Offset: 0x00446C4E
		public bool ForColonists
		{
			get
			{
				return this.ForOwnerType == BedOwnerType.Colonist;
			}
		}

		// Token: 0x17002465 RID: 9317
		// (get) Token: 0x0600EC11 RID: 60433 RVA: 0x00448A59 File Offset: 0x00446C59
		public bool ForHumanBabies
		{
			get
			{
				return this.def.building.bed_humanlike && this.def.building.bed_maxBodySize < LifeStageDefOf.HumanlikeChild.bodySizeFactor;
			}
		}

		// Token: 0x17002466 RID: 9318
		// (get) Token: 0x0600EC12 RID: 60434 RVA: 0x00448A8B File Offset: 0x00446C8B
		// (set) Token: 0x0600EC13 RID: 60435 RVA: 0x00448A94 File Offset: 0x00446C94
		public BedOwnerType ForOwnerType
		{
			get
			{
				return this.forOwnerType;
			}
			set
			{
				if (value == this.forOwnerType || !this.def.building.bed_humanlike || this.ForHumanBabies)
				{
					return;
				}
				if (value == BedOwnerType.Slave && !ModLister.CheckIdeology("Slavery"))
				{
					return;
				}
				this.RemoveAllOwners(false);
				this.forOwnerType = value;
				this.Notify_ColorChanged();
				this.NotifyRoomBedTypeChanged();
			}
		}

		// Token: 0x17002467 RID: 9319
		// (get) Token: 0x0600EC14 RID: 60436 RVA: 0x00448AF0 File Offset: 0x00446CF0
		// (set) Token: 0x0600EC15 RID: 60437 RVA: 0x00448AF8 File Offset: 0x00446CF8
		public bool Medical
		{
			get
			{
				return this.medicalInt;
			}
			set
			{
				if (value == this.medicalInt)
				{
					return;
				}
				if (value && !this.def.building.bed_canBeMedical)
				{
					return;
				}
				this.RemoveAllOwners(false);
				this.medicalInt = value;
				this.Notify_ColorChanged();
				if (base.Spawned)
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
					this.NotifyRoomBedTypeChanged();
				}
				this.FacilityChanged();
			}
		}

		// Token: 0x17002468 RID: 9320
		// (get) Token: 0x0600EC16 RID: 60438 RVA: 0x00448B6D File Offset: 0x00446D6D
		public bool AnyUnownedSleepingSlot
		{
			get
			{
				if (this.Medical)
				{
					Log.Warning("Tried to check for unowned sleeping slot on medical bed " + ((this != null) ? this.ToString() : null));
					return false;
				}
				return this.CompAssignableToPawn.HasFreeSlot;
			}
		}

		// Token: 0x17002469 RID: 9321
		// (get) Token: 0x0600EC17 RID: 60439 RVA: 0x00448BA0 File Offset: 0x00446DA0
		public int TotalSleepingSlots
		{
			get
			{
				if (this.Medical)
				{
					Log.Warning("Tried to check for total sleeping slots on medical bed " + ((this != null) ? this.ToString() : null));
					return 0;
				}
				return this.CompAssignableToPawn.TotalSlots;
			}
		}

		// Token: 0x1700246A RID: 9322
		// (get) Token: 0x0600EC18 RID: 60440 RVA: 0x00448BD4 File Offset: 0x00446DD4
		public bool AnyUnoccupiedSleepingSlot
		{
			get
			{
				for (int i = 0; i < this.SleepingSlotsCount; i++)
				{
					if (this.GetCurOccupant(i) == null)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700246B RID: 9323
		// (get) Token: 0x0600EC19 RID: 60441 RVA: 0x00448BFE File Offset: 0x00446DFE
		public IEnumerable<Pawn> CurOccupants
		{
			get
			{
				int num;
				for (int i = 0; i < this.SleepingSlotsCount; i = num + 1)
				{
					Pawn curOccupant = this.GetCurOccupant(i);
					if (curOccupant != null)
					{
						yield return curOccupant;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x1700246C RID: 9324
		// (get) Token: 0x0600EC1A RID: 60442 RVA: 0x00448C10 File Offset: 0x00446E10
		public bool AnyOccupants
		{
			get
			{
				for (int i = 0; i < this.SleepingSlotsCount; i++)
				{
					if (this.GetCurOccupant(i) != null)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x1700246D RID: 9325
		// (get) Token: 0x0600EC1B RID: 60443 RVA: 0x00448C3A File Offset: 0x00446E3A
		public override Color DrawColor
		{
			get
			{
				if (this.def.MadeFromStuff)
				{
					return base.DrawColor;
				}
				return this.DrawColorTwo;
			}
		}

		// Token: 0x1700246E RID: 9326
		// (get) Token: 0x0600EC1C RID: 60444 RVA: 0x00448C58 File Offset: 0x00446E58
		public override Color DrawColorTwo
		{
			get
			{
				if (this.def.building != null && !this.def.building.bed_UseSheetColor)
				{
					return base.DrawColorTwo;
				}
				bool medical = this.Medical;
				BedOwnerType bedOwnerType = this.forOwnerType;
				if (bedOwnerType != BedOwnerType.Prisoner)
				{
					if (bedOwnerType != BedOwnerType.Slave)
					{
						if (medical)
						{
							return Building_Bed.SheetColorMedical;
						}
						if (this.def == ThingDefOf.RoyalBed)
						{
							return Building_Bed.SheetColorRoyal;
						}
						return Building_Bed.SheetColorNormal;
					}
					else
					{
						if (!medical)
						{
							return Building_Bed.SheetColorForSlave;
						}
						return Building_Bed.SheetColorMedicalForSlave;
					}
				}
				else
				{
					if (!medical)
					{
						return Building_Bed.SheetColorForPrisoner;
					}
					return Building_Bed.SheetColorMedicalForPrisoner;
				}
			}
		}

		// Token: 0x1700246F RID: 9327
		// (get) Token: 0x0600EC1D RID: 60445 RVA: 0x00448CE2 File Offset: 0x00446EE2
		public int SleepingSlotsCount
		{
			get
			{
				return BedUtility.GetSleepingSlotsCount(this.def.size);
			}
		}

		// Token: 0x17002470 RID: 9328
		// (get) Token: 0x0600EC1E RID: 60446 RVA: 0x00448CF4 File Offset: 0x00446EF4
		private bool PlayerCanSeeOwners
		{
			get
			{
				return this.CompAssignableToPawn.PlayerCanSeeAssignments;
			}
		}

		// Token: 0x0600EC1F RID: 60447 RVA: 0x00448D04 File Offset: 0x00446F04
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			Region validRegionAt_NoRebuild = map.regionGrid.GetValidRegionAt_NoRebuild(base.Position);
			if (validRegionAt_NoRebuild != null && validRegionAt_NoRebuild.Room.IsPrisonCell)
			{
				this.ForPrisoners = true;
			}
			if (!this.alreadySetDefaultMed)
			{
				this.alreadySetDefaultMed = true;
				if (this.def.building.bed_defaultMedical)
				{
					this.Medical = true;
				}
			}
			if (!respawningAfterLoad)
			{
				District district = this.GetDistrict(RegionType.Set_Passable);
				if (district != null)
				{
					district.Notify_RoomShapeOrContainedBedsChanged();
					district.Room.Notify_RoomShapeChanged();
				}
			}
		}

		// Token: 0x0600EC20 RID: 60448 RVA: 0x00448D8C File Offset: 0x00446F8C
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				if (mode != DestroyMode.Vanish)
				{
					this.RemoveAllOwners(mode == DestroyMode.KillFinalize);
				}
				else if (InstallBlueprintUtility.ExistingBlueprintFor(this) == null)
				{
					foreach (Pawn pawn in this.OwnersForReading)
					{
						Messages.Message("MessageBedLostAssignment".Translate(this.def, pawn), new LookTargets(new TargetInfo[]
						{
							this,
							pawn
						}), MessageTypeDefOf.CautionInput, false);
					}
				}
				this.ForOwnerType = BedOwnerType.Colonist;
				this.Medical = false;
				this.alreadySetDefaultMed = false;
			}
			District district = this.GetDistrict(RegionType.Set_Passable);
			base.DeSpawn(mode);
			if (district != null)
			{
				district.Notify_RoomShapeOrContainedBedsChanged();
				district.Room.Notify_RoomShapeChanged();
			}
		}

		// Token: 0x0600EC21 RID: 60449 RVA: 0x00448E84 File Offset: 0x00447084
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.medicalInt, "medical", false, false);
			Scribe_Values.Look<bool>(ref this.alreadySetDefaultMed, "alreadySetDefaultMed", false, false);
			Scribe_Values.Look<BedOwnerType>(ref this.forOwnerType, "forOwnerType", BedOwnerType.Colonist, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x0600EC22 RID: 60450 RVA: 0x00448ED4 File Offset: 0x004470D4
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			Room room = this.GetRoom(RegionType.Set_All);
			if (room != null && Building_Bed.RoomCanBePrisonCell(room))
			{
				room.DrawFieldEdges();
			}
		}

		// Token: 0x0600EC23 RID: 60451 RVA: 0x00448F01 File Offset: 0x00447101
		public static bool RoomCanBePrisonCell(Room r)
		{
			return r.ProperRoom && !r.IsHuge;
		}

		// Token: 0x0600EC24 RID: 60452 RVA: 0x00448F16 File Offset: 0x00447116
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (base.Faction == Faction.OfPlayer)
			{
				if (this.def.building.bed_humanlike && !this.ForHumanBabies)
				{
					if (ModsConfig.IdeologyActive)
					{
						yield return new Command_SetBedOwnerType(this);
					}
					else
					{
						Command_Toggle command_Toggle = new Command_Toggle();
						command_Toggle.defaultLabel = "CommandBedSetForPrisonersLabel".Translate();
						command_Toggle.defaultDesc = "CommandBedSetForPrisonersDesc".Translate();
						command_Toggle.icon = ContentFinder<Texture2D>.Get("UI/Commands/ForPrisoners", true);
						command_Toggle.isActive = (() => this.ForPrisoners);
						command_Toggle.toggleAction = delegate()
						{
							this.SetBedOwnerTypeByInterface(this.ForPrisoners ? BedOwnerType.Colonist : BedOwnerType.Prisoner);
						};
						if (!Building_Bed.RoomCanBePrisonCell(this.GetRoom(RegionType.Set_All)) && !this.ForPrisoners)
						{
							command_Toggle.Disable("CommandBedSetForPrisonersFailOutdoors".Translate());
						}
						command_Toggle.hotKey = KeyBindingDefOf.Misc3;
						command_Toggle.turnOffSound = null;
						command_Toggle.turnOnSound = null;
						yield return command_Toggle;
					}
				}
				if (this.def.building.bed_canBeMedical)
				{
					yield return new Command_Toggle
					{
						defaultLabel = "CommandBedSetAsMedicalLabel".Translate(),
						defaultDesc = "CommandBedSetAsMedicalDesc".Translate(),
						icon = ContentFinder<Texture2D>.Get("UI/Commands/AsMedical", true),
						isActive = (() => this.Medical),
						toggleAction = delegate()
						{
							this.Medical = !this.Medical;
						},
						hotKey = KeyBindingDefOf.Misc2
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600EC25 RID: 60453 RVA: 0x00448F28 File Offset: 0x00447128
		public void SetBedOwnerTypeByInterface(BedOwnerType ownerType)
		{
			if (Building_Bed.lastBedOwnerSetChangeFrame == Time.frameCount)
			{
				return;
			}
			Building_Bed.lastBedOwnerSetChangeFrame = Time.frameCount;
			((this.ForOwnerType != ownerType) ? SoundDefOf.Checkbox_TurnedOn : SoundDefOf.Checkbox_TurnedOff).PlayOneShotOnCamera(null);
			List<Building_Bed> bedsToAffect = new List<Building_Bed>();
			foreach (Building_Bed building_Bed in Find.Selector.SelectedObjects.OfType<Building_Bed>())
			{
				if (building_Bed.ForOwnerType != ownerType)
				{
					Room room = building_Bed.GetRoom(RegionType.Set_All);
					if (room == null && ownerType != BedOwnerType.Prisoner)
					{
						if (!bedsToAffect.Contains(building_Bed))
						{
							bedsToAffect.Add(building_Bed);
						}
					}
					else
					{
						foreach (Building_Bed building_Bed2 in room.ContainedBeds)
						{
							if (building_Bed2.ForOwnerType != ownerType)
							{
								if (building_Bed2.ForOwnerType == BedOwnerType.Prisoner && !bedsToAffect.Contains(building_Bed2))
								{
									bedsToAffect.Add(building_Bed2);
								}
								else if (ownerType == BedOwnerType.Prisoner && Building_Bed.RoomCanBePrisonCell(room) && !bedsToAffect.Contains(building_Bed2))
								{
									bedsToAffect.Add(building_Bed2);
								}
								else if (building_Bed2 == building_Bed && !bedsToAffect.Contains(building_Bed2))
								{
									bedsToAffect.Add(building_Bed2);
								}
							}
						}
					}
				}
			}
			Action action = delegate()
			{
				List<District> list = new List<District>();
				List<Room> list2 = new List<Room>();
				foreach (Building_Bed building_Bed4 in bedsToAffect)
				{
					District district = building_Bed4.GetDistrict(RegionType.Set_Passable);
					Room room2 = district.Room;
					if (ownerType == BedOwnerType.Prisoner && room2.TouchesMapEdge)
					{
						building_Bed4.ForOwnerType = BedOwnerType.Colonist;
					}
					else
					{
						building_Bed4.ForOwnerType = ownerType;
					}
					if (!room2.TouchesMapEdge)
					{
						if (!list2.Contains(room2))
						{
							list2.Add(room2);
						}
						if (!list.Contains(district))
						{
							list.Add(district);
						}
					}
				}
				foreach (District district2 in list)
				{
					district2.Notify_RoomShapeOrContainedBedsChanged();
				}
				foreach (Room room3 in list2)
				{
					room3.Notify_RoomShapeChanged();
				}
			};
			if ((from b in bedsToAffect
			where b.OwnersForReading.Any((Pawn owner) => owner.RaceProps.Humanlike) && b != this
			select b).Count<Building_Bed>() == 0)
			{
				action();
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (!ModsConfig.IdeologyActive)
			{
				if (ownerType == BedOwnerType.Prisoner)
				{
					stringBuilder.Append("TurningOnPrisonerBedWarning".Translate());
				}
				else
				{
					stringBuilder.Append("TurningOffPrisonerBedWarning".Translate());
				}
			}
			else
			{
				stringBuilder.Append("ChangingOwnerTypeBedWarning".Translate());
			}
			stringBuilder.AppendLine();
			foreach (Building_Bed building_Bed3 in bedsToAffect)
			{
				if (ownerType != building_Bed3.ForOwnerType)
				{
					for (int i = 0; i < building_Bed3.OwnersForReading.Count; i++)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append(building_Bed3.OwnersForReading[i].LabelShort);
					}
				}
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append("AreYouSure".Translate());
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(stringBuilder.ToString(), action, false, null, WindowLayer.Dialog));
		}

		// Token: 0x0600EC26 RID: 60454 RVA: 0x0044927C File Offset: 0x0044747C
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (this.def.building.bed_humanlike && this.def.building.bed_DisplayOwnerType && base.Faction == Faction.OfPlayer)
			{
				switch (this.ForOwnerType)
				{
				case BedOwnerType.Colonist:
					stringBuilder.AppendInNewLine("ForColonistUse".Translate());
					break;
				case BedOwnerType.Prisoner:
					stringBuilder.AppendInNewLine("ForPrisonerUse".Translate());
					break;
				case BedOwnerType.Slave:
					stringBuilder.AppendInNewLine("ForSlaveUse".Translate());
					break;
				default:
					Log.Error(string.Format("Unknown bed owner type: {0}", this.ForOwnerType));
					break;
				}
			}
			if (this.Medical)
			{
				stringBuilder.AppendInNewLine("MedicalBed".Translate());
				if (base.Spawned)
				{
					stringBuilder.AppendInNewLine("RoomInfectionChanceFactor".Translate() + ": " + this.GetRoom(RegionType.Set_All).GetStat(RoomStatDefOf.InfectionChanceFactor).ToStringPercent());
				}
			}
			else if (this.PlayerCanSeeOwners && this.def.building.bed_DisplayOwnersInInspectString)
			{
				if (this.OwnersForReading.Count == 0)
				{
					stringBuilder.AppendInNewLine("Owner".Translate() + ": " + "Nobody".Translate());
				}
				else if (this.OwnersForReading.Count == 1)
				{
					stringBuilder.AppendInNewLine("Owner".Translate() + ": " + this.OwnersForReading[0].Label);
				}
				else
				{
					stringBuilder.AppendInNewLine("Owners".Translate() + ": ");
					bool flag = false;
					for (int i = 0; i < this.OwnersForReading.Count; i++)
					{
						if (flag)
						{
							stringBuilder.Append(", ");
						}
						flag = true;
						stringBuilder.Append(this.OwnersForReading[i].LabelShort);
					}
				}
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				if (this.OwnersForReading.Count == 1 && ChildcareUtility.CanSuckle(this.OwnersForReading[0], out breastfeedFailReason))
				{
					Pawn p = this.OwnersForReading[0];
					float ambientTemperature = base.AmbientTemperature;
					if (!p.SafeTemperatureRange().IncludesEpsilon(ambientTemperature))
					{
						stringBuilder.AppendInNewLine("BedUnsafeTemperature".Translate().Colorize(ColoredText.WarningColor));
					}
					else if (!p.ComfortableTemperatureRange().IncludesEpsilon(ambientTemperature))
					{
						stringBuilder.AppendInNewLine("BedUncomfortableTemperature".Translate());
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600EC27 RID: 60455 RVA: 0x0044955F File Offset: 0x0044775F
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			FloatMenuOption bedRestFloatMenuOption = this.GetBedRestFloatMenuOption(myPawn);
			if (bedRestFloatMenuOption != null)
			{
				yield return bedRestFloatMenuOption;
			}
			yield break;
		}

		// Token: 0x0600EC28 RID: 60456 RVA: 0x00449578 File Offset: 0x00447778
		public FloatMenuOption GetBedRestFloatMenuOption(Pawn myPawn)
		{
			if (!myPawn.RaceProps.Humanlike || this.ForPrisoners || !this.Medical || myPawn.Drafted || base.Faction != Faction.OfPlayer || !RestUtility.CanUseBedEver(myPawn, this.def))
			{
				return null;
			}
			if (!HealthAIUtility.ShouldSeekMedicalRest(myPawn))
			{
				if (myPawn.health.surgeryBills.AnyShouldDoNow && !WorkGiver_PatientGoToBedTreatment.AnyAvailableDoctorFor(myPawn))
				{
					return new FloatMenuOption("UseMedicalBed".Translate() + " (" + "NoDoctor".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				return new FloatMenuOption("UseMedicalBed".Translate() + " (" + "NotInjured".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else
			{
				if (myPawn.IsSlaveOfColony && !this.ForSlaves)
				{
					return new FloatMenuOption("UseMedicalBed".Translate() + " (" + "NotForSlaves".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				Action action = delegate()
				{
					if (!this.ForPrisoners && this.Medical && myPawn.CanReserveAndReach(this, PathEndMode.ClosestTouch, Danger.Deadly, this.SleepingSlotsCount, -1, null, true))
					{
						if (myPawn.CurJobDef == JobDefOf.LayDown && myPawn.CurJob.GetTarget(TargetIndex.A).Thing == this)
						{
							myPawn.CurJob.restUntilHealed = true;
						}
						else
						{
							Job job = JobMaker.MakeJob(JobDefOf.LayDown, this);
							job.restUntilHealed = true;
							myPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
						}
						myPawn.mindState.ResetLastDisturbanceTick();
					}
				};
				return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("UseMedicalBed".Translate(), action, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), myPawn, this, (this.AnyUnoccupiedSleepingSlot ? "ReservedBy" : "SomeoneElseSleeping").CapitalizeFirst(), null);
			}
		}

		// Token: 0x0600EC29 RID: 60457 RVA: 0x00449770 File Offset: 0x00447970
		public override void DrawGUIOverlay()
		{
			if (this.Medical)
			{
				return;
			}
			if (Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest && this.PlayerCanSeeOwners)
			{
				Color defaultThingLabelColor = GenMapUI.DefaultThingLabelColor;
				if (!this.OwnersForReading.Any<Pawn>())
				{
					GenMapUI.DrawThingLabel(this, "Unowned".Translate(), defaultThingLabelColor);
					return;
				}
				if (this.OwnersForReading.Count == 1)
				{
					Pawn pawn = this.OwnersForReading[0];
					if (pawn.InBed() && pawn.CurrentBed() == this)
					{
						return;
					}
					if (pawn.RaceProps.Animal && !Prefs.AnimalNameMode.ShouldDisplayAnimalName(pawn))
					{
						return;
					}
					GenMapUI.DrawThingLabel(this, pawn.LabelShort, defaultThingLabelColor);
					return;
				}
				else
				{
					for (int i = 0; i < this.OwnersForReading.Count; i++)
					{
						Pawn pawn2 = this.OwnersForReading[i];
						if (!pawn2.InBed() || this.OwnersForReading[i].CurrentBed() != this || !(pawn2.Position == this.GetSleepingSlotPos(i)))
						{
							if (pawn2.RaceProps.Animal && !Prefs.AnimalNameMode.ShouldDisplayAnimalName(pawn2))
							{
								return;
							}
							GenMapUI.DrawThingLabel(this.GetMultiOwnersLabelScreenPosFor(i), pawn2.LabelShort, defaultThingLabelColor);
						}
					}
				}
			}
		}

		// Token: 0x0600EC2A RID: 60458 RVA: 0x004498AC File Offset: 0x00447AAC
		public Pawn GetCurOccupant(int slotIndex)
		{
			if (!base.Spawned)
			{
				return null;
			}
			IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
			List<Thing> list = base.Map.thingGrid.ThingsListAt(sleepingSlotPos);
			for (int i = 0; i < list.Count; i++)
			{
				Pawn pawn = list[i] as Pawn;
				if (pawn != null && pawn.CurJob != null && pawn.GetPosture().InBed())
				{
					return pawn;
				}
			}
			return null;
		}

		// Token: 0x0600EC2B RID: 60459 RVA: 0x00449918 File Offset: 0x00447B18
		public int GetCurOccupantSlotIndex(Pawn curOccupant)
		{
			for (int i = 0; i < this.SleepingSlotsCount; i++)
			{
				if (this.GetCurOccupant(i) == curOccupant)
				{
					return i;
				}
			}
			Log.Error("Could not find pawn " + ((curOccupant != null) ? curOccupant.ToString() : null) + " on any of sleeping slots.");
			return 0;
		}

		// Token: 0x0600EC2C RID: 60460 RVA: 0x00449964 File Offset: 0x00447B64
		public Pawn GetCurOccupantAt(IntVec3 pos)
		{
			for (int i = 0; i < this.SleepingSlotsCount; i++)
			{
				if (this.GetSleepingSlotPos(i) == pos)
				{
					return this.GetCurOccupant(i);
				}
			}
			return null;
		}

		// Token: 0x0600EC2D RID: 60461 RVA: 0x0044999A File Offset: 0x00447B9A
		public IntVec3 GetSleepingSlotPos(int index)
		{
			return BedUtility.GetSleepingSlotPos(index, base.Position, base.Rotation, this.def.size);
		}

		// Token: 0x0600EC2E RID: 60462 RVA: 0x004499B9 File Offset: 0x00447BB9
		public IntVec3 GetFootSlotPos(int index)
		{
			return BedUtility.GetFeetSlotPos(index, base.Position, base.Rotation, this.def.size);
		}

		// Token: 0x0600EC2F RID: 60463 RVA: 0x004499D8 File Offset: 0x00447BD8
		public bool IsOwner(Pawn p)
		{
			int? num;
			return this.IsOwner(p, out num);
		}

		// Token: 0x0600EC30 RID: 60464 RVA: 0x004499F0 File Offset: 0x00447BF0
		public bool IsOwner(Pawn p, out int? assignedSleepingSlot)
		{
			int num = base.GetComp<CompAssignableToPawn>().AssignedPawnsForReading.IndexOf(p);
			if (num >= 0)
			{
				assignedSleepingSlot = new int?(num);
				return true;
			}
			assignedSleepingSlot = null;
			return false;
		}

		// Token: 0x0600EC31 RID: 60465 RVA: 0x00449A2C File Offset: 0x00447C2C
		private void RemoveAllOwners(bool destroyed = false)
		{
			for (int i = this.OwnersForReading.Count - 1; i >= 0; i--)
			{
				Pawn pawn = this.OwnersForReading[i];
				pawn.ownership.UnclaimBed();
				string key = "MessageBedLostAssignment";
				if (destroyed)
				{
					key = "MessageBedDestroyed";
				}
				Messages.Message(key.Translate(this.def, pawn), new LookTargets(new TargetInfo[]
				{
					this,
					pawn
				}), MessageTypeDefOf.CautionInput, false);
			}
		}

		// Token: 0x0600EC32 RID: 60466 RVA: 0x00449AC8 File Offset: 0x00447CC8
		private void NotifyRoomBedTypeChanged()
		{
			Room room = this.GetRoom(RegionType.Set_All);
			if (room != null)
			{
				room.Notify_BedTypeChanged();
			}
		}

		// Token: 0x0600EC33 RID: 60467 RVA: 0x00449AE8 File Offset: 0x00447CE8
		public void NotifyRoomAssignedPawnsChanged()
		{
			Room room = this.GetRoom(RegionType.Set_All);
			if (room != null)
			{
				room.Notify_BedTypeChanged();
			}
		}

		// Token: 0x0600EC34 RID: 60468 RVA: 0x00449B08 File Offset: 0x00447D08
		private void FacilityChanged()
		{
			CompFacility compFacility = this.TryGetComp<CompFacility>();
			CompAffectedByFacilities compAffectedByFacilities = this.TryGetComp<CompAffectedByFacilities>();
			if (compFacility != null)
			{
				compFacility.Notify_ThingChanged();
			}
			if (compAffectedByFacilities != null)
			{
				compAffectedByFacilities.Notify_ThingChanged();
			}
		}

		// Token: 0x0600EC35 RID: 60469 RVA: 0x00449B38 File Offset: 0x00447D38
		private Vector3 GetMultiOwnersLabelScreenPosFor(int slotIndex)
		{
			IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
			Vector3 drawPos = this.DrawPos;
			if (base.Rotation.IsHorizontal)
			{
				drawPos.z = (float)sleepingSlotPos.z + 0.6f;
			}
			else
			{
				drawPos.x = (float)sleepingSlotPos.x + 0.5f;
				drawPos.z += -0.4f;
			}
			Vector2 v = drawPos.MapToUIPosition();
			if (!base.Rotation.IsHorizontal && this.SleepingSlotsCount == 2)
			{
				v = this.AdjustOwnerLabelPosToAvoidOverlapping(v, slotIndex);
			}
			return v;
		}

		// Token: 0x0600EC36 RID: 60470 RVA: 0x00449BD8 File Offset: 0x00447DD8
		private Vector3 AdjustOwnerLabelPosToAvoidOverlapping(Vector3 screenPos, int slotIndex)
		{
			Text.Font = GameFont.Tiny;
			float num = Text.CalcSize(this.OwnersForReading[slotIndex].LabelShort).x + 1f;
			Vector2 vector = this.DrawPos.MapToUIPosition();
			float num2 = Mathf.Abs(screenPos.x - vector.x);
			IntVec3 sleepingSlotPos = this.GetSleepingSlotPos(slotIndex);
			if (num > num2 * 2f)
			{
				float num3;
				if (slotIndex == 0)
				{
					num3 = (float)this.GetSleepingSlotPos(1).x;
				}
				else
				{
					num3 = (float)this.GetSleepingSlotPos(0).x;
				}
				if ((float)sleepingSlotPos.x < num3)
				{
					screenPos.x -= (num - num2 * 2f) / 2f;
				}
				else
				{
					screenPos.x += (num - num2 * 2f) / 2f;
				}
			}
			return screenPos;
		}

		// Token: 0x0600EC37 RID: 60471 RVA: 0x00449CAC File Offset: 0x00447EAC
		private bool RemoveLeastDesirableInteractionCells(List<IntVec3> cells, Map map)
		{
			cells.RemoveAll(map, (Map innerMap, IntVec3 cell) => !cell.Standable(innerMap) || !TouchPathEndModeUtility.IsAdjacentOrInsideAndAllowedToTouch(cell, new LocalTargetInfo(this), map.pathing.Normal), false);
			return cells.RemoveAll_IfNotAll(map, delegate(Map innerMap, IntVec3 cell)
			{
				Building building = map.edificeGrid[cell];
				if (building == null)
				{
					return true;
				}
				ThingDef def = building.def;
				bool? flag = (def != null) ? new bool?(def.IsBed) : null;
				bool flag2 = true;
				return !(flag.GetValueOrDefault() == flag2 & flag != null);
			}) && cells.RemoveAll_IfNotAll(map, (Map innerMap, IntVec3 cell) => cell.GetDoor(map) == null);
		}

		// Token: 0x0600EC38 RID: 60472 RVA: 0x00449D1C File Offset: 0x00447F1C
		public IntVec3? FindPreferredInteractionCell(IntVec3 occupantLocation, CellSearchPattern customSearchPattern = null)
		{
			CellRect cellRect = this.OccupiedRect();
			if (!cellRect.Contains(occupantLocation))
			{
				Log.Error(string.Format("interiorLocation {0} is not within the bounds of this bed {1}.", occupantLocation, cellRect));
				return null;
			}
			Building_Bed.tmpOrderedInteractionCells.Clear();
			(customSearchPattern ?? Building_Bed.defaultBedInteractionCellsOrder).AddCellsToList(Building_Bed.tmpOrderedInteractionCells, this, cellRect, occupantLocation, base.Rotation);
			this.RemoveLeastDesirableInteractionCells(Building_Bed.tmpOrderedInteractionCells, base.Map);
			if (Building_Bed.tmpOrderedInteractionCells.Count == 0)
			{
				return null;
			}
			return new IntVec3?(Building_Bed.tmpOrderedInteractionCells[0]);
		}

		// Token: 0x17002471 RID: 9329
		// (get) Token: 0x0600EC39 RID: 60473 RVA: 0x00449DC0 File Offset: 0x00447FC0
		public override IntVec3 InteractionCell
		{
			get
			{
				IntVec3? intVec = this.FindPreferredInteractionCell(base.Position, null);
				if (intVec == null)
				{
					return base.InteractionCell;
				}
				return intVec.GetValueOrDefault();
			}
		}

		// Token: 0x04009C9D RID: 40093
		private BedOwnerType forOwnerType;

		// Token: 0x04009C9E RID: 40094
		private bool medicalInt;

		// Token: 0x04009C9F RID: 40095
		private bool alreadySetDefaultMed;

		// Token: 0x04009CA0 RID: 40096
		private static int lastBedOwnerSetChangeFrame = -1;

		// Token: 0x04009CA1 RID: 40097
		private static List<IntVec3> tmpOrderedInteractionCells = new List<IntVec3>(8);

		// Token: 0x04009CA2 RID: 40098
		private static readonly Color SheetColorNormal = new Color(0.6313726f, 0.8352941f, 0.7058824f);

		// Token: 0x04009CA3 RID: 40099
		private static readonly Color SheetColorRoyal = new Color(0.67058825f, 0.9137255f, 0.74509805f);

		// Token: 0x04009CA4 RID: 40100
		public static readonly Color SheetColorForPrisoner = new Color(1f, 0.7176471f, 0.12941177f);

		// Token: 0x04009CA5 RID: 40101
		private static readonly Color SheetColorMedical = new Color(0.3882353f, 0.62352943f, 0.8862745f);

		// Token: 0x04009CA6 RID: 40102
		private static readonly Color SheetColorMedicalForPrisoner = new Color(0.654902f, 0.3764706f, 0.15294118f);

		// Token: 0x04009CA7 RID: 40103
		private static readonly Color SheetColorForSlave = new Color32(252, 244, 3, byte.MaxValue);

		// Token: 0x04009CA8 RID: 40104
		private static readonly Color SheetColorMedicalForSlave = new Color32(153, 148, 0, byte.MaxValue);

		// Token: 0x04009CA9 RID: 40105
		private static readonly BedInteractionCellSearchPattern defaultBedInteractionCellsOrder = new BedInteractionCellSearchPattern();
	}
}
