using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public abstract class AppSFSinglePOUType : AppSFPOUType
{
	private POUInst pouInst;

	private int subTreeSize;

	protected POUInst POUInst
	{
		get
		{
			return pouInst;
		}
		set
		{
			pouInst = value;
		}
	}

	public int SubTreeSize
	{
		set
		{
			subTreeSize = value;
		}
	}

	public AppSFSinglePOUType(SAXElementBase parent, POUInst pouInst, int subTreeSize)
		: base(parent)
	{
		this.pouInst = pouInst;
		this.subTreeSize = subTreeSize;
	}

	public override bool HandleAttributes(SAXAttributes attributes)
	{
		if (!base.HandleAttributes(attributes))
		{
			return false;
		}
		base.POUType.SubTreeSize = (uint)subTreeSize;
		return true;
	}
}
