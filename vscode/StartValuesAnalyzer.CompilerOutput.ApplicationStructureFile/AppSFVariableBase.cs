using StartValuesAnalyzer.POU;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public abstract class AppSFVariableBase : SAXElementBase
{
	private readonly POUType pouType;

	private DataInst variable;

	public override string ElementName => "Var";

	protected POUType POUType => pouType;

	protected DataInst Variable
	{
		get
		{
			return variable;
		}
		set
		{
			variable = value;
		}
	}

	public AppSFVariableBase(SAXElementBase parent, POUType pouType)
		: base(parent)
	{
		this.pouType = pouType;
	}

	protected abstract DataInst CreateVariable(string name);

	public override bool StartElement(string elementName, SAXAttributes attributes)
	{
		if (!elementName.Equals("DataInst"))
		{
			ErrorHandler.ReportStartElementError(ElementName, elementName);
			return false;
		}
		if (base.Child == null)
		{
			base.Child = new AppSFDataInst(this, variable);
		}
		else
		{
			((AppSFDataInst)base.Child).DataInst = variable;
		}
		if (!base.Child.HandleAttributes(attributes))
		{
			if (!ErrorHandler.ErrorSet)
			{
				ErrorHandler.ReportAttributeError(elementName, attributes);
			}
			return false;
		}
		return true;
	}

	public override bool HandleAttributes(SAXAttributes attributes)
	{
		string attributeValue = "";
		attributes.GetAttribute("Name", ref attributeValue);
		variable = CreateVariable(attributeValue);
		if (variable == null)
		{
			ErrorHandler.ReportError(XMLErrorCode.XMLFailToCreate, "Variable (Name: " + attributeValue + ")");
			return false;
		}
		return true;
	}
}
