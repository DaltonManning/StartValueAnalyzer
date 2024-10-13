using StartValuesAnalyzer.ModApplic;
using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFRootModType : AppSFSinglePOUType
{
	private readonly ApplicationUnit application;

	public AppSFRootModType(SAXElementBase parent, CMInst rootModInst, ApplicationUnit application, int subTreeSize)
		: base(parent, rootModInst, subTreeSize)
	{
		this.application = application;
	}

	protected override POUType CreatePOUType(string name)
	{
		return ((CMInst)base.POUInst).CreateRootModType(application);
	}
}
