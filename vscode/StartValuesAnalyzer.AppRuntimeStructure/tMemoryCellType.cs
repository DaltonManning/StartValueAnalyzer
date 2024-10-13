namespace StartValuesAnalyzer.AppRuntimeStructure;

public class tMemoryCellType
{
	private tMemoryCellSimpleType simpleType;

	private bool coldRetain;

	private bool retain;

	private bool constant;

	private bool noSort;

	private bool inParameter;

	private bool outParameter;

	private bool inPicture;

	public bool ColdRetain
	{
		get
		{
			return coldRetain;
		}
		set
		{
			coldRetain = value;
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

	public bool InParameter
	{
		get
		{
			return inParameter;
		}
		set
		{
			inParameter = value;
		}
	}

	public bool OutParameter
	{
		get
		{
			return outParameter;
		}
		set
		{
			outParameter = value;
		}
	}

	public bool InPicture
	{
		get
		{
			return inPicture;
		}
		set
		{
			inPicture = value;
		}
	}

	public tMemoryCellType(tMemoryCellSimpleType simpleType)
	{
		this.simpleType = simpleType;
		coldRetain = false;
		retain = false;
		constant = false;
		noSort = false;
		inParameter = false;
		outParameter = false;
		inPicture = false;
	}

	public tMemoryCellSimpleType GetSimpleType()
	{
		return simpleType;
	}

	public bool IsType(tMemoryCellSimpleType asimpleType)
	{
		return simpleType == asimpleType;
	}

	public bool IsString()
	{
		return simpleType == tMemoryCellSimpleType.StringMCType;
	}

	public bool IsConstantString()
	{
		if (Constant)
		{
			return IsString();
		}
		return false;
	}
}
