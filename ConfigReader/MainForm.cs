using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ConfigReader
{
    public class MainForm : Form
    {
        private readonly TextBox pathBox = new TextBox { ReadOnly = true, Dock = DockStyle.Fill };
        private readonly Button browseButton = new Button { Text = "Browse...", AutoSize = true };
        private readonly DataGridView grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AutoGenerateColumns = true,
            AllowUserToResizeColumns = true,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };

        public MainForm()
        {
            Text = "App.config Reader";
            Width = 900;
            Height = 500;

            var top = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
            top.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            top.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            top.Controls.Add(pathBox, 0, 0);
            top.Controls.Add(browseButton, 1, 0);

            Controls.Add(grid);
            Controls.Add(top);
            browseButton.Click += (s, e) => Browse();
        }

        private void Browse()
        {
            using (var dlg = new OpenFileDialog
            {
                Title = "Select a config file",
                Filter = "Config files (*.config)|*.config|XML files (*.xml)|*.xml|All files (*.*)|*.*"
            })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    grid.DataSource = new BindingList<ConfigEntry>(ConfigParser.Parse(dlg.FileName));
                    grid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    pathBox.Text = dlg.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "Could not read file: " + ex.Message, Text,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
