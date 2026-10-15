using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse;

public class UnnaturalCorpse : Corpse
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__20 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public UnnaturalCorpse _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__20(int _003C_003E1__state)
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
				UnnaturalCorpse CS_0024_003C_003E8__locals9 = _003C_003E4__this;
				Gizmo gizmo;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Corpse)CS_0024_003C_003E8__locals9).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0088;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0088;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00c8;
				case 3:
					_003C_003E1__state = -1;
					if (!CS_0024_003C_003E8__locals9.teleport)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Teleport",
							action = delegate
							{
								CS_0024_003C_003E8__locals9.teleport = true;
							}
						};
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_0158;
				case 4:
					_003C_003E1__state = -1;
					goto IL_0158;
				case 5:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Awake",
						action = delegate
						{
							CS_0024_003C_003E8__locals9.tracker.DevAwaken();
						}
					};
					_003C_003E1__state = 6;
					return true;
				case 6:
					_003C_003E1__state = -1;
					if (!CS_0024_003C_003E8__locals9.tracker.CanDestroyViaResearch)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Unlock deactivation",
							action = delegate
							{
								CS_0024_003C_003E8__locals9.tracker.DevUnlockDeactivation();
							}
						};
						_003C_003E1__state = 7;
						return true;
					}
					break;
				case 7:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_00c8:
					if (!DebugSettings.ShowDevGizmos)
					{
						return false;
					}
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Disappear",
						action = delegate
						{
							Find.Anomaly.DevRemoveUnnaturalCorpse(CS_0024_003C_003E8__locals9.tracker.Haunted);
						}
					};
					_003C_003E1__state = 3;
					return true;
					IL_0158:
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Mental break",
						action = delegate
						{
							CS_0024_003C_003E8__locals9.tracker.TryTriggerMentalBreak();
						}
					};
					_003C_003E1__state = 5;
					return true;
					IL_0088:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					gizmo = AnomalyUtility.OpenCodexGizmo(CS_0024_003C_003E8__locals9);
					if (gizmo != null)
					{
						_003C_003E2__current = gizmo;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00c8;
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
			_003CGetGizmos_003Ed__20 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__20(0)
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

	private UnnaturalCorpseTracker tracker;

	private bool disappear;

	private bool teleport;

	private int lastTeleport;

	private CompForbiddable compForbidInt;

	private const int MTBTeleportHours = 24;

	public CompForbiddable Forbiddable => compForbidInt ?? (compForbidInt = GetComp<CompForbiddable>());

	public UnnaturalCorpseTracker Tracker => tracker;

	public UnnaturalCorpse()
	{
		innerContainer.contentsLookMode = LookMode.Deep;
	}

	public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
	{
		base.Destroy(mode);
		tracker?.Notify_CorpseDestroyed();
	}

	public override void PostMake()
	{
		base.PostMake();
		lastTeleport = GenTicks.TicksGame;
		Forbiddable.Forbidden = true;
		this.TryGetComp<CompRottable>().disabled = true;
	}

	protected override void TickInterval(int delta)
	{
		base.TickInterval(delta);
		if (disappear)
		{
			DoDisappear();
		}
		else
		{
			if (base.Destroyed || (base.ParentHolder != null && !(base.ParentHolder is Map)))
			{
				return;
			}
			if (Rand.MTBEventOccurs(24f, 2500f, delta))
			{
				teleport = true;
			}
			if (teleport && IsOutsideView() && AnomalyUtility.TryGetNearbyUnseenCell(tracker.Haunted, out var pos))
			{
				if (base.MapHeld != tracker.Haunted.MapHeld)
				{
					SkipUtility.SkipTo(this, pos, tracker.Haunted.MapHeld);
				}
				else
				{
					base.Position = pos;
				}
				teleport = false;
				lastTeleport = GenTicks.TicksGame;
				base.Map.listerHaulables.Notify_Unforbidden(this);
			}
		}
	}

	public override void TickRare()
	{
		TickRareInt();
	}

	public override void Notify_Studied(Pawn studier, float amount, KnowledgeCategoryDef category = null)
	{
		base.Notify_Studied(studier, amount, category);
		if (category.IsAnomalyResearchCategory())
		{
			tracker.Notify_Studied(studier, amount);
		}
	}

	public void DoStudiedDeactivation(Pawn interactor)
	{
		IntVec3 positionHeld = base.PositionHeld;
		Map mapHeld = base.MapHeld;
		tracker.Notify_Finished();
		EffecterDefOf.MeatExplosion.Spawn(base.PositionHeld, base.MapHeld).Cleanup();
		Destroy();
		Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Shard), positionHeld, mapHeld);
		TaggedString label = "UnnaturalCorpseDeactivatedLetter".Translate();
		TaggedString text = "UnnaturalCorpseDeactivatedLetterDesc".Translate(interactor.Named("PAWN"));
		text += "\n\n" + "UnnaturalCorpseDeactivatedLetterDescAppended".Translate(tracker.Haunted.Named("PAWN"));
		Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.PositiveEvent, thing);
		if (tracker.Haunted.health.hediffSet.TryGetHediff(HediffDefOf.CorpseTorment, out var hediff))
		{
			tracker.Haunted.health.RemoveHediff(hediff);
		}
		Find.Anomaly.RemoveCorpseTracker(tracker.Haunted);
	}

	public void DoDisappear()
	{
		disappear = true;
		Thing spawnedParentOrMe = base.SpawnedParentOrMe;
		if (spawnedParentOrMe is Building_Casket building_Casket)
		{
			SendDisappearedLetter();
			building_Casket.EjectContents();
			Destroy();
		}
		else if (spawnedParentOrMe != this)
		{
			SendDisappearedLetter();
			base.ParentHolder.GetDirectlyHeldThings().TryDrop(this, ThingPlaceMode.Near, 1, out var _);
			SkipUtility.SkipDeSpawn(this);
			Destroy();
		}
		else if (IsOutsideView())
		{
			SendDisappearedLetter();
			Destroy();
		}
	}

	private void SendDisappearedLetter()
	{
		TaggedString label = "LetterLabelCorpseDisappeared".Translate();
		TaggedString text = "LetterCorpseDisappeared".Translate();
		if (!tracker.Haunted.DestroyedOrNull() && !tracker.Haunted.Dead)
		{
			text += " " + "LetterCorpseDisappearedPawnAliveAppend".Translate(tracker.Haunted.Named("PAWN"));
		}
		Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NeutralEvent);
	}

	public void LinkToTracker(UnnaturalCorpseTracker newTracker)
	{
		if (tracker != null)
		{
			Log.ErrorOnce("mysterious corpse " + Label + " was linked to a new tracker when it still had one assigned.", 62521241);
		}
		tracker = newTracker;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__20))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__20(-2)
		{
			_003C_003E4__this = this
		};
	}

	private bool IsOutsideView()
	{
		if (base.SpawnedOrAnyParentSpawned && base.MapHeld.reservationManager.IsReservedByAnyoneOf(this, Faction.OfPlayer))
		{
			return false;
		}
		if (Find.CurrentMap != base.MapHeld)
		{
			return true;
		}
		return !Find.CameraDriver.CurrentViewRect.ExpandedBy(1).Contains(base.PositionHeld);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref tracker, "tracker");
		Scribe_Values.Look(ref disappear, "disappears", defaultValue: false);
		Scribe_Values.Look(ref teleport, "teleport", defaultValue: false);
		Scribe_Values.Look(ref lastTeleport, "lastTeleport", 0);
		if (Scribe.mode == LoadSaveMode.PostLoadInit && tracker == null)
		{
			Log.Error("Failed to find corpse tracker, this should not happen. Destroying corpse.");
			Destroy();
		}
	}
}
