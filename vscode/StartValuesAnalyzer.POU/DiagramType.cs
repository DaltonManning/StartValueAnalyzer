using System;
using System.Collections.Generic;

namespace StartValuesAnalyzer.POU;

public abstract class DiagramType : ModType
{
	protected Dictionary<string, ObjInst> diagrams;

	public override IDictionary<string, ObjInst> Diagrams => diagrams;

	public override IDictionary<string, ObjInst> Parameters => null;

	public override IDictionary<string, ObjInst> ExternalVariables => null;

	public DiagramType(string name)
		: base(name)
	{
		diagrams = new Dictionary<string, ObjInst>(StringComparer.OrdinalIgnoreCase);
	}

	public override DiagramInst CreateDiagram(string name)
	{
		DiagramInst diagramInst = new DiagramInst(name);
		if (!AppendObjInstAndSetFather(diagramInst, diagrams, this))
		{
			diagramInst = null;
		}
		return diagramInst;
	}

	public override Parameter CreateParameter(string name)
	{
		return null;
	}

	public override Parameter CreateExternalVariable(string name)
	{
		return null;
	}

	public override void BuildTypeStruct()
	{
		BuildListStruct(sysGenVars);
		BuildListStruct(variables);
		BuildListStruct(functionBlocks);
		BuildListStruct(controlModules);
		BuildListStruct(diagrams);
	}

	public override ObjInst FindInstance(string name)
	{
		ObjInst objInst = FindSubObject(diagrams, name);
		if (objInst != null)
		{
			return objInst;
		}
		return base.FindInstance(name);
	}
}
