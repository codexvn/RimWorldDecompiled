using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002947 RID: 10567
	public class MinifiedTree : MinifiedThing, IThingHolderTickable, IThingHolder
	{
		// Token: 0x17002327 RID: 8999
		// (get) Token: 0x0600E76E RID: 59246 RVA: 0x00002501 File Offset: 0x00000701
		public bool ShouldTickContents
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002328 RID: 9000
		// (get) Token: 0x0600E76F RID: 59247 RVA: 0x0043706E File Offset: 0x0043526E
		public int TicksTillDeath
		{
			get
			{
				return this.ticksTillDeath;
			}
		}

		// Token: 0x17002329 RID: 9001
		// (get) Token: 0x0600E770 RID: 59248 RVA: 0x00437076 File Offset: 0x00435276
		public Plant InnerTree
		{
			get
			{
				return (Plant)base.InnerThing;
			}
		}

		// Token: 0x1700232A RID: 9002
		// (get) Token: 0x0600E771 RID: 59249 RVA: 0x00437084 File Offset: 0x00435284
		public override Graphic Graphic
		{
			get
			{
				if (this.cachedGraphic == null)
				{
					this.cachedGraphic = base.InnerThing.Graphic.ExtractInnerGraphicFor(base.InnerThing, null);
					Vector2 minifiedDrawSize = base.GetMinifiedDrawSize(base.InnerThing.def.size.ToVector2(), 1.1f);
					Vector2 newDrawSize = new Vector2(minifiedDrawSize.x / (float)base.InnerThing.def.size.x * this.cachedGraphic.drawSize.x, minifiedDrawSize.y / (float)base.InnerThing.def.size.z * this.cachedGraphic.drawSize.y);
					this.cachedGraphic = this.cachedGraphic.GetCopy(newDrawSize, ShaderTypeDefOf.Cutout.Shader);
				}
				return this.cachedGraphic;
			}
		}

		// Token: 0x0600E772 RID: 59250 RVA: 0x00437167 File Offset: 0x00435367
		protected override Graphic LoadCrateFrontGraphic()
		{
			return GraphicDatabase.Get<Graphic_Single>("Things/Item/Minified/BurlapBag", ShaderDatabase.Cutout, base.GetMinifiedDrawSize(base.InnerThing.def.size.ToVector2(), 1.1f) * 1.16f, Color.white);
		}

		// Token: 0x0600E773 RID: 59251 RVA: 0x004371A8 File Offset: 0x004353A8
		private void RecordTreeDeath(LookTargets lookTargets)
		{
			if (base.InnerThing.def.plant.treeLoversCareIfChopped)
			{
				using (IEnumerator<Ideo> enumerator = Faction.OfPlayer.ideos.AllIdeos.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.WarnPlayerOnDesignateChopTree)
						{
							Messages.Message("MessageMinifiedTreeDied".Translate(), lookTargets, MessageTypeDefOf.NegativeEvent, true);
							break;
						}
					}
				}
			}
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.MinifiedTreeDied, new SignalArgs(base.InnerThing.Named(HistoryEventArgsNames.Victim))), true);
		}

		// Token: 0x0600E774 RID: 59252 RVA: 0x0043725C File Offset: 0x0043545C
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (base.InnerThing == null)
			{
				base.Destroy(mode);
				return;
			}
			Caravan anyParent = ThingOwnerUtility.GetAnyParent<Caravan>(this);
			ActiveTransporterInfo activeTransporterInfo = base.ParentHolder as ActiveTransporterInfo;
			this.RecordTreeDeath((anyParent == null) ? new LookTargets(base.PositionHeld, base.MapHeld) : anyParent);
			ThingDef harvestedThingDef = base.InnerThing.def.plant.harvestedThingDef;
			int i = (int)((float)((Plant)base.InnerThing).YieldNow() * 0.5f);
			List<Thing> list = new List<Thing>();
			while (i > 0)
			{
				int num = Mathf.Min(i, harvestedThingDef.stackLimit);
				Thing thing = ThingMaker.MakeThing(harvestedThingDef, null);
				thing.stackCount = num;
				list.Add(thing);
				i -= num;
			}
			IntVec3 center = (anyParent == null) ? base.PositionHeld : IntVec3.Invalid;
			Map map = (anyParent == null) ? base.MapHeld : null;
			if (base.ParentHolder != null)
			{
				base.ParentHolder.GetDirectlyHeldThings().Remove(this);
			}
			base.Destroy(mode);
			if (anyParent != null)
			{
				using (List<Thing>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Thing thing2 = enumerator.Current;
						anyParent.AddPawnOrItem(thing2, true);
					}
					return;
				}
			}
			if (activeTransporterInfo != null)
			{
				using (List<Thing>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Thing item = enumerator.Current;
						activeTransporterInfo.innerContainer.TryAdd(item, true);
					}
					return;
				}
			}
			if (map != null)
			{
				foreach (Thing thing3 in list)
				{
					GenPlace.TryPlaceThing(thing3, center, map, ThingPlaceMode.Near, null, null, null, 1);
				}
			}
		}

		// Token: 0x0600E775 RID: 59253 RVA: 0x00437444 File Offset: 0x00435644
		public override void PostMake()
		{
			base.PostMake();
			this.ticksTillDeath = 420000;
		}

		// Token: 0x0600E776 RID: 59254 RVA: 0x00437457 File Offset: 0x00435657
		protected override void Tick()
		{
			base.Tick();
			this.ticksTillDeath--;
			if (this.ticksTillDeath <= 0)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600E777 RID: 59255 RVA: 0x0043747D File Offset: 0x0043567D
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Destroy",
					action = delegate()
					{
						this.Destroy(DestroyMode.Vanish);
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Die in 1 hour",
					action = delegate()
					{
						this.ticksTillDeath = 2500;
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Die in 1 day",
					action = delegate()
					{
						this.ticksTillDeath = 60000;
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0600E778 RID: 59256 RVA: 0x0043748D File Offset: 0x0043568D
		public override void Notify_MyMapRemoved()
		{
			base.Notify_MyMapRemoved();
			this.RecordTreeDeath(null);
		}

		// Token: 0x0600E779 RID: 59257 RVA: 0x0043749C File Offset: 0x0043569C
		public override string GetInspectString()
		{
			return "MinifiedTreeWillDieIn".Translate(this.ticksTillDeath.ToStringTicksToPeriod(true, false, true, true, false).Named("time"));
		}

		// Token: 0x0600E77A RID: 59258 RVA: 0x004374C7 File Offset: 0x004356C7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksTillDeath, "ticksTillDeath", 0, false);
		}

		// Token: 0x04009A96 RID: 39574
		private int ticksTillDeath;

		// Token: 0x04009A97 RID: 39575
		public const int InitialTicksTillDeath = 420000;

		// Token: 0x04009A98 RID: 39576
		public const float DyingYieldPercentage = 0.5f;
	}
}
