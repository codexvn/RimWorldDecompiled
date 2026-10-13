using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002ECF RID: 11983
	public class CompWakeUpDormant : ThingComp
	{
		// Token: 0x17002BA0 RID: 11168
		// (get) Token: 0x06010EB3 RID: 69299 RVA: 0x004DE8E0 File Offset: 0x004DCAE0
		private CompProperties_WakeUpDormant Props
		{
			get
			{
				return (CompProperties_WakeUpDormant)this.props;
			}
		}

		// Token: 0x17002BA1 RID: 11169
		// (get) Token: 0x06010EB4 RID: 69300 RVA: 0x004DE8F0 File Offset: 0x004DCAF0
		private CompCanBeDormant DormantComp
		{
			get
			{
				CompCanBeDormant result;
				if ((result = this.dormantComp) == null)
				{
					result = (this.dormantComp = this.parent.TryGetComp<CompCanBeDormant>());
				}
				return result;
			}
		}

		// Token: 0x06010EB5 RID: 69301 RVA: 0x004DE91B File Offset: 0x004DCB1B
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.wakeUpIfTargetClose = this.Props.wakeUpIfAnyTargetClose;
		}

		// Token: 0x06010EB6 RID: 69302 RVA: 0x004DE935 File Offset: 0x004DCB35
		public override void CompTick()
		{
			if (this.parent.IsHashIntervalTick(250))
			{
				this.TickRareWorker();
			}
		}

		// Token: 0x06010EB7 RID: 69303 RVA: 0x004DE950 File Offset: 0x004DCB50
		public void TickRareWorker()
		{
			if (!this.parent.Spawned || this.parent.Faction == Faction.OfPlayer)
			{
				return;
			}
			if (this.wakeUpIfTargetClose)
			{
				int num = GenRadial.NumCellsInRadius(this.Props.wakeUpCheckRadius);
				for (int i = 0; i < num; i++)
				{
					IntVec3 intVec = this.parent.Position + GenRadial.RadialPattern[i];
					if (intVec.InBounds(this.parent.Map) && GenSight.LineOfSight(this.parent.Position, intVec, this.parent.Map))
					{
						foreach (Thing thing in intVec.GetThingList(this.parent.Map))
						{
							if (this.Props.wakeUpTargetingParams.CanTarget(thing, null))
							{
								this.Activate(thing, true, false, false);
								return;
							}
						}
					}
				}
			}
			if (this.Props.wakeUpOnThingConstructedRadius > 0f)
			{
				Thing thing2 = GenClosest.ClosestThingReachable(this.parent.Position, this.parent.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false), this.Props.wakeUpOnThingConstructedRadius, (Thing t) => (t.def.building == null || t.def.building.wakeDormantPawnsOnConstruction) && t.Faction == Faction.OfPlayer, null, 0, -1, false, RegionType.Set_Passable, false, false);
				if (thing2 != null)
				{
					this.Activate(thing2, true, false, false);
				}
			}
		}

		// Token: 0x06010EB8 RID: 69304 RVA: 0x004DEAF4 File Offset: 0x004DCCF4
		public void Activate(Thing waker, bool sendSignal = true, bool silent = false, bool instant = false)
		{
			if (this.DormantComp != null && this.DormantComp.Awake)
			{
				return;
			}
			if (sendSignal && (!this.sentSignal || !this.Props.onlySendSignalOnce))
			{
				if (!string.IsNullOrEmpty(this.Props.wakeUpSignalTag))
				{
					if (this.Props.onlyWakeUpSameFaction)
					{
						Find.SignalManager.SendSignal(new Signal(this.Props.wakeUpSignalTag, this.parent.Named("SUBJECT"), this.parent.Faction.Named("FACTION")));
					}
					else
					{
						Find.SignalManager.SendSignal(new Signal(this.Props.wakeUpSignalTag, this.parent.Named("SUBJECT")));
					}
				}
				if (!silent && this.parent.Spawned && this.Props.wakeUpSound != null)
				{
					this.Props.wakeUpSound.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
				}
				if (this.parent.Spawned && waker is Pawn && !silent && (!this.Props.activateMessageKey.NullOrEmpty() || !this.Props.activatePluralMessageKey.NullOrEmpty()))
				{
					CompWakeUpDormant.tmpActivatedThings.Clear();
					if (this.groupID >= 0)
					{
						List<Thing> list = this.parent.Map.listerThings.ThingsInGroup(ThingRequestGroup.WakeUpDormant);
						for (int i = 0; i < list.Count; i++)
						{
							if (list[i].TryGetComp<CompWakeUpDormant>().groupID == this.groupID)
							{
								CompWakeUpDormant.tmpActivatedThings.Add(list[i]);
							}
						}
					}
					else
					{
						CompWakeUpDormant.tmpActivatedThings.Add(this.parent);
					}
					if (CompWakeUpDormant.tmpActivatedThings.Count > 1 && !this.Props.activatePluralMessageKey.NullOrEmpty())
					{
						Messages.Message(this.Props.activatePluralMessageKey.Translate(waker.Named("WAKER")), CompWakeUpDormant.tmpActivatedThings, this.Props.activateMessageType ?? MessageTypeDefOf.NegativeEvent, false);
					}
					else if (CompWakeUpDormant.tmpActivatedThings.Count > 0 && !this.Props.activateMessageKey.NullOrEmpty())
					{
						Messages.Message(this.Props.activateMessageKey.Translate(waker.Named("WAKER")), CompWakeUpDormant.tmpActivatedThings, this.Props.activateMessageType ?? MessageTypeDefOf.NegativeEvent, false);
					}
					CompWakeUpDormant.tmpActivatedThings.Clear();
				}
				this.sentSignal = true;
			}
			if (this.DormantComp != null)
			{
				if (this.Props.wakeUpWithDelay && !instant)
				{
					this.DormantComp.WakeUpWithDelay();
					return;
				}
				this.DormantComp.WakeUp();
			}
		}

		// Token: 0x06010EB9 RID: 69305 RVA: 0x004DEDD6 File Offset: 0x004DCFD6
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.Props.wakeUpOnDamage && dinfo.Def.ExternalViolenceFor(this.parent))
			{
				this.Activate(dinfo.Instigator, true, false, true);
			}
		}

		// Token: 0x06010EBA RID: 69306 RVA: 0x004DEE0C File Offset: 0x004DD00C
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (this.wakeUpIfTargetClose && !this.Props.radiusCheckInspectPaneKey.NullOrEmpty())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text = text + (this.Props.radiusCheckInspectPaneKey.Translate() + ": ") + Mathf.RoundToInt(this.Props.wakeUpCheckRadius).ToString();
			}
			return text;
		}

		// Token: 0x06010EBB RID: 69307 RVA: 0x004DEE8D File Offset: 0x004DD08D
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.sentSignal, "sentSignal", false, false);
			Scribe_Values.Look<int>(ref this.groupID, "groupID", -1, false);
			Scribe_Values.Look<bool>(ref this.wakeUpIfTargetClose, "wakeUpIfColonistClose", false, false);
		}

		// Token: 0x0400B005 RID: 45061
		public bool wakeUpIfTargetClose;

		// Token: 0x0400B006 RID: 45062
		private bool sentSignal;

		// Token: 0x0400B007 RID: 45063
		public int groupID = -1;

		// Token: 0x0400B008 RID: 45064
		private CompCanBeDormant dormantComp;

		// Token: 0x0400B009 RID: 45065
		private static List<Thing> tmpActivatedThings = new List<Thing>();
	}
}
