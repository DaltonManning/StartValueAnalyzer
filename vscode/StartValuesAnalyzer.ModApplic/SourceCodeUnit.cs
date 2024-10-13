using System;
using System.Collections.Generic;
using StartValuesAnalyzer.AppRuntimeStructure;
using StartValuesAnalyzer.POU;

namespace StartValuesAnalyzer.ModApplic;

public abstract class SourceCodeUnit : FileOrganizationUnit
{
	private LibraryUnit systemLib;

	private TypeDefs typeDefs;

	protected Dictionary<string, LibraryUnit> libraries;

	public LibraryUnit SystemLib => systemLib;

	public IDictionary<string, LibraryUnit> LibraryReferance
	{
		set
		{
			libraries = new Dictionary<string, LibraryUnit>(value, StringComparer.OrdinalIgnoreCase);
			SetupSystemLibRef();
		}
	}

	public SourceCodeUnit(string name)
		: base(name)
	{
		typeDefs = new TypeDefs(this);
		libraries = new Dictionary<string, LibraryUnit>(StringComparer.OrdinalIgnoreCase);
	}

	public SimpleType CreateSimpleDataType(string name, tValType valType)
	{
		return typeDefs.CreateSimpleDataType(name, valType);
	}

	public StructType CreateStructDataType(string name, tValType valType)
	{
		return typeDefs.CreateStructDataType(name, valType);
	}

	public ArrayType CreateArrayDataType(string name)
	{
		return typeDefs.CreateArrayDataType(name);
	}

	public ReusableModType CreateControlModuleType(string name)
	{
		return typeDefs.CreateControlModuleType(name);
	}

	public ReusableDiagramType CreateDiagramType(string name)
	{
		return typeDefs.CreateDiagramType(name);
	}

	public FBType CreateFunctionBlockType(string name)
	{
		return typeDefs.CreateFunctionBlockType(name);
	}

	public ReusableProgType CreateProgramType(string name)
	{
		return typeDefs.CreateProgramType(name);
	}

	public virtual void BuildObjectStruct()
	{
		typeDefs.BuildTypeStruct();
	}

	public ObjType FindObjTypeScope(string typeName, tObjTypeClass objTypeClass)
	{
		ObjType objType = FindObjTypeLocal(typeName, objTypeClass);
		if (objType != null)
		{
			return objType;
		}
		foreach (KeyValuePair<string, LibraryUnit> library in libraries)
		{
			objType = library.Value.FindObjTypeLocal(typeName, objTypeClass);
			if (objType != null)
			{
				return objType;
			}
		}
		return null;
	}

	public ObjType FindObjTypeLocal(string typeName, tObjTypeClass objTypeClass)
	{
		return typeDefs.FindObjType(typeName, objTypeClass);
	}

	public ObjType FindQualifiedObjTypeScope(ObjInst objInst, tObjTypeClass objTypeClass, ref bool scuNotFound)
	{
		return FindQualifiedObjTypeScope(objInst.TypeRefSCUName, objInst.TypeRefName, objTypeClass, ref scuNotFound);
	}

	public ObjType FindQualifiedObjTypeScope(string typeSCUName, string typeName, tObjTypeClass objTypeClass, ref bool scuNotFound)
	{
		ObjType objType = null;
		scuNotFound = false;
		if (typeSCUName.Equals(base.Name, StringComparison.CurrentCultureIgnoreCase))
		{
			objType = FindObjTypeLocal(typeName, objTypeClass);
		}
		else if (typeSCUName.Equals(SystemLib.Name))
		{
			objType = SystemLib.FindObjTypeLocal(typeName, objTypeClass);
		}
		else if (typeSCUName.Length == 0)
		{
			objType = FindObjTypeScope(typeName, objTypeClass);
		}
		else
		{
			LibraryUnit value = null;
			if (libraries.TryGetValue(typeSCUName, out value))
			{
				objType = value.FindObjTypeLocal(typeName, objTypeClass);
			}
			else
			{
				objType = null;
				scuNotFound = true;
			}
		}
		return objType;
	}

	public LibraryUnit CreateLibrary(string name)
	{
		LibraryUnit libraryUnit = new LibraryUnit(name);
		if (!AddLibraryReference(libraryUnit))
		{
			libraryUnit = null;
		}
		return libraryUnit;
	}

	public bool AddLibraryReference(LibraryUnit library)
	{
		try
		{
			libraries.Add(library.Name, library);
			return true;
		}
		catch (ArgumentException)
		{
			return false;
		}
	}

	public virtual void DeleteLibraryRefs()
	{
		libraries.Clear();
		systemLib = null;
	}

	public void DeleteTypes()
	{
		typeDefs.DeleteTypes();
	}

	protected void SetupSystemLibRef()
	{
		if (systemLib == null && !libraries.TryGetValue("System", out systemLib) && base.Name.Equals("System", StringComparison.CurrentCultureIgnoreCase))
		{
			systemLib = this as LibraryUnit;
		}
	}
}
