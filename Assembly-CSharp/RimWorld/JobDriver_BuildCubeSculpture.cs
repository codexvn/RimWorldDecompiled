using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_BuildCubeSculpture : JobDriver
{
	private enum CubeMaterial
	{
		Dirt,
		Stone,
		Sand,
		Scrap
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__7 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_BuildCubeSculpture _003C_003E4__this;

		Toil IEnumerator<Toil>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CMakeNewToils_003Ed__7(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_BuildCubeSculpture CS_0024_003C_003E8__locals15 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Toil toil2 = Toils_Goto.Goto(TargetIndex.A, PathEndMode.OnCell);
				toil2.AddFinishAction(CS_0024_003C_003E8__locals15.CacheGroundType);
				_003C_003E2__current = toil2;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_General.Wait(5000);
				toil.WithProgressBar(TargetIndex.A, () => CS_0024_003C_003E8__locals15.workDone / 5000f);
				toil.WithEffect(CS_0024_003C_003E8__locals15.GetSculptureEffecter, TargetIndex.A, null);
				toil.activeSkill = () => SkillDefOf.Construction;
				toil.handlingFacing = true;
				toil.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals15.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals15.TargetA);
					float num2 = 1.7f;
					if (CS_0024_003C_003E8__locals15.pawn.skills != null)
					{
						CS_0024_003C_003E8__locals15.pawn.skills.Learn(SkillDefOf.Construction, 0.25f * (float)delta);
						if (!CS_0024_003C_003E8__locals15.pawn.skills.GetSkill(SkillDefOf.Construction).TotallyDisabled)
						{
							num2 *= CS_0024_003C_003E8__locals15.pawn.GetStatValue(StatDefOf.ConstructionSpeed);
						}
					}
					if (CS_0024_003C_003E8__locals15.pawn.Stuff != null)
					{
						num2 *= CS_0024_003C_003E8__locals15.pawn.Stuff.GetStatValueAbstract(StatDefOf.ConstructionSpeedFactor);
					}
					CS_0024_003C_003E8__locals15.workDone += num2 * (float)delta;
					if (CS_0024_003C_003E8__locals15.workDone >= 5000f)
					{
						CS_0024_003C_003E8__locals15.ReadyForNextToil();
					}
				};
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(CS_0024_003C_003E8__locals15.PlaceAndFinish);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__7(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	private CubeMaterial cubeMaterial = CubeMaterial.Scrap;

	private float workDone;

	private const TargetIndex Target = TargetIndex.A;

	private const int BuildTimeTicks = 5000;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
	}

	public override string GetReport()
	{
		if (base.CurToilIndex == 0)
		{
			return "ReportBuildingCubeEnroute".Translate();
		}
		return job.def.reportString.Formatted(GetMaterialString());
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__7))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void CacheGroundType()
	{
		TerrainDef terrainDef = pawn.MapHeld.terrainGrid.TerrainAt(base.TargetA.Cell);
		if (terrainDef == null)
		{
			cubeMaterial = CubeMaterial.Scrap;
		}
		else if (terrainDef.categoryType == TerrainDef.TerrainCategoryType.Soil)
		{
			cubeMaterial = CubeMaterial.Dirt;
		}
		else if (terrainDef.categoryType == TerrainDef.TerrainCategoryType.Stone)
		{
			cubeMaterial = CubeMaterial.Stone;
		}
		else if (terrainDef.categoryType == TerrainDef.TerrainCategoryType.Sand)
		{
			cubeMaterial = CubeMaterial.Sand;
		}
		else
		{
			cubeMaterial = CubeMaterial.Scrap;
		}
	}

	private void PlaceAndFinish()
	{
		Thing thing = GenSpawn.Spawn(GetSculptureThing(), base.TargetA.Cell, pawn.MapHeld);
		thing.SetFaction(Faction.OfPlayer);
		CompQuality compQuality = thing.TryGetComp<CompQuality>();
		if (compQuality != null)
		{
			compQuality.SetQuality(QualityUtility.GenerateQualityCreatedByPawn(pawn, SkillDefOf.Construction, consumeInspiration: false), ArtGenerationContext.Colony);
			QualityUtility.SendCraftNotification(thing, pawn);
		}
		CompArt compArt = thing.TryGetComp<CompArt>();
		if (compArt != null)
		{
			if (compQuality == null)
			{
				compArt.InitializeArt(ArtGenerationContext.Colony);
			}
			compArt.JustCreatedBy(pawn);
		}
		if (pawn.health.hediffSet.TryGetHediff<Hediff_CubeInterest>(out var hediff))
		{
			hediff.Notify_BuiltSculpture(thing);
		}
		if (pawn.InMentalState && pawn.MentalStateDef == MentalStateDefOf.CubeSculpting)
		{
			pawn.MentalState.RecoverFromState();
		}
	}

	private Thing GetSculptureThing()
	{
		ThingDef def = null;
		if (cubeMaterial == CubeMaterial.Dirt)
		{
			def = ThingDefOf.DirtCubeSculpture;
		}
		else if (cubeMaterial == CubeMaterial.Stone)
		{
			def = ThingDefOf.StoneCubeSculpture;
		}
		else if (cubeMaterial == CubeMaterial.Sand)
		{
			def = ThingDefOf.SandCubeSculpture;
		}
		else if (cubeMaterial == CubeMaterial.Scrap)
		{
			def = ThingDefOf.ScrapCubeSculpture;
		}
		return ThingMaker.MakeThing(def);
	}

	private EffecterDef GetSculptureEffecter()
	{
		EffecterDef result = null;
		if (cubeMaterial == CubeMaterial.Dirt)
		{
			result = EffecterDefOf.ConstructDirt;
		}
		else if (cubeMaterial == CubeMaterial.Stone)
		{
			result = EffecterDefOf.ConstructDirt;
		}
		else if (cubeMaterial == CubeMaterial.Sand)
		{
			result = EffecterDefOf.ConstructDirt;
		}
		else if (cubeMaterial == CubeMaterial.Scrap)
		{
			result = EffecterDefOf.ConstructMetal;
		}
		return result;
	}

	private string GetMaterialString()
	{
		return cubeMaterial switch
		{
			CubeMaterial.Dirt => "CubeMaterialDirt".Translate(), 
			CubeMaterial.Stone => "CubeMaterialStone".Translate(), 
			CubeMaterial.Sand => "CubeMaterialSand".Translate(), 
			CubeMaterial.Scrap => "CubeMaterialScrap".Translate(), 
			_ => "CubeMaterialScrap".Translate(), 
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref cubeMaterial, "cubeMaterial", CubeMaterial.Dirt);
		Scribe_Values.Look(ref workDone, "workDone", 0f);
	}
}
