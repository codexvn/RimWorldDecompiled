using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002CBC RID: 11452
	public class CompGoldenCube : CompInteractable, IRoofCollapseAlert
	{
		// Token: 0x170028A8 RID: 10408
		// (get) Token: 0x06010123 RID: 65827 RVA: 0x004A6724 File Offset: 0x004A4924
		public new CompProperties_GoldenCube Props
		{
			get
			{
				return (CompProperties_GoldenCube)this.props;
			}
		}

		// Token: 0x170028A9 RID: 10409
		// (get) Token: 0x06010124 RID: 65828 RVA: 0x004A6734 File Offset: 0x004A4934
		public CompStudyUnlocks StudyUnlocks
		{
			get
			{
				CompStudyUnlocks result;
				if ((result = this.studyUnlocksInt) == null)
				{
					result = (this.studyUnlocksInt = this.parent.GetComp<CompStudyUnlocks>());
				}
				return result;
			}
		}

		// Token: 0x170028AA RID: 10410
		// (get) Token: 0x06010125 RID: 65829 RVA: 0x004A675F File Offset: 0x004A495F
		public bool Deactivatable
		{
			get
			{
				return this.StudyUnlocks.Completed;
			}
		}

		// Token: 0x06010126 RID: 65830 RVA: 0x004A676C File Offset: 0x004A496C
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			if (base.ValidateTarget(target, false))
			{
				this.OrderDeactivation(target.Pawn);
			}
		}

		// Token: 0x06010127 RID: 65831 RVA: 0x004A6788 File Offset: 0x004A4988
		public override void CompTickInterval(int delta)
		{
			base.CompTickInterval(delta);
			if (!this.parent.IsHashIntervalTick(2500, delta))
			{
				return;
			}
			Pawn pawn;
			if ((!this.AnyPawnHasHediff() || Rand.MTBEventOccurs(12f, 60000f, 2500f)) && QuestUtility.TryGetIdealColonist(out pawn, this.parent.MapHeld, new Func<Pawn, bool>(this.ValidatePawn)))
			{
				this.GiveHediff(pawn);
			}
		}

		// Token: 0x06010128 RID: 65832 RVA: 0x004A67F8 File Offset: 0x004A49F8
		private void GiveHediff(Pawn pawn)
		{
			pawn.health.AddHediff(HediffDefOf.CubeInterest, null, null, null);
			Messages.Message("MessageGoldenCubeInterest".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x06010129 RID: 65833 RVA: 0x004A684C File Offset: 0x004A4A4C
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (!this.Deactivatable)
			{
				yield break;
			}
			AcceptanceReport acceptanceReport = this.CanInteract(selPawn, true);
			FloatMenuOption floatMenuOption = new FloatMenuOption(this.Props.jobString.CapitalizeFirst(), delegate()
			{
				this.OrderDeactivation(selPawn);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			if (!acceptanceReport.Accepted)
			{
				floatMenuOption.Disabled = true;
				floatMenuOption.Label = floatMenuOption.Label + " (" + acceptanceReport.Reason + ")";
			}
			yield return floatMenuOption;
			yield break;
		}

		// Token: 0x0601012A RID: 65834 RVA: 0x004A6863 File Offset: 0x004A4A63
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!this.Deactivatable)
			{
				yield break;
			}
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0601012B RID: 65835 RVA: 0x00035ADE File Offset: 0x00033CDE
		public override string CompInspectStringExtra()
		{
			return "";
		}

		// Token: 0x0601012C RID: 65836 RVA: 0x004A6874 File Offset: 0x004A4A74
		protected override void OnInteracted(Pawn caster)
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			foreach (Pawn pawn in Find.WorldPawns.AllPawnsAlive)
			{
				if (pawn.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false) && pawn.IsCaravanMember() && pawn.Faction == Faction.OfPlayer)
				{
					PawnBanishUtility.Banish(pawn, false);
					list2.Add(pawn.LabelShort);
				}
				this.CurePawn(pawn);
			}
			foreach (Map map in Find.Maps)
			{
				for (int i = map.mapPawns.AllPawns.Count - 1; i >= 0; i--)
				{
					Pawn pawn2 = map.mapPawns.AllPawns[i];
					if (pawn2.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false))
					{
						PawnUtility.ForceEjectFromContainer(pawn2);
						pawn2.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, true, false, false, null, true, false, false);
						list.Add(pawn2.LabelShort);
					}
					this.CurePawn(pawn2);
				}
			}
			if (list.Any<string>() || list2.Any<string>())
			{
				TaggedString taggedString = "CubeDeactivationBerserkDesc".Translate(caster.Named("PAWN"));
				if (list.Any<string>())
				{
					taggedString += "\n\n" + "CubeDeactivationBerserkPawns".Translate() + ":\n" + list.ToLineList("- ");
				}
				if (list2.Any<string>())
				{
					taggedString += "\n\n" + "CubeDeactivationLostPawns".Translate() + ":\n" + list2.ToLineList("- ");
				}
				Find.LetterStack.ReceiveLetter("CubeDeactivationBerserk".Translate(), taggedString, list.Any<string>() ? LetterDefOf.ThreatBig : LetterDefOf.NegativeEvent, null, 0, true);
			}
			this.parent.Kill(null, null);
		}

		// Token: 0x0601012D RID: 65837 RVA: 0x004A6AD0 File Offset: 0x004A4CD0
		private bool AnyPawnHasHediff()
		{
			if (this.parent.SpawnedOrAnyParentSpawned)
			{
				foreach (Pawn pawn in this.parent.MapHeld.mapPawns.AllPawns)
				{
					if (pawn.IsColonist && pawn.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false))
					{
						return true;
					}
				}
			}
			foreach (Map map in Find.Maps)
			{
				if (map != this.parent.MapHeld)
				{
					foreach (Pawn pawn2 in map.mapPawns.AllPawns)
					{
						Hediff hediff;
						if (pawn2.IsColonist && pawn2.health.hediffSet.TryGetHediff(HediffDefOf.CubeInterest, out hediff))
						{
							return true;
						}
					}
				}
			}
			foreach (Pawn pawn3 in Find.WorldPawns.AllPawnsAlive)
			{
				Hediff hediff;
				if (pawn3.IsColonist && pawn3.health.hediffSet.TryGetHediff(HediffDefOf.CubeInterest, out hediff))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0601012E RID: 65838 RVA: 0x004A6C84 File Offset: 0x004A4E84
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Gold, null);
			thing.stackCount = CompGoldenCube.GoldRange.RandomInRange;
			GenPlace.TryPlaceThing(thing, this.parent.PositionHeld, prevMap, ThingPlaceMode.Near, null, null, null, 1);
		}

		// Token: 0x0601012F RID: 65839 RVA: 0x004A6CD0 File Offset: 0x004A4ED0
		private void CurePawn(Pawn pawn)
		{
			Hediff hediff;
			if (pawn.health.hediffSet.TryGetHediff(HediffDefOf.CubeInterest, out hediff))
			{
				pawn.health.RemoveHediff(hediff);
			}
			Hediff hediff2;
			if (pawn.health.hediffSet.TryGetHediff(HediffDefOf.CubeWithdrawal, out hediff2))
			{
				pawn.health.RemoveHediff(hediff2);
			}
			Hediff hediff3;
			if (pawn.health.hediffSet.TryGetHediff(HediffDefOf.CubeComa, out hediff3))
			{
				pawn.health.RemoveHediff(hediff3);
			}
		}

		// Token: 0x06010130 RID: 65840 RVA: 0x004A6D4C File Offset: 0x004A4F4C
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			AcceptanceReport result = base.CanInteract(activateBy, checkOptionalItems);
			if (!result.Accepted)
			{
				return result;
			}
			if (activateBy != null)
			{
				if (activateBy.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false))
				{
					return "CannotDisableCube".Translate(activateBy.Named("PAWN"));
				}
				if (checkOptionalItems && !activateBy.HasReserved(ThingDefOf.Shard) && !activateBy.CanReserveAndReachableOfDef(ThingDefOf.Shard, PathEndMode.Touch, Danger.None, 1, -1, null, false))
				{
					return "NoItemReservedOrReachable".Translate(ThingDefOf.Shard.label);
				}
			}
			else if (checkOptionalItems && !ReservationUtility.ExistsUnreservedAmountOfDef(this.parent.MapHeld, ThingDefOf.Shard, Faction.OfPlayer, 1, null))
			{
				return "NoItemReserved".Translate(ThingDefOf.Shard.label);
			}
			return true;
		}

		// Token: 0x06010131 RID: 65841 RVA: 0x004A6E2C File Offset: 0x004A502C
		private void OrderDeactivation(Pawn pawn)
		{
			TaggedString taggedString = "CubeDeactivationConfirmation".Translate();
			List<Pawn> list = Find.Maps.SelectMany((Map m) => from p in m.mapPawns.AllPawns
			where p.Faction == Faction.OfPlayer && p.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false)
			select p).ToList<Pawn>();
			List<Pawn> list2 = Find.WorldObjects.Caravans.SelectMany((Caravan c) => from p in c.pawns.InnerListForReading
			where p.Faction == Faction.OfPlayer && p.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false)
			select p).ToList<Pawn>();
			if (list.Any<Pawn>())
			{
				taggedString += "\n\n" + "CubeDeactivationConfirmationPawnsBerserk".Translate() + ":\n" + (from p in list
				select p.LabelShort).ToLineList("- ", false);
			}
			if (list2.Any<Pawn>())
			{
				taggedString += "\n\n" + "CubeDeactivationConfirmationPawnsLost".Translate() + ":\n" + (from p in list2
				select p.LabelShort).ToLineList("- ", false);
			}
			taggedString += "\n\n" + "CubeDeactivationConfirmationEnd".Translate();
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(taggedString, delegate()
			{
				Thing t;
				if (!pawn.TryFindReserveAndReachableOfDef(ThingDefOf.Shard, out t, PathEndMode.Touch, Danger.None, 1, -1, null, false))
				{
					return;
				}
				Job job = JobMaker.MakeJob(JobDefOf.InteractThing, this.parent, t);
				job.count = 1;
				job.playerForced = true;
				pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, false, null, WindowLayer.Dialog));
		}

		// Token: 0x06010132 RID: 65842 RVA: 0x004A6FBC File Offset: 0x004A51BC
		private bool ValidatePawn(Pawn pawn)
		{
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.CubeInterest, false) || pawn.health.hediffSet.HasHediff(HediffDefOf.CubeComa, false))
			{
				return false;
			}
			if (pawn.MapHeld != this.parent.MapHeld)
			{
				return false;
			}
			if (this.parent.IsInCaravan())
			{
				Caravan caravan = pawn.GetCaravan();
				bool? flag;
				if (caravan == null)
				{
					flag = null;
				}
				else
				{
					IEnumerable<Thing> allThings = caravan.AllThings;
					flag = ((allThings != null) ? new bool?(allThings.Contains(this.parent)) : null);
				}
				bool? flag2 = flag;
				return flag2.GetValueOrDefault();
			}
			return true;
		}

		// Token: 0x06010133 RID: 65843 RVA: 0x004A7060 File Offset: 0x004A5260
		public RoofCollapseResponse Notify_OnBeforeRoofCollapse()
		{
			IntVec3 cell;
			if (RCellFinder.TryFindRandomCellNearWith(this.parent.Position, (IntVec3 c) => CompGoldenCube.IsValidCell(c, this.parent.MapHeld), this.parent.MapHeld, out cell, 10, 2147483647))
			{
				SkipUtility.SkipTo(this.parent, cell, this.parent.MapHeld);
			}
			return RoofCollapseResponse.RemoveThing;
		}

		// Token: 0x06010134 RID: 65844 RVA: 0x004A433E File Offset: 0x004A253E
		private static bool IsValidCell(IntVec3 cell, Map map)
		{
			return cell.InBounds(map) && cell.Walkable(map);
		}

		// Token: 0x0400A7EF RID: 42991
		private CompStudyUnlocks studyUnlocksInt;

		// Token: 0x0400A7F0 RID: 42992
		private const float EnthrallMTBDays = 12f;

		// Token: 0x0400A7F1 RID: 42993
		private static readonly IntRange GoldRange = new IntRange(125, 175);
	}
}
