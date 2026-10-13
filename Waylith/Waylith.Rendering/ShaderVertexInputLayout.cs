using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Waylith.Graphics.Meshes;

namespace Waylith.Rendering;

public sealed class ShaderVertexInputLayout
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__15 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ShaderVertexInputLayout _003C_003E4__this;

		private HashSet<int> _003CseenLocations_003E5__2;

		private HashSet<VertexAttributeSemantic> _003CseenSemantics_003E5__3;

		private HashSet<ShaderInstanceInputSemantic> _003CseenInstanceSemantics_003E5__4;

		private List<ShaderVertexInputSlot>.Enumerator _003C_003E7__wrap4;

		private ShaderVertexInputSlot _003Cslot_003E5__6;

		private List<ShaderInstanceInputSlot>.Enumerator _003C_003E7__wrap6;

		private ShaderInstanceInputSlot _003Cslot_003E5__8;

		private int _003ClocationCount_003E5__9;

		private int _003Ci_003E5__10;

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
		public _003CConfigErrors_003Ed__15(int _003C_003E1__state)
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
			case 2:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 4:
			case 5:
			case 6:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003CseenLocations_003E5__2 = null;
			_003CseenSemantics_003E5__3 = null;
			_003CseenInstanceSemantics_003E5__4 = null;
			_003C_003E7__wrap4 = default(List<ShaderVertexInputSlot>.Enumerator);
			_003Cslot_003E5__6 = null;
			_003C_003E7__wrap6 = default(List<ShaderInstanceInputSlot>.Enumerator);
			_003Cslot_003E5__8 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ShaderVertexInputLayout shaderVertexInputLayout = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003CseenLocations_003E5__2 = new HashSet<int>();
					_003CseenSemantics_003E5__3 = new HashSet<VertexAttributeSemantic>();
					_003C_003E7__wrap4 = shaderVertexInputLayout.Slots.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_01ea;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0105;
				case 2:
					_003C_003E1__state = -3;
					goto IL_0174;
				case 3:
					_003C_003E1__state = -3;
					goto IL_01e3;
				case 4:
					_003C_003E1__state = -4;
					goto IL_02c9;
				case 5:
					_003C_003E1__state = -4;
					goto IL_0338;
				case 6:
					{
						_003C_003E1__state = -4;
						goto IL_03d5;
					}
					IL_01ea:
					if (_003C_003E7__wrap4.MoveNext())
					{
						_003Cslot_003E5__6 = _003C_003E7__wrap4.Current;
						if (_003Cslot_003E5__6.location < 0)
						{
							_003C_003E2__current = $"Vertex input semantic {_003Cslot_003E5__6.semantic} has invalid location {_003Cslot_003E5__6.location}.";
							_003C_003E1__state = 1;
							return true;
						}
						goto IL_0105;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap4 = default(List<ShaderVertexInputSlot>.Enumerator);
					_003CseenInstanceSemantics_003E5__4 = new HashSet<ShaderInstanceInputSemantic>();
					_003C_003E7__wrap6 = shaderVertexInputLayout.InstanceSlots.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_03ff;
					IL_0105:
					if (!_003CseenLocations_003E5__2.Add(_003Cslot_003E5__6.location))
					{
						_003C_003E2__current = $"Vertex input location {_003Cslot_003E5__6.location} is declared more than once.";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0174;
					IL_03ff:
					if (_003C_003E7__wrap6.MoveNext())
					{
						_003Cslot_003E5__8 = _003C_003E7__wrap6.Current;
						if (_003Cslot_003E5__8.location < 0)
						{
							_003C_003E2__current = $"Instance input semantic {_003Cslot_003E5__8.semantic} has invalid location {_003Cslot_003E5__8.location}.";
							_003C_003E1__state = 4;
							return true;
						}
						goto IL_02c9;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap6 = default(List<ShaderInstanceInputSlot>.Enumerator);
					return false;
					IL_01e3:
					_003Cslot_003E5__6 = null;
					goto IL_01ea;
					IL_0174:
					if (!_003CseenSemantics_003E5__3.Add(_003Cslot_003E5__6.semantic))
					{
						_003C_003E2__current = $"Vertex input semantic {_003Cslot_003E5__6.semantic} is declared more than once.";
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_01e3;
					IL_02c9:
					if (!_003CseenInstanceSemantics_003E5__4.Add(_003Cslot_003E5__8.semantic))
					{
						_003C_003E2__current = $"Instance input semantic {_003Cslot_003E5__8.semantic} is declared more than once.";
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_0338;
					IL_03d5:
					_003Ci_003E5__10++;
					goto IL_03e7;
					IL_0338:
					_003ClocationCount_003E5__9 = ((_003Cslot_003E5__8.semantic != 0) ? 1 : 4);
					_003Ci_003E5__10 = 0;
					goto IL_03e7;
					IL_03e7:
					if (_003Ci_003E5__10 < _003ClocationCount_003E5__9)
					{
						if (!_003CseenLocations_003E5__2.Add(_003Cslot_003E5__8.location + _003Ci_003E5__10))
						{
							_003C_003E2__current = $"Input location {_003Cslot_003E5__8.location + _003Ci_003E5__10} is declared more than once.";
							_003C_003E1__state = 6;
							return true;
						}
						goto IL_03d5;
					}
					_003Cslot_003E5__8 = null;
					goto IL_03ff;
				}
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
			((IDisposable)_003C_003E7__wrap4/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap6/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__15(0)
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

	public List<ShaderVertexInputSlot> slots;

	public List<ShaderInstanceInputSlot> instanceSlots;

	public int fallbackStrideBytes;

	public List<ShaderVertexInputSlot> Slots => slots;

	public List<ShaderInstanceInputSlot> InstanceSlots => instanceSlots;

	public int FallbackStrideBytes => fallbackStrideBytes;

	public bool HasErrors { get; }

	public ShaderVertexInputLayout(params ShaderVertexInputSlot[] slots)
		: this(slots, Array.Empty<ShaderInstanceInputSlot>())
	{
	}

	public ShaderVertexInputLayout(ShaderVertexInputSlot[]? slots, ShaderInstanceInputSlot[]? instanceSlots, bool hasErrors = false)
	{
		this.slots = ((slots != null) ? new List<ShaderVertexInputSlot>(slots) : new List<ShaderVertexInputSlot>());
		this.instanceSlots = ((instanceSlots != null) ? new List<ShaderInstanceInputSlot>(instanceSlots) : new List<ShaderInstanceInputSlot>());
		fallbackStrideBytes = AssignFallbackOffsets(this.slots);
		HasErrors = hasErrors;
	}

	private static int AssignFallbackOffsets(List<ShaderVertexInputSlot> slots)
	{
		int num = 0;
		for (int i = 0; i < slots.Count; i++)
		{
			ShaderVertexInputSlot shaderVertexInputSlot = slots[i];
			if (shaderVertexInputSlot.required)
			{
				shaderVertexInputSlot.defaultOffsetBytes = -1;
				continue;
			}
			shaderVertexInputSlot.defaultOffsetBytes = num;
			num += 16;
		}
		return num;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__15))]
	public IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__15(-2)
		{
			_003C_003E4__this = this
		};
	}
}
