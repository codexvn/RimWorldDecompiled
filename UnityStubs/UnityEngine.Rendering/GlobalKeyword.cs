namespace UnityEngine.Rendering;

public struct GlobalKeyword
{
	private readonly string _name;

	internal string Name => _name;

	private GlobalKeyword(string name)
	{
		_name = name ?? string.Empty;
	}

	public static GlobalKeyword Create(string name)
	{
		return new GlobalKeyword(name);
	}
}
