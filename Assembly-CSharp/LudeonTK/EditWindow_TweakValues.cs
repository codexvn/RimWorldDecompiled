using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace LudeonTK;

public class EditWindow_TweakValues : EditWindow
{
	private struct TweakInfo
	{
		public FieldInfo field;

		public TweakValue tweakValue;

		public float initial;
	}

	[CompilerGenerated]
	private sealed class _003CFindAllTweakables_003Ed__11 : IEnumerable<FieldInfo>, IEnumerable, IEnumerator<FieldInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FieldInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private List<Type>.Enumerator _003C_003E7__wrap1;

		private FieldInfo[] _003C_003E7__wrap2;

		private int _003C_003E7__wrap3;

		FieldInfo IEnumerator<FieldInfo>.Current
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
		public _003CFindAllTweakables_003Ed__11(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<Type>.Enumerator);
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				if (num != 0)
				{
					if (num != 1)
					{
						return false;
					}
					_003C_003E1__state = -3;
					goto IL_0113;
				}
				_003C_003E1__state = -1;
				_003C_003E7__wrap1 = GenTypes.AllTypes.GetEnumerator();
				_003C_003E1__state = -3;
				goto IL_013b;
				IL_0121:
				if (_003C_003E7__wrap3 < _003C_003E7__wrap2.Length)
				{
					FieldInfo fieldInfo = _003C_003E7__wrap2[_003C_003E7__wrap3];
					if (fieldInfo.TryGetAttribute<TweakValue>() != null)
					{
						if (!fieldInfo.IsStatic)
						{
							Log.Error($"Field {fieldInfo.DeclaringType.FullName}.{fieldInfo.Name} is marked with TweakValue, but isn't static; TweakValue won't work");
						}
						else if (fieldInfo.IsLiteral)
						{
							Log.Error($"Field {fieldInfo.DeclaringType.FullName}.{fieldInfo.Name} is marked with TweakValue, but is const; TweakValue won't work");
						}
						else
						{
							if (!fieldInfo.IsInitOnly)
							{
								_003C_003E2__current = fieldInfo;
								_003C_003E1__state = 1;
								return true;
							}
							Log.Error($"Field {fieldInfo.DeclaringType.FullName}.{fieldInfo.Name} is marked with TweakValue, but is readonly; TweakValue won't work");
						}
					}
					goto IL_0113;
				}
				_003C_003E7__wrap2 = null;
				goto IL_013b;
				IL_0113:
				_003C_003E7__wrap3++;
				goto IL_0121;
				IL_013b:
				if (_003C_003E7__wrap1.MoveNext())
				{
					Type current = _003C_003E7__wrap1.Current;
					_003C_003E7__wrap2 = current.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					_003C_003E7__wrap3 = 0;
					goto IL_0121;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Type>.Enumerator);
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
		IEnumerator<FieldInfo> IEnumerable<FieldInfo>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CFindAllTweakables_003Ed__11(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FieldInfo>)this).GetEnumerator();
		}
	}

	[TweakValue("TweakValue", 0f, 300f)]
	public static float CategoryWidth = 180f;

	[TweakValue("TweakValue", 0f, 300f)]
	public static float TitleWidth = 300f;

	[TweakValue("TweakValue", 0f, 300f)]
	public static float NumberWidth = 140f;

	private Vector2 scrollPosition;

	private static List<TweakInfo> tweakValueFields;

	public override Vector2 InitialSize => new Vector2(1000f, 600f);

	public override bool IsDebug => true;

	public EditWindow_TweakValues()
	{
		optionalTitle = "TweakValues";
		if (tweakValueFields == null)
		{
			tweakValueFields = (from ti in FindAllTweakables().Select(delegate(FieldInfo field)
				{
					TweakInfo result = default(TweakInfo);
					result.field = field;
					result.tweakValue = field.TryGetAttribute<TweakValue>();
					result.initial = GetAsFloat(field);
					return result;
				})
				orderby $"{ti.tweakValue.category}.{ti.field.DeclaringType.Name}"
				select ti).ToList();
		}
	}

	[IteratorStateMachine(typeof(_003CFindAllTweakables_003Ed__11))]
	private IEnumerable<FieldInfo> FindAllTweakables()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CFindAllTweakables_003Ed__11(-2);
	}

	public override void DoWindowContents(Rect inRect)
	{
		Text.Font = GameFont.Small;
		Rect rect;
		Rect outRect = (rect = inRect.ContractedBy(4f));
		rect.xMax -= 33f;
		Rect rect2 = new Rect(0f, 0f, CategoryWidth, Text.CalcHeight("test", 1000f));
		Rect rect3 = new Rect(rect2.xMax, 0f, TitleWidth, rect2.height);
		Rect rect4 = new Rect(rect3.xMax, 0f, NumberWidth, rect2.height);
		Rect rect5 = new Rect(rect4.xMax, 0f, rect.width - rect4.xMax, rect2.height);
		DevGUI.BeginScrollView(outRect, ref scrollPosition, new Rect(0f, 0f, rect.width, rect2.height * (float)tweakValueFields.Count));
		foreach (TweakInfo tweakValueField in tweakValueFields)
		{
			DevGUI.Label(rect2, tweakValueField.tweakValue.category);
			DevGUI.Label(rect3, $"{tweakValueField.field.DeclaringType.Name}.{tweakValueField.field.Name}");
			float num;
			bool flag;
			if (tweakValueField.field.FieldType == typeof(float) || tweakValueField.field.FieldType == typeof(int) || tweakValueField.field.FieldType == typeof(ushort))
			{
				float asFloat = GetAsFloat(tweakValueField.field);
				num = DevGUI.HorizontalSlider(rect5, GetAsFloat(tweakValueField.field), tweakValueField.tweakValue.min, tweakValueField.tweakValue.max);
				SetFromFloat(tweakValueField.field, num);
				flag = asFloat != num;
			}
			else if (tweakValueField.field.FieldType == typeof(bool))
			{
				bool num2 = (bool)tweakValueField.field.GetValue(null);
				bool checkOn = num2;
				DevGUI.Checkbox(new Rect(rect5.xMin, rect5.yMin, rect2.height, rect2.height), ref checkOn);
				tweakValueField.field.SetValue(null, checkOn);
				num = (checkOn ? 1 : 0);
				flag = num2 != checkOn;
			}
			else
			{
				Log.ErrorOnce($"Attempted to tweakvalue unknown field type {tweakValueField.field.FieldType}", 83944645);
				flag = false;
				num = tweakValueField.initial;
			}
			if (num != tweakValueField.initial)
			{
				GUI.color = Color.red;
				Text.WordWrap = false;
				DevGUI.Label(rect4, $"{tweakValueField.initial} -> {num}");
				Text.WordWrap = true;
				GUI.color = Color.white;
				if (DevGUI.ButtonInvisible(rect4))
				{
					flag = true;
					if (tweakValueField.field.FieldType == typeof(float) || tweakValueField.field.FieldType == typeof(int) || tweakValueField.field.FieldType == typeof(ushort))
					{
						SetFromFloat(tweakValueField.field, tweakValueField.initial);
					}
					else if (tweakValueField.field.FieldType == typeof(bool))
					{
						tweakValueField.field.SetValue(null, tweakValueField.initial != 0f);
					}
					else
					{
						Log.ErrorOnce($"Attempted to tweakvalue unknown field type {tweakValueField.field.FieldType}", 83944646);
					}
				}
			}
			else
			{
				DevGUI.Label(rect4, $"{tweakValueField.initial}");
			}
			if (flag)
			{
				MethodInfo method = tweakValueField.field.DeclaringType.GetMethod(tweakValueField.field.Name + "_Changed", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				if (method != null)
				{
					method.Invoke(null, null);
				}
			}
			rect2.y += rect2.height;
			rect3.y += rect2.height;
			rect4.y += rect2.height;
			rect5.y += rect2.height;
		}
		DevGUI.EndScrollView();
	}

	private float GetAsFloat(FieldInfo field)
	{
		if (field.FieldType == typeof(float))
		{
			return (float)field.GetValue(null);
		}
		if (field.FieldType == typeof(bool))
		{
			return ((bool)field.GetValue(null)) ? 1 : 0;
		}
		if (field.FieldType == typeof(int))
		{
			return (int)field.GetValue(null);
		}
		if (field.FieldType == typeof(ushort))
		{
			return (int)(ushort)field.GetValue(null);
		}
		Log.ErrorOnce($"Attempted to return unknown field type {field.FieldType} as a float", 83944644);
		return 0f;
	}

	private void SetFromFloat(FieldInfo field, float input)
	{
		if (field.FieldType == typeof(float))
		{
			field.SetValue(null, input);
		}
		else if (field.FieldType == typeof(bool))
		{
			field.SetValue(null, input != 0f);
		}
		else if (field.FieldType == typeof(int))
		{
			field.SetValue(field, (int)input);
		}
		else if (field.FieldType == typeof(ushort))
		{
			field.SetValue(field, (ushort)input);
		}
		else
		{
			Log.ErrorOnce($"Attempted to set unknown field type {field.FieldType} from a float", 83944645);
		}
	}
}
