using System.Collections.Generic;
using StartValuesAnalyzer.AppRuntimeStructure;

namespace StartValuesAnalyzer.POU;

public abstract class POUType : ObjType
{
	private uint subTreeSize;

	public uint SubTreeSize
	{
		get
		{
			return subTreeSize;
		}
		set
		{
			subTreeSize = value;
		}
	}

	public virtual IDictionary<string, ObjInst> Programs => null;

	public virtual IDictionary<string, ObjInst> ControlModules => null;

	public virtual IDictionary<string, ObjInst> Diagrams => null;

	public virtual IDictionary<string, ObjInst> FunctionBlocks => null;

	public virtual IDictionary<string, ObjInst> Parameters => null;

	public virtual IDictionary<string, ObjInst> ExternalVariables => null;

	public virtual IDictionary<string, ObjInst> GlobalVariables => null;

	public virtual IDictionary<string, ObjInst> CommunicationVariables => null;

	public virtual IDictionary<string, ObjInst> Variables => null;

	public virtual IDictionary<string, ObjInst> SysGenVars => null;

	public virtual IDictionary<string, ObjInst> MultiParameters => null;

	public POUType(string name)
		: base(name)
	{
		subTreeSize = 0u;
	}

	public virtual ProgInst CreateProgram(string name)
	{
		return null;
	}

	public virtual CMInst CreateControlModule(string name)
	{
		return null;
	}

	public virtual DiagramInst CreateDiagram(string name)
	{
		return null;
	}

	public virtual FBInst CreateFunctionBlock(string name)
	{
		return null;
	}

	public virtual Parameter CreateParameter(string name)
	{
		return null;
	}

	public virtual Parameter CreateExternalVariable(string name)
	{
		return null;
	}

	public virtual Variable CreateGlobalVariable(string name)
	{
		return null;
	}

	public virtual CommunicationVariable CreateCommunicationVariable(string name)
	{
		return null;
	}

	public virtual Variable CreateVariable(string name)
	{
		return null;
	}

	public virtual Variable CreateSystemGeneratedVariable(string name, tSysGenVarType varType)
	{
		return null;
	}

	public virtual Parameter CreateMultiParameter(string name)
	{
		return null;
	}

	public override void GetRetainProperties(string path, CProgramInstance programInstance, ushort pouInstanceIndex, tMemReference memRef, List<RetainProperties> retainProperties)
	{
		IDictionary<string, ObjInst> dictionary = null;
		for (int i = 0; i < 10; i++)
		{
			dictionary = i switch
			{
				0 => Parameters, 
				1 => GlobalVariables, 
				2 => Variables, 
				3 => ExternalVariables, 
				4 => CommunicationVariables, 
				5 => SysGenVars, 
				6 => ControlModules, 
				7 => Programs, 
				8 => Diagrams, 
				9 => FunctionBlocks, 
				_ => null, 
			};
			if (dictionary == null)
			{
				continue;
			}
			foreach (KeyValuePair<string, ObjInst> item in dictionary)
			{
				item.Value.GetRetainProperties(path, programInstance, pouInstanceIndex, memRef, retainProperties);
			}
		}
	}
}
