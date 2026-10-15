using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Waylith.Rendering;

namespace Verse;

public class ShaderDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__16 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ShaderDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

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
		public _003CConfigErrors_003Ed__16(int _003C_003E1__state)
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
			case 3:
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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ShaderDef shaderDef = _003C_003E4__this;
				ShaderDef complexVersion;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)shaderDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0110;
				case 3:
					{
						_003C_003E1__state = -4;
						goto IL_0160;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					complexVersion = shaderDef.complexVersion;
					if (complexVersion != null && !complexVersion.IsComplex)
					{
						_003C_003E2__current = $"Shader provides complex variant \"{shaderDef.complexVersion.defName}\", but that def is not flagged {"isComplex"}.";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0110;
					IL_0110:
					if (shaderDef.shaderProgram == null)
					{
						break;
					}
					_003C_003E7__wrap1 = shaderDef.shaderProgram.ConfigErrors().GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0160;
					IL_0160:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					break;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__16 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__16(0)
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

	public bool isComplex;

	[NoTranslate]
	public List<string> shaderKeywords;

	public ShaderProgramDefinition shaderProgram;

	public ShaderDef complexVersion;

	public ShaderDef iconVersion;

	[Unsaved(false)]
	private UnityEngine.Shader shaderInt;

	[Unsaved(false)]
	private bool warnedUnregistered;

	public UnityEngine.Shader Shader
	{
		get
		{
			if (shaderInt != null)
			{
				return shaderInt;
			}
			if (ShaderRegistry.TryResolve(defName, out Waylith.Rendering.Shader shader))
			{
				return shaderInt = UnityEngine.Shader.ForDef(defName, shader);
			}
			if (!warnedUnregistered)
			{
				warnedUnregistered = true;
				Log.Warning("Shader def '" + defName + "' is not registered (no engine program ported yet); using engine default.");
			}
			return UnityEngine.Shader.FromEngineShader(defName, null);
		}
	}

	public bool IsComplex => isComplex;

	public bool HasComplexVersion => complexVersion != null;

	public ShaderDef ComplexVersion => complexVersion;

	public static implicit operator UnityEngine.Shader(ShaderDef def)
	{
		return def?.Shader;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__16))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__16(-2)
		{
			_003C_003E4__this = this
		};
	}
}
