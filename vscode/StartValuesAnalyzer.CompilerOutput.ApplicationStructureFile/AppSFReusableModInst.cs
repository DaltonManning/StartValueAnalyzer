using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFReusableModInst : AppSFReusablePOUInst
{
	public AppSFReusableModInst(SAXElementBase parent, POUType pouType)
		: base(parent, pouType)
	{
	}

	protected override POUInst CreatePOUInst(string name)
	{
		return pouType.CreateControlModule(name);
	}
}
