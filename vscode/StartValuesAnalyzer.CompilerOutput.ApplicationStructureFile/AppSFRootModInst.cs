using StartValuesAnalyzer.ModApplic;
using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFRootModInst : AppSFSinglePOUInst
{
	private readonly ApplicationUnit application;

	public AppSFRootModInst(SAXElementBase parent, ApplicationUnit application)
		: base(parent)
	{
		this.application = application;
	}

	protected override POUInst CreatePOUInst(string name)
	{
		return application.CreateRootMod();
	}

	protected override AppSFSinglePOUType CreateChild()
	{
		return new AppSFRootModType(this, (CMInst)base.POUInst, application, base.SubTreeSize);
	}

	protected override void UpdateChild()
	{
	}
}
