using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WK.Libraries.BetterFolderBrowserNS;
using System.Diagnostics;

namespace FileSearch_EX
{
    public partial class test : Form
    {
        public test()
        {
            InitializeComponent();
        }

        // 파일을 찾을 경로 선택
        private void button1_Click(object sender, EventArgs e)
        {
            BetterFolderBrowser ex = new BetterFolderBrowser();
            ex.Multiselect = false;
            if (ex.ShowDialog() == DialogResult.OK)
            {
                this.textBox1.Text = ex.SelectedPath;  // 파일 검색 경로
            }
        }

        // 파일 검색 및 내용 확인
        public async void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;

            string searchDirectory = textBox1.Text;
            string searchTerm = textBox2.Text;
            List<string> resultFiles = new List<string>();

            if (string.IsNullOrWhiteSpace(searchDirectory))
            {
                MessageBox.Show("경로를 선택해 주세요.");
                button2.Enabled = true;
                return;
            }
            await Task.Run(() =>
            {
                try
                {
                    var files = string.IsNullOrWhiteSpace(searchTerm)
                        ? FastDirectoryEnumerator.DirectorySearch(searchDirectory, "*.*", 0).ToList() // 모든 파일 검색
                        : FastDirectoryEnumerator.DirectorySearch(searchDirectory, "*.txt", 0).ToList(); // 텍스트 파일만 검색

                    files.ForEach(file =>
                    {
                        try
                        {
                            string fileName = file.FullName;

                            if (!string.IsNullOrWhiteSpace(searchTerm))
                            {
                                string fileContent = File.ReadAllText(fileName);
                                if (fileContent.Contains(searchTerm))
                                {
                                    resultFiles.Add(fileName);  // 검색어가 포함된 파일만 리스트에 추가
                                }
                            }
                            else
                            {
                                resultFiles.Add(fileName);
                            }
                        }
                        catch
                        {
                  
                        }
                    });
                }
                catch
                {
        
                }
            });

            this.Invoke((MethodInvoker)delegate
            {
                listBox1.Items.Clear();

                if (resultFiles.Count > 0)
                {
                    listBox1.Items.AddRange(resultFiles.ToArray());
                }
                else
                {
                    MessageBox.Show("검색어가 포함된 파일이 없습니다.");
                }

                button2.Enabled = true;
            });
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Process ExternalProcess = new Process();
            // 실행파일 경로
            ExternalProcess.StartInfo.FileName = listBox1.SelectedItem.ToString();
            // 실행시킬 파일 크기
            ExternalProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            // Process 시작
            ExternalProcess.Start();
            ExternalProcess.WaitForExit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BetterFolderBrowser ex = new BetterFolderBrowser();
            ex.Multiselect = false;
            if (ex.ShowDialog() == DialogResult.OK)
            {
                this.textBox3.Text = ex.SelectedPath;  // 파일 검색 경로
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;

            string targetPath = textBox3.Text;
            string selectedFile = listBox1.SelectedItem?.ToString(); // 선택된 파일 가져오기

            if (string.IsNullOrWhiteSpace(targetPath))
            {
                MessageBox.Show("경로를 선택해 주세요.");
                button3.Enabled = true;
                return;
            } else if (string.IsNullOrWhiteSpace(selectedFile))
            {
                MessageBox.Show("이동할 파일을 선택해 주세요");
                button3.Enabled = true;
                return;
            }

            await Task.Run(() =>
            {
                try
                {
                    // 파일 이동
                    string fileName = Path.GetFileName(selectedFile);
                    string destinationPath = Path.Combine(targetPath, fileName);
                    File.Move(selectedFile, destinationPath);
                    MessageBox.Show("파일을 이동하였습니다");
                    listBox1.Refresh();
                }
                catch
                {
   
                }
            });     
            button3.Enabled = true;
        }
    }
}
