using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFDataInst : SAXElementBase
{
	private DataInst dataInst;

	private AppSFSimpleType appSFSimpleType;

	private AppSFComplexType appSFComplexType;

	public override string ElementName => "DataInst";

	public DataInst DataInst
	{
		set
		{
			dataInst = value;
		}
	}

	public AppSFDataInst(SAXElementBase parent, DataInst dataInst)
		: base(parent)
	{
		this.dataInst = dataInst;
	}

	public override bool StartElement(string elementName, SAXAttributes attributes)
	{
		switch (elementName)
		{
		case "SimpleType":
			if (appSFSimpleType == null)
			{
				appSFSimpleType = new AppSFSimpleType(this, dataInst);
			}
			else
			{
				appSFSimpleType.DataInst = dataInst;
			}
			base.Child = appSFSimpleType;
			break;
		case "Complex":
			if (appSFComplexType == null)
			{
				appSFComplexType = new AppSFComplexType(this, dataInst);
			}
			else
			{
				appSFComplexType.DataInst = dataInst;
			}
			base.Child = appSFComplexType;
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
		bool attributeValue = false;
		bool attributeValue2 = false;
		bool attributeValue3 = false;
		bool attributeValue4 = false;
		bool attributeValue5 = false;
		bool attributeValue6 = false;
		bool attributeValue7 = false;
		bool attributeValue8 = false;
		bool attributeValue9 = false;
		int attributeValue10 = 0;
		attributes.GetAttribute("Hidden", ref attributeValue);
		attributes.GetAttribute("Const", ref attributeValue2);
		attributes.GetAttribute("State", ref attributeValue3);
		attributes.GetAttribute("Retain", ref attributeValue4);
		attributes.GetAttribute("ColdRetain", ref attributeValue5);
		attributes.GetAttribute("NoSort", ref attributeValue6);
		attributes.GetAttribute("Volatile", ref attributeValue7);
		attributes.GetAttribute("ReadOnly", ref attributeValue8);
		attributes.GetAttribute("NoUserAccess", ref attributeValue9);
		attributes.GetAttribute("VarOffset", ref attributeValue10);
		dataInst.Hidden = attributeValue;
		dataInst.Constant = attributeValue2;
		dataInst.State = attributeValue3;
		dataInst.Retain = attributeValue4;
		dataInst.ColdRetain = attributeValue5;
		dataInst.NoSort = attributeValue6;
		dataInst.Volatile = attributeValue7;
		dataInst.ReadOnly = attributeValue8;
		dataInst.NoUserAccess = attributeValue9;
		dataInst.VarOffset = (ushort)attributeValue10;
		return true;
	}
}
