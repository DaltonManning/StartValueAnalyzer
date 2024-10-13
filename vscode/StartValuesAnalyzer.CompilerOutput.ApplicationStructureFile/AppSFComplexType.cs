using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFComplexType : SAXElementBase
{
	private DataInst dataInst;

	public DataInst DataInst
	{
		set
		{
			dataInst = value;
		}
	}

	public override string ElementName => "Complex";

	public AppSFComplexType(SAXElementBase parent, DataInst dataInst)
		: base(parent)
	{
		this.dataInst = dataInst;
	}

	public override bool HandleAttributes(SAXAttributes attributes)
	{
		string attributeValue = "";
		string attributeValue2 = "";
		attributes.GetAttribute("TypeName", ref attributeValue);
		attributes.GetAttribute("SCU", ref attributeValue2);
		dataInst.TypeRefName = attributeValue;
		dataInst.TypeRefSCUName = attributeValue2;
		return true;
	}
}
