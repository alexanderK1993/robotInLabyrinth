using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace robotInLabyrinth
{
    /// <summary>
    /// Поиск в ширину
    /// </summary>
    class BreadthFirstSearch
    {

        /// <summary>
        /// Дерево поиска
        /// </summary>
        private Tree tree = new Tree();

        /// <summary>
        /// Вход
        /// </summary>
        private Point entry;

        /// <summary>
        /// Выход
        /// </summary>
        private Point exit;

        /// <summary>
        /// Инициализация класса
        /// </summary>
        public BreadthFirstSearch(Point parEntry, Point parExit)
        {
            entry = parEntry;
            exit = parExit;
        }

        /// <summary>
        /// Поиск решения методом поиска в ширину
        /// </summary>
        /// <param name="labyrinth">лабиринт</param>
        /// <param name="fullWay">полный путь</param>
        /// <param name="answer">конечный путь</param>
        /// <param name="rating">оценки эффективности поиска</param>
        public void SearchAnswer(int[,] labyrinth, out List<Point> fullWay,
            out List<Point> answer, out double[] rating)
        {
            int currentDepth = 0;
            fullWay = new List<Point>();
            answer = new List<Point>();
            rating = new double[4];
            tree.CurrentNode = tree.AddNode(entry);
            fullWay.Add(tree.FindNodeId(tree.CurrentNode).Coordinate);
            Node newNode;
            do
            {
                newNode = tree.GetUnreviewedNodeSpecifiedDepth(currentDepth);
                if (newNode.Coordinate.IsEmpty == true)
                {
                    tree.GenerateNewNodesInSpecifiedDepth(labyrinth, currentDepth);
                    currentDepth++;
                }
                else
                {
                    fullWay.Add(newNode.Coordinate);
                   tree.CurrentNode = newNode.Id;
                }
            }
            while (newNode.Coordinate != exit);
            for (int i = 0; i < tree.ListNode.Count; i++)
            {
                tree.ListNode[i].IncludedInSolution = false;
            }
            List<Node> promList = new List<Node>();

            Node currentNode = tree.FindNodeCoordinate(exit);
            promList.Add(currentNode);
            while (currentNode != null)
            {
                currentNode = tree.FindParentNode(currentNode.Id);
                promList.Add(currentNode);
            }
            int count = promList.Count;
            for (int i = 0; i < count-1; i++)
            {
                answer.Add(promList[count - 2 - i].Coordinate);
            }

            rating[0] = tree.MaxDepth;
            rating[1] = answer.Count;
            rating[2] = fullWay.Count;
            rating[3] = (double)fullWay.Count / (double)answer.Count;
        }

    }
}
