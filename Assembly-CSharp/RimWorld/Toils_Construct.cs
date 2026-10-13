using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001188 RID: 4488
	public static class Toils_Construct
	{
		// Token: 0x06006DCB RID: 28107 RVA: 0x00225298 File Offset: 0x00223498
		public static Toil MakeSolidThingFromBlueprintIfNecessary(TargetIndex blueTarget, TargetIndex targetToUpdate = TargetIndex.None)
		{
			Toil toil = ToilMaker.MakeToil("MakeSolidThingFromBlueprintIfNecessary");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.jobs.curJob;
				Blueprint blueprint = curJob.GetTarget(blueTarget).Thing as Blueprint;
				if (blueprint != null && !blueprint.Destroyed)
				{
					bool flag = targetToUpdate != TargetIndex.None && curJob.GetTarget(targetToUpdate).Thing == blueprint;
					Thing thing;
					bool flag2;
					if (blueprint.TryReplaceWithSolidThing(actor, out thing, out flag2))
					{
						curJob.SetTarget(blueTarget, thing);
						if (flag)
						{
							curJob.SetTarget(targetToUpdate, thing);
						}
						if (thing is Frame)
						{
							actor.Reserve(thing, curJob, 5, 1, null, true, false);
						}
					}
				}
			};
			return toil;
		}

		// Token: 0x06006DCC RID: 28108 RVA: 0x002252E8 File Offset: 0x002234E8
		public static Toil UninstallIfMinifiable(TargetIndex thingInd)
		{
			Toil uninstallIfMinifiable = ToilMaker.MakeToil("UninstallIfMinifiable").FailOnDestroyedNullOrForbidden(thingInd);
			uninstallIfMinifiable.initAction = delegate()
			{
				Pawn actor = uninstallIfMinifiable.actor;
				JobDriver curDriver = actor.jobs.curDriver;
				Thing thing = actor.CurJob.GetTarget(thingInd).Thing;
				if (thing.def.Minifiable)
				{
					curDriver.uninstallWorkLeft = thing.def.building.uninstallWork;
					return;
				}
				curDriver.ReadyForNextToil();
			};
			uninstallIfMinifiable.tickIntervalAction = delegate(int delta)
			{
				Pawn actor = uninstallIfMinifiable.actor;
				JobDriver curDriver = actor.jobs.curDriver;
				Job curJob = actor.CurJob;
				float num = StatDefOf.ConstructionSpeed.Worker.IsDisabledFor(actor) ? 0.1f : actor.GetStatValue(StatDefOf.ConstructionSpeed, true, -1);
				curDriver.uninstallWorkLeft -= num * 1.7f * (float)delta;
				if (curDriver.uninstallWorkLeft <= 0f)
				{
					Thing thing = curJob.GetTarget(thingInd).Thing;
					bool flag = Find.Selector.IsSelected(thing);
					MinifiedThing minifiedThing = thing.MakeMinified(DestroyMode.Vanish);
					Thing thing2 = GenSpawn.Spawn(minifiedThing, thing.Position, uninstallIfMinifiable.actor.Map, WipeMode.Vanish);
					if (flag && thing2 != null)
					{
						Find.Selector.Select(thing2, false, false);
					}
					curJob.SetTarget(thingInd, minifiedThing);
					actor.jobs.curDriver.ReadyForNextToil();
				}
			};
			uninstallIfMinifiable.defaultCompleteMode = ToilCompleteMode.Never;
			uninstallIfMinifiable.WithProgressBar(thingInd, () => 1f - uninstallIfMinifiable.actor.jobs.curDriver.uninstallWorkLeft / uninstallIfMinifiable.actor.CurJob.targetA.Thing.def.building.uninstallWork, false, -0.5f, false);
			return uninstallIfMinifiable;
		}
	}
}
