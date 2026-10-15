using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Precept_Building : Precept_ThingStyle
{
	[CompilerGenerated]
	private sealed class _003CEditFloatMenuOptions_003Ed__12 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Precept_Building _003C_003E4__this;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CEditFloatMenuOptions_003Ed__12(int _003C_003E1__state)
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
			Precept_Building precept_Building = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = precept_Building.EditFloatMenuOption();
				_003C_003E1__state = 1;
				return true;
			case 1:
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CEditFloatMenuOptions_003Ed__12 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CEditFloatMenuOptions_003Ed__12(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetAlerts_003Ed__13 : IEnumerable<Alert>, IEnumerable, IEnumerator<Alert>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Alert _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Precept_Building _003C_003E4__this;

		Alert IEnumerator<Alert>.Current
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
		public _003CGetAlerts_003Ed__13(int _003C_003E1__state)
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
			Precept_Building precept_Building = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Map currentMap = Find.CurrentMap;
				if (currentMap == null || !precept_Building.presenceDemand.AppliesTo(currentMap))
				{
					return false;
				}
				if (!precept_Building.presenceDemand.BuildingPresent(currentMap))
				{
					_003C_003E2__current = precept_Building.presenceDemand.AlertCachedMissingMissing;
					_003C_003E1__state = 1;
					return true;
				}
				if (!precept_Building.presenceDemand.roomRequirements.NullOrEmpty())
				{
					_003C_003E2__current = precept_Building.presenceDemand.AlertCachedMissingDisrespected;
					_003C_003E1__state = 2;
					return true;
				}
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				break;
			}
			return false;
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
		IEnumerator<Alert> IEnumerable<Alert>.GetEnumerator()
		{
			_003CGetAlerts_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetAlerts_003Ed__13(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Alert>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__17 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Precept_Building _003C_003E4__this;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__17(int _003C_003E1__state)
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
			Precept_Building precept_Building = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.BasicsImportant, "Stat_Thing_RelatedToIdeos_Name".Translate(), precept_Building.ideo.name.ApplyTag(precept_Building.ideo).Resolve(), "Stat_Thing_RelatedToIdeos_Desc".Translate(), 1110, null, new Dialog_InfoCard.Hyperlink[1]
				{
					new Dialog_InfoCard.Hyperlink(precept_Building.ideo)
				});
				_003C_003E1__state = 1;
				return true;
			case 1:
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__17(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	public IdeoBuildingPresenceDemand presenceDemand;

	public override bool CanRegenerate => true;

	protected override string NameRootSymbol => "r_ideoBuildingName";

	public override void Init(Ideo ideo, FactionDef generatingFor = null)
	{
		base.Init(ideo, (FactionDef)null);
		if (ModLister.CheckIdeology("Ideology building"))
		{
			GeneratePresenceDemand();
		}
	}

	public override void Regenerate(Ideo ideo, FactionDef generatingFor = null)
	{
		GeneratePresenceDemand();
		if (UsesGeneratedName)
		{
			RegenerateName();
		}
		ClearTipCache();
		Notify_ThingDefSet();
	}

	protected void GeneratePresenceDemand()
	{
		presenceDemand = new IdeoBuildingPresenceDemand(this);
		presenceDemand.minExpectation = def.buildingMinExpectations.RandomElement();
		int num = Mathf.CeilToInt(def.roomRequirementCountCurve.Evaluate(Rand.Value));
		if ((base.ThingDef.ritualFocus == null || !base.ThingDef.ritualFocus.consumable) && num > 0)
		{
			presenceDemand.roomRequirements = new List<RoomRequirement>();
			List<RoomRequirement> list = def.buildingRoomRequirements.ToList();
			list.Shuffle();
			for (int i = 0; i < num; i++)
			{
				presenceDemand.roomRequirements.Add(list[i]);
			}
			presenceDemand.roomRequirements.AddRange(def.buildingRoomRequirementsFixed);
		}
	}

	public override string TransformThingLabel(string label)
	{
		return label + " (" + base.LabelCap + ")";
	}

	private string CostListString()
	{
		List<string> list = new List<string>();
		if (!base.ThingDef.CostList.NullOrEmpty())
		{
			for (int i = 0; i < base.ThingDef.CostList.Count; i++)
			{
				list.Add(string.Concat(base.ThingDef.CostList[i].thingDef.LabelCap + " x", base.ThingDef.CostList[i].count.ToString()));
			}
		}
		if (!base.ThingDef.stuffCategories.NullOrEmpty())
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
			list.Add(text.CapitalizeFirst() + " x" + base.ThingDef.CostStuffCount);
		}
		return list.ToCommaList().CapitalizeFirst();
	}

	public override string GetTip()
	{
		if (tipCached.NullOrEmpty())
		{
			Precept.tmpCompsDesc.Clear();
			if (base.ThingDef?.description != null)
			{
				Precept.tmpCompsDesc.Append(base.ThingDef.description);
			}
			else if (!base.Description.NullOrEmpty())
			{
				Precept.tmpCompsDesc.Append(base.Description);
			}
			Precept.tmpCompsDesc.AppendLine();
			Precept.tmpCompsDesc.AppendInNewLine(ColorizeDescTitle("IdeoBuildingVariationOf".Translate() + ": "));
			Precept.tmpCompsDesc.AppendInNewLine(base.ThingDef.LabelCap.Resolve().Indented());
			Precept.tmpCompsDesc.AppendInNewLine(ColorizeDescTitle("Size".Translate().CapitalizeFirst() + ": ").Indented());
			Precept.tmpCompsDesc.Append(base.ThingDef.size.x + "x" + base.ThingDef.size.z);
			Precept.tmpCompsDesc.AppendInNewLine(ColorizeDescTitle("Cost".Translate().CapitalizeFirst() + ": ").Indented());
			Precept.tmpCompsDesc.Append(CostListString());
			if (base.ThingDef.ritualFocus == null || !base.ThingDef.ritualFocus.consumable)
			{
				Precept.tmpCompsDesc.AppendLine();
				Precept.tmpCompsDesc.AppendInNewLine(ColorizeDescTitle("IdeoBuildingMinimumExpectations".Translate() + ": "));
				Precept.tmpCompsDesc.Append(presenceDemand.minExpectation.LabelCap);
				if (!presenceDemand.roomRequirements.NullOrEmpty())
				{
					Precept.tmpCompsDesc.AppendLine();
					Precept.tmpCompsDesc.AppendInNewLine(ColorizeDescTitle("RoomRequirements".Translate() + ":"));
					Precept.tmpCompsDesc.AppendInNewLine(presenceDemand.RoomRequirementsInfo.ToLineList(" -  ", capitalizeItems: true));
				}
			}
			tipCached = Precept.tmpCompsDesc.ToString();
		}
		return tipCached;
	}

	public override List<Thought_Situational> SituationThoughtsToAdd(Pawn pawn, List<Thought_Situational> activeThoughts)
	{
		tmpSituationalThoughts.Clear();
		if (!pawn.IsFreeColonist)
		{
			return tmpSituationalThoughts;
		}
		Map mapHeld = pawn.MapHeld;
		if (mapHeld != null)
		{
			if (!presenceDemand.AppliesTo(mapHeld))
			{
				return tmpSituationalThoughts;
			}
			if (!presenceDemand.BuildingPresent(mapHeld))
			{
				if (!activeThoughts.Any((Thought_Situational t) => t is Thought_IdeoMissingBuilding thought_IdeoMissingBuilding && thought_IdeoMissingBuilding.demand == presenceDemand))
				{
					Thought_IdeoMissingBuilding thought_IdeoMissingBuilding2 = (Thought_IdeoMissingBuilding)ThoughtMaker.MakeThought(ThoughtDefOf.IdeoBuildingMissing);
					if (thought_IdeoMissingBuilding2 != null)
					{
						thought_IdeoMissingBuilding2.pawn = pawn;
						thought_IdeoMissingBuilding2.demand = presenceDemand;
						thought_IdeoMissingBuilding2.sourcePrecept = this;
						tmpSituationalThoughts.Add(thought_IdeoMissingBuilding2);
					}
				}
			}
			else if (!presenceDemand.RequirementsSatisfied(mapHeld) && !activeThoughts.Any((Thought_Situational t) => t is Thought_IdeoDisrespectedBuilding thought_IdeoDisrespectedBuilding && thought_IdeoDisrespectedBuilding.demand == presenceDemand))
			{
				Thought_IdeoDisrespectedBuilding thought_IdeoDisrespectedBuilding2 = (Thought_IdeoDisrespectedBuilding)ThoughtMaker.MakeThought(ThoughtDefOf.IdeoBuildingDisrespected);
				if (thought_IdeoDisrespectedBuilding2 != null)
				{
					thought_IdeoDisrespectedBuilding2.pawn = pawn;
					thought_IdeoDisrespectedBuilding2.demand = presenceDemand;
					thought_IdeoDisrespectedBuilding2.sourcePrecept = this;
					tmpSituationalThoughts.Add(thought_IdeoDisrespectedBuilding2);
				}
			}
		}
		return tmpSituationalThoughts;
	}

	[IteratorStateMachine(typeof(_003CEditFloatMenuOptions_003Ed__12))]
	public override IEnumerable<FloatMenuOption> EditFloatMenuOptions()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CEditFloatMenuOptions_003Ed__12(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetAlerts_003Ed__13))]
	public override IEnumerable<Alert> GetAlerts()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetAlerts_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Deep.Look(ref presenceDemand, "presenceDemand");
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			presenceDemand.parent = this;
		}
	}

	public override void CopyTo(Precept precept)
	{
		base.CopyTo(precept);
		Precept_Building precept_Building = (Precept_Building)precept;
		precept_Building.presenceDemand = presenceDemand.Copy();
		precept_Building.presenceDemand.parent = precept_Building;
	}

	public override string InspectStringExtra(Thing thing)
	{
		return ("Stat_Thing_RelatedToIdeos_Name".Translate() + ": " + ideo.name.ApplyTag(ideo)).Resolve();
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__17))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats(Thing thing)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__17(-2)
		{
			_003C_003E4__this = this
		};
	}
}
