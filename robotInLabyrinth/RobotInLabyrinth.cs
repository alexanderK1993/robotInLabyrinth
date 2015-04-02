using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace robotInLabyrinth
{
    /// <summary>
    /// Главный класс программы
    /// </summary>
    public partial class RobotInLabyrinth : Form
    {
        int countAnswer = 0;
        /// <summary>
        /// Игнорировать нажатие на рабиокнопку
        /// </summary>
        bool missCheck=false;
        int lastIndexFullWay=0;
        int lastIndexAnswer=0;
        double[,] valuesRating = new double[4, 4];
        /// <summary>
        /// Содержит полный путь найденный методом ветвей и границ
        /// </summary>
        List<Point> branchAndBoundMethodDetails;

        /// <summary>
        /// Содержит координаты решения методом ветвей и границ
        /// </summary>
        List<Point> branchAndBoundMethodAnswer;

        /// <summary>
        /// Содержит полный путь найденный встречным поиском
        /// </summary>
        List<Point> counterclaimSearchDetails;

        /// <summary>
        /// Содержит координаты решения встречным поиском
        /// </summary>
        List<Point> counterclaimSearchAnswer;
        /// <summary>
        /// Содержит полный путь найденный поиском от наилучшего частичного пути
        /// </summary>
        List<Point> searchFromBestPartialPathDetails;

        /// <summary>
        /// Содержит координаты решения поиском от наилучшего частичного пути
        /// </summary>
        List<Point> searchFromBestPartialPathAnswer;

        /// <summary>
        /// Содержит полный путь найденный методом поиска в глубину
        /// </summary>
        List<Point> depthFirstSearchDetails;

        /// <summary>
        /// Содержит координаты решения поиском в глубину
        /// </summary>
        List<Point> depthFirstSearchAnswer;

        /// <summary>
        /// Содержит полный путь найденный методом
        /// </summary>
        List<Point> breadthFirstSearchDetails;

        /// <summary>
        /// Содержит координаты решения поиском в глубину
        /// </summary>
        List<Point> breadthFirstSearchAnswer;

        /// <summary>
        /// лабиринт
        /// </summary>
        Labyrinth labyrinth;

        /// <summary>
        /// высота лабиринта
        /// </summary>
        int height;

        /// <summary>
        /// ширина лабиринта
        /// </summary>
        int width;

        /// <summary>
        /// Графическая поверхность
        /// </summary>
        Graphics g;

        /// <summary>
        /// Изображения для построения графики
        /// </summary>
        Image[] image;

        /// <summary>
        /// размер клетки лабиринта
        /// </summary>
        int size;

        /// <summary>
        /// вход и выход из лабиринта
        /// </summary>
        Point[] exit = new Point[2];

        //с какой вероятностью будет поставлена горизонтальная стена
        const int CHANCE_CREATE_HORIZONTAL_WALL = 100;

        Random rnd = new Random();
        /// <summary>
        /// Конструктор
        /// </summary>
        public RobotInLabyrinth()
        {
            InitializeComponent();
        }

        /// <summary>
        /// сгенерировать лабиринт
        /// </summary>
        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            radioButtonWay1.Enabled = false;
            radioButtonWay2.Enabled = false;
            radioButtonWay3.Enabled = false;
            radioButtonWay4.Enabled = false;
            buttonAnswer.Enabled = false;
            buttonBreadthFirstSearchRefresh.Enabled = false;
            buttonBranchAndBoundMethod.Enabled = false;
            buttonDepthFirstSearchRefresh.Enabled = false;
            buttonSearchFromBestPartialPathRefresh.Enabled = false;
            width = (int)numericUpDownWidthLabyrinth.Value;
            height = (int)numericUpDownHeightLabyrinth.Value;
            labyrinth = new Labyrinth(height, width, trackBar1.Value);
            labyrinth.GenerateLabyrinth();
            size = imageList.ImageSize.Height;
      
            labyrinth.DrawLabyrinth(g, image, size);
            radioButtonLeft.Checked = true;
            radioButtonLeftExit.Checked = true;
            numericUpDownNumberCell.Maximum = width - 1;
            numericUpDownNumberCellExit.Maximum = width - 1;
            buttonGenerateExit.Enabled = true;
            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
            labelFullWay.Visible = false;
            labelResult.Visible = false;
            listBoxDetails.Visible = false;
            listBoxResult.Visible = false;

            labelDepth1.Text = "0";
            labelDepth2.Text = "0";
            labelDepth3.Text = "0";
            labelDepth4.Text = "0";
            labelWay1.Text = "0";
            labelWay2.Text = "0";
            labelWay3.Text = "0";
            labelWay4.Text = "0";
            labelNodes1.Text = "0";
            labelNodes2.Text = "0";
            labelNodes3.Text = "0";
            labelNode4.Text = "0";
            labelSearch1.Text = "0";
            labelSearch2.Text = "0";
            labelSearch3.Text = "0";
            labelSearch4.Text = "0";
        }

        /// <summary>
        /// изменяет рисунок лабиринта
        /// </summary>
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            labyrinth.ChanceCreateVerticalWall = trackBar1.Value;
        }

        /// <summary>
        /// Сделать вход и выход в лабиринте
        /// </summary>
        private void buttonGenerateExit_Click(object sender, EventArgs e)
        {
            buttonAnswer.Enabled = true;
            countAnswer++;
            if (radioButtonLeft.Checked)
            {
                labyrinth.Entry = new Point(1, (int)numericUpDownNumberCell.Value);
            }
            else
                if (radioButtonRight.Checked)
                {
                    labyrinth.Entry = new Point((int)numericUpDownWidthLabyrinth.Value
                        , (int)numericUpDownNumberCell.Value);
                }
                else
                    if (radioButtonUp.Checked)
                    {
                        labyrinth.Entry = new Point((int)numericUpDownNumberCell.Value, 1);
                    }
                    else
                        if (radioButtonDown.Checked)
                        {
                            labyrinth.Entry = new Point((int)numericUpDownNumberCell.Value
                                , (int)numericUpDownHeightLabyrinth.Value);
                        }

            if (radioButtonLeftExit.Checked)
            {
                labyrinth.Exit = new Point(1, (int)numericUpDownNumberCellExit.Value);
            }
            else
                if (radioButtonRightExit.Checked)
                {
                    labyrinth.Exit = new Point((int)numericUpDownWidthLabyrinth.Value,
                        (int)numericUpDownNumberCellExit.Value);
                }
                else
                    if (radioButtonUpExit.Checked)
                    {
                        labyrinth.Exit = new Point((int)numericUpDownNumberCellExit.Value, 1);
                    }
                    else
                        if (radioButtonDownExit.Checked)
                        {
                            labyrinth.Exit = new Point((int)numericUpDownNumberCellExit.Value,
                                (int)numericUpDownHeightLabyrinth.Value);
                        }
            labyrinth.GenerateExit(g, image, size);
            groupBoxFindExit.Enabled = true;

           
            listBoxDetails.Items.Clear();
            labelFullWay.Visible = false;
            labelResult.Visible = false;
            listBoxDetails.Visible = false;
            listBoxResult.Visible = false;

            
        }

        /// <summary>
        /// Инициализауия программы при загрузке формы
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            g = panelLabyrinth.CreateGraphics();
            image = new Image[imageList.Images.Count];
            for (int i = 0; i < imageList.Images.Count; i++)
            { image[i] = imageList.Images[i]; }

        }

        /// <summary>
        /// Кнопка изменяющая положение входа в лабиринт
        /// </summary>
        private void radioButtonLeft_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownNumberCell.Maximum = height - 1;
        }

        /// <summary>
        /// Кнопка изменяющая положение входа в лабиринт
        /// </summary>
        private void radioButtonRight_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownNumberCell.Maximum = height - 1;
        }

        /// <summary>
        /// Кнопка изменяющая положение входа в лабиринт
        /// </summary>
        private void radioButtonUp_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownNumberCell.Maximum = width - 1;
        }

        /// <summary>
        /// Кнопка изменяющая положение входа в лабиринт
        /// </summary>
        private void radioButtonDown_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownNumberCell.Maximum = width - 1;
        }

        /// <summary>
        /// Кнопка изменяющая положение выхода из лабиринта
        /// </summary>
        private void radioButtonLeftExit_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownNumberCellExit.Maximum = height - 1;
        }

        /// <summary>
        /// Кнопка изменяющая положение выхода из лабиринта
        /// </summary>
        private void radioButtonRightExit_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownNumberCellExit.Maximum = height - 1;
        }

        private void radioButtonUpExit_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownNumberCellExit.Maximum = width - 1;
        }

        /// <summary>
        /// Кнопка изменяющая положение выхода из лабиринта
        /// </summary>
        private void radioButtonDownExit_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDownNumberCellExit.Maximum = width - 1;

        }

        /// <summary>
        /// Перерисовать лабиринт,вывести метод поиска в глубину
        /// </summary>
        private void buttonDepthFirstSearchRefresh_Click(object sender, EventArgs e)
        {
            labyrinth.PositionRobot = labyrinth.Entry;
            labyrinth.DrawFullWay(g, image, size, depthFirstSearchDetails, depthFirstSearchAnswer, labyrinth.PositionRobot);
            listBoxDetails.Items.Clear();
            listBoxResult.Items.Clear();
            foreach (var coordinate in depthFirstSearchDetails)
            {
                listBoxDetails.Items.Add(coordinate);
            }
            foreach (var coordinate in depthFirstSearchAnswer)
            {
                listBoxResult.Items.Add(coordinate);
            }
        
            listBoxDetails.SelectedIndex = 0;
            listBoxResult.SelectedIndex = 0;
            listBoxDetails.Select();
            radioButtonWay2.Checked = true;
        }

        /// <summary>
        /// При изменение индекса внутри списка полного пути
        /// </summary>
        private void listBoxDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            Point val = (Point)listBoxDetails.Items[listBoxDetails.SelectedIndex];
            labyrinth.RedrawRobot(g, image, size, labyrinth.PositionRobot, val);
            List<Point> list=new List<Point>();
            foreach(var point in listBoxDetails.Items)
            {
                list.Add((Point)point);
            }
            if (lastIndexFullWay + 1 == listBoxDetails.SelectedIndex)
            {
               
            }
            else
            {
                if (lastIndexFullWay <= listBoxDetails.SelectedIndex)
                {
                    labyrinth.DrawPartWay(g, image, size, list, labyrinth.PositionRobot);
                }
                else
                {
                    if (radioButtonWay1.Checked)
                    {
                        labyrinth.DrawFullWay(g, image, size, breadthFirstSearchDetails, breadthFirstSearchAnswer, labyrinth.PositionRobot);
                    }
                    else
                        if (radioButtonWay2.Checked)
                        {
                            labyrinth.DrawFullWay(g, image, size, depthFirstSearchDetails, depthFirstSearchAnswer, labyrinth.PositionRobot);
                        }
                        else
                            if (radioButtonWay3.Checked)
                        {
                            labyrinth.DrawFullWay(g, image, size, searchFromBestPartialPathDetails, searchFromBestPartialPathAnswer, labyrinth.PositionRobot);
                        }
                        else
                                if (radioButtonWay4.Checked)
                                {
                                    labyrinth.DrawFullWay(g, image, size, breadthFirstSearchDetails, breadthFirstSearchAnswer, labyrinth.PositionRobot);
                                }
                  }
            }
            lastIndexFullWay = listBoxDetails.SelectedIndex;
        }

        /// <summary>
        /// При изменение индекса внутри списка конечного решения
        /// </summary>
        private void listBoxResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            Point val = (Point)listBoxResult.Items[listBoxResult.SelectedIndex];
            labyrinth.RedrawRobot(g, image, size, labyrinth.PositionRobot, val);
            List<Point> list = new List<Point>();
            foreach (var point in listBoxResult.Items)
            {
                list.Add((Point)point);
            }
                    if (radioButtonWay1.Checked)
                    {
                        labyrinth.DrawAnswer(g, image, size, breadthFirstSearchDetails, breadthFirstSearchAnswer, labyrinth.PositionRobot);
                    }
                    else
                        if (radioButtonWay2.Checked)
                        {
                            labyrinth.DrawAnswer(g, image, size, depthFirstSearchDetails, depthFirstSearchAnswer, labyrinth.PositionRobot);
                        }
                        else
                            if (radioButtonWay3.Checked)
                            {
                                labyrinth.DrawAnswer(g, image, size, searchFromBestPartialPathDetails, searchFromBestPartialPathAnswer, labyrinth.PositionRobot);
                            }
                            else
                                if (radioButtonWay4.Checked)
                                {
                                    labyrinth.DrawAnswer(g, image, size, breadthFirstSearchDetails, breadthFirstSearchAnswer, labyrinth.PositionRobot);
                                }
                
            
            lastIndexAnswer = listBoxResult.SelectedIndex;
        }

        /// <summary>
        /// Перерисовать лабиринт,вывести метод поиска в ширину
        /// </summary>
        private void buttonBreadthFirstSearchRefresh_Click(object sender, EventArgs e)
        {
            labyrinth.PositionRobot = labyrinth.Entry;
            labyrinth.DrawFullWay(g, image, size, breadthFirstSearchDetails, breadthFirstSearchAnswer, labyrinth.PositionRobot);
            listBoxDetails.Items.Clear();
            listBoxResult.Items.Clear();
            foreach (var coordinate in breadthFirstSearchDetails)
            {
                listBoxDetails.Items.Add(coordinate);
            }
            foreach (var coordinate in breadthFirstSearchAnswer)
            {
                listBoxResult.Items.Add(coordinate);
            }
            labelFullWay.Visible = true;
            labelResult.Visible = true;
            listBoxDetails.Visible = true;
            listBoxResult.Visible = true;
          
            listBoxDetails.SelectedIndex = 0;
            listBoxResult.SelectedIndex = 0;
            listBoxDetails.Select();
            radioButtonWay1.Checked = true;
        }

        /// <summary>
        /// Перерисовать лабиринт,вывести метод поиска от наилучшего частичного пути
        /// </summary>
        private void buttonSearchFromBestPartialPathRefresh_Click(object sender, EventArgs e)
        {
            labyrinth.PositionRobot = labyrinth.Entry;
            labyrinth.DrawFullWay(g, image, size, searchFromBestPartialPathDetails, searchFromBestPartialPathAnswer, labyrinth.PositionRobot);
            listBoxDetails.Items.Clear();
            listBoxResult.Items.Clear();
            foreach (var coordinate in searchFromBestPartialPathDetails)
            {
                listBoxDetails.Items.Add(coordinate);
            }
            foreach (var coordinate in searchFromBestPartialPathAnswer)
            {
                listBoxResult.Items.Add(coordinate);
            }
           
            listBoxDetails.SelectedIndex = 0;
            listBoxResult.SelectedIndex = 0;
            listBoxDetails.Select();
            radioButtonWay3.Checked = true;
           
        }

        /// <summary>
        /// Событие при переключении выбранного метода
        /// </summary>
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if ((radioButtonWay1.Checked)&&(!missCheck))
            {
                labyrinth.PositionRobot = labyrinth.Entry;
                labyrinth.DrawLabyrinth(g, image, size);
                labyrinth.DrawFullWay(g, image, size, breadthFirstSearchDetails, breadthFirstSearchAnswer, labyrinth.PositionRobot);
                listBoxDetails.Items.Clear();
                listBoxResult.Items.Clear();
                foreach (var coordinate in breadthFirstSearchDetails)
                {
                    listBoxDetails.Items.Add(coordinate);
                }
                foreach (var coordinate in breadthFirstSearchAnswer)
                {
                    listBoxResult.Items.Add(coordinate);
                }
                labelFullWay.Visible = true;
                labelResult.Visible = true;
                listBoxDetails.Visible = true;
                listBoxResult.Visible = true;
              
                listBoxDetails.SelectedIndex = 0;
                listBoxResult.SelectedIndex = 0;
                listBoxDetails.Select();
               
            }
            missCheck = false;

        }

        /// <summary>
        /// Событие при переключении выбранного метода
        /// </summary>
        private void radioButtonWay2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonWay2.Checked)
            {
                labyrinth.PositionRobot = labyrinth.Entry;
                labyrinth.DrawLabyrinth(g, image, size);
                labyrinth.DrawFullWay(g, image, size, depthFirstSearchDetails, depthFirstSearchAnswer, labyrinth.PositionRobot);
                listBoxDetails.Items.Clear();
                listBoxResult.Items.Clear();
                foreach (var coordinate in depthFirstSearchDetails)
                {
                    listBoxDetails.Items.Add(coordinate);
                }
                foreach (var coordinate in depthFirstSearchAnswer)
                {
                    listBoxResult.Items.Add(coordinate);
                }

                listBoxDetails.SelectedIndex = 0;
                listBoxResult.SelectedIndex = 0;
                listBoxDetails.Select();
            }
        }

        /// <summary>
        /// Событие при переключении выбранного метода
        /// </summary>
        private void radioButtonWay3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonWay3.Checked)
            {
                labyrinth.PositionRobot = labyrinth.Entry;
                labyrinth.DrawLabyrinth(g, image, size);
                labyrinth.DrawFullWay(g, image, size, searchFromBestPartialPathDetails, searchFromBestPartialPathAnswer, labyrinth.PositionRobot);
                listBoxDetails.Items.Clear();
                listBoxResult.Items.Clear();
                foreach (var coordinate in searchFromBestPartialPathDetails)
                {
                    listBoxDetails.Items.Add(coordinate);
                }
                foreach (var coordinate in searchFromBestPartialPathAnswer)
                {
                    listBoxResult.Items.Add(coordinate);
                }
              
                listBoxDetails.SelectedIndex = 0;
                listBoxResult.SelectedIndex = 0;
                listBoxDetails.Select();
            }
        }

        /// <summary>
        /// Событие при переключении выбранного метода
        /// </summary>
        private void radioButtonWay4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonWay4.Checked)
            {
                labyrinth.PositionRobot = labyrinth.Entry;
                labyrinth.DrawLabyrinth(g, image, size);
                labyrinth.DrawFullWay(g, image, size, branchAndBoundMethodDetails, branchAndBoundMethodAnswer, labyrinth.PositionRobot);
                listBoxDetails.Items.Clear();
                listBoxResult.Items.Clear();
                foreach (var coordinate in branchAndBoundMethodDetails)
                {
                    listBoxDetails.Items.Add(coordinate);
                }
                foreach (var coordinate in branchAndBoundMethodAnswer)
                {
                    listBoxResult.Items.Add(coordinate);
                }

                listBoxDetails.SelectedIndex = 0;
                listBoxResult.SelectedIndex = 0;
                listBoxDetails.Select();

            }
        }

        /// <summary>
        /// Перерисовать лабиринт,вывести стратегию ветвей и границ
        /// </summary>
        private void buttonBranchAndBoundMethod_Click(object sender, EventArgs e)
        {
            labyrinth.PositionRobot = labyrinth.Entry;
            labyrinth.DrawFullWay(g, image, size, branchAndBoundMethodDetails, branchAndBoundMethodAnswer, labyrinth.PositionRobot);
            listBoxDetails.Items.Clear();
            listBoxResult.Items.Clear();
            foreach (var coordinate in branchAndBoundMethodDetails)
            {
                listBoxDetails.Items.Add(coordinate);
            }
            foreach (var coordinate in branchAndBoundMethodAnswer)
            {
                listBoxResult.Items.Add(coordinate);
            }

            listBoxDetails.SelectedIndex = 0;
            listBoxResult.SelectedIndex = 0;
            listBoxDetails.Select();
            radioButtonWay4.Checked = true;
        }

        /// <summary>
        /// Найти решение всеми методами
        /// </summary>
        private void buttonAnswer_Click(object sender, EventArgs e)
        {
            BreadthFirstSearch breadthFirstSearch = new BreadthFirstSearch(labyrinth.Entry, labyrinth.Exit);
            double[] rating = new double[4];
            breadthFirstSearch.SearchAnswer(labyrinth.GetLabyrinth, out breadthFirstSearchDetails, out breadthFirstSearchAnswer, out rating);

            buttonBreadthFirstSearchRefresh.Enabled = true;
            labelDepth1.Text = rating[0].ToString();
            labelWay1.Text = rating[1].ToString();
            labelNodes1.Text = rating[2].ToString();
            labelSearch1.Text = rating[3].ToString("f2");

            valuesRating[0, 0] += rating[0];
            valuesRating[0, 1] += rating[1];
            valuesRating[0, 2] += rating[2];
            valuesRating[0, 3] += rating[3];
            labelDepthSr1.Text = (valuesRating[0, 0] / countAnswer).ToString("f2");
            labelWaySr1.Text = (valuesRating[0, 1] / countAnswer).ToString("f2");
            labelNodesSr1.Text = (valuesRating[0, 2] / countAnswer).ToString("f2");
            labelSearchSr1.Text = (valuesRating[0, 3] / countAnswer).ToString("f2");

            DepthFirstSearch depthFirstSearch = new DepthFirstSearch(labyrinth.Entry, labyrinth.Exit);
            depthFirstSearch.SearchAnswer(
                labyrinth.GetLabyrinth, out depthFirstSearchDetails, out depthFirstSearchAnswer, out rating);
            buttonDepthFirstSearchRefresh.Enabled = true;
            labelDepth2.Text = rating[0].ToString();
            labelWay2.Text = rating[1].ToString();
            labelNodes2.Text = rating[2].ToString();
            labelSearch2.Text = rating[3].ToString("f2");

            valuesRating[1, 0] += rating[0];
            valuesRating[1, 1] += rating[1];
            valuesRating[1, 2] += rating[2];
            valuesRating[1, 3] += rating[3];
            labelDepthSr2.Text = (valuesRating[1, 0] / countAnswer).ToString("f2");
            labelWaySr2.Text = (valuesRating[1, 1] / countAnswer).ToString("f2");
            labelNodesSr2.Text = (valuesRating[1, 2] / countAnswer).ToString("f2");
            labelSearchSr2.Text = (valuesRating[1, 3] / countAnswer).ToString("f2");

            SearchFromBestPartialPath searchFromBestPartialPath = new SearchFromBestPartialPath(labyrinth.Entry, labyrinth.Exit);
            searchFromBestPartialPath.SearchAnswer(
                labyrinth.GetLabyrinth, out searchFromBestPartialPathDetails,
                out searchFromBestPartialPathAnswer, out rating);
            buttonSearchFromBestPartialPathRefresh.Enabled = true;
            labelDepth3.Text = rating[0].ToString();
            labelWay3.Text = rating[1].ToString();
            labelNodes3.Text = rating[2].ToString();
            labelSearch3.Text = rating[3].ToString("f2");

            valuesRating[2, 0] += rating[0];
            valuesRating[2, 1] += rating[1];
            valuesRating[2, 2] += rating[2];
            valuesRating[2, 3] += rating[3];
            labelDepthSr3.Text = (valuesRating[2, 0] / countAnswer).ToString("f2");
            labelWaySr3.Text = (valuesRating[2, 1] / countAnswer).ToString("f2");
            labelNodesSr3.Text = (valuesRating[2, 2] / countAnswer).ToString("f2");
            labelSearchSr3.Text = (valuesRating[2, 3] / countAnswer).ToString("f2");

            BranchAndBoundMethod branchAndBoundMethod = new BranchAndBoundMethod(labyrinth.Entry, labyrinth.Exit);
            branchAndBoundMethod.SearchAnswer(
                labyrinth.GetLabyrinth, out branchAndBoundMethodDetails,
                out branchAndBoundMethodAnswer, out rating);
            buttonBranchAndBoundMethod.Enabled = true;
            labelDepth4.Text = rating[0].ToString();
            labelWay4.Text = rating[1].ToString();
            labelNode4.Text = rating[2].ToString();
            labelSearch4.Text = rating[3].ToString("f2");

            valuesRating[3, 0] += rating[0];
            valuesRating[3, 1] += rating[1];
            valuesRating[3, 2] += rating[2];
            valuesRating[3, 3] += rating[3];
            labelDepthSr4.Text = (valuesRating[3, 0] / countAnswer).ToString("f2");
            labelWaySr4.Text = (valuesRating[3, 1] / countAnswer).ToString("f2");
            labelNodesSr4.Text = (valuesRating[3, 2] / countAnswer).ToString("f2");
            labelSearchSr4.Text = (valuesRating[3, 3] / countAnswer).ToString("f2");

            missCheck = true;
            radioButtonWay1.Checked = true;



            labyrinth.PositionRobot = labyrinth.Entry;
            labyrinth.DrawFullWay(g, image, size, breadthFirstSearchDetails, breadthFirstSearchAnswer, labyrinth.PositionRobot);
            listBoxDetails.Items.Clear();
            listBoxResult.Items.Clear();
            foreach (var coordinate in breadthFirstSearchDetails)
            {
                listBoxDetails.Items.Add(coordinate);
            }
            foreach (var coordinate in breadthFirstSearchAnswer)
            {
                listBoxResult.Items.Add(coordinate);
            }
            labelFullWay.Visible = true;
            labelResult.Visible = true;
            listBoxDetails.Visible = true;
            listBoxResult.Visible = true;

            listBoxDetails.SelectedIndex = 0;
            listBoxResult.SelectedIndex = 0;
            listBoxDetails.Select();

            radioButtonWay1.Enabled = true;
            radioButtonWay2.Enabled = true;
            radioButtonWay3.Enabled = true;
            radioButtonWay4.Enabled = true;
        }

        /// <summary>
        ///Происходит при клике на лист с конечным результатом 
        /// </summary>
        private void listBoxResult_Enter(object sender, EventArgs e)
        {
            labyrinth.DrawLabyrinth(g, image, size);
        }
    }
}
