using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFModuleInstances : AppSFPOUInstancesBase
{
	public override string ElementName => "ModuleInsts";

	public AppSFModuleInstances(SAXElementBase parent, POUType pouType)
		: base(parent, pouType)
	{
	}

	protected override AppSFPOUInstance CreateSingleChild()
	{
		return new AppSFSingleModInst(this, base.POUType);
	}

	protected override AppSFPOUInstance CreateReusableChild()
	{
		return new AppSFReusableModInst(this, base.POUType);
	}
}
