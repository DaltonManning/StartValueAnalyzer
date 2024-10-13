using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFReusableDiagramInst : AppSFReusablePOUInst
{
	public AppSFReusableDiagramInst(SAXElementBase parent, POUType pouType)
		: base(parent, pouType)
	{
	}

	protected override POUInst CreatePOUInst(string name)
	{
		return pouType.CreateDiagram(name);
	}
}
