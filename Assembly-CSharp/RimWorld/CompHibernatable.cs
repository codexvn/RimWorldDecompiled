using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002131 RID: 8497
	public class CompHibernatable : ThingComp
	{
		// Token: 0x17001C29 RID: 7209
		// (get) Token: 0x0600B961 RID: 47457 RVA: 0x00357E93 File Offset: 0x00356093
		public CompProperties_Hibernatable Props
		{
			get
			{
				return (CompProperties_Hibernatable)this.props;
			}
		}

		// Token: 0x17001C2A RID: 7210
		// (get) Token: 0x0600B962 RID: 47458 RVA: 0x00357EA0 File Offset: 0x003560A0
		// (set) Token: 0x0600B963 RID: 47459 RVA: 0x00357EA8 File Offset: 0x003560A8
		public HibernatableStateDef State
		{
			get
			{
				return this.state;
			}
			set
			{
				if (this.state == value)
				{
					return;
				}
				this.state = value;
				this.parent.Map.info.parent.Notify_HibernatableChanged();
			}
		}

		// Token: 0x17001C2B RID: 7211
		// (get) Token: 0x0600B964 RID: 47460 RVA: 0x00357ED5 File Offset: 0x003560D5
		public bool Running
		{
			get
			{
				return this.State == HibernatableStateDefOf.Running;
			}
		}

		// Token: 0x0600B965 RID: 47461 RVA: 0x00357EE4 File Offset: 0x003560E4
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.parent.Map.info.parent.Notify_HibernatableChanged();
			}
		}

		// Token: 0x0600B966 RID: 47462 RVA: 0x00357F0A File Offset: 0x0035610A
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			map.info.parent.Notify_HibernatableChanged();
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x0600B967 RID: 47463 RVA: 0x00357F44 File Offset: 0x00356144
		public void Startup()
		{
			if (this.State != HibernatableStateDefOf.Hibernating)
			{
				Log.ErrorOnce("Attempted to start a non-hibernating object", 34361223);
				return;
			}
			this.State = HibernatableStateDefOf.Starting;
			this.endStartupTick = Mathf.RoundToInt((float)Find.TickManager.TicksGame + this.Props.startupDays * 60000f);
		}

		// Token: 0x0600B968 RID: 47464 RVA: 0x00357FA4 File Offset: 0x003561A4
		public override string CompInspectStringExtra()
		{
			if (this.State == HibernatableStateDefOf.Hibernating)
			{
				return "HibernatableHibernating".Translate();
			}
			if (this.State == HibernatableStateDefOf.Starting)
			{
				return string.Format("{0}: {1}", "HibernatableStartingUp".Translate(), (this.endStartupTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false));
			}
			return null;
		}

		// Token: 0x0600B969 RID: 47465 RVA: 0x00358014 File Offset: 0x00356214
		public override void CompTick()
		{
			if (this.State == HibernatableStateDefOf.Starting && Find.TickManager.TicksGame > this.endStartupTick)
			{
				this.State = HibernatableStateDefOf.Running;
				this.endStartupTick = 0;
				string str;
				if (this.parent.Map.Parent.GetComponent<EscapeShipComp>() != null)
				{
					str = "LetterHibernateComplete".Translate();
				}
				else
				{
					str = "LetterHibernateCompleteStandalone".Translate();
				}
				Find.LetterStack.ReceiveLetter("LetterLabelHibernateComplete".Translate(), str, LetterDefOf.PositiveEvent, new GlobalTargetInfo(this.parent), null, null, null, null, 0, true);
			}
			if (this.State != HibernatableStateDefOf.Hibernating)
			{
				if (this.sustainer == null || this.sustainer.Ended)
				{
					this.sustainer = this.Props.sustainerActive.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
				}
				this.sustainer.Maintain();
				return;
			}
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x0600B96A RID: 47466 RVA: 0x0035813B File Offset: 0x0035633B
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<HibernatableStateDef>(ref this.state, "hibernateState");
			Scribe_Values.Look<int>(ref this.endStartupTick, "hibernateendStartupTick", 0, false);
		}

		// Token: 0x04007FDA RID: 32730
		private HibernatableStateDef state = HibernatableStateDefOf.Hibernating;

		// Token: 0x04007FDB RID: 32731
		private int endStartupTick;

		// Token: 0x04007FDC RID: 32732
		private Sustainer sustainer;
	}
}
