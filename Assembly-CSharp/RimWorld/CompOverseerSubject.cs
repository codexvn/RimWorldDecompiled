using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public class CompOverseerSubject : ThingComp
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass23_0
	{
		public Pawn localPawn;

		internal void _003CCompGetGizmosExtra_003Eb__2()
		{
			foreach (Pawn item in Find.Selector.SelectedPawns.Where((Pawn p) => p.RaceProps.IsMechanoid))
			{
				item.GetOverseer()?.relations.RemoveDirectRelation(PawnRelationDefOf.Overseer, item);
				item.SetFaction(Faction.OfPlayer);
				localPawn.relations.AddDirectRelation(PawnRelationDefOf.Overseer, item);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__23 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompOverseerSubject _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__23(int _003C_003E1__state)
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
				CompOverseerSubject CS_0024_003C_003E8__locals8 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingComp)CS_0024_003C_003E8__locals8).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Make feral (event)",
						action = delegate
						{
							CS_0024_003C_003E8__locals8.TryMakeFeral();
						}
					};
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Assign to overseer",
						action = delegate
						{
							List<FloatMenuOption> list = new List<FloatMenuOption>();
							List<Pawn> freeColonists = CS_0024_003C_003E8__locals8.parent.Map.mapPawns.FreeColonists;
							for (int i = 0; i < freeColonists.Count; i++)
							{
								_003C_003Ec__DisplayClass23_0 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass23_0
								{
									localPawn = freeColonists[i]
								};
								if (MechanitorUtility.IsMechanitor(CS_0024_003C_003E8__locals5.localPawn))
								{
									list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals5.localPawn.LabelShortCap, delegate
									{
										foreach (Pawn item in Find.Selector.SelectedPawns.Where((Pawn p) => p.RaceProps.IsMechanoid))
										{
											item.GetOverseer()?.relations.RemoveDirectRelation(PawnRelationDefOf.Overseer, item);
											item.SetFaction(Faction.OfPlayer);
											CS_0024_003C_003E8__locals5.localPawn.relations.AddDirectRelation(PawnRelationDefOf.Overseer, item);
										}
									}));
								}
							}
							if (list.Any())
							{
								Find.WindowStack.Add(new FloatMenu(list));
							}
						}
					};
					_003C_003E1__state = 4;
					return true;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Make feral",
							action = CS_0024_003C_003E8__locals8.ForceFeral
						};
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
			_003CCompGetGizmosExtra_003Ed__23 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__23(0)
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

	private int delayUntilFeralCheck;

	private Effecter effect;

	public static bool debugDisableNeedsOverseerEffect;

	private List<Pawn> tmpFeralPawns = new List<Pawn>();

	public CompProperties_OverseerSubject Props => (CompProperties_OverseerSubject)props;

	public Pawn Parent => (Pawn)parent;

	public OverseerSubjectState State
	{
		get
		{
			if (Overseer?.mechanitor == null)
			{
				return OverseerSubjectState.RequiresOverseer;
			}
			if (Overseer.mechanitor.ControlledPawns.Contains(Parent))
			{
				return OverseerSubjectState.Overseen;
			}
			return OverseerSubjectState.RequiresBandwidth;
		}
	}

	private Pawn Overseer => ((Pawn)parent)?.relations?.GetFirstDirectRelationPawn(PawnRelationDefOf.Overseer);

	public int DelayUntilFeralCheckTicks => delayUntilFeralCheck;

	public void Notify_DisconnectedFromOverseer()
	{
		if (Parent.Drafted)
		{
			Parent.drafter.Drafted = false;
		}
		if (Parent.carryTracker?.CarriedThing != null)
		{
			Parent.carryTracker.TryDropCarriedThing(Parent.Position, ThingPlaceMode.Near, out var _);
		}
		Parent.jobs?.EndCurrentJob(JobCondition.InterruptForced);
		Parent.jobs?.CheckForJobOverride();
		delayUntilFeralCheck = Props.delayUntilFeralCheck;
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (!respawningAfterLoad)
		{
			delayUntilFeralCheck = Props.delayUntilFeralCheck;
		}
	}

	public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
	{
		if (!ModLister.CheckBiotech("Overseer subject"))
		{
			parent.Destroy();
			return;
		}
		effect?.Cleanup();
		effect = null;
		base.PostDeSpawn(map, mode);
	}

	public override void CompTick()
	{
		base.CompTick();
		if (!debugDisableNeedsOverseerEffect && CanGoFeral(Parent))
		{
			if (Props.needsOverseerEffect != null && effect == null)
			{
				effect = Props.needsOverseerEffect.SpawnAttached(parent, parent.Map);
			}
		}
		else
		{
			effect?.Cleanup();
			effect = null;
		}
		effect?.EffectTick(parent, parent);
		if (CanGoFeral(Parent))
		{
			if (delayUntilFeralCheck > 0)
			{
				delayUntilFeralCheck--;
			}
			if (delayUntilFeralCheck <= 0 && Rand.MTBEventOccurs(Props.feralMtbDays, 60000f, 1f))
			{
				TryMakeFeral();
			}
		}
	}

	private static bool CanGoFeral(Pawn pawn)
	{
		if (!pawn.Spawned || !pawn.Awake())
		{
			return false;
		}
		return pawn.IsColonyMechRequiringMechanitor();
	}

	public bool TryMakeFeral()
	{
		tmpFeralPawns.Clear();
		tmpFeralPawns.Add(Parent);
		foreach (Thing item in GenRadial.RadialDistinctThingsAround(parent.Position, parent.Map, Props.feralCascadeRadialDistance, useCenter: true))
		{
			if (item is Pawn pawn && !tmpFeralPawns.Contains(pawn) && CanGoFeral(pawn))
			{
				tmpFeralPawns.Add(pawn);
			}
		}
		for (int i = 0; i < tmpFeralPawns.Count; i++)
		{
			tmpFeralPawns[i].OverseerSubject.ForceFeral();
		}
		IEnumerable<IGrouping<PawnKindDef, Pawn>> source = from p in tmpFeralPawns
			group p by p.kindDef;
		Find.LetterStack.ReceiveLetter("LetterLabelMechsFeral".Translate(), "LetterMechsFeral".Translate(Faction.OfMechanoids, source.Select((IGrouping<PawnKindDef, Pawn> g) => string.Concat(g.Key.LabelCap + " x", g.Count().ToString())).ToLineList(" - ")), LetterDefOf.ThreatBig, tmpFeralPawns);
		for (int j = 0; j < tmpFeralPawns.Count; j++)
		{
			tmpFeralPawns[j].GetLord()?.Notify_PawnLost(tmpFeralPawns[j], PawnLostCondition.ForcedToJoinOtherLord, null);
		}
		LordMaker.MakeNewLord(Faction.OfMechanoids, new LordJob_ExitMapBest(LocomotionUrgency.Jog, canDig: false, canDefendSelf: true), parent.MapHeld, tmpFeralPawns);
		for (int k = 0; k < tmpFeralPawns.Count; k++)
		{
			if (tmpFeralPawns[k].CurJob != null)
			{
				tmpFeralPawns[k].jobs.EndCurrentJob(JobCondition.InterruptForced);
			}
		}
		tmpFeralPawns.Clear();
		return true;
	}

	private void ForceFeral()
	{
		Pawn overseer = Overseer;
		if (overseer != null)
		{
			Messages.Message("MessageMechanitorLostControlOfMech".Translate(overseer, Parent) + ": " + Parent.LabelShortCap, new LookTargets(new Pawn[2] { Parent, overseer }), MessageTypeDefOf.NeutralEvent);
			Parent.relations.RemoveDirectRelation(PawnRelationDefOf.Overseer, overseer);
		}
		parent.SetFaction(Faction.OfMechanoids);
	}

	public override string CompInspectStringExtra()
	{
		if (parent.Faction == Faction.OfPlayer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Pawn overseer = Overseer;
			TaggedString taggedString = "Overseer".Translate();
			if (overseer?.mechanitor != null)
			{
				taggedString += ": " + Overseer.LabelShort;
				if (!overseer.mechanitor.ControlledPawns.Contains(parent))
				{
					taggedString += " (" + "InsufficientBandwidth".Translate() + ")";
				}
			}
			else
			{
				taggedString += ": " + "OverseerNone".Translate();
			}
			stringBuilder.Append(taggedString);
			if (State != OverseerSubjectState.Overseen)
			{
				TaggedString ts = ((delayUntilFeralCheck > 0) ? ("Uncontrolled".Translate() + " (" + (Props.delayUntilFeralCheck - delayUntilFeralCheck).ToStringTicksToPeriod(allowSeconds: true, shortForm: true) + ")") : ("Danger".Translate() + ": " + "MayGoFeral".Translate()));
				stringBuilder.AppendInNewLine(ts.Colorize(ColorLibrary.RedReadable));
			}
			return stringBuilder.ToString();
		}
		return null;
	}

	public override void Notify_AbandonedAtTile(PlanetTile tile)
	{
		base.Notify_AbandonedAtTile(tile);
		Pawn overseer = Overseer;
		if (overseer != null)
		{
			Parent.relations.RemoveDirectRelation(PawnRelationDefOf.Overseer, overseer);
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__23))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__23(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref delayUntilFeralCheck, "delayUntilFeralCheck", 0);
	}
}
