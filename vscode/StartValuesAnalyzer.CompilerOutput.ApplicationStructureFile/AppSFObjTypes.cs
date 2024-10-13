using StartValuesAnalyzer.ModApplic;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public abstract class AppSFObjTypes : SAXElementBase
{
	private readonly SourceCodeUnit sourceCodeUnit;

	public SourceCodeUnit SCU => sourceCodeUnit;

	public AppSFObjTypes(SAXElementBase parent, SourceCodeUnit sourceCodeUnit)
		: base(parent)
	{
		this.sourceCodeUnit = sourceCodeUnit;
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
