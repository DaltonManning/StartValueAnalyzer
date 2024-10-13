using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public abstract class AppSFPOUType : SAXElementBase
{
	private POUType pouType;

	public override string ElementName => "POUType";

	protected POUType POUType
	{
		get
		{
			return pouType;
		}
		set
		{
			pouType = value;
		}
	}

	public AppSFPOUType(SAXElementBase parent)
		: base(parent)
	{
	}

	protected abstract POUType CreatePOUType(string name);

	public override bool StartElement(string elementName, SAXAttributes attributes)
	{
		switch (elementName)
		{
		case "POUDef":
			base.Child = new SAXIgnoreElement(this);
			break;
		case "Params":
			base.Child = new AppSFParameters(this, pouType);
			break;
		case "ExtVars":
			base.Child = new AppSFExternalVariables(this, pouType);
			break;
		case "GlobVars":
			base.Child = new AppSFGlobalVariables(this, (RootModType)pouType);
			break;
		case "LocVars":
			base.Child = new AppSFLocalVariables(this, pouType);
			break;
		case "CommVars":
			base.Child = new AppSFCommVariables(this, pouType);
			break;
		case "SysGenLocVars":
			base.Child = new AppSFSysGenLocalVariables(this, pouType);
			break;
		case "MultiParams":
			base.Child = new AppSFMultiParameters(this, pouType);
			break;
		case "ProgInsts":
			base.Child = new AppSFProgramInstances(this, (RootModType)pouType);
			break;
		case "DiagramInsts":
			base.Child = new AppSFDiagramInstances(this, pouType);
			break;
		case "ModuleInsts":
			base.Child = new AppSFModuleInstances(this, pouType);
			break;
		case "FBs":
			base.Child = new AppSFFunctionBlocks(this, pouType);
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
		pouType = CreatePOUType(attributeValue);
		return pouType != null;
	}
}
