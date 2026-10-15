using System;

namespace Verse.ProfilerUI;

public sealed class ProfilerColumn
{
	public ProfilerColumnKind kind;

	public float width;

	public Func<string> header;

	public Action sort;

	public ProfilerColumn(ProfilerColumnKind kind, float width, Func<string> header, Action sort = null)
	{
		this.kind = kind;
		this.width = width;
		this.header = header;
		this.sort = sort;
	}
}
