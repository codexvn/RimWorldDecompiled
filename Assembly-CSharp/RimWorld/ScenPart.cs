using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public abstract class ScenPart : IExposable
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__36 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ScenPart _003C_003E4__this;

		string IEnumerator<string>.Current
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
		public _003CConfigErrors_003Ed__36(int _003C_003E1__state)
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
			ScenPart scenPart = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (scenPart.def == null)
				{
					_003C_003E2__current = scenPart.GetType()?.ToString() + " has null def.";
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__36 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__36(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[TranslationHandle]
	public ScenPartDef def;

	public bool visible = true;

	public bool summarized;

	public static float RowHeight => Text.LineHeight;

	public virtual string Label => def.LabelCap;

	public virtual bool OverrideDangerMusic => false;

	public virtual bool Valid()
	{
		return def != null;
	}

	public virtual void ExposeData()
	{
		Scribe_Defs.Look(ref def, "def");
	}

	public ScenPart CopyForEditing()
	{
		ScenPart scenPart = CopyForEditingInner();
		scenPart.def = def;
		return scenPart;
	}

	protected virtual ScenPart CopyForEditingInner()
	{
		return (ScenPart)MemberwiseClone();
	}

	public virtual void DoEditInterface(Listing_ScenEdit listing)
	{
		listing.GetScenPartRect(this, RowHeight);
	}

	public virtual string Summary(Scenario scen)
	{
		return def.description;
	}

	public virtual IEnumerable<string> GetSummaryListEntries(string tag)
	{
		return Enumerable.Empty<string>();
	}

	public virtual void Randomize()
	{
	}

	public virtual bool TryMerge(ScenPart other)
	{
		return false;
	}

	public virtual bool CanCoexistWith(ScenPart other)
	{
		return true;
	}

	public virtual IEnumerable<Page> GetConfigPages()
	{
		return Enumerable.Empty<Page>();
	}

	public virtual bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
	{
		return true;
	}

	public virtual void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
	{
	}

	public virtual void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
	{
	}

	public virtual void Notify_PawnDied(Corpse corpse)
	{
	}

	public virtual void PreConfigure()
	{
	}

	public virtual void PostWorldGenerate()
	{
	}

	public virtual void PostIdeoChosen()
	{
	}

	public virtual void PreMapGenerate()
	{
	}

	public virtual IEnumerable<Thing> PlayerStartingThings()
	{
		return Enumerable.Empty<Thing>();
	}

	public virtual void GenerateIntoMap(Map map)
	{
	}

	public virtual void PostMapGenerate(Map map)
	{
	}

	public virtual void PostGravshipLanded(Map map)
	{
	}

	public virtual void MapRemoved(Map map)
	{
	}

	public virtual void PostGameStart()
	{
	}

	public virtual void Tick()
	{
	}

	public virtual IEnumerable<Alert> GetAlerts()
	{
		return Enumerable.Empty<Alert>();
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__36))]
	public virtual IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__36(-2)
		{
			_003C_003E4__this = this
		};
	}

	public virtual bool HasNullDefs()
	{
		return def == null;
	}

	public override int GetHashCode()
	{
		if (def == null)
		{
			return 0;
		}
		return def.GetHashCode();
	}
}
