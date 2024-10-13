namespace StartValuesAnalyzer.POU;

public abstract class CMType : ModType
{
	public CMType(string name)
		: base(name)
	{
	}

	public override void BuildTypeStruct()
	{
		BuildListStruct(parameters);
		BuildListStruct(sysGenVars);
		BuildListStruct(variables);
		BuildListStruct(functionBlocks);
		BuildListStruct(externalVariables);
		BuildListStruct(controlModules);
	}
}
