using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFSingleProgType : AppSFSinglePOUType
{
	public ProgInst ProgInst
	{
		set
		{
			base.POUInst = value;
		}
	}

	public AppSFSingleProgType(SAXElementBase parent, ProgInst progInst, int subTreeSize)
		: base(parent, progInst, subTreeSize)
	{
	}

	protected override POUType CreatePOUType(string name)
	{
		return ((ProgInst)base.POUInst).CreateSingleType();
	}
}
