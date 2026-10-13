using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200294A RID: 10570
	public abstract class Blueprint : ThingWithComps, IConstructible
	{
		// Token: 0x1700232D RID: 9005
		// (get) Token: 0x0600E78D RID: 59277 RVA: 0x0043789C File Offset: 0x00435A9C
		public override string Label
		{
			get
			{
				return this.def.entityDefToBuild.label + "BlueprintLabelExtra".Translate();
			}
		}

		// Token: 0x1700232E RID: 9006
		// (get) Token: 0x0600E78E RID: 59278
		protected abstract float WorkTotal { get; }

		// Token: 0x1700232F RID: 9007
		// (get) Token: 0x0600E78F RID: 59279 RVA: 0x004378C4 File Offset: 0x00435AC4
		public override Graphic Graphic
		{
			get
			{
				ThingStyleDef styleDef = this.StyleDef;
				if (((styleDef != null) ? styleDef.blueprintGraphicData : null) != null)
				{
					if (this.styleGraphicInt == null)
					{
						this.styleGraphicInt = styleDef.blueprintGraphicData.Graphic;
					}
					return this.styleGraphicInt;
				}
				return base.Graphic;
			}
		}

		// Token: 0x0600E790 RID: 59280 RVA: 0x0043790C File Offset: 0x00435B0C
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Gizmo selectMonumentMarkerGizmo = QuestUtility.GetSelectMonumentMarkerGizmo(this);
			if (selectMonumentMarkerGizmo != null)
			{
				yield return selectMonumentMarkerGizmo;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600E791 RID: 59281 RVA: 0x0043791C File Offset: 0x00435B1C
		public virtual bool TryReplaceWithSolidThing(Pawn workerPawn, out Thing createdThing, out bool jobEnded)
		{
			bool flag = Find.Selector.IsSelected(this);
			jobEnded = false;
			if (GenConstruct.FirstBlockingThing(this, workerPawn) != null)
			{
				workerPawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				jobEnded = true;
				createdThing = null;
				return false;
			}
			bool flag2;
			createdThing = this.MakeSolidThing(out flag2);
			Map map = base.Map;
			if (this.Graphic is Graphic_Random)
			{
				createdThing.overrideGraphicIndex = new int?(this.thingIDNumber);
			}
			GenSpawn.WipeExistingThings(base.Position, base.Rotation, createdThing.def, map, DestroyMode.Deconstruct);
			if (createdThing is Plant)
			{
				Plant plant = base.Position.GetPlant(base.Map);
				if (plant != null)
				{
					plant.Destroy(DestroyMode.Vanish);
				}
			}
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			if (createdThing.def.CanHaveFaction)
			{
				createdThing.SetFactionDirect(workerPawn.Faction);
			}
			Thing thing = GenSpawn.Spawn(createdThing, base.Position, map, base.Rotation, WipeMode.Vanish, false, false);
			if ((flag2 || flag) && thing != null)
			{
				Find.Selector.Select(thing, false, false);
			}
			foreach (Pawn pawn in map.mapPawns.AllPawnsSpawned)
			{
				pawn.pather.NotifyThingTransformed(this, thing);
			}
			foreach (IntVec3 intVec in thing.OccupiedRect().Cells)
			{
				Plan plan;
				if (map.planManager.TryGetPlan(intVec, out plan))
				{
					plan.RemoveCell(intVec);
				}
			}
			return true;
		}

		// Token: 0x0600E792 RID: 59282 RVA: 0x00437AD0 File Offset: 0x00435CD0
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (WorldComponent_GravshipController.CutsceneInProgress && !WorldComponent_GravshipController.GravshipRenderInProgess)
			{
				return;
			}
			base.DrawAt(drawLoc, flip);
		}

		// Token: 0x0600E793 RID: 59283 RVA: 0x00437AE9 File Offset: 0x00435CE9
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			map.blueprintGrid.Register(this);
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x0600E794 RID: 59284 RVA: 0x00437AFF File Offset: 0x00435CFF
		public void InheritStyle(Precept_ThingStyle styleSource, ThingStyleDef styleDef)
		{
			base.StyleSourcePrecept = styleSource;
			if (styleSource == null)
			{
				this.StyleDef = styleDef;
			}
		}

		// Token: 0x0600E795 RID: 59285 RVA: 0x00437B12 File Offset: 0x00435D12
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			map.blueprintGrid.DeRegister(this);
		}

		// Token: 0x0600E796 RID: 59286
		protected abstract Thing MakeSolidThing(out bool shouldSelect);

		// Token: 0x0600E797 RID: 59287
		public abstract List<ThingDefCountClass> TotalMaterialCost();

		// Token: 0x0600E798 RID: 59288 RVA: 0x00002501 File Offset: 0x00000701
		public bool IsCompleted()
		{
			return false;
		}

		// Token: 0x0600E799 RID: 59289 RVA: 0x00437B2C File Offset: 0x00435D2C
		public int ThingCountNeeded(ThingDef stuff)
		{
			foreach (ThingDefCountClass thingDefCountClass in this.TotalMaterialCost())
			{
				if (thingDefCountClass.thingDef == stuff)
				{
					return thingDefCountClass.count;
				}
			}
			return 0;
		}

		// Token: 0x0600E79A RID: 59290 RVA: 0x00437B90 File Offset: 0x00435D90
		public int SpaceRemainingFor(ThingDef stuff)
		{
			return this.ThingCountNeeded(stuff);
		}

		// Token: 0x0600E79B RID: 59291
		public abstract BuildableDef EntityToBuild();

		// Token: 0x0600E79C RID: 59292
		public abstract ThingStyleDef EntityToBuildStyle();

		// Token: 0x0600E79D RID: 59293
		public abstract ThingDef EntityToBuildStuff();

		// Token: 0x0600E79E RID: 59294 RVA: 0x00437B9C File Offset: 0x00435D9C
		public Thing BlockingHaulableOnTop()
		{
			if (this.def.entityDefToBuild.passability == Traversability.Standable)
			{
				return null;
			}
			foreach (IntVec3 c in this.OccupiedRect())
			{
				List<Thing> thingList = c.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (thing.def.EverHaulable)
					{
						return thing;
					}
				}
			}
			return null;
		}

		// Token: 0x0600E79F RID: 59295 RVA: 0x00437C3C File Offset: 0x00435E3C
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			stringBuilder.AppendLineIfNotEmpty();
			float workTotal = this.WorkTotal;
			if (!Mathf.Approximately(workTotal, -1f))
			{
				stringBuilder.Append("WorkLeft".Translate() + ": " + workTotal.ToStringWorkAmount());
			}
			return stringBuilder.ToString();
		}
	}
}
