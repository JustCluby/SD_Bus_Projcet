using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace SD_Bus_Projcet
{
    public partial class Form1 : Form
    {
        //@@region: vars
        string s_fileReadPath = "data.xml";
        string s_fileWritePath = "write.xml";
        string s_noRecordDate = "1/01/0001 12:00:00 AM";
        class Bus{
            public int id;
            public DateTime date;  //the date this record is for
            public DateTime time_in;
            public DateTime time_out;
            public bool status; //true is "in" false is "out"
        }

        static Bus[] Buses; //was gonna use another array called recorded it can be done with this instead
        static Bus[] Search; //stores what the user searches for

        //@@region end

        //Algato amoogus sus code - Mark

        //@@region: load form
        public Form1()
        {
            InitializeComponent();
            ReadFile(s_fileReadPath);
        }
        //@@region end 

        int i;
        //@@region: btnPrint test onclick
        private void btnPrint_Click(object sender, EventArgs e)
        {
            DateTime dt_curDate = DateTime.Now;
            label5.Text = Convert.ToString(dt_curDate);
            label1.Text = Convert.ToString(Buses[i].id);

            //check to see if the record is nil
            if (Convert.ToString(Buses[i].time_in) == s_noRecordDate)
            {
                label2.Text = "No Record";
            }
            else
            {
                label2.Text = Convert.ToString(Buses[i].time_in);
            }

            //check again ._.
            if (Convert.ToString(Buses[i].time_out) == s_noRecordDate)
            {
                label3.Text = "No Record";
            }
            else
            {
                label3.Text = Convert.ToString(Buses[i].time_out);
            }
            label4.Text = Convert.ToString(Buses[i].status);
            i++;
            if (i > Buses.Length - 1)
            {
                i = 0;
            }
        }
        //@@region end


        void WriteFile(string filepath)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filepath);
            string time_check_in;
            string time_check_out;

            XmlNodeList busrecordList = document.GetElementsByTagName("Bus");
            Buses = new Bus[busrecordList.Count];

            int index = 0;
            foreach (XmlNode bus in busrecordList)
            {
                bus["time_in"].InnerText = "1";
            }
        }


        //@@region: read file 
        //reads the xml file given and stores it in bus record
        void ReadFile(string filepath)
        {
            XmlDocument document = new XmlDocument(); 
            document.Load(filepath);
            string time_check_in;
            string time_check_out;

            XmlNodeList busrecordList = document.GetElementsByTagName("Bus");
            Buses = new Bus[busrecordList.Count];

            int index = 0;
            foreach (XmlNode bus in busrecordList)
            {
                //gets data from XML document and stores in bus
                Bus b = new Bus();
                b.id = Convert.ToInt32(bus.Attributes["id"].Value);
                time_check_in = Convert.ToString(bus["time_in"].InnerText);
                if (time_check_in != "nil") //checks if time exists if not set to 1/01/0001 which we will check for later
                {
                    b.time_in = Convert.ToDateTime(bus["time_in"].InnerText);
                }
                b.date = Convert.ToDateTime(bus["date"].InnerText);
                time_check_out = Convert.ToString(bus["time_out"].InnerText); //same check
                if (time_check_out != "nil")
                {
                    b.time_out = Convert.ToDateTime(bus["time_out"].InnerText);
                }
                b.status = Convert.ToBoolean(bus["status"].InnerText);

                //add to bus to array   
                Buses[index] = b;
                index++;
            }
        }
        //@@region end

        //@@region: record
        private int Record(int i_busID, bool b_Status, Bus[] Buses)
        {
            DateTime dt_dateNow = DateTime.Now;
            DateTime dt_curDate = dt_dateNow.Date;

            for (int i = 0; i < Buses.Length; i++)
            {
                if (Buses[i].id == i_busID && Buses[i].date == dt_curDate) //check if its the right bus and day
                {
                    if (chbx_ChangeBusStatus.Checked == true)
                    {
                        if (chbx_Amended.Checked == true)
                        {
                            Buses[i].time_in = new DateTime(dt_dateNow.Year, dt_dateNow.Month, dt_dateNow.Day, Convert.ToInt32(comboBox1.Text) /*hour*/, Convert.ToInt32(comboBox2.Text) /*minute*/, 0);
                        }
                        else
                        {
                            Buses[i].time_in = DateTime.Now;
                        }
                    }
                    else
                    {
                        if (chbx_Amended.Checked == true)
                        {
                            Buses[i].time_out = new DateTime(dt_dateNow.Year, dt_dateNow.Month, dt_dateNow.Day, Convert.ToInt32(comboBox1.Text) /*hour*/, Convert.ToInt32(comboBox2.Text) /*minute*/, 0);
                        }
                        else
                        {
                            Buses[i].time_out = DateTime.Now;
                        }
                    }
                    return 1;
                }
                else return -1;
            }
            return -2;
        }

        private void chbx_ChangeBusStatus_CheckedChanged(object sender, EventArgs e)
        {
            bool b_curCheckStatus = chbx_ChangeBusStatus.Checked; //true is 'in' false is 'out'
            int i_returnCode;
            i_returnCode = Record(Convert.ToInt32(comboBox3.Text), false, Buses);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WriteFile(s_fileWritePath);
        }

        private void btnUIBuses_Click(object sender, EventArgs e)
        {
            btnUIBuses.BackColor = Color.FromArgb(230, 200, 9); //Yellow
            btnUIData.BackColor = Color.FromArgb(47, 45, 134); //Dark Blue
        }

        private void btnUIData_Click(object sender, EventArgs e)
        {
            btnUIData.BackColor = Color.FromArgb(230, 200, 9); //Yellow
            btnUIBuses.BackColor = Color.FromArgb(47, 45, 134); //Dark Blue
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
