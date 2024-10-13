using StartValuesAnalyzer.AppRuntimeStructure;

namespace StartValuesAnalyzer.POU;

public class ArrayComponent : DataInst
{
	public ArrayComponent()
		: base("")
	{
	}

	protected override void GetMemRef(ref tMemReference memRef, ushort externalOffset)
	{
	}
}
