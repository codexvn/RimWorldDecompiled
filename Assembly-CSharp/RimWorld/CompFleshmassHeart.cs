using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C93 RID: 11411
	public class CompFleshmassHeart : CompGrowsFleshmassTendrils
	{
		// Token: 0x1700286F RID: 10351
		// (get) Token: 0x06010011 RID: 65553 RVA: 0x0049C89B File Offset: 0x0049AA9B
		public Building_FleshmassHeart Heart
		{
			get
			{
				return this.parent as Building_FleshmassHeart;
			}
		}

		// Token: 0x06010012 RID: 65554 RVA: 0x004A2BA0 File Offset: 0x004A0DA0
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.threatPoints, "threatPoints", 0f, false);
			Scribe_Values.Look<float>(ref this.fleshbeastPoints, "fleshbeastPoints", 0f, false);
			Scribe_Values.Look<int>(ref this.nextFleshbeast, "nextFleshbeast", 0, false);
			Scribe_Values.Look<int>(ref this.lastGrowthCycle, "lastGrowthCycle", 0, false);
			Scribe_Values.Look<int>(ref this.nerveBundlesThisGrowthCycle, "nerveBundlesThisGrowthCycle", 0, false);
			Scribe_Values.Look<int>(ref this.spittersThisGrowthCycle, "spittersThisGrowthCycle", 0, false);
			Scribe_Collections.Look<Thing>(ref this.nerveBundles, "nerveBundles", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.spitters, "spitters", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x06010013 RID: 65555 RVA: 0x004A2C54 File Offset: 0x004A0E54
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				if (this.threatPoints <= 0f)
				{
					this.threatPoints = StorytellerUtility.DefaultThreatPointsNow(base.Map);
				}
				this.fleshbeastPoints = (float)Mathf.RoundToInt(this.threatPoints * 0.75f);
				this.lastGrowthCycle = Find.TickManager.TicksGame;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x06010014 RID: 65556 RVA: 0x004A2CB4 File Offset: 0x004A0EB4
		public override void CompTick()
		{
			int num = Find.TickManager.TicksGame - this.lastGrowthCycle;
			if (base.Props.maxGrowthCycleHours > 0 && num > base.Props.maxGrowthCycleHours * 2500)
			{
				this.StartGrowthCycle();
			}
			else if (base.Props.mtbGrowthCycleHours > 0 && num > base.Props.minGrowthCycleHours * 2500 && Rand.MTBEventOccurs((float)base.Props.mtbGrowthCycleHours, 2500f, 1f))
			{
				this.StartGrowthCycle();
			}
			if (Rand.MTBEventOccurs(2f, 2500f, 1f))
			{
				this.StartMiniGrowth();
			}
			if (base.Props.spawnFleshbeasts && this.fleshbeastPoints > 0f && Find.TickManager.TicksGame >= this.nextFleshbeast)
			{
				this.fleshbeastPoints -= this.SpawnFleshBeast();
				this.nextFleshbeast = Find.TickManager.TicksGame + CompFleshmassHeart.FleshbeastSpawnIntervalRange.RandomInRange;
			}
			base.CompTick();
		}

		// Token: 0x06010015 RID: 65557 RVA: 0x004A2DC4 File Offset: 0x004A0FC4
		private void StartGrowthCycle()
		{
			float num = StorytellerUtility.DefaultThreatPointsNow(base.Map);
			float num2 = FleshbeastUtility.ExistingFleshBeastThreat(base.Map);
			int growthPoints = this.growthPoints;
			SimpleCurve growthCyclePointsByThreat = base.Props.growthCyclePointsByThreat;
			this.growthPoints = growthPoints + Mathf.RoundToInt((growthCyclePointsByThreat != null) ? growthCyclePointsByThreat.Evaluate(num) : 0f);
			float num3 = this.fleshbeastPoints;
			SimpleCurve fleshbeastPointsByThreat = base.Props.fleshbeastPointsByThreat;
			this.fleshbeastPoints = num3 + Mathf.Clamp((fleshbeastPointsByThreat != null) ? fleshbeastPointsByThreat.Evaluate(num) : 0f, 0f, num - num2);
			this.lastGrowthCycle = Find.TickManager.TicksGame;
			this.nerveBundlesThisGrowthCycle = 0;
			this.spittersThisGrowthCycle = 0;
		}

		// Token: 0x06010016 RID: 65558 RVA: 0x004A2E6C File Offset: 0x004A106C
		private void StartMiniGrowth()
		{
			float num = StorytellerUtility.DefaultThreatPointsNow(base.Map);
			int growthPoints = this.growthPoints;
			SimpleCurve growthCyclePointsByThreat = base.Props.growthCyclePointsByThreat;
			this.growthPoints = growthPoints + Mathf.RoundToInt((growthCyclePointsByThreat != null) ? growthCyclePointsByThreat.Evaluate(num / 5f) : 0f);
		}

		// Token: 0x06010017 RID: 65559 RVA: 0x004A2EBC File Offset: 0x004A10BC
		protected override void Grow()
		{
			if (this.nerveBundles.Count < 3 && this.nerveBundlesThisGrowthCycle < 1)
			{
				Thing thing = this.TrySpawnSpecialMass(ThingDefOf.NerveBundle);
				if (thing != null)
				{
					Messages.Message("NerveBundleGrewMessage".Translate(), thing, MessageTypeDefOf.NeutralEvent, true);
					this.nerveBundles.Add(thing);
					this.nerveBundlesThisGrowthCycle++;
					return;
				}
			}
			if ((float)this.spitters.Count < CompFleshmassHeart.TargetSpittersCountByThreatPointsCurve.Evaluate(this.threatPoints) && this.spittersThisGrowthCycle < 1)
			{
				Thing thing2 = this.TrySpawnSpecialMass(ThingDefOf.FleshmassSpitter);
				if (thing2 != null)
				{
					this.spitters.Add(thing2);
					this.spittersThisGrowthCycle++;
					return;
				}
			}
			base.Grow();
		}

		// Token: 0x06010018 RID: 65560 RVA: 0x004A2F84 File Offset: 0x004A1184
		private Thing TrySpawnSpecialMass(ThingDef def)
		{
			if (base.ContiguousFleshmass.EnumerableNullOrEmpty<IntVec3>())
			{
				return null;
			}
			for (int i = 0; i < 10; i++)
			{
				IntVec3 intVec = base.ContiguousFleshmass.RandomElement<IntVec3>();
				if (this.CanSpawnSpecialMass(def, intVec))
				{
					Thing thing = GenSpawn.Spawn(def, intVec, base.Map, WipeMode.Vanish);
					thing.TryGetComp<CompFleshmassHeartChild>().heart = this.Heart;
					thing.SetFaction(this.Heart.Faction, null);
					return thing;
				}
			}
			return null;
		}

		// Token: 0x06010019 RID: 65561 RVA: 0x004A2FF8 File Offset: 0x004A11F8
		private bool CanSpawnSpecialMass(ThingDef def, IntVec3 pos)
		{
			if (!GenAdj.OccupiedRect(pos, Rot4.North, def.size).InBounds(this.parent.Map))
			{
				return false;
			}
			if (this.parent.Position.InHorDistOf(pos, 4f))
			{
				return false;
			}
			using (List<Thing>.Enumerator enumerator = this.nerveBundles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Position.InHorDistOf(pos, 4f))
					{
						return false;
					}
				}
			}
			using (List<Thing>.Enumerator enumerator = this.spitters.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Position.InHorDistOf(pos, 4f))
					{
						return false;
					}
				}
			}
			foreach (IntVec3 c in GenAdj.CellsOccupiedBy(pos, Rot4.North, def.size))
			{
				Building edifice = c.GetEdifice(this.parent.Map);
				if (((edifice != null) ? edifice.def : null) != ThingDefOf.Fleshmass_Active)
				{
					return false;
				}
				if (def == ThingDefOf.FleshmassSpitter && c.Roofed(this.parent.Map))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0601001A RID: 65562 RVA: 0x004A318C File Offset: 0x004A138C
		private float SpawnFleshBeast()
		{
			PawnKindDef kind = this.PickFleshBeastToSpawn();
			Faction ofEntities = Faction.OfEntities;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			float? fixedBiologicalAge = new float?(0f);
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, ofEntities, context, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			Building_FleshmassHeart building_FleshmassHeart = this.parent as Building_FleshmassHeart;
			if (building_FleshmassHeart != null)
			{
				building_FleshmassHeart.DefendHeartLord.AddPawn(pawn);
			}
			IntVec3 intVec = this.FindValidSpawnPoint();
			IntVec3 intVec2 = CellFinder.StandableCellNear(intVec, base.Map, 5f, null);
			if (intVec2 == IntVec3.Invalid)
			{
				return pawn.kindDef.combatPower;
			}
			GenSpawn.Spawn(pawn, intVec2, base.Map, WipeMode.FullRefund);
			pawn.rotationTracker.FaceCell(intVec2);
			EffecterDefOf.MeatExplosionExtraLarge.Spawn(intVec, base.Map, 1f).Cleanup();
			return pawn.kindDef.combatPower;
		}

		// Token: 0x0601001B RID: 65563 RVA: 0x004A32CC File Offset: 0x004A14CC
		private PawnKindDef PickFleshBeastToSpawn()
		{
			PawnKindDef pawnKindDef = FleshbeastUtility.AllFleshbeasts.RandomElement<PawnKindDef>();
			if (this.fleshbeastPoints < pawnKindDef.combatPower)
			{
				return PawnKindDefOf.Fingerspike;
			}
			return pawnKindDef;
		}

		// Token: 0x0601001C RID: 65564 RVA: 0x004A32FC File Offset: 0x004A14FC
		private IntVec3 FindValidSpawnPoint()
		{
			if (this.contiguousFleshmass.Empty<IntVec3>())
			{
				return this.parent.OccupiedRect().Cells.RandomElement<IntVec3>();
			}
			for (int i = 0; i < 20; i++)
			{
				IntVec3 intVec = this.contiguousFleshmass.RandomElement<IntVec3>();
				if (base.AdjacentFleshmass(intVec) < 4)
				{
					return intVec;
				}
			}
			this.contiguousFleshmass.Shuffle<IntVec3>();
			foreach (IntVec3 intVec2 in this.contiguousFleshmass)
			{
				if (base.AdjacentFleshmass(intVec2) < 4)
				{
					return intVec2;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x0601001D RID: 65565 RVA: 0x004A33B8 File Offset: 0x004A15B8
		public void Notify_ChildDied(Thing thing)
		{
			if (thing.def == ThingDefOf.NerveBundle)
			{
				this.nerveBundles.Remove(thing);
			}
			if (thing.def == ThingDefOf.FleshmassSpitter)
			{
				this.spitters.Remove(thing);
			}
		}

		// Token: 0x0601001E RID: 65566 RVA: 0x004A33F0 File Offset: 0x004A15F0
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			foreach (Thing thing in this.nerveBundles)
			{
				CompFleshmassHeartChild compFleshmassHeartChild = thing.TryGetComp<CompFleshmassHeartChild>();
				if (compFleshmassHeartChild != null)
				{
					compFleshmassHeartChild.Notify_HeartDied();
				}
			}
			foreach (Thing thing2 in this.spitters)
			{
				CompFleshmassHeartChild compFleshmassHeartChild2 = thing2.TryGetComp<CompFleshmassHeartChild>();
				if (compFleshmassHeartChild2 != null)
				{
					compFleshmassHeartChild2.Notify_HeartDied();
				}
			}
		}

		// Token: 0x0601001F RID: 65567 RVA: 0x004A3498 File Offset: 0x004A1698
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
					defaultLabel = "DEV: Start growth cycle",
					defaultDesc = "Start growth cycle",
					action = new Action(this.StartGrowthCycle)
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Add 100 growth points",
					defaultDesc = "Add 100 growth points",
					action = delegate()
					{
						this.growthPoints += 100;
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Add 200 fleshbeast points",
					defaultDesc = "Add 200 fleshbeast points",
					action = delegate()
					{
						this.fleshbeastPoints += 200f;
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Do Tachycardiac Overload",
					defaultDesc = "Start Tachycardiac overload",
					action = delegate()
					{
						Building_FleshmassHeart building_FleshmassHeart = this.parent as Building_FleshmassHeart;
						if (building_FleshmassHeart == null)
						{
							return;
						}
						building_FleshmassHeart.StartTachycardiacOverload();
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0400A76B RID: 42859
		private static readonly IntRange FleshbeastSpawnIntervalRange = new IntRange(60, 240);

		// Token: 0x0400A76C RID: 42860
		private const int MinDistanceBetweenSpecialMass = 4;

		// Token: 0x0400A76D RID: 42861
		private const int TargetNerveBundleCount = 3;

		// Token: 0x0400A76E RID: 42862
		private const int MaxNerveBundlesPerGrowthCycle = 1;

		// Token: 0x0400A76F RID: 42863
		private const int MaxSpittersPerGrowthCycle = 1;

		// Token: 0x0400A770 RID: 42864
		private const float InitialFleshbeastPointMultiplier = 0.75f;

		// Token: 0x0400A771 RID: 42865
		private const int SmallGrowthMTBHours = 2;

		// Token: 0x0400A772 RID: 42866
		private static readonly SimpleCurve TargetSpittersCountByThreatPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(200f, 1f),
				true
			},
			{
				new CurvePoint(800f, 2f),
				true
			},
			{
				new CurvePoint(3000f, 4f),
				true
			},
			{
				new CurvePoint(10000f, 7f),
				true
			}
		};

		// Token: 0x0400A773 RID: 42867
		public float threatPoints;

		// Token: 0x0400A774 RID: 42868
		private float fleshbeastPoints;

		// Token: 0x0400A775 RID: 42869
		private int nextFleshbeast = -99999;

		// Token: 0x0400A776 RID: 42870
		private int lastGrowthCycle = -99999;

		// Token: 0x0400A777 RID: 42871
		private int nerveBundlesThisGrowthCycle;

		// Token: 0x0400A778 RID: 42872
		private int spittersThisGrowthCycle;

		// Token: 0x0400A779 RID: 42873
		private List<Thing> nerveBundles = new List<Thing>();

		// Token: 0x0400A77A RID: 42874
		private List<Thing> spitters = new List<Thing>();
	}
}
