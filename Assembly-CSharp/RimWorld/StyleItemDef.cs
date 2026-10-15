using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public abstract class StyleItemDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__16 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public StyleItemDef _003C_003E4__this;

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
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				StyleItemDef styleItemDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)styleItemDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (styleItemDef.texPath.NullOrEmpty() && !styleItemDef.noGraphic)
					{
						_003C_003E2__current = "no texPath with noGraphic disabled.";
						_003C_003E1__state = 2;
						return true;
					}
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

	[CompilerGenerated]
	private sealed class _003Cget_AllStyleItemDefs_003Ed__14 : IEnumerable<StyleItemDef>, IEnumerable, IEnumerator<StyleItemDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StyleItemDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IEnumerator<HairDef> _003C_003E7__wrap1;

		private IEnumerator<BeardDef> _003C_003E7__wrap2;

		private IEnumerator<TattooDef> _003C_003E7__wrap3;

		StyleItemDef IEnumerator<StyleItemDef>.Current
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
		public _003Cget_AllStyleItemDefs_003Ed__14(int _003C_003E1__state)
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
			case -5:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E7__wrap3 = null;
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
					_003C_003E7__wrap1 = DefDatabase<HairDef>.AllDefs.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006e;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006e;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00cb;
				case 3:
					{
						_003C_003E1__state = -5;
						break;
					}
					IL_006e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						HairDef current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap2 = DefDatabase<BeardDef>.AllDefs.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00cb;
					IL_00cb:
					if (_003C_003E7__wrap2.MoveNext())
					{
						BeardDef current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					_003C_003E7__wrap3 = DefDatabase<TattooDef>.AllDefs.GetEnumerator();
					_003C_003E1__state = -5;
					break;
				}
				if (_003C_003E7__wrap3.MoveNext())
				{
					TattooDef current3 = _003C_003E7__wrap3.Current;
					_003C_003E2__current = current3;
					_003C_003E1__state = 3;
					return true;
				}
				_003C_003Em__Finally3();
				_003C_003E7__wrap3 = null;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<StyleItemDef> IEnumerable<StyleItemDef>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003Cget_AllStyleItemDefs_003Ed__14(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StyleItemDef>)this).GetEnumerator();
		}
	}

	[LoadAlias("hairTags")]
	[NoTranslate]
	public List<string> styleTags = new List<string>();

	[LoadAlias("hairGender")]
	public StyleGender styleGender = StyleGender.Any;

	private StyleItemCategoryDef category;

	public bool noGraphic;

	public ShaderTypeDef overrideShaderTypeDef;

	public GeneDef requiredGene;

	public MutantDef requiredMutant;

	[NoTranslate]
	public string texPath;

	[NoTranslate]
	public string iconPath;

	public virtual Texture2D Icon
	{
		get
		{
			if (noGraphic)
			{
				return BaseContent.ClearTex;
			}
			return ContentFinder<Texture2D>.Get(iconPath ?? (texPath + "_south"));
		}
	}

	public StyleItemCategoryDef StyleItemCategory
	{
		get
		{
			if (category == null)
			{
				return StyleItemCategoryDefOf.Misc;
			}
			return category;
		}
	}

	public static IEnumerable<StyleItemDef> AllStyleItemDefs
	{
		[IteratorStateMachine(typeof(_003Cget_AllStyleItemDefs_003Ed__14))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AllStyleItemDefs_003Ed__14(-2);
		}
	}

	public abstract Graphic GraphicFor(Pawn pawn, Color color);

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
