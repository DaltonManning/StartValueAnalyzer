using StartValuesAnalyzer.ModApplic;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class StructureFileSAXContentHandler : SAXContentHandlerBase
{
	public StructureFileSAXContentHandler(ApplicationUnit application)
		: base(new AppSFFileRoot(application))
	{
	}
}
