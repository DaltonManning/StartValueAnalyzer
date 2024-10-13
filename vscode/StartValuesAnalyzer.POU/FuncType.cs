using System;
using System.Collections.Generic;

namespace StartValuesAnalyzer.POU;

public class FuncType : POUType
{
	protected Dictionary<string, ObjInst> parameters;

	public virtual tObjTypeClass ObjTypeClass => tObjTypeClass.FuncTypeClass;

	public override IDictionary<string, ObjInst> Parameters => parameters;

	public FuncType(string name)
		: base(name)
	{
		parameters = new Dictionary<string, ObjInst>(StringComparer.OrdinalIgnoreCase);
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
	}

	public override ObjInst FindInstance(string name)
	{
		FindSubObject(parameters, name);
		return base.FindInstance(name);
	}
}
