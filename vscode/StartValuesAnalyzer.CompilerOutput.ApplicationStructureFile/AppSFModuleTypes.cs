using StartValuesAnalyzer.ModApplic;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFModuleTypes : AppSFPOUTypes
{
	public override string ElementName => "ModuleTypes";

	public AppSFModuleTypes(SAXElementBase parent, SourceCodeUnit sourceCodeUnit)
		: base(parent, sourceCodeUnit)
	{
	}

	protected override SAXElementBase CreateChild()
	{
		return new AppSFModuleType(this, base.SCU);
	}
}
