using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFSingleModType : AppSFSinglePOUType
{
	public CMInst CMInst
	{
		set
		{
			base.POUInst = value;
		}
	}

	public AppSFSingleModType(SAXElementBase parent, CMInst cmInst, int subTreeSize)
		: base(parent, cmInst, subTreeSize)
	{
	}

	protected override POUType CreatePOUType(string name)
	{
		return ((CMInst)base.POUInst).CreateSingleType();
	}
}
