using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TripPlanning.GisAnalysis.DataStructs;

namespace TripPlanning.GisAnalysis
{
    public class PathResolver:IPathResolver
    {
        public Node StartNode { get; private set; }
        public List<Node> Nodes { get; set; } = new List<Node>();//结点的集合
        public List<Edge> Edges { get; set; } = new List<Edge>();
        public Dictionary<Node, List<Edge>> AdjacentEdgesDictionary { get; set; } = new Dictionary<Node, List<Edge>>();
        public List<Node> CoveredNodes { get; set; } = new List<Node>();

        public List<Path> Paths
        {
            get { return this._paths; }
        }

        //路径表--该路径结点列表:该路径总距离
        List<Path> _paths = new List<Path>();

        /// <summary>
        /// 结点+边+起点,构造最短路径模拟器
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="edges"></param>
        /// <param name="startNode"></param>
        public PathResolver(List<Node> nodes, List<Edge> edges, Node startNode)
        {
            Nodes = nodes;
            Edges = edges;
            foreach (var node in nodes)
            {
                AdjacentEdgesDictionary[node] = Edges?.FindAll((x) => x.Start == node);
            }

            SetStartNode(startNode);
        }

        private void SetStartNode(Node startNode)
        {
            if (AdjacentEdgesDictionary[startNode] == null || AdjacentEdgesDictionary[startNode].Count == 0)
            {
                Debug.Write("起始节点不存在任何连接边!");
                return;
            }
            StartNode = Nodes.Find(item => item == startNode);
        }

        /// <summary>
        /// 生成起点至终点的最短路径,关键代码段
        /// </summary>
        /// <returns>路径点集,最短距离</returns>
        public void FormShortestPath()
        {
            //初始化路径表,将起点加入路径
            _paths.Add(new Path()
            {
                Nodes = new List<Node>() { StartNode },
                TotalWeight = 0
            });

            for (int i = 0; i < _paths.Count; i++)//一级嵌套,遍历所有路径
            {
                foreach (Node node in _paths[i].Nodes)//二级嵌套,遍历路径中所有结点
                {
                    if (CoveredNodes.Contains(node))//结点已经遍历,遍历路径中下一结点
                    {
                        continue;
                    }

                    foreach (Edge edge in AdjacentEdgesDictionary[node])//三级嵌套,遍历结点所有有向边
                    {
                        Path newIPath = new Path() { Nodes = new List<Node>() };
                        newIPath.Nodes.AddRange(_paths[i].Nodes);//初始化临时路径
                        newIPath.TotalWeight = _paths[i].TotalWeight + edge.Weight;//临时路径总权重=当前路径权重+即将连接的边的权重
                        newIPath.Nodes.Add(edge.End);//将连接的结点加入路径中
                        if (newIPath.TotalWeight < GetMinWeight(edge.End))//保留短于当前最短路径的路径
                        {
                            _paths.Add(newIPath);
                        }
                    }
                    CoveredNodes.Add(node);//设置该点状态为已经遍历
                }
            }
            foreach (var item in Nodes)
            {
                _paths.RemoveAll((x) => (x.TotalWeight > GetMinWeight(item)) && (x.EndNode == item));
            }
            Debug.Write("已经计算出所有路径最短路径表!");
        }

        /// <summary>
        /// 从路径表中获取起点至INode的当前最小权重
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private double GetMinWeight(Node node)
        {
            double minWeight = double.MaxValue;
            foreach (var curPath in _paths)
            {
                if (curPath.StartNode == StartNode && curPath.EndNode == node && curPath.TotalWeight <= minWeight)
                {
                    minWeight = curPath.TotalWeight;
                }
            }
            return minWeight;
        }

        public List<Path> GetShortestPaths(Node endNode, double maxWeight)
        {
            List<Path> validPaths = _paths.FindAll((x) => x.EndNode == endNode);

            double minWeight = double.MaxValue;
            foreach (var item in validPaths)
            {
                if (item.TotalWeight < minWeight)
                {
                    minWeight = item.TotalWeight;
                }
            }

            return validPaths.Where(path => Math.Abs(path.TotalWeight - minWeight) < GeoAnalysisConfig.Tolerance).ToList();
        }
    }
}