using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Pearl.Interfaces;
using Pearl.Plugins.Utilities;

namespace EntitlementTest
{
    /// <summary>
    /// The form tool
    /// </summary>
    public partial class FormTool : Form
    {
        /// <summary>
        /// The dict.
        /// </summary>
        [Import] public IDictionary<string, string> Dict;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormTool"/> class.
        /// </summary>
        public FormTool()
        {
            InitializeComponent();

            Load += FmMain_Load;
            dataGridView1.CellMouseDown += DataGridView1_CellMouseDown;
            comboBoxResponse.SelectedIndex = 0;
        }

        /// <summary>
        /// Gets or sets the operation.
        /// </summary>
        [Import]
        public IGenericRestOperations Operation { get; set; }

        /// <summary>
        /// Gets or sets the manager.
        /// </summary>
        [Import]
        public IAuthenticationManager Manager { get; set; }

        /// <summary>
        /// Gets or sets the lazy ticket.
        /// </summary>
        [Import]
        public Lazy<ITicket> LazyTicket { get; set; }

        /// <summary>
        /// Gets or sets the config.
        /// </summary>
        [Import]
        private IConfigManager Config { get; set; }

        /// <summary>
        /// Gets or sets the auto updater.
        /// </summary>
        [Import]
        private IAutoUpdater AutoUpdater { get; set; }

        /// <summary>
        /// Gets or sets the logger.
        /// </summary>
        [Import]
        private ILogger logger { get; set; }

        /// <summary>
        /// The fm main_ load.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void FmMain_Load(object sender, EventArgs e)
        {
            var catalog = new DirectoryCatalog("plugins");
            var container = new CompositionContainer(catalog);

            container.ComposeParts(this);

            // foreach(var export in this.ImportsManager)
            // {
            // //string exportedMenuText = export.Metadata["Name"] as string;
            // var manager = export.Value;
            // if (manager == null)
            // {
            // MessageBox.Show("fff");
            // }
            // }

            RefreshGridView();
        }

