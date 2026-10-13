using System.Collections.Generic;

namespace Waylith.Rendering;

public readonly struct MeshDrawSubmissionSnapshot
{
	public MeshDrawCommand[] Commands { get; }

	public int CommandCount { get; }

	public DrawPropertyArena PropertyArena { get; }

	public List<PersistentDrawCallSource> PersistentSources { get; }

	public MeshDrawSubmissionSnapshot(MeshDrawCommand[] commands, int commandCount, DrawPropertyArena propertyArena, List<PersistentDrawCallSource> persistentSources)
	{
		Commands = commands;
		CommandCount = commandCount;
		PropertyArena = propertyArena;
		PersistentSources = persistentSources;
	}
}
