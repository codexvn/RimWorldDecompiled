using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BC4 RID: 11204
	[StaticConstructorOnStartup]
	public class CompAffectedByFacilities : ThingComp
	{
		// Token: 0x17002748 RID: 10056
		// (get) Token: 0x0600FA97 RID: 64151 RVA: 0x0048CFC0 File Offset: 0x0048B1C0
		public List<Thing> LinkedFacilitiesListForReading
		{
			get
			{
				return this.linkedFacilities;
			}
		}

		// Token: 0x0600FA98 RID: 64152 RVA: 0x0048CFC8 File Offset: 0x0048B1C8
		public bool CanLinkTo(Thing facility)
		{
			CompFacility compFacility;
			if (!facility.TryGetComp(out compFacility))
			{
				return false;
			}
			if (!compFacility.CanLink())
			{
				return false;
			}
			if (!this.CanPotentiallyLinkTo(facility.def, facility.Position, facility.Rotation))
			{
				return false;
			}
			if (!this.IsValidFacilityForMe(facility))
			{
				return false;
			}
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i] == facility)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600FA99 RID: 64153 RVA: 0x0048D03A File Offset: 0x0048B23A
		public static bool CanPotentiallyLinkTo_Static(Thing facility, ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map myMap)
		{
			return CompAffectedByFacilities.CanPotentiallyLinkTo_Static(facility.def, facility.Position, facility.Rotation, myDef, myPos, myRot, myMap) && CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility, myDef, myPos, myRot);
		}

		// Token: 0x0600FA9A RID: 64154 RVA: 0x0048D06C File Offset: 0x0048B26C
		public bool CanPotentiallyLinkTo(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			if (!CompAffectedByFacilities.CanPotentiallyLinkTo_Static(facilityDef, facilityPos, facilityRot, this.parent.def, this.parent.Position, this.parent.Rotation, this.parent.Map))
			{
				return false;
			}
			if (!this.IsPotentiallyValidFacilityForMe(facilityDef, facilityPos, facilityRot))
			{
				return false;
			}
			int num = 0;
			bool flag = false;
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i].def == facilityDef)
				{
					num++;
					if (this.IsBetter(facilityDef, facilityPos, facilityRot, this.linkedFacilities[i]))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				return true;
			}
			CompProperties_Facility compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
			return num + 1 <= compProperties.maxSimultaneous;
		}

		// Token: 0x0600FA9B RID: 64155 RVA: 0x0048D128 File Offset: 0x0048B328
		public static bool CanPotentiallyLinkTo_Static(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot, ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map myMap)
		{
			CompProperties_Facility compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
			if (compProperties.mustBePlacedAdjacent)
			{
				CellRect rect = GenAdj.OccupiedRect(myPos, myRot, myDef.size);
				CellRect rect2 = GenAdj.OccupiedRect(facilityPos, facilityRot, facilityDef.size);
				if (!GenAdj.AdjacentTo8WayOrInside(rect, rect2))
				{
					return false;
				}
			}
			if (!compProperties.mustBePlacedFacingThingLinear)
			{
				if (compProperties.mustBePlacedAdjacentCardinalToBedHead || compProperties.mustBePlacedAdjacentCardinalToAndFacingBedHead)
				{
					if (!myDef.IsBed)
					{
						return false;
					}
					CellRect other = GenAdj.OccupiedRect(facilityPos, facilityRot, facilityDef.size);
					bool flag = false;
					int sleepingSlotsCount = BedUtility.GetSleepingSlotsCount(myDef.size);
					for (int i = 0; i < sleepingSlotsCount; i++)
					{
						IntVec3 sleepingSlotPos = BedUtility.GetSleepingSlotPos(i, myPos, myRot, myDef.size);
						if (sleepingSlotPos.IsAdjacentToCardinalOrInside(other))
						{
							if (compProperties.mustBePlacedAdjacentCardinalToAndFacingBedHead)
							{
								if (other.MovedBy(facilityRot.FacingCell).Contains(sleepingSlotPos))
								{
									flag = true;
								}
							}
							else
							{
								flag = true;
							}
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				if (!compProperties.mustBePlacedAdjacent && !compProperties.mustBePlacedAdjacentCardinalToBedHead && !compProperties.mustBePlacedAdjacentCardinalToAndFacingBedHead)
				{
					Vector3 a = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
					Vector3 b = GenThing.TrueCenter(facilityPos, facilityRot, facilityDef.size, facilityDef.Altitude);
					float num = Vector3.Distance(a, b);
					if (num > compProperties.maxDistance || (compProperties.minDistance > 0f && num < compProperties.minDistance))
					{
						return false;
					}
				}
				return true;
			}
			if (ContainmentUtility.IsLinearBuildingBlocked(facilityDef, facilityPos, facilityRot, myMap, null))
			{
				return false;
			}
			CellRect cellRect = GenAdj.OccupiedRect(myPos, myRot, myDef.size);
			foreach (IntVec3 value in ContainmentUtility.GetInhibitorAffectedCells(facilityDef, facilityPos, facilityRot, myMap))
			{
				if (cellRect.Cells.Contains(value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600FA9C RID: 64156 RVA: 0x0048D2F8 File Offset: 0x0048B4F8
		public bool IsValidFacilityForMe(Thing facility)
		{
			return CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility, this.parent.def, this.parent.Position, this.parent.Rotation);
		}

		// Token: 0x0600FA9D RID: 64157 RVA: 0x0048D328 File Offset: 0x0048B528
		private bool IsPotentiallyValidFacilityForMe(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			if (!CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facilityDef, facilityPos, facilityRot, this.parent.def, this.parent.Position, this.parent.Rotation, this.parent.Map))
			{
				return false;
			}
			if (facilityDef.GetCompProperties<CompProperties_Facility>().canLinkToMedBedsOnly)
			{
				Building_Bed building_Bed = this.parent as Building_Bed;
				if (building_Bed == null || !building_Bed.Medical)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600FA9E RID: 64158 RVA: 0x0048D394 File Offset: 0x0048B594
		private static bool IsPotentiallyValidFacilityForMe_Static(Thing facility, ThingDef myDef, IntVec3 myPos, Rot4 myRot)
		{
			return CompAffectedByFacilities.IsPotentiallyValidFacilityForMe_Static(facility.def, facility.Position, facility.Rotation, myDef, myPos, myRot, facility.Map);
		}

		// Token: 0x0600FA9F RID: 64159 RVA: 0x0048D3B8 File Offset: 0x0048B5B8
		private static bool IsPotentiallyValidFacilityForMe_Static(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot, ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			if (!facilityDef.GetCompProperties<CompProperties_Facility>().requiresLOS)
			{
				return true;
			}
			CellRect cellRect = GenAdj.OccupiedRect(myPos, myRot, myDef.size);
			CellRect cellRect2 = GenAdj.OccupiedRect(facilityPos, facilityRot, facilityDef.size);
			bool result = false;
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					for (int k = cellRect2.minZ; k <= cellRect2.maxZ; k++)
					{
						for (int l = cellRect2.minX; l <= cellRect2.maxX; l++)
						{
							IntVec3 start = new IntVec3(j, 0, i);
							IntVec3 end = new IntVec3(l, 0, k);
							if (GenSight.LineOfSight(start, end, map, cellRect, cellRect2, null, false))
							{
								return true;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600FAA0 RID: 64160 RVA: 0x0048D48C File Offset: 0x0048B68C
		public void Notify_NewLink(Thing facility)
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i] == facility)
				{
					Log.Error("Notify_NewLink was called but the link is already here.");
					return;
				}
			}
			Thing potentiallySupplantedFacility = this.GetPotentiallySupplantedFacility(facility.def, facility.Position, facility.Rotation);
			if (potentiallySupplantedFacility != null)
			{
				potentiallySupplantedFacility.TryGetComp<CompFacility>().Notify_LinkRemoved(this.parent);
				this.linkedFacilities.Remove(potentiallySupplantedFacility);
			}
			this.linkedFacilities.Add(facility);
		}

		// Token: 0x0600FAA1 RID: 64161 RVA: 0x0048D510 File Offset: 0x0048B710
		public void Notify_LinkRemoved(Thing thing)
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i] == thing)
				{
					this.linkedFacilities.RemoveAt(i);
					return;
				}
			}
			Log.Error("Notify_LinkRemoved was called but there is no such link here.");
		}

		// Token: 0x0600FAA2 RID: 64162 RVA: 0x0048D559 File Offset: 0x0048B759
		public void Notify_FacilityDespawned()
		{
			this.RelinkAll();
		}

		// Token: 0x0600FAA3 RID: 64163 RVA: 0x0048D559 File Offset: 0x0048B759
		public void Notify_LOSBlockerSpawnedOrDespawned()
		{
			this.RelinkAll();
		}

		// Token: 0x0600FAA4 RID: 64164 RVA: 0x0048D559 File Offset: 0x0048B759
		public void Notify_ThingChanged()
		{
			this.RelinkAll();
		}

		// Token: 0x0600FAA5 RID: 64165 RVA: 0x0048D561 File Offset: 0x0048B761
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.LinkToNearbyFacilities();
		}

		// Token: 0x0600FAA6 RID: 64166 RVA: 0x0048D569 File Offset: 0x0048B769
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			this.UnlinkAll();
		}

		// Token: 0x0600FAA7 RID: 64167 RVA: 0x0048D574 File Offset: 0x0048B774
		public override void PostDrawExtraSelectionOverlays()
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.IsFacilityActive(this.linkedFacilities[i]))
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedFacilities[i].TrueCenter());
				}
				else
				{
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), this.linkedFacilities[i].TrueCenter(), CompAffectedByFacilities.InactiveFacilityLineMat, 0.2f);
				}
			}
		}

		// Token: 0x0600FAA8 RID: 64168 RVA: 0x0048D5FC File Offset: 0x0048B7FC
		private bool IsBetter(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot, Thing thanThisFacility)
		{
			if (facilityDef != thanThisFacility.def)
			{
				Log.Error("Comparing two different facility defs.");
				return false;
			}
			Vector3 b = GenThing.TrueCenter(facilityPos, facilityRot, facilityDef.size, facilityDef.Altitude);
			Vector3 a = this.parent.TrueCenter();
			float num = Vector3.Distance(a, b);
			float num2 = Vector3.Distance(a, thanThisFacility.TrueCenter());
			if (num != num2)
			{
				return num < num2;
			}
			if (facilityPos.x != thanThisFacility.Position.x)
			{
				return facilityPos.x < thanThisFacility.Position.x;
			}
			return facilityPos.z < thanThisFacility.Position.z;
		}

		// Token: 0x17002749 RID: 10057
		// (get) Token: 0x0600FAA9 RID: 64169 RVA: 0x0048D699 File Offset: 0x0048B899
		private IEnumerable<Thing> ThingsICanLinkTo
		{
			get
			{
				if (!this.parent.Spawned)
				{
					yield break;
				}
				IEnumerable<Thing> enumerable = CompAffectedByFacilities.PotentialThingsToLinkTo(this.parent.def, this.parent.Position, this.parent.Rotation, this.parent.Map);
				foreach (Thing thing in enumerable)
				{
					if (this.CanLinkTo(thing))
					{
						yield return thing;
					}
				}
				IEnumerator<Thing> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x0600FAAA RID: 64170 RVA: 0x0048D6A9 File Offset: 0x0048B8A9
		public static IEnumerable<Thing> PotentialThingsToLinkTo(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map myMap)
		{
			CompAffectedByFacilities.alreadyReturnedCount.Clear();
			CompProperties_AffectedByFacilities compProperties = myDef.GetCompProperties<CompProperties_AffectedByFacilities>();
			if (compProperties.linkableFacilities == null)
			{
				yield break;
			}
			IEnumerable<Thing> enumerable = Enumerable.Empty<Thing>();
			for (int i = 0; i < compProperties.linkableFacilities.Count; i++)
			{
				enumerable = enumerable.Concat(myMap.listerThings.ThingsOfDef(compProperties.linkableFacilities[i]));
			}
			Vector3 myTrueCenter = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
			IOrderedEnumerable<Thing> orderedEnumerable = from x in enumerable
			orderby Vector3.Distance(myTrueCenter, x.TrueCenter()), x.Position.x, x.Position.z
			select x;
			foreach (Thing thing in orderedEnumerable)
			{
				CompFacility compFacility;
				if (thing.TryGetComp(out compFacility) && compFacility.CanLink() && CompAffectedByFacilities.CanPotentiallyLinkTo_Static(thing, myDef, myPos, myRot, myMap))
				{
					int num;
					if (CompAffectedByFacilities.alreadyReturnedCount.TryGetValue(thing.def, out num))
					{
						if (num >= compFacility.Props.maxSimultaneous)
						{
							continue;
						}
					}
					else
					{
						CompAffectedByFacilities.alreadyReturnedCount.Add(thing.def, 0);
					}
					Dictionary<ThingDef, int> dictionary = CompAffectedByFacilities.alreadyReturnedCount;
					ThingDef def = thing.def;
					int num2 = dictionary[def];
					dictionary[def] = num2 + 1;
					yield return thing;
				}
			}
			IEnumerator<Thing> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600FAAB RID: 64171 RVA: 0x0048D6D0 File Offset: 0x0048B8D0
		public static void DrawLinesToPotentialThingsToLinkTo(ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			Vector3 a = GenThing.TrueCenter(myPos, myRot, myDef.size, myDef.Altitude);
			foreach (Thing t in CompAffectedByFacilities.PotentialThingsToLinkTo(myDef, myPos, myRot, map))
			{
				GenDraw.DrawLineBetween(a, t.TrueCenter());
			}
		}

		// Token: 0x0600FAAC RID: 64172 RVA: 0x0048D73C File Offset: 0x0048B93C
		public static void DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo(float curX, ref float curY, ThingDef myDef, IntVec3 myPos, Rot4 myRot, Map map)
		{
			CompAffectedByFacilities.<>c__DisplayClass26_0 CS$<>8__locals1;
			CS$<>8__locals1.curX = curX;
			int num = 0;
			foreach (Thing thing in CompAffectedByFacilities.PotentialThingsToLinkTo(myDef, myPos, myRot, map))
			{
				num++;
				if (num == 1)
				{
					CompAffectedByFacilities.<DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|26_0(ref curY, "FacilityPotentiallyLinkedTo".Translate() + ":", ref CS$<>8__locals1);
				}
				CompAffectedByFacilities.<DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|26_0(ref curY, "  - " + thing.LabelCap, ref CS$<>8__locals1);
			}
		}

		// Token: 0x0600FAAD RID: 64173 RVA: 0x0048D7D4 File Offset: 0x0048B9D4
		public void DrawRedLineToPotentiallySupplantedFacility(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			Thing potentiallySupplantedFacility = this.GetPotentiallySupplantedFacility(facilityDef, facilityPos, facilityRot);
			if (potentiallySupplantedFacility != null)
			{
				GenDraw.DrawLineBetween(this.parent.TrueCenter(), potentiallySupplantedFacility.TrueCenter(), CompAffectedByFacilities.InactiveFacilityLineMat, 0.2f);
			}
		}

		// Token: 0x0600FAAE RID: 64174 RVA: 0x0048D810 File Offset: 0x0048BA10
		private Thing GetPotentiallySupplantedFacility(ThingDef facilityDef, IntVec3 facilityPos, Rot4 facilityRot)
		{
			Thing thing = null;
			int num = 0;
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				if (this.linkedFacilities[i].def == facilityDef)
				{
					if (thing == null)
					{
						thing = this.linkedFacilities[i];
					}
					num++;
				}
			}
			if (num == 0)
			{
				return null;
			}
			CompProperties_Facility compProperties = facilityDef.GetCompProperties<CompProperties_Facility>();
			if (num + 1 <= compProperties.maxSimultaneous)
			{
				return null;
			}
			Thing thing2 = thing;
			for (int j = 0; j < this.linkedFacilities.Count; j++)
			{
				if (facilityDef == this.linkedFacilities[j].def && this.IsBetter(thing2.def, thing2.Position, thing2.Rotation, this.linkedFacilities[j]))
				{
					thing2 = this.linkedFacilities[j];
				}
			}
			return thing2;
		}

		// Token: 0x0600FAAF RID: 64175 RVA: 0x0048D8E8 File Offset: 0x0048BAE8
		public override float GetStatOffset(StatDef stat)
		{
			float num = 0f;
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				CompFacility compFacility = this.linkedFacilities[i].TryGetComp<CompFacility>();
				if (compFacility.StatOffsets != null)
				{
					float statOffsetFromList = compFacility.StatOffsets.GetStatOffsetFromList(stat);
					if (statOffsetFromList != 0f && this.IsFacilityActive(this.linkedFacilities[i]))
					{
						num += statOffsetFromList;
					}
				}
			}
			return num;
		}

		// Token: 0x0600FAB0 RID: 64176 RVA: 0x0048D95C File Offset: 0x0048BB5C
		public override void GetStatsExplanation(StatDef stat, StringBuilder sb, string whitespace = "")
		{
			this.alreadyUsed.Clear();
			bool flag = false;
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				bool flag2 = false;
				for (int j = 0; j < this.alreadyUsed.Count; j++)
				{
					if (this.alreadyUsed[j] == this.linkedFacilities[i].def)
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2 && this.IsFacilityActive(this.linkedFacilities[i]))
				{
					CompFacility compFacility = this.linkedFacilities[i].TryGetComp<CompFacility>();
					if (compFacility.StatOffsets != null)
					{
						float num = compFacility.StatOffsets.GetStatOffsetFromList(stat);
						if (num != 0f)
						{
							if (!flag)
							{
								flag = true;
								sb.AppendLine();
								sb.AppendLine(whitespace + "StatsReport_Facilities".Translate() + ":");
							}
							int num2 = 0;
							for (int k = 0; k < this.linkedFacilities.Count; k++)
							{
								if (this.IsFacilityActive(this.linkedFacilities[k]) && this.linkedFacilities[k].def == this.linkedFacilities[i].def)
								{
									num2++;
								}
							}
							num *= (float)num2;
							sb.Append(whitespace + "    ");
							if (num2 != 1)
							{
								sb.Append(num2.ToString() + "x ");
							}
							sb.AppendLine(this.linkedFacilities[i].LabelCap + ": " + num.ToStringByStyle(stat.toStringStyle, ToStringNumberSense.Offset));
							this.alreadyUsed.Add(this.linkedFacilities[i].def);
						}
					}
				}
			}
		}

		// Token: 0x0600FAB1 RID: 64177 RVA: 0x0048D561 File Offset: 0x0048B761
		private void RelinkAll()
		{
			this.LinkToNearbyFacilities();
		}

		// Token: 0x0600FAB2 RID: 64178 RVA: 0x0048DB38 File Offset: 0x0048BD38
		public bool IsFacilityActive(Thing facility)
		{
			return facility.TryGetComp<CompFacility>().CanBeActive;
		}

		// Token: 0x0600FAB3 RID: 64179 RVA: 0x0048DB48 File Offset: 0x0048BD48
		private void LinkToNearbyFacilities()
		{
			this.UnlinkAll();
			if (!this.parent.Spawned)
			{
				return;
			}
			foreach (Thing thing in this.ThingsICanLinkTo)
			{
				CompFacility compFacility;
				if (thing.TryGetComp(out compFacility))
				{
					this.linkedFacilities.Add(thing);
					compFacility.Notify_NewLink(this.parent);
				}
			}
		}

		// Token: 0x0600FAB4 RID: 64180 RVA: 0x0048DBC4 File Offset: 0x0048BDC4
		private void UnlinkAll()
		{
			for (int i = 0; i < this.linkedFacilities.Count; i++)
			{
				this.linkedFacilities[i].TryGetComp<CompFacility>().Notify_LinkRemoved(this.parent);
			}
			this.linkedFacilities.Clear();
		}

		// Token: 0x0600FAB7 RID: 64183 RVA: 0x0048DC60 File Offset: 0x0048BE60
		[CompilerGenerated]
		internal static void <DrawPlaceMouseAttachmentsToPotentialThingsToLinkTo>g__DrawTextLine|26_0(ref float y, string text, ref CompAffectedByFacilities.<>c__DisplayClass26_0 A_2)
		{
			float lineHeight = Text.LineHeight;
			Widgets.Label(new Rect(A_2.curX, y, 999f, lineHeight), text);
			y += lineHeight;
		}

		// Token: 0x0400A496 RID: 42134
		private List<Thing> linkedFacilities = new List<Thing>();

		// Token: 0x0400A497 RID: 42135
		public static Material InactiveFacilityLineMat = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, new Color(1f, 0.5f, 0.5f));

		// Token: 0x0400A498 RID: 42136
		private static readonly Dictionary<ThingDef, int> alreadyReturnedCount = new Dictionary<ThingDef, int>();

		// Token: 0x0400A499 RID: 42137
		private List<ThingDef> alreadyUsed = new List<ThingDef>();
	}
}
