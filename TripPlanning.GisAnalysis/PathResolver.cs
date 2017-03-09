using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TripPlanning.Gis.DataStructs;

namespace TripPlanning.Gis
{
    public class PathResolver
    {
        public INode StartNode { get; private set; }
        public List<INode> Nodes { get; set; } = new List<INode>();//结点的集合
        public List<IEdge> Edges { get; set; } = new List<IEdge>();
        public Dictionary<INode, List<IEdge>> AdjacentEdgesDictionary { get; set; } = new Dictionary<INode, List<IEdge>>();
        public List<INode> CoveredNodes { get; set; } = new List<INode>();

        public List<IPath> Paths
        {
            get { return this._paths; }
        }

        //路径表--该路径结点列表:该路径总距离
        List<IPath> _paths = new List<IPath>();

        /// <summary>
        /// 结点+边+起点+图类型,构造最短路径模拟器
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="edges"></param>
        /// <param name="startNode"></param>
        public PathResolver(IEnumerable<INode> nodes, IEnumerable<IEdge> edges, INode startNode)
        {
            Nodes = nodes.ToList();
            Edges = edges as List<IEdge>;
            foreach (var node in nodes)
            {
                AdjacentEdgesDictionary[node] = Edges?.FindAll((x) => x.Start == node);
            }

            SetStartINode(startNode);
        }

        public PathResolver(List<IEdge> setedIEdges)
            : this(setedIEdges.SelectMany(item => new List<INode> { item.Start, item.End }).Distinct(), setedIEdges, setedIEdges[0].Start)
        {

        }

        private void SetStartINode(INode startNode)
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
        public void FormShortestPath<T>() where T:IPath ,new()
        {
            //初始化路径表,将起点加入路径
            _paths.Add(new T()
            {
                Nodes = new List<INode>() { StartNode },
                TotalWeight = 0
            });

            for (int i = 0; i < _paths.Count; i++)//一级嵌套,遍历所有路径
            {
                foreach (INode node in _paths[i].Nodes)//二级嵌套,遍历路径中所有结点
                {
                    if (CoveredNodes.Contains(node))//结点已经遍历,遍历路径中下一结点
                    {
                        continue;
                    }

                    foreach (IEdge edge in AdjacentEdgesDictionary[node])//三级嵌套,遍历结点所有有向边
                    {
                        IPath newIPath = new T() { Nodes = new List<INode>() };
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
        private double GetMinWeight(INode node)
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

        public List<IPath> GetShortestPath(INode INode)
        {
            List<IPath> validPaths = _paths.FindAll((x) => x.EndNode == INode);

            double minWeight = double.MaxValue;
            foreach (var item in validPaths)
            {
                if (item.TotalWeight < minWeight)
                {
                    minWeight = item.TotalWeight;
                }
            }

            return validPaths.Where(path => Math.Abs(path.TotalWeight - minWeight) < 10 * Double.MinValue).ToList();
        }
    }
}