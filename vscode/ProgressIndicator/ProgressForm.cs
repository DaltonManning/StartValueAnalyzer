using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ProgressIndicator;

public class ProgressForm : Form
{
	private delegate void SetTitleTextCallback(string text);

	private delegate void SetLargeTextCallback(string text);

	private delegate void SetSmallTextCallback(string text);

	private delegate void SetRemainingTimeCallback(string text);

	private delegate void SetMinimumCallback(int value);

	private delegate void SetMaximumCallback(int value);

	private delegate void SetValueCallback(int value);

	private delegate void SetReleaseTimeCallback(int value);

	private delegate void SetStopCallback(bool value);

	private delegate void SetTimerCallback(int value);

	private int m_Counter;

	private int m_ReleaseTime;

	private bool m_Stop;

	private DateTime m_startTime;

	private IContainer components;

	private Label lblRemainingTime;

	private Label lblSmallText;

	private Label lblLargeText;

	private ProgressBar progBar;

	private Timer timProgress;

	public int ReleaseTime
	{
		set
		{
			m_ReleaseTime = value;
		}
	}

	public DateTime StartTime
	{
		get
		{
			return m_startTime;
		}
		set
		{
			m_startTime = value;
		}
	}

	public ProgressForm()
	{
		InitializeComponent();
	}

	public void StartAutomatic()
	{
		m_Counter = 0;
		m_Stop = false;
		timProgress.Interval = 1000;
		timProgress.Enabled = true;
	}

	public void SetTitleText(string text)
	{
		if (base.InvokeRequired)
		{
			SetTitleTextCallback method = SetTitleText;
			Invoke(method, text);
		}
		else
		{
			Text = text;
		}
	}

	public void SetLargeText(string text)
	{
		if (lblLargeText.InvokeRequired)
		{
			SetLargeTextCallback method = SetLargeText;
			Invoke(method, text);
		}
		else
		{
			lblLargeText.Text = text;
		}
	}

	public void SetSmallText(string text)
	{
		if (lblLargeText.InvokeRequired)
		{
			try
			{
				SetSmallTextCallback method = SetSmallText;
				Invoke(method, text);
				return;
			}
			catch
			{
				return;
			}
		}
		lblSmallText.Text = text;
	}

	public void SetRemainingTime(string text)
	{
		if (lblLargeText.InvokeRequired)
		{
			try
			{
				SetRemainingTimeCallback method = SetRemainingTime;
				Invoke(method, text);
				return;
			}
			catch
			{
				return;
			}
		}
		lblRemainingTime.Text = text;
	}

	public void SetMinimum(int value)
	{
		if (lblLargeText.InvokeRequired)
		{
			try
			{
				SetMinimumCallback method = SetMinimum;
				Invoke(method, value);
				return;
			}
			catch
			{
				return;
			}
		}
		progBar.Minimum = value;
	}

	public void SetMaximum(int value)
	{
		if (lblLargeText.InvokeRequired)
		{
			try
			{
				SetMaximumCallback method = SetMaximum;
				Invoke(method, value);
				return;
			}
			catch
			{
				return;
			}
		}
		progBar.Maximum = value;
	}

	public void SetValue(int value)
	{
		if (lblLargeText.InvokeRequired)
		{
			try
			{
				SetValueCallback method = SetValue;
				Invoke(method, value);
				return;
			}
			catch
			{
				return;
			}
		}
		progBar.Value = value;
	}

	public void SetReleaseTime(int value)
	{
		if (lblLargeText.InvokeRequired)
		{
			try
			{
				SetReleaseTimeCallback method = SetReleaseTime;
				Invoke(method, value);
				return;
			}
			catch
			{
				return;
			}
		}
		ReleaseTime = value;
	}

	public void SetStop(bool value)
	{
		if (base.InvokeRequired)
		{
			try
			{
				SetStopCallback method = SetStop;
				Invoke(method, value);
				return;
			}
			catch
			{
				return;
			}
		}
		m_Stop = value;
		progBar.Value = progBar.Maximum;
		timProgress.Interval = 1000 * m_ReleaseTime;
		if (m_ReleaseTime <= 0)
		{
			timProgress.Interval = 10;
		}
		timProgress.Enabled = true;
	}

	private void timProgress_Tick(object sender, EventArgs e)
	{
		string text = (DateTime.Now - StartTime).ToString();
		int num = text.LastIndexOf('.');
		if (num + 4 < text.Length)
		{
			text = text.Remove(num + 4);
		}
		lblRemainingTime.Text = text;
		if (m_Stop)
		{
			timProgress.Enabled = false;
			m_Stop = false;
			Close();
		}
		else if (m_Counter + 1 < progBar.Maximum)
		{
			progBar.Value = m_Counter;
			m_Counter++;
			timProgress.Interval = 200000 / (progBar.Maximum - m_Counter);
		}
	}

	private void ProgressForm_Load(object sender, EventArgs e)
	{
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
		this.components = new System.ComponentModel.Container();
		this.lblRemainingTime = new System.Windows.Forms.Label();
		this.lblSmallText = new System.Windows.Forms.Label();
		this.lblLargeText = new System.Windows.Forms.Label();
		this.progBar = new System.Windows.Forms.ProgressBar();
		this.timProgress = new System.Windows.Forms.Timer(this.components);
		base.SuspendLayout();
		this.lblRemainingTime.AutoSize = true;
		this.lblRemainingTime.Location = new System.Drawing.Point(12, 40);
		this.lblRemainingTime.Name = "lblRemainingTime";
		this.lblRemainingTime.Size = new System.Drawing.Size(213, 13);
		this.lblRemainingTime.TabIndex = 7;
		this.lblRemainingTime.Text = "About 4 Minutes and 30 Seconds remaining";
		this.lblSmallText.AutoSize = true;
		this.lblSmallText.Location = new System.Drawing.Point(12, 27);
		this.lblSmallText.Name = "lblSmallText";
		this.lblSmallText.Size = new System.Drawing.Size(98, 13);
		this.lblSmallText.TabIndex = 6;
		this.lblSmallText.Text = "Detailed action text";
		this.lblLargeText.AutoSize = true;
		this.lblLargeText.Font = new System.Drawing.Font("Microsoft Sans Serif", 11f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblLargeText.Location = new System.Drawing.Point(12, 9);
		this.lblLargeText.Name = "lblLargeText";
		this.lblLargeText.Size = new System.Drawing.Size(81, 18);
		this.lblLargeText.TabIndex = 5;
		this.lblLargeText.Text = "Action Text";
		this.progBar.BackColor = System.Drawing.Color.LimeGreen;
		this.progBar.ForeColor = System.Drawing.Color.LimeGreen;
		this.progBar.Location = new System.Drawing.Point(12, 62);
		this.progBar.Name = "progBar";
		this.progBar.Size = new System.Drawing.Size(328, 23);
		this.progBar.TabIndex = 4;
		this.timProgress.Tick += new System.EventHandler(timProgress_Tick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(354, 97);
		base.ControlBox = false;
		base.Controls.Add(this.lblRemainingTime);
		base.Controls.Add(this.lblSmallText);
		base.Controls.Add(this.lblLargeText);
		base.Controls.Add(this.progBar);
		base.MaximizeBox = false;
		base.Name = "ProgressForm";
		base.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
		base.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
		this.Text = "Progress";
		base.Load += new System.EventHandler(ProgressForm_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
