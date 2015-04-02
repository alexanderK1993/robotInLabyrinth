using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace robotInLabyrinth
{
    /// <summary>
    /// Поиск от наилучшего частичного пути
    /// </summary>
    class SearchFromBestPartialPath:FunctionRating
    {

        /// <summary>
        /// Дерево поиска
        /// </summary>
        private Tree tree = new Tree();

        /// <summary>
        /// Координаты входа в лабиринт
        /// </summary>
        private Point entry;

        /// <summary>
        /// Координаты выхода в лабиринт
        /// </summary>
        private Point exit;

        /// <summary>
        /// Инициализация класса
        /// </summary>
        public SearchFromBestPartialPath(Point parEntry, Point parExit)
        {
            entry = parEntry;
            exit = parExit;
        }

        /// <summary>
        /// Поиск решения методом поиска от наилучшего частичного пути
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
            currentDepth++;
            Node newNode = new Node();
            List<Node> newNodes = new List<Node>();
            Point position = entry;
            int index;
            int indexMax;
            double max;
            double lastRating = -1;
            int maxIndex=-1;
            bool returnPrevNode = false;
            double[] sortRating=new double[labyrinth.GetLength(0)*labyrinth.GetLength(1)];
            int[] sortIndex= new int[labyrinth.GetLength(0)*labyrinth.GetLength(1)];
            double value;
           
            do
            {
                returnPrevNode = false;
                index = -1;
                max = -1;
                indexMax = -1;
                tree.GenerateNewNodes(labyrinth);
                newNodes = tree.GetUnreviewedNodes(); 
                if (newNodes.Count != 0)
                {
                    for (int i = 0; i < newNodes.Count; i++)
                    {
                        value = functionRating(labyrinth.GetLength(0)
                                        , labyrinth.GetLength(1), newNodes[i].Coordinate, exit);
                        index = newNodes[i].Id;
                        if (value > max)
                        {
                            max = value;
                            indexMax = newNodes[i].Id;
                        }
                        maxIndex++;
                        sortRating[maxIndex] = value;
                        sortIndex[maxIndex] = index;
                        for (int j = maxIndex; j > 0; j--)
                        {
                            if (sortRating[j] < sortRating[j - 1])
                            {
                                double promRating;
                                int promIndex;
                                promRating = sortRating[j - 1];
                                sortRating[j - 1] = sortRating[j];
                                sortRating[j] = promRating;
                                promIndex = sortIndex[j - 1];
                                sortIndex[j - 1] = sortIndex[j];
                                sortIndex[j] = promIndex;
                            }
                            else break;
                        }
                    }
                    if (max >= lastRating)
                    {
                        tree.CurrentNode = tree.ListNode[indexMax].Id;
                        fullWay.Add(tree.ListNode[tree.CurrentNode].Coordinate);

                    }
                    else
                    {
                        returnPrevNode = true;
                    }
                    lastRating = max;
                }
                else 
                {
                    returnPrevNode = true;
                }

                if (returnPrevNode)
                {
                    bool success = false;
                    for (int i = maxIndex; i >-1; i--)
                    {
                        if (tree.ListNode[sortIndex[i]].Overlooked == false)
                        {
                            success = true;
                            tree.CurrentNode = tree.ListNode[sortIndex[i]].Id;
                            fullWay.Add(tree.ListNode[sortIndex[i]].Coordinate);
                            break;
                        }
                    }
                    if ((success == false)&&(index!=-1))
                    {
                        tree.CurrentNode = tree.ListNode[index].Id;
                        fullWay.Add(tree.ListNode[index].Coordinate);
                    }
                }
            }
            while (tree.ListNode[tree.CurrentNode].Coordinate != exit);
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
            for (int i = 0; i < count - 1; i++)
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
