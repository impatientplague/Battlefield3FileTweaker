using System;
using System.ComponentModel;
using System.Windows.Forms;
namespace dbxTweaker
{
	public class loadingScreen : Form
	{
		private IContainer components;
		public loadingScreen()
		{
			this.InitializeComponent();
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
			this.components = new Container();
			base.AutoScaleMode = AutoScaleMode.Font;
			this.Text = "Form1";
		}
	}
}
