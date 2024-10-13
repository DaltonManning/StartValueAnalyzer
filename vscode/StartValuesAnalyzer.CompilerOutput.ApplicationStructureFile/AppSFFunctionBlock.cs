using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFFunctionBlock : AppSFReusablePOUInst
{
	public AppSFFunctionBlock(SAXElementBase parent, POUType pouType)
		: base(parent, pouType)
	{
	}

	protected override POUInst CreatePOUInst(string name)
	{
		return pouType.CreateFunctionBlock(name);
	}
}
