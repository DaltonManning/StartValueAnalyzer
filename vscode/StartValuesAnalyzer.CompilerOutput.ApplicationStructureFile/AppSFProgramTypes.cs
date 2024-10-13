using StartValuesAnalyzer.ModApplic;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFProgramTypes : AppSFPOUTypes
{
	public override string ElementName => "ProgTypes";

	public AppSFProgramTypes(SAXElementBase parent, SourceCodeUnit sourceCodeUnit)
		: base(parent, sourceCodeUnit)
	{
	}

	protected override SAXElementBase CreateChild()
	{
		return new AppSFProgramType(this, base.SCU);
	}
}
