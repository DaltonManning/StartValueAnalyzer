using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFSysGenLocalVariable : AppSFVariableBase
{
	public AppSFSysGenLocalVariable(SAXElementBase parent, POUType pouType)
		: base(parent, pouType)
	{
	}

	public override bool HandleAttributes(SAXAttributes attributes)
	{
		base.Variable = null;
		tSysGenVarType tSysGenVarType = tSysGenVarType.vNotASysGenVarType;
		string attributeValue = "";
		string attributeValue2 = "";
		attributes.GetAttribute("Name", ref attributeValue);
		attributes.GetAttribute("Kind", ref attributeValue2);
		switch (attributeValue2)
		{
		case "CompGen":
			tSysGenVarType = tSysGenVarType.vCompGen;
			break;
		case "SeqGen":
			tSysGenVarType = tSysGenVarType.vSeqGen;
			break;
		case "GraphGen":
			tSysGenVarType = tSysGenVarType.vGraphGen;
			break;
		case "DefGen":
			tSysGenVarType = tSysGenVarType.vDefGen;
			break;
		case "LitGen":
			tSysGenVarType = tSysGenVarType.vLitGen;
			break;
		case "ProjGen":
			tSysGenVarType = tSysGenVarType.vProjGen;
			break;
		case "STBatchGen":
			tSysGenVarType = tSysGenVarType.vSTBatchGen;
			break;
		case "FDCompGen":
			tSysGenVarType = tSysGenVarType.vFDCompGen;
			break;
		default:
		{
			string errorMessage = "System generated variable kind = = " + attributeValue2;
			ErrorHandler.ReportError(XMLErrorCode.XMLInvalidAttributeValue, errorMessage);
			return false;
		}
		}
		base.Variable = base.POUType.CreateSystemGeneratedVariable(attributeValue, tSysGenVarType);
		if (base.Variable == null)
		{
			ErrorHandler.ReportError(XMLErrorCode.XMLFailToCreate, "System generated variable (Name: " + attributeValue + ")");
			return false;
		}
		return true;
	}

	protected override DataInst CreateVariable(string name)
	{
		ErrorHandler.ReportError(XMLErrorCode.XMLGeneralError, ElementName);
		return null;
	}
}
