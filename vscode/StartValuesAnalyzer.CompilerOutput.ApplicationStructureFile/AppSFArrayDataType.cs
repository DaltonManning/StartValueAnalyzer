using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFArrayDataType : SAXElementBase
{
	private ArrayType arrayType;

	private ArrayComponent arrayComponent;

	public override string ElementName => "Array";

	public ArrayType ArrayType
	{
		set
		{
			arrayType = value;
		}
	}

	public AppSFArrayDataType(SAXElementBase parent, ArrayType arrayType)
		: base(parent)
	{
		this.arrayType = arrayType;
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
			base.Child = new AppSFDataInst(this, arrayComponent);
		}
		else
		{
			((AppSFDataInst)base.Child).DataInst = arrayComponent;
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
		arrayComponent = arrayType.CreateArrayComponent();
		if (arrayComponent == null)
		{
			ErrorHandler.ReportError(XMLErrorCode.XMLFailToCreate, "ArrayComponent");
			return false;
		}
		int attributeValue = 0;
		int attributeValue2 = 0;
		attributes.GetAttribute("LBound", ref attributeValue);
		attributes.GetAttribute("UBound", ref attributeValue2);
		arrayType.FirstIndex = attributeValue;
		arrayType.LastIndex = attributeValue2;
		return true;
	}
}
