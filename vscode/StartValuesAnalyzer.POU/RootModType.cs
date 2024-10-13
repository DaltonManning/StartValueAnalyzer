using System;
using System.Collections.Generic;
using StartValuesAnalyzer.ModApplic;

namespace StartValuesAnalyzer.POU;

public class RootModType : SingleModType
{
	protected Dictionary<string, ObjInst> programs;

	protected Dictionary<string, ObjInst> diagrams;

	protected Dictionary<string, ObjInst> globalVariables;

	public override SourceCodeUnit SCU
	{
		get
		{
			return base.SCU;
		}
		set
		{
			sourceCodeUnit = value;
		}
	}

	public override IDictionary<string, ObjInst> Programs => programs;

	public override IDictionary<string, ObjInst> Diagrams => diagrams;

	public override IDictionary<string, ObjInst> Parameters => null;

	public override IDictionary<string, ObjInst> ExternalVariables => null;

	public override IDictionary<string, ObjInst> GlobalVariables => globalVariables;

	public override IDictionary<string, ObjInst> CommunicationVariables => null;

	public RootModType()
	{
		programs = new Dictionary<string, ObjInst>(StringComparer.OrdinalIgnoreCase);
		diagrams = new Dictionary<string, ObjInst>(StringComparer.OrdinalIgnoreCase);
		globalVariables = new Dictionary<string, ObjInst>(StringComparer.OrdinalIgnoreCase);
	}

	public override ProgInst CreateProgram(string name)
	{
		ProgInst progInst = new ProgInst(name);
		if (!AppendObjInstAndSetFather(progInst, programs, this))
		{
			progInst = null;
		}
		return progInst;
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

	public override Variable CreateGlobalVariable(string name)
	{
		Variable variable = new UserDefVar(name);
		if (!AppendObjInstAndSetFather(variable, globalVariables, this))
		{
			variable = null;
		}
		return variable;
	}

	public override CommunicationVariable CreateCommunicationVariable(string name)
	{
		return null;
	}

	public override void BuildTypeStruct()
	{
		BuildListStruct(globalVariables);
		BuildListStruct(programs);
		BuildListStruct(diagrams);
		base.BuildTypeStruct();
	}

	public override ObjInst FindInstance(string name)
	{
		ObjInst objInst = FindSubObject(programs, name);
		if (objInst != null)
		{
			return objInst;
		}
		objInst = FindSubObject(diagrams, name);
		if (objInst != null)
		{
			return objInst;
		}
		objInst = FindSubObject(globalVariables, name);
		if (objInst != null)
		{
			return objInst;
		}
		return base.FindInstance(name);
	}
}
