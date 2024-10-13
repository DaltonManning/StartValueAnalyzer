using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFStructDataType : SAXElementBase
{
	private StructType structType;

	private StructComponent structComponent;

	public override string ElementName => "Struct";

	public StructType StructType
	{
		set
		{
			structType = value;
		}
	}

	public AppSFStructDataType(SAXElementBase parent, StructType structType)
		: base(parent)
	{
		this.structType = structType;
	}

	public override bool StartElement(string elementName, SAXAttributes attributes)
	{
		if (!elementName.Equals("DataInst"))
		{
			ErrorHandler.ReportStartElementError(ElementName, elementName);
			return false;
		}
		if (base.Child == null)
		{
			base.Child = new AppSFDataInst(this, structComponent);
		}
		else
		{
			((AppSFDataInst)base.Child).DataInst = structComponent;
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
		attributes.GetAttribute("Name", ref attributeValue);
		structComponent = structType.CreateStructComponent(attributeValue);
		if (structComponent == null)
		{
			ErrorHandler.ReportError(XMLErrorCode.XMLFailToCreate, "StructComponent (Name: " + attributeValue + ")");
			return false;
		}
		return true;
	}
}
