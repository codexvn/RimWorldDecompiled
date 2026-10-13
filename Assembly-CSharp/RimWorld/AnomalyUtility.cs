using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Grammar;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020034FB RID: 13563
	public static class AnomalyUtility
	{
		// Token: 0x060130E5 RID: 78053 RVA: 0x005A69B4 File Offset: 0x005A4BB4
		public static UnnaturalCorpse MakeUnnaturalCorpse(Pawn pawn)
		{
			Pawn pawn2 = Find.PawnDuplicator.Duplicate(pawn);
			pawn2.apparel.DestroyAll(DestroyMode.Vanish);
			pawn2.health.SetDead();
			pawn2.SetFaction(null, null);
			UnnaturalCorpse unnaturalCorpse = (UnnaturalCorpse)ThingMaker.MakeThing(pawn.def.race.unnaturalCorpseDef, null);
			unnaturalCorpse.InnerPawn = pawn2;
			unnaturalCorpse.Age = Mathf.RoundToInt(AnomalyUtility.UnnaturalCorpseAgeRangeDays.RandomInRange * 60000f);
			return unnaturalCorpse;
		}

		// Token: 0x060130E6 RID: 78054 RVA: 0x005A6A2C File Offset: 0x005A4C2C
		public static bool TryGetNearbyUnseenCell(Pawn pawn, out IntVec3 pos)
		{
			if (!pawn.SpawnedOrAnyParentSpawned)
			{
				pos = IntVec3.Invalid;
				return false;
			}
			Map mapHeld = pawn.MapHeld;
			CellRect view = Find.CameraDriver.CurrentViewRect.ExpandedBy(1);
			IntVec3 positionHeld = pawn.PositionHeld;
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(positionHeld, 4.9f, false))
			{
				if (AnomalyUtility.IsValidUnseenCell(view, intVec, mapHeld))
				{
					AnomalyUtility.cells.Add(intVec);
				}
			}
			if (AnomalyUtility.cells.Empty<IntVec3>())
			{
				foreach (IntVec3 intVec2 in GenRadial.RadialCellsAround(positionHeld, 9.9f, false))
				{
					if (AnomalyUtility.IsValidUnseenCell(view, intVec2, mapHeld))
					{
						AnomalyUtility.cells.Add(intVec2);
					}
				}
			}
			bool flag = AnomalyUtility.cells.Any<IntVec3>();
			pos = (flag ? AnomalyUtility.cells.RandomElement<IntVec3>() : IntVec3.Invalid);
			AnomalyUtility.cells.Clear();
			return flag;
		}

		// Token: 0x060130E7 RID: 78055 RVA: 0x005A6B64 File Offset: 0x005A4D64
		public static bool IsValidUnseenCell(CellRect view, IntVec3 cell, Map map)
		{
			return (map != Find.CurrentMap || !view.Contains(cell)) && !cell.Fogged(map) && cell.Walkable(map) && cell.GetFence(map) == null && cell.GetDoor(map) == null;
		}

		// Token: 0x060130E8 RID: 78056 RVA: 0x005A6BA0 File Offset: 0x005A4DA0
		public static bool TryDuplicatePawn(Pawn originalPawn, IntVec3 targetCell, Map map, out Pawn duplicatePawn, Faction faction = null, bool allowCreepjoiners = false, bool randomOutcome = false, bool negativeOutcomes = true)
		{
			if (!allowCreepjoiners && originalPawn.IsCreepJoiner)
			{
				duplicatePawn = null;
				return false;
			}
			duplicatePawn = Find.PawnDuplicator.Duplicate(originalPawn);
			if (faction != null)
			{
				duplicatePawn.SetFaction(faction, null);
			}
			map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.Skip_EntryNoDelay.Spawn(targetCell, map, 1f), targetCell, 60);
			SoundDefOf.Psycast_Skip_Entry.PlayOneShot(new TargetInfo(targetCell, map, false));
			if (randomOutcome && negativeOutcomes)
			{
				int num = Rand.RangeInclusive(0, 3);
				if (num == 1 && (duplicatePawn.health.hediffSet.HasHediffOrWillBecome(HediffDefOf.OrganDecay, false) || duplicatePawn.health.hediffSet.HasHediffOrWillBecome(HediffDefOf.OrganDecayCreepjoiner, false)))
				{
					num++;
				}
				if (num == 2 && duplicatePawn.health.hediffSet.HasHediffOrWillBecome(HediffDefOf.CrumblingMind, false))
				{
					num++;
				}
				switch (num)
				{
				case 0:
					AnomalyUtility.AddDuplicateSickness(originalPawn, duplicatePawn);
					break;
				case 1:
					duplicatePawn.health.AddHediff(HediffDefOf.OrganDecayUndiagnosedDuplicaton, null, null, null);
					break;
				case 2:
					duplicatePawn.health.AddHediff(HediffDefOf.CrumblingMindUndiagnosedDuplication, null, null, null);
					break;
				case 3:
					duplicatePawn.SetFaction(Faction.OfEntities, null);
					Find.LetterStack.ReceiveLetter("ObeliskHostileDuplicateLetterLabel".Translate(), "ObeliskHostileDuplicateLetter".Translate(originalPawn.Named("PAWN")), LetterDefOf.ThreatBig, duplicatePawn, null, null, null, null, 0, true);
					break;
				default:
					Log.Error("Unhandled outcome in pawn duplication " + num.ToString());
					break;
				}
			}
			else if (negativeOutcomes)
			{
				AnomalyUtility.AddDuplicateSickness(originalPawn, duplicatePawn);
			}
			Find.PawnDuplicator.AddDuplicate(originalPawn.duplicate.duplicateOf, originalPawn);
			Find.PawnDuplicator.AddDuplicate(duplicatePawn.duplicate.duplicateOf, duplicatePawn);
			GenSpawn.Spawn(duplicatePawn, targetCell, map, WipeMode.Vanish);
			return duplicatePawn != null;
		}

		// Token: 0x060130E9 RID: 78057 RVA: 0x005A6D94 File Offset: 0x005A4F94
		private static void AddDuplicateSickness(Pawn originalPawn, Pawn duplicatePawn)
		{
			Hediff_DuplicateSickness hediff_DuplicateSickness = (Hediff_DuplicateSickness)originalPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DuplicateSickness, false);
			if (hediff_DuplicateSickness == null)
			{
				hediff_DuplicateSickness = (Hediff_DuplicateSickness)HediffMaker.MakeHediff(HediffDefOf.DuplicateSickness, originalPawn, null);
				originalPawn.health.AddHediff(hediff_DuplicateSickness, null, null, null);
			}
			else
			{
				hediff_DuplicateSickness.GetComp<HediffComp_SeverityPerDay>().severityPerDay = 0.1f;
			}
			Hediff_DuplicateSickness hediff = (Hediff_DuplicateSickness)HediffMaker.MakeHediff(HediffDefOf.DuplicateSickness, duplicatePawn, null);
			duplicatePawn.health.AddHediff(hediff, null, null, null);
		}

		// Token: 0x060130EA RID: 78058 RVA: 0x005A6E24 File Offset: 0x005A5024
		public static bool Inhumanized(this Pawn pawn)
		{
			if (ModsConfig.AnomalyActive)
			{
				Pawn_HealthTracker health = pawn.health;
				if (((health != null) ? health.hediffSet : null) != null)
				{
					return pawn.health.hediffSet.HasHediff(HediffDefOf.Inhumanized, false);
				}
			}
			return false;
		}

		// Token: 0x060130EB RID: 78059 RVA: 0x005A6E5C File Offset: 0x005A505C
		public static void Rehumanize(this Pawn pawn)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Inhumanized, false);
			if (firstHediffOfDef == null)
			{
				Log.Error("Tried to re-humanized a pawn that was not inhumanized.");
				return;
			}
			pawn.health.RemoveHediff(firstHediffOfDef);
		}

		// Token: 0x060130EC RID: 78060 RVA: 0x005A6EA4 File Offset: 0x005A50A4
		public static string GetBiosignatureName(int biosignature)
		{
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(RulePackDefOf.NamerBiosignature);
			Rand.PushState(biosignature);
			string result = GrammarResolver.Resolve("r_root", request, null, false, null, null, null, true);
			Rand.PopState();
			return result;
		}

		// Token: 0x060130ED RID: 78061 RVA: 0x005A6EE8 File Offset: 0x005A50E8
		public static Pawn FindEntityOnPlatform(Map map, EntityQueryType queryType)
		{
			AnomalyUtility.tmpEntities.Clear();
			foreach (Building_HoldingPlatform building_HoldingPlatform in map.listerBuildings.AllBuildingsColonistOfClass<Building_HoldingPlatform>())
			{
				Pawn heldPawn = building_HoldingPlatform.HeldPawn;
				CompHoldingPlatformTarget compHoldingPlatformTarget = (heldPawn != null) ? heldPawn.TryGetComp<CompHoldingPlatformTarget>() : null;
				if (compHoldingPlatformTarget != null)
				{
					switch (queryType)
					{
					case EntityQueryType.Any:
						AnomalyUtility.tmpEntities.Add(building_HoldingPlatform.HeldPawn);
						break;
					case EntityQueryType.ForSlaughter:
						if (compHoldingPlatformTarget.Props.canBeExecuted)
						{
							AnomalyUtility.tmpEntities.Add(building_HoldingPlatform.HeldPawn);
						}
						break;
					case EntityQueryType.ForRelease:
						if (ContainmentUtility.InitiateEscapeMtbDays(building_HoldingPlatform.HeldPawn, null) > 0f)
						{
							AnomalyUtility.tmpEntities.Add(building_HoldingPlatform.HeldPawn);
						}
						break;
					}
				}
			}
			Pawn result;
			if (AnomalyUtility.tmpEntities.TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x060130EE RID: 78062 RVA: 0x005A6FD4 File Offset: 0x005A51D4
		public static bool ShouldNotifyCodex(Thing thing, EntityDiscoveryType discoveryType, out List<EntityCodexEntryDef> entries)
		{
			entries = null;
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (thing.Spawned && thing.Position.Fogged(thing.Map))
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null && pawn.IsHiddenFromPlayer())
			{
				return false;
			}
			entries = AnomalyUtility.GetCodexEntriesFor(thing);
			if (entries.NullOrEmpty<EntityCodexEntryDef>())
			{
				return false;
			}
			if (discoveryType != EntityDiscoveryType.Unfog)
			{
				if (discoveryType == EntityDiscoveryType.Spawn && MapGenerator.mapBeingGenerated == thing.MapHeld)
				{
					foreach (EntityCodexEntryDef entityCodexEntryDef in entries)
					{
						if (entityCodexEntryDef.startDiscovered)
						{
							return true;
						}
						if (entityCodexEntryDef.discoveryType == discoveryType && entityCodexEntryDef.allowDiscoveryWhileMapGenerating)
						{
							return true;
						}
					}
					return false;
				}
				using (List<EntityCodexEntryDef>.Enumerator enumerator = entries.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.discoveryType == discoveryType)
						{
							return true;
						}
					}
				}
				return false;
			}
			return true;
		}

		// Token: 0x060130EF RID: 78063 RVA: 0x005A70F0 File Offset: 0x005A52F0
		public static Gizmo OpenCodexGizmo(Thing thing)
		{
			if (!ModLister.CheckAnomaly("Codex gizmo"))
			{
				return null;
			}
			if ((Find.Anomaly.HighestLevelReached <= 0 && Find.Storyteller.difficulty.AnomalyPlaystyleDef.generateMonolith) || Find.Selector.SelectedObjects.Count != 1)
			{
				return null;
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null && pawn.Drafted)
			{
				return null;
			}
			List<EntityCodexEntryDef> entries = AnomalyUtility.GetCodexEntriesFor(thing);
			if (entries.NullOrEmpty<EntityCodexEntryDef>())
			{
				return null;
			}
			return new Command_Action
			{
				defaultLabel = "EntityCodex".Translate() + "...",
				defaultDesc = "EntityCodexGizmoTip".Translate(),
				icon = AnomalyUtility.OpenCodexGizmoIcon.Texture,
				action = delegate()
				{
					Find.WindowStack.Add(new Dialog_EntityCodex(entries.FirstOrDefault<EntityCodexEntryDef>()));
				}
			};
		}

		// Token: 0x060130F0 RID: 78064 RVA: 0x005A71D4 File Offset: 0x005A53D4
		public static List<EntityCodexEntryDef> GetCodexEntriesFor(Thing thing)
		{
			AnomalyUtility.tmpCodexEntries.Clear();
			if (thing.def.entityCodexEntry != null)
			{
				AnomalyUtility.tmpCodexEntries.Add(thing.def.entityCodexEntry);
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null && pawn.IsMutant && pawn.mutant.HasTurned && pawn.mutant.Def.codexEntry != null)
			{
				AnomalyUtility.tmpCodexEntries.Add(pawn.mutant.Def.codexEntry);
			}
			if (thing is UnnaturalCorpse)
			{
				AnomalyUtility.tmpCodexEntries.Add(EntityCodexEntryDefOf.UnnaturalCorpse);
			}
			return AnomalyUtility.tmpCodexEntries;
		}

		// Token: 0x0400C599 RID: 50585
		private static readonly FloatRange UnnaturalCorpseAgeRangeDays = new FloatRange(3f, 15f);

		// Token: 0x0400C59A RID: 50586
		private const float DuplicateSicknessSeverityPerDay = 0.1f;

		// Token: 0x0400C59B RID: 50587
		private static readonly CachedTexture OpenCodexGizmoIcon = new CachedTexture("UI/Icons/OpenCodex");

		// Token: 0x0400C59C RID: 50588
		private static readonly List<IntVec3> cells = new List<IntVec3>();

		// Token: 0x0400C59D RID: 50589
		private static List<Pawn> tmpEntities = new List<Pawn>();

		// Token: 0x0400C59E RID: 50590
		private static List<EntityCodexEntryDef> tmpCodexEntries = new List<EntityCodexEntryDef>();
	}
}
