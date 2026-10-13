using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020023E9 RID: 9193
	public class RoyalTitlePermitWorker_OrbitalStrike : RoyalTitlePermitWorker_Targeted
	{
		// Token: 0x0600C750 RID: 51024 RVA: 0x003A168C File Offset: 0x0039F88C
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
			return true;
		}

		// Token: 0x0600C751 RID: 51025 RVA: 0x003A16E4 File Offset: 0x0039F8E4
		public override void DrawHighlight(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(this.caller.Position, base.RangeClamped, Color.white, null);
			GenDraw.DrawRadiusRing(target.Cell, this.def.royalAid.radius + this.def.royalAid.explosionRadiusRange.max, Color.white, null);
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x0600C752 RID: 51026 RVA: 0x003A1754 File Offset: 0x0039F954
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			this.CallBombardment(target.Cell);
		}

		// Token: 0x0600C753 RID: 51027 RVA: 0x003A1763 File Offset: 0x0039F963
		public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			string t;
			if (this.AidDisabled_NewTemp(map, pawn, faction, out t, false))
			{
				yield return new FloatMenuOption(this.def.LabelCap + ": " + t, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			if (faction.HostileTo(Faction.OfPlayer))
			{
				yield return new FloatMenuOption(this.def.LabelCap + ": " + "CommandCallRoyalAidFactionHostile".Translate(faction.Named("FACTION")), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			string label = this.def.LabelCap + ": ";
			Action action = null;
			bool free;
			if (base.FillAidOption(pawn, faction, ref label, out free))
			{
				action = delegate()
				{
					this.BeginCallBombardment(pawn, faction, map, free);
				};
			}
			yield return new FloatMenuOption(label, action, faction.def.FactionIcon, faction.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false);
			yield break;
		}

		// Token: 0x0600C754 RID: 51028 RVA: 0x003A1788 File Offset: 0x0039F988
		private void BeginCallBombardment(Pawn caller, Faction faction, Map map, bool free)
		{
			this.targetingParameters = new TargetingParameters();
			this.targetingParameters.canTargetLocations = true;
			this.targetingParameters.canTargetSelf = true;
			this.targetingParameters.canTargetFires = true;
			this.targetingParameters.canTargetItems = true;
			this.caller = caller;
			this.map = map;
			this.faction = faction;
			this.free = free;
			float rangeActual = base.RangeClamped;
			this.targetingParameters.validator = delegate(TargetInfo target)
			{
				if (rangeActual > 0f && target.Cell.DistanceTo(caller.Position) > rangeActual)
				{
					return false;
				}
				if (target.Cell.Fogged(map))
				{
					return false;
				}
				RoofDef roof = target.Cell.GetRoof(map);
				return roof == null || !roof.isThickRoof;
			};
			Find.Targeter.BeginTargeting(this, null, false, null, null, true);
		}

		// Token: 0x0600C755 RID: 51029 RVA: 0x003A1840 File Offset: 0x0039FA40
		private void CallBombardment(IntVec3 targetCell)
		{
			Bombardment bombardment = (Bombardment)GenSpawn.Spawn(ThingDefOf.Bombardment, targetCell, this.map, WipeMode.Vanish);
			bombardment.impactAreaRadius = this.def.royalAid.radius;
			bombardment.explosionRadiusRange = this.def.royalAid.explosionRadiusRange;
			bombardment.bombIntervalTicks = this.def.royalAid.intervalTicks;
			bombardment.randomFireRadius = 1;
			bombardment.explosionCount = this.def.royalAid.explosionCount;
			bombardment.warmupTicks = this.def.royalAid.warmupTicks;
			bombardment.instigator = this.caller;
			SoundDefOf.OrbitalStrike_Ordered.PlayOneShotOnCamera(null);
			this.caller.royalty.GetPermit(this.def, this.faction).Notify_Used();
			if (!this.free)
			{
				this.caller.royalty.TryRemoveFavor(this.faction, this.def.royalAid.favorCost);
			}
		}

		// Token: 0x040087E9 RID: 34793
		private Faction faction;
	}
}
