using StartValuesAnalyzer.ModApplic;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class AppSFDataTypes : AppSFObjTypes
{
	public override string ElementName => "DataTypes";

	public AppSFDataTypes(SAXElementBase parent, SourceCodeUnit sourceCodeUnit)
		: base(parent, sourceCodeUnit)
	{
	}

	protected override bool CorrectElementName(string elementName)
	{
		return elementName.Equals("DataType");
	}

	protected override SAXElementBase CreateChild()
	{
		return new AppSFDataType(this, base.SCU);
	}
}
