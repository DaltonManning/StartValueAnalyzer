using System.Collections.Generic;

namespace StartValuesAnalyzer.POU;

public class ReusableDiagramType : DiagramType
{
	public override IDictionary<string, ObjInst> Parameters => parameters;

	public ReusableDiagramType(string name)
		: base(name)
	{
	}

	public override Parameter CreateParameter(string name)
	{
		Parameter parameter = new Parameter(name);
		if (!AppendObjInstAndSetFather(parameter, parameters, this))
		{
			parameter = null;
		}
		return parameter;
	}

	public override void BuildTypeStruct()
	{
		BuildListStruct(parameters);
		base.BuildTypeStruct();
	}
}
