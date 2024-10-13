using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using ProgressIndicator;
using StartValuesAnalyzer.AppRuntimeStructure;
using StartValuesAnalyzer.Communication;
using StartValuesAnalyzer.CompilerOutput.ApplicationStructureFile;
using StartValuesAnalyzer.ModApplic;

namespace StartValuesAnalyzer;

public class Form1 : Form
{
	private ApplicationUnit m_Application;

	private CProgramInstance m_ProgramInststance;

	private global::ProgressIndicator.ProgressIndicator m_progressInd = new global::ProgressIndicator.ProgressIndicator();

	private string m_directoryPath;

	private StreamWriter m_sessionLogWriter;

	private IContainer components;

	private RichTextBox txtReport;

	private MenuStrip menuStrip1;

	private ToolStripMenuItem selectApplicationCdoXmlFilesToolStripMenuItem;

	private ToolStripMenuItem settingsToolStripMenuItem;

	private ToolStripMenuItem setMaxNrOfOPCItemsAtATimetemsToolStripMenuItem;

	private ToolStripMenuItem compareToolStripMenuItem;

	private ToolStripMenuItem filesToolStripMenuItem;

	private ToolStripMenuItem selectApplicationsToolStripMenuItem;

	public string DirectoryPath => m_directoryPath;

	public Form1()
	{
		InitializeComponent();
	}