        /// <summary>
        /// The data grid view 1_ cell mouse down.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
            }
        }

        /// <summary>
        /// The refresh grid view.
        /// </summary>
        private void RefreshGridView()
        {
            if (cmbKey.Items.Count == 0)
            {
                cmbKey.DataSource = Dict.Keys.ToList();
            }

            IAuthenticationTicket ticket = Manager.GetAuthenticationTicket();
            Dict["(Class)ticket"] = ticket == null ? "null" : ticket.ToString();

            Dict["(Bool)IsAuthenticated"] = Manager.IsAuthenticated ? "True" : "False";

            dataGridView1.DataSource = (from v in Dict
                select new
                {
                    v.Key, 
                    v.Value
                }
                ).ToArray();
            dataGridView1.Columns[0].Width = 200;
            dataGridView1.Columns[1].Width = 400;
        }

        /// <summary>
        /// The btn set_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void BtnSet_Click(object sender, EventArgs e)
        {
            if (cmbKey.SelectedIndex == -1)
            {
                return;
            }

            Dict[cmbKey.SelectedItem.ToString()] = textBox1.Text;

            RefreshGridView();
        }

        /// <summary>
        /// The button 1_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void button1_Click(object sender, EventArgs e)
        {
            Manager.ValidateUser();

            RefreshGridView();

            if (Manager.IsAuthenticated)
            {
                MessageBox.Show("Log in successfully");
            }
            else
            {
                MessageBox.Show("Log in failed");
            }
        }


        /// <summary>
        /// The button 1_ click_1.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            ITicket ticket = LazyTicket.Value;
            ticket.UserName = textUserName.Text;
            ticket.Password = textPassword.Text;

            Manager.ValidateUser(ticket);


            RefreshGridView();

            if (Manager.IsAuthenticated)
            {
                MessageBox.Show("Log in successfully");
            }
            else
            {
                MessageBox.Show("Log in failed");
            }
        }

        /// <summary>
        /// The tool strip menu item copy_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void toolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(dataGridView1.GetClipboardContent());
        }

        /// <summary>
        /// The button 2_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void button2_Click(object sender, EventArgs e)
        {
            textOutput.Text = AESEncryption.EncryptString(textInput.Text);
        }

        /// <summary>
        /// The button 3_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void button3_Click(object sender, EventArgs e)
        {
            textOutput.Text = AESEncryption.DecryptString(textInput.Text);
        }

        /// <summary>
        /// The button use header_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void buttonUseHeader_Click(object sender, EventArgs e)
        {
            try
            {
                string p = GetResult<string>();
                richReturn.Text = p;
            }
            catch (Exception ex)
            {
                richReturn.Text = ex.InnerException.Message +
                                  Environment.NewLine +
                                  ex.InnerException.StackTrace;
            }
        }

        /// <summary>
        /// The get result.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        private string GetResult<T>() where T : class
        {
            var dictHeader = new Dictionary<string, string>();

            if (checkBoxUserHeader.Checked)
            {
                int rowCount = dataGridViewHeader.RowCount;
                for (int i = 0; i < rowCount - 1; i++)
                {
                    string key = dataGridViewHeader.Rows[i].Cells[0].Value.ToString();
                    string val = dataGridViewHeader.Rows[i].Cells[1].Value.ToString();

                    if (dictHeader.ContainsKey(key))
                    {
                        MessageBox.Show("The header contains Duplicated [" + key + "]");
                        throw new Exception("The header contains Duplicated [" + key + "]");
                    }

                    dictHeader[key] = val;
                }
            }

            if (chkBasicAuthenticatin.Checked)
            {
                int rowCount = dataGridAuthentication.Rows.Count;
                if (rowCount > 1)
                {
                    string userName = dataGridAuthentication.Rows[0].Cells[1].Value.ToString();
                    string password = dataGridAuthentication.Rows[1].Cells[1].Value.ToString();
                    string authInfo = userName + ":" + password;
                    authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                    dictHeader["Authentication"] = authInfo;
                }
            }


            Operation.ConfigRequestHeader(dictHeader);


            string url = textUrl.Text;
            string command = textCommand.Text;
            var dictCommand = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(textCommand.Text))
            {
                dictCommand = new Dictionary<string, object>();
                int rowCount = dataGridViewCommand.RowCount;
                for (int i = 0; i < rowCount - 1; i++)
                {
                    string key = dataGridViewCommand.Rows[i].Cells[0].Value.ToString();
                    object val = dataGridViewCommand.Rows[i].Cells[1].Value;

                    if (dictCommand.ContainsKey(key))
                    {
                        MessageBox.Show("The header contains Duplicated [" + key + "]");
                        throw new Exception("The header contains Duplicated [" + key + "]");
                    }

                    dictCommand[key] = val;
                }
            }

            // var command = this.textCommand.Text;
            if (radioButtonGet.Checked)
            {
                if (comboBoxResponse.SelectedIndex == 1)
                {
                    Task<Stream> task = Operation.GetForObjectAsync<Stream>(url, command, dictCommand);
                    Stream stream = task.Result;
                    string filePath;
                    filePath = Assembly.GetExecutingAssembly().Location;
                    filePath = Path.GetDirectoryName(filePath) + @"\test.txt";

                    using (FileStream fileStream = File.Create(filePath, (int) stream.Length))
                    {
                        var bytesInStream = new byte[stream.Length];
                        stream.Read(bytesInStream, 0, bytesInStream.Length);
                        fileStream.Write(bytesInStream, 0, bytesInStream.Length);
                    }

                    return filePath;
                }
                else
                {
                    Task<string> task = Operation.GetForObjectAsync<string>(url, command, dictCommand);
                    string rest = task.Result;
                    return rest;
                }
            }

            object Content = JsonConvert.DeserializeObject(richTextBox1.Text.Trim());
            object o = Content;
            if (comboBoxResponse.SelectedIndex == 1)
            {
                Task<Stream> task = Operation.PostForObjectAsync<Stream>(url, command, dictCommand, o);
                Stream stream = task.Result;
                string filePath;
                filePath = Assembly.GetExecutingAssembly().Location;
                filePath = Path.GetDirectoryName(filePath) + @"\test.txt";

                using (FileStream fileStream = File.Create(filePath, (int) stream.Length))
                {
                    var bytesInStream = new byte[stream.Length];
                    stream.Read(bytesInStream, 0, bytesInStream.Length);
                    fileStream.Write(bytesInStream, 0, bytesInStream.Length);
                }

                return filePath;
            }
            else
            {
                Task<string> task = Operation.PostForObjectAsync<string>(url, command, dictCommand, o);
                string rest = task.Result;
                return rest;
            }
        }


        /// <summary>
        /// The data grid view 2_ cell content click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        /// <summary>
        /// The check box user header_ checked changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void checkBoxUserHeader_CheckedChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The button file_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void buttonFile_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// The button 4_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void button4_Click(object sender, EventArgs e)
        {
            Config.Reload();
            Config.Set("Remote", "UpdateCheckUrl", txtCheckUrl.Text.Trim());
            Config.Set("Remote", "UpdateFilePath", txtUpdateFileUrl.Text.Trim());

            richTextAutoUpdate.Text = "finished saving autoupdate configuration.";

// this.logger.Info(this.richTextAutoUpdate.Text);
        }

        /// <summary>
        /// The button 5_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void button5_Click(object sender, EventArgs e)
        {
            txtCheckUrl.Text = Config.Get("Remote", "UpdateCheckUrl");
            txtUpdateFileUrl.Text = Config.Get("Remote", "UpdateFilePath");
            richTextAutoUpdate.Text = richTextAutoUpdate.Text +
                                      "\nfinished loading autoupdate configuration.";
        }

        /// <summary>
        /// The button 6_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void button6_Click(object sender, EventArgs e)
        {
            string message = richTextAutoUpdate.Text;
            try
            {
                AutoUpdater.CheckUpdates();
                message = message + "\nfinished checking the update.";
            }
            catch (Exception ex)
            {
                message = message +
                          ex.InnerException.Message +
                          Environment.NewLine +
                          ex.InnerException.StackTrace;
            }

            richTextAutoUpdate.Text = message;
        }

        /// <summary>
        /// The button 7_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void button7_Click(object sender, EventArgs e)
        {
            string message = richTextAutoUpdate.Text;

            try
            {
                AutoUpdater.DownloadUpdates();
                message = message + "\nfinished downloading the updates.";
            }
            catch (Exception ex)
            {
                message = message +
                          ex.InnerException.Message +
                          Environment.NewLine +
                          ex.InnerException.StackTrace;
            }

            richTextAutoUpdate.Text = message;
        }
    }
}