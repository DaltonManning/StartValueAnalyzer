using System;
using System.Collections.Generic;
using StartValuesAnalyzer.ModApplic;

namespace StartValuesAnalyzer.POU;

public class SingleDiagramType : DiagramType
{
	protected Dictionary<string, ObjInst> communicationVariables;

	public override string Name
	{
		get
		{
			if (firstInst == null)
			{
				return name;
			}
			return firstInst.Name;
		}
		set
		{
			name = value;
		}
	}

	public override ObjType Father
	{
		get
		{
			if (firstInst == null)
			{
				return null;
			}
			return firstInst.Father;
		}
	}

	public override SourceCodeUnit SCU
	{
		get
		{
			return base.SCU;
		}
		set
		{
		}
	}

	public override IDictionary<string, ObjInst> CommunicationVariables => communicationVariables;

	public SingleDiagramType()
		: base("")
	{
		communicationVariables = new Dictionary<string, ObjInst>(StringComparer.OrdinalIgnoreCase);
	}

	public override CommunicationVariable CreateCommunicationVariable(string name)
	{
		CommunicationVariable communicationVariable = new CommunicationVariable(name);
		if (!AppendObjInstAndSetFather(communicationVariable, communicationVariables, this))
		{
			communicationVariable = null;
		}
		return communicationVariable;
	}

	public override void BuildTypeStruct()
	{
		BuildListStruct(communicationVariables);
		base.BuildTypeStruct();
	}

	public override ObjInst FindInstance(string name)
	{
		ObjInst objInst = FindSubObject(communicationVariables, name);
		if (objInst != null)
		{
			return objInst;
		}
		return base.FindInstance(name);
	}
}
