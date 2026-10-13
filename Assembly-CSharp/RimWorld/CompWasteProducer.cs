using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002ED2 RID: 11986
	public class CompWasteProducer : ThingComp
	{
		// Token: 0x17002BA2 RID: 11170
		// (get) Token: 0x06010EC2 RID: 69314 RVA: 0x004DEF41 File Offset: 0x004DD141
		private CompProperties_WasteProducer Props
		{
			get
			{
				return (CompProperties_WasteProducer)this.props;
			}
		}

		// Token: 0x17002BA3 RID: 11171
		// (get) Token: 0x06010EC3 RID: 69315 RVA: 0x004DEF4E File Offset: 0x004DD14E
		public Thing Waste
		{
			get
			{
				return this.parent.TryGetInnerInteractableThingOwner().FirstOrFallback((Thing t) => t.def == ThingDefOf.Wastepack, null);
			}
		}

		// Token: 0x17002BA4 RID: 11172
		// (get) Token: 0x06010EC4 RID: 69316 RVA: 0x004DEF80 File Offset: 0x004DD180
		public bool CanEmptyNow
		{
			get
			{
				Building_MechGestator building_MechGestator = this.parent as Building_MechGestator;
				return (building_MechGestator == null || building_MechGestator.GestatingMech == null) && this.Waste != null;
			}
		}

		// Token: 0x06010EC5 RID: 69317 RVA: 0x004DEFAF File Offset: 0x004DD1AF
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotech("Waste producer"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x06010EC6 RID: 69318 RVA: 0x004DEFD4 File Offset: 0x004DD1D4
		public void ProduceWaste(int amountToMake)
		{
			int num = Mathf.CeilToInt((float)amountToMake / (float)ThingDefOf.Wastepack.stackLimit);
			for (int i = 0; i < num; i++)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Wastepack, null);
				thing.stackCount = Mathf.Min(amountToMake, ThingDefOf.Wastepack.stackLimit);
				if (!this.parent.TryGetInnerInteractableThingOwner().TryAdd(thing, true))
				{
					break;
				}
				amountToMake -= thing.stackCount;
			}
		}

		// Token: 0x06010EC7 RID: 69319 RVA: 0x004DF044 File Offset: 0x004DD244
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (this.Props.showContentsInInspectPane && this.Waste != null)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				int num = 0;
				foreach (Thing thing in ((IEnumerable<Thing>)this.parent.TryGetInnerInteractableThingOwner()))
				{
					if (thing.def == ThingDefOf.Wastepack)
					{
						num += thing.stackCount;
					}
				}
				text = text + ("Contents".Translate() + ": " + this.Waste.LabelCapNoCount + " x") + num.ToString();
			}
			return text;
		}
	}
}
