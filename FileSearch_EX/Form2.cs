using System;
using System.IO;
using System.Windows.Forms;

namespace FileSearch_EX
{
    public partial class Form2 : Form
    {
        public Form2(string filePath)
        {
            InitializeComponent();
            LoadFileContent(filePath);
        }

        private void LoadFileContent(string filePath)
        {
            try
            {
                string content = File.ReadAllText(filePath);
                textBox1.Text = content;  // 파일 내용을 TextBox에 표시
            }
            catch (Exception ex)
            {
                MessageBox.Show($"파일을 읽는 중 오류가 발생했습니다: {ex.Message}");
            }
        }
    }
}
