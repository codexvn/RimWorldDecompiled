using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld;

[StaticConstructorOnStartup]
public class MapPortal : Building, IThingHolder
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__40 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MapPortal _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

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
		public _003CGetGizmos_003Ed__40(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				MapPortal CS_0024_003C_003E8__locals13 = _003C_003E4__this;
				string reason;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building)CS_0024_003C_003E8__locals13).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007c;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007c;
				case 2:
					_003C_003E1__state = -1;
					if (CS_0024_003C_003E8__locals13.LoadInProgress)
					{
						_003C_003E2__current = new Command_Action
						{
							action = CS_0024_003C_003E8__locals13.CancelLoad,
							icon = CancelEnterTex,
							defaultLabel = CS_0024_003C_003E8__locals13.CancelEnterString,
							defaultDesc = "CommandCancelEnterPortalDesc".Translate()
						};
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0193;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0193;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007c:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E2__current = new Command_Action
					{
						action = delegate
						{
							Dialog_EnterPortal window = new Dialog_EnterPortal(CS_0024_003C_003E8__locals13);
							Find.WindowStack.Add(window);
						},
						icon = CS_0024_003C_003E8__locals13.EnterTex,
						defaultLabel = CS_0024_003C_003E8__locals13.EnterString + "...",
						defaultDesc = "CommandEnterPortalDesc".Translate(CS_0024_003C_003E8__locals13.Label),
						Disabled = !CS_0024_003C_003E8__locals13.IsEnterable(out reason),
						disabledReason = reason
					};
					_003C_003E1__state = 2;
					return true;
					IL_0193:
					if (CS_0024_003C_003E8__locals13.pocketMap != null)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "CommandViewPocketMapLabel".Translate(CS_0024_003C_003E8__locals13.def.portal.pocketMapGenerator.label),
							defaultDesc = "CommandViewPocketMapDesc".Translate(CS_0024_003C_003E8__locals13.def.portal.pocketMapGenerator.label),
							icon = ViewPocketMapTex,
							action = delegate
							{
								CameraJumper.TryJumpAndSelect(CS_0024_003C_003E8__locals13.exit);
							}
						};
						_003C_003E1__state = 4;
						return true;
					}
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__40 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__40(0)
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

	private static readonly Texture2D ViewPocketMapTex = ContentFinder<Texture2D>.Get("UI/Commands/ViewCave");

	private static readonly Texture2D CancelEnterTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel");

	private static readonly Texture2D DefaultEnterTex = ContentFinder<Texture2D>.Get("UI/Commands/EnterCave");

	protected Map pocketMap;

	public PocketMapExit exit;

	protected bool beenEntered;

	public List<TransferableOneWay> leftToLoad;

	public PortalContainerProxy containerProxy;

	public bool notifiedCantLoadMore;

	public Map PocketMap
	{
		get
		{
			Map map = pocketMap;
			if (map != null && map.Parent?.HasMap == false)
			{
				pocketMap = null;
			}
			return pocketMap;
		}
	}

	public bool PocketMapExists => PocketMap != null;

	public virtual bool AutoDraftOnEnter => false;

	protected virtual Texture2D EnterTex => DefaultEnterTex;

	public virtual string EnterString => "EnterPortal".Translate(Label);

	public virtual string CancelEnterString => "CommandCancelEnterPortal".Translate();

	public virtual string EnteringString => "EnteringPortal".Translate(Label);

	public bool LoadInProgress
	{
		get
		{
			if (leftToLoad != null)
			{
				return leftToLoad.Any();
			}
			return false;
		}
	}

	public bool AnyPawnCanLoadAnythingNow
	{
		get
		{
			if (!LoadInProgress)
			{
				return false;
			}
			if (!base.Spawned)
			{
				return false;
			}
			IReadOnlyList<Pawn> allPawnsSpawned = base.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (allPawnsSpawned[i].CurJobDef == JobDefOf.HaulToPortal && ((JobDriver_HaulToPortal)allPawnsSpawned[i].jobs.curDriver).MapPortal == this)
				{
					return true;
				}
				if (allPawnsSpawned[i].CurJobDef == JobDefOf.EnterPortal && ((JobDriver_EnterPortal)allPawnsSpawned[i].jobs.curDriver).MapPortal == this)
				{
					return true;
				}
			}
			for (int j = 0; j < allPawnsSpawned.Count; j++)
			{
				Thing thing = allPawnsSpawned[j].mindState?.duty?.focus.Thing;
				if (thing != null && thing == this && allPawnsSpawned[j].CanReach(thing, PathEndMode.Touch, Danger.Deadly))
				{
					return true;
				}
			}
			for (int k = 0; k < allPawnsSpawned.Count; k++)
			{
				if (allPawnsSpawned[k].IsColonist && EnterPortalUtility.HasJobOnPortal(allPawnsSpawned[k], this))
				{
					return true;
				}
			}
			return false;
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Map map = pocketMap;
		if (map != null && map.Parent?.HasMap == false)
		{
			pocketMap = null;
		}
		Scribe_References.Look(ref pocketMap, "pocketMap");
		Scribe_References.Look(ref exit, "exit");
		Scribe_Values.Look(ref beenEntered, "beenEntered", defaultValue: false);
		Scribe_Collections.Look(ref leftToLoad, "leftToLoad", LookMode.Deep);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			leftToLoad?.RemoveAll((TransferableOneWay x) => x.AnyThing == null);
		}
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		containerProxy = new PortalContainerProxy
		{
			portal = this
		};
	}

	protected override void Tick()
	{
		base.Tick();
		if (this.IsHashIntervalTick(60) && base.Spawned && LoadInProgress && !notifiedCantLoadMore && !AnyPawnCanLoadAnythingNow && leftToLoad[0]?.AnyThing != null)
		{
			notifiedCantLoadMore = true;
			Messages.Message("MessageCantLoadMoreIntoPortal".Translate(Label, Faction.OfPlayer.def.pawnsPlural, leftToLoad[0].AnyThing), this, MessageTypeDefOf.CautionInput);
		}
	}

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return containerProxy;
	}

	public void Notify_ThingAdded(Thing t)
	{
		SubtractFromToLoadList(t, t.stackCount);
	}

	public void AddToTheToLoadList(TransferableOneWay t, int count)
	{
		if (!t.HasAnyThing || count <= 0)
		{
			return;
		}
		if (leftToLoad == null)
		{
			leftToLoad = new List<TransferableOneWay>();
		}
		TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatching(t.AnyThing, leftToLoad, TransferAsOneMode.PodsOrCaravanPacking);
		if (transferableOneWay != null)
		{
			for (int i = 0; i < t.things.Count; i++)
			{
				if (!transferableOneWay.things.Contains(t.things[i]))
				{
					transferableOneWay.things.Add(t.things[i]);
				}
			}
			if (transferableOneWay.CanAdjustBy(count).Accepted)
			{
				transferableOneWay.AdjustBy(count);
			}
		}
		else
		{
			TransferableOneWay transferableOneWay2 = new TransferableOneWay();
			leftToLoad.Add(transferableOneWay2);
			transferableOneWay2.things.AddRange(t.things);
			transferableOneWay2.AdjustTo(count);
		}
	}

	public int SubtractFromToLoadList(Thing t, int count)
	{
		if (leftToLoad == null)
		{
			return 0;
		}
		TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(t, leftToLoad, TransferAsOneMode.PodsOrCaravanPacking);
		if (transferableOneWay == null)
		{
			return 0;
		}
		if (transferableOneWay.CountToTransfer <= 0)
		{
			return 0;
		}
		int num = Mathf.Min(count, transferableOneWay.CountToTransfer);
		transferableOneWay.AdjustBy(-num);
		transferableOneWay.things.Remove(t);
		if (transferableOneWay.CountToTransfer <= 0)
		{
			leftToLoad.Remove(transferableOneWay);
		}
		return num;
	}

	public void CancelLoad()
	{
		Lord lord = base.Map.lordManager.lords.FirstOrDefault((Lord l) => l.LordJob is LordJob_LoadAndEnterPortal lordJob_LoadAndEnterPortal && lordJob_LoadAndEnterPortal.portal == this);
		if (lord != null)
		{
			base.Map.lordManager.RemoveLord(lord);
		}
		leftToLoad.Clear();
	}

	public virtual bool IsEnterable(out string reason)
	{
		foreach (ThingComp allComp in base.AllComps)
		{
			AcceptanceReport acceptanceReport = allComp.CanEnterPortal();
			if (!acceptanceReport.Accepted)
			{
				reason = acceptanceReport.Reason;
				return false;
			}
		}
		reason = "";
		return true;
	}

	public virtual Map GetOtherMap()
	{
		if (PocketMap == null)
		{
			GeneratePocketMap();
		}
		return PocketMap;
	}

	public virtual IntVec3 GetDestinationLocation()
	{
		return exit?.Position ?? IntVec3.Invalid;
	}

	public virtual void OnEntered(Pawn pawn)
	{
		Notify_ThingAdded(pawn);
		if (!beenEntered)
		{
			beenEntered = true;
			if (!def.portal.enteredLetterLabel.NullOrEmpty())
			{
				Find.LetterStack.ReceiveLetter(def.portal.enteredLetterLabel, def.portal.enteredLetterText.Formatted(pawn.Named("PAWN")), def.portal.enteredLetterDef, exit);
			}
		}
		if (Find.CurrentMap == base.Map)
		{
			def.portal.traverseSound?.PlayOneShot(this);
		}
		else if (Find.CurrentMap == exit.Map)
		{
			def.portal.traverseSound?.PlayOneShot(exit);
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__40))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__40(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void GeneratePocketMap()
	{
		PocketMapUtility.currentlyGeneratingPortal = this;
		pocketMap = GeneratePocketMapInt();
		PocketMapUtility.currentlyGeneratingPortal = null;
	}

	protected virtual Map GeneratePocketMapInt()
	{
		return PocketMapUtility.GeneratePocketMap(new IntVec3(def.portal.pocketMapSize, 1, def.portal.pocketMapSize), def.portal.pocketMapGenerator, GetExtraGenSteps(), base.Map);
	}

	protected virtual IEnumerable<GenStepWithParams> GetExtraGenSteps()
	{
		return Enumerable.Empty<GenStepWithParams>();
	}
}
