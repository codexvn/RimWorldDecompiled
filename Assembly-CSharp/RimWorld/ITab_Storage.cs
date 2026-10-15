using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class ITab_Storage : ITab
{
	[CompilerGenerated]
	private sealed class _003CHiddenSpecialThingFilters_003Ed__15 : IEnumerable<SpecialThingFilterDef>, IEnumerable, IEnumerator<SpecialThingFilterDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private SpecialThingFilterDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		SpecialThingFilterDef IEnumerator<SpecialThingFilterDef>.Current
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
		public _003CHiddenSpecialThingFilters_003Ed__15(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (ModsConfig.IdeologyActive)
				{
					_003C_003E2__current = SpecialThingFilterDefOf.AllowVegetarian;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = SpecialThingFilterDefOf.AllowCarnivore;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = SpecialThingFilterDefOf.AllowCannibal;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = SpecialThingFilterDefOf.AllowInsectMeat;
				_003C_003E1__state = 4;
				return true;
			case 4:
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
		IEnumerator<SpecialThingFilterDef> IEnumerable<SpecialThingFilterDef>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CHiddenSpecialThingFilters_003Ed__15(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<SpecialThingFilterDef>)this).GetEnumerator();
		}
	}

	private ThingFilterUI.UIState thingFilterState = new ThingFilterUI.UIState();

	private static readonly Vector2 WinSize = new Vector2(300f, 480f);

	protected virtual IStoreSettingsParent SelStoreSettingsParent
	{
		get
		{
			if (base.SelObject is Thing t)
			{
				IStoreSettingsParent thingOrThingCompStoreSettingsParent = GetThingOrThingCompStoreSettingsParent(t);
				if (thingOrThingCompStoreSettingsParent != null)
				{
					return thingOrThingCompStoreSettingsParent;
				}
				return null;
			}
			if (base.AllSelObjects.Count > 1)
			{
				bool flag = true;
				StorageGroup storageGroup = (base.AllSelObjects.First() as IStorageGroupMember)?.Group;
				if (storageGroup != null)
				{
					foreach (object allSelObject in base.AllSelObjects)
					{
						if (!(allSelObject is IStorageGroupMember storageGroupMember))
						{
							flag = false;
							break;
						}
						if (storageGroupMember.Group != storageGroup)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return (base.AllSelObjects.First() as IStorageGroupMember)?.Group;
					}
				}
				return null;
			}
			return base.SelObject as IStoreSettingsParent;
		}
	}

	public override bool IsVisible
	{
		get
		{
			if (base.SelObject != null)
			{
				if (base.SelObject is Thing { Faction: not null } thing && thing.Faction != Faction.OfPlayer)
				{
					return false;
				}
			}
			else
			{
				if (base.AllSelObjects.Count <= 1)
				{
					return false;
				}
				foreach (object allSelObject in base.AllSelObjects)
				{
					if (allSelObject is Thing { Faction: not null } thing2 && thing2.Faction != Faction.OfPlayer)
					{
						return false;
					}
				}
			}
			return SelStoreSettingsParent?.StorageTabVisible ?? false;
		}
	}

	protected virtual bool IsPrioritySettingVisible => true;

	private float TopAreaHeight => IsPrioritySettingVisible ? 35 : 20;

	public ITab_Storage()
	{
		size = WinSize;
		labelKey = "TabStorage";
		tutorTag = "Storage";
	}

	public override void OnOpen()
	{
		base.OnOpen();
		thingFilterState.quickSearch.Reset();
	}

	protected override void FillTab()
	{
		IStoreSettingsParent storeSettingsParent = SelStoreSettingsParent;
		StorageSettings settings = storeSettingsParent.GetStoreSettings();
		Rect rect = new Rect(0f, 0f, WinSize.x, WinSize.y).ContractedBy(10f);
		Widgets.BeginGroup(rect);
		if (IsPrioritySettingVisible)
		{
			Text.Font = GameFont.Small;
			Rect rect2 = new Rect(0f, 0f, 160f, TopAreaHeight - 6f);
			if (Widgets.ButtonText(rect2, "Priority".Translate() + ": " + settings.Priority.Label().CapitalizeFirst(), drawBackground: true, doMouseoverSound: true, active: true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (StoragePriority value in Enum.GetValues(typeof(StoragePriority)))
				{
					if (value != 0)
					{
						StoragePriority localPr = value;
						list.Add(new FloatMenuOption(localPr.Label().CapitalizeFirst(), delegate
						{
							settings.Priority = localPr;
						}));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			UIHighlighter.HighlightOpportunity(rect2, "StoragePriority");
		}
		ThingFilter parentFilter = null;
		if (storeSettingsParent.GetParentStoreSettings() != null)
		{
			parentFilter = storeSettingsParent.GetParentStoreSettings().filter;
		}
		Rect rect3 = new Rect(0f, TopAreaHeight, rect.width, rect.height - TopAreaHeight);
		Bill[] first = (from b in BillUtility.GlobalBills()
			where b is Bill_Production && b.GetSlotGroup() == storeSettingsParent && b.recipe.WorkerCounter.CanPossiblyStore((Bill_Production)b, b.GetSlotGroup())
			select b).ToArray();
		ThingFilterUI.DoThingFilterConfigWindow(rect3, thingFilterState, settings.filter, parentFilter, 8, null, HiddenSpecialThingFilters());
		Bill[] second = (from b in BillUtility.GlobalBills()
			where b is Bill_Production && b.GetSlotGroup() == storeSettingsParent && b.recipe.WorkerCounter.CanPossiblyStore((Bill_Production)b, b.GetSlotGroup())
			select b).ToArray();
		foreach (Bill item in first.Except(second))
		{
			Messages.Message("MessageBillValidationStoreZoneInsufficient".Translate(item.LabelCap, item.billStack.billGiver.LabelShort.CapitalizeFirst(), SlotGroup.GetGroupLabel(item.GetSlotGroup())), item.billStack.billGiver as Thing, MessageTypeDefOf.RejectInput, historical: false);
		}
		PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.StorageTab, KnowledgeAmount.FrameDisplayed);
		Widgets.EndGroup();
	}

	protected IStoreSettingsParent GetThingOrThingCompStoreSettingsParent(Thing t)
	{
		if (t is IStoreSettingsParent result)
		{
			return result;
		}
		if (t is ThingWithComps { AllComps: var allComps })
		{
			for (int i = 0; i < allComps.Count; i++)
			{
				if (allComps[i] is IStoreSettingsParent result2)
				{
					return result2;
				}
			}
		}
		return null;
	}

	public override void Notify_ClickOutsideWindow()
	{
		base.Notify_ClickOutsideWindow();
		thingFilterState.quickSearch.Unfocus();
	}

	[IteratorStateMachine(typeof(_003CHiddenSpecialThingFilters_003Ed__15))]
	private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CHiddenSpecialThingFilters_003Ed__15(-2);
	}
}