	private void selectApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
	{
		using OpenFileDialog openFileDialog = new OpenFileDialog();
		if (Directory.Exists("C:\\ABB Industrial IT Data\\Control IT Data\\OPC Server for AC 800M\\Files"))
		{
			openFileDialog.InitialDirectory = "C:\\ABB Industrial IT Data\\Control IT Data \\OPC Server for AC 800M\\Files";
		}
		openFileDialog.Title = "Select Application(s)";
		openFileDialog.DefaultExt = "cdoXML";
		openFileDialog.Filter = "Application domain (*.cdoXML)|*.cdoXML";
		openFileDialog.Multiselect = true;
		if (openFileDialog.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
		CreateWorkingFolder("StartValuesData");
		CreateSessionLogFile();
		ClearActivityReport();
		StartProgressIndicator();
		ReportActivity(openFileDialog.FileNames.Length + " Application file(s) selected", showFullDTtext: true);
		string[] fileNames = openFileDialog.FileNames;
		foreach (string text in fileNames)
		{
			SetProgressLargeText(Path.GetFileName(text));
			if (ParseApplication(text))
			{
				CollectRetainValuesCommunicateAndPrintToFiles();
			}
		}
		StopProgressIndicator();
		ReportActivity("All selected Application files are completed ", showFullDTtext: false);
		CloseSessionLogFile();
	}

	private bool ParseApplication(string cdoFileName)
	{
		bool result = false;
		CParseAppDomain cParseAppDomain = new CParseAppDomain();
		try
		{
			ReportActivity("Parse " + Path.GetFileName(cdoFileName), showFullDTtext: false);
			m_ProgramInststance = cParseAppDomain.Parse(cdoFileName);
		}
		catch (Exception ex)
		{
			m_ProgramInststance = null;
			MessageBox.Show("Parse " + ex.Message, "Failed to parse domain file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			ReportActivity("Failed to parse domain file. Reason: " + ex.Message, showFullDTtext: false);
		}
		if (m_ProgramInststance != null)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(cdoFileName);
			string path = Path.Combine(Path.GetDirectoryName(cdoFileName), fileNameWithoutExtension + ".rrsXml");
			m_Application = new ApplicationUnit(fileNameWithoutExtension);
			try
			{
				ReportActivity("Parse " + Path.GetFileName(path), showFullDTtext: false);
				ApplicationStructureFile.UnpackFile(path, m_Application);
				m_Application.ProgramInstance = m_ProgramInststance;
				result = true;
			}
			catch (Exception ex2)
			{
				m_ProgramInststance = null;
				m_Application = null;
				MessageBox.Show("Parse " + ex2.Message, "Failed to parse structure file.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				ReportActivity("Failed to parse structure file. Reason: " + ex2.Message, showFullDTtext: false);
			}
		}
		return result;
	}

	private void CollectRetainValuesCommunicateAndPrintToFiles()
	{
		try
		{
			List<RetainProperties> list = new List<RetainProperties>();
			ReportActivity("Get retain properties", showFullDTtext: false);
			m_Application.GetRetainProperties(list);
			List<RetainProperties> list2 = new List<RetainProperties>();
			List<RetainProperties> list3 = new List<RetainProperties>();
			foreach (RetainProperties item in list)
			{
				if (item.ValType == tValType.StringType)
				{
					list3.Add(item);
				}
				else
				{
					list2.Add(item);
				}
			}
			ReportActivity("Connect to OPC Server. Communicate " + list.Count + " items (" + list3.Count + " strings)", showFullDTtext: false);
			OpcComm opcComm = new OpcComm();
			opcComm.ActivityReport += OPCActivityReport;
			int nrOfGoodItems = 0;
			int nrOfBadItems = 0;
			int nrOfUncertainItems = 0;
			opcComm.ConnectAndReadItems(list2, isStringItems: false, OpcComm.MaxNrToCommunicateAtATime, ref nrOfGoodItems, ref nrOfBadItems, ref nrOfUncertainItems);
			opcComm.ConnectAndReadItems(list3, isStringItems: true, OpcComm.MaxNrStringsToCommunicateAtATime, ref nrOfGoodItems, ref nrOfBadItems, ref nrOfUncertainItems);
			ReportActivity("NrGood=" + nrOfGoodItems + " NrBad=" + nrOfBadItems + " NrUncertain=" + nrOfUncertainItems, showFullDTtext: false);
			if (nrOfBadItems > 0)
			{
				ReportActivity("Warning. Bad quality received!", showFullDTtext: false);
			}
			opcComm.ActivityReport -= OPCActivityReport;
			ReportActivity("Write values to files", showFullDTtext: false);
			WriteToFile(list, writeCRFile: true, writeDiffValuesOnly: false);
			WriteToFile(list, writeCRFile: false, writeDiffValuesOnly: false);
			WriteToFile(list, writeCRFile: true, writeDiffValuesOnly: true);
			WriteToFile(list, writeCRFile: false, writeDiffValuesOnly: true);
			WriteCRCandidatesToFile(list);
		}
		catch (Exception ex)
		{
			ReportActivity("Failed. Reason: " + ex.Message.Replace("\r\n", " "), showFullDTtext: false);
		}
	}

	private void OPCActivityReport(string msgText)
	{
		ReportActivity(msgText, showFullDTtext: false);
	}

	private bool ValuesEqual(string initVal, string commVal, tValType valType)
	{
		bool flag = false;
		if (valType == tValType.RealType && commVal.Length > 0)
		{
			try
			{
				return XmlConvert.ToDouble(initVal.Replace(',', '.')) == XmlConvert.ToDouble(commVal.Replace(',', '.'));
			}
			catch
			{
				return initVal == commVal;
			}
		}
		return initVal == commVal;
	}

	private string BuildFileName(bool writeCRFile, bool writeDiffValuesOnly)
	{
		return Path.Combine(path2: writeCRFile ? ((!writeDiffValuesOnly) ? (m_Application.Name + "CRValues.txt") : (m_Application.Name + "DiffCRValues.txt")) : ((!writeDiffValuesOnly) ? (m_Application.Name + "RValues.txt") : (m_Application.Name + "DiffRValues.txt")), path1: DirectoryPath);
	}

	private void WriteToFile(List<RetainProperties> retainProperties, bool writeCRFile, bool writeDiffValuesOnly)
	{
		string path = BuildFileName(writeCRFile, writeDiffValuesOnly);
		using FileStream stream = File.Create(path);
		using StreamWriter streamWriter = new StreamWriter(stream);
		streamWriter.WriteLine("Path\tInitalValue\tCommValue\tQuality\tType");
		foreach (RetainProperties retainProperty in retainProperties)
		{
			if ((writeCRFile && retainProperty.ColdRetain) || (!writeCRFile && retainProperty.Retain && !retainProperty.ColdRetain))
			{
				string text = retainProperty.InitValue.Trim();
				string text2 = retainProperty.CommunicatedValue.Trim();
				bool flag = true;
				bool flag2 = ValuesEqual(text, text2, retainProperty.ValType);
				if (flag2 && retainProperty.ValType == tValType.RealType)
				{
					text = text2;
				}
				if (writeDiffValuesOnly)
				{
					flag = !flag2;
				}
				if (flag)
				{
					streamWriter.WriteLine(retainProperty.Path + "\t" + text + "\t" + text2 + "\t" + retainProperty.Quality + "\t" + retainProperty.ValTypeAsString);
				}
			}
		}
	}

	private string GetCRCandidatesFileName()
	{
		string path = m_Application.Name + "CRCandidates.txt";
		return Path.Combine(DirectoryPath, path);
	}

	private void WriteCRCandidatesToFile(List<RetainProperties> retainProperties)
	{
		string cRCandidatesFileName = GetCRCandidatesFileName();
		using FileStream stream = File.Create(cRCandidatesFileName);
		using StreamWriter streamWriter = new StreamWriter(stream);
		streamWriter.WriteLine("Path\tInitalValue\tCommValue\tQuality\tType");
		foreach (RetainProperties retainProperty in retainProperties)
		{
			string text = retainProperty.InitValue.Trim();
			string text2 = retainProperty.CommunicatedValue.Trim();
			bool flag = ValuesEqual(text, text2, retainProperty.ValType);
			if (retainProperty.Retain && !retainProperty.ColdRetain && retainProperty.Read && !retainProperty.Written && !flag)
			{
				if (flag && retainProperty.ValType == tValType.RealType)
				{
					text = text2;
				}
				streamWriter.WriteLine(retainProperty.Path + "\t" + text + "\t" + text2 + "\t" + retainProperty.Quality + "\t" + retainProperty.ValTypeAsString);
			}
		}
	}

	private void setMaxNrOfOPCItemsAtATimetemsToolStripMenuItem_Click(object sender, EventArgs e)
	{
		using MaxNrOPCItems maxNrOPCItems = new MaxNrOPCItems();
		maxNrOPCItems.MaxNrOfNonStringItems = OpcComm.MaxNrToCommunicateAtATime;
		maxNrOPCItems.MaxNrOfStringItems = OpcComm.MaxNrStringsToCommunicateAtATime;
		maxNrOPCItems.NrOfOPCRetries = OpcComm.MaxNrOPCCallRetries;
		if (maxNrOPCItems.ShowDialog(this) == DialogResult.OK)
		{
			OpcComm.MaxNrToCommunicateAtATime = maxNrOPCItems.MaxNrOfNonStringItems;
			OpcComm.MaxNrStringsToCommunicateAtATime = maxNrOPCItems.MaxNrOfStringItems;
			OpcComm.MaxNrOPCCallRetries = maxNrOPCItems.NrOfOPCRetries;
		}
	}

	public void ReportActivity(string txtStr, bool showFullDTtext)
	{
		m_progressInd.SmallText = txtStr;
		string text = ((!showFullDTtext) ? $"{DateTime.Now:HH.mm.ss.fff}" : $"{DateTime.Now:yyyy-MM-dd HH.mm.ss.fff}");
		RichTextBox richTextBox = txtReport;
		object obj = richTextBox.Text;
		richTextBox.Text = string.Concat(obj, text, "  ", txtStr, '\n');
		if (m_sessionLogWriter != null)
		{
			m_sessionLogWriter.WriteLine(text + "  " + txtStr);
		}
		Refresh();
	}

	private void ClearActivityReport()
	{
		txtReport.Text = "";
		Refresh();
	}

	private void SetProgressLargeText(string appName)
	{
		m_progressInd.LargeText = appName.Remove(appName.IndexOf('.'));
	}

	private void StartProgressIndicator()
	{
		m_progressInd.StartDelay = 2;
		m_progressInd.ReleaseTime = 1;
		m_progressInd.Style = global::ProgressIndicator.ProgressIndicator.eStyle.Automatic;
		m_progressInd.Start(base.Location, base.Size);
		m_progressInd.RemainingTime = "";
	}

	private void StopProgressIndicator()
	{
		m_progressInd.Stop();
	}

	private void CreateWorkingFolder(string shortFolderName)
	{
		string text = $"{DateTime.Now:yyyy-MM-dd HH.mm.ss.fff}";
		m_directoryPath = Path.Combine(Application.StartupPath, shortFolderName + "_" + text);
		Directory.CreateDirectory(m_directoryPath);
	}

	private void CreateSessionLogFile()
	{
		string path = Path.Combine(DirectoryPath, "SessionLog.txt");
		m_sessionLogWriter = new StreamWriter(path);
	}

	private void CloseSessionLogFile()
	{
		if (m_sessionLogWriter != null)
		{
			((IDisposable)m_sessionLogWriter).Dispose();
		}
	}

	private void Form1_Load(object sender, EventArgs e)
	{
	}

	private void filesToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			CreateWorkingFolder("ComparedValues");
			CreateSessionLogFile();
			ClearActivityReport();
			ReportActivity("Compare file(s): ", showFullDTtext: true);
			Dictionary<string, string> folder1Map = SelectFilesFromFolder(1);
			Dictionary<string, string> folder2Map = SelectFilesFromFolder(2);
			Dictionary<string, KeyValuePair<string, string>> filesInCommon = null;
			Dictionary<string, string> filesNotInCommon = null;
			StartProgressIndicator();
			CompareFolderMapContents(folder1Map, folder2Map, out filesInCommon, out filesNotInCommon);
			foreach (KeyValuePair<string, KeyValuePair<string, string>> item in filesInCommon)
			{
				KeyValuePair<string, string> value = item.Value;
				ReportActivity("Compare file: " + Path.Combine(value.Key, item.Key) + " with file: " + Path.Combine(value.Value, item.Key), showFullDTtext: false);
				SetProgressLargeText("compare files " + item.Key);
				if (value.Key == value.Value)
				{
					ReportActivity("Warning. The file is compared with itself. You are supposed to select files from different folders", showFullDTtext: false);
				}
				if (item.Key != "SessionLog.txt")
				{
					CompareTwoFiles(Path.Combine(value.Key, item.Key), Path.Combine(value.Value, item.Key));
				}
			}
			foreach (KeyValuePair<string, string> item2 in filesNotInCommon)
			{
				if (item2.Key != "SessionLog.txt")
				{
					ReportActivity("This file couldn't be compared because the file only exists in one of the two selection sets. File: " + Path.Combine(item2.Value, item2.Key), showFullDTtext: false);
				}
			}
		}
		catch (Exception ex)
		{
			ReportActivity("Failed. Reason: " + ex.Message.Replace("\r\n", " "), showFullDTtext: false);
		}
		finally
		{
			StopProgressIndicator();
			CloseSessionLogFile();
		}
	}

