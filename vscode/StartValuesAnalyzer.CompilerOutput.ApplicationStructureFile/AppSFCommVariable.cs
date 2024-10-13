using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFCommVariable : AppSFVariableBase
{
	public AppSFCommVariable(SAXElementBase parent, POUType pouType)
		: base(parent, pouType)
	{
	}

	public override bool HandleAttributes(SAXAttributes attributes)
	{
		string attributeValue = "";
		attributes.GetAttribute("Direction", ref attributeValue);
		tParDirection tParDirection = AppSFParameterBase.StringToParDirection(attributeValue);
		if (tParDirection == tParDirection.cNoOfParDirections)
		{
			string errorMessage = "Communication variable direction = " + attributeValue;
			ErrorHandler.ReportError(XMLErrorCode.XMLInvalidAttributeValue, errorMessage);
			return false;
		}
		string attributeValue2 = "";
		attributes.GetAttribute("Name", ref attributeValue2);
		base.Variable = CreateVariable(attributeValue2);
		if (base.Variable == null)
		{
			ErrorHandler.ReportError(XMLErrorCode.XMLFailToCreate, "CommunicationVariable (Name: " + attributeValue2 + ")");
			return false;
		}
		((CommunicationVariable)base.Variable).Direction = tParDirection;
		return true;
	}

	protected override DataInst CreateVariable(string name)
	{
		return base.POUType.CreateCommunicationVariable(name);
	}
}
