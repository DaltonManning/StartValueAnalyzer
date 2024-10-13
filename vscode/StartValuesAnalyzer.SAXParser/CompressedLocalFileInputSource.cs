using System.IO;

namespace StartValuesAnalyzer.SAXParser;

public class CompressedLocalFileInputSource : IInputSource
{
	private string fileName;

	public virtual string Source => fileName;

	public CompressedLocalFileInputSource(string fileName)
	{
		this.fileName = fileName;
	}

	public virtual Stream MakeStream()
	{
		return new CompressedFileStream(fileName);
	}
}
