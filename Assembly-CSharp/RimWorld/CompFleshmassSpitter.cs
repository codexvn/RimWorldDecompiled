using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002C97 RID: 11415
	public class CompFleshmassSpitter : CompFleshmassHeartChild, IAttackTargetSearcher, IVerbOwner
	{
		// Token: 0x17002873 RID: 10355
		// (get) Token: 0x06010038 RID: 65592 RVA: 0x004A3987 File Offset: 0x004A1B87
		private Verb AttackVerb
		{
			get
			{
				return this.AllVerbs[0];
			}
		}

		// Token: 0x17002874 RID: 10356
		// (get) Token: 0x06010039 RID: 65593 RVA: 0x004A3995 File Offset: 0x004A1B95
		private int TicksToNextSpit
		{
			get
			{
				return this.lastSpitTick + this.nextSpitDelay - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17002875 RID: 10357
		// (get) Token: 0x0601003A RID: 65594 RVA: 0x004A39AF File Offset: 0x004A1BAF
		private bool OnCooldown
		{
			get
			{
				return this.TicksToNextSpit > 0;
			}
		}

		// Token: 0x17002876 RID: 10358
		// (get) Token: 0x0601003B RID: 65595 RVA: 0x0048FDB8 File Offset: 0x0048DFB8
		public Thing Thing
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17002877 RID: 10359
		// (get) Token: 0x0601003C RID: 65596 RVA: 0x004A39BA File Offset: 0x004A1BBA
		public Verb CurrentEffectiveVerb
		{
			get
			{
				return this.AttackVerb;
			}
		}

		// Token: 0x17002878 RID: 10360
		// (get) Token: 0x0601003D RID: 65597 RVA: 0x004A39C2 File Offset: 0x004A1BC2
		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.lastAttackedTarget;
			}
		}

		// Token: 0x17002879 RID: 10361
		// (get) Token: 0x0601003E RID: 65598 RVA: 0x004A39CA File Offset: 0x004A1BCA
		public int LastAttackTargetTick
		{
			get
			{
				return this.lastSpitTick;
			}
		}

		// Token: 0x1700287A RID: 10362
		// (get) Token: 0x0601003F RID: 65599 RVA: 0x004A39D4 File Offset: 0x004A1BD4
		public VerbTracker VerbTracker
		{
			get
			{
				VerbTracker result;
				if ((result = this.verbTracker) == null)
				{
					result = (this.verbTracker = new VerbTracker(this));
				}
				return result;
			}
		}

		// Token: 0x1700287B RID: 10363
		// (get) Token: 0x06010040 RID: 65600 RVA: 0x0013A1A6 File Offset: 0x001383A6
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.parent.def.Verbs;
			}
		}

		// Token: 0x1700287C RID: 10364
		// (get) Token: 0x06010041 RID: 65601 RVA: 0x0013A1B8 File Offset: 0x001383B8
		public List<Tool> Tools
		{
			get
			{
				return this.parent.def.tools;
			}
		}

		// Token: 0x1700287D RID: 10365
		// (get) Token: 0x06010042 RID: 65602 RVA: 0x0036EC81 File Offset: 0x0036CE81
		public ImplementOwnerTypeDef ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.NativeVerb;
			}
		}

		// Token: 0x1700287E RID: 10366
		// (get) Token: 0x06010043 RID: 65603 RVA: 0x0048FDB8 File Offset: 0x0048DFB8
		public Thing ConstantCaster
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x06010044 RID: 65604 RVA: 0x004A39FA File Offset: 0x004A1BFA
		public string UniqueVerbOwnerID()
		{
			return "CompFleshmassSpitter_" + this.parent.ThingID;
		}

		// Token: 0x06010045 RID: 65605 RVA: 0x00002501 File Offset: 0x00000701
		public bool VerbsStillUsableBy(Pawn p)
		{
			return false;
		}

		// Token: 0x1700287F RID: 10367
		// (get) Token: 0x06010046 RID: 65606 RVA: 0x004A3A11 File Offset: 0x004A1C11
		public List<Verb> AllVerbs
		{
			get
			{
				return this.VerbTracker.AllVerbs;
			}
		}

		// Token: 0x06010047 RID: 65607 RVA: 0x004A3A20 File Offset: 0x004A1C20
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.SetupVerbs();
			if (!respawningAfterLoad)
			{
				this.lastSpitTick = Find.TickManager.TicksGame;
				this.nextSpitDelay = CompFleshmassSpitter.SpitIntervalRangeTicks.RandomInRange;
			}
		}

		// Token: 0x06010048 RID: 65608 RVA: 0x004A3A5C File Offset: 0x004A1C5C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.nextSpitDelay, "nextSpitDelay", 0, false);
			Scribe_Values.Look<int>(ref this.lastSpitTick, "lastSpitTick", 0, false);
			Scribe_TargetInfo.Look(ref this.lastAttackedTarget, "lastAttackedTarget");
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
		}

		// Token: 0x06010049 RID: 65609 RVA: 0x004A3ABD File Offset: 0x004A1CBD
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultDesc = "Dev: Cooldown Spit",
					defaultLabel = "Dev: Cooldown Spit",
					action = delegate()
					{
						this.lastSpitTick = -999999;
						this.nextSpitDelay = -999999;
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x0601004A RID: 65610 RVA: 0x004A3AD0 File Offset: 0x004A1CD0
		private void SetupVerbs()
		{
			foreach (Verb verb in this.AllVerbs)
			{
				verb.caster = this.parent;
			}
		}

		// Token: 0x0601004B RID: 65611 RVA: 0x004A3B28 File Offset: 0x004A1D28
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			Effecter effecter = this.progressBarEffecter;
			if (effecter != null)
			{
				effecter.ForceEnd();
			}
			this.progressBarEffecter = null;
		}

		// Token: 0x0601004C RID: 65612 RVA: 0x004A3B44 File Offset: 0x004A1D44
		public override void CompTick()
		{
			base.CompTick();
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.OnCooldown)
			{
				if (this.progressBarEffecter == null)
				{
					this.progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
				}
				this.progressBarEffecter.EffectTick(this.parent, TargetInfo.Invalid);
				MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBarEffecter.children[0]).mote;
				mote.progress = 1f - (float)Mathf.Max(this.TicksToNextSpit, 0) / (float)this.nextSpitDelay;
				mote.offsetZ = -0.8f;
				this.parent.overrideGraphicIndex = new int?(1);
			}
			else
			{
				this.parent.overrideGraphicIndex = new int?(0);
			}
			if (this.TicksToNextSpit <= 0 && this.parent.IsHashIntervalTick(180))
			{
				Thing thing = (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(this, TargetScanFlags.NeedThreat, (Thing t) => !t.Position.Roofed(t.Map), 0f, 9999f);
				if (thing == null)
				{
					return;
				}
				this.AttackVerb.TryStartCastOn(thing, false, true, false, false);
				this.lastSpitTick = Find.TickManager.TicksGame;
				this.nextSpitDelay = CompFleshmassSpitter.SpitIntervalRangeTicks.RandomInRange;
				Messages.Message("SpitterAttacking".Translate(), this.parent, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x0601004D RID: 65613 RVA: 0x004A3CC7 File Offset: 0x004A1EC7
		public override void PostDrawExtraSelectionOverlays()
		{
			this.AttackVerb.DrawHighlight(LocalTargetInfo.Invalid);
		}

		// Token: 0x0601004E RID: 65614 RVA: 0x004A3CDC File Offset: 0x004A1EDC
		public override string CompInspectStringExtra()
		{
			if (this.TicksToNextSpit > 0)
			{
				return "SpitterGatheringSpit".Translate() + ": " + this.TicksToNextSpit.ToStringTicksToPeriod(true, false, true, true, false);
			}
			return "SpitterReadyToSpit".Translate();
		}

		// Token: 0x0400A783 RID: 42883
		private static readonly IntRange SpitIntervalRangeTicks = new IntRange(5000, 7500);

		// Token: 0x0400A784 RID: 42884
		private const int SpitCheckIntervalTicks = 180;

		// Token: 0x0400A785 RID: 42885
		private const int ReadySpriteIndex = 0;

		// Token: 0x0400A786 RID: 42886
		private const int CooldownSpriteIndex = 1;

		// Token: 0x0400A787 RID: 42887
		private int lastSpitTick = -99999;

		// Token: 0x0400A788 RID: 42888
		private int nextSpitDelay = -99999;

		// Token: 0x0400A789 RID: 42889
		private LocalTargetInfo lastAttackedTarget;

		// Token: 0x0400A78A RID: 42890
		private VerbTracker verbTracker;

		// Token: 0x0400A78B RID: 42891
		private Effecter progressBarEffecter;
	}
}
