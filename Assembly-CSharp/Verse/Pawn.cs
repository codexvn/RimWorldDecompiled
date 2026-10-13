using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020007A2 RID: 1954
	public class Pawn : ThingWithComps, IStrippable, IBillGiver, IVerbOwner, ITrader, IAttackTarget, ILoadReferenceable, IAttackTargetSearcher, IThingHolder, IObservedThoughtGiver, ISearchableContents, IEquatable<Pawn>
	{
		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x06002FFA RID: 12282 RVA: 0x000F6020 File Offset: 0x000F4220
		// (set) Token: 0x06002FFB RID: 12283 RVA: 0x000F6028 File Offset: 0x000F4228
		public Name Name
		{
			get
			{
				return this.nameInt;
			}
			set
			{
				this.nameInt = value;
			}
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06002FFC RID: 12284 RVA: 0x000F6031 File Offset: 0x000F4231
		public RaceProperties RaceProps
		{
			get
			{
				return this.def.race;
			}
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06002FFD RID: 12285 RVA: 0x000F603E File Offset: 0x000F423E
		public Job CurJob
		{
			get
			{
				Pawn_JobTracker pawn_JobTracker = this.jobs;
				if (pawn_JobTracker == null)
				{
					return null;
				}
				return pawn_JobTracker.curJob;
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x06002FFE RID: 12286 RVA: 0x000F6051 File Offset: 0x000F4251
		public JobDef CurJobDef
		{
			get
			{
				Job curJob = this.CurJob;
				if (curJob == null)
				{
					return null;
				}
				return curJob.def;
			}
		}

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x06002FFF RID: 12287 RVA: 0x000F6064 File Offset: 0x000F4264
		public bool Downed
		{
			get
			{
				return this.health.Downed;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06003000 RID: 12288 RVA: 0x000F6071 File Offset: 0x000F4271
		public bool Crawling
		{
			get
			{
				return this.Downed && this.health.CanCrawl && this.CurJobDef != null && this.CurJobDef.isCrawlingIfDowned && !this.InBed();
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x06003001 RID: 12289 RVA: 0x000F60A8 File Offset: 0x000F42A8
		public bool CanAttackWhileCrawling
		{
			get
			{
				return this.IsMutant && this.mutant.Def.canAttackWhileCrawling;
			}
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x06003002 RID: 12290 RVA: 0x000F60C4 File Offset: 0x000F42C4
		public bool Flying
		{
			get
			{
				Pawn_FlightTracker pawn_FlightTracker = this.flight;
				return pawn_FlightTracker != null && pawn_FlightTracker.Flying;
			}
		}

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x06003003 RID: 12291 RVA: 0x000F60D7 File Offset: 0x000F42D7
		public bool Swimming
		{
			get
			{
				Job curJob = this.CurJob;
				return curJob != null && curJob.swimming;
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x06003004 RID: 12292 RVA: 0x000F60EA File Offset: 0x000F42EA
		public bool Dead
		{
			get
			{
				return this.health.Dead;
			}
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x06003005 RID: 12293 RVA: 0x000F60F7 File Offset: 0x000F42F7
		public bool DeadOrDowned
		{
			get
			{
				return this.Dead || this.Downed;
			}
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06003006 RID: 12294 RVA: 0x000F6109 File Offset: 0x000F4309
		public string KindLabel
		{
			get
			{
				return GenLabel.BestKindLabel(this, false, false, false, -1);
			}
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x06003007 RID: 12295 RVA: 0x000F6115 File Offset: 0x000F4315
		public bool InMentalState
		{
			get
			{
				return !this.Dead && this.mindState.mentalStateHandler.InMentalState;
			}
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x06003008 RID: 12296 RVA: 0x000F6131 File Offset: 0x000F4331
		public MentalState MentalState
		{
			get
			{
				if (!this.Dead)
				{
					return this.mindState.mentalStateHandler.CurState;
				}
				return null;
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x06003009 RID: 12297 RVA: 0x000F614D File Offset: 0x000F434D
		public MentalStateDef MentalStateDef
		{
			get
			{
				if (!this.Dead)
				{
					return this.mindState.mentalStateHandler.CurStateDef;
				}
				return null;
			}
		}

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x0600300A RID: 12298 RVA: 0x000F6169 File Offset: 0x000F4369
		public bool InAggroMentalState
		{
			get
			{
				return !this.Dead && this.mindState.mentalStateHandler.InMentalState && this.mindState.mentalStateHandler.CurStateDef.IsAggro;
			}
		}

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x0600300B RID: 12299 RVA: 0x000F619C File Offset: 0x000F439C
		public bool Inspired
		{
			get
			{
				if (!this.Dead)
				{
					Pawn_MindState pawn_MindState = this.mindState;
					if (((pawn_MindState != null) ? pawn_MindState.inspirationHandler : null) != null)
					{
						return this.mindState.inspirationHandler.Inspired;
					}
				}
				return false;
			}
		}

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x0600300C RID: 12300 RVA: 0x000F61CC File Offset: 0x000F43CC
		public Inspiration Inspiration
		{
			get
			{
				if (!this.Dead)
				{
					return this.mindState.inspirationHandler.CurState;
				}
				return null;
			}
		}

		// Token: 0x170008CA RID: 2250
		// (get) Token: 0x0600300D RID: 12301 RVA: 0x000F61E8 File Offset: 0x000F43E8
		public InspirationDef InspirationDef
		{
			get
			{
				if (!this.Dead)
				{
					return this.mindState.inspirationHandler.CurStateDef;
				}
				return null;
			}
		}

		// Token: 0x170008CB RID: 2251
		// (get) Token: 0x0600300E RID: 12302 RVA: 0x000F6204 File Offset: 0x000F4404
		public override Vector3 DrawPos
		{
			get
			{
				return this.Drawer.DrawPos;
			}
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x0600300F RID: 12303 RVA: 0x000F6211 File Offset: 0x000F4411
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06003010 RID: 12304 RVA: 0x000F6219 File Offset: 0x000F4419
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.def.Verbs;
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06003011 RID: 12305 RVA: 0x000F6226 File Offset: 0x000F4426
		public List<Tool> Tools
		{
			get
			{
				return this.def.tools;
			}
		}

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06003012 RID: 12306 RVA: 0x000F6233 File Offset: 0x000F4433
		public bool ShouldAvoidFences
		{
			get
			{
				return this.def.race.FenceBlocked || (this.roping != null && this.roping.AnyRopeesFenceBlocked);
			}
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06003013 RID: 12307 RVA: 0x000F6260 File Offset: 0x000F4460
		public bool DrawNonHumanlikeSwimmingGraphic
		{
			get
			{
				return base.Spawned && this.WaterCellCost != null && !this.RaceProps.Humanlike && this.ageTracker.CurKindLifeStage.swimmingGraphicData != null && base.Position.GetTerrain(base.Map).IsWater;
			}
		}

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06003014 RID: 12308 RVA: 0x000F62BE File Offset: 0x000F44BE
		public bool DrawNonHumanlikeStationaryGraphic
		{
			get
			{
				return base.Spawned && !this.RaceProps.Humanlike && !this.pather.Moving && this.ageTracker.CurKindLifeStage.stationaryGraphicData != null;
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x06003015 RID: 12309 RVA: 0x000F62F9 File Offset: 0x000F44F9
		public bool CanOpenDoors
		{
			get
			{
				return (!this.IsMutant || this.mutant.Def.canOpenDoors) && this.kindDef.canOpenDoors;
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x06003016 RID: 12310 RVA: 0x000F6328 File Offset: 0x000F4528
		public bool CanOpenAnyDoor
		{
			get
			{
				if (WildManUtility.WildManShouldReachOutsideNow(this))
				{
					return true;
				}
				Lord lord = this.lord;
				return (((lord != null) ? lord.LordJob : null) != null && this.lord.LordJob.CanOpenAnyDoor(this)) || (this.IsMutant && this.mutant.Def.canOpenAnyDoor) || this.kindDef.canOpenAnyDoor;
			}
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06003017 RID: 12311 RVA: 0x000F6394 File Offset: 0x000F4594
		public int? WaterCellCost
		{
			get
			{
				if (this.Flying)
				{
					return new int?(1);
				}
				if (ModsConfig.BiotechActive && this.genes != null && this.genes.WaterCellCost != null)
				{
					return this.genes.WaterCellCost;
				}
				if (this.def.race.waterCellCost != null)
				{
					return this.def.race.waterCellCost;
				}
				if (!this.Swimming)
				{
					return null;
				}
				return new int?(1);
			}
		}

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06003018 RID: 12312 RVA: 0x000F6420 File Offset: 0x000F4620
		public bool IsColonist
		{
			get
			{
				return base.Faction != null && base.Faction.IsPlayer && this.RaceProps.Humanlike && (!this.IsSlave || this.guest.SlaveIsSecure) && !this.IsSubhuman;
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06003019 RID: 12313 RVA: 0x000F646F File Offset: 0x000F466F
		public bool IsFreeColonist
		{
			get
			{
				return this.IsColonist && this.HostFaction == null;
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x0600301A RID: 12314 RVA: 0x000F6484 File Offset: 0x000F4684
		public bool IsFreeNonSlaveColonist
		{
			get
			{
				return this.IsFreeColonist && !this.IsSlave;
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x0600301B RID: 12315 RVA: 0x000F6499 File Offset: 0x000F4699
		public bool CanTakeOrder
		{
			get
			{
				return this.IsColonistPlayerControlled || this.IsColonyMech || this.IsColonySubhumanPlayerControlled;
			}
		}

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x0600301C RID: 12316 RVA: 0x000F64B3 File Offset: 0x000F46B3
		public bool IsCreepJoiner
		{
			get
			{
				return ModsConfig.AnomalyActive && this.creepjoiner != null;
			}
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x0600301D RID: 12317 RVA: 0x000F64C7 File Offset: 0x000F46C7
		public Faction HostFaction
		{
			get
			{
				Pawn_GuestTracker pawn_GuestTracker = this.guest;
				if (pawn_GuestTracker == null)
				{
					return null;
				}
				return pawn_GuestTracker.HostFaction;
			}
		}

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x0600301E RID: 12318 RVA: 0x000F64DA File Offset: 0x000F46DA
		public Faction SlaveFaction
		{
			get
			{
				Pawn_GuestTracker pawn_GuestTracker = this.guest;
				if (pawn_GuestTracker == null)
				{
					return null;
				}
				return pawn_GuestTracker.SlaveFaction;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x0600301F RID: 12319 RVA: 0x000F64ED File Offset: 0x000F46ED
		public Ideo Ideo
		{
			get
			{
				Pawn_IdeoTracker pawn_IdeoTracker = this.ideo;
				if (pawn_IdeoTracker == null)
				{
					return null;
				}
				return pawn_IdeoTracker.Ideo;
			}
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06003020 RID: 12320 RVA: 0x000F6500 File Offset: 0x000F4700
		public bool ShouldHaveIdeo
		{
			get
			{
				return !this.DevelopmentalStage.Baby() && !this.kindDef.preventIdeo && (!this.IsMutant || !this.mutant.Def.disablesIdeo);
			}
		}

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x06003021 RID: 12321 RVA: 0x000F653B File Offset: 0x000F473B
		public bool Drafted
		{
			get
			{
				return this.drafter != null && this.drafter.Drafted;
			}
		}

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x06003022 RID: 12322 RVA: 0x000F6552 File Offset: 0x000F4752
		public bool IsPrisoner
		{
			get
			{
				return this.guest != null && this.guest.IsPrisoner;
			}
		}

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x06003023 RID: 12323 RVA: 0x000F6569 File Offset: 0x000F4769
		public bool IsPrisonerOfColony
		{
			get
			{
				return this.guest != null && this.guest.IsPrisoner && this.guest.HostFaction.IsPlayer;
			}
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06003024 RID: 12324 RVA: 0x000F6592 File Offset: 0x000F4792
		public bool IsSlave
		{
			get
			{
				return this.guest != null && this.guest.IsSlave;
			}
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06003025 RID: 12325 RVA: 0x000F65A9 File Offset: 0x000F47A9
		public bool IsSlaveOfColony
		{
			get
			{
				return this.IsSlave && base.Faction.IsPlayer;
			}
		}

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06003026 RID: 12326 RVA: 0x000F65C0 File Offset: 0x000F47C0
		public bool IsFreeman
		{
			get
			{
				return this.HostFaction == null;
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06003027 RID: 12327 RVA: 0x000F65CB File Offset: 0x000F47CB
		public bool IsMutant
		{
			get
			{
				return this.mutant != null;
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06003028 RID: 12328 RVA: 0x000F65D6 File Offset: 0x000F47D6
		public bool IsSubhuman
		{
			get
			{
				return this.IsMutant && this.mutant.Def.consideredSubhuman;
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06003029 RID: 12329 RVA: 0x000F65F2 File Offset: 0x000F47F2
		public bool IsDuplicate
		{
			get
			{
				return ModsConfig.AnomalyActive && this.duplicate != null && this.duplicate.duplicateOf != int.MinValue && this.duplicate.duplicateOf != this.thingIDNumber;
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x0600302A RID: 12330 RVA: 0x000F662D File Offset: 0x000F482D
		public bool IsEntity
		{
			get
			{
				return ModsConfig.AnomalyActive && (!this.RaceProps.Humanlike || this.IsSubhuman || this.IsShambler) && base.Faction == Faction.OfEntities;
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x0600302B RID: 12331 RVA: 0x000F6662 File Offset: 0x000F4862
		public bool IsShambler
		{
			get
			{
				return ModsConfig.AnomalyActive && ((this.IsMutant && this.mutant.Def == MutantDefOf.Shambler) || this.health.hediffSet.HasHediff(HediffDefOf.ShamblerCorpse, false));
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x0600302C RID: 12332 RVA: 0x000F669F File Offset: 0x000F489F
		public bool IsGhoul
		{
			get
			{
				return ModsConfig.AnomalyActive && this.IsMutant && this.mutant.Def == MutantDefOf.Ghoul;
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x0600302D RID: 12333 RVA: 0x000F66C4 File Offset: 0x000F48C4
		public bool IsAwokenCorpse
		{
			get
			{
				return ModsConfig.AnomalyActive && this.IsMutant && this.mutant.Def == MutantDefOf.AwokenCorpse;
			}
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x0600302E RID: 12334 RVA: 0x000F66E9 File Offset: 0x000F48E9
		public bool IsAnimal
		{
			get
			{
				return this.RaceProps.Animal && !this.IsSubhuman;
			}
		}

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x0600302F RID: 12335 RVA: 0x000F6703 File Offset: 0x000F4903
		public bool HasShowGizmosOnCorpseHediff
		{
			get
			{
				Pawn_HealthTracker pawn_HealthTracker = this.health;
				return ((pawn_HealthTracker != null) ? pawn_HealthTracker.hediffSet : null) != null && this.health.hediffSet.HasHediffShowGizmosOnCorpse();
			}
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06003030 RID: 12336 RVA: 0x000F672C File Offset: 0x000F492C
		public DevelopmentalStage DevelopmentalStage
		{
			get
			{
				Pawn_AgeTracker pawn_AgeTracker = this.ageTracker;
				DevelopmentalStage? developmentalStage;
				if (pawn_AgeTracker == null)
				{
					developmentalStage = null;
				}
				else
				{
					LifeStageDef curLifeStage = pawn_AgeTracker.CurLifeStage;
					developmentalStage = ((curLifeStage != null) ? new DevelopmentalStage?(curLifeStage.developmentalStage) : null);
				}
				DevelopmentalStage? developmentalStage2 = developmentalStage;
				if (developmentalStage2 == null)
				{
					return DevelopmentalStage.Adult;
				}
				return developmentalStage2.GetValueOrDefault();
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06003031 RID: 12337 RVA: 0x000F6780 File Offset: 0x000F4980
		public GuestStatus? GuestStatus
		{
			get
			{
				if (this.guest != null && (this.HostFaction != null || this.guest.GuestStatus != RimWorld.GuestStatus.Guest))
				{
					return new GuestStatus?(this.guest.GuestStatus);
				}
				return null;
			}
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06003032 RID: 12338 RVA: 0x000F67C4 File Offset: 0x000F49C4
		public bool IsColonistPlayerControlled
		{
			get
			{
				return base.Spawned && this.IsColonist && this.MentalStateDef == null && (this.HostFaction == null || this.IsSlave);
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06003033 RID: 12339 RVA: 0x000F67F0 File Offset: 0x000F49F0
		public bool IsColonyMech
		{
			get
			{
				return ModsConfig.BiotechActive && this.RaceProps.IsMechanoid && base.Faction == Faction.OfPlayer && this.MentalStateDef == null && (this.HostFaction == null || this.IsSlave);
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06003034 RID: 12340 RVA: 0x000F682D File Offset: 0x000F4A2D
		public bool IsColonyMechPlayerControlled
		{
			get
			{
				return base.Spawned && this.IsColonyMech && this.OverseerSubject != null && this.OverseerSubject.State == OverseerSubjectState.Overseen;
			}
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06003035 RID: 12341 RVA: 0x000F6857 File Offset: 0x000F4A57
		public bool IsColonySubhuman
		{
			get
			{
				return this.IsSubhuman && base.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06003036 RID: 12342 RVA: 0x000F6870 File Offset: 0x000F4A70
		public bool IsColonySubhumanPlayerControlled
		{
			get
			{
				return base.Spawned && this.IsColonySubhuman && this.mutant.Def.canBeDrafted;
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06003037 RID: 12343 RVA: 0x000F6894 File Offset: 0x000F4A94
		public bool IsPlayerControlled
		{
			get
			{
				return this.IsColonistPlayerControlled || this.IsColonyMechPlayerControlled || this.IsColonySubhumanPlayerControlled;
			}
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06003038 RID: 12344 RVA: 0x000F68AE File Offset: 0x000F4AAE
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				yield return this.InteractionCell;
				yield break;
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06003039 RID: 12345 RVA: 0x000F68BE File Offset: 0x000F4ABE
		public bool InContainerEnclosed
		{
			get
			{
				return base.ParentHolder.IsEnclosingContainer();
			}
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x0600303A RID: 12346 RVA: 0x000F68CB File Offset: 0x000F4ACB
		public Corpse Corpse
		{
			get
			{
				return base.ParentHolder as Corpse;
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x0600303B RID: 12347 RVA: 0x000F68D8 File Offset: 0x000F4AD8
		public Pawn CarriedBy
		{
			get
			{
				Pawn_CarryTracker pawn_CarryTracker = base.ParentHolder as Pawn_CarryTracker;
				if (pawn_CarryTracker == null)
				{
					return null;
				}
				return pawn_CarryTracker.pawn;
			}
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x0600303C RID: 12348 RVA: 0x000F68FC File Offset: 0x000F4AFC
		public virtual bool CanAttackWhenPathingBlocked
		{
			get
			{
				return !this.IsAwokenCorpse;
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x0600303D RID: 12349 RVA: 0x000F6908 File Offset: 0x000F4B08
		public bool HarmedByVacuum
		{
			get
			{
				return ModsConfig.OdysseyActive && !this.RaceProps.IsMechanoid && (!this.IsMutant || this.mutant.Def.breathesAir) && this.GetStatValue(StatDefOf.VacuumResistance, true, 60) < 1f;
			}
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x0600303E RID: 12350 RVA: 0x000F695C File Offset: 0x000F4B5C
		public bool ConcernedByVacuum
		{
			get
			{
				return ModsConfig.OdysseyActive && !this.RaceProps.IsMechanoid && (!this.IsMutant || this.mutant.Def.breathesAir) && this.GetStatValue(StatDefOf.VacuumResistance, true, 60) < 0.75f;
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x0600303F RID: 12351 RVA: 0x000F69B0 File Offset: 0x000F4BB0
		private string LabelPrefix
		{
			get
			{
				if (this.IsMutant && this.mutant.HasTurned)
				{
					return this.mutant.Def.namePrefix;
				}
				return string.Empty;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06003040 RID: 12352 RVA: 0x000F69E0 File Offset: 0x000F4BE0
		public override string LabelNoCount
		{
			get
			{
				if (this.Name == null)
				{
					return this.LabelPrefix + this.KindLabel;
				}
				if (this.story == null || this.story.TitleShortCap.NullOrEmpty() || this.IsSubhuman)
				{
					return this.LabelPrefix + this.Name.ToStringShort;
				}
				return this.LabelPrefix + this.Name.ToStringShort + (", " + this.story.TitleShortCap).Colorize(ColoredText.SubtleGrayColor);
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06003041 RID: 12353 RVA: 0x000F6A75 File Offset: 0x000F4C75
		public override string LabelShort
		{
			get
			{
				if (this.Name != null)
				{
					return this.LabelPrefix + this.Name.ToStringShort;
				}
				return this.LabelNoCount;
			}
		}

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06003042 RID: 12354 RVA: 0x000F6A9C File Offset: 0x000F4C9C
		public TaggedString LabelNoCountColored
		{
			get
			{
				if (this.Name == null)
				{
					return this.LabelPrefix + this.KindLabel;
				}
				if (this.story == null || this.story.TitleShortCap.NullOrEmpty() || this.IsSubhuman)
				{
					return this.LabelPrefix + this.Name.ToStringShort.Colorize(ColoredText.NameColor);
				}
				return this.LabelPrefix + this.Name.ToStringShort.Colorize(ColoredText.NameColor) + (", " + this.story.TitleShortCap).Colorize(ColoredText.SubtleGrayColor);
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06003043 RID: 12355 RVA: 0x000F6B58 File Offset: 0x000F4D58
		public TaggedString NameShortColored
		{
			get
			{
				if (this.Name != null)
				{
					return this.LabelPrefix + this.Name.ToStringShort.Colorize(ColoredText.NameColor);
				}
				return this.LabelPrefix + this.KindLabel;
			}
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06003044 RID: 12356 RVA: 0x000F6BAC File Offset: 0x000F4DAC
		public TaggedString NameFullColored
		{
			get
			{
				if (this.Name != null)
				{
					return this.LabelPrefix + this.Name.ToStringFull.Colorize(ColoredText.NameColor);
				}
				return this.LabelPrefix + this.KindLabel;
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06003045 RID: 12357 RVA: 0x000F6C00 File Offset: 0x000F4E00
		public TaggedString LegalStatus
		{
			get
			{
				if (this.IsSlave)
				{
					return "Slave".Translate().CapitalizeFirst();
				}
				if (base.Faction != null)
				{
					return new TaggedString(base.Faction.def.pawnSingular);
				}
				return "Colonist".Translate();
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06003046 RID: 12358 RVA: 0x000F6C50 File Offset: 0x000F4E50
		public float TicksPerMoveCardinal
		{
			get
			{
				return this.TicksPerMove(false);
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06003047 RID: 12359 RVA: 0x000F6C59 File Offset: 0x000F4E59
		public float TicksPerMoveDiagonal
		{
			get
			{
				return this.TicksPerMove(true);
			}
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06003048 RID: 12360 RVA: 0x000F6C62 File Offset: 0x000F4E62
		public override string DescriptionDetailed
		{
			get
			{
				return this.DescriptionFlavor;
			}
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06003049 RID: 12361 RVA: 0x000F6C6C File Offset: 0x000F4E6C
		public override string DescriptionFlavor
		{
			get
			{
				if (ModsConfig.AnomalyActive && this.IsSubhuman && !this.mutant.Def.description.NullOrEmpty())
				{
					return this.mutant.Def.description;
				}
				if (this.IsBaseliner())
				{
					return this.def.description;
				}
				string t;
				if (this.genes.Xenotype == XenotypeDefOf.Baseliner)
				{
					t = ((this.genes.CustomXenotype == null) ? this.genes.Xenotype.description : "UniqueXenotypeDesc".Translate());
				}
				else
				{
					t = this.genes.Xenotype.description;
				}
				return "StatsReport_NonBaselinerDescription".Translate(this.genes.XenotypeLabel) + "\n\n" + t;
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x0600304A RID: 12362 RVA: 0x000F6D47 File Offset: 0x000F4F47
		public override IEnumerable<DefHyperlink> DescriptionHyperlinks
		{
			get
			{
				foreach (DefHyperlink defHyperlink in base.DescriptionHyperlinks)
				{
					yield return defHyperlink;
				}
				IEnumerator<DefHyperlink> enumerator = null;
				if (!this.IsBaseliner() && this.genes.CustomXenotype == null)
				{
					yield return new DefHyperlink(this.genes.Xenotype);
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x0600304B RID: 12363 RVA: 0x000F6D58 File Offset: 0x000F4F58
		public Pawn_DrawTracker Drawer
		{
			get
			{
				Pawn_DrawTracker result;
				if ((result = this.drawer) == null)
				{
					result = (this.drawer = new Pawn_DrawTracker(this));
				}
				return result;
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x0600304C RID: 12364 RVA: 0x000F6D80 File Offset: 0x000F4F80
		public Faction HomeFaction
		{
			get
			{
				if (base.Faction == null || !base.Faction.IsPlayer)
				{
					return base.Faction;
				}
				if (this.IsSlave && this.SlaveFaction != null)
				{
					return this.SlaveFaction;
				}
				if (this.HasExtraMiniFaction(null))
				{
					return this.GetExtraMiniFaction(null);
				}
				return this.GetExtraHomeFaction(null) ?? base.Faction;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x0600304D RID: 12365 RVA: 0x000F6DE2 File Offset: 0x000F4FE2
		public bool Deathresting
		{
			get
			{
				return ModsConfig.BiotechActive && this.health.hediffSet.HasHediff(HediffDefOf.Deathrest, false);
			}
		}

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x0600304E RID: 12366 RVA: 0x000F6E03 File Offset: 0x000F5003
		public bool HasDeathRefusalOrResurrecting
		{
			get
			{
				return ModsConfig.AnomalyActive && (this.health.hediffSet.HasHediff<Hediff_DeathRefusal>(false) || this.health.hediffSet.HasHediff(HediffDefOf.Rising, false));
			}
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x0600304F RID: 12367 RVA: 0x000F6E39 File Offset: 0x000F5039
		public override bool Suspended
		{
			get
			{
				return base.Suspended || Find.WorldPawns.GetSituation(this) == WorldPawnSituation.ReservedByQuest;
			}
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06003050 RID: 12368 RVA: 0x000F6E57 File Offset: 0x000F5057
		public Faction DeadlifeDustFaction
		{
			get
			{
				if (GenTicks.TicksGame - this.deadlifeDustFactionTick < 12500 && this.deadlifeDustFaction != null)
				{
					return this.deadlifeDustFaction;
				}
				return Faction.OfPlayer;
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06003051 RID: 12369 RVA: 0x000F6E80 File Offset: 0x000F5080
		public bool HasPsylink
		{
			get
			{
				Pawn_PsychicEntropyTracker pawn_PsychicEntropyTracker = this.psychicEntropy;
				return ((pawn_PsychicEntropyTracker != null) ? pawn_PsychicEntropyTracker.Psylink : null) != null;
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06003052 RID: 12370 RVA: 0x000F6E97 File Offset: 0x000F5097
		public CompOverseerSubject OverseerSubject
		{
			get
			{
				if (ModsConfig.BiotechActive && this.overseerSubject == null && this.RaceProps.IsMechanoid)
				{
					this.overseerSubject = base.GetComp<CompOverseerSubject>();
				}
				return this.overseerSubject;
			}
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x000F6EC7 File Offset: 0x000F50C7
		public virtual bool ShouldShowQuestionMark()
		{
			if (ModsConfig.AnomalyActive && this.creepjoiner != null)
			{
				return this.creepjoiner.IsOnEntryLord;
			}
			return this.CanTradeNow;
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06003054 RID: 12372 RVA: 0x000F6EEA File Offset: 0x000F50EA
		public override int UpdateRateTicks
		{
			get
			{
				if (!this.RaceProps.Animal)
				{
					return base.UpdateRateTicks;
				}
				return 15;
			}
		}

		// Token: 0x06003055 RID: 12373 RVA: 0x000F6109 File Offset: 0x000F4309
		public string GetKindLabelSingular()
		{
			return GenLabel.BestKindLabel(this, false, false, false, -1);
		}

		// Token: 0x06003056 RID: 12374 RVA: 0x000F6F02 File Offset: 0x000F5102
		public string GetKindLabelPlural(int count = -1)
		{
			return GenLabel.BestKindLabel(this, false, false, true, count);
		}

		// Token: 0x06003057 RID: 12375 RVA: 0x000F6F0E File Offset: 0x000F510E
		public static void ResetStaticData()
		{
			Pawn.NotSurgeryReadyTrans = "NotSurgeryReady".Translate();
			Pawn.CannotReachTrans = "CannotReach".Translate();
		}

		// Token: 0x06003058 RID: 12376 RVA: 0x000F6F38 File Offset: 0x000F5138
		public override void Notify_DefsHotReloaded()
		{
			base.Notify_DefsHotReloaded();
			this.Drawer.renderer.SetAllGraphicsDirty();
		}

		// Token: 0x06003059 RID: 12377 RVA: 0x000F6F50 File Offset: 0x000F5150
		public void MarkDeadlifeDustForFaction(Faction faction)
		{
			this.deadlifeDustFaction = faction;
			this.deadlifeDustFactionTick = GenTicks.TicksGame;
		}

		// Token: 0x0600305A RID: 12378 RVA: 0x000F6F64 File Offset: 0x000F5164
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Dead)
			{
				Log.Warning("Tried to spawn Dead Pawn " + this.ToStringSafe<Pawn>() + ". Replacing with corpse.");
				Corpse corpse = (Corpse)ThingMaker.MakeThing(this.RaceProps.corpseDef, null);
				corpse.InnerPawn = this;
				GenSpawn.Spawn(corpse, base.Position, map, WipeMode.Vanish);
				return;
			}
			if (this.def == null || this.kindDef == null)
			{
				Log.Warning("Tried to spawn pawn without def " + this.ToStringSafe<Pawn>() + ".");
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			if (Find.WorldPawns.Contains(this))
			{
				Find.WorldPawns.RemovePawn(this);
			}
			PawnComponentsUtility.AddComponentsForSpawn(this);
			if (!PawnUtility.InValidState(this))
			{
				Log.Error("Pawn " + this.ToStringSafe<Pawn>() + " spawned in invalid state. Destroying...");
				try
				{
					this.DeSpawn(DestroyMode.Vanish);
				}
				catch (Exception ex)
				{
					string str = "Tried to despawn ";
					string str2 = this.ToStringSafe<Pawn>();
					string str3 = " because of the previous error but couldn't: ";
					Exception ex2 = ex;
					Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
				}
				Find.WorldPawns.PassToWorld(this, PawnDiscardDecideMode.Discard);
				return;
			}
			this.Drawer.Notify_Spawned();
			this.rotationTracker.Notify_Spawned();
			if (!respawningAfterLoad)
			{
				this.pather.ResetToCurrentPosition();
			}
			base.Map.mapPawns.RegisterPawn(this);
			base.Map.autoSlaughterManager.Notify_PawnSpawned();
			if (this.relations != null)
			{
				this.relations.everSeenByPlayer = true;
			}
			AddictionUtility.CheckDrugAddictionTeachOpportunity(this);
			Pawn_NeedsTracker pawn_NeedsTracker = this.needs;
			if (pawn_NeedsTracker != null)
			{
				Need_Mood mood = pawn_NeedsTracker.mood;
				if (mood != null)
				{
					PawnRecentMemory recentMemory = mood.recentMemory;
					if (recentMemory != null)
					{
						recentMemory.Notify_Spawned(respawningAfterLoad);
					}
				}
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker = this.equipment;
			if (pawn_EquipmentTracker != null)
			{
				pawn_EquipmentTracker.Notify_PawnSpawned();
			}
			Pawn_HealthTracker pawn_HealthTracker = this.health;
			if (pawn_HealthTracker != null)
			{
				pawn_HealthTracker.Notify_Spawned();
			}
			Pawn_MechanitorTracker pawn_MechanitorTracker = this.mechanitor;
			if (pawn_MechanitorTracker != null)
			{
				pawn_MechanitorTracker.Notify_PawnSpawned(respawningAfterLoad);
			}
			Pawn_MutantTracker pawn_MutantTracker = this.mutant;
			if (pawn_MutantTracker != null)
			{
				pawn_MutantTracker.Notify_Spawned(respawningAfterLoad);
			}
			Pawn_InfectionVectorTracker pawn_InfectionVectorTracker = this.infectionVectors;
			if (pawn_InfectionVectorTracker != null)
			{
				pawn_InfectionVectorTracker.NotifySpawned(respawningAfterLoad);
			}
			if (base.Faction == Faction.OfPlayer)
			{
				Ideo ideo = this.Ideo;
				if (ideo != null)
				{
					ideo.RecacheColonistBelieverCount();
				}
			}
			if (!respawningAfterLoad)
			{
				if ((base.Faction == Faction.OfPlayer || this.IsPlayerControlled) && base.Position.Fogged(map))
				{
					FloodFillerFog.FloodUnfog(base.Position, map);
				}
				Find.GameEnder.CheckOrUpdateGameOver();
				if (base.Faction == Faction.OfPlayer)
				{
					Find.StoryWatcher.statsRecord.UpdateGreatestPopulation();
					Find.World.StoryState.RecordPopulationIncrease();
				}
				if (!this.IsSubhuman)
				{
					PawnDiedOrDownedThoughtsUtility.RemoveDiedThoughts(this);
				}
				if (this.IsQuestLodger())
				{
					for (int i = this.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
					{
						if (this.health.hediffSet.hediffs[i].def.removeOnQuestLodgers)
						{
							this.health.RemoveHediff(this.health.hediffSet.hediffs[i]);
						}
					}
				}
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				if (this.IsPlayerControlled && base.PositionHeld.Fogged(base.Map))
				{
					FloodFillerFog.FloodUnfog(base.PositionHeld, base.Map);
				}
			});
			if (this.RaceProps.soundAmbience != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.sustainerAmbient = this.RaceProps.soundAmbience.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
				});
			}
			if (this.RaceProps.soundMoving != null)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.sustainerMoving = this.RaceProps.soundMoving.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
				});
			}
			if (this.Ideo != null && this.Ideo.hidden)
			{
				this.Ideo.hidden = false;
			}
		}

		// Token: 0x0600305B RID: 12379 RVA: 0x000F72D4 File Offset: 0x000F54D4
		public override void PostMake()
		{
			base.PostMake();
			this.canBeDormant = base.GetComp<CompCanBeDormant>();
			this.activity = base.GetComp<CompActivity>();
		}

		// Token: 0x0600305C RID: 12380 RVA: 0x000F72F4 File Offset: 0x000F54F4
		public override void PostMapInit()
		{
			base.PostMapInit();
			this.pather.TryResumePathingAfterLoading();
		}

		// Token: 0x0600305D RID: 12381 RVA: 0x000F7307 File Offset: 0x000F5507
		public void DrawShadowAt(Vector3 drawLoc)
		{
			this.Drawer.DrawShadowAt(drawLoc);
		}

		// Token: 0x0600305E RID: 12382 RVA: 0x000F7318 File Offset: 0x000F5518
		public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
		{
			base.DynamicDrawPhaseAt(phase, drawLoc, flip);
			this.Drawer.renderer.DynamicDrawPhaseAt(phase, drawLoc, null, false);
		}

		// Token: 0x0600305F RID: 12383 RVA: 0x000F734A File Offset: 0x000F554A
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.Comps_PostDraw();
			Pawn_MechanitorTracker pawn_MechanitorTracker = this.mechanitor;
			if (pawn_MechanitorTracker == null)
			{
				return;
			}
			pawn_MechanitorTracker.DrawCommandRadius();
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x000F7364 File Offset: 0x000F5564
		public override void DrawGUIOverlay()
		{
			this.Drawer.ui.DrawPawnGUIOverlay();
			for (int i = 0; i < base.AllComps.Count; i++)
			{
				base.AllComps[i].DrawGUIOverlay();
			}
			SilhouetteUtility.DrawGUISilhouette(this);
			if (DebugViewSettings.drawPatherState)
			{
				this.pather.DrawDebugGUI();
			}
		}

		// Token: 0x06003061 RID: 12385 RVA: 0x000F73C0 File Offset: 0x000F55C0
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.IsPlayerControlled)
			{
				PawnPath curPath = this.pather.curPath;
				if (curPath != null)
				{
					curPath.DrawPath(this);
				}
				this.jobs.DrawLinesBetweenTargets();
			}
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x000F73F4 File Offset: 0x000F55F4
		public override void TickRare()
		{
			base.TickRare();
			if (!this.Suspended)
			{
				Pawn_ApparelTracker pawn_ApparelTracker = this.apparel;
				if (pawn_ApparelTracker != null)
				{
					pawn_ApparelTracker.ApparelTrackerTickRare();
				}
			}
			Pawn_TrainingTracker pawn_TrainingTracker = this.training;
			if (pawn_TrainingTracker != null)
			{
				pawn_TrainingTracker.TrainingTrackerTickRare();
			}
			if (base.Spawned && this.RaceProps.IsFlesh && base.AmbientTemperature < 40f)
			{
				GenTemperature.PushHeat(this, 0.3f * this.BodySize * 4.1666665f * (this.def.race.Humanlike ? 1f : 0.6f));
			}
		}

		// Token: 0x06003063 RID: 12387 RVA: 0x000F748C File Offset: 0x000F568C
		protected override void Tick()
		{
			if (DebugSettings.noAnimals && base.Spawned && this.IsAnimal)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.Tick();
			if (this.IsHashIntervalTick(250))
			{
				this.TickRare();
			}
			bool suspended = this.Suspended;
			if (!suspended)
			{
				if (base.Spawned)
				{
					this.pather.PatherTick();
				}
				if (base.Spawned)
				{
					this.verbTracker.VerbsTick();
				}
				if (base.Spawned)
				{
					Pawn_RopeTracker pawn_RopeTracker = this.roping;
					if (pawn_RopeTracker != null)
					{
						pawn_RopeTracker.RopingTick();
					}
					Pawn_FlightTracker pawn_FlightTracker = this.flight;
					if (pawn_FlightTracker != null)
					{
						pawn_FlightTracker.FlightTick();
					}
					this.natives.NativeVerbsTick();
				}
				if (base.Spawned)
				{
					this.stances.StanceTrackerTick();
				}
				if (!this.IsWorldPawn())
				{
					Pawn_JobTracker pawn_JobTracker = this.jobs;
					if (pawn_JobTracker != null)
					{
						pawn_JobTracker.JobTrackerTick();
					}
				}
				this.health.HealthTick();
				if (base.Spawned && this.IsHiddenFromPlayer() && Find.Selector.IsSelected(this))
				{
					Find.Selector.Deselect(this);
				}
			}
			if (!suspended)
			{
				if (this.equipment != null)
				{
					using (ProfilerBlock.Scope("equipment"))
					{
						this.equipment.EquipmentTrackerTick();
					}
				}
				Pawn_AbilityTracker pawn_AbilityTracker = this.abilities;
				if (pawn_AbilityTracker != null)
				{
					pawn_AbilityTracker.AbilitiesTick();
				}
				Pawn_InventoryTracker pawn_InventoryTracker = this.inventory;
				if (pawn_InventoryTracker != null)
				{
					pawn_InventoryTracker.InventoryTrackerTick();
				}
				Pawn_GeneTracker pawn_GeneTracker = this.genes;
				if (pawn_GeneTracker != null)
				{
					pawn_GeneTracker.GeneTrackerTick();
				}
				if (ModsConfig.AnomalyActive && base.Spawned)
				{
					Pawn_MutantTracker pawn_MutantTracker = this.mutant;
					if (pawn_MutantTracker != null)
					{
						pawn_MutantTracker.MutantTrackerTick();
					}
					BloodRainUtility.BloodRainTick(this);
				}
			}
			if (base.Spawned && !base.Position.Fogged(base.Map))
			{
				if (this.RaceProps.soundAmbience != null && (this.sustainerAmbient == null || this.sustainerAmbient.Ended))
				{
					this.sustainerAmbient = this.RaceProps.soundAmbience.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
				}
				Sustainer sustainer = this.sustainerAmbient;
				if (sustainer != null)
				{
					sustainer.Maintain();
				}
				if (this.pather != null && this.pather.Moving && this.RaceProps.soundMoving != null)
				{
					if (this.sustainerMoving == null || this.sustainerMoving.Ended)
					{
						this.sustainerMoving = this.RaceProps.soundMoving.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
					}
					Sustainer sustainer2 = this.sustainerMoving;
					if (sustainer2 != null)
					{
						sustainer2.Maintain();
					}
				}
			}
			Pawn_DrawTracker pawn_DrawTracker = this.drawer;
			if (pawn_DrawTracker == null)
			{
				return;
			}
			pawn_DrawTracker.renderer.EffectersTick(suspended || this.IsWorldPawn());
		}

		// Token: 0x06003064 RID: 12388 RVA: 0x000F7738 File Offset: 0x000F5938
		protected override void TickInterval(int delta)
		{
			if (DebugSettings.noAnimals && base.Spawned && this.IsAnimal)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.TickInterval(delta);
			bool suspended = this.Suspended;
			if (!suspended)
			{
				if (!this.IsWorldPawn())
				{
					using (ProfilerBlock.Scope("jobs interval"))
					{
						Pawn_JobTracker pawn_JobTracker = this.jobs;
						if (pawn_JobTracker != null)
						{
							pawn_JobTracker.JobTrackerTickInterval(delta);
						}
					}
				}
				using (ProfilerBlock.Scope("health interval"))
				{
					this.health.HealthTickInterval(delta);
				}
				if (!this.Dead)
				{
					using (ProfilerBlock.Scope("mind state interval"))
					{
						this.mindState.MindStateTickInterval(delta);
					}
					this.carryTracker.CarryHandsTickInterval(delta);
					if (!base.InCryptosleep && this.RaceProps.Humanlike)
					{
						Pawn_InfectionVectorTracker pawn_InfectionVectorTracker = this.infectionVectors;
						if (pawn_InfectionVectorTracker != null)
						{
							pawn_InfectionVectorTracker.InfectionTickInterval(delta);
						}
					}
					if (this.showNamePromptOnTick != -1 && this.showNamePromptOnTick == Find.TickManager.TicksGame)
					{
						Find.WindowStack.Add(this.NamePawnDialog(null));
					}
				}
			}
			if (!base.Spawned)
			{
				Thing firstParentThing = ThingOwnerUtility.GetFirstParentThing(this);
				if (firstParentThing != null)
				{
					PawnUtility.GainComfortFromThingIfPossible(this, firstParentThing, delta);
				}
			}
			if (!this.Dead)
			{
				this.needs.NeedsTrackerTickInterval(delta);
			}
			if (!suspended)
			{
				Pawn_ApparelTracker pawn_ApparelTracker = this.apparel;
				if (pawn_ApparelTracker != null)
				{
					pawn_ApparelTracker.ApparelTrackerTickInterval(delta);
				}
				if (this.interactions != null && base.Spawned)
				{
					using (ProfilerBlock.Scope("interactions"))
					{
						this.interactions.InteractionsTrackerTickInterval(delta);
					}
				}
				Pawn_CallTracker pawn_CallTracker = this.caller;
				if (pawn_CallTracker != null)
				{
					pawn_CallTracker.CallTrackerTickInterval(delta);
				}
				Pawn_SkillTracker pawn_SkillTracker = this.skills;
				if (pawn_SkillTracker != null)
				{
					pawn_SkillTracker.SkillsTickInterval(delta);
				}
				Pawn_DraftController pawn_DraftController = this.drafter;
				if (pawn_DraftController != null)
				{
					pawn_DraftController.DraftControllerTickInterval(delta);
				}
				Pawn_RelationsTracker pawn_RelationsTracker = this.relations;
				if (pawn_RelationsTracker != null)
				{
					pawn_RelationsTracker.RelationsTrackerTickInterval(delta);
				}
				if (ModsConfig.RoyaltyActive && this.psychicEntropy != null)
				{
					this.psychicEntropy.PsychicEntropyTrackerTickInterval(delta);
				}
				if (this.RaceProps.Humanlike)
				{
					this.guest.GuestTrackerTickInterval(delta);
				}
				Pawn_IdeoTracker pawn_IdeoTracker = this.ideo;
				if (pawn_IdeoTracker != null)
				{
					pawn_IdeoTracker.IdeoTrackerTickInterval(delta);
				}
				Pawn_GeneTracker pawn_GeneTracker = this.genes;
				if (pawn_GeneTracker != null)
				{
					pawn_GeneTracker.GeneTrackerTickInterval(delta);
				}
				if (this.royalty != null && ModsConfig.RoyaltyActive)
				{
					this.royalty.RoyaltyTrackerTickInterval(delta);
				}
				if (this.style != null && ModsConfig.IdeologyActive)
				{
					this.style.StyleTrackerTickInterval(delta);
				}
				if (this.styleObserver != null && ModsConfig.IdeologyActive)
				{
					this.styleObserver.StyleObserverTickInterval(delta);
				}
				if (this.surroundings != null && ModsConfig.IdeologyActive)
				{
					this.surroundings.SurroundingsTrackerTickInterval(delta);
				}
				if (ModsConfig.BiotechActive)
				{
					Pawn_LearningTracker pawn_LearningTracker = this.learning;
					if (pawn_LearningTracker != null)
					{
						pawn_LearningTracker.LearningTickInterval(delta);
					}
					PollutionUtility.PawnPollutionTickInterval(this, delta);
				}
				GasUtility.PawnGasEffectsTickInterval(this, delta);
				ToxicUtility.PawnToxicTickInterval(this, delta);
				VacuumUtility.PawnVacuumTickInterval(this, delta);
				if (ModsConfig.AnomalyActive && base.Spawned)
				{
					Pawn_MutantTracker pawn_MutantTracker = this.mutant;
					if (pawn_MutantTracker != null)
					{
						pawn_MutantTracker.MutantTrackerTickInterval(delta);
					}
					Pawn_CreepJoinerTracker pawn_CreepJoinerTracker = this.creepjoiner;
					if (pawn_CreepJoinerTracker != null)
					{
						pawn_CreepJoinerTracker.TickInterval(delta);
					}
				}
				if (!this.IsMutant || !this.mutant.Def.disableAging)
				{
					this.ageTracker.AgeTickInterval(delta);
				}
				this.records.RecordsTickInterval(delta);
			}
			Pawn_GuiltTracker pawn_GuiltTracker = this.guilt;
			if (pawn_GuiltTracker == null)
			{
				return;
			}
			pawn_GuiltTracker.GuiltTrackerTickInterval(delta);
		}

		// Token: 0x06003065 RID: 12389 RVA: 0x000F7AD0 File Offset: 0x000F5CD0
		public void ProcessPostTickVisuals(int ticksPassed, CellRect viewRect)
		{
			if (!this.Suspended && base.Spawned)
			{
				if (Current.ProgramState != ProgramState.Playing || viewRect.Contains(base.Position))
				{
					this.Drawer.ProcessPostTickVisuals(ticksPassed);
				}
				this.rotationTracker.ProcessPostTickVisuals(ticksPassed);
			}
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x000F7B1C File Offset: 0x000F5D1C
		public void TickMothballed(int interval)
		{
			if (!this.Suspended)
			{
				this.ageTracker.AgeTickMothballed(interval);
				this.records.RecordsTickMothballed(interval);
			}
		}

		// Token: 0x06003067 RID: 12391 RVA: 0x000F7B40 File Offset: 0x000F5D40
		public void Notify_Teleported(bool endCurrentJob = true, bool resetTweenedPos = true)
		{
			if (resetTweenedPos)
			{
				this.Drawer.tweener.Notify_Teleported();
			}
			this.pather.Notify_Teleported_Int();
			if (endCurrentJob)
			{
				Pawn_JobTracker pawn_JobTracker = this.jobs;
				if (((pawn_JobTracker != null) ? pawn_JobTracker.curJob : null) != null)
				{
					this.jobs.EndCurrentJob(JobCondition.InterruptForced, this.jobs.curJob.startTick != GenTicks.TicksGame, true);
				}
			}
		}

		// Token: 0x06003068 RID: 12392 RVA: 0x000F7BAC File Offset: 0x000F5DAC
		public virtual SurgicalInspectionOutcome DoSurgicalInspection(Pawn surgeon, out string desc)
		{
			if (!ModsConfig.AnomalyActive)
			{
				desc = "";
				return SurgicalInspectionOutcome.Nothing;
			}
			bool flag = false;
			bool flag2 = false;
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = this.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
			{
				Hediff hediff = this.health.hediffSet.hediffs[i];
				HediffComp_SurgeryInspectable hediffComp_SurgeryInspectable;
				if (hediff.TryGetComp(out hediffComp_SurgeryInspectable))
				{
					if (hediff.Visible)
					{
						hediffComp_SurgeryInspectable.DoSurgicalInspectionVisible(surgeon);
						if (hediffComp_SurgeryInspectable.Props.preventLetterIfPreviouslyDetected)
						{
							flag2 = true;
						}
					}
					else
					{
						SurgicalInspectionOutcome surgicalInspectionOutcome = hediffComp_SurgeryInspectable.DoSurgicalInspection(surgeon);
						if (surgicalInspectionOutcome == SurgicalInspectionOutcome.DetectedNoLetter)
						{
							flag2 = true;
							hediff.SetVisible();
						}
						else if (surgicalInspectionOutcome == SurgicalInspectionOutcome.Detected)
						{
							flag = true;
							if (!string.IsNullOrEmpty(hediffComp_SurgeryInspectable.Props.surgicalDetectionDesc))
							{
								stringBuilder.Append("\n\n" + hediffComp_SurgeryInspectable.Props.surgicalDetectionDesc.Formatted(this.Named("PAWN"), surgeon.Named("SURGEON")));
							}
							hediff.SetVisible();
						}
					}
				}
			}
			if (this.IsCreepJoiner && this.creepjoiner.DoSurgicalInspection(surgeon, stringBuilder))
			{
				flag = true;
			}
			desc = stringBuilder.ToString();
			if (flag2)
			{
				return SurgicalInspectionOutcome.DetectedNoLetter;
			}
			if (!flag)
			{
				return SurgicalInspectionOutcome.Nothing;
			}
			return SurgicalInspectionOutcome.Detected;
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x000F7CE8 File Offset: 0x000F5EE8
		public void Notify_BecameVisible()
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].Notify_BecameVisible();
			}
		}

		// Token: 0x0600306A RID: 12394 RVA: 0x000F7D1C File Offset: 0x000F5F1C
		public void Notify_BecameInvisible()
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].Notify_BecameInvisible();
			}
		}

		// Token: 0x0600306B RID: 12395 RVA: 0x000F7D50 File Offset: 0x000F5F50
		public void Notify_ForcedVisible()
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].Notify_ForcedVisible();
			}
		}

		// Token: 0x0600306C RID: 12396 RVA: 0x000F7D84 File Offset: 0x000F5F84
		public void Notify_PassedToWorld()
		{
			if (((base.Faction == null && this.RaceProps.Humanlike) || (base.Faction != null && base.Faction.IsPlayer) || base.Faction == Faction.OfAncients || base.Faction == Faction.OfAncientsHostile) && !this.Dead && Find.WorldPawns.GetSituation(this) == WorldPawnSituation.Free)
			{
				bool tryMedievalOrBetter = base.Faction != null && base.Faction.def.techLevel >= TechLevel.Medieval;
				Faction faction;
				if (this.HasExtraHomeFaction(null) && !this.GetExtraHomeFaction(null).IsPlayer)
				{
					if (base.Faction != this.GetExtraHomeFaction(null))
					{
						this.SetFaction(this.GetExtraHomeFaction(null), null);
					}
				}
				else if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, tryMedievalOrBetter, false, TechLevel.Undefined, TechLevel.Undefined, false, false))
				{
					if (base.Faction != faction)
					{
						this.SetFaction(faction, null);
					}
				}
				else if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, tryMedievalOrBetter, true, TechLevel.Undefined, TechLevel.Undefined, false, false))
				{
					if (base.Faction != faction)
					{
						this.SetFaction(faction, null);
					}
				}
				else if (base.Faction != null)
				{
					this.SetFaction(null, null);
				}
			}
			this.becameWorldPawnTickAbs = GenTicks.TicksAbs;
			if (!this.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(this))
			{
				this.ClearMind_NewTemp(false, false, true, false);
			}
			Pawn_RelationsTracker pawn_RelationsTracker = this.relations;
			if (pawn_RelationsTracker != null)
			{
				pawn_RelationsTracker.Notify_PassedToWorld();
			}
			foreach (ThingComp thingComp in base.AllComps)
			{
				thingComp.Notify_PassedToWorld();
			}
			Pawn_DrawTracker pawn_DrawTracker = this.drawer;
			if (pawn_DrawTracker == null)
			{
				return;
			}
			PawnRenderer renderer = pawn_DrawTracker.renderer;
			if (renderer == null)
			{
				return;
			}
			PawnRenderTree renderTree = renderer.renderTree;
			if (renderTree == null)
			{
				return;
			}
			renderTree.SetDirty();
		}

		// Token: 0x0600306D RID: 12397 RVA: 0x000F7F48 File Offset: 0x000F6148
		public override void Notify_LeftBehind()
		{
			base.Notify_LeftBehind();
			Pawn_RelationsTracker pawn_RelationsTracker = this.relations;
			if (pawn_RelationsTracker == null)
			{
				return;
			}
			pawn_RelationsTracker.Notify_PawnLeftBehind();
		}

		// Token: 0x0600306E RID: 12398 RVA: 0x000F7F60 File Offset: 0x000F6160
		public void Notify_AddBedThoughts()
		{
			foreach (ThingComp thingComp in base.AllComps)
			{
				thingComp.Notify_AddBedThoughts(this);
			}
			Ideo ideo = this.Ideo;
			if (ideo == null)
			{
				return;
			}
			ideo.Notify_AddBedThoughts(this);
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x000F7FC4 File Offset: 0x000F61C4
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			float num = 1f;
			if (ModsConfig.BiotechActive && this.genes != null)
			{
				num *= this.genes.FactorForDamage(dinfo);
			}
			num *= this.health.FactorForDamage(dinfo);
			dinfo.SetAmount(dinfo.Amount * num);
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			this.health.PreApplyDamage(dinfo, out absorbed);
		}

		// Token: 0x06003070 RID: 12400 RVA: 0x000F803C File Offset: 0x000F623C
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (dinfo.Def.ExternalViolenceFor(this))
			{
				this.records.AddTo(RecordDefOf.DamageTaken, totalDamageDealt);
			}
			if (dinfo.Def.makesBlood && this.health.CanBleed && !dinfo.InstantPermanentInjury && totalDamageDealt > 0f && Rand.Chance(0.5f))
			{
				this.health.DropBloodFilth();
			}
			this.health.PostApplyDamage(dinfo, totalDamageDealt);
			if (!this.Dead)
			{
				this.mindState.Notify_DamageTaken(dinfo);
				if (ModsConfig.AnomalyActive)
				{
					Pawn pawn = dinfo.Instigator as Pawn;
					if (pawn != null)
					{
						List<InfectionPathwayDef> list = dinfo.Def.isRanged ? pawn.kindDef.rangedAttackInfectionPathways : pawn.kindDef.meleeAttackInfectionPathways;
						if (list != null)
						{
							InfectionPathwayUtility.AddInfectionPathways(list, this, pawn);
						}
					}
				}
			}
		}

		// Token: 0x06003071 RID: 12401 RVA: 0x000F811E File Offset: 0x000F631E
		public override Thing SplitOff(int count)
		{
			if (count <= 0 || count >= this.stackCount)
			{
				return base.SplitOff(count);
			}
			throw new NotImplementedException("Split off on Pawns is not supported (unless we're taking a full stack).");
		}

		// Token: 0x06003072 RID: 12402 RVA: 0x000F8140 File Offset: 0x000F6340
		private float TicksPerMove(bool diagonal)
		{
			float num = this.GetStatValue(StatDefOf.MoveSpeed, true, -1);
			if (this.Downed && this.health.CanCrawl)
			{
				num = this.GetStatValue(StatDefOf.CrawlSpeed, true, -1);
			}
			if (RestraintsUtility.InRestraints(this))
			{
				num *= 0.35f;
			}
			Pawn_CarryTracker pawn_CarryTracker = this.carryTracker;
			if (((pawn_CarryTracker != null) ? pawn_CarryTracker.CarriedThing : null) != null && this.carryTracker.CarriedThing.def.category == ThingCategory.Pawn)
			{
				num *= 0.6f;
			}
			float num2 = num / 60f;
			float num3;
			if (num2 == 0f)
			{
				num3 = 450f;
			}
			else
			{
				num3 = 1f / num2;
				if (base.Spawned && !base.Map.roofGrid.Roofed(base.Position))
				{
					num3 /= base.Map.weatherManager.CurMoveSpeedMultiplier;
				}
				if (diagonal)
				{
					num3 *= 1.41421f;
				}
			}
			num3 = Mathf.Clamp(num3, 1f, 450f);
			if (this.debugMaxMoveSpeed)
			{
				return 1f;
			}
			return num3;
		}

		// Token: 0x06003073 RID: 12403 RVA: 0x000F8240 File Offset: 0x000F6440
		private void DoKillSideEffects(DamageInfo? dinfo, Hediff exactCulprit, bool spawned)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.Storyteller.Notify_PawnEvent(this, AdaptationEvent.Died, null);
			}
			if (this.IsColonist && !this.wasLeftBehindStartingPawn)
			{
				Find.StoryWatcher.statsRecord.Notify_ColonistKilled();
			}
			if (spawned && ((dinfo != null && dinfo.Value.Def.ExternalViolenceFor(this)) || (((exactCulprit != null) ? exactCulprit.sourceDef : null) != null && exactCulprit.sourceDef.IsWeapon)))
			{
				LifeStageUtility.PlayNearestLifestageSound(this, (LifeStageAge lifeStage) => lifeStage.soundDeath, (GeneDef gene) => gene.soundDeath, (MutantDef mutantDef) => mutantDef.soundDeath, 1f);
			}
			if (((dinfo != null) ? dinfo.GetValueOrDefault().Instigator : null) != null)
			{
				Pawn pawn = dinfo.Value.Instigator as Pawn;
				if (pawn != null)
				{
					RecordsUtility.Notify_PawnKilled(this, pawn);
					Pawn_EquipmentTracker pawn_EquipmentTracker = pawn.equipment;
					if (pawn_EquipmentTracker != null)
					{
						pawn_EquipmentTracker.Notify_KilledPawn();
					}
					Need_KillThirst need_KillThirst;
					if (this.RaceProps.Humanlike && pawn.needs != null && pawn.needs.TryGetNeed<Need_KillThirst>(out need_KillThirst))
					{
						need_KillThirst.Notify_KilledPawn(dinfo);
					}
					if (pawn.health.hediffSet != null)
					{
						for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
						{
							pawn.health.hediffSet.hediffs[i].Notify_KilledPawn(this, dinfo);
						}
					}
					if (HistoryEventUtility.IsKillingInnocentAnimal(pawn, this))
					{
						Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.KilledInnocentAnimal, pawn.Named(HistoryEventArgsNames.Doer), this.Named(HistoryEventArgsNames.Victim)), true);
					}
				}
			}
			TaleUtility.Notify_PawnDied(this, dinfo);
			if (spawned)
			{
				Find.BattleLog.Add(new BattleLogEntry_StateTransition(this, this.RaceProps.DeathActionWorker.DeathRules, ((dinfo != null) ? dinfo.GetValueOrDefault().Instigator : null) as Pawn, exactCulprit, (dinfo != null) ? dinfo.GetValueOrDefault().HitPart : null));
			}
		}

		// Token: 0x06003074 RID: 12404 RVA: 0x000F849C File Offset: 0x000F669C
		private void PreDeathPawnModifications(DamageInfo? dinfo, Map map)
		{
			this.health.surgeryBills.Clear();
			for (int i = 0; i < this.health.hediffSet.hediffs.Count; i++)
			{
				this.health.hediffSet.hediffs[i].Notify_PawnKilled();
			}
			Pawn_ApparelTracker pawn_ApparelTracker = this.apparel;
			if (pawn_ApparelTracker != null)
			{
				pawn_ApparelTracker.Notify_PawnKilled(dinfo);
			}
			Pawn_RelationsTracker pawn_RelationsTracker = this.relations;
			if (pawn_RelationsTracker != null)
			{
				pawn_RelationsTracker.Notify_PawnKilled(dinfo, map);
			}
			Pawn_ConnectionsTracker pawn_ConnectionsTracker = this.connections;
			if (pawn_ConnectionsTracker != null)
			{
				pawn_ConnectionsTracker.Notify_PawnKilled();
			}
			this.meleeVerbs.Notify_PawnKilled();
		}

		// Token: 0x06003075 RID: 12405 RVA: 0x000F8538 File Offset: 0x000F6738
		private void DropBeforeDying(DamageInfo? dinfo, ref Map map, ref bool spawned)
		{
			Pawn_CarryTracker pawn_CarryTracker = base.ParentHolder as Pawn_CarryTracker;
			Thing thing;
			if (pawn_CarryTracker != null && this.holdingOwner.TryDrop(this, pawn_CarryTracker.pawn.Position, pawn_CarryTracker.pawn.Map, ThingPlaceMode.Near, out thing, null, null, true))
			{
				map = pawn_CarryTracker.pawn.Map;
				spawned = true;
			}
			PawnDiedOrDownedThoughtsUtility.RemoveLostThoughts(this);
			PawnDiedOrDownedThoughtsUtility.RemoveResuedRelativeThought(this);
			PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(this, dinfo, PawnDiedOrDownedThoughtsKind.Died);
			if (this.IsAnimal)
			{
				PawnDiedOrDownedThoughtsUtility.GiveVeneratedAnimalDiedThoughts(this, map);
			}
		}

		// Token: 0x06003076 RID: 12406 RVA: 0x000F85B4 File Offset: 0x000F67B4
		private void RemoveFromHoldingContainer(ref Map map, ref bool spawned, DamageInfo? dinfo)
		{
			if (ModsConfig.AnomalyActive)
			{
				Building_HoldingPlatform building_HoldingPlatform = base.ParentHolder as Building_HoldingPlatform;
				if (building_HoldingPlatform != null && building_HoldingPlatform.Spawned)
				{
					building_HoldingPlatform.Notify_PawnDied(this, dinfo);
					spawned = true;
					map = building_HoldingPlatform.Map;
				}
			}
			CompTransporter compTransporter = base.ParentHolder as CompTransporter;
			if (compTransporter != null)
			{
				Thing thing;
				compTransporter.innerContainer.TryDrop(this, ThingPlaceMode.Near, out thing, null, null);
			}
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x000F8614 File Offset: 0x000F6814
		public override void Kill(DamageInfo? dinfo, Hediff exactCulprit = null)
		{
			int num = 0;
			this.health.isBeingKilled = true;
			try
			{
				num = 1;
				IntVec3 positionHeld = base.PositionHeld;
				Map map = base.Map;
				Map map2 = this.prevMap = base.MapHeld;
				Lord prevLord = this.GetLord();
				bool spawned = base.Spawned;
				bool spawnedOrAnyParentSpawned = base.SpawnedOrAnyParentSpawned;
				bool flag = this.IsWorldPawn();
				Pawn_GuiltTracker pawn_GuiltTracker = this.guilt;
				bool? flag2 = (pawn_GuiltTracker != null) ? new bool?(pawn_GuiltTracker.IsGuilty) : null;
				Caravan caravan = this.GetCaravan();
				bool isShambler = this.IsShambler;
				Building_Grave assignedGrave = null;
				if (this.ownership != null)
				{
					assignedGrave = this.ownership.AssignedGrave;
				}
				Building_Bed currentBed = this.CurrentBed();
				this.RemoveFromHoldingContainer(ref map, ref spawned, dinfo);
				ThingOwner thingOwner = null;
				bool inContainerEnclosed = this.InContainerEnclosed;
				if (inContainerEnclosed)
				{
					thingOwner = this.holdingOwner;
					thingOwner.Remove(this);
				}
				bool flag3 = false;
				bool flag4 = false;
				bool flag5 = false;
				if (Current.ProgramState == ProgramState.Playing && map != null)
				{
					flag3 = (map.designationManager.DesignationOn(this, DesignationDefOf.Hunt) != null);
					flag4 = this.ShouldBeSlaughtered();
					foreach (Lord lord in map.lordManager.lords)
					{
						LordJob_Ritual lordJob_Ritual = lord.LordJob as LordJob_Ritual;
						if (lordJob_Ritual != null && lordJob_Ritual.pawnsDeathIgnored.Contains(this))
						{
							flag5 = true;
							break;
						}
					}
				}
				bool flag6 = PawnUtility.ShouldSendNotificationAbout(this) && ((!flag4 && !flag5) || dinfo == null || dinfo.Value.Def != DamageDefOf.ExecutionCut) && !this.ForceNoDeathNotification;
				num = 2;
				this.DoKillSideEffects(dinfo, exactCulprit, spawned);
				num = 3;
				this.PreDeathPawnModifications(dinfo, map);
				num = 4;
				this.DropBeforeDying(dinfo, ref map, ref spawned);
				num = 5;
				this.health.SetDead();
				if (this.health.deflectionEffecter != null)
				{
					this.health.deflectionEffecter.Cleanup();
					this.health.deflectionEffecter = null;
				}
				if (this.health.woundedEffecter != null)
				{
					this.health.woundedEffecter.Cleanup();
					this.health.woundedEffecter = null;
				}
				if (caravan != null)
				{
					caravan.Notify_MemberDied(this);
				}
				Lord lord2 = this.GetLord();
				if (lord2 != null)
				{
					lord2.Notify_PawnLost(this, PawnLostCondition.Killed, dinfo);
				}
				if (ModsConfig.AnomalyActive)
				{
					Find.Anomaly.Notify_PawnDied(this);
				}
				MeditationFocusTypeAvailabilityCache.Notify_PawnDiedOrDestroyed(this);
				bool flag7 = base.DeSpawnOrDeselect(DestroyMode.Vanish);
				if (this.royalty != null)
				{
					this.royalty.Notify_PawnKilled();
				}
				Corpse corpse = null;
				if (!PawnGenerator.IsPawnBeingGeneratedAndNotAllowsDead(this) && this.RaceProps.corpseDef != null)
				{
					if (inContainerEnclosed)
					{
						corpse = this.MakeCorpse(assignedGrave, currentBed);
						if (!thingOwner.TryAdd(corpse, true))
						{
							corpse.Destroy(DestroyMode.Vanish);
							corpse = null;
						}
					}
					else if (spawnedOrAnyParentSpawned)
					{
						if (this.holdingOwner != null)
						{
							this.holdingOwner.Remove(this);
						}
						corpse = this.MakeCorpse(assignedGrave, currentBed);
						if (GenPlace.TryPlaceThing(corpse, positionHeld, map2, ThingPlaceMode.Direct, null, null, null, 1) || GenPlace.TryPlaceThing(corpse, positionHeld, map2, ThingPlaceMode.Near, null, null, null, 1))
						{
							corpse.Rotation = base.Rotation;
							if (HuntJobUtility.WasKilledByHunter(this, dinfo))
							{
								((Pawn)dinfo.Value.Instigator).Reserve(corpse, ((Pawn)dinfo.Value.Instigator).CurJob, 1, -1, null, true, false);
							}
							else if (!flag3 && !flag4)
							{
								corpse.SetForbiddenIfOutsideHomeArea();
							}
							Fire fire = this.GetAttachment(ThingDefOf.Fire) as Fire;
							if (fire != null)
							{
								FireUtility.TryStartFireIn(corpse.Position, corpse.Map, fire.CurrentSize(), fire.instigator, null);
							}
						}
						else
						{
							corpse.Destroy(DestroyMode.Vanish);
							corpse = null;
						}
					}
					else if (caravan != null && caravan.Spawned)
					{
						corpse = this.MakeCorpse(assignedGrave, currentBed);
						caravan.AddPawnOrItem(corpse, true);
					}
					else if (this.holdingOwner != null || this.IsWorldPawn())
					{
						Corpse.PostCorpseDestroy(this, false);
					}
					else
					{
						corpse = this.MakeCorpse(assignedGrave, currentBed);
					}
				}
				if (spawned)
				{
					this.DropAndForbidEverything(false, false);
				}
				if (spawned)
				{
					GenLeaving.DoLeavingsFor(this, map, DestroyMode.KillFinalize, null);
				}
				if (corpse != null)
				{
					Hediff firstHediffOfDef = this.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup, false);
					Hediff firstHediffOfDef2 = this.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Scaria, false);
					CompRottable comp = corpse.GetComp<CompRottable>();
					if (comp != null && ((firstHediffOfDef != null && Rand.Value < firstHediffOfDef.Severity) || (firstHediffOfDef2 != null && Rand.Chance(Find.Storyteller.difficulty.scariaRotChance))))
					{
						comp.RotImmediately(RotStage.Rotting);
					}
					if (this.addCorpseToLord && lord2 != null)
					{
						lord2.AddCorpse(corpse);
					}
				}
				this.Drawer.renderer.SetAllGraphicsDirty();
				if (ModsConfig.AnomalyActive && this.kindDef == PawnKindDefOf.Revenant)
				{
					RevenantUtility.OnRevenantDeath(this, map);
				}
				Pawn_DuplicateTracker pawn_DuplicateTracker = this.duplicate;
				if (pawn_DuplicateTracker != null)
				{
					pawn_DuplicateTracker.Notify_PawnKilled();
				}
				this.Drawer.renderer.SetAnimation(null);
				if (!base.Destroyed)
				{
					base.Kill(dinfo, exactCulprit);
				}
				PawnComponentsUtility.RemoveComponentsOnKilled(this);
				this.health.hediffSet.DirtyCache();
				PortraitsCache.SetDirty(this);
				GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this);
				if (flag7 && corpse != null && !corpse.Destroyed)
				{
					Find.Selector.Select(corpse, false, false);
				}
				num = 6;
				this.health.hediffSet.Notify_PawnDied(dinfo, exactCulprit);
				if (this.IsMutant)
				{
					this.mutant.Notify_Died(corpse, dinfo, exactCulprit);
				}
				Pawn_GeneTracker pawn_GeneTracker = this.genes;
				if (pawn_GeneTracker != null)
				{
					pawn_GeneTracker.Notify_PawnDied(dinfo, exactCulprit);
				}
				Faction homeFaction = this.HomeFaction;
				if (homeFaction != null)
				{
					DamageInfo? dinfo2 = dinfo;
					bool wasWorldPawn = flag;
					bool? flag8 = flag2;
					bool flag9 = true;
					homeFaction.Notify_MemberDied(this, dinfo2, wasWorldPawn, flag8.GetValueOrDefault() == flag9 & flag8 != null, map2);
				}
				if (corpse != null)
				{
					if (this.RaceProps.DeathActionWorker != null && spawned && !isShambler)
					{
						this.RaceProps.DeathActionWorker.PawnDied(corpse, prevLord);
					}
					if (Find.Scenario != null)
					{
						Find.Scenario.Notify_PawnDied(corpse);
					}
				}
				if (base.Faction != null && base.Faction.IsPlayer)
				{
					BillUtility.Notify_ColonistUnavailable(this);
				}
				if (spawnedOrAnyParentSpawned)
				{
					GenHostility.Notify_PawnLostForTutor(this, map2);
				}
				if (base.Faction != null && base.Faction.IsPlayer && Current.ProgramState == ProgramState.Playing)
				{
					Find.ColonistBar.MarkColonistsDirty();
				}
				Pawn_PsychicEntropyTracker pawn_PsychicEntropyTracker = this.psychicEntropy;
				if (pawn_PsychicEntropyTracker != null)
				{
					pawn_PsychicEntropyTracker.Notify_PawnDied();
				}
				try
				{
					Ideo ideo = this.Ideo;
					if (ideo != null)
					{
						ideo.Notify_MemberDied(this);
					}
					Ideo ideo2 = this.Ideo;
					if (ideo2 != null)
					{
						ideo2.Notify_MemberLost(this, map);
					}
				}
				catch (Exception ex)
				{
					string str = "Error while notifying ideo of pawn death: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
				if (this.IsMutant && this.mutant.Def.clearMutantStatusOnDeath)
				{
					if (this.mutant.HasTurned)
					{
						this.mutant.Revert(true);
					}
					else
					{
						this.mutant = null;
					}
				}
				if (flag6)
				{
					this.health.NotifyPlayerOfKilled(dinfo, exactCulprit, caravan);
				}
				Find.QuestManager.Notify_PawnKilled(this, dinfo);
				Find.FactionManager.Notify_PawnKilled(this);
				Find.IdeoManager.Notify_PawnKilled(this);
				if (ModsConfig.BiotechActive && MechanitorUtility.IsMechanitor(this))
				{
					Find.History.Notify_MechanitorDied();
				}
				this.Notify_DisabledWorkTypesChanged();
				Find.BossgroupManager.Notify_PawnKilled(this);
				if (this.IsCreepJoiner)
				{
					this.creepjoiner.Notify_CreepJoinerKilled();
				}
				this.prevMap = null;
				this.health.isBeingKilled = false;
			}
			catch (Exception arg)
			{
				Log.Error(string.Format("Error while killing {0} during phase {1}: {2}", this.ToStringSafe<Pawn>(), num, arg));
			}
		}

		// Token: 0x06003078 RID: 12408 RVA: 0x000F8DFC File Offset: 0x000F6FFC
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.Vanish && mode != DestroyMode.KillFinalize)
			{
				Log.Error(string.Concat(new string[]
				{
					"Destroyed pawn ",
					(this != null) ? this.ToString() : null,
					" with unsupported mode ",
					mode.ToString(),
					"."
				}));
			}
			Map mapHeld = base.MapHeld;
			base.Destroy(mode);
			Find.WorldPawns.Notify_PawnDestroyed(this);
			if (this.ownership != null)
			{
				Building_Grave assignedGrave = this.ownership.AssignedGrave;
				this.ownership.UnclaimAll();
				if (mode == DestroyMode.KillFinalize && assignedGrave != null)
				{
					assignedGrave.CompAssignableToPawn.TryAssignPawn(this);
				}
			}
			this.ClearMind_NewTemp(false, true, true, false);
			Lord lord = this.GetLord();
			if (lord != null)
			{
				PawnLostCondition cond = (mode == DestroyMode.KillFinalize) ? PawnLostCondition.Killed : PawnLostCondition.Vanished;
				lord.Notify_PawnLost(this, cond, null);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.GameEnder.CheckOrUpdateGameOver();
				Find.TaleManager.Notify_PawnDestroyed(this);
			}
			foreach (Pawn pawn in from p in PawnsFinder.AllMapsWorldAndTemporary_Alive
			where p.playerSettings != null && p.playerSettings.Master == this
			select p)
			{
				pawn.playerSettings.Master = null;
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker = this.equipment;
			if (pawn_EquipmentTracker != null)
			{
				pawn_EquipmentTracker.Notify_PawnDied();
			}
			if (ModsConfig.AnomalyActive && Find.Anomaly != null)
			{
				Find.Anomaly.Notify_PawnDied(this);
			}
			if (mode != DestroyMode.KillFinalize)
			{
				Pawn_EquipmentTracker pawn_EquipmentTracker2 = this.equipment;
				if (pawn_EquipmentTracker2 != null)
				{
					pawn_EquipmentTracker2.DestroyAllEquipment(DestroyMode.Vanish);
				}
				Pawn_InventoryTracker pawn_InventoryTracker = this.inventory;
				if (pawn_InventoryTracker != null)
				{
					pawn_InventoryTracker.DestroyAll(DestroyMode.Vanish);
				}
				Pawn_ApparelTracker pawn_ApparelTracker = this.apparel;
				if (pawn_ApparelTracker != null)
				{
					pawn_ApparelTracker.DestroyAll(DestroyMode.Vanish);
				}
			}
			WorldPawns worldPawns = Find.WorldPawns;
			if (!worldPawns.IsBeingDiscarded(this) && !worldPawns.Contains(this))
			{
				worldPawns.PassToWorld(this, PawnDiscardDecideMode.Decide);
			}
			if (base.Faction.IsPlayerSafe())
			{
				Ideo ideo = this.Ideo;
				if (ideo != null)
				{
					ideo.RecacheColonistBelieverCount();
				}
			}
			Pawn_RelationsTracker pawn_RelationsTracker = this.relations;
			if (pawn_RelationsTracker != null)
			{
				pawn_RelationsTracker.Notify_PawnDestroyed(mode);
			}
			MeditationFocusTypeAvailabilityCache.Notify_PawnDiedOrDestroyed(this);
			Pawn_DrawTracker pawn_DrawTracker = this.Drawer;
			if (pawn_DrawTracker == null)
			{
				return;
			}
			PawnRenderer renderer = pawn_DrawTracker.renderer;
			if (renderer == null)
			{
				return;
			}
			PawnRenderTree renderTree = renderer.renderTree;
			if (renderTree == null)
			{
				return;
			}
			renderTree.SetDirty();
		}

		// Token: 0x06003079 RID: 12409 RVA: 0x000F9028 File Offset: 0x000F7228
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			Pawn_JobTracker pawn_JobTracker = this.jobs;
			if (((pawn_JobTracker != null) ? pawn_JobTracker.curJob : null) != null)
			{
				this.jobs.StopAll(false, true);
			}
			base.DeSpawn(mode);
			Pawn_PathFollower pawn_PathFollower = this.pather;
			if (pawn_PathFollower != null)
			{
				pawn_PathFollower.StopDead();
			}
			Pawn_RopeTracker pawn_RopeTracker = this.roping;
			if (pawn_RopeTracker != null)
			{
				pawn_RopeTracker.Notify_DeSpawned();
			}
			this.mindState.droppedWeapon = null;
			Pawn_NeedsTracker pawn_NeedsTracker = this.needs;
			if (pawn_NeedsTracker != null)
			{
				Need_Mood mood = pawn_NeedsTracker.mood;
				if (mood != null)
				{
					mood.thoughts.situational.Notify_SituationalThoughtsDirty();
				}
			}
			Pawn_MeleeVerbs pawn_MeleeVerbs = this.meleeVerbs;
			if (pawn_MeleeVerbs != null)
			{
				pawn_MeleeVerbs.Notify_PawnDespawned();
			}
			Pawn_MechanitorTracker pawn_MechanitorTracker = this.mechanitor;
			if (pawn_MechanitorTracker != null)
			{
				pawn_MechanitorTracker.Notify_DeSpawned(mode);
			}
			MeditationFocusTypeAvailabilityCache.Notify_PawnDiedOrDestroyed(this);
			this.ClearAllReservations(false);
			if (map != null)
			{
				map.mapPawns.DeRegisterPawn(this);
				map.autoSlaughterManager.Notify_PawnDespawned();
			}
			PawnComponentsUtility.RemoveComponentsOnDespawned(this);
			if (this.sustainerAmbient != null)
			{
				this.sustainerAmbient.End();
				this.sustainerAmbient = null;
			}
			if (this.sustainerMoving != null)
			{
				this.sustainerMoving.End();
				this.sustainerMoving = null;
			}
		}

		// Token: 0x0600307A RID: 12410 RVA: 0x000F913C File Offset: 0x000F733C
		public override void Discard(bool silentlyRemoveReferences = false)
		{
			if (Find.WorldPawns.Contains(this))
			{
				Log.Warning("Tried to discard a world pawn " + ((this != null) ? this.ToString() : null) + ".");
				return;
			}
			base.Discard(silentlyRemoveReferences);
			if (this.relations != null)
			{
				if (this.RaceProps.Humanlike)
				{
					if (this.relations.Children.Count((Pawn x) => !x.markedForDiscard) > 1)
					{
						foreach (Pawn pawn in this.relations.Children)
						{
							if (!pawn.markedForDiscard)
							{
								DirectPawnRelation directRelation = pawn.relations.GetDirectRelation(PawnRelationDefOf.Parent, this);
								pawn.relations.ElevateToVirtualRelation(directRelation);
							}
						}
					}
				}
				this.relations.ClearAllRelations();
			}
			if (this.pather != null)
			{
				this.pather.DisposeAndClearCurPathRequest();
				this.pather.DisposeAndClearCurPath();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.PlayLog.Notify_PawnDiscarded(this, silentlyRemoveReferences);
				Find.BattleLog.Notify_PawnDiscarded(this, silentlyRemoveReferences);
				Find.TaleManager.Notify_PawnDiscarded(this, silentlyRemoveReferences);
				Find.QuestManager.Notify_PawnDiscarded(this);
			}
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_Alive)
			{
				Pawn_NeedsTracker pawn_NeedsTracker = pawn2.needs;
				if (pawn_NeedsTracker != null)
				{
					Need_Mood mood = pawn_NeedsTracker.mood;
					if (mood != null)
					{
						mood.thoughts.memories.Notify_PawnDiscarded(this);
					}
				}
			}
			Corpse.PostCorpseDestroy(this, true);
		}

		// Token: 0x0600307B RID: 12411 RVA: 0x000F92F8 File Offset: 0x000F74F8
		public Corpse MakeCorpse(Building_Grave assignedGrave, Building_Bed currentBed)
		{
			return this.MakeCorpse(assignedGrave, currentBed != null, (currentBed != null) ? currentBed.Rotation.AsAngle : 0f);
		}

		// Token: 0x0600307C RID: 12412 RVA: 0x000F9328 File Offset: 0x000F7528
		public Corpse MakeCorpse(Building_Grave assignedGrave, bool inBed, float bedRotation)
		{
			if (this.holdingOwner != null)
			{
				string str = "We can't make corpse because the pawn is in a ThingOwner. Remove him from the container first. This should have been already handled before calling this method. holder=";
				IThingHolder parentHolder = base.ParentHolder;
				Log.Warning(str + ((parentHolder != null) ? parentHolder.ToString() : null));
				return null;
			}
			if (this.RaceProps.corpseDef == null)
			{
				return null;
			}
			Corpse corpse = (Corpse)ThingMaker.MakeThing(this.RaceProps.corpseDef, null);
			corpse.InnerPawn = this;
			if (assignedGrave != null)
			{
				corpse.InnerPawn.ownership.ClaimGrave(assignedGrave);
			}
			if (inBed)
			{
				corpse.InnerPawn.Drawer.renderer.wiggler.SetToCustomRotation(bedRotation + 180f);
			}
			return corpse;
		}

		// Token: 0x0600307D RID: 12413 RVA: 0x000F93C8 File Offset: 0x000F75C8
		public virtual void ExitMap(bool allowedToJoinOrCreateCaravan, Rot4 exitDir)
		{
			if (this.IsWorldPawn())
			{
				Log.Warning("Called ExitMap() on world pawn " + ((this != null) ? this.ToString() : null));
				return;
			}
			Ideo ideo = this.Ideo;
			if (ideo != null)
			{
				ideo.Notify_MemberLost(this, base.Map);
			}
			if (allowedToJoinOrCreateCaravan && CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this))
			{
				CaravanExitMapUtility.ExitMapAndJoinOrCreateCaravan(this, exitDir);
				return;
			}
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_PawnLost(this, PawnLostCondition.ExitedMap, null);
			}
			Pawn_CarryTracker pawn_CarryTracker = this.carryTracker;
			if (((pawn_CarryTracker != null) ? pawn_CarryTracker.CarriedThing : null) != null)
			{
				Pawn pawn = this.carryTracker.CarriedThing as Pawn;
				if (pawn != null)
				{
					if (base.Faction != null && base.Faction != pawn.Faction)
					{
						base.Faction.kidnapped.Kidnap(pawn, this);
					}
					else
					{
						if (!this.teleporting)
						{
							this.carryTracker.innerContainer.Remove(pawn);
						}
						pawn.teleporting = this.teleporting;
						pawn.ExitMap(false, exitDir);
						pawn.teleporting = false;
					}
				}
				else
				{
					this.carryTracker.CarriedThing.Destroy(DestroyMode.Vanish);
				}
				if (!this.teleporting || pawn == null)
				{
					this.carryTracker.innerContainer.Clear();
				}
			}
			bool flag = ThingOwnerUtility.AnyParentIs<ActiveTransporterInfo>(this) || ThingOwnerUtility.AnyParentIs<TravellingTransporters>(this);
			bool flag2 = this.IsCaravanMember() || this.teleporting || flag;
			bool flag3 = !flag2 || (!this.IsPrisoner && !this.IsSlave && !flag) || (this.guest != null && this.guest.Released);
			bool flag4 = flag3 && (this.IsPrisoner || this.IsSlave) && this.guest != null && this.guest.Released;
			bool flag5 = flag4 || (this.guest != null && this.guest.HostFaction == Faction.OfPlayer);
			if (flag3 && !flag2)
			{
				foreach (Thing thing in this.EquippedWornOrInventoryThings)
				{
					Precept_ThingStyle styleSourcePrecept = thing.GetStyleSourcePrecept();
					if (styleSourcePrecept != null)
					{
						styleSourcePrecept.Notify_ThingLost(thing, false);
					}
				}
			}
			Faction faction = base.Faction;
			if (faction != null)
			{
				faction.Notify_MemberExitedMap(this, flag4);
			}
			if (base.Faction == Faction.OfPlayer && this.IsSlave && this.SlaveFaction != null && this.SlaveFaction != Faction.OfPlayer && this.guest.Released)
			{
				this.SlaveFaction.Notify_MemberExitedMap(this, flag4);
			}
			if (this.ownership != null && flag5)
			{
				this.ownership.UnclaimAll();
			}
			if (this.guest != null)
			{
				bool isPrisonerOfColony = this.IsPrisonerOfColony;
				if (flag4)
				{
					this.guest.SetGuestStatus(null, RimWorld.GuestStatus.Guest);
				}
				if (isPrisonerOfColony)
				{
					this.guest.SetNoInteraction();
					if (!this.guest.Released && flag3)
					{
						GuestUtility.Notify_PrisonerEscaped(this);
					}
				}
				this.guest.Released = false;
			}
			base.DeSpawnOrDeselect(DestroyMode.Vanish);
			this.inventory.UnloadEverything = false;
			if (flag3)
			{
				this.ClearMind_NewTemp(false, false, true, false);
			}
			Pawn_RelationsTracker pawn_RelationsTracker = this.relations;
			if (pawn_RelationsTracker != null)
			{
				pawn_RelationsTracker.Notify_ExitedMap();
			}
			Find.WorldPawns.PassToWorld(this, PawnDiscardDecideMode.Decide);
			QuestUtility.SendQuestTargetSignals(this.questTags, "LeftMap", this.Named("SUBJECT"));
			Find.FactionManager.Notify_PawnLeftMap(this);
			Find.IdeoManager.Notify_PawnLeftMap(this);
		}

		// Token: 0x0600307E RID: 12414 RVA: 0x000F9730 File Offset: 0x000F7930
		public override void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			base.PreTraded(action, playerNegotiator, trader);
			if (base.SpawnedOrAnyParentSpawned)
			{
				this.DropAndForbidEverything(false, false);
			}
			Pawn_Ownership pawn_Ownership = this.ownership;
			if (pawn_Ownership != null)
			{
				pawn_Ownership.UnclaimAll();
			}
			if (action == TradeAction.PlayerSells)
			{
				Faction faction = this.GetExtraHomeFaction(null) ?? this.GetExtraHostFaction(null);
				if (faction != null && faction != Faction.OfPlayer)
				{
					Faction.OfPlayer.TryAffectGoodwillWith(faction, Faction.OfPlayer.GoodwillToMakeHostile(faction), true, true, HistoryEventDefOf.MemberSold, new GlobalTargetInfo?(this));
				}
			}
			Pawn_GuestTracker pawn_GuestTracker = this.guest;
			if (pawn_GuestTracker != null)
			{
				pawn_GuestTracker.SetGuestStatus(null, RimWorld.GuestStatus.Guest);
			}
			if (action == TradeAction.PlayerBuys)
			{
				if (this.guest != null && this.guest.joinStatus == JoinStatus.JoinAsSlave)
				{
					this.guest.SetGuestStatus(Faction.OfPlayer, RimWorld.GuestStatus.Slave);
				}
				else
				{
					Need_Mood mood = this.needs.mood;
					if (mood != null)
					{
						mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FreedFromSlavery, null, null);
					}
					this.SetFaction(Faction.OfPlayer, null);
				}
			}
			else if (action == TradeAction.PlayerSells)
			{
				if (this.RaceProps.Humanlike)
				{
					TaleRecorder.RecordTale(TaleDefOf.SoldPrisoner, new object[]
					{
						playerNegotiator,
						this,
						trader
					});
				}
				if (base.Faction != null)
				{
					this.SetFaction(null, null);
				}
				if (this.RaceProps.IsFlesh)
				{
					this.relations.Notify_PawnSold(playerNegotiator);
				}
			}
			this.ClearMind_NewTemp(false, false, true, false);
		}

		// Token: 0x0600307F RID: 12415 RVA: 0x000F988C File Offset: 0x000F7A8C
		public void PreKidnapped(Pawn kidnapper)
		{
			Find.Storyteller.Notify_PawnEvent(this, AdaptationEvent.Kidnapped, null);
			if (this.IsColonist && kidnapper != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.KidnappedColonist, new object[]
				{
					kidnapper,
					this
				});
			}
			Pawn_Ownership pawn_Ownership = this.ownership;
			if (pawn_Ownership != null)
			{
				pawn_Ownership.UnclaimAll();
			}
			if (this.guest != null && !this.guest.IsSlave)
			{
				this.guest.SetGuestStatus(null, RimWorld.GuestStatus.Guest);
			}
			if (this.RaceProps.IsFlesh)
			{
				this.relations.Notify_PawnKidnapped();
			}
			this.ClearMind_NewTemp(false, false, true, false);
		}

		// Token: 0x06003080 RID: 12416 RVA: 0x000F9927 File Offset: 0x000F7B27
		public override AcceptanceReport ClaimableBy(Faction by)
		{
			return false;
		}

		// Token: 0x06003081 RID: 12417 RVA: 0x000F9930 File Offset: 0x000F7B30
		public override bool AdoptableBy(Faction by, StringBuilder reason = null)
		{
			if (base.Faction == by)
			{
				return false;
			}
			Pawn_AgeTracker pawn_AgeTracker = this.ageTracker;
			bool flag;
			if (pawn_AgeTracker == null)
			{
				flag = false;
			}
			else
			{
				LifeStageDef curLifeStage = pawn_AgeTracker.CurLifeStage;
				bool? flag2 = (curLifeStage != null) ? new bool?(curLifeStage.claimable) : null;
				bool flag3 = false;
				flag = (flag2.GetValueOrDefault() == flag3 & flag2 != null);
			}
			if (flag)
			{
				return false;
			}
			string value;
			if (base.FactionPreventsClaimingOrAdopting(base.Faction, false, out value))
			{
				if (reason != null)
				{
					reason.Append(value);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06003082 RID: 12418 RVA: 0x000F99AC File Offset: 0x000F7BAC
		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (newFaction == base.Faction)
			{
				Log.Warning("Used SetFaction to change " + this.ToStringSafe<Pawn>() + " to same faction " + newFaction.ToStringSafe<Faction>());
				return;
			}
			Faction faction = base.Faction;
			Pawn_GuestTracker pawn_GuestTracker = this.guest;
			if (pawn_GuestTracker != null)
			{
				pawn_GuestTracker.SetGuestStatus(null, RimWorld.GuestStatus.Guest);
			}
			if (base.Spawned)
			{
				base.Map.mapPawns.DeRegisterPawn(this);
				base.Map.pawnDestinationReservationManager.ReleaseAllClaimedBy(this);
				base.Map.designationManager.RemoveAllDesignationsOn(this, false);
				base.Map.autoSlaughterManager.Notify_PawnChangedFaction();
			}
			if ((newFaction == Faction.OfPlayer || base.Faction == Faction.OfPlayer) && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_PawnLost(this, PawnLostCondition.ChangedFaction, null);
			}
			if (PawnUtility.IsFactionLeader(this))
			{
				Faction factionLeaderFaction = PawnUtility.GetFactionLeaderFaction(this);
				if (newFaction != factionLeaderFaction && !this.HasExtraHomeFaction(factionLeaderFaction) && !this.HasExtraMiniFaction(factionLeaderFaction))
				{
					factionLeaderFaction.Notify_LeaderLost();
				}
			}
			if (newFaction == Faction.OfPlayer && this.RaceProps.Humanlike && !this.IsQuestLodger())
			{
				this.ChangeKind(newFaction.def.basicMemberKind);
			}
			base.SetFaction(newFaction, null);
			PawnComponentsUtility.AddAndRemoveDynamicComponents(this, false);
			if (base.Faction != null && base.Faction.IsPlayer)
			{
				Pawn_WorkSettings pawn_WorkSettings = this.workSettings;
				if (pawn_WorkSettings != null)
				{
					pawn_WorkSettings.EnableAndInitialize();
				}
				Find.StoryWatcher.watcherPopAdaptation.Notify_PawnEvent(this, PopAdaptationEvent.GainedColonist);
			}
			if (this.Drafted)
			{
				this.drafter.Drafted = false;
			}
			ReachabilityUtility.ClearCacheFor(this);
			this.health.surgeryBills.Clear();
			if (base.Spawned)
			{
				base.Map.mapPawns.RegisterPawn(this);
			}
			this.GenerateNecessaryName();
			Pawn_PlayerSettings pawn_PlayerSettings = this.playerSettings;
			if (pawn_PlayerSettings != null)
			{
				pawn_PlayerSettings.ResetMedicalCare();
			}
			this.ClearMind_NewTemp(true, false, true, false);
			if (!this.Dead && this.needs.mood != null)
			{
				this.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			if (base.Spawned)
			{
				base.Map.attackTargetsCache.UpdateTarget(this);
			}
			Find.GameEnder.CheckOrUpdateGameOver();
			AddictionUtility.CheckDrugAddictionTeachOpportunity(this);
			Pawn_NeedsTracker pawn_NeedsTracker = this.needs;
			if (pawn_NeedsTracker != null)
			{
				pawn_NeedsTracker.AddOrRemoveNeedsAsAppropriate();
			}
			Pawn_PlayerSettings pawn_PlayerSettings2 = this.playerSettings;
			if (pawn_PlayerSettings2 != null)
			{
				pawn_PlayerSettings2.Notify_FactionChanged();
			}
			Pawn_RelationsTracker pawn_RelationsTracker = this.relations;
			if (pawn_RelationsTracker != null)
			{
				pawn_RelationsTracker.Notify_ChangedFaction();
			}
			if (this.IsAnimal && newFaction == Faction.OfPlayer)
			{
				this.training.SetWantedRecursive(TrainableDefOf.Tameness, true);
				this.training.Train(TrainableDefOf.Tameness, recruiter, true);
				if (this.RaceProps.Roamer && this.mindState != null)
				{
					this.mindState.lastStartRoamCooldownTick = new int?(Find.TickManager.TicksGame);
				}
			}
			if (faction == Faction.OfPlayer)
			{
				BillUtility.Notify_ColonistUnavailable(this);
			}
			if (newFaction == Faction.OfPlayer)
			{
				Find.StoryWatcher.statsRecord.UpdateGreatestPopulation();
				Find.World.StoryState.RecordPopulationIncrease();
			}
			if (newFaction != null)
			{
				newFaction.Notify_PawnJoined(this);
			}
			Ideo ideo = this.Ideo;
			if (ideo != null)
			{
				ideo.Notify_MemberChangedFaction(this, faction, newFaction);
			}
			Pawn_AgeTracker pawn_AgeTracker = this.ageTracker;
			if (pawn_AgeTracker != null)
			{
				pawn_AgeTracker.ResetAgeReversalDemand(Pawn_AgeTracker.AgeReversalReason.Recruited, false);
			}
			Pawn_RopeTracker pawn_RopeTracker = this.roping;
			if (pawn_RopeTracker != null)
			{
				pawn_RopeTracker.BreakAllRopes();
			}
			if (ModsConfig.BiotechActive)
			{
				Pawn_MechanitorTracker pawn_MechanitorTracker = this.mechanitor;
				if (pawn_MechanitorTracker != null)
				{
					pawn_MechanitorTracker.Notify_ChangedFaction();
				}
			}
			Pawn_CreepJoinerTracker pawn_CreepJoinerTracker = this.creepjoiner;
			if (pawn_CreepJoinerTracker != null)
			{
				pawn_CreepJoinerTracker.Notify_ChangedFaction();
			}
			if (faction != null)
			{
				Find.FactionManager.Notify_PawnLeftFaction(faction);
			}
		}

		// Token: 0x06003083 RID: 12419 RVA: 0x000F9D29 File Offset: 0x000F7F29
		[Obsolete]
		public void ClearMind(bool ifLayingKeepLaying = false, bool clearInspiration = false, bool clearMentalState = true)
		{
			this.ClearMind_NewTemp(ifLayingKeepLaying, clearInspiration, clearMentalState, false);
		}

		// Token: 0x06003084 RID: 12420 RVA: 0x000F9D38 File Offset: 0x000F7F38
		public void ClearMind_NewTemp(bool ifLayingKeepLaying = false, bool clearInspiration = false, bool clearMentalState = true, bool wasDowned = false)
		{
			Pawn_PathFollower pawn_PathFollower = this.pather;
			if (pawn_PathFollower != null)
			{
				pawn_PathFollower.StopDead();
			}
			Pawn_MindState pawn_MindState = this.mindState;
			if (pawn_MindState != null)
			{
				pawn_MindState.Reset(clearInspiration, clearMentalState, wasDowned);
			}
			Pawn_JobTracker pawn_JobTracker = this.jobs;
			if (pawn_JobTracker != null)
			{
				pawn_JobTracker.StopAll(ifLayingKeepLaying, true);
			}
			this.VerifyReservations(null);
		}

		// Token: 0x06003085 RID: 12421 RVA: 0x000F9D88 File Offset: 0x000F7F88
		public void ClearAllReservations(bool releaseDestinationsOnlyIfObsolete = true)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (releaseDestinationsOnlyIfObsolete)
				{
					maps[i].pawnDestinationReservationManager.ReleaseAllObsoleteClaimedBy(this);
				}
				else
				{
					maps[i].pawnDestinationReservationManager.ReleaseAllClaimedBy(this);
				}
				maps[i].reservationManager.ReleaseAllClaimedBy(this);
				maps[i].enrouteManager.ReleaseAllClaimedBy(this);
				maps[i].physicalInteractionReservationManager.ReleaseAllClaimedBy(this);
				maps[i].attackTargetReservationManager.ReleaseAllClaimedBy(this);
			}
		}

		// Token: 0x06003086 RID: 12422 RVA: 0x000F9E20 File Offset: 0x000F8020
		public void ClearReservationsForJob(Job job)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].pawnDestinationReservationManager.ReleaseClaimedBy(this, job);
				maps[i].reservationManager.ReleaseClaimedBy(this, job);
				maps[i].enrouteManager.ReleaseAllClaimedBy(this);
				maps[i].physicalInteractionReservationManager.ReleaseClaimedBy(this, job);
				maps[i].attackTargetReservationManager.ReleaseClaimedBy(this, job);
			}
		}

		// Token: 0x06003087 RID: 12423 RVA: 0x000F9EA4 File Offset: 0x000F80A4
		public void VerifyReservations(Job prevJob = null)
		{
			if (this.jobs == null)
			{
				return;
			}
			if (this.CurJob != null || this.jobs.jobQueue.Count > 0 || this.jobs.startingNewJob)
			{
				return;
			}
			bool flag = false;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				LocalTargetInfo obj = maps[i].reservationManager.FirstReservationFor(this);
				if (obj.IsValid)
				{
					Log.ErrorOnce(string.Format("Reservation manager failed to clean up properly; {0} still reserving {1}, prev job: {2}", this.ToStringSafe<Pawn>(), obj.ToStringSafe<LocalTargetInfo>(), prevJob), 97771429 ^ this.thingIDNumber);
					flag = true;
				}
				LocalTargetInfo obj2 = maps[i].physicalInteractionReservationManager.FirstReservationFor(this);
				if (obj2.IsValid)
				{
					Log.ErrorOnce(string.Concat(new string[]
					{
						"Physical interaction reservation manager failed to clean up properly; ",
						this.ToStringSafe<Pawn>(),
						" still reserving ",
						obj2.ToStringSafe<LocalTargetInfo>(),
						", prev job: {prevJob}"
					}), 19586765 ^ this.thingIDNumber);
					flag = true;
				}
				IAttackTarget attackTarget = maps[i].attackTargetReservationManager.FirstReservationFor(this);
				if (attackTarget != null)
				{
					Log.ErrorOnce(string.Concat(new string[]
					{
						"Attack target reservation manager failed to clean up properly; ",
						this.ToStringSafe<Pawn>(),
						" still reserving ",
						attackTarget.ToStringSafe<IAttackTarget>(),
						", prev job: {prevJob}"
					}), 100495878 ^ this.thingIDNumber);
					flag = true;
				}
				IntVec3 obj3 = maps[i].pawnDestinationReservationManager.FirstObsoleteReservationFor(this);
				if (obj3.IsValid)
				{
					Job job = maps[i].pawnDestinationReservationManager.FirstObsoleteReservationJobFor(this);
					Log.ErrorOnce(string.Concat(new string[]
					{
						"Pawn destination reservation manager failed to clean up properly; ",
						this.ToStringSafe<Pawn>(),
						"/",
						job.ToStringSafe<Job>(),
						"/",
						job.def.ToStringSafe<JobDef>(),
						" still reserving ",
						obj3.ToStringSafe<IntVec3>(),
						", prev job: {prevJob}"
					}), 1958674 ^ this.thingIDNumber);
					flag = true;
				}
			}
			if (flag)
			{
				this.ClearAllReservations(true);
			}
		}

		// Token: 0x06003088 RID: 12424 RVA: 0x000FA0C0 File Offset: 0x000F82C0
		public void DropAndForbidEverything(bool keepInventoryAndEquipmentIfInBed = false, bool rememberPrimary = false)
		{
			if (this.kindDef.destroyGearOnDrop)
			{
				this.equipment.DestroyAllEquipment(DestroyMode.Vanish);
				this.apparel.DestroyAll(DestroyMode.Vanish);
			}
			if (!this.InContainerEnclosed)
			{
				if (base.SpawnedOrAnyParentSpawned)
				{
					Pawn_CarryTracker pawn_CarryTracker = this.carryTracker;
					if (((pawn_CarryTracker != null) ? pawn_CarryTracker.CarriedThing : null) != null)
					{
						Thing thing;
						this.carryTracker.TryDropCarriedThing(base.PositionHeld, ThingPlaceMode.Near, out thing, null);
					}
					if (!keepInventoryAndEquipmentIfInBed || !this.InBed())
					{
						Pawn_EquipmentTracker pawn_EquipmentTracker = this.equipment;
						if (pawn_EquipmentTracker != null)
						{
							pawn_EquipmentTracker.DropAllEquipment(base.PositionHeld, true, rememberPrimary);
						}
						if (this.inventory != null && this.inventory.innerContainer.TotalStackCount > 0)
						{
							this.inventory.DropAllNearPawn(base.PositionHeld, true, false);
						}
					}
				}
				return;
			}
			Pawn_CarryTracker pawn_CarryTracker2 = this.carryTracker;
			if (((pawn_CarryTracker2 != null) ? pawn_CarryTracker2.CarriedThing : null) != null)
			{
				this.carryTracker.innerContainer.TryTransferToContainer(this.carryTracker.CarriedThing, this.holdingOwner, true);
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker2 = this.equipment;
			if (((pawn_EquipmentTracker2 != null) ? pawn_EquipmentTracker2.Primary : null) != null)
			{
				this.equipment.TryTransferEquipmentToContainer(this.equipment.Primary, this.holdingOwner);
			}
			Pawn_InventoryTracker pawn_InventoryTracker = this.inventory;
			if (pawn_InventoryTracker == null)
			{
				return;
			}
			pawn_InventoryTracker.innerContainer.TryTransferAllToContainer(this.holdingOwner, true);
		}

		// Token: 0x06003089 RID: 12425 RVA: 0x000FA208 File Offset: 0x000F8408
		public void GenerateNecessaryName()
		{
			if (this.Name != null)
			{
				return;
			}
			if (base.Faction != Faction.OfPlayer)
			{
				return;
			}
			if (this.RaceProps.Animal || (ModsConfig.BiotechActive && this.RaceProps.IsMechanoid))
			{
				this.Name = PawnBioAndNameGenerator.GeneratePawnName(this, NameStyle.Numeric, null, false, null);
			}
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x000FA260 File Offset: 0x000F8460
		public Verb TryGetAttackVerb(Thing target, bool allowManualCastWeapons = false, bool allowTurrets = false)
		{
			Pawn_EquipmentTracker pawn_EquipmentTracker = this.equipment;
			if (((pawn_EquipmentTracker != null) ? pawn_EquipmentTracker.Primary : null) != null && this.equipment.PrimaryEq.PrimaryVerb.Available() && (!this.equipment.PrimaryEq.PrimaryVerb.verbProps.onlyManualCast || (this.CurJob != null && this.CurJob.def != JobDefOf.Wait_Combat) || allowManualCastWeapons))
			{
				return this.equipment.PrimaryEq.PrimaryVerb;
			}
			if (allowManualCastWeapons && this.apparel != null)
			{
				Verb firstApparelVerb = this.apparel.FirstApparelVerb;
				if (firstApparelVerb != null && firstApparelVerb.Available())
				{
					return firstApparelVerb;
				}
			}
			if (allowTurrets)
			{
				List<ThingComp> allComps = base.AllComps;
				for (int i = 0; i < allComps.Count; i++)
				{
					CompTurretGun compTurretGun = allComps[i] as CompTurretGun;
					if (compTurretGun != null && !compTurretGun.TurretDestroyed && compTurretGun.GunCompEq.PrimaryVerb.Available())
					{
						return compTurretGun.GunCompEq.PrimaryVerb;
					}
				}
			}
			if (this.kindDef.canMeleeAttack)
			{
				return this.meleeVerbs.TryGetMeleeVerb(target);
			}
			return null;
		}

		// Token: 0x0600308B RID: 12427 RVA: 0x000FA37C File Offset: 0x000F857C
		public bool TryStartAttack(LocalTargetInfo targ)
		{
			if (this.stances.FullBodyBusy)
			{
				return false;
			}
			if (this.WorkTagIsDisabled(WorkTags.Violent))
			{
				return false;
			}
			bool allowManualCastWeapons = !this.IsColonist;
			Verb verb = this.TryGetAttackVerb(targ.Thing, allowManualCastWeapons, false);
			return verb != null && verb.TryStartCastOn(verb.verbProps.ai_RangedAlawaysShootGroundBelowTarget ? targ.Cell : targ, false, true, false, false);
		}

		// Token: 0x0600308C RID: 12428 RVA: 0x000FA3E8 File Offset: 0x000F85E8
		public override IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			if (this.RaceProps.meatDef != null)
			{
				int num = GenMath.RoundRandom(this.GetStatValue(StatDefOf.MeatAmount, true, -1) * efficiency);
				if (num > 0)
				{
					Thing thing = ThingMaker.MakeThing(this.RaceProps.meatDef, null);
					thing.stackCount = num;
					yield return thing;
				}
			}
			foreach (Thing thing2 in base.ButcherProducts(butcher, efficiency))
			{
				yield return thing2;
			}
			IEnumerator<Thing> enumerator = null;
			if (this.RaceProps.leatherDef != null)
			{
				int num2 = GenMath.RoundRandom(this.GetStatValue(StatDefOf.LeatherAmount, true, -1) * efficiency);
				if (num2 > 0)
				{
					Thing thing3 = ThingMaker.MakeThing(this.RaceProps.leatherDef, null);
					thing3.stackCount = num2;
					yield return thing3;
				}
			}
			if (!this.RaceProps.Humanlike)
			{
				Pawn.<>c__DisplayClass327_0 CS$<>8__locals1 = new Pawn.<>c__DisplayClass327_0();
				CS$<>8__locals1.lifeStage = this.ageTracker.CurKindLifeStage;
				if (CS$<>8__locals1.lifeStage.butcherBodyPart != null && (this.gender == Gender.None || (this.gender == Gender.Male && CS$<>8__locals1.lifeStage.butcherBodyPart.allowMale) || (this.gender == Gender.Female && CS$<>8__locals1.lifeStage.butcherBodyPart.allowFemale)))
				{
					for (;;)
					{
						IEnumerable<BodyPartRecord> notMissingParts = this.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
						Func<BodyPartRecord, bool> predicate;
						if ((predicate = CS$<>8__locals1.<>9__0) == null)
						{
							predicate = (CS$<>8__locals1.<>9__0 = ((BodyPartRecord x) => x.IsInGroup(CS$<>8__locals1.lifeStage.butcherBodyPart.bodyPartGroup)));
						}
						BodyPartRecord bodyPartRecord = notMissingParts.FirstOrDefault(predicate);
						if (bodyPartRecord == null)
						{
							break;
						}
						this.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this, bodyPartRecord), null, null, null);
						yield return ThingMaker.MakeThing(CS$<>8__locals1.lifeStage.butcherBodyPart.thing ?? bodyPartRecord.def.spawnThingOnRemoved, null);
					}
				}
				CS$<>8__locals1 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600308D RID: 12429 RVA: 0x000FA408 File Offset: 0x000F8608
		public TaggedString FactionDesc(TaggedString name, bool extraFactionsInfo, string nameLabel, string genderLabel)
		{
			Pawn.tmpExtraFactions.Clear();
			QuestUtility.GetExtraFactionsFromQuestParts(this, Pawn.tmpExtraFactions, null);
			GuestUtility.GetExtraFactionsFromGuestStatus(this, Pawn.tmpExtraFactions);
			TaggedString taggedString;
			if (base.Faction != null && !base.Faction.Hidden)
			{
				if (Pawn.tmpExtraFactions.Count == 0 && this.SlaveFaction == null)
				{
					taggedString = "PawnMainDescFactionedWrap".Translate(name, base.Faction.NameColored, nameLabel.Named("NAME"), genderLabel.Named("GENDER"));
				}
				else
				{
					taggedString = "PawnMainDescUnderFactionedWrap".Translate(name, base.Faction.NameColored);
				}
			}
			else
			{
				taggedString = name;
			}
			if (extraFactionsInfo)
			{
				for (int i = 0; i < Pawn.tmpExtraFactions.Count; i++)
				{
					if (base.Faction != Pawn.tmpExtraFactions[i].faction && !Pawn.tmpExtraFactions[i].faction.Hidden)
					{
						taggedString += "\n" + Pawn.tmpExtraFactions[i].factionType.GetLabel().CapitalizeFirst() + ": " + Pawn.tmpExtraFactions[i].faction.NameColored.Resolve();
					}
				}
			}
			Pawn.tmpExtraFactions.Clear();
			return taggedString;
		}

		// Token: 0x0600308E RID: 12430 RVA: 0x000FA568 File Offset: 0x000F8768
		public string MainDesc(bool writeFaction, bool writeGender = true)
		{
			bool flag = base.Faction == null || !base.Faction.IsPlayer;
			string text = writeGender ? ((this.gender == Gender.None) ? string.Empty : this.gender.GetLabel(this.AnimalOrWildMan())) : string.Empty;
			string text2 = string.Empty;
			if (this.RaceProps.Animal || this.RaceProps.IsMechanoid)
			{
				text2 = GenLabel.BestKindLabel(this, false, true, false, -1);
				if (this.Name != null)
				{
					if (!text.NullOrEmpty())
					{
						text += " ";
					}
					text += text2;
				}
			}
			if (this.ageTracker != null)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "AgeIndicator".Translate(this.ageTracker.AgeNumberString);
			}
			if (this.IsMutant && this.mutant.HasTurned && this.mutant.Def.overrideLabel)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += this.mutant.Def.label;
			}
			else if (!this.RaceProps.Animal && !this.RaceProps.IsMechanoid && flag && !this.IsCreepJoiner)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text2 = GenLabel.BestKindLabel(this, false, true, false, -1);
				text += text2;
			}
			if (writeFaction)
			{
				text = this.FactionDesc(text, true, text2, this.gender.GetLabel(this.RaceProps.Animal)).Resolve();
			}
			return text.CapitalizeFirst();
		}

		// Token: 0x0600308F RID: 12431 RVA: 0x000FA72C File Offset: 0x000F892C
		public string GetJobReport()
		{
			string result;
			try
			{
				Lord lord = this.GetLord();
				object obj;
				if (lord == null)
				{
					obj = null;
				}
				else
				{
					LordJob lordJob = lord.LordJob;
					obj = ((lordJob != null) ? lordJob.GetJobReport(this) : null);
				}
				object obj2;
				if ((obj2 = obj) == null)
				{
					Pawn_JobTracker pawn_JobTracker = this.jobs;
					if (pawn_JobTracker == null)
					{
						obj2 = null;
					}
					else
					{
						JobDriver curDriver = pawn_JobTracker.curDriver;
						obj2 = ((curDriver != null) ? curDriver.GetReport() : null);
					}
				}
				object obj3 = obj2;
				result = ((obj3 != null) ? obj3.CapitalizeFirst() : null);
			}
			catch (Exception ex)
			{
				string str = "JobDriver.GetReport() exception: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				result = null;
			}
			return result;
		}

		// Token: 0x06003090 RID: 12432 RVA: 0x000FA7BC File Offset: 0x000F89BC
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!this.def.hideMainDesc)
			{
				stringBuilder.AppendLine(this.MainDesc(PawnUtility.ShouldDisplayFactionInInspectString(this), true));
			}
			Pawn_RoyaltyTracker pawn_RoyaltyTracker = this.royalty;
			RoyalTitle royalTitle = (pawn_RoyaltyTracker != null) ? pawn_RoyaltyTracker.MostSeniorTitle : null;
			if (royalTitle != null)
			{
				stringBuilder.AppendLine("PawnTitleDescWrap".Translate(royalTitle.def.GetLabelCapFor(this), royalTitle.faction.NameColored).Resolve());
			}
			string inspectString = base.GetInspectString();
			if (!inspectString.NullOrEmpty())
			{
				stringBuilder.AppendLine(inspectString);
			}
			if (this.TraderKind != null)
			{
				stringBuilder.AppendLine(this.TraderKind.LabelCap);
			}
			if (this.InMentalState)
			{
				string inspectLine = this.MentalState.InspectLine;
				if (!string.IsNullOrEmpty(inspectLine))
				{
					stringBuilder.AppendLine(inspectLine);
				}
			}
			Pawn.states.Clear();
			Pawn_HealthTracker pawn_HealthTracker = this.health;
			if (((pawn_HealthTracker != null) ? pawn_HealthTracker.hediffSet : null) != null)
			{
				List<Hediff> hediffs = this.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					Hediff hediff = hediffs[i];
					if (!hediff.def.battleStateLabel.NullOrEmpty())
					{
						Pawn.states.AddUnique(hediff.def.battleStateLabel);
					}
					string inspectString2 = hediff.GetInspectString();
					if (!inspectString2.NullOrEmpty())
					{
						stringBuilder.AppendLine(inspectString2);
					}
				}
			}
			if (Pawn.states.Count > 0)
			{
				Pawn.states.Sort();
				stringBuilder.AppendLine(string.Format("{0}: {1}", "State".Translate(), Pawn.states.ToCommaList(false, false).CapitalizeFirst()));
				Pawn.states.Clear();
			}
			Pawn_FlightTracker pawn_FlightTracker = this.flight;
			string text = (pawn_FlightTracker != null) ? pawn_FlightTracker.GetStatusString() : null;
			if (!text.NullOrEmpty())
			{
				stringBuilder.AppendLine(text);
			}
			Pawn_StanceTracker pawn_StanceTracker = this.stances;
			if (((pawn_StanceTracker != null) ? pawn_StanceTracker.stunner : null) != null && this.stances.stunner.Stunned)
			{
				if (this.stances.stunner.Hypnotized)
				{
					stringBuilder.AppendLine("InTrance".Translate());
				}
				else if (this.stances.stunner.StunFromEMP)
				{
					stringBuilder.AppendLine("StunnedByEMP".Translate() + ": " + this.stances.stunner.StunTicksLeft.ToStringSecondsFromTicks());
				}
				else
				{
					stringBuilder.AppendLine("StunLower".Translate().CapitalizeFirst() + ": " + this.stances.stunner.StunTicksLeft.ToStringSecondsFromTicks());
				}
			}
			Pawn_StanceTracker pawn_StanceTracker2 = this.stances;
			if (((pawn_StanceTracker2 != null) ? pawn_StanceTracker2.stagger : null) != null && this.stances.stagger.Staggered)
			{
				stringBuilder.AppendLine("SlowedByDamage".Translate() + ": " + this.stances.stagger.StaggerTicksLeft.ToStringSecondsFromTicks());
			}
			if (this.Inspired)
			{
				stringBuilder.AppendLine(this.Inspiration.InspectLine);
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker = this.equipment;
			if (((pawn_EquipmentTracker != null) ? pawn_EquipmentTracker.Primary : null) != null)
			{
				stringBuilder.AppendLine("Equipped".TranslateSimple() + ": " + ((this.equipment.Primary != null) ? this.equipment.Primary.Label : "EquippedNothing".TranslateSimple()).CapitalizeFirst());
			}
			if (this.abilities != null)
			{
				for (int j = 0; j < this.abilities.AllAbilitiesForReading.Count; j++)
				{
					string inspectString3 = this.abilities.AllAbilitiesForReading[j].GetInspectString();
					if (!inspectString3.NullOrEmpty())
					{
						stringBuilder.AppendLine(inspectString3);
					}
				}
			}
			Pawn_CarryTracker pawn_CarryTracker = this.carryTracker;
			if (((pawn_CarryTracker != null) ? pawn_CarryTracker.CarriedThing : null) != null && (this.CurJob == null || this.CurJob.showCarryingInspectLine))
			{
				stringBuilder.Append("Carrying".Translate() + ": ");
				stringBuilder.AppendLine(this.carryTracker.CarriedThing.LabelCap);
			}
			Pawn_RopeTracker pawn_RopeTracker = this.roping;
			if (pawn_RopeTracker != null && pawn_RopeTracker.IsRoped)
			{
				stringBuilder.AppendLine(this.roping.InspectLine);
			}
			if (ModsConfig.BiotechActive && this.IsColonyMech && this.needs.energy != null)
			{
				TaggedString taggedString = "MechEnergy".Translate() + ": " + this.needs.energy.CurLevelPercentage.ToStringPercent();
				float maxLevel = this.needs.energy.MaxLevel;
				if (this.IsCharging())
				{
					taggedString += " (+" + "PerDay".Translate((50f / maxLevel).ToStringPercent()) + ")";
				}
				else if (this.IsSelfShutdown())
				{
					taggedString += " (+" + "PerDay".Translate((1f / maxLevel).ToStringPercent()) + ")";
				}
				else
				{
					taggedString += " (-" + "PerDay".Translate((this.needs.energy.FallPerDay / maxLevel).ToStringPercent()) + ")";
				}
				stringBuilder.AppendLine(taggedString);
			}
			string text2 = null;
			if (PawnUtility.ShouldDisplayLordReport(this))
			{
				Lord lord = this.GetLord();
				if (((lord != null) ? lord.LordJob : null) != null)
				{
					text2 = lord.LordJob.GetReport(this);
				}
			}
			if (PawnUtility.ShouldDisplayJobReport(this))
			{
				string jobReport = this.GetJobReport();
				if (text2.NullOrEmpty())
				{
					text2 = jobReport;
				}
				else if (!jobReport.NullOrEmpty())
				{
					text2 = text2 + ": " + jobReport;
				}
			}
			if (!text2.NullOrEmpty())
			{
				stringBuilder.AppendLine(text2.CapitalizeFirst().EndWithPeriod());
			}
			Pawn_JobTracker pawn_JobTracker = this.jobs;
			if (((pawn_JobTracker != null) ? pawn_JobTracker.curJob : null) != null)
			{
				Pawn_JobTracker pawn_JobTracker2 = this.jobs;
				if (pawn_JobTracker2 != null && pawn_JobTracker2.jobQueue.Count > 0)
				{
					try
					{
						string text3 = this.jobs.jobQueue[0].job.GetReport(this).CapitalizeFirst();
						if (this.jobs.jobQueue.Count > 1)
						{
							text3 = text3 + " (+" + (this.jobs.jobQueue.Count - 1).ToString() + ")";
						}
						stringBuilder.AppendLine("Queued".Translate() + ": " + text3);
					}
					catch (Exception ex)
					{
						string str = "JobDriver.GetReport() exception: ";
						Exception ex2 = ex;
						Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
					}
				}
			}
			if (this.IsMutant && this.mutant.Def.overrideInspectString)
			{
				string inspectString4 = this.mutant.GetInspectString();
				if (!inspectString4.NullOrEmpty())
				{
					stringBuilder.AppendLine(inspectString4);
				}
			}
			if (ModsConfig.AnomalyActive)
			{
				Pawn_HealthTracker pawn_HealthTracker2 = this.health;
				if (((pawn_HealthTracker2 != null) ? pawn_HealthTracker2.hediffSet : null) != null)
				{
					Hediff_MetalhorrorImplant firstHediff = this.health.hediffSet.GetFirstHediff<Hediff_MetalhorrorImplant>();
					if (firstHediff != null && firstHediff.Emerging)
					{
						stringBuilder.AppendLine("Emerging".Translate());
					}
				}
				if (this.IsCreepJoiner)
				{
					string inspectString5 = this.creepjoiner.GetInspectString();
					if (!inspectString5.NullOrEmpty())
					{
						stringBuilder.AppendLine(inspectString5);
					}
				}
			}
			if (ModsConfig.BiotechActive)
			{
				Pawn_NeedsTracker pawn_NeedsTracker = this.needs;
				if (((pawn_NeedsTracker != null) ? pawn_NeedsTracker.energy : null) != null && this.needs.energy.IsLowEnergySelfShutdown)
				{
					stringBuilder.AppendLine("MustBeCarriedToRecharger".Translate());
				}
			}
			if (RestraintsUtility.ShouldShowRestraintsInfo(this))
			{
				stringBuilder.AppendLine("InRestraints".Translate());
			}
			if (this.guest != null && !this.guest.Recruitable && !this.IsSubhuman && !this.IsCreepJoiner)
			{
				if (base.Faction == null)
				{
					stringBuilder.AppendLine("UnrecruitableNoFaction".Translate().CapitalizeFirst());
				}
				else if (base.Faction != Faction.OfPlayer || this.IsSlaveOfColony || this.IsPrisonerOfColony)
				{
					stringBuilder.AppendLine("Unrecruitable".Translate().CapitalizeFirst());
				}
			}
			if (Prefs.DevMode && DebugSettings.showLocomotionUrgency && this.CurJob != null)
			{
				stringBuilder.AppendLine("Locomotion Urgency: " + this.CurJob.locomotionUrgency.ToString());
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06003091 RID: 12433 RVA: 0x000FB0E4 File Offset: 0x000F92E4
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.IsColonistPlayerControlled || this.IsColonyMech || this.IsColonySubhumanPlayerControlled)
			{
				Lord lord2 = this.GetLord();
				AcceptanceReport allowsDrafting = (lord2 != null) ? lord2.AllowsDrafting(this) : true;
				if (this.drafter != null)
				{
					foreach (Gizmo gizmo2 in this.drafter.GetGizmos())
					{
						if (!allowsDrafting && !gizmo2.Disabled)
						{
							gizmo2.Disabled = true;
							gizmo2.disabledReason = allowsDrafting.Reason;
						}
						yield return gizmo2;
					}
					enumerator = null;
				}
				foreach (Gizmo gizmo3 in PawnAttackGizmoUtility.GetAttackGizmos(this))
				{
					if (!allowsDrafting && !gizmo3.Disabled)
					{
						gizmo3.Disabled = true;
						gizmo3.disabledReason = allowsDrafting.Reason;
					}
					yield return gizmo3;
				}
				enumerator = null;
				allowsDrafting = default(AcceptanceReport);
			}
			if (this.equipment != null)
			{
				foreach (Gizmo gizmo4 in this.equipment.GetGizmos())
				{
					yield return gizmo4;
				}
				enumerator = null;
			}
			if (this.carryTracker != null)
			{
				foreach (Gizmo gizmo5 in this.carryTracker.GetGizmos())
				{
					yield return gizmo5;
				}
				enumerator = null;
			}
			if (this.needs != null)
			{
				foreach (Gizmo gizmo6 in this.needs.GetGizmos())
				{
					yield return gizmo6;
				}
				enumerator = null;
			}
			if (Find.Selector.SingleSelectedThing == this && this.psychicEntropy != null && this.psychicEntropy.NeedToShowGizmo())
			{
				yield return this.psychicEntropy.GetGizmo();
				if (DebugSettings.ShowDevGizmos)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Psyfocus -20%",
						action = delegate()
						{
							this.psychicEntropy.OffsetPsyfocusDirectly(-0.2f);
						}
					};
					yield return new Command_Action
					{
						defaultLabel = "DEV: Psyfocus +20%",
						action = delegate()
						{
							this.psychicEntropy.OffsetPsyfocusDirectly(0.2f);
						}
					};
					yield return new Command_Action
					{
						defaultLabel = "DEV: Neural heat -20",
						action = delegate()
						{
							this.psychicEntropy.TryAddEntropy(-20f, null, true, false);
						}
					};
					yield return new Command_Action
					{
						defaultLabel = "DEV: Neural heat +20",
						action = delegate()
						{
							this.psychicEntropy.TryAddEntropy(20f, null, true, false);
						}
					};
				}
			}
			if (ModsConfig.BiotechActive)
			{
				if (MechanitorUtility.IsMechanitor(this))
				{
					foreach (Gizmo gizmo7 in this.mechanitor.GetGizmos())
					{
						yield return gizmo7;
					}
					enumerator = null;
				}
				if (this.RaceProps.IsMechanoid)
				{
					foreach (Gizmo gizmo8 in MechanitorUtility.GetMechGizmos(this))
					{
						yield return gizmo8;
					}
					enumerator = null;
				}
				if (this.RaceProps.Humanlike && this.ageTracker.AgeBiologicalYears < 13 && !this.Drafted && Find.Selector.SelectedPawns.Count < 2 && this.DevelopmentalStage.Child())
				{
					yield return new Gizmo_GrowthTier(this);
					if (DebugSettings.ShowDevGizmos)
					{
						yield return new Command_Action
						{
							defaultLabel = "DEV: Set growth tier",
							action = delegate()
							{
								List<FloatMenuOption> list = new List<FloatMenuOption>();
								for (int i = 0; i < GrowthUtility.GrowthTiers.Length; i++)
								{
									int tier = i;
									list.Add(new FloatMenuOption(tier.ToString(), delegate()
									{
										this.ageTracker.growthPoints = GrowthUtility.GrowthTiers[tier].pointsRequirement;
									}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
								}
								Find.WindowStack.Add(new FloatMenu(list));
							}
						};
					}
				}
			}
			if (this.IsMutant)
			{
				foreach (Gizmo gizmo9 in this.mutant.GetGizmos())
				{
					yield return gizmo9;
				}
				enumerator = null;
			}
			if (ModsConfig.AnomalyActive && this.IsCreepJoiner)
			{
				foreach (Gizmo gizmo10 in this.creepjoiner.GetGizmos())
				{
					yield return gizmo10;
				}
				enumerator = null;
			}
			if (this.abilities != null)
			{
				foreach (Gizmo gizmo11 in this.abilities.GetGizmos())
				{
					yield return gizmo11;
				}
				enumerator = null;
			}
			if (this.IsColonistPlayerControlled || this.IsColonyMech || this.IsPrisonerOfColony)
			{
				if (this.playerSettings != null)
				{
					foreach (Gizmo gizmo12 in this.playerSettings.GetGizmos())
					{
						yield return gizmo12;
					}
					enumerator = null;
				}
				foreach (Gizmo gizmo13 in this.health.GetGizmos())
				{
					yield return gizmo13;
				}
				enumerator = null;
			}
			if (this.Dead && this.HasShowGizmosOnCorpseHediff)
			{
				foreach (Gizmo gizmo14 in this.health.GetGizmos())
				{
					yield return gizmo14;
				}
				enumerator = null;
			}
			if (this.apparel != null)
			{
				foreach (Gizmo gizmo15 in this.apparel.GetGizmos())
				{
					yield return gizmo15;
				}
				enumerator = null;
			}
			if (this.inventory != null)
			{
				foreach (Gizmo gizmo16 in this.inventory.GetGizmos())
				{
					yield return gizmo16;
				}
				enumerator = null;
			}
			if (this.mindState != null)
			{
				foreach (Gizmo gizmo17 in this.mindState.GetGizmos())
				{
					yield return gizmo17;
				}
				enumerator = null;
			}
			if (this.royalty != null && this.IsColonistPlayerControlled)
			{
				bool anyPermitOnCooldown = false;
				foreach (FactionPermit factionPermit in this.royalty.AllFactionPermits)
				{
					if (factionPermit.OnCooldown)
					{
						anyPermitOnCooldown = true;
					}
					IEnumerable<Gizmo> pawnGizmos = factionPermit.Permit.Worker.GetPawnGizmos(this, factionPermit.Faction);
					if (pawnGizmos != null)
					{
						foreach (Gizmo gizmo18 in pawnGizmos)
						{
							yield return gizmo18;
						}
						enumerator = null;
					}
				}
				List<FactionPermit>.Enumerator enumerator2 = default(List<FactionPermit>.Enumerator);
				if (this.royalty.HasAidPermit)
				{
					yield return this.royalty.RoyalAidGizmo();
				}
				if (DebugSettings.ShowDevGizmos && anyPermitOnCooldown)
				{
					yield return new Command_Action
					{
						defaultLabel = "Reset permit cooldowns",
						action = delegate()
						{
							foreach (FactionPermit factionPermit2 in this.royalty.AllFactionPermits)
							{
								factionPermit2.ResetCooldown();
							}
						}
					};
				}
				foreach (RoyalTitle royalTitle in this.royalty.AllTitlesForReading)
				{
					if (royalTitle.def.permits != null)
					{
						Faction faction = royalTitle.faction;
						foreach (RoyalTitlePermitDef royalTitlePermitDef in royalTitle.def.permits)
						{
							IEnumerable<Gizmo> pawnGizmos2 = royalTitlePermitDef.Worker.GetPawnGizmos(this, faction);
							if (pawnGizmos2 != null)
							{
								foreach (Gizmo gizmo19 in pawnGizmos2)
								{
									yield return gizmo19;
								}
								enumerator = null;
							}
						}
						List<RoyalTitlePermitDef>.Enumerator enumerator4 = default(List<RoyalTitlePermitDef>.Enumerator);
						faction = null;
					}
				}
				List<RoyalTitle>.Enumerator enumerator3 = default(List<RoyalTitle>.Enumerator);
			}
			foreach (Gizmo gizmo20 in QuestUtility.GetQuestRelatedGizmos(this))
			{
				yield return gizmo20;
			}
			enumerator = null;
			if (this.royalty != null && ModsConfig.RoyaltyActive)
			{
				foreach (Gizmo gizmo21 in this.royalty.GetGizmos())
				{
					yield return gizmo21;
				}
				enumerator = null;
			}
			if (this.connections != null && ModsConfig.IdeologyActive)
			{
				foreach (Gizmo gizmo22 in this.connections.GetGizmos())
				{
					yield return gizmo22;
				}
				enumerator = null;
			}
			if (this.genes != null)
			{
				foreach (Gizmo gizmo23 in this.genes.GetGizmos())
				{
					yield return gizmo23;
				}
				enumerator = null;
			}
			if (this.training != null)
			{
				foreach (Gizmo gizmo24 in this.training.GetGizmos())
				{
					yield return gizmo24;
				}
				enumerator = null;
			}
			Lord lord = this.GetLord();
			Lord lord3 = lord;
			if (((lord3 != null) ? lord3.LordJob : null) != null)
			{
				foreach (Gizmo gizmo25 in lord.LordJob.GetPawnGizmos(this))
				{
					yield return gizmo25;
				}
				enumerator = null;
				if (lord.CurLordToil != null)
				{
					foreach (Gizmo gizmo26 in lord.CurLordToil.GetPawnGizmos(this))
					{
						yield return gizmo26;
					}
					enumerator = null;
				}
			}
			if (DebugSettings.ShowDevGizmos && ModsConfig.BiotechActive)
			{
				Pawn_RelationsTracker pawn_RelationsTracker = this.relations;
				if (pawn_RelationsTracker != null && pawn_RelationsTracker.IsTryRomanceOnCooldown)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Reset try romance cooldown",
						action = delegate()
						{
							this.relations.romanceEnableTick = -1;
						}
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06003092 RID: 12434 RVA: 0x000FB0F4 File Offset: 0x000F92F4
		public virtual IEnumerable<FloatMenuOption> GetExtraFloatMenuOptionsFor(IntVec3 sq)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}

		// Token: 0x06003093 RID: 12435 RVA: 0x000FB0FB File Offset: 0x000F92FB
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (ModsConfig.AnomalyActive && this.creepjoiner != null)
			{
				foreach (FloatMenuOption floatMenuOption2 in this.creepjoiner.GetFloatMenuOptions(selPawn))
				{
					yield return floatMenuOption2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06003094 RID: 12436 RVA: 0x000FB114 File Offset: 0x000F9314
		public override TipSignal GetTooltip()
		{
			string value = "";
			if (this.gender != Gender.None)
			{
				if (!this.LabelCap.EqualsIgnoreCase(this.KindLabel))
				{
					value = "PawnTooltipGenderAndKindLabel".Translate(this.GetGenderLabel(), this.KindLabel);
				}
				else
				{
					value = this.GetGenderLabel();
				}
			}
			else if (!this.LabelCap.EqualsIgnoreCase(this.KindLabel))
			{
				value = this.KindLabel;
			}
			string generalConditionLabel = HealthUtility.GetGeneralConditionLabel(this, false);
			bool flag = !string.IsNullOrEmpty(value);
			Pawn_EquipmentTracker pawn_EquipmentTracker = this.equipment;
			string text;
			if (((pawn_EquipmentTracker != null) ? pawn_EquipmentTracker.Primary : null) != null)
			{
				if (flag)
				{
					text = "PawnTooltipWithDescAndPrimaryEquip".Translate(this.LabelCap, value, this.equipment.Primary.LabelCap, generalConditionLabel);
				}
				else
				{
					text = "PawnTooltipWithPrimaryEquipNoDesc".Translate(this.LabelCap, value, generalConditionLabel);
				}
			}
			else if (flag)
			{
				text = "PawnTooltipWithDescNoPrimaryEquip".Translate(this.LabelCap, value, generalConditionLabel);
			}
			else
			{
				text = "PawnTooltipNoDescNoPrimaryEquip".Translate(this.LabelCap, generalConditionLabel);
			}
			return new TipSignal(text, this.thingIDNumber * 152317, TooltipPriority.Pawn);
		}

		// Token: 0x06003095 RID: 12437 RVA: 0x000FB283 File Offset: 0x000F9483
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (ModsConfig.BiotechActive && this.genes != null && this.genes.Xenotype != XenotypeDefOf.Baseliner)
			{
				string reportText = this.genes.UniqueXenotype ? "UniqueXenotypeDesc".Translate().ToString() : this.DescriptionFlavor;
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Race".Translate(), this.def.LabelCap + " (" + this.genes.XenotypeLabel + ")", reportText, 4205, null, this.genes.UniqueXenotype ? null : Gen.YieldSingle<Dialog_InfoCard.Hyperlink>(new Dialog_InfoCard.Hyperlink(this.genes.Xenotype, -1)), false, false);
			}
			if (ModsConfig.BiotechActive && this.RaceProps.Humanlike && !Mathf.Approximately(this.ageTracker.BiologicalTicksPerTick, 1f))
			{
				yield return new StatDrawEntry(StatCategoryDefOf.PawnHealth, "StatsReport_AgeRateMultiplier".Translate(), this.ageTracker.BiologicalTicksPerTick.ToStringPercent(), "StatsReport_AgeRateMultiplier_Desc".Translate(), 4195, null, null, false, false);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "BodySize".Translate(), this.BodySize.ToString("F2"), "Stat_Race_BodySize_Desc".Translate(), 4195, null, null, false, false);
			if (this.RaceProps.lifeStageAges.Count > 1 && this.RaceProps.Animal)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Growth".Translate(), this.ageTracker.Growth.ToStringPercent(), "Stat_Race_Growth_Desc".Translate(), 4203, null, null, false, false);
			}
			if (ModsConfig.RoyaltyActive && this.RaceProps.intelligence == Intelligence.Humanlike)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.PawnPsyfocus, "MeditationFocuses".Translate(), MeditationUtility.FocusTypesAvailableForPawnString(this).CapitalizeFirst(), ("MeditationFocusesPawnDesc".Translate() + "\n\n" + MeditationUtility.FocusTypeAvailableExplanation(this)).Resolve(), 4011, null, MeditationUtility.FocusObjectsForPawnHyperlinks(this), false, false);
			}
			if (this.apparel != null && !this.apparel.AllRequirements.EnumerableNullOrEmpty<ApparelRequirementWithSource>())
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (ApparelRequirementWithSource apparelRequirementWithSource in this.apparel.AllRequirements)
				{
					string text = null;
					string t;
					if (!ApparelUtility.IsRequirementActive(apparelRequirementWithSource.requirement, apparelRequirementWithSource.Source, this, out t))
					{
						text = " [" + "ApparelRequirementDisabledLabel".Translate() + ": " + t + "]";
					}
					stringBuilder.Append("- ");
					bool flag = true;
					foreach (ThingDef thingDef in apparelRequirementWithSource.requirement.AllRequiredApparelForPawn(this, false, true))
					{
						if (!flag)
						{
							stringBuilder.Append(", ");
						}
						stringBuilder.Append(thingDef.LabelCap);
						flag = false;
					}
					if (apparelRequirementWithSource.Source == ApparelRequirementSource.Title)
					{
						stringBuilder.Append(" ");
						if (ModsConfig.BiotechActive)
						{
							stringBuilder.Append("ApparelRequirementOrAnyPsycasterOrPrestigeApparelOrMechlord".Translate());
						}
						else
						{
							stringBuilder.Append("ApparelRequirementOrAnyPsycasterOrPrestigeApparel".Translate());
						}
					}
					stringBuilder.Append(" (");
					stringBuilder.Append("Source".Translate());
					stringBuilder.Append(": ");
					stringBuilder.Append(apparelRequirementWithSource.SourceLabelCap);
					stringBuilder.Append(")");
					if (text != null)
					{
						stringBuilder.Append(text);
					}
					stringBuilder.AppendLine();
				}
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Stat_Pawn_RequiredApparel_Name".Translate(), "", "Stat_Pawn_RequiredApparel_Name".Translate() + ":\n\n" + stringBuilder.ToString(), 100, null, null, false, false);
			}
			if (ModsConfig.IdeologyActive && this.Ideo != null)
			{
				foreach (StatDrawEntry statDrawEntry2 in DarknessCombatUtility.GetStatEntriesForPawn(this))
				{
					yield return statDrawEntry2;
				}
				enumerator = null;
			}
			if (this.genes != null)
			{
				foreach (StatDrawEntry statDrawEntry3 in this.genes.SpecialDisplayStats())
				{
					yield return statDrawEntry3;
				}
				enumerator = null;
			}
			if (ModsConfig.BiotechActive && this.RaceProps.Humanlike)
			{
				TaggedString taggedString = "DevelopmentStage_Adult".Translate();
				TaggedString taggedString2 = "StatsReport_DevelopmentStageDesc_Adult".Translate();
				if (this.ageTracker.CurLifeStage.developmentalStage == DevelopmentalStage.Child)
				{
					taggedString = "DevelopmentStage_Child".Translate();
					taggedString2 = "StatsReport_DevelopmentStageDesc_ChildPart1".Translate() + ":\n\n" + (from w in this.RaceProps.lifeStageWorkSettings
					where w.minAge > 0 && w.workType.visible
					select w into d
					select (d.workType.labelShort + " (" + "AgeIndicator".Translate(d.minAge) + ")").RawText).ToLineList("  - ", true) + "\n\n" + "StatsReport_DevelopmentStageDesc_ChildPart2".Translate();
				}
				else if (this.ageTracker.CurLifeStage.developmentalStage == DevelopmentalStage.Baby)
				{
					taggedString = "DevelopmentStage_Baby".Translate();
					taggedString2 = "StatsReport_DevelopmentStageDesc_Baby".Translate();
				}
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "StatsReport_DevelopmentStage".Translate(), taggedString, taggedString2, 4200, null, null, false, false);
			}
			if (this.IsMutant)
			{
				foreach (StatDrawEntry statDrawEntry4 in this.mutant.SpecialDisplayStats())
				{
					yield return statDrawEntry4;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06003096 RID: 12438 RVA: 0x000FB293 File Offset: 0x000F9493
		public PathingContext GetPathContext(Pathing pathing)
		{
			if (this.Flying)
			{
				return pathing.Flying;
			}
			if (this.ShouldAvoidFences && (this.CurJob == null || !this.CurJob.canBashFences))
			{
				return pathing.FenceBlocked;
			}
			return pathing.Normal;
		}

		// Token: 0x06003097 RID: 12439 RVA: 0x000FB2D0 File Offset: 0x000F94D0
		public bool Sterile()
		{
			if (!this.ageTracker.CurLifeStage.reproductive)
			{
				return true;
			}
			if (this.RaceProps.Humanlike)
			{
				if (!ModsConfig.BiotechActive)
				{
					return true;
				}
				if (this.GetStatValue(StatDefOf.Fertility, true, -1) <= 0f)
				{
					return true;
				}
			}
			return this.health.hediffSet.HasHediffPreventsPregnancy() || this.SterileGenes();
		}

		// Token: 0x06003098 RID: 12440 RVA: 0x000FB33C File Offset: 0x000F953C
		public bool AnythingToStrip()
		{
			if (!this.kindDef.canStrip)
			{
				return false;
			}
			if (this.equipment != null && this.equipment.HasAnything())
			{
				return true;
			}
			if (this.inventory != null && this.inventory.innerContainer.Count > 0)
			{
				return true;
			}
			if (this.apparel != null)
			{
				if (base.Destroyed)
				{
					if (this.apparel.AnyApparel)
					{
						return true;
					}
				}
				else if (this.apparel.AnyApparelUnlocked)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003099 RID: 12441 RVA: 0x000FB3BC File Offset: 0x000F95BC
		public void Strip(bool notifyFaction = true)
		{
			Caravan caravan = this.GetCaravan();
			if (caravan != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(this, caravan.PawnsListForReading, null);
				if (this.apparel != null)
				{
					CaravanInventoryUtility.MoveAllApparelToSomeonesInventory(this, caravan.PawnsListForReading, base.Destroyed);
				}
				if (this.equipment != null)
				{
					CaravanInventoryUtility.MoveAllEquipmentToSomeonesInventory(this, caravan.PawnsListForReading);
				}
			}
			else
			{
				Corpse corpse = this.Corpse;
				IntVec3 pos = (corpse != null) ? corpse.PositionHeld : base.PositionHeld;
				Pawn_EquipmentTracker pawn_EquipmentTracker = this.equipment;
				if (pawn_EquipmentTracker != null)
				{
					pawn_EquipmentTracker.DropAllEquipment(pos, false, false);
				}
				Pawn_ApparelTracker pawn_ApparelTracker = this.apparel;
				if (pawn_ApparelTracker != null)
				{
					pawn_ApparelTracker.DropAll(pos, false, base.Destroyed, null);
				}
				Pawn_InventoryTracker pawn_InventoryTracker = this.inventory;
				if (pawn_InventoryTracker != null)
				{
					pawn_InventoryTracker.DropAllNearPawn(pos, false, false);
				}
			}
			if (notifyFaction && base.Faction != null)
			{
				base.Faction.Notify_MemberStripped(this, Faction.OfPlayer);
			}
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x000FB488 File Offset: 0x000F9688
		public Thought_Memory GiveObservedThought(Pawn observer)
		{
			if (ModsConfig.AnomalyActive && base.Spawned && !this.Downed)
			{
				Pawn_MindState pawn_MindState = this.mindState;
				DutyDef dutyDef;
				if (pawn_MindState == null)
				{
					dutyDef = null;
				}
				else
				{
					PawnDuty duty = pawn_MindState.duty;
					dutyDef = ((duty != null) ? duty.def : null);
				}
				CompChimera compChimera;
				if (dutyDef == DutyDefOf.ChimeraAttack && this.TryGetComp(out compChimera))
				{
					return compChimera.GiveObservedThought(observer);
				}
			}
			return null;
		}

		// Token: 0x0600309B RID: 12443 RVA: 0x00002C42 File Offset: 0x00000E42
		public HistoryEventDef GiveObservedHistoryEvent(Pawn observer)
		{
			return null;
		}

		// Token: 0x0600309C RID: 12444 RVA: 0x000FB4E4 File Offset: 0x000F96E4
		public void HearClamor(Thing source, ClamorDef type)
		{
			if (this.Dead || this.Downed || this.Deathresting || this.IsSelfShutdown())
			{
				return;
			}
			if (type == ClamorDefOf.Movement || type == ClamorDefOf.BabyCry)
			{
				Pawn pawn = source as Pawn;
				if (pawn != null)
				{
					this.CheckForDisturbedSleep(pawn);
				}
				this.NotifyLordOfClamor(source, type);
				return;
			}
			if (type == ClamorDefOf.Harm)
			{
				if (base.Faction != Faction.OfPlayer && !this.Awake() && base.Faction == source.Faction && this.HostFaction == null)
				{
					this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
					if (this.CurJob != null)
					{
						this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
					this.NotifyLordOfClamor(source, type);
					return;
				}
			}
			else if (type == ClamorDefOf.Construction)
			{
				if (base.Faction != Faction.OfPlayer && !this.Awake() && base.Faction != source.Faction && this.HostFaction == null)
				{
					this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
					if (this.CurJob != null)
					{
						this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
					this.NotifyLordOfClamor(source, type);
					return;
				}
			}
			else if (type == ClamorDefOf.Ability)
			{
				if (base.Faction != Faction.OfPlayer && base.Faction != source.Faction && this.HostFaction == null)
				{
					if (!this.Awake())
					{
						this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
						if (this.CurJob != null)
						{
							this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
						}
					}
					this.NotifyLordOfClamor(source, type);
					return;
				}
			}
			else if (type == ClamorDefOf.Impact)
			{
				this.mindState.Notify_ClamorImpact(source);
				if (this.CurJob != null && !this.Awake())
				{
					this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				this.NotifyLordOfClamor(source, type);
			}
		}

		// Token: 0x0600309D RID: 12445 RVA: 0x000FB6DF File Offset: 0x000F98DF
		private void NotifyLordOfClamor(Thing source, ClamorDef type)
		{
			Lord lord = this.GetLord();
			if (lord == null)
			{
				return;
			}
			lord.Notify_Clamor(source, type);
		}

		// Token: 0x0600309E RID: 12446 RVA: 0x000FB6F4 File Offset: 0x000F98F4
		public override void Notify_UsedVerb(Pawn pawn, Verb verb)
		{
			base.Notify_UsedVerb(pawn, verb);
			if (Rand.Chance((this.IsMutant && this.mutant.Def.soundAttackChance > 0f) ? this.mutant.Def.soundAttackChance : this.ageTracker.CurLifeStage.soundAttackChance))
			{
				LifeStageUtility.PlayNearestLifestageSound(pawn, (LifeStageAge lifeStage) => lifeStage.soundAttack, null, (MutantDef mutantDef) => mutantDef.soundAttack, 1f);
			}
			this.UpdatePyroVerbThought(verb);
		}

		// Token: 0x0600309F RID: 12447 RVA: 0x000FB7A2 File Offset: 0x000F99A2
		public override void Notify_Explosion(Explosion explosion)
		{
			base.Notify_Explosion(explosion);
			this.mindState.Notify_Explosion(explosion);
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x000FB7B7 File Offset: 0x000F99B7
		public override void Notify_BulletImpactNearby(BulletImpactData impactData)
		{
			Pawn_ApparelTracker pawn_ApparelTracker = this.apparel;
			if (pawn_ApparelTracker == null)
			{
				return;
			}
			pawn_ApparelTracker.Notify_BulletImpactNearby(impactData);
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x000FB7CC File Offset: 0x000F99CC
		public virtual void Notify_Downed()
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].Notify_Downed();
			}
		}

		// Token: 0x060030A2 RID: 12450 RVA: 0x000FB800 File Offset: 0x000F9A00
		public virtual void Notify_Released()
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].Notify_Released();
			}
			if (ModsConfig.AnomalyActive)
			{
				Pawn_CreepJoinerTracker pawn_CreepJoinerTracker = this.creepjoiner;
				if (pawn_CreepJoinerTracker == null)
				{
					return;
				}
				pawn_CreepJoinerTracker.Notify_Released();
			}
		}

		// Token: 0x060030A3 RID: 12451 RVA: 0x000FB848 File Offset: 0x000F9A48
		public virtual void Notify_PrisonBreakout()
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].Notify_PrisonBreakout();
			}
			if (ModsConfig.AnomalyActive)
			{
				Pawn_CreepJoinerTracker pawn_CreepJoinerTracker = this.creepjoiner;
				if (pawn_CreepJoinerTracker == null)
				{
					return;
				}
				pawn_CreepJoinerTracker.Notify_PrisonBreakout();
			}
		}

		// Token: 0x060030A4 RID: 12452 RVA: 0x000FB890 File Offset: 0x000F9A90
		public virtual void Notify_DuplicatedFrom(Pawn source)
		{
			List<ThingComp> allComps = base.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				allComps[i].Notify_DuplicatedFrom(source);
			}
			if (ModsConfig.AnomalyActive)
			{
				Pawn_CreepJoinerTracker pawn_CreepJoinerTracker = this.creepjoiner;
				if (pawn_CreepJoinerTracker == null)
				{
					return;
				}
				pawn_CreepJoinerTracker.Notify_DuplicatedFrom(source);
			}
		}

		// Token: 0x060030A5 RID: 12453 RVA: 0x000FB8DC File Offset: 0x000F9ADC
		private void CheckForDisturbedSleep(Pawn source)
		{
			if (this.needs.mood == null)
			{
				return;
			}
			if (this.Awake())
			{
				return;
			}
			if (base.Faction != Faction.OfPlayer)
			{
				return;
			}
			if (Find.TickManager.TicksGame < this.lastSleepDisturbedTick + 300)
			{
				return;
			}
			if (this.Deathresting)
			{
				return;
			}
			if (source != null)
			{
				if (LovePartnerRelationUtility.LovePartnerRelationExists(this, source))
				{
					return;
				}
				if (source.RaceProps.petness > 0f)
				{
					return;
				}
				if (source.relations != null)
				{
					if (source.relations.DirectRelations.Any((DirectPawnRelation dr) => dr.def == PawnRelationDefOf.Bond))
					{
						return;
					}
				}
			}
			this.lastSleepDisturbedTick = Find.TickManager.TicksGame;
			this.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleepDisturbed, null, null);
		}

		// Token: 0x060030A6 RID: 12454 RVA: 0x000FB9BC File Offset: 0x000F9BBC
		public float GetAcceptArrestChance(Pawn arrester)
		{
			if (this.Downed || this.WorkTagIsDisabled(WorkTags.Violent) || (this.guilt != null && this.guilt.IsGuilty && this.IsColonist && !this.IsQuestLodger()))
			{
				return 1f;
			}
			if (ModsConfig.AnomalyActive && this.health.hediffSet.HasHediff(HediffDefOf.RevenantHypnosis, false))
			{
				return 1f;
			}
			if (ModsConfig.BiotechActive && this.genes != null && this.genes.AggroMentalBreakSelectionChanceFactor <= 0f)
			{
				return 1f;
			}
			return (StatDefOf.ArrestSuccessChance.Worker.IsDisabledFor(arrester) ? StatDefOf.ArrestSuccessChance.valueIfMissing : arrester.GetStatValue(StatDefOf.ArrestSuccessChance, true, -1)) * this.kindDef.acceptArrestChanceFactor;
		}

		// Token: 0x060030A7 RID: 12455 RVA: 0x000FBA88 File Offset: 0x000F9C88
		public bool CheckAcceptArrest(Pawn arrester)
		{
			float acceptArrestChance = this.GetAcceptArrestChance(arrester);
			Faction homeFaction = this.HomeFaction;
			if (homeFaction != null && homeFaction != arrester.factionInt)
			{
				homeFaction.Notify_MemberCaptured(this, arrester.Faction);
			}
			List<ThingComp> allComps = base.AllComps;
			if (this.Downed || this.WorkTagIsDisabled(WorkTags.Violent) || Rand.Value < acceptArrestChance)
			{
				for (int i = 0; i < allComps.Count; i++)
				{
					allComps[i].Notify_Arrested(true);
					if (ModsConfig.AnomalyActive)
					{
						Pawn_CreepJoinerTracker pawn_CreepJoinerTracker = this.creepjoiner;
						if (pawn_CreepJoinerTracker != null)
						{
							pawn_CreepJoinerTracker.Notify_Arrested(true);
						}
					}
				}
				return true;
			}
			Messages.Message("MessageRefusedArrest".Translate(this.LabelShort, this), this, MessageTypeDefOf.ThreatSmall, true);
			for (int j = 0; j < allComps.Count; j++)
			{
				allComps[j].Notify_Arrested(false);
			}
			if (ModsConfig.AnomalyActive)
			{
				Pawn_CreepJoinerTracker pawn_CreepJoinerTracker2 = this.creepjoiner;
				if (pawn_CreepJoinerTracker2 != null)
				{
					pawn_CreepJoinerTracker2.Notify_Arrested(false);
				}
			}
			if (base.Faction == null || !arrester.HostileTo(this))
			{
				this.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, false, false, false, null, false, false, false);
			}
			return false;
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x000FBBB4 File Offset: 0x000F9DB4
		public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
		{
			if (!base.Spawned)
			{
				return true;
			}
			if (!this.InMentalState && this.GetTraderCaravanRole() == TraderCaravanRole.Carrier && !(this.jobs.curDriver is JobDriver_AttackMelee))
			{
				return true;
			}
			if (this.mindState.duty != null && this.mindState.duty.def.threatDisabled)
			{
				return true;
			}
			if (!this.mindState.Active)
			{
				return true;
			}
			if (this.IsColonyMechRequiringMechanitor())
			{
				return true;
			}
			Pawn pawn = ((disabledFor != null) ? disabledFor.Thing : null) as Pawn;
			if (this.Downed && (!this.CanAttackWhileCrawling || !this.Crawling))
			{
				if (disabledFor == null)
				{
					return true;
				}
				bool flag;
				if (pawn == null)
				{
					flag = (null != null);
				}
				else
				{
					Pawn_MindState pawn_MindState = pawn.mindState;
					flag = (((pawn_MindState != null) ? pawn_MindState.duty : null) != null);
				}
				if (!flag || !pawn.mindState.duty.attackDownedIfStarving || !pawn.Starving())
				{
					return true;
				}
			}
			CompActivity compActivity;
			return (ModsConfig.AnomalyActive && this.TryGetComp(out compActivity) && compActivity.IsDormant) || this.IsPsychologicallyInvisible() || (this.ThreatDisabledBecauseNonAggressiveRoamer(pawn) || (pawn != null && pawn.ThreatDisabledBecauseNonAggressiveRoamer(this)));
		}

		// Token: 0x060030A9 RID: 12457 RVA: 0x000FBCD4 File Offset: 0x000F9ED4
		public bool ThreatDisabledBecauseNonAggressiveRoamer(Pawn otherPawn)
		{
			if (!this.RaceProps.Roamer || base.Faction != Faction.OfPlayer)
			{
				return false;
			}
			Lord lord = (otherPawn != null) ? otherPawn.GetLord() : null;
			return (lord == null || !lord.CurLordToil.AllowAggressiveTargetingOfRoamers) && !this.InAggroMentalState && !this.IsFighting() && Find.TickManager.TicksGame >= this.mindState.lastEngageTargetTick + 360;
		}

		// Token: 0x060030AA RID: 12458 RVA: 0x000FBD4C File Offset: 0x000F9F4C
		private void UpdatePyroVerbThought(Verb verb)
		{
			if (this.story == null)
			{
				return;
			}
			if (!this.story.traits.HasTrait(TraitDefOf.Pyromaniac))
			{
				return;
			}
			if (!verb.IsIncendiary_Melee() && !verb.IsIncendiary_Ranged())
			{
				return;
			}
			if (verb.CurrentTarget.Pawn == null || !verb.CurrentTarget.Pawn.Spawned || !this.IsValidPyroThoughtTarget(verb.CurrentTarget.Pawn))
			{
				foreach (IntVec3 c in GenRadial.RadialCellsAround(verb.CurrentTarget.Cell, verb.EffectiveRange, true))
				{
					if (c.InBounds(base.MapHeld))
					{
						foreach (Thing thing in c.GetThingList(base.MapHeld))
						{
							if (this.IsValidPyroThoughtTarget(thing))
							{
								Pawn_NeedsTracker pawn_NeedsTracker = this.needs;
								if (pawn_NeedsTracker == null)
								{
									break;
								}
								Need_Mood mood = pawn_NeedsTracker.mood;
								if (mood == null)
								{
									break;
								}
								ThoughtHandler thoughts = mood.thoughts;
								if (thoughts == null)
								{
									break;
								}
								MemoryThoughtHandler memories = thoughts.memories;
								if (memories == null)
								{
									break;
								}
								memories.TryGainMemory(ThoughtDefOf.PyroUsed, null, null);
								break;
							}
						}
					}
				}
				return;
			}
			Pawn_NeedsTracker pawn_NeedsTracker2 = this.needs;
			if (pawn_NeedsTracker2 == null)
			{
				return;
			}
			Need_Mood mood2 = pawn_NeedsTracker2.mood;
			if (mood2 == null)
			{
				return;
			}
			ThoughtHandler thoughts2 = mood2.thoughts;
			if (thoughts2 == null)
			{
				return;
			}
			MemoryThoughtHandler memories2 = thoughts2.memories;
			if (memories2 == null)
			{
				return;
			}
			memories2.TryGainMemory(ThoughtDefOf.PyroUsed, null, null);
		}

		// Token: 0x060030AB RID: 12459 RVA: 0x000FBEE4 File Offset: 0x000FA0E4
		private bool IsValidPyroThoughtTarget(Thing thing)
		{
			Pawn pawn = thing as Pawn;
			return pawn != null && !pawn.Downed && !pawn.IsPsychologicallyInvisible() && !pawn.Fogged() && pawn.HostileTo(Faction.OfPlayer);
		}

		// Token: 0x060030AC RID: 12460 RVA: 0x000FBF20 File Offset: 0x000FA120
		public List<WorkTypeDef> GetDisabledWorkTypes(bool permanentOnly = false)
		{
			Pawn.<>c__DisplayClass363_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.permanentOnly = permanentOnly;
			if (Scribe.mode != LoadSaveMode.Inactive)
			{
				this.cachedDisabledWorkTypesPermanent = null;
				this.cachedDisabledWorkTypes = null;
			}
			if (CS$<>8__locals1.permanentOnly)
			{
				if (this.cachedDisabledWorkTypesPermanent == null)
				{
					this.cachedDisabledWorkTypesPermanent = new List<WorkTypeDef>();
					this.<GetDisabledWorkTypes>g__FillList|363_0(this.cachedDisabledWorkTypesPermanent, ref CS$<>8__locals1);
				}
				return this.cachedDisabledWorkTypesPermanent;
			}
			if (this.cachedDisabledWorkTypes == null)
			{
				this.cachedDisabledWorkTypes = new List<WorkTypeDef>();
				this.<GetDisabledWorkTypes>g__FillList|363_0(this.cachedDisabledWorkTypes, ref CS$<>8__locals1);
			}
			return this.cachedDisabledWorkTypes;
		}

		// Token: 0x060030AD RID: 12461 RVA: 0x000FBFAC File Offset: 0x000FA1AC
		public List<string> GetReasonsForDisabledWorkType(WorkTypeDef workType)
		{
			List<string> result;
			if (this.cachedReasonsForDisabledWorkTypes != null && this.cachedReasonsForDisabledWorkTypes.TryGetValue(workType, out result))
			{
				return result;
			}
			List<string> list = new List<string>();
			foreach (BackstoryDef backstoryDef in this.story.AllBackstories)
			{
				foreach (WorkTypeDef workTypeDef in backstoryDef.DisabledWorkTypes)
				{
					if (workType == workTypeDef)
					{
						list.Add("WorkDisabledByBackstory".Translate(backstoryDef.TitleCapFor(this.gender)));
						break;
					}
				}
			}
			for (int i = 0; i < this.story.traits.allTraits.Count; i++)
			{
				Trait trait = this.story.traits.allTraits[i];
				using (List<WorkTypeDef>.Enumerator enumerator2 = trait.GetDisabledWorkTypes().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current == workType && !trait.Suppressed)
						{
							list.Add("WorkDisabledByTrait".Translate(trait.LabelCap));
							break;
						}
					}
				}
			}
			if (this.royalty != null)
			{
				foreach (RoyalTitle royalTitle in this.royalty.AllTitlesForReading)
				{
					if (royalTitle.conceited)
					{
						foreach (WorkTypeDef workTypeDef2 in royalTitle.def.DisabledWorkTypes)
						{
							if (workType == workTypeDef2)
							{
								list.Add("WorkDisabledByRoyalTitle".Translate(royalTitle.Label));
								break;
							}
						}
					}
				}
			}
			if (ModsConfig.IdeologyActive && this.Ideo != null)
			{
				Precept_Role role = this.Ideo.GetRole(this);
				if (role != null)
				{
					foreach (WorkTypeDef workTypeDef3 in role.DisabledWorkTypes)
					{
						if (workType == workTypeDef3)
						{
							list.Add("WorkDisabledRole".Translate(role.LabelForPawn(this)));
							break;
						}
					}
				}
			}
			if (this.IsMutant)
			{
				foreach (WorkTypeDef workTypeDef4 in this.mutant.Def.DisabledWorkTypes)
				{
					if (workType == workTypeDef4)
					{
						list.Add(this.mutant.Def.LabelCap);
						break;
					}
				}
			}
			foreach (QuestPart_WorkDisabled questPart_WorkDisabled in QuestUtility.GetWorkDisabledQuestPart(this))
			{
				foreach (WorkTypeDef workTypeDef5 in questPart_WorkDisabled.DisabledWorkTypes)
				{
					if (workType == workTypeDef5)
					{
						list.Add("WorkDisabledByQuest".Translate(questPart_WorkDisabled.quest.name));
						break;
					}
				}
			}
			if (this.guest != null && this.guest.IsSlave)
			{
				foreach (WorkTypeDef workTypeDef6 in this.guest.GetDisabledWorkTypes())
				{
					if (workType == workTypeDef6)
					{
						list.Add("WorkDisabledSlave".Translate());
						break;
					}
				}
			}
			if (this.health != null)
			{
				foreach (WorkTypeDef workTypeDef7 in this.health.DisabledWorkTypes)
				{
					if (workType == workTypeDef7)
					{
						list.Add("WorkDisabledHealth".Translate());
						break;
					}
				}
			}
			int value;
			if (this.IsWorkTypeDisabledByAge(workType, out value))
			{
				list.Add("WorkDisabledAge".Translate(this, this.ageTracker.AgeBiologicalYears, workType.labelShort, value));
			}
			if (this.cachedReasonsForDisabledWorkTypes == null)
			{
				this.cachedReasonsForDisabledWorkTypes = new Dictionary<WorkTypeDef, List<string>>();
			}
			this.cachedReasonsForDisabledWorkTypes[workType] = list;
			return list;
		}

		// Token: 0x060030AE RID: 12462 RVA: 0x000FC4D0 File Offset: 0x000FA6D0
		public bool WorkTypeIsDisabled(WorkTypeDef w)
		{
			return this.GetDisabledWorkTypes(false).Contains(w);
		}

		// Token: 0x060030AF RID: 12463 RVA: 0x000FC4E0 File Offset: 0x000FA6E0
		public bool OneOfWorkTypesIsDisabled(List<WorkTypeDef> wts)
		{
			for (int i = 0; i < wts.Count; i++)
			{
				if (this.WorkTypeIsDisabled(wts[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060030B0 RID: 12464 RVA: 0x000FC510 File Offset: 0x000FA710
		public void Notify_DisabledWorkTypesChanged()
		{
			this.cachedDisabledWorkTypes = null;
			this.cachedDisabledWorkTypesPermanent = null;
			this.cachedReasonsForDisabledWorkTypes = null;
			Pawn_WorkSettings pawn_WorkSettings = this.workSettings;
			if (pawn_WorkSettings != null)
			{
				pawn_WorkSettings.Notify_DisabledWorkTypesChanged();
			}
			Pawn_SkillTracker pawn_SkillTracker = this.skills;
			if (pawn_SkillTracker == null)
			{
				return;
			}
			pawn_SkillTracker.Notify_SkillDisablesChanged();
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x060030B1 RID: 12465 RVA: 0x000FC548 File Offset: 0x000FA748
		public WorkTags CombinedDisabledWorkTags
		{
			get
			{
				Pawn_StoryTracker pawn_StoryTracker = this.story;
				WorkTags workTags = (pawn_StoryTracker != null) ? pawn_StoryTracker.DisabledWorkTagsBackstoryTraitsAndGenes : WorkTags.None;
				workTags |= this.kindDef.disabledWorkTags;
				if (this.royalty != null)
				{
					foreach (RoyalTitle royalTitle in this.royalty.AllTitlesForReading)
					{
						if (royalTitle.conceited)
						{
							workTags |= royalTitle.def.disabledWorkTags;
						}
					}
				}
				if (ModsConfig.IdeologyActive && this.Ideo != null)
				{
					Precept_Role role = this.Ideo.GetRole(this);
					if (role != null)
					{
						workTags |= role.def.roleDisabledWorkTags;
					}
				}
				Pawn_HealthTracker pawn_HealthTracker = this.health;
				if (((pawn_HealthTracker != null) ? pawn_HealthTracker.hediffSet : null) != null)
				{
					foreach (Hediff hediff in this.health.hediffSet.hediffs)
					{
						HediffStage curStage = hediff.CurStage;
						if (curStage != null)
						{
							workTags |= curStage.disabledWorkTags;
						}
					}
				}
				foreach (QuestPart_WorkDisabled questPart_WorkDisabled in QuestUtility.GetWorkDisabledQuestPart(this))
				{
					workTags |= questPart_WorkDisabled.disabledWorkTags;
				}
				if (this.IsMutant)
				{
					workTags |= this.mutant.Def.workDisables;
					if (!this.mutant.IsPassive)
					{
						workTags &= ~WorkTags.Violent;
					}
				}
				return workTags;
			}
		}

		// Token: 0x060030B2 RID: 12466 RVA: 0x000FC6E8 File Offset: 0x000FA8E8
		public bool WorkTagIsDisabled(WorkTags w)
		{
			return (this.CombinedDisabledWorkTags & w) > WorkTags.None;
		}

		// Token: 0x060030B3 RID: 12467 RVA: 0x000FC6F8 File Offset: 0x000FA8F8
		public override bool PreventPlayerSellingThingsNearby(out string reason)
		{
			if (base.Faction.HostileTo(Faction.OfPlayer) && this.HostFaction == null && !this.Downed && !this.InMentalState)
			{
				reason = "Enemies".Translate();
				return true;
			}
			reason = null;
			return false;
		}

		// Token: 0x060030B4 RID: 12468 RVA: 0x000FC746 File Offset: 0x000FA946
		public void ChangeKind(PawnKindDef newKindDef)
		{
			if (this.kindDef == newKindDef)
			{
				return;
			}
			this.kindDef = newKindDef;
			if (this.IsWildMan())
			{
				this.mindState.WildManEverReachedOutside = false;
				ReachabilityUtility.ClearCacheFor(this);
			}
		}

		// Token: 0x060030B5 RID: 12469 RVA: 0x000FC774 File Offset: 0x000FA974
		public bool CompsWantHoldWeapon()
		{
			using (List<ThingComp>.Enumerator enumerator = base.AllComps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.WantHoldWeapon(this))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060030B6 RID: 12470 RVA: 0x000FC7D0 File Offset: 0x000FA9D0
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.canBeDormant = base.GetComp<CompCanBeDormant>();
				this.activity = base.GetComp<CompActivity>();
			}
			Scribe_Defs.Look<PawnKindDef>(ref this.kindDef, "kindDef");
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.Male, false);
			Scribe_Values.Look<int>(ref this.becameWorldPawnTickAbs, "becameWorldPawnTickAbs", -1, false);
			Scribe_Values.Look<bool>(ref this.teleporting, "teleporting", false, false);
			Scribe_Values.Look<int>(ref this.showNamePromptOnTick, "showNamePromptOnTick", -1, false);
			Scribe_Values.Look<int>(ref this.babyNamingDeadline, "babyNamingDeadline", -1, false);
			Scribe_Values.Look<bool>(ref this.addCorpseToLord, "addCorpseToLord", false, false);
			Scribe_Values.Look<int>(ref this.timesRaisedAsShambler, "timesRaisedAsShambler", 0, false);
			Scribe_Values.Look<int>(ref this.lastSleepDisturbedTick, "lastSleepDisturbedTick", 0, false);
			Scribe_Values.Look<bool>(ref this.dontGivePreArrivalPathway, "dontGivePreArrivalPathway", false, false);
			Scribe_Values.Look<int>(ref this.lastVacuumBurntTick, "lastVacuumBurntTick", 0, false);
			Scribe_Deep.Look<Name>(ref this.nameInt, "name", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.Saving && GenTicks.TicksGame - this.deadlifeDustFactionTick > 12500)
			{
				this.deadlifeDustFactionTick = 0;
				this.deadlifeDustFaction = null;
			}
			Scribe_Values.Look<int>(ref this.deadlifeDustFactionTick, "deadlifeDustFactionTick", 0, false);
			Scribe_References.Look<Faction>(ref this.deadlifeDustFaction, "deadlifeDustFaction", false);
			Scribe_Deep.Look<Pawn_MindState>(ref this.mindState, "mindState", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_JobTracker>(ref this.jobs, "jobs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StanceTracker>(ref this.stances, "stances", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InfectionVectorTracker>(ref this.infectionVectors, "infectionVectors", new object[]
			{
				this
			});
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_NativeVerbs>(ref this.natives, "natives", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_MeleeVerbs>(ref this.meleeVerbs, "meleeVerbs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RotationTracker>(ref this.rotationTracker, "rotationTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PathFollower>(ref this.pather, "pather", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_CarryTracker>(ref this.carryTracker, "carryTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_ApparelTracker>(ref this.apparel, "apparel", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StoryTracker>(ref this.story, "story", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_EquipmentTracker>(ref this.equipment, "equipment", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_DraftController>(ref this.drafter, "drafter", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_AgeTracker>(ref this.ageTracker, "ageTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_HealthTracker>(ref this.health, "healthTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RecordsTracker>(ref this.records, "records", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InventoryTracker>(ref this.inventory, "inventory", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_FilthTracker>(ref this.filth, "filth", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RopeTracker>(ref this.roping, "roping", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_NeedsTracker>(ref this.needs, "needs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_GuestTracker>(ref this.guest, "guest", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_GuiltTracker>(ref this.guilt, "guilt", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RoyaltyTracker>(ref this.royalty, "royalty", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RelationsTracker>(ref this.relations, "social", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PsychicEntropyTracker>(ref this.psychicEntropy, "psychicEntropy", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_MutantTracker>(ref this.mutant, "shambler", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_Ownership>(ref this.ownership, "ownership", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InteractionsTracker>(ref this.interactions, "interactions", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_SkillTracker>(ref this.skills, "skills", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_AbilityTracker>(ref this.abilities, "abilities", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_IdeoTracker>(ref this.ideo, "ideo", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_WorkSettings>(ref this.workSettings, "workSettings", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TraderTracker>(ref this.trader, "trader", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_OutfitTracker>(ref this.outfits, "outfits", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_DrugPolicyTracker>(ref this.drugs, "drugs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_FoodRestrictionTracker>(ref this.foodRestriction, "foodRestriction", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TimetableTracker>(ref this.timetable, "timetable", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PlayerSettings>(ref this.playerSettings, "playerSettings", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TrainingTracker>(ref this.training, "training", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StyleTracker>(ref this.style, "style", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StyleObserverTracker>(ref this.styleObserver, "styleObserver", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_ConnectionsTracker>(ref this.connections, "connections", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InventoryStockTracker>(ref this.inventoryStock, "inventoryStock", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_SurroundingsTracker>(ref this.surroundings, "treeSightings", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_Thinker>(ref this.thinker, "thinker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_MechanitorTracker>(ref this.mechanitor, "mechanitor", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_GeneTracker>(ref this.genes, "genes", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_LearningTracker>(ref this.learning, "learning", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_ReadingTracker>(ref this.reading, "reading", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_CreepJoinerTracker>(ref this.creepjoiner, "creepjoiner", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_DuplicateTracker>(ref this.duplicate, "duplicate", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_FlightTracker>(ref this.flight, "flight", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.wasLeftBehindStartingPawn, "wasLeftBehindStartingPawn", false, false);
			Scribe_Values.Look<bool>(ref this.everLostEgo, "everBrainWiped", false, false);
			Scribe_Values.Look<bool>(ref this.wasDraftedBeforeSkip, "wasDraftedBeforeSkip", false, false);
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x060030B7 RID: 12471 RVA: 0x000FCEC8 File Offset: 0x000FB0C8
		public override string ToString()
		{
			if (this.story != null)
			{
				return this.LabelShort;
			}
			if (this.thingIDNumber > 0)
			{
				return base.ThingID;
			}
			if (this.kindDef != null)
			{
				return this.KindLabel + "_" + base.ThingID;
			}
			if (this.def != null)
			{
				return base.ThingID;
			}
			return base.GetType().ToString();
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x060030B8 RID: 12472 RVA: 0x000FCF2D File Offset: 0x000FB12D
		public TraderKindDef TraderKind
		{
			get
			{
				Pawn_TraderTracker pawn_TraderTracker = this.trader;
				if (pawn_TraderTracker == null)
				{
					return null;
				}
				return pawn_TraderTracker.traderKind;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x060030B9 RID: 12473 RVA: 0x000FCF40 File Offset: 0x000FB140
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.TraderKind.tradeCurrency;
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x060030BA RID: 12474 RVA: 0x000FCF4D File Offset: 0x000FB14D
		public IEnumerable<Thing> Goods
		{
			get
			{
				return this.trader.Goods;
			}
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x060030BB RID: 12475 RVA: 0x000FCF5A File Offset: 0x000FB15A
		public int RandomPriceFactorSeed
		{
			get
			{
				return this.trader.RandomPriceFactorSeed;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x060030BC RID: 12476 RVA: 0x000FCF67 File Offset: 0x000FB167
		public string TraderName
		{
			get
			{
				return this.trader.TraderName;
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x060030BD RID: 12477 RVA: 0x000FCF74 File Offset: 0x000FB174
		public bool CanTradeNow
		{
			get
			{
				return this.trader != null && this.trader.CanTradeNow;
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x060030BE RID: 12478 RVA: 0x00006AAC File Offset: 0x00004CAC
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x060030BF RID: 12479 RVA: 0x000FCF8B File Offset: 0x000FB18B
		public float BodySize
		{
			get
			{
				return this.ageTracker.CurLifeStage.bodySizeFactor * this.RaceProps.baseBodySize;
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x060030C0 RID: 12480 RVA: 0x000FCFA9 File Offset: 0x000FB1A9
		public float HealthScale
		{
			get
			{
				return this.ageTracker.CurLifeStage.healthScaleFactor * this.RaceProps.baseHealthScale;
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x060030C1 RID: 12481 RVA: 0x000FCFC7 File Offset: 0x000FB1C7
		public IEnumerable<Thing> EquippedWornOrInventoryThings
		{
			get
			{
				IEnumerable<Thing> innerContainer = this.inventory.innerContainer;
				Pawn_ApparelTracker pawn_ApparelTracker = this.apparel;
				IEnumerable<Thing> first = innerContainer.ConcatIfNotNull((pawn_ApparelTracker != null) ? pawn_ApparelTracker.WornApparel : null);
				Pawn_EquipmentTracker pawn_EquipmentTracker = this.equipment;
				return first.ConcatIfNotNull((pawn_EquipmentTracker != null) ? pawn_EquipmentTracker.AllEquipmentListForReading : null);
			}
		}

		// Token: 0x060030C2 RID: 12482 RVA: 0x000FD002 File Offset: 0x000FB202
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			return this.trader.ColonyThingsWillingToBuy(playerNegotiator);
		}

		// Token: 0x060030C3 RID: 12483 RVA: 0x000FD010 File Offset: 0x000FB210
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x060030C4 RID: 12484 RVA: 0x000FD020 File Offset: 0x000FB220
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x060030C5 RID: 12485 RVA: 0x000FD030 File Offset: 0x000FB230
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x060030C6 RID: 12486 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public float TargetPriorityFactor
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x060030C7 RID: 12487 RVA: 0x000FD034 File Offset: 0x000FB234
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				if (!base.Spawned)
				{
					return LocalTargetInfo.Invalid;
				}
				Stance curStance = this.stances.curStance;
				if (curStance is Stance_Warmup || curStance is Stance_Cooldown)
				{
					return ((Stance_Busy)curStance).focusTarg;
				}
				return LocalTargetInfo.Invalid;
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x060030C8 RID: 12488 RVA: 0x000FD030 File Offset: 0x000FB230
		Thing IAttackTargetSearcher.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x060030C9 RID: 12489 RVA: 0x000FD07C File Offset: 0x000FB27C
		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.mindState.lastAttackedTarget;
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x060030CA RID: 12490 RVA: 0x000FD089 File Offset: 0x000FB289
		public int LastAttackTargetTick
		{
			get
			{
				return this.mindState.lastAttackTargetTick;
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x060030CB RID: 12491 RVA: 0x000FD098 File Offset: 0x000FB298
		public Verb CurrentEffectiveVerb
		{
			get
			{
				Building_Turret building_Turret = this.MannedThing() as Building_Turret;
				if (building_Turret != null)
				{
					return building_Turret.AttackVerb;
				}
				return this.TryGetAttackVerb(null, !this.IsColonist, false);
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x060030CC RID: 12492 RVA: 0x000FD0CC File Offset: 0x000FB2CC
		private bool ForceNoDeathNotification
		{
			get
			{
				return this.forceNoDeathNotification || this.kindDef.forceNoDeathNotification;
			}
		}

		// Token: 0x060030CD RID: 12493 RVA: 0x000FD0E3 File Offset: 0x000FB2E3
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return base.GetUniqueLoadID();
		}

		// Token: 0x060030CE RID: 12494 RVA: 0x000FD0EB File Offset: 0x000FB2EB
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p == this;
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x060030CF RID: 12495 RVA: 0x000FD030 File Offset: 0x000FB230
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x060030D0 RID: 12496 RVA: 0x000FD0F1 File Offset: 0x000FB2F1
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Bodypart;
			}
		}

		// Token: 0x060030D1 RID: 12497 RVA: 0x000FD0F8 File Offset: 0x000FB2F8
		public PlanetTile GetRootTile()
		{
			return base.Tile;
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x00002C42 File Offset: 0x00000E42
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x000FD100 File Offset: 0x000FB300
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.inventory != null)
			{
				outChildren.Add(this.inventory);
			}
			if (this.carryTracker != null)
			{
				outChildren.Add(this.carryTracker);
			}
			if (this.equipment != null)
			{
				outChildren.Add(this.equipment);
			}
			if (this.apparel != null)
			{
				outChildren.Add(this.apparel);
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x060030D4 RID: 12500 RVA: 0x000FD169 File Offset: 0x000FB369
		public BillStack BillStack
		{
			get
			{
				return this.health.surgeryBills;
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x060030D5 RID: 12501 RVA: 0x000FD178 File Offset: 0x000FB378
		public override IntVec3 InteractionCell
		{
			get
			{
				Building_Bed building_Bed = this.CurrentBed();
				IntVec3? intVec = (building_Bed != null) ? building_Bed.FindPreferredInteractionCell(base.Position, null) : null;
				if (intVec == null)
				{
					return base.InteractionCell;
				}
				return intVec.GetValueOrDefault();
			}
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x000FD1C0 File Offset: 0x000FB3C0
		public bool CurrentlyUsableForBills()
		{
			if (!this.InBed())
			{
				JobFailReason.Is(Pawn.NotSurgeryReadyTrans, null);
				return false;
			}
			if (!this.InteractionCell.IsValid)
			{
				JobFailReason.Is(Pawn.CannotReachTrans, null);
				return false;
			}
			return true;
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x000FD200 File Offset: 0x000FB400
		public bool UsableForBillsAfterFueling()
		{
			return this.CurrentlyUsableForBills();
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x000FD208 File Offset: 0x000FB408
		public void Notify_BillDeleted(Bill bill)
		{
			Xenogerm xenogerm = bill.xenogerm;
			if (xenogerm == null)
			{
				return;
			}
			xenogerm.Notify_BillRemoved();
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x000FD21A File Offset: 0x000FB41A
		public bool Equals(Pawn other)
		{
			return this.def.defName == other.def.defName && this.thingIDNumber == other.thingIDNumber;
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x060030DA RID: 12506 RVA: 0x000FD249 File Offset: 0x000FB449
		public ThingOwner SearchableContents
		{
			get
			{
				Pawn_CarryTracker pawn_CarryTracker = this.carryTracker;
				if (pawn_CarryTracker == null)
				{
					return null;
				}
				return pawn_CarryTracker.innerContainer;
			}
		}

		// Token: 0x060030ED RID: 12525 RVA: 0x000FD46C File Offset: 0x000FB66C
		[CompilerGenerated]
		private void <GetDisabledWorkTypes>g__FillList|363_0(List<WorkTypeDef> list, ref Pawn.<>c__DisplayClass363_0 A_2)
		{
			if (this.IsMutant && this.mutant.HasTurned)
			{
				List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					foreach (WorkTypeDef item in this.mutant.Def.DisabledWorkTypes)
					{
						if (!list.Contains(item))
						{
							list.Add(item);
						}
					}
				}
				return;
			}
			if (this.story != null && !this.IsSlave)
			{
				foreach (BackstoryDef backstoryDef in this.story.AllBackstories)
				{
					foreach (WorkTypeDef item2 in backstoryDef.DisabledWorkTypes)
					{
						if (!list.Contains(item2))
						{
							list.Add(item2);
						}
					}
				}
				for (int j = 0; j < this.story.traits.allTraits.Count; j++)
				{
					if (!this.story.traits.allTraits[j].Suppressed)
					{
						foreach (WorkTypeDef item3 in this.story.traits.allTraits[j].GetDisabledWorkTypes())
						{
							if (!list.Contains(item3))
							{
								list.Add(item3);
							}
						}
					}
				}
			}
			if (ModsConfig.BiotechActive && this.IsColonyMech)
			{
				List<WorkTypeDef> allDefsListForReading2 = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				for (int k = 0; k < allDefsListForReading2.Count; k++)
				{
					if (!this.RaceProps.mechEnabledWorkTypes.Contains(allDefsListForReading2[k]) && !list.Contains(allDefsListForReading2[k]))
					{
						list.Add(allDefsListForReading2[k]);
					}
				}
			}
			if (!A_2.permanentOnly)
			{
				if (this.health != null)
				{
					foreach (WorkTypeDef item4 in this.health.DisabledWorkTypes)
					{
						if (!list.Contains(item4))
						{
							list.Add(item4);
						}
					}
				}
				if (this.royalty != null && !this.IsSlave)
				{
					foreach (RoyalTitle royalTitle in this.royalty.AllTitlesForReading)
					{
						if (royalTitle.conceited)
						{
							foreach (WorkTypeDef item5 in royalTitle.def.DisabledWorkTypes)
							{
								if (!list.Contains(item5))
								{
									list.Add(item5);
								}
							}
						}
					}
				}
				if (ModsConfig.IdeologyActive && this.Ideo != null)
				{
					Precept_Role role = this.Ideo.GetRole(this);
					if (role != null)
					{
						foreach (WorkTypeDef item6 in role.DisabledWorkTypes)
						{
							if (!list.Contains(item6))
							{
								list.Add(item6);
							}
						}
					}
				}
				if (ModsConfig.BiotechActive && this.genes != null)
				{
					foreach (Gene gene in this.genes.GenesListForReading)
					{
						foreach (WorkTypeDef item7 in gene.DisabledWorkTypes)
						{
							if (!list.Contains(item7))
							{
								list.Add(item7);
							}
						}
					}
				}
				foreach (QuestPart_WorkDisabled questPart_WorkDisabled in QuestUtility.GetWorkDisabledQuestPart(this))
				{
					foreach (WorkTypeDef item8 in questPart_WorkDisabled.DisabledWorkTypes)
					{
						if (!list.Contains(item8))
						{
							list.Add(item8);
						}
					}
				}
				if (this.guest != null)
				{
					foreach (WorkTypeDef item9 in this.guest.GetDisabledWorkTypes())
					{
						if (!list.Contains(item9))
						{
							list.Add(item9);
						}
					}
				}
				for (int l = 0; l < this.RaceProps.lifeStageWorkSettings.Count; l++)
				{
					LifeStageWorkSettings lifeStageWorkSettings = this.RaceProps.lifeStageWorkSettings[l];
					if (lifeStageWorkSettings.IsDisabled(this) && !list.Contains(lifeStageWorkSettings.workType))
					{
						list.Add(lifeStageWorkSettings.workType);
					}
				}
			}
		}

		// Token: 0x04002525 RID: 9509
		public PawnKindDef kindDef;

		// Token: 0x04002526 RID: 9510
		private Name nameInt;

		// Token: 0x04002527 RID: 9511
		public Gender gender;

		// Token: 0x04002528 RID: 9512
		public Pawn_AgeTracker ageTracker;

		// Token: 0x04002529 RID: 9513
		public Pawn_HealthTracker health;

		// Token: 0x0400252A RID: 9514
		public Pawn_RecordsTracker records;

		// Token: 0x0400252B RID: 9515
		public Pawn_InventoryTracker inventory;

		// Token: 0x0400252C RID: 9516
		public Pawn_MeleeVerbs meleeVerbs;

		// Token: 0x0400252D RID: 9517
		public VerbTracker verbTracker;

		// Token: 0x0400252E RID: 9518
		public Pawn_Ownership ownership;

		// Token: 0x0400252F RID: 9519
		public Pawn_CarryTracker carryTracker;

		// Token: 0x04002530 RID: 9520
		public Pawn_NeedsTracker needs;

		// Token: 0x04002531 RID: 9521
		public Pawn_MindState mindState;

		// Token: 0x04002532 RID: 9522
		public Pawn_SurroundingsTracker surroundings;

		// Token: 0x04002533 RID: 9523
		public Pawn_Thinker thinker;

		// Token: 0x04002534 RID: 9524
		public Pawn_JobTracker jobs;

		// Token: 0x04002535 RID: 9525
		public Pawn_StanceTracker stances;

		// Token: 0x04002536 RID: 9526
		public Pawn_InfectionVectorTracker infectionVectors;

		// Token: 0x04002537 RID: 9527
		public Pawn_DuplicateTracker duplicate;

		// Token: 0x04002538 RID: 9528
		public Pawn_RotationTracker rotationTracker;

		// Token: 0x04002539 RID: 9529
		public Pawn_PathFollower pather;

		// Token: 0x0400253A RID: 9530
		public Pawn_NativeVerbs natives;

		// Token: 0x0400253B RID: 9531
		public Pawn_FilthTracker filth;

		// Token: 0x0400253C RID: 9532
		public Pawn_RopeTracker roping;

		// Token: 0x0400253D RID: 9533
		public Pawn_FlightTracker flight;

		// Token: 0x0400253E RID: 9534
		public Pawn_EquipmentTracker equipment;

		// Token: 0x0400253F RID: 9535
		public Pawn_ApparelTracker apparel;

		// Token: 0x04002540 RID: 9536
		public Pawn_SkillTracker skills;

		// Token: 0x04002541 RID: 9537
		public Pawn_StoryTracker story;

		// Token: 0x04002542 RID: 9538
		public Pawn_GuestTracker guest;

		// Token: 0x04002543 RID: 9539
		public Pawn_GuiltTracker guilt;

		// Token: 0x04002544 RID: 9540
		public Pawn_RoyaltyTracker royalty;

		// Token: 0x04002545 RID: 9541
		public Pawn_AbilityTracker abilities;

		// Token: 0x04002546 RID: 9542
		public Pawn_IdeoTracker ideo;

		// Token: 0x04002547 RID: 9543
		public Pawn_GeneTracker genes;

		// Token: 0x04002548 RID: 9544
		public Pawn_CreepJoinerTracker creepjoiner;

		// Token: 0x04002549 RID: 9545
		public Pawn_WorkSettings workSettings;

		// Token: 0x0400254A RID: 9546
		public Pawn_TraderTracker trader;

		// Token: 0x0400254B RID: 9547
		public Pawn_StyleTracker style;

		// Token: 0x0400254C RID: 9548
		public Pawn_StyleObserverTracker styleObserver;

		// Token: 0x0400254D RID: 9549
		public Pawn_ConnectionsTracker connections;

		// Token: 0x0400254E RID: 9550
		public Pawn_TrainingTracker training;

		// Token: 0x0400254F RID: 9551
		public Pawn_CallTracker caller;

		// Token: 0x04002550 RID: 9552
		public Pawn_PsychicEntropyTracker psychicEntropy;

		// Token: 0x04002551 RID: 9553
		public Pawn_MutantTracker mutant;

		// Token: 0x04002552 RID: 9554
		public Pawn_RelationsTracker relations;

		// Token: 0x04002553 RID: 9555
		public Pawn_InteractionsTracker interactions;

		// Token: 0x04002554 RID: 9556
		public Pawn_PlayerSettings playerSettings;

		// Token: 0x04002555 RID: 9557
		public Pawn_OutfitTracker outfits;

		// Token: 0x04002556 RID: 9558
		public Pawn_DrugPolicyTracker drugs;

		// Token: 0x04002557 RID: 9559
		public Pawn_FoodRestrictionTracker foodRestriction;

		// Token: 0x04002558 RID: 9560
		public Pawn_TimetableTracker timetable;

		// Token: 0x04002559 RID: 9561
		public Pawn_InventoryStockTracker inventoryStock;

		// Token: 0x0400255A RID: 9562
		public Pawn_MechanitorTracker mechanitor;

		// Token: 0x0400255B RID: 9563
		public Pawn_LearningTracker learning;

		// Token: 0x0400255C RID: 9564
		public Pawn_ReadingTracker reading;

		// Token: 0x0400255D RID: 9565
		public Pawn_DraftController drafter;

		// Token: 0x0400255E RID: 9566
		public Lord lord;

		// Token: 0x0400255F RID: 9567
		public bool markedForDiscard;

		// Token: 0x04002560 RID: 9568
		private Pawn_DrawTracker drawer;

		// Token: 0x04002561 RID: 9569
		public int becameWorldPawnTickAbs = -1;

		// Token: 0x04002562 RID: 9570
		public bool teleporting;

		// Token: 0x04002563 RID: 9571
		public bool forceNoDeathNotification;

		// Token: 0x04002564 RID: 9572
		public int showNamePromptOnTick = -1;

		// Token: 0x04002565 RID: 9573
		public int babyNamingDeadline = -1;

		// Token: 0x04002566 RID: 9574
		private Sustainer sustainerAmbient;

		// Token: 0x04002567 RID: 9575
		private Sustainer sustainerMoving;

		// Token: 0x04002568 RID: 9576
		public bool addCorpseToLord;

		// Token: 0x04002569 RID: 9577
		public int timesRaisedAsShambler;

		// Token: 0x0400256A RID: 9578
		private int lastSleepDisturbedTick;

		// Token: 0x0400256B RID: 9579
		public int lastVacuumBurntTick;

		// Token: 0x0400256C RID: 9580
		public Map prevMap;

		// Token: 0x0400256D RID: 9581
		private Faction deadlifeDustFaction;

		// Token: 0x0400256E RID: 9582
		private int deadlifeDustFactionTick;

		// Token: 0x0400256F RID: 9583
		public bool wasLeftBehindStartingPawn;

		// Token: 0x04002570 RID: 9584
		public bool debugMaxMoveSpeed;

		// Token: 0x04002571 RID: 9585
		public bool wasDraftedBeforeSkip;

		// Token: 0x04002572 RID: 9586
		public bool dontGivePreArrivalPathway;

		// Token: 0x04002573 RID: 9587
		public bool everLostEgo;

		// Token: 0x04002574 RID: 9588
		private const float HumanSizedHeatOutput = 0.3f;

		// Token: 0x04002575 RID: 9589
		private const float AnimalHeatOutputFactor = 0.6f;

		// Token: 0x04002576 RID: 9590
		public const int DefaultBabyNamingPeriod = 60000;

		// Token: 0x04002577 RID: 9591
		public const int DefaultGrowthMomentChoicePeriod = 120000;

		// Token: 0x04002578 RID: 9592
		private const int SleepDisturbanceMinInterval = 300;

		// Token: 0x04002579 RID: 9593
		private const int DeadlifeFactionExpiryTicks = 12500;

		// Token: 0x0400257A RID: 9594
		private const float HeatPushMaxTemperature = 40f;

		// Token: 0x0400257B RID: 9595
		public const int MaxMoveTicks = 450;

		// Token: 0x0400257C RID: 9596
		private static string NotSurgeryReadyTrans;

		// Token: 0x0400257D RID: 9597
		private static string CannotReachTrans;

		// Token: 0x0400257E RID: 9598
		[Unsaved(false)]
		private CompOverseerSubject overseerSubject;

		// Token: 0x0400257F RID: 9599
		[Unsaved(false)]
		public CompCanBeDormant canBeDormant;

		// Token: 0x04002580 RID: 9600
		[Unsaved(false)]
		public CompActivity activity;

		// Token: 0x04002581 RID: 9601
		private static List<ExtraFaction> tmpExtraFactions = new List<ExtraFaction>();

		// Token: 0x04002582 RID: 9602
		private static List<string> states = new List<string>();

		// Token: 0x04002583 RID: 9603
		private List<WorkTypeDef> cachedDisabledWorkTypes;

		// Token: 0x04002584 RID: 9604
		private List<WorkTypeDef> cachedDisabledWorkTypesPermanent;

		// Token: 0x04002585 RID: 9605
		private Dictionary<WorkTypeDef, List<string>> cachedReasonsForDisabledWorkTypes;
	}
}
