using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFSimpleType : SAXElementBase
{
	private DataInst dataInst;

	public DataInst DataInst
	{
		set
		{
			dataInst = value;
		}
	}

	public override string ElementName => "SimpleType";

	public AppSFSimpleType(SAXElementBase parent, DataInst dataInst)
		: base(parent)
	{
		this.dataInst = dataInst;
	}

	public override bool HandleAttributes(SAXAttributes attributes)
	{
		string attributeValue = "";
		string type = "";
		attributes.GetAttribute("Type", ref attributeValue);
		GetTypeString(attributeValue, ref type);
		dataInst.TypeRefName = type;
		return true;
	}

	private void GetTypeString(string xmlType, ref string type)
	{
		switch (xmlType)
		{
		case "B":
			type = "bool";
			break;
		case "DI":
			type = "dint";
			break;
		case "R":
			type = "real";
			break;
		case "W":
			type = "word";
			break;
		case "I":
			type = "int";
			break;
		case "UI":
			type = "uint";
			break;
		case "DW":
			type = "dword";
			break;
		case "S":
			type = "string";
			break;
		case "T":
			type = "date_and_time";
			break;
		case "D":
			type = "time";
			break;
		case "O":
			type = "tObject";
			break;
		case "A":
			type = "ArrayObject";
			break;
		case "Q":
			type = "QueueObject";
			break;
		case "An":
			type = "AnyType";
			break;
		default:
			type = "";
			break;
		}
	}
}
