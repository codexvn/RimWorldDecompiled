using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DFB RID: 15867
	public class SpaceMapParent : MapParent, INameableWorldObject, IResourceWorldObject
	{
		// Token: 0x170035E8 RID: 13800
		// (get) Token: 0x06015A3F RID: 88639 RVA: 0x0067A541 File Offset: 0x00678741
		public override bool GravShipCanLandOn
		{
			get
			{
				return !base.HasMap;
			}
		}

		// Token: 0x170035E9 RID: 13801
		// (get) Token: 0x06015A40 RID: 88640 RVA: 0x0067A54C File Offset: 0x0067874C
		public override string Label
		{
			get
			{
				if (!string.IsNullOrEmpty(this.nameInt))
				{
					return this.nameInt;
				}
				return base.Label;
			}
		}

		// Token: 0x170035EA RID: 13802
		// (get) Token: 0x06015A41 RID: 88641 RVA: 0x0067A568 File Offset: 0x00678768
		// (set) Token: 0x06015A42 RID: 88642 RVA: 0x0067A570 File Offset: 0x00678770
		public string Name
		{
			get
			{
				return this.nameInt;
			}
			set
			{
				this.nameInt = value;
			}
		}

		// Token: 0x170035EB RID: 13803
		// (get) Token: 0x06015A43 RID: 88643 RVA: 0x0067A579 File Offset: 0x00678779
		// (set) Token: 0x06015A44 RID: 88644 RVA: 0x0067A581 File Offset: 0x00678781
		public ThingDef PreciousResource
		{
			get
			{
				return this.preciousResource;
			}
			set
			{
				this.preciousResource = value;
			}
		}

		// Token: 0x170035EC RID: 13804
		// (get) Token: 0x06015A45 RID: 88645 RVA: 0x0067A58A File Offset: 0x0067878A
		public override MapGeneratorDef MapGeneratorDef
		{
			get
			{
				return this.def.mapGenerator ?? MapGeneratorDefOf.Space;
			}
		}

		// Token: 0x06015A46 RID: 88646 RVA: 0x0067A5A0 File Offset: 0x006787A0
		public override IEnumerable<FloatMenuOption> GetTransportersFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetTransportersFloatMenuOptions(pods, launchAction))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			foreach (FloatMenuOption floatMenuOption2 in TransportersArrivalAction_VisitSpace.GetFloatMenuOptions(launchAction, pods, this))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06015A47 RID: 88647 RVA: 0x0067A5BE File Offset: 0x006787BE
		public override IEnumerable<FloatMenuOption> GetShuttleFloatMenuOptions(IEnumerable<IThingHolder> pods, Action<PlanetTile, TransportersArrivalAction> launchAction)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetShuttleFloatMenuOptions(pods, launchAction))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			foreach (FloatMenuOption floatMenuOption2 in TransportersArrivalAction_VisitSpace.GetFloatMenuOptions(launchAction, pods, this))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06015A48 RID: 88648 RVA: 0x0067A5DC File Offset: 0x006787DC
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			alsoRemoveWorldObject = false;
			if (base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				return false;
			}
			foreach (PocketMapParent pocketMapParent in Find.World.pocketMaps.ToList<PocketMapParent>())
			{
				if (pocketMapParent.sourceMap == base.Map && pocketMapParent.Map.mapPawns.AnyPawnBlockingMapRemoval)
				{
					return false;
				}
			}
			if (ModsConfig.OdysseyActive && base.Map.listerThings.AnyThingWithDef(ThingDefOf.GravAnchor))
			{
				return false;
			}
			if (TransporterUtility.IncomingTransporterPreventingMapRemoval(base.Map))
			{
				return false;
			}
			alsoRemoveWorldObject = true;
			return true;
		}

		// Token: 0x06015A49 RID: 88649 RVA: 0x0067A6A4 File Offset: 0x006788A4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.nameInt, "nameInt", null, false);
			Scribe_Defs.Look<ThingDef>(ref this.preciousResource, "preciousResource");
		}

		// Token: 0x0400F06D RID: 61549
		public ThingDef preciousResource;

		// Token: 0x0400F06E RID: 61550
		public string nameInt;
	}
}
