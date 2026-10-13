using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200221E RID: 8734
	public class CompAbilityEffect_Farskip : CompAbilityEffect
	{
		// Token: 0x17001D24 RID: 7460
		// (get) Token: 0x0600BEED RID: 48877 RVA: 0x00375E9A File Offset: 0x0037409A
		private new CompProperties_AbilityFarskip Props
		{
			get
			{
				return (CompProperties_AbilityFarskip)this.props;
			}
		}

		// Token: 0x0600BEEE RID: 48878 RVA: 0x00375EA8 File Offset: 0x003740A8
		public override void Apply(GlobalTargetInfo target)
		{
			Caravan caravan = this.parent.pawn.GetCaravan();
			MapParent mapParent = target.WorldObject as MapParent;
			Map targetMap = (mapParent != null) ? mapParent.Map : null;
			IntVec3 targetCell = IntVec3.Invalid;
			List<Pawn> list = this.PawnsToSkip().ToList<Pawn>();
			if (this.parent.pawn.Spawned)
			{
				foreach (Pawn pawn in list)
				{
					this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_Entry.Spawn(pawn, pawn.Map, 1f), pawn.Position, 60, null);
				}
				SoundDefOf.Psycast_Skip_Pulse.PlayOneShot(new TargetInfo(target.Cell, this.parent.pawn.Map, false));
			}
			if (this.ShouldEnterMap(target))
			{
				Pawn pawn2 = this.AlliedPawnOnMap(targetMap);
				if (pawn2 != null)
				{
					targetCell = pawn2.Position;
				}
				else
				{
					targetCell = this.parent.pawn.Position;
				}
			}
			if (targetCell.IsValid)
			{
				Predicate<IntVec3> <>9__0;
				foreach (Pawn pawn3 in list)
				{
					if (pawn3.Spawned)
					{
						pawn3.teleporting = true;
						pawn3.ExitMap(false, Rot4.Invalid);
						AbilityUtility.DoClamor(pawn3.Position, (float)this.Props.clamorRadius, this.parent.pawn, this.Props.clamorType);
						pawn3.teleporting = false;
					}
					IntVec3 targetCell2 = targetCell;
					Map targetMap2 = targetMap;
					int firstTryWithRadius = 4;
					Predicate<IntVec3> extraValidator;
					if ((extraValidator = <>9__0) == null)
					{
						extraValidator = (<>9__0 = ((IntVec3 cell) => cell != targetCell && cell.GetRoom(targetMap) == targetCell.GetRoom(targetMap)));
					}
					IntVec3 intVec;
					CellFinder.TryFindRandomSpawnCellForPawnNear(targetCell2, targetMap2, out intVec, firstTryWithRadius, extraValidator);
					GenSpawn.Spawn(pawn3, intVec, targetMap, WipeMode.Vanish);
					if (pawn3.drafter != null && pawn3.IsColonistPlayerControlled && !pawn3.Downed)
					{
						pawn3.drafter.Drafted = true;
					}
					pawn3.stances.stunner.StunFor(this.Props.stunTicks.RandomInRange, this.parent.pawn, false, true, false);
					pawn3.Notify_Teleported(true, true);
					CompAbilityEffect_Teleport.SendSkipUsedSignal(pawn3, this.parent.pawn);
					if (pawn3.IsPrisoner)
					{
						pawn3.guest.WaitInsteadOfEscapingForDefaultTicks();
					}
					this.parent.AddEffecterToMaintain(EffecterDefOf.Skip_ExitNoDelay.Spawn(pawn3, pawn3.Map, 1f), pawn3.Position, 60, targetMap);
					SoundDefOf.Psycast_Skip_Exit.PlayOneShot(new TargetInfo(intVec, pawn3.Map, false));
					if ((pawn3.IsColonist || pawn3.RaceProps.packAnimal || pawn3.IsColonyMech) && pawn3.Map.IsPlayerHome)
					{
						pawn3.inventory.UnloadEverything = true;
					}
				}
				if (caravan != null)
				{
					caravan.Destroy();
					return;
				}
			}
			else
			{
				Caravan caravan2 = target.WorldObject as Caravan;
				if (caravan2 != null && caravan2.Faction == this.parent.pawn.Faction)
				{
					if (caravan != null)
					{
						caravan.pawns.TryTransferAllToContainer(caravan2.pawns, true);
						caravan2.Notify_Merged(new List<Caravan>
						{
							caravan
						});
						caravan.Destroy();
						return;
					}
					using (List<Pawn>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Pawn pawn4 = enumerator.Current;
							caravan2.AddPawn(pawn4, true);
							pawn4.ExitMap(false, Rot4.Invalid);
							AbilityUtility.DoClamor(pawn4.Position, (float)this.Props.clamorRadius, this.parent.pawn, this.Props.clamorType);
						}
						return;
					}
				}
				if (caravan != null)
				{
					caravan.Tile = target.Tile;
					caravan.pather.StopDead();
					return;
				}
				CaravanMaker.MakeCaravan(list, this.parent.pawn.Faction, target.Tile, false);
				foreach (Pawn pawn5 in list)
				{
					pawn5.ExitMap(false, Rot4.Invalid);
				}
			}
		}

		// Token: 0x0600BEEF RID: 48879 RVA: 0x00376388 File Offset: 0x00374588
		public override IEnumerable<PreCastAction> GetPreCastActions()
		{
			yield return new PreCastAction
			{
				action = delegate(LocalTargetInfo t, LocalTargetInfo d)
				{
					foreach (Pawn pawn in this.PawnsToSkip())
					{
						FleckCreationData dataAttachedOverlay = FleckMaker.GetDataAttachedOverlay(pawn, FleckDefOf.PsycastSkipFlashEntry, new Vector3(-0.5f, 0f, -0.5f), 1f, -1f);
						dataAttachedOverlay.link.detachAfterTicks = 5;
						pawn.Map.flecks.CreateFleck(dataAttachedOverlay);
					}
				},
				ticksAwayFromCast = 5
			};
			yield break;
		}

		// Token: 0x0600BEF0 RID: 48880 RVA: 0x00376398 File Offset: 0x00374598
		private IEnumerable<Pawn> PawnsToSkip()
		{
			Caravan caravan = this.parent.pawn.GetCaravan();
			if (caravan != null)
			{
				foreach (Pawn pawn in caravan.pawns)
				{
					yield return pawn;
				}
				List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
			}
			else
			{
				bool homeMap = this.parent.pawn.Map.IsPlayerHome;
				bool sentLodgerMessage = false;
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(this.parent.pawn.Position, this.parent.pawn.Map, this.parent.def.EffectRadius, true))
				{
					Pawn pawn2 = thing as Pawn;
					if (pawn2 != null)
					{
						if (pawn2.IsQuestLodger())
						{
							if (!sentLodgerMessage)
							{
								Messages.Message("MessageLodgersCantFarskip".Translate(), pawn2, MessageTypeDefOf.NegativeEvent, false);
							}
							sentLodgerMessage = true;
						}
						else if (!pawn2.Dead && (pawn2.IsColonist || pawn2.IsPrisonerOfColony || (!homeMap && pawn2.IsAnimal && pawn2.Faction != null && pawn2.Faction.IsPlayer) || pawn2.IsColonySubhuman))
						{
							yield return pawn2;
						}
					}
				}
				IEnumerator<Thing> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600BEF1 RID: 48881 RVA: 0x003763A8 File Offset: 0x003745A8
		private Pawn AlliedPawnOnMap(Map targetMap)
		{
			return targetMap.mapPawns.AllPawnsSpawned.FirstOrDefault((Pawn p) => !p.NonHumanlikeOrWildMan() && p.IsColonist && p.HomeFaction == Faction.OfPlayer && !this.PawnsToSkip().Contains(p));
		}

		// Token: 0x0600BEF2 RID: 48882 RVA: 0x003763C8 File Offset: 0x003745C8
		private bool ShouldEnterMap(GlobalTargetInfo target)
		{
			Caravan caravan = target.WorldObject as Caravan;
			if (caravan != null && caravan.Faction == this.parent.pawn.Faction)
			{
				return false;
			}
			MapParent mapParent = target.WorldObject as MapParent;
			return mapParent != null && mapParent.HasMap && (this.AlliedPawnOnMap(mapParent.Map) != null || mapParent.Map == this.parent.pawn.Map);
		}

		// Token: 0x0600BEF3 RID: 48883 RVA: 0x00376444 File Offset: 0x00374644
		private bool ShouldJoinCaravan(GlobalTargetInfo target)
		{
			Caravan caravan = target.WorldObject as Caravan;
			return caravan != null && caravan.Faction == this.parent.pawn.Faction;
		}

		// Token: 0x0600BEF4 RID: 48884 RVA: 0x0037647C File Offset: 0x0037467C
		public override bool Valid(GlobalTargetInfo target, bool throwMessages = false)
		{
			Caravan caravan = this.parent.pawn.GetCaravan();
			if (caravan != null && caravan.ImmobilizedByMass)
			{
				return false;
			}
			Caravan caravan2 = target.WorldObject as Caravan;
			return (caravan == null || caravan != caravan2) && (this.ShouldEnterMap(target) || this.ShouldJoinCaravan(target)) && base.Valid(target, throwMessages);
		}

		// Token: 0x0600BEF5 RID: 48885 RVA: 0x003764DC File Offset: 0x003746DC
		public override bool CanApplyOn(GlobalTargetInfo target)
		{
			MapParent mapParent = target.WorldObject as MapParent;
			return (mapParent == null || mapParent.Map == null || this.AlliedPawnOnMap(mapParent.Map) != null) && base.CanApplyOn(target);
		}

		// Token: 0x0600BEF6 RID: 48886 RVA: 0x00376518 File Offset: 0x00374718
		public override Window ConfirmationDialog(GlobalTargetInfo target, Action confirmAction)
		{
			Pawn pawn = this.PawnsToSkip().FirstOrDefault((Pawn p) => p.IsQuestLodger());
			if (pawn != null)
			{
				return Dialog_MessageBox.CreateConfirmation("FarskipConfirmTeleportingLodger".Translate(pawn.Named("PAWN")), confirmAction, false, null, WindowLayer.Dialog);
			}
			return null;
		}

		// Token: 0x0600BEF7 RID: 48887 RVA: 0x00376574 File Offset: 0x00374774
		public override string WorldMapExtraLabel(GlobalTargetInfo target)
		{
			Caravan caravan = this.parent.pawn.GetCaravan();
			if (caravan != null && caravan.ImmobilizedByMass)
			{
				return "CaravanImmobilizedByMass".Translate();
			}
			if (!this.Valid(target, false))
			{
				return "AbilityNeedAllyToSkip".Translate();
			}
			if (this.ShouldJoinCaravan(target))
			{
				return "AbilitySkipToJoinCaravan".Translate();
			}
			return "AbilitySkipToRandomAlly".Translate();
		}
	}
}
