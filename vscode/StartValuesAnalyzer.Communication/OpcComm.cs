using System;
using System.Collections.Generic;
using Opc;
using Opc.Da;
using OpcCom;
using StartValuesAnalyzer.AppRuntimeStructure;

namespace StartValuesAnalyzer.Communication;

internal class OpcComm
{
	private Opc.Da.Server m_server;

	private static int m_MaxNrToCommunicateAtATime = 5000;

	private static int m_MaxNrStringsToCommunicateAtATime = 1500;

	private static int m_MaxNrOPCCallRetries = 1;

	public static int MaxNrToCommunicateAtATime
	{
		get
		{
			return m_MaxNrToCommunicateAtATime;
		}
		set
		{
			m_MaxNrToCommunicateAtATime = value;
		}
	}

	public static int MaxNrStringsToCommunicateAtATime
	{
		get
		{
			return m_MaxNrStringsToCommunicateAtATime;
		}
		set
		{
			m_MaxNrStringsToCommunicateAtATime = value;
		}
	}

	public static int MaxNrOPCCallRetries
	{
		get
		{
			return m_MaxNrOPCCallRetries;
		}
		set
		{
			m_MaxNrOPCCallRetries = value;
		}
	}

	public event OPCActivityReportEventHandler ActivityReport;

	protected virtual void OnActivityReport(string activityText)
	{
		if (this.ActivityReport != null)
		{
			this.ActivityReport(activityText);
		}
	}

	private static void UpdateQualityCounters(qualityBits q, ref int nrOfGoodItems, ref int nrOfBadItems, ref int nrOfUncertainItems)
	{
		switch (q)
		{
		case qualityBits.good:
		case qualityBits.goodLocalOverride:
			nrOfGoodItems++;
			break;
		case qualityBits.bad:
		case qualityBits.badConfigurationError:
		case qualityBits.badNotConnected:
		case qualityBits.badDeviceFailure:
		case qualityBits.badSensorFailure:
		case qualityBits.badLastKnownValue:
		case qualityBits.badCommFailure:
		case qualityBits.badOutOfService:
		case qualityBits.badWaitingForInitialData:
			nrOfBadItems++;
			break;
		default:
			nrOfUncertainItems++;
			break;
		}
	}

	public void ConnectAndReadItems(List<RetainProperties> retainProperties, bool isStringItems, int MaxNrItemsToCommunicateAtATime, ref int nrOfGoodItems, ref int nrOfBadItems, ref int nrOfUncertainItems)
	{
		if (retainProperties.Count == 0)
		{
			return;
		}
		URL url = new URL("opcda://localhost/ABB.AC800MC_OpcDaServer/{68aec2ca-93cd-11d1-94e1-0020afc84400}");
		m_server = new Opc.Da.Server(new OpcCom.Factory(), url);
		m_server.Connect();
		Item[] array = new Item[retainProperties.Count];
		for (int i = 0; i < retainProperties.Count; i++)
		{
			Item item = new Item(new ItemIdentifier(retainProperties[i].Path));
			item.ClientHandle = i;
			array[i] = item;
		}
		int num = retainProperties.Count / MaxNrItemsToCommunicateAtATime;
		if (retainProperties.Count % MaxNrItemsToCommunicateAtATime > 0)
		{
			num++;
		}
		int num2 = retainProperties.Count;
		int num3 = 0;
		for (int j = 0; j < num; j++)
		{
			int num4 = ((num2 < MaxNrItemsToCommunicateAtATime) ? num2 : MaxNrItemsToCommunicateAtATime);
			num2 -= num4;
			Item[] array2 = new Item[num4];
			for (int k = 0; k < num4; k++)
			{
				array2[k] = array[num3++];
			}
			ItemValueResult[] array3 = null;
			bool flag = false;
			int num5 = 1 + MaxNrOPCCallRetries;
			for (int l = 1; l <= num5; l++)
			{
				if (flag)
				{
					break;
				}
				try
				{
					if (isStringItems)
					{
						OnActivityReport("Call OPC read sync. Read " + array2.Length + " string items");
					}
					else
					{
						OnActivityReport("Call OPC read sync. Read " + array2.Length + " non string items");
					}
					array3 = m_server.Read(array2);
					flag = true;
				}
				catch (Exception ex)
				{
					if (l == num5)
					{
						throw ex;
					}
					OnActivityReport("OPC Call Failed. Reason: " + ex.Message.Replace("\r\n", " "));
					OnActivityReport("Retry OPC call");
				}
			}
			if (array3 == null)
			{
				continue;
			}
			ItemValueResult[] array4 = array3;
			foreach (ItemValueResult itemValueResult in array4)
			{
				RetainProperties retainProperties2 = retainProperties[(int)itemValueResult.ClientHandle];
				UpdateQualityCounters(itemValueResult.Quality.QualityBits, ref nrOfGoodItems, ref nrOfBadItems, ref nrOfUncertainItems);
				string theValue = "";
				if (itemValueResult.Value != null)
				{
					if (itemValueResult.Value is DateTime)
					{
						DateTime dateTime = (DateTime)itemValueResult.Value;
						theValue = $"{dateTime:yyyy-MM-dd HH:mm:ss}";
					}
					else
					{
						theValue = itemValueResult.Value.ToString();
					}
				}
				retainProperties2.SetCommunicatedProperties(itemValueResult.Quality.ToString(), theValue);
			}
		}
		m_server.Disconnect();
	}
}
