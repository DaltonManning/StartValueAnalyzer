using System.Collections.Generic;
using StartValuesAnalyzer.AppRuntimeStructure;
using StartValuesAnalyzer.ModApplic;

namespace StartValuesAnalyzer.POU;

public abstract class ObjInst
{
	protected string name;

	protected ObjType type;

	protected ObjType father;

	private TypeRef typeRef;

	private ObjInst prevInst;

	private ObjInst nextInst;

	public virtual string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public ObjType Father
	{
		get
		{
			return father;
		}
		set
		{
			father = value;
		}
	}

	public ObjType Type => type;

	public abstract tObjTypeClass ObjTypeClass { get; }

	public ObjInst NextInst
	{
		get
		{
			return nextInst;
		}
		set
		{
			nextInst = value;
		}
	}

	public ObjInst PrevInst
	{
		get
		{
			return prevInst;
		}
		set
		{
			prevInst = value;
		}
	}

	public bool ReusableInst => typeRef.TypePtr == null;

	public bool SingleInst => !ReusableInst;

	public string TypeRefName
	{
		get
		{
			return typeRef.TypeName;
		}
		set
		{
			typeRef.TypeName = value;
			typeRef.TypePtr = null;
		}
	}

	public string TypeRefSCUName
	{
		get
		{
			return typeRef.TypeSCUName;
		}
		set
		{
			typeRef.TypeSCUName = value;
			typeRef.TypePtr = null;
		}
	}

	public ObjType SingleTypeRef
	{
		get
		{
			return typeRef.TypePtr;
		}
		protected set
		{
			if (typeRef.TypePtr != null)
			{
				UnlinkFromType();
			}
			typeRef.TypeName = "";
			typeRef.TypeSCUName = "";
			typeRef.TypePtr = value;
			LinkToType(value);
		}
	}

	protected ObjType UndefType => ObjType.GetUndefType(ObjTypeClass);

	public ObjInst(string name)
	{
		this.name = name;
		typeRef = new TypeRef();
	}

	public void LinkToType(ObjType objType)
	{
		if (type != objType)
		{
			UnlinkFromType();
			if (objType != null)
			{
				objType.AddInstanceRef(this);
				type = objType;
			}
		}
	}

	public void UnlinkFromType()
	{
		if (type != null)
		{
			type.RemoveInstanceRef(this);
			prevInst = null;
			nextInst = null;
			type = null;
		}
	}

	public virtual void BuildObjectStruct(ObjType father)
	{
		this.father = father;
		if (type == null)
		{
			if (!ReusableInst)
			{
				return;
			}
			SourceCodeUnit fatherSCU = null;
			if (this.father != null)
			{
				fatherSCU = this.father.SCU;
			}
			ObjType objType = FindTypeDeclaration(fatherSCU);
			if (objType == null)
			{
				if (typeRef.TypeSCUName.Length == 0)
				{
					_ = "Instance '" + Name + "' was unable to find type '" + typeRef.TypeName + "'.";
				}
				else
				{
					_ = "Instance '" + Name + "' was unable to find type '" + typeRef.TypeName + "' in SCU '" + typeRef.TypeSCUName + "'.";
				}
				objType = UndefType;
			}
			LinkToType(objType);
		}
		else if (SingleInst)
		{
			type.BuildTypeStruct();
		}
	}

	private ObjType FindTypeDeclaration(SourceCodeUnit fatherSCU)
	{
		if (fatherSCU == null)
		{
			return null;
		}
		ObjType objType = null;
		if (typeRef.TypeSCUName.Length != 0)
		{
			bool scuNotFound = false;
			return fatherSCU.FindQualifiedObjTypeScope(this, ObjTypeClass, ref scuNotFound);
		}
		return fatherSCU.SystemLib.FindObjTypeScope(typeRef.TypeName, ObjTypeClass);
	}

	public abstract bool GetVariableFromPath(string path, ref DataInst dataInst, ref ushort pouInstanceIndex, ref tMemReference memRef, ref bool writeAllowed, ref bool cvStatus);

	public abstract void GetRetainProperties(string path, CProgramInstance programInstance, ushort pouInstanceIndex, tMemReference memRef, List<RetainProperties> retainProperties);
}
