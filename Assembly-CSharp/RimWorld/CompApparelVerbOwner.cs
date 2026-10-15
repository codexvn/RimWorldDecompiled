using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompApparelVerbOwner : ThingComp, IVerbOwner
{
	[CompilerGenerated]
	private sealed class _003CCompGetWornGizmosExtra_003Ed__24 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompApparelVerbOwner _003C_003E4__this;

		private ThingWithComps _003Cgear_003E5__2;

		private IEnumerator<Gizmo> _003C_003E7__wrap2;

		private List<Verb>.Enumerator _003C_003E7__wrap3;

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
		public _003CCompGetWornGizmosExtra_003Ed__24(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003Cgear_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E7__wrap3 = default(List<Verb>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompApparelVerbOwner compApparelVerbOwner = _003C_003E4__this;
				bool drafted;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap2 = ((ThingComp)compApparelVerbOwner).CompGetWornGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0074;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0074;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_0074:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					drafted = compApparelVerbOwner.Wearer.Drafted;
					if ((drafted && !compApparelVerbOwner.Props.displayGizmoWhileDrafted) || (!drafted && !compApparelVerbOwner.Props.displayGizmoWhileUndrafted))
					{
						return false;
					}
					_003Cgear_003E5__2 = compApparelVerbOwner.parent;
					_003C_003E7__wrap3 = compApparelVerbOwner.VerbTracker.AllVerbs.GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				while (_003C_003E7__wrap3.MoveNext())
				{
					Verb current2 = _003C_003E7__wrap3.Current;
					if (current2.verbProps.hasStandardCommand)
					{
						_003C_003E2__current = compApparelVerbOwner.CreateVerbTargetCommand(_003Cgear_003E5__2, current2);
						_003C_003E1__state = 2;
						return true;
					}
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap3 = default(List<Verb>.Enumerator);
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetWornGizmosExtra_003Ed__24 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetWornGizmosExtra_003Ed__24(0)
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

	private VerbTracker verbTracker;

	public CompProperties_ApparelVerbOwner Props => props as CompProperties_ApparelVerbOwner;

	public Pawn Wearer => (base.ParentHolder as Pawn_ApparelTracker)?.pawn;

	public List<VerbProperties> VerbProperties => parent.def.Verbs;

	public List<Tool> Tools => parent.def.tools;

	public ImplementOwnerTypeDef ImplementOwnerTypeDef => ImplementOwnerTypeDefOf.NativeVerb;

	public Thing ConstantCaster => Wearer;

	public virtual string GizmoExtraLabel => null;

	public VerbTracker VerbTracker
	{
		get
		{
			if (verbTracker == null)
			{
				verbTracker = new VerbTracker(this);
			}
			return verbTracker;
		}
	}

	public List<Verb> AllVerbs => VerbTracker.AllVerbs;

	public string UniqueVerbOwnerID()
	{
		return "CompVerbOwner_" + parent.ThingID;
	}

	public bool VerbsStillUsableBy(Pawn p)
	{
		return Wearer == p;
	}

	public override void PostExposeData()
	{
		Scribe_Deep.Look(ref verbTracker, "verbTracker", this);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			SetupVerbs();
		}
	}

	public override void Initialize(CompProperties props)
	{
		base.Initialize(props);
		SetupVerbs();
	}

	private void SetupVerbs()
	{
		for (int i = 0; i < AllVerbs.Count; i++)
		{
			AllVerbs[i].caster = Wearer;
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetWornGizmosExtra_003Ed__24))]
	public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetWornGizmosExtra_003Ed__24(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Command_VerbTarget CreateVerbTargetCommand(Thing gear, Verb verb)
	{
		Command_VerbOwner command_VerbOwner = new Command_VerbOwner(this);
		command_VerbOwner.defaultDesc = gear.def.description;
		command_VerbOwner.hotKey = Props.hotKey;
		command_VerbOwner.defaultLabel = verb.verbProps.label;
		command_VerbOwner.verb = verb;
		if (verb.verbProps.defaultProjectile != null && verb.verbProps.commandIcon == null)
		{
			command_VerbOwner.icon = verb.verbProps.defaultProjectile.uiIcon;
			command_VerbOwner.iconAngle = verb.verbProps.defaultProjectile.uiIconAngle;
			command_VerbOwner.iconOffset = verb.verbProps.defaultProjectile.uiIconOffset;
			command_VerbOwner.overrideColor = verb.verbProps.defaultProjectile.graphicData.color;
		}
		else
		{
			command_VerbOwner.icon = ((verb.UIIcon != BaseContent.BadTex) ? verb.UIIcon : gear.def.uiIcon);
			command_VerbOwner.iconAngle = gear.def.uiIconAngle;
			command_VerbOwner.iconOffset = gear.def.uiIconOffset;
			command_VerbOwner.defaultIconColor = gear.DrawColor;
		}
		string reason;
		if (!Wearer.IsColonistPlayerControlled)
		{
			command_VerbOwner.Disable("CannotOrderNonControlled".Translate());
		}
		else if (verb.verbProps.violent && Wearer.WorkTagIsDisabled(WorkTags.Violent))
		{
			command_VerbOwner.Disable("IsIncapableOfViolenceLower".Translate(Wearer.LabelShort, Wearer).CapitalizeFirst() + ".");
		}
		else if (!CanBeUsed(out reason))
		{
			command_VerbOwner.Disable(reason);
		}
		return command_VerbOwner;
	}

	public virtual void UsedOnce()
	{
	}

	public virtual bool CanBeUsed(out string reason)
	{
		reason = "";
		if (parent.MapHeld == null)
		{
			return false;
		}
		if (parent.MapHeld.IsPocketMap && VerbProperties.Any((VerbProperties vp) => !vp.useableInPocketMaps))
		{
			reason = "CannotUseReason_PocketMap".Translate(parent.MapHeld.generatorDef.label);
			return false;
		}
		if (parent.MapHeld.Biome.inVacuum && VerbProperties.Any((VerbProperties vp) => !vp.useableInVacuum))
		{
			reason = "CannotFunctionInVacuum".Translate();
			return false;
		}
		return true;
	}
}
