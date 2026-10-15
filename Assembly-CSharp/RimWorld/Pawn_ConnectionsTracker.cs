using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class Pawn_ConnectionsTracker : IExposable
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass10_0
	{
		public Thing t;

		internal void _003CGetGizmos_003Eb__1()
		{
			CameraJumper.TryJumpAndSelect(t);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__10 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Pawn_ConnectionsTracker _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__10(int _003C_003E1__state)
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
			Pawn_ConnectionsTracker CS_0024_003C_003E8__locals19 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!CS_0024_003C_003E8__locals19.connectedThings.Any())
				{
					return false;
				}
				if (Find.Selector.SingleSelectedThing != CS_0024_003C_003E8__locals19.pawn)
				{
					break;
				}
				if (CS_0024_003C_003E8__locals19.pawn.IsColonistPlayerControlled)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "CommandSelectConnectedTree".Translate(),
						onHover = CS_0024_003C_003E8__locals19.DrawConnectionLines,
						icon = Widgets.GetIconFor(ThingDefOf.Plant_TreeGauranlen, null, null, null),
						action = delegate
						{
							if (CS_0024_003C_003E8__locals19.ConnectedThings.Count == 1)
							{
								CameraJumper.TryJumpAndSelect(CS_0024_003C_003E8__locals19.ConnectedThings[0]);
							}
							else if (CS_0024_003C_003E8__locals19.ConnectedThings.Count > 1)
							{
								List<FloatMenuOption> list = new List<FloatMenuOption>();
								for (int i = 0; i < CS_0024_003C_003E8__locals19.ConnectedThings.Count; i++)
								{
									_003C_003Ec__DisplayClass10_0 CS_0024_003C_003E8__locals14 = new _003C_003Ec__DisplayClass10_0
									{
										t = CS_0024_003C_003E8__locals19.ConnectedThings[i]
									};
									string text = "NoCaste".Translate();
									CompTreeConnection compTreeConnection = CS_0024_003C_003E8__locals14.t.TryGetComp<CompTreeConnection>();
									if (compTreeConnection != null && compTreeConnection.Mode != null)
									{
										text = compTreeConnection.Mode.label;
									}
									list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals14.t.LabelCap + " (" + text + ")", delegate
									{
										CameraJumper.TryJumpAndSelect(CS_0024_003C_003E8__locals14.t);
									}, CS_0024_003C_003E8__locals14.t.def, null, forceBasicStyle: false, MenuOptionPriority.Default, null, null, 0f, null, null, playSelectionSound: true, 0, null));
								}
								if (list.Any())
								{
									Find.WindowStack.Add(new FloatMenu(list));
								}
							}
						}
					};
					_003C_003E1__state = 1;
					return true;
				}
				if (CS_0024_003C_003E8__locals19.pawn.Faction == Faction.OfPlayer && CS_0024_003C_003E8__locals19.pawn.RaceProps.Dryad)
				{
					_003C_003E2__current = new Command_Toggle
					{
						defaultLabel = "CommandHealInHealingPod".Translate(),
						defaultDesc = "CommandHealInHealingPodDesc".Translate(ThingDefOf.DryadHealingPod.GetCompProperties<CompProperties_DryadCocoon>().daysToComplete.Named("DAYS")),
						icon = Widgets.GetIconFor(ThingDefOf.DryadHealingPod, null, null, null),
						isActive = () => CS_0024_003C_003E8__locals19.pawn.mindState.returnToHealingPod,
						toggleAction = delegate
						{
							CS_0024_003C_003E8__locals19.pawn.mindState.returnToHealingPod = !CS_0024_003C_003E8__locals19.pawn.mindState.returnToHealingPod;
						}
					};
					_003C_003E1__state = 2;
					return true;
				}
				break;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__10(0)
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

	private Pawn pawn;

	private List<Thing> connectedThings = new List<Thing>();

	public List<Thing> ConnectedThings => connectedThings;

	public Pawn_ConnectionsTracker()
	{
	}

	public Pawn_ConnectionsTracker(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void ConnectTo(Thing thing)
	{
		connectedThings.Add(thing);
	}

	public void Notify_PawnKilled()
	{
		for (int num = connectedThings.Count - 1; num >= 0; num--)
		{
			connectedThings[num].TryGetComp<CompTreeConnection>()?.Notify_PawnDied(pawn);
			connectedThings.RemoveAt(num);
		}
	}

	public void Notify_ConnectedThingDestroyed(Thing thing)
	{
		if (connectedThings.Remove(thing) && pawn.needs?.mood?.thoughts?.memories != null)
		{
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ConnectedTreeDied);
		}
	}

	public void Notify_ConnectedThingLeftBehind(Thing thing)
	{
		connectedThings.Remove(thing);
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__10))]
	public IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__10(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void DrawConnectionLines()
	{
		foreach (Thing connectedThing in ConnectedThings)
		{
			DrawConnectionLine(connectedThing);
		}
	}

	public void DrawConnectionLine(Thing t)
	{
		if (t.Spawned && t.Map == pawn.Map)
		{
			GenDraw.DrawLineBetween(pawn.TrueCenter(), t.TrueCenter(), SimpleColor.Orange);
		}
	}

	public void ExposeData()
	{
		Scribe_Collections.Look(ref connectedThings, "connectedThings", LookMode.Reference);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			connectedThings.RemoveAll((Thing x) => x?.Destroyed ?? true);
		}
	}
}
