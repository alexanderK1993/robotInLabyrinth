using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace robotInLabyrinth
{
    /// <summary>
    ///Реализация узла дерева 
    /// </summary>
    public class Node
    {

        /// <summary>
        /// номер узла
        /// </summary>
        private int id;

        /// <summary>
        /// координаты узла на карте лабиринта
        /// </summary>
        private Point coordinate;

        /// <summary>
        /// Как далеко находится узел от корневого узла
        /// </summary>
        private int depth;

        /// <summary>
        /// Оценка узла
        /// </summary>
        private double rating;

        /// <summary>
        /// Был ли узел просмотрен
        /// </summary>
        private bool overlooked = false;

        /// <summary>
        /// Входит ли узел в итоговое решение
        /// </summary>
        bool includedInSolution=false;

        public Node()
        {
            id = 0;
            coordinate = new Point();
            rating = -1;
        }


        public Node(int parId, Point parCoordinate,int parDepth)
        {
            id = parId;
            coordinate = parCoordinate;
            depth = parDepth;
        }

        public bool Overlooked
        {
            set { overlooked = value; }
            get { return overlooked; }
        }


        public bool IncludedInSolution
        {
            set { includedInSolution = value; }
            get { return includedInSolution; }
        }
        /// <summary>
        /// Номер узла
        /// </summary>
        public int Id
        {
            get { return id; }
        }

        public Point Coordinate
        {
            get { return coordinate; }
        }
        public int Depth
        {
            set { depth = value; }
            get { return depth; }
        }
        public double Rating
        {
            set { rating = value; }
            get { return rating; }
        }
    }

    /// <summary>
    /// Реализация ветви дерева
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// Номер родительского узла
        /// </summary>
        private int idParentNode;

        /// <summary>
        /// Номер дочернего узла
        /// </summary>
        private int idDaughterNode;

        public Connection(int parIdParentNode, int parIdDaughterNode)
        {
            idParentNode = parIdParentNode;
            idDaughterNode = parIdDaughterNode;
        }
        /// <summary>
        /// Родительский узел
        /// </summary>
        public int IdParentNode
        {
            get { return idParentNode; }
        }
        /// <summary>
        /// Дочерний узел
        /// </summary>
        public int IdDaughterNode
        {
            get { return idDaughterNode; }
        }
    }

    /// <summary>
    /// Реализация дерева
    /// </summary>
    public class Tree
    {
        /// <summary>
        /// Номер текущего родительского узла
        /// </summary>
        int currentNodeId = 0;

        /// <summary>
        /// Список узлов представляющих собой дерево
        /// </summary>
        private List<Node> listNode = new List<Node>();

        /// <summary>
        /// Список соединений узлов
        /// </summary>
        private List<Connection> listConnection = new List<Connection>();

        /// <summary>
        /// номер текущего добавляемого узла
        /// </summary>
        int id = 0;

        /// <summary>
        ///Максимальная глубина на которой выполнялся поиск.
        /// </summary>
        int maxDepth;

        /// <summary>
        /// Добавляем узел к дереву
        /// </summary>
        public int AddNode(Point coordinate)
        {
            if (id != 0)
            {                 
                listNode.Add(new Node(id, coordinate,listNode[currentNodeId].Depth));
                listConnection.Add(new Connection(currentNodeId, id));
                listNode[id].Depth = listNode[currentNodeId].Depth + 1;
                if (maxDepth < listNode[id].Depth)
                {
                    maxDepth = listNode[id].Depth;
                }
            }
            else
            {
                listNode.Add(new Node(id, coordinate,0));
                listNode[id].Depth = listNode[currentNodeId].Depth + 1;
            }
            return id++;
        }

        /// <summary>
        /// Возвращает узел по его индексу
        /// </summary>
        /// <param name="idNode">Номер узла</param>
        /// <returns></returns>
        public Node FindNodeId(int idNode)
        {
            return listNode[idNode];
        }

        /// <summary>
        /// Проверяет наличие узла в дереве
        /// </summary>
        public bool ExistNodeCoordinate(Point coordinate)
        {
            for (int i = 0; i < listNode.Count; i++)
            {
                if (listNode[i].Coordinate == coordinate)
                    return true;
            }
            return false;
        }

        public Node FindNodeCoordinate(Point coordinate)
        {
            for (int i = 0; i < listNode.Count; i++)
            {
                if (listNode[i].Coordinate == coordinate)
                    return listNode[i];
            }
            return new Node();
        }

        public int FindIdNodeCoordinate(Point coordinate)
        {
            for (int i = 0; i < listNode.Count; i++)
            {
                if (listNode[i].Coordinate == coordinate)
                    return listNode[i].Id;
            }
            return -1;
        }

        /// <summary>
        /// Возвращает следующий непросмотренный узел
        /// </summary>
        /// <returns></returns>
        public Point FindNewNode()
        {
            var unreviewedNodes = GetUnreviewedNodes();
            for (int i = 0; i < unreviewedNodes.Count; i++)
            {
                if (unreviewedNodes[i] != null)
                {
                    CurrentNode = unreviewedNodes[i].Id;
                    return unreviewedNodes[i].Coordinate;
                    
                }
            }
            bool findParent = false;
            listNode[currentNodeId].IncludedInSolution = false;
            for (int i = 0; i < listConnection.Count; i++)
            {
                if (listConnection[i].IdDaughterNode == currentNodeId)
                {
               
                    currentNodeId = listConnection[i].IdParentNode;
                    findParent = true;
                    break;
                }
            }
            if (findParent == false)
            {
                return new Point();
            }
            
            return FindNewNode();            
        }


        /// <summary>
        /// Генерирует узлы дерева на указанной глубине
        /// </summary>
        public bool GenerateNewNodesInSpecifiedDepth(int[,] labyrinth, int depth)
        {
            bool success = false;
            Point currentCooordinate;
            int count = listNode.Count;
            for (int i = 0; i < count; i++)
            {
                if (depth == listNode[i].Depth)
                {
                    currentCooordinate = listNode[i].Coordinate;
                    currentNodeId = listNode[i].Id;
                   success= AddNodes(currentCooordinate, labyrinth);
                }
            }
            return success;
        }

        public Node FindParentNode(int daughterId)
        {
            for (int i = 0; i < listConnection.Count; i++)
            {
                if (listConnection[i].IdDaughterNode == daughterId)
                {
                    return FindNodeId(listConnection[i].IdParentNode);
                }
            }
            return null;
        }
        /// <summary>
        /// Генерирует новые узлы исходящие из текущего узла
        /// </summary>
        public void GenerateNewNodes(int[,] labyrinth)
        {
            Point currentCooordinate = listNode[currentNodeId].Coordinate;
            AddNodes(currentCooordinate, labyrinth);
        }

        /// <summary>
        /// Добавить новые узлы исходящие из текущего
        /// </summary>
        public bool AddNodes(Point currentCooordinate, int[,] labyrinth)
        {
            bool success = false;
            int width=labyrinth.GetLength(0);
            int height=labyrinth.GetLength(1);
            if ((currentCooordinate.X - 1 - 1 <width )
                && (currentCooordinate.Y - 1 <height)
                &&(currentCooordinate.X - 1 - 1 > -1)
                && (currentCooordinate.Y - 1 > -1)
                          && (labyrinth[currentCooordinate.X - 1 - 1, currentCooordinate.Y - 1] == 0)
                          && (ExistNodeCoordinate(
                          new Point(currentCooordinate.X - 1, currentCooordinate.Y)) == false))
            {
                AddNode(new Point(currentCooordinate.X - 1, currentCooordinate.Y));
                success = true;
            }

            if ((currentCooordinate.X + 1 - 1 < width)
                && (currentCooordinate.Y - 1 < height)
                &&(currentCooordinate.X + 1 - 1 > -1) && 
                (currentCooordinate.Y - 1 > -1) 
                && (labyrinth[currentCooordinate.X + 1 - 1, currentCooordinate.Y - 1] == 0)
                && (ExistNodeCoordinate(
                new Point(currentCooordinate.X + 1, currentCooordinate.Y)) == false))
            {
                AddNode(new Point(currentCooordinate.X + 1, currentCooordinate.Y));
                success = true;
            }

            if ((currentCooordinate.X - 1 < width)
                && (currentCooordinate.Y - 1-1 < height)
                &&(currentCooordinate.X - 1 > -1) && (currentCooordinate.Y - 1 - 1 > -1)
                && (labyrinth[currentCooordinate.X - 1, currentCooordinate.Y - 1 - 1] == 0)
               && (ExistNodeCoordinate(
               new Point(currentCooordinate.X, currentCooordinate.Y - 1)) == false))
            {
                AddNode(new Point(currentCooordinate.X, currentCooordinate.Y - 1));
                success = true;
            }

            if ((currentCooordinate.X - 1 < width)
                && (currentCooordinate.Y +1- 1 < height)
                &&(currentCooordinate.X - 1 > -1) 
                &&(currentCooordinate.Y + 1 - 1 > -1) 
                && (labyrinth[currentCooordinate.X - 1, currentCooordinate.Y + 1 - 1] == 0)
              && (ExistNodeCoordinate(
              new Point(currentCooordinate.X, currentCooordinate.Y + 1)) == false))
            {
                AddNode(new Point(currentCooordinate.X, currentCooordinate.Y + 1));
                success = true;
            }
            return success;
        }

        /// <summary>
        /// Делает узел просмотренным
        /// </summary>
        /// <param name="idNode">номер узла</param>
        public Point MakeNodeOverlooked(int idNode)
        {
            if (listNode.Count >= idNode + 1)
            {
                listNode[idNode].Overlooked = true;
                return listNode[idNode].Coordinate;
            }
            else
                return new Point() ;
        }


        /// <summary>
        /// Возвращает непросмотренные вершины исходящие из текущей вершины
        /// </summary>
        /// <returns></returns>
        public List<Node> GetUnreviewedNodes()
        {
            List<Node> returnsNodes = new List<Node>();
            for (int i = 0; i < listConnection.Count; i++)
            {
                if ((listConnection[i].IdParentNode == currentNodeId) && (listNode[listConnection[i].IdDaughterNode].Overlooked == false))
                {
                    returnsNodes.Add(listNode[listConnection[i].IdDaughterNode]);
                }
            }
            return returnsNodes;
        }

        /// <summary>
        /// Найти все непросмотренные узлы на указанной глубине
        /// </summary>
        public Node GetUnreviewedNodeSpecifiedDepth(int depth)
        {
            for (int i = 0; i < listNode.Count; i++)
            {
                if ((listNode[i].Depth == depth)&&(listNode[i].Overlooked==false))
                { 
                   return listNode[i];
                }
            }
            return new Node();
        }

        /// <summary>
        /// Генерирует часть дерева для выполнения следующего шага
        /// </summary>
        public Point GenerateStep(int[,] labyrinth)
        {
            bool newNode = false;
            GenerateNewNodes(labyrinth);
            var nodesForNextStep = GetUnreviewedNodes();
            for (int i = 0; i < nodesForNextStep.Count(); i++)
            {
                if (nodesForNextStep[i] != null)
                {
                    CurrentNode = nodesForNextStep[i].Id;
                    newNode = true;
                    break;
                }
            }
            if (newNode)
                return FindNodeId(CurrentNode).Coordinate;
            else
                return new Point();
        }


        /// <summary>
        /// Текущий узел
        /// </summary>
        public int CurrentNode
        {
            set
            {
                currentNodeId = value;
                listNode[listNode.FindIndex(x => x.Id == currentNodeId)].IncludedInSolution = true;
                MakeNodeOverlooked(currentNodeId);
            }
            get { return currentNodeId; }
        }

     
        public List<Node> ListNode
        {
            get { return listNode; }
        }
        public int MaxDepth
        {
            set { maxDepth = value; }
            get { return maxDepth; }
        }
    }
}