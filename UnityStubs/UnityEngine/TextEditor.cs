using System;
using UnityStubs.Gui;

namespace UnityEngine;

public class TextEditor
{
	private string _text = string.Empty;

	private int _cursorIndex;

	private int _selectIndex;

	public int controlID { get; set; }

	public bool multiline { get; set; }

	public Rect position { get; set; }

	public bool isPasswordField { get; set; }

	public GUIStyle style { get; set; }

	public Vector2 scrollOffset { get; set; }

	public string text
	{
		get
		{
			return _text;
		}
		set
		{
			_text = value ?? string.Empty;
			_cursorIndex = Math.Clamp(_cursorIndex, 0, _text.Length);
			_selectIndex = Math.Clamp(_selectIndex, 0, _text.Length);
		}
	}

	public int cursorIndex
	{
		get
		{
			return _cursorIndex;
		}
		set
		{
			_cursorIndex = Math.Clamp(value, 0, _text.Length);
		}
	}

	public int selectIndex
	{
		get
		{
			return _selectIndex;
		}
		set
		{
			_selectIndex = Math.Clamp(value, 0, _text.Length);
		}
	}

	public bool hasSelection => _cursorIndex != _selectIndex;

	public string SelectedText
	{
		get
		{
			if (!hasSelection)
			{
				return string.Empty;
			}
			int num = Math.Min(_cursorIndex, _selectIndex);
			return _text.Substring(num, Math.Max(_cursorIndex, _selectIndex) - num);
		}
	}

	public void SelectAll()
	{
		_selectIndex = 0;
		_cursorIndex = _text.Length;
	}

	public void SelectNone()
	{
		_selectIndex = _cursorIndex;
	}

	public void Copy()
	{
		if (hasSelection)
		{
			GUIUtility.systemCopyBuffer = SelectedText;
		}
	}

	public void Cut()
	{
		if (hasSelection)
		{
			Copy();
			DeleteSelection();
		}
	}

	public bool Paste()
	{
		string systemCopyBuffer = GUIUtility.systemCopyBuffer;
		if (string.IsNullOrEmpty(systemCopyBuffer))
		{
			return false;
		}
		DeleteSelection();
		_text = _text.Insert(_cursorIndex, systemCopyBuffer);
		_cursorIndex = (_selectIndex = _cursorIndex + systemCopyBuffer.Length);
		return true;
	}

	public bool DeleteSelection()
	{
		if (!hasSelection)
		{
			return false;
		}
		int num = Math.Min(_cursorIndex, _selectIndex);
		_text = _text.Remove(num, Math.Max(_cursorIndex, _selectIndex) - num);
		_cursorIndex = (_selectIndex = num);
		return true;
	}

	public void Insert(char c)
	{
		DeleteSelection();
		_text = _text.Insert(_cursorIndex, c.ToString());
		_cursorIndex = (_selectIndex = _cursorIndex + 1);
	}

	public void MoveTextStart()
	{
		_cursorIndex = (_selectIndex = 0);
	}

	public void MoveTextEnd()
	{
		_cursorIndex = (_selectIndex = _text.Length);
	}

	public void MoveLineStart()
	{
		int num = _text.LastIndexOf('\n', Math.Max(0, _cursorIndex - 1));
		_cursorIndex = (_selectIndex = ((num >= 0) ? (num + 1) : 0));
	}

	public void MoveLineEnd()
	{
		int num = ((_cursorIndex < _text.Length) ? _text.IndexOf('\n', _cursorIndex) : (-1));
		_cursorIndex = (_selectIndex = ((num < 0) ? _text.Length : num));
	}

	public void MoveLeft()
	{
		_cursorIndex = Math.Max(0, _cursorIndex - 1);
		_selectIndex = _cursorIndex;
	}

	public void MoveRight()
	{
		_cursorIndex = Math.Min(_text.Length, _cursorIndex + 1);
		_selectIndex = _cursorIndex;
	}

	public void SelectToPosition(Vector2 cursorPosition)
	{
		_cursorIndex = IndexAtPosition(cursorPosition);
	}

	public void MoveCursorToPosition(Vector2 cursorPosition)
	{
		_cursorIndex = (_selectIndex = IndexAtPosition(cursorPosition));
	}

	private int IndexAtPosition(Vector2 guiPosition)
	{
		IGuiBackend current = GuiBackend.Current;
		if (current == null)
		{
			return _cursorIndex;
		}
		Rect rect = GUI.TextFieldTextRect(position, multiline, style);
		Vector2 vector = guiPosition - rect.position + scrollOffset;
		bool flag = style?.wordWrap ?? multiline;
		return current.TextIndexAt(style?.FontIndex ?? 1, style?.fontSize ?? 0, style?.fontStyle ?? FontStyle.Normal, _text, flag ? rect.width : (-1f), multiline ? vector : new Vector2(vector.x, 0f), style?.richText ?? false);
	}

	public void OnFocus()
	{
		SelectAll();
	}

	public void OnLostFocus()
	{
		SelectNone();
	}
}
