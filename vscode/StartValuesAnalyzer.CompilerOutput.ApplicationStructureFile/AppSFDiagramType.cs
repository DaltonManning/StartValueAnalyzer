using StartValuesAnalyzer.ModApplic;
using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFDiagramType : AppSFReusablePOUType
{
	public AppSFDiagramType(SAXElementBase parent, SourceCodeUnit sourceCodeUnit)
		: base(parent, sourceCodeUnit)
	{
	}

	protected override POUType CreatePOUType(string name)
	{
		return base.SCU.CreateDiagramType(name);
	}
}
