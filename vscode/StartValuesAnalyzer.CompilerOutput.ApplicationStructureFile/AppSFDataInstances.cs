using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public abstract class AppSFDataInstances : SAXElementBase
{
	private readonly POUType pouType;

	protected POUType POUType => pouType;

	public AppSFDataInstances(SAXElementBase parent, POUType pouType)
		: base(parent)
	{
		this.pouType = pouType;
	}

	protected abstract bool CorrectElementName(string elementName);

	protected abstract SAXElementBase CreateChild();

	public override bool StartElement(string elementName, SAXAttributes attributes)
	{
		if (!CorrectElementName(elementName))
		{
			ErrorHandler.ReportStartElementError(ElementName, elementName);
			return false;
		}
		if (base.Child == null)
		{
			base.Child = CreateChild();
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
}
