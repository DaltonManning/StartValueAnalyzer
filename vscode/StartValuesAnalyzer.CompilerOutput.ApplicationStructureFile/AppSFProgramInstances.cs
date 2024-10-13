using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFProgramInstances : AppSFPOUInstancesBase
{
	public override string ElementName => "ProgInsts";

	public AppSFProgramInstances(SAXElementBase parent, RootModType rootModType)
		: base(parent, rootModType)
	{
	}

	protected override AppSFPOUInstance CreateSingleChild()
	{
		return new AppSFProgramInst(this, (RootModType)base.POUType);
	}

	protected override AppSFPOUInstance CreateReusableChild()
	{
		return null;
	}
}
