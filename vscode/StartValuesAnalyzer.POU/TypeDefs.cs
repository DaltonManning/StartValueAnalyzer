using System;
using System.Collections.Generic;
using StartValuesAnalyzer.AppRuntimeStructure;
using StartValuesAnalyzer.ModApplic;

namespace StartValuesAnalyzer.POU;

public class TypeDefs
{
	private readonly SourceCodeUnit sourceCodeUnit;

	private Dictionary<string, DataType> dataTypes;

	private Dictionary<string, ReusableModType> controlModuleTypes;

	private Dictionary<string, ReusableDiagramType> diagramTypes;

	private Dictionary<string, FBType> functionBlockTypes;

	private Dictionary<string, ReusableProgType> programTypes;

	private Dictionary<string, FuncType> functionTypes;

	public TypeDefs(SourceCodeUnit sourceCodeUnit)
	{
		this.sourceCodeUnit = sourceCodeUnit;
		dataTypes = new Dictionary<string, DataType>(StringComparer.OrdinalIgnoreCase);
		controlModuleTypes = new Dictionary<string, ReusableModType>(StringComparer.OrdinalIgnoreCase);
		diagramTypes = new Dictionary<string, ReusableDiagramType>(StringComparer.OrdinalIgnoreCase);
		functionBlockTypes = new Dictionary<string, FBType>(StringComparer.OrdinalIgnoreCase);
		programTypes = new Dictionary<string, ReusableProgType>(StringComparer.OrdinalIgnoreCase);
		functionTypes = new Dictionary<string, FuncType>(StringComparer.OrdinalIgnoreCase);
	}

	public SimpleType CreateSimpleDataType(string name, tValType valType)
	{
		SimpleType simpleType = new SimpleType(name);
		try
		{
			dataTypes.Add(simpleType.Name, simpleType);
			simpleType.SCU = sourceCodeUnit;
			simpleType.ValType = valType;
		}
		catch (ArgumentException)
		{
			simpleType = null;
		}
		return simpleType;
	}

	public StructType CreateStructDataType(string name, tValType valType)
	{
		StructType structType = new StructType(name);
		try
		{
			dataTypes.Add(structType.Name, structType);
			structType.SCU = sourceCodeUnit;
			structType.ValType = valType;
		}
		catch (ArgumentException)
		{
			structType = null;
		}
		return structType;
	}

	public ArrayType CreateArrayDataType(string name)
	{
		ArrayType arrayType = new ArrayType(name);
		try
		{
			dataTypes.Add(arrayType.Name, arrayType);
			arrayType.SCU = sourceCodeUnit;
		}
		catch (ArgumentException)
		{
			arrayType = null;
		}
		return arrayType;
	}

	public ReusableModType CreateControlModuleType(string name)
	{
		ReusableModType reusableModType = new ReusableModType(name);
		try
		{
			controlModuleTypes.Add(reusableModType.Name, reusableModType);
			reusableModType.SCU = sourceCodeUnit;
		}
		catch (ArgumentException)
		{
			reusableModType = null;
		}
		return reusableModType;
	}

	public ReusableDiagramType CreateDiagramType(string name)
	{
		ReusableDiagramType reusableDiagramType = new ReusableDiagramType(name);
		try
		{
			diagramTypes.Add(reusableDiagramType.Name, reusableDiagramType);
			reusableDiagramType.SCU = sourceCodeUnit;
		}
		catch (ArgumentException)
		{
			reusableDiagramType = null;
		}
		return reusableDiagramType;
	}

	public FBType CreateFunctionBlockType(string name)
	{
		FBType fBType = new FBType(name);
		try
		{
			functionBlockTypes.Add(fBType.Name, fBType);
			fBType.SCU = sourceCodeUnit;
		}
		catch (ArgumentException)
		{
			fBType = null;
		}
		return fBType;
	}

	public ReusableProgType CreateProgramType(string name)
	{
		ReusableProgType reusableProgType = new ReusableProgType(name);
		try
		{
			programTypes.Add(reusableProgType.Name, reusableProgType);
			reusableProgType.SCU = sourceCodeUnit;
		}
		catch (ArgumentException)
		{
			reusableProgType = null;
		}
		return reusableProgType;
	}

	public ObjType FindObjType(string typeName, tObjTypeClass objTypeClass)
	{
		switch (objTypeClass)
		{
		case tObjTypeClass.DataTypeClass:
		{
			DataType value6 = null;
			dataTypes.TryGetValue(typeName, out value6);
			return value6;
		}
		case tObjTypeClass.CMTypeClass:
		{
			ReusableModType value5 = null;
			controlModuleTypes.TryGetValue(typeName, out value5);
			return value5;
		}
		case tObjTypeClass.DiagramTypeClass:
		{
			ReusableDiagramType value4 = null;
			diagramTypes.TryGetValue(typeName, out value4);
			return value4;
		}
		case tObjTypeClass.FBTypeClass:
		{
			FBType value3 = null;
			functionBlockTypes.TryGetValue(typeName, out value3);
			return value3;
		}
		case tObjTypeClass.ProgTypeClass:
		{
			ReusableProgType value2 = null;
			programTypes.TryGetValue(typeName, out value2);
			return value2;
		}
		case tObjTypeClass.FuncTypeClass:
		{
			FuncType value = null;
			functionTypes.TryGetValue(typeName, out value);
			return value;
		}
		default:
			return null;
		}
	}

	public void BuildTypeStruct()
	{
		foreach (KeyValuePair<string, DataType> dataType in dataTypes)
		{
			dataType.Value.BuildTypeStruct();
		}
		foreach (KeyValuePair<string, ReusableModType> controlModuleType in controlModuleTypes)
		{
			controlModuleType.Value.BuildTypeStruct();
		}
		foreach (KeyValuePair<string, ReusableDiagramType> diagramType in diagramTypes)
		{
			diagramType.Value.BuildTypeStruct();
		}
		foreach (KeyValuePair<string, FBType> functionBlockType in functionBlockTypes)
		{
			functionBlockType.Value.BuildTypeStruct();
		}
		foreach (KeyValuePair<string, ReusableProgType> programType in programTypes)
		{
			programType.Value.BuildTypeStruct();
		}
		foreach (KeyValuePair<string, FuncType> functionType in functionTypes)
		{
			functionType.Value.BuildTypeStruct();
		}
	}

	public void DeleteTypes()
	{
		dataTypes.Clear();
		controlModuleTypes.Clear();
		diagramTypes.Clear();
		functionBlockTypes.Clear();
		programTypes.Clear();
		functionTypes.Clear();
	}
}
