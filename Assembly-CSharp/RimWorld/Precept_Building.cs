using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002650 RID: 9808
	public class Precept_Building : Precept_ThingStyle
	{
		// Token: 0x170020DE RID: 8414
		// (get) Token: 0x0600D6E0 RID: 55008 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool CanRegenerate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170020DF RID: 8415
		// (get) Token: 0x0600D6E1 RID: 55009 RVA: 0x003FB8D0 File Offset: 0x003F9AD0
		protected override string NameRootSymbol
		{
			get
			{
				return "r_ideoBuildingName";
			}
		}

		// Token: 0x0600D6E2 RID: 55010 RVA: 0x003FB8D7 File Offset: 0x003F9AD7
		public override void Init(Ideo ideo, FactionDef generatingFor = null)
		{
			base.Init(ideo, null);
			if (!ModLister.CheckIdeology("Ideology building"))
			{
				return;
			}
			this.GeneratePresenceDemand();
		}

		// Token: 0x0600D6E3 RID: 55011 RVA: 0x003FB8F4 File Offset: 0x003F9AF4
		public override void Regenerate(Ideo ideo, FactionDef generatingFor = null)
		{
			this.GeneratePresenceDemand();
			if (this.UsesGeneratedName)
			{
				base.RegenerateName();
			}
			this.ClearTipCache();
			this.Notify_ThingDefSet();
		}

		// Token: 0x0600D6E4 RID: 55012 RVA: 0x003FB918 File Offset: 0x003F9B18
		protected void GeneratePresenceDemand()
		{
			this.presenceDemand = new IdeoBuildingPresenceDemand(this);
			this.presenceDemand.minExpectation = this.def.buildingMinExpectations.RandomElement<ExpectationDef>();
			int num = Mathf.CeilToInt(this.def.roomRequirementCountCurve.Evaluate(Rand.Value));
			if ((base.ThingDef.ritualFocus == null || !base.ThingDef.ritualFocus.consumable) && num > 0)
			{
				this.presenceDemand.roomRequirements = new List<RoomRequirement>();
				List<RoomRequirement> list = this.def.buildingRoomRequirements.ToList<RoomRequirement>();
				list.Shuffle<RoomRequirement>();
				for (int i = 0; i < num; i++)
				{
					this.presenceDemand.roomRequirements.Add(list[i]);
				}
				this.presenceDemand.roomRequirements.AddRange(this.def.buildingRoomRequirementsFixed);
			}
		}

		// Token: 0x0600D6E5 RID: 55013 RVA: 0x003FB9EF File Offset: 0x003F9BEF
		public override string TransformThingLabel(string label)
		{
			return label + " (" + base.LabelCap + ")";
		}

		// Token: 0x0600D6E6 RID: 55014 RVA: 0x003FBA08 File Offset: 0x003F9C08
		private string CostListString()
		{
			List<string> list = new List<string>();
			if (!base.ThingDef.CostList.NullOrEmpty<ThingDefCountClass>())
			{
				for (int i = 0; i < base.ThingDef.CostList.Count; i++)
				{
					list.Add(base.ThingDef.CostList[i].thingDef.LabelCap + " x" + base.ThingDef.CostList[i].count.ToString());
				}
			}
			if (!base.ThingDef.stuffCategories.NullOrEmpty<StuffCategoryDef>())
			{
				string text = "";
				for (int j = 0; j < base.ThingDef.stuffCategories.Count; j++)
				{
					if (j > 0)
					{
						text += "/";
					}
					text += base.ThingDef.stuffCategories[j].label;
				}
				list.Add(text.CapitalizeFirst() + " x" + base.ThingDef.CostStuffCount.ToString());
			}
			return list.ToCommaList(false, false).CapitalizeFirst();
		}

		// Token: 0x0600D6E7 RID: 55015 RVA: 0x003FBB30 File Offset: 0x003F9D30
		public override string GetTip()
		{
			if (this.tipCached.NullOrEmpty())
			{
				Precept.tmpCompsDesc.Clear();
				ThingDef thingDef = base.ThingDef;
				if (((thingDef != null) ? thingDef.description : null) != null)
				{
					Precept.tmpCompsDesc.Append(base.ThingDef.description);
				}
				else if (!base.Description.NullOrEmpty())
				{
					Precept.tmpCompsDesc.Append(base.Description);
				}
				Precept.tmpCompsDesc.AppendLine();
				Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("IdeoBuildingVariationOf".Translate() + ": "));
				Precept.tmpCompsDesc.AppendInNewLine(base.ThingDef.LabelCap.Resolve().Indented("    "));
				Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("Size".Translate().CapitalizeFirst() + ": ").Indented("    "));
				Precept.tmpCompsDesc.Append(base.ThingDef.size.x.ToString() + "x" + base.ThingDef.size.z.ToString());
				Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("Cost".Translate().CapitalizeFirst() + ": ").Indented("    "));
				Precept.tmpCompsDesc.Append(this.CostListString());
				if (base.ThingDef.ritualFocus == null || !base.ThingDef.ritualFocus.consumable)
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("IdeoBuildingMinimumExpectations".Translate() + ": "));
					Precept.tmpCompsDesc.Append(this.presenceDemand.minExpectation.LabelCap);
					if (!this.presenceDemand.roomRequirements.NullOrEmpty<RoomRequirement>())
					{
						Precept.tmpCompsDesc.AppendLine();
						Precept.tmpCompsDesc.AppendInNewLine(base.ColorizeDescTitle("RoomRequirements".Translate() + ":"));
						Precept.tmpCompsDesc.AppendInNewLine(this.presenceDemand.RoomRequirementsInfo.ToLineList(" -  ", true));
					}
				}
				this.tipCached = Precept.tmpCompsDesc.ToString();
			}
			return this.tipCached;
		}

		// Token: 0x0600D6E8 RID: 55016 RVA: 0x003FBDA4 File Offset: 0x003F9FA4
		public override List<Thought_Situational> SituationThoughtsToAdd(Pawn pawn, List<Thought_Situational> activeThoughts)
		{
			this.tmpSituationalThoughts.Clear();
			if (!pawn.IsFreeColonist)
			{
				return this.tmpSituationalThoughts;
			}
			Map mapHeld = pawn.MapHeld;
			if (mapHeld != null)
			{
				if (!this.presenceDemand.AppliesTo(mapHeld))
				{
					return this.tmpSituationalThoughts;
				}
				if (!this.presenceDemand.BuildingPresent(mapHeld))
				{
					if (!activeThoughts.Any(delegate(Thought_Situational t)
					{
						Thought_IdeoMissingBuilding thought_IdeoMissingBuilding2 = t as Thought_IdeoMissingBuilding;
						return thought_IdeoMissingBuilding2 != null && thought_IdeoMissingBuilding2.demand == this.presenceDemand;
					}))
					{
						Thought_IdeoMissingBuilding thought_IdeoMissingBuilding = (Thought_IdeoMissingBuilding)ThoughtMaker.MakeThought(ThoughtDefOf.IdeoBuildingMissing);
						if (thought_IdeoMissingBuilding != null)
						{
							thought_IdeoMissingBuilding.pawn = pawn;
							thought_IdeoMissingBuilding.demand = this.presenceDemand;
							thought_IdeoMissingBuilding.sourcePrecept = this;
							this.tmpSituationalThoughts.Add(thought_IdeoMissingBuilding);
						}
					}
				}
				else if (!this.presenceDemand.RequirementsSatisfied(mapHeld) && !activeThoughts.Any(delegate(Thought_Situational t)
				{
					Thought_IdeoDisrespectedBuilding thought_IdeoDisrespectedBuilding2 = t as Thought_IdeoDisrespectedBuilding;
					return thought_IdeoDisrespectedBuilding2 != null && thought_IdeoDisrespectedBuilding2.demand == this.presenceDemand;
				}))
				{
					Thought_IdeoDisrespectedBuilding thought_IdeoDisrespectedBuilding = (Thought_IdeoDisrespectedBuilding)ThoughtMaker.MakeThought(ThoughtDefOf.IdeoBuildingDisrespected);
					if (thought_IdeoDisrespectedBuilding != null)
					{
						thought_IdeoDisrespectedBuilding.pawn = pawn;
						thought_IdeoDisrespectedBuilding.demand = this.presenceDemand;
						thought_IdeoDisrespectedBuilding.sourcePrecept = this;
						this.tmpSituationalThoughts.Add(thought_IdeoDisrespectedBuilding);
					}
				}
			}
			return this.tmpSituationalThoughts;
		}

		// Token: 0x0600D6E9 RID: 55017 RVA: 0x003FBEB1 File Offset: 0x003FA0B1
		public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
		{
			yield return base.EditFloatMenuOption();
			yield break;
		}

		// Token: 0x0600D6EA RID: 55018 RVA: 0x003FBEC1 File Offset: 0x003FA0C1
		public override IEnumerable<Alert> GetAlerts()
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap == null || !this.presenceDemand.AppliesTo(currentMap))
			{
				yield break;
			}
			if (!this.presenceDemand.BuildingPresent(currentMap))
			{
				yield return this.presenceDemand.AlertCachedMissingMissing;
			}
			else if (!this.presenceDemand.roomRequirements.NullOrEmpty<RoomRequirement>())
			{
				yield return this.presenceDemand.AlertCachedMissingDisrespected;
			}
			yield break;
		}

		// Token: 0x0600D6EB RID: 55019 RVA: 0x003FBED1 File Offset: 0x003FA0D1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<IdeoBuildingPresenceDemand>(ref this.presenceDemand, "presenceDemand", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.presenceDemand.parent = this;
			}
		}

		// Token: 0x0600D6EC RID: 55020 RVA: 0x003FBF04 File Offset: 0x003FA104
		public override void CopyTo(Precept precept)
		{
			base.CopyTo(precept);
			Precept_Building precept_Building = (Precept_Building)precept;
			precept_Building.presenceDemand = this.presenceDemand.Copy();
			precept_Building.presenceDemand.parent = precept_Building;
		}

		// Token: 0x0600D6ED RID: 55021 RVA: 0x003FBF3C File Offset: 0x003FA13C
		public override string InspectStringExtra(Thing thing)
		{
			return ("Stat_Thing_RelatedToIdeos_Name".Translate() + ": " + this.ideo.name.ApplyTag(this.ideo)).Resolve();
		}

		// Token: 0x0600D6EE RID: 55022 RVA: 0x003FBF80 File Offset: 0x003FA180
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(Thing thing)
		{
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_Thing_RelatedToIdeos_Name".Translate(), this.ideo.name.ApplyTag(this.ideo).Resolve(), "Stat_Thing_RelatedToIdeos_Desc".Translate(), 1110, null, new Dialog_InfoCard.Hyperlink[]
			{
				new Dialog_InfoCard.Hyperlink(this.ideo)
			}, false, false);
			yield break;
		}

		// Token: 0x04009205 RID: 37381
		public IdeoBuildingPresenceDemand presenceDemand;
	}
}
