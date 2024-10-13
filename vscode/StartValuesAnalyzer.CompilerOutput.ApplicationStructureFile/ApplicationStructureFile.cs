using System;
using StartValuesAnalyzer.ModApplic;
using StartValuesAnalyzer.SAXParser;

namespace StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;

public class ApplicationStructureFile
{
	public static void UnpackFile(string path, ApplicationUnit application)
	{
		XMLUtility xMLUtility = new XMLUtility();
		StructureFileSAXContentHandler contentHandler = new StructureFileSAXContentHandler(application);
		try
		{
			xMLUtility.StartDecompressingSAXParser(contentHandler, path);
			application.SetUpLibRefsBetweenLibs();
			application.BuildAppAndLibs();
		}
		catch (Exception ex)
		{
			CleanUpApplication(application);
			throw ex;
		}
	}

	private static void CleanUpApplication(ApplicationUnit application)
	{
		application.DeleteRootMod();
		application.DeleteTypes();
		application.DeleteLibraryRefs();
	}
}
