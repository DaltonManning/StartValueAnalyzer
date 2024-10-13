using System.IO;

namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CProgramMemory
{
	private CProgramDefinition m_ProgramDefinition;

	private ushort m_NrOfPOUInstances;

	private ulong m_TotalColdRetainSize;

	private CPOUInstance[] m_POUInstances;

	public CProgramMemory(ushort nrOfInstances, CProgramDefinition pProgramDefinition, ulong totalColdRetainSize)
	{
		m_NrOfPOUInstances = nrOfInstances;
		m_ProgramDefinition = pProgramDefinition;
		m_TotalColdRetainSize = totalColdRetainSize;
		m_POUInstances = new CPOUInstance[(ushort)(nrOfInstances + 1)];
	}

	public CProgramDefinition GetProgramDefinition()
	{
		return m_ProgramDefinition;
	}

	public CPOUInstance CreatePOUInstance(ushort POUInstanceNr, ushort DefinitionNr, ushort MultiSize)
	{
		CPOUDefinition pOUDefinition = m_ProgramDefinition.GetPOUDefinition(DefinitionNr);
		CPOUInstance cPOUInstance = new CPOUInstance(pOUDefinition, POUInstanceNr, MultiSize, DefinitionNr);
		m_POUInstances[POUInstanceNr] = cPOUInstance;
		return cPOUInstance;
	}

	public CPOUInstance GetPOUInstance(ushort POUInstanceNr)
	{
		return m_POUInstances[POUInstanceNr];
	}

	public void PrintProgMemToFile(StreamWriter sw)
	{
		for (int i = 0; i < m_POUInstances.Length; i++)
		{
			m_POUInstances[i]?.PrintPOUInstance(sw);
		}
	}

	public ushort GetNrOfPOUInstances()
	{
		return m_NrOfPOUInstances;
	}

	public ulong GetTotalColdRetainSize()
	{
		return m_TotalColdRetainSize;
	}
}
