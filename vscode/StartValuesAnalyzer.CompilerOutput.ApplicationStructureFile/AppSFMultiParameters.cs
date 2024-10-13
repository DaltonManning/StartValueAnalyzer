using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFMultiParameters : AppSFParametersBase
{
	public override string ElementName => "MultiParams";

	public AppSFMultiParameters(SAXElementBase parent, POUType pouType)
		: base(parent, pouType)
	{
	}

	protected override SAXElementBase CreateChild()
	{
		return new AppSFMultiParameter(this, base.POUType);
	}
}
