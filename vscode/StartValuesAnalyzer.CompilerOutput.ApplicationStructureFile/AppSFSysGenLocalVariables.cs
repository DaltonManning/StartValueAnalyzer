using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFSysGenLocalVariables : AppSFVariablesBase
{
	public override string ElementName => "SysGenLocVars";

	public AppSFSysGenLocalVariables(SAXElementBase parent, POUType pouType)
		: base(parent, pouType)
	{
	}

	protected override SAXElementBase CreateChild()
	{
		return new AppSFSysGenLocalVariable(this, base.POUType);
	}
}
