using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C7E RID: 11390
	public class CompFacility : ThingComp
	{
		// Token: 0x1400001C RID: 28
		// (add) Token: 0x0600FFA0 RID: 65440 RVA: 0x004A10C8 File Offset: 0x0049F2C8
		// (remove) Token: 0x0600FFA1 RID: 65441 RVA: 0x004A1100 File Offset: 0x0049F300
		public event Action<CompFacility, Thing> OnLinkAdded;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x0600FFA2 RID: 65442 RVA: 0x004A1138 File Offset: 0x0049F338
		// (remove) Token: 0x0600FFA3 RID: 65443 RVA: 0x004A1170 File Offset: 0x0049F370
		public event Action<CompFacility, Thing> OnLinkRemoved;

		// Token: 0x17002855 RID: 10325
		// (get) Token: 0x0600FFA4 RID: 65444 RVA: 0x004A11A8 File Offset: 0x0049F3A8
		public virtual bool CanBeActive
		{
			get
			{
				CompPowerTrader compPowerTrader = this.parent.TryGetComp<CompPowerTrader>();
				return compPowerTrader == null || compPowerTrader.PowerOn;
			}
		}

		// Token: 0x17002856 RID: 10326
		// (get) Token: 0x0600FFA5 RID: 65445 RVA: 0x004A11CF File Offset: 0x0049F3CF
		public List<Thing> LinkedBuildings
		{
			get
			{
				return this.linkedBuildings;
			}
		}

		// Token: 0x17002857 RID: 10327
		// (get) Token: 0x0600FFA6 RID: 65446 RVA: 0x004A11D7 File Offset: 0x0049F3D7
		public virtual List<StatModifier> StatOffsets
		{
			get
			{
				return this.Props.statOffsets;
			}
		}

		// Token: 0x17002858 RID: 10328
		// (get) Token: 0x0600FFA7 RID: 65447 RVA: 0x004A11E4 File Offset: 0x0049F3E4
		public CompProperties_Facility Props
		{
			get
			{
				return (CompProperties_Facility)this.props;
			}
		}

		// Token: 0x17002859 RID: 10329
		// (get) Token: 0x0600FFA8 RID: 65448 RVA: 0x004A11F1 File Offset: 0x0049F3F1
		protected virtual string MaxConnectedString
		{
			get
			{
				return "FacilityMaxSimultaneousConnections".Translate();
			}
		}

		// Token: 0x0600FFA9 RID: 65449 RVA: 0x004A1204 File Offset: 0x0049F404
		public static void DrawLinesToPotentialThingsToLinkTo(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			CompProperties_Facility compProperties = myDef.GetCompProperties<CompProperties_Facility>();
			if (((compProperties != null) ? compProperties.linkableBuildings : null) == null)
			{
				return;
			}
			Vector3 a = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
			for (int i = 0; i < compProperties.linkableBuildings.Count; i++)
			{
				foreach (Thing thing in map.listerThings.ThingsOfDef(compProperties.linkableBuildings[i]))
				{
					CompAffectedByFacilities compAffectedByFacilities = thing.TryGetComp<CompAffectedByFacilities>();
					if (compAffectedByFacilities != null && compAffectedByFacilities.CanPotentiallyLinkTo(myDef, myPos, myRot))
					{
						GenDraw.DrawLineBetween(a, thing.TrueCenter());
						compAffectedByFacilities.DrawRedLineToPotentiallySupplantedFacility(myDef, myPos, myRot);
					}
				}
			}
		}

		// Token: 0x0600FFAA RID: 65450 RVA: 0x004A12D8 File Offset: 0x0049F4D8
		public static void DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo(float curX, ref float curY, ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			CompFacility.<>c__DisplayClass19_0 CS$<>8__locals1;
			CS$<>8__locals1.curX = curX;
			CompProperties_Facility compProperties = myDef.GetCompProperties<CompProperties_Facility>();
			int num = 0;
			for (int i = 0; i < compProperties.linkableBuildings.Count; i++)
			{
				foreach (Thing thing in map.listerThings.ThingsOfDef(compProperties.linkableBuildings[i]))
				{
					CompAffectedByFacilities compAffectedByFacilities = thing.TryGetComp<CompAffectedByFacilities>();
					if (compAffectedByFacilities != null && compAffectedByFacilities.CanPotentiallyLinkTo(myDef, myPos, myRot))
					{
						num++;
						if (num == 1)
						{
							CompFacility.<DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|19_0(ref curY, "FacilityPotentiallyLinkedTo".Translate() + ":", ref CS$<>8__locals1);
						}
						CompFacility.<DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|19_0(ref curY, "  - " + thing.LabelCap, ref CS$<>8__locals1);
					}
				}
			}
			if (num == 0)
			{
				CompFacility.<DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|19_0(ref curY, "FacilityNoPotentialLinks".Translate(), ref CS$<>8__locals1);
			}
		}

		// Token: 0x0600FFAB RID: 65451 RVA: 0x004A13DC File Offset: 0x0049F5DC
		public override void CompTick()
		{
			base.CompTick();
			if (this.Props.mustBePlacedFacingThingLinear && this.parent.Spawned && this.parent.IsHashIntervalTick(120))
			{
				bool flag = ContainmentUtility.IsLinearBuildingBlocked(this.parent.def, this.parent.Position, this.parent.Rotation, this.parent.Map, null);
				if ((this.linkedBuildings.Any<Thing>() && flag) || (this.linkedBuildings.Empty<Thing>() && !flag))
				{
					this.RelinkAll();
				}
			}
		}

		// Token: 0x0600FFAC RID: 65452 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanLink()
		{
			return true;
		}

		// Token: 0x0600FFAD RID: 65453 RVA: 0x004A1470 File Offset: 0x0049F670
		public void Notify_NewLink(Thing thing)
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				if (this.linkedBuildings[i] == thing)
				{
					Log.Error("Notify_NewLink was called but the link is already here.");
					return;
				}
			}
			this.linkedBuildings.Add(thing);
			Action<CompFacility, Thing> onLinkAdded = this.OnLinkAdded;
			if (onLinkAdded == null)
			{
				return;
			}
			onLinkAdded(this, thing);
		}

		// Token: 0x0600FFAE RID: 65454 RVA: 0x004A14CC File Offset: 0x0049F6CC
		public void Notify_LinkRemoved(Thing thing)
		{
			int i = 0;
			while (i < this.linkedBuildings.Count)
			{
				if (this.linkedBuildings[i] == thing)
				{
					this.linkedBuildings.RemoveAt(i);
					Action<CompFacility, Thing> onLinkRemoved = this.OnLinkRemoved;
					if (onLinkRemoved == null)
					{
						return;
					}
					onLinkRemoved(this, thing);
					return;
				}
				else
				{
					i++;
				}
			}
			Log.Error("Notify_LinkRemoved was called but there is no such link here.");
		}

		// Token: 0x0600FFAF RID: 65455 RVA: 0x004A1527 File Offset: 0x0049F727
		public void Notify_LOSBlockerSpawnedOrDespawned()
		{
			this.RelinkAll();
		}

		// Token: 0x0600FFB0 RID: 65456 RVA: 0x004A1527 File Offset: 0x0049F727
		public void Notify_ThingChanged()
		{
			this.RelinkAll();
		}

		// Token: 0x0600FFB1 RID: 65457 RVA: 0x004A152F File Offset: 0x0049F72F
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.LinkToNearbyBuildings();
		}

		// Token: 0x0600FFB2 RID: 65458 RVA: 0x004A1527 File Offset: 0x0049F727
		public override void PostMapInit()
		{
			this.RelinkAll();
		}

		// Token: 0x0600FFB3 RID: 65459 RVA: 0x004A1538 File Offset: 0x0049F738
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			this.thingsToNotify.Clear();
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				this.thingsToNotify.Add(this.linkedBuildings[i]);
			}
			this.UnlinkAll();
			foreach (Thing thing in this.thingsToNotify)
			{
				thing.TryGetComp<CompAffectedByFacilities>().Notify_FacilityDespawned();
			}
		}

		// Token: 0x0600FFB4 RID: 65460 RVA: 0x004A15CC File Offset: 0x0049F7CC
		public override void PostDrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				if (this.linkedBuildings[i].TryGetComp<CompAffectedByFacilities>().IsFacilityActive(this.parent))
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedBuildings[i].TrueCenter());
				}
				else
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedBuildings[i].TrueCenter(), CompAffectedByFacilities.InactiveFacilityLineMat, 0.2f);
				}
			}
		}

		// Token: 0x0600FFB5 RID: 65461 RVA: 0x004A165C File Offset: 0x0049F85C
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.StatOffsets != null)
			{
				bool flag = this.AmIActiveForAnyone();
				for (int i = 0; i < this.StatOffsets.Count; i++)
				{
					StatDef stat = this.StatOffsets[i].stat;
					stringBuilder.Append(stat.OffsetLabelCap);
					stringBuilder.Append(": ");
					stringBuilder.Append(this.StatOffsets[i].ValueToStringAsOffset);
					if (!flag)
					{
						stringBuilder.Append(" (");
						stringBuilder.Append("InactiveFacility".Translate());
						stringBuilder.Append(")");
					}
					if (i < this.StatOffsets.Count - 1)
					{
						stringBuilder.AppendLine();
					}
				}
				stringBuilder.Append("\n");
			}
			CompProperties_Facility props = this.Props;
			if (props.showMaxSimultaneous)
			{
				stringBuilder.Append(this.MaxConnectedString);
				stringBuilder.Append(": " + props.maxSimultaneous.ToString());
			}
			if (props.mustBePlacedFacingThingLinear && this.parent.Spawned && ContainmentUtility.IsLinearBuildingBlocked(this.parent.def, this.parent.Position, this.parent.Rotation, this.parent.Map, null))
			{
				stringBuilder.AppendInNewLine("FacilityFrontBlocked".Translate());
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600FFB6 RID: 65462 RVA: 0x004A152F File Offset: 0x0049F72F
		private void RelinkAll()
		{
			this.LinkToNearbyBuildings();
		}

		// Token: 0x0600FFB7 RID: 65463 RVA: 0x004A17D8 File Offset: 0x0049F9D8
		private void LinkToNearbyBuildings()
		{
			this.UnlinkAll();
			CompProperties_Facility props = this.Props;
			if (props.linkableBuildings == null)
			{
				return;
			}
			for (int i = 0; i < props.linkableBuildings.Count; i++)
			{
				foreach (Thing thing in this.parent.Map.listerThings.ThingsOfDef(props.linkableBuildings[i]))
				{
					CompAffectedByFacilities compAffectedByFacilities = thing.TryGetComp<CompAffectedByFacilities>();
					if (compAffectedByFacilities != null && compAffectedByFacilities.CanLinkTo(this.parent))
					{
						this.linkedBuildings.Add(thing);
						compAffectedByFacilities.Notify_NewLink(this.parent);
						Action<CompFacility, Thing> onLinkAdded = this.OnLinkAdded;
						if (onLinkAdded != null)
						{
							onLinkAdded(this, compAffectedByFacilities.parent);
						}
					}
				}
			}
		}

		// Token: 0x0600FFB8 RID: 65464 RVA: 0x004A18C0 File Offset: 0x0049FAC0
		private bool AmIActiveForAnyone()
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				if (this.linkedBuildings[i].TryGetComp<CompAffectedByFacilities>().IsFacilityActive(this.parent))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600FFB9 RID: 65465 RVA: 0x004A1904 File Offset: 0x0049FB04
		private void UnlinkAll()
		{
			for (int i = 0; i < this.linkedBuildings.Count; i++)
			{
				this.linkedBuildings[i].TryGetComp<CompAffectedByFacilities>().Notify_LinkRemoved(this.parent);
				Action<CompFacility, Thing> onLinkRemoved = this.OnLinkRemoved;
				if (onLinkRemoved != null)
				{
					onLinkRemoved(this, this.linkedBuildings[i]);
				}
			}
			this.linkedBuildings.Clear();
		}

		// Token: 0x0600FFBB RID: 65467 RVA: 0x004A198C File Offset: 0x0049FB8C
		[CompilerGenerated]
		internal static void <DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|19_0(ref float y, string text, ref CompFacility.<>c__DisplayClass19_0 A_2)
		{
			float lineHeight = Text.LineHeight;
			Widgets.Label(new Rect(A_2.curX, y, 999f, lineHeight), text);
			y += lineHeight;
		}

		// Token: 0x0400A73D RID: 42813
		private List<Thing> linkedBuildings = new List<Thing>();

		// Token: 0x0400A73E RID: 42814
		private const int UpdateRateIntervalTicks = 120;

		// Token: 0x0400A741 RID: 42817
		private HashSet<Thing> thingsToNotify = new HashSet<Thing>();
	}
}
