using System;

namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CProgramDefinition
{
	private ushort m_NrOfPOUDefinitions;

	private CPOUDefinition[] m_POUDefinitions;

	public ushort GetNrOfPOUDefinitions => m_NrOfPOUDefinitions;

	public CProgramDefinition(ushort nrOfPOUDefinitions)
	{
		m_NrOfPOUDefinitions = nrOfPOUDefinitions;
		m_POUDefinitions = new CPOUDefinition[m_NrOfPOUDefinitions + 1];
	}

	public CPOUDefinition CreatePOUDefinition(ushort index, string name, ushort noOfParameters, ushort noOfVariables, ushort noOfLiterals, ushort noOfFBsAndProgs, ushort noOfMultiParameters, ushort noOfCompositeObjectVariables, ushort startOfSysGenVariables)
	{
		if (index <= 0 || index > m_NrOfPOUDefinitions || m_POUDefinitions[index] != null)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		CPOUDefinition cPOUDefinition = new CPOUDefinition(name, noOfParameters, noOfVariables, noOfLiterals, noOfFBsAndProgs, noOfMultiParameters, noOfCompositeObjectVariables, startOfSysGenVariables);
		m_POUDefinitions[index] = cPOUDefinition;
		return cPOUDefinition;
	}

	public CPOUDefinition GetPOUDefinition(ushort index)
	{
		if (index <= 0 || index > m_NrOfPOUDefinitions)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		return m_POUDefinitions[index];
	}
}
