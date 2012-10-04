using dbxTweaker.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
namespace dbxTweaker
{
	public class CasCat
	{
		public const uint DbxMagic = 263377358u;
		public const string VanillaBackupName = "cas.cat_backup";
		public Main mainForm;
		public Dictionary<int, BinaryReader> casReaders;
		public BinaryWriter casWriter;
		public int casWriterNumber;
		public string casPath;
		public string modName;
		public List<CatEntry> catEntries;
		public Dictionary<byte[], string> guidTable;
		public CasCat(string catPath, Main handle)
		{
			this.mainForm = handle;
			this.casPath = Path.GetDirectoryName(catPath) + "\\";
			this.modName = Path.GetFileName(catPath);
			this.setUserDefaultDirectory();
			this.casWriterNumber = 0;
			this.guidTable = new Dictionary<byte[], string>(new ByteArrayComparer());
			this.casReaders = new Dictionary<int, BinaryReader>();
			this.catEntries = new List<CatEntry>();
			using (BinaryReader binaryReader = new BinaryReader(File.Open(catPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
			{
				if (new string(binaryReader.ReadChars(16)) != "NyanNyanNyanNyan")
				{
					throw new Exception("No Nyan found. Invalid cat file header.");
				}
				while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length / 1L)
				{
					CatEntry catEntry = new CatEntry(binaryReader);
					this.catEntries.Add(catEntry);
					BinaryReader binaryReader2 = this.getBinaryReader((int)catEntry.archiveNumber);
					binaryReader2.BaseStream.Position = (long)((ulong)catEntry.offset);
					uint num = binaryReader2.ReadUInt32();
					if (num == 263377358u)
					{
						DbxFile dbxFile = new DbxFile(binaryReader2, catEntry, this);
						dbxFile.InitializeFilename(binaryReader2);
						this.guidTable[dbxFile.dbxHeader.fileGuid] = dbxFile.filename;
						this.addToTreeView(dbxFile);
					}
				}
			}
		}
		public void setUserDefaultDirectory()
		{
			if (!File.Exists(this.casPath + this.modName))
			{
				throw new Exception("the specified cat file does not exist");
			}
			Settings.Default.lastDirectory = this.casPath;
			if (this.casPath.EndsWith("\\Update\\Patch\\Data\\"))
			{
				string text = this.casPath.Substring(0, this.casPath.Length - 18) + "Data\\";
				Settings.Default.PatchedCasDirectory = this.casPath;
				if (!File.Exists(this.casPath + "cas.cat_backup") && File.Exists(this.casPath + "cas.cat"))
				{
					File.Copy(this.casPath + "cas.cat", this.casPath + "cas.cat_backup");
				}
				if (File.Exists(text + "cas.cat"))
				{
					Settings.Default.UnpatchedCasDirectory = text;
					if (!File.Exists(text + "cas.cat_backup") && File.Exists(text + "cas.cat"))
					{
						File.Copy(text + "cas.cat", text + "cas.cat_backup");
					}
				}
			}
			else
			{
				if (!this.casPath.EndsWith("\\Data\\"))
				{
					return;
				}
				string text2 = this.casPath.Substring(0, this.casPath.Length - 5) + "Update\\Patch\\Data\\";
				Settings.Default.UnpatchedCasDirectory = this.casPath;
				if (!File.Exists(this.casPath + "cas.cat_backup") && File.Exists(this.casPath + "cas.cat"))
				{
					File.Copy(this.casPath + "cas.cat", this.casPath + "cas.cat_backup");
				}
				if (File.Exists(text2 + "cas.cat"))
				{
					Settings.Default.PatchedCasDirectory = text2;
					if (!File.Exists(text2 + "cas.cat_backup") && File.Exists(text2 + "cas.cat"))
					{
						File.Copy(text2 + "cas.cat", text2 + "cas.cat_backup");
					}
				}
			}
			Settings.Default.Save();
		}
		public void addToTreeView(DbxFile dbxfile)
		{
			string[] array = dbxfile.filename.Split(new char[]
			{
				'/'
			});
			if (!this.mainForm.treeView1.Nodes.ContainsKey(array[0]))
			{
				this.mainForm.treeView1.Nodes.Add(array[0], array[0]);
			}
			if (array.Count<string>() == 1)
			{
				this.mainForm.treeView1.Nodes[array[0]].Tag = dbxfile;
				return;
			}
			this.addSubNode(dbxfile, 1, array.ToList<string>(), this.mainForm.treeView1.Nodes[array[0]]);
		}
		public void addSubNode(DbxFile dbxfile, int index, List<string> elems, TreeNode tn)
		{
			string text = elems[index];
			if (!tn.Nodes.ContainsKey(text))
			{
				tn.Nodes.Add(text, text);
			}
			if (elems.Count - 1 == index)
			{
				tn.Nodes[text].Tag = dbxfile;
				return;
			}
			this.addSubNode(dbxfile, index + 1, elems.ToList<string>(), tn.Nodes[text]);
		}
		public BinaryReader getBinaryReader(int archiveNumber)
		{
			string path = this.casPath + "cas_" + ((archiveNumber < 10) ? ("0" + archiveNumber) : archiveNumber.ToString()) + ".cas";
			if (this.casReaders.ContainsKey(archiveNumber))
			{
				return this.casReaders[archiveNumber];
			}
			BinaryReader binaryReader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
			this.casReaders.Add(archiveNumber, binaryReader);
			if (archiveNumber >= 50)
			{
				this.casWriterNumber = archiveNumber;
			}
			return binaryReader;
		}
		public BinaryWriter getWriter(DbxFile dbx)
		{
			if (dbx.catEntry.archiveNumber >= 50u)
			{
				try
				{
					this.casWriter = new BinaryWriter(File.Open(string.Concat(new object[]
					{
						this.casPath,
						"cas_",
						dbx.catEntry.archiveNumber,
						".cas"
					}), FileMode.Open, FileAccess.Write, FileShare.ReadWrite));
				}
				catch (Exception)
				{
				}
				return this.casWriter;
			}
			uint num = 4294967295u;
			uint num2 = 4294967295u;
			if (this.casWriterNumber >= 50)
			{
				try
				{
					this.casWriter = new BinaryWriter(File.Open(string.Concat(new object[]
					{
						this.casPath,
						"cas_",
						this.casWriterNumber,
						".cas"
					}), FileMode.Open, FileAccess.Write, FileShare.ReadWrite));
				}
				catch (Exception)
				{
				}
				this.casWriter.Seek(0, SeekOrigin.End);
				num = (uint)this.casWriterNumber;
				num2 = (uint)this.casWriter.BaseStream.Position + 32u;
			}
			else
			{
				for (int i = 50; i < 100; i++)
				{
					if (!File.Exists(string.Concat(new object[]
					{
						this.casPath,
						"cas_",
						i,
						".cas"
					})))
					{
						this.casWriterNumber = i;
						this.casWriter = new BinaryWriter(File.Open(string.Concat(new object[]
						{
							this.casPath,
							"cas_",
							i,
							".cas"
						}), FileMode.Create, FileAccess.Write, FileShare.ReadWrite));
						num = (uint)i;
						num2 = 32u;
						break;
					}
				}
			}
			if (this.casWriter == null || num == 4294967295u)
			{
				throw new Exception("Cannot create new cas file in the range 50-99");
			}
			BinaryReader binaryReader = this.casReaders[(int)dbx.catEntry.archiveNumber];
			uint offset = dbx.catEntry.offset;
			BinaryWriter binaryWriter = new BinaryWriter(File.Open(this.casPath + this.modName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite));
			binaryReader.BaseStream.Position = (long)((ulong)(offset - 32u));
			this.casWriter.Write(binaryReader.ReadBytes((int)(dbx.catEntry.size + 32u)));
			for (int j = 0; j < this.catEntries.Count<CatEntry>(); j++)
			{
				if (this.catEntries[j].sha1.SequenceEqual(dbx.catEntry.sha1))
				{
					int num3 = 16 + j * 32;
					this.catEntries[j].archiveNumber = num;
					this.catEntries[j].offset = num2;
					binaryWriter.Seek(num3 + 20, SeekOrigin.Begin);
					binaryWriter.Write(num2);
					binaryWriter.Seek(4, SeekOrigin.Current);
					binaryWriter.Write(num);
					binaryWriter.Close();
					break;
				}
			}
			return this.casWriter;
		}
		public static void createVanillaCat(string path)
		{
			try
			{
				CasCat.createVanillaCatFromBackup(path);
			}
			catch
			{
				CasCat.createVanillaCatFromCas(path);
			}
		}
		private static void createVanillaCatFromBackup(string directory)
		{
			File.Copy(directory + "cas.cat_backup", directory + "cas.cat", true);
		}
		private static void createVanillaCatFromCas(string directory)
		{
			string text = directory + "cas.cat";
			if (!File.Exists(text))
			{
				MessageBox.Show("No cat file to restore found.");
				return;
			}
			using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(text, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)))
			{
				binaryWriter.Write("NyanNyanNyanNyan".ToCharArray());
				for (int i = 1; i < 50; i++)
				{
					string path = directory + "cas_" + ((i < 10) ? ("0" + i) : i.ToString()) + ".cas";
					try
					{
						using (BinaryReader binaryReader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))
						{
							while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
							{
								CasHeader casHeader = new CasHeader(binaryReader);
								binaryWriter.Write(casHeader.sha1);
								binaryWriter.Write((uint)binaryReader.BaseStream.Position);
								binaryWriter.Write(casHeader.size);
								binaryWriter.Write(i);
								binaryReader.BaseStream.Position += (long)((ulong)casHeader.size);
							}
						}
					}
					catch
					{
						break;
					}
				}
			}
			File.Copy(text, directory + "cas.cat_backup", true);
		}
	}
}
