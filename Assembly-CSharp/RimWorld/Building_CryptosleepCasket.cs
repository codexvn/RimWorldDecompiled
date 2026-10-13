using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020029EE RID: 10734
	public class Building_CryptosleepCasket : Building_Casket
	{
		// Token: 0x0600ECF6 RID: 60662 RVA: 0x0044C75F File Offset: 0x0044A95F
		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				if (allowSpecialEffects)
				{
					SoundDefOf.CryptosleepCasket_Accept.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600ECF7 RID: 60663 RVA: 0x0044C792 File Offset: 0x0044A992
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			if (myPawn.IsQuestLodger())
			{
				FloatMenuOption floatMenuOption = new FloatMenuOption("CannotUseReason".Translate("CryptosleepCasketGuestsNotAllowed".Translate()), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield return floatMenuOption;
				yield break;
			}
			foreach (FloatMenuOption floatMenuOption2 in base.GetFloatMenuOptions(myPawn))
			{
				yield return floatMenuOption2;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (this.innerContainer.Count == 0)
			{
				if (!myPawn.CanReach(this, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					FloatMenuOption floatMenuOption3 = new FloatMenuOption("CannotUseNoPath".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					yield return floatMenuOption3;
				}
				else
				{
					JobDef jobDef = JobDefOf.EnterCryptosleepCasket;
					string label = "EnterCryptosleepCasket".Translate();
					Action <>9__1;
					Action action = delegate()
					{
						if (!ModsConfig.BiotechActive)
						{
							myPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(jobDef, this), new JobTag?(JobTag.Misc), false);
							return;
						}
						Hediff_PsychicBond hediff_PsychicBond = myPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond, false) as Hediff_PsychicBond;
						if (hediff_PsychicBond == null || !ThoughtWorker_PsychicBondProximity.NearPsychicBondedPerson(myPawn, hediff_PsychicBond))
						{
							myPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(jobDef, this), new JobTag?(JobTag.Misc), false);
							return;
						}
						WindowStack windowStack = Find.WindowStack;
						TaggedString text = "PsychicBondDistanceWillBeActive_Cryptosleep".Translate(myPawn.Named("PAWN"), ((Pawn)hediff_PsychicBond.target).Named("BOND"));
						Action confirmedAct;
						if ((confirmedAct = <>9__1) == null)
						{
							confirmedAct = (<>9__1 = delegate()
							{
								myPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(jobDef, this), new JobTag?(JobTag.Misc), false);
							});
						}
						windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, true, null, WindowLayer.Dialog));
					};
					yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), myPawn, this, "ReservedBy", null);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600ECF8 RID: 60664 RVA: 0x0044C7A9 File Offset: 0x0044A9A9
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (base.Faction == Faction.OfPlayer && this.innerContainer.Count > 0 && this.def.building.isPlayerEjectable)
			{
				Command_Action command_Action = new Command_Action();
				command_Action.action = new Action(this.EjectContents);
				command_Action.defaultLabel = "CommandPodEject".Translate();
				command_Action.defaultDesc = "CommandPodEjectDesc".Translate();
				if (this.innerContainer.Count == 0)
				{
					command_Action.Disable("CommandPodEjectFailEmpty".Translate());
				}
				command_Action.hotKey = KeyBindingDefOf.Misc8;
				command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/PodEject", true);
				yield return command_Action;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600ECF9 RID: 60665 RVA: 0x0044C7BC File Offset: 0x0044A9BC
		public override void EjectContents()
		{
			ThingDef filth_Slime = ThingDefOf.Filth_Slime;
			foreach (Thing thing in ((IEnumerable<Thing>)this.innerContainer))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					PawnComponentsUtility.AddComponentsForSpawn(pawn);
					pawn.filth.GainFilth(filth_Slime);
					if (pawn.RaceProps.IsFlesh)
					{
						pawn.health.AddHediff(HediffDefOf.CryptosleepSickness, null, null, null);
					}
				}
			}
			if (!base.Destroyed)
			{
				SoundDefOf.CryptosleepCasket_Eject.PlayOneShot(SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.None));
			}
			base.EjectContents();
		}

		// Token: 0x0600ECFA RID: 60666 RVA: 0x0044C87C File Offset: 0x0044AA7C
		public static Building_CryptosleepCasket FindCryptosleepCasketFor(Pawn p, Pawn traveler, bool ignoreOtherReservations = false)
		{
			Building_CryptosleepCasket.<>c__DisplayClass5_0 CS$<>8__locals1 = new Building_CryptosleepCasket.<>c__DisplayClass5_0();
			CS$<>8__locals1.traveler = traveler;
			CS$<>8__locals1.ignoreOtherReservations = ignoreOtherReservations;
			if (Building_CryptosleepCasket.cachedCaskets == null)
			{
				Building_CryptosleepCasket.cachedCaskets = (from def in DefDatabase<ThingDef>.AllDefs
				where def.IsCryptosleepCasket
				select def).ToList<ThingDef>();
			}
			foreach (ThingDef singleDef in Building_CryptosleepCasket.cachedCaskets)
			{
				Building_CryptosleepCasket.<>c__DisplayClass5_1 CS$<>8__locals2 = new Building_CryptosleepCasket.<>c__DisplayClass5_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.queuing = KeyBindingDefOf.QueueOrder.IsDownEvent;
				Building_CryptosleepCasket building_CryptosleepCasket = (Building_CryptosleepCasket)GenClosest.ClosestThingReachable(p.PositionHeld, p.MapHeld, ThingRequest.ForDef(singleDef), PathEndMode.InteractionCell, TraverseParms.For(CS$<>8__locals2.CS$<>8__locals1.traveler, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals2.<FindCryptosleepCasketFor>g__Validator|1), null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (building_CryptosleepCasket != null)
				{
					return building_CryptosleepCasket;
				}
			}
			return null;
		}

		// Token: 0x04009CF3 RID: 40179
		private static List<ThingDef> cachedCaskets;
	}
}
