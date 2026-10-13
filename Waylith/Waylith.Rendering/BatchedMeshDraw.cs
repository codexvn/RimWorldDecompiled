namespace Waylith.Rendering;

public readonly struct BatchedMeshDraw
{
	public QueuedMeshDraw Draw { get; }

	public DrawPropertyRange LocalProperties { get; }

	public long SubmissionOrder { get; }

	public BatchedMeshDraw(in QueuedMeshDraw draw, DrawPropertyRange localProperties, long submissionOrder)
	{
		Draw = draw;
		LocalProperties = localProperties;
		SubmissionOrder = submissionOrder;
	}
}
