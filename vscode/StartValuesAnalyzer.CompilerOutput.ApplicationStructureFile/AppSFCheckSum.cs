using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFCheckSum : SAXElementBase
{
	public override string ElementName => "CheckSum";

	public AppSFCheckSum(SAXElementBase parent)
		: base(parent)
	{
	}

	public override bool HandleAttributes(SAXAttributes attributes)
	{
		string attributeValue = "";
		attributes.GetAttribute("Value", ref attributeValue);
		return true;
	}
}
