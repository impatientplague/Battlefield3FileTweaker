using dbxTweaker.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
namespace dbxTweaker
{
	public class Main : Form
	{
		public CasCat cat;
		public string catPath;
		public Form loadingWindow;
		private IContainer components;
		public TreeView treeView1;
		private SplitContainer splitContainer1;
		public PropertyGrid propertyGrid1;
		private MenuStrip menuStrip1;
		private ToolStripMenuItem fileToolStripMenuItem;
		private ToolStripMenuItem createModToolStripMenuItem;
		private ToolStripMenuItem openModToolStripMenuItem;
		private ToolStripMenuItem activateModMenuOption;
		private ToolStripMenuItem restoreOriginalCatToolStripMenuItem;
		public OpenFileDialog selectCatFileDialog;
		private ToolStripMenuItem restoreUpdatedCatToolStripMenuItem;
		public Main()
		{
			this.InitializeComponent();
		}
		private void Form1_Load(object sender, EventArgs e)
		{
			this.MoveSplitter(this.propertyGrid1, 300);
		}
		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			DbxFile dbxFile = e.Node.Tag as DbxFile;
			if (dbxFile == null)
			{
				this.propertyGrid1.SelectedObject = null;
				return;
			}
			if (dbxFile.instances == null)
			{
				dbxFile.InitializePayload();
			}
			this.propertyGrid1.SelectedObject = dbxFile;
		}
		private void MoveSplitter(PropertyGrid propertyGrid, int x)
		{
			object obj = typeof(PropertyGrid).InvokeMember("gridView", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField, null, propertyGrid, null);
			obj.GetType().InvokeMember("MoveSplitterTo", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, obj, new object[]
			{
				x
			});
		}
		private void restoreOriginalCatToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CasCat.createVanillaCat(Settings.Default.UnpatchedCasDirectory);
		}
		private void restoreUpdatedCatToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CasCat.createVanillaCat(Settings.Default.PatchedCasDirectory);
		}
		private void openCat()
		{
			this.treeView1.Nodes.Clear();
			this.propertyGrid1.SelectedObject = null;
			this.treeView1.BeginUpdate();
			this.cat = new CasCat(this.catPath, this);
			this.treeView1.Sort();
			this.treeView1.EndUpdate();
			this.Text = this.cat.modName + " - BF3 Gameplay File Tweaker";
		}
		private void openModToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.selectCatFileDialog.InitialDirectory = Settings.Default.lastDirectory;
			if (this.selectCatFileDialog.ShowDialog() != DialogResult.OK)
			{
				return;
			}
			this.catPath = this.selectCatFileDialog.FileName;
			this.openCat();
		}
		private void createModToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CreateNewModDialog createNewModDialog = new CreateNewModDialog
			{
				Owner = this
			};
			createNewModDialog.ShowDialog();
			if (createNewModDialog.catPath == "" || createNewModDialog.modPath == "")
			{
				return;
			}
			File.Copy(createNewModDialog.catPath, createNewModDialog.modPath);
			this.catPath = createNewModDialog.modPath;
			this.openCat();
		}
		private void activateModMenuOption_Click(object sender, EventArgs e)
		{
			if (this.cat == null)
			{
				return;
			}
			this.cat.casWriter.Close();
			if (this.cat.modName != "cas.cat")
			{
				File.Copy(this.cat.casPath + this.cat.modName, this.cat.casPath + "cas.cat", true);
			}
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
			this.treeView1 = new TreeView();
			this.splitContainer1 = new SplitContainer();
			this.propertyGrid1 = new PropertyGrid();
			this.menuStrip1 = new MenuStrip();
			this.fileToolStripMenuItem = new ToolStripMenuItem();
			this.createModToolStripMenuItem = new ToolStripMenuItem();
			this.openModToolStripMenuItem = new ToolStripMenuItem();
			this.activateModMenuOption = new ToolStripMenuItem();
			this.restoreOriginalCatToolStripMenuItem = new ToolStripMenuItem();
			this.restoreUpdatedCatToolStripMenuItem = new ToolStripMenuItem();
			this.selectCatFileDialog = new OpenFileDialog();
			((ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			base.SuspendLayout();
			this.treeView1.Dock = DockStyle.Fill;
			this.treeView1.HideSelection = false;
			this.treeView1.Location = new Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new Size(233, 843);
			this.treeView1.TabIndex = 1;
			this.treeView1.AfterSelect += new TreeViewEventHandler(this.treeView1_AfterSelect);
			this.splitContainer1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.splitContainer1.Location = new Point(12, 27);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.treeView1);
			this.splitContainer1.Panel2.Controls.Add(this.propertyGrid1);
			this.splitContainer1.Size = new Size(1131, 843);
			this.splitContainer1.SplitterDistance = 233;
			this.splitContainer1.TabIndex = 3;
			this.propertyGrid1.BackColor = SystemColors.Window;
			this.propertyGrid1.Dock = DockStyle.Fill;
			this.propertyGrid1.Location = new Point(0, 0);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.PropertySort = PropertySort.NoSort;
			this.propertyGrid1.Size = new Size(894, 843);
			this.propertyGrid1.TabIndex = 4;
			this.propertyGrid1.ToolbarVisible = false;
			this.menuStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.fileToolStripMenuItem
			});
			this.menuStrip1.Location = new Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new Size(1155, 24);
			this.menuStrip1.TabIndex = 5;
			this.menuStrip1.Text = "menuStrip1";
			this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.createModToolStripMenuItem,
				this.openModToolStripMenuItem,
				this.activateModMenuOption,
				this.restoreOriginalCatToolStripMenuItem,
				this.restoreUpdatedCatToolStripMenuItem
			});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			this.createModToolStripMenuItem.Name = "createModToolStripMenuItem";
			this.createModToolStripMenuItem.Size = new Size(289, 22);
			this.createModToolStripMenuItem.Text = "Create Cat";
			this.createModToolStripMenuItem.Click += new EventHandler(this.createModToolStripMenuItem_Click);
			this.openModToolStripMenuItem.Name = "openModToolStripMenuItem";
			this.openModToolStripMenuItem.Size = new Size(289, 22);
			this.openModToolStripMenuItem.Text = "Open Cat";
			this.openModToolStripMenuItem.Click += new EventHandler(this.openModToolStripMenuItem_Click);
			this.activateModMenuOption.Name = "activateModMenuOption";
			this.activateModMenuOption.Size = new Size(289, 22);
			this.activateModMenuOption.Text = "Save And Activate Currently Selected Cat";
			this.activateModMenuOption.Click += new EventHandler(this.activateModMenuOption_Click);
			this.restoreOriginalCatToolStripMenuItem.Name = "restoreOriginalCatToolStripMenuItem";
			this.restoreOriginalCatToolStripMenuItem.Size = new Size(289, 22);
			this.restoreOriginalCatToolStripMenuItem.Text = "Restore Unpatched Cat";
			this.restoreOriginalCatToolStripMenuItem.Click += new EventHandler(this.restoreOriginalCatToolStripMenuItem_Click);
			this.restoreUpdatedCatToolStripMenuItem.Name = "restoreUpdatedCatToolStripMenuItem";
			this.restoreUpdatedCatToolStripMenuItem.Size = new Size(289, 22);
			this.restoreUpdatedCatToolStripMenuItem.Text = "Restore Updated Cat";
			this.restoreUpdatedCatToolStripMenuItem.Click += new EventHandler(this.restoreUpdatedCatToolStripMenuItem_Click);
			this.selectCatFileDialog.Filter = "cat file|*.cat";
			this.selectCatFileDialog.InitialDirectory = Settings.Default.UnpatchedCasDirectory;
			this.selectCatFileDialog.Title = "Select cat file.";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(1155, 882);
			base.Controls.Add(this.splitContainer1);
			base.Controls.Add(this.menuStrip1);
			base.MainMenuStrip = this.menuStrip1;
			base.Name = "Main";
			this.Text = "BF3 Gameplay File Tweaker";
			base.Load += new EventHandler(this.Form1_Load);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
