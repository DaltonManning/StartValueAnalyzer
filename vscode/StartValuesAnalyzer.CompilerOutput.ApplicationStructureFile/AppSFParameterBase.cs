using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public abstract class AppSFParameterBase : SAXElementBase
{
	private readonly POUType pouType;

	private Parameter parameter;

	public override string ElementName => "Param";

	protected POUType POUType => pouType;

	protected Parameter Parameter
	{
		get
		{
			return parameter;
		}
		set
		{
			parameter = value;
		}
	}

	public AppSFParameterBase(SAXElementBase parent, POUType pouType)
		: base(parent)
	{
		this.pouType = pouType;
	}

	protected abstract Parameter CreateParameter(string name);

	public override bool StartElement(string elementName, SAXAttributes attributes)
	{
		if (!elementName.Equals("DataInst"))
		{
			ErrorHandler.ReportStartElementError(ElementName, elementName);
			return false;
		}
		if (base.Child == null)
		{
			base.Child = new AppSFDataInst(this, parameter);
		}
		else
		{
			((AppSFDataInst)base.Child).DataInst = parameter;
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
		string attributeValue = "";
		attributes.GetAttribute("Direction", ref attributeValue);
		tParDirection tParDirection = StringToParDirection(attributeValue);
		if (tParDirection == tParDirection.cNoOfParDirections)
		{
			string errorMessage = "Parameter direction = " + attributeValue;
			ErrorHandler.ReportError(XMLErrorCode.XMLInvalidAttributeValue, errorMessage);
			return false;
		}
		string attributeValue2 = "";
		tParTransfer tParTransfer = tParTransfer.cNoOfParTransfers;
		attributes.GetAttribute("TransMethod", ref attributeValue2);
		switch (attributeValue2)
		{
		case "DynRef":
			tParTransfer = tParTransfer.vDynamicRefPar;
			break;
		case "StatRef":
			tParTransfer = tParTransfer.vStaticRefPar;
			break;
		case "Value":
			tParTransfer = tParTransfer.vValuePar;
			break;
		default:
		{
			string errorMessage2 = "Parameter transfer method = " + attributeValue2;
			ErrorHandler.ReportError(XMLErrorCode.XMLInvalidAttributeValue, errorMessage2);
			return false;
		}
		}
		string attributeValue3 = "";
		attributes.GetAttribute("Name", ref attributeValue3);
		parameter = CreateParameter(attributeValue3);
		if (parameter == null)
		{
			ErrorHandler.ReportError(XMLErrorCode.XMLFailToCreate, "Parameter (Name: " + attributeValue3 + ")");
			return false;
		}
		parameter.ParDirection = tParDirection;
		parameter.ParTransfer = tParTransfer;
		return true;
	}

	public static tParDirection StringToParDirection(string direction)
	{
		tParDirection result = tParDirection.cNoOfParDirections;
		switch (direction)
		{
		case "In":
			result = tParDirection.vInPar;
			break;
		case "Out":
			result = tParDirection.vOutPar;
			break;
		case "InOut":
			result = tParDirection.vInOutPar;
			break;
		case "Ext":
			result = tParDirection.vExternalPar;
			break;
		}
		return result;
	}
}
