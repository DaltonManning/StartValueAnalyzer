using System;
using System.Collections.Generic;

namespace StartValuesAnalyzer.POU;

public abstract class ModType : POUType
{
	protected Dictionary<string, ObjInst> controlModules;

	protected Dictionary<string, ObjInst> functionBlocks;

	protected Dictionary<string, ObjInst> parameters;

	protected Dictionary<string, ObjInst> externalVariables;

	protected Dictionary<string, ObjInst> variables;

	protected Dictionary<string, ObjInst> sysGenVars;

	public override IDictionary<string, ObjInst> ControlModules => controlModules;

	public override IDictionary<string, ObjInst> FunctionBlocks => functionBlocks;

	public override IDictionary<string, ObjInst> Parameters => parameters;

	public override IDictionary<string, ObjInst> ExternalVariables => externalVariables;

	public override IDictionary<string, ObjInst> Variables => variables;

	public override IDictionary<string, ObjInst> SysGenVars => sysGenVars;

	public ModType(string name)
		: base(name)
	{
		controlModules = new Dictionary<string, ObjInst>(StringComparer.OrdinalIgnoreCase);
		functionBlocks = new Dictionary<string, ObjInst>(StringComparer.OrdinalIgnoreCase);
		parameters = new Dictionary<string, ObjInst>(StringComparer.OrdinalIgnoreCase);
		externalVariables = new Dictionary<string, ObjInst>(StringComparer.OrdinalIgnoreCase);
		variables = new Dictionary<string, ObjInst>(StringComparer.OrdinalIgnoreCase);
		sysGenVars = new Dictionary<string, ObjInst>(StringComparer.OrdinalIgnoreCase);
	}

	public override CMInst CreateControlModule(string name)
	{
		CMInst cMInst = new CMInst(name);
		if (!AppendObjInstAndSetFather(cMInst, controlModules, this))
		{
			cMInst = null;
		}
		return cMInst;
	}

	public override FBInst CreateFunctionBlock(string name)
	{
		FBInst fBInst = new FBInst(name);
		if (!AppendObjInstAndSetFather(fBInst, functionBlocks, this))
		{
			fBInst = null;
		}
		return fBInst;
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

	public override Parameter CreateExternalVariable(string name)
	{
		Parameter parameter = new Parameter(name);
		if (!AppendObjInstAndSetFather(parameter, externalVariables, this))
		{
			parameter = null;
		}
		return parameter;
	}

	public override Variable CreateVariable(string name)
	{
		Variable variable = new UserDefVar(name);
		if (!AppendObjInstAndSetFather(variable, variables, this))
		{
			variable = null;
		}
		return variable;
	}

	public override Variable CreateSystemGeneratedVariable(string name, tSysGenVarType varType)
	{
		Variable variable = null;
		switch (varType)
		{
		case tSysGenVarType.vCompGen:
			variable = new CompGenVar(name);
			break;
		case tSysGenVarType.vSeqGen:
			variable = new SeqGenVar(name);
			break;
		case tSysGenVarType.vGraphGen:
			variable = new GraphGenVar(name);
			break;
		case tSysGenVarType.vDefGen:
			variable = new DefGenVar(name);
			break;
		case tSysGenVarType.vLitGen:
			variable = new LitGenVar(name);
			break;
		case tSysGenVarType.vProjGen:
			variable = new ProjGenVar(name);
			break;
		case tSysGenVarType.vSTBatchGen:
			variable = new STBatchGenVar(name);
			break;
		case tSysGenVarType.vFDCompGen:
			variable = new FDCompGenVar(name);
			break;
		default:
			return null;
		}
		if (!AppendObjInstAndSetFather(variable, sysGenVars, this))
		{
			if (varType == tSysGenVarType.vLitGen || varType == tSysGenVarType.vGraphGen)
			{
				ObjInst value = null;
				variable = ((!sysGenVars.TryGetValue(variable.Name, out value)) ? null : (value as Variable));
			}
			else
			{
				variable = null;
			}
		}
		return variable;
	}

	public override ObjInst FindInstance(string name)
	{
		ObjInst objInst = FindSubObject(controlModules, name);
		if (objInst != null)
		{
			return objInst;
		}
		objInst = FindSubObject(functionBlocks, name);
		if (objInst != null)
		{
			return objInst;
		}
		objInst = FindSubObject(parameters, name);
		if (objInst != null)
		{
			return objInst;
		}
		objInst = FindSubObject(externalVariables, name);
		if (objInst != null)
		{
			return objInst;
		}
		objInst = FindSubObject(variables, name);
		if (objInst != null)
		{
			return objInst;
		}
		objInst = FindSubObject(sysGenVars, name);
		if (objInst != null)
		{
			return objInst;
		}
		return base.FindInstance(name);
	}
}
