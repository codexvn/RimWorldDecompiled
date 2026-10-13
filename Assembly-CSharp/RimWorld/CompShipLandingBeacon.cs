using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E0F RID: 11791
	public class CompShipLandingBeacon : ThingComp
	{
		// Token: 0x17002A7D RID: 10877
		// (get) Token: 0x06010993 RID: 67987 RVA: 0x004C85F0 File Offset: 0x004C67F0
		public CompProperties_ShipLandingBeacon Props
		{
			get
			{
				return (CompProperties_ShipLandingBeacon)this.props;
			}
		}

		// Token: 0x17002A7E RID: 10878
		// (get) Token: 0x06010994 RID: 67988 RVA: 0x004C85FD File Offset: 0x004C67FD
		public List<ShipLandingArea> LandingAreas
		{
			get
			{
				return this.landingAreas;
			}
		}

		// Token: 0x17002A7F RID: 10879
		// (get) Token: 0x06010995 RID: 67989 RVA: 0x004C8608 File Offset: 0x004C6808
		public bool Active
		{
			get
			{
				CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
				return comp == null || comp.PowerOn;
			}
		}

		// Token: 0x06010996 RID: 67990 RVA: 0x004C862C File Offset: 0x004C682C
		private bool CanLinkTo(CompShipLandingBeacon other)
		{
			return other != this && ShipLandingBeaconUtility.CanLinkTo(this.parent.Position, other);
		}

		// Token: 0x06010997 RID: 67991 RVA: 0x004C8648 File Offset: 0x004C6848
		public void EstablishConnections()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			List<CompShipLandingBeacon> list = new List<CompShipLandingBeacon>();
			List<CompShipLandingBeacon> list2 = new List<CompShipLandingBeacon>();
			List<Thing> list3 = this.parent.Map.listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon);
			foreach (Thing thing in list3)
			{
				CompShipLandingBeacon compShipLandingBeacon = thing.TryGetComp<CompShipLandingBeacon>();
				if (compShipLandingBeacon != null && this.CanLinkTo(compShipLandingBeacon))
				{
					if (this.parent.Position.x == compShipLandingBeacon.parent.Position.x)
					{
						list2.Add(compShipLandingBeacon);
					}
					else if (this.parent.Position.z == compShipLandingBeacon.parent.Position.z)
					{
						list.Add(compShipLandingBeacon);
					}
				}
			}
			using (List<CompShipLandingBeacon>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CompShipLandingBeacon h = enumerator2.Current;
					using (List<CompShipLandingBeacon>.Enumerator enumerator3 = list2.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							CompShipLandingBeacon v = enumerator3.Current;
							Thing thing2 = list3.FirstOrDefault((Thing x) => x.Position.x == h.parent.Position.x && x.Position.z == v.parent.Position.z);
							if (thing2 != null)
							{
								this.TryAddArea(new ShipLandingArea(CellRect.FromLimits(thing2.Position, this.parent.Position).ContractedBy(1), this.parent.Map)
								{
									beacons = new List<CompShipLandingBeacon>
									{
										this,
										thing2.TryGetComp<CompShipLandingBeacon>(),
										v,
										h
									}
								});
							}
						}
					}
				}
			}
			for (int i = this.landingAreas.Count - 1; i >= 0; i--)
			{
				using (List<CompShipLandingBeacon>.Enumerator enumerator2 = this.landingAreas[i].beacons.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (!enumerator2.Current.TryAddArea(this.landingAreas[i]))
						{
							this.RemoveArea(this.landingAreas[i]);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06010998 RID: 67992 RVA: 0x004C8920 File Offset: 0x004C6B20
		private void RemoveArea(ShipLandingArea area)
		{
			foreach (CompShipLandingBeacon compShipLandingBeacon in area.beacons)
			{
				if (compShipLandingBeacon.landingAreas.Contains(area))
				{
					compShipLandingBeacon.landingAreas.Remove(area);
				}
			}
			this.landingAreas.Remove(area);
		}

		// Token: 0x06010999 RID: 67993 RVA: 0x004C8994 File Offset: 0x004C6B94
		public bool TryAddArea(ShipLandingArea newArea)
		{
			if (!this.landingAreas.Contains(newArea))
			{
				for (int i = this.landingAreas.Count - 1; i >= 0; i--)
				{
					if (this.landingAreas[i].MyRect.Overlaps(newArea.MyRect) && this.landingAreas[i].MyRect != newArea.MyRect)
					{
						if (this.landingAreas[i].MyRect.Area <= newArea.MyRect.Area)
						{
							return false;
						}
						this.RemoveArea(this.landingAreas[i]);
					}
				}
				this.landingAreas.Add(newArea);
			}
			return true;
		}

		// Token: 0x0601099A RID: 67994 RVA: 0x004C8A5C File Offset: 0x004C6C5C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			CompGlower compGlower = this.parent.TryGetComp<CompGlower>();
			if (compGlower != null)
			{
				this.fieldColor = compGlower.GlowColor.ToColor.ToOpaque();
			}
			this.EstablishConnections();
			foreach (ShipLandingArea shipLandingArea in this.landingAreas)
			{
				shipLandingArea.RecalculateBlockingThing();
			}
		}

		// Token: 0x0601099B RID: 67995 RVA: 0x004C8ADC File Offset: 0x004C6CDC
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			for (int i = this.landingAreas.Count - 1; i >= 0; i--)
			{
				this.RemoveArea(this.landingAreas[i]);
			}
			foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon))
			{
				CompShipLandingBeacon compShipLandingBeacon = thing.TryGetComp<CompShipLandingBeacon>();
				if (compShipLandingBeacon != null)
				{
					compShipLandingBeacon.EstablishConnections();
				}
			}
		}

		// Token: 0x0601099C RID: 67996 RVA: 0x004C8B6C File Offset: 0x004C6D6C
		public override void CompTickRare()
		{
			foreach (ShipLandingArea shipLandingArea in this.landingAreas)
			{
				shipLandingArea.RecalculateBlockingThing();
			}
		}

		// Token: 0x0601099D RID: 67997 RVA: 0x004C8BBC File Offset: 0x004C6DBC
		public override void PostDrawExtraSelectionOverlays()
		{
			foreach (ShipLandingArea shipLandingArea in this.landingAreas)
			{
				if (shipLandingArea.Active)
				{
					Color color = shipLandingArea.Clear ? this.fieldColor : Color.red;
					color.a = Pulser.PulseBrightness(1f, 0.6f);
					GenDraw.DrawFieldEdges(shipLandingArea.MyRect.ToList<IntVec3>(), color, null, null, 2900);
				}
				foreach (CompShipLandingBeacon compShipLandingBeacon in shipLandingArea.beacons)
				{
					if (this.CanLinkTo(compShipLandingBeacon))
					{
						GenDraw.DrawLineBetween(this.parent.TrueCenter(), compShipLandingBeacon.parent.TrueCenter(), SimpleColor.White, 0.2f);
					}
				}
			}
		}

		// Token: 0x0601099E RID: 67998 RVA: 0x004C8CD0 File Offset: 0x004C6ED0
		public override string CompInspectStringExtra()
		{
			if (!this.parent.Spawned)
			{
				return null;
			}
			string text = "";
			if (!this.Active)
			{
				text += "NotUsable".Translate() + ": " + "Unpowered".Translate().CapitalizeFirst();
			}
			int i = 0;
			while (i < this.landingAreas.Count)
			{
				if (!this.landingAreas[i].Clear)
				{
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += "NotUsable".Translate() + ": ";
					if (this.landingAreas[i].BlockedByRoof)
					{
						text += "BlockedByRoof".Translate().CapitalizeFirst();
						break;
					}
					if (this.landingAreas[i].BlockedByTerrainAffordance)
					{
						text += "BlockedByTerrain".Translate(this.Props.landingAreaTerrainSupport);
						break;
					}
					text += "BlockedBy".Translate(this.landingAreas[i].FirstBlockingThing).CapitalizeFirst();
					break;
				}
				else
				{
					i++;
				}
			}
			foreach (Thing thing in this.parent.Map.listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon))
			{
				if (thing != this.parent && ShipLandingBeaconUtility.AlignedDistanceTooShort(this.parent.Position, thing.Position, this.Props.edgeLengthRange.min - 1f))
				{
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += "NotUsable".Translate() + ": " + "TooCloseToOtherBeacon".Translate().CapitalizeFirst();
					break;
				}
			}
			return text;
		}

		// Token: 0x0400ACF7 RID: 44279
		private List<ShipLandingArea> landingAreas = new List<ShipLandingArea>();

		// Token: 0x0400ACF8 RID: 44280
		private Color fieldColor = Color.white;
	}
}
