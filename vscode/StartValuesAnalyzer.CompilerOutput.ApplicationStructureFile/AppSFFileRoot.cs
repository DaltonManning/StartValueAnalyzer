using StartValuesAnalyzer.ModApplic;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFFileRoot : AppSFApplicationBase
{
	private XMLError appSFError;

	public override string ElementName => "ApplicationStructureXMLRoot";

	public override XMLError ErrorHandler => appSFError;

	public AppSFFileRoot(ApplicationUnit application)
		: base(application)
	{
		appSFError = new XMLError();
	}

	public override bool StartElement(string elementName, SAXAttributes attributes)
	{
		if (!elementName.Equals("ApplicationStructure"))
		{
			ErrorHandler.ReportStartElementError(ElementName, elementName);
			return false;
		}
		AppSFAppStructure appSFAppStructure = (AppSFAppStructure)(base.Child = new AppSFAppStructure(this, base.Application));
		if (!appSFAppStructure.HandleAttributes(attributes))
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
