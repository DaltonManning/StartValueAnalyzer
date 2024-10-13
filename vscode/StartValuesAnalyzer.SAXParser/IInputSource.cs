using System.IO;

namespace StartValuesAnalyzer.SAXParser;

public interface IInputSource
{
	string Source { get; }

	Stream MakeStream();
}
