using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003583 RID: 13699
	[DefOf]
	public static class ThingDefOf
	{
		// Token: 0x06013372 RID: 78706 RVA: 0x005B591D File Offset: 0x005B3B1D
		static ThingDefOf()
		{
			DefOfHelper.EnsureInitializedInCtor(typeof(ThingDefOf));
		}

		// Token: 0x0400C743 RID: 51011
		public static ThingDef Silver;

		// Token: 0x0400C744 RID: 51012
		public static ThingDef Gold;

		// Token: 0x0400C745 RID: 51013
		public static ThingDef Steel;

		// Token: 0x0400C746 RID: 51014
		public static ThingDef WoodLog;

		// Token: 0x0400C747 RID: 51015
		public static ThingDef MedicineHerbal;

		// Token: 0x0400C748 RID: 51016
		public static ThingDef MedicineIndustrial;

		// Token: 0x0400C749 RID: 51017
		public static ThingDef MedicineUltratech;

		// Token: 0x0400C74A RID: 51018
		public static ThingDef BlocksGranite;

		// Token: 0x0400C74B RID: 51019
		public static ThingDef Plasteel;

		// Token: 0x0400C74C RID: 51020
		public static ThingDef Beer;

		// Token: 0x0400C74D RID: 51021
		public static ThingDef Chocolate;

		// Token: 0x0400C74E RID: 51022
		public static ThingDef ComponentIndustrial;

		// Token: 0x0400C74F RID: 51023
		public static ThingDef ComponentSpacer;

		// Token: 0x0400C750 RID: 51024
		public static ThingDef InsectJelly;

		// Token: 0x0400C751 RID: 51025
		public static ThingDef Cloth;

		// Token: 0x0400C752 RID: 51026
		public static ThingDef Leather_Plain;

		// Token: 0x0400C753 RID: 51027
		public static ThingDef RawPotatoes;

		// Token: 0x0400C754 RID: 51028
		public static ThingDef Granite;

		// Token: 0x0400C755 RID: 51029
		public static ThingDef Wort;

		// Token: 0x0400C756 RID: 51030
		public static ThingDef AIPersonaCore;

		// Token: 0x0400C757 RID: 51031
		public static ThingDef Chemfuel;

		// Token: 0x0400C758 RID: 51032
		public static ThingDef Uranium;

		// Token: 0x0400C759 RID: 51033
		public static ThingDef Jade;

		// Token: 0x0400C75A RID: 51034
		public static ThingDef Shell_HighExplosive;

		// Token: 0x0400C75B RID: 51035
		public static ThingDef Shell_AntigrainWarhead;

		// Token: 0x0400C75C RID: 51036
		public static ThingDef ReinforcedBarrel;

		// Token: 0x0400C75D RID: 51037
		[MayRequireAnomaly]
		public static ThingDef Bioferrite;

		// Token: 0x0400C75E RID: 51038
		[MayRequireAnomaly]
		public static ThingDef Leather_Dread;

		// Token: 0x0400C75F RID: 51039
		[MayRequireOdyssey]
		public static ThingDef GravlitePanel;

		// Token: 0x0400C760 RID: 51040
		public static ThingDef MealSurvivalPack;

		// Token: 0x0400C761 RID: 51041
		public static ThingDef MealNutrientPaste;

		// Token: 0x0400C762 RID: 51042
		public static ThingDef MealSimple;

		// Token: 0x0400C763 RID: 51043
		public static ThingDef MealFine;

		// Token: 0x0400C764 RID: 51044
		public static ThingDef Pemmican;

		// Token: 0x0400C765 RID: 51045
		public static ThingDef Kibble;

		// Token: 0x0400C766 RID: 51046
		public static ThingDef Hay;

		// Token: 0x0400C767 RID: 51047
		public static ThingDef Meat_Human;

		// Token: 0x0400C768 RID: 51048
		[MayRequireBiotech]
		public static ThingDef BabyFood;

		// Token: 0x0400C769 RID: 51049
		public static ThingDef SmokeleafJoint;

		// Token: 0x0400C76A RID: 51050
		public static ThingDef Flake;

		// Token: 0x0400C76B RID: 51051
		public static ThingDef Yayo;

		// Token: 0x0400C76C RID: 51052
		public static ThingDef GoJuice;

		// Token: 0x0400C76D RID: 51053
		public static ThingDef WakeUp;

		// Token: 0x0400C76E RID: 51054
		public static ThingDef Luciferium;

		// Token: 0x0400C76F RID: 51055
		public static ThingDef Penoxycyline;

		// Token: 0x0400C770 RID: 51056
		public static ThingDef DropPodIncoming;

		// Token: 0x0400C771 RID: 51057
		public static ThingDef DropPodLeaving;

		// Token: 0x0400C772 RID: 51058
		public static ThingDef ShipChunkIncoming;

		// Token: 0x0400C773 RID: 51059
		public static ThingDef ShipChunkIncoming_SmallExplosion;

		// Token: 0x0400C774 RID: 51060
		public static ThingDef CrashedShipPartIncoming;

		// Token: 0x0400C775 RID: 51061
		public static ThingDef MeteoriteIncoming;

		// Token: 0x0400C776 RID: 51062
		public static ThingDef MeteoriteCraterIncoming;

		// Token: 0x0400C777 RID: 51063
		public static ThingDef FlyerArrival;

		// Token: 0x0400C778 RID: 51064
		public static ThingDef FlyerLeaving;

		// Token: 0x0400C779 RID: 51065
		[MayRequireRoyalty]
		public static ThingDef ShuttleIncoming;

		// Token: 0x0400C77A RID: 51066
		[MayRequireRoyalty]
		public static ThingDef ShuttleCrashing;

		// Token: 0x0400C77B RID: 51067
		[MayRequireIdeology]
		public static ThingDef SpacedroneIncoming;

		// Token: 0x0400C77C RID: 51068
		[MayRequireAnomaly]
		public static ThingDef WarpedObeliskIncoming;

		// Token: 0x0400C77D RID: 51069
		[MayRequireAnomaly]
		public static ThingDef NoctolithIncoming;

		// Token: 0x0400C77E RID: 51070
		[MayRequireAnomaly]
		public static ThingDef IncineratorSpray;

		// Token: 0x0400C77F RID: 51071
		[MayRequireAnomaly]
		public static ThingDef DyingRevenant;

		// Token: 0x0400C780 RID: 51072
		[MayRequireAnomaly]
		public static ThingDef VoidStructureIncoming;

		// Token: 0x0400C781 RID: 51073
		[MayRequireAnomaly]
		public static ThingDef MonolithIncoming;

		// Token: 0x0400C782 RID: 51074
		[MayRequireOdyssey]
		public static ThingDef LavaRockIncoming;

		// Token: 0x0400C783 RID: 51075
		[MayRequireOdyssey]
		public static ThingDef OrbitalDebrisSpawner;

		// Token: 0x0400C784 RID: 51076
		public static ThingDef PawnFlyer;

		// Token: 0x0400C785 RID: 51077
		[MayRequireAnomaly]
		public static ThingDef PawnFlyer_Stun;

		// Token: 0x0400C786 RID: 51078
		[MayRequireAnomaly]
		public static ThingDef PawnFlyer_ConsumeLeap;

		// Token: 0x0400C787 RID: 51079
		public static ThingDef ActiveDropPod;

		// Token: 0x0400C788 RID: 51080
		public static ThingDef Fire;

		// Token: 0x0400C789 RID: 51081
		public static ThingDef ChunkSlagSteel;

		// Token: 0x0400C78A RID: 51082
		public static ThingDef SteamGeyser;

		// Token: 0x0400C78B RID: 51083
		public static ThingDef Hive;

		// Token: 0x0400C78C RID: 51084
		public static ThingDef ShipChunk;

		// Token: 0x0400C78D RID: 51085
		public static ThingDef GlowPod;

		// Token: 0x0400C78E RID: 51086
		public static ThingDef MinifiedThing;

		// Token: 0x0400C78F RID: 51087
		public static ThingDef MinifiedThingRealtimeDraw;

		// Token: 0x0400C790 RID: 51088
		public static ThingDef MinifiedTree;

		// Token: 0x0400C791 RID: 51089
		public static ThingDef Dye;

		// Token: 0x0400C792 RID: 51090
		public static ThingDef Skull;

		// Token: 0x0400C793 RID: 51091
		public static ThingDef TextBook;

		// Token: 0x0400C794 RID: 51092
		public static ThingDef Schematic;

		// Token: 0x0400C795 RID: 51093
		public static ThingDef Novel;

		// Token: 0x0400C796 RID: 51094
		public static ThingDef BurningPowerCell;

		// Token: 0x0400C797 RID: 51095
		public static ThingDef VitalsMonitor;

		// Token: 0x0400C798 RID: 51096
		public static ThingDef CraterSmall;

		// Token: 0x0400C799 RID: 51097
		public static ThingDef CraterMedium;

		// Token: 0x0400C79A RID: 51098
		public static ThingDef CraterLarge;

		// Token: 0x0400C79B RID: 51099
		public static ThingDef Slate;

		// Token: 0x0400C79C RID: 51100
		public static ThingDef Gloomlight;

		// Token: 0x0400C79D RID: 51101
		[MayRequireOdyssey]
		public static ThingDef MechPylon;

		// Token: 0x0400C79E RID: 51102
		public static ThingDef Vent;

		// Token: 0x0400C79F RID: 51103
		public static ThingDef MeleeWeapon_Knife;

		// Token: 0x0400C7A0 RID: 51104
		public static ThingDef AncientSpacerCrate;

		// Token: 0x0400C7A1 RID: 51105
		[MayRequireRoyalty]
		public static ThingDef MonumentMarker;

		// Token: 0x0400C7A2 RID: 51106
		[MayRequireRoyalty]
		public static ThingDef PsychicAmplifier;

		// Token: 0x0400C7A3 RID: 51107
		[MayRequireIdeology]
		public static ThingDef Skullspike;

		// Token: 0x0400C7A4 RID: 51108
		[MayRequireBiotech]
		public static ThingDef Mechlink;

		// Token: 0x0400C7A5 RID: 51109
		[MayRequireBiotech]
		public static ThingDef HemogenPack;

		// Token: 0x0400C7A6 RID: 51110
		[MayRequireBiotech]
		public static ThingDef Genepack;

		// Token: 0x0400C7A7 RID: 51111
		[MayRequireBiotech]
		public static ThingDef Xenogerm;

		// Token: 0x0400C7A8 RID: 51112
		[MayRequireBiotech]
		public static ThingDef Wastepack;

		// Token: 0x0400C7A9 RID: 51113
		[MayRequireBiotech]
		public static ThingDef Shell_Toxic;

		// Token: 0x0400C7AA RID: 51114
		[MayRequireAnomaly]
		public static ThingDef Shard;

		// Token: 0x0400C7AB RID: 51115
		[MayRequireAnomaly]
		public static ThingDef Meat_Twisted;

		// Token: 0x0400C7AC RID: 51116
		[MayRequireAnomaly]
		public static ThingDef GoldenCube;

		// Token: 0x0400C7AD RID: 51117
		[MayRequireAnomaly]
		public static ThingDef Tome;

		// Token: 0x0400C7AE RID: 51118
		[MayRequireAnomaly]
		public static ThingDef GrayFleshSample;

		// Token: 0x0400C7AF RID: 51119
		[MayRequireAnomaly]
		public static ThingDef RevenantSpine;

		// Token: 0x0400C7B0 RID: 51120
		[MayRequireAnomaly]
		public static ThingDef FleshmassNeuralLump;

		// Token: 0x0400C7B1 RID: 51121
		[MayRequireAnomaly]
		public static ThingDef RevenantFleshChunk;

		// Token: 0x0400C7B2 RID: 51122
		[MayRequireAnomaly]
		public static ThingDef GrayStatueDeadlifeDust;

		// Token: 0x0400C7B3 RID: 51123
		[MayRequireAnomaly]
		public static ThingDef GrayStatueTeleporter;

		// Token: 0x0400C7B4 RID: 51124
		[MayRequireAnomaly]
		public static ThingDef MonolithFragment;

		// Token: 0x0400C7B5 RID: 51125
		[MayRequireOdyssey]
		public static ThingDef GravEngine;

		// Token: 0x0400C7B6 RID: 51126
		[MayRequireOdyssey]
		public static ThingDef GravFieldExtender;

		// Token: 0x0400C7B7 RID: 51127
		[MayRequireOdyssey]
		public static ThingDef ChemfuelTank;

		// Token: 0x0400C7B8 RID: 51128
		[MayRequireOdyssey]
		public static ThingDef SmallThruster;

		// Token: 0x0400C7B9 RID: 51129
		[MayRequireOdyssey]
		public static ThingDef LargeThruster;

		// Token: 0x0400C7BA RID: 51130
		[MayRequireOdyssey]
		public static ThingDef PilotConsole;

		// Token: 0x0400C7BB RID: 51131
		[MayRequireOdyssey]
		public static ThingDef Gravcore;

		// Token: 0x0400C7BC RID: 51132
		[MayRequireOdyssey]
		public static ThingDef ShipChunk_Mech;

		// Token: 0x0400C7BD RID: 51133
		[MayRequireOdyssey]
		public static ThingDef RubblePile;

		// Token: 0x0400C7BE RID: 51134
		[MayRequireOdyssey]
		public static ThingDef MechRelay;

		// Token: 0x0400C7BF RID: 51135
		[MayRequireOdyssey]
		public static ThingDef MechStabilizer;

		// Token: 0x0400C7C0 RID: 51136
		[MayRequireOdyssey]
		public static ThingDef GeothermalVent;

		// Token: 0x0400C7C1 RID: 51137
		[MayRequireOdyssey]
		public static ThingDef SolidIce;

		// Token: 0x0400C7C2 RID: 51138
		[MayRequireOdyssey]
		public static ThingDef AncientSmokeVent;

		// Token: 0x0400C7C3 RID: 51139
		[MayRequireOdyssey]
		public static ThingDef AncientToxVent;

		// Token: 0x0400C7C4 RID: 51140
		[MayRequireOdyssey]
		public static ThingDef AncientHeatVent;

		// Token: 0x0400C7C5 RID: 51141
		[MayRequireOdyssey]
		public static ThingDef AncientDestroyedConsole;

		// Token: 0x0400C7C6 RID: 51142
		[MayRequireOdyssey]
		public static ThingDef AncientEmergencyLight_Red;

		// Token: 0x0400C7C7 RID: 51143
		[MayRequireOdyssey]
		public static ThingDef AncientEmergencyLight_Blue;

		// Token: 0x0400C7C8 RID: 51144
		[MayRequireOdyssey]
		public static ThingDef AncientEmergencyLight_Green;

		// Token: 0x0400C7C9 RID: 51145
		[MayRequireOdyssey]
		public static ThingDef Turret_AncientArmoredTurret;

		// Token: 0x0400C7CA RID: 51146
		[MayRequireOdyssey]
		public static ThingDef AncientGravReactor;

		// Token: 0x0400C7CB RID: 51147
		[MayRequireOdyssey]
		public static ThingDef AncientSecurityTerminal;

		// Token: 0x0400C7CC RID: 51148
		[MayRequireOdyssey]
		public static ThingDef AncientGravEngine;

		// Token: 0x0400C7CD RID: 51149
		[MayRequireOdyssey]
		public static ThingDef AncientTransportPod;

		// Token: 0x0400C7CE RID: 51150
		[MayRequireOdyssey]
		public static ThingDef MalfunctioningTransportPod;

		// Token: 0x0400C7CF RID: 51151
		[MayRequireOdyssey]
		public static ThingDef LifeSupportUnit;

		// Token: 0x0400C7D0 RID: 51152
		[MayRequireOdyssey]
		public static ThingDef AncientTerraformer;

		// Token: 0x0400C7D1 RID: 51153
		[MayRequireOdyssey]
		public static ThingDef CerebrexCore;

		// Token: 0x0400C7D2 RID: 51154
		[MayRequireOdyssey]
		public static ThingDef CerebrexCore_Destroyed;

		// Token: 0x0400C7D3 RID: 51155
		[MayRequireOdyssey]
		public static ThingDef CerebrexStabilizer;

		// Token: 0x0400C7D4 RID: 51156
		[MayRequireOdyssey]
		public static ThingDef MechanoidProcessor;

		// Token: 0x0400C7D5 RID: 51157
		[MayRequireOdyssey]
		public static ThingDef Vacstone;

		// Token: 0x0400C7D6 RID: 51158
		[MayRequireOdyssey]
		public static ThingDef HunterDroneTrap;

		// Token: 0x0400C7D7 RID: 51159
		[MayRequireOdyssey]
		public static ThingDef WaspDroneTrap;

		// Token: 0x0400C7D8 RID: 51160
		[MayRequireOdyssey]
		public static ThingDef PassengerShuttle;

		// Token: 0x0400C7D9 RID: 51161
		[MayRequireOdyssey]
		public static ThingDef SurveyScanner;

		// Token: 0x0400C7DA RID: 51162
		[MayRequireOdyssey]
		public static ThingDef Map;

		// Token: 0x0400C7DB RID: 51163
		[MayRequireOdyssey]
		public static ThingDef SealedCrate;

		// Token: 0x0400C7DC RID: 51164
		[MayRequireOdyssey]
		public static ThingDef ChunkVacstone;

		// Token: 0x0400C7DD RID: 51165
		[MayRequire("Ludeon.RimWorld.Royalty,Ludeon.RimWorld.Biotech")]
		public static ThingDef Fish_Toxfish;

		// Token: 0x0400C7DE RID: 51166
		public static ThingDef Filth_Blood;

		// Token: 0x0400C7DF RID: 51167
		public static ThingDef Filth_BloodSmear;

		// Token: 0x0400C7E0 RID: 51168
		public static ThingDef Filth_AmnioticFluid;

		// Token: 0x0400C7E1 RID: 51169
		public static ThingDef Filth_Dirt;

		// Token: 0x0400C7E2 RID: 51170
		public static ThingDef Filth_Vomit;

		// Token: 0x0400C7E3 RID: 51171
		public static ThingDef Filth_AnimalFilth;

		// Token: 0x0400C7E4 RID: 51172
		public static ThingDef Filth_Trash;

		// Token: 0x0400C7E5 RID: 51173
		public static ThingDef Filth_Slime;

		// Token: 0x0400C7E6 RID: 51174
		public static ThingDef Filth_FireFoam;

		// Token: 0x0400C7E7 RID: 51175
		public static ThingDef Filth_Fuel;

		// Token: 0x0400C7E8 RID: 51176
		public static ThingDef Filth_RubbleRock;

		// Token: 0x0400C7E9 RID: 51177
		public static ThingDef Filth_RubbleBuilding;

		// Token: 0x0400C7EA RID: 51178
		public static ThingDef Filth_CorpseBile;

		// Token: 0x0400C7EB RID: 51179
		public static ThingDef Filth_BloodInsect;

		// Token: 0x0400C7EC RID: 51180
		public static ThingDef Filth_Ash;

		// Token: 0x0400C7ED RID: 51181
		public static ThingDef Filth_MachineBits;

		// Token: 0x0400C7EE RID: 51182
		public static ThingDef Filth_Water;

		// Token: 0x0400C7EF RID: 51183
		public static ThingDef Filth_Hair;

		// Token: 0x0400C7F0 RID: 51184
		public static ThingDef Filth_BlastMark;

		// Token: 0x0400C7F1 RID: 51185
		public static ThingDef Filth_GestationFluid;

		// Token: 0x0400C7F2 RID: 51186
		public static ThingDef Filth_LooseGround;

		// Token: 0x0400C7F3 RID: 51187
		[MayRequireIdeology]
		public static ThingDef Filth_DriedBlood;

		// Token: 0x0400C7F4 RID: 51188
		[MayRequireIdeology]
		public static ThingDef Filth_ScatteredDocuments;

		// Token: 0x0400C7F5 RID: 51189
		[MayRequireIdeology]
		public static ThingDef Filth_MoldyUniform;

		// Token: 0x0400C7F6 RID: 51190
		[MayRequireIdeology]
		public static ThingDef Filth_PodSlime;

		// Token: 0x0400C7F7 RID: 51191
		[MayRequireIdeology]
		public static ThingDef Filth_OilSmear;

		// Token: 0x0400C7F8 RID: 51192
		[MayRequireBiotech]
		public static ThingDef Filth_Floordrawing;

		// Token: 0x0400C7F9 RID: 51193
		[MayRequireBiotech]
		public static ThingDef Filth_FlammableBile;

		// Token: 0x0400C7FA RID: 51194
		[MayRequireAnomaly]
		public static ThingDef Filth_RevenantSmear;

		// Token: 0x0400C7FB RID: 51195
		[MayRequireAnomaly]
		public static ThingDef Filth_RevenantBloodPool;

		// Token: 0x0400C7FC RID: 51196
		[MayRequireAnomaly]
		public static ThingDef Filth_GrayFleshNoticeable;

		// Token: 0x0400C7FD RID: 51197
		[MayRequireAnomaly]
		public static ThingDef Filth_GrayFlesh;

		// Token: 0x0400C7FE RID: 51198
		[MayRequireAnomaly]
		public static ThingDef Filth_TwistedFlesh;

		// Token: 0x0400C7FF RID: 51199
		[MayRequireAnomaly]
		public static ThingDef FloorEtching;

		// Token: 0x0400C800 RID: 51200
		[MayRequireAnomaly]
		public static ThingDef FloorEtchingRambling;

		// Token: 0x0400C801 RID: 51201
		[MayRequireAnomaly]
		public static ThingDef GrayBox;

		// Token: 0x0400C802 RID: 51202
		[MayRequireAnomaly]
		public static ThingDef Filth_MetalhorrorDebris;

		// Token: 0x0400C803 RID: 51203
		public static ThingDef RectTrigger;

		// Token: 0x0400C804 RID: 51204
		public static ThingDef TriggerUnfogged;

		// Token: 0x0400C805 RID: 51205
		public static ThingDef TriggerContainerEmptied;

		// Token: 0x0400C806 RID: 51206
		public static ThingDef Explosion;

		// Token: 0x0400C807 RID: 51207
		public static ThingDef Bombardment;

		// Token: 0x0400C808 RID: 51208
		public static ThingDef PowerBeam;

		// Token: 0x0400C809 RID: 51209
		public static ThingDef SignalAction_Letter;

		// Token: 0x0400C80A RID: 51210
		public static ThingDef SignalAction_Ambush;

		// Token: 0x0400C80B RID: 51211
		public static ThingDef SignalAction_OpenCasket;

		// Token: 0x0400C80C RID: 51212
		public static ThingDef SignalAction_OpenDoor;

		// Token: 0x0400C80D RID: 51213
		public static ThingDef SignalAction_Message;

		// Token: 0x0400C80E RID: 51214
		public static ThingDef SignalAction_Infestation;

		// Token: 0x0400C80F RID: 51215
		public static ThingDef SignalAction_SoundOneShot;

		// Token: 0x0400C810 RID: 51216
		public static ThingDef SignalAction_DormancyWakeUp;

		// Token: 0x0400C811 RID: 51217
		public static ThingDef SignalAction_Incident;

		// Token: 0x0400C812 RID: 51218
		public static ThingDef SignalAction_StartWick;

		// Token: 0x0400C813 RID: 51219
		public static ThingDef Blight;

		// Token: 0x0400C814 RID: 51220
		public static ThingDef TunnelHiveSpawner;

		// Token: 0x0400C815 RID: 51221
		public static ThingDef PawnGroundSpawner;

		// Token: 0x0400C816 RID: 51222
		[MayRequireRoyalty]
		public static ThingDef Flashstorm;

		// Token: 0x0400C817 RID: 51223
		public static ThingDef RadialTrigger;

		// Token: 0x0400C818 RID: 51224
		[MayRequireIdeology]
		public static ThingDef TunnelJellySpawner;

		// Token: 0x0400C819 RID: 51225
		[MayRequireBiotech]
		public static ThingDef MechShield;

		// Token: 0x0400C81A RID: 51226
		[MayRequireBiotech]
		public static ThingDef CocoonSpawner;

		// Token: 0x0400C81B RID: 51227
		[MayRequireAnomaly]
		public static ThingDef PitGateSpawner;

		// Token: 0x0400C81C RID: 51228
		[MayRequireAnomaly]
		public static ThingDef FleshmassHeartSpawner;

		// Token: 0x0400C81D RID: 51229
		[MayRequireAnomaly]
		public static ThingDef PitBurrowSpawner;

		// Token: 0x0400C81E RID: 51230
		[MayRequireAnomaly]
		public static ThingDef MetalHellFloorCracks;

		// Token: 0x0400C81F RID: 51231
		[MayRequireAnomaly]
		public static ThingDef MetalHellFloorMarkings;

		// Token: 0x0400C820 RID: 51232
		[MayRequireAnomaly]
		public static ThingDef MetalHellAmbience;

		// Token: 0x0400C821 RID: 51233
		[MayRequireOdyssey]
		public static ThingDef LavaEmergence;

		// Token: 0x0400C822 RID: 51234
		[MayRequireOdyssey]
		public static ThingDef LavaEmergenceImmediate;

		// Token: 0x0400C823 RID: 51235
		[MayRequireOdyssey]
		public static ThingDef SeasonalFlood;

		// Token: 0x0400C824 RID: 51236
		[MayRequireOdyssey]
		public static ThingDef TorrentialRainFlood;

		// Token: 0x0400C825 RID: 51237
		[MayRequireOdyssey]
		public static ThingDef InsectAmbush;

		// Token: 0x0400C826 RID: 51238
		[MayRequireOdyssey]
		public static ThingDef GravshipLandingMarker;

		// Token: 0x0400C827 RID: 51239
		[MayRequireOdyssey]
		public static ThingDef Blueprint_Substructure;

		// Token: 0x0400C828 RID: 51240
		[MayRequireOdyssey]
		public static ThingDef Frame_Substructure;

		// Token: 0x0400C829 RID: 51241
		public static ThingDef Sandstone;

		// Token: 0x0400C82A RID: 51242
		public static ThingDef Ship_Beam;

		// Token: 0x0400C82B RID: 51243
		public static ThingDef Ship_Reactor;

		// Token: 0x0400C82C RID: 51244
		public static ThingDef Ship_CryptosleepCasket;

		// Token: 0x0400C82D RID: 51245
		public static ThingDef Ship_ComputerCore;

		// Token: 0x0400C82E RID: 51246
		public static ThingDef Ship_Engine;

		// Token: 0x0400C82F RID: 51247
		public static ThingDef Ship_SensorCluster;

		// Token: 0x0400C830 RID: 51248
		public static ThingDef MineableSteel;

		// Token: 0x0400C831 RID: 51249
		public static ThingDef MineableComponentsIndustrial;

		// Token: 0x0400C832 RID: 51250
		public static ThingDef MineableGold;

		// Token: 0x0400C833 RID: 51251
		public static ThingDef RaisedRocks;

		// Token: 0x0400C834 RID: 51252
		[MayRequireOdyssey]
		public static ThingDef BurrowWall;

		// Token: 0x0400C835 RID: 51253
		[MayRequireAnomaly]
		public static ThingDef Fleshmass;

		// Token: 0x0400C836 RID: 51254
		[MayRequireAnomaly]
		public static ThingDef Fleshmass_Active;

		// Token: 0x0400C837 RID: 51255
		[MayRequireAnomaly]
		public static ThingDef FleshSack;

		// Token: 0x0400C838 RID: 51256
		[MayRequireAnomaly]
		public static ThingDef FleshmassHeart;

		// Token: 0x0400C839 RID: 51257
		[MayRequireAnomaly]
		public static ThingDef Fleshbulb;

		// Token: 0x0400C83A RID: 51258
		[MayRequireAnomaly]
		public static ThingDef NerveBundle;

		// Token: 0x0400C83B RID: 51259
		[MayRequireAnomaly]
		public static ThingDef FleshmassSpitter;

		// Token: 0x0400C83C RID: 51260
		[MayRequireOdyssey]
		public static ThingDef MineableObsidian;

		// Token: 0x0400C83D RID: 51261
		public static ThingDef Door;

		// Token: 0x0400C83E RID: 51262
		public static ThingDef Wall;

		// Token: 0x0400C83F RID: 51263
		public static ThingDef Bed;

		// Token: 0x0400C840 RID: 51264
		public static ThingDef Bedroll;

		// Token: 0x0400C841 RID: 51265
		public static ThingDef SleepingSpot;

		// Token: 0x0400C842 RID: 51266
		public static ThingDef Stool;

		// Token: 0x0400C843 RID: 51267
		public static ThingDef OrbitalTradeBeacon;

		// Token: 0x0400C844 RID: 51268
		public static ThingDef NutrientPasteDispenser;

		// Token: 0x0400C845 RID: 51269
		public static ThingDef Grave;

		// Token: 0x0400C846 RID: 51270
		public static ThingDef Sandbags;

		// Token: 0x0400C847 RID: 51271
		public static ThingDef Barricade;

		// Token: 0x0400C848 RID: 51272
		public static ThingDef AncientCryptosleepCasket;

		// Token: 0x0400C849 RID: 51273
		[MayRequireBiotech]
		public static ThingDef DeathrestCasket;

		// Token: 0x0400C84A RID: 51274
		public static ThingDef SolarGenerator;

		// Token: 0x0400C84B RID: 51275
		public static ThingDef WoodFiredGenerator;

		// Token: 0x0400C84C RID: 51276
		public static ThingDef ChemfuelPoweredGenerator;

		// Token: 0x0400C84D RID: 51277
		public static ThingDef PowerConduit;

		// Token: 0x0400C84E RID: 51278
		public static ThingDef HiddenConduit;

		// Token: 0x0400C84F RID: 51279
		public static ThingDef Battery;

		// Token: 0x0400C850 RID: 51280
		public static ThingDef GeothermalGenerator;

		// Token: 0x0400C851 RID: 51281
		public static ThingDef WatermillGenerator;

		// Token: 0x0400C852 RID: 51282
		public static ThingDef Hopper;

		// Token: 0x0400C853 RID: 51283
		public static ThingDef BilliardsTable;

		// Token: 0x0400C854 RID: 51284
		public static ThingDef MarriageSpot;

		// Token: 0x0400C855 RID: 51285
		public static ThingDef PartySpot;

		// Token: 0x0400C856 RID: 51286
		public static ThingDef TransportPod;

		// Token: 0x0400C857 RID: 51287
		[MayRequireRoyalty]
		public static ThingDef MeditationSpot;

		// Token: 0x0400C858 RID: 51288
		[MayRequireIdeology]
		public static ThingDef RitualSpot;

		// Token: 0x0400C859 RID: 51289
		[MayRequireIdeology]
		public static ThingDef Ideogram;

		// Token: 0x0400C85A RID: 51290
		public static ThingDef RoyalBed;

		// Token: 0x0400C85B RID: 51291
		public static ThingDef TrapSpike;

		// Token: 0x0400C85C RID: 51292
		public static ThingDef Cooler;

		// Token: 0x0400C85D RID: 51293
		public static ThingDef Heater;

		// Token: 0x0400C85E RID: 51294
		public static ThingDef Snowman;

		// Token: 0x0400C85F RID: 51295
		public static ThingDef WindTurbine;

		// Token: 0x0400C860 RID: 51296
		public static ThingDef FermentingBarrel;

		// Token: 0x0400C861 RID: 51297
		public static ThingDef DeepDrill;

		// Token: 0x0400C862 RID: 51298
		public static ThingDef GroundPenetratingScanner;

		// Token: 0x0400C863 RID: 51299
		public static ThingDef CollapsedRocks;

		// Token: 0x0400C864 RID: 51300
		public static ThingDef TorchLamp;

		// Token: 0x0400C865 RID: 51301
		public static ThingDef StandingLamp;

		// Token: 0x0400C866 RID: 51302
		public static ThingDef WallLamp;

		// Token: 0x0400C867 RID: 51303
		public static ThingDef Campfire;

		// Token: 0x0400C868 RID: 51304
		public static ThingDef FirefoamPopper;

		// Token: 0x0400C869 RID: 51305
		public static ThingDef PassiveCooler;

		// Token: 0x0400C86A RID: 51306
		public static ThingDef CaravanPackingSpot;

		// Token: 0x0400C86B RID: 51307
		public static ThingDef Table1x2c;

		// Token: 0x0400C86C RID: 51308
		public static ThingDef Table2x2c;

		// Token: 0x0400C86D RID: 51309
		public static ThingDef Table2x4c;

		// Token: 0x0400C86E RID: 51310
		public static ThingDef TableButcher;

		// Token: 0x0400C86F RID: 51311
		public static ThingDef DiningChair;

		// Token: 0x0400C870 RID: 51312
		public static ThingDef Shelf;

		// Token: 0x0400C871 RID: 51313
		public static ThingDef ShelfSmall;

		// Token: 0x0400C872 RID: 51314
		public static ThingDef PsychicEmanator;

		// Token: 0x0400C873 RID: 51315
		public static ThingDef CommsConsole;

		// Token: 0x0400C874 RID: 51316
		public static ThingDef Sarcophagus;

		// Token: 0x0400C875 RID: 51317
		public static ThingDef Turret_Mortar;

		// Token: 0x0400C876 RID: 51318
		public static ThingDef Turret_MiniTurret;

		// Token: 0x0400C877 RID: 51319
		public static ThingDef Turret_AutoMiniTurret;

		// Token: 0x0400C878 RID: 51320
		[MayRequireRoyalty]
		public static ThingDef MechCapsule;

		// Token: 0x0400C879 RID: 51321
		public static ThingDef DefoliatorShipPart;

		// Token: 0x0400C87A RID: 51322
		public static ThingDef PsychicDronerShipPart;

		// Token: 0x0400C87B RID: 51323
		public static ThingDef Column;

		// Token: 0x0400C87C RID: 51324
		public static ThingDef Urn;

		// Token: 0x0400C87D RID: 51325
		public static ThingDef SteleLarge;

		// Token: 0x0400C87E RID: 51326
		public static ThingDef SteleGrand;

		// Token: 0x0400C87F RID: 51327
		public static ThingDef EggBox;

		// Token: 0x0400C880 RID: 51328
		[MayRequireRoyalty]
		public static ThingDef Shuttle;

		// Token: 0x0400C881 RID: 51329
		[MayRequireRoyalty]
		public static ThingDef Drape;

		// Token: 0x0400C882 RID: 51330
		[MayRequireRoyalty]
		public static ThingDef ActivatorProximity;

		// Token: 0x0400C883 RID: 51331
		[MayRequireRoyalty]
		public static ThingDef ShieldGeneratorBullets;

		// Token: 0x0400C884 RID: 51332
		[MayRequireRoyalty]
		public static ThingDef ShieldGeneratorMortar;

		// Token: 0x0400C885 RID: 51333
		public static ThingDef Turret_AutoChargeBlaster;

		// Token: 0x0400C886 RID: 51334
		public static ThingDef Turret_AutoInferno;

		// Token: 0x0400C887 RID: 51335
		[MayRequireRoyalty]
		public static ThingDef BroadshieldProjector;

		// Token: 0x0400C888 RID: 51336
		[MayRequireRoyalty]
		public static ThingDef Brazier;

		// Token: 0x0400C889 RID: 51337
		[MayRequireRoyalty]
		public static ThingDef ShuttleCrashed;

		// Token: 0x0400C88A RID: 51338
		[MayRequireBiotech]
		public static ThingDef ShuttleCrashed_Exitable;

		// Token: 0x0400C88B RID: 51339
		[MayRequireBiotech]
		public static ThingDef ShuttleCrashed_Exitable_Mechanitor;

		// Token: 0x0400C88C RID: 51340
		[MayRequireIdeology]
		public static ThingDef StylingStation;

		// Token: 0x0400C88D RID: 51341
		[MayRequireIdeology]
		public static ThingDef Spacedrone;

		// Token: 0x0400C88E RID: 51342
		[MayRequireIdeology]
		public static ThingDef Reliquary;

		// Token: 0x0400C88F RID: 51343
		[MayRequireIdeology]
		public static ThingDef Lectern;

		// Token: 0x0400C890 RID: 51344
		[MayRequireIdeology]
		public static ThingDef AncientTerminal;

		// Token: 0x0400C891 RID: 51345
		[MayRequireIdeology]
		public static ThingDef AncientTerminal_Worshipful;

		// Token: 0x0400C892 RID: 51346
		public static ThingDef AncientEquipmentBlocks;

		// Token: 0x0400C893 RID: 51347
		public static ThingDef AncientMachine;

		// Token: 0x0400C894 RID: 51348
		public static ThingDef AncientStorageCylinder;

		// Token: 0x0400C895 RID: 51349
		[MayRequireIdeology]
		public static ThingDef AncientCommsConsole;

		// Token: 0x0400C896 RID: 51350
		public static ThingDef AncientHermeticCrate;

		// Token: 0x0400C897 RID: 51351
		[MayRequireIdeology]
		public static ThingDef LightBall;

		// Token: 0x0400C898 RID: 51352
		[MayRequireIdeology]
		public static ThingDef Loudspeaker;

		// Token: 0x0400C899 RID: 51353
		[MayRequireIdeology]
		public static ThingDef Drum;

		// Token: 0x0400C89A RID: 51354
		public static ThingDef AncientLamp;

		// Token: 0x0400C89B RID: 51355
		[MayRequireIdeology]
		public static ThingDef SkyLantern;

		// Token: 0x0400C89C RID: 51356
		[MayRequireIdeology]
		public static ThingDef ArchonexusCore;

		// Token: 0x0400C89D RID: 51357
		[MayRequireIdeology]
		public static ThingDef MajorArchotechStructure;

		// Token: 0x0400C89E RID: 51358
		[MayRequireIdeology]
		public static ThingDef MajorArchotechStructureStudiable;

		// Token: 0x0400C89F RID: 51359
		[MayRequireIdeology]
		public static ThingDef ArchotechTower;

		// Token: 0x0400C8A0 RID: 51360
		[MayRequireIdeology]
		public static ThingDef GrandArchotechStructure;

		// Token: 0x0400C8A1 RID: 51361
		public static ThingDef AncientLockerBank;

		// Token: 0x0400C8A2 RID: 51362
		public static ThingDef AncientLargeCrate;

		// Token: 0x0400C8A3 RID: 51363
		public static ThingDef AncientCrate;

		// Token: 0x0400C8A4 RID: 51364
		public static ThingDef AncientFence;

		// Token: 0x0400C8A5 RID: 51365
		public static ThingDef AncientRazorWire;

		// Token: 0x0400C8A6 RID: 51366
		public static ThingDef AncientMegaCannonTripod;

		// Token: 0x0400C8A7 RID: 51367
		public static ThingDef AncientMegaCannonBarrel;

		// Token: 0x0400C8A8 RID: 51368
		public static ThingDef ChunkMechanoidSlag;

		// Token: 0x0400C8A9 RID: 51369
		public static ThingDef AncientBed;

		// Token: 0x0400C8AA RID: 51370
		public static ThingDef AncientBarrel;

		// Token: 0x0400C8AB RID: 51371
		public static ThingDef AncientLargeContainer;

		// Token: 0x0400C8AC RID: 51372
		public static ThingDef AncientSystemRack;

		// Token: 0x0400C8AD RID: 51373
		public static ThingDef AncientMicrowave;

		// Token: 0x0400C8AE RID: 51374
		public static ThingDef AncientRefrigerator;

		// Token: 0x0400C8AF RID: 51375
		public static ThingDef AncientOven;

		// Token: 0x0400C8B0 RID: 51376
		public static ThingDef AncientStove;

		// Token: 0x0400C8B1 RID: 51377
		public static ThingDef AncientKitchenSink;

		// Token: 0x0400C8B2 RID: 51378
		public static ThingDef AncientOperatingTable;

		// Token: 0x0400C8B3 RID: 51379
		public static ThingDef AncientMechDropBeacon;

		// Token: 0x0400C8B4 RID: 51380
		public static ThingDef AncientTankTrap;

		// Token: 0x0400C8B5 RID: 51381
		public static ThingDef AncientRustedCar;

		// Token: 0x0400C8B6 RID: 51382
		public static ThingDef AncientRustedJeep;

		// Token: 0x0400C8B7 RID: 51383
		public static ThingDef AncientGenerator;

		// Token: 0x0400C8B8 RID: 51384
		public static ThingDef AncientDisplayBank;

		// Token: 0x0400C8B9 RID: 51385
		public static ThingDef AncientSecurityTurret;

		// Token: 0x0400C8BA RID: 51386
		public static ThingDef AncientRustedCarFrame;

		// Token: 0x0400C8BB RID: 51387
		public static ThingDef AncientRustedEngineBlock;

		// Token: 0x0400C8BC RID: 51388
		public static ThingDef AncientLargeRustedEngineBlock;

		// Token: 0x0400C8BD RID: 51389
		[MayRequireIdeology]
		public static ThingDef BiosculpterPod;

		// Token: 0x0400C8BE RID: 51390
		[MayRequireIdeology]
		public static ThingDef NeuralSupercharger;

		// Token: 0x0400C8BF RID: 51391
		public static ThingDef AncientWarwalkerTorso;

		// Token: 0x0400C8C0 RID: 51392
		public static ThingDef AncientWarwalkerClaw;

		// Token: 0x0400C8C1 RID: 51393
		public static ThingDef AncientWarwalkerLeg;

		// Token: 0x0400C8C2 RID: 51394
		public static ThingDef AncientMiniWarwalkerRemains;

		// Token: 0x0400C8C3 RID: 51395
		public static ThingDef AncientWarspiderRemains;

		// Token: 0x0400C8C4 RID: 51396
		public static ThingDef AncientShipBeacon;

		// Token: 0x0400C8C5 RID: 51397
		[MayRequireIdeology]
		public static ThingDef AncientSecurityCrate;

		// Token: 0x0400C8C6 RID: 51398
		public static ThingDef AncientCryptosleepPod;

		// Token: 0x0400C8C7 RID: 51399
		[MayRequireIdeology]
		public static ThingDef AncientEnemyTerminal;

		// Token: 0x0400C8C8 RID: 51400
		public static ThingDef AncientFuelNode;

		// Token: 0x0400C8C9 RID: 51401
		public static ThingDef AncientPodCar;

		// Token: 0x0400C8CA RID: 51402
		public static ThingDef AncientHydrant;

		// Token: 0x0400C8CB RID: 51403
		public static ThingDef AncientTank;

		// Token: 0x0400C8CC RID: 51404
		[MayRequireBiotech]
		public static ThingDef AncientExostriderRemains;

		// Token: 0x0400C8CD RID: 51405
		public static ThingDef AncientWarwalkerFoot;

		// Token: 0x0400C8CE RID: 51406
		public static ThingDef AncientMechGestatorTank;

		// Token: 0x0400C8CF RID: 51407
		public static ThingDef AncientLargeMechGestator;

		// Token: 0x0400C8D0 RID: 51408
		public static ThingDef AncientBasicRecharger;

		// Token: 0x0400C8D1 RID: 51409
		public static ThingDef AncientStandardRecharger;

		// Token: 0x0400C8D2 RID: 51410
		[MayRequireBiotech]
		public static ThingDef AncientToxifierGenerator;

		// Token: 0x0400C8D3 RID: 51411
		[MayRequireBiotech]
		public static ThingDef AncientBandNode;

		// Token: 0x0400C8D4 RID: 51412
		[MayRequireBiotech]
		public static ThingDef GeneAssembler;

		// Token: 0x0400C8D5 RID: 51413
		[MayRequireBiotech]
		public static ThingDef GeneExtractor;

		// Token: 0x0400C8D6 RID: 51414
		[MayRequireBiotech]
		public static ThingDef GrowthVat;

		// Token: 0x0400C8D7 RID: 51415
		[MayRequireBiotech]
		public static ThingDef HumanEmbryo;

		// Token: 0x0400C8D8 RID: 51416
		[MayRequireBiotech]
		public static ThingDef HumanOvum;

		// Token: 0x0400C8D9 RID: 51417
		[MayRequireBiotech]
		public static ThingDef ArchiteCapsule;

		// Token: 0x0400C8DA RID: 51418
		[MayRequireBiotech]
		public static ThingDef ToyBox;

		// Token: 0x0400C8DB RID: 51419
		[MayRequireBiotech]
		public static ThingDef SchoolDesk;

		// Token: 0x0400C8DC RID: 51420
		[MayRequireBiotech]
		public static ThingDef Blackboard;

		// Token: 0x0400C8DD RID: 51421
		[MayRequireBiotech]
		public static ThingDef BandNode;

		// Token: 0x0400C8DE RID: 51422
		[MayRequireBiotech]
		public static ThingDef BabyDecoration;

		// Token: 0x0400C8DF RID: 51423
		public static ThingDef SimpleResearchBench;

		// Token: 0x0400C8E0 RID: 51424
		public static ThingDef HydroponicsBasin;

		// Token: 0x0400C8E1 RID: 51425
		[MayRequireBiotech]
		public static ThingDef ToxifierGenerator;

		// Token: 0x0400C8E2 RID: 51426
		[MayRequireBiotech]
		public static ThingDef SanguphageMeetingTorch;

		// Token: 0x0400C8E3 RID: 51427
		[MayRequireBiotech]
		public static ThingDef SubcoreEncoder;

		// Token: 0x0400C8E4 RID: 51428
		[MayRequireBiotech]
		public static ThingDef GeneBank;

		// Token: 0x0400C8E5 RID: 51429
		[MayRequireOdyssey]
		public static ThingDef MechanoidDropPod;

		// Token: 0x0400C8E6 RID: 51430
		[MayRequireAnomaly]
		public static ThingDef PitGate;

		// Token: 0x0400C8E7 RID: 51431
		[MayRequireAnomaly]
		public static ThingDef CaveExit;

		// Token: 0x0400C8E8 RID: 51432
		[MayRequireAnomaly]
		public static ThingDef PitBurrow;

		// Token: 0x0400C8E9 RID: 51433
		[MayRequireAnomaly]
		public static ThingDef Electroharvester;

		// Token: 0x0400C8EA RID: 51434
		[MayRequireAnomaly]
		public static ThingDef ElectricInhibitor;

		// Token: 0x0400C8EB RID: 51435
		[MayRequireAnomaly]
		public static ThingDef PsychicRitualSpot;

		// Token: 0x0400C8EC RID: 51436
		[MayRequireAnomaly]
		public static ThingDef WarpedObelisk_Mutator;

		// Token: 0x0400C8ED RID: 51437
		[MayRequireAnomaly]
		public static ThingDef WarpedObelisk_Duplicator;

		// Token: 0x0400C8EE RID: 51438
		[MayRequireAnomaly]
		public static ThingDef WarpedObelisk_Abductor;

		// Token: 0x0400C8EF RID: 51439
		[MayRequireAnomaly]
		public static ThingDef WarpedObelisk_Labyrinth;

		// Token: 0x0400C8F0 RID: 51440
		[MayRequireAnomaly]
		public static ThingDef Noctolith;

		// Token: 0x0400C8F1 RID: 51441
		[MayRequireAnomaly]
		public static ThingDef DirtCubeSculpture;

		// Token: 0x0400C8F2 RID: 51442
		[MayRequireAnomaly]
		public static ThingDef StoneCubeSculpture;

		// Token: 0x0400C8F3 RID: 51443
		[MayRequireAnomaly]
		public static ThingDef SandCubeSculpture;

		// Token: 0x0400C8F4 RID: 51444
		[MayRequireAnomaly]
		public static ThingDef ScrapCubeSculpture;

		// Token: 0x0400C8F5 RID: 51445
		[MayRequireAnomaly]
		public static ThingDef VoidStructure;

		// Token: 0x0400C8F6 RID: 51446
		[MayRequireAnomaly]
		public static ThingDef VoidmetalWall;

		// Token: 0x0400C8F7 RID: 51447
		[MayRequireAnomaly]
		public static ThingDef VoidmetalMassSmall;

		// Token: 0x0400C8F8 RID: 51448
		[MayRequireAnomaly]
		public static ThingDef VoidmetalMassMedium;

		// Token: 0x0400C8F9 RID: 51449
		[MayRequireAnomaly]
		public static ThingDef VoidmetalMassLarge;

		// Token: 0x0400C8FA RID: 51450
		[MayRequireAnomaly]
		public static ThingDef GrayDoor;

		// Token: 0x0400C8FB RID: 51451
		[MayRequireAnomaly]
		public static ThingDef GrayWall;

		// Token: 0x0400C8FC RID: 51452
		[MayRequireAnomaly]
		public static ThingDef LabyrinthMatter;

		// Token: 0x0400C8FD RID: 51453
		[MayRequireAnomaly]
		public static ThingDef VoidNode;

		// Token: 0x0400C8FE RID: 51454
		[MayRequireAnomaly]
		public static ThingDef VoidMonolith;

		// Token: 0x0400C8FF RID: 51455
		[MayRequireAnomaly]
		public static ThingDef TwistedArchotechSupport_Small;

		// Token: 0x0400C900 RID: 51456
		[MayRequireAnomaly]
		public static ThingDef TwistedArchotechSupport_Large;

		// Token: 0x0400C901 RID: 51457
		[MayRequireAnomaly]
		public static ThingDef TwistedArchotechSupport_Vertical;

		// Token: 0x0400C902 RID: 51458
		[MayRequireAnomaly]
		public static ThingDef FleshbeastGroundSpawner;

		// Token: 0x0400C903 RID: 51459
		[MayRequireAnomaly]
		public static ThingDef SecurityDoor;

		// Token: 0x0400C904 RID: 51460
		[MayRequireAnomaly]
		public static ThingDef BioferriteHarvester;

		// Token: 0x0400C905 RID: 51461
		[MayRequireAnomaly]
		public static ThingDef AtmosphericHeater;

		// Token: 0x0400C906 RID: 51462
		[MayRequireAnomaly]
		public static ThingDef Mote_RitualCandleA;

		// Token: 0x0400C907 RID: 51463
		[MayRequireAnomaly]
		public static ThingDef Mote_RitualCandleB;

		// Token: 0x0400C908 RID: 51464
		[MayRequireAnomaly]
		public static ThingDef Mote_RitualCandleC;

		// Token: 0x0400C909 RID: 51465
		[MayRequireAnomaly]
		public static ThingDef Mote_IncineratorBurst;

		// Token: 0x0400C90A RID: 51466
		[MayRequireAnomaly]
		public static ThingDef SleepSuppressor;

		// Token: 0x0400C90B RID: 51467
		[MayRequireAnomaly]
		public static ThingDef BioferriteGenerator;

		// Token: 0x0400C90C RID: 51468
		[MayRequireAnomaly]
		public static ThingDef EndlessPit2x2c;

		// Token: 0x0400C90D RID: 51469
		[MayRequireAnomaly]
		public static ThingDef EndlessPit3x2c;

		// Token: 0x0400C90E RID: 51470
		[MayRequireAnomaly]
		public static ThingDef EndlessPit3x3c;

		// Token: 0x0400C90F RID: 51471
		[MayRequireOdyssey]
		public static ThingDef InsectLairEntrance;

		// Token: 0x0400C910 RID: 51472
		[MayRequireOdyssey]
		public static ThingDef AncientFortifiedWall;

		// Token: 0x0400C911 RID: 51473
		[MayRequireOdyssey]
		public static ThingDef OrbitalAncientFortifiedWall;

		// Token: 0x0400C912 RID: 51474
		[MayRequireOdyssey]
		public static ThingDef AncientBlastDoor;

		// Token: 0x0400C913 RID: 51475
		[MayRequireOdyssey]
		public static ThingDef EggSac;

		// Token: 0x0400C914 RID: 51476
		[MayRequireOdyssey]
		public static ThingDef AncientHatch;

		// Token: 0x0400C915 RID: 51477
		[MayRequireOdyssey]
		public static ThingDef AncientHatchExit;

		// Token: 0x0400C916 RID: 51478
		[MayRequireOdyssey]
		public static ThingDef AncientExplosivesCrate;

		// Token: 0x0400C917 RID: 51479
		[MayRequireOdyssey]
		public static ThingDef Statue;

		// Token: 0x0400C918 RID: 51480
		[MayRequireOdyssey]
		public static ThingDef GaussCannon;

		// Token: 0x0400C919 RID: 51481
		[MayRequireOdyssey]
		public static ThingDef AnticraftBeam;

		// Token: 0x0400C91A RID: 51482
		[MayRequireOdyssey]
		public static ThingDef GravshipHull;

		// Token: 0x0400C91B RID: 51483
		[MayRequireOdyssey]
		public static ThingDef GravAnchor;

		// Token: 0x0400C91C RID: 51484
		public static ThingDef Plant_Potato;

		// Token: 0x0400C91D RID: 51485
		public static ThingDef Plant_TreeOak;

		// Token: 0x0400C91E RID: 51486
		public static ThingDef Plant_TreePoplar;

		// Token: 0x0400C91F RID: 51487
		public static ThingDef Plant_TreePine;

		// Token: 0x0400C920 RID: 51488
		public static ThingDef Plant_TreePalm;

		// Token: 0x0400C921 RID: 51489
		public static ThingDef Plant_Alocasia;

		// Token: 0x0400C922 RID: 51490
		public static ThingDef Plant_TreeBirch;

		// Token: 0x0400C923 RID: 51491
		public static ThingDef Plant_Grass;

		// Token: 0x0400C924 RID: 51492
		public static ThingDef Plant_Ambrosia;

		// Token: 0x0400C925 RID: 51493
		public static ThingDef Plant_Dandelion;

		// Token: 0x0400C926 RID: 51494
		public static ThingDef Glowstool;

		// Token: 0x0400C927 RID: 51495
		public static ThingDef Bryolux;

		// Token: 0x0400C928 RID: 51496
		public static ThingDef Agarilux;

		// Token: 0x0400C929 RID: 51497
		public static ThingDef Plant_HealrootWild;

		// Token: 0x0400C92A RID: 51498
		[MayRequireRoyalty]
		public static ThingDef Plant_TreeAnima;

		// Token: 0x0400C92B RID: 51499
		[MayRequireRoyalty]
		public static ThingDef AnimusStone;

		// Token: 0x0400C92C RID: 51500
		[MayRequireRoyalty]
		public static ThingDef NatureShrine_Small;

		// Token: 0x0400C92D RID: 51501
		[MayRequireRoyalty]
		public static ThingDef NatureShrine_Large;

		// Token: 0x0400C92E RID: 51502
		[MayRequireRoyalty]
		public static ThingDef Plant_GrassAnima;

		// Token: 0x0400C92F RID: 51503
		[MayRequireIdeology]
		public static ThingDef Plant_Nutrifungus;

		// Token: 0x0400C930 RID: 51504
		[MayRequireIdeology]
		public static ThingDef Plant_PodGauranlen;

		// Token: 0x0400C931 RID: 51505
		[MayRequireIdeology]
		public static ThingDef Plant_TreeGauranlen;

		// Token: 0x0400C932 RID: 51506
		[MayRequireIdeology]
		public static ThingDef DryadCocoon;

		// Token: 0x0400C933 RID: 51507
		[MayRequireIdeology]
		public static ThingDef DryadHealingPod;

		// Token: 0x0400C934 RID: 51508
		[MayRequireIdeology]
		public static ThingDef GaumakerCocoon;

		// Token: 0x0400C935 RID: 51509
		[MayRequireIdeology]
		public static ThingDef Plant_Fibercorn;

		// Token: 0x0400C936 RID: 51510
		[MayRequireBiotech]
		public static ThingDef Plant_Toxipotato;

		// Token: 0x0400C937 RID: 51511
		[MayRequireBiotech]
		public static ThingDef Plant_RatPalm;

		// Token: 0x0400C938 RID: 51512
		[MayRequireBiotech]
		public static ThingDef Plant_GrayGrass;

		// Token: 0x0400C939 RID: 51513
		[MayRequireAnomaly]
		public static ThingDef Plant_TreeHarbinger;

		// Token: 0x0400C93A RID: 51514
		[MayRequireOdyssey]
		public static ThingDef Boomshroom;

		// Token: 0x0400C93B RID: 51515
		[MayRequireOdyssey]
		public static ThingDef Plant_Psilocap;

		// Token: 0x0400C93C RID: 51516
		[MayRequireOdyssey]
		public static ThingDef Plant_TreeArchean;

		// Token: 0x0400C93D RID: 51517
		public static ThingDef Human;

		// Token: 0x0400C93E RID: 51518
		public static ThingDef Muffalo;

		// Token: 0x0400C93F RID: 51519
		public static ThingDef Dromedary;

		// Token: 0x0400C940 RID: 51520
		public static ThingDef Cow;

		// Token: 0x0400C941 RID: 51521
		public static ThingDef Goat;

		// Token: 0x0400C942 RID: 51522
		public static ThingDef Chicken;

		// Token: 0x0400C943 RID: 51523
		public static ThingDef Thrumbo;

		// Token: 0x0400C944 RID: 51524
		[MayRequireBiotech]
		public static ThingDef Mech_Apocriton;

		// Token: 0x0400C945 RID: 51525
		[MayRequireBiotech]
		public static ThingDef Mech_Constructoid;

		// Token: 0x0400C946 RID: 51526
		[MayRequireAnomaly]
		public static ThingDef Fingerspike;

		// Token: 0x0400C947 RID: 51527
		[MayRequireAnomaly]
		public static ThingDef Metalhorror;

		// Token: 0x0400C948 RID: 51528
		[MayRequireAnomaly]
		public static ThingDef CreepJoiner;

		// Token: 0x0400C949 RID: 51529
		public static ThingDef Spark;

		// Token: 0x0400C94A RID: 51530
		[MayRequireAnomaly]
		public static ThingDef DisruptorFlare;

		// Token: 0x0400C94B RID: 51531
		public static ThingDef Apparel_ShieldBelt;

		// Token: 0x0400C94C RID: 51532
		public static ThingDef Apparel_Parka;

		// Token: 0x0400C94D RID: 51533
		public static ThingDef Apparel_Tuque;

		// Token: 0x0400C94E RID: 51534
		[MayRequireIdeology]
		public static ThingDef Apparel_BodyStrap;

		// Token: 0x0400C94F RID: 51535
		[MayRequireIdeology]
		public static ThingDef Apparel_Collar;

		// Token: 0x0400C950 RID: 51536
		[MayRequireBiotech]
		public static ThingDef Apparel_GasMask;

		// Token: 0x0400C951 RID: 51537
		[MayRequireOdyssey]
		public static ThingDef Apparel_VacsuitHelmet;

		// Token: 0x0400C952 RID: 51538
		[MayRequireOdyssey]
		public static ThingDef Apparel_Vacsuit;

		// Token: 0x0400C953 RID: 51539
		[MayRequireOdyssey]
		public static ThingDef Apparel_CerebrexNode;

		// Token: 0x0400C954 RID: 51540
		public static ThingDef Mote_Text;

		// Token: 0x0400C955 RID: 51541
		public static ThingDef Mote_TempRoof;

		// Token: 0x0400C956 RID: 51542
		public static ThingDef Mote_ColonistFleeing;

		// Token: 0x0400C957 RID: 51543
		public static ThingDef Mote_ColonistAttacking;

		// Token: 0x0400C958 RID: 51544
		public static ThingDef Mote_Speech;

		// Token: 0x0400C959 RID: 51545
		public static ThingDef Mote_ThoughtBad;

		// Token: 0x0400C95A RID: 51546
		public static ThingDef Mote_ThoughtGood;

		// Token: 0x0400C95B RID: 51547
		public static ThingDef Mote_Stun;

		// Token: 0x0400C95C RID: 51548
		public static ThingDef Mote_Bombardment;

		// Token: 0x0400C95D RID: 51549
		public static ThingDef Mote_PowerBeam;

		// Token: 0x0400C95E RID: 51550
		public static ThingDef Mote_Leaf;

		// Token: 0x0400C95F RID: 51551
		public static ThingDef Mote_ForceJob;

		// Token: 0x0400C960 RID: 51552
		public static ThingDef Mote_ForceJobMaintained;

		// Token: 0x0400C961 RID: 51553
		[MayRequireRoyalty]
		public static ThingDef Mote_CastPsycast;

		// Token: 0x0400C962 RID: 51554
		[MayRequireRoyalty]
		public static ThingDef Mote_PsychicLinkLine;

		// Token: 0x0400C963 RID: 51555
		[MayRequireRoyalty]
		public static ThingDef Mote_PsychicLinkPulse;

		// Token: 0x0400C964 RID: 51556
		[MayRequireRoyalty]
		public static ThingDef Mote_PsyfocusPulse;

		// Token: 0x0400C965 RID: 51557
		[MayRequireRoyalty]
		public static ThingDef Mote_Bestow;

		// Token: 0x0400C966 RID: 51558
		[MayRequireIdeology]
		public static ThingDef Mote_LightBall;

		// Token: 0x0400C967 RID: 51559
		[MayRequireIdeology]
		public static ThingDef Mote_LightBallLights;

		// Token: 0x0400C968 RID: 51560
		[MayRequireIdeology]
		public static ThingDef Mote_LoudspeakerLights;

		// Token: 0x0400C969 RID: 51561
		[MayRequireIdeology]
		public static ThingDef Mote_RolePositionHighlight;

		// Token: 0x0400C96A RID: 51562
		[MayRequireIdeology]
		public static ThingDef Mote_RolePawnHighlight;

		// Token: 0x0400C96B RID: 51563
		[MayRequireIdeology]
		public static ThingDef Mote_GauranlenCasteChanged;

		// Token: 0x0400C96C RID: 51564
		[MayRequireBiotech]
		public static ThingDef Mote_ResurrectFlash;

		// Token: 0x0400C96D RID: 51565
		[MayRequireBiotech]
		public static ThingDef Mote_PollutionPump;

		// Token: 0x0400C96E RID: 51566
		[MayRequireBiotech]
		public static ThingDef Mote_MechCharging;

		// Token: 0x0400C96F RID: 51567
		[MayRequireBiotech]
		public static ThingDef Mote_ChargingCablesPulse;

		// Token: 0x0400C970 RID: 51568
		[MayRequireBiotech]
		public static ThingDef SoftScannerGlow_North;

		// Token: 0x0400C971 RID: 51569
		[MayRequireBiotech]
		public static ThingDef SoftScannerGlow_East;

		// Token: 0x0400C972 RID: 51570
		[MayRequireBiotech]
		public static ThingDef SoftScannerGlow_West;

		// Token: 0x0400C973 RID: 51571
		[MayRequireBiotech]
		public static ThingDef SoftScannerGlow_South;

		// Token: 0x0400C974 RID: 51572
		[MayRequireBiotech]
		public static ThingDef RipScannerGlow_South;

		// Token: 0x0400C975 RID: 51573
		[MayRequireBiotech]
		public static ThingDef RipScannerGlow_North;

		// Token: 0x0400C976 RID: 51574
		[MayRequireBiotech]
		public static ThingDef RipScannerGlow_East;

		// Token: 0x0400C977 RID: 51575
		[MayRequireBiotech]
		public static ThingDef RipScannerGlow_West;

		// Token: 0x0400C978 RID: 51576
		[MayRequireBiotech]
		public static ThingDef Mote_XenogermImplantation;

		// Token: 0x0400C979 RID: 51577
		[MayRequireBiotech]
		public static ThingDef Mote_MechResurrectWarmupOnTarget;

		// Token: 0x0400C97A RID: 51578
		[MayRequireBiotech]
		public static ThingDef Mote_Toy;

		// Token: 0x0400C97B RID: 51579
		[MayRequireBiotech]
		public static ThingDef Mote_Deathresting;

		// Token: 0x0400C97C RID: 51580
		[MayRequireBiotech]
		public static ThingDef Mote_VatGlowHorizontal;

		// Token: 0x0400C97D RID: 51581
		[MayRequireBiotech]
		public static ThingDef Mote_VatGlowVertical;

		// Token: 0x0400C97E RID: 51582
		[MayRequireBiotech]
		public static ThingDef Mote_BabyCryingDots;

		// Token: 0x0400C97F RID: 51583
		[MayRequireAnomaly]
		public static ThingDef Mote_HateChantCall;

		// Token: 0x0400C980 RID: 51584
		[MayRequireAnomaly]
		public static ThingDef Mote_HateChantShadow;

		// Token: 0x0400C981 RID: 51585
		[MayRequireAnomaly]
		public static ThingDef Mote_PsychicRitualInvocation;

		// Token: 0x0400C982 RID: 51586
		[MayRequireAnomaly]
		public static ThingDef Mote_ChargingElectroCables;

		// Token: 0x0400C983 RID: 51587
		[MayRequireAnomaly]
		public static ThingDef Mote_HarbingerTreeRoots;

		// Token: 0x0400C984 RID: 51588
		[MayRequireRoyalty]
		public static ThingDef Throne;

		// Token: 0x0400C985 RID: 51589
		[MayRequireRoyalty]
		public static ThingDef Harp;

		// Token: 0x0400C986 RID: 51590
		[MayRequireRoyalty]
		public static ThingDef Harpsichord;

		// Token: 0x0400C987 RID: 51591
		[MayRequireRoyalty]
		public static ThingDef Piano;

		// Token: 0x0400C988 RID: 51592
		[MayRequireRoyalty]
		public static ThingDef ShipLandingBeacon;

		// Token: 0x0400C989 RID: 51593
		[MayRequireRoyalty]
		public static ThingDef ActivatorCountdown;

		// Token: 0x0400C98A RID: 51594
		[MayRequireAnomaly]
		public static ThingDef Mote_ShamblerAlert;

		// Token: 0x0400C98B RID: 51595
		[MayRequireAnomaly]
		public static ThingDef Mote_PsychicBanshee;

		// Token: 0x0400C98C RID: 51596
		[MayRequireAnomaly]
		public static ThingDef Mote_RevenantHeard;

		// Token: 0x0400C98D RID: 51597
		[MayRequireAnyOf("Ludeon.RimWorld.Odyssey,Ludeon.RimWorld.Biotech")]
		public static ThingDef Mote_TerrorHalo;

		// Token: 0x0400C98E RID: 51598
		public static ThingDef DelayedEffecterSpawner;

		// Token: 0x0400C98F RID: 51599
		[MayRequireAnomaly]
		public static ThingDef DelayedMetalhorrorEmerger;
	}
}