	private void CompareFolderMapContents(Dictionary<string, string> folder1Map, Dictionary<string, string> folder2Map, out Dictionary<string, KeyValuePair<string, string>> filesInCommon, out Dictionary<string, string> filesNotInCommon)
	{
		string value = null;
		filesInCommon = new Dictionary<string, KeyValuePair<string, string>>();
		filesNotInCommon = new Dictionary<string, string>();
		foreach (KeyValuePair<string, string> item in folder1Map)
		{
			if (folder2Map.TryGetValue(item.Key, out value))
			{
				filesInCommon.Add(item.Key, new KeyValuePair<string, string>(item.Value, value));
			}
			else
			{
				filesNotInCommon.Add(item.Key, item.Value);
			}
		}
		foreach (KeyValuePair<string, string> item2 in folder2Map)
		{
			if (!filesInCommon.TryGetValue(item2.Key, out var _))
			{
				filesNotInCommon.Add(item2.Key, item2.Value);
			}
		}
	}

	private void CompareTwoFiles(string filePath1, string filePath2)
	{
		if (Path.GetFileName(filePath1) != Path.GetFileName(filePath2))
		{
			throw new Exception("The filenames are not equal. Name1 = " + Path.GetFileName(filePath1) + ". Name2 = " + Path.GetFileName(filePath2));
		}
		Dictionary<string, CompareFilesValues> theMap = new Dictionary<string, CompareFilesValues>();
		ReadCompareFile(isFile1: true, filePath1, theMap);
		ReadCompareFile(isFile1: false, filePath2, theMap);
		WriteComparedResultFile(filePath1, filePath2, Path.GetFileNameWithoutExtension(filePath1) + "Compared.txt", theMap);
		ReportActivity("Compare " + Path.GetFileName(filePath1) + " files succeeded ", showFullDTtext: false);
	}

