using StartValuesAnalyzer.ModApplic;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public abstract class AppSFApplicationBase : SAXElementBase
{
	private readonly ApplicationUnit application;

	public ApplicationUnit Application => application;

	public AppSFApplicationBase(ApplicationUnit application)
	{
		this.application = application;
	}

	public AppSFApplicationBase(SAXElementBase parent, ApplicationUnit application)
		: base(parent)
	{
		this.application = application;
	}
}
