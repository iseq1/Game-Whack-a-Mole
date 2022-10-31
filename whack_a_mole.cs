using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsForms_21._09._22
{
    public partial class Form1 : Form
    {
     int item;
        int width = 60;
        int height = 60;
        int startX = 300;
        int startY = 100;
        int space = 10;
        public static int hits = 0;
        public enum Colors { Red, Green, Blue, Magenta};
        public static int misses = 0;
        public static int lost = 0;
        DateTime game_start_time;
        List<PictureBox> buttons = new List<PictureBox>();
        TimeSpan game_duration = new TimeSpan(0, 0, 0, 10);
        public static Image hidden = Image.FromFile("hidden.png");
        public static Image alive = Image.FromFile("alive.png");

        public Form1()
        {

            InitializeComponent();

            game_start_time = DateTime.Now;
            
            foreach(var val in listBox1.Items)
            {

            }
            for (int i =0;i<3;i++)
            {

                for(int j=0;j<3;j++)
                {
                    PictureBox b = new PictureBox();
                    b.Text = "";
                    b.Size = new Size(width, height);
                    b.Location = new Point(
                        startX + j * (width + space),
                        startY + i * (height + space));
                    b.Click += B_Click;
                    b.Name = "button_" + i + "_" + j;
                    b.Tag = new button_data(i, j, b);
                    b.Image = hidden;
                    b.SizeMode = PictureBoxSizeMode.Zoom;
                    b.Font = new Font("Arial", 20);
                    buttons.Add(b);
                    this.Controls.Add(b);

                }
            }

        }

        private void B_Click(object sender, EventArgs e)
        {
            //if (sender is PictureBox)
            PictureBox current = sender as PictureBox;
            button_data mole = current.Tag as button_data;
            if (mole.state)
            {
                hits++;
                mole.hide();
                button_data.alive_moles = Math.Max(0, button_data.alive_moles - 1);
            }
            else
            {
                misses++;
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            item = int.Parse(comboBox1.SelectedItem.ToString());
            MessageBox.Show("Выбран элемент "+ item);

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int val = trackBar1.Value;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string current_value = textBox1.Text;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now - game_start_time >= game_duration)
            {
                timer1.Enabled = false;
                MessageBox.Show("Игра завершена!\nПопаданий: " + hits + "\nПромахов: " + misses + "\nУпущенных кротов: " + lost);
            }
            foreach(var button in buttons)
            {
                button_data data = button.Tag as button_data;
                if (data.state == false)
                {
                    if (DateTime.Now - data.change_time > new TimeSpan(0,0,0,0,data.hidden_interval)
                        && button_data.alive_moles + 1 <= button_data.max_alive
                        )
                    {
                        data.show();
                        button_data.alive_moles++;
                    }
                }
                else
                {
                    if (DateTime.Now - data.change_time > new TimeSpan(0, 0, 0, 0, data.life_interval))
                    {
                        Form1.lost++;
                        data.hide();
                        button_data.alive_moles = Math.Max(0, button_data.alive_moles-1);
                    }
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2(100);
            f2.Show();
        }
    }
    class button_data
    {
        public static int alive_moles = 0;
        public const int max_alive = 2;
        static Random r = new Random();
        public Form1.Colors color;
        static int min_delay = 1000;
        static int max_delay = 3000;
        public int life_interval = 2000;
        public int i;
        public int j;
        public bool state = false;
        public DateTime change_time;
        public int hidden_interval = 0;
        public PictureBox parent = null;
        public button_data(int i, int j, PictureBox current)
        {
            this.i = i;
            this.j = j;
            this.parent = current;
            this.hidden_interval = r.Next(min_delay, max_delay);
        }
        public void hide()
        {
            state = false;
            color = Form1.Colors.Blue;
            this.hidden_interval = r.Next(min_delay, max_delay);
            parent.Text = "";
            change_time = DateTime.Now;
            parent.Image = Form1.hidden;
        }
        public void show()
        {
            state = true;
            parent.Text = "X";
            change_time = DateTime.Now;
            parent.Image = Form1.alive;
        }
    }
}
