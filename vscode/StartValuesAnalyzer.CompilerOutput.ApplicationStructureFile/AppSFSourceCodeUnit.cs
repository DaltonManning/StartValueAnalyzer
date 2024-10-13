using System;
using StartValuesAnalyzer.ModApplic;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFSourceCodeUnit : AppSFApplicationBase
{
	private SourceCodeUnit sourceCodeUnit;

	public override string ElementName => "SCU";

	public AppSFSourceCodeUnit(SAXElementBase parent, ApplicationUnit application)
		: base(parent, application)
	{
	}

	public override bool StartElement(string elementName, SAXAttributes attributes)
	{
		switch (elementName)
		{
		case "DataTypes":
			base.Child = new AppSFDataTypes(this, sourceCodeUnit);
			break;
		case "ModuleTypes":
			base.Child = new AppSFModuleTypes(this, sourceCodeUnit);
			break;
		case "FBTs":
			base.Child = new AppSFFunctionBlockTypes(this, sourceCodeUnit);
			break;
		case "DiagramTypes":
			base.Child = new AppSFDiagramTypes(this, sourceCodeUnit);
			break;
		case "ProgTypes":
			base.Child = new AppSFProgramTypes(this, sourceCodeUnit);
			break;
		default:
			ErrorHandler.ReportStartElementError(ElementName, elementName);
			return false;
		}
		return true;
	}

	public override bool HandleAttributes(SAXAttributes attributes)
	{
		string attributeValue = "";
		attributes.GetAttribute("Name", ref attributeValue);
		if (!base.Application.Name.Equals(attributeValue, StringComparison.CurrentCultureIgnoreCase))
		{
			sourceCodeUnit = base.Application.CreateLibrary(attributeValue);
			if (sourceCodeUnit == null)
			{
				ErrorHandler.ReportError(XMLErrorCode.XMLFailToCreate, "LibraryUnit (Name:" + attributeValue + ")");
				return false;
			}
		}
		else
		{
			sourceCodeUnit = base.Application;
		}
		return true;
	}
}
