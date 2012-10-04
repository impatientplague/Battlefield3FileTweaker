using dbxTweaker.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
namespace dbxTweaker
{
	public class CreateNewModDialog : Form
	{
		public string catPath = "";
		public string modPath = "";
		private IContainer components;
		private TextBox ModNameTextBox;
		private Button button1;
		private Label label1;
		private Label label2;
		private TextBox catFileToUseTextBox;
		public CreateNewModDialog()
		{
			this.InitializeComponent();
		}
		private void Form1_Load(object sender, EventArgs e)
		{
		}
		private void ModNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if ("/\\:*?\"<>|".Contains(e.KeyChar))
			{
				e.Handled = true;
			}
		}
		private void button1_Click(object sender, EventArgs e)
		{
			if (this.ModNameTextBox.Text == "" || this.catFileToUseTextBox.Text == "")
			{
				return;
			}
			this.catPath = this.catFileToUseTextBox.Text;
			this.modPath = Path.GetDirectoryName(this.catPath) + "\\" + this.ModNameTextBox.Text + ".cat";
			if (File.Exists(this.modPath))
			{
				MessageBox.Show("The filename is already in use");
				this.catPath = "";
				return;
			}
			base.Close();
		}
		private void textBox1_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Filter = "cat file|*.cat",
				Title = "Select cat file.",
				InitialDirectory = Settings.Default.lastDirectory
			};
			if (openFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			this.catFileToUseTextBox.Text = openFileDialog.FileName;
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.ModNameTextBox = new TextBox();
			this.button1 = new Button();
			this.label1 = new Label();
			this.label2 = new Label();
			this.catFileToUseTextBox = new TextBox();
			base.SuspendLayout();
			this.ModNameTextBox.Location = new Point(15, 25);
			this.ModNameTextBox.Name = "ModNameTextBox";
			this.ModNameTextBox.Size = new Size(100, 20);
			this.ModNameTextBox.TabIndex = 0;
			this.ModNameTextBox.KeyPress += new KeyPressEventHandler(this.ModNameTextBox_KeyPress);
			this.button1.Location = new Point(255, 146);
			this.button1.Name = "button1";
			this.button1.Size = new Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Accept";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new Size(86, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Name of the cat:";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(12, 75);
			this.label2.Name = "label2";
			this.label2.Size = new Size(129, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Inherit assets from cat file:";
			this.catFileToUseTextBox.BackColor = SystemColors.Window;
			this.catFileToUseTextBox.Location = new Point(15, 91);
			this.catFileToUseTextBox.Name = "catFileToUseTextBox";
			this.catFileToUseTextBox.ReadOnly = true;
			this.catFileToUseTextBox.Size = new Size(315, 20);
			this.catFileToUseTextBox.TabIndex = 4;
			this.catFileToUseTextBox.Click += new EventHandler(this.textBox1_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(352, 190);
			base.Controls.Add(this.catFileToUseTextBox);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.ModNameTextBox);
			base.Name = "CreateNewModDialog";
			this.Text = "Create New Cat";
			base.Load += new EventHandler(this.Form1_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
