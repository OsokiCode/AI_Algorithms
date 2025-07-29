using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AStarAlgorithm
{
    public partial class Form1 : Form
    {
        TextBox[,] baslangicMatris = new TextBox[3, 3];
        TextBox[,] hedefMatris = new TextBox[3, 3];

        public Form1()
        {
            InitializeComponent();

            baslangicMatris[0, 0] = textBox1;
            baslangicMatris[0, 1] = textBox2;
            baslangicMatris[0, 2] = textBox3;
            baslangicMatris[1, 0] = textBox4;
            baslangicMatris[1, 1] = textBox5;
            baslangicMatris[1, 2] = textBox6;
            baslangicMatris[2, 0] = textBox7;
            baslangicMatris[2, 1] = textBox8;
            baslangicMatris[2, 2] = textBox9;

            hedefMatris[0, 0] = textBox10;
            hedefMatris[0, 1] = textBox11;
            hedefMatris[0, 2] = textBox12;
            hedefMatris[1, 0] = textBox13;
            hedefMatris[1, 1] = textBox14;
            hedefMatris[1, 2] = textBox15;
            hedefMatris[2, 0] = textBox16;
            hedefMatris[2, 1] = textBox17;
            hedefMatris[2, 2] = textBox18;
        }
        private void AddMatrixToListBox(int[,] matrix)
        {
            for (int i = 0; i < 3; i++)
            {
                string row = $"{matrix[i, 0]} {matrix[i, 1]} {matrix[i, 2]}";
                listBox1.Items.Add(row);
            }
            listBox1.Items.Add("");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int[,] baslangicDurumu = new int[3, 3];
            int[,] hedefDurumu = new int[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int.TryParse(baslangicMatris[i, j].Text, out baslangicDurumu[i, j]);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int.TryParse(hedefMatris[i, j].Text, out hedefDurumu[i, j]);
                }
            }

            listBox1.Items.Clear();

            listBox1.Items.Add("Baþlangýç Durumu:");
            AddMatrixToListBox(baslangicDurumu);

            listBox1.Items.Add("\nHedef Durumu:");
            AddMatrixToListBox(hedefDurumu);

            AStar aStar = new AStar();
            var solutionSteps = aStar.Solve(baslangicDurumu, hedefDurumu);

            if (solutionSteps != null && solutionSteps.Count > 0)
            {
                listBox1.Items.Add("\nÇözüm Adýmlarý:");

                foreach (var step in solutionSteps)
                {
                    AddMatrixToListBox(step);
                }
            }
            else
            {
                listBox1.Items.Add("Çözüm bulunamadý.");
            }
        }

        public class AStar
        {
            public class Node
            {
                public int[,] State { get; set; }  
                public Node Parent { get; set; }    
                public int G { get; set; }           
                public int H { get; set; }           
                public int F => G + H;              

                public Node(int[,] state, Node parent = null)
                {
                    State = state;
                    Parent = parent;
                    G = parent?.G + 1 ?? 0;
                    H = CalculateManhattanDistance(State);
                }

                private int CalculateManhattanDistance(int[,] state)
                {
                    int distance = 0;
                    int[,] goalState = new int[,]
                    {
                        { 1, 2, 3 },
                        { 4, 5, 6 },
                        { 7, 8, 0 }
                    };

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            int currentValue = state[i, j];
                            if (currentValue != 0)
                            {
                                int goalX = (currentValue - 1) / 3;
                                int goalY = (currentValue - 1) % 3;
                                distance += Math.Abs(i - goalX) + Math.Abs(j - goalY);
                            }
                        }
                    }

                    return distance;
                }
            }

            public List<int[,]> Solve(int[,] start, int[,] goal)
            {
                List<Node> openList = new List<Node>();
                List<Node> closedList = new List<Node>();

                Node startNode = new Node(start);
                openList.Add(startNode);

                List<int[,]> solutionSteps = new List<int[,]>();

                while (openList.Count > 0)
                {
                    Node currentNode = openList.OrderBy(node => node.F).First();
                    openList.Remove(currentNode);
                    closedList.Add(currentNode);

                    if (IsGoalState(currentNode.State, goal))
                    {

                        while (currentNode != null)
                        {
                            solutionSteps.Insert(0, currentNode.State);
                            currentNode = currentNode.Parent;
                        }
                        break;
                    }

                    foreach (var neighbor in GetNeighbors(currentNode))
                    {

                        if (closedList.Any(n => AreMatricesEqual(n.State, neighbor.State)))
                            continue;

                        if (openList.Any(n => AreMatricesEqual(n.State, neighbor.State) && n.F <= neighbor.F))
                            continue;

                        openList.Add(neighbor);
                    }
                }

                return solutionSteps;
            }

            private bool IsGoalState(int[,] state, int[,] goal)
            {
                return AreMatricesEqual(state, goal);
            }

            private bool AreMatricesEqual(int[,] matrix1, int[,] matrix2)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (matrix1[i, j] != matrix2[i, j])
                            return false;
                    }
                }
                return true;
            }

            private List<Node> GetNeighbors(Node currentNode)
            {
                List<Node> neighbors = new List<Node>();
                int[,] state = currentNode.State;

                int emptyRow = -1, emptyCol = -1;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (state[i, j] == 0)
                        {
                            emptyRow = i;
                            emptyCol = j;
                            break;
                        }
                    }
                }

                int[] rowMoves = { -1, 1, 0, 0 };
                int[] colMoves = { 0, 0, -1, 1 };

                for (int i = 0; i < 4; i++)
                {
                    int newRow = emptyRow + rowMoves[i];
                    int newCol = emptyCol + colMoves[i];

                    if (newRow >= 0 && newRow < 3 && newCol >= 0 && newCol < 3)
                    {
                        int[,] newState = (int[,])state.Clone();
                        newState[emptyRow, emptyCol] = newState[newRow, newCol];
                        newState[newRow, newCol] = 0;
                        neighbors.Add(new Node(newState, currentNode));
                    }
                }

                return neighbors;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
