using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace robotInLabyrinth
{

    /// <summary>
    /// Поиск в глубину
    /// </summary>
    class DepthFirstSearch
    {
        /// <summary>
        /// Вход
        /// </summary>
        private Point entry;

        /// <summary>
        /// Выход
        /// </summary>
        private Point exit;

        /// <summary>
        /// Дерево поиска
        /// </summary>
        private Tree tree = new Tree();

  
        /// <summary>
        /// Инициализация класса
        /// </summary>
        public DepthFirstSearch(Point parStart, Point parFinish)
        {
            entry = parStart;
            exit = parFinish;
        }

        /// <summary>
        /// Поиск решения методом поиска в глубину
        /// </summary>
        /// <param name="labyrinth">лабиринт</param>
        /// <param name="fullWay">полный путь</param>
        /// <param name="answer">конечный путь</param>
        /// <param name="rating">оценки эффективности поиска</param>
        public void SearchAnswer(int[,] labyrinth, out List<Point> fullWay, out List<Point> answer, out double[] rating)
        {
            fullWay = new List<Point>();
            answer = new List<Point>();
            rating = new double[4];
            tree.CurrentNode = tree.AddNode(entry);
        
            fullWay.Add(tree.FindNodeId(tree.CurrentNode).Coordinate);
            Point newNodeCoordinate;
            do
            {
                newNodeCoordinate = tree.GenerateStep(labyrinth);
                 if (newNodeCoordinate.IsEmpty == false)
                {
                    fullWay.Add(newNodeCoordinate);
                }
                else
                {
                    newNodeCoordinate = tree.FindNewNode();
                    if (newNodeCoordinate.IsEmpty == false)
                    {
                        fullWay.Add(newNodeCoordinate);
                    }
                }
            }
            while (newNodeCoordinate != exit);
            for (int i = 0; i < fullWay.Count; i++)
            {
                Node node = tree.FindNodeCoordinate(fullWay[i]);
                if (node != null)
                {
                    if (node.IncludedInSolution == true)
                    {
                        answer.Add(node.Coordinate);
                    }

                }
            }
            rating[0] = tree.MaxDepth;
            rating[1] = answer.Count;
            rating[2] = fullWay.Count;
            rating[3] = (double)fullWay.Count / (double)answer.Count;
        }
    }
}
