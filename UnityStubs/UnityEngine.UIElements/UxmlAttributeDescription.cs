namespace UnityEngine.UIElements;

public abstract class UxmlAttributeDescription
{
	public enum Use
	{
		None,
		Optional,
		Prohibited,
		Required
	}

	public string name { get; set; }

	public string type { get; protected set; }

	public string typeNamespace { get; protected set; }

	public abstract string defaultValueAsString { get; }

	public Use use { get; set; }

	public string[] obsoleteNames { get; set; }
}
