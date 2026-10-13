using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002C6A RID: 11370
	public abstract class CompDryadHolder : ThingComp, IThingHolder, ISuspendableThingHolder
	{
		// Token: 0x1700282B RID: 10283
		// (get) Token: 0x0600FEE5 RID: 65253 RVA: 0x0049E3F7 File Offset: 0x0049C5F7
		public CompProperties_DryadCocoon Props
		{
			get
			{
				return (CompProperties_DryadCocoon)this.props;
			}
		}

		// Token: 0x1700282C RID: 10284
		// (get) Token: 0x0600FEE6 RID: 65254 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool IsContentsSuspended
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700282D RID: 10285
		// (get) Token: 0x0600FEE7 RID: 65255 RVA: 0x0049E404 File Offset: 0x0049C604
		protected CompTreeConnection TreeComp
		{
			get
			{
				if (this.cachedTreeComp == null)
				{
					Thing thing = this.tree;
					this.cachedTreeComp = ((thing != null) ? thing.TryGetComp<CompTreeConnection>() : null);
				}
				return this.cachedTreeComp;
			}
		}

		// Token: 0x1700282E RID: 10286
		// (get) Token: 0x0600FEE8 RID: 65256 RVA: 0x0049E42C File Offset: 0x0049C62C
		private Material FrontMat
		{
			get
			{
				if (this.cachedFrontMat == null)
				{
					this.cachedFrontMat = MaterialPool.MatFrom("Things/Building/Misc/DryadSphere/DryadSphereFront", ShaderDatabase.Cutout);
				}
				return this.cachedFrontMat;
			}
		}

		// Token: 0x0600FEE9 RID: 65257 RVA: 0x0049E457 File Offset: 0x0049C657
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.innerContainer = new ThingOwner<Thing>(this, false, LookMode.Deep, true);
			}
		}

		// Token: 0x0600FEEA RID: 65258 RVA: 0x0049E478 File Offset: 0x0049C678
		public override void CompTick()
		{
			if (this.tickComplete < 0)
			{
				return;
			}
			if (this.tree == null || this.tree.Destroyed)
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			if (Find.TickManager.TicksGame >= this.tickComplete)
			{
				this.Complete();
			}
		}

		// Token: 0x0600FEEB RID: 65259 RVA: 0x0049E4CC File Offset: 0x0049C6CC
		public override void PostDraw()
		{
			if (this.Props.drawContents)
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					this.innerContainer[i].DrawNowAt(this.parent.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop), false);
				}
				Matrix4x4 matrix = default(Matrix4x4);
				Vector3 pos = this.parent.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop.AltitudeFor() + 0.01f);
				Quaternion q = Quaternion.Euler(0f, this.parent.Rotation.AsAngle, 0f);
				Vector3 s = new Vector3(this.parent.Graphic.drawSize.x, 1f, this.parent.Graphic.drawSize.y);
				matrix.SetTRS(pos, q, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, this.FrontMat, 0);
			}
		}

		// Token: 0x0600FEEC RID: 65260 RVA: 0x0049E5CC File Offset: 0x0049C7CC
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.tickComplete >= 0 && DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Complete",
					action = new Action(this.Complete)
				};
			}
			yield break;
		}

		// Token: 0x0600FEED RID: 65261 RVA: 0x0049E5DC File Offset: 0x0049C7DC
		public virtual void TryAcceptPawn(Pawn p)
		{
			bool flag = p.DeSpawnOrDeselect(DestroyMode.Vanish);
			this.innerContainer.TryAddOrTransfer(p, 1, true);
			if (flag)
			{
				Find.Selector.Select(p, false, false);
			}
			SoundDefOf.Pawn_EnterDryadPod.PlayOneShot(SoundInfo.InMap(this.parent, MaintenanceType.None));
			if (p.connections != null)
			{
				foreach (Thing thing in p.connections.ConnectedThings)
				{
					if (thing.TryGetComp<CompTreeConnection>() != null)
					{
						this.tree = thing;
						break;
					}
				}
			}
		}

		// Token: 0x0600FEEE RID: 65262 RVA: 0x0049E688 File Offset: 0x0049C888
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "CasketContains".Translate() + ": " + this.innerContainer.ContentsString.CapitalizeFirst();
			if (this.tickComplete >= 0)
			{
				text = string.Concat(new string[]
				{
					text,
					"\n",
					"TimeLeft".Translate().CapitalizeFirst(),
					": ",
					(this.tickComplete - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor)
				});
			}
			return text;
		}

		// Token: 0x0600FEEF RID: 65263
		protected abstract void Complete();

		// Token: 0x0600FEF0 RID: 65264 RVA: 0x0049E751 File Offset: 0x0049C951
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600FEF1 RID: 65265 RVA: 0x0049E759 File Offset: 0x0049C959
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600FEF2 RID: 65266 RVA: 0x0049E768 File Offset: 0x0049C968
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.tickComplete, "tickComplete", -1, false);
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			Scribe_References.Look<Thing>(ref this.tree, "tree", false);
		}

		// Token: 0x0400A6F5 RID: 42741
		protected int tickComplete = -1;

		// Token: 0x0400A6F6 RID: 42742
		protected ThingOwner innerContainer;

		// Token: 0x0400A6F7 RID: 42743
		protected Thing tree;

		// Token: 0x0400A6F8 RID: 42744
		private CompTreeConnection cachedTreeComp;

		// Token: 0x0400A6F9 RID: 42745
		private Material cachedFrontMat;

		// Token: 0x0400A6FA RID: 42746
		protected const int ExpiryDurationTicks = 600;
	}
}
