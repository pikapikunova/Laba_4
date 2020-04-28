using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(600, 660);
            pictureBox1.MouseClick += new MouseEventHandler(pictureBox1_MouseClick);
        }
        Fild fild1 = new Fild();
        Cat blackCat = new Cat();
        PictureBox pictureBox2 = new PictureBox();
        int curr = 61;

        private void button1_Click(object sender, EventArgs e)
        {

            fild1.FillField(pictureBox1, blackCat);
            Graphics g = pictureBox1.CreateGraphics();
            blackCat.FillMartix(fild1.circ);
            button1.Enabled = false;
            Cursor.Position = new Point(45, 45);
            g.DrawImage(blackCat.im, 266,243, 38,38);

        }


        void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;
            fild1.Click(pictureBox1, x, y);
            blackCat.UpdateMatrix(fild1, x, y);
            int prev = curr;
            if ((curr == blackCat.BfsAlg(curr)) & (curr <= 109) & (curr != 22) & (curr != 23) & (curr != 33) & (curr != 34) & (curr != 44) & (curr
                != 45) & (curr != 55) & (curr != 56) & (curr != 66) & (curr != 67) & (curr != 77) & (curr != 78) & (curr
                != 88) & (curr != 89) & (curr != 99) & (curr != 100) & (curr >= 13)) 
                MessageBox.Show("Вы выиграли!");
            else
            {
                if (curr == blackCat.BfsAlg(curr))
                     MessageBox.Show("Вы проиграли!");
                else
                {
                    curr = blackCat.BfsAlg(curr);
                    Graphics g = pictureBox1.CreateGraphics();
                    blackCat.Motion(fild1.circ, blackCat.translatei(curr), blackCat.translatej(blackCat.translatei(curr), curr), blackCat.im, g);
                    fild1.circ[blackCat.translatei(prev), blackCat.translatej(blackCat.translatei(prev), prev)].fillCircle(g);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
            curr = 61;
            Cursor.Position = new Point(45, 45);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 65)
                Cursor.Position = new Point(Cursor.Position.X - 10, Cursor.Position.Y);
            else if (e.KeyValue == 87)
                Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - 10);
            else if (e.KeyValue == 83)
                Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + 10);
            else if (e.KeyValue == 68)
                Cursor.Position = new Point(Cursor.Position.X + 10, Cursor.Position.Y);
            else if (e.KeyValue == 70)
            {
                Mouse mouse = new Mouse();
                mouse.rightClick();
            }
        }   
    }

    public class Mouse
    {
            [DllImport("User32.Dll")]
            private static extern void mouse_event(UInt32 dwFlags, UInt32 dx, UInt32 dy, UInt32 dwData, IntPtr dwExtraInfo);
            private const UInt32 RIGHTDOWN = 0x0008;
            private const UInt32 RIGHTUP = 0x0010;

            public void rightClick()
            {
                mouse_event(RIGHTDOWN | RIGHTUP, 0, 0, 0, IntPtr.Zero);
            }
    }



        public class Cat
        {
            public Image im = Image.FromFile(@"C:\Users\Nasty\source\repos\WindowsFormsApp3\WindowsFormsApp3\cat.png");

            bool[,] matrix = new bool[122, 122];

        public void Motion(Circle[,] circ, int x, int y, Image im, Graphics g)
        {
            g.DrawImage(im,circ[x, y].x+2, circ[x, y].y+3,38,38);
        }

        public int BfsAlg(int start)
            {
                int[,] rebra = new int[122, 122];
                int[] predsessors = new int[122];
                predsessors[start] = 0;
                //Очередь вершин на рассмотрение
                Queue<int> openVertex = new Queue<int>();
                Queue<int> Way = new Queue<int>();
                //Список уже рассмотренных вершин
                List<int> CloseVertex = new List<int>();
                //Начинаем обход с 1й вершины
                openVertex.Enqueue(start);


                //До тех пор, пока не обошли все вершины
                int index = 0;
                while (openVertex.Count != 0)
                {
                    //Выталкиваем из начала списка индекс текущей вершины
                    index = openVertex.Dequeue();
                    Console.WriteLine(index);
                    if (index > 109 || index == 22 || index == 23 || index == 33 || index == 34 || index == 44 ||
                        index == 45 || index == 55 || index == 56 || index == 66 || index == 67 || index == 77 || index == 78 ||
                        index == 88 || index == 89 || index == 99 || index == 100 || index < 13) break;
                    for (short j = 0; j < 122; j++)
                    {
                        //Если ребро не нулевое 
                        if (matrix[index, j] != false)
                        {
                            //И вершина еще не была рассмотрена и не находится в очереди на рассмотрение
                            if (!CloseVertex.Contains(j) && !openVertex.Contains(j))
                            {
                                //Добавить вершину в список на рассмотрение
                                openVertex.Enqueue(j);
                                predsessors[j] = index;
                            }
                        }
                    }
                    //Добавляем вершину в список уже рассмотренных
                    CloseVertex.Add(index);
                }
                while (index != 0)
                {
                    if (predsessors[predsessors[index]] == 0)
                        break;
                    else
                        index = predsessors[index];
                }
                return index;
            }

            public void UpdateMatrix(Fild fild1, int x, int y)
            {
                int whichRow = fild1.WhichRow(y);
                int whichColomn = fild1.WhichColomn(x, y);
                for (int t = 0; t < 122; t++)
                {
                    matrix[(whichRow - 1) * 10 + whichRow + whichColomn - 1, t] = false;
                    matrix[t, (whichRow - 1) * 10 + whichRow + whichColomn - 1] = false;
                }
            }

            public void FillMartix(Circle[,] circ)
            {

                int n = 1;
                int i1 = 1;
                int c = 1;
                for (int i = 1; i < 12; i++)
                {
                    for (int j = 1; j <= i; j++)
                    {
                        if (i == j)
                        {
                            matrix[i, j] = false;
                        }
                        else
                        {

                            if (j == c)
                            {
                                matrix[i, j] = true;
                                matrix[j, i] = true;
                                c++;
                            }
                            else
                            {
                                matrix[i, j] = false;
                                matrix[j, i] = false;
                            }
                        }
                    }
                }

                n++;
                c++;
                for (; n < 12; n++)
                {
                    c = (n - 1) * 10 + n;
                    if (n % 2 == 0)
                    {
                        i1 = FillChet(n, i1, c);
                    }
                    else

                    {
                        i1 = FillNechet(n, i1, c);
                        i1 += 1;
                    }
                }
                for (int i = 1; i < 12; i++)
                    for (int j = 1; j < 12; j++)
                    {
                        if (circ[i, j].Flag == false)
                            for (int t = 0; t < 122; t++)
                            {
                                matrix[(i - 1) * 10 + i + j - 1, t] = false;
                                matrix[t, (i - 1) * 10 + i + j - 1] = false;
                            }
                    }
            }

            public int translatei(int curr)
            {
                int i = 0;
                int j;
                int t = 11;
                int n = 0;
                for (n = 1; n < 122; n += 11)
                {
                    if (curr >= n && curr <= t)
                        if (t > 99 && n > 89)
                        {
                            i = t / 10 - 1;
                        }
                        else
                            i = t / 10;
                    t += 11;
                }
                t += 11;
                return i;
            }

            public int translatej(int i, int curr)
            {
                int j;
                j = curr + 1 - (i - 1) * 10 - i;
                return j;
            }

            public int FillChet(int n, int i1, int c)
            {
                for (int i = (n - 1) * 10 + n; i < n * 11 + 1; i++)
                {
                    for (int j = 1; j <= i; j++)
                    {
                        if (i == (n - 1) * 10 + n)
                        {

                            if (j == i1)
                            {
                                matrix[i, j] = true;
                                matrix[j, i] = true;
                            }
                            else
                            if (j == i1 + 1)
                            {
                                matrix[i, j] = true;
                                matrix[j, i] = true;
                            }
                            else
                            {
                                matrix[i, j] = false;
                                matrix[j, i] = false;

                            }
                        }
                        else
                        {
                            if (i != n * 11)
                            {
                                if (j == i1)
                                {
                                    matrix[i, j] = true;
                                    matrix[j, i] = true;
                                }
                                else
                                {
                                    if (j == i1 + 1)
                                    {
                                        matrix[i, j] = true;
                                        matrix[j, i] = true;
                                    }
                                    else
                                    {
                                        if (i == j)
                                        {
                                            matrix[i, j] = false;
                                            matrix[j, i] = false;
                                        }
                                        else
                                        {
                                            if (j == c)
                                            {
                                                matrix[i, j] = true;
                                                c++;
                                                matrix[j, i] = true;
                                            }
                                            else
                                            {
                                                matrix[i, j] = false;
                                                matrix[j, i] = false;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (j == i1)
                                {
                                    matrix[i, j] = true;
                                    matrix[j, i] = true;
                                }
                                else
                                {
                                    if (i == j)
                                    {
                                        matrix[i, j] = false;
                                        matrix[j, i] = false;
                                    }
                                    else
                                    {
                                        if (j == c)
                                        {
                                            matrix[i, j] = true;
                                            c++;
                                            matrix[j, i] = true;
                                        }
                                        else
                                        {
                                            matrix[i, j] = false;
                                            matrix[j, i] = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    i1++;
                    Console.WriteLine('\n');
                }
                return i1;
            }

            public int FillNechet(int n, int i1, int c)
            {
                for (int i = (n - 1) * 10 + n; i < n * 11 + 1; i++)
                {
                    for (int j = 0; j <= i; j++)
                    {
                        if (i == (n - 1) * 10 + n)
                        {
                            if (j == i1)
                            {
                                matrix[i, j] = true;
                                matrix[j, i] = true;
                                i1--;
                            }
                            else
                            {
                                matrix[i, j] = false;
                                matrix[j, i] = false;
                            }
                        }
                        else
                        {
                            if (j == i1)
                            {
                                matrix[i, j] = true;
                                matrix[j, i] = true;
                            }
                            else
                            {
                                if (j == i1 + 1)
                                {
                                    matrix[i, j] = true;
                                    matrix[j, i] = true;
                                }
                                else
                                {
                                    if (j == i)
                                    {
                                        matrix[i, j] = false;
                                        matrix[j, i] = false;
                                    }
                                    else
                                    {
                                        if (j == c)
                                        {
                                            matrix[i, j] = true;
                                            matrix[j, i] = true;
                                            c++;
                                        }
                                        else
                                        {
                                            matrix[i, j] = false;
                                            matrix[j, i] = false;
                                        }

                                    }
                                }
                            }
                        }

                    }
                    i1++;
                    Console.WriteLine('\n');

                }
                return i1;
            }


            public static System.Drawing.Drawing2D.GraphicsPath BuildTransparencyPath(Image im)
            {
                int x;
                int y;
                Bitmap bmp = new Bitmap(im);
                System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
                Color mask = bmp.GetPixel(0, 0);

                for (x = 0; x <= bmp.Width - 1; x++)
                {
                    for (y = 0; y <= bmp.Height - 1; y++)
                    {
                        if (!bmp.GetPixel(x, y).Equals(mask))
                        {
                            gp.AddRectangle(new Rectangle(x, y, 1, 1));
                        }
                    }
                }
                bmp.Dispose();
                return gp;
            }
        }

        public class Circle
        {
            public bool Flag;

            public int x;
            public int y;

            public Circle(int X, int Y, bool f)
            {
                x = X;
                y = Y;
                Flag = f;
            }

            public void fillCircle(Graphics g)
            {
                SolidBrush Green;
                if (this.Flag == true)
                    Green = new SolidBrush(Color.GreenYellow);
                else
                    Green = new SolidBrush(Color.ForestGreen);
                g.FillEllipse(Green, this.x, this.y, 45, 45);
            }
        }

        public class Fild
        {
            List<int> masX = new List<int>() { 0, 48, 96, 144, 192, 240, 288, 336, 384, 432, 480 };
            List<int> masY = new List<int>() { 24, 72, 120, 168, 216, 264, 312, 360, 408, 456, 504 };
            public Circle[,] circ = new Circle[12, 12];

            public void FillField(PictureBox pictureBox1, Cat cat)
            {
                Graphics g = pictureBox1.CreateGraphics();

                int a = 1;
                int b = 1;

                circ[1, 1] = new Circle(1, 1, true);

                foreach (int i in masX)
                {
                    int ranY;
                    if (a % 2 == 0)
                    {
                        if (a == 2 || a == 3 || a == 4 || a == 5 || a == 7 || a == 8 || a == 9 || a == 11)
                            ranY = masY[new Random().Next(0, masY.Count)];
                        else
                            ranY = -5;
                        b = 1;
                        foreach (int j in masY)
                        {
                            if (j == ranY)
                            {
                                circ[a, b] = new Circle(j, i, false);
                            }
                            else
                            {
                                circ[a, b] = new Circle(j, i, true);
                            }
                            circ[a, b].fillCircle(g);
                            b++;
                        }
                    }
                    else
                    {
                        if (a == 2 || a == 5 || a == 8 || a == 9 || a == 11)
                            ranY = masX[new Random().Next(0, masX.Count)];
                        else
                            ranY = -5;
                        b = 1;
                        foreach (int j in masX)
                        {
                            if (j == ranY)
                            {
                                circ[a, b] = new Circle(j, i, false);
                            }
                            else
                            {
                                circ[a, b] = new Circle(j, i, true);
                            }
                            circ[a, b].fillCircle(g);
                            b++;
                        }
                    }
                    a++;
                }
            }

            public void Click(PictureBox pictureBox1, int x, int y)
            {
                Graphics g = pictureBox1.CreateGraphics();

                int whichRow = WhichRow(y);
                int whichColomn = WhichColomn(x, y);
                circ[whichRow, whichColomn].Flag = false;
                circ[whichRow, whichColomn].fillCircle(g);

            }

            public int WhichRow(int y)
            {
                int a = 0;
                bool t = false;
                for (int i = 0; i < masX.Count && t == false; i++)
                {
                    if (y <= masX[i])
                        t = true;
                    else
                        a++;
                }
                return a;
            }
            public int WhichColomn(int x, int y)
            {
                int a = 0;
                bool t = false;

                if (WhichRow(y) % 2 == 0)
                    for (int i = 0; i < masY.Count && t == false; i++)
                    {
                        if (x <= masY[i])
                            t = true;
                        else
                            a++;
                    }
                else
                    for (int i = 0; i < masX.Count && t == false; i++)
                    {
                        if (x <= masX[i])
                            t = true;
                        else
                            a++;
                    }
                return a;
            }

        }


    }

   