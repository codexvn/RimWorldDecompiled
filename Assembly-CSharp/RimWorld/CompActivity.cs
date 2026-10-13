using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BA6 RID: 11174
	public sealed class CompActivity : ThingComp
	{
		// Token: 0x17002712 RID: 10002
		// (get) Token: 0x0600F9AD RID: 63917 RVA: 0x00489F50 File Offset: 0x00488150
		public CompProperties_Activity Props
		{
			get
			{
				return (CompProperties_Activity)this.props;
			}
		}

		// Token: 0x17002713 RID: 10003
		// (get) Token: 0x0600F9AE RID: 63918 RVA: 0x00489F5D File Offset: 0x0048815D
		public float ActivityLevel
		{
			get
			{
				return this.activityLevelPercent;
			}
		}

		// Token: 0x17002714 RID: 10004
		// (get) Token: 0x0600F9AF RID: 63919 RVA: 0x00489F65 File Offset: 0x00488165
		public bool IsActive
		{
			get
			{
				return this.state == ActivityState.Active;
			}
		}

		// Token: 0x17002715 RID: 10005
		// (get) Token: 0x0600F9B0 RID: 63920 RVA: 0x00489F70 File Offset: 0x00488170
		public bool IsDormant
		{
			get
			{
				return this.state == ActivityState.Passive;
			}
		}

		// Token: 0x17002716 RID: 10006
		// (get) Token: 0x0600F9B1 RID: 63921 RVA: 0x00489F7B File Offset: 0x0048817B
		public bool Deactivated
		{
			get
			{
				return this.deactivated;
			}
		}

		// Token: 0x17002717 RID: 10007
		// (get) Token: 0x0600F9B2 RID: 63922 RVA: 0x00489F83 File Offset: 0x00488183
		public ActivityState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x17002718 RID: 10008
		// (get) Token: 0x0600F9B3 RID: 63923 RVA: 0x00489F8B File Offset: 0x0048818B
		public float ActivityResearchFactor
		{
			get
			{
				return this.Props.activityResearchFactorCurve.Evaluate(this.ActivityLevel);
			}
		}

		// Token: 0x17002719 RID: 10009
		// (get) Token: 0x0600F9B4 RID: 63924 RVA: 0x00489FA4 File Offset: 0x004881A4
		public bool CanBeSuppressed
		{
			get
			{
				using (IEnumerator<IActivity> enumerator = this.parent.GetComps<IActivity>().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.CanBeSuppressed())
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x0600F9B5 RID: 63925 RVA: 0x00489FFC File Offset: 0x004881FC
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.SetActivity(this.Props.startingRange.RandomInRange, true);
		}

		// Token: 0x0600F9B6 RID: 63926 RVA: 0x0048A01C File Offset: 0x0048821C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.SuppressingEntities, OpportunityType.Important);
		}

		// Token: 0x0600F9B7 RID: 63927 RVA: 0x0048A030 File Offset: 0x00488230
		public override void CompTick()
		{
			this.AdjustActivity(this.Props.Worker.GetChangeRatePerDay(this.parent) / 60000f);
		}

		// Token: 0x0600F9B8 RID: 63928 RVA: 0x0048A054 File Offset: 0x00488254
		public override string CompInspectStringExtra()
		{
			if (this.Deactivated)
			{
				return "Deactivated".Translate() + ".";
			}
			if (this.IsActive)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			this.Props.Worker.GetInspectString(this.parent, stringBuilder);
			return stringBuilder.ToString();
		}

		// Token: 0x0600F9B9 RID: 63929 RVA: 0x0048A0B0 File Offset: 0x004882B0
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.Deactivated)
			{
				yield break;
			}
			if (this.gizmo == null)
			{
				this.gizmo = new ActivityGizmo(this.parent);
			}
			if (Find.Selector.SelectedObjects.Count == 1 && this.IsDormant)
			{
				yield return this.gizmo;
			}
			if (!DebugSettings.godMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Activity -5%",
				action = delegate()
				{
					this.AdjustActivity(-0.05f);
				}
			};
			yield return new Command_Action
			{
				defaultLabel = "DEV: Activity +5%",
				action = delegate()
				{
					this.AdjustActivity(0.05f);
				}
			};
			yield return new Command_Action
			{
				defaultLabel = "DEV: " + (this.IsDormant ? "Go active" : "Go passive"),
				action = delegate()
				{
					this.activityLevelPercent = (this.IsDormant ? 1f : 0f);
				}
			};
			yield break;
		}

		// Token: 0x0600F9BA RID: 63930 RVA: 0x0048A0C0 File Offset: 0x004882C0
		public void AdjustActivity(float delta)
		{
			this.SetActivity(this.activityLevelPercent + delta, false);
		}

		// Token: 0x0600F9BB RID: 63931 RVA: 0x0048A0D4 File Offset: 0x004882D4
		public void SetActivity(float activity, bool dontDirtyGraphic = false)
		{
			if (this.Deactivated)
			{
				return;
			}
			this.activityLevelPercent = Mathf.Clamp01(activity);
			if (this.activityLevelPercent >= 1f && this.state != ActivityState.Active && this.CanActivate())
			{
				this.EnterActiveState();
			}
			else if (this.activityLevelPercent <= 0f && this.state != ActivityState.Passive)
			{
				this.EnterPassiveState();
			}
			if (!dontDirtyGraphic && this.Props.dirtyGraphicsOnActivityChange)
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn != null && Mathf.Abs(this.activityLevelPercent - this.lastGraphicDirtyPercent) > 0.01f)
				{
					this.lastGraphicDirtyPercent = this.activityLevelPercent;
					pawn.Drawer.renderer.SetAllGraphicsDirty();
				}
			}
		}

		// Token: 0x0600F9BC RID: 63932 RVA: 0x0048A18C File Offset: 0x0048838C
		private bool CanActivate()
		{
			using (IEnumerator<IActivity> enumerator = this.parent.GetComps<IActivity>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.CanActivate())
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600F9BD RID: 63933 RVA: 0x0048A1E4 File Offset: 0x004883E4
		public bool ShouldGoPassive()
		{
			if (this.Deactivated)
			{
				return false;
			}
			using (IEnumerator<IActivity> enumerator = this.parent.GetComps<IActivity>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ShouldGoPassive())
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600F9BE RID: 63934 RVA: 0x0048A248 File Offset: 0x00488448
		public void EnterActiveState()
		{
			if (this.Deactivated)
			{
				return;
			}
			this.triggeredNaturally = (this.activityLevelPercent >= 1f);
			this.activityLevelPercent = 1f;
			this.state = ActivityState.Active;
			foreach (IActivity activity in this.parent.GetComps<IActivity>())
			{
				activity.OnActivityActivated();
			}
			if ((this.triggeredNaturally || this.Props.showLetterOnManualActivation) && this.Props.showLetterOnActivated)
			{
				Find.LetterStack.ReceiveLetter(this.Props.letterTitle, this.Props.letterDesc, this.Props.letterDef, this.parent, null, null, null, null, 0, true);
			}
		}

		// Token: 0x0600F9BF RID: 63935 RVA: 0x0048A330 File Offset: 0x00488530
		public void EnterPassiveState()
		{
			if (this.Deactivated)
			{
				return;
			}
			this.activityLevelPercent = 0f;
			this.state = ActivityState.Passive;
			foreach (IActivity activity in this.parent.GetComps<IActivity>())
			{
				activity.OnPassive();
			}
		}

		// Token: 0x0600F9C0 RID: 63936 RVA: 0x0048A39C File Offset: 0x0048859C
		public void Deactivate()
		{
			if (this.IsActive)
			{
				this.EnterPassiveState();
			}
			this.activityLevelPercent = 0f;
			this.deactivated = true;
			this.suppressionEnabled = false;
			this.suppressIfAbove = 0.5f;
			this.gizmo = null;
			CompStudiable comp = this.parent.GetComp<CompStudiable>();
			if (comp == null)
			{
				return;
			}
			comp.Notify_ActivityDeactivated();
		}

		// Token: 0x0600F9C1 RID: 63937 RVA: 0x0048A3F8 File Offset: 0x004885F8
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (this.Deactivated)
			{
				return;
			}
			if (totalDamageDealt > 0f && this.IsDormant && this.Props.changePerDamage > 0f)
			{
				float x = 1f;
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					x = pawn.health.summaryHealth.SummaryHealthPercent;
				}
				else if (this.parent.def.useHitPoints)
				{
					x = (float)this.parent.HitPoints / (float)this.parent.MaxHitPoints;
				}
				float num = this.Props.changePerDamage * totalDamageDealt * this.Props.damagedActivityMultiplierCurve.Evaluate(x);
				this.SetActivity(this.activityLevelPercent + num, false);
				if (this.IsActive)
				{
					return;
				}
				TaggedString taggedString = "MessageActivityRisingDamage".Translate(this.parent.LabelNoParenthesis);
				if (MessagesRepeatAvoider.MessageShowAllowed(taggedString, 15f))
				{
					Messages.Message(taggedString, this.parent, MessageTypeDefOf.CautionInput, true);
				}
			}
		}

		// Token: 0x0600F9C2 RID: 63938 RVA: 0x0048A50E File Offset: 0x0048870E
		public void Notify_HeldOnPlatform()
		{
			this.suppressionEnabled = true;
		}

		// Token: 0x0600F9C3 RID: 63939 RVA: 0x0048A518 File Offset: 0x00488718
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.activityLevelPercent, "activityPercent", 0f, false);
			Scribe_Values.Look<ActivityState>(ref this.state, "state", ActivityState.Passive, false);
			Scribe_Values.Look<float>(ref this.suppressIfAbove, "suppressIfAbove", 0f, false);
			Scribe_Values.Look<bool>(ref this.suppressionEnabled, "suppressionEnabled", false, false);
			Scribe_Values.Look<bool>(ref this.triggeredNaturally, "triggeredNaturally", false, false);
			Scribe_Values.Look<float>(ref this.lastGraphicDirtyPercent, "lastGraphicDirtyPercent", 0f, false);
			Scribe_Values.Look<bool>(ref this.deactivated, "deactivated", false, false);
		}

		// Token: 0x0400A436 RID: 42038
		private float activityLevelPercent;

		// Token: 0x0400A437 RID: 42039
		private ActivityState state;

		// Token: 0x0400A438 RID: 42040
		private bool triggeredNaturally;

		// Token: 0x0400A439 RID: 42041
		private bool deactivated;

		// Token: 0x0400A43A RID: 42042
		public bool suppressionEnabled;

		// Token: 0x0400A43B RID: 42043
		public float suppressIfAbove = 0.4f;

		// Token: 0x0400A43C RID: 42044
		private float lastGraphicDirtyPercent;

		// Token: 0x0400A43D RID: 42045
		private Gizmo gizmo;
	}
}
