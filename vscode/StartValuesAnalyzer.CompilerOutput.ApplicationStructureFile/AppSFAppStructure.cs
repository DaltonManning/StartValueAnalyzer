using System.Runtime.InteropServices;
using StartValuesAnalyzer.ModApplic;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFAppStructure : AppSFApplicationBase
{
	public override string ElementName => "ApplicationStructure";

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern int GetACP();

	public AppSFAppStructure(SAXElementBase parent, ApplicationUnit application)
		: base(parent, application)
	{
	}

	public override bool StartElement(string elementName, SAXAttributes attributes)
	{
		switch (elementName)
		{
		case "ProgSystems":
			base.Child = new SAXIgnoreElement(this);
			break;
		case "POUInst":
			base.Child = new AppSFRootModInst(this, base.Application);
			break;
		case "SCU":
			if (base.Child == null || base.Child.GetType() != typeof(AppSFSourceCodeUnit))
			{
				base.Child = new AppSFSourceCodeUnit(this, base.Application);
			}
			break;
		case "CheckSum":
			base.Child = new AppSFCheckSum(this);
			break;
		default:
			ErrorHandler.ReportStartElementError(ElementName, elementName);
			return false;
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
		int attributeAsInt = attributes.GetAttributeAsInt("CodePage");
		int aCP = GetACP();
		int attributeAsInt2 = attributes.GetAttributeAsInt("SyntaxVersion");
		if ((long)attributeAsInt2 < 4L)
		{
			string errorMessage = "Supported version: " + 4L + ". File version: " + attributeAsInt2 + ".";
			ErrorHandler.ReportError(XMLErrorCode.XMLUnsupportedSyntaxVersion, errorMessage);
			return false;
		}
		return true;
	}
}
