using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public abstract class AppSFPOUInstancesBase : SAXElementBase
{
	private readonly POUType pouType;

	private AppSFPOUInstance appSFSingleInst;

	private AppSFPOUInstance appSFReusableInst;

	protected POUType POUType => pouType;

	public AppSFPOUInstancesBase(SAXElementBase parent, POUType pouType)
		: base(parent)
	{
		this.pouType = pouType;
	}

	protected abstract AppSFPOUInstance CreateSingleChild();

	protected abstract AppSFPOUInstance CreateReusableChild();

	public override bool StartElement(string elementName, SAXAttributes attributes)
	{
		if (!elementName.Equals("POUInst"))
		{
			ErrorHandler.ReportStartElementError(ElementName, elementName);
			return false;
		}
		string attributeValue = "";
		attributes.GetAttribute("POUKind", ref attributeValue);
		if (attributeValue.Equals("Single"))
		{
			if (appSFSingleInst == null)
			{
				appSFSingleInst = CreateSingleChild();
			}
			base.Child = appSFSingleInst;
		}
		else
		{
			if (!attributeValue.Equals("Normal"))
			{
				string errorMessage = "Instance kind = " + attributeValue;
				ErrorHandler.ReportError(XMLErrorCode.XMLInvalidAttributeValue, errorMessage);
				return false;
			}
			if (appSFReusableInst == null)
			{
				appSFReusableInst = CreateReusableChild();
			}
			base.Child = appSFReusableInst;
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
