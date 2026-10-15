using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

[StaticConstructorOnStartup]
public class CompHackable : ThingComp, IThingGlower
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__51 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompHackable _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__51(int _003C_003E1__state)
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
			CompHackable CS_0024_003C_003E8__locals20 = _003C_003E4__this;
			Command_Action command_Action;
			AcceptanceReport acceptanceReport2;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!CS_0024_003C_003E8__locals20.Props.onlyRemotelyHackable)
				{
					if (!CS_0024_003C_003E8__locals20.IsHacked)
					{
						_003C_003E2__current = new Command_Toggle
						{
							defaultLabel = "AutoHack".Translate(),
							defaultDesc = "AutoHackDesc".Translate(),
							icon = AutohackTexture,
							hotKey = KeyBindingDefOf.Misc3,
							isActive = () => CS_0024_003C_003E8__locals20.autohack,
							toggleAction = delegate
							{
								CS_0024_003C_003E8__locals20.autohack = !CS_0024_003C_003E8__locals20.autohack;
								if (CS_0024_003C_003E8__locals20.autohack && !CS_0024_003C_003E8__locals20.Props.autohackWarningString.NullOrEmpty())
								{
									Messages.Message(CS_0024_003C_003E8__locals20.Props.autohackWarningString.Formatted(CS_0024_003C_003E8__locals20.parent), CS_0024_003C_003E8__locals20.parent, MessageTypeDefOf.NegativeEvent, historical: false);
								}
							}
						};
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00cc;
				}
				goto IL_0153;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00cc;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0153;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Complete hack",
					action = delegate
					{
						CS_0024_003C_003E8__locals20.HackNow();
					}
				};
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals20.LockedOut)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Unlock",
						action = CS_0024_003C_003E8__locals20.EndLockout
					};
					_003C_003E1__state = 5;
					return true;
				}
				break;
			case 5:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_00cc:
				command_Action = new Command_Action
				{
					defaultLabel = "HackGizmo".Translate(),
					defaultDesc = "HackDesc".Translate(),
					icon = HackTexture,
					action = delegate
					{
						Find.Targeter.BeginTargeting(TargetingParameters.ForColonist(), delegate(LocalTargetInfo target)
						{
							target.Pawn?.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Hack, CS_0024_003C_003E8__locals20.parent), JobTag.Misc);
						}, delegate(LocalTargetInfo target)
						{
							Pawn pawn = target.Pawn;
							if (pawn != null && pawn.IsColonistPlayerControlled)
							{
								GenDraw.DrawTargetHighlight(target);
							}
						}, (LocalTargetInfo target) => CS_0024_003C_003E8__locals20.ValidateHacker(target).Accepted, null, null, HackTexture, playSoundOnAction: true, delegate(LocalTargetInfo target)
						{
							AcceptanceReport acceptanceReport = CS_0024_003C_003E8__locals20.ValidateHacker(target);
							Pawn pawn2 = target.Pawn;
							if (pawn2 != null && pawn2.IsColonistPlayerControlled && !acceptanceReport.Accepted)
							{
								Widgets.MouseAttachedLabel(("CannotChooseHacker".Translate() + ": " + acceptanceReport.Reason.CapitalizeFirst()).Colorize(ColorLibrary.RedReadable), 0f, 0f, null);
							}
							else
							{
								Widgets.MouseAttachedLabel("CommandChooseHacker".Translate(), 0f, 0f, null);
							}
						});
					}
				};
				acceptanceReport2 = CS_0024_003C_003E8__locals20.CanHackNow(reportAlreadyHacked: true);
				if (!acceptanceReport2.Accepted)
				{
					command_Action.Disable(acceptanceReport2.Reason.CapitalizeFirst());
				}
				_003C_003E2__current = command_Action;
				_003C_003E1__state = 2;
				return true;
				IL_0153:
				if (DebugSettings.ShowDevGizmos && !CS_0024_003C_003E8__locals20.IsHacked)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Hack +10%",
						action = delegate
						{
							CS_0024_003C_003E8__locals20.Hack(CS_0024_003C_003E8__locals20.defence * 0.1f);
						}
					};
					_003C_003E1__state = 3;
					return true;
				}
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
			_003CCompGetGizmosExtra_003Ed__51 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__51(0)
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

	[CompilerGenerated]
	private sealed class _003CCompMultiSelectFloatMenuOptions_003Ed__46 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompHackable _003C_003E4__this;

		private IEnumerable<Pawn> selPawns;

		public IEnumerable<Pawn> _003C_003E3__selPawns;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CCompMultiSelectFloatMenuOptions_003Ed__46(int _003C_003E1__state)
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
			CompHackable CS_0024_003C_003E8__locals16 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals16.Props.onlyRemotelyHackable)
				{
					return false;
				}
				AcceptanceReport acceptanceReport = CS_0024_003C_003E8__locals16.CanHackNow(reportAlreadyHacked: false);
				if (!acceptanceReport.Accepted)
				{
					if (!CS_0024_003C_003E8__locals16.IsHacked)
					{
						_003C_003E2__current = new FloatMenuOption("CannotHack".Translate(CS_0024_003C_003E8__locals16.parent.Label) + ": " + acceptanceReport.Reason.CapitalizeFirst(), null);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00bf;
				}
				tmpAllowedPawns.Clear();
				foreach (Pawn item in selPawns)
				{
					if (item.CanReach(CS_0024_003C_003E8__locals16.parent, PathEndMode.Touch, Danger.Deadly))
					{
						tmpAllowedPawns.Add(item);
					}
				}
				if (tmpAllowedPawns.Count <= 0)
				{
					_003C_003E2__current = new FloatMenuOption("CannotHack".Translate(CS_0024_003C_003E8__locals16.parent.Label) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
					_003C_003E1__state = 2;
					return true;
				}
				tmpAllowedPawns.Clear();
				foreach (Pawn item2 in selPawns)
				{
					if (HackUtility.IsCapableOfHacking(item2))
					{
						tmpAllowedPawns.Add(item2);
					}
				}
				if (tmpAllowedPawns.Count <= 0)
				{
					_003C_003E2__current = new FloatMenuOption("CannotHack".Translate(CS_0024_003C_003E8__locals16.parent.Label) + ": " + "IncapableOfHacking".Translate(), null);
					_003C_003E1__state = 3;
					return true;
				}
				tmpAllowedPawns.Clear();
				if (CS_0024_003C_003E8__locals16.Props.intellectualSkillPrerequisite > 0)
				{
					foreach (Pawn item3 in selPawns)
					{
						if (item3.skills?.GetSkill(SkillDefOf.Intellectual).Level >= CS_0024_003C_003E8__locals16.Props.intellectualSkillPrerequisite)
						{
							tmpAllowedPawns.Add(item3);
						}
					}
					if (tmpAllowedPawns.Count <= 0)
					{
						_003C_003E2__current = new FloatMenuOption("CannotHack".Translate(CS_0024_003C_003E8__locals16.parent.Label) + ": " + "SkillTooLow".Translate(SkillDefOf.Intellectual.label, selPawns.First().skills.GetSkill(SkillDefOf.Intellectual).Level, CS_0024_003C_003E8__locals16.Props.intellectualSkillPrerequisite), null);
						_003C_003E1__state = 4;
						return true;
					}
					tmpAllowedPawns.Clear();
				}
				foreach (Pawn item4 in selPawns)
				{
					if (HackUtility.IsCapableOfHacking(item4) && item4.CanReach(CS_0024_003C_003E8__locals16.parent, PathEndMode.Touch, Danger.Deadly))
					{
						tmpAllowedPawns.Add(item4);
					}
				}
				if (tmpAllowedPawns.Count <= 0)
				{
					break;
				}
				_003C_003E2__current = new FloatMenuOption("Hack".Translate(CS_0024_003C_003E8__locals16.parent.Label), delegate
				{
					tmpAllowedPawns[0].jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Hack, CS_0024_003C_003E8__locals16.parent), JobTag.Misc);
					for (int i = 1; i < tmpAllowedPawns.Count; i++)
					{
						FloatMenuOptionProvider_DraftedMove.PawnGotoAction(CS_0024_003C_003E8__locals16.parent.Position, tmpAllowedPawns[i], RCellFinder.BestOrderedGotoDestNear(CS_0024_003C_003E8__locals16.parent.Position, tmpAllowedPawns[i]));
					}
				});
				_003C_003E1__state = 5;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_00bf;
			case 2:
				_003C_003E1__state = -1;
				return false;
			case 3:
				_003C_003E1__state = -1;
				return false;
			case 4:
				_003C_003E1__state = -1;
				return false;
			case 5:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_00bf:
				return false;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CCompMultiSelectFloatMenuOptions_003Ed__46 _003CCompMultiSelectFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCompMultiSelectFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CCompMultiSelectFloatMenuOptions_003Ed__ = new _003CCompMultiSelectFloatMenuOptions_003Ed__46(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CCompMultiSelectFloatMenuOptions_003Ed__.selPawns = _003C_003E3__selPawns;
			return _003CCompMultiSelectFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetAdditionalLeavings_003Ed__32 : IEnumerable<ThingDefCountClass>, IEnumerable, IEnumerator<ThingDefCountClass>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDefCountClass _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompHackable _003C_003E4__this;

		private List<ThingDefCountClass>.Enumerator _003C_003E7__wrap1;

		ThingDefCountClass IEnumerator<ThingDefCountClass>.Current
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
		public _003CGetAdditionalLeavings_003Ed__32(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<ThingDefCountClass>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompHackable compHackable = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (compHackable.IsHacked)
					{
						return false;
					}
					_003C_003E7__wrap1 = compHackable.Props.dropOnDestroyed.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					ThingDefCountClass current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<ThingDefCountClass>.Enumerator);
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ThingDefCountClass> IEnumerable<ThingDefCountClass>.GetEnumerator()
		{
			_003CGetAdditionalLeavings_003Ed__32 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetAdditionalLeavings_003Ed__32(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDefCountClass>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__49 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompHackable _003C_003E4__this;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__49(int _003C_003E1__state)
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
			CompHackable compHackable = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (compHackable.Props.onlyRemotelyHackable)
				{
					return false;
				}
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Basics, "HackProgress".Translate(), compHackable.progress.ToStringWorkAmount() + " / " + compHackable.defence.ToStringWorkAmount(), "Stat_Thing_HackingProgress".Translate(), 3100);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__49 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__49(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	public string hackingStartedSignal;

	public string hackingCompletedSignal;

	private float progress;

	public float defence;

	private float lastUserSpeed = 1f;

	private int lastHackTick = -1;

	private Pawn lastUser;

	private float progressLastLockout;

	private int lockedOutUntilTick = -1;

	private bool lockedOutPermanently;

	private bool sentLetter;

	private bool autohack;

	private bool hacked;

	public const string HackedSignal = "Hacked";

	private const float MinHackProgressBeforeLockout = 3000f;

	private static readonly Texture2D HackTexture = ContentFinder<Texture2D>.Get("UI/Commands/Hack");

	private static readonly Texture2D AutohackTexture = ContentFinder<Texture2D>.Get("UI/Commands/Autohack");

	private static readonly List<Thing> tmpSpawnedThings = new List<Thing>();

	private static readonly List<Pawn> tmpAllowedPawns = new List<Pawn>();

	public CompProperties_Hackable Props => (CompProperties_Hackable)props;

	public float ProgressPercent => progress / defence;

	public bool Autohack => autohack;

	public bool IsHacked => hacked;

	public bool LockedOut
	{
		get
		{
			if (!lockedOutPermanently)
			{
				return lockedOutUntilTick > Find.TickManager.TicksGame;
			}
			return true;
		}
	}

	public bool ShouldBeLitNow()
	{
		if (IsHacked)
		{
			return Props.glowIfHacked;
		}
		return true;
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (!respawningAfterLoad && !parent.BeingTransportedOnGravship)
		{
			defence = Props.defence;
		}
		base.PostSpawnSetup(respawningAfterLoad);
	}

	public void HackNow()
	{
		progress = defence + 1f;
		ProcessHacked(null, suppressMessages: true);
	}

	public void Hack(float amount, Pawn hacker = null, bool suppressMessages = false)
	{
		bool isHacked = IsHacked;
		progress += amount;
		progress = Mathf.Clamp(progress, 0f, defence);
		if (!isHacked && progress >= defence)
		{
			ProcessHacked(hacker, suppressMessages);
		}
		if (lastHackTick < 0)
		{
			if (!hackingStartedSignal.NullOrEmpty())
			{
				Find.SignalManager.SendSignal(new Signal(hackingStartedSignal, parent.Named("SUBJECT")));
			}
			QuestUtility.SendQuestTargetSignals(parent.questTags, "HackingStarted", parent.Named("SUBJECT"));
		}
		lastUserSpeed = amount;
		lastHackTick = Find.TickManager.TicksGame;
		lastUser = hacker;
		if (hacker != null && (Props.lockoutDurationHoursRange.TrueMax > 0 || Props.lockoutPermanently) && !IsHacked && progress - progressLastLockout > 3000f && Rand.MTBEventOccurs(hacker.GetStatValue(StatDefOf.HackingStealth), amount * 60f, 1f))
		{
			LockOut(hacker);
		}
	}

	private void ProcessHacked(Pawn hacker, bool suppressMessages)
	{
		if (hacked)
		{
			return;
		}
		hacked = true;
		if (!hackingCompletedSignal.NullOrEmpty())
		{
			Find.SignalManager.SendSignal(new Signal(hackingCompletedSignal, parent.Named("SUBJECT")));
		}
		Pawn arg = hacker ?? ((Current.ProgramState == ProgramState.Playing && parent.Map.mapPawns.ColonistCount > 0) ? parent.Map.mapPawns.FreeColonists.RandomElement() : null);
		QuestUtility.SendQuestTargetSignals(parent.questTags, "Hacked", parent.Named("SUBJECT"), arg.Named("ACTIVATOR"));
		parent.BroadcastCompSignal("Hacked");
		if (Props.completedQuest != null)
		{
			Slate slate = new Slate();
			slate.Set("map", parent.Map);
			if (Props.completedQuest.CanRun(slate, parent.Map))
			{
				QuestUtility.GenerateQuestAndMakeAvailable(Props.completedQuest, slate);
			}
		}
		OnHacked(hacker, suppressMessages);
	}

	[IteratorStateMachine(typeof(_003CGetAdditionalLeavings_003Ed__32))]
	public override IEnumerable<ThingDefCountClass> GetAdditionalLeavings(Map map, DestroyMode mode)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetAdditionalLeavings_003Ed__32(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostDestroy(DestroyMode mode, Map previousMap)
	{
		if (!sentLetter && !Props.destroyedLetterLabel.NullOrEmpty() && !Props.destroyedLetterText.NullOrEmpty())
		{
			sentLetter = true;
			Find.LetterStack.ReceiveLetter(Props.destroyedLetterLabel, Props.destroyedLetterText, Props.destroyedLetterDef);
		}
	}

	public override bool CompPreventClaimingBy(Faction faction)
	{
		if (faction.IsPlayer)
		{
			return !IsHacked;
		}
		return false;
	}

	protected virtual void OnHacked(Pawn hacker = null, bool suppressMessages = false)
	{
		if (parent is IHackable hackable)
		{
			hackable.OnHacked(hacker);
		}
		foreach (ThingComp allComp in parent.AllComps)
		{
			allComp.Notify_Hacked(hacker);
		}
		GenLeaving.DropThingsNear(parent, Props.dropOnHacked, tmpSpawnedThings);
		if (!Props.hackedMessage.NullOrEmpty() && hacker != null && !suppressMessages)
		{
			Messages.Message(Props.hackedMessage.Formatted(hacker.Named("HACKER"), parent.Named("SUBJECT")), parent, MessageTypeDefOf.PositiveEvent, historical: false);
		}
		if (!sentLetter && !Props.hackedLetterLabel.NullOrEmpty() && !Props.hackedLetterText.NullOrEmpty() && !suppressMessages)
		{
			sentLetter = true;
			Find.LetterStack.ReceiveLetter(Props.hackedLetterLabel, Props.hackedLetterText, Props.hackedLetterDef, tmpSpawnedThings);
		}
		tmpSpawnedThings.Clear();
		if (parent.Spawned)
		{
			parent.DirtyMapMesh(parent.Map);
		}
	}

	public override void PostDraw()
	{
		if (Props.graphicDrawerType != DrawerType.MapMeshOnly)
		{
			if (IsHacked && Props.hackedGraphicData != null)
			{
				Props.hackedGraphicData.Graphic.Draw(parent.DrawPos + Props.hackedGraphicOffset, parent.Rotation, parent);
			}
			else if (!IsHacked && Props.unhackedGraphicData != null)
			{
				Props.unhackedGraphicData.Graphic.Draw(parent.DrawPos + Props.unhackedGraphicOffset, parent.Rotation, parent);
			}
		}
	}

	public override void PostPrintOnto(SectionLayer layer)
	{
		if (Props.graphicDrawerType != DrawerType.RealtimeOnly)
		{
			if (IsHacked && Props.hackedGraphicData != null)
			{
				Props.hackedGraphicData.Graphic.Print(layer, parent, 0f);
			}
			else if (!IsHacked && Props.unhackedGraphicData != null)
			{
				Props.unhackedGraphicData.Graphic.Print(layer, parent, 0f);
			}
		}
	}

	public override bool DontDrawParent()
	{
		if (IsHacked && Props.hackedGraphicData != null)
		{
			return true;
		}
		if (!IsHacked && Props.unhackedGraphicData != null)
		{
			return true;
		}
		return false;
	}

	public override void CompTick()
	{
		if (lockedOutUntilTick >= 0 && lockedOutUntilTick <= Find.TickManager.TicksGame && !Props.lockoutPermanently)
		{
			EndLockout();
		}
	}

	public void LockOut(Pawn hacker)
	{
		if (Props.lockoutPermanently)
		{
			lockedOutPermanently = true;
			Messages.Message("MessageHackerLockedOutPermanently".Translate(hacker.Named("HACKER")), parent, MessageTypeDefOf.NegativeEvent);
		}
		else
		{
			int num = Props.lockoutDurationHoursRange.RandomInRange * 2500;
			progressLastLockout = progress;
			lockedOutUntilTick = Find.TickManager.TicksGame + num;
			Messages.Message("MessageHackerLockedOut".Translate(hacker.Named("HACKER"), num.ToStringTicksToPeriod().Named("DURATION")), parent, MessageTypeDefOf.NegativeEvent);
		}
		OnLockedOut(hacker);
	}

	protected virtual void OnLockedOut(Pawn hacker = null)
	{
		Pawn arg = hacker ?? ((parent.Map.mapPawns.ColonistCount > 0) ? parent.Map.mapPawns.FreeColonists.RandomElement() : null);
		QuestUtility.SendQuestTargetSignals(parent.questTags, "LockedOut", parent.Named("SUBJECT"), arg.Named("ACTIVATOR"));
		if (parent is IHackable hackable)
		{
			hackable.OnLockedOut(hacker);
		}
	}

	private void EndLockout()
	{
		lockedOutUntilTick = -1;
		lockedOutPermanently = false;
		Messages.Message("MessageHackableUnlocked".Translate(parent), parent, MessageTypeDefOf.PositiveEvent);
	}

	private AcceptanceReport CanHackNow(bool reportAlreadyHacked)
	{
		if (IsHacked && reportAlreadyHacked)
		{
			return "AlreadyHacked".Translate();
		}
		if (IsHacked)
		{
			return false;
		}
		if (LockedOut)
		{
			return "LockedOut".Translate();
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CCompMultiSelectFloatMenuOptions_003Ed__46))]
	public override IEnumerable<FloatMenuOption> CompMultiSelectFloatMenuOptions(IEnumerable<Pawn> selPawns)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompMultiSelectFloatMenuOptions_003Ed__46(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawns = selPawns
		};
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref progress, "progress", 0f);
		Scribe_Values.Look(ref lastUserSpeed, "lastUserSpeed", 0f);
		Scribe_Values.Look(ref lastHackTick, "lastHackTick", 0);
		Scribe_Values.Look(ref defence, "defence", 0f);
		Scribe_References.Look(ref lastUser, "lasterUser");
		Scribe_Values.Look(ref hackingStartedSignal, "hackingStartedSignal");
		Scribe_Values.Look(ref hackingCompletedSignal, "hackingCompletedSignal");
		Scribe_Values.Look(ref lockedOutUntilTick, "lockedOutUntilTick", -1);
		Scribe_Values.Look(ref progressLastLockout, "progressLastLockout", 0f);
		Scribe_Values.Look(ref lockedOutPermanently, "lockedOutPermanently", defaultValue: false);
		Scribe_Values.Look(ref sentLetter, "sentLetter", defaultValue: false);
		Scribe_Values.Look(ref autohack, "autohack", defaultValue: false);
		Scribe_Values.Look(ref hacked, "hacked", defaultValue: false);
		if (Scribe.mode == LoadSaveMode.PostLoadInit && parent.Spawned)
		{
			hacked = hacked || progress >= defence;
		}
	}

	public override string CompInspectStringExtra()
	{
		TaggedString empty = TaggedString.Empty;
		bool isHacked = IsHacked;
		if (!isHacked && !Props.notHackedInspectString.NullOrEmpty())
		{
			empty += Props.notHackedInspectString;
		}
		if ((!isHacked || Props.showProgressAfterHackCompletion) && !Props.onlyRemotelyHackable)
		{
			if (empty.Length > 0)
			{
				empty += "\n";
			}
			empty += "HackProgress".Translate() + ": " + progress.ToStringWorkAmount() + " / " + defence.ToStringWorkAmount();
			if (isHacked)
			{
				empty += " (" + "Hacked".Translate() + ")";
			}
		}
		if (isHacked)
		{
			if (!Props.hackedInspectString.NullOrEmpty())
			{
				if (empty.Length > 0)
				{
					empty += "\n";
				}
				empty += Props.hackedInspectString;
			}
		}
		else
		{
			if (LockedOut)
			{
				if (empty.Length > 0)
				{
					empty += "\n";
				}
				empty += ("LockedOut".Translate() + ": " + (lockedOutUntilTick - Find.TickManager.TicksGame).ToStringTicksToPeriod()).Colorize(ColorLibrary.RedReadable);
			}
			if (lastHackTick > Find.TickManager.TicksGame - 30)
			{
				string text = ((lastUser == null) ? ((string)StatDefOf.HackingSpeed.LabelCap) : ((string)("HackingLastUser".Translate(lastUser) + " " + StatDefOf.HackingSpeed.label)));
				if (empty.Length > 0)
				{
					empty += "\n";
				}
				empty += text + ": " + StatDefOf.HackingSpeed.ValueToString(lastUserSpeed);
			}
		}
		if (!IsHacked && Props.intellectualSkillPrerequisite > 0)
		{
			if (empty.Length > 0)
			{
				empty += "\n";
			}
			empty += string.Concat("IntellectualSkillPrerequisite".Translate() + ": ", Props.intellectualSkillPrerequisite.ToString());
		}
		return empty.Resolve();
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__49))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__49(-2)
		{
			_003C_003E4__this = this
		};
	}

	private AcceptanceReport ValidateHacker(LocalTargetInfo target)
	{
		if (!(target.Thing is Pawn pawn))
		{
			return false;
		}
		if (!pawn.CanReach(parent, PathEndMode.Touch, Danger.Deadly))
		{
			return "NoPath".Translate();
		}
		if (!HackUtility.IsCapableOfHacking(pawn))
		{
			return "IncapableOfHacking".Translate();
		}
		if (Props.intellectualSkillPrerequisite > 0 && pawn.skills?.GetSkill(SkillDefOf.Intellectual).Level < Props.intellectualSkillPrerequisite)
		{
			return "SkillTooLow".Translate(SkillDefOf.Intellectual.label, pawn.skills.GetSkill(SkillDefOf.Intellectual).Level, Props.intellectualSkillPrerequisite);
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__51))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__51(-2)
		{
			_003C_003E4__this = this
		};
	}

	public AcceptanceReport CanHackNow(Pawn pawn)
	{
		if (Props.onlyRemotelyHackable || IsHacked)
		{
			return false;
		}
		if (LockedOut)
		{
			return "LockedOut".Translate();
		}
		if (!HackUtility.IsCapableOfHacking(pawn))
		{
			return "IncapableOfHacking".Translate();
		}
		if (!pawn.CanReach(parent, PathEndMode.ClosestTouch, Danger.Deadly))
		{
			return "NoPath".Translate().CapitalizeFirst();
		}
		if (Props.intellectualSkillPrerequisite > 0 && pawn.skills.GetSkill(SkillDefOf.Intellectual).Level < Props.intellectualSkillPrerequisite)
		{
			return "SkillTooLow".Translate(SkillDefOf.Intellectual.label, pawn.skills.GetSkill(SkillDefOf.Intellectual).Level, Props.intellectualSkillPrerequisite);
		}
		return true;
	}
}
