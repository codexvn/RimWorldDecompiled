using System;

namespace Verse
{
	// Token: 0x0200044F RID: 1103
	public enum ThingRequestGroup : byte
	{
		// Token: 0x04001873 RID: 6259
		Undefined,
		// Token: 0x04001874 RID: 6260
		Nothing,
		// Token: 0x04001875 RID: 6261
		Everything,
		// Token: 0x04001876 RID: 6262
		HaulableEver,
		// Token: 0x04001877 RID: 6263
		HaulableAlways,
		// Token: 0x04001878 RID: 6264
		FoodSource,
		// Token: 0x04001879 RID: 6265
		MechCharger,
		// Token: 0x0400187A RID: 6266
		FoodSourceNotPlantOrTree,
		// Token: 0x0400187B RID: 6267
		Corpse,
		// Token: 0x0400187C RID: 6268
		Blueprint,
		// Token: 0x0400187D RID: 6269
		BuildingArtificial,
		// Token: 0x0400187E RID: 6270
		BuildingFrame,
		// Token: 0x0400187F RID: 6271
		Pawn,
		// Token: 0x04001880 RID: 6272
		PotentialBillGiver,
		// Token: 0x04001881 RID: 6273
		Medicine,
		// Token: 0x04001882 RID: 6274
		Filth,
		// Token: 0x04001883 RID: 6275
		AttackTarget,
		// Token: 0x04001884 RID: 6276
		Weapon,
		// Token: 0x04001885 RID: 6277
		Apparel,
		// Token: 0x04001886 RID: 6278
		Refuelable,
		// Token: 0x04001887 RID: 6279
		Styleable,
		// Token: 0x04001888 RID: 6280
		HaulableEverOrMinifiable,
		// Token: 0x04001889 RID: 6281
		Drug,
		// Token: 0x0400188A RID: 6282
		Shell,
		// Token: 0x0400188B RID: 6283
		HarvestablePlant,
		// Token: 0x0400188C RID: 6284
		Fire,
		// Token: 0x0400188D RID: 6285
		Bed,
		// Token: 0x0400188E RID: 6286
		Plant,
		// Token: 0x0400188F RID: 6287
		NonStumpPlant,
		// Token: 0x04001890 RID: 6288
		WasteProducer,
		// Token: 0x04001891 RID: 6289
		Book,
		// Token: 0x04001892 RID: 6290
		HoldingPlatformTarget,
		// Token: 0x04001893 RID: 6291
		PsychicRitualSpot,
		// Token: 0x04001894 RID: 6292
		EntityHolder,
		// Token: 0x04001895 RID: 6293
		SubstructureFootprint,
		// Token: 0x04001896 RID: 6294
		PassengerShuttle,
		// Token: 0x04001897 RID: 6295
		Fish,
		// Token: 0x04001898 RID: 6296
		Construction,
		// Token: 0x04001899 RID: 6297
		HasGUIOverlay,
		// Token: 0x0400189A RID: 6298
		MinifiedThing,
		// Token: 0x0400189B RID: 6299
		Grave,
		// Token: 0x0400189C RID: 6300
		Art,
		// Token: 0x0400189D RID: 6301
		ThingHolder,
		// Token: 0x0400189E RID: 6302
		ActiveTransporter,
		// Token: 0x0400189F RID: 6303
		Transporter,
		// Token: 0x040018A0 RID: 6304
		LongRangeMineralScanner,
		// Token: 0x040018A1 RID: 6305
		AffectsSky,
		// Token: 0x040018A2 RID: 6306
		WindSource,
		// Token: 0x040018A3 RID: 6307
		AlwaysFlee,
		// Token: 0x040018A4 RID: 6308
		ResearchBench,
		// Token: 0x040018A5 RID: 6309
		Facility,
		// Token: 0x040018A6 RID: 6310
		AffectedByFacilities,
		// Token: 0x040018A7 RID: 6311
		CreatesInfestations,
		// Token: 0x040018A8 RID: 6312
		WithCustomRectForSelector,
		// Token: 0x040018A9 RID: 6313
		ProjectileInterceptor,
		// Token: 0x040018AA RID: 6314
		ConditionCauser,
		// Token: 0x040018AB RID: 6315
		MusicalInstrument,
		// Token: 0x040018AC RID: 6316
		MusicSource,
		// Token: 0x040018AD RID: 6317
		Throne,
		// Token: 0x040018AE RID: 6318
		FoodDispenser,
		// Token: 0x040018AF RID: 6319
		Projectile,
		// Token: 0x040018B0 RID: 6320
		Chunk,
		// Token: 0x040018B1 RID: 6321
		MeditationFocus,
		// Token: 0x040018B2 RID: 6322
		Seed,
		// Token: 0x040018B3 RID: 6323
		DryadSpawner,
		// Token: 0x040018B4 RID: 6324
		Studiable,
		// Token: 0x040018B5 RID: 6325
		Suppressable,
		// Token: 0x040018B6 RID: 6326
		ActionDelay,
		// Token: 0x040018B7 RID: 6327
		MechGestator,
		// Token: 0x040018B8 RID: 6328
		GenepackHolder,
		// Token: 0x040018B9 RID: 6329
		Dissolving,
		// Token: 0x040018BA RID: 6330
		SubcoreScanner,
		// Token: 0x040018BB RID: 6331
		BossgroupCaller,
		// Token: 0x040018BC RID: 6332
		WakeUpDormant,
		// Token: 0x040018BD RID: 6333
		Atomizer,
		// Token: 0x040018BE RID: 6334
		PowerTrader,
		// Token: 0x040018BF RID: 6335
		MapPortal,
		// Token: 0x040018C0 RID: 6336
		BuildingGroundSpawner,
		// Token: 0x040018C1 RID: 6337
		Hackable,
		// Token: 0x040018C2 RID: 6338
		Door,
		// Token: 0x040018C3 RID: 6339
		CostProvider,
		// Token: 0x040018C4 RID: 6340
		ApparelSource
	}
}
