using UnityStubs.Internal;

namespace UnityEngine;

public class TouchScreenKeyboard
{
	public enum Status
	{
		Canceled = 2,
		Done = 1,
		LostFocus = 3,
		Visible = 0
	}

	public static bool isSupported
	{
		get
		{
			StubWarner.LogStub("isSupported", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TouchScreenKeyboard.cs", 5);
			return false;
		}
	}

	public Status status
	{
		get
		{
			StubWarner.LogStub("status", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TouchScreenKeyboard.cs", 6);
			return Status.Visible;
		}
	}

	public string text
	{
		get
		{
			StubWarner.LogStub("text", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TouchScreenKeyboard.cs", 7);
			return null;
		}
	}

	public bool active
	{
		set
		{
			StubWarner.LogStub("active", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TouchScreenKeyboard.cs", 9);
		}
	}

	public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType, bool autocorrection, bool multiline, bool secure, bool alert, string textPlaceholder, int characterLimit)
	{
		StubWarner.LogStub("Open", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\TouchScreenKeyboard.cs", 8);
		return null;
	}
}
