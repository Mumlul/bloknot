using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Блокнотик
{
    public partial class Form1 :Form
    {
        string currentFile = "";

        public Form1()
        {
            InitializeComponent();
            InitializeMenuStrip();
            InitializeStatusStrip();

        }

        private void InitializeMenuStrip()
        {
            MenuStrip menuStrip = new MenuStrip();

            // Файл
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("Файл");
            ToolStripMenuItem openMenuItem = new ToolStripMenuItem("Открыть...");
            ToolStripMenuItem saveMenuItem = new ToolStripMenuItem("Сохранить");
            ToolStripMenuItem saveAsMenuItem = new ToolStripMenuItem("Сохранить как...");
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("Выход");

            // Правка
            ToolStripMenuItem editMenu = new ToolStripMenuItem("Правка");
            ToolStripMenuItem copyMenuItem = new ToolStripMenuItem("Копировать");
            ToolStripMenuItem cutMenuItem = new ToolStripMenuItem("Вырезать");
            ToolStripMenuItem pasteMenuItem = new ToolStripMenuItem("Вставить");
            ToolStripMenuItem fontMenuItem = new ToolStripMenuItem("Шрифт...");
            ToolStripMenuItem searchMenuItem = new ToolStripMenuItem("Найти...");
            ToolStripMenuItem replaceMenuItem = new ToolStripMenuItem("Заменить...");
            ToolStripMenuItem data = new ToolStripMenuItem("Дата и время");

            // Вставка
            ToolStripMenuItem insertMenu = new ToolStripMenuItem("Вставка");
            ToolStripMenuItem insertImageMenuItem = new ToolStripMenuItem("Вставить изображение...");

            // Настройка событий
            openMenuItem.Click += OpenMenuItem_Click;
            saveMenuItem.Click += SaveMenuItem_Click;
            saveAsMenuItem.Click += SaveAsMenuItem_Click;
            exitMenuItem.Click += ExitMenuItem_Click;

            copyMenuItem.Click += CopyMenuItem_Click;
            cutMenuItem.Click += CutMenuItem_Click;
            pasteMenuItem.Click += PasteMenuItem_Click;
            fontMenuItem.Click += FontMenuItem_Click;
            searchMenuItem.Click += SearchMenuItem_Click;
            replaceMenuItem.Click += ReplaceMenuItem_Click;
            insertImageMenuItem.Click += InsertImageMenuItem_Click;
            data.Click += DD;

            // Добавление пунктов меню
            fileMenu.DropDownItems.Add(openMenuItem);
            fileMenu.DropDownItems.Add(saveMenuItem);
            fileMenu.DropDownItems.Add(saveAsMenuItem);
            fileMenu.DropDownItems.Add(exitMenuItem);

            editMenu.DropDownItems.Add(copyMenuItem);
            editMenu.DropDownItems.Add(cutMenuItem);
            editMenu.DropDownItems.Add(pasteMenuItem);
            editMenu.DropDownItems.Add(fontMenuItem);
            editMenu.DropDownItems.Add(searchMenuItem);
            editMenu.DropDownItems.Add(replaceMenuItem);
            editMenu.DropDownItems.Add(data);

            insertMenu.DropDownItems.Add(insertImageMenuItem);

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(editMenu);
            menuStrip.Items.Add(insertMenu);

            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip); // Добавляем меню первым
        }

        private void DD(object sender, EventArgs e)
        {
            richTextBox2.Text += DateTime.Now;
        }
        private void InitializeStatusStrip()
        {
            statusStrip1 = new StatusStrip();

            // Создаем метки для отображения информации
            toolStripStatusLabelLines = new ToolStripStatusLabel { Text = "Строк: 0" };
            toolStripStatusLabelChars = new ToolStripStatusLabel { Text = "Символов: 0" };

            // Добавляем метки в StatusStrip
            statusStrip1.Items.Add(toolStripStatusLabelLines);
            statusStrip1.Items.Add(toolStripStatusLabelChars);

            this.Controls.Add(statusStrip1); // Добавляем StatusStrip последним
            statusStrip1.Dock = DockStyle.Bottom; // Прикрепляем к нижней части формы
        }
        private StatusStrip statusStrip1; // Объявление StatusStrip
        private ToolStripStatusLabel toolStripStatusLabelLines; // Метка для строк
        private ToolStripStatusLabel toolStripStatusLabelChars; // Метка для символов

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentFile = openFileDialog.FileName;
                richTextBox2.Text = File.ReadAllText(currentFile);
            }
        }

        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFile))
            {
                SaveAsMenuItem_Click(sender, e);
            }
            else
            {
                File.WriteAllText(currentFile, richTextBox2.Text);
            }
        }

        private void SaveAsMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentFile = saveFileDialog.FileName;
                File.WriteAllText(currentFile, richTextBox2.Text);
            }
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CopyMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox2.Copy();
        }

        private void CutMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox2.Cut();
        }

        private void PasteMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox2.Paste();
        }

        private void FontMenuItem_Click(object sender, EventArgs e)
        {
            using (FontDialog fontDialog = new FontDialog())
            {
                if (richTextBox2.SelectionLength > 0)
                {
                    fontDialog.Font = richTextBox2.SelectionFont;
                }
                else
                {
                    fontDialog.Font = richTextBox2.Font;
                }

                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    if (richTextBox2.SelectionLength > 0)
                    {
                        richTextBox2.SelectionFont = fontDialog.Font;
                    }
                    else
                    {
                        richTextBox2.Font = fontDialog.Font;
                    }
                }
            }
        }

        private void SearchMenuItem_Click(object sender, EventArgs e)
        {
            using (Form searchForm = new Form())
            {
                searchForm.Text = "Поиск";
                searchForm.Size = new Size(300, 100);

                Label label = new Label { Text = "Введите текст для поиска:", Location = new Point(10, 10) };
                TextBox searchBox = new TextBox { Location = new Point(10, 30), Width = 200 };
                Button searchButton = new Button { Text = "Найти", Location = new Point(220, 30) };

                searchButton.Click += (s, ev) =>
                {
                    string searchText = searchBox.Text;
                    if (!string.IsNullOrEmpty(searchText))
                    {
                        int index = richTextBox2.Find(searchText, richTextBox2.SelectionStart + richTextBox2.SelectionLength, RichTextBoxFinds.None);
                        if (index < 0)
                        {
                            MessageBox.Show("Текст не найден.", "Поиск", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                };

                searchForm.Controls.Add(label);
                searchForm.Controls.Add(searchBox);
                searchForm.Controls.Add(searchButton);
                searchForm.ShowDialog();
            }
        }

        private void ReplaceMenuItem_Click(object sender, EventArgs e)
        {
            using (Form replaceForm = new Form())
            {
                replaceForm.Text = "Замена";
                replaceForm.Size = new Size(350, 150);

                Label findLabel = new Label { Text = "Найти:", Location = new Point(10, 10) };
                TextBox findBox = new TextBox { Location = new Point(10, 30), Width = 200 };

                Label replaceLabel = new Label { Text = "Заменить на:", Location = new Point(10, 60) };
                TextBox replaceBox = new TextBox { Location = new Point(10, 80), Width = 200 };

                Button replaceButton = new Button { Text = "Заменить", Location = new Point(220, 30) };
                Button replaceAllButton = new Button { Text = "Заменить всё", Location = new Point(220, 60) };

                replaceButton.Click += (s, ev) =>
                {
                    string findText = findBox.Text;
                    string replaceText = replaceBox.Text;

                    if (!string.IsNullOrEmpty(findText))
                    {
                        int index = richTextBox2.Find(findText, richTextBox2.SelectionStart + richTextBox2.SelectionLength, RichTextBoxFinds.None);
                        if (index >= 0)
                        {
                            richTextBox2.SelectedText = replaceText;
                        }
                        else
                        {
                            MessageBox.Show("Текст не найден.", "Замена", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                };

                replaceAllButton.Click += (s, ev) =>
                {
                    string findText = findBox.Text;
                    string replaceText = replaceBox.Text;

                    if (!string.IsNullOrEmpty(findText))
                    {
                        string content = richTextBox2.Text;
                        content = Regex.Replace(content, Regex.Escape(findText), replaceText);
                        richTextBox2.Text = content;
                    }
                };

                replaceForm.Controls.Add(findLabel);
                replaceForm.Controls.Add(findBox);
                replaceForm.Controls.Add(replaceLabel);
                replaceForm.Controls.Add(replaceBox);
                replaceForm.Controls.Add(replaceButton);
                replaceForm.Controls.Add(replaceAllButton);
                replaceForm.ShowDialog();
            }
        }

        private void InsertImageMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Изображения|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Image image = Image.FromFile(openFileDialog.FileName);
                        Clipboard.SetImage(image);
                        richTextBox2.Paste(); // Вставляем изображение из буфера обмена
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при загрузке изображения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //Подсчет кол-ва строк и символов
        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            // Подсчитываем количество строк
            int lineCount = richTextBox2.Lines.Length;

            // Подсчитываем количество символов
            int charCount = richTextBox2.Text.Length;

            // Обновляем метки в StatusStrip
            toolStripStatusLabelLines.Text = $"Строк: {lineCount}";
            toolStripStatusLabelChars.Text = $"Символов: {charCount}";
        }
    }

    
}

