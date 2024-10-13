using System;
using System.Collections.Generic;
using StartValuesAnalyzer.AppRuntimeStructure;

namespace StartValuesAnalyzer.POU;

public class StructType : DataType
{
	private Dictionary<string, ObjInst> components;

	public override IDictionary<string, ObjInst> Components => components;

	public StructType(string name)
		: base(name)
	{
		components = new Dictionary<string, ObjInst>(StringComparer.OrdinalIgnoreCase);
	}

	public StructComponent CreateStructComponent(string name)
	{
		StructComponent structComponent = new StructComponent(name);
		if (!AppendObjInstAndSetFather(structComponent, components, this))
		{
			structComponent = null;
		}
		return structComponent;
	}

	public override void BuildTypeStruct()
	{
		BuildListStruct(components);
	}

	public override ObjInst FindInstance(string name)
	{
		ObjInst objInst = FindSubObject(components, name);
		if (objInst != null)
		{
			return objInst;
		}
		return base.FindInstance(name);
	}

	public override void GetRetainProperties(string path, CProgramInstance programInstance, ushort pouInstanceIndex, tMemReference memRef, List<RetainProperties> retainProperties)
	{
		int count = retainProperties.Count;
		foreach (KeyValuePair<string, ObjInst> component in components)
		{
			component.Value.GetRetainProperties(path, programInstance, pouInstanceIndex, memRef, retainProperties);
		}
		if ((ValType == tValType.TimeType || ValType == tValType.DurationType) && count != retainProperties.Count)
		{
			RetainProperties retainProperties2 = retainProperties[retainProperties.Count - 2];
			RetainProperties retainProperties3 = retainProperties[retainProperties.Count - 1];
			string initValue;
			if (ValType == tValType.TimeType)
			{
				ulong fileTime = 119599200000000000L + ulong.Parse(retainProperties2.InitValue) * 864000000000L + ulong.Parse(retainProperties3.InitValue) * 10000;
				DateTime dateTime = DateTime.FromFileTime((long)fileTime);
				initValue = $"{dateTime:yyyy-MM-dd HH:mm:ss}";
			}
			else
			{
				initValue = (double.Parse(retainProperties2.InitValue) * 86400000.0 + double.Parse(retainProperties3.InitValue)).ToString();
			}
			retainProperties3 = null;
			retainProperties.RemoveAt(retainProperties.Count - 1);
			string path2 = retainProperties2.Path;
			int length = path2.LastIndexOf('.');
			retainProperties2.Path = path2.Substring(0, length);
			retainProperties2.InitValue = initValue;
			retainProperties2.ValType = ValType;
		}
	}
}
