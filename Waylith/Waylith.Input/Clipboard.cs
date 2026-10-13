using Brutal.GlfwApi;

namespace Waylith.Input;

public static class Clipboard
{
	public static string Text
	{
		get
		{
			return Glfw.Clipboard ?? string.Empty;
		}
		set
		{
			Glfw.Clipboard = value ?? string.Empty;
		}
	}
}
