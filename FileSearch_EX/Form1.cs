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

        // 파일 검색 및 내용 확인 메서드
        private async void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;

            string searchDirectory = textBox1.Text;  // 파일을 검색할 경로
            string searchTerm = textBox2.Text;       // 검색어
            List<string> resultFiles = new List<string>();

            if (string.IsNullOrWhiteSpace(searchDirectory) || string.IsNullOrWhiteSpace(searchTerm))
            {
                MessageBox.Show("검색어를 입력해주세요.");
                button2.Enabled = true;
                return;
            }
            // 비동기 작업으로 파일 검색 수행
            await Task.Run(() =>
            {
                var files = Directory.GetFiles(searchDirectory, "*.txt", SearchOption.AllDirectories).ToList();

                files.ForEach(file =>
                {
                    // 파일 내용을 읽어서 검색어가 포함된 파일을 필터링
                    string fileContent = File.ReadAllText(file);

                    if (fileContent.Contains(searchTerm))
                    {
                        resultFiles.Add(file);  // 검색어가 포함된 파일 추가
                    }
                });
            });
            this.Invoke((MethodInvoker)delegate
            {
                listBox1.Items.Clear();  // 기존 리스트 초기화
                if (resultFiles.Count > 0)
                {
                    listBox1.Items.AddRange(resultFiles.ToArray());  // 결과 파일 리스트에 추가
                }
                else
                {
                    MessageBox.Show("검색어가 포함된 파일이 없습니다.");
                }
            });

            button2.Enabled = true;
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            Process ExternalProcess = new Process();
            // 실행파일 경로
            ExternalProcess.StartInfo.FileName = listBox1.Items[0].ToString();
            // 실행시킬 파일 크기
            ExternalProcess.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            //Process 시작
            ExternalProcess.Start();
            ExternalProcess.WaitForExit();

        }
    }
}
