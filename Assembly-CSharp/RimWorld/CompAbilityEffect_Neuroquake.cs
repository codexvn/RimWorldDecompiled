using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002251 RID: 8785
	public class CompAbilityEffect_Neuroquake : CompAbilityEffect
	{
		// Token: 0x17001D47 RID: 7495
		// (get) Token: 0x0600BFB5 RID: 49077 RVA: 0x00378F6C File Offset: 0x0037716C
		public new CompProperties_AbilityNeuroquake Props
		{
			get
			{
				return (CompProperties_AbilityNeuroquake)this.props;
			}
		}

		// Token: 0x0600BFB6 RID: 49078 RVA: 0x00378F7C File Offset: 0x0037717C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (this.affectedFactions == null)
			{
				this.affectedFactions = new Dictionary<Faction, Pair<bool, Pawn>>();
			}
			else
			{
				this.affectedFactions.Clear();
			}
			this.giveMentalStateTo.Clear();
			foreach (Pawn pawn in this.parent.pawn.Map.mapPawns.AllPawnsSpawned)
			{
				if (this.CanApplyEffects(pawn) && !pawn.Fogged())
				{
					bool flag = !pawn.Spawned || pawn.Position.InHorDistOf(this.parent.pawn.Position, this.parent.def.EffectRadius) || !pawn.Position.InHorDistOf(this.parent.pawn.Position, this.Props.mentalStateRadius);
					this.AffectGoodwill(pawn.HomeFaction, !flag, pawn);
					if (!flag)
					{
						this.giveMentalStateTo.Add(pawn);
					}
					else
					{
						this.GiveNeuroquakeThought(pawn);
					}
				}
			}
			foreach (Map map in Find.Maps)
			{
				if (map != this.parent.pawn.Map && Find.WorldGrid.TraversalDistanceBetween(map.Tile, this.parent.pawn.Map.Tile, true, this.Props.worldRangeTiles + 1, false) <= this.Props.worldRangeTiles)
				{
					foreach (Pawn p in map.mapPawns.AllPawns)
					{
						if (this.CanApplyEffects(p))
						{
							this.GiveNeuroquakeThought(p);
						}
					}
				}
			}
			foreach (Caravan caravan in Find.WorldObjects.Caravans)
			{
				if (Find.WorldGrid.TraversalDistanceBetween(caravan.Tile, this.parent.pawn.Map.Tile, true, this.Props.worldRangeTiles + 1, false) <= this.Props.worldRangeTiles)
				{
					foreach (Pawn p2 in caravan.pawns)
					{
						if (this.CanApplyEffects(p2))
						{
							this.GiveNeuroquakeThought(p2);
						}
					}
				}
			}
			foreach (Pawn pawn2 in this.giveMentalStateTo)
			{
				MentalStateDef def;
				if (!pawn2.RaceProps.IsMechanoid)
				{
					def = MentalStateDefOf.Berserk;
				}
				else
				{
					def = MentalStateDefOf.BerserkMechanoid;
				}
				CompAbilityEffect_GiveMentalState.TryGiveMentalState(def, pawn2, this.parent.def, StatDefOf.PsychicSensitivity, this.parent.pawn, false);
				RestUtility.WakeUp(pawn2, true);
			}
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (!faction.IsPlayer && !faction.defeated && !faction.HostileTo(Faction.OfPlayer))
				{
					this.AffectGoodwill(faction, false, null);
				}
			}
			if (this.parent.pawn.Faction == Faction.OfPlayer)
			{
				foreach (KeyValuePair<Faction, Pair<bool, Pawn>> keyValuePair in this.affectedFactions)
				{
					Faction key = keyValuePair.Key;
					bool first = keyValuePair.Value.First;
					Pawn second = keyValuePair.Value.Second;
					int goodwillChange = first ? this.Props.goodwillImpactForBerserk : this.Props.goodwillImpactForNeuroquake;
					Faction.OfPlayer.TryAffectGoodwillWith(key, goodwillChange, true, true, HistoryEventDefOf.UsedHarmfulAbility, null);
				}
			}
			base.Apply(target, dest);
			this.affectedFactions.Clear();
			this.giveMentalStateTo.Clear();
		}

		// Token: 0x0600BFB7 RID: 49079 RVA: 0x00379448 File Offset: 0x00377648
		private void AffectGoodwill(Faction faction, bool gaveMentalBreak, Pawn p = null)
		{
			Pair<bool, Pawn> pair;
			if (faction != null && !faction.IsPlayer && !faction.HostileTo(Faction.OfPlayer) && (p == null || !p.IsSlaveOfColony) && (!this.affectedFactions.TryGetValue(faction, out pair) || (!pair.First && gaveMentalBreak)))
			{
				this.affectedFactions[faction] = new Pair<bool, Pawn>(gaveMentalBreak, p);
			}
		}

		// Token: 0x0600BFB8 RID: 49080 RVA: 0x003794A9 File Offset: 0x003776A9
		private void GiveNeuroquakeThought(Pawn p)
		{
			Pawn_NeedsTracker needs = p.needs;
			if (needs == null)
			{
				return;
			}
			Need_Mood mood = needs.mood;
			if (mood == null)
			{
				return;
			}
			mood.thoughts.memories.TryGainMemory(ThoughtDefOf.NeuroquakeEcho, null, null);
		}

		// Token: 0x0600BFB9 RID: 49081 RVA: 0x003794D6 File Offset: 0x003776D6
		private bool CanApplyEffects(Pawn p)
		{
			return !p.kindDef.isBoss && (!p.Dead && !p.Suspended) && p.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) > float.Epsilon;
		}

		// Token: 0x0600BFBA RID: 49082 RVA: 0x00379510 File Offset: 0x00377710
		public override void OnGizmoUpdate()
		{
			if (CompAbilityEffect_Neuroquake.cachedRadiusCellsTarget == null || CompAbilityEffect_Neuroquake.cachedRadiusCellsTarget.Value == this.parent.pawn.Position || CompAbilityEffect_Neuroquake.cachedRadiusCellsMap != this.parent.pawn.Map)
			{
				CompAbilityEffect_Neuroquake.cachedRadiusCells.Clear();
				foreach (IntVec3 item in this.parent.pawn.Map.AllCells)
				{
					if (item.InHorDistOf(this.parent.pawn.Position, this.Props.mentalStateRadius))
					{
						CompAbilityEffect_Neuroquake.cachedRadiusCells.Add(item);
					}
				}
				CompAbilityEffect_Neuroquake.cachedRadiusCellsTarget = new IntVec3?(this.parent.pawn.Position);
			}
			GenDraw.DrawFieldEdges(CompAbilityEffect_Neuroquake.cachedRadiusCells, 2900);
		}

		// Token: 0x0400837C RID: 33660
		private Dictionary<Faction, Pair<bool, Pawn>> affectedFactions;

		// Token: 0x0400837D RID: 33661
		private List<Pawn> giveMentalStateTo = new List<Pawn>();

		// Token: 0x0400837E RID: 33662
		private static List<IntVec3> cachedRadiusCells = new List<IntVec3>();

		// Token: 0x0400837F RID: 33663
		private static IntVec3? cachedRadiusCellsTarget = null;

		// Token: 0x04008380 RID: 33664
		private static Map cachedRadiusCellsMap = null;
	}
}
