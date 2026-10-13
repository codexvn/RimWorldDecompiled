using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003584 RID: 13700
	[DefOf]
	public static class SoundDefOf
	{
		// Token: 0x06013373 RID: 78707 RVA: 0x005B592E File Offset: 0x005B3B2E
		static SoundDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(SoundDefOf));
		}

		// Token: 0x0400C990 RID: 51600
		public static SoundDef Tick_High;

		// Token: 0x0400C991 RID: 51601
		public static SoundDef Tick_Low;

		// Token: 0x0400C992 RID: 51602
		public static SoundDef Tick_Tiny;

		// Token: 0x0400C993 RID: 51603
		public static SoundDef DragElement;

		// Token: 0x0400C994 RID: 51604
		public static SoundDef DropElement;

		// Token: 0x0400C995 RID: 51605
		public static SoundDef Crunch;

		// Token: 0x0400C996 RID: 51606
		public static SoundDef Click;

		// Token: 0x0400C997 RID: 51607
		public static SoundDef ClickReject;

		// Token: 0x0400C998 RID: 51608
		public static SoundDef CancelMode;

		// Token: 0x0400C999 RID: 51609
		public static SoundDef TabClose;

		// Token: 0x0400C99A RID: 51610
		public static SoundDef TabOpen;

		// Token: 0x0400C99B RID: 51611
		public static SoundDef Checkbox_TurnedOff;

		// Token: 0x0400C99C RID: 51612
		public static SoundDef Checkbox_TurnedOn;

		// Token: 0x0400C99D RID: 51613
		public static SoundDef RowTabSelect;

		// Token: 0x0400C99E RID: 51614
		public static SoundDef ArchitectCategorySelect;

		// Token: 0x0400C99F RID: 51615
		public static SoundDef ExecuteTrade;

		// Token: 0x0400C9A0 RID: 51616
		public static SoundDef FloatMenu_Open;

		// Token: 0x0400C9A1 RID: 51617
		public static SoundDef FloatMenu_Cancel;

		// Token: 0x0400C9A2 RID: 51618
		public static SoundDef DialogBoxAppear;

		// Token: 0x0400C9A3 RID: 51619
		public static SoundDef TutorMessageAppear;

		// Token: 0x0400C9A4 RID: 51620
		public static SoundDef TinyBell;

		// Token: 0x0400C9A5 RID: 51621
		public static SoundDef PageChange;

		// Token: 0x0400C9A6 RID: 51622
		public static SoundDef DragSlider;

		// Token: 0x0400C9A7 RID: 51623
		public static SoundDef DragGoto;

		// Token: 0x0400C9A8 RID: 51624
		public static SoundDef Lesson_Activated;

		// Token: 0x0400C9A9 RID: 51625
		public static SoundDef Lesson_Deactivated;

		// Token: 0x0400C9AA RID: 51626
		public static SoundDef DraftOn;

		// Token: 0x0400C9AB RID: 51627
		public static SoundDef DraftOff;

		// Token: 0x0400C9AC RID: 51628
		public static SoundDef Interact_RemovePaint;

		// Token: 0x0400C9AD RID: 51629
		public static SoundDef CommsWindow_Open;

		// Token: 0x0400C9AE RID: 51630
		public static SoundDef CommsWindow_Close;

		// Token: 0x0400C9AF RID: 51631
		public static SoundDef RadioComms_Ambience;

		// Token: 0x0400C9B0 RID: 51632
		public static SoundDef InfoCard_Open;

		// Token: 0x0400C9B1 RID: 51633
		public static SoundDef InfoCard_Close;

		// Token: 0x0400C9B2 RID: 51634
		public static SoundDef Clock_Stop;

		// Token: 0x0400C9B3 RID: 51635
		public static SoundDef Clock_Normal;

		// Token: 0x0400C9B4 RID: 51636
		public static SoundDef Clock_Fast;

		// Token: 0x0400C9B5 RID: 51637
		public static SoundDef Clock_Superfast;

		// Token: 0x0400C9B6 RID: 51638
		public static SoundDef Quest_Accepted;

		// Token: 0x0400C9B7 RID: 51639
		public static SoundDef Quest_Succeded;

		// Token: 0x0400C9B8 RID: 51640
		public static SoundDef Quest_Concluded;

		// Token: 0x0400C9B9 RID: 51641
		public static SoundDef Quest_Failed;

		// Token: 0x0400C9BA RID: 51642
		[MayRequireIdeology]
		public static SoundDef DislikedWorkTypeActivated;

		// Token: 0x0400C9BB RID: 51643
		[MayRequireIdeology]
		public static SoundDef ArchonexusNewColonyAccept;

		// Token: 0x0400C9BC RID: 51644
		[MayRequireIdeology]
		public static SoundDef GameStartSting_FirstArchonexusCycle;

		// Token: 0x0400C9BD RID: 51645
		[MayRequireIdeology]
		public static SoundDef GameStartSting_SecondArchonexusCycle;

		// Token: 0x0400C9BE RID: 51646
		[MayRequireBiotech]
		public static SoundDef StartRecombining;

		// Token: 0x0400C9BF RID: 51647
		public static SoundDef Mouseover_Standard;

		// Token: 0x0400C9C0 RID: 51648
		public static SoundDef Mouseover_Category;

		// Token: 0x0400C9C1 RID: 51649
		public static SoundDef Mouseover_Command;

		// Token: 0x0400C9C2 RID: 51650
		public static SoundDef Mouseover_ButtonToggle;

		// Token: 0x0400C9C3 RID: 51651
		public static SoundDef Mouseover_Tab;

		// Token: 0x0400C9C4 RID: 51652
		public static SoundDef ThingSelected;

		// Token: 0x0400C9C5 RID: 51653
		public static SoundDef MapSelected;

		// Token: 0x0400C9C6 RID: 51654
		public static SoundDef ColonistSelected;

		// Token: 0x0400C9C7 RID: 51655
		public static SoundDef ColonistOrdered;

		// Token: 0x0400C9C8 RID: 51656
		public static SoundDef LetterArrive;

		// Token: 0x0400C9C9 RID: 51657
		public static SoundDef Designate_DragStandard;

		// Token: 0x0400C9CA RID: 51658
		public static SoundDef Designate_DragStandard_Changed;

		// Token: 0x0400C9CB RID: 51659
		public static SoundDef Designate_DragStandard_Changed_NoCam;

		// Token: 0x0400C9CC RID: 51660
		public static SoundDef Designate_DragZone_Changed;

		// Token: 0x0400C9CD RID: 51661
		public static SoundDef Designate_DragBuilding;

		// Token: 0x0400C9CE RID: 51662
		public static SoundDef Designate_DragAreaAdd;

		// Token: 0x0400C9CF RID: 51663
		public static SoundDef Designate_DragAreaDelete;

		// Token: 0x0400C9D0 RID: 51664
		public static SoundDef Designate_Failed;

		// Token: 0x0400C9D1 RID: 51665
		public static SoundDef Designate_ZoneAdd;

		// Token: 0x0400C9D2 RID: 51666
		public static SoundDef Designate_ZoneAdd_Stockpile;

		// Token: 0x0400C9D3 RID: 51667
		public static SoundDef Designate_ZoneAdd_Dumping;

		// Token: 0x0400C9D4 RID: 51668
		public static SoundDef Designate_ZoneAdd_Growing;

		// Token: 0x0400C9D5 RID: 51669
		public static SoundDef Designate_ZoneAdd_Roof;

		// Token: 0x0400C9D6 RID: 51670
		public static SoundDef Designate_ZoneAdd_AllowedArea;

		// Token: 0x0400C9D7 RID: 51671
		public static SoundDef Designate_ZoneAdd_RemoveSnow;

		// Token: 0x0400C9D8 RID: 51672
		[MayRequireBiotech]
		public static SoundDef Designate_ZoneAdd_RemovePollution;

		// Token: 0x0400C9D9 RID: 51673
		public static SoundDef Designate_ZoneDelete;

		// Token: 0x0400C9DA RID: 51674
		public static SoundDef Designate_Cancel;

		// Token: 0x0400C9DB RID: 51675
		public static SoundDef Designate_Haul;

		// Token: 0x0400C9DC RID: 51676
		public static SoundDef Designate_Mine;

		// Token: 0x0400C9DD RID: 51677
		public static SoundDef Designate_SmoothSurface;

		// Token: 0x0400C9DE RID: 51678
		public static SoundDef Designate_PlanRemove;

		// Token: 0x0400C9DF RID: 51679
		public static SoundDef Designate_PlanAdd;

		// Token: 0x0400C9E0 RID: 51680
		public static SoundDef Designate_Claim;

		// Token: 0x0400C9E1 RID: 51681
		public static SoundDef Designate_Paint;

		// Token: 0x0400C9E2 RID: 51682
		public static SoundDef Designate_RemovePaint;

		// Token: 0x0400C9E3 RID: 51683
		public static SoundDef Designate_Deconstruct;

		// Token: 0x0400C9E4 RID: 51684
		public static SoundDef Designate_Hunt;

		// Token: 0x0400C9E5 RID: 51685
		public static SoundDef Designate_Slaughter;

		// Token: 0x0400C9E6 RID: 51686
		public static SoundDef Designate_PlaceBuilding;

		// Token: 0x0400C9E7 RID: 51687
		public static SoundDef Designate_CutPlants;

		// Token: 0x0400C9E8 RID: 51688
		public static SoundDef Designate_HarvestWood;

		// Token: 0x0400C9E9 RID: 51689
		public static SoundDef Designate_HarvestPlants;

		// Token: 0x0400C9EA RID: 51690
		public static SoundDef Designate_ReleaseToWild;

		// Token: 0x0400C9EB RID: 51691
		public static SoundDef Designate_ExtractTree;

		// Token: 0x0400C9EC RID: 51692
		public static SoundDef Designate_RemoveFloor;

		// Token: 0x0400C9ED RID: 51693
		public static SoundDef Designate_RemoveBridge;

		// Token: 0x0400C9EE RID: 51694
		public static SoundDef Designate_Tame;

		// Token: 0x0400C9EF RID: 51695
		public static SoundDef Designate_ExtractSkull;

		// Token: 0x0400C9F0 RID: 51696
		[MayRequireBiotech]
		public static SoundDef Designate_Adopt;

		// Token: 0x0400C9F1 RID: 51697
		public static SoundDef Standard_Drop;

		// Token: 0x0400C9F2 RID: 51698
		public static SoundDef Standard_Pickup;

		// Token: 0x0400C9F3 RID: 51699
		public static SoundDef BulletImpact_Ground;

		// Token: 0x0400C9F4 RID: 51700
		public static SoundDef Pawn_Melee_Punch_HitBuilding_Generic;

		// Token: 0x0400C9F5 RID: 51701
		public static SoundDef Ambient_AltitudeWind;

		// Token: 0x0400C9F6 RID: 51702
		public static SoundDef Ambient_Space;

		// Token: 0x0400C9F7 RID: 51703
		public static SoundDef Power_OnSmall;

		// Token: 0x0400C9F8 RID: 51704
		public static SoundDef Power_OffSmall;

		// Token: 0x0400C9F9 RID: 51705
		public static SoundDef Thunder_OnMap;

		// Token: 0x0400C9FA RID: 51706
		public static SoundDef Thunder_OffMap;

		// Token: 0x0400C9FB RID: 51707
		public static SoundDef Interact_CleanFilth;

		// Token: 0x0400C9FC RID: 51708
		public static SoundDef Interact_Sow;

		// Token: 0x0400C9FD RID: 51709
		public static SoundDef Interact_ConstructDirt;

		// Token: 0x0400C9FE RID: 51710
		public static SoundDef Interact_Tend;

		// Token: 0x0400C9FF RID: 51711
		public static SoundDef Interact_BeatFire;

		// Token: 0x0400CA00 RID: 51712
		public static SoundDef Interact_Ignite;

		// Token: 0x0400CA01 RID: 51713
		public static SoundDef Roof_Start;

		// Token: 0x0400CA02 RID: 51714
		public static SoundDef Roof_Finish;

		// Token: 0x0400CA03 RID: 51715
		public static SoundDef Finish_Wood;

		// Token: 0x0400CA04 RID: 51716
		public static SoundDef Roof_Collapse;

		// Token: 0x0400CA05 RID: 51717
		public static SoundDef PsychicPulseGlobal;

		// Token: 0x0400CA06 RID: 51718
		public static SoundDef PsychicSootheGlobal;

		// Token: 0x0400CA07 RID: 51719
		public static SoundDef GeyserSpray;

		// Token: 0x0400CA08 RID: 51720
		public static SoundDef TurretAcquireTarget;

		// Token: 0x0400CA09 RID: 51721
		public static SoundDef FlickSwitch;

		// Token: 0x0400CA0A RID: 51722
		public static SoundDef PlayBilliards;

		// Token: 0x0400CA0B RID: 51723
		public static SoundDef Building_Complete;

		// Token: 0x0400CA0C RID: 51724
		public static SoundDef Replant_Complete;

		// Token: 0x0400CA0D RID: 51725
		public static SoundDef RawMeat_Eat;

		// Token: 0x0400CA0E RID: 51726
		public static SoundDef HissSmall;

		// Token: 0x0400CA0F RID: 51727
		public static SoundDef HissJet;

		// Token: 0x0400CA10 RID: 51728
		public static SoundDef MetalHitImportant;

		// Token: 0x0400CA11 RID: 51729
		public static SoundDef Door_OpenPowered;

		// Token: 0x0400CA12 RID: 51730
		public static SoundDef Door_ClosePowered;

		// Token: 0x0400CA13 RID: 51731
		public static SoundDef Door_OpenManual;

		// Token: 0x0400CA14 RID: 51732
		public static SoundDef Door_CloseManual;

		// Token: 0x0400CA15 RID: 51733
		public static SoundDef EnergyShield_AbsorbDamage;

		// Token: 0x0400CA16 RID: 51734
		public static SoundDef EnergyShield_Reset;

		// Token: 0x0400CA17 RID: 51735
		public static SoundDef Pawn_Melee_Punch_HitPawn;

		// Token: 0x0400CA18 RID: 51736
		public static SoundDef Pawn_Melee_Punch_Miss;

		// Token: 0x0400CA19 RID: 51737
		public static SoundDef MeleeHit_Unarmed;

		// Token: 0x0400CA1A RID: 51738
		public static SoundDef Artillery_ShellLoaded;

		// Token: 0x0400CA1B RID: 51739
		public static SoundDef TechMedicineUsed;

		// Token: 0x0400CA1C RID: 51740
		public static SoundDef Building_Deconstructed;

		// Token: 0x0400CA1D RID: 51741
		public static SoundDef CryptosleepCasket_Accept;

		// Token: 0x0400CA1E RID: 51742
		public static SoundDef CryptosleepCasket_Eject;

		// Token: 0x0400CA1F RID: 51743
		public static SoundDef TrapSpring;

		// Token: 0x0400CA20 RID: 51744
		public static SoundDef TrapArm;

		// Token: 0x0400CA21 RID: 51745
		public static SoundDef FireBurning;

		// Token: 0x0400CA22 RID: 51746
		public static SoundDef Vomit;

		// Token: 0x0400CA23 RID: 51747
		public static SoundDef Roping;

		// Token: 0x0400CA24 RID: 51748
		public static SoundDef ResearchStart;

		// Token: 0x0400CA25 RID: 51749
		public static SoundDef ThingUninstalled;

		// Token: 0x0400CA26 RID: 51750
		public static SoundDef ShipTakeoff;

		// Token: 0x0400CA27 RID: 51751
		public static SoundDef Corpse_Drop;

		// Token: 0x0400CA28 RID: 51752
		public static SoundDef Tornado;

		// Token: 0x0400CA29 RID: 51753
		public static SoundDef Tunnel;

		// Token: 0x0400CA2A RID: 51754
		public static SoundDef MechanoidsWakeUp;

		// Token: 0x0400CA2B RID: 51755
		public static SoundDef FlashstormAmbience;

		// Token: 0x0400CA2C RID: 51756
		public static SoundDef Recipe_Surgery;

		// Token: 0x0400CA2D RID: 51757
		public static SoundDef Execute_Cut;

		// Token: 0x0400CA2E RID: 51758
		public static SoundDef Recipe_ButcherCorpseMechanoid;

		// Token: 0x0400CA2F RID: 51759
		public static SoundDef Explosion_FirefoamPopper;

		// Token: 0x0400CA30 RID: 51760
		public static SoundDef Psycast_Skip_Entry;

		// Token: 0x0400CA31 RID: 51761
		public static SoundDef EmergeFromWater;

		// Token: 0x0400CA32 RID: 51762
		public static SoundDef CocoonDestroyed;

		// Token: 0x0400CA33 RID: 51763
		public static SoundDef Crater_FilledIn;

		// Token: 0x0400CA34 RID: 51764
		public static SoundDef MechSerumUsed;

		// Token: 0x0400CA35 RID: 51765
		public static SoundDef ShipReactor_Startup;

		// Token: 0x0400CA36 RID: 51766
		public static SoundDef WindTurbine_Ambience;

		// Token: 0x0400CA37 RID: 51767
		public static SoundDef Bombardment_PreImpact;

		// Token: 0x0400CA38 RID: 51768
		[MayRequireIdeology]
		public static SoundDef Archotech_Invoked;

		// Token: 0x0400CA39 RID: 51769
		[MayRequireIdeology]
		public static SoundDef InsectsWakeUp;

		// Token: 0x0400CA3A RID: 51770
		[MayRequireRoyalty]
		public static SoundDef PsycastPsychicEffect;

		// Token: 0x0400CA3B RID: 51771
		[MayRequireRoyalty]
		public static SoundDef PsycastPsychicPulse;

		// Token: 0x0400CA3C RID: 51772
		[MayRequireRoyalty]
		public static SoundDef Psycast_Skip_Pulse;

		// Token: 0x0400CA3D RID: 51773
		[MayRequireRoyalty]
		public static SoundDef Psycast_Skip_Exit;

		// Token: 0x0400CA3E RID: 51774
		[MayRequireRoyalty]
		public static SoundDef PsychicEntropyOverloaded;

		// Token: 0x0400CA3F RID: 51775
		[MayRequireRoyalty]
		public static SoundDef PsychicEntropyHyperloaded;

		// Token: 0x0400CA40 RID: 51776
		[MayRequireRoyalty]
		public static SoundDef PsychicEntropyBrainCharring;

		// Token: 0x0400CA41 RID: 51777
		[MayRequireRoyalty]
		public static SoundDef PsychicEntropyBrainRoasting;

		// Token: 0x0400CA42 RID: 51778
		[MayRequireRoyalty]
		public static SoundDef MeditationGainPsyfocus;

		// Token: 0x0400CA43 RID: 51779
		[MayRequireRoyalty]
		public static SoundDef MechClusterDefeated;

		// Token: 0x0400CA44 RID: 51780
		[MayRequireRoyalty]
		public static SoundDef TechprintApplied;

		// Token: 0x0400CA45 RID: 51781
		[MayRequireRoyalty]
		public static SoundDef Bestowing_Start;

		// Token: 0x0400CA46 RID: 51782
		[MayRequireRoyalty]
		public static SoundDef Bestowing_Warmup;

		// Token: 0x0400CA47 RID: 51783
		[MayRequireRoyalty]
		public static SoundDef Bestowing_Finished;

		// Token: 0x0400CA48 RID: 51784
		[MayRequireRoyalty]
		public static SoundDef OrbitalStrike_Ordered;

		// Token: 0x0400CA49 RID: 51785
		[MayRequireRoyalty]
		public static SoundDef Broadshield_Startup;

		// Token: 0x0400CA4A RID: 51786
		[MayRequireIdeology]
		public static SoundDef Hacking_Started;

		// Token: 0x0400CA4B RID: 51787
		[MayRequireIdeology]
		public static SoundDef Hacking_InProgress;

		// Token: 0x0400CA4C RID: 51788
		[MayRequireIdeology]
		public static SoundDef Hacking_Completed;

		// Token: 0x0400CA4D RID: 51789
		[MayRequireIdeology]
		public static SoundDef Hacking_Suspended;

		// Token: 0x0400CA4E RID: 51790
		[MayRequireIdeology]
		public static SoundDef Speech_Leader_Male;

		// Token: 0x0400CA4F RID: 51791
		[MayRequireIdeology]
		public static SoundDef Speech_Leader_Female;

		// Token: 0x0400CA50 RID: 51792
		[MayRequireIdeology]
		public static SoundDef Relic_Installed;

		// Token: 0x0400CA51 RID: 51793
		[MayRequireIdeology]
		public static SoundDef Interact_Prune;

		// Token: 0x0400CA52 RID: 51794
		[MayRequireIdeology]
		public static SoundDef HairCutting;

		// Token: 0x0400CA53 RID: 51795
		[MayRequireIdeology]
		public static SoundDef Pawn_Dryad_Spawn;

		// Token: 0x0400CA54 RID: 51796
		[MayRequireIdeology]
		public static SoundDef GauranlenProductionModeSet;

		// Token: 0x0400CA55 RID: 51797
		[MayRequireIdeology]
		public static SoundDef GauranlenConnectionTorn;

		// Token: 0x0400CA56 RID: 51798
		[MayRequireIdeology]
		public static SoundDef Pawn_EnterDryadPod;

		// Token: 0x0400CA57 RID: 51799
		[MayRequireIdeology]
		public static SoundDef AncientRelicRoomReveal;

		// Token: 0x0400CA58 RID: 51800
		[MayRequireIdeology]
		public static SoundDef AncientRelicTakenAlarm;

		// Token: 0x0400CA59 RID: 51801
		[MayRequireIdeology]
		public static SoundDef ArchonexusThreatsAwakened_Alarm;

		// Token: 0x0400CA5A RID: 51802
		[MayRequireIdeology]
		public static SoundDef DuelMusic;

		// Token: 0x0400CA5B RID: 51803
		[MayRequireIdeology]
		public static SoundDef DanceParty_NoMusic;

		// Token: 0x0400CA5C RID: 51804
		[MayRequireIdeology]
		public static SoundDef DryadCasteSet;

		// Token: 0x0400CA5D RID: 51805
		[MayRequireIdeology]
		public static SoundDef Interact_ReleaseSkylantern;

		// Token: 0x0400CA5E RID: 51806
		[MayRequireIdeology]
		public static SoundDef Interact_RecolorApparel;

		// Token: 0x0400CA5F RID: 51807
		[MayRequireIdeology]
		public static SoundDef RitualSustainer_Theist;

		// Token: 0x0400CA60 RID: 51808
		[MayRequireBiotech]
		public static SoundDef DisconnectedMech;

		// Token: 0x0400CA61 RID: 51809
		[MayRequireBiotech]
		public static SoundDef MechChargerCharging;

		// Token: 0x0400CA62 RID: 51810
		[MayRequireBiotech]
		public static SoundDef GrowthVat_Open;

		// Token: 0x0400CA63 RID: 51811
		[MayRequireBiotech]
		public static SoundDef GrowthVat_Close;

		// Token: 0x0400CA64 RID: 51812
		[MayRequireBiotech]
		public static SoundDef GrowthVat_Working;

		// Token: 0x0400CA65 RID: 51813
		[MayRequireBiotech]
		public static SoundDef GeneExtractor_Working;

		// Token: 0x0400CA66 RID: 51814
		[MayRequireBiotech]
		public static SoundDef GeneAssembler_Complete;

		// Token: 0x0400CA67 RID: 51815
		[MayRequireBiotech]
		public static SoundDef ShieldMech;

		// Token: 0x0400CA68 RID: 51816
		[MayRequireBiotech]
		public static SoundDef RepairMech_Touch;

		// Token: 0x0400CA69 RID: 51817
		[MayRequireBiotech]
		public static SoundDef RepairMech_Remote;

		// Token: 0x0400CA6A RID: 51818
		[MayRequireBiotech]
		public static SoundDef MechChargerStart;

		// Token: 0x0400CA6B RID: 51819
		[MayRequireBiotech]
		public static SoundDef MechSelfShutdown;

		// Token: 0x0400CA6C RID: 51820
		[MayRequireBiotech]
		public static SoundDef Toxifier_Pollute;

		// Token: 0x0400CA6D RID: 51821
		[MayRequireBiotech]
		public static SoundDef MechGestator_Ambience;

		// Token: 0x0400CA6E RID: 51822
		[MayRequireBiotech]
		public static SoundDef MechGestatorCycle_Started;

		// Token: 0x0400CA6F RID: 51823
		[MayRequireBiotech]
		public static SoundDef MechGestatorBill_Completed;

		// Token: 0x0400CA70 RID: 51824
		[MayRequireBiotech]
		public static SoundDef MechGestator_MaterialInserted;

		// Token: 0x0400CA71 RID: 51825
		[MayRequireBiotech]
		public static SoundDef ControlMech_Complete;

		// Token: 0x0400CA72 RID: 51826
		[MayRequireBiotech]
		public static SoundDef WastepackDissolution;

		// Token: 0x0400CA73 RID: 51827
		[MayRequireBiotech]
		public static SoundDef Mechlink_Removed;

		// Token: 0x0400CA74 RID: 51828
		[MayRequireBiotech]
		public static SoundDef MechlinkCorpseReveal;

		// Token: 0x0400CA75 RID: 51829
		[MayRequireBiotech]
		public static SoundDef Bloodfeed_Cast;

		// Token: 0x0400CA76 RID: 51830
		[MayRequireAnomaly]
		public static SoundDef PitGateOpen;

		// Token: 0x0400CA77 RID: 51831
		[MayRequireAnomaly]
		public static SoundDef Pawn_Shambler_Alert;

		// Token: 0x0400CA78 RID: 51832
		[MayRequireAnomaly]
		public static SoundDef Pawn_Shambler_Rise;

		// Token: 0x0400CA79 RID: 51833
		[MayRequireAnomaly]
		public static SoundDef PitGateCollapsing;

		// Token: 0x0400CA7A RID: 51834
		[MayRequireAnomaly]
		public static SoundDef PitGateCollapsing_End;

		// Token: 0x0400CA7B RID: 51835
		[MayRequireAnomaly]
		public static SoundDef PitGateGas;

		// Token: 0x0400CA7C RID: 51836
		[MayRequireAnomaly]
		public static SoundDef UndercaveRumble;

		// Token: 0x0400CA7D RID: 51837
		[MayRequireAnomaly]
		public static SoundDef UndercaveCollapsingStage1;

		// Token: 0x0400CA7E RID: 51838
		[MayRequireAnomaly]
		public static SoundDef UndercaveCollapsingStage2;

		// Token: 0x0400CA7F RID: 51839
		[MayRequireAnomaly]
		public static SoundDef UndercaveCollapsing_End;

		// Token: 0x0400CA80 RID: 51840
		[MayRequireAnomaly]
		public static SoundDef Pawn_Fleshbeast_EmergeFromPitGate;

		// Token: 0x0400CA81 RID: 51841
		[MayRequireAnomaly]
		public static SoundDef Pawn_Sightstealer_Howl;

		// Token: 0x0400CA82 RID: 51842
		[MayRequireAnomaly]
		public static SoundDef Sightstealer_DistantHowl;

		// Token: 0x0400CA83 RID: 51843
		[MayRequireAnomaly]
		public static SoundDef Sightstealer_DistantHowlLarge;

		// Token: 0x0400CA84 RID: 51844
		[MayRequireAnomaly]
		public static SoundDef Sightstealer_SummonedHowl;

		// Token: 0x0400CA85 RID: 51845
		[MayRequireAnomaly]
		public static SoundDef Pawn_Revenant_Revealed;

		// Token: 0x0400CA86 RID: 51846
		[MayRequireAnomaly]
		public static SoundDef Pawn_Revenant_Stealth;

		// Token: 0x0400CA87 RID: 51847
		[MayRequireAnomaly]
		public static SoundDef Pawn_Revenant_Hypnotize;

		// Token: 0x0400CA88 RID: 51848
		[MayRequireAnomaly]
		public static SoundDef Pawn_Revenant_StartledScream;

		// Token: 0x0400CA89 RID: 51849
		[MayRequireAnomaly]
		public static SoundDef Pawn_SelfResurrection;

		// Token: 0x0400CA8A RID: 51850
		[MayRequireAnomaly]
		public static SoundDef PsychicBanshee;

		// Token: 0x0400CA8B RID: 51851
		[MayRequireAnomaly]
		public static SoundDef HateChant_Sighing;

		// Token: 0x0400CA8C RID: 51852
		[MayRequireAnomaly]
		public static SoundDef Pawn_HateChanter_Call;

		// Token: 0x0400CA8D RID: 51853
		[MayRequireAnomaly]
		public static SoundDef PsychicRitual_Ongoing;

		// Token: 0x0400CA8E RID: 51854
		[MayRequireAnomaly]
		public static SoundDef PsychicRitual_Ongoing_Human;

		// Token: 0x0400CA8F RID: 51855
		[MayRequireAnomaly]
		public static SoundDef PsychicRitual_Complete;

		// Token: 0x0400CA90 RID: 51856
		[MayRequireAnomaly]
		public static SoundDef PsychicRitual_Interrupted;

		// Token: 0x0400CA91 RID: 51857
		[MayRequireAnomaly]
		public static SoundDef PsychicRitual_Interrupted_Human;

		// Token: 0x0400CA92 RID: 51858
		[MayRequireAnomaly]
		public static SoundDef ChainToPlatform;

		// Token: 0x0400CA93 RID: 51859
		[MayRequireAnomaly]
		public static SoundDef ReleaseFromPlatform;

		// Token: 0x0400CA94 RID: 51860
		[MayRequireAnomaly]
		public static SoundDef ObeliskAmbientStageOne;

		// Token: 0x0400CA95 RID: 51861
		[MayRequireAnomaly]
		public static SoundDef ObeliskAmbientStageTwo;

		// Token: 0x0400CA96 RID: 51862
		[MayRequireAnomaly]
		public static SoundDef ObeliskAmbientStageThree;

		// Token: 0x0400CA97 RID: 51863
		[MayRequireAnomaly]
		public static SoundDef FleshmassHeart_Throb;

		// Token: 0x0400CA98 RID: 51864
		[MayRequireAnomaly]
		public static SoundDef FleshbeastDigging;

		// Token: 0x0400CA99 RID: 51865
		[MayRequireAnomaly]
		public static SoundDef Pawn_Chimera_SpeedupRoar;

		// Token: 0x0400CA9A RID: 51866
		[MayRequireAnomaly]
		public static SoundDef Noctolith_Destroyed;

		// Token: 0x0400CA9B RID: 51867
		[MayRequireAnomaly]
		public static SoundDef FleshmassAmbience;

		// Token: 0x0400CA9C RID: 51868
		[MayRequireAnomaly]
		public static SoundDef FleshmassBirth;

		// Token: 0x0400CA9D RID: 51869
		[MayRequireAnomaly]
		public static SoundDef DarknessDamage;

		// Token: 0x0400CA9E RID: 51870
		[MayRequireAnomaly]
		public static SoundDef TraversePitGate;

		// Token: 0x0400CA9F RID: 51871
		[MayRequireAnomaly]
		public static SoundDef Pawn_Awoken_Hypnotize;

		// Token: 0x0400CAA0 RID: 51872
		[MayRequireAnomaly]
		public static SoundDef Pawn_Devourer_Digesting;

		// Token: 0x0400CAA1 RID: 51873
		[MayRequireAnomaly]
		public static SoundDef Pawn_Nociosphere_Arc;

		// Token: 0x0400CAA2 RID: 51874
		[MayRequireAnomaly]
		public static SoundDef Pawn_Nociosphere_Departing;

		// Token: 0x0400CAA3 RID: 51875
		[MayRequireAnomaly]
		public static SoundDef VoidMonolith_InspectLoop;

		// Token: 0x0400CAA4 RID: 51876
		[MayRequireAnomaly]
		public static SoundDef PitBurrow_Collapse;

		// Token: 0x0400CAA5 RID: 51877
		[MayRequireAnomaly]
		public static SoundDef VoidStructure_AmbientPreActivate;

		// Token: 0x0400CAA6 RID: 51878
		[MayRequireAnomaly]
		public static SoundDef VoidStructure_AmbientPostActivate;

		// Token: 0x0400CAA7 RID: 51879
		[MayRequireAnomaly]
		public static SoundDef VoidNode_Ambient;

		// Token: 0x0400CAA8 RID: 51880
		[MayRequireAnomaly]
		public static SoundDef VoidNode_Explode;

		// Token: 0x0400CAA9 RID: 51881
		[MayRequireAnomaly]
		public static SoundDef BioferriteHarvester_Ambient;

		// Token: 0x0400CAAA RID: 51882
		[MayRequireAnomaly]
		public static SoundDef EntityChainHigh;

		// Token: 0x0400CAAB RID: 51883
		[MayRequireAnomaly]
		public static SoundDef EntityChainLow;

		// Token: 0x0400CAAC RID: 51884
		[MayRequireOdyssey]
		public static SoundDef CerebrexCore_Pain;

		// Token: 0x0400CAAD RID: 51885
		[MayRequireOdyssey]
		public static SoundDef DroneTrapSpring;

		// Token: 0x0400CAAE RID: 51886
		[MayRequireOdyssey]
		public static SoundDef Interact_CatchFish;

		// Token: 0x0400CAAF RID: 51887
		[MayRequireOdyssey]
		public static SoundDef Gravship_Launch;

		// Token: 0x0400CAB0 RID: 51888
		[MayRequireOdyssey]
		public static SoundDef Gravship_Engine_Start;

		// Token: 0x0400CAB1 RID: 51889
		[MayRequireOdyssey]
		public static SoundDef Gravship_Engine_Loop;

		// Token: 0x0400CAB2 RID: 51890
		[MayRequireOdyssey]
		public static SoundDef Gravship_Antigrav_Loop;

		// Token: 0x0400CAB3 RID: 51891
		[MayRequireOdyssey]
		public static SoundDef Gravship_Land;

		// Token: 0x0400CAB4 RID: 51892
		[MayRequireOdyssey]
		public static SoundDef Gravship_RetroThruster_Loop;

		// Token: 0x0400CAB5 RID: 51893
		[MayRequireOdyssey]
		public static SoundDef Gravship_RetroThruster_Oneshot_01;

		// Token: 0x0400CAB6 RID: 51894
		[MayRequireOdyssey]
		public static SoundDef Gravship_RetroThruster_Oneshot_02;

		// Token: 0x0400CAB7 RID: 51895
		[MayRequireOdyssey]
		public static SoundDef Gravship_RetroThruster_Oneshot_03;

		// Token: 0x0400CAB8 RID: 51896
		[MayRequireOdyssey]
		public static SoundDef Gravship_LandingServos;

		// Token: 0x0400CAB9 RID: 51897
		[MayRequireOdyssey]
		public static SoundDef Gravship_Touchdown;

		// Token: 0x0400CABA RID: 51898
		[MayRequireOdyssey]
		public static SoundDef Gravship_Engine_Stop;

		// Token: 0x0400CABB RID: 51899
		public static SoundDef GameStartSting;

		// Token: 0x0400CABC RID: 51900
		public static SoundDef PlanetkillerImpact;
	}
}
