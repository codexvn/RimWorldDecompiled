using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002C37 RID: 11319
	public class CompCanBeDormant : ThingComp
	{
		// Token: 0x170027EC RID: 10220
		// (get) Token: 0x0600FDC6 RID: 64966 RVA: 0x00499987 File Offset: 0x00497B87
		public CompProperties_CanBeDormant Props
		{
			get
			{
				return (CompProperties_CanBeDormant)this.props;
			}
		}

		// Token: 0x170027ED RID: 10221
		// (get) Token: 0x0600FDC7 RID: 64967 RVA: 0x00499994 File Offset: 0x00497B94
		private bool WaitingToWakeUp
		{
			get
			{
				return this.wakeUpOnTick != int.MinValue;
			}
		}

		// Token: 0x170027EE RID: 10222
		// (get) Token: 0x0600FDC8 RID: 64968 RVA: 0x004999A6 File Offset: 0x00497BA6
		protected virtual JobDef SleepJob
		{
			get
			{
				return JobDefOf.Wait_AsleepDormancy;
			}
		}

		// Token: 0x170027EF RID: 10223
		// (get) Token: 0x0600FDC9 RID: 64969 RVA: 0x004999B0 File Offset: 0x00497BB0
		public virtual bool Awake
		{
			get
			{
				if (this.wokeUpTick == -2147483648 || this.wokeUpTick > Find.TickManager.TicksGame)
				{
					if (this.parent.Spawned && this.Props.jobDormancy)
					{
						Pawn pawn = this.parent as Pawn;
						if (pawn != null)
						{
							return pawn.CurJobDef != this.SleepJob;
						}
					}
					return false;
				}
				return true;
			}
		}

		// Token: 0x170027F0 RID: 10224
		// (get) Token: 0x0600FDCA RID: 64970 RVA: 0x00499A1A File Offset: 0x00497C1A
		protected virtual bool ShowZs
		{
			get
			{
				return !(this.parent is Pawn) && this.Props.showSleepingZs && (this.Props.delayedWakeUpDoesZs || this.wakeUpOnTick == int.MinValue);
			}
		}

		// Token: 0x0600FDCB RID: 64971 RVA: 0x00499A56 File Offset: 0x00497C56
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.makeTick = GenTicks.TicksGame;
			if (!this.Props.startsDormant)
			{
				this.WakeUp();
			}
		}

		// Token: 0x0600FDCC RID: 64972 RVA: 0x00499A7C File Offset: 0x00497C7C
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.wakeUpSignalTag = this.Props.wakeUpSignalTag;
		}

		// Token: 0x0600FDCD RID: 64973 RVA: 0x00499A96 File Offset: 0x00497C96
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode || !DebugSettings.godMode || this.Props.dontShowDevGizmos)
			{
				yield break;
			}
			if (!this.Awake)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Wake Up",
					action = new Action(this.WakeUp)
				};
				yield return new Command_Action
				{
					action = delegate()
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						for (int i = 0; i < 10; i++)
						{
							float num = (float)i * 0.5f;
							int delayTicks = Mathf.RoundToInt(num * 60f);
							string label = num.ToString("F1") + "s";
							list.Add(new FloatMenuOption(label, delegate()
							{
								this.wakeUpOnTick = Find.TickManager.TicksGame + delayTicks;
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					},
					defaultLabel = "DEV: Wake up after...",
					defaultDesc = "DEV: Wake up after a delay"
				};
			}
			else if (this.Props.jobDormancy)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: To Sleep",
					action = new Action(this.ToSleep)
				};
			}
			yield break;
		}

		// Token: 0x0600FDCE RID: 64974 RVA: 0x00499AA8 File Offset: 0x00497CA8
		public override string CompInspectStringExtra()
		{
			Pawn pawn = this.parent as Pawn;
			if (pawn != null && (pawn.IsSelfShutdown() || pawn.IsDeactivated() || pawn.Awake()))
			{
				return null;
			}
			if (this.Awake)
			{
				if (this.parent.Faction != Faction.OfPlayer && this.makeTick != this.wokeUpTick)
				{
					Pawn pawn2 = this.parent as Pawn;
					if (pawn2 == null || !pawn2.Downed)
					{
						return this.Props.awakeStateLabelKey.Translate((GenTicks.TicksGame - this.wokeUpTick).TicksToDays().ToString("0.#"));
					}
				}
				return null;
			}
			if (!this.Props.wakeUpDelayStateLabelKey.NullOrEmpty() && this.wakeUpOnTick != -2147483648)
			{
				return this.Props.wakeUpDelayStateLabelKey.Translate();
			}
			return this.Props.dormantStateLabelKey.Translate();
		}

		// Token: 0x0600FDCF RID: 64975 RVA: 0x00499BA4 File Offset: 0x00497DA4
		public void WakeUpWithDelay()
		{
			if (!this.Awake)
			{
				this.wakeUpOnTick = Find.TickManager.TicksGame + this.Props.wakeUpDelayRange.RandomInRange;
				this.repeatWakeUpSignalOnTick = ((this.Props.wakeUpRepeatSignalDelayRange != null) ? (Find.TickManager.TicksGame + this.Props.wakeUpRepeatSignalDelayRange.Value.RandomInRange) : int.MinValue);
			}
		}

		// Token: 0x0600FDD0 RID: 64976 RVA: 0x00499C1C File Offset: 0x00497E1C
		public virtual void WakeUp()
		{
			if (this.Awake)
			{
				return;
			}
			this.wokeUpTick = GenTicks.TicksGame;
			this.wakeUpOnTick = int.MinValue;
			Pawn pawn = this.parent as Pawn;
			Building building = this.parent as Building;
			Lord lord = ((pawn != null) ? pawn.GetLord() : null) ?? ((building != null) ? building.GetLord() : null);
			if (lord != null)
			{
				lord.Notify_DormancyWakeup();
			}
			if (this.parent.Spawned)
			{
				IAttackTarget attackTarget = this.parent as IAttackTarget;
				if (attackTarget != null)
				{
					this.parent.Map.attackTargetsCache.UpdateTarget(attackTarget);
				}
				if (this.Props.jobDormancy)
				{
					Pawn pawn2 = this.parent as Pawn;
					if (pawn2 != null && pawn2.CurJobDef == this.SleepJob)
					{
						pawn2.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
		}

		// Token: 0x0600FDD1 RID: 64977 RVA: 0x00499CF4 File Offset: 0x00497EF4
		public virtual void ToSleep()
		{
			if (!this.Awake)
			{
				return;
			}
			this.wokeUpTick = int.MinValue;
			if (this.parent.Spawned)
			{
				IAttackTarget attackTarget = this.parent as IAttackTarget;
				if (attackTarget != null)
				{
					this.parent.Map.attackTargetsCache.UpdateTarget(attackTarget);
				}
				if (this.Props.jobDormancy)
				{
					Pawn pawn = this.parent as Pawn;
					if (pawn != null)
					{
						Job job = JobMaker.MakeJob(this.SleepJob, pawn.Position);
						job.forceSleep = true;
						pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
					}
				}
			}
		}

		// Token: 0x0600FDD2 RID: 64978 RVA: 0x00499DAE File Offset: 0x00497FAE
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (this.wakeUpOnTick != -2147483648 && Find.TickManager.TicksGame >= this.wakeUpOnTick)
			{
				this.WakeUp();
			}
			this.TickRareWorker();
		}

		// Token: 0x0600FDD3 RID: 64979 RVA: 0x00499DE4 File Offset: 0x00497FE4
		public override void CompTick()
		{
			base.CompTick();
			if (this.wakeUpOnTick != -2147483648)
			{
				if (Find.TickManager.TicksGame >= this.wakeUpOnTick)
				{
					this.WakeUp();
					Effecter effecter = this.wakeUpEffect;
					if (effecter != null)
					{
						effecter.Cleanup();
					}
					this.wakeUpEffect = null;
				}
				else
				{
					if (this.Props.wakeUpEffect != null && this.wakeUpEffect == null)
					{
						this.wakeUpEffect = this.Props.wakeUpEffect.SpawnAttached(this.parent, this.parent.Map, 1f);
					}
					Effecter effecter2 = this.wakeUpEffect;
					if (effecter2 != null)
					{
						effecter2.EffectTick(this.parent, this.parent);
					}
					if (this.repeatWakeUpSignalOnTick != -2147483648 && Find.TickManager.TicksGame == this.repeatWakeUpSignalOnTick)
					{
						Find.SignalManager.SendSignal(new Signal(this.wakeUpSignalTag, this.parent.Named("SUBJECT")));
					}
				}
			}
			if (this.parent.IsHashIntervalTick(250))
			{
				this.TickRareWorker();
			}
		}

		// Token: 0x0600FDD4 RID: 64980 RVA: 0x00499F00 File Offset: 0x00498100
		public void TickRareWorker()
		{
			if (!this.parent.Spawned || this.Awake)
			{
				return;
			}
			if (this.ShowZs && !this.parent.Position.Fogged(this.parent.Map))
			{
				FleckMaker.ThrowMetaIcon(this.parent.Position, this.parent.Map, FleckDefOf.SleepZ, 0.42f);
			}
		}

		// Token: 0x0600FDD5 RID: 64981 RVA: 0x00499F70 File Offset: 0x00498170
		public override void Notify_SignalReceived(Signal signal)
		{
			if (string.IsNullOrEmpty(this.wakeUpSignalTag) || this.Awake)
			{
				return;
			}
			Thing thing;
			Faction faction;
			if ((signal.tag == this.wakeUpSignalTag || (this.wakeUpSignalTags != null && this.wakeUpSignalTags.Contains(signal.tag))) && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && thing != this.parent && thing != null && thing.Map == this.parent.Map && this.parent.Position.DistanceTo(thing.Position) <= this.Props.maxDistAwakenByOther && (!signal.args.TryGetArg<Faction>("FACTION", out faction) || faction == null || faction == this.parent.Faction) && (this.Props.canWakeUpFogged || !this.parent.Fogged()) && !this.WaitingToWakeUp)
			{
				this.WakeUpWithDelay();
			}
		}

		// Token: 0x0600FDD6 RID: 64982 RVA: 0x0049A078 File Offset: 0x00498278
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.wokeUpTick, "wokeUpTick", int.MinValue, false);
			Scribe_Values.Look<int>(ref this.wakeUpOnTick, "wakeUpOnTick", int.MinValue, false);
			Scribe_Values.Look<string>(ref this.wakeUpSignalTag, "wakeUpSignalTag", null, false);
			Scribe_Collections.Look<string>(ref this.wakeUpSignalTags, "wakeUpSignalTags", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.makeTick, "makeTick", 0, false);
			Scribe_Values.Look<int>(ref this.repeatWakeUpSignalOnTick, "repeatWakeUpSignalOnTick", int.MinValue, false);
		}

		// Token: 0x0400A65A RID: 42586
		private int makeTick;

		// Token: 0x0400A65B RID: 42587
		public int wokeUpTick = int.MinValue;

		// Token: 0x0400A65C RID: 42588
		public int wakeUpOnTick = int.MinValue;

		// Token: 0x0400A65D RID: 42589
		private int repeatWakeUpSignalOnTick = int.MaxValue;

		// Token: 0x0400A65E RID: 42590
		private Effecter wakeUpEffect;

		// Token: 0x0400A65F RID: 42591
		public string wakeUpSignalTag;

		// Token: 0x0400A660 RID: 42592
		public List<string> wakeUpSignalTags;

		// Token: 0x0400A661 RID: 42593
		public const string DefaultWakeUpSignal = "CompCanBeDormant.WakeUp";
	}
}
