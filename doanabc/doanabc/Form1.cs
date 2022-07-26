using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace doanabc
{
    public partial class Form1 : Form
    {
        IMongoCollection<BsonDocument> collectiondv = Connec.getconnectdv();//ket noi den collection khach hang
        IMongoCollection<BsonDocument> collectioncdv = Connec.getconnectcdv();//ket noi den cac collection dich vu
        IMongoCollection<BsonDocument> collectionnv = Connec.getconnectnv();//ket noi den cac collection nhan vien
   
        public Form1()
        {
            InitializeComponent();
            foreach (BsonDocument document in collectiondv.AsQueryable()) //duyet tung document trong colection dv
            {
                comboBox1.Items.Add(document.GetElement("tenkh").Value.ToString()); //lay ra ten khach hang cua tung document trong collection dv
            }
            foreach (BsonDocument document in collectionnv.AsQueryable())
            {
                comboBox4.Items.Add(document.GetElement("tennv").Value.ToString());
            }
            foreach (BsonDocument document in collectioncdv.AsQueryable())
            {
                comboBox5.Items.Add(document.GetElement("tendv").Value.ToString());
            }
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            textBox6.Enabled = false;
            textBox5.Enabled = false;
            textBox3.Enabled = false;
            richTextBox2.Enabled = false;
            textBox10.Enabled = false;
            textBox9.Enabled = false;
            textBox8.Enabled = false;
        }
        // lay ra thong tin khach hang
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
                return;
            var builder = Builders<BsonDocument>.Filter;
            var filter1 = builder.Eq("tenkh", comboBox1.SelectedItem.ToString());
            var cursor = collectiondv.Find(filter1).FirstOrDefault();
            textBox1.Text = cursor.GetElement("makh").Value.ToString();
            textBox2.Text = cursor.GetElement("tenkh").Value.ToString();
            comboBox2.SelectedItem = cursor.GetElement("phai").Value.ToString();
            richTextBox1.Text = cursor.GetElement("diachi").Value.ToString();
            textBox4.Text = cursor.GetElement("sdt").Value.ToString();
            dataGridView1.Rows.Clear();
            try
            {
                BsonArray bsonArray = (BsonArray)cursor.GetElement("nhanvien").Value;
                foreach (var b in bsonArray)
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                    row.Cells[0].Value = b["manv"];
                    row.Cells[1].Value = b["tennv"];
                    row.Cells[2].Value = b["dichvu"]["tendv"];
                    row.Cells[3].Value = b["thoigian"];
                    dataGridView1.Rows.Add(row);
                }
            }
            catch (Exception)
            {

            }
            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        //tim kiem khach hang theo ten khach hang
        private void button9_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();//xoa dong 
            foreach (BsonDocument document in collectiondv.AsQueryable())
            {
                if(document.GetElement("tenkh").Value.ToString().ToLower().Contains(textBox7.Text.ToLower()))
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridView2.Rows[0].Clone();
                    row.Cells[0].Value = document.GetElement("makh").Value.ToString();
                    row.Cells[1].Value = document.GetElement("tenkh").Value.ToString();
                    row.Cells[2].Value = document.GetElement("phai").Value.ToString();
                    row.Cells[3].Value = document.GetElement("sdt").Value.ToString();
                    row.Cells[4].Value = document.GetElement("diachi").Value.ToString();
                    dataGridView2.Rows.Add(row);
                }    
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter1 = builder.Eq("makh", dataGridView2.CurrentRow.Cells[0].Value.ToString()); //Lọc ra các phần tử có thuộc tính là  "makh"
            var cursor = collectiondv.Find(filter1).FirstOrDefault();
            comboBox1.SelectedItem = cursor.GetElement("tenkh").Value.ToString();
        }
        //lay ra thong tin khach hang
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter1 = builder.Eq("tennv", comboBox4.SelectedItem.ToString());
            var cursor = collectionnv.Find(filter1).FirstOrDefault();
            textBox6.Text = cursor.GetElement("manv").Value.ToString();
            textBox5.Text = cursor.GetElement("tennv").Value.ToString();
            comboBox3.SelectedItem = cursor.GetElement("phai").Value.ToString();
            richTextBox2.Text = cursor.GetElement("diachi").Value.ToString();
            textBox3.Text = cursor.GetElement("SDT").Value.ToString();
        }
        //lay ra thong tin dich vu
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter1 = builder.Eq("tendv", comboBox5.SelectedItem.ToString());
            var cursor = collectioncdv.Find(filter1).FirstOrDefault();
            textBox10.Text = cursor.GetElement("madv").Value.ToString();
            textBox9.Text = cursor.GetElement("tendv").Value.ToString();
            textBox8.Text = cursor.GetElement("giadv").Value.ToString();
        }
        //them khach hang
        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            textBox1.Clear();
            textBox2.Clear();
            textBox4.Clear();
            richTextBox1.Clear();
            comboBox2.SelectedIndex = -1;
            button7.Enabled = true;
            comboBox1.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
        }
        //Luu thong tin khach hang
        private void button7_Click(object sender, EventArgs e)
        {
            BsonDocument document = new BsonDocument()
             .Add("makh", textBox1.Text)
             .Add("tenkh", textBox2.Text)
             .Add("phai", comboBox2.SelectedItem.ToString())
             .Add("sdt", textBox4.Text)
             .Add("diachi", richTextBox1.Text);
            collectiondv.InsertOneAsync(document);
            button7.Enabled = false;
            comboBox1.Items.Clear();
            foreach (BsonDocument document1 in collectiondv.AsQueryable())
            {
                comboBox1.Items.Add(document1.GetElement("tenkh").Value.ToString());
            }
            button7.Enabled = false;           
            comboBox1.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
        }
        // Xoa khach hang
        private void button2_Click(object sender, EventArgs e)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter1 = builder.Eq("makh", textBox1.Text.ToString());
            collectiondv.DeleteOne(filter1);
            comboBox1.Items.Clear();
            foreach (BsonDocument document in collectiondv.AsQueryable())
            {
                comboBox1.Items.Add(document.GetElement("tenkh").Value.ToString());
            }
            button1_Click(sender,e);
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button7.Enabled = false;
            comboBox1.Enabled = true;
        }
        // sua thoong tin khach hang
        private void button3_Click(object sender, EventArgs e)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter1 = builder.Eq("makh", textBox1.Text.ToString());
            try
            {
               var update = Builders<BsonDocument>.Update.Set("tenkh", textBox2.Text)
               .Set("phai", comboBox2.SelectedItem.ToString())
               .Set("sdt", textBox4.Text)
               .Set("diachi", richTextBox1.Text);
                collectiondv.UpdateOne(filter1, update);
            }
            catch (Exception)
            {
            }
            button2.Enabled = false;
            button1.Enabled = false;
            button7.Enabled = true;
        }
        //them thong tin khach hang
        private void button6_Click(object sender, EventArgs e)
        {
            textBox5.Clear();
            textBox6.Clear();
            textBox3.Clear();
            richTextBox2.Clear();
            textBox10.Clear();
            textBox9.Clear();
            textBox8.Clear();
            button8.Enabled = true;
            comboBox4.Enabled = true;
            comboBox5.Enabled = true;
            button5.Enabled = false;
            button6.Enabled = false;
        }
        //xoa thong tin nhan vien va dich vu cua khach hang
        private void button5_Click(object sender, EventArgs e)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter1 = builder.Eq("makh", textBox1.Text);
            var cursor = collectiondv.Find(filter1).FirstOrDefault();
            BsonArray bsonArray = (BsonArray)cursor.GetElement("nhanvien").Value;
            BsonDocument selc = new BsonDocument();
            int i = 0;
            foreach (BsonDocument item in bsonArray)
            {
                if (DateTime.Compare(DateTime.Parse(item.GetElement("thoigian").Value.ToString()),dateTimePicker1.Value)==0)
                {
                    break;
                }
                i++;

            }
            bsonArray.RemoveAt(i);
            var update = Builders<BsonDocument>.Update.Set("nhanvien", bsonArray);
            collectiondv.UpdateOne(filter1, update);
            dataGridView1.Rows.Clear();
            try
            {
                BsonArray bsonArray1 = (BsonArray)cursor.GetElement("nhanvien").Value;
                foreach (var b in bsonArray1)
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                    row.Cells[0].Value = b["manv"];
                    row.Cells[1].Value = b["tennv"];
                    row.Cells[2].Value = b["dichvu"]["tendv"];
                    row.Cells[3].Value = b["thoigian"];
                    dataGridView1.Rows.Add(row);
                }
            }
            catch (Exception)
            {

            }
        }
        //luu thong tin khach hang
        private void button8_Click(object sender, EventArgs e)
        {
            var builder = Builders<BsonDocument>.Filter;
            var filter1 = builder.Eq("makh", textBox1.Text.ToString());
            BsonDocument document = collectiondv.Find(filter1).FirstOrDefault();
            BsonDocument arrayChildDocument = new BsonDocument();
            BsonDocument ChildDocument = new BsonDocument()
             .Add("madv", textBox10.Text)
             .Add("tendv", textBox9.Text)
             .Add("giadv", textBox8.Text);
            arrayChildDocument = new BsonDocument();
            try
            {
                arrayChildDocument.Add(
           new BsonElement("manv", textBox6.Text),
           new BsonElement("tennv", textBox5.Text),
           new BsonElement("phai", comboBox3.SelectedItem.ToString()),
           new BsonElement("SDT", textBox3.Text),
           new BsonElement("diachi", richTextBox2.Text),
           new BsonElement("thoigian", dateTimePicker1.Value),
           new BsonElement("dichvu", ChildDocument)
           );
            }
            catch (Exception)
            {
            }
           
            var update = Builders<BsonDocument>.Update.AddToSet("nhanvien", arrayChildDocument);
            collectiondv.UpdateOne(filter1, update);
            dateTimePicker1.Value = DateTime.Now;
            dataGridView1.Rows.Clear();
            var cursor = collectiondv.Find(filter1).FirstOrDefault();
            try
            {
                BsonArray bsonArray = (BsonArray)cursor.GetElement("nhanvien").Value;
                foreach (var b in bsonArray)
                {
                    DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                    row.Cells[0].Value = b["manv"];
                    row.Cells[1].Value = b["tennv"];
                    row.Cells[2].Value = b["dichvu"]["tendv"];
                    row.Cells[3].Value = b["thoigian"];
                    dataGridView1.Rows.Add(row);
                }
            }
            catch (Exception)
            {

            }
            button8.Enabled = false;
            comboBox4.Enabled = false;
            comboBox5.Enabled = false;
            button5.Enabled = true;
            button6.Enabled = true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                comboBox4.SelectedItem = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                comboBox5.SelectedItem = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                dateTimePicker1.Value = DateTime.Parse(dataGridView1.CurrentRow.Cells[3].Value.ToString());
            }
            catch (Exception)
            {

            }
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
