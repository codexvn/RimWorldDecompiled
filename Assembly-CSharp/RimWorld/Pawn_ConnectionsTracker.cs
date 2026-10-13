using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020024B0 RID: 9392
	public class Pawn_ConnectionsTracker : IExposable
	{
		// Token: 0x17001F18 RID: 7960
		// (get) Token: 0x0600CBF8 RID: 52216 RVA: 0x003BC372 File Offset: 0x003BA572
		public List<Thing> ConnectedThings
		{
			get
			{
				return this.connectedThings;
			}
		}

		// Token: 0x0600CBF9 RID: 52217 RVA: 0x003BC37A File Offset: 0x003BA57A
		public Pawn_ConnectionsTracker()
		{
		}

		// Token: 0x0600CBFA RID: 52218 RVA: 0x003BC38D File Offset: 0x003BA58D
		public Pawn_ConnectionsTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CBFB RID: 52219 RVA: 0x003BC3A7 File Offset: 0x003BA5A7
		public void ConnectTo(Thing thing)
		{
			this.connectedThings.Add(thing);
		}

		// Token: 0x0600CBFC RID: 52220 RVA: 0x003BC3B8 File Offset: 0x003BA5B8
		public void Notify_PawnKilled()
		{
			for (int i = this.connectedThings.Count - 1; i >= 0; i--)
			{
				CompTreeConnection compTreeConnection = this.connectedThings[i].TryGetComp<CompTreeConnection>();
				if (compTreeConnection != null)
				{
					compTreeConnection.Notify_PawnDied(this.pawn);
				}
				this.connectedThings.RemoveAt(i);
			}
		}

		// Token: 0x0600CBFD RID: 52221 RVA: 0x003BC40C File Offset: 0x003BA60C
		public void Notify_ConnectedThingDestroyed(Thing thing)
		{
			if (this.connectedThings.Remove(thing))
			{
				Pawn_NeedsTracker needs = this.pawn.needs;
				bool flag;
				if (needs == null)
				{
					flag = (null != null);
				}
				else
				{
					Need_Mood mood = needs.mood;
					if (mood == null)
					{
						flag = (null != null);
					}
					else
					{
						ThoughtHandler thoughts = mood.thoughts;
						flag = (((thoughts != null) ? thoughts.memories : null) != null);
					}
				}
				if (flag)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.ConnectedTreeDied, null, null);
				}
			}
		}

		// Token: 0x0600CBFE RID: 52222 RVA: 0x003BC47E File Offset: 0x003BA67E
		public void Notify_ConnectedThingLeftBehind(Thing thing)
		{
			this.connectedThings.Remove(thing);
		}

		// Token: 0x0600CBFF RID: 52223 RVA: 0x003BC48D File Offset: 0x003BA68D
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (!this.connectedThings.Any<Thing>())
			{
				yield break;
			}
			if (Find.Selector.SingleSelectedThing == this.pawn)
			{
				if (this.pawn.IsColonistPlayerControlled)
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandSelectConnectedTree".Translate(),
						onHover = new Action(this.DrawConnectionLines),
						icon = Widgets.GetIconFor(ThingDefOf.Plant_TreeGauranlen, null, null, null),
						action = delegate()
						{
							if (this.ConnectedThings.Count == 1)
							{
								CameraJumper.TryJumpAndSelect(this.ConnectedThings[0], CameraJumper.MovementMode.Pan);
								return;
							}
							if (this.ConnectedThings.Count > 1)
							{
								List<FloatMenuOption> list = new List<FloatMenuOption>();
								for (int i = 0; i < this.ConnectedThings.Count; i++)
								{
									Thing t = this.ConnectedThings[i];
									string str = "NoCaste".Translate();
									CompTreeConnection compTreeConnection = t.TryGetComp<CompTreeConnection>();
									if (compTreeConnection != null && compTreeConnection.Mode != null)
									{
										str = compTreeConnection.Mode.label;
									}
									list.Add(new FloatMenuOption(t.LabelCap + " (" + str + ")", delegate()
									{
										CameraJumper.TryJumpAndSelect(t, CameraJumper.MovementMode.Pan);
									}, t.def, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null));
								}
								if (list.Any<FloatMenuOption>())
								{
									Find.WindowStack.Add(new FloatMenu(list));
								}
							}
						}
					};
				}
				else if (this.pawn.Faction == Faction.OfPlayer && this.pawn.RaceProps.Dryad)
				{
					yield return new Command_Toggle
					{
						defaultLabel = "CommandHealInHealingPod".Translate(),
						defaultDesc = "CommandHealInHealingPodDesc".Translate(ThingDefOf.DryadHealingPod.GetCompProperties<CompProperties_DryadCocoon>().daysToComplete.Named("DAYS")),
						icon = Widgets.GetIconFor(ThingDefOf.DryadHealingPod, null, null, null),
						isActive = (() => this.pawn.mindState.returnToHealingPod),
						toggleAction = delegate()
						{
							this.pawn.mindState.returnToHealingPod = !this.pawn.mindState.returnToHealingPod;
						}
					};
				}
			}
			yield break;
		}

		// Token: 0x0600CC00 RID: 52224 RVA: 0x003BC4A0 File Offset: 0x003BA6A0
		private void DrawConnectionLines()
		{
			foreach (Thing t in this.ConnectedThings)
			{
				this.DrawConnectionLine(t);
			}
		}

		// Token: 0x0600CC01 RID: 52225 RVA: 0x003BC4F4 File Offset: 0x003BA6F4
		public void DrawConnectionLine(Thing t)
		{
			if (t.Spawned && t.Map == this.pawn.Map)
			{
				GenDraw.DrawLineBetween(this.pawn.TrueCenter(), t.TrueCenter(), SimpleColor.Orange, 0.2f);
			}
		}

		// Token: 0x0600CC02 RID: 52226 RVA: 0x003BC530 File Offset: 0x003BA730
		public void ExposeData()
		{
			Scribe_Collections.Look<Thing>(ref this.connectedThings, "connectedThings", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.connectedThings.RemoveAll((Thing x) => x == null || x.Destroyed);
			}
		}

		// Token: 0x04008B12 RID: 35602
		private Pawn pawn;

		// Token: 0x04008B13 RID: 35603
		private List<Thing> connectedThings = new List<Thing>();
	}
}
