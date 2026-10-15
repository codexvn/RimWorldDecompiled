using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse;

public class VerbTracker : IExposable
{
	[CompilerGenerated]
	private sealed class _003CGetVerbsCommands_003Ed__10 : IEnumerable<Command>, IEnumerable, IEnumerator<Command>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Command _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public VerbTracker _003C_003E4__this;

		private CompEquippable _003Cce_003E5__2;

		private Thing _003CownerThing_003E5__3;

		private List<Verb> _003Cverbs_003E5__4;

		private int _003Ci_003E5__5;

		Command IEnumerator<Command>.Current
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
		public _003CGetVerbsCommands_003Ed__10(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cce_003E5__2 = null;
			_003CownerThing_003E5__3 = null;
			_003Cverbs_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			VerbTracker verbTracker = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				IVerbOwner directOwner = verbTracker.directOwner;
				_003Cce_003E5__2 = directOwner as CompEquippable;
				if (_003Cce_003E5__2 == null)
				{
					return false;
				}
				_003CownerThing_003E5__3 = _003Cce_003E5__2.parent;
				_003Cverbs_003E5__4 = verbTracker.AllVerbs;
				_003Ci_003E5__5 = 0;
				goto IL_00c0;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_00ae;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_00ae:
				_003Ci_003E5__5++;
				goto IL_00c0;
				IL_00c0:
				if (_003Ci_003E5__5 < _003Cverbs_003E5__4.Count)
				{
					Verb verb = _003Cverbs_003E5__4[_003Ci_003E5__5];
					if (verb.verbProps.hasStandardCommand)
					{
						_003C_003E2__current = verbTracker.CreateVerbTargetCommand(_003CownerThing_003E5__3, verb);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00ae;
				}
				if (!verbTracker.directOwner.Tools.NullOrEmpty() && _003Cce_003E5__2 != null && _003Cce_003E5__2.parent.def.IsMeleeWeapon)
				{
					_003C_003E2__current = verbTracker.CreateVerbTargetCommand(_003CownerThing_003E5__3, _003Cverbs_003E5__4.FirstOrDefault((Verb v) => v.verbProps.IsMeleeAttack));
					_003C_003E1__state = 2;
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
		IEnumerator<Command> IEnumerable<Command>.GetEnumerator()
		{
			_003CGetVerbsCommands_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetVerbsCommands_003Ed__10(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Command>)this).GetEnumerator();
		}
	}

	public IVerbOwner directOwner;

	private List<Verb> verbs;

	public List<Verb> AllVerbs
	{
		get
		{
			if (verbs == null)
			{
				InitVerbsFromZero();
			}
			return verbs;
		}
	}

	public Verb PrimaryVerb
	{
		get
		{
			if (verbs == null)
			{
				InitVerbsFromZero();
			}
			for (int i = 0; i < verbs.Count; i++)
			{
				if (verbs[i].verbProps.isPrimary)
				{
					return verbs[i];
				}
			}
			return null;
		}
	}

	public bool AnyVerbBursting
	{
		get
		{
			for (int i = 0; i < verbs.Count; i++)
			{
				if (verbs[i].state == VerbState.Bursting)
				{
					return true;
				}
			}
			return false;
		}
	}

	public VerbTracker(IVerbOwner directOwner)
	{
		this.directOwner = directOwner;
	}

	public void VerbsTick()
	{
		if (verbs != null)
		{
			for (int i = 0; i < verbs.Count; i++)
			{
				verbs[i].VerbTick();
			}
		}
	}

	[IteratorStateMachine(typeof(_003CGetVerbsCommands_003Ed__10))]
	public IEnumerable<Command> GetVerbsCommands()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetVerbsCommands_003Ed__10(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Command_VerbTarget CreateVerbTargetCommand(Thing ownerThing, Verb verb)
	{
		Command_VerbTarget command_VerbTarget = new Command_VerbTarget();
		command_VerbTarget.defaultDesc = ownerThing.LabelCap + ": " + ownerThing.def.description.CapitalizeFirst();
		command_VerbTarget.ownerThing = ownerThing;
		command_VerbTarget.tutorTag = "VerbTarget";
		command_VerbTarget.verb = verb;
		if (verb.caster.Faction != Faction.OfPlayer && !DebugSettings.ShowDevGizmos)
		{
			command_VerbTarget.Disable("CannotOrderNonControlled".Translate());
		}
		else if (verb.CasterIsPawn)
		{
			string reason;
			if (verb.CasterPawn.RaceProps.IsMechanoid && !MechanitorUtility.EverControllable(verb.CasterPawn) && !DebugSettings.ShowDevGizmos)
			{
				command_VerbTarget.Disable("CannotOrderNonControlled".Translate());
			}
			else if (verb.CasterPawn.WorkTagIsDisabled(WorkTags.Violent))
			{
				command_VerbTarget.Disable("IsIncapableOfViolence".Translate(verb.CasterPawn.LabelShort, verb.CasterPawn));
			}
			else if (!verb.CasterPawn.Drafted && !DebugSettings.ShowDevGizmos)
			{
				command_VerbTarget.Disable("IsNotDrafted".Translate(verb.CasterPawn.LabelShort, verb.CasterPawn));
			}
			else if (verb is Verb_LaunchProjectile)
			{
				Apparel apparel = verb.FirstApparelPreventingShooting();
				if (apparel != null)
				{
					command_VerbTarget.Disable("ApparelPreventsShooting".Translate(verb.CasterPawn.Named("PAWN"), apparel.Named("APPAREL")).CapitalizeFirst());
				}
			}
			else if (EquipmentUtility.RolePreventsFromUsing(verb.CasterPawn, verb.EquipmentSource, out reason))
			{
				command_VerbTarget.Disable(reason);
			}
		}
		if ((!verb.EquipmentSource.TryGetComp<CompUniqueWeapon>(out var comp) || !comp.IgnoreAccuracyMaluses) && verb.caster.Spawned && verb.caster.Map.weatherManager.CurWeatherMaxRangeCap >= 0f)
		{
			command_VerbTarget.defaultDescPostfix = "\n\n" + ("WeatherMaxRangeCap".Translate() + ": " + verb.caster.Map.weatherManager.curWeather.LabelCap).Colorize(ColoredText.WarningColor);
		}
		return command_VerbTarget;
	}

	public Verb GetVerb(VerbCategory category)
	{
		List<Verb> allVerbs = AllVerbs;
		if (allVerbs != null)
		{
			for (int i = 0; i < allVerbs.Count; i++)
			{
				if (allVerbs[i].verbProps.category == category)
				{
					return allVerbs[i];
				}
			}
		}
		return null;
	}

	public void ExposeData()
	{
		Scribe_Collections.Look(ref verbs, "verbs", LookMode.Deep);
		if (Scribe.mode != LoadSaveMode.ResolvingCrossRefs || verbs == null)
		{
			return;
		}
		if (verbs.RemoveAll((Verb x) => x == null) != 0)
		{
			Log.Error("Some verbs were null after loading. directOwner=" + directOwner.ToStringSafe());
		}
		List<Verb> sources = verbs;
		verbs = new List<Verb>();
		InitVerbs(delegate(Type type, string id)
		{
			Verb verb = sources.FirstOrDefault((Verb v) => v.loadID == id && v.GetType() == type);
			if (verb == null)
			{
				Log.Warning($"Replaced verb {type}/{id}; may have been changed through a version update or a mod change");
				verb = (Verb)Activator.CreateInstance(type);
			}
			verbs.Add(verb);
			return verb;
		});
	}

	public void InitVerbsFromZero()
	{
		verbs = new List<Verb>();
		InitVerbs(delegate(Type type, string id)
		{
			Verb verb = (Verb)Activator.CreateInstance(type);
			verbs.Add(verb);
			return verb;
		});
	}

	private void InitVerbs(Func<Type, string, Verb> creator)
	{
		List<VerbProperties> verbProperties = directOwner.VerbProperties;
		if (verbProperties != null)
		{
			for (int i = 0; i < verbProperties.Count; i++)
			{
				try
				{
					VerbProperties verbProperties2 = verbProperties[i];
					string text = Verb.CalculateUniqueLoadID(directOwner, i);
					InitVerb(creator(verbProperties2.verbClass, text), verbProperties2, null, null, text);
				}
				catch (Exception ex)
				{
					Log.Error("Could not instantiate Verb (directOwner=" + directOwner.ToStringSafe() + "): " + ex);
				}
			}
		}
		List<Tool> tools = directOwner.Tools;
		if (tools == null)
		{
			return;
		}
		for (int j = 0; j < tools.Count; j++)
		{
			Tool tool = tools[j];
			foreach (ManeuverDef maneuver in tool.Maneuvers)
			{
				try
				{
					VerbProperties verb = maneuver.verb;
					string text2 = Verb.CalculateUniqueLoadID(directOwner, tool, maneuver);
					InitVerb(creator(verb.verbClass, text2), verb, tool, maneuver, text2);
				}
				catch (Exception ex2)
				{
					Log.Error("Could not instantiate Verb (directOwner=" + directOwner.ToStringSafe() + "): " + ex2);
				}
			}
		}
	}

	private void InitVerb(Verb verb, VerbProperties properties, Tool tool, ManeuverDef maneuver, string id)
	{
		verb.loadID = id;
		verb.verbProps = properties;
		verb.verbTracker = this;
		verb.tool = tool;
		verb.maneuver = maneuver;
		verb.caster = directOwner.ConstantCaster;
	}

	public void VerbsNeedReinitOnLoad()
	{
		verbs = null;
	}
}
