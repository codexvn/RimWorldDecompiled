namespace Waylith.Rendering;

public sealed record MaterialUniformBlockSlot(int PropertyId, int Size, MaterialUniformSlot[] Members, MaterialBlockArraySlot[] Arrays);
