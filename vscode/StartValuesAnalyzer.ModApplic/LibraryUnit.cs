namespace StartValuesAnalyzer.ModApplic;

public class LibraryUnit : SourceCodeUnit
{
	public LibraryUnit(string name)
		: base(name)
	{
	}

	public void DeleteLibraryReference(LibraryUnit library)
	{
		if (library != null)
		{
			libraries.Remove(library.Name);
		}
	}
}
