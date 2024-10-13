namespace StartValuesAnalyzer.POU;

public class ArrayType : DataType
{
	protected ArrayComponent component;

	protected int firstIndex;

	protected int lastIndex;

	public int FirstIndex
	{
		set
		{
			firstIndex = value;
		}
	}

	public int LastIndex
	{
		set
		{
			lastIndex = value;
		}
	}

	public ArrayType(string name)
		: base(name)
	{
		component = new ArrayComponent();
		firstIndex = 0;
		lastIndex = 0;
	}

	public ArrayComponent CreateArrayComponent()
	{
		component = new ArrayComponent();
		component.Father = this;
		firstIndex = 0;
		lastIndex = 0;
		return component;
	}
}
