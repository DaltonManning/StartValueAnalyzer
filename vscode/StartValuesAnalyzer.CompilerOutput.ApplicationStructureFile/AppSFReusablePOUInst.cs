using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public abstract class AppSFReusablePOUInst : AppSFPOUInstance
{
	protected readonly POUType pouType;

	public AppSFReusablePOUInst(SAXElementBase parent, POUType pouType)
		: base(parent)
	{
		this.pouType = pouType;
	}

	public override bool HandleAttributes(SAXAttributes attributes)
	{
		if (!base.HandleAttributes(attributes))
		{
			return false;
		}
		string attributeValue = "";
		attributes.GetAttribute("SCU", ref attributeValue);
		base.POUInst.TypeRefSCUName = attributeValue;
		attributeValue = "";
		attributes.GetAttribute("TypeName", ref attributeValue);
		base.POUInst.TypeRefName = attributeValue;
		return true;
	}
}
