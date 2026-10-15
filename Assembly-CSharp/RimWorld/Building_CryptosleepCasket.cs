using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

public class Building_CryptosleepCasket : Building_Casket
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public Pawn myPawn;

		public Building_CryptosleepCasket _003C_003E4__this;

		public JobDef jobDef;

		public Action _003C_003E9__1;

		internal void _003CGetFloatMenuOptions_003Eb__0()
		{
			if (ModsConfig.BiotechActive)
			{
				if (!(myPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond) is Hediff_PsychicBond hediff_PsychicBond) || !ThoughtWorker_PsychicBondProximity.NearPsychicBondedPerson(myPawn, hediff_PsychicBond))
				{
					myPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(jobDef, _003C_003E4__this), JobTag.Misc);
					return;
				}
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("PsychicBondDistanceWillBeActive_Cryptosleep".Translate(myPawn.Named("PAWN"), ((Pawn)hediff_PsychicBond.target).Named("BOND")), delegate
				{
					myPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(jobDef, _003C_003E4__this), JobTag.Misc);
				}, destructive: true));
			}
			else
			{
				myPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(jobDef, _003C_003E4__this), JobTag.Misc);
			}
		}

		internal void _003CGetFloatMenuOptions_003Eb__1()
		{
			myPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(jobDef, _003C_003E4__this), JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__2 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn myPawn;

		public Pawn _003C_003E3__myPawn;

		public Building_CryptosleepCasket _003C_003E4__this;

		private _003C_003Ec__DisplayClass2_0 _003C_003E8__1;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

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
		public _003CGetFloatMenuOptions_003Ed__2(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Building_CryptosleepCasket building_CryptosleepCasket = _003C_003E4__this;
				string label;
				Action action;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass2_0();
					_003C_003E8__1.myPawn = myPawn;
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					if (_003C_003E8__1.myPawn.IsQuestLodger())
					{
						FloatMenuOption floatMenuOption = new FloatMenuOption("CannotUseReason".Translate("CryptosleepCasketGuestsNotAllowed".Translate()), null);
						_003C_003E2__current = floatMenuOption;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003E7__wrap1 = ((ThingWithComps)building_CryptosleepCasket).GetFloatMenuOptions(_003C_003E8__1.myPawn).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_011a;
				case 1:
					_003C_003E1__state = -1;
					return false;
				case 2:
					_003C_003E1__state = -3;
					goto IL_011a;
				case 3:
					_003C_003E1__state = -1;
					break;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_011a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (building_CryptosleepCasket.innerContainer.Count != 0)
					{
						break;
					}
					if (!_003C_003E8__1.myPawn.CanReach(building_CryptosleepCasket, PathEndMode.InteractionCell, Danger.Deadly))
					{
						FloatMenuOption floatMenuOption2 = new FloatMenuOption("CannotUseNoPath".Translate(), null);
						_003C_003E2__current = floatMenuOption2;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003E8__1.jobDef = JobDefOf.EnterCryptosleepCasket;
					label = "EnterCryptosleepCasket".Translate();
					action = delegate
					{
						if (ModsConfig.BiotechActive)
						{
							if (!(_003C_003E8__1.myPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.PsychicBond) is Hediff_PsychicBond hediff_PsychicBond) || !ThoughtWorker_PsychicBondProximity.NearPsychicBondedPerson(_003C_003E8__1.myPawn, hediff_PsychicBond))
							{
								_003C_003E8__1.myPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(_003C_003E8__1.jobDef, _003C_003E8__1._003C_003E4__this), JobTag.Misc);
							}
							else
							{
								Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("PsychicBondDistanceWillBeActive_Cryptosleep".Translate(_003C_003E8__1.myPawn.Named("PAWN"), ((Pawn)hediff_PsychicBond.target).Named("BOND")), delegate
								{
									_003C_003E8__1.myPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(_003C_003E8__1.jobDef, _003C_003E8__1._003C_003E4__this), JobTag.Misc);
								}, destructive: true));
							}
						}
						else
						{
							_003C_003E8__1.myPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(_003C_003E8__1.jobDef, _003C_003E8__1._003C_003E4__this), JobTag.Misc);
						}
					};
					_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, action), _003C_003E8__1.myPawn, building_CryptosleepCasket);
					_003C_003E1__state = 4;
					return true;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetFloatMenuOptions_003Ed__2 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__2(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetFloatMenuOptions_003Ed__.myPawn = _003C_003E3__myPawn;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__3 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_CryptosleepCasket _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__3(int _003C_003E1__state)
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
				Building_CryptosleepCasket building_CryptosleepCasket = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building_Casket)building_CryptosleepCasket).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (building_CryptosleepCasket.Faction == Faction.OfPlayer && building_CryptosleepCasket.innerContainer.Count > 0 && building_CryptosleepCasket.def.building.isPlayerEjectable)
					{
						Command_Action command_Action = new Command_Action
						{
							action = building_CryptosleepCasket.EjectContents,
							defaultLabel = "CommandPodEject".Translate(),
							defaultDesc = "CommandPodEjectDesc".Translate()
						};
						if (building_CryptosleepCasket.innerContainer.Count == 0)
						{
							command_Action.Disable("CommandPodEjectFailEmpty".Translate());
						}
						command_Action.hotKey = KeyBindingDefOf.Misc8;
						command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/PodEject");
						_003C_003E2__current = command_Action;
						_003C_003E1__state = 2;
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
			_003CGetGizmos_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__3(0)
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

	private static List<ThingDef> cachedCaskets;

	public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
	{
		if (base.TryAcceptThing(thing, allowSpecialEffects))
		{
			if (allowSpecialEffects)
			{
				SoundDefOf.CryptosleepCasket_Accept.PlayOneShot(new TargetInfo(base.Position, base.Map));
			}
			return true;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__2))]
	public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__2(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__myPawn = myPawn
		};
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__3))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__3(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void EjectContents()
	{
		ThingDef filth_Slime = ThingDefOf.Filth_Slime;
		foreach (Thing item in (IEnumerable<Thing>)innerContainer)
		{
			if (item is Pawn pawn)
			{
				PawnComponentsUtility.AddComponentsForSpawn(pawn);
				pawn.filth.GainFilth(filth_Slime);
				if (pawn.RaceProps.IsFlesh)
				{
					pawn.health.AddHediff(HediffDefOf.CryptosleepSickness, null, null);
				}
			}
		}
		if (!base.Destroyed)
		{
			SoundDefOf.CryptosleepCasket_Eject.PlayOneShot(SoundInfo.InMap(new TargetInfo(base.Position, base.Map)));
		}
		base.EjectContents();
	}

	public static Building_CryptosleepCasket FindCryptosleepCasketFor(Pawn p, Pawn traveler, bool ignoreOtherReservations = false)
	{
		if (cachedCaskets == null)
		{
			cachedCaskets = DefDatabase<ThingDef>.AllDefs.Where((ThingDef def) => def.IsCryptosleepCasket).ToList();
		}
		foreach (ThingDef cachedCasket in cachedCaskets)
		{
			bool queuing = KeyBindingDefOf.QueueOrder.IsDownEvent;
			Building_CryptosleepCasket building_CryptosleepCasket = (Building_CryptosleepCasket)GenClosest.ClosestThingReachable(p.PositionHeld, p.MapHeld, ThingRequest.ForDef(cachedCasket), PathEndMode.InteractionCell, TraverseParms.For(traveler), 9999f, Validator);
			if (building_CryptosleepCasket != null)
			{
				return building_CryptosleepCasket;
			}
			bool Validator(Thing x)
			{
				if (!((Building_CryptosleepCasket)x).HasAnyContents && (!queuing || !traveler.HasReserved(x)))
				{
					return traveler.CanReserve(x, 1, -1, null, ignoreOtherReservations);
				}
				return false;
			}
		}
		return null;
	}
}
