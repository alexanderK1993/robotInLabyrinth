using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace robotInLabyrinth
{
    /// <summary>
    /// Класс реализующий лабиринт
    /// </summary>
    class Labyrinth
    {
        /// <summary>
        /// с какой вероятностью будет поставлена горизонтальная стена
        /// </summary>
        private const int CHANCE_CREATE_HORIZONTAL_WALL = 100;

        /// <summary>
        /// с какой вероятностью будет поставлена вертикальная стена
        /// </summary>
        private int chanceCreateVerticalWall = 50;

        /// <summary>
        /// высота лабиринта
        /// </summary>
        private int height;

        /// <summary>
        /// ширина лабиринта
        /// </summary>
        private int width;

        /// <summary>
        /// массив ячеек лабиринта
        /// </summary>
        private int[,] labyrinth;

        /// <summary>
        /// максимальное количество множеств
        /// </summary>
        private int countQuantity;

        /// <summary>
        /// Текущая позиция робота в лабиринте
        /// </summary>
        Point positionRobot;

        /// <summary>
        /// выход из лабиринта
        /// </summary>
        Point exit = new Point();

        /// <summary>
        /// Вход в лабиринт
        /// </summary>
        Point entry = new Point();

        /// <summary>
        /// Конструктор
        /// </summary>
        public Labyrinth(int parHeight, int parWidth, int parChanceCreateVerticalWall)
        {
            chanceCreateVerticalWall = parChanceCreateVerticalWall;
            height = parHeight;
            width = parWidth;
        }
        Random rnd = new Random();

        /// <summary>
        /// выводит лабиринт на графической поверхности
        /// </summary>
        /// <param name="g">Куда выводить лабиринт</param>
        /// <param name="image">массив изображений из которых строится лабиринт</param>
        /// <param name="size">размер одной клетки лабиринта</param>
        public void DrawLabyrinth(Graphics g, Image[] image, int size)
        {
            g.Clear(Color.FromArgb(240, 240, 240));
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    if (labyrinth[i, j] == 0)
                    {
                        g.DrawImage(image[1], new Point(i * size + 15, j * size + 15));
                    }
                    else
                        g.DrawImage(image[0], new Point(i * size + 15, j * size + 15));
                }

            for (int i = 1; i < width + 1; i++)
            {
                g.DrawString(
                    i.ToString(),
                    new Font(new FontFamily("Helvetica"), 10, FontStyle.Regular, GraphicsUnit.Point)
                    , new SolidBrush(Color.Black)
                    , new PointF(size * i, 0)
                    );
            }
            for (int i = 1; i < height + 1; i++)
            {
                g.DrawString(
                   i.ToString(),
                   new Font(new FontFamily("Helvetica"), 10, FontStyle.Regular, GraphicsUnit.Point)
                   , new SolidBrush(Color.Black)
                   , new PointF(-1, size * i)
                   );
            }
        }

        /// <summary>
        /// Отображает графически весь путь пройденный алгоритмом
        /// </summary>
        /// <param name="g">графическая поверхность</param>
        /// <param name="image">изображения</param>
        /// <param name="size">размер изображений</param>
        /// <param name="fullWay">Массив точек представляющий полный путь</param>
        /// <param name="answer">Массив точек представляющий путь до выхода</param>
        /// <param name="positonRobot">текущее положение робота</param>
        public void DrawFullWay(Graphics g, Image[] image, int size, List<Point> fullWay, List<Point> answer, Point positionRobot)
        {

            //  DrawLabyrinth(g, image, size);
            int index = fullWay.FindIndex(x => x == positionRobot);
            int indexAnswer=0;
            for (int i = 0; i < index; i++)
            {
                if (answer.FindIndex(x => x == fullWay[i]) > -1)
                {
                    indexAnswer++;
                }
            }
            // DrawLabyrinth(g, image, size);
            for (int i = 0; i < index; i++)
            {
                g.DrawImage(image[6], new Point(fullWay[i].X * size, fullWay[i].Y * size));
            }
            int removedPoint;

            for (int i = index; i < fullWay.Count; i++)
            {
               removedPoint = answer.FindIndex(x => x == fullWay[i]);
                if (removedPoint == -1)
                    g.DrawImage(image[4], new Point(fullWay[i].X * size, fullWay[i].Y * size));
            }
            index = answer.FindIndex(x => x == positionRobot);
            for (int i = indexAnswer; i < answer.Count; i++)
            {

                g.DrawImage(image[5], new Point(answer[i].X * size, answer[i].Y * size));
            }
            RedrawRobot(g, image, size, positionRobot, positionRobot);
            RedrawExit(g, image, size);
            //  DrawPartWay(g, image, size, fullWay, positionRobot);
        }

        /// <summary>
        /// Перерисовывает часть отображаемого пути в лабиринте
        /// </summary>
        public void DrawPartWay(Graphics g, Image[] image, int size, List<Point> fullWay, Point positonRobot)
        {
            int index = fullWay.FindIndex(x => x == positionRobot);

            for (int i = 0; i < index + 1; i++)
            {
                g.DrawImage(image[6], new Point(fullWay[i].X * size, fullWay[i].Y * size));
            }
            RedrawRobot(g, image, size, positionRobot, positionRobot);
            RedrawExit(g, image, size);

        }

        /// <summary>
        /// Рисует путь найденного решения в лабиринте
        /// </summary>
        public void DrawAnswer(Graphics g, Image[] image, int size, List<Point> fullWay, List<Point> answer, Point positonRobot)
        {
            int index = fullWay.FindIndex(x => x == positionRobot);
            int indexAnswer = 0;
            for (int i = 0; i < index; i++)
            {
                if (answer.FindIndex(x => x == fullWay[i]) > -1)
                {
                    indexAnswer++;
                }
            }
            for (int i = 0; i < index; i++)
            {
                g.DrawImage(image[1], new Point(fullWay[i].X * size, fullWay[i].Y * size));
            }
            for (int i = 0; i < indexAnswer; i++)
            {
                g.DrawImage(image[6], new Point(answer[i].X * size, answer[i].Y * size));
            }
            for (int i = indexAnswer; i < answer.Count; i++)
            {
                g.DrawImage(image[5], new Point(answer[i].X * size, answer[i].Y * size));
            }
            RedrawRobot(g, image, size, positionRobot, positionRobot);
            RedrawExit(g, image, size);
        }

        /// <summary>
        /// Перерисовывает робота в лабиринте
        /// </summary>
        public void RedrawRobot(Graphics g, Image[] image, int size, Point pastPosition, Point currentPosition)
        {
            positionRobot = currentPosition;
            g.DrawImage(image[6], new Point(pastPosition.X * size - 1, pastPosition.Y * size - 1));
            g.DrawImage(image[2], new Point(currentPosition.X * size - 1, currentPosition.Y * size - 1));
        }

        /// <summary>
        /// Перерисовывает выход в лабиринте
        /// </summary>
        public void RedrawExit(Graphics g, Image[] image, int size)
        {
            g.DrawImage(image[3], new Point(exit.X * size - 1, exit.Y * size - 1));
        }
        /// <summary>
        ///рисует вход и выход в лабиринте
        /// </summary>
        public void GenerateExit(Graphics g, Image[] image, int size)
        {

            for (int i = 0; i < width; i++)
            {
                labyrinth[i, 0] = 1;
                labyrinth[i, height - 1] = 1;
            }
            for (int i = 1; i < height - 1; i++)
            {
                labyrinth[0, i] = 1;
                labyrinth[width - 1, i] = 1;
            }
            labyrinth[entry.X - 1, entry.Y - 1] = 0;
            labyrinth[exit.X - 1, exit.Y - 1] = 0;
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    if (labyrinth[i, j] == 0)
                    {
                        g.DrawImage(image[1], new Point(i * size + 15, j * size + 15));
                    }
                    else
                        g.DrawImage(image[0], new Point(i * size + 15, j * size + 15));
                }
            g.DrawImage(image[2], new Point(entry.X * size - 1, entry.Y * size - 1));
            g.DrawImage(image[3], new Point(exit.X * size - 1, exit.Y * size - 1));
            positionRobot = entry;
        }

        /// <summary>
        /// Сгенерировать лабиринт по алгоритму Эллера
        /// </summary>
        public void GenerateLabyrinth()
        {
            countQuantity = width * 10;
            int percent;
            labyrinth = new int[width, height];
            //высота текущего сгенерированного лабиринта
            int currentHeight = 0;


            //массив множеств ячеек
            List<int>[] quantityList = new List<int>[countQuantity];
            for (int i = 0; i < countQuantity; i++)
            {
                quantityList[i] = new List<int>();
            }
            //ячейки разбитые на множества
            int[,] setLabirynth = new int[width, height];

            //указатель на текущее множество
            int index = 0;

            //шаг 1
            for (int i = 0; i < width; i++)
            {
                labyrinth[i, 0] = 1;
            }
            labyrinth[0, 1] = 1;
            labyrinth[width - 1, 1] = 1;
            labyrinth[0, 2] = 1;
            labyrinth[width - 1, 2] = 1;
            currentHeight = 2;
            for (int step = 0; step < (height - 3) / 2; step++)
            {
                //шаг 2
                for (int i = 1; i < width - 1; i += 2)
                {
                    for (int j = 1; j < currentHeight; j += 2)
                    {
                        if (setLabirynth[i, currentHeight - 1] == 0)
                        {
                            bool success = false;
                            while (success == false)
                            {
                                if (quantityList[index].Count == 0)
                                {
                                    success = true;
                                    setLabirynth[i, currentHeight - 1] = index + 1;
                                    quantityList[index].Add(i);
                                }
                                else index++;
                                if (index >= countQuantity)
                                {
                                    index = 0;
                                }

                            }
                        }
                    }
                }
                //шаг 3

                for (int i = 1; i < width - 3; i += 2)
                {
                    //шаг 3.1
                    if (setLabirynth[i, currentHeight - 1] == setLabirynth[i + 2, currentHeight - 1])
                    {
                        labyrinth[i + 1, currentHeight - 1] = 1;
                        labyrinth[i + 1, currentHeight] = 1;
                        if (labyrinth[i, currentHeight - 2] == 1)
                        {
                            labyrinth[i + 1, currentHeight - 2] = 1;
                        }
                    }
                    else
                    {
                        //шаг 3.2
                        percent = rnd.Next(1, 100);
                        if (percent <= chanceCreateVerticalWall)
                        {
                            labyrinth[i + 1, currentHeight - 1] = 1;
                            labyrinth[i + 1, currentHeight] = 1;
                            if (labyrinth[i, currentHeight - 2] == 1)
                            {
                                labyrinth[i + 1, currentHeight - 2] = 1;
                            }
                        }
                        else
                        {
                            //добавляем одно множество к другому
                            quantityList[setLabirynth[i, currentHeight - 1] - 1].
                                AddRange(quantityList[setLabirynth[i + 2, currentHeight - 1] - 1]);
                            int numberQuntity = setLabirynth[i + 2, currentHeight - 1] - 1;
                            int count = quantityList[setLabirynth[i + 2, currentHeight - 1] - 1].Count;
                            for (int j = 0; j < count; j++)
                            {
                                setLabirynth[quantityList[setLabirynth[i + 2, currentHeight - 1] - 1][j],
                                     currentHeight - 1] = setLabirynth[i, currentHeight - 1];
                            }
                            //очищаем лист множества
                            quantityList[numberQuntity].Clear();
                        }
                    }
                }
                //шаг 4
                for (int i = 1; i < width - 3; i += 2)
                {
                    percent = rnd.Next(1, 100);
                    if (percent <= CHANCE_CREATE_HORIZONTAL_WALL)
                    {

                        int count = quantityList[setLabirynth[i, currentHeight - 1] - 1].Count;
                        if (count > 1)
                        {
                            int countWall = 0;
                            for (int j = 0; j < count; j++)
                            {
                                if (labyrinth[quantityList[setLabirynth[i, currentHeight - 1] - 1][j],
                                    currentHeight] == 1)
                                {
                                    countWall++;
                                }
                            }
                            if (countWall < count - 1)
                            {
                                labyrinth[i, currentHeight] = 1;
                                labyrinth[i - 1, currentHeight] = 1;
                                labyrinth[i + 1, currentHeight] = 1;
                                if (labyrinth[i + 1, currentHeight - 1] == 1)
                                {
                                    labyrinth[i + 1, currentHeight] = 1;
                                }
                            }
                        }
                    }
                }
                //шаг 5
                currentHeight += 2;

                labyrinth[0, currentHeight - 1] = 1;
                labyrinth[width - 1, currentHeight - 1] = 1;
                labyrinth[0, currentHeight] = 1;
                labyrinth[width - 1, currentHeight] = 1;
                for (int i = 1; i < width - 2; i += 2)
                {
                    if (labyrinth[i, currentHeight - 2] == 1)
                    {
                        quantityList[setLabirynth[i, currentHeight - 3] - 1].Remove(i);
                    }
                }
            }
            for (int i = 1; i < width; i++)
            {
                labyrinth[i, currentHeight] = 1;
            }
        }


        /// <summary>
        /// Получить лабиринт
        /// </summary>
        public int[,] GetLabyrinth
        {
            get { return labyrinth; }
        }

        /// <summary>
        /// Задать или получить вход лабиринта
        /// </summary>
        public Point Entry
        {
            set { entry = value; }
            get { return entry; }
        }

        /// <summary>
        /// Задать или получить выход лабиринта
        /// </summary>
        public Point Exit
        {
            set { exit = value; }
            get { return exit; }
        }

        /// <summary>
        /// Получить или задать положение робота
        /// </summary>
        public Point PositionRobot
        {
            set { positionRobot = value; }
            get { return positionRobot; }
        }

        /// <summary>
        /// Установить вероятность создания вертикальной стены
        /// </summary>
        public int ChanceCreateVerticalWall
        {
            set { chanceCreateVerticalWall = value; }
        }
    }
}
