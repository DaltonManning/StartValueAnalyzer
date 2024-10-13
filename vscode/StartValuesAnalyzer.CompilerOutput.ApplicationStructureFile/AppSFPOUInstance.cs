using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public abstract class AppSFPOUInstance : SAXElementBase
{
	private POUInst pouInst;

	public override string ElementName => "POUInst";

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

	public AppSFPOUInstance(SAXElementBase parent)
		: base(parent)
	{
	}

	protected abstract POUInst CreatePOUInst(string name);

	public override bool HandleAttributes(SAXAttributes attributes)
	{
		string attributeValue = "";
		int attributeValue2 = 0;
		attributes.GetAttribute("Name", ref attributeValue);
		attributes.GetAttribute("POUOffset", ref attributeValue2);
		pouInst = CreatePOUInst(attributeValue);
		pouInst.POUOffset = (ushort)attributeValue2;
		return true;
	}
}
