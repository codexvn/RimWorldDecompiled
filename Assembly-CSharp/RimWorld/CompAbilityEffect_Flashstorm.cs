using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002230 RID: 8752
	public class CompAbilityEffect_Flashstorm : CompAbilityEffect
	{
		// Token: 0x0600BF47 RID: 48967 RVA: 0x003777D8 File Offset: 0x003759D8
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Map map = this.parent.pawn.Map;
			Thing conditionCauser = GenSpawn.Spawn(ThingDefOf.Flashstorm, target.Cell, this.parent.pawn.Map, WipeMode.Vanish);
			GameCondition_Flashstorm gameCondition_Flashstorm = (GameCondition_Flashstorm)GameConditionMaker.MakeCondition(GameConditionDefOf.Flashstorm, -1);
			gameCondition_Flashstorm.centerLocation = target.Cell.ToIntVec2;
			gameCondition_Flashstorm.areaRadiusOverride = new IntRange(Mathf.RoundToInt(this.parent.def.EffectRadius), Mathf.RoundToInt(this.parent.def.EffectRadius));
			gameCondition_Flashstorm.Duration = Mathf.RoundToInt((float)this.parent.def.EffectDuration(this.parent.pawn).SecondsToTicks());
			gameCondition_Flashstorm.suppressEndMessage = true;
			gameCondition_Flashstorm.initialStrikeDelay = new IntRange(60, 180);
			gameCondition_Flashstorm.conditionCauser = conditionCauser;
			gameCondition_Flashstorm.ambientSound = true;
			map.gameConditionManager.RegisterCondition(gameCondition_Flashstorm);
			this.ApplyGoodwillImpact(target, gameCondition_Flashstorm.AreaRadius);
		}

		// Token: 0x0600BF48 RID: 48968 RVA: 0x003778E8 File Offset: 0x00375AE8
		private void ApplyGoodwillImpact(LocalTargetInfo target, int radius)
		{
			if (this.parent.pawn.Faction != Faction.OfPlayer)
			{
				return;
			}
			this.affectedFactionCache.Clear();
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(target.Cell, this.parent.pawn.Map, (float)radius, true))
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && thing.Faction != null && thing.Faction != this.parent.pawn.Faction && !thing.Faction.HostileTo(this.parent.pawn.Faction) && !this.affectedFactionCache.Contains(thing.Faction) && (base.Props.applyGoodwillImpactToLodgers || !pawn.IsQuestLodger()))
				{
					this.affectedFactionCache.Add(thing.Faction);
					Faction.OfPlayer.TryAffectGoodwillWith(thing.Faction, base.Props.goodwillImpact, true, true, HistoryEventDefOf.UsedHarmfulAbility, null);
				}
			}
			this.affectedFactionCache.Clear();
		}

		// Token: 0x0600BF49 RID: 48969 RVA: 0x00377A34 File Offset: 0x00375C34
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (target.Cell.Roofed(this.parent.pawn.Map))
			{
				if (throwMessages)
				{
					Messages.Message("CannotUseAbility".Translate(this.parent.def.label) + ": " + "AbilityRoofed".Translate(), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x04008347 RID: 33607
		private HashSet<Faction> affectedFactionCache = new HashSet<Faction>();
	}
}
