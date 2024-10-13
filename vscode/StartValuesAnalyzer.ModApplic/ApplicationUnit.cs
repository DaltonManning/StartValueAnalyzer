using System.Collections.Generic;
using StartValuesAnalyzer.AppRuntimeStructure;
using StartValuesAnalyzer.POU;

namespace StartValuesAnalyzer.ModApplic;

public class ApplicationUnit : SourceCodeUnit
{
	private CMInst rootMod;

	private CProgramInstance programInstance;

	private string timestamp;

	public CMInst RootMod => rootMod;

	public CProgramInstance ProgramInstance
	{
		get
		{
			return programInstance;
		}
		set
		{
			programInstance = value;
		}
	}

	public string Timestamp
	{
		get
		{
			return timestamp;
		}
		set
		{
			timestamp = value;
		}
	}

	public ApplicationUnit(string name)
		: base(name)
	{
		timestamp = "";
	}

	public CMInst CreateRootMod()
	{
		DeleteRootMod();
		rootMod = new CMInst(base.Name);
		return rootMod;
	}

	public void DeleteRootMod()
	{
		rootMod = null;
	}

	public override void BuildObjectStruct()
	{
		base.BuildObjectStruct();
		rootMod.BuildObjectStruct(null);
	}

	public void SetUpLibRefsBetweenLibs()
	{
		foreach (KeyValuePair<string, LibraryUnit> library in libraries)
		{
			if (library.Value != null)
			{
				library.Value.LibraryReferance = libraries;
				library.Value.DeleteLibraryReference(library.Value);
			}
		}
		SetupSystemLibRef();
	}

	public void BuildAppAndLibs()
	{
		foreach (KeyValuePair<string, LibraryUnit> library in libraries)
		{
			library.Value.BuildObjectStruct();
		}
		BuildObjectStruct();
	}

	public override void DeleteLibraryRefs()
	{
		foreach (KeyValuePair<string, LibraryUnit> library in libraries)
		{
			library.Value.DeleteLibraryRefs();
		}
		base.DeleteLibraryRefs();
	}

	public bool GetVariableFromPath(string path, ref DataInst dataInst, ref CProgramInstance programInstance, ref ushort pouInstanceIndex, ref tMemReference memRef, ref bool writeAllowed, ref bool cvStatus)
	{
		if (rootMod == null)
		{
			return false;
		}
		pouInstanceIndex = 0;
		bool variableFromPath = rootMod.GetVariableFromPath(path, ref dataInst, ref pouInstanceIndex, ref memRef, ref writeAllowed, ref cvStatus);
		if (variableFromPath)
		{
			programInstance = this.programInstance;
		}
		return variableFromPath;
	}

	public void GetRetainProperties(List<RetainProperties> retainProperties)
	{
		if (rootMod != null && programInstance != null)
		{
			ushort pouInstanceIndex = 0;
			rootMod.GetRetainProperties("Applications", programInstance, pouInstanceIndex, default(tMemReference), retainProperties);
		}
	}
}