	private void WriteComparedResultFile(string filePath1, string filePath2, string outFileName, Dictionary<string, CompareFilesValues> theMap)
	{
		using StreamWriter streamWriter = new StreamWriter(Path.Combine(DirectoryPath, outFileName));
		streamWriter.WriteLine("File1=" + filePath1);
		streamWriter.WriteLine("File2=" + filePath2);
		streamWriter.WriteLine();
		streamWriter.WriteLine("The following differences have been found:");
		streamWriter.WriteLine();
		streamWriter.WriteLine("Path\tInitalValue1\tCommValue1\tQuality1\tType1\tXXXXX\tInitalValue2\tCommValue2\tQuality2\tType2");
		foreach (KeyValuePair<string, CompareFilesValues> item in theMap)
		{
			if (!item.Value.EqualValues())
			{
				streamWriter.WriteLine(item.Key + item.Value.Value1 + "\tXXXXX" + item.Value.Value2);
			}
		}
	}

	private Dictionary<string, string> SelectFilesFromFolder(int folderNr)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		using (OpenFileDialog openFileDialog = new OpenFileDialog())
		{
			if (folderNr == 1)
			{
				openFileDialog.Title = "Select File(s) from the first folder";
			}
			else
			{
				openFileDialog.Title = "Select File(s) from the second folder";
			}
			openFileDialog.InitialDirectory = Application.StartupPath;
			openFileDialog.DefaultExt = "*.txt";
			openFileDialog.Filter = "Startvalues (*.txt)|*.txt";
			openFileDialog.Multiselect = true;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string[] fileNames = openFileDialog.FileNames;
				foreach (string path in fileNames)
				{
					dictionary.Add(Path.GetFileName(path), Path.GetDirectoryName(path));
				}
			}
		}
		return dictionary;
	}

	private void ReadCompareFile(bool isFile1, string filePath, Dictionary<string, CompareFilesValues> theMap)
	{
		using StreamReader streamReader = new StreamReader(filePath);
		string text = null;
		string path = null;
		string theValue = null;
		for (text = streamReader.ReadLine(); text != null; text = streamReader.ReadLine())
		{
			ExtractPathAndValuefromLine(text, out path, out theValue);
			if (path.Length > 0)
			{
				CompareFilesValues value = null;
				if (theMap.TryGetValue(path, out value))
				{
					if (isFile1)
					{
						value.Value1 = theValue;
					}
					else
					{
						value.Value2 = theValue;
					}
				}
				else
				{
					theMap.Add(value: (!isFile1) ? new CompareFilesValues("\t\t\t\t", theValue) : new CompareFilesValues(theValue, "\t\t\t\t"), key: path);
				}
			}
		}
	}

	private void ExtractPathAndValuefromLine(string lineStr, out string path, out string theValue)
	{
		path = "";
		theValue = "";
		string text = "Applications.";
		int num = lineStr.IndexOf(text);
		if (num >= 0)
		{
			int num2 = lineStr.IndexOfAny(new char[2] { '\t', ' ' }, num + text.Length);
			if (num2 >= num + text.Length)
			{
				theValue = lineStr.Substring(num2);
				path = lineStr.Substring(0, num2);
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
            this.txtReport = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.selectApplicationCdoXmlFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectApplicationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setMaxNrOfOPCItemsAtATimetemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtReport
            // 
            this.txtReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReport.Location = new System.Drawing.Point(0, 24);
            this.txtReport.Name = "txtReport";
            this.txtReport.ReadOnly = true;
            this.txtReport.Size = new System.Drawing.Size(374, 207);
            this.txtReport.TabIndex = 4;
            this.txtReport.Text = "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectApplicationCdoXmlFilesToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.compareToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(374, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // selectApplicationCdoXmlFilesToolStripMenuItem
            // 
            this.selectApplicationCdoXmlFilesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectApplicationsToolStripMenuItem});
            this.selectApplicationCdoXmlFilesToolStripMenuItem.Name = "selectApplicationCdoXmlFilesToolStripMenuItem";
            this.selectApplicationCdoXmlFilesToolStripMenuItem.Size = new System.Drawing.Size(192, 20);
            this.selectApplicationCdoXmlFilesToolStripMenuItem.Text = "Collect Values From Controller(s)";
            // 
            // selectApplicationsToolStripMenuItem
            // 
            this.selectApplicationsToolStripMenuItem.Name = "selectApplicationsToolStripMenuItem";
            this.selectApplicationsToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
            this.selectApplicationsToolStripMenuItem.Text = "Select Application(s)";
            this.selectApplicationsToolStripMenuItem.Click += new System.EventHandler(this.selectApplicationsToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setMaxNrOfOPCItemsAtATimetemsToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // setMaxNrOfOPCItemsAtATimetemsToolStripMenuItem
            // 
            this.setMaxNrOfOPCItemsAtATimetemsToolStripMenuItem.Name = "setMaxNrOfOPCItemsAtATimetemsToolStripMenuItem";
            this.setMaxNrOfOPCItemsAtATimetemsToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.setMaxNrOfOPCItemsAtATimetemsToolStripMenuItem.Text = "Data Collection";
            this.setMaxNrOfOPCItemsAtATimetemsToolStripMenuItem.Click += new System.EventHandler(this.setMaxNrOfOPCItemsAtATimetemsToolStripMenuItem_Click);
            // 
            // compareToolStripMenuItem
            // 
            this.compareToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filesToolStripMenuItem});
            this.compareToolStripMenuItem.Name = "compareToolStripMenuItem";
            this.compareToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.compareToolStripMenuItem.Text = "Compare";
            // 
            // filesToolStripMenuItem
            // 
            this.filesToolStripMenuItem.Name = "filesToolStripMenuItem";
            this.filesToolStripMenuItem.Size = new System.Drawing.Size(97, 22);
            this.filesToolStripMenuItem.Text = "Files";
            this.filesToolStripMenuItem.Click += new System.EventHandler(this.filesToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 231);
            this.Controls.Add(this.txtReport);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Start Values Analyzer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

	}
}
