using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002D93 RID: 11667
	public class CompOverseerSubject : ThingComp
	{
		// Token: 0x170029CD RID: 10701
		// (get) Token: 0x06010679 RID: 67193 RVA: 0x004BBCCE File Offset: 0x004B9ECE
		public CompProperties_OverseerSubject Props
		{
			get
			{
				return (CompProperties_OverseerSubject)this.props;
			}
		}

		// Token: 0x170029CE RID: 10702
		// (get) Token: 0x0601067A RID: 67194 RVA: 0x0049B545 File Offset: 0x00499745
		public Pawn Parent
		{
			get
			{
				return (Pawn)this.parent;
			}
		}

		// Token: 0x170029CF RID: 10703
		// (get) Token: 0x0601067B RID: 67195 RVA: 0x004BBCDB File Offset: 0x004B9EDB
		public OverseerSubjectState State
		{
			get
			{
				Pawn overseer = this.Overseer;
				if (((overseer != null) ? overseer.mechanitor : null) == null)
				{
					return OverseerSubjectState.RequiresOverseer;
				}
				if (this.Overseer.mechanitor.ControlledPawns.Contains(this.Parent))
				{
					return OverseerSubjectState.Overseen;
				}
				return OverseerSubjectState.RequiresBandwidth;
			}
		}

		// Token: 0x170029D0 RID: 10704
		// (get) Token: 0x0601067C RID: 67196 RVA: 0x004BBD13 File Offset: 0x004B9F13
		private Pawn Overseer
		{
			get
			{
				Pawn pawn = (Pawn)this.parent;
				if (pawn == null)
				{
					return null;
				}
				Pawn_RelationsTracker relations = pawn.relations;
				if (relations == null)
				{
					return null;
				}
				return relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Overseer, null);
			}
		}

		// Token: 0x170029D1 RID: 10705
		// (get) Token: 0x0601067D RID: 67197 RVA: 0x004BBD3C File Offset: 0x004B9F3C
		public int DelayUntilFeralCheckTicks
		{
			get
			{
				return this.delayUntilFeralCheck;
			}
		}

		// Token: 0x0601067E RID: 67198 RVA: 0x004BBD44 File Offset: 0x004B9F44
		public void Notify_DisconnectedFromOverseer()
		{
			if (this.Parent.Drafted)
			{
				this.Parent.drafter.Drafted = false;
			}
			Pawn_CarryTracker carryTracker = this.Parent.carryTracker;
			if (((carryTracker != null) ? carryTracker.CarriedThing : null) != null)
			{
				Thing thing;
				this.Parent.carryTracker.TryDropCarriedThing(this.Parent.Position, ThingPlaceMode.Near, out thing, null);
			}
			Pawn_JobTracker jobs = this.Parent.jobs;
			if (jobs != null)
			{
				jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
			Pawn_JobTracker jobs2 = this.Parent.jobs;
			if (jobs2 != null)
			{
				jobs2.CheckForJobOverride(0f, true);
			}
			this.delayUntilFeralCheck = this.Props.delayUntilFeralCheck;
		}

		// Token: 0x0601067F RID: 67199 RVA: 0x004BBDEF File Offset: 0x004B9FEF
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.delayUntilFeralCheck = this.Props.delayUntilFeralCheck;
			}
		}

		// Token: 0x06010680 RID: 67200 RVA: 0x004BBE05 File Offset: 0x004BA005
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (!ModLister.CheckBiotech("Overseer subject"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			Effecter effecter = this.effect;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.effect = null;
			base.PostDeSpawn(map, mode);
		}

		// Token: 0x06010681 RID: 67201 RVA: 0x004BBE40 File Offset: 0x004BA040
		public override void CompTick()
		{
			base.CompTick();
			if (!CompOverseerSubject.debugDisableNeedsOverseerEffect && CompOverseerSubject.CanGoFeral(this.Parent))
			{
				if (this.Props.needsOverseerEffect != null && this.effect == null)
				{
					this.effect = this.Props.needsOverseerEffect.SpawnAttached(this.parent, this.parent.Map, 1f);
				}
			}
			else
			{
				Effecter effecter = this.effect;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.effect = null;
			}
			Effecter effecter2 = this.effect;
			if (effecter2 != null)
			{
				effecter2.EffectTick(this.parent, this.parent);
			}
			if (CompOverseerSubject.CanGoFeral(this.Parent))
			{
				if (this.delayUntilFeralCheck > 0)
				{
					this.delayUntilFeralCheck--;
				}
				if (this.delayUntilFeralCheck <= 0 && Rand.MTBEventOccurs((float)this.Props.feralMtbDays, 60000f, 1f))
				{
					this.TryMakeFeral();
				}
			}
		}

		// Token: 0x06010682 RID: 67202 RVA: 0x004BBF3A File Offset: 0x004BA13A
		private static bool CanGoFeral(Pawn pawn)
		{
			return pawn.Spawned && pawn.Awake() && pawn.IsColonyMechRequiringMechanitor();
		}

		// Token: 0x06010683 RID: 67203 RVA: 0x004BBF54 File Offset: 0x004BA154
		public bool TryMakeFeral()
		{
			this.tmpFeralPawns.Clear();
			this.tmpFeralPawns.Add(this.Parent);
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(this.parent.Position, this.parent.Map, (float)this.Props.feralCascadeRadialDistance, true))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && !this.tmpFeralPawns.Contains(pawn) && CompOverseerSubject.CanGoFeral(pawn))
				{
					this.tmpFeralPawns.Add(pawn);
				}
			}
			for (int i = 0; i < this.tmpFeralPawns.Count; i++)
			{
				this.tmpFeralPawns[i].OverseerSubject.ForceFeral();
			}
			IEnumerable<IGrouping<PawnKindDef, Pawn>> source = from p in this.tmpFeralPawns
			group p by p.kindDef;
			Find.LetterStack.ReceiveLetter("LetterLabelMechsFeral".Translate(), "LetterMechsFeral".Translate(Faction.OfMechanoids, (from g in source
			select g.Key.LabelCap + " x" + g.Count<Pawn>().ToString()).ToLineList(" - ", false)), LetterDefOf.ThreatBig, this.tmpFeralPawns, null, null, null, null, 0, true);
			for (int j = 0; j < this.tmpFeralPawns.Count; j++)
			{
				Lord lord = this.tmpFeralPawns[j].GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(this.tmpFeralPawns[j], PawnLostCondition.ForcedToJoinOtherLord, null);
				}
			}
			LordMaker.MakeNewLord(Faction.OfMechanoids, new LordJob_ExitMapBest(LocomotionUrgency.Jog, false, true), this.parent.MapHeld, this.tmpFeralPawns);
			for (int k = 0; k < this.tmpFeralPawns.Count; k++)
			{
				if (this.tmpFeralPawns[k].CurJob != null)
				{
					this.tmpFeralPawns[k].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
			this.tmpFeralPawns.Clear();
			return true;
		}

		// Token: 0x06010684 RID: 67204 RVA: 0x004BC198 File Offset: 0x004BA398
		private void ForceFeral()
		{
			Pawn overseer = this.Overseer;
			if (overseer != null)
			{
				Messages.Message("MessageMechanitorLostControlOfMech".Translate(overseer, this.Parent) + ": " + this.Parent.LabelShortCap, new LookTargets(new Pawn[]
				{
					this.Parent,
					overseer
				}), MessageTypeDefOf.NeutralEvent, true);
				this.Parent.relations.RemoveDirectRelation(PawnRelationDefOf.Overseer, overseer);
			}
			this.parent.SetFaction(Faction.OfMechanoids, null);
		}

		// Token: 0x06010685 RID: 67205 RVA: 0x004BC234 File Offset: 0x004BA434
		public override string CompInspectStringExtra()
		{
			if (this.parent.Faction == Faction.OfPlayer)
			{
				StringBuilder stringBuilder = new StringBuilder();
				Pawn overseer = this.Overseer;
				TaggedString taggedString = "Overseer".Translate();
				if (((overseer != null) ? overseer.mechanitor : null) != null)
				{
					taggedString += ": " + this.Overseer.LabelShort;
					if (!overseer.mechanitor.ControlledPawns.Contains(this.parent))
					{
						taggedString += " (" + "InsufficientBandwidth".Translate() + ")";
					}
				}
				else
				{
					taggedString += ": " + "OverseerNone".Translate();
				}
				stringBuilder.Append(taggedString);
				if (this.State != OverseerSubjectState.Overseen)
				{
					TaggedString ts = (this.delayUntilFeralCheck > 0) ? ("Uncontrolled".Translate() + " (" + (this.Props.delayUntilFeralCheck - this.delayUntilFeralCheck).ToStringTicksToPeriod(true, true, true, true, false) + ")") : ("Danger".Translate() + ": " + "MayGoFeral".Translate());
					stringBuilder.AppendInNewLine(ts.Colorize(ColorLibrary.RedReadable));
				}
				return stringBuilder.ToString();
			}
			return null;
		}

		// Token: 0x06010686 RID: 67206 RVA: 0x004BC394 File Offset: 0x004BA594
		public override void Notify_AbandonedAtTile(PlanetTile tile)
		{
			base.Notify_AbandonedAtTile(tile);
			Pawn overseer = this.Overseer;
			if (overseer != null)
			{
				this.Parent.relations.RemoveDirectRelation(PawnRelationDefOf.Overseer, overseer);
			}
		}

		// Token: 0x06010687 RID: 67207 RVA: 0x004BC3C8 File Offset: 0x004BA5C8
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Make feral",
					action = new Action(this.ForceFeral)
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Make feral (event)",
					action = delegate()
					{
						this.TryMakeFeral();
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Assign to overseer",
					action = delegate()
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						List<Pawn> freeColonists = this.parent.Map.mapPawns.FreeColonists;
						for (int i = 0; i < freeColonists.Count; i++)
						{
							Pawn localPawn = freeColonists[i];
							if (MechanitorUtility.IsMechanitor(localPawn))
							{
								list.Add(new FloatMenuOption(localPawn.LabelShortCap, delegate()
								{
									foreach (Pawn pawn in from p in Find.Selector.SelectedPawns
									where p.RaceProps.IsMechanoid
									select p)
									{
										Pawn overseer = pawn.GetOverseer();
										if (overseer != null)
										{
											overseer.relations.RemoveDirectRelation(PawnRelationDefOf.Overseer, pawn);
										}
										pawn.SetFaction(Faction.OfPlayer, null);
										localPawn.relations.AddDirectRelation(PawnRelationDefOf.Overseer, pawn);
									}
								}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
						}
						if (list.Any<FloatMenuOption>())
						{
							Find.WindowStack.Add(new FloatMenu(list));
						}
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06010688 RID: 67208 RVA: 0x004BC3D8 File Offset: 0x004BA5D8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.delayUntilFeralCheck, "delayUntilFeralCheck", 0, false);
		}

		// Token: 0x0400AB2E RID: 43822
		private int delayUntilFeralCheck;

		// Token: 0x0400AB2F RID: 43823
		private Effecter effect;

		// Token: 0x0400AB30 RID: 43824
		public static bool debugDisableNeedsOverseerEffect;

		// Token: 0x0400AB31 RID: 43825
		private List<Pawn> tmpFeralPawns = new List<Pawn>();
	}
}
