using System;
using System.Collections.Generic;
using StartValuesAnalyzer.AppRuntimeStructure;
using StartValuesAnalyzer.ModApplic;

namespace StartValuesAnalyzer.POU;

public abstract class ObjType
{
	protected string name;

	protected SourceCodeUnit sourceCodeUnit;

	protected ObjInst firstInst;

	protected ObjInst lastInst;

	protected uint refCount;

	private static Dictionary<tObjTypeClass, ObjType> UndefTypes = new Dictionary<tObjTypeClass, ObjType>();

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

	public uint RefCount => refCount;

	public virtual ObjType Father => null;

	public virtual SourceCodeUnit SCU
	{
		get
		{
			ObjType objType = this;
			do
			{
				if (objType.sourceCodeUnit != null)
				{
					return objType.sourceCodeUnit;
				}
				objType = objType.Father;
			}
			while (objType != null);
			return null;
		}
		set
		{
			sourceCodeUnit = value;
		}
	}

	public virtual tValType ValType
	{
		get
		{
			return tValType.UndefType;
		}
		set
		{
		}
	}

	public ObjType(string name)
	{
		this.name = name;
		sourceCodeUnit = null;
		firstInst = null;
		lastInst = null;
		refCount = 0u;
	}

	public void AddInstanceRef(ObjInst objInst)
	{
		if (objInst != null)
		{
			if (lastInst != null)
			{
				objInst.PrevInst = lastInst;
				lastInst.NextInst = objInst;
			}
			else
			{
				firstInst = objInst;
			}
			lastInst = objInst;
			refCount++;
		}
	}

	public void RemoveInstanceRef(ObjInst objInst)
	{
		if (objInst != null)
		{
			ObjInst nextInst = objInst.NextInst;
			ObjInst prevInst = objInst.PrevInst;
			if (nextInst != null)
			{
				nextInst.PrevInst = prevInst;
			}
			else
			{
				lastInst = prevInst;
			}
			if (prevInst != null)
			{
				prevInst.NextInst = nextInst;
			}
			else
			{
				firstInst = nextInst;
			}
			if (refCount != 0)
			{
				refCount--;
			}
		}
	}

	public abstract void BuildTypeStruct();

	public virtual ObjInst FindInstance(string name)
	{
		return null;
	}

	public virtual bool GetVariableFromPath(string path, ref DataInst dataInst, ref ushort pouInstanceIndex, ref tMemReference memRef, ref bool writeAllowed, ref bool cvStatus)
	{
		if (path.Length == 0)
		{
			return false;
		}
		bool flag = true;
		int num = path.IndexOf('.');
		string text;
		if (num == -1)
		{
			text = path;
			flag = false;
		}
		else
		{
			text = path.Substring(0, num);
		}
		ObjInst objInst = FindInstance(text);
		if (objInst != null)
		{
			string path2 = "";
			if (flag)
			{
				num++;
				path2 = path.Substring(num, path.Length - num);
			}
			return objInst.GetVariableFromPath(path2, ref dataInst, ref pouInstanceIndex, ref memRef, ref writeAllowed, ref cvStatus);
		}
		return false;
	}

	protected bool AppendObjInstAndSetFather(ObjInst objInst, IDictionary<string, ObjInst> destList, ObjType father)
	{
		if (objInst == null || destList == null)
		{
			return false;
		}
		try
		{
			destList.Add(objInst.Name, objInst);
			objInst.Father = father;
			return true;
		}
		catch (ArgumentException)
		{
			return false;
		}
	}

	protected void BuildListStruct(IDictionary<string, ObjInst> instances)
	{
		if (instances == null)
		{
			return;
		}
		foreach (KeyValuePair<string, ObjInst> instance in instances)
		{
			instance.Value.BuildObjectStruct(this);
		}
	}

	protected void DeleteInstances(IDictionary<string, ObjInst> instances)
	{
		instances?.Clear();
	}

	protected ObjInst FindSubObject(IDictionary<string, ObjInst> objInsts, string name)
	{
		ObjInst value = null;
		objInsts?.TryGetValue(name, out value);
		return value;
	}

	private void UnlinkAllInstances()
	{
		while (firstInst != null)
		{
			firstInst.UnlinkFromType();
		}
	}

	public static ObjType GetUndefType(tObjTypeClass idClass)
	{
		if (idClass == tObjTypeClass.cNoOfIdClasses)
		{
			return null;
		}
		ObjType value = null;
		if (!UndefTypes.TryGetValue(idClass, out value))
		{
			switch (idClass)
			{
			case tObjTypeClass.DataTypeClass:
				value = new SimpleType("<Undefined>");
				break;
			case tObjTypeClass.CMTypeClass:
				value = new ReusableModType("<Undefined>");
				break;
			case tObjTypeClass.DiagramTypeClass:
				value = new ReusableDiagramType("<Undefined>");
				break;
			case tObjTypeClass.FBTypeClass:
				value = new FBType("<Undefined>");
				break;
			case tObjTypeClass.ProgTypeClass:
				value = new ReusableProgType("<Undefined>");
				break;
			case tObjTypeClass.FuncTypeClass:
				value = new FuncType("<Undefined>");
				break;
			default:
				return null;
			}
			value.ValType = tValType.UndefType;
			UndefTypes.Add(idClass, value);
		}
		return value;
	}

	public abstract void GetRetainProperties(string path, CProgramInstance programInstance, ushort pouInstanceIndex, tMemReference memRef, List<RetainProperties> retainProperties);
}
