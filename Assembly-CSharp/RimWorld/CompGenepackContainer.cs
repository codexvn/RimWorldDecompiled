using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompGenepackContainer : ThingComp, IThingHolder, ISearchableContents
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__27 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompGenepackContainer _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CCompGetGizmosExtra_003Ed__27(int _003C_003E1__state)
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
			CompGenepackContainer CS_0024_003C_003E8__locals7 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals7.parent.Faction == Faction.OfPlayer && CS_0024_003C_003E8__locals7.innerContainer.Any)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "EjectAll".Translate(),
						defaultDesc = "EjectAllDesc".Translate(),
						icon = EjectTex.Texture,
						action = delegate
						{
							CS_0024_003C_003E8__locals7.EjectContents(CS_0024_003C_003E8__locals7.parent.Map);
						}
					};
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00b1;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00b1;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_00b1:
				if (!DebugSettings.ShowDevGizmos)
				{
					break;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Fill with new packs",
					action = delegate
					{
						CS_0024_003C_003E8__locals7.innerContainer.ClearAndDestroyContents();
						for (int i = 0; i < CS_0024_003C_003E8__locals7.Props.maxCapacity; i++)
						{
							CS_0024_003C_003E8__locals7.innerContainer.TryAdd(ThingMaker.MakeThing(ThingDefOf.Genepack));
						}
					}
				};
				_003C_003E1__state = 2;
				return true;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__27 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__27(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	public ThingOwner innerContainer;

	public List<Thing> leftToLoad = new List<Thing>();

	public bool autoLoad = true;

	[Unsaved(false)]
	private List<Genepack> tmpGenepacks = new List<Genepack>();

	private static readonly CachedTexture EjectTex = new CachedTexture("UI/Gizmos/EjectAll");

	public CompProperties_GenepackContainer Props => (CompProperties_GenepackContainer)props;

	public bool StorageTabVisible => true;

	public bool PowerOn => parent.TryGetComp<CompPowerTrader>().PowerOn;

	public bool Full => ContainedGenepacks.Count >= Props.maxCapacity;

	public bool CanLoadMore
	{
		get
		{
			if (!Full)
			{
				return ContainedGenepacks.Count + leftToLoad.Count < Props.maxCapacity;
			}
			return false;
		}
	}

	public ThingOwner SearchableContents => innerContainer;

	public List<Genepack> ContainedGenepacks
	{
		get
		{
			tmpGenepacks.Clear();
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (innerContainer[i] is Genepack genepack && genepack.GeneSet.GenesListForReading.Any())
				{
					tmpGenepacks.Add(genepack);
				}
			}
			return tmpGenepacks;
		}
	}

	public override void PostPostMake()
	{
		if (!ModLister.CheckBiotech("Genepack container"))
		{
			parent.Destroy();
			return;
		}
		base.PostPostMake();
		innerContainer = new ThingOwner<Thing>(this);
	}

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return innerContainer;
	}

	public override void PostDestroy(DestroyMode mode, Map previousMap)
	{
		innerContainer.ClearAndDestroyContents();
		base.PostDestroy(mode, previousMap);
	}

	public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
	{
		if (mode != DestroyMode.WillReplace)
		{
			EjectContents(map);
		}
		for (int i = 0; i < leftToLoad.Count; i++)
		{
			((Genepack)leftToLoad[i]).targetContainer = null;
		}
		leftToLoad.Clear();
	}

	public override void PostDrawExtraSelectionOverlays()
	{
		for (int i = 0; i < leftToLoad.Count; i++)
		{
			if (leftToLoad[i].Map == parent.Map)
			{
				GenDraw.DrawLineBetween(parent.DrawPos, leftToLoad[i].DrawPos);
			}
		}
	}

	public void EjectContents(Map destMap = null)
	{
		if (destMap == null)
		{
			destMap = parent.Map;
		}
		IntVec3 dropLoc = (parent.def.hasInteractionCell ? parent.InteractionCell : parent.Position);
		innerContainer.TryDropAll(dropLoc, destMap, ThingPlaceMode.Near);
	}

	public override void CompTickRare()
	{
		if (innerContainer != null)
		{
			for (int i = 0; i < innerContainer.Count; i++)
			{
				innerContainer[i].TickRare();
			}
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__27))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__27(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string CompInspectStringExtra()
	{
		return "GenepacksStored".Translate() + $": {innerContainer.Count} / {Props.maxCapacity}\n" + "CasketContains".Translate() + ": " + innerContainer.ContentsString.CapitalizeFirst();
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
		Scribe_Collections.Look(ref leftToLoad, "leftToLoad", LookMode.Reference);
		Scribe_Values.Look(ref autoLoad, "autoLoad", defaultValue: true);
	}
}
