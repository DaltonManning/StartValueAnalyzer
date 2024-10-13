using StartValuesAnalyzer.ModApplic;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFDiagramTypes : AppSFPOUTypes
{
	public override string ElementName => "DiagramTypes";

	public AppSFDiagramTypes(SAXElementBase parent, SourceCodeUnit sourceCodeUnit)
		: base(parent, sourceCodeUnit)
	{
	}

	protected override SAXElementBase CreateChild()
	{
		return new AppSFDiagramType(this, base.SCU);
	}
}
