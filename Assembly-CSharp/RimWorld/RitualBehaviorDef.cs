using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class RitualBehaviorDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__22 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RitualBehaviorDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private List<RitualStage>.Enumerator _003C_003E7__wrap2;

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
		public _003CConfigErrors_003Ed__22(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = default(List<RitualStage>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				RitualBehaviorDef ritualBehaviorDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)ritualBehaviorDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007e;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007e;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00eb;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0144;
				case 4:
					_003C_003E1__state = -1;
					goto IL_017b;
				case 5:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap2 = ritualBehaviorDef.stages.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00eb;
					IL_017b:
					if (!ritualBehaviorDef.roles.NullOrEmpty() && ritualBehaviorDef.roles.Count((RitualRole x) => x.defaultForSelectedColonist) > 1)
					{
						_003C_003E2__current = ">1 default role for selected pawn";
						_003C_003E1__state = 5;
						return true;
					}
					break;
					IL_00eb:
					while (_003C_003E7__wrap2.MoveNext())
					{
						if (_003C_003E7__wrap2.Current.endTriggers.NullOrEmpty())
						{
							_003C_003E2__current = "ritual stage with no endTrigger";
							_003C_003E1__state = 2;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<RitualStage>.Enumerator);
					if (!string.IsNullOrEmpty(ritualBehaviorDef.spectatorsLabel) && string.IsNullOrEmpty(ritualBehaviorDef.spectatorGerund))
					{
						_003C_003E2__current = "ritual has spectatorLabel but no spectatorGerund";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0144;
					IL_0144:
					if (!string.IsNullOrEmpty(ritualBehaviorDef.spectatorGerund) && string.IsNullOrEmpty(ritualBehaviorDef.spectatorsLabel))
					{
						_003C_003E2__current = "ritual has spectatorGerund but no spectatorLabel";
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_017b;
				}
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__22 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__22(0)
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

	[CompilerGenerated]
	private sealed class _003CRequiredRoles_003Ed__24 : IEnumerable<RitualRole>, IEnumerable, IEnumerator<RitualRole>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private RitualRole _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RitualBehaviorDef _003C_003E4__this;

		private int _003Ci_003E5__2;

		RitualRole IEnumerator<RitualRole>.Current
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
		public _003CRequiredRoles_003Ed__24(int _003C_003E1__state)
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
			RitualBehaviorDef ritualBehaviorDef = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0075;
			}
			_003C_003E1__state = -1;
			if (ritualBehaviorDef.roles.NullOrEmpty())
			{
				return false;
			}
			_003Ci_003E5__2 = 0;
			goto IL_0085;
			IL_0085:
			if (_003Ci_003E5__2 < ritualBehaviorDef.roles.Count)
			{
				if (ritualBehaviorDef.roles[_003Ci_003E5__2].required)
				{
					_003C_003E2__current = ritualBehaviorDef.roles[_003Ci_003E5__2];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0075;
			}
			return false;
			IL_0075:
			_003Ci_003E5__2++;
			goto IL_0085;
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
		IEnumerator<RitualRole> IEnumerable<RitualRole>.GetEnumerator()
		{
			_003CRequiredRoles_003Ed__24 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CRequiredRoles_003Ed__24(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<RitualRole>)this).GetEnumerator();
		}
	}

	public Type workerClass = typeof(RitualBehaviorWorker);

	public List<RitualRole> roles;

	public List<RitualStage> stages;

	public IntRange durationTicks = new IntRange(7500, 7500);

	public List<PreceptRequirement> preceptRequirements;

	public List<RitualCancellationTrigger> cancellationTriggers;

	public bool displayCompletedMessage = true;

	[NoTranslate]
	public string useVisualEffectsFromRoleIdIdeo;

	public int displayOrder;

	[MustTranslate]
	public string letterTitle;

	[MustTranslate]
	public string letterText;

	[MustTranslate]
	public string spectatorsLabel;

	[MustTranslate]
	public string spectatorGerund;

	public bool spectatorsIgnoreBleeding;

	public List<SoundDef> soundDefsPerEnhancerCount;

	public int maxEnhancerDistance;

	public RitualSpectatorFilter spectatorFilter;

	public RitualPosition_Lectern FirstLecternPosition
	{
		get
		{
			foreach (RitualStage stage in stages)
			{
				if (stage.roleBehaviors == null)
				{
					continue;
				}
				foreach (RitualRoleBehavior roleBehavior in stage.roleBehaviors)
				{
					if (roleBehavior.CustomPositionsForReading == null)
					{
						continue;
					}
					foreach (RitualPosition item in roleBehavior.CustomPositionsForReading)
					{
						if (item is RitualPosition_Lectern result)
						{
							return result;
						}
					}
				}
			}
			return null;
		}
	}

	public bool UsesLectern => FirstLecternPosition != null;

	public RitualBehaviorWorker GetInstance()
	{
		return (RitualBehaviorWorker)Activator.CreateInstance(workerClass, this);
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__22))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__22(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostLoad()
	{
		base.PostLoad();
		if (!stages.NullOrEmpty())
		{
			for (int i = 0; i < stages.Count; i++)
			{
				stages[i].parent = this;
			}
		}
	}

	[IteratorStateMachine(typeof(_003CRequiredRoles_003Ed__24))]
	public IEnumerable<RitualRole> RequiredRoles()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRequiredRoles_003Ed__24(-2)
		{
			_003C_003E4__this = this
		};
	}
}
