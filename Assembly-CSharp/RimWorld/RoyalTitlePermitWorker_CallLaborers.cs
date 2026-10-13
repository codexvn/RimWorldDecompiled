using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020023D8 RID: 9176
	public class RoyalTitlePermitWorker_CallLaborers : RoyalTitlePermitWorker_Targeted
	{
		// Token: 0x0600C6F4 RID: 50932 RVA: 0x0039F9B4 File Offset: 0x0039DBB4
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!base.CanHitTarget(target))
			{
				if (target.IsValid && showMessages)
				{
					Messages.Message(this.def.LabelCap + ": " + "AbilityCannotHitTarget".Translate(), MessageTypeDefOf.RejectInput, true);
				}
				return false;
			}
			AcceptanceReport acceptanceReport = RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(target, this.map, null, null);
			if (!acceptanceReport.Accepted)
			{
				Messages.Message(acceptanceReport.Reason, new LookTargets(target.Cell, this.map), MessageTypeDefOf.RejectInput, false);
			}
			return acceptanceReport.Accepted;
		}

		// Token: 0x0600C6F5 RID: 50933 RVA: 0x0039FA56 File Offset: 0x0039DC56
		public override void DrawHighlight(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(this.caller.Position, base.RangeClamped, Color.white, null);
			RoyalTitlePermitWorker_CallShuttle.DrawShuttleGhost(target, this.map, ThingDefOf.Shuttle, ThingDefOf.Shuttle.defaultPlacingRot);
		}

		// Token: 0x0600C6F6 RID: 50934 RVA: 0x0039FA8F File Offset: 0x0039DC8F
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			this.CallLaborers(target.Cell);
		}

		// Token: 0x0600C6F7 RID: 50935 RVA: 0x0039FAA0 File Offset: 0x0039DCA0
		public override void OnGUI(LocalTargetInfo target)
		{
			if (!target.IsValid || !RoyalTitlePermitWorker_CallShuttle.ShuttleCanLandHere(target, this.map, null, null).Accepted)
			{
				GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
			}
		}

		// Token: 0x0600C6F8 RID: 50936 RVA: 0x0039FAE0 File Offset: 0x0039DCE0
		public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			string t;
			if (this.AidDisabled_NewTemp(map, pawn, faction, out t, true))
			{
				yield return new FloatMenuOption(this.def.LabelCap + ": " + t, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			Action action = null;
			string label = this.def.LabelCap + " (" + "CommandCallLaborersNumLaborers".Translate(this.def.royalAid.pawnCount) + "): ";
			bool free;
			if (base.FillAidOption(pawn, faction, ref label, out free))
			{
				action = delegate()
				{
					this.BeginCallLaborers(pawn, map, faction, free);
				};
			}
			yield return new FloatMenuOption(label, action, faction.def.FactionIcon, faction.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false);
			yield break;
		}

		// Token: 0x0600C6F9 RID: 50937 RVA: 0x0039FB08 File Offset: 0x0039DD08
		private void BeginCallLaborers(Pawn pawn, Map map, Faction faction, bool free)
		{
			if (faction.HostileTo(Faction.OfPlayer))
			{
				return;
			}
			this.targetingParameters = new TargetingParameters();
			this.targetingParameters.canTargetLocations = true;
			this.targetingParameters.canTargetSelf = false;
			this.targetingParameters.canTargetPawns = false;
			this.targetingParameters.canTargetFires = false;
			this.targetingParameters.canTargetBuildings = true;
			this.targetingParameters.canTargetItems = true;
			this.caller = pawn;
			this.map = map;
			this.calledFaction = faction;
			this.free = free;
			float rangeActual = base.RangeClamped;
			this.targetingParameters.validator = ((TargetInfo target) => rangeActual <= 0f || target.Cell.DistanceTo(this.caller.Position) <= rangeActual);
			Find.Targeter.BeginTargeting(this, null, false, null, null, true);
		}

		// Token: 0x0600C6FA RID: 50938 RVA: 0x0039FBD4 File Offset: 0x0039DDD4
		private void CallLaborers(IntVec3 landingCell)
		{
			QuestScriptDef permit_CallLaborers = QuestScriptDefOf.Permit_CallLaborers;
			Slate slate = new Slate();
			slate.Set<Map>("map", this.map, false);
			slate.Set<int>("laborersCount", this.def.royalAid.pawnCount, false);
			slate.Set<Faction>("permitFaction", this.calledFaction, false);
			slate.Set<PawnKindDef>("laborersPawnKind", this.def.royalAid.pawnKindDef, false);
			slate.Set<float>("laborersDurationDays", this.def.royalAid.aidDurationDays, false);
			slate.Set<IntVec3>("landingCell", landingCell, false);
			QuestUtility.GenerateQuestAndMakeAvailable(permit_CallLaborers, slate);
			this.caller.royalty.GetPermit(this.def, this.calledFaction).Notify_Used();
			if (!this.free)
			{
				this.caller.royalty.TryRemoveFavor(this.calledFaction, this.def.royalAid.favorCost);
			}
		}

		// Token: 0x04008793 RID: 34707
		private Faction calledFaction;
	}
}
