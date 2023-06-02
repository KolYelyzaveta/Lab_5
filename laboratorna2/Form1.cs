using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;

namespace laboratorna2
{
    public partial class Form1 : Form
    {
        public Hotel _Hotel = new Hotel("#1 Hotel");
        public int Percent { get; set; }
        public bool Pause = false;
        public List<string> Status = new List<string>();
        Thread StartThread;
        Thread StopThread;
        public Form1()
        {
            _Hotel.Rooms = new List<Room>()
            {
                new Room(4, 0, 200, Room.Status.FREE),
                new Room(4, 0, 200, Room.Status.FREE),
                new Room(4, 0, 200, Room.Status.FREE),
                new Room(4, 0, 200, Room.Status.FREE),
                new Room(4, 0, 200, Room.Status.FREE),
                new Room(4, 0, 200, Room.Status.FREE),
                new Room(4, 0, 200, Room.Status.FREE),
                new Room(4, 0, 200, Room.Status.FREE),
                new Room(4, 0, 200, Room.Status.FREE),
                new Room(4, 0, 200, Room.Status.FREE)
            };
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Pause = false;
            _Hotel.IsOpen = true;
            var thread = new Thread(() =>
            {
                while (_Hotel.IsOpen && Pause == false)
                {
                    StartThread = new Thread(() => StartSeason());
                    StartThread.Start();
                    Thread.Sleep(300);
                    this.Invoke(new Action(() =>
                    {
                        ConnectDataToDataGridView();
                        Status.Add("Hotel IsOpen = " + _Hotel.IsOpen);
                        SerializeJson("Hotel.json", Status);
                        label1.Text = "Касса: " + _Hotel.Cash.ToString();
                        if (Percent == 0 || Percent < 10)
                        {
                            label2.Text = "Отель свободен на: " + Percent.ToString() + "%";
                            Thread.Sleep(300);
                            _Hotel.IsOpen = false;
                            Status.Add("Hotel IsOpen = " + _Hotel.IsOpen);
                            StopThread = new Thread(() => StopSeason());
                            StopThread.Start();
                            ConnectDataToDataGridView();
                            SerializeJson("Hotel.json", Status);
                        }
                        label2.Text = "Отель свободен на: " + Percent.ToString() + "%";
                    }));
                }
            });
            thread.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = _Hotel.Name;
            label1.Text = "Касса: " + _Hotel.Cash.ToString();
            Percent = GetHotelFreeProcent();
            label2.Text = "Отель свободен на: " + Percent.ToString() + "%";
            ConnectDataToDataGridView();
        }

        public int GetHotelFreeProcent()
        {
            int freeRooms = 0;
            foreach (var room in _Hotel.Rooms)
            {
                if (room.CurrentStatus == Room.Status.FREE)
                {
                    freeRooms++;
                }
            }
            if (freeRooms == _Hotel.Rooms.Count)
            {
                _Hotel.IsOpen = true;
                return 100;
            }
            else if (freeRooms > 0)
            {
                _Hotel.IsOpen = true;
                freeRooms = 100 / _Hotel.Rooms.Count * freeRooms;
                return freeRooms;
            }
            {
                return 0;
            }
        }

        public void ConnectDataToDataGridView()
        {
            dataGridView1.DataSource = _Hotel.Rooms;
            dataGridView1.Refresh();
            label1.Refresh();
            label2.Refresh();
        }

        public void StartSeason()
        {
            var random = new Random();
            int tourists = random.Next(1, 5);
            int index = random.Next(_Hotel.Rooms.Count);
            if (_Hotel.Rooms[index].CurrentStatus == Room.Status.FREE)
            {
                _Hotel.Rooms[index].TakenPlaces = tourists;
                _Hotel.Rooms[index].CurrentStatus = Room.Status.LOCKED;
                _Hotel.Cash += _Hotel.Rooms[index].Price * tourists;
                Percent = GetHotelFreeProcent();
            }
        }

        public void StopSeason()
        {
            var random = new Random();
            if (!_Hotel.IsOpen && Pause == false)
            {
                for (int i = 0; i < random.Next(_Hotel.Rooms.Count); i++)
                {
                    int index = random.Next(_Hotel.Rooms.Count);
                    _Hotel.Rooms[index].TakenPlaces = 0;
                    _Hotel.Rooms[index].CurrentStatus = Room.Status.FREE;
                }
                Percent = GetHotelFreeProcent();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Pause = true;
            StartThread.Abort();
        }

        public static void SerializeJson<T>(string fileName, IEnumerable<T> data)
        {
            var json = new DataContractJsonSerializer(data.GetType());
            using (var file = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                json.WriteObject(file, data);
            }
        }
    }
}
