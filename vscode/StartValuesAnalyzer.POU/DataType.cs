using System.Collections.Generic;
using StartValuesAnalyzer.AppRuntimeStructure;

namespace StartValuesAnalyzer.POU;

public abstract class DataType : ObjType
{
	protected tValType valType = tValType.cNoOfValTypes;

	protected bool accessible;

	public override tValType ValType
	{
		get
		{
			return valType;
		}
		set
		{
			valType = value;
			switch (valType)
			{
			case tValType.BoolType:
			case tValType.WordType:
			case tValType.DWordType:
			case tValType.IntType:
			case tValType.DIntType:
			case tValType.UIntType:
			case tValType.RealType:
			case tValType.StringType:
			case tValType.DurationType:
			case tValType.TimeType:
				accessible = true;
				break;
			default:
				accessible = false;
				break;
			}
		}
	}

	public virtual IDictionary<string, ObjInst> Components => null;

	public bool Accessible => accessible;

	public DataType(string name)
		: base(name)
	{
	}

	public override void BuildTypeStruct()
	{
	}

	public override bool GetVariableFromPath(string path, ref DataInst dataInst, ref ushort pouInstanceIndex, ref tMemReference memRef, ref bool writeAllowed, ref bool cvStatus)
	{
		if (path.Length == 0)
		{
			return true;
		}
		return base.GetVariableFromPath(path, ref dataInst, ref pouInstanceIndex, ref memRef, ref writeAllowed, ref cvStatus);
	}

	public override void GetRetainProperties(string path, CProgramInstance programInstance, ushort pouInstanceIndex, tMemReference memRef, List<RetainProperties> retainProperties)
	{
	}
}
