using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020001CF RID: 463
	public struct TraverseParms : IEquatable<TraverseParms>
	{
		// Token: 0x06000CA6 RID: 3238 RVA: 0x0003B374 File Offset: 0x00039574
		public static TraverseParms For(Pawn pawn, Danger maxDanger = Danger.Deadly, TraverseMode mode = TraverseMode.ByPawn, bool canBashDoors = false, bool alwaysUseAvoidGrid = false, bool canBashFences = false, bool avoidPersistentDanger = true)
		{
			if (pawn == null)
			{
				Log.Error("TraverseParms for null pawn.");
				return TraverseParms.For(TraverseMode.NoPassClosedDoors, maxDanger, canBashDoors, alwaysUseAvoidGrid, canBashFences, true, false);
			}
			TraverseParms result = new TraverseParms
			{
				pawn = pawn,
				maxDanger = maxDanger,
				mode = mode,
				canBashDoors = canBashDoors,
				canBashFences = canBashFences,
				alwaysUseAvoidGrid = alwaysUseAvoidGrid,
				fenceBlocked = pawn.ShouldAvoidFences,
				avoidPersistentDanger = avoidPersistentDanger,
				avoidDarknessDanger = GameCondition_UnnaturalDarkness.AffectedByDarkness(pawn),
				avoidFog = (pawn.IsPlayerControlled && !pawn.Drafted)
			};
			if (pawn.CurJob != null)
			{
				IBuildableDriver buildableDriver = pawn.CurJob.GetCachedDriver(pawn) as IBuildableDriver;
				CellRect cellRect;
				if (buildableDriver != null && buildableDriver.TryGetBuildableRect(out cellRect))
				{
					result.targetBuildable = cellRect;
				}
			}
			return result;
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0003B448 File Offset: 0x00039648
		public static TraverseParms For(TraverseMode mode, Danger maxDanger = Danger.Deadly, bool canBashDoors = false, bool alwaysUseAvoidGrid = false, bool canBashFences = false, bool avoidPersistentDanger = true, bool fogBlocked = false)
		{
			return new TraverseParms
			{
				pawn = null,
				mode = mode,
				maxDanger = maxDanger,
				canBashDoors = canBashDoors,
				canBashFences = canBashFences,
				alwaysUseAvoidGrid = alwaysUseAvoidGrid,
				fenceBlocked = false,
				avoidPersistentDanger = avoidPersistentDanger,
				avoidFog = fogBlocked
			};
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x0003B4A9 File Offset: 0x000396A9
		public TraverseParms WithFenceblockedOf(Pawn otherPawn)
		{
			return this.WithFenceblocked(otherPawn.ShouldAvoidFences);
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0003B4B8 File Offset: 0x000396B8
		public TraverseParms WithFenceblocked(bool forceFenceblocked)
		{
			return new TraverseParms
			{
				pawn = this.pawn,
				mode = this.mode,
				maxDanger = this.maxDanger,
				canBashDoors = this.canBashDoors,
				canBashFences = this.canBashFences,
				alwaysUseAvoidGrid = this.alwaysUseAvoidGrid,
				fenceBlocked = (this.fenceBlocked || forceFenceblocked),
				avoidPersistentDanger = true
			};
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0003B533 File Offset: 0x00039733
		public void Validate()
		{
			if (this.mode == TraverseMode.ByPawn && this.pawn == null)
			{
				Log.Error("Invalid traverse parameters: IfPawnAllowed but traverser = null.");
			}
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x0003B54F File Offset: 0x0003974F
		public static implicit operator TraverseParms(TraverseMode m)
		{
			if (m == TraverseMode.ByPawn)
			{
				throw new InvalidOperationException("Cannot implicitly convert TraverseMode.ByPawn to RegionTraverseParameters.");
			}
			return TraverseParms.For(m, Danger.Deadly, false, false, false, true, false);
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x0003B56C File Offset: 0x0003976C
		public static bool operator ==(TraverseParms a, TraverseParms b)
		{
			return a.pawn == b.pawn && a.mode == b.mode && a.canBashDoors == b.canBashDoors && a.canBashFences == b.canBashFences && a.maxDanger == b.maxDanger && a.alwaysUseAvoidGrid == b.alwaysUseAvoidGrid && a.fenceBlocked == b.fenceBlocked && a.avoidPersistentDanger == b.avoidPersistentDanger;
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x0003B5EB File Offset: 0x000397EB
		public static bool operator !=(TraverseParms a, TraverseParms b)
		{
			return !(a == b);
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x0003B5F8 File Offset: 0x000397F8
		public override bool Equals(object obj)
		{
			if (obj is TraverseParms)
			{
				TraverseParms other = (TraverseParms)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x0003B620 File Offset: 0x00039820
		public bool Equals(TraverseParms other)
		{
			return other.pawn == this.pawn && other.mode == this.mode && other.canBashDoors == this.canBashDoors && other.canBashFences == this.canBashFences && other.maxDanger == this.maxDanger && other.alwaysUseAvoidGrid == this.alwaysUseAvoidGrid && other.fenceBlocked == this.fenceBlocked && other.avoidPersistentDanger == this.avoidPersistentDanger;
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0003B6A0 File Offset: 0x000398A0
		public override int GetHashCode()
		{
			int seed = this.canBashDoors ? 1 : 0;
			seed = ((this.pawn != null) ? Gen.HashCombine<Pawn>(seed, this.pawn) : Gen.HashCombineStruct<TraverseMode>(seed, this.mode));
			seed = Gen.HashCombineStruct<bool>(seed, this.canBashFences);
			seed = Gen.HashCombineStruct<Danger>(seed, this.maxDanger);
			seed = Gen.HashCombineStruct<bool>(seed, this.alwaysUseAvoidGrid);
			seed = Gen.HashCombineStruct<bool>(seed, this.fenceBlocked);
			return Gen.HashCombineStruct<bool>(seed, this.avoidPersistentDanger);
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0003B720 File Offset: 0x00039920
		public override string ToString()
		{
			string text = this.canBashDoors ? " canBashDoors" : "";
			string text2 = this.canBashFences ? " canBashFences" : "";
			string text3 = this.alwaysUseAvoidGrid ? " alwaysUseAvoidGrid" : "";
			string text4 = this.fenceBlocked ? " fenceBlocked" : "";
			string text5 = this.avoidPersistentDanger ? " avoidPersistentDanger" : "";
			if (this.mode == TraverseMode.ByPawn)
			{
				return string.Format("({0} {1} {2}{3}{4}{5}{6}{7})", new object[]
				{
					this.mode,
					this.maxDanger,
					this.pawn,
					text,
					text2,
					text3,
					text4,
					text5
				});
			}
			return string.Format("({0} {1}{2}{3}{4}{5}{6})", new object[]
			{
				this.mode,
				this.maxDanger,
				text,
				text2,
				text3,
				text4,
				text5
			});
		}

		// Token: 0x04000737 RID: 1847
		public Pawn pawn;

		// Token: 0x04000738 RID: 1848
		public TraverseMode mode;

		// Token: 0x04000739 RID: 1849
		public Danger maxDanger;

		// Token: 0x0400073A RID: 1850
		public bool canBashDoors;

		// Token: 0x0400073B RID: 1851
		public bool canBashFences;

		// Token: 0x0400073C RID: 1852
		public bool alwaysUseAvoidGrid;

		// Token: 0x0400073D RID: 1853
		public bool fenceBlocked;

		// Token: 0x0400073E RID: 1854
		public bool avoidPersistentDanger;

		// Token: 0x0400073F RID: 1855
		public bool avoidDarknessDanger;

		// Token: 0x04000740 RID: 1856
		public bool avoidFog;

		// Token: 0x04000741 RID: 1857
		public CellRect targetBuildable;
	}
}
