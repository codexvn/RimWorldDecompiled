using System;
using System.Collections.Generic;
using System.Linq;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002CC7 RID: 11463
	public class CompGrowsFleshmassTendrils : ThingComp
	{
		// Token: 0x170028B9 RID: 10425
		// (get) Token: 0x0601016F RID: 65903 RVA: 0x004A788E File Offset: 0x004A5A8E
		public CompProperties_GrowsFleshmassTendrils Props
		{
			get
			{
				return (CompProperties_GrowsFleshmassTendrils)this.props;
			}
		}

		// Token: 0x170028BA RID: 10426
		// (get) Token: 0x06010170 RID: 65904 RVA: 0x004A789B File Offset: 0x004A5A9B
		protected Map Map
		{
			get
			{
				return this.parent.Map;
			}
		}

		// Token: 0x170028BB RID: 10427
		// (get) Token: 0x06010171 RID: 65905 RVA: 0x004A78A8 File Offset: 0x004A5AA8
		private IntVec3 Position
		{
			get
			{
				return this.parent.Position;
			}
		}

		// Token: 0x170028BC RID: 10428
		// (get) Token: 0x06010172 RID: 65906 RVA: 0x004A78B5 File Offset: 0x004A5AB5
		public int GrowthPoints
		{
			get
			{
				return this.growthPoints;
			}
		}

		// Token: 0x170028BD RID: 10429
		// (get) Token: 0x06010173 RID: 65907 RVA: 0x004A78BD File Offset: 0x004A5ABD
		public IEnumerable<IntVec3> ContiguousFleshmass
		{
			get
			{
				return this.contiguousFleshmass;
			}
		}

		// Token: 0x06010174 RID: 65908 RVA: 0x004A78C8 File Offset: 0x004A5AC8
		public override void PostExposeData()
		{
			Scribe_Collections.Look<FleshTendril>(ref this.tendrils, "tendrils", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.growthPoints, "growthPoints", 0, false);
			Scribe_Values.Look<int>(ref this.nextGrowth, "nextGrowth", 0, false);
			Scribe_Values.Look<int>(ref this.fleshmassUntilFleshbeastBirth, "fleshmassUntilFleshbeastBirth", 0, false);
		}

		// Token: 0x06010175 RID: 65909 RVA: 0x004A7924 File Offset: 0x004A5B24
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.growthPoints = Mathf.RoundToInt(this.Props.startingGrowthPointsByThreat.Evaluate(StorytellerUtility.DefaultThreatPointsNow(this.Map)));
				foreach (IntVec3 pos in this.parent.OccupiedRect().Cells)
				{
					this.CreateTendril(pos);
				}
				if (this.Props.fleshbeastBirthThresholdRange != null)
				{
					this.fleshmassUntilFleshbeastBirth = this.Props.fleshbeastBirthThresholdRange.Value.RandomInRange;
					return;
				}
			}
			else
			{
				this.RecalculateContiguousFleshmass();
			}
		}

		// Token: 0x06010176 RID: 65910 RVA: 0x004A79F4 File Offset: 0x004A5BF4
		public override void CompTick()
		{
			base.CompTick();
			if (this.growthPoints > 0 && Find.TickManager.TicksGame >= this.nextGrowth)
			{
				this.Grow();
				this.growthPoints--;
				this.nextGrowth = Find.TickManager.TicksGame + CompGrowsFleshmassTendrils.GrowthIntervalTicksRange.RandomInRange;
			}
			if (this.drawDebug)
			{
				this.DrawDebug();
			}
		}

		// Token: 0x06010177 RID: 65911 RVA: 0x004A7A64 File Offset: 0x004A5C64
		protected virtual void Grow()
		{
			if (this.fleshMassWasDestroyed)
			{
				this.CheckForDetachedTendrils();
				this.fleshMassWasDestroyed = false;
			}
			IntVec3 intVec = IntVec3.Invalid;
			int num = 0;
			while (intVec == IntVec3.Invalid && num < 10)
			{
				intVec = this.FindValidGrowthPoint();
				num++;
			}
			if (intVec == IntVec3.Invalid)
			{
				return;
			}
			Thing thing = GenSpawn.Spawn(ThingDefOf.Fleshmass_Active, intVec, this.Map, WipeMode.FullRefund);
			thing.SetFaction(this.parent.Faction, null);
			thing.TryGetComp<CompFleshmass>().source = this.parent;
			this.contiguousFleshmass.Add(intVec);
			this.furthestGrownSquared = Mathf.Max(this.furthestGrownSquared, (float)intVec.DistanceToSquared(this.Position));
			EffecterDefOf.MeatExplosionTiny.Spawn(intVec, this.Map, 1f).Cleanup();
		}

		// Token: 0x06010178 RID: 65912 RVA: 0x004A7B34 File Offset: 0x004A5D34
		private IntVec3 FindValidGrowthPoint()
		{
			float value = Rand.Value;
			if (value < CompGrowsFleshmassTendrils.GrowCoreChance)
			{
				return this.GrowCore();
			}
			if (value < CompGrowsFleshmassTendrils.GrowCoreChance + CompGrowsFleshmassTendrils.ThickenTendrilChance && this.contiguousFleshmass.Count > CompGrowsFleshmassTendrils.GrowthsBeforeThickening)
			{
				return this.ThickenTendril();
			}
			return this.GrowTendril();
		}

		// Token: 0x06010179 RID: 65913 RVA: 0x004A7B84 File Offset: 0x004A5D84
		private IntVec3 GrowCore()
		{
			IntVec3 startCell = this.parent.OccupiedRect().Cells.RandomElement<IntVec3>();
			IntVec3 curPos = startCell;
			int num = 0;
			Func<IntVec3, float> <>9__0;
			while (curPos.InHorDistOf(startCell, (float)CompGrowsFleshmassTendrils.CoreRadius) && num < 1000)
			{
				IEnumerable<IntVec3> cardinalDirections = GenAdj.CardinalDirections;
				Func<IntVec3, float> weightSelector;
				if ((weightSelector = <>9__0) == null)
				{
					weightSelector = (<>9__0 = delegate(IntVec3 d)
					{
						int num2 = startCell.DistanceToSquared(curPos);
						return (float)Mathf.Max(startCell.DistanceToSquared(curPos + d) - num2, 0);
					});
				}
				IntVec3 b;
				cardinalDirections.TryRandomElementByWeight(weightSelector, out b);
				curPos += b;
				if (this.AdjacentFleshmass(curPos) == 0)
				{
					return IntVec3.Invalid;
				}
				if (this.CanPlaceFleshmass(curPos))
				{
					return curPos;
				}
				num++;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x0601017A RID: 65914 RVA: 0x004A7C5C File Offset: 0x004A5E5C
		private IntVec3 GrowTendril()
		{
			if (this.tendrils.Count < CompGrowsFleshmassTendrils.MinTendrils)
			{
				for (int i = 0; i < CompGrowsFleshmassTendrils.MinTendrils - this.tendrils.Count; i++)
				{
					if (this.contiguousFleshmass.Count == 0)
					{
						this.CreateTendril(this.parent.OccupiedRect().RandomCell);
					}
					else
					{
						this.CreateTendril(this.contiguousFleshmass.RandomElement<IntVec3>());
					}
				}
			}
			FleshTendril tendril = this.tendrils.RandomElement<FleshTendril>();
			IntVec3 intVec;
			if (!GenAdj.CardinalDirections.TryRandomElementByWeight(delegate(IntVec3 dir)
			{
				if (!this.CanPlaceFleshmass(tendril.currentPos + dir))
				{
					return 0f;
				}
				if (tendril.sourceNode.DistanceToSquared(tendril.currentPos + dir) <= tendril.sourceNode.DistanceToSquared(tendril.currentPos))
				{
					return 0f;
				}
				return Mathf.Max(this.Position.DistanceTo(tendril.currentPos + dir) - this.Position.DistanceTo(tendril.currentPos), 0.5f);
			}, out intVec))
			{
				this.tendrils.Remove(tendril);
				return IntVec3.Invalid;
			}
			IntVec3 intVec2 = tendril.currentPos + intVec * 2;
			if (!intVec2.InBounds(this.Map) || this.CellContainsFleshmass(intVec2))
			{
				this.tendrils.Remove(tendril);
				return IntVec3.Invalid;
			}
			tendril.currentPos += intVec;
			tendril.length++;
			if (tendril.length > CompGrowsFleshmassTendrils.BranchDist.RandomInRange)
			{
				this.CreateBranch(tendril);
			}
			return tendril.currentPos;
		}

		// Token: 0x0601017B RID: 65915 RVA: 0x004A7DCC File Offset: 0x004A5FCC
		private IntVec3 ThickenTendril()
		{
			IntVec3 cellToThicken;
			(from cell in this.contiguousFleshmass
			where this.AdjacentFleshmass(cell) < 4
			select cell).TryRandomElementByWeight((IntVec3 cell) => this.furthestGrownSquared - (float)this.Position.DistanceToSquared(cell), out cellToThicken);
			IntVec3 b;
			if (GenAdj.AdjacentCells.TryRandomElementByWeight(delegate(IntVec3 dir)
			{
				IntVec3 cell = cellToThicken + dir;
				int num = this.AdjacentFleshmass(cell);
				if (!this.CanPlaceFleshmass(cell))
				{
					return 0f;
				}
				if (this.CellContainsFleshmass(cell))
				{
					return 0f;
				}
				if (num == 1)
				{
					return 1f;
				}
				if (num == 2)
				{
					return 10f;
				}
				if (num == 3)
				{
					return 100f;
				}
				if (num == 4)
				{
					return 1000f;
				}
				return 0f;
			}, out b))
			{
				return cellToThicken + b;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x0601017C RID: 65916 RVA: 0x004A7E41 File Offset: 0x004A6041
		private void CreateBranch(FleshTendril tendril)
		{
			this.tendrils.Remove(tendril);
			this.CreateTendril(tendril.currentPos);
			this.CreateTendril(tendril.currentPos);
		}

		// Token: 0x0601017D RID: 65917 RVA: 0x004A7E68 File Offset: 0x004A6068
		private void CreateTendril(IntVec3 pos)
		{
			this.tendrils.Add(new FleshTendril
			{
				sourceNode = pos,
				currentPos = pos
			});
		}

		// Token: 0x0601017E RID: 65918 RVA: 0x004A7E88 File Offset: 0x004A6088
		public override void PostSwapMap()
		{
			this.RecalculateContiguousFleshmass();
		}

		// Token: 0x0601017F RID: 65919 RVA: 0x004A7E90 File Offset: 0x004A6090
		private void RecalculateContiguousFleshmass()
		{
			this.furthestGrownSquared = 0f;
			this.contiguousFleshmass.Clear();
			this.Map.floodFiller.FloodFill(this.Position, new Predicate<IntVec3>(this.CellContainsFleshmass), delegate(IntVec3 pos)
			{
				this.contiguousFleshmass.Add(pos);
			}, int.MaxValue, false, null);
			this.furthestGrownSquared = (float)this.contiguousFleshmass.Max((IntVec3 cell) => cell.DistanceToSquared(this.Position));
		}

		// Token: 0x06010180 RID: 65920 RVA: 0x004A7F08 File Offset: 0x004A6108
		private bool CellContainsFleshmass(IntVec3 cell)
		{
			Building edifice = cell.GetEdifice(this.Map);
			return edifice != null && (edifice.def == ThingDefOf.Fleshmass_Active || edifice.def == ThingDefOf.FleshmassSpitter || edifice.def == ThingDefOf.NerveBundle || edifice.HasComp<CompGrowsFleshmassTendrils>());
		}

		// Token: 0x06010181 RID: 65921 RVA: 0x004A7F58 File Offset: 0x004A6158
		private bool CanPlaceFleshmass(IntVec3 cell)
		{
			if (!cell.InBounds(this.Map))
			{
				return false;
			}
			if (!cell.SupportsStructureType(this.Map, ThingDefOf.Fleshmass_Active.terrainAffordanceNeeded))
			{
				return false;
			}
			foreach (Thing thing in cell.GetThingList(this.Map))
			{
				if (!(thing is Building_Door))
				{
					Building building = thing as Building;
					if ((building == null || !building.IsClearableFreeBuilding) && !(thing is Blueprint))
					{
						ThingDef def = thing.def;
						bool? flag;
						if (def == null)
						{
							flag = null;
						}
						else
						{
							BuildingProperties building2 = def.building;
							flag = ((building2 != null) ? new bool?(building2.isPowerConduit) : null);
						}
						bool? flag2 = flag;
						if (!flag2.GetValueOrDefault())
						{
							ThingDef def2 = thing.def;
							bool? flag3;
							if (def2 == null)
							{
								flag3 = null;
							}
							else
							{
								BuildingProperties building3 = def2.building;
								flag3 = ((building3 != null) ? new bool?(building3.isAttachment) : null);
							}
							flag2 = flag3;
							if (!flag2.GetValueOrDefault())
							{
								if (!GenConstruct.CanPlaceBlueprintOver(ThingDefOf.Fleshmass_Active, thing.def, null, null))
								{
									return false;
								}
								ThingDef def3 = thing.def;
								if (def3 != null && def3.preventSkyfallersLandingOn)
								{
									return false;
								}
								Pawn pawn = thing as Pawn;
								if (pawn != null && pawn.RaceProps.Humanlike)
								{
									return false;
								}
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06010182 RID: 65922 RVA: 0x004A80E8 File Offset: 0x004A62E8
		protected int AdjacentFleshmass(IntVec3 cell)
		{
			int num = 0;
			for (int i = 0; i < GenAdj.CardinalDirections.Length; i++)
			{
				IntVec3 intVec = cell + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(this.Map) && this.CellContainsFleshmass(intVec))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06010183 RID: 65923 RVA: 0x004A8137 File Offset: 0x004A6337
		public void Notify_FleshmassDestroyed(Thing mass)
		{
			this.fleshMassWasDestroyed = true;
		}

		// Token: 0x06010184 RID: 65924 RVA: 0x004A8140 File Offset: 0x004A6340
		public void Notify_FleshmassDestroyedByPlayer(Thing mass)
		{
			if (this.fleshmassUntilFleshbeastBirth > 0)
			{
				this.fleshmassUntilFleshbeastBirth--;
			}
			if (this.fleshmassUntilFleshbeastBirth == 0)
			{
				FleshbeastUtility.DoFleshbeastResponse(this, mass.Position);
				this.fleshmassUntilFleshbeastBirth = this.Props.fleshbeastBirthThresholdRange.Value.RandomInRange;
			}
		}

		// Token: 0x06010185 RID: 65925 RVA: 0x004A8198 File Offset: 0x004A6398
		public void CheckForDetachedTendrils()
		{
			this.RecalculateContiguousFleshmass();
			this.tendrils.RemoveAll((FleshTendril tendril) => !this.contiguousFleshmass.Contains(tendril.currentPos));
			if (this.contiguousFleshmass.Count < 10)
			{
				foreach (IntVec3 pos in this.parent.OccupiedRect().Cells)
				{
					this.CreateTendril(pos);
				}
			}
		}

		// Token: 0x06010186 RID: 65926 RVA: 0x004A8220 File Offset: 0x004A6420
		private void DrawDebug()
		{
			if (this.fleshMassWasDestroyed)
			{
				this.CheckForDetachedTendrils();
				this.fleshMassWasDestroyed = false;
			}
			foreach (FleshTendril fleshTendril in this.tendrils)
			{
				GenDraw.DrawFieldEdges(new List<IntVec3>
				{
					fleshTendril.currentPos
				}, Color.green, null, null, 2900);
			}
		}

		// Token: 0x06010187 RID: 65927 RVA: 0x004A82AC File Offset: 0x004A64AC
		public override string CompInspectStringExtra()
		{
			if (DebugSettings.godMode)
			{
				return "DEV: Fleshmass until fleshbeast birth: " + this.fleshmassUntilFleshbeastBirth.ToString();
			}
			return "";
		}

		// Token: 0x06010188 RID: 65928 RVA: 0x004A82D0 File Offset: 0x004A64D0
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "DEV: View tendril growth debug",
					defaultDesc = "View tendril growth debug",
					isActive = (() => this.drawDebug),
					toggleAction = delegate()
					{
						this.drawDebug = !this.drawDebug;
					}
				};
			}
			yield break;
		}

		// Token: 0x0400A81E RID: 43038
		private static readonly IntRange GrowthIntervalTicksRange = new IntRange(5, 15);

		// Token: 0x0400A81F RID: 43039
		private static readonly IntRange BranchDist = new IntRange(20, 50);

		// Token: 0x0400A820 RID: 43040
		[TweakValue("Fleshmass Heart", 0f, 1f)]
		private static float GrowCoreChance = 0.1f;

		// Token: 0x0400A821 RID: 43041
		[TweakValue("Fleshmass Heart", 0f, 1f)]
		private static float ThickenTendrilChance = 0.6f;

		// Token: 0x0400A822 RID: 43042
		[TweakValue("Fleshmass Heart", 0f, 300f)]
		private static int GrowthsBeforeThickening = 100;

		// Token: 0x0400A823 RID: 43043
		[TweakValue("Fleshmass Heart", 0f, 20f)]
		private static int CoreRadius = 15;

		// Token: 0x0400A824 RID: 43044
		[TweakValue("Fleshmass Heart", 0f, 10f)]
		private static int MinTendrils = 3;

		// Token: 0x0400A825 RID: 43045
		private List<FleshTendril> tendrils = new List<FleshTendril>();

		// Token: 0x0400A826 RID: 43046
		protected int growthPoints;

		// Token: 0x0400A827 RID: 43047
		private int nextGrowth = -99999;

		// Token: 0x0400A828 RID: 43048
		private int fleshmassUntilFleshbeastBirth = -1;

		// Token: 0x0400A829 RID: 43049
		protected List<IntVec3> contiguousFleshmass = new List<IntVec3>();

		// Token: 0x0400A82A RID: 43050
		private float furthestGrownSquared;

		// Token: 0x0400A82B RID: 43051
		private bool fleshMassWasDestroyed;

		// Token: 0x0400A82C RID: 43052
		private bool drawDebug;
	}
}
