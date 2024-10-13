namespace StartValuesAnalyzer.POU;

public class TypeRef
{
	private string typeName;

	private string typeSCUName;

	private ObjType typePtr;

	public string TypeName
	{
		get
		{
			return typeName;
		}
		set
		{
			typeName = value;
		}
	}

	public string TypeSCUName
	{
		get
		{
			return typeSCUName;
		}
		set
		{
			typeSCUName = value;
		}
	}

	public ObjType TypePtr
	{
		get
		{
			return typePtr;
		}
		set
		{
			typePtr = value;
		}
	}

	public TypeRef()
	{
		typeName = "";
		typeSCUName = "";
		typePtr = null;
	}
}
