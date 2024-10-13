using System;
using System.Drawing;
using System.Threading;

namespace ProgressIndicator;

public class ProgressIndicator
{
	public enum eStyle
	{
		Automatic,
		Manual
	}

	private enum eState
	{
		Starting,
		Running,
		Stopping
	}

	private eStyle m_Style;

	private eState m_state;

	private int m_StartDelay = 2;

	private int m_ReleaseTime = 1;

	private string m_TitleText = "";

	private string m_LargeText = "";

	private string m_SmallText = "";

	private string m_RemainingTime = "";

	private int m_Maximum = 100;

	private int m_Minimum;

	private int m_Value;

	public ProgressForm progressForm;

	private Thread progressThread;

	private Point m_fatherLocation;

	private Size m_fatherSize;

	public eStyle Style
	{
		get
		{
			return m_Style;
		}
		set
		{
			m_Style = value;
		}
	}

	public string Title
	{
		set
		{
			m_TitleText = value;
			if (progressForm != null)
			{
				progressForm.SetTitleText(value);
			}
		}
	}

	public string LargeText
	{
		set
		{
			m_LargeText = value;
			if (progressForm != null)
			{
				progressForm.SetLargeText(value);
			}
		}
	}

	public string SmallText
	{
		set
		{
			m_SmallText = value;
			if (progressForm != null)
			{
				progressForm.SetSmallText(value);
			}
		}
	}

	public string RemainingTime
	{
		set
		{
			m_RemainingTime = value;
			if (progressForm != null)
			{
				progressForm.SetRemainingTime(value);
			}
		}
	}

	public int Maximum
	{
		set
		{
			m_Maximum = value;
			if (progressForm != null)
			{
				progressForm.SetMaximum(value);
			}
		}
	}

	public int Minimum
	{
		set
		{
			m_Minimum = value;
			if (progressForm != null)
			{
				progressForm.SetMinimum(value);
			}
		}
	}

	public int Value
	{
		set
		{
			m_Value = value;
			if (progressForm != null)
			{
				progressForm.SetValue(value);
			}
		}
	}

	public int StartDelay
	{
		set
		{
			m_StartDelay = value;
		}
	}

	public int ReleaseTime
	{
		set
		{
			m_ReleaseTime = value;
			if (progressForm != null)
			{
				progressForm.SetReleaseTime(value);
			}
		}
	}

	private void ThreadTask()
	{
		DateTime now = DateTime.Now;
		m_state = eState.Starting;
		Thread.Sleep(m_StartDelay * 1000);
		if (m_state == eState.Starting)
		{
			progressForm = null;
			progressForm = new ProgressForm();
			Point location = new Point(m_fatherLocation.X, m_fatherLocation.Y + m_fatherSize.Height / 2);
			progressForm.Location = location;
			progressForm.StartTime = now;
			progressForm.SetTitleText(m_TitleText);
			progressForm.SetLargeText(m_LargeText);
			progressForm.SetSmallText(m_SmallText);
			progressForm.SetRemainingTime(m_RemainingTime);
			progressForm.SetReleaseTime(m_ReleaseTime);
			progressForm.SetMaximum(m_Maximum);
			progressForm.SetMinimum(m_Minimum);
			progressForm.SetValue(m_Value);
			m_state = eState.Running;
			if (m_Style == eStyle.Automatic)
			{
				progressForm.StartAutomatic();
			}
			progressForm.ShowDialog();
			progressThread.Abort();
		}
	}

	public void Start(Point fatherLocation, Size fatherSize)
	{
		m_fatherLocation = fatherLocation;
		m_fatherSize = fatherSize;
		if (progressThread != null && progressThread.IsAlive)
		{
			progressThread.Abort();
		}
		progressThread = null;
		progressThread = new Thread(ThreadTask);
		progressThread.IsBackground = true;
		progressThread.Start();
	}

	public void Stop()
	{
		if (m_state == eState.Starting)
		{
			m_state = eState.Stopping;
		}
		if (m_state == eState.Running)
		{
			m_state = eState.Stopping;
			progressForm.SetStop(value: true);
		}
	}
}
