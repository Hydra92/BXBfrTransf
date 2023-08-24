using NPOI.XWPF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using word = Microsoft.Office.Interop.Word;
using Excle = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Core;

namespace BXBfrTransf
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form_main());

            
        }

        static public bool WordConvertMHT(string sourcePath, string targetPath)
        {
            bool result;
            word.ApplicationClass wordApp = null;
            word.Document wordDoc = null;
            word.WdSaveFormat saveFormat = word.WdSaveFormat.wdFormatWebArchive;   //mht格式
            //由于使用的是COM库，因此有许多变量需要用Missing.Value代替
            Object Nothing = Type.Missing;

            //如果已存在，则删除
            if (!File.Exists((string)sourcePath))
            {
                File.Delete((string)sourcePath);
            }

            try
            {

                wordApp = new word.ApplicationClass
                {
                    // 屏蔽宏
                    AutomationSecurity = MsoAutomationSecurity.msoAutomationSecurityLow,
                    Visible = false
                };
                object file = sourcePath;
                wordDoc = wordApp.Documents.Open(ref file,
                    ref Nothing, true, ref Nothing, ref Nothing,
                    ref Nothing, ref Nothing, ref Nothing, ref Nothing,
                    ref Nothing, ref Nothing, ref Nothing, ref Nothing,
                    ref Nothing, ref Nothing, ref Nothing);
                
                //WdSaveFormat为Word
                object path = targetPath;
                object format = saveFormat;
                wordDoc.SaveAs(ref path, ref format,
                    ref Nothing, ref Nothing, ref Nothing, ref Nothing,
                    ref Nothing, ref Nothing, ref Nothing, ref Nothing,
                    ref Nothing, ref Nothing, ref Nothing, ref Nothing,
                    ref Nothing, ref Nothing);
                //关闭wordDoc文档对象
                wordDoc.Close(ref Nothing, ref Nothing, ref Nothing);
                //关闭wordApp组件对象
                wordApp.Quit(ref Nothing, ref Nothing, ref Nothing);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
                //wordDoc.Close(ref Nothing, ref Nothing, ref Nothing);
                wordApp.Quit(ref Nothing, ref Nothing, ref Nothing);
            
            }
            return true;
        }

        static public bool MHTConvertExcle(string sourcePath, string targetPath)
        {
            bool result;
            string version;
            const int oldVersion = -4143;
            const int newVersion = 56;
            int formatNum;
            Excle.ApplicationClass excleApp = null;
            Excle.Workbook exclewb = null;
            Excle.XlWebFormatting openFormat = Excle.XlWebFormatting.xlWebFormattingAll;   //mht格式
            //由于使用的是COM库，因此有许多变量需要用Missing.Value代替
            Object Nothing = Type.Missing;

            

            try
            {

                excleApp = new Excle.ApplicationClass
                {
                    // 屏蔽宏
                    //AutomationSecurity = MsoAutomationSecurity.msoAutomationSecurityLow,
                    Visible = false
                };
                //object file = sourcePath;
                exclewb = excleApp.Workbooks.Open( sourcePath,
                    Nothing, Nothing, openFormat,  Nothing,
                    Nothing, Nothing, Nothing, Nothing,
                    Nothing, Nothing, Nothing, Nothing,
                    Nothing);
                version = excleApp.Version;
                if(Convert.ToDouble(version)<12)
                {
                    formatNum = oldVersion;
                }
                else
                {
                    formatNum = newVersion;
                }
                object path = targetPath;
                exclewb.SaveAs(path, formatNum);
                //关闭wordDoc文档对象
                exclewb.Close();
                //RunAutoMacros(XlRunAutoMacro);
                //关闭wordApp组件对象
                excleApp.Quit();

                //删除mht
                if (File.Exists((string)sourcePath))
                {
                    File.Delete((string)sourcePath);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                MessageBox.Show(ex.Message);
                exclewb.Close();
                excleApp.Quit();

            }
            return true;
        }
        /// <summary>
        /// excle转换成BFR格式
        /// </summary>
        /// <param name="sourcePath">excle文件路径，含文件名</param>
        /// <param name="targetPath">转换后的文件路径</param>
        /// <param name="rootCode">根节点代码</param>
        /// <param name="reportEN">报表英文名，用于写sql</param>
        static public void ExcleConventBFR(string sourcePath, string targetPath,string rootCode,string reportEN)
        {
            string excleSourcePath = sourcePath.Substring(0,sourcePath.LastIndexOf("\\"));
            string excleSourceName = sourcePath.Substring(sourcePath.LastIndexOf("\\")+1);
            Console.WriteLine(excleSourcePath);
            string excleTargetPath = excleSourcePath + "\\ftproot\\ROOT" + rootCode.ToUpper();
            Directory.CreateDirectory(excleTargetPath);
            if (File.Exists(sourcePath))
            {
                File.Move(sourcePath, excleTargetPath +"\\"+ excleSourceName);
            }
            //TODO:制作xml，并打包成BFR格式 
        }

        //随机生成6位数字字母
        static public string RoundNW(int num)
        {
            string str = "";
            Random random = new Random();
            
            for (int i=0;i< num;i++)
            {
                
                if(random.Next(2)==1)
                {
                    str += ((char)random.Next(65,91)).ToString();
                }
                else
                {
                    str += random.Next(10);
                }
            }
            return str;
        }

    }
}
