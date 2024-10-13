using System;
using System.IO;

namespace StartValuesAnalyzer.AppRuntimeStructure;

public class CPOUInstance
{
	private ushort m_InstanceNr;

	private ushort m_MultiSize;

	private CPOUDefinition m_pPOUDefinition;

	private ushort m_DefinitionNr;

	private CMemoryCell[] m_MemoryCells;

	public ushort MultiSize
	{
		get
		{
			return m_MultiSize;
		}
		set
		{
			m_MultiSize = value;
		}
	}

	public CPOUDefinition POUDefinition => m_pPOUDefinition;

	public ushort InstanceNr => m_InstanceNr;

	public CPOUInstance(CPOUDefinition pPOUDefinition, ushort InstanceNr, ushort MultiSize, ushort DefinitionNr)
	{
		m_pPOUDefinition = pPOUDefinition;
		m_InstanceNr = InstanceNr;
		m_MultiSize = MultiSize;
		m_DefinitionNr = DefinitionNr;
		m_MemoryCells = new CMemoryCell[pPOUDefinition.GetNrOfMemoryCells(MultiSize) + 1];
	}

	public CMemoryCell GetMemoryCell(ushort index)
	{
		return m_MemoryCells[index];
	}

	public void SetMemoryCell(ushort index, CMemoryCell memCell)
	{
		if (m_MemoryCells[index] != null)
		{
			throw new ArgumentOutOfRangeException("SetMemoryCell");
		}
		m_MemoryCells[index] = memCell;
	}

	public void PrintPOUInstance(StreamWriter sw)
	{
		sw.Write("INSTANCE   " + m_InstanceNr);
		if (m_MultiSize > 0)
		{
			sw.Write(" MultiSize = " + m_MultiSize);
		}
		sw.WriteLine();
		sw.WriteLine("DEFINITION " + m_DefinitionNr + "  " + m_pPOUDefinition.Name);
		sw.WriteLine("-------------------------------------------");
		string text = "   ";
		for (int i = 0; i < m_pPOUDefinition.GetNrOfParameters; i += 2)
		{
			if (i == 0)
			{
				sw.WriteLine(text + "   --Parameters--------------------");
			}
			sw.WriteLine(text + (i + 1) + " " + m_MemoryCells[i].ToString());
		}
		for (int j = m_pPOUDefinition.GetNrOfParameters + 1; j <= m_pPOUDefinition.GetNrOfVariables + m_pPOUDefinition.GetNrOfParameters; j++)
		{
			if (j == m_pPOUDefinition.GetNrOfParameters + 1)
			{
				sw.WriteLine(text + "   --User defined variables--------");
			}
			ushort index = (ushort)(j - m_pPOUDefinition.GetNrOfParameters - 1);
			tMemoryCellType memCellType = m_pPOUDefinition.GetTypeArray.GetMemCellType(index);
			string text2 = ((!memCellType.ColdRetain) ? ((!memCellType.Retain) ? "-----" : "---R") : "---CR");
			memCellType.GetSimpleType();
			string text3 = m_pPOUDefinition.GetInitValueTable.GetMemoryCell(index).ToString();
			sw.WriteLine(text + j + " " + memCellType.GetSimpleType().ToString() + "  V=" + text3 + "  " + text2);
		}
		sw.WriteLine();
	}
}
