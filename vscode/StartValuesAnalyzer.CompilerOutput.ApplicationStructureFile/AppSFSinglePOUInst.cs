using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public abstract class AppSFSinglePOUInst : AppSFPOUInstance
{
	private int subTreeSize;

	protected int SubTreeSize => subTreeSize;

	public AppSFSinglePOUInst(SAXElementBase parent)
		: base(parent)
	{
	}

	protected abstract AppSFSinglePOUType CreateChild();

	protected abstract void UpdateChild();

	public override bool StartElement(string elementName, SAXAttributes attributes)
	{
		if (!elementName.Equals("POUType"))
		{
			ErrorHandler.ReportStartElementError(ElementName, elementName);
			return false;
		}
		if (base.Child == null)
		{
			base.Child = CreateChild();
		}
		else
		{
			UpdateChild();
		}
		if (!base.Child.HandleAttributes(attributes))
		{
			if (!ErrorHandler.ErrorSet)
			{
				ErrorHandler.ReportAttributeError(elementName, attributes);
			}
			return false;
		}
		return true;
	}

	public override bool HandleAttributes(SAXAttributes attributes)
	{
		if (!base.HandleAttributes(attributes))
		{
			return false;
		}
		attributes.GetAttribute("SubTreeSize", ref subTreeSize);
		return true;
	}
}
