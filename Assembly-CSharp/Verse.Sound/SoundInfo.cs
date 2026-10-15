using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Verse.Sound;

public struct SoundInfo
{
	[CompilerGenerated]
	private sealed class _003Cget_DefinedParameters_003Ed__18 : IEnumerable<KeyValuePair<string, float>>, IEnumerable, IEnumerator<KeyValuePair<string, float>>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private KeyValuePair<string, float> _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public SoundInfo _003C_003E4__this;

		public SoundInfo _003C_003E3___003C_003E4__this;

		private Dictionary<string, float>.Enumerator _003C_003E7__wrap1;

		KeyValuePair<string, float> IEnumerator<KeyValuePair<string, float>>.Current
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
		public _003Cget_DefinedParameters_003Ed__18(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(Dictionary<string, float>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (_003C_003E4__this.parameters == null)
					{
						return false;
					}
					_003C_003E7__wrap1 = _003C_003E4__this.parameters.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					KeyValuePair<string, float> current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(Dictionary<string, float>.Enumerator);
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
		IEnumerator<KeyValuePair<string, float>> IEnumerable<KeyValuePair<string, float>>.GetEnumerator()
		{
			_003Cget_DefinedParameters_003Ed__18 _003Cget_DefinedParameters_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003Cget_DefinedParameters_003Ed__ = this;
			}
			else
			{
				_003Cget_DefinedParameters_003Ed__ = new _003Cget_DefinedParameters_003Ed__18(0);
			}
			_003Cget_DefinedParameters_003Ed__._003C_003E4__this = _003C_003E3___003C_003E4__this;
			return _003Cget_DefinedParameters_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<string, float>>)this).GetEnumerator();
		}
	}

	private Dictionary<string, float> parameters;

	public float volumeFactor;

	public float pitchFactor;

	public bool testPlay;

	public bool forcedPlayOnCamera;

	public bool IsOnCamera { get; private set; }

	public TargetInfo Maker { get; private set; }

	public MaintenanceType Maintenance { get; private set; }

	public IEnumerable<KeyValuePair<string, float>> DefinedParameters
	{
		[IteratorStateMachine(typeof(_003Cget_DefinedParameters_003Ed__18))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_DefinedParameters_003Ed__18(-2)
			{
				_003C_003E3___003C_003E4__this = this
			};
		}
	}

	public static SoundInfo OnCamera(MaintenanceType maint = MaintenanceType.None)
	{
		SoundInfo result = default(SoundInfo);
		result.IsOnCamera = true;
		result.Maintenance = maint;
		result.Maker = TargetInfo.Invalid;
		result.testPlay = false;
		result.volumeFactor = (result.pitchFactor = 1f);
		return result;
	}

	public static SoundInfo InMap(TargetInfo maker, MaintenanceType maint = MaintenanceType.None)
	{
		SoundInfo result = default(SoundInfo);
		result.IsOnCamera = false;
		result.Maintenance = maint;
		result.Maker = maker;
		result.testPlay = false;
		result.volumeFactor = (result.pitchFactor = 1f);
		return result;
	}

	public void SetParameter(string key, float value)
	{
		if (parameters == null)
		{
			parameters = new Dictionary<string, float>();
		}
		parameters[key] = value;
	}

	public static implicit operator SoundInfo(TargetInfo source)
	{
		return InMap(source);
	}

	public static implicit operator SoundInfo(Thing sourceThing)
	{
		return InMap(sourceThing);
	}

	public override string ToString()
	{
		string text = null;
		if (parameters != null && parameters.Count > 0)
		{
			text = "parameters=";
			foreach (KeyValuePair<string, float> parameter in parameters)
			{
				text = text + parameter.Key.ToString() + "-" + parameter.Value + " ";
			}
		}
		string text2 = null;
		if (Maker.HasThing || Maker.Cell.IsValid)
		{
			text2 = Maker.ToString();
		}
		string text3 = null;
		if (Maintenance != 0)
		{
			text3 = ", Maint=" + Maintenance;
		}
		return "(" + (IsOnCamera ? "Camera" : "World from ") + text2 + text + text3 + ")";
	}
}
