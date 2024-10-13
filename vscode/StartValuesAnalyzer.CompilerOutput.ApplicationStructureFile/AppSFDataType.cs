using StartValuesAnalyzer.AppRuntimeStructure;
using StartValuesAnalyzer.ModApplic;
using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFDataType : SAXElementBase
{
	private readonly SourceCodeUnit sourceCodeUnit;

	private DataType dataType;

	private AppSFStructDataType appSFStructDataType;

	private AppSFArrayDataType appSFArrayDataType;

	public override string ElementName => "DataType";

	public AppSFDataType(SAXElementBase parent, SourceCodeUnit sourceCodeUnit)
		: base(parent)
	{
		this.sourceCodeUnit = sourceCodeUnit;
	}

	public override bool StartElement(string elementName, SAXAttributes attributes)
	{
		switch (elementName)
		{
		case "Struct":
			if (appSFStructDataType == null)
			{
				appSFStructDataType = new AppSFStructDataType(this, (StructType)dataType);
			}
			else
			{
				appSFStructDataType.StructType = (StructType)dataType;
			}
			base.Child = appSFStructDataType;
			break;
		case "Array":
			if (appSFArrayDataType == null)
			{
				appSFArrayDataType = new AppSFArrayDataType(this, (ArrayType)dataType);
			}
			else
			{
				appSFArrayDataType.ArrayType = (ArrayType)dataType;
			}
			base.Child = appSFArrayDataType;
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
		string attributeValue = "";
		string attributeValue2 = "";
		string attributeValue3 = "";
		attributes.GetAttribute("Name", ref attributeValue3);
		attributes.GetAttribute("Type", ref attributeValue2);
		attributes.GetAttribute("Kind", ref attributeValue);
		switch (attributeValue)
		{
		case "Simple":
			dataType = sourceCodeUnit.CreateSimpleDataType(attributeValue3, StringToValType(attributeValue2));
			if (dataType == null)
			{
				ErrorHandler.ReportError(XMLErrorCode.XMLFailToCreate, "SimpleType (Name: " + attributeValue3 + ")");
				return false;
			}
			break;
		case "Struct":
			dataType = sourceCodeUnit.CreateStructDataType(attributeValue3, StringToValType(attributeValue2));
			if (dataType == null)
			{
				ErrorHandler.ReportError(XMLErrorCode.XMLFailToCreate, "StructType (Name: " + attributeValue3 + ")");
				return false;
			}
			break;
		case "Array":
			dataType = sourceCodeUnit.CreateArrayDataType(attributeValue3);
			if (dataType == null)
			{
				ErrorHandler.ReportError(XMLErrorCode.XMLFailToCreate, "ArrayType (Name: " + attributeValue3 + ")");
				return false;
			}
			dataType.ValType = tValType.UndefType;
			break;
		default:
			ErrorHandler.ReportError(XMLErrorCode.XMLInvalidAttributeValue, "DataType (Attribute: " + attributeValue3 + ")");
			return false;
		}
		return true;
	}

	private tValType StringToValType(string type)
	{
		return type switch
		{
			"B" => tValType.BoolType, 
			"DI" => tValType.DIntType, 
			"I" => tValType.IntType, 
			"UI" => tValType.UIntType, 
			"DW" => tValType.DWordType, 
			"W" => tValType.WordType, 
			"R" => tValType.RealType, 
			"S" => tValType.StringType, 
			"D" => tValType.DurationType, 
			"T" => tValType.TimeType, 
			"O" => tValType.ObjectType, 
			"A" => tValType.ArrayObjectType, 
			"Q" => tValType.QueueObjectType, 
			_ => tValType.UndefType, 
		};
	}
}
