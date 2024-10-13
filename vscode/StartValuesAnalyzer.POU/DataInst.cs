using System;
using System.Collections.Generic;
using StartValuesAnalyzer.AppRuntimeStructure;

namespace StartValuesAnalyzer.POU;

public abstract class DataInst : ObjInst
{
	protected ushort varOffset;

	protected bool hidden;

	protected bool constant;

	protected bool state;

	protected bool retain;

	protected bool coldRetain;

	protected bool noSort;

	protected bool readOnly;

	protected bool noUserAccess;

	protected bool volatile_;

	public override tObjTypeClass ObjTypeClass => tObjTypeClass.DataTypeClass;

	public ushort VarOffset
	{
		get
		{
			return varOffset;
		}
		set
		{
			varOffset = value;
		}
	}

	public bool Hidden
	{
		get
		{
			return hidden;
		}
		set
		{
			hidden = value;
		}
	}

	public bool Constant
	{
		get
		{
			return constant;
		}
		set
		{
			constant = value;
		}
	}

	public bool State
	{
		get
		{
			return state;
		}
		set
		{
			state = value;
		}
	}

	public bool Retain
	{
		get
		{
			return retain;
		}
		set
		{
			retain = value;
		}
	}

	public bool ColdRetain
	{
		get
		{
			return coldRetain;
		}
		set
		{
			coldRetain = value;
			if (value)
			{
				retain = true;
			}
		}
	}

	public bool NoSort
	{
		get
		{
			return noSort;
		}
		set
		{
			noSort = value;
		}
	}

	public bool ReadOnly
	{
		get
		{
			return readOnly;
		}
		set
		{
			readOnly = value;
		}
	}

	public bool NoUserAccess
	{
		get
		{
			return noUserAccess;
		}
		set
		{
			noUserAccess = value;
		}
	}

	public bool Volatile
	{
		get
		{
			return volatile_;
		}
		set
		{
			volatile_ = value;
		}
	}

	public bool WriteProtected
	{
		get
		{
			if (!constant)
			{
				return readOnly;
			}
			return true;
		}
	}

	public virtual bool Accessible
	{
		get
		{
			if (type == null || noUserAccess)
			{
				return false;
			}
			return ((DataType)type).Accessible;
		}
	}

	public bool ValueInIODataType
	{
		get
		{
			if (!name.Equals("Value", StringComparison.CurrentCultureIgnoreCase) && !name.Equals("IOValue", StringComparison.CurrentCultureIgnoreCase))
			{
				return false;
			}
			if (father == null)
			{
				return false;
			}
			string text = father.Name;
			if (text.Equals("BoolIO", StringComparison.CurrentCultureIgnoreCase) || text.Equals("RealIO", StringComparison.CurrentCultureIgnoreCase) || text.Equals("DintIO", StringComparison.CurrentCultureIgnoreCase) || text.Equals("DwordIO", StringComparison.CurrentCultureIgnoreCase))
			{
				return true;
			}
			return false;
		}
	}

	public DataInst(string name)
		: base(name)
	{
	}

	public override bool GetVariableFromPath(string path, ref DataInst dataInst, ref ushort pouInstanceIndex, ref tMemReference memRef, ref bool writeAllowed, ref bool cvStatus)
	{
		if (type == null)
		{
			return false;
		}
		dataInst = this;
		GetMemRef(ref memRef);
		writeAllowed = !WriteProtected;
		cvStatus = false;
		return type.GetVariableFromPath(path, ref dataInst, ref pouInstanceIndex, ref memRef, ref writeAllowed, ref cvStatus);
	}

	protected virtual void GetMemRef(ref tMemReference memRef)
	{
		GetMemRef(ref memRef, 0);
	}

	protected abstract void GetMemRef(ref tMemReference memRef, ushort externalOffset);

	public override void GetRetainProperties(string path, CProgramInstance programInstance, ushort pouInstanceIndex, tMemReference memRef, List<RetainProperties> retainProperties)
	{
		if (type != null && !noUserAccess)
		{
			string path2 = path + "." + Name;
			GetMemRef(ref memRef);
			type.GetRetainProperties(path2, programInstance, pouInstanceIndex, memRef, retainProperties);
		}
	}
}
