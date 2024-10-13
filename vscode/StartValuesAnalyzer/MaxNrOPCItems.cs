using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace StartValuesAnalyzer;

public class MaxNrOPCItems : Form
{
	private int m_MaxNrOfNonStringItems = 5000;

	private int m_MaxNrOfStringItems = 1500;

	private int m_NrOfOPCRetries = 1;

	private IContainer components;

	private TextBox txtNr;

	private Button btnOk;

	private Button btnCancel;

	private Label label1;

	private Label label2;

	private TextBox txtMaxNrStringItems;

	private Label label3;

	private TextBox txtOPCRetries;

	public int MaxNrOfNonStringItems
	{
		get
		{
			return m_MaxNrOfNonStringItems;
		}
		set
		{
			m_MaxNrOfNonStringItems = value;
		}
	}

	public int MaxNrOfStringItems
	{
		get
		{
			return m_MaxNrOfStringItems;
		}
		set
		{
			m_MaxNrOfStringItems = value;
		}
	}

	public int NrOfOPCRetries
	{
		get
		{
			return m_NrOfOPCRetries;
		}
		set
		{
			m_NrOfOPCRetries = value;
		}
	}

	public MaxNrOPCItems()
	{
		InitializeComponent();
	}

	private void btnOk_Click(object sender, EventArgs e)
	{
		string text = "";
		try
		{
			text = txtNr.Text;
			int num = int.Parse(txtNr.Text);
			if (num >= 500 && num <= 20000)
			{
				m_MaxNrOfNonStringItems = num;
				text = txtMaxNrStringItems.Text;
				num = int.Parse(txtMaxNrStringItems.Text);
				if (num >= 150 && num <= 6000)
				{
					m_MaxNrOfStringItems = num;
					text = txtOPCRetries.Text;
					num = int.Parse(txtOPCRetries.Text);
					if (num >= 1 && num <= 5)
					{
						m_NrOfOPCRetries = num;
						base.DialogResult = DialogResult.OK;
						Close();
						return;
					}
					throw new Exception("Value Out of range");
				}
				throw new Exception("Value Out of range");
			}
			throw new Exception("Value Out of range");
		}
		catch (Exception ex)
		{
			MessageBox.Show(this, ex.Message + " Value=" + text, "Value out of Range", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void MaxNrOPCItems_Load(object sender, EventArgs e)
	{
		txtNr.Text = m_MaxNrOfNonStringItems.ToString();
		txtMaxNrStringItems.Text = m_MaxNrOfStringItems.ToString();
		txtOPCRetries.Text = m_NrOfOPCRetries.ToString();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartValuesAnalyzer.MaxNrOPCItems));
		this.txtNr = new System.Windows.Forms.TextBox();
		this.btnOk = new System.Windows.Forms.Button();
		this.btnCancel = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtMaxNrStringItems = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtOPCRetries = new System.Windows.Forms.TextBox();
		base.SuspendLayout();
		this.txtNr.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtNr.Location = new System.Drawing.Point(213, 12);
		this.txtNr.Name = "txtNr";
		this.txtNr.Size = new System.Drawing.Size(100, 20);
		this.txtNr.TabIndex = 0;
		this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnOk.Location = new System.Drawing.Point(328, 10);
		this.btnOk.Name = "btnOk";
		this.btnOk.Size = new System.Drawing.Size(75, 23);
		this.btnOk.TabIndex = 4;
		this.btnOk.Text = "OK";
		this.btnOk.UseVisualStyleBackColor = true;
		this.btnOk.Click += new System.EventHandler(btnOk_Click);
		this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btnCancel.Location = new System.Drawing.Point(328, 39);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(75, 23);
		this.btnCancel.TabIndex = 5;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = true;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(1, 14);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(206, 13);
		this.label1.TabIndex = 3;
		this.label1.Text = "Max no of non-string items in one OPC call";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(1, 44);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(185, 13);
		this.label2.TabIndex = 4;
		this.label2.Text = "Max no of string items in one OPC call";
		this.txtMaxNrStringItems.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtMaxNrStringItems.Location = new System.Drawing.Point(213, 44);
		this.txtMaxNrStringItems.Name = "txtMaxNrStringItems";
		this.txtMaxNrStringItems.Size = new System.Drawing.Size(100, 20);
		this.txtMaxNrStringItems.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(1, 74);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(108, 13);
		this.label3.TabIndex = 6;
		this.label3.Text = "No of OPC call retries";
		this.txtOPCRetries.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.txtOPCRetries.Location = new System.Drawing.Point(213, 74);
		this.txtOPCRetries.Name = "txtOPCRetries";
		this.txtOPCRetries.Size = new System.Drawing.Size(100, 20);
		this.txtOPCRetries.TabIndex = 3;
		base.AcceptButton = this.btnOk;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.CancelButton = this.btnCancel;
		base.ClientSize = new System.Drawing.Size(415, 108);
		base.Controls.Add(this.txtOPCRetries);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.txtMaxNrStringItems);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.btnCancel);
		base.Controls.Add(this.btnOk);
		base.Controls.Add(this.txtNr);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "MaxNrOPCItems";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Data Collection Settings";
		base.Load += new System.EventHandler(MaxNrOPCItems_Load);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
