using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002046 RID: 8262
	public abstract class GenStep_Ambush : GenStep
	{
		// Token: 0x0600B580 RID: 46464 RVA: 0x00342C08 File Offset: 0x00340E08
		public override void Generate(Map map, GenStepParams parms)
		{
			CellRect rectToDefend;
			IntVec3 root;
			if (!SiteGenStepUtility.TryFindRootToSpawnAroundRectOfInterest(out rectToDefend, out root, map))
			{
				return;
			}
			this.SpawnTrigger(rectToDefend, root, map, parms);
		}

		// Token: 0x0600B581 RID: 46465 RVA: 0x00342C2C File Offset: 0x00340E2C
		private void SpawnTrigger(CellRect rectToDefend, IntVec3 root, Map map, GenStepParams parms)
		{
			string signalTag = "ambushActivated-" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			CellRect rect;
			if (root.IsValid)
			{
				rect = CellRect.CenteredOn(root, 17);
			}
			else
			{
				rect = rectToDefend.ExpandedBy(12);
			}
			SignalAction_Ambush signalAction_Ambush = this.MakeAmbushSignalAction(rectToDefend, root, parms);
			signalAction_Ambush.signalTag = signalTag;
			GenSpawn.Spawn(signalAction_Ambush, rect.CenterCell, map, WipeMode.Vanish);
			RectTrigger rectTrigger = this.MakeRectTrigger();
			rectTrigger.signalTag = signalTag;
			rectTrigger.Rect = rect;
			GenSpawn.Spawn(rectTrigger, rect.CenterCell, map, WipeMode.Vanish);
			TriggerUnfogged triggerUnfogged = (TriggerUnfogged)ThingMaker.MakeThing(ThingDefOf.TriggerUnfogged, null);
			triggerUnfogged.signalTag = signalTag;
			GenSpawn.Spawn(triggerUnfogged, rect.CenterCell, map, WipeMode.Vanish);
		}

		// Token: 0x0600B582 RID: 46466 RVA: 0x00342CDD File Offset: 0x00340EDD
		protected virtual RectTrigger MakeRectTrigger()
		{
			return (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
		}

		// Token: 0x0600B583 RID: 46467 RVA: 0x00342CF0 File Offset: 0x00340EF0
		protected virtual SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root, GenStepParams parms)
		{
			SignalAction_Ambush signalAction_Ambush = (SignalAction_Ambush)ThingMaker.MakeThing(ThingDefOf.SignalAction_Ambush, null);
			if (parms.sitePart != null)
			{
				signalAction_Ambush.points = parms.sitePart.parms.threatPoints;
			}
			else
			{
				signalAction_Ambush.points = this.defaultPointsRange.RandomInRange;
			}
			int num = Rand.RangeInclusive(0, 2);
			if (num == 0)
			{
				signalAction_Ambush.ambushType = SignalActionAmbushType.Manhunters;
			}
			else if (num == 1 && PawnGroupMakerUtility.CanGenerateAnyNormalGroup(Faction.OfMechanoids, signalAction_Ambush.points))
			{
				signalAction_Ambush.ambushType = SignalActionAmbushType.Mechanoids;
			}
			else
			{
				signalAction_Ambush.ambushType = SignalActionAmbushType.Normal;
			}
			return signalAction_Ambush;
		}

		// Token: 0x04007C9F RID: 31903
		public FloatRange defaultPointsRange = new FloatRange(180f, 340f);
	}
}
