using System.Collections.Generic;

namespace StartValuesAnalyzer.POU;

public abstract class ProgType : FBType
{
	public override IDictionary<string, ObjInst> ExternalVariables => null;

	public override IDictionary<string, ObjInst> MultiParameters => null;

	public ProgType(string name)
		: base(name)
	{
	}

	public override Parameter CreateExternalVariable(string name)
	{
		return null;
	}

	public override Parameter CreateMultiParameter(string name)
	{
		return null;
	}

	public override void BuildTypeStruct()
	{
		BuildListStruct(parameters);
		BuildListStruct(variables);
		BuildListStruct(sysGenVars);
		BuildListStruct(functionBlocks);
		BuildListStruct(externalVariables);
		BuildListStruct(controlModules);
	}
}
