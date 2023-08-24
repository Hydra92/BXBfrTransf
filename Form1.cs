using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BXBfrTransf
{
    public partial class Form_main : Form
    {
        string savePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        string fileName="sample";

        public Form_main()
        {
            InitializeComponent();
            
        }

        private void Form_main_Load(object sender, EventArgs e)
        {
            //comboBoxbefore.DataSource = FileType;
            //comboBoxafter.DataSource = FileType;
            //comboBoxafter.SelectedItem = FileType[2];
        }

        private string[] LogType =
        {
            "normal",
            "warning",
            "error"
        };

        private string[] FileType = { "word", "excle", "BFR" };

        public  bool WriteLog(string log, string type)
        {
            string time = DateTime.Now.ToString("T");
            richTextBox1.AppendText (time +" "+ log);
            richTextBox1.AppendText(System.Environment.NewLine);
            return true;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;
            var fileExtension = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "word files|*.doc;*.docx|excle files|*.xls;*.xlsx|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 3;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    fileExtension = Path.GetExtension(filePath).TrimStart('.');
                    fileName = Path.GetFileNameWithoutExtension(filePath);

                    ////Read the contents of the file into a stream
                    //var fileStream = openFileDialog.OpenFile();

                    //using (StreamReader reader = new StreamReader(fileStream))
                    //{
                    //    fileContent = reader.ReadToEnd();
                    //}
                }
            }

            //MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
            textBox1.Text = filePath;
            WriteLog("选择文件",LogType[0]);

            if(fileExtension=="doc"||fileExtension=="docx")
            {
                comboBoxbefore.Text = FileType[0];
            }
            else if(fileExtension=="xls"||fileExtension=="xlsx")
            {
                comboBoxbefore.Text = FileType[1];
            }
            else
            {
                comboBoxbefore.Text = fileExtension;
            }

            comboBoxafter.Text = "excle";
            
        }

        

        private void comboBoxbefore_TextChanged(object sender, EventArgs e)
        {
            if (!FileType.Contains(comboBoxbefore.Text))
            {
                WriteLog("文件格式好像不对，可能无法转换", LogType[1]);
            }
        }

        //转换按钮
        private void button2_Click(object sender, EventArgs e)
        {
            WriteLog("转换为BFR格式功能有BUG，暂转为excle", LogType[0]);
            WriteLog("转换开始，请等待.....", LogType[0]);
            string tarPath = savePath + "\\" + fileName + ".mht";
            string tarPath2 = savePath + "\\" + fileName + ".xls";
            bool flag = comboBoxbefore.Text == "word" && comboBoxafter.Text == "excle";
            if (flag)
            {
                
                Program.WordConvertMHT(textBox1.Text, tarPath);
                Program.MHTConvertExcle(tarPath, tarPath2);
                //Program.ExcleConventBFR(tarPath2, tarPath2, "qmtcwh", "56");
                WriteLog("转换完成，文件在桌面上", LogType[0]);
            }

            
            //WriteLog(, LogType[0]);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            //将光标位置设置到当前内容的末尾
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            //滚动到光标位置
            richTextBox1.ScrollToCaret();
        }
        //转换后的文件格式
        private void comboBoxafter_TextChanged(object sender, EventArgs e)
        {
            if(comboBoxafter.Text!="BFR")
            {
                rootCode.ReadOnly = true;
                reportEN.ReadOnly = true;
                return;
            }
            rootCode.ReadOnly = false;
            reportEN.ReadOnly = false;
            reportId.Text = Program.RoundNW(6);
        }
    }
}
