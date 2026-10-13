using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002CD0 RID: 11472
	[StaticConstructorOnStartup]
	public class CompHackable : ThingComp, IThingGlower
	{
		// Token: 0x170028C1 RID: 10433
		// (get) Token: 0x060101A9 RID: 65961 RVA: 0x004A8846 File Offset: 0x004A6A46
		public CompProperties_Hackable Props
		{
			get
			{
				return (CompProperties_Hackable)this.props;
			}
		}

		// Token: 0x170028C2 RID: 10434
		// (get) Token: 0x060101AA RID: 65962 RVA: 0x004A8853 File Offset: 0x004A6A53
		public float ProgressPercent
		{
			get
			{
				return this.progress / this.defence;
			}
		}

		// Token: 0x170028C3 RID: 10435
		// (get) Token: 0x060101AB RID: 65963 RVA: 0x004A8862 File Offset: 0x004A6A62
		public bool Autohack
		{
			get
			{
				return this.autohack;
			}
		}

		// Token: 0x170028C4 RID: 10436
		// (get) Token: 0x060101AC RID: 65964 RVA: 0x004A886A File Offset: 0x004A6A6A
		public bool IsHacked
		{
			get
			{
				return this.hacked;
			}
		}

		// Token: 0x170028C5 RID: 10437
		// (get) Token: 0x060101AD RID: 65965 RVA: 0x004A8872 File Offset: 0x004A6A72
		public bool LockedOut
		{
			get
			{
				return this.lockedOutPermanently || this.lockedOutUntilTick > Find.TickManager.TicksGame;
			}
		}

		// Token: 0x060101AE RID: 65966 RVA: 0x004A8890 File Offset: 0x004A6A90
		public bool ShouldBeLitNow()
		{
			return !this.IsHacked || this.Props.glowIfHacked;
		}

		// Token: 0x060101AF RID: 65967 RVA: 0x004A88A7 File Offset: 0x004A6AA7
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.defence = this.Props.defence;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x060101B0 RID: 65968 RVA: 0x004A88D1 File Offset: 0x004A6AD1
		public void HackNow()
		{
			this.progress = this.defence + 1f;
			this.ProcessHacked(null, true);
		}

		// Token: 0x060101B1 RID: 65969 RVA: 0x004A88F0 File Offset: 0x004A6AF0
		public void Hack(float amount, Pawn hacker = null, bool suppressMessages = false)
		{
			bool isHacked = this.IsHacked;
			this.progress += amount;
			this.progress = Mathf.Clamp(this.progress, 0f, this.defence);
			if (!isHacked && this.progress >= this.defence)
			{
				this.ProcessHacked(hacker, suppressMessages);
			}
			if (this.lastHackTick < 0)
			{
				if (!this.hackingStartedSignal.NullOrEmpty())
				{
					Find.SignalManager.SendSignal(new Signal(this.hackingStartedSignal, this.parent.Named("SUBJECT")));
				}
				QuestUtility.SendQuestTargetSignals(this.parent.questTags, "HackingStarted", this.parent.Named("SUBJECT"));
			}
			this.lastUserSpeed = amount;
			this.lastHackTick = Find.TickManager.TicksGame;
			this.lastUser = hacker;
			if (hacker != null && (this.Props.lockoutDurationHoursRange.TrueMax > 0 || this.Props.lockoutPermanently) && !this.IsHacked && this.progress - this.progressLastLockout > 3000f && Rand.MTBEventOccurs(hacker.GetStatValue(StatDefOf.HackingStealth, true, -1), amount * 60f, 1f))
			{
				this.LockOut(hacker);
			}
		}

		// Token: 0x060101B2 RID: 65970 RVA: 0x004A8A2C File Offset: 0x004A6C2C
		private void ProcessHacked(Pawn hacker, bool suppressMessages)
		{
			if (this.hacked)
			{
				return;
			}
			this.hacked = true;
			if (!this.hackingCompletedSignal.NullOrEmpty())
			{
				Find.SignalManager.SendSignal(new Signal(this.hackingCompletedSignal, this.parent.Named("SUBJECT")));
			}
			Pawn arg = hacker ?? ((Current.ProgramState == ProgramState.Playing && this.parent.Map.mapPawns.ColonistCount > 0) ? this.parent.Map.mapPawns.FreeColonists.RandomElement<Pawn>() : null);
			QuestUtility.SendQuestTargetSignals(this.parent.questTags, "Hacked", this.parent.Named("SUBJECT"), arg.Named("ACTIVATOR"));
			this.parent.BroadcastCompSignal("Hacked");
			if (this.Props.completedQuest != null)
			{
				Slate slate = new Slate();
				slate.Set<Map>("map", this.parent.Map, false);
				if (this.Props.completedQuest.CanRun(slate, this.parent.Map))
				{
					QuestUtility.GenerateQuestAndMakeAvailable(this.Props.completedQuest, slate);
				}
			}
			this.OnHacked(hacker, suppressMessages);
		}

		// Token: 0x060101B3 RID: 65971 RVA: 0x004A8B60 File Offset: 0x004A6D60
		public override IEnumerable<ThingDefCountClass> GetAdditionalLeavings(Map map, DestroyMode mode)
		{
			if (this.IsHacked)
			{
				yield break;
			}
			foreach (ThingDefCountClass thingDefCountClass in this.Props.dropOnDestroyed)
			{
				yield return thingDefCountClass;
			}
			List<ThingDefCountClass>.Enumerator enumerator = default(List<ThingDefCountClass>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060101B4 RID: 65972 RVA: 0x004A8B70 File Offset: 0x004A6D70
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (!this.sentLetter && !this.Props.destroyedLetterLabel.NullOrEmpty() && !this.Props.destroyedLetterText.NullOrEmpty())
			{
				this.sentLetter = true;
				Find.LetterStack.ReceiveLetter(this.Props.destroyedLetterLabel, this.Props.destroyedLetterText, this.Props.destroyedLetterDef, null, 0, true);
			}
		}

		// Token: 0x060101B5 RID: 65973 RVA: 0x004A8BE8 File Offset: 0x004A6DE8
		public override bool CompPreventClaimingBy(Faction faction)
		{
			return faction.IsPlayer && !this.IsHacked;
		}

		// Token: 0x060101B6 RID: 65974 RVA: 0x004A8C00 File Offset: 0x004A6E00
		protected virtual void OnHacked(Pawn hacker = null, bool suppressMessages = false)
		{
			IHackable hackable = this.parent as IHackable;
			if (hackable != null)
			{
				hackable.OnHacked(hacker);
			}
			foreach (ThingComp thingComp in this.parent.AllComps)
			{
				thingComp.Notify_Hacked(hacker);
			}
			GenLeaving.DropThingsNear(this.parent, this.Props.dropOnHacked, CompHackable.tmpSpawnedThings, true, null);
			if (!this.Props.hackedMessage.NullOrEmpty() && hacker != null && !suppressMessages)
			{
				Messages.Message(this.Props.hackedMessage.Formatted(hacker.Named("HACKER"), this.parent.Named("SUBJECT")), this.parent, MessageTypeDefOf.PositiveEvent, false);
			}
			if (!this.sentLetter && !this.Props.hackedLetterLabel.NullOrEmpty() && !this.Props.hackedLetterText.NullOrEmpty() && !suppressMessages)
			{
				this.sentLetter = true;
				Find.LetterStack.ReceiveLetter(this.Props.hackedLetterLabel, this.Props.hackedLetterText, this.Props.hackedLetterDef, CompHackable.tmpSpawnedThings, null, null, null, null, 0, true);
			}
			CompHackable.tmpSpawnedThings.Clear();
			if (this.parent.Spawned)
			{
				this.parent.DirtyMapMesh(this.parent.Map);
			}
		}

		// Token: 0x060101B7 RID: 65975 RVA: 0x004A8D90 File Offset: 0x004A6F90
		public override void PostDraw()
		{
			if (this.Props.graphicDrawerType == DrawerType.MapMeshOnly)
			{
				return;
			}
			if (this.IsHacked && this.Props.hackedGraphicData != null)
			{
				this.Props.hackedGraphicData.Graphic.Draw(this.parent.DrawPos + this.Props.hackedGraphicOffset, this.parent.Rotation, this.parent, 0f);
				return;
			}
			if (!this.IsHacked && this.Props.unhackedGraphicData != null)
			{
				this.Props.unhackedGraphicData.Graphic.Draw(this.parent.DrawPos + this.Props.unhackedGraphicOffset, this.parent.Rotation, this.parent, 0f);
			}
		}

		// Token: 0x060101B8 RID: 65976 RVA: 0x004A8E64 File Offset: 0x004A7064
		public override void PostPrintOnto(SectionLayer layer)
		{
			if (this.Props.graphicDrawerType == DrawerType.RealtimeOnly)
			{
				return;
			}
			if (this.IsHacked && this.Props.hackedGraphicData != null)
			{
				this.Props.hackedGraphicData.Graphic.Print(layer, this.parent, 0f);
				return;
			}
			if (!this.IsHacked && this.Props.unhackedGraphicData != null)
			{
				this.Props.unhackedGraphicData.Graphic.Print(layer, this.parent, 0f);
			}
		}

		// Token: 0x060101B9 RID: 65977 RVA: 0x004A8EED File Offset: 0x004A70ED
		public override bool DontDrawParent()
		{
			return (this.IsHacked && this.Props.hackedGraphicData != null) || (!this.IsHacked && this.Props.unhackedGraphicData != null);
		}

		// Token: 0x060101BA RID: 65978 RVA: 0x004A8F1E File Offset: 0x004A711E
		public override void CompTick()
		{
			if (this.lockedOutUntilTick >= 0 && this.lockedOutUntilTick <= Find.TickManager.TicksGame && !this.Props.lockoutPermanently)
			{
				this.EndLockout();
			}
		}

		// Token: 0x060101BB RID: 65979 RVA: 0x004A8F50 File Offset: 0x004A7150
		public void LockOut(Pawn hacker)
		{
			if (this.Props.lockoutPermanently)
			{
				this.lockedOutPermanently = true;
				Messages.Message("MessageHackerLockedOutPermanently".Translate(hacker.Named("HACKER")), this.parent, MessageTypeDefOf.NegativeEvent, true);
			}
			else
			{
				int num = this.Props.lockoutDurationHoursRange.RandomInRange * 2500;
				this.progressLastLockout = this.progress;
				this.lockedOutUntilTick = Find.TickManager.TicksGame + num;
				Messages.Message("MessageHackerLockedOut".Translate(hacker.Named("HACKER"), num.ToStringTicksToPeriod(true, false, true, true, false).Named("DURATION")), this.parent, MessageTypeDefOf.NegativeEvent, true);
			}
			this.OnLockedOut(hacker);
		}

		// Token: 0x060101BC RID: 65980 RVA: 0x004A9024 File Offset: 0x004A7224
		protected virtual void OnLockedOut(Pawn hacker = null)
		{
			Pawn arg = hacker ?? ((this.parent.Map.mapPawns.ColonistCount > 0) ? this.parent.Map.mapPawns.FreeColonists.RandomElement<Pawn>() : null);
			QuestUtility.SendQuestTargetSignals(this.parent.questTags, "LockedOut", this.parent.Named("SUBJECT"), arg.Named("ACTIVATOR"));
			IHackable hackable = this.parent as IHackable;
			if (hackable != null)
			{
				hackable.OnLockedOut(hacker);
			}
		}

		// Token: 0x060101BD RID: 65981 RVA: 0x004A90B2 File Offset: 0x004A72B2
		private void EndLockout()
		{
			this.lockedOutUntilTick = -1;
			this.lockedOutPermanently = false;
			Messages.Message("MessageHackableUnlocked".Translate(this.parent), this.parent, MessageTypeDefOf.PositiveEvent, true);
		}

		// Token: 0x060101BE RID: 65982 RVA: 0x004A90F4 File Offset: 0x004A72F4
		private AcceptanceReport CanHackNow(bool reportAlreadyHacked)
		{
			if (this.IsHacked && reportAlreadyHacked)
			{
				return "AlreadyHacked".Translate();
			}
			if (this.IsHacked)
			{
				return false;
			}
			if (this.LockedOut)
			{
				return "LockedOut".Translate();
			}
			return true;
		}

		// Token: 0x060101BF RID: 65983 RVA: 0x004A9148 File Offset: 0x004A7348
		public override IEnumerable<FloatMenuOption> CompMultiSelectFloatMenuOptions(IEnumerable<Pawn> selPawns)
		{
			if (this.Props.onlyRemotelyHackable)
			{
				yield break;
			}
			AcceptanceReport acceptanceReport = this.CanHackNow(false);
			if (!acceptanceReport.Accepted)
			{
				if (!this.IsHacked)
				{
					yield return new FloatMenuOption("CannotHack".Translate(this.parent.Label) + ": " + acceptanceReport.Reason.CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				yield break;
			}
			CompHackable.tmpAllowedPawns.Clear();
			foreach (Pawn pawn in selPawns)
			{
				if (pawn.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					CompHackable.tmpAllowedPawns.Add(pawn);
				}
			}
			if (CompHackable.tmpAllowedPawns.Count <= 0)
			{
				yield return new FloatMenuOption("CannotHack".Translate(this.parent.Label) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			CompHackable.tmpAllowedPawns.Clear();
			foreach (Pawn pawn2 in selPawns)
			{
				if (HackUtility.IsCapableOfHacking(pawn2))
				{
					CompHackable.tmpAllowedPawns.Add(pawn2);
				}
			}
			if (CompHackable.tmpAllowedPawns.Count <= 0)
			{
				yield return new FloatMenuOption("CannotHack".Translate(this.parent.Label) + ": " + "IncapableOfHacking".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			CompHackable.tmpAllowedPawns.Clear();
			if (this.Props.intellectualSkillPrerequisite > 0)
			{
				foreach (Pawn pawn3 in selPawns)
				{
					Pawn_SkillTracker skills = pawn3.skills;
					int? num = (skills != null) ? new int?(skills.GetSkill(SkillDefOf.Intellectual).Level) : null;
					int intellectualSkillPrerequisite = this.Props.intellectualSkillPrerequisite;
					if (num.GetValueOrDefault() >= intellectualSkillPrerequisite & num != null)
					{
						CompHackable.tmpAllowedPawns.Add(pawn3);
					}
				}
				if (CompHackable.tmpAllowedPawns.Count <= 0)
				{
					yield return new FloatMenuOption("CannotHack".Translate(this.parent.Label) + ": " + "SkillTooLow".Translate(SkillDefOf.Intellectual.label, selPawns.First<Pawn>().skills.GetSkill(SkillDefOf.Intellectual).Level, this.Props.intellectualSkillPrerequisite), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					yield break;
				}
				CompHackable.tmpAllowedPawns.Clear();
			}
			foreach (Pawn pawn4 in selPawns)
			{
				if (HackUtility.IsCapableOfHacking(pawn4) && pawn4.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					CompHackable.tmpAllowedPawns.Add(pawn4);
				}
			}
			if (CompHackable.tmpAllowedPawns.Count > 0)
			{
				yield return new FloatMenuOption("Hack".Translate(this.parent.Label), delegate()
				{
					CompHackable.tmpAllowedPawns[0].jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Hack, this.parent), new JobTag?(JobTag.Misc), false);
					for (int i = 1; i < CompHackable.tmpAllowedPawns.Count; i++)
					{
						FloatMenuOptionProvider_DraftedMove.PawnGotoAction(this.parent.Position, CompHackable.tmpAllowedPawns[i], RCellFinder.BestOrderedGotoDestNear(this.parent.Position, CompHackable.tmpAllowedPawns[i], null, true));
					}
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
		}

		// Token: 0x060101C0 RID: 65984 RVA: 0x004A9160 File Offset: 0x004A7360
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.progress, "progress", 0f, false);
			Scribe_Values.Look<float>(ref this.lastUserSpeed, "lastUserSpeed", 0f, false);
			Scribe_Values.Look<int>(ref this.lastHackTick, "lastHackTick", 0, false);
			Scribe_Values.Look<float>(ref this.defence, "defence", 0f, false);
			Scribe_References.Look<Pawn>(ref this.lastUser, "lasterUser", false);
			Scribe_Values.Look<string>(ref this.hackingStartedSignal, "hackingStartedSignal", null, false);
			Scribe_Values.Look<string>(ref this.hackingCompletedSignal, "hackingCompletedSignal", null, false);
			Scribe_Values.Look<int>(ref this.lockedOutUntilTick, "lockedOutUntilTick", -1, false);
			Scribe_Values.Look<float>(ref this.progressLastLockout, "progressLastLockout", 0f, false);
			Scribe_Values.Look<bool>(ref this.lockedOutPermanently, "lockedOutPermanently", false, false);
			Scribe_Values.Look<bool>(ref this.sentLetter, "sentLetter", false, false);
			Scribe_Values.Look<bool>(ref this.autohack, "autohack", false, false);
			Scribe_Values.Look<bool>(ref this.hacked, "hacked", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.parent.Spawned)
			{
				this.hacked = (this.hacked || this.progress >= this.defence);
			}
		}

		// Token: 0x060101C1 RID: 65985 RVA: 0x004A92A4 File Offset: 0x004A74A4
		public override string CompInspectStringExtra()
		{
			TaggedString t = TaggedString.Empty;
			bool isHacked = this.IsHacked;
			if (!isHacked && !this.Props.notHackedInspectString.NullOrEmpty())
			{
				t += this.Props.notHackedInspectString;
			}
			if ((!isHacked || this.Props.showProgressAfterHackCompletion) && !this.Props.onlyRemotelyHackable)
			{
				if (t.Length > 0)
				{
					t += "\n";
				}
				t += "HackProgress".Translate() + ": " + this.progress.ToStringWorkAmount() + " / " + this.defence.ToStringWorkAmount();
				if (isHacked)
				{
					t += " (" + "Hacked".Translate() + ")";
				}
			}
			if (isHacked)
			{
				if (!this.Props.hackedInspectString.NullOrEmpty())
				{
					if (t.Length > 0)
					{
						t += "\n";
					}
					t += this.Props.hackedInspectString;
				}
			}
			else
			{
				if (this.LockedOut)
				{
					if (t.Length > 0)
					{
						t += "\n";
					}
					t += ("LockedOut".Translate() + ": " + (this.lockedOutUntilTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false)).Colorize(ColorLibrary.RedReadable);
				}
				if (this.lastHackTick > Find.TickManager.TicksGame - 30)
				{
					string str;
					if (this.lastUser != null)
					{
						str = "HackingLastUser".Translate(this.lastUser) + " " + StatDefOf.HackingSpeed.label;
					}
					else
					{
						str = StatDefOf.HackingSpeed.LabelCap;
					}
					if (t.Length > 0)
					{
						t += "\n";
					}
					t += str + ": " + StatDefOf.HackingSpeed.ValueToString(this.lastUserSpeed, ToStringNumberSense.Absolute, true);
				}
			}
			if (!this.IsHacked && this.Props.intellectualSkillPrerequisite > 0)
			{
				if (t.Length > 0)
				{
					t += "\n";
				}
				t += "IntellectualSkillPrerequisite".Translate() + ": " + this.Props.intellectualSkillPrerequisite.ToString();
			}
			return t.Resolve();
		}

		// Token: 0x060101C2 RID: 65986 RVA: 0x004A953B File Offset: 0x004A773B
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.Props.onlyRemotelyHackable)
			{
				yield break;
			}
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "HackProgress".Translate(), this.progress.ToStringWorkAmount() + " / " + this.defence.ToStringWorkAmount(), "Stat_Thing_HackingProgress".Translate(), 3100, null, null, false, false);
			yield break;
		}

		// Token: 0x060101C3 RID: 65987 RVA: 0x004A954C File Offset: 0x004A774C
		private AcceptanceReport ValidateHacker(LocalTargetInfo target)
		{
			Pawn pawn = target.Thing as Pawn;
			if (pawn == null)
			{
				return false;
			}
			if (!pawn.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return "NoPath".Translate();
			}
			if (!HackUtility.IsCapableOfHacking(pawn))
			{
				return "IncapableOfHacking".Translate();
			}
			if (this.Props.intellectualSkillPrerequisite > 0)
			{
				Pawn_SkillTracker skills = pawn.skills;
				int? num = (skills != null) ? new int?(skills.GetSkill(SkillDefOf.Intellectual).Level) : null;
				int intellectualSkillPrerequisite = this.Props.intellectualSkillPrerequisite;
				if (num.GetValueOrDefault() < intellectualSkillPrerequisite & num != null)
				{
					return "SkillTooLow".Translate(SkillDefOf.Intellectual.label, pawn.skills.GetSkill(SkillDefOf.Intellectual).Level, this.Props.intellectualSkillPrerequisite);
				}
			}
			return true;
		}

		// Token: 0x060101C4 RID: 65988 RVA: 0x004A965A File Offset: 0x004A785A
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!this.Props.onlyRemotelyHackable)
			{
				if (!this.IsHacked)
				{
					yield return new Command_Toggle
					{
						defaultLabel = "AutoHack".Translate(),
						defaultDesc = "AutoHackDesc".Translate(),
						icon = CompHackable.AutohackTexture,
						hotKey = KeyBindingDefOf.Misc3,
						isActive = (() => this.autohack),
						toggleAction = delegate()
						{
							this.autohack = !this.autohack;
							if (this.autohack && !this.Props.autohackWarningString.NullOrEmpty())
							{
								Messages.Message(this.Props.autohackWarningString.Formatted(this.parent), this.parent, MessageTypeDefOf.NegativeEvent, false);
							}
						}
					};
				}
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = "HackGizmo".Translate(),
					defaultDesc = "HackDesc".Translate(),
					icon = CompHackable.HackTexture,
					action = delegate()
					{
						Find.Targeter.BeginTargeting(TargetingParameters.ForColonist(), delegate(LocalTargetInfo target)
						{
							Pawn pawn = target.Pawn;
							if (pawn == null)
							{
								return;
							}
							pawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Hack, this.parent), new JobTag?(JobTag.Misc), false);
						}, delegate(LocalTargetInfo target)
						{
							Pawn pawn = target.Pawn;
							if (pawn != null && pawn.IsColonistPlayerControlled)
							{
								GenDraw.DrawTargetHighlight(target);
							}
						}, (LocalTargetInfo target) => this.ValidateHacker(target).Accepted, null, null, CompHackable.HackTexture, true, delegate(LocalTargetInfo target)
						{
							AcceptanceReport acceptanceReport2 = this.ValidateHacker(target);
							Pawn pawn = target.Pawn;
							if (pawn != null && pawn.IsColonistPlayerControlled && !acceptanceReport2.Accepted)
							{
								Widgets.MouseAttachedLabel(("CannotChooseHacker".Translate() + ": " + acceptanceReport2.Reason.CapitalizeFirst()).Colorize(ColorLibrary.RedReadable), 0f, 0f, null);
								return;
							}
							Widgets.MouseAttachedLabel("CommandChooseHacker".Translate(), 0f, 0f, null);
						}, null);
					}
				};
				AcceptanceReport acceptanceReport = this.CanHackNow(true);
				if (!acceptanceReport.Accepted)
				{
					command_Action.Disable(acceptanceReport.Reason.CapitalizeFirst());
				}
				yield return command_Action;
			}
			if (DebugSettings.ShowDevGizmos && !this.IsHacked)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Hack +10%",
					action = delegate()
					{
						this.Hack(this.defence * 0.1f, null, false);
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Complete hack",
					action = delegate()
					{
						this.HackNow();
					}
				};
				if (this.LockedOut)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Unlock",
						action = new Action(this.EndLockout)
					};
				}
			}
			yield break;
		}

		// Token: 0x060101C5 RID: 65989 RVA: 0x004A966C File Offset: 0x004A786C
		public AcceptanceReport CanHackNow(Pawn pawn)
		{
			if (this.Props.onlyRemotelyHackable || this.IsHacked)
			{
				return false;
			}
			if (this.LockedOut)
			{
				return "LockedOut".Translate();
			}
			if (!HackUtility.IsCapableOfHacking(pawn))
			{
				return "IncapableOfHacking".Translate();
			}
			if (!pawn.CanReach(this.parent, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return "NoPath".Translate().CapitalizeFirst();
			}
			if (this.Props.intellectualSkillPrerequisite > 0 && pawn.skills.GetSkill(SkillDefOf.Intellectual).Level < this.Props.intellectualSkillPrerequisite)
			{
				return "SkillTooLow".Translate(SkillDefOf.Intellectual.label, pawn.skills.GetSkill(SkillDefOf.Intellectual).Level, this.Props.intellectualSkillPrerequisite);
			}
			return true;
		}

		// Token: 0x0400A858 RID: 43096
		public string hackingStartedSignal;

		// Token: 0x0400A859 RID: 43097
		public string hackingCompletedSignal;

		// Token: 0x0400A85A RID: 43098
		private float progress;

		// Token: 0x0400A85B RID: 43099
		public float defence;

		// Token: 0x0400A85C RID: 43100
		private float lastUserSpeed = 1f;

		// Token: 0x0400A85D RID: 43101
		private int lastHackTick = -1;

		// Token: 0x0400A85E RID: 43102
		private Pawn lastUser;

		// Token: 0x0400A85F RID: 43103
		private float progressLastLockout;

		// Token: 0x0400A860 RID: 43104
		private int lockedOutUntilTick = -1;

		// Token: 0x0400A861 RID: 43105
		private bool lockedOutPermanently;

		// Token: 0x0400A862 RID: 43106
		private bool sentLetter;

		// Token: 0x0400A863 RID: 43107
		private bool autohack;

		// Token: 0x0400A864 RID: 43108
		private bool hacked;

		// Token: 0x0400A865 RID: 43109
		public const string HackedSignal = "Hacked";

		// Token: 0x0400A866 RID: 43110
		private const float MinHackProgressBeforeLockout = 3000f;

		// Token: 0x0400A867 RID: 43111
		private static readonly Texture2D HackTexture = ContentFinder<Texture2D>.Get("UI/Commands/Hack", true);

		// Token: 0x0400A868 RID: 43112
		private static readonly Texture2D AutohackTexture = ContentFinder<Texture2D>.Get("UI/Commands/Autohack", true);

		// Token: 0x0400A869 RID: 43113
		private static readonly List<Thing> tmpSpawnedThings = new List<Thing>();

		// Token: 0x0400A86A RID: 43114
		private static readonly List<Pawn> tmpAllowedPawns = new List<Pawn>();
	}
}
