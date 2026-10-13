using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000299 RID: 665
	public class JobDef : Def
	{
		// Token: 0x0600108F RID: 4239 RVA: 0x0004E847 File Offset: 0x0004CA47
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.joySkill != null && this.joyXpPerTick == 0f)
			{
				yield return "funSkill is not null but funXpPerTick is zero";
			}
			yield break;
			yield break;
		}

		// Token: 0x04000DA2 RID: 3490
		public Type driverClass;

		// Token: 0x04000DA3 RID: 3491
		[MustTranslate]
		public string reportString = "Doing something.";

		// Token: 0x04000DA4 RID: 3492
		public bool playerInterruptible = true;

		// Token: 0x04000DA5 RID: 3493
		public bool forceCompleteBeforeNextJob;

		// Token: 0x04000DA6 RID: 3494
		public CheckJobOverrideOnDamageMode checkOverrideOnDamage = CheckJobOverrideOnDamageMode.Always;

		// Token: 0x04000DA7 RID: 3495
		public bool alwaysShowWeapon;

		// Token: 0x04000DA8 RID: 3496
		public bool neverShowWeapon;

		// Token: 0x04000DA9 RID: 3497
		public bool suspendable = true;

		// Token: 0x04000DAA RID: 3498
		public bool casualInterruptible = true;

		// Token: 0x04000DAB RID: 3499
		public bool allowOpportunisticPrefix;

		// Token: 0x04000DAC RID: 3500
		public bool collideWithPawns;

		// Token: 0x04000DAD RID: 3501
		public bool isIdle;

		// Token: 0x04000DAE RID: 3502
		public TaleDef taleOnCompletion;

		// Token: 0x04000DAF RID: 3503
		public bool neverFleeFromEnemies;

		// Token: 0x04000DB0 RID: 3504
		public bool sleepCanInterrupt = true;

		// Token: 0x04000DB1 RID: 3505
		public bool makeTargetPrisoner;

		// Token: 0x04000DB2 RID: 3506
		public int waitAfterArriving;

		// Token: 0x04000DB3 RID: 3507
		public bool carryThingAfterJob;

		// Token: 0x04000DB4 RID: 3508
		public bool dropThingBeforeJob = true;

		// Token: 0x04000DB5 RID: 3509
		public bool isCrawlingIfDowned = true;

		// Token: 0x04000DB6 RID: 3510
		public bool alwaysShowReport;

		// Token: 0x04000DB7 RID: 3511
		public bool abilityCasting;

		// Token: 0x04000DB8 RID: 3512
		public bool tryStartFlying;

		// Token: 0x04000DB9 RID: 3513
		public bool ifFlyingKeepFlying;

		// Token: 0x04000DBA RID: 3514
		public float overrideFlyChance = -1f;

		// Token: 0x04000DBB RID: 3515
		public bool displayAsAreaInFloatMenu = true;

		// Token: 0x04000DBC RID: 3516
		public int joyDuration = 4000;

		// Token: 0x04000DBD RID: 3517
		public int joyMaxParticipants = 1;

		// Token: 0x04000DBE RID: 3518
		public float joyGainRate = 1f;

		// Token: 0x04000DBF RID: 3519
		public SkillDef joySkill;

		// Token: 0x04000DC0 RID: 3520
		public float joyXpPerTick;

		// Token: 0x04000DC1 RID: 3521
		public JoyKindDef joyKind;

		// Token: 0x04000DC2 RID: 3522
		public Rot4 faceDir = Rot4.Invalid;

		// Token: 0x04000DC3 RID: 3523
		public int learningDuration = 20000;

		// Token: 0x04000DC4 RID: 3524
		public ReservationLayerDef containerReservationLayer;
	}
}
