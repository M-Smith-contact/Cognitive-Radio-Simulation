using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeniorProject
{
    public partial class Form1 : Form
    {
        Node[] arrNobf;
        Node[] arrBf;
        Color[] colors = { Color.Red, Color.Green, Color.Orange, Color.Blue, Color.Yellow, Color.Violet, Color.Black };
        Bitmap nobf = new Bitmap(600, 600);
        Bitmap bf = new Bitmap(600, 600);
        Bitmap st = new Bitmap(10, 10);
        Bitmap sr = new Bitmap(10, 10);
        Bitmap pt = new Bitmap(10, 10);
        Bitmap pr = new Bitmap(10, 10);
        Bitmap clr = new Bitmap(45, 10);
        public static int prxx, prxy, letterb, d, band;
        public static decimal eHatB, minEnergyTx, minEnergyRx, eNetworkNobf, eNetworkBf, ePathNobf, ePathBf;
        public Dictionary<int, decimal> bEHatB = new Dictionary<int, decimal>(9);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //initialize dictionary to hold b-eHatb pairs
            bEHatB.Add(1, 0.00000000000000000003949189M);
            bEHatB.Add(2, 0.00000000000000000004013381M);
            bEHatB.Add(4, 0.00000000000000000009155914M);
            bEHatB.Add(6, 0.00000000000000000023179140M);
            bEHatB.Add(8, 0.00000000000000000066401600M);
            bEHatB.Add(10, 0.00000000000000000199351900M);
            bEHatB.Add(12, 0.00000000000000000603898100M);
            bEHatB.Add(14, 0.00000000000000002025209000M);
            bEHatB.Add(16, 0.00000000000000006733371000M);
            this.nodes.SelectedIndex = 3;
            this.sourceX.Text = "300";
            this.sourceY.Text = "300";
            this.ptx.Text = "150";
            this.pty.Text = "150";
            this.prx.Text = "450";
            this.pry.Text = "450";
            this.distance.SelectedIndex = 0;
            using (Graphics g = Graphics.FromImage(st))
            {
                g.DrawEllipse(new Pen(Color.Red), 1, 1, 8, 8);
            }
            this.SUtx.Image = st;
            using (Graphics g = Graphics.FromImage(sr))
            {
                g.DrawEllipse(new Pen(Color.Black), 1, 1, 8, 8);
            }
            this.SUrx.Image = sr;
            using (Graphics g = Graphics.FromImage(pt))
            {
                g.FillRectangle(new SolidBrush(Color.Black), 1, 1, 8, 8);
            }
            this.PUtx.Image = pt;
            using (Graphics g = Graphics.FromImage(pr))
            {
                g.FillRectangle(new SolidBrush(Color.Red), 1, 1, 8, 8);
            }
            this.PUrx.Image = pr;
            using(Graphics g = Graphics.FromImage(clr))
            {
                g.DrawLine(new Pen(colors[0], 3.0F), 3, 10, 6, 0);
                g.DrawLine(new Pen(colors[1], 3.0F), 9, 10, 12, 0);
                g.DrawLine(new Pen(colors[2], 3.0F), 15, 10, 18, 0);
                g.DrawLine(new Pen(colors[3], 3.0F), 21, 10, 24, 0);
                g.DrawLine(new Pen(colors[4], 3.0F), 27, 10, 30, 0);
                g.DrawLine(new Pen(colors[5], 3.0F), 33, 10, 36, 0);
                g.DrawLine(new Pen(colors[6], 3.0F), 39, 10, 42, 0);
            }
            this.clrs.Image = clr;
        }

        private void populate_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            arrNobf = new Node[int.Parse(nodes.Text) + 2];
            arrBf = new Node[int.Parse(nodes.Text) + 2];
            using (Graphics g = Graphics.FromImage(nobf))
            {
                g.Clear(Form1.DefaultBackColor);
            }
            using (Graphics g = Graphics.FromImage(bf))
            {
                g.Clear(Form1.DefaultBackColor);
            }

            for (int i = 0; i < arrNobf.Length; i++ )
            {
                if (i == 0) //source node always first element of the array
                {
                    arrNobf[i] = new Node(i, int.Parse(sourceX.Text), int.Parse(sourceY.Text), false, -1);
                    arrBf[i] = new Node(i, int.Parse(sourceX.Text), int.Parse(sourceY.Text), false, -1);
                }
                else if (i == arrNobf.Length - 2)   //primary tx always next to last element of array
                {
                    arrNobf[i] = new Node(i, int.Parse(ptx.Text), int.Parse(pty.Text), true, i);
                    arrBf[i] = new Node(i, int.Parse(ptx.Text), int.Parse(pty.Text), true, i);
                }
                else if (i == arrNobf.Length - 1)   //primary rx always last element of array
                {
                    prxx = int.Parse(prx.Text);
                    prxy = int.Parse(pry.Text);
                    arrNobf[i] = new Node(i, prxx, prxy, true, i);
                    arrBf[i] = new Node(i, prxx, prxy, true, i);
                }
                else    //secondary rx all others
                {
                    int x = rand.Next(601);
                    int y = rand.Next(601);
                    arrNobf[i] = new Node(i, x, y, false, i);
                    arrBf[i] = new Node(i, x, y, false, i);
                }
                
                using (Graphics g = Graphics.FromImage(nobf))
                {
                    if(arrNobf[i].isPrimary && i == arrNobf.Length - 2)     //primary tx solid black rectangle
                    {
                        g.FillRectangle(new SolidBrush(Color.Black), arrNobf[i].x - 6, arrNobf[i].y - 6, 12, 12);
                    }
                    else if (arrNobf[i].isPrimary && i == arrNobf.Length - 1)   //primary rx solid red rectangle
                    {
                        g.FillRectangle(new SolidBrush(Color.Red), arrNobf[i].x - 6, arrNobf[i].y - 6, 12, 12);
                    }
                    else if (i == 0)    //source node red circle
                    {
                        g.DrawEllipse(new Pen(Color.Red), arrNobf[i].x - 4, arrNobf[i].y - 4, 8, 8);
                    }
                    else    //secondary rx black circles
                    {
                        g.DrawEllipse(new Pen(Color.Black), arrNobf[i].x - 4, arrNobf[i].y - 4, 8, 8);
                    }
                }
                using (Graphics g = Graphics.FromImage(bf))
                {
                    if (arrBf[i].isPrimary && i == arrBf.Length - 2)    //primary tx solid black rectangle
                    {
                        g.FillRectangle(new SolidBrush(Color.Black), arrBf[i].x - 6, arrBf[i].y - 6, 12, 12);
                    }
                    else if (arrBf[i].isPrimary && i == arrBf.Length - 1)   //primary rx solid red rectangle
                    {
                        g.FillRectangle(new SolidBrush(Color.Red), arrBf[i].x - 6, arrBf[i].y - 6, 12, 12);
                    }
                    else if (i == 0)    //source node red circle
                    {
                        g.DrawEllipse(new Pen(Color.Red), arrBf[i].x - 4, arrBf[i].y - 4, 8, 8);
                    }
                    else    //secondary rx black circles
                    {
                        g.DrawEllipse(new Pen(Color.Black), arrBf[i].x - 4, arrBf[i].y - 4, 8, 8);
                    }
                }
            }
            displayAreaNobf.Image = nobf;
            displayAreaBf.Image = bf;
            calcB.Enabled = true;
            b.Text = "";
            eHatbBox.Text = "";
            spanningTreeNobf.Enabled = false;
            spanningTreeBf.Enabled = false;
        }

        public static double findDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2.0) + Math.Pow(y2 - y1, 2.0));
        }

        public static double findDistance(int x1, int y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2.0) + Math.Pow(y2 - y1, 2.0));
        }

        private void calcB_Click(object sender, EventArgs e)
        {
            decimal[] energies = new decimal[9];
            int index = 0;
            d = int.Parse(distance.Text);
            band = int.Parse(bandwidth.Text);
            minEnergyTx = decimal.MaxValue;
            KeyValuePair<int, decimal> minPair = new KeyValuePair<int,decimal>();
            foreach(KeyValuePair<int, decimal> vals in bEHatB)
            {
                //calculate associated energy with each b-eHatb pair
                letterb = vals.Key;
                eHatB = vals.Value;
                decimal alpha = (decimal)(3 * (Math.Sqrt(Math.Pow(2.0, letterb) - 1)) / (.35 * (Math.Sqrt(Math.Pow(2.0, letterb)) + 1)));
                energies[index] = (decimal)(0.5M * (1 + alpha) * eHatB * (decimal)Math.Pow(4 * Math.PI * d, 2.0) / (.00316M * .1199M * .1199M) * .1M) + (.04864M + .05M) / (letterb * band);
                if (energies[index] < minEnergyTx)
                {
                    //find minimum energy
                    minEnergyTx = energies[index];
                    minPair = new KeyValuePair<int, decimal>(letterb, eHatB);
                }
                index++;
            }
            //use b-eHatb pair that corresponds to the minimum energy
            letterb = minPair.Key;
            eHatB = minPair.Value;
            minEnergyRx = (0.0625M + 0.05M) / (letterb * band);
            b.Text = letterb.ToString();
            eHatbBox.Text = eHatB.ToString("E");
            spanningTreeNobf.Enabled = true;
            spanningTreeBf.Enabled = true;
        }

        private void spanningTreeNobf_Click(object sender, EventArgs e)
        {
            int hopCount = 0;
            List<Node> thisHop = new List<Node>();
            List<Node> nextHop = new List<Node>();
            double dist;
            eNetworkNobf = 0;
            ePathNobf = 0;
            thisHop.Add(arrNobf[0]);
            Node source = new Node();
            while (thisHop.Count > 0)
            {
                if (hopCount == 0)//only the source node is currently in thisHop
                {
                    source = thisHop[0];
                    source.hops = hopCount;
                    foreach (Node n in arrNobf)
                    {
                        if (thisHop.Contains(n) || n.isPrimary)
                            continue;
                        dist = findDistance(source.x, source.y, n.x, n.y);
                        if (dist <= d)
                        {
                            source.myChildren.append(n.name);
                            n.parent = source.name;
                            thisHop.Add(n);
                        }
                    }
                    thisHop.RemoveAt(0);
                }
                else
                {
                    foreach (Node current in thisHop)
                    {
                        current.hops = hopCount;
                        foreach (Node n in arrNobf)
                        {
                            if (n.isPrimary || thisHop.Contains(n) || nextHop.Contains(n) || n.parent != n.name)
                                continue;
                            dist = findDistance(current.x, current.y, n.x, n.y);
                            if (dist <= d)
                            {
                                nextHop.Add(n);
                            }
                        }
                    }
                    foreach (Node child in nextHop)
                    {
                        foreach (Node parents in thisHop)
                        {
                            dist = findDistance(child.x, child.y, parents.x, parents.y);
                            if (dist <= d)
                            {
                                child.possibleParents.Add(parents, dist);
                            }
                        }
                        double maxDist = double.MinValue;
                        Node theParent = new Node();
                        foreach (KeyValuePair<Node, double> stuff in child.possibleParents)
                        {
                            if(stuff.Value > maxDist)
                            {
                                maxDist = stuff.Value;
                                theParent = stuff.Key;
                            }
                        }
                        child.parent = theParent.name;
                        theParent.myChildren.append(child.name);
                    }//end foreach Node child in nextHop
                    thisHop.Clear();
                    for(int i = 0; i < nextHop.Count; i++)
                    {
                        thisHop.Add(nextHop[i]);
                    }
                    nextHop.Clear();
                }//end else
                hopCount++;
            }

            foreach(Node n in arrNobf)
            {
                if (n.isPrimary)
                    continue;
                if (n.myChildren.header != null)
                    eNetworkNobf += minEnergyTx;
                Item curr = new Item();
                curr = n.myChildren.header;
                while(curr != null)
                {
                    using (Graphics g = Graphics.FromImage(nobf))
                    {
                        g.DrawLine(new Pen(colors[n.hops % 7]), n.x, n.y, arrNobf[curr.nodeNum].x, arrNobf[curr.nodeNum].y);
                    }
                    curr = curr.next;
                }
            }
            ePathNobf = (hopCount - 1) * minEnergyTx;
            networkNobf.Text = eNetworkNobf.ToString("E");
            pathNobf.Text = ePathNobf.ToString("E");
            displayAreaNobf.Image = nobf;
            spanningTreeNobf.Enabled = false;
            calcB.Enabled = false;
        }

        private void spanningTreeBf_Click(object sender, EventArgs e)
        {
            int hopCount = 0;
            List<Node> thisHop = new List<Node>();
            List<Node> nextHop = new List<Node>();
            double dist, bfDist, alphaAngle;
            decimal energyBf, alphaB;
            alphaB = (decimal)(3 * (Math.Sqrt(Math.Pow(2.0, letterb) - 1)) / (.35 * (Math.Sqrt(Math.Pow(2.0, letterb)) + 1)));
            eNetworkBf = 0;
            ePathBf = 0;
            thisHop.Add(arrBf[0]);
            Node source = new Node();
            while (thisHop.Count > 0)
            {
                if (hopCount == 0)
                {
                    source = thisHop[0];
                    source.hops = hopCount;
                    foreach (Node n in arrBf)
                    {
                        if (thisHop.Contains(n) || n.isPrimary)
                            continue;
                        dist = findDistance(source.x, source.y, n.x, n.y);
                        alphaAngle = Math.PI * (findDistance(n.x, n.y, source.ax, source.ay) - findDistance(n.x, n.y, source.bx, source.by)) / 0.125;
                        energyBf = (minEnergyTx - ((.04864M + .05M) / (letterb * band))) * 0.5M * (decimal)Math.Sqrt(2.0 + 2.0 * Math.Cos(alphaAngle));
                        bfDist = Math.Sqrt((double)(energyBf * 2 * .00316M * .1199M * .1199M / ((1 + alphaB) * eHatB * (decimal)Math.Pow(4 * Math.PI, 2.0) * .1M)));
                        if (dist <= bfDist)
                        {
                            source.myChildren.append(n.name);
                            n.parent = source.name;
                            thisHop.Add(n);
                        }
                    }
                    thisHop.RemoveAt(0);
                }
                else
                {
                    foreach (Node current in thisHop)
                    {
                        current.hops = hopCount;
                        foreach (Node n in arrBf)
                        {
                            if (n.isPrimary || thisHop.Contains(n) || nextHop.Contains(n) || n.parent != n.name)
                                continue;
                            dist = findDistance(current.x, current.y, n.x, n.y);
                            alphaAngle = Math.PI * (findDistance(n.x, n.y, current.ax, current.ay) - findDistance(n.x, n.y, current.bx, current.by)) / 0.125;
                            energyBf = (minEnergyTx - ((.04864M + .05M) / (letterb * band))) * 0.5M * (decimal)Math.Sqrt(2.0 + 2.0 * Math.Cos(alphaAngle));
                            bfDist = Math.Sqrt((double)(energyBf * 2 * .00316M * .1199M * .1199M / ((1 + alphaB) * eHatB * (decimal)Math.Pow(4 * Math.PI, 2.0) * .1M)));
                            if (dist <= bfDist)
                            {
                                nextHop.Add(n);
                            }
                        }
                    }
                    foreach (Node child in nextHop)
                    {
                        foreach (Node parents in thisHop)
                        {
                            dist = findDistance(child.x, child.y, parents.x, parents.y);
                            alphaAngle = Math.PI * (findDistance(child.x, child.y, parents.ax, parents.ay) - findDistance(child.x, child.y, parents.bx, parents.by)) / 0.125;
                            energyBf = (minEnergyTx - ((.04864M + .05M) / (letterb * band))) * 0.5M * (decimal)Math.Sqrt(2.0 + 2.0 * Math.Cos(alphaAngle));
                            bfDist = Math.Sqrt((double)(energyBf * 2 * .00316M * .1199M * .1199M / ((1 + alphaB) * eHatB * (decimal)Math.Pow(4 * Math.PI, 2.0) * .1M)));
                            if (dist <= bfDist)
                            {
                                child.possibleParents.Add(parents, dist);
                            }
                        }
                        double maxDist = double.MinValue;
                        Node theParent = new Node();
                        foreach (KeyValuePair<Node, double> stuff in child.possibleParents)
                        {
                            if (stuff.Value > maxDist)
                            {
                                maxDist = stuff.Value;
                                theParent = stuff.Key;
                            }
                        }
                        child.parent = theParent.name;
                        theParent.myChildren.append(child.name);
                    }
                    thisHop.Clear();
                    for (int i = 0; i < nextHop.Count; i++)
                    {
                        thisHop.Add(nextHop[i]);
                    }
                    nextHop.Clear();
                }
                hopCount++;
            }

            foreach (Node n in arrBf)
            {
                if (n.isPrimary)
                    continue;
                if (n.myChildren.header != null)
                    eNetworkBf += minEnergyTx;
                Item curr = new Item();
                curr = n.myChildren.header;
                while (curr != null)
                {
                    using (Graphics g = Graphics.FromImage(bf))
                    {
                        g.DrawLine(new Pen(colors[n.hops % 7]), n.x, n.y, arrBf[curr.nodeNum].x, arrBf[curr.nodeNum].y);
                    }
                    curr = curr.next;
                }
            }
            ePathBf = (hopCount - 1) * minEnergyTx;
            networkBf.Text = eNetworkBf.ToString("E");
            pathBf.Text = ePathBf.ToString("E");
            displayAreaBf.Image = bf;
            spanningTreeBf.Enabled = false;
            calcB.Enabled = false;
            populate.Focus();
        }
    }

    public class Node
    {
        public int name, x, y, parent, hops;
        public double ax, ay, bx, by;
        public bool isPrimary;
        public ChildList myChildren;
        public Dictionary<Node, double> possibleParents;

        public Node() { }

        public Node(int _name, int _x, int _y, bool _isPrimary, int _parent)
        {
            name = _name;
            x = _x;
            y = _y;
            parent = _parent;
            isPrimary = _isPrimary;
            hops = -1;
            if (!isPrimary)
            {
                double l = Form1.findDistance(Form1.prxx, Form1.prxy, x, y);
                ax = 0.0625 / l * (Form1.prxx - x) + x;
                ay = 0.0625 / l * (Form1.prxy - y) + y;
                bx = (l + 0.0625) / l * (x - Form1.prxx) + Form1.prxx;
                by = (l + 0.0625) / l * (y - Form1.prxy) + Form1.prxy;
            }
            else
            {
                ax = -1;
                ay = -1;
                bx = -1;
                by = -1;
            }
            myChildren = new ChildList();
            possibleParents = new Dictionary<Node, double>();
        }
    }

    public class Item
    {
        public int nodeNum;
        public Item next;

        public Item() { }

        public Item(int n)
        {
            nodeNum = n;
        }
    }

    public class ChildList
    {
        public Item header;

        public ChildList()
        {
            header = null;
        }
        
        public void append(int num)
        {
            Item newItem = new Item(num);
            newItem.next = header;
            header = newItem;
        }
    }
}
